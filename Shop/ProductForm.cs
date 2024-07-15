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
    public partial class ProductForm : Form
    {
        DBConnect dBCon = new DBConnect();
        public ProductForm()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button_category_Click(object sender, EventArgs e)
        {
            CategoryForm category = new CategoryForm();
            category.Show();
            this.Hide();
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            
            
                // Buat objek PrintPreviewDialog
                PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();

                // Mengaitkan PrintDocument dengan PrintPreviewDialog
                printPreviewDialog1.Document = printDocument1;

                // Sisanya dari kode pengaturan form...
                getCategory();
                getTable();
            

        }

        private void getCategory()
        {
            string selectQuerry = "SELECT * FROM Category";
            SqlCommand command = new SqlCommand(selectQuerry, dBCon.GetCon());
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            ComboBox_category.DataSource = table;
            ComboBox_category.ValueMember = "CatName";
            ComboBox_search.DataSource = table;
            ComboBox_search.ValueMember = "CatName";
        }

        private void getTable()
        {
            string selectQuerry = "SELECT * FROM Product";
            SqlCommand command = new SqlCommand(selectQuerry, dBCon.GetCon());
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView_product.DataSource = table;
        }

        private void clear()
        {
            TextBox_id.Clear();
            TextBox_name.Clear();
            TextBox_price.Clear();
            TextBox_quantity.Clear();
            ComboBox_category.SelectedIndex = 0;
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            try
            {
                string insertQuery = "INSERT INTO Product VALUES("+TextBox_id.Text+", '"+TextBox_name.Text+ "', "+TextBox_price.Text+","+TextBox_quantity.Text  + ", '"+ComboBox_category.Text+"')";
                SqlCommand command = new SqlCommand(insertQuery, dBCon.GetCon());
                dBCon.OpenCon();
                command.ExecuteNonQuery();
                MessageBox.Show("Product Added Successfully", "Add Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dBCon.CloseCon();
                getTable();
                clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_update_Click(object sender, EventArgs e)
        {
            try
            {
                if (TextBox_id.Text == "" || TextBox_name.Text == "" || TextBox_price.Text == "" || TextBox_quantity.Text == "")
                {
                    MessageBox.Show("Missing Information","Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                { 
                string updateQuery = "UPDATE Product SET ProdName='" + TextBox_name.Text + "',ProdPrice=" + TextBox_price.Text + ",ProdQty=" + TextBox_quantity.Text + ",ProdCat='" + ComboBox_category.Text + "'WHERE ProdId="+TextBox_id.Text+"";
                SqlCommand command = new SqlCommand(updateQuery, dBCon.GetCon());
                dBCon.OpenCon();
                command.ExecuteNonQuery();
                MessageBox.Show("Product Update Successfully", "Update Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dBCon.CloseCon();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void dataGridView_product_Click_1(object sender, EventArgs e)
        {
            TextBox_id.Text = dataGridView_product.SelectedRows[0].Cells[0].Value.ToString();
            TextBox_name.Text = dataGridView_product.SelectedRows[0].Cells[1].Value.ToString();
            TextBox_price.Text = dataGridView_product.SelectedRows[0].Cells[2].Value.ToString();
            TextBox_quantity.Text = dataGridView_product.SelectedRows[0].Cells[3].Value.ToString();
            ComboBox_category.Text = dataGridView_product.SelectedRows[0].Cells[4].Value.ToString();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (TextBox_id.Text =="")
                {
                    MessageBox.Show("Missing Information", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string deleteQuery = "DELETE FROM Product WHERE ProdId=" + TextBox_id.Text + "";
                    SqlCommand command = new SqlCommand(deleteQuery, dBCon.GetCon());
                    dBCon.OpenCon();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Category Delete Successfully", "Update Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void button_refresh_Click(object sender, EventArgs e)
        {
            getTable();
        }

        private void ComboBox_search_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string selectQuerry = "SELECT * FROM Product WHERE ProdCat='"+ComboBox_search.SelectedValue.ToString()+"'";
            SqlCommand command = new SqlCommand(selectQuerry, dBCon.GetCon());
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView_product.DataSource = table;
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

        private void dataGridView_product_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginForm1 login = new LoginForm1();
            login.Show();
            this.Hide();
        }

        private void sellerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SellerForm seller = new SellerForm();
            seller.Show();
            this.Hide();
        }

        private void categoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CategoryForm category = new CategoryForm();
            category.Show();
            this.Hide();
        }

        private void sellingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SellingForm selling = new SellingForm();
            selling.Show();
            this.Hide();
        }

        private void TextBox_id_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView_product_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

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
            e.Graphics.DrawString("Product Id", font, brush, new PointF(xPos, yPos));
            xPos += 150; // Lebar kolom 1
            e.Graphics.FillRectangle(Brushes.LightGray, new RectangleF(xPos, yPos, 150, cellHeight));
            e.Graphics.DrawString("Product Name", font, brush, new PointF(xPos, yPos));
            xPos += 150; // Lebar kolom 2
            e.Graphics.FillRectangle(Brushes.LightGray, new RectangleF(xPos, yPos, 150, cellHeight));
            e.Graphics.DrawString("Product Price", font, brush, new PointF(xPos, yPos));
            xPos += 150; // Lebar kolom 3
            e.Graphics.FillRectangle(Brushes.LightGray, new RectangleF(xPos, yPos, 150, cellHeight));
            e.Graphics.DrawString("Product Quantity", font, brush, new PointF(xPos, yPos));
            xPos += 150; // Lebar kolom 4
            e.Graphics.FillRectangle(Brushes.LightGray, new RectangleF(xPos, yPos, 150, cellHeight));
            e.Graphics.DrawString("Product Category", font, brush, new PointF(xPos, yPos));
            yPos += cellHeight;

            // Garis pembatas kolom header
            e.Graphics.DrawLine(pen, 50, yPos, xPos, yPos);

            // Menggambar isi tabel dari DataGridView
            foreach (DataGridViewRow row in dataGridView_product.Rows)
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
                e.Graphics.DrawLine(pen, 50, yPos, xPos, yPos);
            }
        }


        private void button_print_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }
    }
}
