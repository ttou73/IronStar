using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core;
using Fusion.Core.Mathematics;
using Fusion.Core.Configuration;
using Fusion.Engine.Common;
using Fusion.Drivers.Graphics;
using System.Runtime.InteropServices;
using Fusion.Engine.Graphics.Lights;


namespace Fusion.Engine.Graphics {
	public class LightGrid : DisposableBase {

		const int MaxLights = 4096;
		const int MaxDecals = 4096;
		const int IndexTableSize = 256 * 512;

		public readonly Game Game;
		public readonly int Width;
		public readonly int Height;
		public readonly int Depth;

		readonly RenderSystem rs;

		public int GridLinearSize { get { return Width * Height * Depth; } }
		
		Texture3D gridTexture;
		FormattedBuffer  indexData;
		StructuredBuffer lightData;
		StructuredBuffer decalData;

		internal Texture3D GridTexture { get { return gridTexture;	} }
		internal StructuredBuffer LightDataGpu { get { return lightData; } }
		internal StructuredBuffer DecalDataGpu { get { return decalData; } }
		internal FormattedBuffer  IndexDataGpu { get { return indexData; } }


		/// <summary>
		/// 
		/// </summary>
		/// <param name="rs"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="depth"></param>
		public LightGrid ( RenderSystem rs, int width, int height, int depth )
		{
			this.rs	=	rs;
			Game	=	rs.Game;
			Width	=	width;
			Height	=	height;
			Depth	=	depth;

			gridTexture	=	new Texture3D( rs.Device, width, height, depth );

			lightData	=	new StructuredBuffer( rs.Device, typeof(SceneRenderer.LIGHT), MaxLights, StructuredBufferFlags.None );
			decalData	=	new StructuredBuffer( rs.Device, typeof(SceneRenderer.DECAL), MaxDecals, StructuredBufferFlags.None );
			indexData	=	new FormattedBuffer( rs.Device, Drivers.Graphics.VertexFormat.UInt, IndexTableSize, StructuredBufferFlags.None ); 

			var rand = new Random();
			var data = new Int2[GridLinearSize];

			for (int i=0; i<Width; i++) {
				for (int j=0; j<Height; j++) {
					for (int k=0; k<Depth; k++) {
						int a = ComputeAddress(i,j,k);
						data[a] = new Int2(rand.Next(10),rand.Next(10));
					}
				}
			}

			gridTexture.SetData( data );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		int	ComputeAddress ( int x, int y, int z ) 
		{
			return x + y * Width + z * Height*Width;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose( bool disposing )
		{
			if (disposing) {
				SafeDispose( ref gridTexture );
				SafeDispose( ref indexData );
				SafeDispose( ref lightData );
				SafeDispose( ref decalData );
			}

			base.Dispose( disposing );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="lightSet"></param>
		public void ClusterizeLightSet ( StereoEye stereoEye, Camera camera, LightSet lightSet )
		{
			var view = camera.GetViewMatrix( stereoEye );
			var proj = camera.GetProjectionMatrix( stereoEye );

			UpdateOmniLightExtentsAndVisibility( view, proj, lightSet );
			ClusterizeOmniLights( view, proj, lightSet );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="proj"></param>
		/// <param name="lightSet"></param>
		void UpdateOmniLightExtentsAndVisibility ( Matrix view, Matrix proj, LightSet lightSet )
		{
			var vp = new Rectangle(0,0,1,1);

			foreach ( var ol in lightSet.OmniLights ) {

				Vector4 min, max;
				ol.Visible	=	false;

				if ( Extents.GetSphereExtent( view, proj, ol.Position, vp, ol.RadiusOuter, false, out min, out max ) ) {

					min.Z	=	( 1 - (float)Math.Exp( 0.03f * ( min.Z ) ) );
					max.Z	=	( 1 - (float)Math.Exp( 0.03f * ( max.Z ) ) );

					ol.Visible		=	true;

					ol.MaxExtent.X	=	Math.Min( Width,  (int)Math.Ceiling( max.X * Width  ) );
					ol.MaxExtent.Y	=	Math.Min( Height, (int)Math.Ceiling( max.Y * Height ) );
					ol.MaxExtent.Z	=	Math.Min( Depth,  (int)Math.Ceiling( max.Z * Depth  ) );

					ol.MinExtent.X	=	Math.Max( 0, (int)Math.Floor( min.X * Width  ) );
					ol.MinExtent.Y	=	Math.Max( 0, (int)Math.Floor( min.Y * Height ) );
					ol.MinExtent.Z	=	Math.Max( 0, (int)Math.Floor( min.Z * Depth  ) );
				}
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="lightSet"></param>
		void ClusterizeOmniLights ( Matrix view, Matrix proj, LightSet lightSet )
		{
			var vp = new Rectangle(0,0,1,1);

			var lightGrid	=	new SceneRenderer.LIGHTINDEX[GridLinearSize];

			var lightData	=	new SceneRenderer.LIGHT[MaxLights];


			foreach ( var ol in lightSet.OmniLights ) {
				if (ol.Visible) {
					for (int i=ol.MinExtent.X; i<ol.MaxExtent.X; i++)
					for (int j=ol.MinExtent.Y; j<ol.MaxExtent.Y; j++)
					for (int k=ol.MinExtent.Z; k<ol.MaxExtent.Z; k++) {
						int a = ComputeAddress(i,j,k);
						lightGrid[a].AddLight();
					}
				}
			}



			uint offset = 0;
			for ( int i=0; i<lightGrid.Length; i++ ) {

				lightGrid[i].Offset = offset;

				offset += lightGrid[i].LightCount;
				offset += lightGrid[i].DecalCount;

				lightGrid[i].Count	= 0;
			}

			var indexData	=	new uint[ offset + 1 /* one extra element */ ];


			uint index = 0;
			foreach ( var ol in lightSet.OmniLights ) {
				if (ol.Visible) {
					for (int i=ol.MinExtent.X; i<ol.MaxExtent.X; i++)
					for (int j=ol.MinExtent.Y; j<ol.MaxExtent.Y; j++)
					for (int k=ol.MinExtent.Z; k<ol.MaxExtent.Z; k++) {
						int a = ComputeAddress(i,j,k);
						indexData[ lightGrid[a].Offset + lightGrid[a].LightCount ] = index;
						lightGrid[a].AddLight();
					}

					lightData[index].LightType		=	SceneRenderer.LightTypeOmni;
					lightData[index].PositionRadius	=	new Vector4( ol.Position, ol.RadiusOuter );
					lightData[index].IntensityFar	=	new Vector4( ol.Intensity.Red, ol.Intensity.Green, ol.Intensity.Blue, 0 );

					index++;
				}
			}


			using ( new PixEvent( "Update cluster structures" ) ) {
				LightDataGpu.SetData( lightData );
				IndexDataGpu.SetData( indexData );
				gridTexture.SetData( lightGrid );
			}
		}



	}
}
