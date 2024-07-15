using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Shop
{
    public partial class LoginForm1 : Form
    {
        DBConnect dBCon = new DBConnect();
        public static string SellerName;
        public LoginForm1()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            textBox_username.Clear();
            textBox_password.Clear();
        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void label_exit_MouseEnter(object sender, EventArgs e)
        {
            label_exit.ForeColor = Color.Red;
        }

        private void label_exit_MouseLeave(object sender, EventArgs e)
        {
            label_exit.ForeColor = Color.Brown;
        }

        private void label_clear_MouseEnter(object sender, EventArgs e)
        {
            label_clear.ForeColor = Color.Red;
        }

        private void label_clear_MouseLeave(object sender, EventArgs e)
        {
            label_clear.ForeColor = Color.Brown;
        }

        private void label_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoginForm1_Load(object sender, EventArgs e)
        {

        }

        private void textBox_password_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_username_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void LoginForm1_Load_1(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (textBox_username.Text == "" || textBox_password.Text == "")
            {
                MessageBox.Show("Please Enter Username and Password", "Misiing Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (comboBox_role.SelectedIndex > -1)
                {
                    if (comboBox_role.SelectedItem.ToString() == "ADMIN")
                    {
                        if (textBox_username.Text == "Admin" && textBox_password.Text == "Admin123")
                        {
                            ProductForm product = new ProductForm();
                            product.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("If you are Admin, Please Enter the correct Id and Password", "Miss Id", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else if (comboBox_role.SelectedItem.ToString() == "SELLER")
                    {
                        string selectQuery = "SELECT * FROM Seller WHERE SellerName='" + textBox_username.Text + "' AND SellerPass='" + textBox_password.Text + "'";
                        DataTable table = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, dBCon.GetCon());
                        adapter.Fill(table);
                        if (table.Rows.Count > 0)
                        {
                            SellerName = textBox_username.Text;
                            SellingForm selling = new SellingForm();
                            selling.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Wrong Username Or Password", "Wrong Infromation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (comboBox_role.SelectedItem.ToString() == "MANAGER")
                    {
                        string selectQuery = "SELECT * FROM Manager WHERE ManagerName='" + textBox_username.Text + "' AND ManagerPass='" + textBox_password.Text + "'";
                        DataTable table = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, dBCon.GetCon());
                        adapter.Fill(table);
                        if (table.Rows.Count > 0)
                        {
                            Manager manager = new Manager();
                            manager.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Wrong Username Or Password", "Wrong Infromation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Select Role", "Wrong Infromation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void comboBox_role_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

