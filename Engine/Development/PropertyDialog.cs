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
	public partial class PropertyDialog : Form {

		public static void Show ( IWin32Window owner, string caption, object targetObject )
		{
			var form = new PropertyDialog();

			form.Text = caption;
			form.mainPropertyGrid.SelectedObject = targetObject;

			var r = form.ShowDialog( owner );

			if (r==DialogResult.OK) {
				return;
			}
		}

		private PropertyDialog()
		{
			InitializeComponent();
		}

		private void buttonClose_Click( object sender, EventArgs e )
		{
			Close();
		}
	}
}
