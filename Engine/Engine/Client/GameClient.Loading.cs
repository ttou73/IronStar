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
			readonly string serverInfo;
			

			public Loading ( ClientContext context, string serverInfo ) : base(context.GameClient, ClientState.Loading)
			{
				Message			=	serverInfo;
				this.serverInfo	=	serverInfo;
				this.context	=	context;
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

				//	
				//	TODO : update loader/precache
				//	........
				//

				context.Instance.Initialize( serverInfo );

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
