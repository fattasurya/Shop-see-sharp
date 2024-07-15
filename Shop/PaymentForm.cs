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
    public partial class PaymentForm : Form
    {
        DBConnect dBCon = new DBConnect();
        string SellerId;
        int TotalHarga = 0;
        public PaymentForm(String SellerId)
        {
            InitializeComponent();
            this.SellerId = SellerId;
            
        }

        private void comboBox_orderid_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PaymentForm_Load(object sender, EventArgs e)
        {

        }

        private void button_save_Click(object sender, EventArgs e)
        {
            bool finish = false;
            try
            {
                if (comboBox_payment.Text == "")
                {
                    MessageBox.Show("Harap isi terlebih dahulu");
                    finish = false;
                }
                else if (comboBox_payment.Text == "Credit" && (textBox_cardnum.Text == "" || comboBox_bankname.Text == ""))
                {
                    MessageBox.Show("Harap Isi Card Number & Nama Bank");
                    finish = false;
                }
                else if (comboBox_payment.Text == "Cash" && (textBox_harga.Text == ""))
                {
                    MessageBox.Show("Harap Isi Input Bayar");
                    finish = false;
                }
                else
                {
                    if (comboBox_payment.Text == "Credit")
                    {
                        int valnum = 0;

                        if (int.TryParse(textBox_cardnum.Text.Trim(), out valnum))
                        {
                            //DBCon.Getcon("UPDATE OrderHeader SET EmployeeId ='" + SellerId + "', Date='" + DateTime.Now.ToString("yyyy-MM-dd") + "', PaymentType='" + comboBox_payment.Text + "', CardNumber='" + textBox_cardnum.Text + "', Bank='" + comboBox_bankname.Text + "' WHERE Id='" + comboBox_orderid.Text + "'");
                            MessageBox.Show("Pembayaran Berhasil");
                            finish = true;
                        }
                        else
                        {
                            //not a number
                            MessageBox.Show("Harap Isikan Nilai Angka dengan benar");
                            finish = false;
                        }
                    }
                    else
                    {
                        int valnum = 0;

                        if (int.TryParse(textBox_harga.Text.Trim(), out valnum))
                        {
                            if (Int32.Parse(textBox_harga.Text) < TotalHarga)
                            {
                                MessageBox.Show("Uang Anda Kurang");
                                finish = false;
                            }
                            else
                            {
                                //koneksi.cud("UPDATE OrderHeader SET EmployeeId ='" + employeeID + "', Date='" + DateTime.Now.ToString("yyyy-MM-dd") + "', PaymentType='" + cbPaymentType.Text + "' WHERE Id='" + cbOrderID.Text + "'");

                                MessageBox.Show("Pembayaran Berhasil");
                                MessageBox.Show("Kembalian Anda : " + (Int32.Parse(textBox_harga.Text) - TotalHarga).ToString());
                                finish = true;
                            }
                        }
                        else
                        {
                            //not a number
                            MessageBox.Show("Harap Isikan Nilai Angka dengan benar");
                            finish = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
              //  if (finish)
              //  { setDefault(); }


            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            
        }

        private void comboBox_payment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_payment.Text != "")
            {
                if (comboBox_payment.Text == "Credit")
                {
          
                    label5.Visible = true;
                    label4.Visible = false;

                    label5.Enabled = true;
                    label4.Enabled = false;


                    comboBox_bankname.Enabled = true;
                    textBox_harga.Enabled = false;


                    comboBox_bankname.Visible = true;
                    textBox_harga.Visible = false;
                }
                else if (comboBox_payment.Text == "Cash")
                {

                    label5.Visible = false;
                    label4.Visible = true;


                    label5.Enabled = false;
                    label4.Enabled = true;


                    comboBox_bankname.Enabled = false;
                    textBox_harga.Enabled = true;


                    comboBox_bankname.Visible = false;
                    textBox_harga.Visible = true;
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox_harga_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
