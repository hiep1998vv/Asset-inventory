namespace sda
{
    partial class camera
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
            this.Camera1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Camera1)).BeginInit();
            this.SuspendLayout();
            // 
            // Camera1
            // 
            this.Camera1.Location = new System.Drawing.Point(1, 6);
            this.Camera1.Name = "Camera1";
            this.Camera1.Size = new System.Drawing.Size(1000, 871);
            this.Camera1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Camera1.TabIndex = 0;
            this.Camera1.TabStop = false;
            this.Camera1.Click += new System.EventHandler(this.Camera1_Click);
            // 
            // camera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 881);
            this.Controls.Add(this.Camera1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "camera";
            this.Text = "camera";
            this.Load += new System.EventHandler(this.camera_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Camera1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox Camera1;
    }
}