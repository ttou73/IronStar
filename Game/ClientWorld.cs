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


namespace IronStar {
	class ClientWorld : IClientInstance {

		public IContentPrecacher CreatePrecacher( string serverInfo )
		{
			throw new NotImplementedException();
		}

		public void FeedAtoms( AtomCollection atoms )
		{
			throw new NotImplementedException();
		}

		public void FeedNotification( string message )
		{
			throw new NotImplementedException();
		}

		public void FeedSnapshot( GameTime serverTime, byte[] snapshot, uint ackCommandID )
		{
			throw new NotImplementedException();
		}

		public void Initialize( string serverInfo )
		{
			throw new NotImplementedException();
		}

		public byte[] Update( GameTime gameTime, uint sentCommandID )
		{
			throw new NotImplementedException();
		}

		public string UserInfo()
		{
			throw new NotImplementedException();
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose( bool disposing )
		{
			if ( !disposedValue ) {
				if ( disposing ) {
					// TODO: dispose managed state (managed objects).
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~ClientWorld() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose( true );
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion

	}
}
