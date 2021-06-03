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
    public partial class Orders : Form
    {
        DataTable dv;
        FoodOrder fg;
        public Orders()
        {
            InitializeComponent();

            DataTable fm = new DataTable(); fm.Columns.Add("Table_Name");
            String sb = "select Table_Name from tbl_table"; DataTable d1 = clsDataLayer.RetreiveQuery(sb);
            if (d1.Rows.Count > 0)
            {
                for (int y = 0; y < d1.Rows.Count; y++)
                {
                    String Tname = d1.Rows[y][0].ToString();
                    String b1 = "select TableStatus from tbl_SaleByTableQ where TableNo='" + Tname + "'";
                    DataTable d2 = clsDataLayer.RetreiveQuery(b1);
                    if (d2.Rows.Count > 0)
                    {
                        String status = d2.Rows[0][0].ToString();
                        if (status.Equals("Active"))
                        {

                        }
                        else
                        {
                            fm.Rows.Add(Tname);
                        }
                    }
                    else
                    {
                        fm.Rows.Add(Tname);
                    }
                }
            }
            clsGeneral.SetAutoCompleteTextBox(txttable, fm);

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
            String query = "select distinct(Product_Name) from tbl_SaleByTable where SaleCode='" + txtcode.Text + "' ";
            DataTable dt = clsDataLayer.RetreiveQuery(query);
            if (dt.Rows.Count > 0)
            {
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    string prod = dt.Rows[a][0].ToString(); decimal dis = 0; decimal tot = 0; decimal grs = 0;
                    string qq = "select sum(Quantity),Sum(Discount),sum(Total_Amount),sum(CGross_Amount) from tbl_SaleByTable where Product_Name='" + prod + "' and SaleCode='" + txtcode.Text + "'";
                    DataTable d2 = clsDataLayer.RetreiveQuery(qq);
                    if (d2.Rows.Count > 0)
                    {
                        dis = Convert.ToDecimal(d2.Rows[0][1].ToString()); tot = Convert.ToDecimal(d2.Rows[0][2].ToString()); grs = Convert.ToDecimal(d2.Rows[0][3].ToString());
                        decimal sell = 0;

                        decimal gs0 = 0; decimal gs1 = 0; decimal gs2 = 0;
                        String h3 = "select distinct Gross_Amount,TotalDiscount,Net_Amount from tbl_SaleByTable where SaleCode='" + txtcode.Text + "'";
                        DataTable h30 = clsDataLayer.RetreiveQuery(h3); if (h30.Rows.Count > 0)
                        {
                            for (int h = 0; h < h30.Rows.Count; h++)
                            {
                                gs0 += Convert.ToDecimal(h30.Rows[h][0].ToString()); gs1 += Convert.ToDecimal(h30.Rows[h][1].ToString()); gs2 += Convert.ToDecimal(h30.Rows[h][2].ToString());
                            }
                        }

                        String s3 = "select Sell_Price,Gross_Amount,TotalDiscount,Net_Amount from tbl_SaleByTable where Product_Name='" + prod + "' and SaleCode='" + txtcode.Text + "'"; DataTable d3 = clsDataLayer.RetreiveQuery(s3);
                        if (d3.Rows.Count > 0)
                        {
                            sell = Convert.ToDecimal(d3.Rows[0][0].ToString());
                        }
                        else { sell = 0; }
                        decimal quant = Convert.ToDecimal(d2.Rows[0][0].ToString()); decimal final = sell * quant;
            #endregion abcd
                        dv.Rows.Add(prod, quant, sell, grs, dis, tot, gs0, gs1, gs2, txtcode.Text, txttoken.Text, txtfloor.Text, txttable.Text, txtwaiter.Text, Login.UserID);
                    }
                }
            }
            if (dv.Rows.Count > 0)
            {
                grid.Rows.Clear();
                foreach (DataRow dr in dv.Rows)
                {
                    int n = grid.Rows.Add();
                    grid.Rows[n].Cells[0].Value = dr["Product_Name"].ToString(); grid.Rows[n].Cells[1].Value = dr["Quantity"].ToString(); grid.Rows[n].Cells[2].Value = dr["Sell_Price"].ToString();
                    grid.Rows[n].Cells[3].Value = dr["CGross_Amount"].ToString(); grid.Rows[n].Cells[4].Value = dr["Discount"].ToString(); grid.Rows[n].Cells[5].Value = dr["Total_Amount"].ToString();
                }
                total();
            }
            else
            {
                MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loaddata()
        {
            try
            {
                String query = "select * from SaleView where SaleCode='" + txtcode.Text + "'"; DataTable da = clsDataLayer.RetreiveQuery(query);
                if (da.Rows.Count > 0)
                {
                    txttable.Text = da.Rows[0]["TableNo"].ToString(); txtfloor.Text = da.Rows[0]["FloorNo"].ToString();
                    txtwaiter.Text = da.Rows[0]["Waiter"].ToString(); txttoken.Text = da.Rows[0]["TokenCode"].ToString();

                    txtbill.Text = da.Rows[0]["Gross_Amount"].ToString();
                    txtdis.Text = da.Rows[0]["TotalDiscount"].ToString(); txtnet.Text = da.Rows[0]["Net_Amount"].ToString();
                    dateTimePicker1.Text = da.Rows[0]["Date"].ToString();

                    grid.Rows.Clear();
                    foreach (DataRow dr in da.Rows)
                    {
                        int n = grid.Rows.Add();
                        grid.Rows[n].Cells[0].Value = dr["Product_Name"].ToString();
                        grid.Rows[n].Cells[1].Value = dr["Quantity"].ToString();
                        grid.Rows[n].Cells[2].Value = dr["Sell_Price"].ToString();
                        grid.Rows[n].Cells[3].Value = dr["CGross_Amount"].ToString();
                        grid.Rows[n].Cells[4].Value = dr["Discount"].ToString();
                        grid.Rows[n].Cells[5].Value = dr["Total_Amount"].ToString();
                    }
                }
                else
                {
                    grid.Rows.Clear();
                }
                //  SaleInven();
            }
            catch { }
        }

        private void txtcode_TextChanged(object sender, EventArgs e)
        {
            loaddata();
        }

        private void grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            grid.Rows[e.RowIndex].Cells[6].Value = "Delete";
        }


        private void individualstock()
        {
            String pdname = grid.CurrentRow.Cells[0].Value.ToString();
            decimal InputQuant = Convert.ToDecimal(grid.CurrentRow.Cells[1].Value.ToString());
            #region all

            String get1 = "select ReceipeProductName,Quantity from vwconsumption where SaleProductName ='" + pdname + "'";
            DataTable d12 = clsDataLayer.RetreiveQuery(get1);
            if (d12.Rows.Count > 0)
            {
            #endregion cond

                for (int a = 0; a < d12.Rows.Count; a++)
                {
                    String Receprod = d12.Rows[a][0].ToString();
                    decimal allowquant = Convert.ToDecimal(d12.Rows[a][1]);
                    decimal stock = 0;
                    String get = "select Quantity from tblStockMaintain where ProductName='" + Receprod + "'";
                    DataTable d1 = clsDataLayer.RetreiveQuery(get);
                    if (d1.Rows.Count > 0) { stock = Convert.ToDecimal(d1.Rows[0][0].ToString()); }
                    decimal final = 0; decimal totalquant = 0;

                    totalquant = allowquant * InputQuant;
                    final = stock + totalquant;
                    String upd = "update tblStockMaintain set Quantity=" + final + " where ProductName='" + Receprod + "'"; clsDataLayer.ExecuteQuery(upd);
                }
            }
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 6)
                {
                    individualstock();

                    String dels = "delete from tbl_SaleByTableQ where SaleCode='" + txtcode.Text + "' and Product_Name='" + grid.CurrentRow.Cells[0].Value + "'"; clsDataLayer.ExecuteQuery(dels);
                    loaddata(); total(); Update();

                }

            }
            catch { }
        }

        private void grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (grid.CurrentCell.ColumnIndex == 1)
                {
                    decimal a = Convert.ToDecimal(grid.CurrentRow.Cells[1].Value); decimal b = Convert.ToDecimal(grid.CurrentRow.Cells[2].Value);
                    decimal d = Convert.ToDecimal(grid.CurrentRow.Cells[4].Value);
                    decimal c = a * b; grid.CurrentRow.Cells[3].Value = c; total(); c = c - d; grid.CurrentRow.Cells[5].Value = c; total();
                }
                else if (grid.CurrentCell.ColumnIndex == 4)
                {
                    decimal a = Convert.ToDecimal(grid.CurrentRow.Cells[1].Value); decimal b = Convert.ToDecimal(grid.CurrentRow.Cells[2].Value);
                    decimal d = Convert.ToDecimal(grid.CurrentRow.Cells[4].Value);
                    decimal c = a * b; grid.CurrentRow.Cells[3].Value = c; total(); c = c - d; grid.CurrentRow.Cells[5].Value = c; total();
                }
            }
            catch
            { }
        }

        private void total()
        {
            decimal total = 0; decimal discount = 0;
            for (int b = 0; b < grid.Rows.Count; b++)
            {
                total += Convert.ToDecimal(grid.Rows[b].Cells[3].Value); discount += Convert.ToDecimal(grid.Rows[b].Cells[4].Value);
            } txtbill.Text = total.ToString();
            decimal mm = 0;
            //    String h = "select TotalDiscount from SaleView where SaleCode='" + txtcode.Text + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(h);
            //if (d1.Rows.Count > 0)
            //{
            //    mm = Convert.ToDecimal(d1.Rows[0][0].ToString());
            //}
            discount += mm; txtdis.Text = discount.ToString();
        }

        private void txtdis_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal a = Convert.ToDecimal(txtbill.Text); decimal b = Convert.ToDecimal(txtdis.Text); decimal c = a - b; txtnet.Text = c.ToString();
                
            }
            catch { }
        }


        private void StockManage()
        {
            try
            {
                for (int b = 0; b < grid.Rows.Count; b++)
                {
                    String pdname = grid.Rows[b].Cells[0].Value.ToString();
                    decimal InputQuant = Convert.ToDecimal(grid.Rows[b].Cells[1].Value.ToString());
                    #region all

                    String get1 = "select ReceipeProductName,Quantity from vwconsumption where SaleProductName ='" + pdname + "'";
                    DataTable d12 = clsDataLayer.RetreiveQuery(get1);
                    if (d12.Rows.Count > 0)
                    {
                        decimal oldtq = 0; decimal multiplyquant = 0; String condstatus = "";
                        String get5 = "select Quantity from tbl_SaleByTableQ where Product_Name='" + pdname + "' and SaleCode='" + txtcode.Text + "'";
                        DataTable d15 = clsDataLayer.RetreiveQuery(get5);
                        if (d15.Rows.Count > 0) { oldtq = Convert.ToDecimal(d15.Rows[0][0].ToString()); }

                        #region cond
                        if (oldtq == InputQuant)
                        {
                            multiplyquant = 0; condstatus = "none";
                        }
                        else if (oldtq < InputQuant)
                        {
                            multiplyquant = InputQuant - oldtq; condstatus = "minus";
                        }
                        else if (oldtq > InputQuant)
                        {
                            multiplyquant = oldtq - InputQuant; condstatus = "plus";
                        }
                        #endregion cond
                        if (!condstatus.Equals("none"))
                        {
                            for (int a = 0; a < d12.Rows.Count; a++)
                            {
                                String Receprod = d12.Rows[a][0].ToString();
                                decimal allowquant = Convert.ToDecimal(d12.Rows[a][1]);
                                decimal stock = 0;
                                String get = "select Quantity from tblStockMaintain where ProductName='" + Receprod + "'";
                                DataTable d1 = clsDataLayer.RetreiveQuery(get);
                                if (d1.Rows.Count > 0) { stock = Convert.ToDecimal(d1.Rows[0][0].ToString()); }
                                decimal final = 0; decimal totalquant = 0;
                                if (condstatus.Equals("plus"))
                                {
                                    totalquant = allowquant * multiplyquant;
                                    final = stock + totalquant;
                                }
                                else if (condstatus.Equals("minus"))
                                {
                                    totalquant = allowquant * multiplyquant;
                                    final = stock - totalquant;
                                }

                                String upd = "update tblStockMaintain set Quantity=" + final + " where ProductName='" + Receprod + "'"; clsDataLayer.ExecuteQuery(upd);
                            }
                        }

                    }
                    #endregion all
                }
            }
            catch { }

        }

        public void Update()
        {
            try
            {
                string q = "select ";
                string cat_find = "";

                if (grid.Rows.Count > 0)
                {
                    StockManage();
                    String dg = "delete from tbl_SaleByTableQ where SaleCode = '" + txtcode.Text + "'"; clsDataLayer.ExecuteQuery(dg);

                    for (int b = 0; b < grid.Rows.Count; b++)
                    {

                        String query = "select Product_Categories from tbl_ProductBySale where Product_Name = '" + grid.Rows[b].Cells[0].Value.ToString() + "'"; DataTable da = clsDataLayer.RetreiveQuery(query);
                        if (da.Rows.Count > 0)
                        {
                            cat_find = da.Rows[0][0].ToString();
                        }
                        else
                        {
                            String query2 = "select Product_Categories from tbl_deals where Product_Name = '" + grid.Rows[b].Cells[0].Value.ToString() + "'"; DataTable da1 = clsDataLayer.RetreiveQuery(query2);
                            if (da1.Rows.Count > 0)
                            {
                                cat_find = da1.Rows[0][0].ToString();
                            }
                        }

                        String ibs = "insert into tbl_SaleByTableQ(FloorNo,TableNo,Waiter,UserName,SaleCode,TokenCode,Product_Name,Quantity,Sell_Price,CGross_Amount,Discount,Total_Amount,descc,TableStatus,[Date],[PrintStatus],[c_name],[c_phone],[c_address],[Product_Categories])values('" + txtfloor.Text + "','" + txttable.Text + "','" + txtwaiter.Text + "','" + Login.UserID + "','" + txtcode.Text + "','" + txttoken.Text + "','" + grid.Rows[b].Cells[0].Value.ToString() + "'," + grid.Rows[b].Cells[1].Value.ToString() + "," + grid.Rows[b].Cells[2].Value.ToString() + "," + grid.Rows[b].Cells[3].Value.ToString() + "," + grid.Rows[b].Cells[4].Value.ToString() + "," + grid.Rows[b].Cells[5].Value.ToString() + ",'','Active','" + dateTimePicker1.Text + "','','','','','" + cat_find + "')";
                        clsDataLayer.ExecuteQuery(ibs);
                    }


                    String del3 = "delete from tbl_Sale where DSaleCode='" + txtcode.Text + "'"; clsDataLayer.ExecuteQuery(del3);
                    String ins3 = "insert into tbl_Sale(Date,DSaleCode,Gross_Amount,TotalDiscount,Net_Amount)values('" + dateTimePicker1.Text + "','" + txtcode.Text + "'," + txtbill.Text + "," + txtdis.Text + "," + txtnet.Text + ")"; clsDataLayer.ExecuteQuery(ins3);
                    //
                    MessageBox.Show("Record Updated! ", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information); loaddata();
                }
                else
                {
                    String del3 = "delete from tbl_Sale where DSaleCode='" + txtcode.Text + "'"; clsDataLayer.ExecuteQuery(del3);
                }
            }
            catch { }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            String ht = "select * from tbl_table where Table_Name='" + txttable.Text + "'";
            DataTable ds = clsDataLayer.RetreiveQuery(ht);
            if (ds.Rows.Count > 0)
            {
                Update();
            }
            else
            {
                MessageBox.Show("Table Not Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }



        private void btnPrint_Click(object sender, EventArgs e)
        {
            String query = "select * from SaleView where SaleCode='" + txtcode.Text + "'"; DataTable da = clsDataLayer.RetreiveQuery(query);
            if (da.Rows.Count > 0)
            {
                pp();
            }
            else { MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void pp()
        {
            try
            {
                string[] array = new string[10];
                string[] array1 = new string[10];
                string[] q = new string[10];
                string[] q1 = new string[10];

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


                String query9 = "select  distinct Product_Categories from my_test where SaleCode='" + txtcode.Text + "'"; DataTable da9 = clsDataLayer.RetreiveQuery(query9);
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
                string token_code =""; 
                string pr_name = "";
                decimal qtyy = 0;
                decimal sell_price= 0;
                decimal discount = 0 ;
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
                decimal net_amount =0;
                string datee = "";


                for (int n = 0; n < q1.Length; n++)
                {

                    if (q1[n] != null)
                    {
                        qchk = q1[n].Trim();
                        String query = "select * from SaleView where SaleCode='" + txtcode.Text + "' and Product_Categories like '%" + q1[n] + "'";
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
                    receiptByTable5 rt = new receiptByTable5(); rt.SetDataSource(dv);
                    view_close2 aop = new view_close2(rt, 0, 0, 0, service_c);
                    aop.Show();
                    rt.PrintToPrinter(1, false, 0, 0);

                }
            }
            catch
            {


            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (txt_dis_p.Text.Length > 0)
            {
                total();
                decimal d_per = Convert.ToDecimal(txt_dis_p.Text);
                decimal d_price = Convert.ToDecimal(txtdis.Text);

                decimal bill = Convert.ToDecimal(txtbill.Text);

                decimal totall = (bill * d_per) / 100;

                txtdis.Text = Convert.ToString(d_price + totall);
            }
            else
            {
                total();
            }
        }

        private void txt_dis_p_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
