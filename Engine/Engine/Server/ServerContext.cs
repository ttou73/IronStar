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
	

	class ServerContext : IDisposable {
		
		readonly Game game;
		readonly Queue<string> notifications = null;

		IServerInstance	serverInstance;
		NetServer		netServer;

		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="server"></param>
		/// <param name="serverInstance"></param>
		public ServerContext ( Game game, string gameId, int port, IServerInstance serverInstance )
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

			netServer		=	new NetServer( netConfig );
			netServer.Start();
		}



		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose( bool disposing )
		{
			if ( !disposedValue ) {
				if ( disposing ) {
					
					netServer.Shutdown("Server shutdown");
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
			netServer.Configuration.SimulatedLoss			=	game.Network.SimulatePacketsLoss;
			netServer.Configuration.SimulatedMinimumLatency	=	game.Network.SimulateMinLatency;
			netServer.Configuration.SimulatedRandomLatency	=	game.Network.SimulateRandomLatency;
			#endif

			//	read input messages :
			DispatchIM( svTime, netServer );

			//	update frame and get snapshot :
			serverInstance.Update( svTime );

			//	send snapshot to clients :
			DispatchSnapshots( netServer, svTime.Total.Ticks );

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


		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		void DispatchIM ( GameTime gameTime, NetServer server )
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
						
						var userGuid	=	msg.SenderConnection.PeekHailGuid();
						var userInfo	=	msg.SenderConnection.PeekHailUserInfo();
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
						DispatchDataIM( gameTime, msg );
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
					serverInstance.ClientConnected( msg.SenderConnection.PeekHailGuid(), msg.SenderConnection.PeekHailUserInfo() );
					break;

				case NetConnectionStatus.Disconnected :
					serverInstance.ClientDeactivated( msg.SenderConnection.PeekHailGuid() );
					serverInstance.ClientDisconnected( msg.SenderConnection.PeekHailGuid() );
					break;

				default:
					break;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="server"></param>
		void DispatchSnapshots ( NetServer server, long serverTicks )
		{
			//	snapshot request is stored in connection's tag.s
			var debug	=	game.Network.ShowSnapshots;
			var conns	=	server.Connections.ToArray();

			var sw		=	new Stopwatch();

			foreach ( var conn in conns ) {

				var state	=	conn.GetState();

				if (state==null) {
					continue;
				}

				if (!state.IsSnapshotRequested) {
					return;
				}

				sw.Reset();
				sw.Start();

				var guid	=	state.ClientGuid;
				var queue	=	state.SnapshotQueue;

				queue.Push( serverInstance.MakeSnapshot( guid ) );
					
				var snapID		=	queue.LatestSnapshotID;
				var ackSnapID	=	state.AckSnapshotID;
				int size		=	0;
				var commandID	=	state.LastCommandID;
				var snapshot	=	queue.Compress( ref ackSnapID, out size );

				//	reset snapshot request :
				state.IsSnapshotRequested = false;

				var msg = server.CreateMessage( snapshot.Length + 4 * 4 + 8 + 1 );
			
				msg.Write( (byte)NetCommand.Snapshot );
				msg.Write( snapID );
				msg.Write( ackSnapID );
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
				var delivery	=	ackSnapID == 0 ? NetDeliveryMethod.ReliableOrdered : NetDeliveryMethod.UnreliableSequenced;

				if (ackSnapID==0) {
					Log.Message("SV: Sending initial snapshot to {0}", conn.PeekHailGuid().ToString() );
				}

				sw.Stop();

				server.SendMessage( msg, conn, delivery, 0 );

				if (debug) {
					Log.Message("Snapshot: #{0} - #{1} : {2} / {3} to {4} at {5} msec", 
						snapID, ackSnapID, snapshot.Length, size, conn.RemoteEndPoint.ToString(), sw.Elapsed.TotalMilliseconds );
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
		void DispatchDataIM ( GameTime gameTime, NetIncomingMessage msg )
		{
			var netCmd = (NetCommand)msg.ReadByte();

			switch (netCmd) {
				case NetCommand.UserCommand : 
					DispatchUserCommand( gameTime, msg );
					break;

				case NetCommand.Notification :
					serverInstance.FeedNotification( msg.SenderConnection.PeekHailGuid(), msg.ReadString() );
					break;
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		void DispatchUserCommand ( GameTime gameTime, NetIncomingMessage msg )
		{	
			var snapshotID	=	msg.ReadUInt32();
			var commandID	=	msg.ReadUInt32();
			var size		=	msg.ReadInt32();

			var data		=	msg.ReadBytes( size );

			var state		=	msg.SenderConnection.GetState();

			//	we got user command and (command count=1)
			//	this means that client receives snapshot:
			if (state.CommandCounter==1) {
				serverInstance.ClientActivated( msg.SenderConnection.PeekHailGuid() );
			}

			//	do not feed server with empty command.
			if (data.Length>0) {
				serverInstance.FeedCommand( msg.SenderConnection.PeekHailGuid(), data, commandID, 0 );
			}

			//	set snapshot request when command get.
			msg.SenderConnection.GetState().RequestSnapshot( snapshotID, commandID );
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
	}
}
