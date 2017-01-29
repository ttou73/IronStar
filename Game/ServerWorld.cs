using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion.Core.Content;
using Fusion.Engine.Server;
using Fusion.Engine.Client;
using Fusion.Core.Extensions;
using IronStar.SFX;
using Fusion.Core.IniParser.Model;
using Fusion.Engine.Graphics;
using IronStar.Mapping;


namespace IronStar {
	class ServerWorld : IServerInstance {

		/// <summary>
		/// Initializes server-side world.
		/// </summary>
		/// <param name="maxPlayers"></param>
		/// <param name="maxEntities"></param>
		public ServerWorld ( GameServer server, string map )
		{
			this.mapName	=	map;
			
			Atoms	=	new AtomCollection();

			Log.Verbose( "world: server" );
			this.serverSide =   true;
			this.Game       =   server.Game;
			this.UserGuid   =   new Guid();
			Content         =   new ContentManager( Game );
			entities        =   new EntityCollection(Atoms);

			AddAtoms();
		}



		/// <summary>
		/// 
		/// </summary>
		void AddAtoms ()
		{
			var atoms = new List<string>();

			atoms.AddRange( Content.EnumerateAssets( "fx" ) );
			atoms.AddRange( Content.EnumerateAssets( "entities" ) );
			atoms.AddRange( Content.EnumerateAssets( "models" ) );

			Atoms.AddRange( atoms );
		}


		void IServerInstance.Initialize()
		{
			this.map		=   Content.Load<Map>( @"maps\" + mapName );
			this.map.ActivateMap( this );


			#region TEMP STUFF
			Random	r = new Random();
			for (int i=0; i<10; i++) {
				Spawn("box", 0, Vector3.Up * 400 + r.GaussRadialDistribution(20,2), 0 );
			}// */
			#endregion


			EntityKilled += MPWorld_EntityKilled;
		}



		public byte[] Update( GameTime gameTime )
		{
			SimulateWorld( gameTime.ElapsedSec );

			//	write world to stream :
			using ( var ms = new MemoryStream() ) {
				WriteToSnapshot( ms );
				return ms.GetBuffer();
			}
		}

		public void FeedCommand( Guid clientGuid, byte[] userCommand, uint commandID, float lag )
		{
			if ( !userCommand.Any() ) {
				return;
			}

			PlayerCommand( clientGuid, userCommand, lag );
		}

		public void FeedNotification( Guid clientGuid, string message )
		{
			Log.Message( "NOTIFICATION {0}: {1}", clientGuid, message );
		}

		public void ClientConnected( Guid clientGuid, string userInfo )
		{
			Log.Message("Client Connected: {0} {1}", clientGuid, userInfo );
			PlayerConnected( clientGuid, userInfo );
		}

		public void ClientActivated( Guid clientGuid )
		{
			Log.Message("Client Activated: {0}", clientGuid );
			PlayerEntered( clientGuid );
		}

		public void ClientDeactivated( Guid clientGuid )
		{
			Log.Message("Client Deactivated: {0}", clientGuid );
			PlayerLeft( clientGuid );
		}

		public void ClientDisconnected( Guid clientGuid )
		{
			Log.Message("Client Disconnected: {0}", clientGuid );
			PlayerDisconnected( clientGuid );
		}

		public bool ApproveClient( Guid clientGuid, string userInfo, out string reason )
		{
			reason = "";
			return true;
			throw new NotImplementedException();
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose( bool disposing )
		{
			if ( !disposedValue ) {
				if ( disposing ) {
					Shutdown();
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose( true );
		}
		#endregion
	}
}
