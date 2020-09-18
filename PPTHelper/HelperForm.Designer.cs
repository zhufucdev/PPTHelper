namespace PPTHelper
{
    partial class HelperForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelperForm));
            this.penOptionBox = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cursorBox = new System.Windows.Forms.PictureBox();
            this.penBox = new System.Windows.Forms.PictureBox();
            this.eraserBox = new System.Windows.Forms.PictureBox();
            this.rightBox = new System.Windows.Forms.PictureBox();
            this.leftBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.penOptionBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cursorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.penBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eraserBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftBox)).BeginInit();
            this.SuspendLayout();
            // 
            // penOptionBox
            // 
            this.penOptionBox.BackColor = System.Drawing.Color.Transparent;
            this.penOptionBox.Image = global::PPTHelper.Properties.Resources.up;
            this.penOptionBox.Location = new System.Drawing.Point(174, 43);
            this.penOptionBox.Name = "penOptionBox";
            this.penOptionBox.Size = new System.Drawing.Size(30, 18);
            this.penOptionBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.penOptionBox.TabIndex = 4;
            this.penOptionBox.TabStop = false;
            this.penOptionBox.Click += new System.EventHandler(this.penOptionBox_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::PPTHelper.Properties.Resources.close;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Location = new System.Drawing.Point(280, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // cursorBox
            // 
            this.cursorBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.cursorBox.BackColor = System.Drawing.Color.Transparent;
            this.cursorBox.BackgroundImage = global::PPTHelper.Properties.Resources.cursor;
            this.cursorBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.cursorBox.Location = new System.Drawing.Point(70, 0);
            this.cursorBox.Name = "cursorBox";
            this.cursorBox.Size = new System.Drawing.Size(64, 64);
            this.cursorBox.TabIndex = 1;
            this.cursorBox.TabStop = false;
            this.cursorBox.Click += new System.EventHandler(this.cursorBox_Click);
            // 
            // penBox
            // 
            this.penBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.penBox.BackColor = System.Drawing.Color.Transparent;
            this.penBox.BackgroundImage = global::PPTHelper.Properties.Resources.pencil;
            this.penBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.penBox.Location = new System.Drawing.Point(140, 0);
            this.penBox.Name = "penBox";
            this.penBox.Size = new System.Drawing.Size(64, 64);
            this.penBox.TabIndex = 2;
            this.penBox.TabStop = false;
            this.penBox.Click += new System.EventHandler(this.penBox_Click);
            // 
            // eraserBox
            // 
            this.eraserBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.eraserBox.BackColor = System.Drawing.Color.Transparent;
            this.eraserBox.BackgroundImage = global::PPTHelper.Properties.Resources.eraser;
            this.eraserBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.eraserBox.Location = new System.Drawing.Point(210, 0);
            this.eraserBox.Name = "eraserBox";
            this.eraserBox.Size = new System.Drawing.Size(64, 64);
            this.eraserBox.TabIndex = 3;
            this.eraserBox.TabStop = false;
            this.eraserBox.Click += new System.EventHandler(this.eraserBox_Click);
            // 
            // rightBox
            // 
            this.rightBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.rightBox.BackColor = System.Drawing.Color.Transparent;
            this.rightBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rightBox.BackgroundImage")));
            this.rightBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rightBox.Location = new System.Drawing.Point(350, 0);
            this.rightBox.Name = "rightBox";
            this.rightBox.Size = new System.Drawing.Size(64, 64);
            this.rightBox.TabIndex = 1;
            this.rightBox.TabStop = false;
            this.rightBox.Click += new System.EventHandler(this.rightBox_Click);
            // 
            // leftBox
            // 
            this.leftBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.leftBox.BackColor = System.Drawing.Color.Transparent;
            this.leftBox.BackgroundImage = global::PPTHelper.Properties.Resources.previous;
            this.leftBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.leftBox.Location = new System.Drawing.Point(0, 0);
            this.leftBox.Name = "leftBox";
            this.leftBox.Size = new System.Drawing.Size(64, 64);
            this.leftBox.TabIndex = 0;
            this.leftBox.TabStop = false;
            this.leftBox.Click += new System.EventHandler(this.leftBox_Click);
            // 
            // HelperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(119F, 119F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(416, 65);
            this.ControlBox = false;
            this.Controls.Add(this.penOptionBox);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.cursorBox);
            this.Controls.Add(this.penBox);
            this.Controls.Add(this.eraserBox);
            this.Controls.Add(this.rightBox);
            this.Controls.Add(this.leftBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimizeBox = false;
            this.Name = "HelperForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "PPTHelper";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.penOptionBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cursorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.penBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eraserBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox leftBox;
        private System.Windows.Forms.PictureBox rightBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox cursorBox;
        private System.Windows.Forms.PictureBox penBox;
        private System.Windows.Forms.PictureBox eraserBox;
        private System.Windows.Forms.PictureBox penOptionBox;
    }
}