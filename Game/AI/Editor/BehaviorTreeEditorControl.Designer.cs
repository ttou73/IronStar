using System.Windows.Forms;

namespace IronStar.AI.Editor
{
    partial class BehaviorTreeEditorControl
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
            if (disposing) Application.RemoveMessageFilter(this);
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
            this.TreeName = new System.Windows.Forms.TextBox();
            this.Header = new System.Windows.Forms.Panel();
            this.AddTree = new System.Windows.Forms.Button();
            this.TreesComboBox = new System.Windows.Forms.ComboBox();
            this.lTreeName = new System.Windows.Forms.Label();
            this.NodeList = new System.Windows.Forms.ListBox();
            this.Right = new System.Windows.Forms.Panel();
            this.ButtonPanel = new System.Windows.Forms.Panel();
            this.AddButton = new System.Windows.Forms.Button();
            this.Workspace = new System.Windows.Forms.Panel();
            this.Header.SuspendLayout();
            this.Right.SuspendLayout();
            this.ButtonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreeName
            // 
            this.TreeName.Location = new System.Drawing.Point(305, 12);
            this.TreeName.Name = "TreeName";
            this.TreeName.Size = new System.Drawing.Size(100, 22);
            this.TreeName.TabIndex = 0;
            // 
            // Header
            // 
            this.Header.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Header.Controls.Add(this.AddTree);
            this.Header.Controls.Add(this.TreesComboBox);
            this.Header.Controls.Add(this.lTreeName);
            this.Header.Controls.Add(this.TreeName);
            this.Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.Header.Location = new System.Drawing.Point(0, 0);
            this.Header.Name = "Header";
            this.Header.Size = new System.Drawing.Size(831, 48);
            this.Header.TabIndex = 0;
            // 
            // AddTree
            // 
            this.AddTree.Location = new System.Drawing.Point(177, 7);
            this.AddTree.Name = "AddTree";
            this.AddTree.Size = new System.Drawing.Size(35, 35);
            this.AddTree.TabIndex = 1;
            this.AddTree.Text = "+";
            this.AddTree.UseVisualStyleBackColor = true;
            this.AddTree.Click += new System.EventHandler(this.AddTree_Click);
            // 
            // TreesComboBox
            // 
            this.TreesComboBox.FormattingEnabled = true;
            this.TreesComboBox.Location = new System.Drawing.Point(0, 10);
            this.TreesComboBox.Name = "TreesComboBox";
            this.TreesComboBox.Size = new System.Drawing.Size(172, 24);
            this.TreesComboBox.TabIndex = 2;
            this.TreesComboBox.SelectedIndexChanged += new System.EventHandler(this.TreesComboBox_SelectedIndexChanged);
            // 
            // lTreeName
            // 
            this.lTreeName.AutoSize = true;
            this.lTreeName.Location = new System.Drawing.Point(218, 17);
            this.lTreeName.Name = "lTreeName";
            this.lTreeName.Size = new System.Drawing.Size(81, 17);
            this.lTreeName.TabIndex = 1;
            this.lTreeName.Text = "Tree name:";
            // 
            // NodeList
            // 
            this.NodeList.Dock = System.Windows.Forms.DockStyle.Left;
            this.NodeList.FormattingEnabled = true;
            this.NodeList.ItemHeight = 16;
            this.NodeList.Location = new System.Drawing.Point(0, 0);
            this.NodeList.Name = "NodeList";
            this.NodeList.Size = new System.Drawing.Size(172, 665);
            this.NodeList.TabIndex = 0;
            // 
            // Right
            // 
            this.Right.Controls.Add(this.ButtonPanel);
            this.Right.Controls.Add(this.NodeList);
            this.Right.Dock = System.Windows.Forms.DockStyle.Left;
            this.Right.Location = new System.Drawing.Point(0, 48);
            this.Right.Name = "Right";
            this.Right.Size = new System.Drawing.Size(217, 665);
            this.Right.TabIndex = 1;
            // 
            // ButtonPanel
            // 
            this.ButtonPanel.Controls.Add(this.AddButton);
            this.ButtonPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ButtonPanel.Location = new System.Drawing.Point(171, 0);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Size = new System.Drawing.Size(46, 665);
            this.ButtonPanel.TabIndex = 1;
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(6, 6);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(35, 35);
            this.AddButton.TabIndex = 0;
            this.AddButton.Text = "+";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // Workspace
            // 
            this.Workspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Workspace.Location = new System.Drawing.Point(217, 48);
            this.Workspace.Name = "Workspace";
            this.Workspace.Size = new System.Drawing.Size(614, 665);
            this.Workspace.TabIndex = 2;
            this.Workspace.Paint += new System.Windows.Forms.PaintEventHandler(this.Workspace_Paint);
            this.Workspace.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Workspace_MouseDown);
            this.Workspace.MouseLeave += new System.EventHandler(this.Workspace_MouseLeave);
            this.Workspace.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Workspace_MouseMove);
            // 
            // BehaviorTreeEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Workspace);
            this.Controls.Add(this.Right);
            this.Controls.Add(this.Header);
            this.Name = "BehaviorTreeEditorControl";
            this.Size = new System.Drawing.Size(831, 713);
            this.Load += new System.EventHandler(this.BehaviorTreeEditorControl_Load);
            this.Header.ResumeLayout(false);
            this.Header.PerformLayout();
            this.Right.ResumeLayout(false);
            this.ButtonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox TreeName;
        private System.Windows.Forms.Panel Header;
        private System.Windows.Forms.Label lTreeName;
        private System.Windows.Forms.ListBox NodeList;
        private System.Windows.Forms.Panel Right;
        private System.Windows.Forms.Panel ButtonPanel;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button AddTree;
        private System.Windows.Forms.ComboBox TreesComboBox;
        public System.Windows.Forms.Panel Workspace;
    }
}
