namespace IronStar.AI.Editor
{
    partial class TreeNodeControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NodeName = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Start = new System.Windows.Forms.Panel();
            this.End = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // NodeName
            // 
            this.NodeName.Location = new System.Drawing.Point(29, 16);
            this.NodeName.Name = "NodeName";
            this.NodeName.Size = new System.Drawing.Size(60, 22);
            this.NodeName.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pictureBox1.Location = new System.Drawing.Point(29, 44);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(60, 60);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Lavender;
            this.panel1.Controls.Add(this.NodeName);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(120, 120);
            this.panel1.TabIndex = 2;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TreeNodeControl_MouseDown);
            this.panel1.MouseLeave += new System.EventHandler(this.TreeNodeControl_MouseLeave);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TreeNodeControl_MouseMove);
            // 
            // Start
            // 
            this.Start.BackColor = System.Drawing.Color.Khaki;
            this.Start.Location = new System.Drawing.Point(60, 120);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(20, 20);
            this.Start.TabIndex = 2;
            this.Start.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Start_MouseClick);
            // 
            // End
            // 
            this.End.BackColor = System.Drawing.Color.Khaki;
            this.End.Location = new System.Drawing.Point(60, 0);
            this.End.Name = "End";
            this.End.Size = new System.Drawing.Size(20, 20);
            this.End.TabIndex = 3;
            this.End.MouseClick += new System.Windows.Forms.MouseEventHandler(this.End_MouseClick);
            // 
            // TreeNodeControl
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Wheat;
            this.Controls.Add(this.End);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.panel1);
            this.Name = "TreeNodeControl";
            this.Size = new System.Drawing.Size(140, 140);
            this.Load += new System.EventHandler(this.TreeNodeControl_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TreeNodeControl_MouseDown);
            this.MouseLeave += new System.EventHandler(this.TreeNodeControl_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TreeNodeControl_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox NodeName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Panel Start;
        public System.Windows.Forms.Panel End;
    }
}
