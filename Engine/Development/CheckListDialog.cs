using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fusion.Development {
	public partial class CheckListDialog : Form {

		public static List<string> ShowDialog ( IWin32Window owner, string message, string caption, IEnumerable<string> list )
		{
			var form = new CheckListDialog(message, caption, list);
			var r = form.ShowDialog(owner);

			if (r==DialogResult.OK) {
				return list.ToList();
			} else {
				return null;
			}
		}


		private List<string> result = null;


		private CheckListDialog( string message, string caption, IEnumerable<string> list )
		{
			InitializeComponent();

			Text = caption;

			listView.Items.Clear();

			label1.Text = message;

			foreach ( var name in list ) {
				listView.Items.Add( name );
			}
		}

		private void buttonCancel_Click( object sender, EventArgs e )
		{
			DialogResult    =   DialogResult.Cancel;
			Close();
		}

		private void buttonOK_Click( object sender, EventArgs e )
		{
			result = new List<string>();

			foreach ( var item in listView.CheckedItems.Cast<ListViewItem>() ) {
				result.Add( item.Text );
			}

			DialogResult    =   DialogResult.OK;
			Close();
		}
	}
}
