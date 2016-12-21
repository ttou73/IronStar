namespace IronStar.Editors {
	partial class EditorForm {
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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.buttonExit = new System.Windows.Forms.Button();
			this.buttonBuild = new System.Windows.Forms.Button();
			this.mainTabs = new System.Windows.Forms.TabControl();
			this.tabModels = new System.Windows.Forms.TabPage();
			this.tabEntities = new System.Windows.Forms.TabPage();
			this.addModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.mainTabs.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(438, 24);
			this.menuStrip1.TabIndex = 3;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addModelToolStripMenuItem,
            this.removeModelToolStripMenuItem});
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(58, 20);
			this.toolStripMenuItem1.Text = "Models";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.buttonExit);
			this.panel1.Controls.Add(this.buttonBuild);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 517);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(438, 44);
			this.panel1.TabIndex = 4;
			// 
			// buttonExit
			// 
			this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonExit.Location = new System.Drawing.Point(3, 11);
			this.buttonExit.Name = "buttonExit";
			this.buttonExit.Size = new System.Drawing.Size(80, 30);
			this.buttonExit.TabIndex = 2;
			this.buttonExit.Text = "Exit";
			this.buttonExit.UseVisualStyleBackColor = true;
			this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
			// 
			// buttonBuild
			// 
			this.buttonBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonBuild.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonBuild.Location = new System.Drawing.Point(334, 11);
			this.buttonBuild.Name = "buttonBuild";
			this.buttonBuild.Size = new System.Drawing.Size(100, 30);
			this.buttonBuild.TabIndex = 3;
			this.buttonBuild.Text = "Build";
			this.buttonBuild.UseVisualStyleBackColor = true;
			this.buttonBuild.Click += new System.EventHandler(this.buttonSaveAndBuild_Click);
			// 
			// mainTabs
			// 
			this.mainTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mainTabs.Controls.Add(this.tabModels);
			this.mainTabs.Controls.Add(this.tabEntities);
			this.mainTabs.Location = new System.Drawing.Point(3, 27);
			this.mainTabs.Name = "mainTabs";
			this.mainTabs.SelectedIndex = 0;
			this.mainTabs.Size = new System.Drawing.Size(432, 484);
			this.mainTabs.TabIndex = 5;
			// 
			// tabModels
			// 
			this.tabModels.Location = new System.Drawing.Point(4, 22);
			this.tabModels.Name = "tabModels";
			this.tabModels.Padding = new System.Windows.Forms.Padding(0, 1, 1, 0);
			this.tabModels.Size = new System.Drawing.Size(424, 458);
			this.tabModels.TabIndex = 0;
			this.tabModels.Text = "Models";
			this.tabModels.UseVisualStyleBackColor = true;
			// 
			// tabEntities
			// 
			this.tabEntities.Location = new System.Drawing.Point(4, 22);
			this.tabEntities.Name = "tabEntities";
			this.tabEntities.Padding = new System.Windows.Forms.Padding(3);
			this.tabEntities.Size = new System.Drawing.Size(424, 458);
			this.tabEntities.TabIndex = 1;
			this.tabEntities.Text = "Entities";
			this.tabEntities.UseVisualStyleBackColor = true;
			// 
			// addModelToolStripMenuItem
			// 
			this.addModelToolStripMenuItem.Name = "addModelToolStripMenuItem";
			this.addModelToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.addModelToolStripMenuItem.Text = "Add Model...";
			this.addModelToolStripMenuItem.Click += new System.EventHandler(this.addModelToolStripMenuItem_Click);
			// 
			// removeModelToolStripMenuItem
			// 
			this.removeModelToolStripMenuItem.Name = "removeModelToolStripMenuItem";
			this.removeModelToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.removeModelToolStripMenuItem.Text = "Remove Model";
			this.removeModelToolStripMenuItem.Click += new System.EventHandler(this.removeModelToolStripMenuItem_Click);
			// 
			// EditorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(438, 561);
			this.Controls.Add(this.mainTabs);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.panel1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MainMenuStrip = this.menuStrip1;
			this.MinimumSize = new System.Drawing.Size(400, 400);
			this.Name = "EditorForm";
			this.Text = "Object Editor";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.mainTabs.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button buttonExit;
		private System.Windows.Forms.Button buttonBuild;
		private System.Windows.Forms.TabControl mainTabs;
		private System.Windows.Forms.TabPage tabModels;
		private System.Windows.Forms.TabPage tabEntities;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem addModelToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeModelToolStripMenuItem;
	}
}