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
using ShooterDemo.Core;
using BEPUphysics;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.EntityStateManagement;
using BEPUphysics.PositionUpdating;
//using BEPUphysics.


namespace ShooterDemo.Controllers {
	public class RigidBody : EntityController {

		readonly Space space;
		readonly Box box;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="game"></param>
		/// <param name="space"></param>
		public RigidBody ( Entity entity, World world, float w, float h, float d, float mass ) : base(entity,world)
		{
			this.space	=	((MPWorld)world).PhysSpace;

			var ms	=	new MotionState();
			ms.AngularVelocity	=	MathConverter.Convert( entity.AngularVelocity );
			ms.LinearVelocity	=	MathConverter.Convert( entity.LinearVelocity );
			ms.Orientation		=	MathConverter.Convert( entity.Rotation );
			ms.Position			=	MathConverter.Convert( entity.Position );
			box	=	new Box(  ms, w, h, d, mass );
			box.PositionUpdateMode	=	PositionUpdateMode.Continuous;

			box.Tag	=	entity;

			space.Add( box );
		}

		Random rand = new Random();


		/// <summary>
		/// 
		/// </summary>
		/// <param name="targetID"></param>
		/// <param name="attackerID"></param>
		/// <param name="damage"></param>
		/// <param name="kickImpulse"></param>
		/// <param name="kickPoint"></param>
		/// <param name="damageType"></param>
		public override bool Damage ( uint targetID, uint attackerID, short damage, Vector3 kickImpulse, Vector3 kickPoint, DamageType damageType )
		{
			var i = MathConverter.Convert( kickImpulse );
			var p = MathConverter.Convert( kickPoint );
			box.ApplyImpulse( p, i );

			return false;
		}




		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update ( float elapsedTime )
		{
			var e = Entity;

			e.Position			=	MathConverter.Convert( box.Position ); 
			e.Rotation			=	MathConverter.Convert( box.Orientation ); 
			e.LinearVelocity	=	MathConverter.Convert( box.LinearVelocity );
			e.AngularVelocity	=	MathConverter.Convert( box.AngularVelocity );
		}
	}
}
