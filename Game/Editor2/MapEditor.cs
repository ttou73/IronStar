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

namespace IronStar.Editor2 {

	/// <summary>
	/// World represents entire game state.
	/// </summary>
	public partial class MapEditor : IEditorInstance {

		readonly string mapName;
		
		public Game Game { get; private set; }
		public ContentManager Content { get; private set; }
		readonly RenderSystem rs;

		EdCamera	edCamera;

		/// <summary>
		/// Initializes server-side world.
		/// </summary>
		/// <param name="maxPlayers"></param>
		/// <param name="maxEntities"></param>
		public MapEditor ( GameEditor editor, string map )
		{
			this.mapName	=	map;

			Log.Verbose( "game editor" );
			this.Game       =   editor.Game;
			this.rs			=	Game.RenderSystem;
			Content         =   new ContentManager( Game );

			edCamera	=	new EdCamera( Game.RenderSystem );

			SetupUI();

			Game.Keyboard.ScanKeyboard =	true;
		}



		/// <summary>
		/// Saved at dispose
		/// </summary>
		public void SaveMap ()
		{
		}



		/// <summary>
		/// 
		/// </summary>
		void IEditorInstance.Initialize()
		{
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose( bool disposing )
		{
			if ( !disposedValue ) {
				if ( disposing ) {
				}

				disposedValue = true;
			}
		}

		private bool disposedValue = false;

		public void Dispose()
		{
			Dispose( true );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		void IEditorInstance.Update( GameTime gameTime )
		{
			edCamera.Update( gameTime );

			rs.RenderWorld.Debug.DrawGrid( 10 );
			//rs.
			//SimulateWorld( gameTime.ElapsedSec );

			//modelManager.Update( gameTime.ElapsedSec, 1 );
		}

	}
}
