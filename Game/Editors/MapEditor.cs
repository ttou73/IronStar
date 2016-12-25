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

namespace IronStar.Editors {
	public partial class MapEditor : UserControl {

		
		class MapFileState {
			public bool Modified = true;
			public string Path = null;
			public Map Map;
		}

		BindingSource bs;


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


		public MapEditor()
		{
			InitializeComponent();
			MapFile = null;
		}



		void UpdateList ()
		{
			bs	=	new BindingSource();
			bs.DataSource = MapFile.Map.Elements;

			mapListBox.DataSource = bs;
			bs.ResetBindings(false);
		}



		void New ()
		{
			if ( MapFile!=null ) {
				if ( MapFile.Modified ) {
					Save(false);
				}
			}

			MapFile =   new MapFileState() { Map = new Map(), Path = null };
			MapFile.Map.Elements.Add( new MapModel() );
			MapFile.Map.Elements.Add( new MapModel() );
			MapFile.Map.Elements.Add( new MapModel() );
			MapFile.Map.Elements.Add( new MapModel() );
			MapFile.Map.Elements.Add( new MapModel() );
			MapFile.Map.Elements.Add( new MapEntity() );
			MapFile.Map.Elements.Add( new MapEntity() );
			MapFile.Map.Elements.Add( new MapEntity() );
			MapFile.Map.Elements.Add( new MapEntity() );
			MapFile.Map.Elements.Add( new MapEntity() );
			MapFile.Map.Elements.Add( new MapEntity() );

			UpdateList();
		}


		public void Save( bool saveAs )
		{
			if ( MapFile==null) {
				Log.Warning("Map Editor : Map is not created");
				return;
			}

			if ( !MapFile.Modified ) {	
				Log.Message("Map Editor : Nothing to save");
				return;
			}

			if ( saveAs || string.IsNullOrWhiteSpace(mapFile.Path) ) {
				string fileName;
				if (Editor.SaveFileDialog("IronStar Map File (*.map)|*.map", out fileName )) {
					MapFile.Path = fileName;
				} else {
					Log.Message( "Map Editor : Saving canceled" );
					return;
				}
			}

			File.WriteAllText( MapFile.Path, Misc.SaveObjectToXml( MapFile.Map, typeof(Map) ) );
		}


		public void Close ()
		{
			if ( MapFile!=null ) {
				if (MapFile.Modified) {
					var r = MessageBox.Show(this, "Unsaved changes. Save?", "Map Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
					if (r==DialogResult.Yes) {
						Save(false);
					} else {
					}
				}
				MapFile = null;
			}
			UpdateList();
		}



		public void AddNode ( MapNode node )
		{
			if ( MapFile!=null ) {
				MapFile.Modified  = true;
				MapFile.Map.Elements.Add( node );
				bs.ResetBindings(false);
			}
		}



		private void newMapToolStripMenuItem_Click( object sender, EventArgs e )
		{
			New();
		}

		private void mapTreeView_AfterSelect( object sender, TreeViewEventArgs e )
		{
			mapPropertyGrid.SelectedObject = e.Node.Tag;
		}

		private void saveToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Save(false);
		}

		private void saveAsToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Save(true);
		}

		private void closeToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Close();
		}

		private void addMapModelToolStripMenuItem_Click( object sender, EventArgs e )
		{
			AddNode( new MapModel() );
		}

		private void addMapEntityToolStripMenuItem_Click( object sender, EventArgs e )
		{
			AddNode( new MapEntity() );
		}

		private void mapListBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			mapPropertyGrid.SelectedObjects = mapListBox.SelectedItems.Cast<object>().ToArray();
		}

		private void mapPropertyGrid_PropertyValueChanged( object s, PropertyValueChangedEventArgs e )
		{
			MapFile.Modified = true;
			bs.ResetBindings(false);
		}
	}
}
