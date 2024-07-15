using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Shop
{
    public partial class Manager : Form
    {
        DBConnect dBCon = new DBConnect();
        public Manager()
        {
            InitializeComponent();
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            try
            {
                string insertQuery = "INSERT INTO Manager VALUES(" + TextBox_id.Text + ", '" + TextBox_name.Text + "', '" + TextBox_age.Text + "','" + TextBox_tlp.Text + "', '" + TextBox_pass.Text + "')";
                SqlCommand command = new SqlCommand(insertQuery, dBCon.GetCon());
                dBCon.OpenCon();
                command.ExecuteNonQuery();
                MessageBox.Show("Manager Added Successfully", "Add Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dBCon.CloseCon();
                getTable();
                clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void getTable()
        {
            string selectQuerry = "SELECT * FROM Manager";
            SqlCommand command = new SqlCommand(selectQuerry, dBCon.GetCon());
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView_manager.DataSource = table;
        }

        private void clear()
        {
            TextBox_id.Clear();
            TextBox_name.Clear();
            TextBox_age.Clear();
            TextBox_tlp.Clear();
            TextBox_pass.Clear();
        }
        
        private void Manager_Load(object sender, EventArgs e)
        {
            getTable();
        }

        private void button_update_Click(object sender, EventArgs e)
        {
            try
            {
                if (TextBox_id.Text == "" || TextBox_name.Text == "" || TextBox_age.Text == "" || TextBox_tlp.Text == "" || TextBox_pass.Text == "")
                {
                    MessageBox.Show("Missing Information", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string updateQuery = "UPDATE Manager SET ManagerName='" + TextBox_name.Text + "',ManagerAge='" + TextBox_age.Text + "',ManagerPhone='" + TextBox_tlp.Text + "',ManagerPass='" + TextBox_pass.Text + "'WHERE ManagerId=" + TextBox_id.Text + "";
                    SqlCommand command = new SqlCommand(updateQuery, dBCon.GetCon());
                    dBCon.OpenCon();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Manager Update Successfully", "Update Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dBCon.CloseCon();
                    getTable();
                    clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (TextBox_id.Text == "")
                {
                    MessageBox.Show("Missing Information", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if ((MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
                    {
                        string deleteQuery = "DELETE FROM Manager WHERE ManagerId=" + TextBox_id.Text + "";
                        SqlCommand command = new SqlCommand(deleteQuery, dBCon.GetCon());
                        dBCon.OpenCon();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Manager Delete Successfully", "Update Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dBCon.CloseCon();
                        getTable();
                        clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView_manager_Click(object sender, EventArgs e)
        {
            TextBox_id.Text = dataGridView_manager.SelectedRows[0].Cells[0].Value.ToString();
            TextBox_name.Text = dataGridView_manager.SelectedRows[0].Cells[1].Value.ToString();
            TextBox_age.Text = dataGridView_manager.SelectedRows[0].Cells[2].Value.ToString();
            TextBox_tlp.Text = dataGridView_manager.SelectedRows[0].Cells[3].Value.ToString();
            TextBox_pass.Text = dataGridView_manager.SelectedRows[0].Cells[4].Value.ToString();
        }

        private void label7_MouseEnter(object sender, EventArgs e)
        {
            label_exitt.ForeColor = Color.Red;
        }

        private void label_exitt_MouseLeave(object sender, EventArgs e)
        {
            label_exitt.ForeColor = Color.Brown;
        }

        private void label_exit_click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginForm1 login = new LoginForm1();
            login.Show();
            this.Hide();
        }

        private void categoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void sellingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sellerToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView_manager_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
