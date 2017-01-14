using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.Threading;
using System.IO;
using Fusion.Engine.Common;
using Fusion.Core.Content;
using Fusion.Engine.Common.Commands;
using Fusion.Core.Shell;

namespace Fusion.Engine.Client {

	/// <summary>
	/// Provides basic client-server interaction and client-side game logic.
	/// </summary>
	public partial class GameClient : GameComponent {

		State state;
		float ping;


		public class ClientEventArgs : EventArgs {	
			public ClientState ClientState;
			public string Message;
		}

		public event EventHandler<ClientEventArgs> ClientStateChanged;


		/// <summary>
		/// Gets current client state.
		/// </summary>
		public ClientState ClientState { get { return state.ClientState; } }


		/// <summary>
		/// Initializes a new instance of this class.
		/// </summary>
		/// <param name="Game"></param>
		public GameClient ( Game game ) : base(game) 
		{
			SetState( new StandBy(this) );
		}



		/// <summary>
		/// 
		/// </summary>
		public override void Initialize()
		{
		}


		/// <summary>
		/// Releases all resources used by the GameClient class.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose ( bool disposing )
		{
			if (disposing) {
			}
			base.Dispose( disposing );
		}

		

		/// <summary>
		/// Sets state
		/// </summary>
		/// <param name="newState"></param>
		void SetState ( State newState )
		{						
			this.state = newState;
			Log.Message("CL: State: {0} {1}", newState.GetType().Name, newState.Message );

			ClientStateChanged?.Invoke( this, new ClientEventArgs(){ ClientState = newState.ClientState, Message = newState.Message } );
		}
							


		/// <summary>
		/// Wait for client completion.
		/// </summary>
		internal void Wait ()
		{	
			if ( !(state is StandBy) && !(state is Disconnected) ) {
				Disconnect("quit");
			}


			while ( !(state is StandBy) ) {
				Thread.Sleep(50);
				Update( new GameTime() );
			}
		}



		/// <summary>
		/// Request connection. Result depends on current client state.
		/// </summary>
		/// <param name="host"></param>
		/// <param name="port"></param>
		internal void Connect ( string host, int port )
		{
			ping	=	float.MaxValue;
			state.UserConnect( host, port );
		}



		/// <summary>
		/// Request diconnect. Result depends on current client state.
		/// </summary>
		/// <param name="host"></param>
		/// <param name="port"></param>
		internal void Disconnect (string message)
		{
			state.UserDisconnect(message);
		}



		/// <summary>
		/// Updates client.
		/// </summary>
		/// <param name="gameTime"></param>
		internal void Update ( GameTime gameTime )
		{
			//
			//	Update client-side game :
			//
			state.Update( gameTime );

			//
			//	Crash test :
			//
			CrashClient.CrashTest();
			FreezeClient.FreezeTest();

			//
			//	Execute command :
			//	Should command be executed in Active state only?
			//	
			try {
				Game.Invoker.ExecuteQueue( gameTime, CommandAffinity.Client );
			} catch ( Exception e ) {
				Log.Error( e.Message );
			}
		}


	
		/// <summary>
		/// Sends server string message.
		/// This method may be used for chat 
		/// or remote server control throw Shell.
		/// </summary>
		/// <param name="message"></param>
		public void NotifyServer ( string message )
		{
			var active = state as Active;
			active?.NotifyServer( message );
		}



		/// <summary>
		/// Gets ping between client and server in seconds.
		/// If not connected return is undefined.
		/// </summary>
		public float Ping {
			get {
				return ping;
			}
		}
	}
}
