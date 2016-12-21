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
using Fusion.Core.Extensions;

namespace IronStar.Editors {
	public partial class ObjectEditor : UserControl {
		
	
		class NamePathTarget {

			public string Name { get; set; }
			public string Path { get; set; } 

			readonly Type baseType;
			readonly Type[] extraTypes;

			public NamePathTarget ( string path, Type baseType, Type[] extraTypes ) 
			{
				this.baseType	=	baseType;
				this.extraTypes	=	extraTypes;
				Name = System.IO.Path.GetFileNameWithoutExtension(path);
				Path = path;
			}


			object target;
			public object Target {
				get {
					if (target==null) {
						LoadTarget();
					}
					return target;
				}
			}


			private object LoadTarget ()
			{
				target = Misc.LoadObjectFromXml( baseType, File.ReadAllText(Path), extraTypes ); 
				return target;
			}


			public void SaveTarget ()
			{
				File.WriteAllText( Path, Misc.SaveObjectToXml( target, baseType, extraTypes ) );
			}
		}



		readonly string fullSourceFolder;
		readonly Game game;
		readonly Type baseType;
		readonly Type[] extraTypes;



		/// <summary>
		/// 
		/// </summary>
		public ObjectEditor( Game game, string sourceFolder, Type baseTargetType, Type[] extraTargetTypes )
		{
			this.baseType	=	baseTargetType;
			this.extraTypes	=	extraTargetTypes;
			this.game		=	game;
			InitializeComponent();

			fullSourceFolder = Path.Combine( Builder.FullInputDirectory, sourceFolder );


			RefreshFileList();

			objectPropertyGrid.PropertySort = PropertySort.Categorized;

			Log.Message("Object editor initialized");
		}



		/// <summary>
		/// 
		/// </summary>
		public void RefreshFileList ()
		{
			objectListBox.DisplayMember = "Name";
			objectListBox.ValueMember = "Path";
			
			var fileList = Directory
				.EnumerateFiles( fullSourceFolder, "*.xml" )
				.Select( fn => new NamePathTarget(fn, baseType, extraTypes) )
				.ToList();
			
			objectListBox.DataSource	=	fileList;
		}



		/// <summary>
		/// Runs create new model procedure.
		/// </summary>
		public void AddNewObjectUI ()
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



		/// <summary>
		/// Runs object remove procedure.
		/// </summary>
		public void RemoveObjectUI ()
		{
			var r = MessageBox.Show(this, "Are you sure to remove selected items?", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Warning );

			if (r!=DialogResult.Yes) {
				return;
			}

			var selected = objectListBox.SelectedItems.Cast<NamePathTarget>().ToArray();

			foreach ( var so in selected ) {
				File.Delete( so.Path );
			}

			RefreshFileList();
		}


		/*-----------------------------------------------------------------------------------------
		 * 
		 *	Event handlers :
		 * 
		-----------------------------------------------------------------------------------------*/

		private void listBox1_SelectedIndexChanged( object sender, EventArgs e )
		{
			objectPropertyGrid.SelectedObjects = objectListBox
					.SelectedItems
					.Cast<NamePathTarget>()
					.Select( namePathTarget => namePathTarget.Target )
					.ToArray();
		}


		private void mainPropertyGrid_PropertyValueChanged( object s, PropertyValueChangedEventArgs e )
		{
			foreach ( var target in objectListBox.SelectedItems.Cast<NamePathTarget>() ) {
				target.SaveTarget();
			}
		}
	}
}
