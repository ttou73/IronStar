using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEPUphysics.BroadPhaseEntries;
using Fusion.Core.Mathematics;
using Fusion.Engine.Graphics;
using IronStar.Core;
using IronStar.Physics;
using IronStar.SFX;
using IronStar.Physics;

namespace IronStar.Entities {

	public class StaticModel : EntityController {


		public class Factory : EntityFactory {

			/// <summary>
			/// 
			/// </summary>
			/// 
			[Category("Static Model")]
			public string Model { get; set; }


			/// <summary>
			/// 
			/// </summary>
			/// <param name="entity"></param>
			/// <param name="world"></param>
			/// <returns></returns>
			public override EntityController Spawn( Entity entity, GameWorld world )
			{
				return new StaticModel( entity, world, null );
			}

		}

		StaticCollisionModel	collisionModel;


		public StaticModel( Entity entity, GameWorld world, StaticModelFactory factory ) : base( entity, world )
		{
			entity.Model	=	world.Atoms[ factory.Model ];

			collisionModel	=	world.Physics.AddStaticCollisionModel( entity.Model, entity );
		}


		public override void Update( float elapsedTime )
		{
		}



		public override bool Damage( uint targetID, uint attackerID, short damage, Vector3 kickImpulse, Vector3 kickPoint, DamageType damageType )
		{
			return false;
		}


		public override void Killed()
		{
			World.Physics.Remove( collisionModel );
		}
	}
}
