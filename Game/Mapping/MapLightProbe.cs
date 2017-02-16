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

	public class MapLightProbe : MapNode {

		[Category("Light probe")]
		[Editor( typeof( SpotFileLocationEditor ), typeof( UITypeEditor ) )]
		public string SpotMaskName { get; set; } = "";
		
		
		[Category("Light probe")]
		public float Width { get; set; } = 2;
		
		[Category("Light probe")]
		public float Height { get; set; } = 2;
		
		[Category("Light probe")]
		public float Depth { get; set; } = 2;


		EnvLight	light;



		/// <summary>
		/// 
		/// </summary>
		public MapLightProbe ()
		{
		}



		public override void SpawnNode( GameWorld world )
		{
			if (!world.IsPresentationEnabled) {
				return;
			}

			var lightSet	=	world.Game.RenderSystem.RenderWorld.LightSet;

			light	=	new EnvLight( WorldMatrix.TranslationVector, 2, 4 );

			lightSet.EnvLights.Add( light );
		}



		public override void ActivateNode()
		{
		}



		public override void DrawNode( DebugRender dr, Color color, bool selected )
		{
			dr.DrawPoint( WorldMatrix.TranslationVector, 1, color, 2 );

			var bbox	=	new BoundingBox( Width, Height, Depth );

			if (selected) {
				
				dr.DrawBox( bbox, WorldMatrix, color );

			} 
		}



		public override void ResetNode( GameWorld world )
		{
			if (light!=null) {
				light.Position		=	WorldMatrix.TranslationVector;
			}
		}



		public override void HardResetNode( GameWorld world )
		{
			KillNode( world );
			SpawnNode( world );
		}



		public override void KillNode( GameWorld world )
		{
			world.Game.RenderSystem.RenderWorld.LightSet.EnvLights.Remove( light );
		}


		public override MapNode DuplicateNode()
		{
			var newNode = (MapLightProbe)MemberwiseClone();
			newNode.light = null;
			return newNode;
		}
	}
}
