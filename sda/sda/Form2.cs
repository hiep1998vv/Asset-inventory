using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sda
{
    public partial class Image_zoom : Form
    {
        public Image_zoom()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(200, 100);
        }

        private void picZoom__Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
