using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronStar.Mapping;
using Fusion;
using Fusion.Core.Extensions;
using System.IO;
using Fusion.Build;
using Native.Fbx;
using System.Reflection;

namespace IronStar.Editors {
	public partial class MapEditor : UserControl {

		
		class MapFileState {
			public bool Modified = true;
			public string Path = null;
			public Map Map;
		}

		MapFileState mapFile = null;
		MapFileState MapFile { 
			get {
				return mapFile;
			}
			set {
				mapFile = value;
				if (mapFile==null) {
					saveToolStripMenuItem.Enabled	= false;
					saveAsToolStripMenuItem.Enabled = false;
					editToolStripMenuItem.Enabled	= false;
				} else {
					saveToolStripMenuItem.Enabled	= true;
					saveAsToolStripMenuItem.Enabled = true;
					editToolStripMenuItem.Enabled	= true;
				}
			}
		}


		HashSet<Type> visibleTypes = new HashSet<Type>();
		List<ToolStripMenuItem> showMenuItems = new List<ToolStripMenuItem>();
		Type[] types;


		public MapEditor()
		{
			InitializeComponent();
			MapFile = null;
			types = Misc.GetAllSubclassesOf( typeof(IronStar.Core.EntityFactory) ).OrderBy( t => t.Name ).ToArray();

			foreach ( var t in types ) {
				visibleTypes.Add(t);
			}

			foreach ( var type in types ) {
				var item    =   new ToolStripMenuItem();
				item.Text   =   "" + type.Name.Replace( "Factory", "" );
				item.Click  +=  ( s, e ) => {
					SetFactory( type );
					PopulatePropertyGrid();
					RefreshMapListItems();
				};

				editToolStripMenuItem.DropDownItems.Add( item );
			}



			var showAll     =   new ToolStripMenuItem();
			showAll.Text    =   "Show All";
			showAll.Click += ( s, e ) => {
				foreach ( var t in types ) {
					visibleTypes.Add( t );
				}
				foreach ( var mi in showMenuItems ) {
					mi.CheckState = CheckState.Checked;
				}
				UpdateMapList();
			};
			showToolStripMenuItem.DropDownItems.Add( showAll );

			var hideAll     =   new ToolStripMenuItem();
			hideAll.Text    =   "Hide All";
			hideAll.Click += ( s, e ) => {
				visibleTypes.Clear();
				foreach ( var mi in showMenuItems ) {
					mi.CheckState = CheckState.Unchecked;
				}
				UpdateMapList();
			};
			showToolStripMenuItem.DropDownItems.Add( hideAll );

			var splitLine = new ToolStripSeparator();
			showToolStripMenuItem.DropDownItems.Add( splitLine );


			foreach ( var type in types ) {
				var item		=   new ToolStripMenuItem();
				item.Checked	=	true;
				item.CheckState	=	CheckState.Checked;
				item.Text		=   "Show " + type.Name.Replace( "Factory", "" );
				item.Click		+=  ( s, e ) => {
					if (item.CheckState==CheckState.Checked) {
						item.CheckState = CheckState.Unchecked;
						visibleTypes.Remove( type );
					} else {
						item.CheckState = CheckState.Checked;
						visibleTypes.Add( type );
					}
					UpdateMapList();
				};

				showMenuItems.Add( item );

				showToolStripMenuItem.DropDownItems.Add( item );
			}
		}


		void SetFactory ( Type type )
		{
			MapFile.Modified = true;

			var targets = mapListBox.SelectedItems.Cast<MapFactory>();

			foreach ( var target in targets ) {
				target.Factory = (Core.EntityFactory)Activator.CreateInstance( type );
			}
			Log.Message("{0} nodes were assigned to {1}", targets.Count(), type.Name );
		}



		void RefreshMapListItems ()
		{
			mapListBox.RefreshListBox();
		}


		void UpdateMapList ()
		{
			if (MapFile==null) {
				mapListBox.Items.Clear();
			} else {
				mapListBox.Items.Clear();
				mapListBox.Items.AddRange( MapFile.Map.Factories.Where( n => visibleTypes.Contains(n.Factory.GetType()) ).ToArray() );
			}
		}



		void NewMap ()
		{
			if ( MapFile!=null ) {
				if ( MapFile.Modified ) {
					SaveMap(false);
				}
			}

			string fullPath;

			if (Editor.OpenFileDialog("Select Scene to Import Nodes", "FBX Scene (*.fbx)|*.fbx", "Scenes", false, out fullPath )) {

				MapFile =   new MapFileState() { Map = new Map(), Path = null };
				MapFile.Map.ScenePath = Builder.GetRelativePath( fullPath );
				ImportSceneNodes();

				UpdateMapList();
			}
		}


		public void SaveMap( bool saveAs )
		{
			if ( MapFile==null) {
				Log.Message("Map Editor : Map is not created");
				return;
			}

			if ( !saveAs ) {
				if ( !MapFile.Modified ) {	
					Log.Message("Map Editor : Nothing to save");
					return;
				}
			}

			if ( saveAs || string.IsNullOrWhiteSpace(mapFile.Path) ) {
				string fileName;
				if (Editor.SaveFileDialog("IronStar Map File (*.map)|*.map", "Maps", out fileName )) {
					MapFile.Path = fileName;
				} else {
					Log.Message( "Map Editor : Saving canceled" );
					return;
				}
			}

			File.WriteAllText( MapFile.Path, Misc.SaveObjectToXml( MapFile.Map, typeof(Map), types ) );
		}


		public void OpenMap()
		{
			if ( MapFile!=null ) {
				if ( MapFile.Modified ) {
					SaveMap( false );
				}
			}

			string fileName;
			if ( Editor.OpenFileDialog( "IronStar Map File (*.map)|*.map", "Maps", false, out fileName ) ) {
			} else {
				Log.Message( "Map Editor : Open canceled" );
				return;
			}


			var map = (Map)Misc.LoadObjectFromXml( typeof(Map), File.ReadAllText(fileName), types );

			MapFile =   new MapFileState() { Map = map, Path = fileName, Modified = false };

			UpdateMapList();
		}


		public void CloseMap ()
		{
			if ( MapFile!=null ) {
				if (MapFile.Modified) {
					var r = MessageBox.Show(this, "Unsaved changes. Save?", "Map Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
					if (r==DialogResult.Yes) {
						SaveMap(false);
					} else {
					}
				}
				MapFile = null;
			}
			UpdateMapList();
		}



		public void ImportSceneNodes ()
		{
			var map = MapFile.Map;
			var loader = new FbxLoader();
			var options = new Options();
			var scene = loader.LoadScene( Builder.GetFullPath( map.ScenePath ), new Options() );

			var hashset = new HashSet<string>( scene.Nodes.Select( n => scene.GetFullNodePath(n) ) );

			if ( map.Factories==null ) {
				map.Factories = new List<MapFactory>();
			}

			//	detect non-existing nodes :
			var nonexisting = map.Factories
						.Where( n => hashset.Contains( n.NodePath ) )
						.ToArray();


			var newNodes = new List<string>();


			//	add new nodes :
			hashset = new HashSet<string>( map.Factories.Select( n => n.NodePath ) );

			foreach ( var node in scene.Nodes ) {


				var nodePath    =   scene.GetFullNodePath(node);

				Core.EntityFactory factory = null;
				
				factory     =   new Entities.StaticModelFactory();

				if ( node.ParentIndex<0 ) {
					factory		=	new Entities.WorldspawnFactory();
				}

				if ( hashset.Contains( nodePath ) ) {
					continue;
				}

				newNodes.Add( nodePath );

				map.Factories.Add( new MapFactory() { NodePath = nodePath, Factory = factory } );
			}

			Log.Message( "Scene hierarchy import completed:" );
			Log.Message( "  {0} missing links", nonexisting.Length );
			Log.Message( "  {0} new nodes", newNodes.Count );
		}



		public void AddNode ( MapFactory node )
		{
			throw new NotImplementedException();
			/*if ( MapFile!=null ) {
				MapFile.Modified  = true;
				MapFile.Map.Elements.Add( node );
				bs.ResetBindings(false);
			} */
		}


		void PopulatePropertyGrid ()
		{
			mapPropertyGrid.SelectedObjects = mapListBox.SelectedItems.Cast<MapFactory>().Select( n => n.Factory ).ToArray();
		}



		private void newMapToolStripMenuItem_Click( object sender, EventArgs e )
		{
			NewMap();
		}

		private void saveToolStripMenuItem_Click( object sender, EventArgs e )
		{
			SaveMap(false);
		}

		private void saveAsToolStripMenuItem_Click( object sender, EventArgs e )
		{
			SaveMap(true);
		}

		private void openToolStripMenuItem_Click( object sender, EventArgs e )
		{
			OpenMap();
		}

		private void closeToolStripMenuItem_Click( object sender, EventArgs e )
		{
			CloseMap();
		}

		private void addMapModelToolStripMenuItem_Click( object sender, EventArgs e )
		{
			AddNode( null );
		}

		private void addMapEntityToolStripMenuItem_Click( object sender, EventArgs e )
		{
			AddNode( null );
		}

		private void mapListBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			PopulatePropertyGrid();
		}

		private void mapPropertyGrid_PropertyValueChanged( object s, PropertyValueChangedEventArgs e )
		{
			MapFile.Modified = true;
		}

		private void updateNodesFromSceneToolStripMenuItem_Click( object sender, EventArgs e )
		{
			ImportSceneNodes();
			UpdateMapList();
		}

		private void removeToolStripMenuItem_Click( object sender, EventArgs e )
		{
			var names = string.Join("\r\n\t", mapListBox.SelectedItems.Cast<MapFactory>().Select( n => n.NodePath ) );

			var r = MessageBox.Show(this, "Are you sure to remove the selected nodes:\r\n\t" + names, "Remove Nodes", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning );

			if (r==DialogResult.Cancel) {
				return;
			}

			var selected = mapListBox.SelectedItems.Cast<MapFactory>().ToArray();

			foreach (var mapNode in selected) {
				MapFile.Map.Factories.Remove( mapNode );
				MapFile.Modified = true;
			}
			UpdateMapList();
		}
	}
}
