using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Windows.Forms;
using AForge;
using AForge.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.Aztec;

namespace sda
{
    class Common
    {

        public static string strString = @"Data Source=CVN-VENG;Initial Catalog=AssetManagement;Persist Security Info=True;User ID=sa;Password=tim@2020";
        public static string strString_User = @"Data Source=CVN-VENG;Initial Catalog=UserInformation;Persist Security Info=True;User ID=sa;Password=tim@2020";
        //public static string category = @"Data Source=TIM03\SQLEXPRESS;Initial Catalog=AssetManagement;Persist Security Info=True;User ID=sa;Password=timeit@2022";

        public static OleDbConnection GetConnection()
        {
            OleDbConnection conn = new OleDbConnection(strString);
            conn.Open();
            return conn;

            //OleDbConnection conUser = new OleDbConnection(strString_User);
            //conUser.Open();
            //return conUser;
        }
        public static SqlConnection open()
        {
            return new SqlConnection(strString);
        }
        public static SqlConnection openUser()
        {
            return new SqlConnection(strString_User);
        }
        public static DataTable FillData(string sql)
        {
            using (SqlConnection conn = new SqlConnection(strString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    using (DataSet ds = new DataSet())
                    {
                        adapter.Fill(ds);
                        conn.Close();
                        conn.Dispose();
                        return ds.Tables[0];
                    }
                }
            }
        }
        public static DataTable FillDataUser(string sql)
        {
            using (SqlConnection conn = new SqlConnection(strString_User))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    using (DataSet ds = new DataSet())
                    {
                        adapter.Fill(ds);
                        conn.Close();
                        conn.Dispose();
                        return ds.Tables[0];
                    }
                }
            }
        }
        public static void Excute_SQL(string sql)
        {
            using (SqlConnection con = new SqlConnection(strString))
            {
                using (SqlCommand cmd = new SqlCommand(sql))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public static string convert_month(int month)
        {
            DateTime date = new DateTime(2022, month, 1);
            return date.ToString("MMM");
        }
        

    }
}
