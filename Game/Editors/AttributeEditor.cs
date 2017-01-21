using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fusion.Engine.Common;

namespace IronStar.Editors {
	public partial class AttributeEditor : Form {

		public static void ShowEditor (Game game)
		{
			var form =	Application.OpenForms.Cast<Form>().FirstOrDefault( f => f is AttributeEditor );

			if (form==null) {
				form = new AttributeEditor(game);
			}

			form.Show();
			form.Activate();
			form.BringToFront();
		}


		public static void CloseEditor()
		{
			var form =    Application.OpenForms.Cast<Form>().FirstOrDefault( f => f is AttributeEditor );
			form?.Close();
		}


		public AttributeEditor( Game game )
		{
			InitializeComponent();
		}
	}
}
