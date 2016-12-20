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


		readonly string fullSourceFolder;
		readonly Game game;


		/// <summary>
		/// 
		/// </summary>
		public ModelEditor( Game game, string sourceFolder )
		{
			this.game	=	game;
			InitializeComponent();

			fullSourceFolder = Path.Combine( Builder.FullInputDirectory, sourceFolder );


			RefreshFileList();

			mainPropertyGrid.PropertySort = PropertySort.Categorized;

			Log.Message("Model editor initialized");
		}


		class NamePathTarget {
			public NamePathTarget ( string path ) {
				Name = System.IO.Path.GetFileNameWithoutExtension(path);
				Path = path;
			}
			public string Name { get; set; }
			public string Path { get; set; } 

			ModelDescriptor target;
			public ModelDescriptor Target {
				get {
					if (target==null) {
						target = ModelDescriptor.LoadFromXml( File.ReadAllText(Path) );
					}
					return target;
				}
			}

			public void Save()
			{
				File.WriteAllText( Path, ModelDescriptor.SaveToXml( target ) );
			}
		}



		public void RefreshFileList ()
		{
			modelListBox.DisplayMember = "Name";
			modelListBox.ValueMember = "Path";
			
			var fileList = Directory
				.EnumerateFiles( fullSourceFolder, "*.xml" )
				.Select( fn => new NamePathTarget(fn) )
				.ToList();
			
			modelListBox.DataSource	=	fileList;
		}




		public void SaveContentAndBuild ()
		{
			Log.Message( "Building..." );
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
			mainPropertyGrid.SelectedObjects = modelListBox
					.SelectedItems
					.Cast<NamePathTarget>()
					.Select( namePathTarget => namePathTarget.Target )
					.ToArray();
		}


		private void mainPropertyGrid_PropertyValueChanged( object s, PropertyValueChangedEventArgs e )
		{
			foreach ( var target in modelListBox.SelectedItems.Cast<NamePathTarget>() ) {
				target.Save();
			}
		}


		private void buttonExit_Click( object sender, EventArgs e )
		{
			Close();
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

				var fileName = Path.Combine( fullSourceFolder, name + ".xml" );

				if (File.Exists( fileName ) ) {
					var r = MessageBox.Show( this, string.Format("Model '{0}' already exists", name), "Add Model", MessageBoxButtons.OKCancel );
					if ( r==DialogResult.OK ) {
						continue;
					} else {
						return;
					}
				}


				File.WriteAllText( fileName, ModelDescriptor.SaveToXml( new ModelDescriptor() ) );

				RefreshFileList();

				return;
			}
		}

		private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Close();
		}

		
		private void saveAndRebuildToolStripMenuItem_Click( object sender, EventArgs e )
		{
			SaveContentAndBuild();
		}


		private void removeModelToolStripMenuItem_Click( object sender, EventArgs e )
		{
			var r = MessageBox.Show(this, "Are you sure to remove selected items?", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Warning );

			if (r!=DialogResult.Yes) {
				return;
			}

			var selected = modelListBox.SelectedItems.Cast<NamePathTarget>().ToArray();

			foreach ( var so in selected ) {
				File.Delete( so.Path );
			}

			RefreshFileList();
		}

	}
}
