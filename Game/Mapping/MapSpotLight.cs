using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Fusion.Core.Mathematics;
using IronStar.Core;
using Fusion.Engine.Graphics;
using IronStar.SFX;
using Fusion.Development;
using System.Drawing.Design;

namespace IronStar.Mapping {

	public class MapSpotLight : MapNode {

		[Category("Spot-light")]
		[Editor( typeof( SpotFileLocationEditor ), typeof( UITypeEditor ) )]
		public string SpotMaskName { get; set; } = "";
		
		
		[Category("Spot-light")]
		public float Intensity { get; set; } = 500;
		
		[Category("Spot-light")]
		public int Temperature { get; set; } = 2400;
		
		[Category("Spot-light")]
		public float Radius { get; set; } = 5;
		
		[Category("Spot-light")]
		public float NearPlane { get; set; } = 0.125f;
		
		[Category("Spot-light")]
		public float FarPlane { get; set; } = 5;
		
		[Category("Spot-light")]
		public float FovVertical { get; set; } = 30;
		
		[Category("Spot-light")]
		public float FovHorizontal { get; set; } = 30;


		public LightTemperature TemperaturePreset { get; set; } = LightTemperature.IncandescentStandard;


		SpotLight	light;


		[XmlIgnore]
		public Color4 LightColor {
			get {
				var temp	=	(TemperaturePreset == LightTemperature.Custom) ? Temperature : (int)TemperaturePreset;
				var color	=	Fusion.Core.Mathematics.Temperature.Get( temp );
				return 	new Color4( color.X * Intensity, color.Y * Intensity, color.Z * Intensity, 0 );
			}
		}


		Matrix SpotView {
			get {
				return	Matrix.Invert(WorldMatrix);
			}
		}

		Matrix SpotProjection {
			get {
				float n		=	NearPlane;
				float f		=	FarPlane;
				float w		=	(float)Math.Tan( MathUtil.DegreesToRadians( FovHorizontal/2 ) ) * Radius / 32.0f;
				float h		=	(float)Math.Tan( MathUtil.DegreesToRadians( FovVertical/2   ) ) * Radius / 32.0f;
				return	Matrix.PerspectiveRH( w, h, n, f );
			}
		}



		/// <summary>
		/// 
		/// </summary>
		public MapSpotLight ()
		{
		}



		public override void SpawnEntity( GameWorld world )
		{
			light		=	new SpotLight();

			float n		=	NearPlane;
			float f		=	FarPlane;
			float w		=	(float)Math.Tan( MathUtil.DegreesToRadians( FovHorizontal/2 ) ) * Radius / 32.0f;
			float h		=	(float)Math.Tan( MathUtil.DegreesToRadians( FovVertical/2   ) ) * Radius / 32.0f;

			light.Intensity		=	LightColor;
			light.SpotView		=	SpotView;
			light.Projection	=	SpotProjection;
			light.RadiusOuter	=	Radius;
			light.RadiusInner	=	0;
			light.TextureIndex	=	0;

			world.Game.RenderSystem.RenderWorld.LightSet.SpotLights.Add( light );
		}



		public override void ActivateEntity()
		{
		}



		public override void Draw( DebugRender dr, Color color, bool selected )
		{
			var transform	=	WorldMatrix;

			var max			= Math.Max( Math.Max( LightColor.Red, LightColor.Green ), Math.Max( LightColor.Blue, 1 ) );

			var dispColor   = new Color( (byte)(LightColor.Red / max * 255), (byte)(LightColor.Green / max * 255), (byte)(LightColor.Blue / max * 255), (byte)255 ); 

			dr.DrawPoint( transform.TranslationVector, 1, color, 2 );

			if (selected) {
				
				var frustum = new BoundingFrustum( SpotView * SpotProjection );
				
				var points  = frustum.GetCorners();

				dr.DrawLine( points[0], points[1], color );
				dr.DrawLine( points[1], points[2], color );
				dr.DrawLine( points[2], points[3], color );
				dr.DrawLine( points[3], points[0], color );

				dr.DrawLine( points[4], points[5], color );
				dr.DrawLine( points[5], points[6], color );
				dr.DrawLine( points[6], points[7], color );
				dr.DrawLine( points[7], points[4], color );

				dr.DrawLine( points[0], points[4], color );
				dr.DrawLine( points[1], points[5], color );
				dr.DrawLine( points[2], points[6], color );
				dr.DrawLine( points[3], points[7], color );

			} else {
			}
		}



		public override void ResetEntity( GameWorld world )
		{
			HardResetEntity( world );
		}



		public override void HardResetEntity( GameWorld world )
		{
			KillEntity( world );
			SpawnEntity( world );
		}



		public override void KillEntity( GameWorld world )
		{
			world.Game.RenderSystem.RenderWorld.LightSet.SpotLights.Remove( light );
		}


		public override MapNode Duplicate()
		{
			var newNode = (MapSpotLight)MemberwiseClone();
			newNode.light = null;
			return newNode;
		}
	}
}
