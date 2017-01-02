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
using Fusion.Engine.Common;
using Fusion.Engine.Input;
using Fusion.Engine.Client;
using Fusion.Engine.Server;
using Fusion.Engine.Graphics;
using IronStar.Core;
using System.ComponentModel;

namespace IronStar.Entities {

	public enum SpawnTarget {
		SinglePlayer,
		IntermissionCamera,
	}


	public class SpawnPoint : EntityController {

		public readonly SpawnTarget SpawnTarget;

		public SpawnPoint( Entity entity, GameWorld world, SpawnTarget target ) : base( entity, world )
		{
			SpawnTarget = target;
		}
	}



	public class SpawnPointFactory : EntityFactory {

		[Category("Common")]
		public SpawnTarget SpawnTarget { get; set; }

		public override EntityController Spawn( Entity entity, GameWorld world )
		{
			return new SpawnPoint( entity, world, SpawnTarget );
		}
	}
}
