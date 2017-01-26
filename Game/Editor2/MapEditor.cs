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
using Fusion.Build;
using BEPUphysics;

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

		readonly Stack<MapFactory[]> selectionStack = new Stack<MapFactory[]>();
		readonly List<MapFactory> selection = new List<MapFactory>();

		Space physSpace;

		Map	map = null;

		public Map Map {
			get {
				return map;
			}
		}

		public readonly EditorConfig Config;


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

			SetupUI();

			Game.Keyboard.ScanKeyboard =	true;

			fullPath	=	Builder.GetFullPath(@"maps\" + map + ".map");

			physSpace	=	new Space();
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
		}



		public void Refresh ()
		{
			var form = Editors.Editor.GetMapEditor();
			if (form==null) {
				return;
			}

			form.SetSelection( GetSelection() );
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
		/// <param name="disposing"></param>
		protected virtual void Dispose( bool disposing )
		{
			if ( !disposedValue ) {
				if ( disposing ) {
					hud?.Dispose();
					hud = null;
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

		public IEnumerable<MapFactory> Selection {
			get { return selection; }
		}

		public MapFactory[] GetSelection() 
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

			rs.RenderWorld.Debug.DrawGrid( 10 );

			hud.Update(gameTime);

			//
			//	Draw unselected :
			//
			foreach ( var item in map.Factories ) {

				var color = new Color(0,4,96);

				rs.RenderWorld.Debug.DrawBasis( item.Transform.World, 0.125f );
				rs.RenderWorld.Debug.DrawBox( item.Factory.BoundingBox, item.Transform.World, color);
			}

			//
			//	Draw selected :
			//
			foreach ( var item in selection ) {

				var color = new Color(67,255,163);

				if (selection.Last()!=item) {
					color = Color.White;
				}

				rs.RenderWorld.Debug.DrawBasis( item.Transform.World, 0.125f );
				rs.RenderWorld.Debug.DrawBox( item.Factory.BoundingBox, item.Transform.World, color);

				rs.RenderWorld.Debug.DrawRing( Matrix.Identity, 1, Color.Blue, 1, 16 );
			}

			var mp = Game.Mouse.Position;

			manipulator?.Update( gameTime, mp.X, mp.Y );
		}


		Ray TransformRay ( Matrix m, Ray r )
		{
			return new Ray( Vector3.TransformCoordinate( r.Position, m ), Vector3.TransformNormal( r.Direction, m ).Normalized() );
		}


		void Focus ()
		{
			var targets = selection.Any() ? selection.ToArray() : map.Factories.ToArray();

			BoundingBox bbox;

			if (!targets.Any()) {
				bbox = new BoundingBox( new Vector3(-10,-10,-10), new Vector3(10,10,10) );
			} else {

				bbox = BoundingBox.FromPoints( targets.Select( t => t.Transform.Translation ).ToArray() );

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
			var pickedItem	=	(MapFactory)null;


			foreach ( var item in map.Factories ) {
				var bbox	=	item.Factory.BoundingBox;
				var iw		=	Matrix.Invert( item.Transform.World );
				float distance;

				var rayT	=	TransformRay( iw, ray );

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

			//PushSelection();
		}

	}
}
