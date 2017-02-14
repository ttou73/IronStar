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
using IronStar.Physics;

namespace IronStar.Mapping {

	public class MapModel : MapNode {


		[Category( "Appearance" )]
		[Description( "Path to FBX scene" )]
		[Editor( typeof( FbxFileLocationEditor ), typeof( UITypeEditor ) )]
		public string ScenePath { get; set; } = "";

		[Category( "Appearance" )]
		[Description( "Entire model scale" )]
		public float Scale { get; set; } = 1;

		[Category( "Appearance" )]
		[Description( "Model glow color multiplier" )]
		public Color4 Color { get; set; } = new Color4( 10, 10, 10, 1 );
		



		MeshInstance[] instances = null;
		StaticCollisionModel[] collidables = null;


		/// <summary>
		/// 
		/// </summary>
		public MapModel ()
		{
		}



		public override void SpawnEntity( GameWorld world )
		{
		}



		public override void ActivateEntity()
		{
		}



		public override void Draw( DebugRender dr, Color color, bool selected )
		{
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
			//world.Game.RenderSystem.RenderWorld.LightSet.SpotLights.Remove( light );
		}


		public override MapNode Duplicate()
		{
			var newNode = (MapModel)MemberwiseClone();
			//newNode.light = null;
			return newNode;
		}
	}
}
