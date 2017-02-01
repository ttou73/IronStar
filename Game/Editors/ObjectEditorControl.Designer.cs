namespace IronStar.Editors {
	partial class ObjectEditorControl {
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
			this.objectPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.panel2 = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.nameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sendObjectToMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.objectListBox = new System.Windows.Forms.ListBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.panel2.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// objectPropertyGrid
			// 
			this.objectPropertyGrid.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.objectPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objectPropertyGrid.Location = new System.Drawing.Point(203, 24);
			this.objectPropertyGrid.Name = "objectPropertyGrid";
			this.objectPropertyGrid.Size = new System.Drawing.Size(239, 493);
			this.objectPropertyGrid.TabIndex = 4;
			this.objectPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.mainPropertyGrid_PropertyValueChanged);
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.panel2.Controls.Add(this.objectPropertyGrid);
			this.panel2.Controls.Add(this.splitter1);
			this.panel2.Controls.Add(this.panel1);
			this.panel2.Controls.Add(this.menuStrip1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(442, 517);
			this.panel2.TabIndex = 9;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(200, 24);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 493);
			this.splitter1.TabIndex = 7;
			this.splitter1.TabStop = false;
			// 
			// menuStrip1
			// 
			this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nameToolStripMenuItem,
            this.actionsToolStripMenuItem,
            this.mappingToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(442, 24);
			this.menuStrip1.TabIndex = 6;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// nameToolStripMenuItem
			// 
			this.nameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.deleteToolStripMenuItem});
			this.nameToolStripMenuItem.Name = "nameToolStripMenuItem";
			this.nameToolStripMenuItem.Size = new System.Drawing.Size(67, 23);
			this.nameToolStripMenuItem.Text = "<Name>";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
			this.newToolStripMenuItem.Text = "New...";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
			this.deleteToolStripMenuItem.Text = "Delete";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
			// 
			// actionsToolStripMenuItem
			// 
			this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
			this.actionsToolStripMenuItem.Size = new System.Drawing.Size(59, 23);
			this.actionsToolStripMenuItem.Text = "Actions";
			// 
			// mappingToolStripMenuItem
			// 
			this.mappingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendObjectToMapToolStripMenuItem});
			this.mappingToolStripMenuItem.Name = "mappingToolStripMenuItem";
			this.mappingToolStripMenuItem.Size = new System.Drawing.Size(67, 23);
			this.mappingToolStripMenuItem.Text = "Mapping";
			// 
			// sendObjectToMapToolStripMenuItem
			// 
			this.sendObjectToMapToolStripMenuItem.Name = "sendObjectToMapToolStripMenuItem";
			this.sendObjectToMapToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
			this.sendObjectToMapToolStripMenuItem.Text = "Send Object to Map";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.objectListBox);
			this.panel1.Controls.Add(this.splitter2);
			this.panel1.Controls.Add(this.comboBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(0, 24);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(200, 493);
			this.panel1.TabIndex = 8;
			// 
			// objectListBox
			// 
			this.objectListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objectListBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.objectListBox.FormattingEnabled = true;
			this.objectListBox.IntegralHeight = false;
			this.objectListBox.Location = new System.Drawing.Point(0, 24);
			this.objectListBox.Name = "objectListBox";
			this.objectListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.objectListBox.Size = new System.Drawing.Size(200, 469);
			this.objectListBox.Sorted = true;
			this.objectListBox.TabIndex = 6;
			this.objectListBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// comboBox1
			// 
			this.comboBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.comboBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.IntegralHeight = false;
			this.comboBox1.Items.AddRange(new object[] {
            "box.fbx",
            "boxes",
            "characters",
            "commCenter.fbx",
            "commCenter.mlt",
            "envLight.tga",
            "mtrlTest.fbx",
            "mtrlTest.mlt",
            "mtrlTestOld.mlt",
            "test",
            "textures",
            "weapon"});
			this.comboBox1.Location = new System.Drawing.Point(0, 0);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(200, 21);
			this.comboBox1.TabIndex = 7;
			// 
			// splitter2
			// 
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter2.Location = new System.Drawing.Point(0, 21);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(200, 3);
			this.splitter2.TabIndex = 8;
			this.splitter2.TabStop = false;
			// 
			// ObjectEditorControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel2);
			this.Name = "ObjectEditorControl";
			this.Size = new System.Drawing.Size(442, 517);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.PropertyGrid objectPropertyGrid;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem nameToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem actionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mappingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sendObjectToMapToolStripMenuItem;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ListBox objectListBox;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Splitter splitter2;
	}
}
