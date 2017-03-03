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


namespace Fusion.Engine.Graphics {
	public class LightGrid : DisposableBase {

		public readonly int Width;
		public readonly int Height;
		public readonly int Depth;

		public int Size { get { return Width * Height * Depth; } }
		
		Texture3D gridTexture;
		StructuredBuffer lightData;
		StructuredBuffer decalData;

		internal Texture3D GridTexture { get { return gridTexture;	} }
		internal StructuredBuffer LightData { get { return lightData; } }
		internal StructuredBuffer DecalData { get { return decalData; } }


		/// <summary>
		/// 
		/// </summary>
		/// <param name="rs"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="depth"></param>
		public LightGrid ( RenderSystem rs, int width, int height, int depth )
		{
			Width	=	width;
			Height	=	height;
			Depth	=	depth;

			gridTexture	=	new Texture3D( rs.Device, width, height, depth );

			lightData	=	new StructuredBuffer( rs.Device, typeof(SceneRenderer.LIGHT), 4096, StructuredBufferFlags.None );
			decalData	=	new StructuredBuffer( rs.Device, typeof(SceneRenderer.DECAL), 4096, StructuredBufferFlags.None );

			var rand = new Random();
			var data = new Int2[Size];

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

			ClusterizeOmniLights( view, proj, lightSet );
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="lightSet"></param>
		void ClusterizeOmniLights ( Matrix view, Matrix proj, LightSet lightSet )
		{
			//var vp = Game.GraphicsDevice.DisplayBounds;

			//omniLightData = Enumerable
			//		.Range(0,RenderSystem.MaxOmniLights)
			//		.Select( i => new OMNILIGHT(){ PositionRadius = Vector4.Zero, Intensity = Vector4.Zero })
			//		.ToArray();

			//int index = 0;

			//foreach ( var light in lightSet.OmniLights ) {

			//	Vector4 min, max;

			//	var visible = GetSphereExtent( view, proj, light.Position, vp, light.RadiusOuter, out min, out max );

			//	if (!visible) {
			//		continue;
			//	}

			//	omniLightData[index].PositionRadius	=	new Vector4( light.Position, light.RadiusOuter );
			//	omniLightData[index].Intensity		=	new Vector4( light.Intensity.ToVector3(), 1.0f / light.RadiusOuter / light.RadiusOuter );
			//	omniLightData[index].ExtentMax		=	max;
			//	omniLightData[index].ExtentMin		=	min;

			//	index++;
			//}

			////#warning Debug omni-lights.
			//#if true
			//if (ShowOmniLights) {
			//	var dr	=	rs.RenderWorld.Debug;

			//	foreach ( var light in lightSet.OmniLights ) {
			//		dr.DrawPoint( light.Position, 1, Color.LightYellow );
			//		dr.DrawSphere( light.Position, light.RadiusOuter, Color.LightYellow, 16 );
			//	}
			//}
			//#endif

			//omniLightBuffer.SetData( omniLightData );
		}



	}
}
