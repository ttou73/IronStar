using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fusion;
using Fusion.Engine.Client;
using Fusion.Engine.Common;
using Fusion.Engine.Server;
using Fusion.Core.Content;
using Fusion.Core.Configuration;
using Fusion.Engine.Graphics;
using ShooterDemo.Core;

namespace ShooterDemo {
	public partial class ShooterServer : GameServer {

		
		World	gameWorld;

		public World World {
			get { return gameWorld; }
		}


		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="engine"></param>
		public ShooterServer ( Game game )
			: base( game )
		{
			SetDefaults();
		}



		/// <summary>
		/// 
		/// </summary>
		public override void Initialize ()
		{
		}



		/// <summary>
		/// Releases all resources used by the GameServer class.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose ( bool disposing )
		{
			if (disposing) {
				//	...
			}
			base.Dispose( disposing );
		}



		/// <summary>
		/// Method is invoked when server started.
		/// </summary>
		/// <param name="map"></param>
		public override void LoadContent ( string map )
		{
			SFX.SfxInstance.EnumerateSFX( (t) => Atoms.Add( t.Name ) );			

			gameWorld	=	new MPWorld( this, map );
			Thread.Sleep(100);
			for (int i=0; i<100; i++) {
				gameWorld.SimulateWorld(0.16f);
			}
		}



		/// <summary>
		/// Method is invoked when server shuts down.
		/// This method will be also called when server crashes.
		/// </summary>
		public override void UnloadContent ()
		{
			if (gameWorld!=null) {
				gameWorld.Cleanup();
			}
			Content.Unload();
		}


		/// <summary>
		/// Runs one step of server-side world simulation.
		/// </summary>
		/// <param name="gameTime"></param>
		/// <returns>Snapshot bytes</returns>
		public override byte[] Update ( GameTime gameTime )
		{

			//	update world
			gameWorld.SimulateWorld( gameTime.ElapsedSec );

			//	write world to stream :
			using ( var ms = new MemoryStream() ) { 

				using ( var writer = new BinaryWriter(ms) ) {

					writer.Write( gameTime.ElapsedSec );

					gameWorld.WriteToSnapshot( writer );

					return ms.GetBuffer();
				}
			}

		}



		/// <summary>
		/// Feed client commands from particular client.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="clientId"></param>
		public override void FeedCommand ( Guid id, byte[] userCommand, uint commandID, float lag )
		{
			if (!userCommand.Any()) {
				return;
			}

			//Log.Message("Lag : {0} ms / Cmd : {1}", lag * 1000, commandID);

			gameWorld.PlayerCommand( id, userCommand, lag );
		}



		/// <summary>
		/// Feed server notification from particular client.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="message"></param>
		public override void FeedNotification ( Guid id, string message )
		{
			Log.Message( "NOTIFICATION {0}: {1}", id, message );
		}



		/// <summary>
		/// Gets server information that required for client to load the game.
		/// This information usually contains map name and game type.
		/// This information is also used for discovery response.
		/// </summary>
		/// <returns></returns>
		public override string ServerInfo ()
		{
			return gameWorld.ServerInfo();
		}



		/// <summary>
		/// Notifies server that client connected.
		/// </summary>
		public override void ClientConnected ( Guid guid, string userInfo )
		{
			gameWorld.PlayerConnected( guid, userInfo );
		}


		public override void ClientActivated ( Guid guid )
		{
			gameWorld.PlayerEntered( guid );
		}


		public override void ClientDeactivated ( Guid guid )
		{
			gameWorld.PlayerLeft( guid );
		}


		/// <summary>
		/// Notifies server that client disconnected.
		/// </summary>
		public override void ClientDisconnected ( Guid guid )
		{
			gameWorld.PlayerDisconnected( guid );
		}



		/// <summary>
		/// Approves client by id and user info.
		/// </summary>
		public override bool ApproveClient ( Guid id, string userInfo, out string reason )
		{
			reason = "Access denied.";
			return gameWorld.ApprovePlayer( id, userInfo );
		}
	}
}
