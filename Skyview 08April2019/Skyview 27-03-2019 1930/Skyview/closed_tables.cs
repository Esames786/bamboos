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
namespace Skyview
{
    public partial class closed_tables : Form
    {
        public closed_tables()
        {
            InitializeComponent();
            close_table();

            txttotal.Enabled = false;
        }


        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        string datee = DateTime.Now.ToString("yyyy-MM-dd");
        public void close_table()
        {
            try
            {
                decimal sum = 0;
                decimal temp = 0;
                SqlDataAdapter sda = new SqlDataAdapter("select * from tbl_SaleReceipt where Date = '" + datee.ToString() + "' and Table_No LIKE '%T%' ", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    grid3.Rows.Clear();
                    foreach (DataRow item in dt.Rows)
                    {

                        int n = grid3.Rows.Add();
                        grid3.Rows[n].Cells[0].Value = item["SaleCode"].ToString();
                        grid3.Rows[n].Cells[1].Value = item["Table_No"].ToString();
                        grid3.Rows[n].Cells[2].Value = item["FloorNo"].ToString();
                        grid3.Rows[n].Cells[3].Value = item["BillAmount"].ToString();
                        grid3.Rows[n].Cells[4].Value = item["CashReceived"].ToString();
                        grid3.Rows[n].Cells[5].Value = item["ReturnAmount"].ToString();
                        grid3.Rows[n].Cells[6].Value = item["Date"].ToString();
                        temp = Convert.ToDecimal(item["BillAmount"].ToString());
                        sum = sum + temp;
                    }
                    txttotal.Text = sum.ToString();
                }
                else
                {
                    grid3.DataSource = null;
                    grid3.Rows.Clear();
                }
            }
            catch
            {


            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            datee = dateTimePicker1.Text;
            close_table();
        }

        private void txttotal_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
