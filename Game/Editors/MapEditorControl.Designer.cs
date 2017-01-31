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
			this.gridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.showAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hideSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.buildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.visibilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.navigationMeshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.irradianceCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.editRecastConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabTransform = new System.Windows.Forms.TabPage();
			this.gridTransform = new System.Windows.Forms.PropertyGrid();
			this.tabFactory = new System.Windows.Forms.TabPage();
			this.gridFactory = new System.Windows.Forms.PropertyGrid();
			this.tabModel = new System.Windows.Forms.TabPage();
			this.gridModel = new System.Windows.Forms.PropertyGrid();
			this.mapMenu.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabTransform.SuspendLayout();
			this.tabFactory.SuspendLayout();
			this.tabModel.SuspendLayout();
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
            this.gridToolStripMenuItem,
            this.toolStripSeparator1,
            this.showAllToolStripMenuItem,
            this.hideSelectedToolStripMenuItem});
			this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
			this.displayToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
			this.displayToolStripMenuItem.Text = "Display";
			// 
			// gridToolStripMenuItem
			// 
			this.gridToolStripMenuItem.Name = "gridToolStripMenuItem";
			this.gridToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.gridToolStripMenuItem.Text = "Grid";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
			// 
			// showAllToolStripMenuItem
			// 
			this.showAllToolStripMenuItem.Name = "showAllToolStripMenuItem";
			this.showAllToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.showAllToolStripMenuItem.Text = "Show All";
			// 
			// hideSelectedToolStripMenuItem
			// 
			this.hideSelectedToolStripMenuItem.Name = "hideSelectedToolStripMenuItem";
			this.hideSelectedToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.hideSelectedToolStripMenuItem.Text = "Hide Selected";
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
			this.navigationMeshToolStripMenuItem.Text = "Navigation Mesh...";
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
			this.tabControl1.Controls.Add(this.tabTransform);
			this.tabControl1.Controls.Add(this.tabFactory);
			this.tabControl1.Controls.Add(this.tabModel);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(130, 24);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(395, 518);
			this.tabControl1.TabIndex = 7;
			// 
			// tabTransform
			// 
			this.tabTransform.Controls.Add(this.gridTransform);
			this.tabTransform.Location = new System.Drawing.Point(4, 22);
			this.tabTransform.Name = "tabTransform";
			this.tabTransform.Size = new System.Drawing.Size(387, 492);
			this.tabTransform.TabIndex = 2;
			this.tabTransform.Text = "Transform";
			this.tabTransform.UseVisualStyleBackColor = true;
			// 
			// gridTransform
			// 
			this.gridTransform.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.gridTransform.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridTransform.Location = new System.Drawing.Point(0, 0);
			this.gridTransform.Name = "gridTransform";
			this.gridTransform.Size = new System.Drawing.Size(387, 492);
			this.gridTransform.TabIndex = 7;
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
			// 
			// tabModel
			// 
			this.tabModel.Controls.Add(this.gridModel);
			this.tabModel.Location = new System.Drawing.Point(4, 22);
			this.tabModel.Name = "tabModel";
			this.tabModel.Size = new System.Drawing.Size(387, 492);
			this.tabModel.TabIndex = 1;
			this.tabModel.Text = "Model";
			this.tabModel.UseVisualStyleBackColor = true;
			// 
			// gridModel
			// 
			this.gridModel.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.gridModel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridModel.Location = new System.Drawing.Point(0, 0);
			this.gridModel.Name = "gridModel";
			this.gridModel.Size = new System.Drawing.Size(387, 492);
			this.gridModel.TabIndex = 7;
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
			this.tabTransform.ResumeLayout(false);
			this.tabFactory.ResumeLayout(false);
			this.tabModel.ResumeLayout(false);
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
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem showAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hideSelectedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem buildToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem visibilityToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem navigationMeshToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem irradianceCacheToolStripMenuItem;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabTransform;
		private System.Windows.Forms.PropertyGrid gridTransform;
		private System.Windows.Forms.TabPage tabFactory;
		private System.Windows.Forms.PropertyGrid gridFactory;
		private System.Windows.Forms.TabPage tabModel;
		private System.Windows.Forms.PropertyGrid gridModel;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem editRecastConfigurationToolStripMenuItem;
	}
}
