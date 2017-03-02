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
	internal partial class LightRenderer {

		/// <summary>
		/// 
		/// </summary>
		void ComputeOmniLightsTiles ( Matrix view, Matrix proj, LightSet lightSet )
		{
			var vp = Game.GraphicsDevice.DisplayBounds;

			omniLightData = Enumerable
					.Range(0,RenderSystem.MaxOmniLights)
					.Select( i => new OMNILIGHT(){ PositionRadius = Vector4.Zero, Intensity = Vector4.Zero })
					.ToArray();

			int index = 0;

			foreach ( var light in lightSet.OmniLights ) {

				Vector4 min, max;

				var visible = GetSphereExtent( view, proj, light.Position, vp, light.RadiusOuter, out min, out max );

				if (!visible) {
					continue;
				}

				omniLightData[index].PositionRadius	=	new Vector4( light.Position, light.RadiusOuter );
				omniLightData[index].Intensity		=	new Vector4( light.Intensity.ToVector3(), 1.0f / light.RadiusOuter / light.RadiusOuter );
				omniLightData[index].ExtentMax		=	max;
				omniLightData[index].ExtentMin		=	min;

				index++;
			}

			//#warning Debug omni-lights.
			#if true
			if (ShowOmniLights) {
				var dr	=	rs.RenderWorld.Debug;

				foreach ( var light in lightSet.OmniLights ) {
					dr.DrawPoint( light.Position, 1, Color.LightYellow );
					dr.DrawSphere( light.Position, light.RadiusOuter, Color.LightYellow, 16 );
				}
			}
			#endif

			omniLightBuffer.SetData( omniLightData );
		}



		/// <summary>
		/// 
		/// </summary>
		void ComputeEnvLightsTiles ( Matrix view, Matrix proj, LightSet lightSet )
		{
			var vp = Game.GraphicsDevice.DisplayBounds;

			envLightData = Enumerable
					.Range(0,RenderSystem.MaxEnvLights)
					.Select( i => new ENVLIGHT(){ Position = Vector4.Zero, Dimensions = Vector4.Zero })
					.ToArray();

			int index = 0;

			foreach ( var light in lightSet.EnvLights ) {

				Vector4 min, max;

				var m = Matrix.Scaling( light.Dimensions ) * 
						Matrix.Translation( light.Position );

				var visible = GetBasisExtent( view, proj, vp, m, out min, out max );

				/*if (!visible) {
					continue;
				} */

				envLightData[index].Position		=	new Vector4( light.Position, 1 );
				envLightData[index].Dimensions		=	new Vector4( light.Dimensions, light.Factor );
				envLightData[index].ExtentMax		=	max;
				envLightData[index].ExtentMin		=	min;

				index++;
			}

			envLightBuffer.SetData( envLightData );
		}



		/// <summary>
		/// 
		/// </summary>
		void ComputeDecalTiles ( Matrix view, Matrix proj, LightSet lightSet )
		{
			var vp = Game.GraphicsDevice.DisplayBounds;

			decalData = Enumerable
					.Range(0,RenderSystem.MaxOmniLights)
					.Select( i => new DECAL() )
					.ToArray();

			int index = 0;

			foreach ( var decal in lightSet.Decals ) {

				Vector4 min, max;

				var visible = GetBasisExtent( view, proj, vp, decal.DecalMatrix, out min, out max );

				if (!visible) {
					continue;
				}

				decalData[index].DecalMatrixInv		=	decal.DecalMatrixInverse;
				decalData[index].BasisX				=	new Vector4(decal.DecalMatrix.Right.Normalized(),	0);
				decalData[index].BasisY				=	new Vector4(decal.DecalMatrix.Up.Normalized(),		0);
				decalData[index].BasisZ				=	new Vector4(decal.DecalMatrix.Backward.Normalized(),0);
				decalData[index].BaseColorMetallic	=	new Vector4( decal.BaseColor.Red, decal.BaseColor.Green, decal.BaseColor.Blue, decal.Metallic );
				decalData[index].EmissionRoughness	=	new Vector4( decal.Emission.Red, decal.Emission.Green, decal.Emission.Blue, decal.Roughness );
				decalData[index].ExtentMax			=	max;
				decalData[index].ExtentMin			=	min;
				decalData[index].ColorFactor		=	decal.ColorFactor;
				decalData[index].SpecularFactor		=	decal.SpecularFactor;
				decalData[index].NormalMapFactor	=	decal.NormalMapFactor;
				decalData[index].FalloffFactor		=	decal.FalloffFactor;
				decalData[index].ImageScaleOffset	=	decal.GetScaleOffset();

				// apply gamma correction :
				decalData[index].BaseColorMetallic.X	=	(float)Math.Pow( decalData[index].BaseColorMetallic.X, 2.2f );
				decalData[index].BaseColorMetallic.Y	=	(float)Math.Pow( decalData[index].BaseColorMetallic.Y, 2.2f );
				decalData[index].BaseColorMetallic.Z	=	(float)Math.Pow( decalData[index].BaseColorMetallic.Z, 2.2f );

				index++;
			}

			decalBuffer.SetData( decalData );
		}




		/// <summary>
		/// 
		/// </summary>
		void ComputeSpotLightsTiles ( Matrix view, Matrix projection, LightSet lightSet )
		{
			var znear	=	projection.M34 * projection.M43 / projection.M33;
			var vp		=	Game.GraphicsDevice.DisplayBounds;

			spotLightData	=	Enumerable
							.Range(0, RenderSystem.MaxSpotLights)
							.Select( i => new SPOTLIGHT() )
							.ToArray();

			int index	=	0;
			int spotId	=	0;

			
			foreach ( var spot in lightSet.SpotLights ) {

				var shadowSO	=	new Vector4( 0.125f, -0.125f, 0.25f*(spotId % 4)+0.125f, 0.25f*(spotId / 4)+0.125f );
				spotId ++;

				var maskSO		=	Vector4.Zero;
				
				if (lightSet.SpotAtlas!=null) {
					var maskRect	=	lightSet.SpotAtlas[ spot.TextureIndex ];
					var maskX		=	maskRect.Left   / (float)lightSet.SpotAtlas.Texture.Width;
					var maskY		=	maskRect.Top    / (float)lightSet.SpotAtlas.Texture.Height;
					var maskW		=	maskRect.Width  / (float)lightSet.SpotAtlas.Texture.Width;
					var maskH		=	maskRect.Height / (float)lightSet.SpotAtlas.Texture.Height;
					maskSO			=	new Vector4( maskW*0.5f, -maskH*0.5f, maskX + maskW/2f, maskY + maskH/2f );
				}

				var bf = new BoundingFrustum( spot.SpotView * spot.Projection );
				var pos = Matrix.Invert(spot.SpotView).TranslationVector;

				//#warning Debug spot-lights.
				#if false
				if (Config.ShowSpotLights) {
					dr.DrawPoint( pos, 0.5f, Color.LightYellow );
					dr.DrawFrustum( bf, Color.LightYellow );
				}
				#endif

				Vector4 min, max;

				bool r = GetFrustumExtent( view, projection, vp, bf, out min, out max );

				if (r) {
					spotLightData[index].ViewProjection		=	spot.SpotView * spot.Projection;
					spotLightData[index].PositionRadius		=	new Vector4( pos, spot.RadiusOuter );
					spotLightData[index].IntensityFar		=	spot.Intensity.ToVector4();
					spotLightData[index].IntensityFar.W		=	spot.Projection.GetFarPlaneDistance();
					spotLightData[index].ExtentMin			=	min;
					spotLightData[index].ExtentMax			=	max;
					spotLightData[index].MaskScaleOffset	=	maskSO;
					spotLightData[index].ShadowScaleOffset	=	shadowSO;
					index ++;
				}

			}

			spotLightBuffer.SetData( spotLightData );
		}


	}
}
