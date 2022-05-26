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
    public partial class MainProg : Form
    {
        public MainProg()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new System.Drawing.Point(0, 0);
            //this.Width = 400;
        }

        public FilterInfoCollection Capturedevice;
        public VideoCaptureDevice finalFrame;

        public camera cam = new camera();
        public void Form1_Load(object sender, EventArgs e)
        {
            // use for Camera QRCode scanner
            Capturedevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in Capturedevice)
            {
                comboBox1.Items.Add(Device.Name);
            }
            comboBox1.SelectedIndex = 0;
            finalFrame = new VideoCaptureDevice();
            
            
            // use for auto choose month inventory
            DateTime dt = DateTime.Now;
            string year_now = dt.Year.ToString();
            int month_now = dt.Month;
            
            month_show.SelectedIndex = (month_now - 1);

            // use this for visualize check data
            chart1.Titles.Add("Checked data information");
            DataTable dtView = Common.FillData(@"select Equipments.ID, Equipments.Name, Equipments.Serial, Equipments.FA_No, Equipments.ENG_control_no, Equipments.Picture, Equipments.Cabinet, Equipments.Describle, Equipments.Quantity, Category.Category, Category.Location from Equipments join Category on Equipments.Category_ID = Category.ID");
            DataTable dt_checked = Common.FillData(@"select * from Equipments join Category on Equipments.Category_ID=Category.ID join All_Inventory on Equipments.ID= All_Inventory.ID_items where All_inventory.Month_Inven = '" + month_show.Text + "' and All_inventory.Year_Inven = '" + year_now + "'");
            showdata(dtView, dt_checked);

            // change chart's area backcolor 
            chart1.ChartAreas[0].BackColor = Color.WhiteSmoke;
            chart1.Legends[0].BackColor = Color.WhiteSmoke;

            reset_info();

            dataGridView1.ReadOnly = true;
            list_location.Text = "All";
        }


        void finalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        { 
            cam.Camera1.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        public void button1_Click(object sender, EventArgs e)   // button turn on camera
        {
            if (label1.Text == "No Action!" || label1.Text == "Finish!")
            {
                button1.BackColor = Color.Red;
                button1.Text = "Turn off Camera";
                finalFrame = new VideoCaptureDevice(Capturedevice[comboBox1.SelectedIndex].MonikerString);
                finalFrame.NewFrame += new NewFrameEventHandler(finalFrame_NewFrame);
                cam.Show();
                this.Width = 450;

                string cameratype = comboBox1.Text;
                switch (cameratype)
                {
                    case "Logitech Webcam C930e":
                        finalFrame.SetCameraProperty(CameraControlProperty.Zoom, 100, CameraControlFlags.Manual);
                        finalFrame.SetCameraProperty(CameraControlProperty.Focus, 0, CameraControlFlags.Manual);
                        break;
                    case "Surface Camera Rear":
                        finalFrame.SetCameraProperty(CameraControlProperty.Zoom, 1, CameraControlFlags.Manual);
                        finalFrame.SetCameraProperty(CameraControlProperty.Focus, 100, CameraControlFlags.Manual);
                        break;
                    default:
                        break;
                }

                cam.Camera1.Image = null;
                cam.Camera1.Visible = true;


                timer1.Enabled = true;
                label1.Text = "Scanning.......!";
                txtQR.Text = "";
                timer1.Start();
                finalFrame.Start();
            }
            else
            {   
                button1.BackColor = Color.FromArgb(128,255,128);  
                button1.Text = "Turn on Camera";
                timer1.Stop();
                label1.Text = "No Action!";               
                finalFrame.Stop();
                cam.Camera1.Visible = false;
                nomal_tablet_focus = 300;
                nomal_webcam_focus = 0;
                cam.Hide();
                this.Width = 1460;
            }
            
        }


        public void timer1_Tick(object sender, EventArgs e) // take qrcode
        {

            BarcodeReader reader = new BarcodeReader();
            if ((Bitmap)cam.Camera1.Image != null)
            {
                Result re1 = reader.Decode((Bitmap)cam.Camera1.Image);
                try
                {
                    string decoded1 = re1.ToString().Trim();
                    if (decoded1 != "")
                    {
                        timer1.Stop();
                        label1.Text = "Finish!";
                        txtQR.Text = decoded1;
                        finalFrame.Stop();
                        cam.Camera1.Visible = false;
                        nomal_tablet_focus = 300;
                        nomal_webcam_focus = 0;
                        cam.Hide();
                        this.Width = 1460;
                        button1.BackColor = Color.FromArgb(128, 255, 128);
                        button1.Text = "Turn on Camera";
                    }
                }
                catch (Exception) { }
            }
        }
        public void comboBox1_SelectedIndexChanged(object sender, EventArgs e)  //event when change camera --> reset all variables
        {
            if (finalFrame != null)
            {
                button1.BackColor = Color.FromArgb(128, 255, 128);
                button1.Text = "Turn on Camera";
                timer1.Stop();
                finalFrame.Stop();
                txtQR.Text = "";
                cam.Camera1.Image = null;
                cam.Hide();
                label1.Text = "No Action!";
            }
        }

        public int nomal_webcam_focus = 0; // define normal webcam focus 
        public int nomal_tablet_focus = 300; // define normal tablet focus

        private void btn_set_vide0_Click(object sender, EventArgs e) // turn on the camera display setting 
        {
            finalFrame.DisplayPropertyPage(IntPtr.Zero);
        }

        private void Zoom_point_Scroll(object sender, EventArgs e)
        {
            //textBox1.Text = Zoom_point.Value.ToString();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
        private void Zoom_in_Click(object sender, EventArgs e) // zoom in and get focus on qrcode
        {
            string cameratype = comboBox1.Text;
            switch (cameratype)
            {
                case "Logitech Webcam C930e":
                    finalFrame.SetCameraProperty(CameraControlProperty.Zoom, 400, CameraControlFlags.Manual);
                    finalFrame.SetCameraProperty(CameraControlProperty.Focus, nomal_webcam_focus, CameraControlFlags.Manual);
                    nomal_webcam_focus += 5;
                    break;
                case "Surface Camera Rear":
                    finalFrame.SetCameraProperty(CameraControlProperty.Zoom, 50, CameraControlFlags.Manual);
                    finalFrame.SetCameraProperty(CameraControlProperty.Focus, nomal_tablet_focus, CameraControlFlags.Manual);
                    nomal_tablet_focus += 5;
                    break;
                default:
                    break;
            }
        }
        private void Zoom_out_Click(object sender, EventArgs e) // zoom out and reset the focus value
        {
            string cameratype = comboBox1.Text;
            switch (cameratype)
            {
                case "Logitech Webcam C930e":
                    finalFrame.SetCameraProperty(CameraControlProperty.Zoom, 100, CameraControlFlags.Manual);
                    finalFrame.SetCameraProperty(CameraControlProperty.Focus, 0, CameraControlFlags.Manual);
                    break;
                case "Surface Camera Rear":
                    finalFrame.SetCameraProperty(CameraControlProperty.Zoom, 1, CameraControlFlags.Manual);
                    finalFrame.SetCameraProperty(CameraControlProperty.Focus, 300, CameraControlFlags.Manual);
                    break;
                default:
                    finalFrame.SetCameraProperty(CameraControlProperty.Zoom, 2, CameraControlFlags.Manual);
                    finalFrame.SetCameraProperty(CameraControlProperty.Focus, 300, CameraControlFlags.Manual);
                    break;
            }
            nomal_webcam_focus = 0;
            nomal_tablet_focus = 300;
        }
        public void button4_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)  // button get information - color:green
        {
            string result_QR = txtQR.Text;
            DataTable dt = Common.FillData(@"select * from Equipments join Category on Equipments.Category_ID = Category.ID where Equipments.ID='" + result_QR + "' ");
            
            if (dt.Rows.Count > 0)
            {
                change_color();
                equiqName.Text = dt.Rows[0]["Name"].ToString().Trim();
                Category.Text = dt.Rows[0]["Category"].ToString().Trim();
                Cabinet.Text = dt.Rows[0]["Cabinet"].ToString().Trim();
                locat_ion.Text = dt.Rows[0]["Location"].ToString().Trim();
                Quantity.Text = dt.Rows[0]["Quantity"].ToString().Trim();
                
                string img_path = @"\\cvn-veng\Code\KeyResource\" + dt.Rows[0]["Picture"].ToString().Trim();
                picEquiq.Image = System.Drawing.Image.FromFile(img_path);

                DateTime dateTime = DateTime.Now;
                string year_inv = dateTime.Year.ToString();
                string month_inv = Common.convert_month(dateTime.Month);
                DataTable dtstatus = Common.FillData(@"select * from All_Inventory where Type= 'Equipment' and Year_Inven = '" + year_inv + "' and Month_Inven = '" + month_inv + "' and ID_items = '" + txtQR.Text + "'");
                if (dtstatus.Rows.Count > 0)
                {
                    status.Text = dtstatus.Rows[0]["Result"].ToString().Trim();
                    status.ForeColor = Color.BlueViolet;
                    btnSubmit.Hide();
                    btnUpdate.Show();
                }
                else
                {
                    status.Text = "Not checked in " + month_inv + "!!";
                    status.ForeColor = Color.Red;
                    btnUpdate.Hide();
                    btnSubmit.Show();
                }


            }
            else
            {
                MessageBox.Show("No Items!");
            }
        }

        private void name_user_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) // event take all the data of cabinet in selected location
        {
            DataTable dt_Cabinet = Common.FillData(@"select distinct Equipments.Cabinet from Equipments join Category on Equipments.Category_ID = Category.ID where Category.Location = '" + list_location.Text + "'");
            list_cabinet.Items.Clear();
            for (int i = 0; i < dt_Cabinet.Rows.Count; i++)
            {
                list_cabinet.Items.Add(dt_Cabinet.Rows[i][0].ToString());
            }
            if (list_location.Text == "All")
            {
                DateTime dt = DateTime.Now;
                string year_now = dt.Year.ToString();
                DataTable dtView = Common.FillData(@"select Equipments.ID, Equipments.Name, Equipments.Serial, Equipments.FA_No, Equipments.ENG_control_no, Equipments.Picture, Equipments.Cabinet, Equipments.Describle, Equipments.Quantity, Category.Category, Category.Location from Equipments join Category on Equipments.Category_ID = Category.ID");
                DataTable dt_checked = Common.FillData(@"select * from Equipments join Category on Equipments.Category_ID=Category.ID join All_Inventory on Equipments.ID= All_Inventory.ID_items where All_Inventory.Type= 'Equipment' and All_inventory.Month_Inven = '" + month_show.Text + "' and All_inventory.Year_Inven = '" + year_now + "'");
                showdata(dtView, dt_checked);
            }
        }

        private void panel20_Paint(object sender, PaintEventArgs e)
        {

        }

        public void change_color()
        {
            equiqName.ForeColor = Color.BlueViolet;
            Cabinet.ForeColor = Color.BlueViolet;
            status.ForeColor = Color.BlueViolet;
            Quantity.ForeColor = Color.BlueViolet;
            locat_ion.ForeColor = Color.BlueViolet;
            Category.ForeColor = Color.BlueViolet;
            lbReason.ForeColor = Color.BlueViolet;

        }

        public void reset_info() // reset label after check or update
        {
            equiqName.Text = "No Information";
            equiqName.ForeColor = Color.Silver;
            Cabinet.Text = "No Information";
            Cabinet.ForeColor = Color.Silver;
            status.Text = "No Information";
            status.ForeColor = Color.Silver;
            Quantity.Text = "No Information";
            Quantity.ForeColor = Color.Silver;
            locat_ion.Text = "No Information";
            locat_ion.ForeColor = Color.Silver;
            Category.Text = "No Information";
            Category.ForeColor = Color.Silver;
            lbOldCabinet.ForeColor = Color.Silver;
            lbOldLocation.ForeColor = Color.Silver;
            lbOldName.ForeColor = Color.Silver;
            lbReason.ForeColor= Color.Silver;   
            txtQR.Text = "";
            txtReasonNG.Text = "";
            CheckOK.Checked = false;
            CheckNG.Checked = false;
            picEquiq.Image = System.Drawing.Image.FromFile(@"\\cvn-filesv\PROJECT\SPECIAL\020.ENG DIV DATA\TIM\Hiep\blank.PNG");
            status.ForeColor = Color.Silver;
        }

        private void button3_Click(object sender, EventArgs e) // this button is for 'submit new status' function
        {
            DateTime myDateTime = DateTime.Now;
            string year_inven = myDateTime.Year.ToString();
            string month_inven = Common.convert_month(myDateTime.Month); // 'convert month' function is built on common.cs
                                                                         // convert int to string , ex: 3 --> Mar
            string type = "Equipment";
            string Id_Items = txtQR.Text;
            string Name_Items = equiqName.Text;
            string location_insert = locat_ion.Text;
            string cabinet_insert = Cabinet.Text;
            string pic_inven = labelCode.Text;
            string date_inven = myDateTime.ToString().Trim();

            

            if (equiqName.Text == "No Information")
            {
                MessageBox.Show("No infomation to submit!\nCheck your input....!");
            }
            else
            {
                DataTable dtcheck = Common.FillData(@"select * from All_Inventory where Type= 'Equipment' and Year_Inven = '" + year_inven + "' and Month_Inven = '" + month_inven + "' and ID_items = '" + Id_Items + "'");
                if (dtcheck.Rows.Count > 0)
                {
                    MessageBox.Show("This items's already been checked!\nIf you want to update status, pls choose 'Update' button!!");
                }
                else
                {
                    if (CheckOK.Checked == true && CheckNG.Checked == false)
                    {
                        if (txtReasonNG.Text != "")
                        {
                            MessageBox.Show("No Reason NG on OK item.\nPls clear this before Submiting!");
                        }
                        else
                        {
                            DialogResult dr = MessageBox.Show("Are you sure to submit this item 'OK'?",
                                                     "Submit", MessageBoxButtons.YesNo);
                            switch (dr)
                            {
                                case DialogResult.Yes:
                                    string Status_insert = "OK";
                                    Common.Excute_SQL(@"insert into All_Inventory (Type, ID_items, Name_items, Location, Cabinet, Result, Year_Inven, Month_Inven, PIC_Inven, Date_inven)" +
                                                      "values ('" + type + "', '" + Id_Items + "', '" + Name_Items + "', '" + location_insert + "', '" + cabinet_insert + "', '" + Status_insert + "', '" + year_inven + "', '" + month_inven + "', '" + pic_inven + "', '" + date_inven + "' )");
                                    MessageBox.Show("This item is checked!");
                                    reset_info();
                                    break;
                                case DialogResult.No:
                                    break;
                            }
                            
                        }

                    }
                    else if (CheckOK.Checked == false && CheckNG.Checked == true)
                    {
                        if (txtReasonNG.Text == "")
                        {
                            MessageBox.Show("Pls input Reason NG!");
                        }
                        else
                        {
                            DialogResult dr = MessageBox.Show("Are you sure to submit this item 'NG'?",
                                              "Submit", MessageBoxButtons.YesNo);
                            switch (dr)
                            {
                                case DialogResult.Yes:
                                    string Status_insert = "NG";
                                    Common.Excute_SQL(@"insert into All_Inventory (Type, ID_items, Name_items, Location, Cabinet, Result, Year_Inven, Month_Inven, PIC_Inven, Date_inven, NGReason)" +
                                                      "values ('" + type + "', '" + Id_Items + "', '" + Name_Items + "', '" + location_insert + "', '" + cabinet_insert + "', '" + Status_insert + "', '" + year_inven + "', '" + month_inven + "', '" + pic_inven + "', '" + date_inven + "', '" + txtReasonNG.Text.Replace("'","") + "' )");
                                    MessageBox.Show("This item is checked!");
                                    reset_info();
                                    break;
                                case DialogResult.No:
                                    break;
                            }
                            
                        }
                    }
                    else
                    {
                        MessageBox.Show("Pls choose 1 status option!");
                    }
                }
            }
            if (list_location.Text != "" & list_cabinet.Text != "")
            {
                show_check_notcheck_data();
            }
            else
            {

            }
        }
        void display_autosize() // display fit size in datagridview table
        {
            for (int i = 0; i < 11; i++)
            {
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
        }

        public int calculate_percent(int a, int b) // function calculate 'checked' percent, 'not checked' percent --> use in 'showdata' function
        {   
            if (a != 0)
            {
                int checked_percent = (100 * b / a);
                int notchecked_percent = 100 - checked_percent;
                if (notchecked_percent > 0)
                {
                    return notchecked_percent;
                }
                else
                {
                    return 0;
                }
            }
            else { return 0; }
           
            


        }

        private void button6_Click(object sender, EventArgs e)
        {
            
        }
        public string coder = "Hiep"; // meo biet de lam gi nua :v
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) // 'cell click' event to show data in datagridview table
        {

            if (dataGridView1.Rows[e.RowIndex].Cells["Name"].Value.ToString() == "")
            {
                MessageBox.Show("No Item found!!!!");
            }
            else
            {
                change_color();
                dataGridView1.CurrentRow.Selected = true;
                equiqName.Text = dataGridView1.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                Category.Text = dataGridView1.Rows[e.RowIndex].Cells["Category"].Value.ToString();
                Cabinet.Text = dataGridView1.Rows[e.RowIndex].Cells["Cabinet"].Value.ToString();
                locat_ion.Text = dataGridView1.Rows[e.RowIndex].Cells["Location"].Value.ToString();
                Quantity.Text = dataGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString();
                txtQR.Text = dataGridView1.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                string id = dataGridView1.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                string img_path = @"\\cvn-veng\Code\KeyResource\" + dataGridView1.Rows[e.RowIndex].Cells["Picture"].Value.ToString();
                picEquiq.Image = System.Drawing.Image.FromFile(img_path);

                DateTime dateTime = DateTime.Now;
                string year_inv = dateTime.Year.ToString();

                DataTable dtstatus = Common.FillData(@"select * from All_Inventory where Type= 'Equipment' and Year_Inven = '" + year_inv + "' and Month_Inven = '" + month_show.Text + "' and ID_items = '" + id + "'");
                if (dtstatus.Rows.Count > 0)
                {
                    status.Text = dtstatus.Rows[0]["Result"].ToString().Trim();
                    status.ForeColor = Color.BlueViolet;
                    lbReason.Text = dtstatus.Rows[0]["NGReason"].ToString();
                    btnSubmit.Hide();
                    btnUpdate.Show();
                }
                else
                {
                    status.Text = "Not checked in " + month_show.Text + "!!";
                    status.ForeColor = Color.Red;
                    lbReason.ForeColor = Color.Silver;
                    btnUpdate.Hide();
                    btnSubmit.Show();
                }
            }
            
        }

        private void panel21_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)  // this button is for 'update status' function
        {
            DateTime myDateTime = DateTime.Now;
            string year_inven = myDateTime.Year.ToString();
            string month_inven = Common.convert_month(myDateTime.Month);
            string date_inven = myDateTime.ToString().Trim();
            string type = "Equipment";
            string Id_Items = txtQR.Text;
            string Name_Items = equiqName.Text;
            string location_insert = locat_ion.Text;
            string cabinet_insert = Cabinet.Text;
            string pic_inven = labelCode.Text;
            
            if (equiqName.Text == "No Information")
            {
                MessageBox.Show("No infomation to submit!\nCheck your input....!");
            }
            else
            {
                DataTable dtcheck = Common.FillData(@"select * from All_Inventory where Type= 'Equipment' and Year_Inven = '" + year_inven + "' and Month_Inven = '" + month_inven + "' and ID_items = '" + Id_Items + "'");
                if (dtcheck.Rows.Count > 0)
                {
                    if (CheckOK.Checked == true)
                    {
                        if (txtReasonNG.Text != "")
                        {
                            MessageBox.Show("No Reason NG on OK item.\nPls clear this before Submiting!");
                        }
                        else
                        {
                            DialogResult dr = MessageBox.Show("Are you sure to change this item status to 'OK'?",
                                                        "Update", MessageBoxButtons.YesNo);
                            switch (dr)
                            {
                                case DialogResult.Yes:
                                    Common.Excute_SQL("update All_Inventory set Result = 'OK', NGReason = '' where Year_Inven = '" + year_inven + "' and Month_Inven = '" + month_inven + "' and ID_items = '" + Id_Items + "' ");
                                    MessageBox.Show("This item is updated successfully!");
                                    reset_info();
                                    break;
                                case DialogResult.No:
                                    break;
                            }
                            
                        }

                    }
                    else if (CheckOK.Checked == false)
                    {
                        if (txtReasonNG.Text == "")
                        {
                            MessageBox.Show("Pls input Reason NG!");
                        }
                        else
                        {
                            DialogResult dr = MessageBox.Show("Are you sure to change this item status to 'NG'?",
                                                    "Update", MessageBoxButtons.YesNo);
                            switch (dr)
                            {
                                case DialogResult.Yes:
                                    Common.Excute_SQL("update All_Inventory set Result = 'NG', NGReason = '" + txtReasonNG.Text.Replace("'", "") + "' where Year_Inven = '" + year_inven + "' and Month_Inven = '" + month_inven + "' and ID_items = '" + Id_Items + "'  ");
                                    MessageBox.Show("This item is updated successfully!");
                                    reset_info();
                                    break;
                                case DialogResult.No:
                                    break;
                            }

                            
                        }
                    }
                    else
                    {
                        MessageBox.Show("Pls choose 1 status option!");
                    }
                }
                else
                {
                    MessageBox.Show("This items's not been checked yet!\nPls choose 'Submit' button!!! ");
                }
            }
        }
        public void showdata(DataTable dtView, DataTable dt_checked) // fuction to show info in pie chart
        {

            dataGridView1.DataSource = dtView;
            dataGridView1.Columns["Picture"].Visible = false;
            dataGridView1.Columns["Quantity"].Visible = false;
            item_notchecked(dtView, dt_checked);
            display_autosize();
            int total_equip = dtView.Rows.Count;
            int equip_checked = dt_checked.Rows.Count;

            string notcheck_equip = calculate_percent(total_equip, equip_checked).ToString();
            string checked_equip = (100 - calculate_percent(total_equip, equip_checked)).ToString();
            chart1.Series["S1"].IsValueShownAsLabel = true;
            chart1.Series["S1"].Points.Clear();
            chart1.Series["S1"].Points.AddXY("Not checked", notcheck_equip);
            chart1.Series["S1"].Points[0].Color = Color.Red;
            chart1.Series["S1"].Points.AddXY("Checked", checked_equip);
            chart1.Series["S1"].Points[1].Color = Color.Green;


            labelTotal.Text = total_equip.ToString();
            labelChecked.Text = equip_checked.ToString();
            int not_Checked = total_equip - equip_checked;
            if (not_Checked > 0)
            {
                labelNotChecked.Text = not_Checked.ToString();
            }
            else
            {
                labelNotChecked.Text = "0";
            }

        }

        void show_check_notcheck_data()  // function to show data in datagridview and pie chart
        {
            string location_search = list_location.Text;
            string cabinet_search = list_cabinet.Text;
            if (location_search == "" || cabinet_search == "")
            {
                if (rdbtn_ItemNotChecked.Checked == true)
                {
                    DateTime dt = DateTime.Now;
                    string year_now = dt.Year.ToString();
                    DataTable dtView1 = Common.FillData(@" select Equipments.ID, Equipments.Name, Equipments.Serial, Equipments.FA_No, Equipments.ENG_control_no, Equipments.Picture, Equipments.Cabinet, Equipments.Describle, Equipments.Quantity, Category.Category, Category.Location"
                                                            + " from Equipments join Category on Equipments.Category_ID = Category.ID");
                    DataTable dt_checked = Common.FillData(@"select Equipments.ID from Equipments join Category on Equipments.Category_ID=Category.ID join All_Inventory on Equipments.ID= All_Inventory.ID_items"
                                                            + " where All_Inventory.Type= 'Equipment' and All_inventory.Month_Inven = '" + month_show.Text + "' and All_inventory.Year_Inven = '" + year_now + "' ");

                    dataGridView1.DataSource = dtView1;
                    display_autosize();
                    dataGridView1.Columns["Picture"].Visible = false;
                    dataGridView1.Columns["Quantity"].Visible = false;
                    item_notchecked(dtView1, dt_checked);

                    showdata(dtView1, dt_checked);
                }
                else
                {
                    DateTime dt = DateTime.Now;
                    string year_now = dt.Year.ToString();
                    DataTable dtView1 = Common.FillData(@" select Equipments.ID, Equipments.Name, Equipments.Serial, Equipments.FA_No, Equipments.ENG_control_no, Equipments.Picture, Equipments.Cabinet, Equipments.Describle, Equipments.Quantity, Category.Category, Category.Location"
                                                            + " from Equipments join Category on Equipments.Category_ID = Category.ID ");
                    DataTable dt_checked = Common.FillData(@"select * from Equipments join Category on Equipments.Category_ID=Category.ID join All_Inventory on Equipments.ID= All_Inventory.ID_items"
                                                            + " where All_Inventory.Type= 'Equipment' and All_inventory.Month_Inven = '" + month_show.Text + "' and All_inventory.Year_Inven = '" + year_now + "' ");

                    dataGridView1.Refresh();
                    dataGridView1.DataSource = dtView1;
                    display_autosize();
                    dataGridView1.Columns["Picture"].Visible = false;
                    dataGridView1.Columns["Quantity"].Visible = false;

                    //showdata(dtView1, dt_checked);
                }
            }
            else
            {
                if (rdbtn_ItemNotChecked.Checked == true)
                {
                    DateTime dt = DateTime.Now;
                    string year_now = dt.Year.ToString();
                    DataTable dtView1 = Common.FillData(@" select Equipments.ID, Equipments.Name, Equipments.Serial, Equipments.FA_No, Equipments.ENG_control_no, Equipments.Picture, Equipments.Cabinet, Equipments.Describle, Equipments.Quantity, Category.Category, Category.Location"
                                                            + " from Equipments join Category on Equipments.Category_ID = Category.ID where Cabinet = '" + cabinet_search + "' and Location = '" + location_search + "'");
                    DataTable dt_checked = Common.FillData(@"select Equipments.ID from Equipments join Category on Equipments.Category_ID=Category.ID join All_Inventory on Equipments.ID= All_Inventory.ID_items"
                                                            + " where All_Inventory.Type= 'Equipment' and All_Inventory.Month_Inven = '" + month_show.Text + "' and All_Inventory.Year_Inven = '" + year_now + "' and Equipments.Cabinet = '" + cabinet_search + "' and Category.Location = '" + location_search + "'");

                    dataGridView1.DataSource = dtView1;
                    display_autosize();
                    dataGridView1.Columns["Picture"].Visible = false;
                    dataGridView1.Columns["Quantity"].Visible = false;
                    item_notchecked(dtView1, dt_checked);
                    showdata(dtView1, dt_checked);
                }
                else
                {
                    DateTime dt = DateTime.Now;
                    string year_now = dt.Year.ToString();
                    DataTable dtView1 = Common.FillData(@" select Equipments.ID, Equipments.Name, Equipments.Serial, Equipments.FA_No, Equipments.ENG_control_no, Equipments.Picture, Equipments.Cabinet, Equipments.Describle, Equipments.Quantity, Category.Category, Category.Location"
                                                            + " from Equipments join Category on Equipments.Category_ID = Category.ID where Cabinet = '" + cabinet_search + "' and Location = '" + location_search + "'");
                    DataTable dt_checked = Common.FillData(@"select * from Equipments join Category on Equipments.Category_ID=Category.ID join All_Inventory on Equipments.ID= All_Inventory.ID_items"
                                                            + " where All_Inventory.Type= 'Equipment' and All_Inventory.Month_Inven = '" + month_show.Text + "' and All_Inventory.Year_Inven = '" + year_now + "' and Equipments.Cabinet = '" + cabinet_search + "' and Category.Location = '" + location_search + "'");
                    
                    dataGridView1.Refresh();
                    dataGridView1.DataSource = dtView1;
                    display_autosize();
                    dataGridView1.Columns["Picture"].Visible = false;
                    dataGridView1.Columns["Quantity"].Visible = false;

                    //showdata(dtView1, dt_checked);
                }
            }
        }
        private void list_cabinet_SelectedIndexChanged(object sender, EventArgs e) // when choose 1 cabinet --> show all info in datagridview table and pie chart
        {
            show_check_notcheck_data();
        }

        private void panel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel15_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CheckOK_CheckedChanged(object sender, EventArgs e)// switch checked ok and ng --> check box
        {
            if (CheckOK.Checked == true)
            {
                CheckNG.Checked = false;
            }
            else
            {
                CheckNG.Checked=true; 
            }
        }

        private void CheckNG_CheckedChanged(object sender, EventArgs e) // switch checked ok and ng --> check box
        {
            if (CheckNG.Checked == true)
            {
                CheckOK.Checked = false;
            }
            else
            {
                CheckOK.Checked = true;
            }
        }

        private void month_show_SelectedIndexChanged(object sender, EventArgs e) // select month to show status, pie chart
        {
            DateTime dt = DateTime.Now;
            string year_now = dt.Year.ToString();
            DataTable dtView = Common.FillData(@"select Equipments.ID, Equipments.Name, Equipments.Serial, Equipments.FA_No, Equipments.ENG_control_no, Equipments.Picture, Equipments.Cabinet, Equipments.Describle, Equipments.Quantity, Category.Category, Category.Location from Equipments join Category on Equipments.Category_ID = Category.ID");
            DataTable dt_checked = Common.FillData(@"select * from Equipments join Category on Equipments.Category_ID=Category.ID join All_Inventory on Equipments.ID= All_Inventory.ID_items where All_Inventory.Type= 'Equipment' and All_inventory.Month_Inven = '" + month_show.Text + "' and All_inventory.Year_Inven = '" + year_now + "'");
            rdbtn_ItemNotChecked.Checked = true;
            rdbtn_ItemChecked.Checked = false;
            if (list_cabinet.Text != "")
            {
                show_check_notcheck_data();
            }
            else
            {
                showdata(dtView, dt_checked);
            }
        }

        void item_notchecked(DataTable dtall, DataTable dtchecked) // function to hide equip that've already checked
        {
            for (int i = 0; i < dtall.Rows.Count; i++)
            {
                for (int j = 0; j < dtchecked.Rows.Count; j++)
                {
                    string a = dtall.Rows[i]["ID"].ToString();
                    string b = dtchecked.Rows[j]["ID"].ToString();
                    if (a == b)
                    {
                        CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView1.DataSource];
                        currencyManager1.SuspendBinding();
                        dataGridView1.Rows[i].Visible = false;
                        currencyManager1.ResumeBinding();
                    }

                }
            }
        }

        private void lbShow_notchecked_Click(object sender, EventArgs e)
        {

        }

        private void rdbtn_ItemNotChecked_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rdbtn_ItemChecked_CheckedChanged(object sender, EventArgs e) // switch checked 'all item' and 'not checked item' --> radio button 
        {
            if (rdbtn_ItemChecked.Checked == true)
            {
                rdbtn_ItemNotChecked.Checked = false;
            }
            else if (rdbtn_ItemChecked.Checked == false)
            {
                rdbtn_ItemNotChecked.Checked = true;
            }

            show_check_notcheck_data();
        }

        private void status_Click(object sender, EventArgs e)
        {

        }

        private void panel13_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void list_newLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btn_getOldInfor_Click(object sender, EventArgs e)  // this button is to get info for 'change cabinet function'
        {
            string id = txtQR.Text;
            if (id == "")
            {
                MessageBox.Show("No ID to search item's information!\nPls input or scan QR-code to get it!!");
            }
            else
            {
                DataTable dt = Common.FillData(@"select * from Equipments join Category on Equipments.Category_ID = Category.ID where Equipments.ID = '" + id + "'");
                if (dt.Rows.Count > 0)
                {
                    lbOldCabinet.ForeColor = Color.BlueViolet;
                    lbOldLocation.ForeColor = Color.BlueViolet;
                    lbOldName.ForeColor = Color.BlueViolet;
                    lbOldName.Text = dt.Rows[0]["Name"].ToString();
                    lbOldCabinet.Text = dt.Rows[0]["Cabinet"].ToString();
                    lbOldLocation.Text = dt.Rows[0]["Location"].ToString();
                }
                else
                {
                    MessageBox.Show("No item!");
                }
            }

            DataTable dt_Cabinet = Common.FillData(@"select distinct Equipments.Cabinet from Equipments join Category on Equipments.Category_ID = Category.ID where Category.Location = '" + lbOldLocation.Text + "'");
            for (int i = 0; i < dt_Cabinet.Rows.Count; i++)
            {
                list_newCabinet.Items.Add(dt_Cabinet.Rows[i][0].ToString());
            }
        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void btn_changeCabinet_Click(object sender, EventArgs e) // this button is for 'change cabinet' function
        {
            if (lbOldName.Text == "No Information")
            {
                MessageBox.Show("Nothing to change!");
            }
            else
            {
                DialogResult dr = MessageBox.Show("Are you sure to apdate cabinet for this item?",
                      "Update cabinet", MessageBoxButtons.YesNo);
                switch (dr)
                {
                    case DialogResult.Yes:
                        Common.Excute_SQL(@"update Equipments set Cabinet = '" + list_newCabinet.Text + "' where Equipments.Name = '" + lbOldName.Text + "'");
                        lbOldName.Text = "No Information";
                        lbOldCabinet.Text = "No Information";
                        lbOldLocation.Text = "No Information";
                        list_newCabinet.ResetText();
                        list_newCabinet.Items.Clear();

                        break;
                    case DialogResult.No:
                        list_newCabinet.Items.Clear();
                        lbOldName.Text = "No Information";
                        lbOldCabinet.Text = "No Information";
                        lbOldLocation.Text = "No Information";
                        list_newCabinet.ResetText();
                        break;
                }
                
            }
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox15_Click(object sender, EventArgs e) // log out function 
        {
            this.Close();
            Form1 f1 = new Form1();
            f1.ShowDialog();
        }

        private void pictureBox20_Click(object sender, EventArgs e)  // quit function
        {
            DialogResult dr = MessageBox.Show("Are you sure to quit this app?",
                      "Quit", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    break;
                case DialogResult.No:
                    break;
            }
            
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void picEquiq_Click(object sender, EventArgs e) // turn on new form and zoom the picture of equip
        {
            if (equiqName.Text == "No Information")
            {
                // no info --> no action
            }
            else
            {
                string result_QR = txtQR.Text;
                DataTable dt = Common.FillData(@"select * from Equipments join Category on Equipments.Category_ID = Category.ID where Equipments.ID='" + result_QR + "' ");

                string img_path = @"\\cvn-veng\Code\KeyResource\" + dt.Rows[0]["Picture"].ToString().Trim();

                Image_zoom image_Zoom = new Image_zoom();
                image_Zoom.Show();
                image_Zoom.picZoom_.Image = System.Drawing.Image.FromFile(img_path);
                
            }
            
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {

        }
    }
}
