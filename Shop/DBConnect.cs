using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Shop
{
    class DBConnect
    {
        public static string database = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\IKA\Documents\Shopdb.mdf;Integrated Security = True; Connect Timeout = 30; ";
        public SqlConnection con = new SqlConnection(database);
        public SqlCommand cmd;
        public SqlDataAdapter adp = new SqlDataAdapter();
        public SqlDataReader dr;
        public DataTable dt = new DataTable();
        public DataSet ds;

        public SqlConnection GetCon()
        {
            return con;
        }

        public void OpenCon()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void CloseCon()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
