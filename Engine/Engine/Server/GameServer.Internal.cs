using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.Threading;
using System.Net;
using Fusion.Core.Shell;
using Fusion.Engine.Common;
using Fusion.Engine.Common.Commands;
using System.Diagnostics;
using Fusion.Core.Content;


namespace Fusion.Engine.Server {

	enum ServerState {
		NotRunning,
		Starting,
		Running,
		ShutdownRequested,
	}
	

	public partial class GameServer : GameComponent {

		Task serverTask = null;
		CancellationTokenSource killToken = null;

		object lockObj = new object();


		Thread serverThread = null;
		ServerState serverState = ServerState.NotRunning;


		/// <summary>
		/// Initiate server thread.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="postCommand"></param>
		public bool Start ( string map, string options )
		{
			lock (lockObj) {

			#if true
				if (serverState!=ServerState.NotRunning) {
					Log.Warning("Server is still running");
					return false;
				}

				serverState	=	ServerState.Starting;

				ThreadStart ts				=	delegate { ServerTaskFunc(map, options); };
				serverThread				=	new Thread( ts );
				serverThread.Name			=	"Game Server Thread";
				serverThread.IsBackground	=	true;
				serverThread.Start();

				return true;

			#else
				if (serverTask!=null) {
					if (!serverTask.IsCompleted) {
						Log.Warning("Server is still running.");
						return false;
					}
				}

				killToken	=	new CancellationTokenSource();
				serverTask	=	new Task( () => ServerTaskFunc(map, options, killToken.Token ) );
				serverTask.Start();
				return true;
			#endif
			}
		}


		
		/// <summary>
		/// Kills server thread.
		/// </summary>
		/// <param name="wait"></param>
		public bool Kill ()
		{
			#if true
			lock (lockObj) {
				if (serverState==ServerState.NotRunning) {
					Log.Warning("Server is not running");
					return false;
				}
				serverState = ServerState.ShutdownRequested;
				return true;
			}
			#else
			lock (lockObj) {
				if (serverTask==null || serverTask.IsCompleted) {
				}
				killToken?.Cancel();
				return true;
			}
			#endif
		}



		/// <summary>
		/// Waits for server thread.
		/// </summary>
		internal void Wait ()
		{
			#if true
			#else
			lock (lockObj) {
				
				killToken?.Cancel();
				Log.Message("Waiting for server task...");
				serverTask?.Wait();
			}
			#endif
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="clientGuid"></param>
		public void Drop ( Guid clientGuid )
		{
			throw new NotImplementedException();
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="map"></param>
		void ServerTaskFunc ( string map, string options )
		{
			serverState	=	ServerState.Running;

			try {
				Log.Message("Server starting: {0} [{1}]", map, options);

				using ( var serverInstance = Game.GameFactory.CreateServer(Game, map, options) ) {

					using ( var context = new ServerContext( Game, Game.GameID, Game.Network.Port, serverInstance ) ) {

						serverInstance.Initialize();

						//	Timer and fixed timestep stuff :
						//	http://gafferongames.com/game-physics/fix-your-timestep/
						var serverFrames    =   0L;
						var accumulator		=	TimeSpan.Zero;
						var stopwatch		=	new Stopwatch();
						stopwatch.Start();

						var currentTime		=	stopwatch.Elapsed;
						var time			=	stopwatch.Elapsed;

						//
						//	server loop :
						//	
						while ( serverState!=ServerState.ShutdownRequested ) {

						_retryTick:
							var targetDelta	=	TimeSpan.FromTicks( (long)(10000000 / TargetFrameRate) );
							var newTime		=	stopwatch.Elapsed;
							var frameTime	=	newTime - currentTime;
							currentTime		=	newTime;

							accumulator +=	 frameTime;

							if ( accumulator < targetDelta ) {
								Thread.Sleep(1);
								goto _retryTick;
							}

							while ( accumulator > targetDelta ) {

								//var svTime = new GameTime( time, targetDelta );
								var svTime	= new GameTime( serverFrames, time, targetDelta );
								
								//
								//	Do actual server stuff :
								//	
								context.UpdateNetworkAndLogic( svTime );

								serverFrames++;
								accumulator	-= targetDelta;
								time		+= targetDelta;
							}
						}

					}
				}

			} catch ( Exception e )	{

				Log.Error("");
				Log.Error("-------- Server crashed --------");

				Log.Error("{0}", e.ToString());

				Log.Error("----------------");
				Log.Error("");

			} finally {

				serverState	=	ServerState.NotRunning;
				Log.Message("Server shut down");
			}

		}
	}
}
