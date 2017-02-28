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
using BEPUphysics;
using BEPUphysics.Character;
using Fusion.Core.IniParser.Model;


namespace IronStar.Entities {
	public partial class Character : EntityController {

		readonly Space space;

		const float StepRate = 0.3f;


		CharacterController controller;
		float		stepCounter = 0;
		bool		rlStep		= false;
		bool		oldTraction = true;
		Vector3		oldVelocity = Vector3.Zero;
		readonly	float heightStanding;
		readonly	float heightCrouch;
		readonly	Vector3 offsetCrouch;
		readonly	Vector3 offsetStanding;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="game"></param>
		/// <param name="space"></param>
		public Character ( Entity entity, GameWorld world, CharacterFactory factory ) : base(entity,world)
		{
			this.space	=	world.PhysSpace;

			heightCrouch	=	factory.CrouchingHeight;
			heightStanding	=	factory.Height;
			offsetCrouch	=	Vector3.Up * heightCrouch / 2;
			offsetStanding	=	Vector3.Up * heightStanding / 2;


			var pos = MathConverter.Convert( entity.Position + offsetStanding );

			controller = new CharacterController( pos, 
					factory.Height				, 
					factory.CrouchingHeight		, 
					factory.Radius				, 
					factory.Margin				, 
					factory.Mass				,
					factory.MaximumTractionSlope, 
					factory.MaximumSupportSlope	, 
					factory.StandingSpeed		,
					factory.CrouchingSpeed		,
					factory.TractionForce		, 
					factory.SlidingSpeed		,
					factory.SlidingForce		,
					factory.AirSpeed			,
					factory.AirForce			, 
					factory.JumpSpeed			, 
					factory.SlidingJumpSpeed	,
					factory.MaximumGlueForce	);


			controller.StepManager.MaximumStepHeight	=	factory.MaxStepHeight;
			controller.Body.Tag	=	entity;
			controller.Tag		=	entity;

			space.Add( controller );


			entity.ActiveItem	=	Inventory.Machinegun;

			entity.SetItemCount( Inventory.Health			,	100	);
			entity.SetItemCount( Inventory.Armor			,	0	);

			entity.SetItemCount( Inventory.Bullets			,	999	);
			entity.SetItemCount( Inventory.Machinegun		,	1	);

			entity.SetItemCount( Inventory.Rockets			,	150	);
			entity.SetItemCount( Inventory.RocketLauncher	,	1	);

			entity.SetItemCount( Inventory.Slugs			,	150	);
			entity.SetItemCount( Inventory.Railgun			,	1	);

			entity.SetItemCount( Inventory.Cells			,	999	);
			entity.SetItemCount( Inventory.HyperBlaster		,	1	);
		}



		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual Vector3 GetPOV ()
		{
			return Entity.Position + Vector3.Up * (IsCrouching ? 0.8f : 1.8f);
		}



		bool IsCrouching {
			get {
				return controller.StanceManager.CurrentStance == Stance.Crouching;
			}
		}



		Vector3 GetPovOffset ()
		{
			return IsCrouching ? offsetCrouch : offsetStanding;
		}


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
			if (damage<=0) {
				return false;
			}

			var c = controller;
			var e = Entity;

			c.SupportFinder.ClearSupportData();
			var i = MathConverter.Convert( kickImpulse );
			var p = MathConverter.Convert( kickPoint );
			c.Body.ApplyImpulse( p, i );

			/**************************************************
			 * 
			 *	1. Accumulate damage and emit FX according to 
			 *	maximum inflicted damage.
			 *	Probably we have to add new controller stage 
			 *	for FX processing (e.g. Update and UpdateFX).
			 *	
			 *	2. Do not scream at each damage. 
			 *	Screams should not overlap!
			 * 
			**************************************************/

			//
			//	calc health :
			//
			var health	=	e.GetItemCount( Inventory.Health );
			health -= damage;

			var dir = kickImpulse.Normalized();

			if (health>75) {
				World.SpawnFX("PlayerPain25", targetID, kickPoint, dir );
			} else
			if (health>50) {
				World.SpawnFX("PlayerPain50", targetID, kickPoint, dir );
			} else
			if (health>25) {
				World.SpawnFX("PlayerPain75", targetID, kickPoint, dir );
			} else
			if (health>0) {
				World.SpawnFX("PlayerPain100", targetID, kickPoint, dir );
			} else
			if (health>-25) {
				World.SpawnFX("PlayerDeath", targetID, e.Position, dir );
			} else {
				World.SpawnFX("PlayerDeathMeat", targetID, e.Position, kickImpulse, dir );
			}

			if (health<=0) {
				World.Kill( targetID );
				return true;
			}

			e.SetItemCount( Inventory.Health, health );

			return false;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update ( float elapsedTime )
		{
			var c = controller;
			var e = Entity;

			UpdateWeaponState( Entity, (short)(elapsedTime*1000) );

			Move();

			e.Position			=	MathConverter.Convert( c.Body.Position ) - GetPovOffset(); 
			e.LinearVelocity	=	MathConverter.Convert( c.Body.LinearVelocity );
			e.AngularVelocity	=	MathConverter.Convert( c.Body.AngularVelocity );

			if (c.SupportFinder.HasTraction) {
				e.State |= EntityState.HasTraction;
			} else {
				e.State &= ~EntityState.HasTraction;
			}

			if (c.StanceManager.CurrentStance==Stance.Crouching) {
				e.State |= EntityState.Crouching;
			} else {
				e.State &= ~EntityState.Crouching;
			}


			UpdateWalkSFX( e, elapsedTime );
			UpdateFallSFX( e, elapsedTime );
		}



		void UpdateWalkSFX ( Entity e, float elapsedTime )
		{					
			stepCounter -= elapsedTime;
			if (stepCounter<=0) {
				stepCounter = StepRate;
				rlStep = !rlStep;

				bool step	=	e.UserCtrlFlags.HasFlag( UserCtrlFlags.Forward )
							|	e.UserCtrlFlags.HasFlag( UserCtrlFlags.Backward )
							|	e.UserCtrlFlags.HasFlag( UserCtrlFlags.StrafeLeft )
							|	e.UserCtrlFlags.HasFlag( UserCtrlFlags.StrafeRight );

				if (step && controller.SupportFinder.HasTraction) {
					if (rlStep) {
						World.SpawnFX("PlayerFootStepR", e.ID, e.Position );
					} else {
						World.SpawnFX("PlayerFootStepL", e.ID, e.Position );
					}
				}
			}
		}



		void UpdateFallSFX ( Entity e, float elapsedTime )
		{
			bool newTraction = controller.SupportFinder.HasTraction;
			
			if (oldTraction!=newTraction && newTraction) {
				//if (((ShooterServer)World.GameServer).ShowFallings) {
				//	Log.Verbose("{0} falls : {1}", e.ID, oldVelocity.Y );
				//}

				if (oldVelocity.Y<-10) {
					//	medium landing :
					World.SpawnFX( "PlayerLanding", e.ID, e.Position, oldVelocity, Quaternion.Identity );
				} else {
					//	light landing :
					World.SpawnFX( "PlayerFootStepL", e.ID, e.Position );
				}
			}

			oldTraction = newTraction;
			oldVelocity = MathConverter.Convert(controller.Body.LinearVelocity);
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		public override void Killed ()
		{
			space.Remove( controller );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="moveVector"></param>
		void Move ()
		{
			var ent	=	Entity;
			var m	=	Matrix.RotationQuaternion( ent.Rotation );

			var move = Vector3.Zero;
			var jump = false;
			var crouch = false;
			if (ent.UserCtrlFlags.HasFlag( UserCtrlFlags.Forward )) move += m.Forward;
			if (ent.UserCtrlFlags.HasFlag( UserCtrlFlags.Backward )) move += m.Backward;
			if (ent.UserCtrlFlags.HasFlag( UserCtrlFlags.StrafeLeft )) move += m.Left;
			if (ent.UserCtrlFlags.HasFlag( UserCtrlFlags.StrafeRight )) move += m.Right;
			if (ent.UserCtrlFlags.HasFlag( UserCtrlFlags.Jump )) jump = true;
			if (ent.UserCtrlFlags.HasFlag( UserCtrlFlags.Crouch )) crouch = true;

			if (controller==null) {
				return;
			}

			controller.HorizontalMotionConstraint.MovementDirection = new BEPUutilities.Vector2( move.X, -move.Z );
			controller.HorizontalMotionConstraint.TargetSpeed	=	8.0f;

			controller.StanceManager.DesiredStance	=	crouch ? Stance.Crouching : Stance.Standing;

			controller.TryToJump = jump;
			
			if (jump && controller.StanceManager.CurrentStance!=Stance.Crouching) {
				if (controller.SupportFinder.HasSupport || controller.SupportFinder.HasTraction) {
					World.SpawnFX( "PlayerJump", ent.ID, ent.Position );
				}
			}
			/*if (jump) {
				controller.Jump();
			} */
		}
	}
}
