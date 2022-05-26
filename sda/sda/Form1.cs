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
    public partial class Form1 : Form
    {
        public System.Drawing.Icon Icon { get; set; }
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(600, 100);
           
            t = txtUser;
        }
       

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            txtUser.Clear();
            txtPassword.Clear();
        }

        //public static string userName;
        public static TextBox t = new TextBox();
        public string TheValue
        {
            get { return txtUser.Text; }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string usercode = t.Text;
            string password = txtPassword.Text;

            DataTable dtUser = Common.FillDataUser(@"select * from User_Info where code = '" + usercode + "'");
            if (dtUser.Rows.Count > 0 )
            {
                string Name = dtUser.Rows[0]["FullName"].ToString().Trim();
                string realPasswd = dtUser.Rows[0]["Password"].ToString().Trim();
                string dept = dtUser.Rows[0]["Dept"].ToString().Trim();
                if (dept == "TIM")
                {
                    if (password == realPasswd)
                    {
                        MainProg main = new MainProg();
                        main.Show();
                        main.name_user.Text = Name;
                        main.labelCode.Text = usercode;
                        main.labelDept.Text = dept;
                        this.Hide();
                    }   
                    else
                    {
                        MessageBox.Show("Login Failed!\nCheck your user and password..!");
                        txtPassword.Clear();
                        txtUser.Clear();
                        txtUser.Focus();

                    }
                }
                else
                {
                    MessageBox.Show("You have no permission!!!");
                    txtUser.Clear();
                    txtPassword.Clear();
                    txtUser.Focus();
                }

            }
            else
            {
                MessageBox.Show("Login Failed!\nCheck your user and password..!");
                txtUser.Clear();
                txtPassword.Clear();
                txtUser.Focus();
            }
        //    if (txtUser.Text =="v139405" && txtPassword.Text =="1")
        //    {
        //        new MainProg().Show();
        //        this.Hide();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Pls check your user and password..!");
        //    }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
