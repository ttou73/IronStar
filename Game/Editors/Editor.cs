using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion;
using Fusion.Core.Shell;
using System.Windows.Forms;
using IronStar.Editors;
using Fusion.Engine.Common;
using IronStar.SFX;

namespace IronStar.Editors {

	public static class Editor {
		
		public static void Run ( Game game )
		{
			var editorForm =	Application.OpenForms.Cast<Form>().FirstOrDefault( form => form is EditorForm );

			if (editorForm==null) {
				editorForm = new EditorForm(game);
			}

			editorForm.Show();
			editorForm.Activate();
			editorForm.BringToFront();
		}


		public static void CloseAll ()
		{
			var editorForm =    Application.OpenForms.Cast<Form>().FirstOrDefault( form => form is EditorForm );
			editorForm?.Close();
		}

	}



	[Command( "editor", CommandAffinity.Default )]
	public class EditorCommand : NoRollbackCommand {

		public EditorCommand( Invoker invoker ) : base(invoker) 
		{
		}

		public override void Execute()
		{
			Editor.Run( Invoker.Game );
		}
	}
}
