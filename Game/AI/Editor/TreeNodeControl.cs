using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronStar.AI.Editor
{
    public partial class TreeNodeControl : UserControl
    {
        private bool isDragging = false;
        private Point mouseInitPos;
        public readonly BehaviorNode Node;
        public Point lastPosition;
        BehaviorTreeEditorControl editor;

        public TreeNodeControl(BehaviorTreeEditorControl editor, BehaviorNode node )
        {
            this.editor = editor;
            this.Node = node;

            InitializeComponent();
        }

        private void TreeNodeControl_Load(object sender, EventArgs e)
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
        }

        private void TreeNodeControl_MouseDown(object sender, MouseEventArgs e)
        {
            this.BringToFront();
            if (e.Button == MouseButtons.Left)
            {
                mouseInitPos = e.Location;
                isDragging = true;
            }
        }

        private void TreeNodeControl_MouseLeave(object sender, EventArgs e)
        {
            isDragging = false;
        }

        private void TreeNodeControl_MouseMove(object sender, MouseEventArgs e)
        {
            lastPosition = Location;
            if (isDragging && e.Button == MouseButtons.Left)
            {
                this.Left += e.X - mouseInitPos.X;
                this.Top += e.Y - mouseInitPos.Y;
                editor.Workspace.Invalidate();
            }
        }

        private void Start_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                editor.RegisterStart(this);
            }
        }

        private void End_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                editor.RegisterEnd(this);
            }
        }
    }
}
