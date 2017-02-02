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
			world			=	new GameWorld( Game, true );
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

			foreach ( var node in map.Nodes ) {
				node.SpawnEntity( world );
			}
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
			Editors.Editor.GetMapEditor()?.SetSelection( selection );
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

				var color = Utils.WireColor;

				rs.RenderWorld.Debug.DrawBasis( item.WorldMatrix, 0.125f );
				rs.RenderWorld.Debug.DrawBox( item.Factory.BoundingBox, item.WorldMatrix, color);
			}

			//
			//	Draw selected :
			//
			foreach ( var item in selection ) {

				var color = Utils.WireColorSelected;

				if (selection.Last()!=item) {
					color = Color.White;
				}

				rs.RenderWorld.Debug.DrawBasis( item.WorldMatrix, 0.125f );
				rs.RenderWorld.Debug.DrawBox( item.Factory.BoundingBox, item.WorldMatrix, color);
			}

			var mp = Game.Mouse.Position;

			manipulator?.Update( gameTime, mp.X, mp.Y );
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



		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		void Select( int x, int y, bool add )
		{
			var ray = camera.PointToRay( x, y );

			var minDistance	=	float.MaxValue;
			var pickedItem	=	(MapNode)null;


			foreach ( var item in map.Nodes ) {
				var bbox	=	item.Factory.BoundingBox;
				var iw		=	Matrix.Invert( item.WorldMatrix );
				float distance;

				var rayT	=	Utils.TransformRay( iw, ray );

				if (rayT.Intersects(ref bbox, out distance)) {
					if (minDistance > distance) {
						minDistance = distance;
						pickedItem = item;
					}
				}
			}

			if (add) {

				if (pickedItem==null) {
					return;
				}

				if (selection.Contains(pickedItem)) {
					selection.Remove(pickedItem);
				} else {
					selection.Add(pickedItem);
				}

			} else {

				ClearSelection();

				if (pickedItem!=null) {
					selection.Add(pickedItem);
				}
			}

			FeedSelection();
		}



		bool  marqueeSelecting = false;
		bool  selectingMarqueeAdd = false;
		Point selectingMarqueeStart;
		Point selectingMarqueeEnd;

		public Rectangle SelectionMarquee {
			get { 
				if (!marqueeSelecting) {
					return new Rectangle(0,0,0,0);
				} else {
					return new Rectangle( 
						Math.Min( selectingMarqueeStart.X, selectingMarqueeEnd.X ),
						Math.Min( selectingMarqueeStart.Y, selectingMarqueeEnd.Y ),
						Math.Abs( selectingMarqueeStart.X - selectingMarqueeEnd.X ),
						Math.Abs( selectingMarqueeStart.Y - selectingMarqueeEnd.Y )
					);
				}
			}
		}


		public void StartMarqueeSelection ( int x, int y, bool add )
		{
			selectingMarqueeAdd = add;
			marqueeSelecting = true;
			selectingMarqueeStart = new Point(x,y);
			selectingMarqueeEnd	  = selectingMarqueeStart;
		}

		public void UpdateMarqueeSelection ( int x, int y )
		{
			if (marqueeSelecting) {
				selectingMarqueeEnd	  = new Point(x,y);
			}
		}

		public void StopMarqueeSelection ( int x, int y )
		{
			if (marqueeSelecting) {


				if (selectingMarqueeStart==selectingMarqueeEnd) {
					marqueeSelecting = false;
					return;
				}

				if (!selectingMarqueeAdd) {
					ClearSelection();
				}

				foreach ( var item in map.Nodes ) {
					if (camera.IsInRectangle( item.Position, SelectionMarquee )) {
						selection.Add( item );
					}
				}

				FeedSelection();

				marqueeSelecting = false;
			}
		}

	}
}
