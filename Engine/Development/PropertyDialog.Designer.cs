namespace Fusion.Development {
	partial class PropertyDialog {
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
			this.buttonClose = new System.Windows.Forms.Button();
			this.mainPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.SuspendLayout();
			// 
			// buttonClose
			// 
			this.buttonClose.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonClose.Location = new System.Drawing.Point(0, 431);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(341, 29);
			this.buttonClose.TabIndex = 0;
			this.buttonClose.Text = "OK";
			this.buttonClose.UseVisualStyleBackColor = true;
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// mainPropertyGrid
			// 
			this.mainPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainPropertyGrid.Location = new System.Drawing.Point(0, 0);
			this.mainPropertyGrid.Name = "mainPropertyGrid";
			this.mainPropertyGrid.Size = new System.Drawing.Size(341, 431);
			this.mainPropertyGrid.TabIndex = 1;
			// 
			// PropertyDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(341, 460);
			this.Controls.Add(this.mainPropertyGrid);
			this.Controls.Add(this.buttonClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "PropertyDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "PropertyDialog";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.PropertyGrid mainPropertyGrid;
	}
}