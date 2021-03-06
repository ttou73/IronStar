﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Fusion.Core;
using Fusion.Core.Mathematics;
using Fusion.Core.Configuration;
using Fusion.Core.Extensions;
using Fusion.Engine.Common;
using Fusion.Drivers.Graphics;
using Fusion.Engine.Imaging;
using System.Diagnostics;
using Fusion.Engine.Storage;
using Fusion.Build.Mapping;
using Fusion.Engine.Graphics.Ubershaders;

namespace Fusion.Engine.Graphics {

	[RequireShader("vtcache")]
	internal class VTSystem : GameComponent {

		readonly RenderSystem rs;

		[Config]
		public int MaxPPF { get; set; }

		[Config]
		public bool ShowPageCaching { get; set; }

		[Config]
		public bool ShowPageLoads { get; set; }

		[Config]
		public bool ShowPhysicalTextures { get; set; }

		[Config]
		public bool ShowPageTexture { get; set; }

		[Config]
		public bool ShowTileBorder { get; set; }

		[Config]
		public bool LockTiles { get; set; }

		[Config]
		public bool ShowTileAddress { get; set; }

		[Config]
		public bool ShowTileCheckers { get; set; }

		[Config]
		public bool RandomColor { get; set; }

		[Config]
		public int PhysicalSize {
			get {
				return physicalSize;
			}
			set {
				if (physicalSize!=value) {
					physicalSize = value;
					physicalSizeDirty = true;
				}
			}
		}
		int physicalSize = 1024;
		bool physicalSizeDirty = true;


		public float PageScaleRCP {
			get; private set;
		}


		public Texture2D		PhysicalPages0;
		public Texture2D		PhysicalPages1;
		public Texture2D		PhysicalPages2;
		public RenderTarget2D	PageTable;
		public StructuredBuffer	PageData;
		public ConstantBuffer	Params;

		VTTileLoader	tileLoader;
		VTTileCache		tileCache;
		VTStamper		tileStamper;

		Ubershader		shader;
		StateFactory	factory;

		Image	fontImage;

		public Image FontImage {
			get {
				return fontImage;
			}
		}

		enum Flags {
			None  = 0,
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="rs"></param>
		/// <param name="baseDirectory"></param>
		public VTSystem ( RenderSystem rs ) : base( rs.Game )
		{
			this.rs	=	rs;

			MaxPPF	=	16;
		}


		Stopwatch stopwatch = new Stopwatch();


		/// <summary>
		/// 
		/// </summary>
		public override void Initialize ()
		{
			ApplyVTState();

			var rand = new Random();
			//PageTable.SetData( Enumerable.Range(0,tableSize*tableSize).Select( i => rand.NextColor4() ).ToArray() );

			Game.Reloading += (s,e) => LoadContent();

			tileLoader		=	new VTTileLoader( this, Game.Content.VTStorage );

			LoadContent();
		}


		/// <summary>
		/// 
		/// </summary>
		void LoadContent ()
		{
			shader	=	Game.Content.Load<Ubershader>("vtcache");
			factory	=	shader.CreateFactory( typeof(Flags), Primitive.TriangleList, VertexInputElement.Empty );
		}



		/// <summary>
		/// 
		/// </summary>
		void ApplyVTState ()
		{
			if (physicalSizeDirty) {

				Log.Message("VT state changes: new size {0}", physicalSize);

				SafeDispose( ref PhysicalPages0	);
				SafeDispose( ref PhysicalPages1	);
				SafeDispose( ref PhysicalPages2	);
				SafeDispose( ref PageTable		);
				SafeDispose( ref PageData		);
				SafeDispose( ref Params			);

				int tableSize	=	VTConfig.VirtualPageCount;
				int physSize	=	physicalSize;
				int physPages	=	physicalSize / VTConfig.PageSizeBordered;
				int maxTiles	=	physPages * physPages;

				PhysicalPages0	=	new Texture2D( rs.Device, physSize, physSize, ColorFormat.Rgba8_sRGB, false, true );
				PhysicalPages1	=	new Texture2D( rs.Device, physSize, physSize, ColorFormat.Rgba8,	  false, false );
				PhysicalPages2	=	new Texture2D( rs.Device, physSize, physSize, ColorFormat.Rgba8,	  false, false );
				PageTable		=	new RenderTarget2D( rs.Device, ColorFormat.Rgba32F, tableSize, tableSize, true, true );
				PageData		=	new StructuredBuffer( rs.Device, typeof(PageGpu), maxTiles, StructuredBufferFlags.None );
				Params			=	new ConstantBuffer( rs.Device, 16 );

				tileCache		=	new VTTileCache( physPages, physicalSize );

				PageScaleRCP	=	VTConfig.PageSize / (float)physSize;
				
				physicalSizeDirty	=	false;
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose ( bool disposing )
		{
			if (disposing) {

				SafeDispose( ref tileLoader );
				
				SafeDispose( ref PhysicalPages0	);
				SafeDispose( ref PhysicalPages1	);
				SafeDispose( ref PhysicalPages2	);
				SafeDispose( ref PageTable		);
				SafeDispose( ref PageData		);
				SafeDispose( ref Params			);
			}
			base.Dispose( disposing );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="baseDir"></param>
		public void Start ( VirtualTexture vt )
		{
			var storage			=	vt.TileStorage;

			tileStamper			=	new VTStamper();

			fontImage			=	Imaging.Image.LoadTga( new MemoryStream( Fusion.Properties.Resources.conchars ) );

			stopwatch.Restart();
		}



		/// <summary>
		/// 
		/// </summary>
		public void Stop ()
		{
			tileLoader.Purge();
			tileCache.Purge();
		}


		const int	BlockSizeX		=	16;
		const int	BlockSizeY		=	16;


		/// <summary>
		///	Updates page table using GPU
		/// </summary>
		void UpdatePageTable ()
		{
			int tableSize	=	VTConfig.VirtualPageCount;

			using (new PixEvent("UpdatePageTable")) {
				
				var pages = tileCache.GetGpuPageData();

				if (pages.Any()) {
					PageData.SetData( pages );
				}

				for (int mip=0; mip<VTConfig.MipCount; mip++) {

					Params.SetData( new Int4( pages.Length, mip, 0,0 ) );

					var device = Game.GraphicsDevice;
					device.ResetStates();
														
					device.PipelineState	=	factory[0];

					device.ComputeShaderConstants[0]	=	Params;
					device.ComputeShaderResources[0]	=	PageData;
					device.SetCSRWTexture( 0, PageTable.GetSurface(mip) );

					int targetSize	=	tableSize >> mip;
					int groupCountX	=	MathUtil.IntDivUp( targetSize, BlockSizeX );
					int groupCountY	=	MathUtil.IntDivUp( targetSize, BlockSizeX );

					device.Dispatch( groupCountX, groupCountY, 1 );
				}
			}
		}

		

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		public void Update ( VTAddress[] data, GameTime gameTime )
		{
			var feedback = data.Distinct().Where( p => p.Dummy!=0 ).ToArray();

			ApplyVTState();

			List<VTAddress> feedbackTree = new List<VTAddress>();

			//	
			//	Build tree :
			//
			foreach ( var addr in feedback ) {

				var paddr = addr;

				feedbackTree.Add( paddr );

				while (paddr.MipLevel < VTConfig.MaxMipLevel) {
					paddr = VTAddress.FromChild( paddr );
					feedbackTree.Add( paddr );
				}

			}

			//
			//	Distinct :
			//	
			feedbackTree = feedbackTree
			//	.Where( p0 => cache.Contains(p0) )
				.Distinct()
				.OrderBy( p1 => p1.MipLevel )
				.ToList();//*/


			//
			//	Detect thrashing and prevention
			//	Get highest mip, remove them, repeat until no thrashing occur.
			//
			while (feedbackTree.Count >= tileCache.Capacity/2 ) {
				int minMip = feedbackTree.Min( va => va.MipLevel );
				feedbackTree.RemoveAll( va => va.MipLevel == minMip );
			}


			if (LockTiles) {
				feedbackTree.Clear();
			}


			if (tileCache!=null) {
			}

			//
			//	Put into cache :
			//
			if (tileCache!=null && tileLoader!=null) {
				foreach ( var addr in feedbackTree ) {
				
					int physAddr;

					if ( tileCache.Add( addr, out physAddr ) ) {

						//Log.Message("...vt tile cache: {0} --> {1}", addr, physAddr );

						tileLoader.RequestTile( addr );
					}
				}
			}

			//
			//	update table :
			//
			if (tileLoader!=null && tileCache!=null) {

				for (int i=0; i<MaxPPF; i++) {
				
					VTTile tile;

					if (tileLoader.TryGetTile( out tile )) {

						Rectangle rect;

						if (tileCache.TranslateAddress( tile.VirtualAddress, tile, out rect )) {
							
							var sz = VTConfig.PageSizeBordered;

							if (RandomColor) {	
								tile.FillRandomColor();
							}

							if (ShowTileCheckers) {
								tile.DrawChecker();
							}

							if (ShowTileAddress) {	
								tile.DrawText( fontImage, 16,16, tile.VirtualAddress.ToString() );
								tile.DrawText( fontImage, 16,32, string.Format("{0} {1}", rect.X/sz, rect.Y/sz ) );
								tile.DrawText( fontImage, 16,48, Math.Floor(stopwatch.Elapsed.TotalMilliseconds).ToString() );
							}

							if (ShowTileBorder) {
								tile.DrawBorder();
							}

							//PhysicalPages.SetData( 0, rect, tile.Data, 0, tile.Data.Length );
							tileStamper?.Add( tile, rect );
						}

					}

				}


				//	emboss tiles to physical texture
				tileStamper?.Update( this, gameTime.ElapsedSec );

				//	update page table :
				UpdatePageTable();
			}
		}
	}
}
