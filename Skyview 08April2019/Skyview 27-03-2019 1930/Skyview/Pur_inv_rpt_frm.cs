using Skyview.bin.Debug.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyview
{
    public partial class Pur_inv_rpt_frm : Form
    {
        public Pur_inv_rpt_frm()
        {
            InitializeComponent();
            clsGeneral.SetAutoCompleteTextBox(txt_vend_name, clsDataLayer.RetreiveQuery("select VenderName from Purchase_View")); txt_vend_name.Enabled = false; tia_name.Enabled = false;
        }

       

        private void SaleInven()
        {
            decimal net = 0;
            string qq1 = "Select sum(amount) from expense where date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "'";
            DataTable dtt1 = clsDataLayer.RetreiveQuery(qq1);
            if (dtt1.Rows.Count > 0)
            {
                string check = dtt1.Rows[0][0].ToString();
                if (check != "")
                {
                    net = Convert.ToDecimal(dtt1.Rows[0][0].ToString());
                }

            }
            DataTable dv = new DataTable();
            dv.Columns.Add("Product_Name"); dv.Columns.Add("Quantity", typeof(decimal)); dv.Columns.Add("CGross_Amount", typeof(decimal));
            dv.Columns.Add("Discount", typeof(decimal)); dv.Columns.Add("Total_Amount", typeof(decimal));

            String query = "select distinct(Product_Name) from SaleView where Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "'";
            DataTable dt = clsDataLayer.RetreiveQuery(query);
            if (dt.Rows.Count > 0)
            {
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    string prod = dt.Rows[a][0].ToString(); decimal dis = 0; decimal tot = 0; decimal grs = 0;
                    string qq = "select sum(Quantity),Sum(Discount),sum(Total_Amount),sum(CGross_Amount) from SaleView where Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "' and Product_Name='" + prod + "'";
                    DataTable d2 = clsDataLayer.RetreiveQuery(qq);
                    if (d2.Rows.Count > 0)
                    {
                        dis = Convert.ToDecimal(d2.Rows[0][1].ToString());
                        tot = Convert.ToDecimal(d2.Rows[0][2].ToString());
                        grs = Convert.ToDecimal(d2.Rows[0][3].ToString());
                        decimal sell = 0;
                        String s3 = "select Sell_Price from [tbl_ProductBySale] where Product_Name='" + prod + "'"; DataTable d3 = clsDataLayer.RetreiveQuery(s3);
                        if (d3.Rows.Count > 0)
                        {
                            sell = Convert.ToDecimal(d3.Rows[0][0].ToString());
                        }
                        else
                        {
                            sell = 0;
                        }
                        decimal quant = Convert.ToDecimal(d2.Rows[0][0].ToString());
                        decimal final = sell * quant;
                        dv.Rows.Add(prod, quant, grs, dis, tot);
                    }
                }
            }
            if (dv.Rows.Count > 0)
            {
                Sale_Inventory pi = new Sale_Inventory();
                pi.SetDataSource(dv);
                Pur_inv_rpt_viewer pv = new Pur_inv_rpt_viewer(pi, txt_dt_from.Value.ToString("yyyy-MM-dd"), txt_dt_to.Value.ToString("yyyy-MM-dd"), "-", net);
                pv.Show();
            }
            else
            {
                MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void BtnSubmit()
        {
            if (comboBox1.SelectedIndex == 0)
            {
                SaleInven();
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                //PurInven();
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                string qq1 = "select * from tbl_SaleByTableQ Where Product_Name= '" + txt_vend_name.Text + "' and Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "'";
                DataTable ds = clsDataLayer.RetreiveQuery(qq1);
                if (ds.Rows.Count > 0)
                {
                    if (txt_vend_name.Text.Length > 0)
                    {
                        DataTable dv = new DataTable();
                        dv.Columns.Add("Product_Name"); dv.Columns.Add("Quantity", typeof(decimal)); dv.Columns.Add("CGross_Amount", typeof(decimal));
                        dv.Columns.Add("Discount", typeof(decimal)); dv.Columns.Add("Total_Amount", typeof(decimal));

                        string prod = txt_vend_name.Text;
                        decimal dis = 0; decimal tot = 0; decimal grs = 0;
                        string qq = "select sum(Quantity),Sum(Discount),sum(Total_Amount),sum(CGross_Amount) from tbl_SaleByTableQ where Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "' and Product_Name='" + txt_vend_name.Text + "'";
                        DataTable d2 = clsDataLayer.RetreiveQuery(qq);
                        if (d2.Rows.Count > 0)
                        {

                            dis = Convert.ToDecimal(d2.Rows[0][1].ToString()); tot = Convert.ToDecimal(d2.Rows[0][2].ToString()); grs = Convert.ToDecimal(d2.Rows[0][3].ToString());
                            decimal sell = 0;
                            String s3 = "select Sell_Price from tbl_SaleByTableQ where Product_Name='" + txt_vend_name.Text + "'"; DataTable d3 = clsDataLayer.RetreiveQuery(s3);
                            if (d3.Rows.Count > 0) { sell = Convert.ToDecimal(d3.Rows[0][0].ToString()); } else { sell = 0; }
                            decimal quant = Convert.ToDecimal(d2.Rows[0][0].ToString()); decimal final = sell * quant;
                            dv.Rows.Add(prod, quant, grs, dis, tot);
                        }
                        if (dv.Rows.Count > 0)
                        {
                            Sale_InventoryProduct pi = new Sale_InventoryProduct();
                            pi.SetDataSource(dv);
                            SaleByProductViewer pv = new SaleByProductViewer(pi, txt_dt_from.Value.ToString("yyyy-MM-dd"), txt_dt_to.Value.ToString("yyyy-MM-dd"), "-");
                            pv.Show();
                        }
                        else
                        {
                            MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Enter Valid Name");
                    }
                }
                else
                {
                    MessageBox.Show("No Record Found");
                }
            }
            else if (comboBox1.SelectedIndex == 3)
            {


                decimal temp = 0;
                decimal de_disco = 0;

                String query2 = " select distinct(SaleCode),TotalDiscount from SaleView  where Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "' and Waiter = '" + txt_vend_name.Text + "' order by SaleCode asc";
                DataTable dt2 = clsDataLayer.RetreiveQuery(query2);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        de_disco = Convert.ToDecimal(dt2.Rows[i][1].ToString());
                        temp = temp + de_disco;
                    }
                }


                String query = "select * from SaleView where Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "' and Waiter = '" + txt_vend_name.Text + "' order by SaleCode asc";
                DataTable dt = clsDataLayer.RetreiveQuery(query);
                if (dt.Rows.Count > 0)
                {
                    Sale_Inventory pi = new Sale_Inventory();
                    pi.SetDataSource(dt);
                    tia_viewer pv = new tia_viewer(pi, txt_dt_from.Value.ToString("yyyy-MM-dd"), txt_dt_to.Value.ToString("yyyy-MM-dd"), txt_vend_name.Text, 0, temp);
                    pv.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (comboBox1.SelectedIndex == 4)
            {


                decimal temp = 0;
                decimal de_disco = 0;

                String query2 = " select distinct(SaleCode),TotalDiscount from SaleView  where Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "' and descc = '" + txt_vend_name.Text + "' order by SaleCode asc";
                DataTable dt2 = clsDataLayer.RetreiveQuery(query2);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        de_disco = Convert.ToDecimal(dt2.Rows[i][1].ToString());
                        temp = temp + de_disco;
                    }
                }

                String query = "select * from SaleView where Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "' and descc = '" + txt_vend_name.Text + "' order by SaleCode asc";
                DataTable dt = clsDataLayer.RetreiveQuery(query);
                if (dt.Rows.Count > 0)
                {

                    Sale_Inventory pi = new Sale_Inventory();
                    pi.SetDataSource(dt);
                    tia_viewer pv = new tia_viewer(pi, txt_dt_from.Value.ToString("yyyy-MM-dd"), txt_dt_to.Value.ToString("yyyy-MM-dd"), txt_vend_name.Text, 0, temp);
                    pv.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (comboBox1.SelectedIndex == 5)
            {
                string tia = "";
                if (tia_name.SelectedIndex == 0)
                {
                    tia = "T1";
                }
                else if (tia_name.SelectedIndex == 1)
                {
                    tia = "T2";
                }
                else if (tia_name.SelectedIndex == 2)
                {
                    tia = "T3";
                }
                else if (tia_name.SelectedIndex == 3)
                {
                    tia = "T4";
                }
                else if (tia_name.SelectedIndex == 4)
                {
                    tia = "T5";
                }
                else if (tia_name.SelectedIndex == 5)
                {
                    tia = "T6";
                }
                else if (tia_name.SelectedIndex == 6)
                {
                    tia = "T7";
                }
                else if (tia_name.SelectedIndex == 7)
                {
                    tia = "T8";
                }
                else if (tia_name.SelectedIndex == 8)
                {
                    tia = "T9";
                }

                decimal temp2 = 0;
                string chk_name = "";

                DataTable dv = new DataTable();
                dv.Rows.Clear();
                dv.Columns.Add("Product_Name"); dv.Columns.Add("Quantity", typeof(decimal)); dv.Columns.Add("CGross_Amount", typeof(decimal));
                dv.Columns.Add("Discount", typeof(decimal)); dv.Columns.Add("Total_Amount", typeof(decimal));


                String query4 = "select distinct(Product_Name) from SaleView where Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "'";
                DataTable dt4 = clsDataLayer.RetreiveQuery(query4);
                if (dt4.Rows.Count > 0)
                {

                    for (int l = 0; l < dt4.Rows.Count; l++)
                    {


                        chk_name = dt4.Rows[l][0].ToString();
                        String query = "select * from SaleView where Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "' and Product_Categories like '%" + tia + "' and Product_Name = '" + chk_name + "'  order by SaleCode asc";
                        DataTable dt = clsDataLayer.RetreiveQuery(query);
                        if (dt.Rows.Count > 0)
                        {
                            decimal dis = 0;
                            decimal tot = 0;
                            decimal grs = 0;
                            decimal sell = 0;
                            decimal quant = 0;
                            string pr_name = "";
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                pr_name = dt.Rows[i]["Product_Name"].ToString();


                                dis = dis + Convert.ToDecimal(dt.Rows[i]["Discount"].ToString());
                                tot = tot + Convert.ToDecimal(dt.Rows[i]["Total_Amount"].ToString());
                                grs = grs + Convert.ToDecimal(dt.Rows[i]["CGross_Amount"].ToString());
                                sell = sell + Convert.ToDecimal(dt.Rows[i]["Sell_Price"].ToString());
                                quant = quant + Convert.ToDecimal(dt.Rows[i]["Quantity"].ToString());
                                temp2 = temp2 + dis;

                            }
                            dv.Rows.Add(pr_name, quant, grs, dis, tot);

                        }
                    }
                }


                if (dv.Rows.Count > 0)
                {
                    Sale_Inventory pi = new Sale_Inventory();
                    pi.SetDataSource(dv);
                    tia_viewer pv = new tia_viewer(pi, txt_dt_from.Value.ToString("yyyy-MM-dd"), txt_dt_to.Value.ToString("yyyy-MM-dd"), txt_vend_name.Text, 0, temp2);
                    pv.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else if (comboBox1.SelectedIndex == 6)
            {
                string dd = "";

                string qq = "Select * from expense where name= '" + txt_vend_name.Text + "' and date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "'";
                DataTable dtt = clsDataLayer.RetreiveQuery(qq);
                if (dtt.Rows.Count > 0)
                {
                    expense_report sw = new expense_report();
                    sw.SetDataSource(dtt);
                    expense_view wv = new expense_view(sw, txt_dt_from.Value.ToString("yyyy-MM-dd"), txt_dt_to.Value.ToString("yyyy-MM-dd"));
                    wv.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (comboBox1.SelectedIndex == 7)
            {
                string dd = "";

                string qq = "Select * from expense where e_code= '" + txt_vend_name.Text + "' and date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "'";
                DataTable dtt = clsDataLayer.RetreiveQuery(qq);
                if (dtt.Rows.Count > 0)
                {
                    expense_report sw = new expense_report();
                    sw.SetDataSource(dtt);
                    expense_view wv = new expense_view(sw, txt_dt_from.Value.ToString("yyyy-MM-dd"), txt_dt_to.Value.ToString("yyyy-MM-dd"));
                    wv.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (comboBox1.SelectedIndex == 8)
            {
                string dd = "";

                string qq = "Select * from expense where date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "'";
                DataTable dtt = clsDataLayer.RetreiveQuery(qq);
                if (dtt.Rows.Count > 0)
                {
                    expense_report sw = new expense_report();
                    sw.SetDataSource(dtt);
                    expense_view wv = new expense_view(sw, txt_dt_from.Value.ToString("yyyy-MM-dd"), txt_dt_to.Value.ToString("yyyy-MM-dd"));
                    wv.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (comboBox1.SelectedIndex == 9)
            {
                string dd = "";

                string qq = "Select * from expense";
                DataTable dtt = clsDataLayer.RetreiveQuery(qq);
                if (dtt.Rows.Count > 0)
                {
                    expense_report sw = new expense_report();
                    sw.SetDataSource(dtt);
                    expense_view wv = new expense_view(sw, txt_dt_from.Value.ToString("yyyy-MM-dd"), txt_dt_to.Value.ToString("yyyy-MM-dd"));
                    wv.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (comboBox1.SelectedIndex == 10)
            {
                string dd = "";

                string qq = "Select * from refund where Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "'";
                DataTable dtt = clsDataLayer.RetreiveQuery(qq);
                if (dtt.Rows.Count > 0)
                {
                    refund sw = new refund();
                    sw.SetDataSource(dtt);
                    expense_view wv = new expense_view(sw, txt_dt_from.Value.ToString("yyyy-MM-dd"), txt_dt_to.Value.ToString("yyyy-MM-dd"));
                    wv.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (comboBox1.SelectedIndex == 11)
            {


                String[] str1 = new string[20];
                String get = "select member_name from member Order By id asc";
                DataTable ds = clsDataLayer.RetreiveQuery(get);
                if (ds.Rows.Count > 0)
                {
                    str1 = new string[ds.Rows.Count];

                    for (int i = 0; i < ds.Rows.Count; i++)
                    {

                        str1[i] = ds.Rows[i][0].ToString();
                    }
                }


                decimal temp = 0;
                decimal de_disco = 0;

                decimal temp2 = 0;
                decimal sale = 0;

                decimal tot = 0;


                DataTable dv = new DataTable();
                dv.Rows.Clear();
                dv.Columns.Add("UserName");
                dv.Columns.Add("CGross_Amount", typeof(decimal));
                dv.Columns.Add("Discount", typeof(decimal));
                dv.Columns.Add("Total_Amount", typeof(decimal));


                for (int a = 0; a < str1.Length; a++)
                {

                    string nn = str1[a].ToString();

                    temp = 0;
                    de_disco = 0;

                    temp2 = 0;
                    sale = 0;

                    tot = 0;

                    String query2 = " select distinct(SaleCode),TotalDiscount from SaleView  where Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "' and descc = '" + nn.ToString() + "' order by SaleCode asc";
                    DataTable dt2 = clsDataLayer.RetreiveQuery(query2);
                    if (dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            de_disco = Convert.ToDecimal(dt2.Rows[i][1].ToString());
                            temp = temp + de_disco;
                        }

                    }


                    String query5 = " select * from SaleView  where Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "' and descc = '" + nn.ToString() + "' order by SaleCode asc";
                    DataTable dt5 = clsDataLayer.RetreiveQuery(query5);
                    if (dt5.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt5.Rows.Count; i++)
                        {
                            sale = Convert.ToDecimal(dt5.Rows[i]["CGross_Amount"].ToString());
                            temp2 = temp2 + sale;
                        }

                    }

                    tot = temp2 - temp;
                    if (nn == "")
                    {
                        nn = "BAMBOO'S";
                    }
                    dv.Rows.Add(nn, temp2, temp, tot);


                }




                if (dv.Rows.Count > 0)
                {

                    total_sale pi = new total_sale();
                    pi.SetDataSource(dv);
                    overall_view pv = new overall_view(pi, txt_dt_from.Value.ToString("yyyy-MM-dd"), txt_dt_to.Value.ToString("yyyy-MM-dd"));
                    pv.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BtnSubmit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txt_vend_name.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                txt_vend_name.Enabled = false;
                tia_name.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                txt_vend_name.Enabled = true;
                tia_name.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                String quer = "select distinct Product_Name from tbl_SaleByTableQ"; DataTable ds = clsDataLayer.RetreiveQuery(quer); if (ds.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txt_vend_name, ds); }
                label4.Text = "PRODUCT NAME"; txt_vend_name.Enabled = true; tia_name.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                String quer = "select distinct Waiter from tbl_SaleByTableQ"; DataTable ds = clsDataLayer.RetreiveQuery(quer); if (ds.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txt_vend_name, ds); }
                label4.Text = "WAITER NAME"; txt_vend_name.Enabled = true; tia_name.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 4)
            {
                String quer = "select distinct descc from tbl_SaleByTableQ"; DataTable ds = clsDataLayer.RetreiveQuery(quer); if (ds.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txt_vend_name, ds); }
                label4.Text = "Deliverd By"; txt_vend_name.Enabled = true; tia_name.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 5)
            {
                tia_name.Enabled = true;
            }
            else if (comboBox1.SelectedIndex == 6)
            {
                String quer = "select distinct name from expense"; DataTable ds = clsDataLayer.RetreiveQuery(quer); if (ds.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txt_vend_name, ds); }
                label4.Text = "Expense By Name"; txt_vend_name.Enabled = true; tia_name.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 7)
            {
                String quer = "select distinct e_code from expense"; DataTable ds = clsDataLayer.RetreiveQuery(quer); if (ds.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txt_vend_name, ds); }
                label4.Text = "Expense By Code"; txt_vend_name.Enabled = true; tia_name.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 8)
            {

                txt_vend_name.Enabled = false; tia_name.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 9)
            {

                txt_vend_name.Enabled = false; tia_name.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 10)
            {

                txt_vend_name.Enabled = false; tia_name.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 11)
            {

                txt_vend_name.Enabled = false; tia_name.Enabled = false;
            }
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                { this.Close(); }
                else if (e.KeyCode == Keys.Enter)
                { BtnSubmit(); }

            }
            catch { }
        }
    }
}
