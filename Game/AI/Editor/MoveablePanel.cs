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
    public partial class MoveablePanel : UserControl
    {


        private bool isMoving;
        private Point mousePoint;

        public MoveablePanel()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        private void MoveablePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                mousePoint = e.Location;
                isMoving = true;
            }
        }

        private void MoveablePanel_MouseEnter(object sender, EventArgs e)
        {

        }

        private void MoveablePanel_MouseLeave(object sender, EventArgs e)
        {
            isMoving = false;
        }

        private void MoveablePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMoving && e.Button == MouseButtons.Middle)
            {
                foreach (var c in Controls)
                {
                    if (c is TreeNodeControl)
                    {
                        var control = c as TreeNodeControl;
                        control.Left += e.X - mousePoint.X;
                        control.Top += e.Y - mousePoint.Y;
                    }
                }
                this.Invalidate();
            }
            mousePoint = e.Location;
        }
    }
}
