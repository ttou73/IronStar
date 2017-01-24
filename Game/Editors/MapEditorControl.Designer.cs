﻿namespace IronStar.Editors {
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
			this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.propertyGridFactory = new System.Windows.Forms.PropertyGrid();
			this.mapListBox = new System.Windows.Forms.ListBox();
			this.buildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.visibilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.navigationMeshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hideSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.irradianceCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mapMenu.SuspendLayout();
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
			// splitter1
			// 
			this.splitter1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.splitter1.Location = new System.Drawing.Point(206, 24);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 518);
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// propertyGridFactory
			// 
			this.propertyGridFactory.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.propertyGridFactory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGridFactory.Location = new System.Drawing.Point(209, 24);
			this.propertyGridFactory.Name = "propertyGridFactory";
			this.propertyGridFactory.Size = new System.Drawing.Size(316, 518);
			this.propertyGridFactory.TabIndex = 5;
			// 
			// mapListBox
			// 
			this.mapListBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.mapListBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.mapListBox.FormattingEnabled = true;
			this.mapListBox.IntegralHeight = false;
			this.mapListBox.Items.AddRange(new object[] {
            "<MapFactory>",
            "<Guid>152a5316-2360-44e7-b652-fa2e2dce5757</Guid>",
            "<Transform>",
            "<Translation X=\"5\" Y=\"0\" Z=\"0\" />",
            "<Rotation X=\"0\" Y=\"0\" Z=\"0\" W=\"1\" />",
            "<Scaling>1</Scaling>",
            "<Visible>true</Visible>",
            "<Frozen>false</Frozen>",
            "</Transform>",
            "<Factory xsi:type=\"StaticModelFactory\">",
            "<Visible>true</Visible>",
            "<Collidable>true</Collidable>",
            "</Factory>"});
			this.mapListBox.Location = new System.Drawing.Point(0, 24);
			this.mapListBox.Name = "mapListBox";
			this.mapListBox.Size = new System.Drawing.Size(206, 518);
			this.mapListBox.TabIndex = 6;
			// 
			// buildToolStripMenuItem
			// 
			this.buildToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.visibilityToolStripMenuItem,
            this.navigationMeshToolStripMenuItem,
            this.irradianceCacheToolStripMenuItem});
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
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.saveToolStripMenuItem.Text = "Save";
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.saveAsToolStripMenuItem.Text = "Save As...";
			// 
			// gridToolStripMenuItem
			// 
			this.gridToolStripMenuItem.Name = "gridToolStripMenuItem";
			this.gridToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.gridToolStripMenuItem.Text = "Grid";
			// 
			// showAllToolStripMenuItem
			// 
			this.showAllToolStripMenuItem.Name = "showAllToolStripMenuItem";
			this.showAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.showAllToolStripMenuItem.Text = "Show All";
			// 
			// hideSelectedToolStripMenuItem
			// 
			this.hideSelectedToolStripMenuItem.Name = "hideSelectedToolStripMenuItem";
			this.hideSelectedToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.hideSelectedToolStripMenuItem.Text = "Hide Selected";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
			// 
			// irradianceCacheToolStripMenuItem
			// 
			this.irradianceCacheToolStripMenuItem.Name = "irradianceCacheToolStripMenuItem";
			this.irradianceCacheToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.irradianceCacheToolStripMenuItem.Text = "Irradiance Cache...";
			// 
			// MapEditorControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.propertyGridFactory);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.mapListBox);
			this.Controls.Add(this.mapMenu);
			this.Name = "MapEditorControl";
			this.Size = new System.Drawing.Size(525, 542);
			this.mapMenu.ResumeLayout(false);
			this.mapMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.MenuStrip mapMenu;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ToolStripMenuItem mapToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem displayToolStripMenuItem;
		private System.Windows.Forms.PropertyGrid propertyGridFactory;
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
		private System.Windows.Forms.ListBox mapListBox;
	}
}