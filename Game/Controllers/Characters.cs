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
using BEPUphysics.Character;


namespace ShooterDemo.Controllers {
	public class Characters : EntityController {

		readonly Space space;

		const float StepRate = 0.3f;


		CharacterController controller;
		float		stepCounter = 0;
		bool		rlStep		= false;
		bool		oldTraction = true;
		Vector3		oldVelocity = Vector3.Zero;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="game"></param>
		/// <param name="space"></param>
		public Characters ( Entity entity, World world,
 			float height = 1.7f, 
			float crouchingHeight = 1.19f, 
			float radius = 0.6f, 
			float margin = 0.1f, 
			float mass = 10f, 
			float maximumTractionSlope = 0.8f, 
			float maximumSupportSlope = 1.3f, 
			float standingSpeed = 8f, 
			float crouchingSpeed = 3f, 
			float tractionForce = 1000f, 
			float slidingSpeed = 6f, 
			float slidingForce = 50f, 
			float airSpeed = 1f, 
			float airForce = 250f, 
			float jumpSpeed = 6f, 
			float slidingJumpSpeed = 3f, 
			float maximumGlueForce = 5000f ) : base(entity,world)
		{
			this.space	=	((MPWorld)world).PhysSpace;

			var pos = MathConverter.Convert( entity.Position );

			controller = new CharacterController( pos, 
					height					, 
					crouchingHeight			, 
					radius					, 
					margin					, 
					mass					,
					maximumTractionSlope	, 
					maximumSupportSlope		, 
					standingSpeed			,
					crouchingSpeed			,
					tractionForce			, 
					slidingSpeed			,
					slidingForce			,
					airSpeed				,
					airForce				, 
					jumpSpeed				, 
					slidingJumpSpeed		,
					maximumGlueForce		);


			controller.StepManager.MaximumStepHeight	=	0.5f;
			controller.Body.Tag	=	entity;
			controller.Tag		=	entity;

			space.Add( controller );
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



			Move();

			e.Position			=	MathConverter.Convert( c.Body.Position ); 
			e.LinearVelocity	=	MathConverter.Convert( c.Body.LinearVelocity );
			e.AngularVelocity	=	MathConverter.Convert( c.Body.AngularVelocity );

			if (c.SupportFinder.HasTraction) {
				e.State |= EntityState.HasTraction;
			} else {
				e.State &= ~EntityState.HasTraction;
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
				if (((ShooterServer)World.GameServer).ShowFallings) {
					Log.Verbose("{0} falls : {1}", e.ID, oldVelocity.Y );
				}

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
			if (ent.UserCtrlFlags.HasFlag( UserCtrlFlags.Forward )) move += m.Forward;
			if (ent.UserCtrlFlags.HasFlag( UserCtrlFlags.Backward )) move += m.Backward;
			if (ent.UserCtrlFlags.HasFlag( UserCtrlFlags.StrafeLeft )) move += m.Left;
			if (ent.UserCtrlFlags.HasFlag( UserCtrlFlags.StrafeRight )) move += m.Right;
			if (ent.UserCtrlFlags.HasFlag( UserCtrlFlags.Jump )) jump = true;

			if (controller==null) {
				return;
			}

			controller.HorizontalMotionConstraint.MovementDirection = new BEPUutilities.Vector2( move.X, -move.Z );
			controller.HorizontalMotionConstraint.TargetSpeed	=	8.0f;

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
