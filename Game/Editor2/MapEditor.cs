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
using Fusion.Engine.Graphics;
using IronStar.Mapping;
using Fusion.Build;
using BEPUphysics;
using IronStar.Core;

namespace IronStar.Editor2 {

	/// <summary>
	/// World represents entire game state.
	/// </summary>
	public partial class MapEditor : IEditorInstance {

		public static readonly BoundingBox DefaultBox = new BoundingBox( Vector3.One * (-0.25f), Vector3.One * 0.25f );

		readonly string mapName;
		readonly string fullPath;
		
		public Game Game { get; private set; }
		public ContentManager Content { get; private set; }
		readonly RenderSystem rs;

		public EditorCamera	camera;
		public Manipulator	manipulator;
		public EditorHud	hud;

		readonly Stack<MapNode[]> selectionStack = new Stack<MapNode[]>();
		readonly List<MapNode> selection = new List<MapNode>();

		Map	map = null;

		public Map Map {
			get {
				return map;
			}
		}

		public readonly EditorConfig Config;

		GameWorld world;

		public GameWorld World { get { return world; } }


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

			Config			=	Game.Config.GetConfig<EditorConfig>();

			camera			=	new EditorCamera( this );
			hud				=	new EditorHud( this );
			manipulator		=	new NullTool( this );
			world			=	new GameWorld( Game, true, new Guid() );
			world.InitServerAtoms();

			SetupUI();

			Game.Keyboard.ScanKeyboard =	true;

			fullPath	=	Builder.GetFullPath(@"maps\" + map + ".map");
		}



		/// <summary>
		/// 
		/// </summary>
		void IEditorInstance.Initialize()
		{
			if (File.Exists(fullPath)) {
				Log.Message("Opening existing map: {0}", fullPath);
				this.map = Map.LoadFromXml( File.OpenRead( fullPath ) );
			} else {
				Log.Message("Creating new map: {0}", fullPath);
				this.map = new Map();
			}

			map.ActivateMap( world );
			world.SimulateWorld( 0 );
			world.PresentWorld( 0.016f, 1 );
		}



		/// <summary>
		/// Saved at dispose
		/// </summary>
		public void SaveMap ()
		{
			Log.Message("Saving map: {0}", fullPath);
			File.Delete( fullPath );
			Map.SaveToXml( map, File.OpenWrite( fullPath ) );
		}


		/// <summary>
		/// 
		/// </summary>
		void FeedSelection ()
		{
			Editors.Editor.GetMapEditor()?.SetSelection( selection, map.Environment );
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose( bool disposing )
		{
			if ( !disposedValue ) {
				if ( disposing ) {

					world?.Dispose();
					hud?.Dispose();

					SaveMap();
				}

				disposedValue = true;
			}
		}

		private bool disposedValue = false;

		public void Dispose()
		{
			Dispose( true );
		}


		/*-----------------------------------------------------------------------------------------
		 * 
		 *	Selection :
		 * 
		-----------------------------------------------------------------------------------------*/

		public IEnumerable<MapNode> Selection {
			get { return selection; }
		}

		public MapNode[] GetSelection() 
		{
			return selection.ToArray();
		}


		public void PushSelection ()
		{
			selectionStack.Push( selection.ToArray() );
		}


		public void PopSelection ()
		{
			selection.Clear();
			selection.AddRange( selectionStack.Pop() );
		}

		public void ClearSelection ()
		{
			selection.Clear();
		}


		public void Select( MapNode factory )
		{
			if ( factory==null ) {
				throw new ArgumentNullException( "factory" );
			}
			if ( !map.Nodes.Contains( factory ) ) {
				throw new ArgumentException( "Provided factory does not exist in current map" );
			}
			selection.Clear();
			selection.Add( factory );

			FeedSelection();
		}


		public void DeleteSelection ()
		{
			foreach ( var se in selection ) {
				se.KillEntity( world );
				map.Nodes.Remove( se );
			}

			ClearSelection();
			FeedSelection();
		}



		/// <summary>
		/// 
		/// </summary>
		public void ResetWorld (bool hardResetSelection)
		{
			EnableSimulation = false;

			if (hardResetSelection) {
				foreach ( var se in selection ) {
					se.HardResetEntity( world );
				}
			}

			foreach ( var node in map.Nodes ) {
				node.ResetEntity( world );
			}

			world.SimulateWorld(0);
		}


		public void SetToEntity ()
		{
			foreach ( var se in selection ) {
				if (se.Entity!=null) {
					se.Position	=	se.Entity.Position;
					se.Rotation	=	se.Entity.Rotation;
				}
			}
		}


		public void ActivateSelected ()
		{
			foreach ( var se in selection ) {
				se?.Entity?.Controller?.Activate(null);
			}
		}


		public bool EnableSimulation { get; set; } = false;

		/*-----------------------------------------------------------------------------------------
		 * 
		 *	Selection :
		 * 
		-----------------------------------------------------------------------------------------*/

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		void IEditorInstance.Update( GameTime gameTime )
		{
			camera.Update( gameTime );

			var dr = rs.RenderWorld.Debug;

			//RefreshAppearance();

			if (EnableSimulation) {
				world.SimulateWorld( gameTime.ElapsedSec );
			}
			world.PresentWorld( gameTime.ElapsedSec, 1 );

			rs.RenderWorld.Debug.DrawGrid( 10 );

			map.DrawNavigationMeshDebug( rs.RenderWorld.Debug );

			hud.Update(gameTime);

			//
			//	Draw unselected :
			//
			foreach ( var item in map.Nodes ) {

				var color = item.Frozen ? Utils.GridColor : Utils.WireColor;

				item.Factory.Draw( dr, item.WorldMatrix, color ); 
			}

			//
			//	Draw selected :
			//
			foreach ( var item in selection ) {

				var color = Utils.WireColorSelected;

				if (selection.Last()!=item) {
					color = Color.White;
				}

				dr.DrawBasis( item.WorldMatrix, 0.5f, 3 );
				item.Factory.Draw( dr, item.WorldMatrix, color ); 
			}

			var mp = Game.Mouse.Position;

			manipulator?.Update( gameTime, mp.X, mp.Y );
		}




		public void UnfreezeAll ()
		{
			foreach ( var node in map.Nodes ) {
				node.Frozen = false;
			}
		}


		public void FreezeSelected ()
		{
			foreach ( var node in Selection ) {
				node.Frozen = true;
			}
			ClearSelection();
		}


		/// <summary>
		/// 
		/// </summary>
		void Focus ()
		{
			var targets = selection.Any() ? selection.ToArray() : map.Nodes.ToArray();

			BoundingBox bbox;

			if (!targets.Any()) {
				bbox = new BoundingBox( new Vector3(-10,-10,-10), new Vector3(10,10,10) );
			} else {

				bbox = BoundingBox.FromPoints( targets.Select( t => t.Position ).ToArray() );

			}


			var size	= Vector3.Distance( bbox.Minimum, bbox.Maximum ) + 1;
			var center	= bbox.Center();

			camera.Target	= center;
			camera.Distance = size;
		}


	}
}
