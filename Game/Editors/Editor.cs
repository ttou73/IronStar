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
using Fusion.Core.Content;
using Fusion.Build;
using System.IO;

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


		public static bool OpenFileDialog( string filter, string dir, bool getRelativePath, out string fileName )		{
			return OpenFileDialog( "Open", filter, dir, getRelativePath, out fileName );
		}


		public static bool OpenFileDialog( string caption, string filter, string dir, bool getRelativePath, out string fileName )
		{
			fileName = null;

			using ( OpenFileDialog ofd = new OpenFileDialog() ) {

				ofd.InitialDirectory    =	Path.Combine(Builder.FullInputDirectory, dir);
				ofd.RestoreDirectory    =   true;
				ofd.Filter              =   filter;
				ofd.Title				=	caption;

				// set file filter info here
				if ( ofd.ShowDialog() == DialogResult.OK ) {

					fileName = ofd.FileName;

					if ( Path.IsPathRooted( fileName ) && getRelativePath ) {
						fileName = ContentUtils.MakeRelativePath( Builder.FullInputDirectory + @"\", fileName );
					}

					return true;
				} else {
					return false;
				}
			}
		}


		public static bool SaveFileDialog( string filter, string dir, out string fileName )
		{
			fileName = null;

			using ( SaveFileDialog sfd = new SaveFileDialog() ) {

				sfd.InitialDirectory    =   Path.Combine(Builder.FullInputDirectory, dir);
				sfd.RestoreDirectory    =   true;
				sfd.Filter              =   filter;

				// set file filter info here
				if ( sfd.ShowDialog() == DialogResult.OK ) {

					fileName = sfd.FileName;

					return true;
				} else {
					return false;
				}
			}
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
