using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion;
using Fusion.Core.Shell;
using System.Windows.Forms;
using IronStar.Development;
using Fusion.Engine.Common;

namespace IronStar.Editors {

	public enum Editors {
		Model,
		FX,
		Entity,
		Megatexture,
		Maps,
	}



	public static class Editor {
		
		public static void Run ( Game game, Editors editor )
		{
			var editorForm =	Application.OpenForms.Cast<Form>().FirstOrDefault( form => form is ModelEditor );

			if (editorForm==null) {
				editorForm = new ModelEditor(game, @"models");
			}

			editorForm.Show();
			editorForm.Activate();
			editorForm.BringToFront();
		}


		public static void CloseAll ()
		{
			var editorForm =    Application.OpenForms.Cast<Form>().FirstOrDefault( form => form is ModelEditor );
			editorForm?.Close();
		}

	}



	[Command( "edit", CommandAffinity.Default )]
	public class EditorCommand : NoRollbackCommand {

		[CommandLineParser.Required()]
		[CommandLineParser.Name( "editor" )]
		public Editors TargetEditor { get; set; }


		public EditorCommand( Invoker invoker ) : base(invoker) 
		{
		}

		public override void Execute()
		{
			Editor.Run( Invoker.Game, TargetEditor );
		}
	}
}
