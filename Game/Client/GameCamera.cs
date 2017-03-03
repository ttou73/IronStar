using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Fusion.Engine.Audio;
using IronStar.Entities;

namespace IronStar.Views {
	public class GameCamera {

		public readonly Game Game;
		public readonly GameWorld World;
		readonly ShooterClient client;
		uint playerID = 0;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="game"></param>
		/// <param name="space"></param>
		public GameCamera ( GameWorld world, ShooterClient client ) 
		{
			this.World	=	world;
			this.Game	=	world.Game;
			this.client	=	client;
			currentFov	=	90;//(world.GameClient as ShooterClient).Fov;
		}


		float currentFov;
		Vector3 filteredPos = Vector3.Zero;

		/// <summary>
		/// 
		/// </summary>
		public float Sensitivity {
			get {
				return 5;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update ( float elapsedTime, float lerpFactor )
		{
			var rw	= Game.RenderSystem.RenderWorld;
			var sw	= Game.SoundSystem.SoundWorld;
			var vp	= Game.RenderSystem.DisplayBounds;

			var aspect	=	(vp.Width) / (float)vp.Height;
		
			//rw.Camera.SetupCameraFov( new Vector3(10,4,10), new Vector3(0,4,0), Vector3.Up, MathUtil.Rad(90), 0.125f, 1024f, 1, 0, aspect );

			var player		=	World.GetEntityOrNull( "player", e => e.UserGuid == World.UserGuid );

			if (player==null) {

				StopShaking();

				playerID = 0;

				var camera	= World.GetEntityOrNull( "camera" );
				#warning use camera
				var cp		= Vector3.Up * 10;
				var cm		= Matrix.Identity;

				rw.Camera.SetupCameraFov( cp, cp + cm.Right, cm.Up, MathUtil.Rad(90), 0.125f, 1024f, 2, 0.05f, aspect );
				return;
			}

			playerID	=	player.ID;
			CalcBobbing( player, elapsedTime );

			var uc	=	client.UserCommand;

			var m	= 	Matrix.RotationYawPitchRoll(	
							uc.Yaw	 + MathUtil.Rad( bobYaw.Offset), 
							uc.Pitch + MathUtil.Rad( bobPitch.Offset), 
							uc.Roll	 + MathUtil.Rad( bobRoll.Offset)
						);

			var ppos	=	player.LerpPosition(lerpFactor);

			//float backoffset = false ? 2 : 0;
			var h		=	player.State.HasFlag( EntityState.Crouching ) ? 0.8f : 1.8f;
			var pos		=	player.Position + Vector3.Up * h; 

			var fwd	=	pos + m.Forward;
			var up	=	m.Up;


			var targetFov	=	MathUtil.Clamp( uc.CtrlFlags.HasFlag( UserCtrlFlags.Zoom ) ? 30 : 110, 10, 140 );

			currentFov		=	MathUtil.Drift( currentFov, targetFov, 360*elapsedTime, 360*elapsedTime );

			rw.Camera.SetupCameraFov( pos, fwd, up, MathUtil.Rad(currentFov), 0.125f, 1024f, 2, 0.05f, aspect );

			sw.Listener	=	new AudioListener();
			sw.Listener.Position	=	pos;
			sw.Listener.Forward		=	m.Forward;
			sw.Listener.Up			=	m.Up;
			sw.Listener.Velocity	=	Vector3.Zero;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="yawSpeed"></param>
		/// <param name="pitchSpeed"></param>
		/// <param name="rollSpeed"></param>
		public void Shake ( uint entityID, float yawSpeed, float pitchSpeed, float rollSpeed )
		{
			if (entityID==playerID) {
				bobYaw	.Kick( yawSpeed );
				bobPitch.Kick( pitchSpeed );
				bobRoll	.Kick( rollSpeed );
			}
		}



		/// <summary>
		/// Stops camera shakes
		/// </summary>
		public void StopShaking ()
		{
			bobYaw	.Stop();
			bobPitch.Stop();
			bobRoll	.Stop();
		}


		Oscillator bobPitch = new Oscillator(75,15);
		Oscillator bobRoll	= new Oscillator(75,15);
		Oscillator bobYaw	= new Oscillator(75,15);

		const float BobStrafe = 1;

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		void CalcBobbing ( Entity player, float elapsedTime )
		{	
			bool hasTraction	=	player.State.HasFlag(EntityState.HasTraction);	

			var rollPull = 0.0f;

			if (hasTraction) {
				if (player.UserCtrlFlags.HasFlag(UserCtrlFlags.StrafeRight)) {
					rollPull	-=	BobStrafe;
				} 
				if (player.UserCtrlFlags.HasFlag(UserCtrlFlags.StrafeLeft)) {
					rollPull	+=	BobStrafe;
				} 
			}

			bobRoll.Target = rollPull;


			bobRoll.Update( elapsedTime );
			bobPitch.Update( elapsedTime );
			bobYaw.Update( elapsedTime );
		}
	}
}
