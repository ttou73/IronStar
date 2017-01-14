using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.Threading;
using System.IO;
using Fusion.Engine.Common;
using Fusion.Engine.Server;


namespace Fusion.Engine.Client {
	public partial class GameClient {

		abstract class State {

			/// <summary>
			/// game client
			/// </summary>
			protected readonly GameClient	gameClient;
			protected readonly Game	game;

			public float Ping;

			/// <summary>
			/// gets client state
			/// </summary>
			public readonly ClientState ClientState;


			/// <summary>
			/// gets message associated with client state.
			/// </summary>
			public string Message { get; protected set; }
			
			
			/// <summary>
			/// Ctor
			/// </summary>
			/// <param name="gameClient"></param>
			public State ( GameClient gameClient, ClientState clientState )
			{
				this.ClientState	=	clientState;
				this.gameClient		=	gameClient;
				this.game			=	gameClient.Game;
			}


			public abstract void UserConnect ( string host, int port );
			public abstract void UserDisconnect ( string reason );
			public abstract void Update ( GameTime gameTime );
			public abstract void StatusChanged ( NetConnectionStatus status, string message, NetConnection connection );
			public abstract void DataReceived ( NetCommand command, NetIncomingMessage msg );

		
			/// <summary>
			/// 
			/// </summary>
			/// <param name="message"></param>
			public void DispatchIM ( NetClient client )
			{
				NetIncomingMessage msg;
				while ((msg = client.ReadMessage()) != null)
				{
					switch (msg.MessageType)
					{
						case NetIncomingMessageType.VerboseDebugMessage:Log.Debug	("CL Net: " + msg.ReadString()); break;
						case NetIncomingMessageType.DebugMessage:		Log.Verbose	("CL Net: " + msg.ReadString()); break;
						case NetIncomingMessageType.WarningMessage:		Log.Warning	("CL Net: " + msg.ReadString()); break;
						case NetIncomingMessageType.ErrorMessage:		Log.Error	("CL Net: " + msg.ReadString()); break;

						case NetIncomingMessageType.ConnectionLatencyUpdated:
							Ping = msg.ReadFloat();
							if (game.Network.ShowLatency) {
								Log.Verbose("...CL ping - {0} {1,6:0.00} ms", msg.SenderEndPoint, (Ping*1000) );
							}
							break;

						case NetIncomingMessageType.StatusChanged:		

							var status	=	(NetConnectionStatus)msg.ReadByte();
							var message	=	msg.ReadString();
							Log.Message("CL: {0} - {1}", status, message );

							StatusChanged( status, message, msg.SenderConnection );

							break;
					
						case NetIncomingMessageType.Data:
						
							var netCmd	=	(NetCommand)msg.ReadByte();
							DataReceived( netCmd, msg );

							break;
					
						default:
							Log.Warning("CL: Unhandled type: " + msg.MessageType);
							break;
					}
					client.Recycle(msg);
				}			
			}
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="client"></param>
		/// <param name="userCmd"></param>
		void SendUserCommand ( NetClient client, uint recvSnapshotFrame, uint cmdCounter, byte[] userCmd )
		{
			var msg = client.CreateMessage( userCmd.Length + 4 * 3 + 1 );

			msg.Write( (byte)NetCommand.UserCommand );
			msg.Write( recvSnapshotFrame );
			msg.Write( cmdCounter );
			msg.Write( userCmd.Length );
			msg.Write( userCmd );

			//	Zero snapshot frame index means that we are waiting for first snapshot.
			//	and command shoud reach the server.
			var delivery	=	recvSnapshotFrame == 0 ? NetDeliveryMethod.ReliableOrdered : NetDeliveryMethod.UnreliableSequenced;

			client.SendMessage( msg, delivery );
		}
	}
}
