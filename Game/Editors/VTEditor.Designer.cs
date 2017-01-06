namespace IronStar.Editors {
	partial class VTEditor {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textureListBox = new System.Windows.Forms.ListBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.texturePropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.megatextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newTextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.renameTextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.importFromFBXSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// textureListBox
			// 
			this.textureListBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.textureListBox.FormattingEnabled = true;
			this.textureListBox.IntegralHeight = false;
			this.textureListBox.Location = new System.Drawing.Point(0, 24);
			this.textureListBox.Name = "textureListBox";
			this.textureListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.textureListBox.Size = new System.Drawing.Size(174, 429);
			this.textureListBox.Sorted = true;
			this.textureListBox.TabIndex = 0;
			this.textureListBox.SelectedIndexChanged += new System.EventHandler(this.textureListBox_SelectedIndexChanged);
			// 
			// splitter1
			// 
			this.splitter1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.splitter1.Location = new System.Drawing.Point(174, 24);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 429);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// menuStrip1
			// 
			this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.megatextureToolStripMenuItem,
            this.editToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(616, 24);
			this.menuStrip1.TabIndex = 3;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// texturePropertyGrid
			// 
			this.texturePropertyGrid.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.texturePropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.texturePropertyGrid.Location = new System.Drawing.Point(177, 24);
			this.texturePropertyGrid.Name = "texturePropertyGrid";
			this.texturePropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
			this.texturePropertyGrid.Size = new System.Drawing.Size(439, 429);
			this.texturePropertyGrid.TabIndex = 4;
			// 
			// megatextureToolStripMenuItem
			// 
			this.megatextureToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem});
			this.megatextureToolStripMenuItem.Name = "megatextureToolStripMenuItem";
			this.megatextureToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
			this.megatextureToolStripMenuItem.Text = "Megatexture";
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.saveToolStripMenuItem.Text = "Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTextureToolStripMenuItem,
            this.renameTextureToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.toolStripSeparator1,
            this.importFromFBXSceneToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "Edit";
			// 
			// newTextureToolStripMenuItem
			// 
			this.newTextureToolStripMenuItem.Name = "newTextureToolStripMenuItem";
			this.newTextureToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.newTextureToolStripMenuItem.Text = "New...";
			this.newTextureToolStripMenuItem.Click += new System.EventHandler(this.newTextureToolStripMenuItem_Click);
			// 
			// renameTextureToolStripMenuItem
			// 
			this.renameTextureToolStripMenuItem.Name = "renameTextureToolStripMenuItem";
			this.renameTextureToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.renameTextureToolStripMenuItem.Text = "Rename...";
			this.renameTextureToolStripMenuItem.Click += new System.EventHandler(this.renameTextureToolStripMenuItem_Click);
			// 
			// removeToolStripMenuItem
			// 
			this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
			this.removeToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.removeToolStripMenuItem.Text = "Remove...";
			this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(202, 6);
			// 
			// importFromFBXSceneToolStripMenuItem
			// 
			this.importFromFBXSceneToolStripMenuItem.Name = "importFromFBXSceneToolStripMenuItem";
			this.importFromFBXSceneToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.importFromFBXSceneToolStripMenuItem.Text = "Import from FBX Scene...";
			this.importFromFBXSceneToolStripMenuItem.Click += new System.EventHandler(this.importFromFBXSceneToolStripMenuItem_Click);
			// 
			// VTEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.texturePropertyGrid);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.textureListBox);
			this.Controls.Add(this.menuStrip1);
			this.Name = "VTEditor";
			this.Size = new System.Drawing.Size(616, 453);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox textureListBox;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.PropertyGrid texturePropertyGrid;
		private System.Windows.Forms.ToolStripMenuItem megatextureToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newTextureToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem renameTextureToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem importFromFBXSceneToolStripMenuItem;
	}
}
