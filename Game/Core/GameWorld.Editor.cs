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
	public partial class GameWorld : IServerInstance, IClientInstance, IEditorInstance {

		readonly bool isEditor = false;

		RenderSystem rs;

		/// <summary>
		/// Initializes server-side world.
		/// </summary>
		/// <param name="maxPlayers"></param>
		/// <param name="maxEntities"></param>
		public GameWorld ( GameEditor editor, string map )
		{
			this.mapName	=	map;

			isEditor	=	true;
			
			Atoms	=	new AtomCollection();

			Log.Verbose( "world: editor" );
			this.serverSide =   false;
			this.Game       =   editor.Game;
			this.UserGuid   =   new Guid();
			this.rs			=	Game.RenderSystem;
			Content         =   new ContentManager( Game );
			entities        =   new EntityCollection(Atoms);

			AddAtoms();
		}



		/// <summary>
		/// Saved at dispose
		/// </summary>
		public void SaveMap ()
		{
			if (isEditor) {

			}
		}



		void IEditorInstance.Initialize()
		{
		}



		void IEditorInstance.Update( GameTime gameTime )
		{
			//rs.
			//SimulateWorld( gameTime.ElapsedSec );

			//modelManager.Update( gameTime.ElapsedSec, 1 );
		}
	}
}
