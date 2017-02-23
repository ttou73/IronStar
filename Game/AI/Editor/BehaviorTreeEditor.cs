using Fusion.Engine.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronStar.AI.Editor
{
    public partial class BehaviorTreeEditor : Form
    {
        public BehaviorTreeEditor(Game game)
        {
            InitializeComponent();
            var editor = new BehaviorTreeEditorControl();
            editor.Dock = DockStyle.Fill;
            this.Controls.Add(editor);
        }

        private void BehaviorTreeEditor_Load(object sender, EventArgs e)
        {

        }
    }
}
