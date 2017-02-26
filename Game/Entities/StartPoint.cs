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

	public enum StartPointType {
		SinglePlayer,
		IntermissionCamera,
	}


	public class StartPoint : EntityController {

		public readonly StartPointType StartPointType;

		public StartPoint( Entity entity, GameWorld world, StartPointType startPointType ) : base( entity, world )
		{
			StartPointType = startPointType;
		}
	}



	public class StartPointFactory : EntityFactory {

		[Category( "Common" )]
		public StartPointType StartPointType { get; set; }

		public override EntityController Spawn( Entity entity, GameWorld world )
		{
			return new StartPoint( entity, world, StartPointType );
		}


		public override void Draw( DebugRender dr, Matrix transform, Color color )
		{
			var p0 = transform.TranslationVector;
			var p1 = transform.TranslationVector + Vector3.Up*2;
			var pf = transform.TranslationVector + transform.Forward;


			dr.DrawRing( p0, 0.50f, color, 16 );
			dr.DrawRing( p1, 0.50f, color, 16 );
			dr.DrawLine( p0, pf, color, color, 5, 1 );
		}
	}
}
