using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fusion;
using Fusion.Build;
using Fusion.Development;
using Fusion.Engine.Common;
using Fusion.Core.Shell;
using Fusion.Core.Utils;

namespace IronStar {

	class Program {
		[STAThread]
		static int Main ( string[] args )
		{
			// 	colored console output :
			Log.AddListener( new ColoredLogListener() );

			//	output for in-game console :
			Log.AddListener( new LogRecorder() );

			//	set verbosity :
			Log.VerbosityLevel = LogMessageType.Verbose;


			//
			//	Build content on startup.
			//	Remove this line in release code.
			//
			Builder.Options.InputDirectory = @"..\..\..\..\Content";
			Builder.Options.TempDirectory = @"..\..\..\..\Temp";
			Builder.Options.OutputDirectory = @"Content";
			Builder.SafeBuild();


			//
			//	Run game :
			//
			using (var game = new Game( "IronStar" )) {

				//	create SV, CL and UI instances :
				game.GameServer = new ShooterServer( game );
				game.GameClient = new ShooterClient( game );
				game.UserInterface = new ShooterInterface( game );

				//	load configuration.
				//	first run will cause warning, 
				//	because configuration file does not exist yet.
				game.Config.Load( "Config.ini" );

				//	enable and disable debug direct3d device :
				game.RenderSystem.UseDebugDevice = false;
				game.RenderSystem.Fullscreen	= false;

				//	enable and disable object tracking :
				game.TrackObjects = false;

				//	set game title :
				game.GameTitle = "IronStar";

				//	apply command-line options here:
				//	...
				if (!LaunchBox.ShowDialog(game, "Config.ini")) {
					return 0;
				}

				//	run:
				game.Run();

				//	save configuration :
				game.Config.Save( "Config.ini" );
			}

			return 0;
		}
	}
}
