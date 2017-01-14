using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Common;
using Lidgren.Network;

namespace Fusion.Engine.Client {

	class ClientContext : IDisposable {

		public readonly Game Game;
		public readonly GameClient GameClient;
		public readonly IClientInstance Instance;
		public readonly NetClient NetClient;
		public readonly Guid Guid;

		public float Ping;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="game"></param>
		public ClientContext ( Game game )
		{
			Guid		=	Guid.NewGuid();
			Game		=	game;
			GameClient	=	game.GameClient;
			Instance	=	game.GameFactory.CreateClient( game, Guid );



			var netConfig	=	new NetPeerConfiguration(Game.GameID);

			netConfig.AutoFlushSendQueue	=	true;
			netConfig.EnableMessageType( NetIncomingMessageType.ConnectionApproval );
			netConfig.EnableMessageType( NetIncomingMessageType.ConnectionLatencyUpdated );
			netConfig.EnableMessageType( NetIncomingMessageType.DiscoveryRequest );
			netConfig.UnreliableSizeBehaviour = NetUnreliableSizeBehaviour.NormalFragmentation;

			if (Debugger.IsAttached) {
				netConfig.ConnectionTimeout		=	float.MaxValue;	
				Log.Message("CL: Debugger is attached: ConnectionTimeout = {0} sec", netConfig.ConnectionTimeout);
			}

			NetClient	=	new NetClient( netConfig );
			NetClient.Start();
		}









		private bool disposedValue = false; // To detect redundant calls


		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose( bool disposing )
		{
			if ( !disposedValue ) {
				if ( disposing ) {
					NetClient.Disconnect("Disconnect");
					NetClient.Shutdown("Disconnect");
					Instance?.Dispose();
				}

				disposedValue = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			Dispose( true );
		}
	}
}
