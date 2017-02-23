#define USE_PRIORITY_QUEUE
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Fusion.Core;
using Fusion.Core.Mathematics;
using Fusion.Core.Configuration;
using Fusion.Engine.Common;
using Fusion.Drivers.Graphics;
using Fusion.Engine.Imaging;
using System.Threading;
using Fusion.Core.Collection;
using Fusion.Engine.Storage;
using Fusion.Build.Mapping;

namespace Fusion.Engine.Graphics {


	/// <summary>
	/// 
	/// </summary>
	internal class VTTileLoader : IDisposable {

		readonly IStorage storage;
		readonly VTSystem vt;

		object lockObj = new object();

		#if USE_PRIORITY_QUEUE
		ConcurrentPriorityQueue<int,VTAddress>	requestQueue;
		#else
		ConcurrentQueue<VTAddress>	requestQueue;
		#endif
		
		ConcurrentQueue<VTTile>		loadedTiles;

		Thread	loaderThread;
		bool	stopLoader = false;
		

		/// <summary>
		/// 
		/// </summary>
		/// <param name="baseDirectory"></param>
		public VTTileLoader ( VTSystem vt, IStorage storage )
		{
			this.storage		=	storage;
			this.vt				=	vt;

			#if USE_PRIORITY_QUEUE
				requestQueue	=	new ConcurrentPriorityQueue<int,VTAddress>();
			#else
				requestQueue	=	new ConcurrentQueue<VTAddress>();
			#endif

			loadedTiles			=	new ConcurrentQueue<VTTile>();

			loaderThread		=	new Thread( new ThreadStart( LoaderTask ) );
			loaderThread.Name	=	"VT Tile Loader Thread";
			loaderThread.IsBackground	=	true;
			loaderThread.Start();
		}


		/// <summary>
		/// Request texture loading
		/// </summary>
		/// <param name="address"></param>
		public void RequestTile ( VTAddress address )
		{
			#if USE_PRIORITY_QUEUE
				requestQueue.Enqueue( address.MipLevel, address );
			#else
				requestQueue.Enqueue( address );
			#endif
		}



		/// <summary>
		/// Gets loaded tile or zero
		/// </summary>
		/// <returns></returns>
		public bool TryGetTile ( out VTTile image )
		{
			return loadedTiles.TryDequeue( out image );
		}



		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose( bool disposing )
		{
			if ( !disposedValue ) {
				if ( disposing ) {
					lock (lockObj) {
						stopLoader	=	true;
					}
				}

				disposedValue = true;
			}
		}


		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			Dispose( true );
		}
		#endregion



		/// <summary>
		/// Removes all loaded tiles and requests
		/// </summary>
		public void Purge ()
		{
			lock (lockObj) {
				requestQueue.Clear();

				VTTile t;

				while (loadedTiles.TryDequeue(out t)) {
				}
			}
		}



		/// <summary>
		/// Functionas running in separate thread
		/// </summary>
		void LoaderTask ()
		{
			while (!stopLoader) {
				
				VTAddress address;

			#if USE_PRIORITY_QUEUE
				address = default(VTAddress);
				KeyValuePair<int,VTAddress> result;
				if (!requestQueue.TryDequeue(out result)) {
					//Thread.Sleep(1);
					continue;
				} else {
					address = result.Value;
				}
			#else
				if (!requestQueue.TryDequeue(out address)) {
					//Thread.Sleep(1);
					continue;
				}
			#endif

					
				var fileName = address.GetFileNameWithoutExtension(".tile");

				//Log.Message("...vt tile load : {0}", fileName );

				try {
					
					var tile = new VTTile( address );
					tile.Read( storage.OpenFile( fileName, FileMode.Open, FileAccess.Read ) );

					loadedTiles.Enqueue( tile );

				} catch ( IOException ioex ) {

					var tile = new VTTile( address );
					tile.Clear( Color.Magenta );

					loadedTiles.Enqueue( tile );

					Log.Warning("{0}", ioex );
				}

			}
		}

	}
}
