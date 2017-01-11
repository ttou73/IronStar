using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion.Core.Mathematics;
using Fusion.Core.Shell;
using Fusion.Engine.Imaging;
using Fusion.Build.Processors;
using Fusion.Engine.Graphics;
using Fusion.Engine.Storage;
using Fusion.Core.IniParser;
using Fusion.Core.IniParser.Model;
using Fusion.Core.Extensions;
using System.Diagnostics;
using Fusion.Core.Content;

namespace Fusion.Build.Mapping {

	[AssetProcessor("MegaTexture", "Performs megatexture assembly")]
	public class VTProcessor : AssetProcessor {


		const string targetMegatexture	=	".megatexture";
		const string targetAllocator	=	".allocator";


		/// <summary>
		/// 
		/// </summary>
		public VTProcessor ()
		{
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="buildContext"></param>
		public override void Process ( AssetSource assetFile, BuildContext context )
		{
			Log.Message("-------- Virtual Texture --------" );

			var stopwatch	=	new Stopwatch();
			stopwatch.Start();

			var xmlFiles	=	Directory.EnumerateFiles( Path.Combine(Builder.FullInputDirectory, "vt"), "*.xml").ToList();

			Log.Message("{0} megatexture segments", xmlFiles.Count);


			//
			//	Process tiles :
			//
			using ( var tileStorage = context.GetVTStorage() ) {

				var pageTable	=	CreateVTTextureTable( xmlFiles, context, tileStorage );


				//
				//	Get allocator and pack/repack textures :
				//	
				Allocator2D allocator = null;

				if (tileStorage.FileExists( targetAllocator ) && tileStorage.FileExists( targetMegatexture ) ) {

					Log.Message("Loading VT allocator...");

					using ( var allocStream = tileStorage.OpenRead( targetAllocator ) ) {

						allocator		=	Allocator2D.LoadState( allocStream );
						var targetTime	=	tileStorage.GetLastWriteTimeUtc(targetMegatexture);

						Log.Message("Repacking textures to atlas...");
						RepackTextureAtlas( pageTable, allocator, targetTime );
					}
			
				} else {
			
					allocator = new Allocator2D(VTConfig.VirtualPageCount);

					Log.Message("Packing ALL textures to atlas...");
					PackTextureAtlas( pageTable.SourceTextures, allocator );

				}

				Log.Message("Saving VT allocator...");

				using ( var allocStream = tileStorage.OpenWrite( targetAllocator ) ) {
					Allocator2D.SaveState( allocStream, allocator );
				}

				//
				//	Generate top-level pages :
				//
				Log.Message( "Generating pages..." );
				GenerateMostDetailedPages( pageTable.SourceTextures, context, pageTable, tileStorage );


				//
				//	Generate mip-maps :
				//
				Log.Message("Generating mipmaps...");
				for (int mip=0; mip<VTConfig.MipCount-1; mip++) {
					Log.Message("Generating mip level {0}/{1}...", mip, VTConfig.MipCount);
					GenerateMipLevels( context, pageTable, mip, tileStorage );
				}


				//
				//	Write asset :
				//
				using ( var stream = tileStorage.OpenWrite( targetMegatexture ) ) {
					using ( var assetStream = AssetStream.OpenWrite( stream, "", new[] {""} ) ) {
						using ( var sw = new BinaryWriter( assetStream ) ) {
							sw.Write( pageTable.SourceTextures.Count );

							foreach ( var tex in pageTable.SourceTextures ) {
								VTTexture.Write( tex, sw );
							}
						}
					}
				}
			}

			stopwatch.Stop();
			Log.Message("{0}", stopwatch.Elapsed.ToString() );

			Log.Message("----------------" );
		}


																				 

		/// <summary>
		/// Creates VT tex table from INI-data and base directory
		/// </summary>
		/// <param name="iniData"></param>
		/// <param name="baseDirectory"></param>
		/// <returns></returns>
		VTTextureTable CreateVTTextureTable ( IEnumerable<string> xmlFilePaths, BuildContext context, IStorage tileStorage )
		{
			var texTable	=	new VTTextureTable();

			foreach ( var xmlFile in xmlFilePaths ) {

				using ( var stream = File.OpenRead( xmlFile ) ) {

					var name		=	Path.GetFileNameWithoutExtension( xmlFile );
					var content		=	(VTTextureContent)Misc.LoadObjectFromXml( typeof(VTTextureContent), stream );
					var writeTime	=	File.GetLastWriteTimeUtc( xmlFile );

					var tex = new VTTexture( content, name, context, writeTime );

					texTable.AddTexture( tex );
				}
			}

			return texTable;
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="textures"></param>
		void PackTextureAtlas ( IEnumerable<VTTexture> textures, Allocator2D allocator )
		{
			foreach ( var tex in textures ) {

				var size = Math.Max(tex.Width/128, tex.Height/128);

				var addr = allocator.Alloc( size, tex.Name );

				tex.TexelOffsetX	=	addr.X * VTConfig.PageSize;
				tex.TexelOffsetY	=	addr.Y * VTConfig.PageSize;
				tex.TilesDirty		=	true;

				Log.Message("...add: {0} : {1}x{2} : tile[{3},{4}]", tex.Name, tex.Width, tex.Height, addr.X, addr.Y );

			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="textures"></param>
		void RepackTextureAtlas ( VTTextureTable vtexTable, Allocator2D allocator, DateTime targetWriteTime )
		{
			//
			//	remove deleted and changes textures from allocator :
			//
			var blockInfo = allocator.GetAllocatedBlockInfo();

			foreach ( var block in blockInfo ) {

				if (!vtexTable.Contains( block.Tag )) {
			
					Log.Message("...removed: {0}", block.Tag);
					allocator.Free( block.Address );

				} else {
					
					if (vtexTable[ block.Tag ].IsModified(targetWriteTime)) {
						Log.Message("...changed: {0}", block.Tag );
						allocator.Free( block.Address );
					} 
				}
			}


			//
			//	add missing textures (note, changed are already removed).
			//
			var blockDictionary	=	allocator.GetAllocatedBlockInfo().ToDictionary( bi => bi.Tag );
			var newTextureList		=	new List<VTTexture>();

			foreach ( var tex in vtexTable.SourceTextures ) {
				Allocator2D.BlockInfo bi;

				if (blockDictionary.TryGetValue( tex.Name, out bi )) {

					tex.TexelOffsetX = bi.Address.X * VTConfig.PageSize;
					tex.TexelOffsetY = bi.Address.Y * VTConfig.PageSize;

				} else {
					newTextureList.Add( tex );
				}
			}

			PackTextureAtlas( newTextureList, allocator );
		}



		/// <summary>
		/// Split textures on tiles.
		/// </summary>
		/// <param name="textures"></param>
		void GenerateMostDetailedPages ( ICollection<VTTexture> textures, BuildContext context, VTTextureTable pageTable, IStorage mapStorage )
		{
			int totalCount = textures.Count;
			int counter = 0;

			foreach ( var texture in textures ) {
				if (texture.TilesDirty) {
					Log.Message("...{0}/{1} - {2}", counter, totalCount, texture.Name );
					texture.SplitIntoPages( context, pageTable, mapStorage );
					counter++;
				}
			}
		}



		/// <summary>
		/// Generates mip levels for all tiles.
		/// </summary>
		/// <param name="buildContext"></param>
		/// <param name="pageTable"></param>
		/// <param name="sourceMipLevel"></param>
		/// <param name="mapStorage"></param>
		void GenerateMipLevels ( BuildContext buildContext, VTTextureTable pageTable, int sourceMipLevel, IStorage mapStorage )
		{
			if (sourceMipLevel>=VTConfig.MipCount) {
				throw new ArgumentOutOfRangeException("mipLevel");
			}

			int count	= VTConfig.VirtualPageCount >> sourceMipLevel;
			int sizeB	= VTConfig.PageSizeBordered;
			var cache	= new TileSamplerCache( mapStorage, pageTable ); 

			for ( int pageX = 0; pageX < count; pageX+=2 ) {
				for ( int pageY = 0; pageY < count; pageY+=2 ) {

					var address00 = new VTAddress( pageX + 0, pageY + 0, sourceMipLevel );
					var address01 = new VTAddress( pageX + 0, pageY + 1, sourceMipLevel );
					var address10 = new VTAddress( pageX + 1, pageY + 0, sourceMipLevel );
					var address11 = new VTAddress( pageX + 1, pageY + 1, sourceMipLevel );
					
					//	there are no images touching target mip-level.
					//	NOTE: we can skip images that are touched by border.
					if ( !pageTable.IsAnyExists( address00, address01, address10, address11 ) ) {
						continue;
					}

					var address =	new VTAddress( pageX/2, pageY/2, sourceMipLevel+1 );

					var tile	=	new VTTile(address);

					var offsetX	=	(pageX) * VTConfig.PageSize;
					var offsetY	=	(pageY) * VTConfig.PageSize;
					var border	=	VTConfig.PageBorderWidth;

					var colorValue	=	Color.Zero;
					var normalValue	=	Color.Zero;
					var specularValue	=	Color.Zero;

					for ( int x=0; x<sizeB; x++) {
						for ( int y=0; y<sizeB; y++) {

							int srcX	=	offsetX + x*2 - border * 2;
							int srcY	=	offsetY + y*2 - border * 2;

							SampleMegatextureQ4( cache, srcX, srcY, sourceMipLevel, ref colorValue, ref normalValue, ref specularValue );
							
							tile.SetValues( x, y, ref colorValue, ref normalValue, ref specularValue );

						}
					}

					pageTable.Add( address );
					pageTable.SaveTile( address, mapStorage, tile );
				}
			}

		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="texelX"></param>
		/// <param name="texelY"></param>
		/// <param name="mipLevel"></param>
		/// <returns></returns>
		void SampleMegatextureQ4 ( TileSamplerCache cache, int texelX, int texelY, int mipLevel, ref Color a, ref Color b, ref Color c )
		{
			int textureSize	=	VTConfig.TextureSize >> mipLevel;
			
			texelX = MathUtil.Clamp( 0, texelX, textureSize );
			texelY = MathUtil.Clamp( 0, texelY, textureSize );

			int pageX	= texelX / VTConfig.PageSize;
			int pageY	= texelY / VTConfig.PageSize;
			int x		= texelX % VTConfig.PageSize;
			int y		= texelY % VTConfig.PageSize;
			int pbw		= VTConfig.PageBorderWidth;

			var address = new VTAddress( pageX, pageY, mipLevel );

			cache.LoadImage( address ).SampleQ4( x+pbw, y+pbw, ref a, ref b, ref c );
		}



		/// <summary>
		/// Generate one big texture where all textures are presented.
		/// </summary>
		void GenerateFallbackImage ( BuildContext buildContext, VTTextureTable pageTable, int sourceMipLevel, IStorage storage )
		{
			int		pageSize		=	VTConfig.PageSize;
			int		numPages		=	VTConfig.VirtualPageCount >> sourceMipLevel;
			int		fallbackSize	=	VTConfig.TextureSize >> sourceMipLevel;

			Image fallbackImage =	new Image( fallbackSize, fallbackSize, Color.Black );

			for ( int pageX=0; pageX<numPages; pageX++) {
				for ( int pageY=0; pageY<numPages; pageY++) {

					var addr	=	new VTAddress( pageX, pageY, sourceMipLevel );
					var image	=	pageTable.LoadPage( addr, storage );

					for ( int x=0; x<pageSize; x++) {
						for ( int y=0; y<pageSize; y++) {

							int u = pageX * pageSize + x;
							int v = pageY * pageSize + y;

							fallbackImage.Write( u, v, image.Sample( x, y ) );
						}
					}
				}
			}

			Image.SaveTga( fallbackImage, storage.OpenWrite("fallback.tga") );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="image00"></param>
		/// <param name="image01"></param>
		/// <param name="image10"></param>
		/// <param name="image11"></param>
		/// <returns></returns>
		Image MipImages ( Image image00, Image image01, Image image10, Image image11 )
		{
			const int pageSize = VTConfig.PageSize;

			if (image00.Width!=image00.Height || image00.Width != pageSize ) {
				throw new ArgumentException("Bad image size");
			}
			if (image01.Width!=image01.Height || image01.Width != pageSize ) {
				throw new ArgumentException("Bad image size");
			}
			if (image10.Width!=image10.Height || image10.Width != pageSize ) {
				throw new ArgumentException("Bad image size");
			}
			if (image11.Width!=image11.Height || image11.Width != pageSize ) {
				throw new ArgumentException("Bad image size");
			}

			var image = new Image( pageSize, pageSize, Color.Black );

			for ( int i=0; i<pageSize/2; i++) {
				for ( int j=0; j<pageSize/2; j++) {
					image.Write( i,j, image00.SampleMip( i, j ) );
				}
			}

			for ( int i=pageSize/2; i<pageSize; i++) {
				for ( int j=pageSize/2; j<pageSize; j++) {
					image.Write( i,j, image11.SampleMip( i, j ) );
				}
			}

			for ( int i=0; i<pageSize/2; i++) {
				for ( int j=pageSize/2; j<pageSize; j++) {
					image.Write( i,j, image01.SampleMip( i, j ) );
				}
			}

			for ( int i=pageSize/2; i<pageSize; i++) {
				for ( int j=0; j<pageSize/2; j++) {
					image.Write( i,j, image10.SampleMip( i, j ) );
				}
			}

			return image;
		} 


	}
}
