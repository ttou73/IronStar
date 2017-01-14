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
using System.Net;


namespace Fusion.Engine.Client {
	public partial class GameClient {

		class Connecting : State {

			public readonly ClientContext context;

			public Connecting ( GameClient gameClient, IPEndPoint endPoint ) : base(gameClient, ClientState.Connecting)
			{
				context	=	new ClientContext( gameClient.Game );

				Message	=	endPoint.ToString();

				//	connect
				var hail	=	context.NetClient.CreateMessage();
				hail.Write( context.Guid.ToByteArray() );
				hail.Write( Encoding.UTF8.GetBytes(context.Instance.UserInfo()) );

				context.NetClient.Connect( endPoint, hail );
			}



			public override void UserConnect ( string host, int port )
			{
				Log.Warning("Connecting in progress");
			}



			public override void UserDisconnect ( string reason )
			{
				context.NetClient.Disconnect( reason );
			}



			public override void Update ( GameTime gameTime )
			{
				DispatchIM( context.NetClient );
			}



			public override void StatusChanged(NetConnectionStatus status, string message, NetConnection connection)
			{
 				if (status==NetConnectionStatus.Connected) {
					var serverInfo		=	connection.RemoteHailMessage.PeekString();
					gameClient.SetState( new Loading( context, serverInfo ) );
				}
 				if (status==NetConnectionStatus.Disconnected) {
					gameClient.SetState( new Disconnected( context, message ) );
				}
			}


			public override void DataReceived ( NetCommand command, NetIncomingMessage msg )
			{
			}
		}
	}
}
