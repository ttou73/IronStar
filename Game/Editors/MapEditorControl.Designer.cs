namespace IronStar.Editors {
	partial class MapEditorControl {
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
			this.mapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.refreshWorldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.gridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.freezeSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.unfreezeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.buildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.visibilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.navigationMeshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.irradianceCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.editRecastConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabNode = new System.Windows.Forms.TabPage();
			this.gridTransform = new System.Windows.Forms.PropertyGrid();
			this.tabFactory = new System.Windows.Forms.TabPage();
			this.gridFactory = new System.Windows.Forms.PropertyGrid();
			this.tabEnv = new System.Windows.Forms.TabPage();
			this.gridEnv = new System.Windows.Forms.PropertyGrid();
			this.mapMenu.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabNode.SuspendLayout();
			this.tabFactory.SuspendLayout();
			this.tabEnv.SuspendLayout();
			this.SuspendLayout();
			// 
			// mapMenu
			// 
			this.mapMenu.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.mapMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapToolStripMenuItem,
            this.createToolStripMenuItem,
            this.displayToolStripMenuItem,
            this.buildToolStripMenuItem});
			this.mapMenu.Location = new System.Drawing.Point(0, 0);
			this.mapMenu.Name = "mapMenu";
			this.mapMenu.Size = new System.Drawing.Size(525, 24);
			this.mapMenu.TabIndex = 1;
			this.mapMenu.Text = "menuStrip1";
			// 
			// mapToolStripMenuItem
			// 
			this.mapToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
			this.mapToolStripMenuItem.Name = "mapToolStripMenuItem";
			this.mapToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
			this.mapToolStripMenuItem.Text = "Map";
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
			this.saveToolStripMenuItem.Text = "Save";
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
			this.saveAsToolStripMenuItem.Text = "Save As...";
			// 
			// createToolStripMenuItem
			// 
			this.createToolStripMenuItem.Name = "createToolStripMenuItem";
			this.createToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
			this.createToolStripMenuItem.Text = "Create";
			// 
			// displayToolStripMenuItem
			// 
			this.displayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshWorldToolStripMenuItem,
            this.toolStripSeparator3,
            this.gridToolStripMenuItem,
            this.toolStripSeparator4,
            this.freezeSelectionToolStripMenuItem,
            this.unfreezeAllToolStripMenuItem});
			this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
			this.displayToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
			this.displayToolStripMenuItem.Text = "Display";
			// 
			// refreshWorldToolStripMenuItem
			// 
			this.refreshWorldToolStripMenuItem.Name = "refreshWorldToolStripMenuItem";
			this.refreshWorldToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
			this.refreshWorldToolStripMenuItem.Text = "Refresh World";
			this.refreshWorldToolStripMenuItem.Click += new System.EventHandler(this.refreshWorldToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(155, 6);
			// 
			// gridToolStripMenuItem
			// 
			this.gridToolStripMenuItem.Name = "gridToolStripMenuItem";
			this.gridToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
			this.gridToolStripMenuItem.Text = "Grid";
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(155, 6);
			// 
			// freezeSelectionToolStripMenuItem
			// 
			this.freezeSelectionToolStripMenuItem.Name = "freezeSelectionToolStripMenuItem";
			this.freezeSelectionToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
			this.freezeSelectionToolStripMenuItem.Text = "Freeze Selection";
			this.freezeSelectionToolStripMenuItem.Click += new System.EventHandler(this.freezeSelectionToolStripMenuItem_Click);
			// 
			// unfreezeAllToolStripMenuItem
			// 
			this.unfreezeAllToolStripMenuItem.Name = "unfreezeAllToolStripMenuItem";
			this.unfreezeAllToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
			this.unfreezeAllToolStripMenuItem.Text = "Unfreeze All";
			this.unfreezeAllToolStripMenuItem.Click += new System.EventHandler(this.unfreezeAllToolStripMenuItem_Click);
			// 
			// buildToolStripMenuItem
			// 
			this.buildToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.visibilityToolStripMenuItem,
            this.navigationMeshToolStripMenuItem,
            this.irradianceCacheToolStripMenuItem,
            this.toolStripSeparator2,
            this.editRecastConfigurationToolStripMenuItem});
			this.buildToolStripMenuItem.Name = "buildToolStripMenuItem";
			this.buildToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
			this.buildToolStripMenuItem.Text = "Build";
			// 
			// visibilityToolStripMenuItem
			// 
			this.visibilityToolStripMenuItem.Name = "visibilityToolStripMenuItem";
			this.visibilityToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.visibilityToolStripMenuItem.Text = "Visibility Query Cache...";
			// 
			// navigationMeshToolStripMenuItem
			// 
			this.navigationMeshToolStripMenuItem.Name = "navigationMeshToolStripMenuItem";
			this.navigationMeshToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.navigationMeshToolStripMenuItem.Text = "Navigation Mesh";
			this.navigationMeshToolStripMenuItem.Click += new System.EventHandler(this.navigationMeshToolStripMenuItem_Click);
			// 
			// irradianceCacheToolStripMenuItem
			// 
			this.irradianceCacheToolStripMenuItem.Name = "irradianceCacheToolStripMenuItem";
			this.irradianceCacheToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.irradianceCacheToolStripMenuItem.Text = "Irradiance Cache...";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(195, 6);
			// 
			// editRecastConfigurationToolStripMenuItem
			// 
			this.editRecastConfigurationToolStripMenuItem.Name = "editRecastConfigurationToolStripMenuItem";
			this.editRecastConfigurationToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.editRecastConfigurationToolStripMenuItem.Text = "Recast Configuration...";
			this.editRecastConfigurationToolStripMenuItem.Click += new System.EventHandler(this.editRecastConfigurationToolStripMenuItem_Click);
			// 
			// splitter1
			// 
			this.splitter1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.splitter1.Location = new System.Drawing.Point(127, 24);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 518);
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(0, 24);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(127, 518);
			this.panel1.TabIndex = 6;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabNode);
			this.tabControl1.Controls.Add(this.tabFactory);
			this.tabControl1.Controls.Add(this.tabEnv);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(130, 24);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(395, 518);
			this.tabControl1.TabIndex = 7;
			// 
			// tabNode
			// 
			this.tabNode.Controls.Add(this.gridTransform);
			this.tabNode.Location = new System.Drawing.Point(4, 22);
			this.tabNode.Name = "tabNode";
			this.tabNode.Size = new System.Drawing.Size(387, 492);
			this.tabNode.TabIndex = 2;
			this.tabNode.Text = "Node";
			this.tabNode.UseVisualStyleBackColor = true;
			// 
			// gridTransform
			// 
			this.gridTransform.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.gridTransform.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridTransform.Location = new System.Drawing.Point(0, 0);
			this.gridTransform.Name = "gridTransform";
			this.gridTransform.Size = new System.Drawing.Size(387, 492);
			this.gridTransform.TabIndex = 7;
			this.gridTransform.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.gridTransform_PropertyValueChanged);
			// 
			// tabFactory
			// 
			this.tabFactory.Controls.Add(this.gridFactory);
			this.tabFactory.Location = new System.Drawing.Point(4, 22);
			this.tabFactory.Name = "tabFactory";
			this.tabFactory.Size = new System.Drawing.Size(387, 492);
			this.tabFactory.TabIndex = 0;
			this.tabFactory.Text = "Factory";
			this.tabFactory.UseVisualStyleBackColor = true;
			// 
			// gridFactory
			// 
			this.gridFactory.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.gridFactory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridFactory.Location = new System.Drawing.Point(0, 0);
			this.gridFactory.Name = "gridFactory";
			this.gridFactory.Size = new System.Drawing.Size(387, 492);
			this.gridFactory.TabIndex = 6;
			this.gridFactory.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.gridFactory_PropertyValueChanged);
			// 
			// tabEnv
			// 
			this.tabEnv.Controls.Add(this.gridEnv);
			this.tabEnv.Location = new System.Drawing.Point(4, 22);
			this.tabEnv.Name = "tabEnv";
			this.tabEnv.Size = new System.Drawing.Size(387, 492);
			this.tabEnv.TabIndex = 1;
			this.tabEnv.Text = "Environment";
			this.tabEnv.UseVisualStyleBackColor = true;
			// 
			// gridEnv
			// 
			this.gridEnv.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.gridEnv.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridEnv.Location = new System.Drawing.Point(0, 0);
			this.gridEnv.Name = "gridEnv";
			this.gridEnv.Size = new System.Drawing.Size(387, 492);
			this.gridEnv.TabIndex = 7;
			this.gridEnv.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.gridEnv_PropertyValueChanged);
			// 
			// MapEditorControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.mapMenu);
			this.Name = "MapEditorControl";
			this.Size = new System.Drawing.Size(525, 542);
			this.mapMenu.ResumeLayout(false);
			this.mapMenu.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabNode.ResumeLayout(false);
			this.tabFactory.ResumeLayout(false);
			this.tabEnv.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.MenuStrip mapMenu;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ToolStripMenuItem mapToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem displayToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem gridToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem buildToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem visibilityToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem navigationMeshToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem irradianceCacheToolStripMenuItem;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabNode;
		private System.Windows.Forms.PropertyGrid gridTransform;
		private System.Windows.Forms.TabPage tabFactory;
		private System.Windows.Forms.PropertyGrid gridFactory;
		private System.Windows.Forms.TabPage tabEnv;
		private System.Windows.Forms.PropertyGrid gridEnv;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem editRecastConfigurationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem refreshWorldToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem freezeSelectionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem unfreezeAllToolStripMenuItem;
	}
}
