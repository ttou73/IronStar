using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Fusion;
using Fusion.Core;
using Fusion.Core.Mathematics;
using Fusion.Engine.Common;
using Fusion.Engine.Client;
using Fusion.Engine.Server;
using Fusion.Engine.Graphics;
using ShooterDemo.Core;


namespace ShooterDemo {
	class ShooterLoader : GameLoader {

		Task loadingTask;

		/// <summary>
		/// Loaded world.
		/// </summary>
		public World World { get; private set; }


		/// <summary>
		/// 
		/// </summary>
		/// <param name="client"></param>
		/// <param name="serverInfo"></param>
		public ShooterLoader ( ShooterClient client, string serverInfo )
		{
			loadingTask	=	new Task( ()=>LoadingTask(client, serverInfo) );

			loadingTask.Start();
		}


		
		/// <summary>								  
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update ( GameTime gameTime )
		{
			//	do nothing.
		}



		/// <summary>
		/// 
		/// </summary>
		public override bool IsCompleted {
			get { 
				return loadingTask.IsCompleted; 
			}
		}


		/// <summary>
		/// 
		/// </summary>
		void LoadingTask ( ShooterClient client, string serverInfo )
		{
			try {
				var world = new MPWorld( client, serverInfo );
				World	=	world;

			} catch ( Exception e ) {
				Log.Error("{0}", e.ToString());
				throw;
			}
		}
	}
}
