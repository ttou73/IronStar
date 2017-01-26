using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
	public partial class ConfigEditorControl : UserControl {
		
		readonly Game game;

		class Target {
			public string Name;
			public object Value;

			public override string ToString()
			{
				return Name;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public ConfigEditorControl( Game game )
		{
			this.game		=	game;

			InitializeComponent();

			RefreshFileConfigList();

			configPropertyGrid.PropertySort = PropertySort.Categorized;

			Log.Message("Config editor initialized");
		}




		/// <summary>
		/// 
		/// </summary>
		public void RefreshFileConfigList ()
		{
			configListBox.DisplayMember = "Name";
			configListBox.ValueMember = "Value";

			configListBox.Items.AddRange( 
				game.Config
					.TargetObjects
					.OrderBy( t1 => t1.Key )
					.Select( t2 => new Target { Name = t2.Key, Value = t2.Value } )
					.ToArray() );
		}


		/*-----------------------------------------------------------------------------------------
		 * 
		 *	Event handlers :
		 * 
		-----------------------------------------------------------------------------------------*/

		private void listBox1_SelectedIndexChanged( object sender, EventArgs e )
		{
			configPropertyGrid.SelectedObjects = configListBox
					.SelectedItems
					.Cast<Target>()
					.Select( namePathTarget => namePathTarget.Value )
					.ToArray();
		}


		private void mainPropertyGrid_PropertyValueChanged( object s, PropertyValueChangedEventArgs e )
		{
			/*foreach ( var target in configListBox.SelectedItems.Cast<NamePathTarget>() ) {
				target.SaveTarget();
			} */
		}

		private void newToolStripMenuItem_Click( object sender, EventArgs e )
		{
			game.Config.Save("Config.ini");
		}

		private void deleteToolStripMenuItem_Click( object sender, EventArgs e )
		{
			game.Config.Load("Config.ini");
		}
	}
}
