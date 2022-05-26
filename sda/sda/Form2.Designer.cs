namespace sda
{
    partial class Image_zoom
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
            this.picZoom_ = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picZoom_)).BeginInit();
            this.SuspendLayout();
            // 
            // picZoom_
            // 
            this.picZoom_.Location = new System.Drawing.Point(6, 4);
            this.picZoom_.Name = "picZoom_";
            this.picZoom_.Size = new System.Drawing.Size(870, 600);
            this.picZoom_.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picZoom_.TabIndex = 0;
            this.picZoom_.TabStop = false;
            this.picZoom_.Click += new System.EventHandler(this.picZoom__Click);
            // 
            // Image_zoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 620);
            this.Controls.Add(this.picZoom_);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Image_zoom";
            this.Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.picZoom_)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox picZoom_;
    }
}