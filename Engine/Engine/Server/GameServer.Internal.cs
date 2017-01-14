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
	

	public partial class GameServer : GameComponent {

		Task serverTask;
		CancellationTokenSource killToken;

		object lockObj = new object();


		/// <summary>
		/// Gets whether server is still alive.
		/// </summary>
		internal bool IsAlive {
			get {
				return serverTask != null; 
			}
		}



		/// <summary>
		/// Initiate server thread.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="postCommand"></param>
		internal void StartInternal ( string map, string postCommand )
		{
			lock (lockObj) {
				if (IsAlive) {
					Log.Warning("Can not start server, it is already running");
					return;
				}

				killToken	=	new CancellationTokenSource();
				serverTask	=	new Task( () => ServerTaskFunc(map, ()=>Game.GameFactory.CreateServer(Game,map), killToken.Token ) );
				serverTask.Start();
			}
		}


		
		/// <summary>
		/// Kills server thread.
		/// </summary>
		/// <param name="wait"></param>
		internal void KillInternal ()
		{
			lock (lockObj) {
				if (!IsAlive) {
					Log.Warning("Server is not running");
				}

				if (killToken!=null) {
					killToken.Cancel();
				}
			}
		}



		/// <summary>
		/// Waits for server thread.
		/// </summary>
		internal void Wait ()
		{
			lock (lockObj) {
				if (killToken!=null) {
					killToken.Cancel();
				}

				if (serverTask!=null) {
					Log.Message("Waiting for server task...");
					serverTask.Wait();
				}
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="map"></param>
		void ServerTaskFunc ( string map, Func<IServerInstance> createServer, CancellationToken killToken )
		{
			try {
				Log.Message("Server starting: {0}", map);

				using ( var serverInstance = createServer() ) {

					serverInstance.Initialize();

					using ( var context = new GameServerContext( Game, Game.GameID, Game.Network.Port, serverInstance ) ) {

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
						while ( !killToken.IsCancellationRequested ) {

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
								var svTime = new GameTime( serverFrames, time, targetDelta );

								//
								//	Do actual server stuff :
								//	
								context.UpdateNetworkAndLogic( svTime );

								accumulator	-= targetDelta;
								time		+= targetDelta;
							}
						}

						Log.Message("Server shut down");
					}
				}
			} catch ( Exception e )
			{
				Log.Error("");
				Log.Error("-------- Server crashed --------");

				Log.Error("{0}", e.ToString());

				Log.Error("----------------");
				Log.Error("");
			}
		}
	}
}
