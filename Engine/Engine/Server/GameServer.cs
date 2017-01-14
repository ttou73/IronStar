using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.Threading;
using Fusion.Core.Shell;
using System.IO;
using Fusion.Engine.Common;
using Fusion.Core.Content;
using Fusion.Core.Configuration;


namespace Fusion.Engine.Server {

	/// <summary>
	/// Provides basic client-server interaction and server-side game logic.
	/// </summary>
	public partial class GameServer : GameComponent {


		/// <summary>
		/// <summary>
		/// Gets and sets target server frame rate.
		/// Value must be within range 1..240.
		/// </summary>
		[Config]
		public float TargetFrameRate {
			get { return targetFrameRate; }
			set {
				if (value<1 || value>240) {
					throw new ArgumentOutOfRangeException("value", "Value must be within range 1..240.");
				}
				targetFrameRate	=	value;
			}
		}
		float targetFrameRate = 60;



		/// <summary>
		/// Initializes a new instance of this class.
		/// </summary>
		/// <param name="Game"></param>
		public GameServer ( Game game ) : base(game)
		{
		}



		/// <summary>
		/// 
		/// </summary>
		public override void Initialize()
		{
			
		}



		/// <summary>
		/// Releases all resources used by the GameServer class.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose ( bool disposing )
		{
			if (disposing) {
			}
			base.Dispose( disposing );
		}



		/// <summary>
		/// Sends text message to all clients.
		/// </summary>
		/// <param name="message"></param>
		public void NotifyClients ( string format, params object[] args )
		{
			throw new NotImplementedException();
		}
	}
}
