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
			this.objectListBox = new System.Windows.Forms.ListBox();
			this.objectPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.SuspendLayout();
			// 
			// objectListBox
			// 
			this.objectListBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.objectListBox.FormattingEnabled = true;
			this.objectListBox.IntegralHeight = false;
			this.objectListBox.Location = new System.Drawing.Point(0, 0);
			this.objectListBox.Name = "objectListBox";
			this.objectListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.objectListBox.Size = new System.Drawing.Size(181, 517);
			this.objectListBox.Sorted = true;
			this.objectListBox.TabIndex = 3;
			this.objectListBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// objectPropertyGrid
			// 
			this.objectPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objectPropertyGrid.Location = new System.Drawing.Point(185, 0);
			this.objectPropertyGrid.Name = "objectPropertyGrid";
			this.objectPropertyGrid.Size = new System.Drawing.Size(257, 517);
			this.objectPropertyGrid.TabIndex = 4;
			this.objectPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.mainPropertyGrid_PropertyValueChanged);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(181, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(4, 517);
			this.splitter1.TabIndex = 7;
			this.splitter1.TabStop = false;
			// 
			// ObjectEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.objectPropertyGrid);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.objectListBox);
			this.Name = "ObjectEditor";
			this.Size = new System.Drawing.Size(442, 517);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox objectListBox;
		private System.Windows.Forms.PropertyGrid objectPropertyGrid;
		private System.Windows.Forms.Splitter splitter1;
	}
}
