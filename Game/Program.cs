using System;
using System.IO;
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
using Fusion.Engine.Imaging;
using IronStar.Editors;
using IronStar.Core;
using IronStar.Mapping;
using Fusion.Core.Extensions;
using Fusion.Build.Mapping;
using Fusion.Engine.Client;
using Fusion.Engine.Server;
using IronStar.Editor2;

namespace IronStar {

	class Program {

		class GameFactory : IGameFactory {

			public IClientInstance CreateClient( Game game, Guid clientGuid )
			{
				return new ShooterClient( game.GameClient, clientGuid );
			}

			public IServerInstance CreateServer( Game game, string map, string options )
			{
				return new ShooterServer( game.GameServer, map );
			}

			public IUserInterface CreateUI( Game game )
			{
				return new ShooterInterface( game );
			}

			public IEditorInstance CreateEditor( Game game, string map )
			{
				return new Editor2.MapEditor( game.GameEditor, map );
			}
		}




		[STAThread]
		static int Main ( string[] args )
		{
			// 	colored console output :
			Log.AddListener( new ColoredLogListener() );

			//	output for in-game console :
			Log.AddListener( new LogRecorder() );

			//	set verbosity :
			Log.VerbosityLevel = LogMessageType.Verbose;

			//Allocator2D.RunTest(512, 1024, @"C:\GITHUB\_alloc_test");
			//return 0; 


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
			using (var game = new Game( "IronStar", new GameFactory() )) {

				//	load configuration.
				//	first run will cause warning, 
				//	because configuration file does not exist yet.
				game.Config.ExposeConfig( new EditorConfig(), "MapEditor", "editor" );

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
				if (!LaunchBox.ShowDialog(game, "Config.ini", ()=>Editor.Run(game))) {
					return 0;
				}

				//	run:
				game.Run();

				//	close editors :
				Editor.CloseAll();

				//	save configuration :
				game.Config.Save( "Config.ini" );
			}

			

			return 0;
		}
	}
}
