namespace PPTHelper
{
    partial class PenOptionForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.colorPanel1 = new PPTHelper.ColorPanel();
            this.SuspendLayout();
            // 
            // colorPanel1
            // 
            this.colorPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.colorPanel1.CellSize = 46;
            this.colorPanel1.ColorSelcted = System.Drawing.Color.Empty;
            this.colorPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colorPanel1.Location = new System.Drawing.Point(5, 5);
            this.colorPanel1.Name = "colorPanel1";
            this.colorPanel1.Size = new System.Drawing.Size(369, 91);
            this.colorPanel1.TabIndex = 0;
            // 
            // PenOptionForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(379, 101);
            this.Controls.Add(this.colorPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PenOptionForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "PenOptionForm";
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.PenOptionForm_Deactivate);
            this.ResumeLayout(false);

        }
        #endregion

        private ColorPanel colorPanel1;
    }
}