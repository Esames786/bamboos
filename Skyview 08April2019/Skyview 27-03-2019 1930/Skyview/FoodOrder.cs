using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Skyview.bin.Debug.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Skyview
{
    public partial class FoodOrder : Form
    {
        String CName = ""; String Categories5 = "";
        public FoodOrder()
        {
            InitializeComponent(); txtfloor.Text = "IN"; ; createbutton(); createtablebutton(); createFloor(); fetch_date(); button2.BackColor = Color.Green; load_waiters();
        }

        public void load_waiters()
        {
            try
            {
                flowwaiter.Controls.Clear();
                String get = "select waiter_name from waiters Order By id asc";
                //  String GetDetail = "select Product_Name from tbl_ProductBySale";
                DataTable ds = clsDataLayer.RetreiveQuery(get);
                if (ds.Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        Button b8 = new Button();
                        b8.Name = i.ToString();
                        b8.Text = ds.Rows[i][0].ToString();
                        //  b.AutoSize = true;
                        b8.Width = 85; b8.Height = 28;
                        b8.Font = new Font(b8.Font.FontFamily, 9);
                        b8.BackColor = Color.FromArgb(192, 255, 192);
                        b8.Click += b8_Click; b8.Focus();
                        flowwaiter.Controls.Add(b8);
                    }
                }
            }
            catch { }
        }

        void b8_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button; txtwaiter.Text = btn.Text;
        }

        public void createtoken()
        {

            txttoken.Text = "DINE IN";

        }

        public void CodeGenerated()
        {
            grid.Rows.Clear();
            String get = "select SaleCode,Product_Name,Quantity,Sell_Price,Total_Amount,FloorNo,TableNo,Waiter from tbl_SaleByTableQ  where TableNo='" + txttable.Text + "' and TableStatus='Active' and FloorNo='" + txtfloor.Text + "' "; DataTable ds = clsDataLayer.RetreiveQuery(get);
            if (ds.Rows.Count > 0)
            {
                String Code = ds.Rows[0][0].ToString(); txtcode.Text = Code; createbutton();
                txtwaiter.Text = ds.Rows[0][7].ToString();
            }
            else
            {
                txtcode.Text = clsGeneral.getMAXCode("tbl_SaleByTableQ", "SaleCode", "SC");
            }
        }

        public void createFloor()
        {
            try
            {
                flowLayoutPanel3.Controls.Clear();
                int ab = 0; String data = "";
                for (int i = 0; i < 1; i++)
                {
                    ab++;
                    if (ab == 1)
                    {
                        data = "IN";
                    }
                   
                    Button b4 = new Button();
                    if (txtfloor.Text == "IN" && data == "IN")
                    {
                        b4.BackColor = Color.Green;
                    }
                    else
                    {
                        b4.BackColor = Color.FromArgb(255, 192, 192);
                    }
                    b4.Name = i.ToString();
                    b4.Text = data;
                    b4.Font = new Font(b4.Font.FontFamily, 12);
                    //  b.AutoSize = true;
                    b4.Width = 186; b4.Height = 56;
                    b4.Click += b4_Click; b4.Focus();
                    flowLayoutPanel3.Controls.Add(b4);
                }
            }
            catch { }
        }

        void b4_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button; txtfloor.Text = btn.Text; createtablebutton(); createFloor(); CodeGenerated(); txttable.Clear();
        }
        
        public void createtablebutton()
        {
            try
            {
                flowLayoutPanel2.Controls.Clear();
                String get = "select Table_Name from tbl_table Order By TCode asc";
                //  String GetDetail = "select Product_Name from tbl_ProductBySale";
                DataTable ds = clsDataLayer.RetreiveQuery(get);
                if (ds.Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        Button b3 = new Button();
                        b3.Name = i.ToString();
                        b3.Text = ds.Rows[i][0].ToString();
                        //  b.AutoSize = true;
                        b3.Width = 55; b3.Height = 50;
                        b3.Font = new Font(b3.Font.FontFamily, 8, FontStyle.Bold);
                        String h1 = "select TableStatus from tbl_SaleByTableQ where FloorNo='" + txtfloor.Text + "' and TableNo='" + b3.Text + "' order by S_Id desc";
                        DataTable ds2 = clsDataLayer.RetreiveQuery(h1);
                        if (ds2.Rows.Count > 0)
                        {
                            String hm = ds2.Rows[0][0].ToString();
                            if (hm.Equals("DeActive"))
                            {
                                b3.BackColor = Color.FromArgb(255, 192, 192);
                            }
                            else
                            {
                                b3.BackColor = Color.Green;
                            }
                        }
                        else
                        {
                            b3.BackColor = Color.FromArgb(255, 192, 192);
                        }
                        b3.Click += b3_Click; b3.Focus();
                        flowLayoutPanel2.Controls.Add(b3);
                    }
                }
            }
            catch { }
        }

        void b3_Click(object sender, EventArgs e)
        {
            if (!txtfloor.Text.Equals(""))
            {
                Button btn = sender as Button; txttable.Text = btn.Text; 
                CodeGenerated(); createtoken(); //createtablebutton();
            }
            else
            {
                MessageBox.Show("Select Floor!", "Floor Select", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void createbutton()
        {
            try
            {
                flowLayoutPanel1.Controls.Clear();
                String get = "select Categories_Name from product_categories";
                //  String GetDetail = "select Product_Name from tbl_ProductBySale";
                DataTable ds = clsDataLayer.RetreiveQuery(get);
                if (ds.Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        Button b = new Button();
                        b.Name = i.ToString();
                        b.Text = ds.Rows[i][0].ToString();
                        //  b.AutoSize = true;
                        b.Width = 100; b.Height = 90;
                        b.Font = new Font(b.Font.FontFamily, 8, FontStyle.Bold);
                        b.BackColor = Color.Aqua;
                        b.Click += b_Click; b.Focus();
                        flowLayoutPanel1.Controls.Add(b);
                    }
                }
            }
            catch { }
        }

        private void createFoodbutton(String Name)
        {

            string checkk = "";
            String get1 = "select Categories_Name from product_categories where Categories_Name = '" + Name + "'";
            DataTable df1 = clsDataLayer.RetreiveQuery(get1);
            if (df1.Rows.Count > 0)
            {
                checkk = df1.Rows[0][0].ToString();
            }
            if (checkk != "DEALS")
            {

                flowLayoutPanel1.Controls.Clear();
                String GetDetail = "select Product_Name from tbl_ProductBySale where Product_Categories='" + Name + "'";
                DataTable ds = clsDataLayer.RetreiveQuery(GetDetail);
                if (ds.Rows.Count > 0)
                {
                    Categories5 = Name;
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        Button b2 = new Button();
                        b2.Name = i.ToString();
                        b2.Text = ds.Rows[i][0].ToString();
                        //  b2.AutoSize = true;
                        b2.Width = 80; b2.Height = 80;
                        b2.Font = new Font(b2.Font.FontFamily, 8, FontStyle.Bold);
                        b2.BackColor = Color.Aqua;
                        b2.Click += b2_Click; b2.KeyDown += b2_KeyDown; b2.Focus();
                        flowLayoutPanel1.Controls.Add(b2);
                    }
                }
                else
                {
                    MessageBox.Show("No Product Found in this Category!");
                    createbutton();
                }
            }
            else
            {
                flowLayoutPanel1.Controls.Clear();
                String GetDetail = "select distinct(d_name),PICode from [tbl_deals] order by PICode";
                DataTable ds = clsDataLayer.RetreiveQuery(GetDetail);
                if (ds.Rows.Count > 0)
                {
                    Categories5 = Name;
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        Button b2 = new Button();
                        b2.Name = i.ToString();
                        b2.Text = ds.Rows[i][0].ToString();
                        //  b2.AutoSize = true;
                        b2.Width = 80; b2.Height = 80;
                        b2.Font = new Font(b2.Font.FontFamily, 8, FontStyle.Bold);
                        b2.BackColor = Color.Aqua;
                        b2.Click += b2_Click; b2.KeyDown += b2_KeyDown; b2.Focus();
                        flowLayoutPanel1.Controls.Add(b2);
                    }
                }
                else
                {
                    MessageBox.Show("No Product Found in this Category!");
                    createbutton();
                }
            }
        }

        void b2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                { this.Close(); }
            }
            catch { }
        }
        bool ck = false;
        private void foodcheck(String name)
        {
            ck = false;
            for (int a = 0; a < grid.Rows.Count; a++)
            {
                String pname = grid.Rows[a].Cells[0].Value.ToString();
                if (pname.Equals(name))
                {
                    ck = true;
                }
            }
        }
        private void total()
        {
            decimal total = 0; decimal discount = 0;
            for (int b = 0; b < grid.Rows.Count; b++)
            {
                total += Convert.ToDecimal(grid.Rows[b].Cells[3].Value); discount += Convert.ToDecimal(grid.Rows[b].Cells[4].Value);
            } txtbill.Text = total.ToString(); txtdiscount.Text = discount.ToString();

            
        }

        void b2_Click(object sender, EventArgs e)
        {
            Button bs = sender as Button;
            //  foodcheck(bs.Text);
            //if()
            if (ck == false)
            {
                if (txtcode.Text.Equals(""))
                {
                    MessageBox.Show("Select Table First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    String GetDetail5 = "select Product_Name,Sell_Price from tbl_deals where d_name='" + bs.Text + "'";
                    DataTable ds5 = clsDataLayer.RetreiveQuery(GetDetail5);
                    if (ds5.Rows.Count > 0)
                    {
                        String GetDetail = "select Product_Name,Sell_Price from tbl_deals where d_name='" + bs.Text + "'";
                        DataTable ds = clsDataLayer.RetreiveQuery(GetDetail);
                        for (int i = 0; i < ds.Rows.Count; i++)
                        {
                            string pnn = ds.Rows[i][0].ToString();
                            decimal sell = Convert.ToDecimal(ds.Rows[i][1].ToString()); decimal qnt = 1;
                            decimal ff = sell * qnt;
                            GridInsert(pnn, qnt.ToString(), sell.ToString(), ff.ToString());
                            total();
                            //  
                        }
                    }
                    else
                    {
                        String GetDetail = "select Product_Name,Sell_Price from tbl_ProductBySale where Product_Name='" + bs.Text + "'";
                        DataTable ds = clsDataLayer.RetreiveQuery(GetDetail);
                        decimal sell = Convert.ToDecimal(ds.Rows[0][1].ToString()); decimal qnt = 1;
                        decimal ff = sell * qnt;
                        GridInsert(bs.Text, qnt.ToString(), sell.ToString(), ff.ToString());
                        total();
                        //  
                    }
                    //
                }
            }
            else { MessageBox.Show("Already Add!"); }
        }
        void b_Click(object sender, EventArgs e)
        {
            Button bs = sender as Button; CName = bs.Text; createtoken(); createFoodbutton(CName);
        }
        private void btn_TblClosed_Click(object sender, EventArgs e)
        {
            try
            {
                String query = "select * from tbl_SaleByTableQ where SaleCode='" + txtcode.Text + "'";
                DataTable fv = clsDataLayer.RetreiveQuery(query);
                if (fv.Rows.Count > 0)
                {
                    PrintReceipt pr = new PrintReceipt(txttable.Text, txtfloor.Text, this, "Food"); pr.txtsale.Text = txtcode.Text; pr.Show(); txtcode.Clear();
                }
                else
                {
                    MessageBox.Show("Kindly add Data in Sale Voucher!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                txtcode.Clear();
            }
            catch { }
        }

        public void GetTotal()
        {
            decimal gs = 0; decimal dis = 0; decimal net = 0;
            decimal igs = Convert.ToDecimal(txtbill.Text); decimal idis = Convert.ToDecimal(txtdiscount.Text); decimal inet = Convert.ToDecimal(txtfinal.Text);
            String sel = "select Gross_Amount,TotalDiscount,Net_Amount from tbl_Sale where DSaleCode='" + txtcode.Text + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(sel); if (d1.Rows.Count > 0)
            {
                gs = Convert.ToDecimal(d1.Rows[0][0].ToString()); dis = Convert.ToDecimal(d1.Rows[0][1].ToString()); net = Convert.ToDecimal(d1.Rows[0][2].ToString());
                gs += igs; dis += idis; decimal final = gs - dis;
                String del = "delete from tbl_Sale where DSaleCode='" + txtcode.Text + "'"; clsDataLayer.ExecuteQuery(del);
                String ins = "insert into tbl_Sale(Date,DSaleCode,Gross_Amount,TotalDiscount,Net_Amount)values('" + dateTimePicker1.Text + "','" + txtcode.Text + "'," + gs + "," + dis + "," + final + ")"; clsDataLayer.ExecuteQuery(ins);
            }
            else
            {
                String ins = "insert into tbl_Sale(Date,DSaleCode,Gross_Amount,TotalDiscount,Net_Amount)values('" + dateTimePicker1.Text + "','" + txtcode.Text + "'," + igs + "," + idis + "," + inet + ")"; clsDataLayer.ExecuteQuery(ins);
            }
        }


        private void InsertOneTime()
        {
            string cat_find = "";
            String sel = "select * from tbl_SaleByTableQ where SaleCode='" + txtcode.Text + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(sel);
            if (d1.Rows.Count < 1)
            {
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
            }
            else
            {
                for (int b = 0; b < grid.Rows.Count; b++)
                {
                    String pdname = grid.Rows[b].Cells[0].Value.ToString();
                    String sel2 = "select Quantity,CGross_Amount,Discount,Total_Amount from tbl_SaleByTableQ where SaleCode='" + txtcode.Text + "' and Product_Name='" + pdname + "'"; DataTable d2 = clsDataLayer.RetreiveQuery(sel2);
                    if (d2.Rows.Count < 1)
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
                    else
                    {
                        decimal qs = Convert.ToDecimal(d2.Rows[0][0].ToString()); decimal gs = Convert.ToDecimal(d2.Rows[0][1].ToString());
                        decimal dis = Convert.ToDecimal(d2.Rows[0][2].ToString()); decimal tot = Convert.ToDecimal(d2.Rows[0][3].ToString());
                        decimal gqs = Convert.ToDecimal(grid.Rows[b].Cells[1].Value); decimal ggs = Convert.ToDecimal(grid.Rows[b].Cells[3].Value);
                        decimal gdis = Convert.ToDecimal(grid.Rows[b].Cells[4].Value); decimal gtot = Convert.ToDecimal(grid.Rows[b].Cells[5].Value);
                        decimal fqs = qs + gqs; decimal fgs = gs + ggs; decimal fdis = dis + gdis; decimal ftot = tot + gtot;
                        String del = "delete from tbl_SaleByTableQ where SaleCode='" + txtcode.Text + "' and Product_Name='" + pdname + "'"; clsDataLayer.ExecuteQuery(del);

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

                        String ibs = "insert into tbl_SaleByTableQ(FloorNo,TableNo,Waiter,UserName,SaleCode,TokenCode,Product_Name,Quantity,Sell_Price,CGross_Amount,Discount,Total_Amount,descc,TableStatus,[Date],[PrintStatus],[c_name],[c_phone],[c_address],[Product_Categories])values('" + txtfloor.Text + "','" + txttable.Text + "','" + txtwaiter.Text + "','" + Login.UserID + "','" + txtcode.Text + "','" + txttoken.Text + "','" + grid.Rows[b].Cells[0].Value.ToString() + "'," + fqs + "," + grid.Rows[b].Cells[2].Value.ToString() + "," + fgs + "," + fdis + "," + ftot + ",'','Active','" + dateTimePicker1.Text + "','','','','','" + cat_find + "')";
                        clsDataLayer.ExecuteQuery(ibs);
                    }
                }
            }
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
                        String get5 = "select Quantity from tbl_SaleByTableQ where Product_Name='" + pdname + "' and SaleCode='" + txtcode.Text + "' and TokenCode='" + txttoken.Text + "'  ";
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
                                else
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


        private void SaveClick()
        {
            try
            {
                if (txtcode.Text.Equals(""))
                {
                    MessageBox.Show("Select Table First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //String del = "delete from tbl_SaleByTable where SaleCode='" + txtcode.Text + "' and TokenCode='" + txttoken.Text + "'"; clsDataLayer.ExecuteQuery(del);


                    GetTotal(); StockManage(); InsertOneTime();
                    createbutton(); printing(); grid.Rows.Clear();
                    //
                    //        PrintReceipt();
                    //   String query = "select * from SaleView where SaleCode='" + txtcode.Text + "' and TokenCode='"+txttoken.Text+"'"; DataTable da = clsDataLayer.RetreiveQuery(query);
                    //   if (da.Rows.Count > 0)
                    //   {
                    //  // receiptByTable5 rt = new receiptByTable5(); rt.SetDataSource(da);
                    //  // rt.PrintOptions.PaperOrientation = PaperOrientation.Portrait; rt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4; rt.PrintToPrinter(0, false, 1, 5); 
                    ////   //AllinOne aop = new AllinOne(rt); aop.Show();
                    //   }
                    //   else { MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    //String query = "select * from SaleView where SaleCode='" + txtcode.Text + "'"; DataTable da = clsDataLayer.RetreiveQuery(query);
                    //if (da.Rows.Count > 0)
                    //{
                    //    receiptByTable rt = new receiptByTable(); rt.SetDataSource(da); AllinOne aop = new AllinOne(rt); aop.Show();
                    //}
                    //else { MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
            catch { }
        }
        public void PrintReceipt()
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument printDocument = new PrintDocument();
            printDialog.Document = printDocument;
            printDocument.PrintPage += printDocument_PrintPage;
            DialogResult result = printDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                printDocument.Print();
            }
        }
        void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
        }


        public void printing()
        {
            try
            {
                String trun = "truncate table tbl_Token2"; clsDataLayer.ExecuteQuery(trun);

                Token rt = new Token();
                for (int i = 0; i < grid.Rows.Count; i++)
                {
                    bool m = Convert.ToBoolean(grid.Rows[i].Cells[8].Value.ToString());
                    if (m == true)
                    {

                        String query = "select Product_Categories from tbl_ProductBySale where Product_Name = '" + grid.Rows[i].Cells[0].Value + "'"; DataTable da = clsDataLayer.RetreiveQuery(query);
                        if (da.Rows.Count > 0)
                        {
                            string cat = da.Rows[0][0].ToString();
                            String ins1 = "insert into tbl_Token2(CGross_Amount,Discount,TokenCode,SaleCode,Product_Name,Quantity,Sell_Price,Total_Amount,FloorNo,TableNo,Waiter,TableStatus,UserName,Date,categories)values(" + grid.Rows[i].Cells[3].Value.ToString() + "," + grid.Rows[i].Cells[4].Value.ToString() + ",'" + txttoken.Text + "','" + txtcode.Text + "','" + grid.Rows[i].Cells[0].Value.ToString() + "','" + grid.Rows[i].Cells[1].Value.ToString() + "','" + grid.Rows[i].Cells[2].Value.ToString() + "'," + grid.Rows[i].Cells[5].Value.ToString() + ",'" + txtfloor.Text + "','" + txttable.Text + "','" + txtwaiter.Text + "','Active','" + Login.UserID + "','" + dateTimePicker1.Text + "','" + cat + "')"; clsDataLayer.ExecuteQuery(ins1);

                        }
                        else
                        {
                            String query2 = "select Product_Categories from tbl_deals where Product_Name = '" + grid.Rows[i].Cells[0].Value + "'"; DataTable da1 = clsDataLayer.RetreiveQuery(query2);
                            if (da1.Rows.Count > 0)
                            {
                                string cat = da1.Rows[0][0].ToString();
                                String ins1 = "insert into tbl_Token2(CGross_Amount,Discount,TokenCode,SaleCode,Product_Name,Quantity,Sell_Price,Total_Amount,FloorNo,TableNo,Waiter,TableStatus,UserName,Date,categories)values(" + grid.Rows[i].Cells[3].Value.ToString() + "," + grid.Rows[i].Cells[4].Value.ToString() + ",'" + txttoken.Text + "','" + txtcode.Text + "','" + grid.Rows[i].Cells[0].Value.ToString() + "','" + grid.Rows[i].Cells[1].Value.ToString() + "','" + grid.Rows[i].Cells[2].Value.ToString() + "'," + grid.Rows[i].Cells[5].Value.ToString() + ",'" + txtfloor.Text + "','" + txttable.Text + "','" + txtwaiter.Text + "','Active','" + Login.UserID + "','" + dateTimePicker1.Text + "','" + cat + "')"; clsDataLayer.ExecuteQuery(ins1);

                            }
                        }


                    }
                }



                String query8 = "select distinct(categories) from tbl_Token2"; DataTable da8 = clsDataLayer.RetreiveQuery(query8);
                if (da8.Rows.Count > 0)
                {
                    string[] array = new string[da8.Rows.Count];
                    for (int k = 0; k < da8.Rows.Count; k++)
                    {
                        array[k] = da8.Rows[k][0].ToString();
                    }
                    for (int l = 0; l < array.Length; l++)
                    {
                        string my = array[l].ToString();
                        string strTrimmed = my.Trim();
                        string finalString = strTrimmed.Substring(strTrimmed.LastIndexOf(" ", strTrimmed.Length));
                        array[l] = finalString;
                    }
                    string[] q = array.Distinct().ToArray();
                    for (int n = 0; n < q.Length; n++)
                    {
                        String query = "select * from tbl_Token2 where categories like '%" + q[n] + "%' "; DataTable da = clsDataLayer.RetreiveQuery(query);
                        if (da.Rows.Count > 0)
                        {
                            rt.SetDataSource(da);
                            rt.PrintToPrinter(1, false, 0, 0);
                            //token_view aop = new token_view(rt); aop.Show();
                        }
                        else { MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }

                }
                trun = "truncate table tbl_Token2"; clsDataLayer.ExecuteQuery(trun);


            }
            catch 
            {
                
                
            }
            

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid.Rows.Count < 1)
                {
                    MessageBox.Show("Kindly Add Product First!");
                }
                else
                {
                    btnsave.Text = "Wait.... Token is Priting..";
                    SaveClick(); createtablebutton(); btnsave.Text = "Save Items";
                }
            }
            catch { }
        }
        private void GridInsert(String Pname, String Quantity, String Sell, String Total)
        {
            try
            {
                bool abc = false;
                decimal quant = 0; decimal sell = 0; decimal dis = 0;
                for (int b = 0; b < grid.Rows.Count; b++)
                {
                    String Prod = grid.Rows[b].Cells[0].Value.ToString();
                    if (Prod.Equals(Pname))
                    {
                        abc = true; dis = Convert.ToDecimal(grid.Rows[b].Cells[4].Value.ToString());
                        quant = Convert.ToDecimal(grid.Rows[b].Cells[1].Value.ToString()); sell = Convert.ToDecimal(grid.Rows[b].Cells[2].Value.ToString());
                        quant++; decimal final = quant * sell; grid.Rows[b].Cells[1].Value = quant.ToString();
                        final = final - dis;
                        grid.Rows[b].Cells[3].Value = final.ToString(); grid.Rows[b].Cells[5].Value = final.ToString();

                        String sel = "select Product_Categories from tbl_ProductBySale where Product_Name='" + Pname + "'"; DataTable ds = clsDataLayer.RetreiveQuery(sel);
                        if (ds.Rows.Count > 0)
                        {
                            string bv = ds.Rows[0][0].ToString();
                            if ((bv == "SERVICE CHARGES TB") || (bv == "SALAD/SAUCE T8") || (bv == "BEVERAGES T8")) 
                            {
                                grid.Rows[b].Cells[8].Value = false;
                            }
                            else
                            {
                                grid.Rows[b].Cells[8].Value = true;
                            }
                        }
                    }
                }
                if (abc == false)
                {
                    int n = grid.Rows.Add(); grid.Rows[n].Cells[0].Value = Pname; grid.Rows[n].Cells[1].Value = Quantity; grid.Rows[n].Cells[2].Value = Sell; grid.Rows[n].Cells[4].Value = "0"; grid.Rows[n].Cells[3].Value = Total.ToString(); grid.Rows[n].Cells[5].Value = Total.ToString();

                    String sel = "select Product_Categories from tbl_ProductBySale where Product_Name='" + Pname + "'"; DataTable ds = clsDataLayer.RetreiveQuery(sel);
                    if (ds.Rows.Count > 0)
                    {
                        string bv = ds.Rows[0][0].ToString();

                        if ((bv == "SERVICE CHARGES TB") || (bv == "SALAD/SAUCE T8") || (bv == "BEVERAGES T8")) 
                        {
                            grid.Rows[n].Cells[8].Value = false;
                        }
                        else
                        {
                            grid.Rows[n].Cells[8].Value = true;
                        }
                    }
                    else
                    {
                        String sel1 = "select Product_Categories from tbl_deals where Product_Name='" + Pname + "'"; DataTable ds1 = clsDataLayer.RetreiveQuery(sel1);
                        if (ds1.Rows.Count > 0)
                        {
                            string bv = ds1.Rows[0][0].ToString();

                            if ((bv == "SERVICE CHARGES TB") || (bv == "SALAD/SAUCE T8") || (bv == "BEVERAGES T8")) 
                            {
                                grid.Rows[n].Cells[8].Value = false;
                            }
                            else
                            {
                                grid.Rows[n].Cells[8].Value = true;
                            }
                        }
                    }

                }
            }
            catch { }
        }
        private void grid_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (grid.CurrentCell.ColumnIndex == 1)
                {
                    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    {
                        e.Handled = true;
                    }
                }
            }
            catch { }
        }
        private void grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if (grid.CurrentCell.ColumnIndex == 1) //Desired Column
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
            }
        }
        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) { e.Handled = true; }
        }
        private void btnBack_Click(object sender, EventArgs e) { createbutton(); }
        private void txtquant_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) { e.Handled = true; }
        }
        private void FoodOrder_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
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
            catch { }
        }
        private void grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (grid.Rows.Count > 0)
                {
                    int yh = grid.CurrentCell.RowIndex;
                    grid.Rows.RemoveAt(yh);
                } total();
            }
        }
        private void btnBack_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                { this.Close(); }
            }
            catch { }
        }
        private void grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            grid.Rows[e.RowIndex].Cells[6].Value = "Delete";
            grid.Rows[e.RowIndex].Cells[7].Value = "Print";
        }
        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 6)
                {
                    if (grid.Rows.Count > 0)
                    {
                        grid.Rows.RemoveAt(e.RowIndex); total();
                    }
                }
                else if (e.ColumnIndex == 7)
                {
                    ////
                    //String del = "delete from tbl_Token where SaleCode='" + txtcode.Text + "' and TokenCode='" + txttoken.Text + "' and Product_Name='" + grid.CurrentRow.Cells[0].Value.ToString() + "'"; clsDataLayer.ExecuteQuery(del);
                    //String ins = "insert into tbl_Token(CGross_Amount,Discount,TokenCode,SaleCode,Product_Name,Quantity,Sell_Price,Total_Amount,FloorNo,TableNo,Waiter,TableStatus,UserName,Date)values(" + grid.CurrentRow.Cells[3].Value.ToString() + "," + grid.CurrentRow.Cells[4].Value.ToString() + ",'" + txttoken.Text + "','" + txtcode.Text + "','" + grid.CurrentRow.Cells[0].Value.ToString() + "','" + grid.CurrentRow.Cells[1].Value.ToString() + "','" + grid.CurrentRow.Cells[2].Value.ToString() + "'," + grid.CurrentRow.Cells[5].Value.ToString() + ",'" + txtfloor.Text + "','" + txttable.Text + "','" + txtwaiter.Text + "','Active','" + Login.UserID + "','" + dateTimePicker1.Text + "')"; clsDataLayer.ExecuteQuery(ins);
                    ////
                    //String upd = "update tbl_Token set PrintStatus='Print' where Product_Name='" + grid.Rows[e.RowIndex].Cells[0].Value + "' and SaleCode='" + txtcode.Text + "' and  TokenCode='" + txttoken.Text + "'"; clsDataLayer.ExecuteQuery(upd);
                    //String query = "select * from tbl_Token where Product_Name='" + grid.Rows[e.RowIndex].Cells[0].Value + "' and SaleCode='" + txtcode.Text + "' and  TokenCode='" + txttoken.Text + "'"; DataTable da = clsDataLayer.RetreiveQuery(query);
                    //if (da.Rows.Count > 0)
                    //{
                    //    Token rt = new Token();
                    //    rt.SetDataSource(da);
                    //    rt.PrintToPrinter(1, false, 0, 0);
                    //    token_view aop = new token_view(rt); aop.Show();
                    //    //PrintDialog pd = new PrintDialog(); PrintDocument pdo = new PrintDocument(); pdo.DocumentName = "Print Document"; pd.Document = pdo;
                    //    //pd.AllowSelection = true; pd.AllowSomePages = true; if (pd.ShowDialog() == DialogResult.OK) pdo.Print();
                    //}
                    //else { MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
            catch { }
        }

        private void txtdiscount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal a = Convert.ToDecimal(txtbill.Text); decimal b = Convert.ToDecimal(txtdiscount.Text); decimal c = a - b; txtfinal.Text = c.ToString();
                
            }
            catch { }
        }
        private void btnOrders_Click(object sender, EventArgs e)
        {
            try
            {
                Orders od = new Orders(); od.txtcode.Text = txtcode.Text; od.Show(); //od.Update(); 
            }
            catch
            { }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            RemainingPayment rp = new RemainingPayment(); rp.Show();
        }
        private void txtfloor_TextChanged(object sender, EventArgs e)
        {
            createtablebutton();
        }
        private void FoodOrder_Click(object sender, EventArgs e)
        {
            createtablebutton();
        }
        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {
        }
        public void date_checker()
        {
            string status = "";
            if (checkBox1.Checked)
            {
                dateTimePicker1.Enabled = false;
                status = "true";
                string delete_check = "truncate table date_checker";
                clsDataLayer.ExecuteQuery(delete_check);
                string date_querrry = "insert into date_checker (date,status) values('" + dateTimePicker1.Text + "','" + status + "')";
                clsDataLayer.ExecuteQuery(date_querrry);
            }
            else
            {
                dateTimePicker1.Enabled = true;
                //status = "false";
                string delete_check = "truncate table date_checker";
                clsDataLayer.ExecuteQuery(delete_check);
                //string date_querrry = "insert into date_checker (date,status) values('" + dateTimePicker1.Text + "','" + status + "')";
                //clsDataLayer.ExecuteQuery(date_querrry);
                dateTimePicker1.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        public void fetch_date()
        {
            string check = "select date,status from date_checker";
            DataTable ds = clsDataLayer.RetreiveQuery(check);
            if (ds.Rows.Count > 0)
            {
                String Code = ds.Rows[0][0].ToString();
                dateTimePicker1.Text = Code.ToString();
                String status_check = ds.Rows[0][1].ToString();
                if (status_check == "true")
                {
                    checkBox1.Checked = true;
                    dateTimePicker1.Enabled = false;
                }
                else
                {
                    checkBox1.Checked = false;
                    dateTimePicker1.Enabled = true;
                }
            }
            else
            {
                dateTimePicker1.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        private void FoodOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        private void checkBox1_Click(object sender, EventArgs e)
        {
            date_checker();
        }
        private void txtwaiter_TextChanged(object sender, EventArgs e)
        {
        }
        private void FoodOrder_Load(object sender, EventArgs e)
        {
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                button2.BackColor = Color.Green;
                uncheckall.BackColor = Color.FromArgb(255, 192, 192);
                int rowscount = grid.Rows.Count;
                if (rowscount > 0)
                {
                    for (int i = 0; i < rowscount; i++)
                    {
                        grid.Rows[i].Cells[8].Value = true;
                    }
                }
            }
            catch
            {
            }
        }
        private void uncheckall_Click(object sender, EventArgs e)
        {
            try
            {
                uncheckall.BackColor = Color.Green;
                button2.BackColor = Color.FromArgb(255, 192, 192);
                int rowscount = grid.Rows.Count;
                if (rowscount > 0)
                {
                    for (int i = 0; i < rowscount; i++)
                    {
                        grid.Rows[i].Cells[8].Value = false;
                    }
                }
            }
            catch
            {
            }
        }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                closed_tables ct = new closed_tables();
                ct.Show();
            }
            catch 
            {
                
            }
        }

        private void txtdiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
