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

		readonly ShooterClient cl;



		/// <summary>
		/// 
		/// </summary>
		/// <param name="cl"></param>
		public GameInput ( ShooterClient cl ) : base(cl.Game)
		{	
			this.cl	=	cl;
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
			if (e.Key==cl.UseWeapon1) {	
				weaponControl &= ~UserCtrlFlags.AllWeapon;
				weaponControl |= UserCtrlFlags.Machinegun;
			}
			if (e.Key==cl.UseWeapon2) {	
				weaponControl &= ~UserCtrlFlags.AllWeapon;
				weaponControl |= UserCtrlFlags.Shotgun;
			}
			if (e.Key==cl.UseWeapon3) {	
				weaponControl &= ~UserCtrlFlags.AllWeapon;
				weaponControl |= UserCtrlFlags.SuperShotgun;
			}
			if (e.Key==cl.UseWeapon4) {	
				weaponControl &= ~UserCtrlFlags.AllWeapon;
				weaponControl |= UserCtrlFlags.GrenadeLauncher;
			}
			if (e.Key==cl.UseWeapon5) {	
				weaponControl &= ~UserCtrlFlags.AllWeapon;
				weaponControl |= UserCtrlFlags.RocketLauncher;
			}
			if (e.Key==cl.UseWeapon6) {	
				weaponControl &= ~UserCtrlFlags.AllWeapon;
				weaponControl |= UserCtrlFlags.Chaingun;
			}
			if (e.Key==cl.UseWeapon7) {	
				weaponControl &= ~UserCtrlFlags.AllWeapon;
				weaponControl |= UserCtrlFlags.Railgun;
			}
			if (e.Key==cl.UseWeapon8) {	
				weaponControl &= ~UserCtrlFlags.AllWeapon;
				weaponControl |= UserCtrlFlags.HyperBlaster;
			}
			if (e.Key==cl.UseWeapon9) {	
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
			
			if (Game.Keyboard.IsKeyDown( cl.MoveForward		)) flags |= UserCtrlFlags.Forward;
			if (Game.Keyboard.IsKeyDown( cl.MoveBackward	)) flags |= UserCtrlFlags.Backward;
			if (Game.Keyboard.IsKeyDown( cl.StrafeLeft		)) flags |= UserCtrlFlags.StrafeLeft;
			if (Game.Keyboard.IsKeyDown( cl.StrafeRight		)) flags |= UserCtrlFlags.StrafeRight;
			if (Game.Keyboard.IsKeyDown( cl.Jump			)) flags |= UserCtrlFlags.Jump;
			if (Game.Keyboard.IsKeyDown( cl.Crouch			)) flags |= UserCtrlFlags.Crouch;
			if (Game.Keyboard.IsKeyDown( cl.Zoom			)) flags |= UserCtrlFlags.Zoom;
			if (Game.Keyboard.IsKeyDown( cl.Attack			)) flags |= UserCtrlFlags.Attack;


			//	http://eliteownage.com/mousesensitivity.html 
			//	Q3A: 16200 dot per 360 turn:
			var vp		=	Game.RenderSystem.DisplayBounds;
			var ui		=	Game.UserInterface as ShooterInterface;
			//var cam		=	World.GetView<CameraView>();

			if (!Game.Console.IsShown) {
				userCommand.CtrlFlags	=	flags | weaponControl;
				userCommand.Yaw         -=  2 * MathUtil.Pi * 5 * Game.Mouse.PositionDelta.X / 16200.0f;
				userCommand.Pitch       -=  2 * MathUtil.Pi * 5 * Game.Mouse.PositionDelta.Y / 16200.0f * ( cl.InvertMouse ? -1 : 1 );
				//UserCommand.Yaw         -=  2 * MathUtil.Pi * cam.Sensitivity * Game.Mouse.PositionDelta.X / 16200.0f;
				//UserCommand.Pitch       -=  2 * MathUtil.Pi * cam.Sensitivity * Game.Mouse.PositionDelta.Y / 16200.0f * ( InvertMouse ? -1 : 1 );
				userCommand.Roll		=	0;
			}
		}

		
	}
}
