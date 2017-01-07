namespace IronStar.Editors {
	partial class ObjectEditor {
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
			this.objectListBox = new System.Windows.Forms.ListBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panel2.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// objectPropertyGrid
			// 
			this.objectPropertyGrid.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.objectPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objectPropertyGrid.Location = new System.Drawing.Point(200, 24);
			this.objectPropertyGrid.Name = "objectPropertyGrid";
			this.objectPropertyGrid.Size = new System.Drawing.Size(242, 493);
			this.objectPropertyGrid.TabIndex = 4;
			this.objectPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.mainPropertyGrid_PropertyValueChanged);
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.panel2.Controls.Add(this.objectPropertyGrid);
			this.panel2.Controls.Add(this.splitter1);
			this.panel2.Controls.Add(this.objectListBox);
			this.panel2.Controls.Add(this.menuStrip1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(442, 517);
			this.panel2.TabIndex = 9;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(197, 24);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 493);
			this.splitter1.TabIndex = 7;
			this.splitter1.TabStop = false;
			// 
			// objectListBox
			// 
			this.objectListBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.objectListBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.objectListBox.FormattingEnabled = true;
			this.objectListBox.IntegralHeight = false;
			this.objectListBox.Location = new System.Drawing.Point(0, 24);
			this.objectListBox.Name = "objectListBox";
			this.objectListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.objectListBox.Size = new System.Drawing.Size(197, 493);
			this.objectListBox.Sorted = true;
			this.objectListBox.TabIndex = 5;
			this.objectListBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// menuStrip1
			// 
			this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.removeToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(442, 24);
			this.menuStrip1.TabIndex = 6;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// addToolStripMenuItem
			// 
			this.addToolStripMenuItem.Name = "addToolStripMenuItem";
			this.addToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
			this.addToolStripMenuItem.Text = "New...";
			this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
			// 
			// removeToolStripMenuItem
			// 
			this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
			this.removeToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
			this.removeToolStripMenuItem.Text = "Delete";
			this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
			// 
			// ObjectEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel2);
			this.Name = "ObjectEditor";
			this.Size = new System.Drawing.Size(442, 517);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.PropertyGrid objectPropertyGrid;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ListBox objectListBox;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
	}
}
