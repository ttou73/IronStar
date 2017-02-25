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
			public bool RequestSnapshot;
			public uint AckSnapshotID;
			public uint CommandID;
			public uint CommandCounter;
			internal SnapshotQueue SnapshotQueue;

			public ClientState ( Guid clientGuid, string userInfo ) 
			{
				ClientGuid		=	clientGuid;
				UserInfo		=	userInfo;
				AckSnapshotID	=	0;
				CommandID		=	0;
				CommandCounter	=	0;
				SnapshotQueue	=	new SnapshotQueue();
			}
		}



		/// <summary>
		/// Reads client GUID from connection hail-message.
		/// </summary>
		/// <param name="conn"></param>
		/// <returns></returns>
		public static Guid GetHailGuid ( this NetConnection conn )
		{
			return new Guid( conn.RemoteHailMessage.PeekBytes(16) );
		}



		/// <summary>
		/// Reads user info from connection hail-message.
		/// </summary>
		/// <param name="conn"></param>
		/// <returns></returns>
		public static string GetHailUserInfo ( this NetConnection conn )
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
			conn.Tag	=	new ClientState( GetHailGuid(conn), GetHailUserInfo(conn) );
		}



		public static ClientState GetState ( this NetConnection conn )
		{
			return conn.Tag as ClientState;
		}



		public static bool IsSnapshotRequested ( this NetConnection conn )
		{
			return (conn.GetState()!=null) && (conn.GetState().RequestSnapshot);
		}


		public static uint GetAcknoldgedSnapshotID ( this NetConnection conn )
		{
			return conn.GetState().AckSnapshotID;
		}


		public static uint GetLastCommandID ( this NetConnection conn )
		{
			return conn.GetState().CommandID;
		}


		public static void SetRequestSnapshot ( this NetConnection conn, uint snapshotAckID, uint commandID )
		{
			var state = conn.GetState();
			state.RequestSnapshot	=	true;
			state.AckSnapshotID		=	snapshotAckID;
			state.CommandID			=	commandID;
			state.CommandCounter++;
		}


		public static void ResetRequestSnapshot ( this NetConnection conn )
		{
			conn.GetState().RequestSnapshot	=	false;
		}
		

		internal static SnapshotQueue GetSnapshotQueue ( this NetConnection conn )
		{
			return conn.GetState().SnapshotQueue;
		}


		public static uint GetCommandCount ( this NetConnection conn )
		{
			return conn.GetState().CommandCounter;
		}
	}
}
