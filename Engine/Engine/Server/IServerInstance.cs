using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Common;

namespace Fusion.Engine.Server {
	public interface IServerInstance : IDisposable {

		/// <summary>
		/// Completes initialization of server instance.
		/// </summary>
		void Initialize ();

		/// <summary>
		/// Runs one step of server-side world simulation.
		/// <remarks>Due to delta compression of snapshot keep data aligned. 
		/// Even small layout change will cause significiant increase of sending data</remarks>
		/// </summary>
		/// <param name="gameTime"></param>
		/// <returns>Snapshot bytes</returns>
		byte[] Update ( GameTime gameTime );

		/// <summary>
		/// Gets atoms defined by server instance.
		/// </summary>
		AtomCollection Atoms { get; }

		/// <summary>
		/// Feed server with commands from particular client.
		/// </summary>
		/// <param name="clientGuid">Client's GUID</param>
		/// <param name="userCommand">Client's user command bytes</param>
		/// <param name="commandID">Client's user command index</param>
		/// <param name="lag">Lag in seconds</param>
		void FeedCommand ( Guid clientGuid, byte[] userCommand, uint commandID, float lag );

		/// <summary>
		/// Feed server with commands from particular client.
		/// </summary>
		/// <param name="clientGuid">Client's GUID</param>
		/// <param name="command">Client's user command stream</param>
		void FeedNotification ( Guid clientGuid, string message );

		/// <summary>
		/// Gets server information that required for client to load the game.
		/// This information usually contains map name and game type.
		/// This information is also used for discovery response.
		/// </summary>
		/// <returns></returns>
		string ServerInfo ();

		/// <summary>
		/// Called when client connected.
		/// </summary>
		/// <param name="clientGuid">Client GUID.</param>
		/// <param name="userInfo">User information. Cann't be used as client identifier.</param>
		void ClientConnected ( Guid clientGuid, string userInfo );

		/// <summary>
		/// Called when client received snapshot and ready to play.
		/// </summary>
		/// <param name="clientGuid">Client GUID.</param>
		void ClientActivated ( Guid clientGuid );

		/// <summary>
		/// Called when client deactivated.
		/// This mehtod would not be called when server shuts down.
		/// </summary>
		/// <param name="clientGuid">Client GUID.</param>
		void ClientDeactivated ( Guid clientGuid );

		/// <summary>
		/// Called when client disconnected.
		/// This mehtod would not be called when server shuts down.
		/// </summary>
		/// <param name="clientGuid">Client IP in format 123.45.67.89:PORT. Could be used as client identifier.</param>
		void ClientDisconnected ( Guid clientGuid );

		/// <summary>
		/// Approves client by id and user information.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="userInfo"></param>
		/// <param name="reason">If method returns false this output parameters contains the reason of denial</param>
		/// <returns></returns>
		bool ApproveClient ( Guid clientGuid, string userInfo, out string reason );
	}
}
