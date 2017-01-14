using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Engine.Common;
using Fusion.Core.Content;

namespace Fusion.Engine.Client {
	public interface IClientInstance : IDisposable {

		/// <summary>
		/// Called when the game has determined that client-side need to be initialized.
		/// </summary>
		/// <param name="gameTime">Cliemt-side game time.</param>
		/// <param name="sentCommandID">Command's ID that are going to be sent.</param>
		/// <returns>User command bytes</returns>
		void Initialize ( string serverInfo );

		/// <summary>
		/// Called when the game has determined that client-side logic needs to be processed.
		/// </summary>
		/// <param name="gameTime">Cliemt-side game time.</param>
		/// <param name="sentCommandID">Command's ID that are going to be sent.</param>
		/// <returns>User command bytes</returns>
		byte[] Update ( GameTime gameTime, uint sentCommandID );

		/// <summary>
		/// Feed server atoms to client
		/// </summary>
		/// <param name="atoms"></param>
		void FeedAtoms ( AtomCollection atoms );

		/// <summary>
		/// Feed server snapshot to client.
		/// Called when fresh snapshot arrived.
		/// <remarks>Not all snapshot could reach client.</remarks>
		/// </summary>
		/// <param name="serverTime">Server time includes number of server frames, total server time and elapsed time since last server frame. 
		/// <param name="snapshotStream">Snapshot data stream.</param>
		/// <param name="ackCommandID">Acknoledged (e.g. received and responsed) command ID. Zero value means first snapshot.</param>
		void FeedSnapshot ( GameTime serverTime, byte[] snapshot, uint ackCommandID );

		/// <summary>
		/// Feed notification from server.
		/// </summary>
		/// <param name="message">Message from server</param>
		void FeedNotification ( string message );

		/// <summary>
		/// Gets user information. 
		/// Called when client-server game logic has determined that server needs user information.
		/// </summary>
		/// <returns>User information</returns>
		string UserInfo ();
	}
}
