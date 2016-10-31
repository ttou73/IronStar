using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Mathematics;
using Fusion.Drivers.Graphics;
using Fusion.Engine.Input;
using Fusion.Engine.Graphics;
using Fusion.Core;
using Fusion.Core.Configuration;
using Fusion.Engine.Tools;
using Fusion;
using Fusion.Engine.Client;
using Fusion.Engine.Common;
using Fusion.Engine.Server;

namespace ShooterDemo {

	class ShooterInterface : Fusion.Engine.Common.UserInterface {

		DiscTexture	background;
		SpriteLayer uiLayer;
		SpriteFont	headerFont;
		SpriteFont	textFont;
		SpriteFont	titleFont;


		/// <summary>
		/// Creates instance of ShooterDemoUserInterface
		/// </summary>
		/// <param name="engine"></param>
		public ShooterInterface ( Game game )
			: base( game )
		{
			ShowMenu	=	true;
		}



		/// <summary>
		/// Called after the ShooterDemoUserInterface is created,
		/// </summary>
		public override void Initialize ()
		{
			uiLayer	=	new SpriteLayer(Game.RenderSystem, 1024);

			//	add console sprite layer to master view layer :
			Game.RenderSystem.SpriteLayers.Add( uiLayer );


			LoadContent();
			Game.Reloading += (s,e) => LoadContent();

			Game.GameClient.ClientStateChanged += GameClient_ClientStateChanged;
		}

		
		
		void GameClient_ClientStateChanged ( object sender, GameClient.ClientEventArgs e )
		{
			Game.Console.Hide();
		}



		void LoadContent ()
		{
			background	=	Game.Content.Load<DiscTexture>(@"ui\background");
			headerFont	=	Game.Content.Load<SpriteFont>(@"fonts\headerFont");
			titleFont	=	Game.Content.Load<SpriteFont>(@"fonts\titleFont");
			textFont	=	Game.Content.Load<SpriteFont>(@"fonts\textFont");
		}



		/// <summary>
		/// Overloaded. Immediately releases the unmanaged resources used by this object. 
		/// </summary>
		protected override void Dispose ( bool disposing )
		{
			if (disposing) {
				SafeDispose( ref uiLayer );
			}
			base.Dispose( disposing );
		}


		float dofFactor = 0;



		/// <summary>
		/// Called when the game has determined that UI logic needs to be processed.
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update ( GameTime gameTime )
		{
			//	update console :
			Game.Console.Update( gameTime );

			uiLayer.Clear();

			var clientState	=	Game.GameClient.ClientState;

			dofFactor	=	MathUtil.Lerp( (float)dofFactor, Game.Keyboard.IsKeyDown(Keys.Q) ? 1.0f : 0.0f, 0.1f );

			Game.RenderSystem.RenderWorld.DofSettings.PlaneInFocus	=	7;
			Game.RenderSystem.RenderWorld.DofSettings.FocalLength	=	0.1f;
			Game.RenderSystem.RenderWorld.DofSettings.Enabled		=	dofFactor > 0.01f;
			Game.RenderSystem.RenderWorld.DofSettings.Aperture		=	dofFactor * 20;



			switch (clientState) {
				case ClientState.StandBy		: DrawStandByScreen(); break;
				case ClientState.Connecting		: DrawLoadingScreen("Connecting..."); break;
				case ClientState.Loading		: DrawLoadingScreen("Loading..."); break;
				case ClientState.Awaiting		: DrawLoadingScreen("Awaiting snapshot..."); break;
				case ClientState.Disconnected	: DrawLoadingScreen("Disconnected."); break;
				case ClientState.Active			: break;
			}

			if (ShowMenu) {

				Game.Keyboard.ScanKeyboard	=	false;
				Game.Mouse.IsMouseCentered	=	false;
				Game.Mouse.IsMouseClipped	=	false;
				Game.Mouse.IsMouseHidden	=	false;

			} else {

				if (!Game.Console.IsShown) {
					Game.Keyboard.ScanKeyboard	=	true;
					Game.Mouse.IsMouseCentered	=	true;
					Game.Mouse.IsMouseClipped	=	true;
					Game.Mouse.IsMouseHidden	=	true;
				} else {
					Game.Keyboard.ScanKeyboard	=	false;
					Game.Mouse.IsMouseCentered	=	false;
					Game.Mouse.IsMouseClipped	=	false;
					Game.Mouse.IsMouseHidden	=	false;
				}
			}
		}



		/// <summary>
		/// Draw loading screen
		/// </summary>
		/// <param name="message"></param>
		void DrawLoadingScreen ( string message )
		{
			var vp = Game.RenderSystem.DisplayBounds;

			uiLayer.Draw( background, 0,0, vp.Width, vp.Height, Color.White );

			uiLayer.Draw( null, 0,vp.Height/4, vp.Width, vp.Height/2, new Color(0,0,0,192) );

			var h = textFont.LineHeight;

			//titleFont.DrawString( uiLayer, message, 100,vp.Height/2 - h*2, new Color(242,242,242) );
			textFont.DrawString( uiLayer, message, 100,vp.Height/2 - h, new Color(220,20,60) );
		}


		/// <summary>
		/// Draws stand-by screen
		/// </summary>
		void DrawStandByScreen ()
		{
			var vp = Game.RenderSystem.DisplayBounds;

			uiLayer.Draw( background, 0,0, vp.Width, vp.Height, Color.White );


			uiLayer.Draw( null, 0,vp.Height/4, vp.Width, vp.Height/2, new Color(0,0,0,192) );

			var h = textFont.LineHeight;
			//titleFont.DrawString( uiLayer, "SHOOTER DEMO", 100,vp.Height/2 - h*2, new Color(242,242,242) );
			titleFont.DrawString( uiLayer, "HEROES OF THE SHOOTER AGE", 100,vp.Height/2 - h*2, new Color(242,242,242) );
			textFont.DrawString( uiLayer, "Fusion Engine Test Project", 100,vp.Height/2 - h, new Color(220,20,60) );

			textFont.DrawString( uiLayer, "Press [~] to open console:", 100,vp.Height/2 + h, new Color(242,242,242) );
			textFont.DrawString( uiLayer, "   - Enter \"map base1\" to start the game.", 100,vp.Height/2 + h*2, new Color(242,242,242) );
			textFont.DrawString( uiLayer, "   - Enter \"killserver\" to stop the game.", 100,vp.Height/2 + h*3, new Color(242,242,242) );
			textFont.DrawString( uiLayer, "   - Enter \"connect <IP:port>\" to connect to the remote game.", 100,vp.Height/2 + h*4, new Color(242,242,242) );
		}




		public bool ShowMenu {
			get; set;
		}



		/// <summary>
		/// Called when user closes game window using Close button or Alt+F4.
		/// </summary>
		public override void RequestToExit ()
		{
			Game.Exit();
		}



		/// <summary>
		/// Called when discovery respone arrives.
		/// </summary>
		/// <param name="endPoint"></param>
		/// <param name="serverInfo"></param>
		public override void DiscoveryResponse ( System.Net.IPEndPoint endPoint, string serverInfo )
		{
			Log.Message( "DISCOVERY : {0} - {1}", endPoint.ToString(), serverInfo );
		}
	}
}
