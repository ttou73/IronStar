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

		public EdCamera			edCamera;
		public EdManipulator	edManipulator;

		Space physSpace;

		Map	map = null;

		public Map Map {
			get {
				return map;
			}
		}


		public MapFactory[] GetSelection() 
		{
			return map.Factories.Where( f => f.Selected ).ToArray();
		}



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

			edCamera		=	new EdCamera( Game.RenderSystem, this );
			edManipulator	=	new EdManipulator( Game.RenderSystem, this );
			edManipulator.Mode	=	ManipulatorMode.TranslationGlobal;

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
				Log.Message("Saving map: {0}", fullPath);
				if ( disposing ) {
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



		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		void IEditorInstance.Update( GameTime gameTime )
		{
			edCamera.Update( gameTime );

			rs.RenderWorld.Debug.DrawGrid( 10 );

			//
			//	Draw unselected :
			//
			foreach ( var item in map.Factories ) {

				if (item.Selected) {
					continue;
				}

				var color = Color.DimGray;

				rs.RenderWorld.Debug.DrawBasis( item.Transform.World, 0.125f );
				rs.RenderWorld.Debug.DrawBox( item.Factory.BoundingBox, item.Transform.World, color);
			}

			//
			//	Draw selected :
			//
			foreach ( var item in map.Factories ) {

				if (!item.Selected) {
					continue;
				}

				var color = new Color(67,255,163);

				rs.RenderWorld.Debug.DrawBasis( item.Transform.World, 0.125f );
				rs.RenderWorld.Debug.DrawBox( item.Factory.BoundingBox, item.Transform.World, color);

				rs.RenderWorld.Debug.DrawRing( Matrix.Identity, 1, Color.Blue, 1, 16 );
			}


			edManipulator.Update( gameTime );
			//rs.
			//SimulateWorld( gameTime.ElapsedSec );

			//modelManager.Update( gameTime.ElapsedSec, 1 );
		}


		Ray TransformRay ( Matrix m, Ray r )
		{
			return new Ray( Vector3.TransformCoordinate( r.Position, m ), Vector3.TransformNormal( r.Direction, m ).Normalized() );
		}


		void Focus ()
		{
			var	selected = map.Factories.Where( f1 => f1.Selected ).ToArray();

			if (!selected.Any()) {
				selected = map.Factories.ToArray();
			}

			//BoundingBox.Merge(
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		void Select( int x, int y, bool add )
		{
			var ray = edCamera.PointToRay( x, y );

			var minDistance		=	float.MaxValue;
			var selectedItem	=	(MapFactory)null;


			foreach ( var item in map.Factories ) {
				var bbox	=	item.Factory.BoundingBox;
				var iw		=	Matrix.Invert( item.Transform.World );
				float distance;

				var rayT	=	TransformRay( iw, ray );

				if (rayT.Intersects(ref bbox, out distance)) {
					if (minDistance > distance) {
						minDistance = distance;
						selectedItem = item;
					}
				}
			}

			if (add) {
				if (selectedItem!=null) {
					selectedItem.Selected = !selectedItem.Selected;
				}
			} else {
				foreach ( var item in map.Factories ) {
					item.Selected = false;
				}
				if (selectedItem!=null) {
					selectedItem.Selected = true;
				}
			}

			edManipulator.Target = selectedItem;
		}

	}
}
