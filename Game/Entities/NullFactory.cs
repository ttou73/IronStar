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

namespace IronStar.Entities {

	public class NullController : EntityController {
		public NullController( Entity entity, GameWorld world ) : base( entity, world )
		{
		}
	}


	public class NullFactory : EntityFactory {

		public override EntityController Spawn( Entity entity, GameWorld world )
		{
			return new NullController( entity, world );
		}
	}
}
