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
using Skyview.bin.Debug.Reports;

namespace Skyview
{
    public partial class View_Product_Stock : Form
    {
        public View_Product_Stock()
        {
            InitializeComponent(); comboSearch.SelectedIndex = 0;
         }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);

        private void showall()
        {
            try
            {
                this.txtSearch.Focus();


                decimal total = 0; decimal pp = 0;
                SqlDataAdapter sda = new SqlDataAdapter("select ProductName,Quantity,Dates,UserName from tblStockMaintain order by ProductName asc", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                    foreach(DataRow dr in dt.Rows)
                    {
                        int n = dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells[0].Value = dr[0].ToString();
                        dataGridView1.Rows[n].Cells[1].Value = dr[1].ToString();
                        dataGridView1.Rows[n].Cells[2].Value = dr[2].ToString();
                        dataGridView1.Rows[n].Cells[3].Value = dr[3].ToString();

                    }
                 //   dataGridView1.DataSource = dt;
                    for (int n = 0; n < dataGridView1.Rows.Count; n++)
                    {
                        decimal val = Convert.ToDecimal(dataGridView1.Rows[n].Cells[1].Value.ToString());

                        if (val < 2)
                        {
                            dataGridView1.Rows[n].Cells[1].Style.BackColor = Color.Yellow;
                        }
                        else if (val == 2)
                        {
                            dataGridView1.Rows[n].Cells[1].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            dataGridView1.Rows[n].Cells[1].Style.BackColor = Color.White;
                        }

                        decimal val1 = Convert.ToDecimal(dataGridView1.Rows[n].Cells[1].Value.ToString());
                        if (val1 > 0)
                        {
                            total += Convert.ToDecimal(dataGridView1.Rows[n].Cells[1].Value.ToString());
                        }
                        txtQuantity.Text = total.ToString();

                    }
                }
                else
                {
                    dataGridView1.Rows.Clear();
                    txtQuantity.Text = "0";
                }
                dataGridView1.PerformLayout();
            }
            catch { }
        }
        private void View_Product_Stock_Load(object sender, EventArgs e)
        {
            showall();
         }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
             try
                {
                    String qs = "";
           
                if (comboSearch.SelectedIndex==0)
            {
                 qs = "SELECT ProductName,Quantity,Dates,UserName FROM tblStockMaintain WHERE ProductName = '" + txtSearch.Text + "'";
            } 
            else if (comboSearch.Text == "Quantity")
            {
                qs = "SELECT ProductName,Quantity,Dates,UserName FROM tblStockMaintain WHERE Quantity = '" + txtSearch.Text + "'";
            } 
                decimal total = 0; decimal pp = 0;
 
                    DataTable dt = clsDataLayer.RetreiveQuery(qs);
                    if (dt.Rows.Count > 0)
                    {
                    dataGridView1.Rows.Clear();
                    foreach (DataRow dr in dt.Rows)
                    {
                        int n = dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells[0].Value = dr[0].ToString();
                        dataGridView1.Rows[n].Cells[1].Value = dr[1].ToString();
                        dataGridView1.Rows[n].Cells[2].Value = dr[2].ToString();
                        dataGridView1.Rows[n].Cells[3].Value = dr[3].ToString();

                    }
                    //   dataGridView1.DataSource = dt;

                    for (int n = 0; n < dataGridView1.Rows.Count;n++)
                        { 
                            decimal val = Convert.ToDecimal(dataGridView1.Rows[n].Cells[1].Value.ToString());

                            if (val < 5)
                            {
                                dataGridView1.Rows[n].Cells[1].Style.BackColor = Color.Yellow;
                            }
                            else if (val == 10)
                            {
                                dataGridView1.Rows[n].Cells[1].Style.BackColor = Color.LightGreen;
                            }
                            else
                            {
                                dataGridView1.Rows[n].Cells[1].Style.BackColor = Color.White;
                            }

                            decimal val1 = Convert.ToDecimal(dataGridView1.Rows[n].Cells[1].Value.ToString());
                            if (val1 > 0)
                            {
                                total += Convert.ToDecimal(dataGridView1.Rows[n].Cells[1].Value.ToString());
                            }
                            txtQuantity.Text = total.ToString(); 
                        }
                        dataGridView1.PerformLayout();
                    } else { showall(); }
                }
             catch  { }
        }

        private void comboSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            { 
                if (comboSearch.Text == "ProductName")
                {
                    txtSearch.Clear();
                    String query = "select distinct ProductName  from tblStockMaintain";
                    DataTable dts = clsDataLayer.RetreiveQuery(query);
                    if (dts.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txtSearch, dts);
                    }
                }
                else if (comboSearch.Text == "Quantity")
                {
                    txtSearch.Clear();
                    String query = "select distinct Quantity  from tblStockMaintain";
                    DataTable dts = clsDataLayer.RetreiveQuery(query);
                    if (dts.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txtSearch, dts);
                    }
                }  
            }
            catch { }
        }
        private void View_Product_Stock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnprint_Click(object sender, EventArgs e)
        {
            try
            {
                
                string q = "";
                if (txtSearch.Text.Equals(""))
                {
                    q = "select * from tblStockMaintain";
                }
                else if (comboSearch.Text == "ProductName")
                {
                    q = "select * from tblStockMaintain where ProductName='" + txtSearch.Text + "'";
                }
                else if (comboSearch.Text == "Quantity")
                {
                    q = "select * from tblStockMaintain where Quantity='" + txtSearch.Text + "' ";
                }  
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    StockReport rpt = new StockReport();
                    rpt.SetDataSource(purchase);
                    PaymentView pop = new PaymentView(rpt);
                    pop.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!","Stop",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                }
            }
            catch { }
        }

         
    }
}
