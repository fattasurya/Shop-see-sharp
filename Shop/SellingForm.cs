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
    using System.Management;
    using System.Web.UI.Design.WebControls;


    namespace Shop
    {
        public partial class SellingForm : Form
        {
            DBConnect dBCon = new DBConnect();
      



            public SellingForm()
            {
                InitializeComponent();
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
            }

            private void getTable()
            {
                string selectQuerry = "SELECT ProdName, ProdPrice FROM Product";
                SqlCommand command = new SqlCommand(selectQuerry, dBCon.GetCon());
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                DataGridView_product.DataSource = table;
            }

            private void getSellBill()
            {

                string selectQuerry = "SELECT * FROM Bill";
                SqlCommand command = new SqlCommand(selectQuerry, dBCon.GetCon());
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                datagrid_listne.DataSource = table;

            }

            private void SellingForm_Load(object sender, EventArgs e)
            {
                label_date.Text = DateTime.Today.ToShortDateString();
                label_seller.Text = LoginForm1.SellerName;
                getTable();
                getCategory();
                getSellBill();

                // Tambahkan event handler untuk DataGridView_order
                dataGridView_order.CellEndEdit += dataGridView_order_CellEndEdit;
            }


            private void button_add_Click(object sender, EventArgs e)
            {

            }

            private void PrintOrderSummary(string billId, int orderId, string customerName, string orderDate, int total, DataTable orderData)
            {
                try
                {
                    if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                    {
                        printDocument1.PrintPage += (sender, e) =>
                        {
                      

                        
                            int yPos = 200;
                            foreach (DataRow row in orderData.Rows)
                            {
                                string prodName = row["ProdName"].ToString();
                                int prodQty = Convert.ToInt32(row["ProdQty"]);
                                int prodPrice = Convert.ToInt32(row["ProdPrice"]);
                                int prodTotal = Convert.ToInt32(row["Total"]);

                                e.Graphics.DrawString($"{prodName}: {prodQty} x {prodPrice} = {prodTotal}",
                                    new Font("Century", 18, FontStyle.Regular), Brushes.Black, new Point(50, yPos));
                                yPos += 40;
                            }
                        
                        };
                        printDocument1.Print();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error printing order summary: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            int grandTotal = 0, n = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView_order.Rows.Count == 0)
            {
                MessageBox.Show("No data available in DataGridView_order.", "Information Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = dBCon.GetCon())
                {
                    connection.Open();

                    int OrderId = GenerateOrderId();

                    foreach (DataGridViewRow row in dataGridView_order.Rows)
                    {
                        if (row.Cells[1].Value != null && row.Cells[2].Value != null && row.Cells[3].Value != null)
                        {
                            string ProdName = row.Cells[1].Value.ToString();
                            int ProdQty = Convert.ToInt32(row.Cells[3].Value);
                            int ProdPrice = Convert.ToInt32(row.Cells[2].Value);
                            int Total = ProdQty * ProdPrice;

                            AddOrderToDatabaseAndGetOrderId(ProdName, ProdQty, ProdPrice, Total, OrderId);
                            ReduceProductQuantity(ProdName, ProdQty);
                            grandTotal += Total;
                        }
                    }

                    UpdateGrandTotal();

                    MessageBox.Show("Orders added successfully.", "Order Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridView_order.Rows.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }








        private int GenerateOrderId()
            {
                // Generate OrderId baru, misalnya dengan menggunakan timestamp
                return (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            }


        private int AddOrderToDatabaseAndGetOrderId(string ProdName, int ProdQty, int ProdPrice, int Total, int OrderId)
        {
            int OrderDetailId = -1;
            try
            {
                string selectQuery = "SELECT ProdId FROM Product WHERE ProdName = @ProdName";
                using (SqlCommand selectCommand = new SqlCommand(selectQuery, dBCon.GetCon()))
                {
                    selectCommand.Parameters.AddWithValue("@ProdName", ProdName);
                    dBCon.OpenCon();
                    int ProdId = Convert.ToInt32(selectCommand.ExecuteScalar());
                    dBCon.CloseCon();

                    string insertQuery = "INSERT INTO [Order] (OrderId, ProdId, ProdName, ProdPrice, ProdQty, Total) VALUES (@OrderId, @ProdId, @ProdName, @ProdPrice, @ProdQty, @Total); SELECT SCOPE_IDENTITY();";
                    using (SqlCommand command = new SqlCommand(insertQuery, dBCon.GetCon()))
                    {
                        command.Parameters.AddWithValue("@OrderId", OrderId);
                        command.Parameters.AddWithValue("@ProdId", ProdId);
                        command.Parameters.AddWithValue("@ProdName", ProdName);
                        command.Parameters.AddWithValue("@ProdPrice", ProdPrice);
                        command.Parameters.AddWithValue("@ProdQty", ProdQty);
                        command.Parameters.AddWithValue("@Total", Total);
                        dBCon.OpenCon();
                        OrderDetailId = Convert.ToInt32(command.ExecuteScalar());
                        dBCon.CloseCon();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding order to database: " + ex.Message);
            }

            return OrderDetailId;
        }






        private void button_refresh_Click(object sender, EventArgs e)
            {
                getTable();
            }

            private void comboBox_category_SelectedIndexChanged(object sender, EventArgs e)
            {
                string selectQuerry = "SELECT ProdName, ProdPrice FROM Product WHERE ProdCat='" + ComboBox_category.SelectedValue.ToString() + "'";
                SqlCommand command = new SqlCommand(selectQuerry, dBCon.GetCon());
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                DataGridView_product.DataSource = table;
            }

            private void dataGridView_seller_CellContentClick(object sender, DataGridViewCellEventArgs e)
            {

            }

            private void DataGridView_product_CellContentClick(object sender, DataGridViewCellEventArgs e)
            {

            }

            private void button_add_Click_1(object sender, EventArgs e)
            {
               /* try
                {
                    // Mendapatkan informasi barang
                    string ProdName = TextBox_name.Text;
                    int ProdQty = Convert.ToInt32(TextBox_quantity.Text);
                    int ProdPrice = Convert.ToInt32(TextBox_price.Text);
                    int Total = ProdQty * ProdPrice;
                    grandTotal += Total;

                    // Generate OrderId
                    int OrderId = GenerateOrderId();

                    // Membuka koneksi ke database (Ini tidak perlu dilakukan di sini karena koneksi telah dibuka sebelumnya)
                    // dBCon.OpenCon(); 

                    // Memulai transaksi
                    using (SqlTransaction transaction = dBCon.GetCon().BeginTransaction())
                    {
                        try
                        {
                            // Menambahkan pesanan ke dalam database (order table) dan mendapatkan OrderId
                            int orderDetailId = AddOrderToDatabaseAndGetOrderId(ProdName, ProdQty, ProdPrice, Total, OrderId, transaction);

                            // Menambahkan pesanan ke tagihan (bill table) dengan menggunakan OrderId yang dihasilkan
                            ///AddOrderToBill(OrderId, grandTotal, transaction);

                            // Commit transaksi
                            transaction.Commit();

                            // Menampilkan pesan sukses
                            MessageBox.Show("Order Added Successfully", "Order Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Memuat ulang data tagihan
                            getSellBill();
                        }
                        catch (Exception ex)
                        {
                            // Rollback transaksi jika terjadi kesalahan
                            transaction.Rollback();
                            throw ex; // Lepaskan pengecualian agar dapat ditangkap di luar blok try-catch
                        }
                    }

                    // Menutup koneksi database (Ini tidak perlu dilakukan di sini karena koneksi akan ditutup ketika metode selesai dieksekusi)
                    // dBCon.CloseCon();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding order: " + ex.Message, "Order Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }*/
            }







            private void ReduceProductQuantity(string ProdName, int ProdQty)
            {
                string updateQuery = "UPDATE Product SET ProdQty = ProdQty - @Quantity WHERE ProdName = @ProdName";
                SqlCommand command = new SqlCommand(updateQuery, dBCon.GetCon());
                command.Parameters.AddWithValue("@Quantity", ProdQty);
                command.Parameters.AddWithValue("@ProdName", ProdName);
                dBCon.OpenCon();
                command.ExecuteNonQuery();
                dBCon.CloseCon();
            }

            private void AddOrderToBill(int OrderId, int Total)
            {
                try
                {
                    string insertQuery = "INSERT INTO Bill (BillId, OrderId, SellerName, SellDate, TotalAmt) VALUES ( @BillId, @OrderId, @SellerName, @SellDate, @TotalAmt)";
                    SqlCommand command = new SqlCommand(insertQuery, dBCon.GetCon());
                    command.Parameters.AddWithValue("@BillId", TextBox_id.Text);
                    command.Parameters.AddWithValue("@OrderId", OrderId);
                    command.Parameters.AddWithValue("@SellerName", label_seller.Text);
                    command.Parameters.AddWithValue("@SellDate", label_date.Text);
                    command.Parameters.AddWithValue("@TotalAmt", Total);
                    dBCon.OpenCon();
                    command.ExecuteNonQuery();
                    dBCon.CloseCon();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding order to bill: " + ex.Message);
                }
            }


            private void button_print_Click(object sender, EventArgs e)
            {
            
                if (datagrid_listne.SelectedRows.Count > 0)
                {
               
                    int orderId = Convert.ToInt32(datagrid_listne.SelectedRows[0].Cells[1].Value);

                
                    DataTable orderData = GetOrderDataByOrderId(orderId);

                
                    if (orderData.Rows.Count > 0)
                    {
                   
                        DataGridViewRow row = datagrid_listne.SelectedRows[0];
                        string billId = row.Cells[0].Value.ToString();
                        string customerName = row.Cells[2].Value.ToString();
                        string orderDate = row.Cells[3].Value.ToString();
                        int total = Convert.ToInt32(row.Cells[4].Value);

                    
                        PrintOrderSummary(billId, orderId, customerName, orderDate, total, orderData);
                    }
                    else
                    {
                        MessageBox.Show("Order data not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select an order.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            private DataTable GetOrderDataByOrderId(int orderId)
            {
                DataTable orderData = new DataTable();
                try
                {
                    string selectQuery = "SELECT * FROM [Order] WHERE OrderId = @OrderId";
                    using (SqlCommand command = new SqlCommand(selectQuery, dBCon.GetCon()))
                    {
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(orderData);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving order data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return orderData;
            }
            private void UpdateTotalForRow(int rowIndex)
            {
                int price = Convert.ToInt32(dataGridView_order.Rows[rowIndex].Cells["ProdPrice"].Value);
                int quantity = Convert.ToInt32(dataGridView_order.Rows[rowIndex].Cells["ProdQty"].Value);
                dataGridView_order.Rows[rowIndex].Cells["Total"].Value = (price * quantity).ToString();
            }

            private void UpdateGrandTotal()
            {
                int total = 0;
                foreach (DataGridViewRow row in dataGridView_order.Rows)
                {
                    total += Convert.ToInt32(row.Cells["Total"].Value);
                }
                label_amount.Text = total.ToString() + " Ribu";
            }



            private void label_logout_Click(object sender, EventArgs e)
            {
                LoginForm1 login = new LoginForm1();
                login.Show();
                this.Hide();
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



            private void ComboBox_category_SelectionChangeCommitted(object sender, EventArgs e)
            {

            }


  


            private void printPreviewDialog1_Load(object sender, EventArgs e)
            {

            }

            private void DataGridView_sellList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
            {
            
            }


            private void datagrid_list_Click_1(object sender, EventArgs e)
            {
                TextBox_id.Text = datagrid_listne.SelectedRows[0].Cells[0].Value.ToString();
                TextBox_orid.Text = datagrid_listne.SelectedRows[0].Cells[1].Value.ToString();
                label_seller.Text = datagrid_listne.SelectedRows[0].Cells[2].Value.ToString();
                label_date.Text = datagrid_listne.SelectedRows[0].Cells[3].Value.ToString();
                label_amount.Text = datagrid_listne.SelectedRows[0].Cells[4].Value.ToString();
            }

            private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
            {
                int yPosOrderSummary = 50;
                int yPosRemainingContent = yPosOrderSummary + 100;

                int marginLeft = 50;
                int marginTop = 50;

                e.Graphics.DrawString("ORDER SUMMARY", new Font("Century", 25, FontStyle.Bold), Brushes.Red, new Point(230, yPosOrderSummary));

                int cellHeight = 40;
                int cellWidth = 300;
                int x = marginLeft;
                int y = yPosRemainingContent + marginTop;

                e.Graphics.DrawString("BILL ID : " + TextBox_id.Text, new Font("Century", 18, FontStyle.Regular), Brushes.Black, new Point(x, y));
                y += cellHeight;
                e.Graphics.DrawString("ORDER ID : " + TextBox_orid.Text, new Font("Century", 18, FontStyle.Regular), Brushes.Black, new Point(x, y));
                y += cellHeight;
                e.Graphics.DrawString("CUSTOMER NAME : " + label_seller.Text, new Font("Century", 18, FontStyle.Regular), Brushes.Black, new Point(x, y));
                y += cellHeight;
                e.Graphics.DrawString("ORDER DATE : " + label_date.Text, new Font("Century", 18, FontStyle.Regular), Brushes.Black, new Point(x, y));
                y += cellHeight;
                e.Graphics.DrawString("TOTAL : " + label_amount.Text, new Font("Century", 18, FontStyle.Regular), Brushes.Black, new Point(x, y));
                y += cellHeight;

            
                y += cellHeight;
                e.Graphics.DrawString("ORDER DETAILS", new Font("Century", 20, FontStyle.Bold), Brushes.Blue, new Point(x, y));
                y += cellHeight;

            
                int orderYPos = y;
                foreach (DataGridViewRow row in dataGridView_order.Rows)
                {
                    string ProdName = row.Cells[1].Value.ToString();
                    int ProdPrice = Convert.ToInt32(row.Cells[2].Value);
                    int ProdQty = Convert.ToInt32(row.Cells[3].Value);
                    int Total = Convert.ToInt32(row.Cells[4].Value);

                    e.Graphics.DrawString($"{ProdName}: {ProdQty} x {ProdPrice} = {Total}",
                        new Font("Century", 18, FontStyle.Regular), Brushes.Black, new Point(x, orderYPos));
                    orderYPos += cellHeight;
                }

 
                y = orderYPos + cellHeight;
                e.Graphics.DrawString("CREATED BY SURYA", new Font("Century", 25, FontStyle.Bold), Brushes.Red, new Point(x, y));
                y += cellHeight;
            }


            private void label_seller_Click(object sender, EventArgs e)
            {

            }

            private void datagrid_listne_CellContentClick(object sender, DataGridViewCellEventArgs e)
            {
                TextBox_id.Text = datagrid_listne.SelectedRows[0].Cells[0].Value.ToString();
                label_seller.Text = datagrid_listne.SelectedRows[0].Cells[1].Value.ToString();
                label_date.Text = datagrid_listne.SelectedRows[0].Cells[2].Value.ToString();
                label_amount.Text = datagrid_listne.SelectedRows[0].Cells[3].Value.ToString();
            }

            private void toolStripMenuItem1_Click(object sender, EventArgs e)
            {
                SellingForm selling = new SellingForm();
                selling.Show();
                this.Hide();
            }

            private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
            {
                LoginForm1 login = new LoginForm1();
                login.Show();
                this.Hide();
            }

            private void datagrid_listne_CellClick(object sender, DataGridViewCellEventArgs e)
            {

            }

            private void label_amount_Click(object sender, EventArgs e)
            {

            }

            private void label8_Click(object sender, EventArgs e)
            {

            }

            private void label_date_Click(object sender, EventArgs e)
            {

            }

            private void DataGridView_product_CellClick(object sender, DataGridViewCellEventArgs e)
            {
                if (e.RowIndex >= 0 && DataGridView_product.SelectedRows.Count > 0)
                {
                    // Dapatkan data yang dipilih dari DataGridView_product
                    string prodName = DataGridView_product.SelectedRows[0].Cells[0].Value.ToString();
                    string prodPrice = DataGridView_product.SelectedRows[0].Cells[1].Value.ToString();

                    // Tambahkan data ke DataGridView_order
                    DataGridViewRow newRow = new DataGridViewRow();
                    newRow.CreateCells(dataGridView_order);
                    newRow.Cells[0].Value = dataGridView_order.Rows.Count + 1;
                    newRow.Cells[1].Value = prodName;
                    newRow.Cells[2].Value = prodPrice;
                    newRow.Cells[3].Value = "1"; // Misalnya, jumlah awalnya 1
                    newRow.Cells[4].Value = prodPrice; // Total awalnya sama dengan harga produk
                    dataGridView_order.Rows.Add(newRow);

                    // Hitung ulang grandTotal
                    grandTotal += Convert.ToInt32(prodPrice);
                    label_amount.Text = grandTotal.ToString() + " Ribu";
                }
            }

            private void dataGridView_order_CellEndEdit(object sender, DataGridViewCellEventArgs e)
            {
                // Pastikan bahwa baris yang diedit adalah valid
                if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView_order.Columns["ProdQty"].Index)
                {
                    // Ambil nilai baru dari sel yang diedit
                    int newQty = Convert.ToInt32(dataGridView_order.Rows[e.RowIndex].Cells["ProdQty"].Value);
                    int prodPrice = Convert.ToInt32(dataGridView_order.Rows[e.RowIndex].Cells["ProdPrice"].Value);

                    // Perhitungkan total baru
                    int newTotal = newQty * prodPrice;

                    // Perbarui nilai Total di DataGridView
                    dataGridView_order.Rows[e.RowIndex].Cells["Total"].Value = newTotal.ToString();

                    // Hitung ulang grandTotal
                    Updategrandtotal();
                }
            }

            private void Updategrandtotal()
            {
                int total = 0;
                foreach (DataGridViewRow row in dataGridView_order.Rows)
                {
                    int rowTotal = Convert.ToInt32(row.Cells["Total"].Value); // Ubah "TotalColumn" sesuai dengan nama kolom yang Anda gunakan
                    total += rowTotal;
                }
                label_amount.Text = total.ToString() + " Ribu";
            }



            private void DataGridView_product_Click(object sender, EventArgs e)
            {
                TextBox_name.Text = DataGridView_product.SelectedRows[0].Cells[0].Value.ToString();
                TextBox_price.Text = DataGridView_product.SelectedRows[0].Cells[1].Value.ToString();
            }
        }
    }
