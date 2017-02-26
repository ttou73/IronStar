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


	public class MapOmniLight : MapNode {

		//[Category("Decal Image")]
		//[Editor( typeof( SpotFileLocationEditor ), typeof( UITypeEditor ) )]
		//public string SpotMaskName { get; set; } = "";
		
		
		[Category("Omni-light")]
		public float Intensity { get; set; } = 500;
		
		[Category("Omni-light")]
		public float Radius { get; set; } = 5;

		[Category("Omni-light")]
		public LightPreset LightPreset { get; set; } = LightPreset.IncandescentStandard;

		OmniLight	light;


		/// <summary>
		/// 
		/// </summary>
		public MapOmniLight ()
		{
		}



		public override void SpawnNode( GameWorld world )
		{
			if (!world.IsPresentationEnabled) {
				return;
			}

			light		=	new OmniLight();

			light.Intensity		=	LightPresetColor.GetColor( LightPreset, Intensity );;
			light.Position		=	WorldMatrix.TranslationVector;
			light.RadiusOuter	=	Radius;
			light.RadiusInner	=	0;

			world.Game.RenderSystem.RenderWorld.LightSet.OmniLights.Add( light );
		}



		public override void ActivateNode()
		{
		}



		public override void UseNode()
		{
		}



		public override void DrawNode( GameWorld world, DebugRender dr, Color color, bool selected )
		{
			var transform	=	WorldMatrix;

			var lightColor	=	LightPresetColor.GetColor( LightPreset, Intensity );

			var max			=	Math.Max( Math.Max( lightColor.Red, lightColor.Green ), Math.Max( lightColor.Blue, 1 ) );

			var dispColor   =	new Color( (byte)(lightColor.Red / max * 255), (byte)(lightColor.Green / max * 255), (byte)(lightColor.Blue / max * 255), (byte)255 ); 

			dr.DrawPoint( transform.TranslationVector, 1, color, 1 );

			if (selected) {
				dr.DrawSphere( transform.TranslationVector, Radius, dispColor );
			} else {
			}
		}



		public override void ResetNode( GameWorld world )
		{
			light.Position		=	WorldMatrix.TranslationVector;
		}



		public override void HardResetNode( GameWorld world )
		{
			KillNode( world );
			SpawnNode( world );
		}



		public override void KillNode( GameWorld world )
		{
			world.Game.RenderSystem.RenderWorld.LightSet.OmniLights.Remove( light );
		}


		public override MapNode DuplicateNode()
		{
			var newNode = (MapOmniLight)MemberwiseClone();
			newNode.light = null;
			return newNode;
		}
	}
}
