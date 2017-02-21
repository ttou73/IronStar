using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fusion.Core.Extensions;

namespace IronStar.AI.Editor
{
    public partial class BehaviorTreeEditorControl : UserControl , IMessageFilter
    {

        //TODO: make moveable panel.
        private bool isWorkspaceMoving;
        private Point mousePoint;
        //

        BehaviorTree activeTree;

        TreeNodeControl start;
        TreeNodeControl end;
        Dictionary<TreeNodeControl, List<TreeNodeControl>> edges = new Dictionary<TreeNodeControl, List<TreeNodeControl>>();

        public BehaviorTreeEditorControl()
        {
            Application.AddMessageFilter(this);
            InitializeComponent();

        }

        private void BehaviorTreeEditorControl_Load(object sender, EventArgs e)
        {
            foreach(var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in a.GetLoadableTypes())
                {
                    if (t.IsSubclassOf(typeof(BehaviorNode)) && t != typeof(BehaviorNode))
                    {
                        NodeList.Items.Add(t.FullName);
                    }
                }
            }

            AddButton.Enabled = false;
            NodeList.Enabled = false;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (NodeList.SelectedItem != null)
            {
                var t = activeTree.AddNode(Type.GetType(NodeList.SelectedItem.ToString()), null);
                Workspace.Controls.Add(new TreeNodeControl(this, t));
            }
        }

        private void TreesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cb = (sender) as ComboBox;
            if (cb.SelectedItem != null)
            {
                AddButton.Enabled = true;
                NodeList.Enabled = true;
                activeTree = (BehaviorTree)cb.SelectedItem;
            }
        }

        private void AddTree_Click(object sender, EventArgs e)
        {
            int i = 0;
            while (true)
            {
                string name = i == 0 ? "New tree" : $"New tree{i}";

                bool good = true;
                foreach (var o in TreesComboBox.Items)
                {
                    if (o.ToString().Equals(name))
                    {
                        i++;
                        good = false;
                        break;
                    }
                }
                if (good)
                {
                    TreesComboBox.Items.Add(new BehaviorTree(name));
                    break;
                }
            }
            
        }

        private void Workspace_Paint(object sender, PaintEventArgs e)
        {
            var p = sender as Panel;
            var g = e.Graphics;

            g.FillRectangle(new SolidBrush(Color.FromArgb(0, Color.Black)), p.DisplayRectangle);

            Point[] points = new Point[4];

            points[0] = new Point(0, 0);
            points[1] = new Point(0, p.Height);
            points[2] = new Point(p.Width, p.Height);
            points[3] = new Point(p.Width, 0);

            Brush brush = new SolidBrush(Workspace.BackColor);

            g.FillPolygon(brush, points);
            Pen pen = new Pen(Color.Khaki, 3f);
            foreach (var kvp in edges)
            {
                var pt1 = new Point(kvp.Key.Location.X + kvp.Key.Start.Location.X + 10, kvp.Key.Location.Y + kvp.Key.Start.Location.Y + 10);
                foreach  (var s in kvp.Value)
                {
                    var pt2 = new Point(s.Location.X + s.End.Location.X + 10, s.Location.Y + s.End.Location.Y + 10);
                     g.DrawLine(pen, pt1, pt2);
                }
            }
        }

        public void RegisterStart(TreeNodeControl c)
        {
            DeregisterStart();
            if (end == c) return;
            c.Start.BackColor = Color.Red;
            start = c; 
            if (end != null)
            {
                CreateEdge();
                DeregisterStart();
                DeregisterEnd();
                start = end = null;
            }
        }

       

        public void RegisterEnd(TreeNodeControl c)
        {
            DeregisterEnd();
            if (start == c) return;
            c.End.BackColor = Color.Red;
            end = c;
            if (start != null)
            {
                CreateEdge();
                DeregisterStart();
                DeregisterEnd();
                start = end = null;
            }
        }

        private void DeregisterStart()
        {
            if (start != null)
            {
                start.Start.BackColor = Color.Khaki;
            }
            start = null;
        }

        private void DeregisterEnd()
        {
            if (end != null)
            {
                end.End.BackColor = Color.Khaki;
            }
            end = null;
        }

        private void CreateEdge()
        {
            if (!edges.ContainsKey(start))
            {
                edges[start] = new List<TreeNodeControl>();
            }
            foreach (var tnc in edges[start])
            {
                if (tnc == end)
                {
                    return;
                }
            }
            edges[start].Add(end);
            start.Node.AddChild(end.Node);
            Workspace.Invalidate();
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x0204)
            {

                DeregisterStart();
                DeregisterEnd();
            }
            return false;
        }

        private void Workspace_MouseLeave(object sender, EventArgs e)
        {
            isWorkspaceMoving = false;
        }

        private void Workspace_MouseMove(object sender, MouseEventArgs e)
        {
            if (isWorkspaceMoving && e.Button == MouseButtons.Middle)
            {
                foreach (var c in Workspace.Controls)
                {
                    if (c is TreeNodeControl)
                    {
                        var control = c as TreeNodeControl;
                        control.Left += e.X - mousePoint.X;
                        control.Top += e.Y - mousePoint.Y;
                    }
                }
                Workspace.Invalidate();
            }
            mousePoint = e.Location;
        }

        private void Workspace_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                mousePoint = e.Location;
                isWorkspaceMoving = true;
            }
        }
    }
}
