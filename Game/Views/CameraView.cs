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
using ShooterDemo.Core;
using BEPUphysics;
using BEPUphysics.Character;
using Fusion.Engine.Audio;



namespace ShooterDemo.Views {
	public class CameraView : WorldView {

		uint playerID = 0;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="game"></param>
		/// <param name="space"></param>
		public CameraView ( World world ) : base( world )
		{
			if (world.IsClientSide) {
				currentFov	=	(world.GameClient as ShooterClient).Fov;
			}
		}


		float currentFov;
		Vector3 filteredPos = Vector3.Zero;

		/// <summary>
		/// 
		/// </summary>
		public float Sensitivity {
			get {
				var cl = ((ShooterClient)World.GameClient);
				return currentFov / cl.Fov * cl.Sensitivity;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update ( float elapsedTime, float lerpFactor )
		{
			var rw	= Game.RenderSystem.RenderWorld;
			var sw	= Game.SoundSystem.SoundWorld;
			var vp	= Game.RenderSystem.DisplayBounds;
			var cl	= ((ShooterClient)World.GameClient);

			var aspect	=	(vp.Width) / (float)vp.Height;
		
			//rw.Camera.SetupCameraFov( new Vector3(10,4,10), new Vector3(0,4,0), Vector3.Up, MathUtil.Rad(90), 0.125f, 1024f, 1, 0, aspect );

			var player	=	World.GetEntityOrNull( e => e.Is("player") && e.UserGuid == World.UserGuid );

			if (player==null) {

				StopShaking();

				playerID = 0;

				var camera	= World.GetEntityOrNull( e => e.Is("camera") );
				var cp		= camera.Position;
				var cm		= Matrix.RotationQuaternion( camera.Rotation );

				rw.Camera.SetupCameraFov( cp, cp + cm.Right, cm.Up, MathUtil.Rad(90), 0.125f, 1024f, 2, 0.05f, aspect );
				return;
			}

			playerID	=	player.ID;
			CalcBobbing( player, elapsedTime );

			var uc	=	(World.GameClient as ShooterClient).UserCommand;

			var m	= 	Matrix.RotationYawPitchRoll(	
							uc.Yaw	 + MathUtil.Rad( bobYaw.Offset), 
							uc.Pitch + MathUtil.Rad( bobPitch.Offset), 
							uc.Roll	 + MathUtil.Rad( bobRoll.Offset)
						);

			var ppos	=	player.LerpPosition(lerpFactor);

			float backoffset = ((ShooterClient)World.GameClient).ThirdPerson ? 2 : 0;
			var pos		=	ppos + Vector3.Up * 1.0f + m.Backward * backoffset;

			var fwd	=	pos + m.Forward;
			var up	=	m.Up;


			var targetFov	=	MathUtil.Clamp( uc.CtrlFlags.HasFlag( UserCtrlFlags.Zoom ) ? cl.ZoomFov : cl.Fov, 10, 140 );

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
