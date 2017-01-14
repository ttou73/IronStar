using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.Threading;
using System.Net;
using Fusion.Core.Shell;
using Fusion.Engine.Common;
using Fusion.Engine.Common.Commands;
using System.Diagnostics;
using Fusion.Core.Content;


namespace Fusion.Engine.Server {
	

	class GameServerContext : IDisposable {
		
		readonly Game game;
		readonly Queue<string> notifications = null;

		IServerInstance	serverInstance;
		NetServer		netServer;
		SnapshotQueue	snapshotQueue;

		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="server"></param>
		/// <param name="serverInstance"></param>
		public GameServerContext ( Game game, string gameId, int port, IServerInstance serverInstance )
		{
			this.game			=	game;
			this.serverInstance	=	serverInstance;

			var netConfig						=   new NetPeerConfiguration(gameId);
			netConfig.Port						=   port;
			netConfig.MaximumConnections		=   32;
			netConfig.UnreliableSizeBehaviour	=	NetUnreliableSizeBehaviour.NormalFragmentation;

			if ( Debugger.IsAttached ) {
				netConfig.ConnectionTimeout     =   float.MaxValue;
				Log.Message( "SV: Debugger is attached: ConnectionTimeout = {0} sec", netConfig.ConnectionTimeout );
			}

			netConfig.EnableMessageType( NetIncomingMessageType.ConnectionApproval );
			netConfig.EnableMessageType( NetIncomingMessageType.DiscoveryRequest );
			netConfig.EnableMessageType( NetIncomingMessageType.DiscoveryResponse );
			netConfig.EnableMessageType( NetIncomingMessageType.ConnectionLatencyUpdated );

			notifications	=	new Queue<string>();
			snapshotQueue   =   new SnapshotQueue(32);

			netServer		=	new NetServer( netConfig );
			netServer.Start();
		}



		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose( bool disposing )
		{
			if ( !disposedValue ) {
				if ( disposing ) {
					netServer.Shutdown("Server shut down");
					serverInstance.Dispose();
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose( true );
		}
		#endregion



		/// <summary>
		/// Updates everything related to network and game logic.
		/// </summary>
		public void UpdateNetworkAndLogic ( GameTime svTime )
		{
			#if DEBUG
			netServer.Configuration.SimulatedLoss				=	Game.Network.SimulatePacketsLoss;
			netServer.Configuration.SimulatedMinimumLatency	=	Game.Network.SimulateMinLatency;
			netServer.Configuration.SimulatedRandomLatency		=	Game.Network.SimulateRandomLatency;
			#endif

			//	read input messages :
			DispatchIM( svTime, snapshotQueue, netServer );

			//	update pings :
			UpdatePings( netServer );

			//	update frame and get snapshot :
			var snapshot = serverInstance.Update( svTime );

			//	push snapshot to queue :
			snapshotQueue.Push( svTime.Total, snapshot );

			//	send snapshot to clients :
			SendSnapshot( netServer, snapshotQueue, svTime.Total.Ticks );

			//	send notifications to clients :
			SendNotifications( netServer );

			//	execute server's command queue :
			game.Invoker.ExecuteQueue( svTime, CommandAffinity.Server );

			//	crash test for server :
			CrashServer.CrashTest();
			FreezeServer.FreezeTest();
			SlowdownServer.SlowTest();
		}


		/*-----------------------------------------------------------------------------------------
		 * 
		 *	Client-server stuff :
		 * 
		-----------------------------------------------------------------------------------------*/

		Dictionary<Guid,float> pings = null;

		void UpdatePings ( NetServer server )
		{
			pings	=	server.Connections.ToDictionary( conn => conn.GetHailGuid(), conn => conn.AverageRoundtripTime );
		}
		

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		void DispatchIM ( GameTime gameTime, SnapshotQueue queue, NetServer server )
		{
			NetIncomingMessage msg;
			while ((msg = server.ReadMessage()) != null)
			{
				switch (msg.MessageType)
				{
					case NetIncomingMessageType.VerboseDebugMessage:Log.Debug	("SV Net: " + msg.ReadString()); break;
					case NetIncomingMessageType.DebugMessage:		Log.Verbose	("SV Net: " + msg.ReadString()); break;
					case NetIncomingMessageType.WarningMessage:		Log.Warning	("SV Net: " + msg.ReadString()); break;
					case NetIncomingMessageType.ErrorMessage:		Log.Error	("SV Net: " + msg.ReadString()); break;

					case NetIncomingMessageType.ConnectionLatencyUpdated:
						if (game.Network.ShowLatency) {
							float latency = msg.ReadFloat();
							Log.Verbose("...SV ping - {0} {1,6:0.00} ms", msg.SenderEndPoint, (latency*1000) );
						}

						break;

					case NetIncomingMessageType.DiscoveryRequest:
						Log.Message("Discovery request from {0}", msg.SenderEndPoint.ToString() );
						var response = server.CreateMessage( serverInstance.ServerInfo() );
						server.SendDiscoveryResponse( response, msg.SenderEndPoint );

						break;

					case NetIncomingMessageType.ConnectionApproval:
						
						var userGuid	=	msg.SenderConnection.GetHailGuid();
						var userInfo	=	msg.SenderConnection.GetHailUserInfo();
						var reason		=	"";
						var approve		=	serverInstance.ApproveClient( userGuid, userInfo, out reason );

						if (approve) {	
							msg.SenderConnection.Approve( server.CreateMessage( serverInstance.ServerInfo() ) );
						} else {
							msg.SenderConnection.Deny( reason );
						}

						break;

					case NetIncomingMessageType.StatusChanged:		
						DispatchStatusChange( msg );
						break;
					
					case NetIncomingMessageType.Data:
						DispatchDataIM( gameTime, queue, msg );
						break;
					
					default:
						Log.Warning("SV: Unhandled type: " + msg.MessageType);
						break;
				}
				server.Recycle(msg);
			}		
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		void DispatchStatusChange ( NetIncomingMessage msg )
		{
			var connStatus	=	(NetConnectionStatus)msg.ReadByte();
			var senderEP	=	msg.SenderEndPoint;
			var text		=	msg.ReadString();

			Log.Message	("SV: {0}: {1}: {2}", connStatus, senderEP.ToString(), text);
			
			switch (connStatus) {
				case NetConnectionStatus.Connected :
					msg.SenderConnection.InitClientState(); 
					serverInstance.ClientConnected( msg.SenderConnection.GetHailGuid(), msg.SenderConnection.GetHailUserInfo() );
					break;

				case NetConnectionStatus.Disconnected :
					serverInstance.ClientDeactivated( msg.SenderConnection.GetHailGuid() );
					serverInstance.ClientDisconnected( msg.SenderConnection.GetHailGuid() );
					break;

				default:
					break;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="server"></param>
		void SendSnapshot ( NetServer server, SnapshotQueue queue, long serverTicks )
		{
			//	snapshot request is stored in connection's tag.s
			var debug	=	game.Network.ShowSnapshots;
			var conns	=	server.Connections.Where ( c => c.IsSnapshotRequested() );

			var sw		=	new Stopwatch();

			foreach ( var conn in conns ) {

				sw.Reset();
				sw.Start();
					
				var frame		=	queue.LastFrame;
				var prevFrame	=	conn.GetRequestedSnapshotID();
				int size		=	0;
				var commandID	=	conn.GetLastCommandID();
				var snapshot	=	queue.Compress( ref prevFrame, out size);

				//	reset snapshot request :
				conn.ResetRequestSnapshot();

				var msg = server.CreateMessage( snapshot.Length + 4 * 4 + 8 + 1 );
			
				msg.Write( (byte)NetCommand.Snapshot );
				msg.Write( frame );
				msg.Write( prevFrame );
				msg.Write( commandID );
				msg.Write( serverTicks );
				msg.Write( snapshot.Length );
				msg.Write( snapshot ); 

				//	append atom table to first snapshot :
				if (commandID==0) {
					serverInstance.Atoms.Write( msg );
				}

				//	Zero snapshot frame index means that client is waiting for first snapshot.
				//	and snapshot should reach the client.
				var delivery	=	prevFrame == 0 ? NetDeliveryMethod.ReliableOrdered : NetDeliveryMethod.UnreliableSequenced;

				if (prevFrame==0) {
					Log.Message("SV: Sending initial snapshot to {0}", conn.GetHailGuid().ToString() );
				}

				sw.Stop();

				server.SendMessage( msg, conn, delivery, 0 );

				if (debug) {
					Log.Message("Snapshot: #{0} - #{1} : {2} / {3} to {4} at {5} msec", 
						frame, prevFrame, snapshot.Length, size, conn.RemoteEndPoint.ToString(), sw.Elapsed.TotalMilliseconds );
				}
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="server"></param>
		void SendNotifications ( NetServer server )
		{
			List<string> messages;
			lock (notifications) {
				messages = notifications.ToList();
				notifications.Clear();
			}

			var conns = server.Connections;

			if (!conns.Any()) {
				return;
			}

			foreach ( var message in messages ) {
				var msg = server.CreateMessage( message.Length + 1 );
				msg.Write( (byte)NetCommand.Notification );
				msg.Write( message );
				server.SendMessage( msg, conns, NetDeliveryMethod.ReliableSequenced, 0 );
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		void DispatchDataIM ( GameTime gameTime, SnapshotQueue queue, NetIncomingMessage msg )
		{
			var netCmd = (NetCommand)msg.ReadByte();

			switch (netCmd) {
				case NetCommand.UserCommand : 
					DispatchUserCommand( gameTime, queue, msg );
					break;

				case NetCommand.Notification :
					serverInstance.FeedNotification( msg.SenderConnection.GetHailGuid(), msg.ReadString() );
					break;
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		void DispatchUserCommand ( GameTime gameTime, SnapshotQueue queue, NetIncomingMessage msg )
		{	
			var snapshotID	=	msg.ReadUInt32();
			var commandID	=	msg.ReadUInt32();
			var size		=	msg.ReadInt32();

			var data		=	msg.ReadBytes( size );

			//	we got user command and (command count=1)
			//	this means that client receives snapshot:
			if (msg.SenderConnection.GetCommandCount()==1) {
				serverInstance.ClientActivated( msg.SenderConnection.GetHailGuid() );
			}

			//	do not feed server with empty command.
			if (data.Length>0) {
				serverInstance.FeedCommand( msg.SenderConnection.GetHailGuid(), data, commandID, queue.GetLag(snapshotID, gameTime) );
			}

			//	set snapshot request when command get.
			msg.SenderConnection.SetRequestSnapshot( snapshotID, commandID );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		void NotifyClientsInternal ( string message )
		{
			if (notifications!=null) {
				lock (notifications) {
					notifications.Enqueue(message);
				}
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		float GetPingInternal ( Guid clientGuid )
		{
			if (pings==null) {
				return float.MaxValue;
			}

			float ping;

			if (pings.TryGetValue( clientGuid, out ping )) {
				return ping;
			} else {
				return float.MaxValue;
			}
		}

	}
}
