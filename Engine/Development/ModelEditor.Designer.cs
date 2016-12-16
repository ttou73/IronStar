namespace Fusion.Development {
	partial class ModelEditor {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.buttonUpdate = new System.Windows.Forms.Button();
			this.buttonExit = new System.Windows.Forms.Button();
			this.buttonSave = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.modelEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAndRebuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.IntegralHeight = false;
			this.listBox1.Location = new System.Drawing.Point(14, 27);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(205, 519);
			this.listBox1.TabIndex = 1;
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Location = new System.Drawing.Point(225, 27);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(250, 519);
			this.propertyGrid1.TabIndex = 2;
			// 
			// buttonUpdate
			// 
			this.buttonUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonUpdate.Location = new System.Drawing.Point(331, 557);
			this.buttonUpdate.Name = "buttonUpdate";
			this.buttonUpdate.Size = new System.Drawing.Size(144, 30);
			this.buttonUpdate.TabIndex = 0;
			this.buttonUpdate.Text = "Save and Rebuild";
			this.buttonUpdate.UseVisualStyleBackColor = true;
			// 
			// buttonExit
			// 
			this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonExit.Location = new System.Drawing.Point(12, 557);
			this.buttonExit.Name = "buttonExit";
			this.buttonExit.Size = new System.Drawing.Size(100, 30);
			this.buttonExit.TabIndex = 0;
			this.buttonExit.Text = "Exit";
			this.buttonExit.UseVisualStyleBackColor = true;
			// 
			// buttonSave
			// 
			this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonSave.Location = new System.Drawing.Point(225, 557);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.Size = new System.Drawing.Size(100, 30);
			this.buttonSave.TabIndex = 0;
			this.buttonSave.Text = "Save";
			this.buttonSave.UseVisualStyleBackColor = true;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modelEditorToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(487, 24);
			this.menuStrip1.TabIndex = 3;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// modelEditorToolStripMenuItem
			// 
			this.modelEditorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addModelToolStripMenuItem,
            this.removeModelToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveToolStripMenuItem,
            this.saveAndRebuildToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
			this.modelEditorToolStripMenuItem.Name = "modelEditorToolStripMenuItem";
			this.modelEditorToolStripMenuItem.Size = new System.Drawing.Size(87, 20);
			this.modelEditorToolStripMenuItem.Text = "Model Editor";
			// 
			// addModelToolStripMenuItem
			// 
			this.addModelToolStripMenuItem.Name = "addModelToolStripMenuItem";
			this.addModelToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.addModelToolStripMenuItem.Text = "Add Model...";
			// 
			// removeModelToolStripMenuItem
			// 
			this.removeModelToolStripMenuItem.Name = "removeModelToolStripMenuItem";
			this.removeModelToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.removeModelToolStripMenuItem.Text = "Remove Model";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(151, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(151, 6);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.saveToolStripMenuItem.Text = "Save";
			// 
			// saveAndRebuildToolStripMenuItem
			// 
			this.saveAndRebuildToolStripMenuItem.Name = "saveAndRebuildToolStripMenuItem";
			this.saveAndRebuildToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.saveAndRebuildToolStripMenuItem.Text = "Update";
			// 
			// ModelEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(487, 599);
			this.Controls.Add(this.propertyGrid1);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.buttonSave);
			this.Controls.Add(this.buttonExit);
			this.Controls.Add(this.buttonUpdate);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "ModelEditor";
			this.Text = "ModelEditor";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Button buttonUpdate;
		private System.Windows.Forms.Button buttonExit;
		private System.Windows.Forms.Button buttonSave;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem modelEditorToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addModelToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeModelToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAndRebuildToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
	}
}