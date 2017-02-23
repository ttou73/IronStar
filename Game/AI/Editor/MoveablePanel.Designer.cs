namespace IronStar.AI.Editor
{
    partial class MoveablePanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MoveablePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "MoveablePanel";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveablePanel_MouseDown);
            this.MouseEnter += new System.EventHandler(this.MoveablePanel_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.MoveablePanel_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MoveablePanel_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
