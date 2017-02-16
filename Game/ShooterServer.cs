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
using IronStar.Core;

namespace IronStar {
	class ShooterServer : IServerInstance {

		readonly GameWorld world;
		readonly string mapName;
		Map map;

		
		public ShooterServer ( GameServer server, string mapName )
		{
			world			=	new GameWorld( server.Game, false, new Guid() );
			this.mapName	=	mapName;
		}


		void IServerInstance.Initialize()
		{
			map		=   world.Content.Load<Map>( @"maps\" + mapName );
			world.InitServerAtoms();
			map.ActivateMap( world, true );

			//EntityKilled += MPWorld_EntityKilled;
		}



		public AtomCollection Atoms {
			get {
				return world.Atoms;
			}
		}


		public string ServerInfo()
		{
			return mapName;
		}


		public byte[] Update( GameTime gameTime )
		{
			world.SimulateWorld( gameTime.ElapsedSec );

			//	write world to stream :
			using ( var ms = new MemoryStream() ) {
				world.WriteToSnapshot( ms );
				return ms.GetBuffer();
			}
		}


		public void FeedCommand( Guid clientGuid, byte[] userCommand, uint commandID, float lag )
		{
			if ( !userCommand.Any() ) {
				return;
			}

			world.PlayerCommand( clientGuid, userCommand, lag );
		}


		public void FeedNotification( Guid clientGuid, string message )
		{
			Log.Message( "NOTIFICATION {0}: {1}", clientGuid, message );
		}


		public void ClientConnected( Guid clientGuid, string userInfo )
		{
			Log.Message("Client Connected: {0} {1}", clientGuid, userInfo );
			world.PlayerConnected( clientGuid, userInfo );
		}


		public void ClientActivated( Guid clientGuid )
		{
			Log.Message("Client Activated: {0}", clientGuid );
			world.PlayerEntered( clientGuid );
		}


		public void ClientDeactivated( Guid clientGuid )
		{
			Log.Message("Client Deactivated: {0}", clientGuid );
			world.PlayerLeft( clientGuid );
		}


		public void ClientDisconnected( Guid clientGuid )
		{
			Log.Message("Client Disconnected: {0}", clientGuid );
			world.PlayerDisconnected( clientGuid );
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
					world?.Dispose();
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
