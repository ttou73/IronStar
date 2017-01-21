namespace IronStar.Editors {
	partial class AttributeEditor {
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabTransform = new System.Windows.Forms.TabPage();
			this.tabFactory = new System.Windows.Forms.TabPage();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.propertyGrid2 = new System.Windows.Forms.PropertyGrid();
			this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabTransform.SuspendLayout();
			this.tabFactory.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(253, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabTransform);
			this.tabControl1.Controls.Add(this.tabFactory);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 24);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(253, 527);
			this.tabControl1.TabIndex = 1;
			// 
			// tabTransform
			// 
			this.tabTransform.Controls.Add(this.propertyGrid1);
			this.tabTransform.Location = new System.Drawing.Point(4, 22);
			this.tabTransform.Name = "tabTransform";
			this.tabTransform.Padding = new System.Windows.Forms.Padding(3);
			this.tabTransform.Size = new System.Drawing.Size(260, 492);
			this.tabTransform.TabIndex = 0;
			this.tabTransform.Text = "Transform";
			this.tabTransform.UseVisualStyleBackColor = true;
			// 
			// tabFactory
			// 
			this.tabFactory.Controls.Add(this.propertyGrid2);
			this.tabFactory.Location = new System.Drawing.Point(4, 22);
			this.tabFactory.Name = "tabFactory";
			this.tabFactory.Padding = new System.Windows.Forms.Padding(3);
			this.tabFactory.Size = new System.Drawing.Size(245, 501);
			this.tabFactory.TabIndex = 1;
			this.tabFactory.Text = "Factory";
			this.tabFactory.UseVisualStyleBackColor = true;
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid1.Location = new System.Drawing.Point(3, 3);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(254, 486);
			this.propertyGrid1.TabIndex = 0;
			// 
			// propertyGrid2
			// 
			this.propertyGrid2.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.propertyGrid2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid2.Location = new System.Drawing.Point(3, 3);
			this.propertyGrid2.Name = "propertyGrid2";
			this.propertyGrid2.Size = new System.Drawing.Size(239, 495);
			this.propertyGrid2.TabIndex = 1;
			// 
			// createToolStripMenuItem
			// 
			this.createToolStripMenuItem.Name = "createToolStripMenuItem";
			this.createToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
			this.createToolStripMenuItem.Text = "Create";
			// 
			// AttributeEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(253, 551);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "AttributeEditor";
			this.Text = "AttributeEditor";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabTransform.ResumeLayout(false);
			this.tabFactory.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabTransform;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.TabPage tabFactory;
		private System.Windows.Forms.PropertyGrid propertyGrid2;
	}
}