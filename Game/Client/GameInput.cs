using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion;
using Fusion.Core;
using Fusion.Core.Mathematics;
using Fusion.Core.Extensions;
using Fusion.Core.Configuration;
using Fusion.Engine.Common;
using Fusion.Engine.Input;
using Fusion.Engine.Client;
using Fusion.Engine.Server;
using Fusion.Engine.Graphics;
using IronStar.Core;
using IronStar.Views;


namespace IronStar.Client {
	public partial class GameInput : GameComponent {

		[Config] public float Sensitivity { get; set; }
		[Config] public bool InvertMouse { get; set; }
		[Config] public float PullFactor { get; set; }
		[Config] public bool ThirdPerson { get; set; }
		
		[Config] public float ZoomFov { get; set; }
		[Config] public float Fov { get; set; }
		[Config] public float BobHeave	{ get; set; }
		[Config] public float BobPitch	{ get; set; }
		[Config] public float BobRoll	{ get; set; }
		[Config] public float BobStrafe  { get; set; }
		[Config] public float BobJump	{ get; set; }
		[Config] public float BobLand	{ get; set; }

		[Config] public Keys	MoveForward		{ get; set; }
		[Config] public Keys	MoveBackward	{ get; set; }
		[Config] public Keys	StrafeRight		{ get; set; }
		[Config] public Keys	StrafeLeft		{ get; set; }
		[Config] public Keys	Jump			{ get; set; }
		[Config] public Keys Crouch			{ get; set; }
		[Config] public Keys Walk			{ get; set; }

		[Config] public Keys Attack			{ get; set; }
		[Config] public Keys Zoom			{ get; set; }
		[Config] public Keys ThrowGrenade	{ get; set; }

		[Config] public Keys UseWeapon1		{ get; set; }
		[Config] public Keys UseWeapon2		{ get; set; }
		[Config] public Keys UseWeapon3		{ get; set; }
		[Config] public Keys UseWeapon4		{ get; set; }
		[Config] public Keys UseWeapon5		{ get; set; }
		[Config] public Keys UseWeapon6		{ get; set; }
		[Config] public Keys UseWeapon7		{ get; set; }
		[Config] public Keys UseWeapon8		{ get; set; }
		[Config] public Keys UseWeapon9		{ get; set; }


		/// <summary>
		/// 
		/// </summary>
		/// <param name="cl"></param>
		public GameInput (Game game) : base(game)
		{	
			Sensitivity	=	5;
			InvertMouse	=	true;
			PullFactor	=	1;

			Fov			=	90.0f;
			ZoomFov		=	30.0f;

			BobHeave	=	0.05f;
			BobPitch	=	1.0f;
			BobRoll		=	2.0f;
			BobStrafe  	=	5.0f;
			BobJump		=	5.0f;
			BobLand		=	5.0f;


			MoveForward		=	Keys.S;
			MoveBackward	=	Keys.Z;
			StrafeRight		=	Keys.X;
			StrafeLeft		=	Keys.A;
			Jump			=	Keys.RightButton;
			Crouch			=	Keys.LeftAlt;
			Walk			=	Keys.LeftShift;
							
			Attack			=	Keys.LeftButton;
			Zoom			=	Keys.D;
			ThrowGrenade	=	Keys.G;
								
			UseWeapon1		=	Keys.D1;
			UseWeapon2		=	Keys.D2;
			UseWeapon3		=	Keys.D3;
			UseWeapon4		=	Keys.D4;
			UseWeapon5		=	Keys.D5;
			UseWeapon6		=	Keys.D6;
			UseWeapon7		=	Keys.D7;
			UseWeapon8		=	Keys.D8;
			UseWeapon9		=	Keys.D9;
		}



		/// <summary>
		/// 
		/// </summary>
		public override void Initialize()
		{
			Game.Keyboard.KeyDown += Keyboard_KeyDown;	
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose( bool disposing )
		{
			Game.Keyboard.KeyDown -= Keyboard_KeyDown;	
		}



		UserCtrlFlags weaponControl;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Keyboard_KeyDown ( object sender, KeyEventArgs e )
		{
			if (e.Key==UseWeapon1) {	
				weaponControl &= ~UserCtrlFlags.AllWeapon;
				weaponControl |= UserCtrlFlags.Machinegun;
			}
			if (e.Key==UseWeapon2) {	
				weaponControl &= ~UserCtrlFlags.AllWeapon;
				weaponControl |= UserCtrlFlags.Shotgun;
			}
			if (e.Key==UseWeapon3) {	
				weaponControl &= ~UserCtrlFlags.AllWeapon;
				weaponControl |= UserCtrlFlags.SuperShotgun;
			}
			if (e.Key==UseWeapon4) {	
				weaponControl &= ~UserCtrlFlags.AllWeapon;
				weaponControl |= UserCtrlFlags.GrenadeLauncher;
			}
			if (e.Key==UseWeapon5) {	
				weaponControl &= ~UserCtrlFlags.AllWeapon;
				weaponControl |= UserCtrlFlags.RocketLauncher;
			}
			if (e.Key==UseWeapon6) {	
				weaponControl &= ~UserCtrlFlags.AllWeapon;
				weaponControl |= UserCtrlFlags.Chaingun;
			}
			if (e.Key==UseWeapon7) {	
				weaponControl &= ~UserCtrlFlags.AllWeapon;
				weaponControl |= UserCtrlFlags.Railgun;
			}
			if (e.Key==UseWeapon8) {	
				weaponControl &= ~UserCtrlFlags.AllWeapon;
				weaponControl |= UserCtrlFlags.HyperBlaster;
			}
			if (e.Key==UseWeapon9) {	
				weaponControl &= ~UserCtrlFlags.AllWeapon;
				weaponControl |= UserCtrlFlags.BFG;
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="userCommand"></param>
		public void Update ( GameTime gameTime, ref UserCommand userCommand )
		{
			var flags = UserCtrlFlags.None;
			
			if (Game.Keyboard.IsKeyDown( MoveForward		)) flags |= UserCtrlFlags.Forward;
			if (Game.Keyboard.IsKeyDown( MoveBackward	)) flags |= UserCtrlFlags.Backward;
			if (Game.Keyboard.IsKeyDown( StrafeLeft		)) flags |= UserCtrlFlags.StrafeLeft;
			if (Game.Keyboard.IsKeyDown( StrafeRight		)) flags |= UserCtrlFlags.StrafeRight;
			if (Game.Keyboard.IsKeyDown( Jump			)) flags |= UserCtrlFlags.Jump;
			if (Game.Keyboard.IsKeyDown( Crouch			)) flags |= UserCtrlFlags.Crouch;
			if (Game.Keyboard.IsKeyDown( Zoom			)) flags |= UserCtrlFlags.Zoom;
			if (Game.Keyboard.IsKeyDown( Attack			)) flags |= UserCtrlFlags.Attack;


			//	http://eliteownage.com/mousesensitivity.html 
			//	Q3A: 16200 dot per 360 turn:
			var vp		=	Game.RenderSystem.DisplayBounds;
			var ui		=	Game.UserInterface as ShooterInterface;
			//var cam		=	World.GetView<CameraView>();

			if (!Game.Console.IsShown) {
				userCommand.CtrlFlags	=	flags | weaponControl;
				userCommand.Yaw         -=  2 * MathUtil.Pi * 5 * Game.Mouse.PositionDelta.X / 16200.0f;
				userCommand.Pitch       -=  2 * MathUtil.Pi * 5 * Game.Mouse.PositionDelta.Y / 16200.0f * ( InvertMouse ? -1 : 1 );
				//UserCommand.Yaw         -=  2 * MathUtil.Pi * cam.Sensitivity * Game.Mouse.PositionDelta.X / 16200.0f;
				//UserCommand.Pitch       -=  2 * MathUtil.Pi * cam.Sensitivity * Game.Mouse.PositionDelta.Y / 16200.0f * ( InvertMouse ? -1 : 1 );
				userCommand.Roll		=	0;
			}
		}

		
	}
}
