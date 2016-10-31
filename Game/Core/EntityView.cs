using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Common;


namespace ShooterDemo.Core {
	public abstract class EntityView {

		public readonly Game Game;
		public readonly World World;
		public readonly Entity Entity;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="game"></param>
		public EntityView ( Entity entity, World world )
		{
			this.World	=	world;
			this.Entity	=	entity;
			this.Game	=	world.Game;
		}

		/// <summary>
		/// Called on each viewable entity.
		/// </summary>
		/// <param name="entity"></param>
		public virtual void Update ( float elapsedTime, float lerpFactor ) {}

		/// <summary>
		/// Called when entity has died.
		/// </summary>
		/// <param name="id"></param>
		public virtual void Killed () {}
	}
}
