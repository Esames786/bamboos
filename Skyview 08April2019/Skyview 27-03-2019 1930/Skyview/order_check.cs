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
    public partial class order_check : Form
    {
        string currentdate = DateTime.Now.ToString("yyyy-MM-dd");
        public order_check()
        {
            InitializeComponent();
            refresh();

        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);

        public void total()
        {

            decimal exp = 0;
            string qq1 = "Select sum(amount) from expense where date between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'";
            DataTable dtt1 = clsDataLayer.RetreiveQuery(qq1);
            if (dtt1.Rows.Count > 0)
            {
                string check = dtt1.Rows[0][0].ToString();
                if (check != "")
                {
                    exp = Convert.ToDecimal(dtt1.Rows[0][0].ToString());
                }

            }

            textBox1.Text = "";
            decimal tot = 0;
            decimal n_a = 0;
            if (dataGridView1.Rows.Count > 0)
            {
                //for (int i = 0; i < dataGridView1.Rows.Count; i++)
                //{
                //    add  += Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value.ToString());
                //}
                String s1 = "";
                DataTable d1 = new DataTable();
                if (checktable.Checked == true)
                {
                    s1 = "select DISTINCT SaleCode as DSaleCode,Net_Amount from meerge where Date between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' and TokenCode != '-' "; d1 = clsDataLayer.RetreiveQuery(s1);
                    if (d1.Rows.Count > 0)
                    {
                        for (int i = 0; i < d1.Rows.Count; i++)
                        {

                            n_a = Convert.ToDecimal(d1.Rows[i][1].ToString());
                            tot = tot + n_a;

                        }

                        label3.Text = "TABLE TOTAL";
                        textBox1.Text = tot.ToString();

                    }
                }
                else if (checktake.Checked == true)
                {
                    s1 = "select DISTINCT SaleCode as DSaleCode,Net_Amount from meerge where Date between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' and TokenCode = '-' "; d1 = clsDataLayer.RetreiveQuery(s1);
                    if (d1.Rows.Count > 0)
                    {
                        for (int i = 0; i < d1.Rows.Count; i++)
                        {

                            n_a = Convert.ToDecimal(d1.Rows[i][1].ToString());
                            tot = tot + n_a;

                        }

                        label3.Text = "TAKE AWAY TOTAL";
                        textBox1.Text = tot.ToString();

                    }
                }
                else
                {
                    s1 = "SELECT DISTINCT SaleCode as DSaleCode,Net_Amount FROM meerge Where Date between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'"; d1 = clsDataLayer.RetreiveQuery(s1);
                    if (d1.Rows.Count > 0)
                    {
                        for (int i = 0; i < d1.Rows.Count; i++)
                        {

                            n_a = Convert.ToDecimal(d1.Rows[i][1].ToString());
                            tot = tot + n_a;

                        }
                        tot = tot - exp;
                        label3.Text = "All TOTAL(including Expense)";
                        textBox1.Text = tot.ToString();

                    }
                }


            }
        }
        private void order_check_Load(object sender, EventArgs e)
        {
            try
            {
                refresh();
            }
            catch { }
        }
        public void refresh()
        {
            textBox1.Text = "";

            dateTimePicker1.Text = currentdate;
            dateTimePicker2.Text = currentdate;

            String h = "SELECT TokenCode,SaleCode as DSaleCode,Product_Name,Quantity,Sell_Price,Total_Amount,FloorNo,TableNo,Waiter,UserName,Date FROM SaleView Where Date between '" + currentdate + "' and '" + currentdate + "' ORDER BY S_Id asc";
            DataTable dt = clsDataLayer.RetreiveQuery(h);
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;

                total();
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }

        public void refresh2()
        {
            textBox1.Text = "";

            SqlDataAdapter sda = new SqlDataAdapter();
            if (checktable.Checked == true)
            {
                sda = new SqlDataAdapter("SELECT TokenCode,SaleCode as DSaleCode,Product_Name,Quantity,Sell_Price,Total_Amount,FloorNo,TableNo,Waiter,UserName,Date FROM SaleView Where Date between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' and TokenCode != '-' ORDER BY S_Id asc", con);
            }
            else if (checktake.Checked == true)
            {
                sda = new SqlDataAdapter("SELECT TokenCode,SaleCode as DSaleCode,Product_Name,Quantity,Sell_Price,Total_Amount,FloorNo,TableNo,Waiter,UserName,Date FROM SaleView Where Date between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' and TokenCode = '-' ORDER BY S_Id asc", con);
            }
            else
            {
                sda = new SqlDataAdapter("SELECT TokenCode,SaleCode as DSaleCode,Product_Name,Quantity,Sell_Price,Total_Amount,FloorNo,TableNo,Waiter,UserName,Date FROM SaleView Where Date between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' ORDER BY S_Id asc", con);
            }

            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                total();
            }
            else
            {
                dataGridView1.DataSource = null;
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {

            refresh();

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            refresh2();

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {


            refresh2();


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //

        private void SaleInven()
        {
            decimal exp = 0;
            string qq1 = "Select sum(amount) from expense where date between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'";
            DataTable dtt1 = clsDataLayer.RetreiveQuery(qq1);
            if (dtt1.Rows.Count > 0)
            {
                string check = dtt1.Rows[0][0].ToString();
                if (check != "")
                {
                    exp = Convert.ToDecimal(dtt1.Rows[0][0].ToString());
                }

            }
            DataTable dv = new DataTable();
            dv.Columns.Add("Product_Name"); dv.Columns.Add("Quantity", typeof(decimal)); dv.Columns.Add("CGross_Amount", typeof(decimal));
            dv.Columns.Add("Discount", typeof(decimal)); dv.Columns.Add("Total_Amount", typeof(decimal));

            String query = "select distinct(Product_Name) from tbl_SaleByTable where Date between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'";
            DataTable dt = clsDataLayer.RetreiveQuery(query);
            if (dt.Rows.Count > 0)
            {
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    string prod = dt.Rows[a][0].ToString(); decimal dis = 0; decimal tot = 0; decimal grs = 0;
                    string qq = "select sum(Quantity),Sum(Discount),sum(Total_Amount),sum(CGross_Amount) from tbl_SaleByTable where Date between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' and Product_Name='" + prod + "'";
                    DataTable d2 = clsDataLayer.RetreiveQuery(qq);
                    if (d2.Rows.Count > 0)
                    {
                        dis = Convert.ToDecimal(d2.Rows[0][1].ToString()); tot = Convert.ToDecimal(d2.Rows[0][2].ToString()); grs = Convert.ToDecimal(d2.Rows[0][3].ToString());
                        decimal sell = 0;
                        //String s3 = "select Sell_Price from tbl_SaleByTable where Product_Name='" + prod + "'"; DataTable d3 = clsDataLayer.RetreiveQuery(s3);
                        //if (d3.Rows.Count > 0) { sell = Convert.ToDecimal(d3.Rows[0][0].ToString()); } else { sell = 0; }
                        decimal quant = Convert.ToDecimal(d2.Rows[0][0].ToString());
                        dv.Rows.Add(prod, quant, grs, dis, tot);
                    }
                }
            }
            if (dv.Rows.Count > 0)
            {
                Sale_Inventory pi = new Sale_Inventory();
                pi.SetDataSource(dv);
                Pur_inv_rpt_viewer pv = new Pur_inv_rpt_viewer(pi, dateTimePicker1.Value.ToString("yyyy-MM-dd"), dateTimePicker2.Value.ToString("yyyy-MM-dd"), "-", exp);
                pv.Show();
            }
            else
            {
                MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                decimal net = 0;
                string ccode = "";
                DataTable sms = new DataTable();
                string qq1 = "Select sum(amount) from expense where date between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'";
                DataTable dtt1 = clsDataLayer.RetreiveQuery(qq1);
                if (dtt1.Rows.Count > 0)
                {
                    string check = dtt1.Rows[0][0].ToString();
                    if (check != "")
                    {
                        net = Convert.ToDecimal(dtt1.Rows[0][0].ToString());
                    }

                }

                //
                if (checktable.Checked == true)
                {
                    textBox1.Text = "";
                    String query1 = "select DISTINCT SaleCode as DSaleCode,Gross_Amount,TotalDiscount,Net_Amount,Date from meerge  where Date between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' and TokenCode != '-'";
                    DataTable da1 = clsDataLayer.RetreiveQuery(query1);
                    if (da1.Rows.Count > 0)
                    {
                        SaleReport rt = new SaleReport(); rt.SetDataSource(da1); AllinOne aop = new AllinOne(rt, net); aop.Show();
                    }
                    else { MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                else if (checktake.Checked == true)
                {

                    textBox1.Text = "";
                    String query1 = "select DISTINCT SaleCode as DSaleCode,Gross_Amount,TotalDiscount,Net_Amount,Date from meerge  where Date between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' and TokenCode = '-'";
                    DataTable da1 = clsDataLayer.RetreiveQuery(query1);
                    if (da1.Rows.Count > 0)
                    {
                        SaleReport rt = new SaleReport(); rt.SetDataSource(da1); AllinOne aop = new AllinOne(rt, net); aop.Show();
                    }
                    else { MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                else
                {
                    textBox1.Text = "";
                    String query = "select * from tbl_Sale where Date between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'";
                    DataTable da = clsDataLayer.RetreiveQuery(query);
                    if (da.Rows.Count > 0)
                    {
                        SaleReport rt = new SaleReport(); rt.SetDataSource(da); AllinOne aop = new AllinOne(rt, net); aop.Show();
                    }
                    else { MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }

            }
            catch { }
        }



        private void checktable_MouseClick(object sender, MouseEventArgs e)
        {
            checktake.Checked = false;
            checkall.Checked = false;
            checktable.Checked = true;
            refresh2();
            label3.Text = "TABLE TOTAL";
        }

        private void checktake_MouseClick(object sender, MouseEventArgs e)
        {
            checktable.Checked = false;
            checkall.Checked = false;
            checktake.Checked = true;
            refresh2();
            label3.Text = "TAKE AWAY TOTAL";
        }

        private void checkall_MouseClick(object sender, MouseEventArgs e)
        {
            checktable.Checked = false;
            checktake.Checked = false;
            checkall.Checked = true;
            refresh2();
            label3.Text = "ALL TOTAL(including Expense)";
        }









    }
}
