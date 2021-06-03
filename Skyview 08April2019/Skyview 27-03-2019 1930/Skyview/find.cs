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
using Microsoft.SqlServer;
using System.Diagnostics;
using Skyview.bin.Debug.Reports;

namespace Skyview
{
    public partial class find : Form
    {
        public find()
        {
            InitializeComponent();
            cat_Add();

        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);

        private void cat_Add()
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT Categories_Name FROM product_categories", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string p = dt.Rows[i][0].ToString();
                    if (p != "DEALS")
                    {
                        txtcat.Items.Add(dt.Rows[i][0]);
                    }
                }
            }
            catch { }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtdisp.Text = txtprod.Text;
            }
            catch
            {


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (listbox.Items.Count > 0)
                {
                    string temp = "";
                    for (int i = 0; i < listbox.Items.Count; i++)
                    {
                        string p_name = listbox.Items[i].ToString();


                        String c_check = "select * from SaleView where Product_Name = '" + p_name + "' and Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "'";
                        DataTable d2 = clsDataLayer.RetreiveQuery(c_check);
                        if (d2.Rows.Count > 0)
                        {
                            for (int j = 0; j < d2.Rows.Count; j++)
                            {
                                string s_code = d2.Rows[j]["SaleCode"].ToString();
                                string product_name = d2.Rows[j]["Product_Name"].ToString();
                                decimal qty = Convert.ToDecimal(d2.Rows[j]["Quantity"].ToString());
                                decimal discount = Convert.ToDecimal(d2.Rows[j]["Discount"].ToString());
                                decimal cgrosss = Convert.ToDecimal(d2.Rows[j]["CGross_Amount"].ToString());
                                decimal total_amount = Convert.ToDecimal(d2.Rows[j]["Total_Amount"].ToString());
                                string floor_no = d2.Rows[j]["FloorNo"].ToString();
                                string tableno = d2.Rows[j]["TableNo"].ToString();
                                string waiter = d2.Rows[j]["Waiter"].ToString();
                                string username = d2.Rows[j]["UserName"].ToString();
                                string description = d2.Rows[j]["descc"].ToString();
                                string date = d2.Rows[j]["Date"].ToString();

                                String c_info = "insert into finding (SaleCode,Product_Name,Quantity,Discount,CGross_Amount,Total_Amount,FloorNo,TableNo,Waiter,UserName,descc,Date) values('" + s_code + "','" + product_name + "','" + qty + "','" + discount + "','" + cgrosss + "','" + total_amount + "','" + floor_no + "','" + tableno + "','" + waiter + "','" + username + "','" + description + "','" + date + "')";
                                if (clsDataLayer.ExecuteQuery(c_info) > 0)
                                {

                                }
                            }

                        }
                        else
                        {
                            temp = temp + p_name + " NOT FOUND On this date " + "\n";
                        }

                    }
                    String c_check3 = "select * from finding";
                    DataTable d3 = clsDataLayer.RetreiveQuery(c_check3);
                    if (d3.Rows.Count > 0)
                    {
                        Sale_InventoryProduct pi = new Sale_InventoryProduct();
                        pi.SetDataSource(d3);
                        SaleByProductViewer pv = new SaleByProductViewer(pi, txt_dt_from.Value.ToString("yyyy-MM-dd"), txt_dt_to.Value.ToString("yyyy-MM-dd"), "-");
                        pv.Show();
                    }
                    String trunc = "truncate table finding";
                    if (clsDataLayer.ExecuteQuery(trunc) > 0)
                    {

                    }
                    if (temp.Length > 0)
                    {
                        MessageBox.Show(temp);
                    }
                }
                else
                {
                    MessageBox.Show("Add items first");
                }

            }
            catch
            {


            }
        }

        private void txtcat_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                txtprod.Items.Clear();
                txtprod.Text = "";
                txtdisp.Text = "";
                SqlDataAdapter sda = new SqlDataAdapter("SELECT  distinct(Product_Name) FROM tbl_ProductBySale where Product_Categories = '" + txtcat.Text + "' ", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    txtprod.Items.Add(dt.Rows[i][0]);
                }

                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT distinct(Product_Name) FROM tbl_deals where Product_Categories = '" + txtcat.Text + "' ", con);
                DataTable dt1 = new DataTable();
                sda1.Fill(dt1);
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    txtprod.Items.Add(dt1.Rows[i][0]);
                }


            }
            catch { }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtdisp.Text.Length > 0)
                {
                    string it = txtdisp.Text.ToString();
                    int chk = 0;
                    for (int i = 0; i < listbox.Items.Count; i++)
                    {
                        string li_item = listbox.Items[i].ToString();

                        if (li_item == it)
                        {
                            MessageBox.Show("ALREADY ADD");
                            chk = 1;
                        }

                    }
                    if (chk != 1)
                    {
                        listbox.Items.Add(it);
                    }
                }
            }
            catch
            {

            }
        }

        private void btnremo_Click(object sender, EventArgs e)
        {
            try
            {
                listbox.Items.Remove(txtdisp.Text);
            }
            catch
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtcat.Text = "";
            txtprod.Text = "";
            txtdisp.Clear();
            listbox.Items.Clear();
        }

        private void listbox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    for (int i = 0; i < listbox.SelectedItems.Count; i++)
                        listbox.Items.Remove(listbox.SelectedItems[i]);
                }
            }
            catch { }
        }

        private void listbox_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                
                for (int i = 0; i < listbox.SelectedItems.Count; i++){
                    string a = listbox.SelectedItems[i].ToString();
                    txtdisp.Text = a;
                }
                    
            }
            catch 
            {
               
            }
        }


    }
}
