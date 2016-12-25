namespace IronStar.Editors {
	partial class MapEditor {
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
			this.mapMenu = new System.Windows.Forms.MenuStrip();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.newMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addMapModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addMapEntityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.duplicateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mapPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.mapListBox = new System.Windows.Forms.ListBox();
			this.mapMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// mapMenu
			// 
			this.mapMenu.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.mapMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.editToolStripMenuItem});
			this.mapMenu.Location = new System.Drawing.Point(0, 0);
			this.mapMenu.Name = "mapMenu";
			this.mapMenu.Size = new System.Drawing.Size(737, 24);
			this.mapMenu.TabIndex = 1;
			this.mapMenu.Text = "menuStrip1";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Checked = true;
			this.toolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMapToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.closeToolStripMenuItem});
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(43, 20);
			this.toolStripMenuItem1.Text = "Map";
			this.toolStripMenuItem1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			// 
			// newMapToolStripMenuItem
			// 
			this.newMapToolStripMenuItem.Name = "newMapToolStripMenuItem";
			this.newMapToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
			this.newMapToolStripMenuItem.Text = "New";
			this.newMapToolStripMenuItem.Click += new System.EventHandler(this.newMapToolStripMenuItem_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
			this.openToolStripMenuItem.Text = "Open...";
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
			this.saveToolStripMenuItem.Text = "Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
			this.saveAsToolStripMenuItem.Text = "Save As...";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// closeToolStripMenuItem
			// 
			this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
			this.closeToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
			this.closeToolStripMenuItem.Text = "Close";
			this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addMapModelToolStripMenuItem,
            this.addMapEntityToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.duplicateToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "Edit";
			// 
			// addMapModelToolStripMenuItem
			// 
			this.addMapModelToolStripMenuItem.Name = "addMapModelToolStripMenuItem";
			this.addMapModelToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.addMapModelToolStripMenuItem.Text = "Add Map Model...";
			this.addMapModelToolStripMenuItem.Click += new System.EventHandler(this.addMapModelToolStripMenuItem_Click);
			// 
			// addMapEntityToolStripMenuItem
			// 
			this.addMapEntityToolStripMenuItem.Name = "addMapEntityToolStripMenuItem";
			this.addMapEntityToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.addMapEntityToolStripMenuItem.Text = "Add Map Entity...";
			this.addMapEntityToolStripMenuItem.Click += new System.EventHandler(this.addMapEntityToolStripMenuItem_Click);
			// 
			// removeToolStripMenuItem
			// 
			this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
			this.removeToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.removeToolStripMenuItem.Text = "Remove";
			// 
			// duplicateToolStripMenuItem
			// 
			this.duplicateToolStripMenuItem.Name = "duplicateToolStripMenuItem";
			this.duplicateToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.duplicateToolStripMenuItem.Text = "Duplicate";
			// 
			// mapPropertyGrid
			// 
			this.mapPropertyGrid.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.mapPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mapPropertyGrid.Location = new System.Drawing.Point(213, 24);
			this.mapPropertyGrid.Name = "mapPropertyGrid";
			this.mapPropertyGrid.Size = new System.Drawing.Size(524, 518);
			this.mapPropertyGrid.TabIndex = 2;
			this.mapPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.mapPropertyGrid_PropertyValueChanged);
			// 
			// splitter1
			// 
			this.splitter1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.splitter1.Location = new System.Drawing.Point(210, 24);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 518);
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// mapListBox
			// 
			this.mapListBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.mapListBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mapListBox.IntegralHeight = false;
			this.mapListBox.Location = new System.Drawing.Point(0, 24);
			this.mapListBox.Name = "mapListBox";
			this.mapListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.mapListBox.Size = new System.Drawing.Size(210, 518);
			this.mapListBox.TabIndex = 4;
			this.mapListBox.SelectedIndexChanged += new System.EventHandler(this.mapListBox_SelectedIndexChanged);
			// 
			// MapEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.mapPropertyGrid);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.mapListBox);
			this.Controls.Add(this.mapMenu);
			this.Name = "MapEditor";
			this.Size = new System.Drawing.Size(737, 542);
			this.mapMenu.ResumeLayout(false);
			this.mapMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.MenuStrip mapMenu;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.PropertyGrid mapPropertyGrid;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ToolStripMenuItem newMapToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addMapModelToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addMapEntityToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem duplicateToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
		private System.Windows.Forms.ListBox mapListBox;
	}
}
