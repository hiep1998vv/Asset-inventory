using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.Aztec;

namespace sda
{
    public partial class camera : Form
    {
        public camera()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new System.Drawing.Point(430, 0);
            this.Hide();
        }
 
        private void Camera1_Click(object sender, EventArgs e)
        {

        }

        private void camera_Load(object sender, EventArgs e)
        {


        }
    }

}
