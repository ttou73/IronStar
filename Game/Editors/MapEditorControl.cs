﻿using System;
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
using Fusion.Engine.Common;
using IronStar.Core;
using IronStar.Editor2;
using Fusion.Development;

namespace IronStar.Editors {
	public partial class MapEditorControl : UserControl {

		readonly Game Game;

		public MapEditorControl( Game game )
		{
			this.Game	=	game;
			InitializeComponent();

			PopulateCreateMenu();
		}


		MapEditor MapEditor {
			get {
				return Game.GameEditor.Instance as MapEditor;
			}
		}


		void PopulateCreateMenu()
		{
			createToolStripMenuItem.Enabled = true;

			var types = Misc.GetAllSubclassesOf( typeof(EntityFactory), false );



			foreach ( var factType in types ) {

				var item    =   new ToolStripMenuItem();

				item.Text   =   factType.Name;

				item.Click  +=  ( s, e ) => {

					var fact  = new MapEntity();

					var mapEditor = Game.GameEditor.Instance as MapEditor;
	
					fact.Factory = (EntityFactory)Activator.CreateInstance( factType );
					
					mapEditor.Map.Nodes.Add( fact );
					mapEditor.Select( fact );
				};

				createToolStripMenuItem.DropDownItems.Add( item );
			}

		}



		public void SetSelection ( IEnumerable<MapNode> selection, object env )
		{
			gridTransform.SelectedObjects	= selection
					.Select( node => node ).ToArray();	

			gridFactory.SelectedObjects		= selection
					.Where( node => node is MapEntity )
					.Select( node1 => (node1 as MapEntity).Factory ).ToArray();	

			gridEnv.SelectedObject			= env;
		}


		private void editRecastConfigurationToolStripMenuItem_Click( object sender, EventArgs e )
		{
			PropertyDialog.Show( this, "Recast Configuration", MapEditor.Map.NavConfig );
		}

		private void navigationMeshToolStripMenuItem_Click( object sender, EventArgs e )
		{
			MapEditor.Map.BuildNavigationMesh( MapEditor.Content );
		}

		private void refreshWorldToolStripMenuItem_Click( object sender, EventArgs e )
		{
			MapEditor.ResetWorld(true);
		}

		private void gridTransform_PropertyValueChanged( object s, PropertyValueChangedEventArgs e )
		{
			MapEditor.ResetWorld(true);
		}

		private void gridFactory_PropertyValueChanged( object s, PropertyValueChangedEventArgs e )
		{
			MapEditor.ResetWorld(true);
		}

		private void gridEnv_PropertyValueChanged( object s, PropertyValueChangedEventArgs e )
		{
			MapEditor.Map.UpdateEnvironment( MapEditor.World );
		}

		private void toolStripMenuItem1_Click( object sender, EventArgs e )
		{
		}

		private void freezeSelectionToolStripMenuItem_Click( object sender, EventArgs e )
		{
			MapEditor.FreezeSelected();
		}

		private void unfreezeAllToolStripMenuItem_Click( object sender, EventArgs e )
		{
			MapEditor.UnfreezeAll();
		}

		private void saveToolStripMenuItem_Click( object sender, EventArgs e )
		{
			MapEditor.SaveMap();
		}

		private void decalToolStripMenuItem_Click( object sender, EventArgs e )
		{
			var node = new MapDecal();
			node.SpawnNode( MapEditor.World );
			MapEditor.Map.Nodes.Add( node );
			MapEditor.Select( node );
		}

		private void omniLightToolStripMenuItem_Click( object sender, EventArgs e )
		{
			var node = new MapOmniLight();
			node.SpawnNode( MapEditor.World );
			MapEditor.Map.Nodes.Add( node );
			MapEditor.Select( node );
		}

		private void spotLightToolStripMenuItem_Click( object sender, EventArgs e )
		{
			var node = new MapSpotLight();
			node.SpawnNode( MapEditor.World );
			MapEditor.Map.Nodes.Add( node );
			MapEditor.Select( node );
		}

		private void modelToolStripMenuItem_Click( object sender, EventArgs e )
		{
			var node = new MapModel();
			node.SpawnNode( MapEditor.World );
			MapEditor.Map.Nodes.Add( node );
			MapEditor.Select( node );
		}

		private void lightProbeToolStripMenuItem_Click( object sender, EventArgs e )
		{
			var node = new MapLightProbe();
			node.SpawnNode( MapEditor.World );
			MapEditor.Map.Nodes.Add( node );
			MapEditor.Select( node );
		}
	}
}
