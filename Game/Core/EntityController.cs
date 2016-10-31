using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Common;
using Fusion.Core.Mathematics;


namespace ShooterDemo.Core {
	public abstract class EntityController {

		public readonly Game Game;
		public readonly World World;
		public readonly Entity Entity;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="world"></param>
		/// <param name="entity"></param>
		public EntityController ( Entity entity, World world, string parameters = "" )
		{
			this.World	=	world;
			this.Entity	=	entity;
			this.Game	=	world.Game;
		}


		/// <summary>
		/// Updates controller.
		/// </summary>
		/// <param name="gameTime"></param>
		public virtual void Update ( float elapsedTime ) 
		{
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="damage"></param>
		/// <param name="kickImpulse"></param>
		/// <param name="kickPoint"></param>
		/// <param name="damageType"></param>
		public virtual bool Damage ( uint targetID, uint attackerID, short damage, Vector3 kickImpulse, Vector3 kickPoint, DamageType damageType )
		{
			return false;
		}


		/// <summary>
		/// Called when entity has died.
		/// </summary>
		/// <param name="id"></param>
		public virtual void Killed () 
		{
		}
	}
}
