using CrystalDecisions.Shared;
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
    public partial class PrintReceipt : Form
    {
        DataTable dv;
        public static String Tno = ""; public static String FloorNo = "";
        FoodOrder fg; String hj = "";

        public PrintReceipt(String Tno1, String Fno, FoodOrder fh, String fname)
        {
            InitializeComponent(); Tno = Tno1; FloorNo = Fno; fg = fh; hj = fname; checkBox1.Checked = true;
            clsGeneral.SetAutoCompleteTextBox(c_name, clsDataLayer.RetreiveQuery("select distinct(c_name) from tbl_SaleByTableQ "));
            clsGeneral.SetAutoCompleteTextBox(txtphone, clsDataLayer.RetreiveQuery("select distinct(c_phone) from tbl_SaleByTableQ"));

            
            member();
            comboBox1.Text = "";
            txtbill.Focus();

        }

        public void member()
        {
            try
            {

                String get = "select member_name from member Order By id asc";
                DataTable ds = clsDataLayer.RetreiveQuery(get);
                if (ds.Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        comboBox1.Items.Add(ds.Rows[i][0].ToString());
                    }
                }
            }
            catch
            {


            }
        }


        public void description()
        {
            try
            {
                if (comboBox1.Text != "")
                {
                    String sel = "select descc from tbl_SaleByTableQ where SaleCode='" + txtsale.Text + "'"; DataTable ds = clsDataLayer.RetreiveQuery(sel);
                    if (ds.Rows.Count > 0)
                    {
                        string desc = comboBox1.Text;
                        string ins = "update tbl_SaleByTableQ set descc ='" + desc + "' where SaleCode='" + txtsale.Text + "'";
                        clsDataLayer.ExecuteQuery(ins);
                    }
                }
            }
            catch
            {
            }
        }

        public void GetTotal()
        {
            decimal gs = 0; decimal dis = 0; decimal net = 0;
            decimal igs = Convert.ToDecimal(txtbill.Text); decimal idis = Convert.ToDecimal(txtdis.Text);
            String sel = "select Gross_Amount,TotalDiscount,Net_Amount from tbl_Sale where DSaleCode='" + txtsale.Text + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(sel); if (d1.Rows.Count > 0)
            {
                gs = Convert.ToDecimal(d1.Rows[0][0].ToString()); dis = Convert.ToDecimal(d1.Rows[0][1].ToString()); net = Convert.ToDecimal(d1.Rows[0][2].ToString());
                dis += idis; decimal final = gs - dis;

                String del = "delete from tbl_Sale where DSaleCode='" + txtsale.Text + "'"; clsDataLayer.ExecuteQuery(del);
                String ins = "insert into tbl_Sale(Date,DSaleCode,Gross_Amount,TotalDiscount,Net_Amount)values('" + fg.dateTimePicker1.Text + "','" + txtsale.Text + "'," + gs + "," + dis + "," + final + ")"; clsDataLayer.ExecuteQuery(ins);
            }
        }


        private void btngen_Click(object sender, EventArgs e)
        {
            if (txtrec.Text.Length > 0)
            {

                if (Convert.ToDecimal(txtrec.Text) > 0)
                {

                    c_info();
                    GetTotal();
                    description();
                    Generate(); fg.createtablebutton(); fg.createbutton(); fg.grid.Rows.Clear();
                }
                else
                {
                    MessageBox.Show("Kindly Receive some amount");
                }
            }
            else
            {
                MessageBox.Show("Kindly Receive some amount");
            }
        }

        public void c_info()
        {
            try
            {
                if (txtrec.Text != "0")
                {
                    if (txtphone.Text.Length == 0)
                    {
                        txtphone.Text = "00";
                    }
                    if (c_name.Text.Length == 0)
                    {
                        c_name.Text = "-";
                    }

                    String c_info = "update [tbl_SaleByTableQ] set c_name = '" + c_name.Text + "' , c_phone = '" + txtphone.Text + "' Where SaleCode  = '" + txtsale.Text + "'";
                    clsDataLayer.ExecuteQuery(c_info);

                }
                else { MessageBox.Show("Kindly Receive the bill amount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
            }
            catch { }
        }

        private void SaleInven()
        {
            dv = new DataTable();
            dv.Columns.Add("Product_Name"); dv.Columns.Add("Quantity", typeof(decimal));
            dv.Columns.Add("Sell_Price", typeof(decimal));
            dv.Columns.Add("CGross_Amount", typeof(decimal));
            dv.Columns.Add("Discount", typeof(decimal)); dv.Columns.Add("Total_Amount", typeof(decimal));
            dv.Columns.Add("Gross_Amount", typeof(decimal));
            dv.Columns.Add("TotalDiscount", typeof(decimal)); dv.Columns.Add("Net_Amount", typeof(decimal));

            dv.Columns.Add("SaleCode"); dv.Columns.Add("TokenCode");
            dv.Columns.Add("FloorNo");
            dv.Columns.Add("TableNo"); dv.Columns.Add("Waiter"); dv.Columns.Add("UserName");
            #region abcd
            String query = "select distinct(Product_Name) from tbl_SaleByTable where SaleCode='" + txtsale.Text + "' ";
            DataTable dt = clsDataLayer.RetreiveQuery(query);
            if (dt.Rows.Count > 0)
            {
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    string prod = dt.Rows[a][0].ToString(); decimal dis = 0; decimal tot = 0; decimal grs = 0;
                    string qq = "select sum(Quantity),Sum(Discount),sum(Total_Amount),sum(CGross_Amount) from tbl_SaleByTable where Product_Name='" + prod + "' and SaleCode='" + txtsale.Text + "'";
                    DataTable d2 = clsDataLayer.RetreiveQuery(qq);
                    if (d2.Rows.Count > 0)
                    {
                        dis = Convert.ToDecimal(d2.Rows[0][1].ToString()); tot = Convert.ToDecimal(d2.Rows[0][2].ToString()); grs = Convert.ToDecimal(d2.Rows[0][3].ToString());
                        decimal sell = 0;

                        decimal gs0 = 0; decimal gs1 = 0; decimal gs2 = 0;
                        String h3 = "select distinct Gross_Amount,TotalDiscount,Net_Amount from tbl_SaleByTable where SaleCode='" + txtsale.Text + "'";
                        DataTable h30 = clsDataLayer.RetreiveQuery(h3); if (h30.Rows.Count > 0)
                        {
                            for (int h = 0; h < h30.Rows.Count; h++)
                            {
                                gs0 += Convert.ToDecimal(h30.Rows[h][0].ToString()); gs1 += Convert.ToDecimal(h30.Rows[h][1].ToString()); gs2 += Convert.ToDecimal(h30.Rows[h][2].ToString());
                            }
                        }

                        String Token = ""; String Floor = ""; String Waiter = ""; String Table = "";
                        String s3 = "select Sell_Price,TokenCode,FloorNo,TableNo,Waiter from tbl_SaleByTable where Product_Name='" + prod + "' and SaleCode='" + txtsale.Text + "'"; DataTable d3 = clsDataLayer.RetreiveQuery(s3);
                        if (d3.Rows.Count > 0)
                        {
                            sell = Convert.ToDecimal(d3.Rows[0][0].ToString()); Token = d3.Rows[0][1].ToString(); FloorNo = d3.Rows[0][2].ToString();
                            Table = d3.Rows[0][3].ToString(); Waiter = d3.Rows[0][4].ToString();
                        }
                        else { sell = 0; Token = ""; FloorNo = ""; Waiter = ""; Table = ""; }
                        decimal quant = Convert.ToDecimal(d2.Rows[0][0].ToString()); decimal final = sell * quant;
            #endregion abcd
                        dv.Rows.Add(prod, quant, sell, grs, dis, tot, gs0, gs1, gs2, txtsale.Text, Token, Floor, Table, Waiter, Login.UserID);
                    }
                }
            }
            if (dv.Rows.Count > 0)
            {
            }
            else
            {
                MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Generate()
        {
            try
            {
                String del = "delete from tbl_SaleReceipt where SaleCode='" + txtsale.Text + "'"; clsDataLayer.ExecuteQuery(del);
                String ins = "insert into tbl_SaleReceipt(SaleCode,Table_No,FloorNo,BillAmount,CashReceived,ReturnAmount,Date,UserName)values('" + txtsale.Text + "','" + Tno + "','" + FloorNo + "'," + txtbill.Text + "," + txtrec.Text + "," + txtreturn.Text + ",'" + fg.dateTimePicker1.Text + "','" + Login.UserID + "')";
                if (clsDataLayer.ExecuteQuery(ins) > 0) { }


                
                string[] array = new string[10];
                string[] array1 = new string[10];
                string[] q = new string[10];
                string[] q1 = new string[10];
                decimal txt_return = Convert.ToDecimal(txtreturn.Text);
                decimal txt_rec = Convert.ToDecimal(txtrec.Text);

                DataTable dv = new DataTable();
                dv.Rows.Clear();
                dv.Columns.Add("SaleCode");
                dv.Columns.Add("TokenCode");
                dv.Columns.Add("Product_Name");
                dv.Columns.Add("Quantity", typeof(decimal));
                dv.Columns.Add("Sell_Price", typeof(decimal));
                dv.Columns.Add("Discount", typeof(decimal));
                dv.Columns.Add("CGross_Amount", typeof(decimal));
                dv.Columns.Add("Total_Amount", typeof(decimal));
                dv.Columns.Add("FloorNo");
                dv.Columns.Add("TableNo");
                dv.Columns.Add("Waiter");
                dv.Columns.Add("UserName");
                dv.Columns.Add("descc");
                dv.Columns.Add("c_name");
                dv.Columns.Add("c_phone");
                dv.Columns.Add("c_address");
                dv.Columns.Add("Gross_Amount", typeof(decimal));
                dv.Columns.Add("TotalDiscount", typeof(decimal));
                dv.Columns.Add("Net_Amount", typeof(decimal));
                dv.Columns.Add("Date");


                String query9 = "select  distinct Product_Categories from my_test where SaleCode='" + txtsale.Text + "'"; DataTable da9 = clsDataLayer.RetreiveQuery(query9);
                if (da9.Rows.Count > 0)
                {

                    array = new string[da9.Rows.Count];
                    array1 = new string[da9.Rows.Count];
                    q = new string[array1.Length + array.Length];
                    q1 = new string[array1.Length + array.Length];


                    for (int j = 0; j < da9.Rows.Count; j++)
                    {
                        string chk_cat_d = da9.Rows[j]["Product_Categories"].ToString();
                        if (chk_cat_d != "")
                        {
                            array1[j] = (chk_cat_d).Trim();
                        }
                    }

                    q1 = array1.Distinct().ToArray();
                    for (int m = 0; m < q1.Length; m++)
                    {
                        if (q1[m] != null)
                        {
                            string my = q1[m].ToString();
                            string strTrimmed = my.Trim();
                            string finalString = strTrimmed.Substring(strTrimmed.LastIndexOf(" ", strTrimmed.Length));
                            q1[m] = finalString;
                        }
                    }
                    q1 = q1.Distinct().ToArray();
                }

                string qchk = "";
                string tb = "TB";
                decimal service_c = 0;
                string s_code = "";
                string token_code = "";
                string pr_name = "";
                decimal qtyy = 0;
                decimal sell_price = 0;
                decimal discount = 0;
                decimal c_gross = 0;
                decimal total_amount = 0;
                string floor_no = "";
                string table_no = "";
                string waiter = "";
                string username = "";
                string descc = "";
                string c_name = "";
                string c_phone = "";
                string c_address = "";
                decimal gr_amount = 0;
                decimal to_dis = 0;
                decimal net_amount = 0;
                string datee = "";


                for (int n = 0; n < q1.Length; n++)
                {

                    if (q1[n] != null)
                    {
                        qchk = q1[n].Trim();
                        String query = "select * from SaleView where SaleCode='" + txtsale.Text + "' and Product_Categories like '%" + q1[n] + "'";
                        DataTable da = clsDataLayer.RetreiveQuery(query);
                        if (da.Rows.Count > 0)
                        {
                            for (int i = 0; i < da.Rows.Count; i++)
                            {
                                if (!qchk.Equals(tb))
                                {
                                    s_code = da.Rows[i]["SaleCode"].ToString();
                                    token_code = da.Rows[i]["TokenCode"].ToString();
                                    pr_name = da.Rows[i]["Product_Name"].ToString();
                                    qtyy = Convert.ToDecimal(da.Rows[i]["Quantity"].ToString());
                                    sell_price = Convert.ToDecimal(da.Rows[i]["Sell_Price"].ToString());
                                    discount = Convert.ToDecimal(da.Rows[i]["Discount"].ToString());
                                    c_gross = Convert.ToDecimal(da.Rows[i]["CGross_Amount"].ToString());
                                    total_amount = Convert.ToDecimal(da.Rows[i]["Total_Amount"].ToString());
                                    floor_no = da.Rows[i]["FloorNo"].ToString();
                                    table_no = da.Rows[i]["TableNo"].ToString();
                                    waiter = da.Rows[i]["Waiter"].ToString();
                                    username = da.Rows[i]["UserName"].ToString();
                                    descc = da.Rows[i]["descc"].ToString();
                                    c_name = da.Rows[i]["c_name"].ToString();
                                    c_phone = da.Rows[i]["c_phone"].ToString();
                                    c_address = da.Rows[i]["c_address"].ToString();
                                    gr_amount = Convert.ToDecimal(da.Rows[i]["Gross_Amount"].ToString());
                                    to_dis = Convert.ToDecimal(da.Rows[i]["TotalDiscount"].ToString());
                                    net_amount = Convert.ToDecimal(da.Rows[i]["Net_Amount"].ToString());
                                    datee = da.Rows[i]["Date"].ToString();

                                    String query3 = "select Product_Name from tbl_deals where Product_Name='" + pr_name + "'"; DataTable da3 = clsDataLayer.RetreiveQuery(query3);
                                    if (da3.Rows.Count > 0)
                                    {
                                        sell_price = 0;
                                        total_amount = 0;
                                    }

                                    dv.Rows.Add(s_code, token_code, pr_name, qtyy, sell_price, discount, c_gross, total_amount, floor_no, table_no, waiter, username, descc, c_name, c_phone, c_address, gr_amount, to_dis, net_amount, datee);
                                }
                                else
                                {
                                    pr_name = da.Rows[i]["Product_Name"].ToString();
                                    c_gross = Convert.ToDecimal(da.Rows[i]["CGross_Amount"].ToString());
                                    service_c = service_c + c_gross;
                                }
                            }

                        }
                        else { MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                }

                if (dv.Rows.Count > 0)
                {
                    String upd = "update tbl_SaleByTableQ set TableStatus='DeActive' where TableNo='" + Tno + "' and FloorNo='" + FloorNo + "' and SaleCode='" + txtsale.Text + "'"; clsDataLayer.ExecuteQuery(upd);
                    receiptByTable5 rt = new receiptByTable5(); rt.SetDataSource(dv);
                    view_close2 aop = new view_close2(rt, txt_rec, txt_return, Convert.ToDecimal(txtdis.Text),service_c);
                    aop.Show();
                    if (checkBox1.Checked == true)
                    {
                        rt.PrintToPrinter(1, false, 0, 0);
                    }
                    this.Close();
                }


            }
            catch { }
        }

        private void txtrec_TextChanged(object sender, EventArgs e)
        {
            b_am();
        }

        public void b_am()
        {
            try
            {
                if (txtrec.Text.Equals("")) { txtreturn.Text = txtbill.Text; }
                else
                {
                    decimal bill = Convert.ToDecimal(txtbill.Text); decimal rec = Convert.ToDecimal(txtrec.Text); decimal final = rec - bill;
                    decimal aa = Convert.ToDecimal(txtbill.Text); decimal bb = Convert.ToDecimal(txtrec.Text);
                    if (aa > bb)
                    {
                        String f = final.ToString();
                        if (f.StartsWith("-"))
                        {
                            txtdis.Text = Math.Abs(final).ToString(); final = 0;
                        }
                    }
                    else
                    {
                        txtdis.Text = "0";
                    }
                    txtreturn.Text = final.ToString();
                }
            }
            catch { }
        }

        private void txtrec_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtrec.Text.Length > 0)
                {

                    if (Convert.ToDecimal(txtrec.Text) > 0)
                    {

                        c_info();
                        GetTotal();
                        Generate(); fg.createtablebutton(); fg.createbutton(); fg.grid.Rows.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Kindly Receive some amount");
                    }
                }
                else
                {
                    MessageBox.Show("Kindly Receive some amount");
                }
            }
        }

        private void txtsale_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal gs0 = 0;
                String h3 = "select Net_Amount from tbl_Sale where DSaleCode='" + txtsale.Text + "'";
                DataTable h30 = clsDataLayer.RetreiveQuery(h3); if (h30.Rows.Count > 0)
                {
                    gs0 = Convert.ToDecimal(h30.Rows[0][0].ToString()); txtbill.Text = gs0.ToString();
                }
            }
            catch { }
        }

        private void txtphone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void c_name_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
                e.Handled = true;
        }

        private void txtrec_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txt_add_dis_TextChanged(object sender, EventArgs e)
        {
            add_dis();
        }

        public void add_dis()
        {
            try
            {


                if (txt_add_dis.Text != "")
                {
                    b_am();

                    decimal bill = Convert.ToDecimal(txtbill.Text);
                    decimal cash_rec = Convert.ToDecimal(txtrec.Text);

                    decimal add_dis = Convert.ToDecimal(txt_add_dis.Text);
                    decimal dis = Convert.ToDecimal(txtdis.Text);

                    decimal ret = Convert.ToDecimal(txtreturn.Text);

                    decimal tot = dis + add_dis;

                    decimal tot_r = ret + tot;

                    decimal my_re = (bill - cash_rec - tot) * (-1);

                    if (ret > 0)
                    {
                        txtreturn.Text = tot_r.ToString();
                    }
                    else
                    {
                        txtreturn.Text = my_re.ToString();
                    }

                    txtdis.Text = tot.ToString();


                }
                else
                {
                    b_am();
                }


            }
            catch
            {

            }
        }

        private void txtrec_Leave(object sender, EventArgs e)
        {
            comboBox1.Focus();
        }

        private void txt_add_dis_Leave(object sender, EventArgs e)
        {
            btngen.Focus();
        }

        private void txt_add_dis_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            txt_add_dis.Focus();
        }






    }
}
