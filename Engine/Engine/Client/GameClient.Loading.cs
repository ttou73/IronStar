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
			
			Task loadingTask;


			public Loading ( ClientContext context, string serverInfo ) : base(context.GameClient, ClientState.Loading)
			{
				Message			=	serverInfo;
				this.serverInfo	=	serverInfo;
				this.context	=	context;

				var precacher	=	context.Instance.CreatePrecacher(serverInfo);

				loadingTask		=	new Task( ()=> precacher.LoadContent() );
				loadingTask.Start();
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


				if (loadingTask.IsCompleted) {

					if (loadingTask.IsFaulted) {
						Log.Error("-------- Precache error --------");
						Log.Error("{0}", loadingTask.Exception);
						Log.Error("----------------");

						gameClient.SetState( new Disconnected(context, "Precaching failed") );
					}

					if (disconnectReason!=null) {
						gameClient.SetState( new Disconnected(context, disconnectReason) );
					} else {
						gameClient.SetState( new Awaiting(context, serverInfo) );
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
