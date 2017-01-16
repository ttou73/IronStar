using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Drivers.Graphics;
using System.Net;
using Lidgren.Network;

namespace Fusion.Engine.Common {

	public sealed class UserInterface : GameComponent {

		NetClient client;

		TimeSpan timeout;

		object lockObj = new object();

		IUserInterface uiInstance;

		/// <summary>
		/// Gets instance if user interface
		/// </summary>
		public IUserInterface Instance {
			get {
				return uiInstance;
			}
		}
		


		/// <summary>
		/// Creates instance of UserInterface
		/// </summary>
		/// <param name="Game"></param>
		public UserInterface ( Game game ) : base(game)
		{
			uiInstance	=	game.GameFactory.CreateUI(game);
		}


		/// <summary>
		/// 
		/// </summary>
		public override void Initialize ()
		{
			uiInstance.Initialize();
		}



		/// <summary>
		/// Overloaded. Immediately releases the unmanaged resources used by this object. 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose ( bool disposing )
		{
			if (disposing) {
				client?.Shutdown("dispose");
				client = null;

				uiInstance?.Dispose();
				uiInstance = null;
			}
			base.Dispose( disposing );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		internal void Update ( GameTime gameTime )
		{
			lock (lockObj) {
				if (client!=null) {

					DispatchIM( client );

					timeout -= gameTime.Elapsed;

					if (timeout<TimeSpan.Zero) {
						StopDiscovery();
					}
				}
			}
			
			uiInstance.Update( gameTime );
		}



		/// <summary>
		/// 
		/// </summary>
		internal void RequestToExit()
		{
			uiInstance.RequestToExit();
		}



		/// <summary>
		/// Starts server discovery.
		/// </summary>
		/// <param name="numPorts">Number of ports to scan.</param>
		/// <param name="timeout">Time to scan.</param>
		public void StartDiscovery ( int numPorts, TimeSpan timeout )
		{
			lock (lockObj) {
				if (client!=null) {
					Log.Warning("Discovery is already started.");
					return;
				}

				this.timeout	=	timeout;

				var netConfig = new NetPeerConfiguration( Game.GameID );
				netConfig.EnableMessageType( NetIncomingMessageType.DiscoveryRequest );
				netConfig.EnableMessageType( NetIncomingMessageType.DiscoveryResponse );

				client	=	new NetClient( netConfig );
				client.Start();

				var svPort	=	Game.Network.Port;

				var ports = Enumerable.Range(svPort, numPorts)
							.Where( p => p <= ushort.MaxValue )
							.ToArray();

				Log.Message("Start discovery on ports: {0}", string.Join(", ", ports) );

				foreach (var port in ports) {
					client.DiscoverLocalPeers( port );
				}
			}
		}



		/// <summary>
		/// Stops server discovery.
		/// </summary>
		public void StopDiscovery ()
		{
			lock (lockObj) {
				if (client==null) {
					Log.Warning("Discovery is already started.");
					return;
				}

				Log.Message("Discovery is stopped.");

				client.Shutdown("stop discovery");
				client = null;
			}
		}


		/// <summary>
		/// Indicates that discovery in progress.
		/// </summary>
		/// <returns></returns>
		bool IsDiscoveryRunning()
		{
			return (client!=null);
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		void DispatchIM ( NetClient client )
		{
			NetIncomingMessage msg;
			while ((msg = client.ReadMessage()) != null)
			{
				switch (msg.MessageType)
				{
					case NetIncomingMessageType.VerboseDebugMessage:Log.Debug	("UI Net: " + msg.ReadString()); break;
					case NetIncomingMessageType.DebugMessage:		Log.Verbose	("UI Net: " + msg.ReadString()); break;
					case NetIncomingMessageType.WarningMessage:		Log.Warning	("UI Net: " + msg.ReadString()); break;
					case NetIncomingMessageType.ErrorMessage:		Log.Error	("UI Net: " + msg.ReadString()); break;

					case NetIncomingMessageType.DiscoveryResponse:
						uiInstance.DiscoveryResponse( msg.SenderEndPoint, msg.ReadString() );
						break;

					//case NetIncomingMessageType.StatusChanged:		

					//	var status	=	(NetConnectionStatus)msg.ReadByte();
					//	var message	=	msg.ReadString();
					//	Log.Message("UI: {0} - {1}", status, message );

					//	break;
					
					//case NetIncomingMessageType.Data:
						
					//	var netCmd	=	(NetCommand)msg.ReadByte();
					//	state.DataReceived( netCmd, msg );

					//	break;
					
					default:
						Log.Warning("CL: Unhandled type: " + msg.MessageType);
						break;
				}
				client.Recycle(msg);
			}			
		}
	}
}
