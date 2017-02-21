using Fusion.Core.Shell;
using Fusion.Engine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronStar.AI.Editor
{
    public static class BTEditor
    {
        public static void Run(Game game)
        {
            var editorForm = Application.OpenForms.Cast<Form>().FirstOrDefault(form => form is BehaviorTreeEditor);

            if (editorForm == null)
            {
                editorForm = new BehaviorTreeEditor(game);
            }

            editorForm.Show();
            editorForm.Activate();
            editorForm.BringToFront();
        }
    }


    [Command("bteditor", CommandAffinity.Default)]
    public class EditorCommand : NoRollbackCommand
    {

        public EditorCommand(Invoker invoker) : base(invoker)
        {
        }

        public override void Execute()
        {
            BTEditor.Run(Invoker.Game);
        }
    }
}
