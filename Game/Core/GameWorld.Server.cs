using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion.Core.Content;
using Fusion.Engine.Server;
using Fusion.Engine.Client;
using Fusion.Core.Extensions;
using IronStar.SFX;
using Fusion.Core.IniParser.Model;
using IronStar.Views;
using Fusion.Engine.Graphics;
using IronStar.Mapping;

namespace IronStar.Core {

	/// <summary>
	/// World represents entire game state.
	/// </summary>
	public partial class GameWorld : IServerInstance, IClientInstance {


		public void Initialize()
		{
			Log.Message("Initialize");
		}

		public byte[] Update( GameTime gameTime )
		{
			SimulateWorld( gameTime.ElapsedSec );

			//	write world to stream :
			using ( var ms = new MemoryStream() ) {
				WriteToSnapshot( ms );
				return ms.GetBuffer();
			}
		}

		public void FeedCommand( Guid clientGuid, byte[] userCommand, uint commandID, float lag )
		{
			if ( !userCommand.Any() ) {
				return;
			}

			PlayerCommand( clientGuid, userCommand, lag );
		}

		public void FeedNotification( Guid clientGuid, string message )
		{
			Log.Message( "NOTIFICATION {0}: {1}", clientGuid, message );
		}

		public void ClientConnected( Guid clientGuid, string userInfo )
		{
			PlayerConnected( clientGuid, userInfo );
		}

		public void ClientActivated( Guid clientGuid )
		{
			PlayerEntered( clientGuid );
		}

		public void ClientDeactivated( Guid clientGuid )
		{
			PlayerLeft( clientGuid );
		}

		public void ClientDisconnected( Guid clientGuid )
		{
			PlayerDisconnected( clientGuid );
		}

		public bool ApproveClient( Guid clientGuid, string userInfo, out string reason )
		{
			reason = "";
			return true;
			throw new NotImplementedException();
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose( bool disposing )
		{
			if ( !disposedValue ) {
				if ( disposing ) {
					Shutdown();
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose( true );
		}
		#endregion
	}
}
