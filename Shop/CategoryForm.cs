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
using System.Web.UI.WebControls;

namespace Shop
{
    public partial class CategoryForm : Form
    {
        DBConnect dBCon = new DBConnect();

        public CategoryForm()
        {
            InitializeComponent();
        }

        private void getTable()
        {
            string selectQuerry = "SELECT * FROM Category";
            SqlCommand command = new SqlCommand(selectQuerry, dBCon.GetCon());
            SqlDataAdapter adapter = new  SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView_category.DataSource = table;


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button_add_Click(object sender, EventArgs e)
        {
            try
            {
                string insertQuery = "INSERT INTO Category VALUES(" + TextBox_id.Text + ",'" + TextBox_name.Text + "','" + TextBox_description.Text + "')";
                SqlCommand command = new SqlCommand(insertQuery, dBCon.GetCon());
                dBCon.OpenCon();
                command.ExecuteNonQuery();
                MessageBox.Show("Category Added Successfully", "Add Information", MessageBoxButtons.OK,MessageBoxIcon.Information);
                dBCon.CloseCon();
                getTable();
                clear();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CategoryForm_Load(object sender, EventArgs e)
        {
            PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();

            // Mengaitkan PrintDocument dengan PrintPreviewDialog
            printPreviewDialog1.Document = printDocument1;
            getTable();
        }

        private void button_update_Click(object sender, EventArgs e)
        {
            try
            {
                if (TextBox_id.Text == " " || TextBox_description.Text == "")
                {
                    MessageBox.Show("Missing Information", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string updateQuery = "UPDATE Category SET CatName='" + TextBox_name.Text + "', CatDes='" + TextBox_description.Text + "'WHERE CatId=" + TextBox_id.Text + " ";
                    SqlCommand command = new SqlCommand(updateQuery, dBCon.GetCon());
                    dBCon.OpenCon();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Category Update Successfully", "Update Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dataGridView_category_Click(object sender, EventArgs e)
        {
            TextBox_id.Text = dataGridView_category.SelectedRows[0].Cells[0].Value.ToString();
            TextBox_name.Text = dataGridView_category.SelectedRows[0].Cells[1].Value.ToString();
            TextBox_description.Text = dataGridView_category.SelectedRows[0].Cells[2].Value.ToString();

        }

        private void clear()
        {
            TextBox_id.Clear();
            TextBox_name.Clear();
            TextBox_description.Clear();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            try
            {
                string deleteQuery = "DELETE FROM Category WHERE CatId=" + TextBox_id.Text + "";
                SqlCommand command = new SqlCommand(deleteQuery, dBCon.GetCon());
                dBCon.OpenCon();
                command.ExecuteNonQuery();
                MessageBox.Show("Category Delete Successfully", "Update Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dBCon.CloseCon();
                getTable();
                clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void label_exit_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void label_exit_MouseEnter(object sender, EventArgs e)
        {
            label_exit.ForeColor = Color.Red;
        }

        private void label_exit_MouseLeave(object sender, EventArgs e)
        {
            label_exit.ForeColor = Color.Brown;
        }

        private void label_logout_Click(object sender, EventArgs e)
        {
            LoginForm1 login = new LoginForm1();
            login.Show();
            this.Hide();
        }

        private void button_product_Click(object sender, EventArgs e)
        { 
            ProductForm product = new ProductForm();
            product.Show();
            this.Hide();
        }

        private void button_seller_Click(object sender, EventArgs e)
        {
            SellerForm seller = new SellerForm();
            seller.Show();
            this.Hide();
        }

        private void button_selling_Click(object sender, EventArgs e)
        {
            SellingForm selling = new SellingForm();
            selling.Show();
            this.Hide();
        }

        private void dataGridView_category_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void sellerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SellerForm seller = new SellerForm();
            seller.Show();
            this.Hide();
        }

        private void categoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ProductForm product = new ProductForm();
            product.Show();
            this.Hide();
        }

        private void sellingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SellingForm selling = new SellingForm();
            selling.Show();
            this.Hide();
        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginForm1 login = new LoginForm1();
            login.Show();
            this.Hide();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font font = new Font("Arial", 12);
            Brush brush = Brushes.Black;
            Pen pen = new Pen(Brushes.Black, 1); // Gunakan pena hitam dengan ketebalan 1

            float cellHeight = font.GetHeight() + 10;
            float xPos = 50;
            float yPos = 50;

            // Judul kolom
            e.Graphics.FillRectangle(Brushes.LightGray, new RectangleF(50, yPos, 150, cellHeight));
            e.Graphics.DrawString("Cat Id", font, brush, new PointF(xPos, yPos));
            xPos += 150; // Lebar kolom 1
            e.Graphics.FillRectangle(Brushes.LightGray, new RectangleF(xPos, yPos, 150, cellHeight));
            e.Graphics.DrawString("Cat Name", font, brush, new PointF(xPos, yPos));
            xPos += 150; // Lebar kolom 2
            e.Graphics.FillRectangle(Brushes.LightGray, new RectangleF(xPos, yPos, 150, cellHeight));
            e.Graphics.DrawString("Cat Des", font, brush, new PointF(xPos, yPos));
            xPos += 150; // Lebar kolom 3
            yPos += cellHeight;

            // Garis pembatas kolom header
            e.Graphics.DrawLine(pen, 50, yPos, xPos, yPos);

            // Menggambar isi tabel dari DataGridView
            foreach (DataGridViewRow row in dataGridView_category.Rows)
            {
                xPos = 50;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    // Pastikan baris tidak kosong
                    if (!row.IsNewRow)
                    {
                        // Menggambar data ke kertas cetak dengan latar belakang putih
                        e.Graphics.FillRectangle(Brushes.White, new RectangleF(xPos, yPos, 150, cellHeight));
                        e.Graphics.DrawString(cell.Value.ToString(), font, brush, new PointF(xPos, yPos));
                        xPos += 150; // Sesuaikan dengan lebar kolom
                    }
                }
                yPos += cellHeight; 

                // Garis pembatas antar baris
                e.Graphics.DrawLine(pen, 50, yPos, xPos, yPos); // Garis horizontal bawah setiap baris
            }
        }

        private void button_print_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }
    }
}
