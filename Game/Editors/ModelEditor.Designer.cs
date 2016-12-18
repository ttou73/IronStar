namespace IronStar.Development {
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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.modelEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.renameModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAndRebuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.buttonSave = new System.Windows.Forms.Button();
			this.buttonExit = new System.Windows.Forms.Button();
			this.buttonSaveAndBuild = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.mainPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.modelListBox = new System.Windows.Forms.ListBox();
			this.menuStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modelEditorToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(438, 24);
			this.menuStrip1.TabIndex = 3;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// modelEditorToolStripMenuItem
			// 
			this.modelEditorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addModelToolStripMenuItem,
            this.renameModelToolStripMenuItem,
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
			this.addModelToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.addModelToolStripMenuItem.Text = "Add Model...";
			this.addModelToolStripMenuItem.Click += new System.EventHandler(this.addModelToolStripMenuItem_Click);
			// 
			// renameModelToolStripMenuItem
			// 
			this.renameModelToolStripMenuItem.Name = "renameModelToolStripMenuItem";
			this.renameModelToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.renameModelToolStripMenuItem.Text = "Rename Model...";
			this.renameModelToolStripMenuItem.Click += new System.EventHandler(this.renameModelToolStripMenuItem_Click);
			// 
			// removeModelToolStripMenuItem
			// 
			this.removeModelToolStripMenuItem.Name = "removeModelToolStripMenuItem";
			this.removeModelToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.removeModelToolStripMenuItem.Text = "Remove Model";
			this.removeModelToolStripMenuItem.Click += new System.EventHandler(this.removeModelToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(160, 6);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.saveToolStripMenuItem.Text = "Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAndRebuildToolStripMenuItem
			// 
			this.saveAndRebuildToolStripMenuItem.Name = "saveAndRebuildToolStripMenuItem";
			this.saveAndRebuildToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.saveAndRebuildToolStripMenuItem.Text = "Save and Build";
			this.saveAndRebuildToolStripMenuItem.Click += new System.EventHandler(this.saveAndRebuildToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(160, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.buttonSave);
			this.panel1.Controls.Add(this.buttonExit);
			this.panel1.Controls.Add(this.buttonSaveAndBuild);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 517);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(438, 44);
			this.panel1.TabIndex = 4;
			// 
			// buttonSave
			// 
			this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSave.Location = new System.Drawing.Point(205, 11);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.Size = new System.Drawing.Size(80, 30);
			this.buttonSave.TabIndex = 1;
			this.buttonSave.Text = "Save";
			this.buttonSave.UseVisualStyleBackColor = true;
			this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
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
			// buttonSaveAndBuild
			// 
			this.buttonSaveAndBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSaveAndBuild.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonSaveAndBuild.Location = new System.Drawing.Point(291, 11);
			this.buttonSaveAndBuild.Name = "buttonSaveAndBuild";
			this.buttonSaveAndBuild.Size = new System.Drawing.Size(144, 30);
			this.buttonSaveAndBuild.TabIndex = 3;
			this.buttonSaveAndBuild.Text = "Save and Build";
			this.buttonSaveAndBuild.UseVisualStyleBackColor = true;
			this.buttonSaveAndBuild.Click += new System.EventHandler(this.buttonSaveAndBuild_Click);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.mainPropertyGrid);
			this.panel2.Controls.Add(this.splitter1);
			this.panel2.Controls.Add(this.modelListBox);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 24);
			this.panel2.Name = "panel2";
			this.panel2.Padding = new System.Windows.Forms.Padding(3, 3, 4, 3);
			this.panel2.Size = new System.Drawing.Size(438, 493);
			this.panel2.TabIndex = 6;
			// 
			// mainPropertyGrid
			// 
			this.mainPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainPropertyGrid.Location = new System.Drawing.Point(187, 3);
			this.mainPropertyGrid.Name = "mainPropertyGrid";
			this.mainPropertyGrid.Size = new System.Drawing.Size(247, 487);
			this.mainPropertyGrid.TabIndex = 3;
			this.mainPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.mainPropertyGrid_PropertyValueChanged);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(184, 3);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 487);
			this.splitter1.TabIndex = 6;
			this.splitter1.TabStop = false;
			// 
			// modelListBox
			// 
			this.modelListBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.modelListBox.FormattingEnabled = true;
			this.modelListBox.IntegralHeight = false;
			this.modelListBox.Location = new System.Drawing.Point(3, 3);
			this.modelListBox.Name = "modelListBox";
			this.modelListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.modelListBox.Size = new System.Drawing.Size(181, 487);
			this.modelListBox.Sorted = true;
			this.modelListBox.TabIndex = 2;
			this.modelListBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// ModelEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(438, 561);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.panel1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MainMenuStrip = this.menuStrip1;
			this.MinimumSize = new System.Drawing.Size(400, 400);
			this.Name = "ModelEditor";
			this.Text = "ModelEditor";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ModelEditor_FormClosing);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem modelEditorToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addModelToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeModelToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAndRebuildToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem renameModelToolStripMenuItem;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button buttonSave;
		private System.Windows.Forms.Button buttonExit;
		private System.Windows.Forms.Button buttonSaveAndBuild;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.PropertyGrid mainPropertyGrid;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ListBox modelListBox;
	}
}