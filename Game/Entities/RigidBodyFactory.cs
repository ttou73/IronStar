using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion;
using Fusion.Core;
using Fusion.Core.Content;
using Fusion.Core.Mathematics;
using Fusion.Core.Extensions;
using Fusion.Engine.Common;
using Fusion.Engine.Input;
using Fusion.Engine.Client;
using Fusion.Engine.Server;
using Fusion.Engine.Graphics;
using IronStar.Core;
using BEPUphysics;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.EntityStateManagement;
using BEPUphysics.PositionUpdating;
using Fusion.Core.IniParser.Model;
using System.ComponentModel;
//using BEPUphysics.


namespace IronStar.Entities {
	public class RigidBodyFactory : EntityFactory {

		[Category("Physics")]
		public float  Width  { get; set; } = 1;
		[Category("Physics")]
		public float  Height { get; set; } = 1;
		[Category("Physics")]
		public float  Depth  { get; set; } = 1;
		[Category("Physics")]
		public float  Mass   { get; set; } = 1;

		[Category("Appearance")]
		public string Model  { get; set; } = "";

		public override EntityController Spawn( Entity entity, GameWorld world )
		{
			return new RigidBody( entity, world, this );
		}


		public override void Draw( DebugRender dr, Matrix transform, Color color )
		{
			var w = Width/2;
			var h = Height/2;
			var d = Depth/2;

			dr.DrawBox( new BoundingBox( new Vector3(-w, -h, -d), new Vector3(w, h, d) ), transform, color );
			dr.DrawPoint( transform.TranslationVector, (w+h+d)/3/2, color );
		}
	}
}
