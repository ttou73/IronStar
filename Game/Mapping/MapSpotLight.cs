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
using Fusion;

namespace IronStar.Mapping {

	public class MapSpotLight : MapNode {

		[Category("Spot-light")]
		[Editor( typeof( SpotFileLocationEditor ), typeof( UITypeEditor ) )]
		public string SpotMaskName { get; set; } = "";
		
		
		[Category("Spot-light")]
		public float Intensity { get; set; } = 500;
		
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

		[Category("Spot-light")]
		public LightPreset LightPreset { get; set; } = LightPreset.IncandescentStandard;

		[Category("Depth biasing")]
		public float DepthBias { get; set; } = 1f / 1024f;

		[Category("Depth biasing")]
		public float SlopeBias { get; set; } = 2;


		SpotLight	light;


		Matrix SpotView {
			get {
				return	Matrix.Invert(WorldMatrix);
			}
		}

		Matrix SpotProjection {
			get {
				float n		=	NearPlane;
				float f		=	FarPlane;
				float w		=	(float)Math.Tan( MathUtil.DegreesToRadians( FovHorizontal/2 ) ) * NearPlane;
				float h		=	(float)Math.Tan( MathUtil.DegreesToRadians( FovVertical/2   ) ) * NearPlane;
				return	Matrix.PerspectiveRH( w, h, n, f );
			}
		}



		/// <summary>
		/// 
		/// </summary>
		public MapSpotLight ()
		{
		}



		public override void SpawnNode( GameWorld world )
		{
			if (!world.IsPresentationEnabled) {
				return;
			}

			var lightSet	=	world.Game.RenderSystem.RenderWorld.LightSet;

			var index		=	lightSet.SpotAtlas.IndexOf( SpotMaskName );

			if (index<0) {
				Log.Warning("Can not spawn spot light with spot mask : {0}", SpotMaskName );
				return;
			}

			light		=	new SpotLight();

			float n		=	NearPlane;
			float f		=	FarPlane;
			float w		=	(float)Math.Tan( MathUtil.DegreesToRadians( FovHorizontal/2 ) ) * Radius / 32.0f;
			float h		=	(float)Math.Tan( MathUtil.DegreesToRadians( FovVertical/2   ) ) * Radius / 32.0f;

			light.Intensity		=	LightPresetColor.GetColor( LightPreset, Intensity );;
			light.SpotView		=	SpotView;
			light.Projection	=	SpotProjection;
			light.RadiusOuter	=	Radius;
			light.RadiusInner	=	0;
			light.TextureIndex	=	index;

			light.DepthBias		=	DepthBias;
			light.SlopeBias		=	SlopeBias;

			world.Game.RenderSystem.RenderWorld.LightSet.SpotLights.Add( light );
		}



		public override void ActivateNode()
		{
		}



		public override void DrawNode( GameWorld world, DebugRender dr, Color color, bool selected )
		{
			var transform	=	WorldMatrix;

			var lightColor	=	LightPresetColor.GetColor( LightPreset, Intensity );;

			var max			= Math.Max( Math.Max( lightColor.Red, lightColor.Green ), Math.Max( lightColor.Blue, 1 ) );

			var dispColor   = new Color( (byte)(lightColor.Red / max * 255), (byte)(lightColor.Green / max * 255), (byte)(lightColor.Blue / max * 255), (byte)255 ); 

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



		public override void ResetNode( GameWorld world )
		{
			light.SpotView		=	SpotView;
			light.Projection	=	SpotProjection;
		}



		public override void HardResetNode( GameWorld world )
		{
			KillNode( world );
			SpawnNode( world );
		}



		public override void KillNode( GameWorld world )
		{
			world.Game.RenderSystem.RenderWorld.LightSet.SpotLights.Remove( light );
		}


		public override MapNode DuplicateNode()
		{
			var newNode = (MapSpotLight)MemberwiseClone();
			newNode.light = null;
			return newNode;
		}
	}
}
