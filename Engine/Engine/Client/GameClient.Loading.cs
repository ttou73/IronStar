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

		class Loading : State {

			/// <summary>
			/// if null - no reason to disconnect.
			/// </summary>
			string disconnectReason = null;
			readonly ClientContext context;
			

			public Loading ( ClientContext context, string serverInfo ) : base(context.GameClient, ClientState.Loading)
			{
				Message			=	serverInfo;
				this.context	=	context;

				context.Instance.Initialize( serverInfo );
			}


			public override void UserConnect ( string host, int port )
			{
				Log.Warning("Already connected. Loading in progress.");
			}


			public override void UserDisconnect ( string reason )
			{
				context.NetClient.Disconnect( reason );
			}


			public override void Update ( GameTime gameTime )
			{
				DispatchIM( context.NetClient );

				//	TODO : update loader/precache

				//	sleep a while to get 
				//	other threads more time.
				Thread.Sleep(1);

				if (true) {
					if (disconnectReason!=null) {
						gameClient.SetState( new Disconnected(context, disconnectReason) );
					} else {
						gameClient.SetState( new Awaiting(context) );
					}
				}
			}


			public override void StatusChanged(NetConnectionStatus status, string message, NetConnection connection)
			{
				if (status==NetConnectionStatus.Disconnected) {
					disconnectReason = message;
				}
			}


			public override void DataReceived ( NetCommand command, NetIncomingMessage msg )
			{
			}
		}
	}
}
