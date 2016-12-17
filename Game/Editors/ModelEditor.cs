using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fusion;
using Fusion.Build;
using Fusion.Engine.Common;
using IronStar.SFX;
using Fusion.Development;

namespace IronStar.Development {
	public partial class ModelEditor : Form {


		BindingList<ModelDescriptor> modelDataSource;
		readonly string fullPath;
		readonly Game game;
		bool modified = false;


		/// <summary>
		/// 
		/// </summary>
		public ModelEditor( Game game, string modelsXml )
		{
			this.game	=	game;
			InitializeComponent();

			fullPath = Path.Combine( Builder.FullInputDirectory, modelsXml );

			if (!File.Exists(fullPath)) {
				modelDataSource = new BindingList<ModelDescriptor>();
				modified = true;
			} else {
				var list = ModelDescriptor.LoadCollectionFromXml( File.ReadAllText(fullPath) ).ToList();
				modelDataSource = new BindingList<ModelDescriptor>(list);
			}

			modelListBox.DataSource = modelDataSource;
			modelListBox.DisplayMember = "Name";
			modelListBox.ValueMember = "Name";

			mainPropertyGrid.PropertySort = PropertySort.Categorized;

			Log.Message("Model editor initialized");
		}




		public void SaveContent ()
		{
			File.WriteAllText( fullPath, ModelDescriptor.SaveCollectionToXml( modelDataSource ) );
			modified = false;
			Log.Message("Content saved");
		}



		public void SaveContentAndBuild ()
		{
			SaveContent();
			Log.Message( "Content saved. Building..." );
			Builder.SafeBuild();
			game.Reload();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listBox1_SelectedIndexChanged( object sender, EventArgs e )
		{
			mainPropertyGrid.SelectedObjects = modelListBox.SelectedItems.Cast<object>().ToArray();
		}

		private void buttonExit_Click( object sender, EventArgs e )
		{
			Close();
		}

		private void buttonSave_Click( object sender, EventArgs e )
		{
			SaveContent();
		}

		private void buttonSaveAndBuild_Click( object sender, EventArgs e )
		{
			SaveContentAndBuild();
		}


		private void addModelToolStripMenuItem_Click( object sender, EventArgs e )
		{
			string name = "";

			while (true) {
				name = NameDialog.Show( this, "Create new model:", "Add Model", name);

				if (name==null) {
					return;
				}

				if (modelDataSource.Any( m => m.Name==name )) {
					var r = MessageBox.Show( this, string.Format("Model '{0}' already exists", name), "Add Model", MessageBoxButtons.OKCancel );
					if (r==DialogResult.OK) {
						continue;
					} else {
						return;
					}
				}

				modelDataSource.Add( new ModelDescriptor() { Name = name } );
				Log.Message("Model added: '{0}'", name);
				modified = true;

				return;
			}
		}

		private void renameModelToolStripMenuItem_Click( object sender, EventArgs e )
		{
			var objectToRename = modelListBox.SelectedItem as ModelDescriptor;

			if (objectToRename==null) {
				Log.Message("Nothing is selected");
				return;
			}

			string name = objectToRename.Name;

			while ( true ) {
				name = NameDialog.Show( this, "Rename model:", "Rename Model", name );

				if ( name==null ) {
					return;
				}

				if ( modelDataSource.Any( m => m.Name==name ) ) {
					var r = MessageBox.Show( this, string.Format("Model '{0}' already exists", name), "Add Model", MessageBoxButtons.OKCancel );
					if ( r==DialogResult.OK ) {
						continue;
					} else {
						return;
					}
				}

				objectToRename.Name = name;
				Log.Message( "Model renamed: '{0}'", name );
				modified = true;
				modelDataSource.ResetBindings();

				return;
			}
		}

		private void ModelEditor_FormClosing( object sender, FormClosingEventArgs e )
		{
			if (modified) {
				var r = MessageBox.Show(this, "Unsaved changes. Save?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
				if (r==DialogResult.Yes) {
					SaveContent();
				}
			}
		}

		private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Close();
		}

		private void saveToolStripMenuItem_Click( object sender, EventArgs e )
		{
			SaveContent();
		}

		private void saveAndRebuildToolStripMenuItem_Click( object sender, EventArgs e )
		{
			SaveContentAndBuild();
		}

		private void removeModelToolStripMenuItem_Click( object sender, EventArgs e )
		{
			var r = MessageBox.Show(this, "Are you sure to remove models?", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Warning );

			if (r!=DialogResult.Yes) {
				return;
			}

			var selected = modelListBox.SelectedItems.Cast<ModelDescriptor>().ToArray();

			foreach ( var so in selected ) {
				modelDataSource.Remove( so );
			}
		}

		private void mainPropertyGrid_SelectedObjectsChanged( object sender, EventArgs e )
		{
		}

		private void mainPropertyGrid_PropertyValueChanged( object s, PropertyValueChangedEventArgs e )
		{
			modified = true;
		}
	}
}
