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
//using BEPUphysics.


namespace IronStar.Entities {
	public class RigidBody : EntityController {

		readonly Space space;
		readonly Box box;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="game"></param>
		/// <param name="space"></param>
		public RigidBody ( Entity entity, GameWorld world, RigidBodyFactory factory ) : base(entity,world)
		{
			this.space	=	world.PhysSpace;

			var width		=	factory.Width;
			var height		=	factory.Height;
			var depth		=	factory.Depth;
			var mass		=	factory.Mass;
			var model		=	factory.Model;

			var ms	=	new MotionState();
			ms.AngularVelocity	=	MathConverter.Convert( entity.AngularVelocity );
			ms.LinearVelocity	=	MathConverter.Convert( entity.LinearVelocity );
			ms.Orientation		=	MathConverter.Convert( entity.Rotation );
			ms.Position			=	MathConverter.Convert( entity.Position );
			box	=	new Box(  ms, width, height, depth, mass );
			box.PositionUpdateMode	=	PositionUpdateMode.Continuous;

			box.Tag	=	entity;

			entity.Model	=	world.Atoms[ model ];

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



		public override void Killed()
		{
			space.Remove(box);
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
