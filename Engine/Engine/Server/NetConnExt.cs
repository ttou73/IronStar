using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Fusion.Engine.Server {

	public static class NetConnExt {

		public class ClientState {

			public readonly Guid ClientGuid;
			public readonly string UserInfo;
			public bool IsSnapshotRequested;
			public uint AckSnapshotID { get; private set; }
			public uint LastCommandID { get; private set; }
			public uint CommandCounter { get; private set; }
			internal readonly SnapshotQueue SnapshotQueue;

			public ClientState ( Guid clientGuid, string userInfo ) 
			{
				ClientGuid		=	clientGuid;
				UserInfo		=	userInfo;
				AckSnapshotID	=	0;
				LastCommandID		=	0;
				CommandCounter	=	0;
				SnapshotQueue	=	new SnapshotQueue();
			}

			public void RequestSnapshot ( uint snapshotAckID, uint commandID )
			{
				IsSnapshotRequested	=	true;
				AckSnapshotID		=	snapshotAckID;
				LastCommandID		=	commandID;
				CommandCounter++;
			}
		}



		/// <summary>
		/// Reads client GUID from connection hail-message.
		/// </summary>
		/// <param name="conn"></param>
		/// <returns></returns>
		public static Guid PeekHailGuid ( this NetConnection conn )
		{
			return new Guid( conn.RemoteHailMessage.PeekBytes(16) );
		}



		/// <summary>
		/// Reads user info from connection hail-message.
		/// </summary>
		/// <param name="conn"></param>
		/// <returns></returns>
		public static string PeekHailUserInfo ( this NetConnection conn )
		{
			var bytes = conn.RemoteHailMessage.PeekDataBuffer();
			return Encoding.UTF8.GetString( bytes, 16, bytes.Length-16);
		}



		/// <summary>
		/// Initializes client state for given connection.
		/// </summary>
		/// <param name="conn"></param>
		public static void InitClientState ( this NetConnection conn )
		{
			conn.Tag	=	new ClientState( PeekHailGuid(conn), PeekHailUserInfo(conn) );
		}



		public static ClientState GetState ( this NetConnection conn )
		{
			return conn.Tag as ClientState;
		}
	}
}
