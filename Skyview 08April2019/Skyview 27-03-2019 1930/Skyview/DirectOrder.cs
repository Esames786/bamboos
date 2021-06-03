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
using System.Data.SqlClient;
namespace Skyview
{
    public partial class DirectOrder : Form
    {
        String CName = ""; String Categories5 = "";
        String UID = Login.UserID;
        public DirectOrder()
        {
            InitializeComponent();
            createbutton();
            CodeGenerated();
            fetch_date();
            GridFresh();
            button2.BackColor = Color.Green;
            member();

            chkclear.Checked = false;
            chkbill.Checked = false;
            load_waiters();

            clsGeneral.SetAutoCompleteTextBox(c_name, clsDataLayer.RetreiveQuery("select distinct(c_name) from [tbl_SaleByTableQ] "));
            clsGeneral.SetAutoCompleteTextBox(txtphone, clsDataLayer.RetreiveQuery("select distinct(c_phone) from [tbl_SaleByTableQ]"));
        }
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        public void GridFresh()
        {
            string dtt = dateTimePicker1.Value.ToString();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT DISTINCT SaleCode, MAX(Date) FROM SaleView where TokenCode= '-' and Date = '" + dtt.ToString() + "' GROUP BY SaleCode ORDER BY MAX(Date) DESC , SaleCode desc", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                grid3.Rows.Clear();
                foreach (DataRow item in dt.Rows)
                {

                    int n = grid3.Rows.Add();
                    grid3.Rows[n].Cells[0].Value = item["SaleCode"].ToString();
                    string ro = item["SaleCode"].ToString();
                    String GetDetail = "select SaleCode from tbl_SaleReceipt where SaleCode='" + ro + "'";
                    DataTable ds = clsDataLayer.RetreiveQuery(GetDetail);
                    if (ds.Rows.Count == 0)
                    {
                        grid3.Rows[n].Cells[0].Style.BackColor = Color.Yellow;
                        grid3.Rows[n].Cells[0].Style.SelectionBackColor = Color.Yellow;
                        grid3.Rows[n].Cells[0].Style.SelectionForeColor = Color.Black;
                        grid3.Columns[0].DefaultCellStyle.Font = new Font("Verdana", 10, FontStyle.Bold);

                    }
                    else
                    {
                        grid3.Rows[n].Cells[0].Style.BackColor = Color.Green;
                        grid3.Rows[n].Cells[0].Style.SelectionBackColor = Color.Green;
                        grid3.Rows[n].Cells[0].Style.SelectionForeColor = Color.Black;
                        grid3.Columns[0].DefaultCellStyle.Font = new Font("Verdana", 10, FontStyle.Bold);

                    }

                }
            }
            else
            {
                grid3.DataSource = null;
                grid3.Rows.Clear();
            }
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
                        b8.Width = 80; b8.Height = 27;
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

        Token rt = new Token();
        public void printing()
        {
            try
            {
                String trun = "truncate table tbl_Token2"; clsDataLayer.ExecuteQuery(trun);

                

                for (int i = 0; i < grid.Rows.Count; i++)
                {
                    bool m = Convert.ToBoolean(grid.Rows[i].Cells[6].Value.ToString());
                    if (m == true)
                    {


                        String query = "select Product_Categories from tbl_ProductBySale where Product_Name = '" + grid.Rows[i].Cells[0].Value + "'"; DataTable da = clsDataLayer.RetreiveQuery(query);
                        if (da.Rows.Count > 0)
                        {
                            string cat = da.Rows[0][0].ToString();
                            String ins1 = "insert into tbl_Token2(CGross_Amount,Discount,TokenCode,SaleCode,Product_Name,Quantity,Sell_Price,Total_Amount,FloorNo,TableNo,Waiter,TableStatus,UserName,Date,categories,descc)values(" + grid.Rows[i].Cells[3].Value.ToString() + "," + grid.Rows[i].Cells[4].Value.ToString() + ",'" + txttoken.Text + "','" + txtcode.Text + "','" + grid.Rows[i].Cells[0].Value.ToString() + "','" + grid.Rows[i].Cells[1].Value.ToString() + "','" + grid.Rows[i].Cells[2].Value.ToString() + "'," + grid.Rows[i].Cells[5].Value.ToString() + ",'-','-','" + txtwaiter.Text + "','Active','" + Login.UserID + "','" + dateTimePicker1.Text + "','" + cat + "','" + richTextBox1.Text + "')"; clsDataLayer.ExecuteQuery(ins1);

                        }
                        else
                        {
                            String query2 = "select Product_Categories from tbl_deals where Product_Name = '" + grid.Rows[i].Cells[0].Value + "'"; DataTable da1 = clsDataLayer.RetreiveQuery(query2);
                            if (da1.Rows.Count > 0)
                            {
                                string cat = da1.Rows[0][0].ToString();
                                String ins1 = "insert into tbl_Token2(CGross_Amount,Discount,TokenCode,SaleCode,Product_Name,Quantity,Sell_Price,Total_Amount,FloorNo,TableNo,Waiter,TableStatus,UserName,Date,categories,descc)values(" + grid.Rows[i].Cells[3].Value.ToString() + "," + grid.Rows[i].Cells[4].Value.ToString() + ",'" + txttoken.Text + "','" + txtcode.Text + "','" + grid.Rows[i].Cells[0].Value.ToString() + "','" + grid.Rows[i].Cells[1].Value.ToString() + "','" + grid.Rows[i].Cells[2].Value.ToString() + "'," + grid.Rows[i].Cells[5].Value.ToString() + ",'-','-','" + txtwaiter.Text + "','Active','" + Login.UserID + "','" + dateTimePicker1.Text + "','" + cat + "','" + richTextBox1.Text + "')"; clsDataLayer.ExecuteQuery(ins1);

                            }
                        }
                    }
                }

                String query8 = "select distinct(categories) from tbl_Token2"; DataTable da8 = clsDataLayer.RetreiveQuery(query8);
                if (da8.Rows.Count > 0)
                {
                    string[] array = new string[da8.Rows.Count];
                    string my = "";
                    string strTrimmed = "";
                    string finalString = "";

                    for (int k = 0; k < da8.Rows.Count; k++)
                    {
                        array[k] = da8.Rows[k][0].ToString();
                    }
                    for (int l = 0; l < array.Length; l++)
                    {
                        my = array[l].ToString();
                        strTrimmed = my.Trim();
                        finalString = strTrimmed.Substring(strTrimmed.LastIndexOf(" ", strTrimmed.Length));
                        array[l] = finalString;
                    }
                    string[] q = array.Distinct().ToArray();
                    for (int n = 0; n < q.Length; n++)
                    {
                        String query = "select * from tbl_Token2 where categories like '%" + q[n] + "' "; DataTable da = clsDataLayer.RetreiveQuery(query);
                        if (da.Rows.Count > 0)
                        {
                            rt.SetDataSource(da);
                            rt.PrintToPrinter(1, false, 0, 0);
                            //token_view aop = new token_view(rt); aop.Show();
                            //PrintDialog pd = new PrintDialog(); PrintDocument pdo = new PrintDocument(); pdo.DocumentName = "Print Document"; pd.Document = pdo;
                            //pd.AllowSelection = true; pd.AllowSomePages = true; if (pd.ShowDialog() == DialogResult.OK) pdo.Print();
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

        public void createtoken()
        {
            if (grid.Rows.Count < 1)
            {
                //txttoken.Text = clsGeneral.getMAXCode("tbl_SaleByTableQ", "TokenCode", "TK"); 
            }
        }


        public void CodeGenerated()
        {
            if (grid.Rows.Count < 1)
            {
                txtcode.Text = clsGeneral.getMAXCode("tbl_SaleByTableQ", "SaleCode", "SC"); c_name.Clear(); txtphone.Clear(); txt_address.Clear(); txtwaiter.Clear();
            }
        }

        private void createbutton()
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
            try
            {
                decimal my_dis = 0;
                decimal temp = 0;
                String GetDetail5 = "select TotalDiscount from tbl_Sale where DSaleCode='" + txtcode.Text + "'";
                DataTable ds5 = clsDataLayer.RetreiveQuery(GetDetail5);
                if (ds5.Rows.Count > 0)
                {
                    my_dis = Convert.ToDecimal(ds5.Rows[0][0].ToString());
                }

                decimal total = 0; decimal discount = 0;
                for (int b = 0; b < grid.Rows.Count; b++)
                {
                    total += Convert.ToDecimal(grid.Rows[b].Cells[3].Value); discount += Convert.ToDecimal(grid.Rows[b].Cells[4].Value);
                }
                txtbill.Text = total.ToString();


                temp = my_dis;


                txtdiscount.Text = temp.ToString();


            }
            catch 
            {
                
                
            }
            
        }

        private void total2()
        {
            try
            {
                

                decimal total = 0; decimal discount = 0;
                for (int b = 0; b < grid.Rows.Count; b++)
                {
                    total += Convert.ToDecimal(grid.Rows[b].Cells[3].Value); discount += Convert.ToDecimal(grid.Rows[b].Cells[4].Value);
                }
                txtbill.Text = total.ToString();
                txtdiscount.Text = discount.ToString();


            }
            catch
            {


            }

        }

        void b2_Click(object sender, EventArgs e)
        {
            Button bs = sender as Button;
            //  foodcheck(bs.Text);
            if (ck == false)
            {
                CodeGenerated();
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
                    CodeGenerated();
                    String GetDetail = "select Product_Name,Sell_Price from tbl_ProductBySale where Product_Name='" + bs.Text + "'";
                    DataTable ds = clsDataLayer.RetreiveQuery(GetDetail);
                    decimal sell = Convert.ToDecimal(ds.Rows[0][1].ToString()); decimal qnt = 1;
                    decimal ff = sell * qnt; GridInsert(bs.Text, qnt.ToString(), sell.ToString(), ff.ToString()); total();

                }
            }
            else { MessageBox.Show("Already Save!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        void b_Click(object sender, EventArgs e)
        {
            Button bs = sender as Button; CName = bs.Text; createtoken(); createFoodbutton(CName);
        }
        public void GetTotal()
        {
            string dd = "";
            String selo = "select Date from tbl_Sale where DSaleCode='" + txtcode.Text + "'";
            DataTable d1o = clsDataLayer.RetreiveQuery(selo);
            if (d1o.Rows.Count > 0)
            {
                dd = d1o.Rows[0][0].ToString();
            }
            String dels = "delete from tbl_Sale where DSaleCode='" + txtcode.Text + "'";
            if (clsDataLayer.ExecuteQuery(dels) == 0) { dd = dateTimePicker1.Text; }
            decimal gs = 0; decimal dis = 0; decimal net = 0;
            decimal igs = Convert.ToDecimal(txtbill.Text); decimal idis = Convert.ToDecimal(txtdiscount.Text); decimal inet = Convert.ToDecimal(txtfinal.Text);
            String sel = "select Gross_Amount,TotalDiscount,Net_Amount from tbl_Sale where DSaleCode='" + txtcode.Text + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(sel); if (d1.Rows.Count > 0)
            {
                gs = Convert.ToDecimal(d1.Rows[0][0].ToString()); dis = Convert.ToDecimal(d1.Rows[0][1].ToString()); net = Convert.ToDecimal(d1.Rows[0][2].ToString());
                gs = igs; dis += idis; decimal final = gs - dis;
                String del = "delete from tbl_Sale where DSaleCode='" + txtcode.Text + "'"; clsDataLayer.ExecuteQuery(del);
                String ins = "insert into tbl_Sale(Date,DSaleCode,Gross_Amount,TotalDiscount,Net_Amount)values('" + dd + "','" + txtcode.Text + "'," + gs + "," + dis + "," + final + ")"; clsDataLayer.ExecuteQuery(ins);
            }
            else
            {
                String ins = "insert into tbl_Sale(Date,DSaleCode,Gross_Amount,TotalDiscount,Net_Amount)values('" + dd + "','" + txtcode.Text + "'," + igs + "," + idis + "," + inet + ")"; clsDataLayer.ExecuteQuery(ins);
            }
        }
        private void InsertOneTime()
        {
            string cat_find = "";
            String del = "delete from tbl_SaleByTableQ where SaleCode='" + txtcode.Text + "'"; clsDataLayer.ExecuteQuery(del);
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

                    String ibs = "insert into tbl_SaleByTableQ(FloorNo,TableNo,Waiter,UserName,SaleCode,TokenCode,Product_Name,Quantity,Sell_Price,CGross_Amount,Discount,Total_Amount,descc,TableStatus,[Date],[PrintStatus],[c_name],[c_phone],[c_address],[Product_Categories])values('-','-','" + txtwaiter.Text + "','" + Login.UserID + "','" + txtcode.Text + "','-','" + grid.Rows[b].Cells[0].Value.ToString() + "'," + grid.Rows[b].Cells[1].Value.ToString() + "," + grid.Rows[b].Cells[2].Value.ToString() + "," + grid.Rows[b].Cells[3].Value.ToString() + "," + grid.Rows[b].Cells[4].Value.ToString() + "," + grid.Rows[b].Cells[5].Value.ToString() + ",'" + richTextBox1.Text + "','-','" + dateTimePicker1.Text + "','','" + c_name.Text + "','" + txtphone.Text + "','" + txt_address.Text + "','" + cat_find + "')";
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

                        String ibs = "insert into tbl_SaleByTableQ(FloorNo,TableNo,Waiter,UserName,SaleCode,TokenCode,Product_Name,Quantity,Sell_Price,CGross_Amount,Discount,Total_Amount,descc,TableStatus,[Date],[PrintStatus],[c_name],[c_phone],[c_address],[Product_Categories])values('-','-','" + txtwaiter.Text + "','" + Login.UserID + "','" + txtcode.Text + "','-','" + grid.Rows[b].Cells[0].Value.ToString() + "'," + grid.Rows[b].Cells[1].Value.ToString() + "," + grid.Rows[b].Cells[2].Value.ToString() + "," + grid.Rows[b].Cells[3].Value.ToString() + "," + grid.Rows[b].Cells[4].Value.ToString() + "," + grid.Rows[b].Cells[5].Value.ToString() + ",'" + richTextBox1.Text + "','-','" + dateTimePicker1.Text + "','','" + c_name.Text + "','" + txtphone.Text + "','" + txt_address.Text + "','" + cat_find + "')";
                        clsDataLayer.ExecuteQuery(ibs);
                    }
                    else
                    {
                        decimal qs = Convert.ToDecimal(d2.Rows[0][0].ToString()); decimal gs = Convert.ToDecimal(d2.Rows[0][1].ToString());
                        decimal dis = Convert.ToDecimal(d2.Rows[0][2].ToString()); decimal tot = Convert.ToDecimal(d2.Rows[0][3].ToString());
                        decimal gqs = Convert.ToDecimal(grid.Rows[b].Cells[1].Value); decimal ggs = Convert.ToDecimal(grid.Rows[b].Cells[3].Value);
                        decimal gdis = Convert.ToDecimal(grid.Rows[b].Cells[4].Value); decimal gtot = Convert.ToDecimal(grid.Rows[b].Cells[5].Value);
                        decimal fqs = qs + gqs; decimal fgs = gs + ggs; decimal fdis = dis + gdis; decimal ftot = tot + gtot;
                        String del0 = "delete from tbl_SaleByTableQ where SaleCode='" + txtcode.Text + "' and Product_Name='" + pdname + "'"; clsDataLayer.ExecuteQuery(del0);


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

                        String ibs = "insert into tbl_SaleByTableQ(FloorNo,TableNo,Waiter,UserName,SaleCode,TokenCode,Product_Name,Quantity,Sell_Price,CGross_Amount,Discount,Total_Amount,descc,TableStatus,[Date],[PrintStatus],[c_name],[c_phone],[c_address],[Product_Categories])values('-','-','" + txtwaiter.Text + "','" + Login.UserID + "','" + txtcode.Text + "','-','" + grid.Rows[b].Cells[0].Value.ToString() + "'," + fqs + "," + grid.Rows[b].Cells[2].Value.ToString() + "," + fgs + "," + fdis + "," + ftot + ",'" + richTextBox1.Text + "','-','" + dateTimePicker1.Text + "','','" + c_name.Text + "','" + txtphone.Text + "','" + txt_address.Text + "','" + cat_find + "')";
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

                    String RCode = "";
                    String g1 = "select RCode from tbl_Consumption where SaleProductName ='" + pdname + "'";
                    DataTable d5 = clsDataLayer.RetreiveQuery(g1);
                    if (d5.Rows.Count > 0)
                    {
                        RCode = d5.Rows[0][0].ToString();
                    }

                    String get1 = "select ReceipeProductName,Quantity from tbl_ConsumptionDetail where RCode ='" + RCode + "'";
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

        private void btn_TblClosed_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid.Rows.Count < 1)
                {
                    MessageBox.Show("Add Product First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    btn_TblClosed.Text = "Wait.... Token is Priting..";
                    string dd = "";
                    String selo = "select Date from tbl_SaleByTableQ where SaleCode='" + txtcode.Text + "'";
                    DataTable d1o = clsDataLayer.RetreiveQuery(selo);
                    if (d1o.Rows.Count > 0)
                    {
                        dd = d1o.Rows[0][0].ToString();
                    }
                    String del = "delete from tbl_SaleByTableQ where SaleCode='" + txtcode.Text + "'"; if (clsDataLayer.ExecuteQuery(del) == 0) { dd = dateTimePicker1.Text; }
                    String sel = "select * from tbl_SaleByTableQ where SaleCode='" + txtcode.Text + "'"; DataTable ds = clsDataLayer.RetreiveQuery(sel);
                    if (ds.Rows.Count < 1)
                    {
                        StockManage(); individualstock();
                        GetTotal(); InsertOneTime(); c_info(); description(); printing(); if (chkbill.Checked == true) { Print(); } GridFresh(); grid.Rows.Clear(); txtbill.Text = ""; txtdiscount.Text = ""; txtfinal.Text = "";
                        createbutton(); PRS pr = new PRS(this); pr.txtsale.Text = txtcode.Text; pr.Show(); pr.check(); btn_TblClosed.Text = "Save"; c_name.Clear(); txtphone.Clear(); txt_address.Clear();
                        richTextBox1.Clear();

                        chkclear.Checked = false;
                        chkbill.Checked = false;

                        txtwaiter.Clear();

                        clsGeneral.SetAutoCompleteTextBox(c_name, clsDataLayer.RetreiveQuery("select distinct(c_name) from [tbl_SaleByTableQ] "));
                        clsGeneral.SetAutoCompleteTextBox(txtphone, clsDataLayer.RetreiveQuery("select distinct(c_phone) from [tbl_SaleByTableQ]"));

                    }
                    else { MessageBox.Show("Already Save!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
            catch { }
        }
        public void view_desc()
        {
            richTextBox1.Clear();
            String sel = "select descc from tbl_SaleByTableQ where SaleCode='" + txtcode.Text + "'"; DataTable ds = clsDataLayer.RetreiveQuery(sel);
            if (ds.Rows.Count > 0)
            {
                richTextBox1.Text = ds.Rows[0][0].ToString();
            }
        }
        public void description()
        {
            try
            {
                String sel = "select descc from tbl_SaleByTableQ where SaleCode='" + txtcode.Text + "'"; DataTable ds = clsDataLayer.RetreiveQuery(sel);
                if (ds.Rows.Count > 0)
                {
                    string desc = richTextBox1.Text;
                    string ins = "update tbl_SaleByTableQ set descc ='" + desc + "' where SaleCode='" + txtcode.Text + "'";
                    clsDataLayer.ExecuteQuery(ins);
                }
            }
            catch
            {
            }
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
                        grid.Rows[b].Cells[3].Value = final.ToString();
                        final = final - dis;
                        grid.Rows[b].Cells[5].Value = final.ToString();

                        String sel = "select Product_Categories from tbl_ProductBySale where Product_Name='" + Pname + "'"; DataTable ds = clsDataLayer.RetreiveQuery(sel);
                        if (ds.Rows.Count > 0)
                        {
                            string bv = ds.Rows[0][0].ToString();
                            if (bv != "SERVICE CHARGES TB")
                            {
                                grid.Rows[b].Cells[6].Value = true;
                            }
                            else
                            {
                                grid.Rows[b].Cells[6].Value = false;
                            }
                        }


                    }
                }
                if (abc == false)
                {
                    int n = grid.Rows.Add(); grid.Rows[n].Cells[0].Value = Pname; grid.Rows[n].Cells[1].Value = Quantity; grid.Rows[n].Cells[2].Value = Sell; grid.Rows[n].Cells[3].Value = Total; ; grid.Rows[n].Cells[4].Value = "0"; grid.Rows[n].Cells[5].Value = Total;
                    String sel = "select Product_Categories from tbl_ProductBySale where Product_Name='" + Pname + "'"; DataTable ds = clsDataLayer.RetreiveQuery(sel);
                    if (ds.Rows.Count > 0)
                    {
                        string bv = ds.Rows[0][0].ToString();

                        if (bv != "SERVICE CHARGES TB")
                        {
                            grid.Rows[n].Cells[6].Value = true;
                        }
                        else
                        {
                            grid.Rows[n].Cells[6].Value = false;
                        }
                    }
                    else
                    {
                        String sel1 = "select Product_Categories from tbl_deals where Product_Name='" + Pname + "'"; DataTable ds1 = clsDataLayer.RetreiveQuery(sel1);
                        if (ds1.Rows.Count > 0)
                        {
                            string bv = ds1.Rows[0][0].ToString();

                            if (bv != "SERVICE CHARGES TB")
                            {
                                grid.Rows[n].Cells[6].Value = true;
                            }
                            else
                            {
                                grid.Rows[n].Cells[6].Value = false;
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
                    decimal c = a * b; grid.CurrentRow.Cells[3].Value = c; c = c - d; grid.CurrentRow.Cells[5].Value = c; total();
                }
                else if (grid.CurrentCell.ColumnIndex == 4)
                {
                    decimal a = Convert.ToDecimal(grid.CurrentRow.Cells[1].Value); decimal b = Convert.ToDecimal(grid.CurrentRow.Cells[2].Value);
                    decimal d = Convert.ToDecimal(grid.CurrentRow.Cells[4].Value);
                    decimal c = a * b; grid.CurrentRow.Cells[3].Value = c; c = c - d; grid.CurrentRow.Cells[5].Value = c; total();
                }
            }
            catch { }
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
        private void txtdiscount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtdiscount.Text.Length > 0)
                {
                    decimal a = Convert.ToDecimal(txtbill.Text); decimal b = Convert.ToDecimal(txtdiscount.Text); decimal c = a - b; txtfinal.Text = c.ToString();
                }
                else
                {
                    txtfinal.Text = txtbill.Text;
                }
            }
            catch { }
        }

        private void individualstock()
        {
            try
            {
                String pdname = grid.CurrentRow.Cells[0].Value.ToString();

                String get5 = "select Quantity from tbl_SaleByTableQ where Product_Name='" + pdname + "' and SaleCode='" + txtcode.Text + "'";
                DataTable d15 = clsDataLayer.RetreiveQuery(get5);
                if (d15.Rows.Count > 0)
                {

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
            }
            catch { }
        }

        private void grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    String GetDetail = "select SaleCode from tbl_SaleReceipt where SaleCode='" + txtcode.Text + "'";
                    DataTable ds = clsDataLayer.RetreiveQuery(GetDetail);
                    if (ds.Rows.Count > 0)
                    {
                        FormID = "Delete";
                        UserNametesting();
                        if (GreenSignal == "YES")
                        {
                            if (grid.Rows.Count > 0)
                            {
                                individualstock();
                                int yh = grid.CurrentCell.RowIndex;
                                grid.Rows.RemoveAt(yh); total();
                            }

                        }
                        else
                        {
                            MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                    else
                    {

                        if (grid.Rows.Count > 0)
                        {
                            individualstock();
                            int yh = grid.CurrentCell.RowIndex;
                            grid.Rows.RemoveAt(yh); total();
                        }

                    }
                }

            }
            catch { }
        }
        private void grid3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtcode.Text = grid3.CurrentRow.Cells[0].Value.ToString(); Searchm(); view_desc(); check();

            chkclear.Checked = false;
            chkbill.Checked = false;
        }

        public void Print()
        {

            decimal txt_rec = 0;
            decimal txt_return = 0;

            String query1 = "select CashReceived,ReturnAmount from tbl_SaleReceipt  where SaleCode='" + txtcode.Text + "'"; DataTable da1 = clsDataLayer.RetreiveQuery(query1);
            if (da1.Rows.Count > 0)
            {
                txt_rec = Convert.ToDecimal(da1.Rows[0][0].ToString());
                txt_return = Convert.ToDecimal(da1.Rows[0][1].ToString());
            }

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
                receipt3 rt = new receipt3(); rt.SetDataSource(dv); viewer_of_table_close aop = new viewer_of_table_close(rt, txt_rec, txt_return, 0,service_c); aop.Show();
                rt.PrintToPrinter(1, false, 0, 0);
            }

            
             
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                String query = "select * from SaleView where SaleCode='" + txtcode.Text + "'"; DataTable da = clsDataLayer.RetreiveQuery(query);
                if (da.Rows.Count > 0)
                {
                    Print();
                }
                else { MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            catch 
            {
                
            }
            
        }
        private void grid3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Print();
        }
        private void Searchm()
        {
            grid.Rows.Clear();
            String sel = "select * from tbl_SaleByTableQ where SaleCode='" + txtcode.Text + "'"; DataTable ds = clsDataLayer.RetreiveQuery(sel);
            if (ds.Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Rows)
                {
                    int n = grid.Rows.Add();
                    //,,SaleCode,,,,
                    grid.Rows[n].Cells[0].Value = dr["Product_Name"].ToString();
                    grid.Rows[n].Cells[1].Value = dr["Quantity"].ToString();
                    grid.Rows[n].Cells[2].Value = dr["Sell_Price"].ToString();
                    grid.Rows[n].Cells[3].Value = dr["CGross_Amount"].ToString();
                    grid.Rows[n].Cells[4].Value = dr["Discount"].ToString();
                    grid.Rows[n].Cells[5].Value = dr["Total_Amount"].ToString();
                    txtwaiter.Text = dr["Waiter"].ToString();
                    grid.Rows[n].Cells[6].Value = false;
                    txttoken.Text = dr["TokenCode"].ToString();
                }
                total();
            }
        }
        private void txtcode_TextChanged(object sender, EventArgs e)
        {
            Searchm();
        }
        private void btnnew_Click(object sender, EventArgs e)
        {
            try
            {

                txt_dis_p.Clear(); txtwaiter.Clear(); grid.Rows.Clear(); txtbill.Text = "0"; txtdiscount.Text = "0"; txtfinal.Text = "0"; createbutton(); CodeGenerated(); GridFresh(); richTextBox1.Clear(); c_name.Clear(); txtphone.Clear(); txt_address.Clear();
            }
            catch { }
        }
        private void grid3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtcode.Text = grid3.CurrentRow.Cells[0].Value.ToString(); Searchm(); view_desc(); check();

            chkclear.Checked = false;
            chkbill.Checked = false;

        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            GridFresh();
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
        private void checkBox1_Click(object sender, EventArgs e)
        {
            date_checker();
        }
        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
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
                        grid.Rows[i].Cells[6].Value = true;
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
                        grid.Rows[i].Cells[6].Value = false;
                    }
                }
            }
            catch
            {
            }
        }

        private void c_name_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
                e.Handled = true;
        }

        private void txtphone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txt_address_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtphone_TextChanged(object sender, EventArgs e)
        {
            nocheck();
        }

        public void nocheck()
        {
            try
            {
                if (txtphone.Text.Length > 1)
                {


                    String h3 = "select c_address from [tbl_SaleByTableQ] where c_phone='" + txtphone.Text + "'";
                    DataTable h30 = clsDataLayer.RetreiveQuery(h3); if (h30.Rows.Count > 0)
                    {
                        txt_address.Text = h30.Rows[0][0].ToString();
                    }
                    else
                    {
                        txt_address.Text = "";
                    }
                }

            }
            catch
            {


            }

        }


        public void check()
        {
            String c_check = "select c_name,c_phone,c_address from [tbl_SaleByTableQ] where SaleCode = '" + txtcode.Text + "'";
            DataTable d2 = clsDataLayer.RetreiveQuery(c_check);
            if (d2.Rows.Count > 0)
            {
                c_name.Text = d2.Rows[0][0].ToString();
                txtphone.Text = d2.Rows[0][1].ToString();
                txt_address.Text = d2.Rows[0][2].ToString();

            }
        }

        public void c_info()
        {
            try
            {
                if (grid.Rows.Count >= 1)
                {
                    if (txtphone.Text.Length == 0)
                    {
                        txtphone.Text = "00";
                    }
                    if (c_name.Text.Length == 0)
                    {
                        c_name.Text = "-";
                    } if (txt_address.Text.Length == 0)
                    {
                        txt_address.Text = "-";
                    }


                    String c_check = "select SaleCode from [tbl_SaleByTableQ] where SaleCode = '" + txtcode.Text + "' ";
                    DataTable d2 = clsDataLayer.RetreiveQuery(c_check);
                    if (d2.Rows.Count > 0)
                    {
                        String c_info = "update [tbl_SaleByTableQ] set  c_name = '" + c_name.Text + "',c_address = '" + txt_address.Text + "',c_phone = '" + txtphone.Text + "' Where SaleCode  = '" + txtcode.Text + "'";
                        clsDataLayer.ExecuteQuery(c_info);

                    }
                    else
                    {

                        

                    }
                }
                else { MessageBox.Show("Kindly Receive the bill amount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
            }
            catch { }
        }



        private void chkclear_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                chkclear.Checked = true;

                richTextBox1.Text = "";
            }
            catch
            {

            }
        }



        private void RefundStockManage()
        {
            try
            {
                String n5 = "select Product_Name,Quantity from tbl_SaleByTableQ where SaleCode = '" + txtcode.Text + "' ";
                DataTable d90 = clsDataLayer.RetreiveQuery(n5);
                if (d90.Rows.Count > 0)
                {
                    for (int b = 0; b < d90.Rows.Count; b++)
                    {
                        String pdname = d90.Rows[b][0].ToString();
                        decimal InputQuant = Convert.ToDecimal(d90.Rows[b][1].ToString());
                        #region all
                        String RCode = "";
                        String g1 = "select RCode from tbl_Consumption where SaleProductName ='" + pdname + "'";
                        DataTable d5 = clsDataLayer.RetreiveQuery(g1);
                        if (d5.Rows.Count > 0)
                        {
                            RCode = d5.Rows[0][0].ToString();
                        }

                        String get1 = "select ReceipeProductName,Quantity from tbl_ConsumptionDetail where RCode ='" + RCode + "'";
                        DataTable d12 = clsDataLayer.RetreiveQuery(get1);
                        if (d12.Rows.Count > 0)
                        {
                            decimal oldtq = 0; decimal multiplyquant = 0; String condstatus = "";

                            #region cond
                            multiplyquant = InputQuant; condstatus = "plus";

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
                                if (condstatus.Equals("plus"))
                                {
                                    totalquant = allowquant * multiplyquant;
                                    final = stock + totalquant;
                                }
                                String upd = "update tblStockMaintain set Quantity=" + final + " where ProductName='" + Receprod + "'"; clsDataLayer.ExecuteQuery(upd);
                            }


                        }
                        #endregion all
                    }
                }
            }
            catch { }

        }


        public string GreenSignal = "";
        public string FormID = "";
        public void UserNametesting()
        {
            string query = "SELECT FORM_ID,ACCESS FROM user_access WHERE USENAME = '" + UID + "' AND FORM_ID = '" + FormID + "'";
            DataTable drs = clsDataLayer.RetreiveQuery(query);
            if (drs.Rows.Count > 0)
            {
                String frmid = drs.Rows[0]["FORM_ID"].ToString();
                String Acs = drs.Rows[0]["ACCESS"].ToString();
                if (Acs.Equals("Yes"))
                {
                    GreenSignal = "YES";
                }
                else
                {
                    GreenSignal = "NO";
                }
            }
            else
            {
                GreenSignal = "NO";
            }
        }

        private void btn_refund_Click(object sender, EventArgs e)
        {
            FormID = "2000";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                if (grid.Rows.Count > 0)
                {
                    String c_check = "select SaleCode from tbl_SaleByTableQ where SaleCode = '" + txtcode.Text + "' ";
                    DataTable data1 = clsDataLayer.RetreiveQuery(c_check);
                    if (data1.Rows.Count > 0)
                    {
                        DialogResult Result = MessageBox.Show("Mr. " + UID + "! This will delete complete order of  '" + txtcode.Text + "'?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (Result == DialogResult.Yes)
                        {
                            RefundStockManage();
                            String sel2 = "select [TokenCode],[SaleCode],[Product_Name],[Quantity],[Sell_Price],[Discount],[CGross_Amount],[Total_Amount],[FloorNo],[TableNo],[Waiter],[UserName],[descc] from tbl_SaleByTableQ where SaleCode='" + txtcode.Text + "'"; DataTable d2 = clsDataLayer.RetreiveQuery(sel2);
                            if (d2.Rows.Count > 0)
                            {
                                for (int b = 0; b < d2.Rows.Count; b++)
                                {
                                    string tk = d2.Rows[b][0].ToString();
                                    string s_c = d2.Rows[b][1].ToString();
                                    string p_n = d2.Rows[b][2].ToString();
                                    decimal qt = Convert.ToDecimal(d2.Rows[b][3].ToString());
                                    decimal s_p = Convert.ToDecimal(d2.Rows[b][4].ToString());
                                    decimal d_c = Convert.ToDecimal(d2.Rows[b][5].ToString());
                                    decimal cga = Convert.ToDecimal(d2.Rows[b][6].ToString());
                                    decimal t_a = Convert.ToDecimal(d2.Rows[b][7].ToString());
                                    string fl = d2.Rows[b][8].ToString();
                                    string tb_no = d2.Rows[b][9].ToString();
                                    string wait = d2.Rows[b][10].ToString();
                                    string u_nme = d2.Rows[b][11].ToString();
                                    string descip = d2.Rows[b][12].ToString();

                                    String ibs = "insert into refund([TokenCode],[SaleCode],[Product_Name],[Quantity],[Sell_Price],[Discount],[CGross_Amount],[Total_Amount],[FloorNo],[TableNo],[Waiter],[UserName],[descc],Date)values('" + tk + "','" + s_c + "','" + p_n + "','" + qt + "','" + s_p + "','" + d_c + "','" + cga + "','" + t_a + "','" + fl + "','" + tb_no + "','" + wait + "','" + u_nme + "','" + descip + "','" + dateTimePicker1.Text + "')";
                                    clsDataLayer.ExecuteQuery(ibs);
                                }

                                String del0 = "delete from tbl_SaleByTableQ where SaleCode='" + txtcode.Text + "'"; clsDataLayer.ExecuteQuery(del0);
                                String del11 = "delete from tbl_SaleByTable where SaleCode='" + txtcode.Text + "'"; clsDataLayer.ExecuteQuery(del11);
                                String del12 = "delete from tbl_Sale where DSaleCode='" + txtcode.Text + "'"; clsDataLayer.ExecuteQuery(del12);
                                String del13 = "delete from tbl_SaleReceipt where SaleCode='" + txtcode.Text + "'"; clsDataLayer.ExecuteQuery(del13);
                            }
                            c_name.Clear(); txtphone.Clear(); txt_address.Clear();
                            GridFresh(); grid.Rows.Clear(); txtbill.Text = ""; txtdiscount.Text = ""; txtfinal.Text = "";
                            richTextBox1.Clear();

                            chkclear.Checked = false;
                            chkbill.Checked = false;


                            MessageBox.Show(" Mr/Ms" + UID + " has deleted '" + txtcode.Text + "' Successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        else if (Result == DialogResult.No)
                        {
                            MessageBox.Show(" Mr/Ms" + UID + " continue your work!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("This order is not found");
                    }
                }
                else
                {
                    MessageBox.Show("Order is not found");
                }
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        private void txtdiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }



        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            richTextBox1.Text = comboBox1.Text;
        }

        private void flowwaiter_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txt_discount_p_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txt_discount_p_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (grid.Rows.Count > 0)
                {
                    if (txt_dis_p.Text.Length > 0)
                    {
                        total2();
                        decimal d_per = Convert.ToDecimal(txt_dis_p.Text);
                        decimal d_price = Convert.ToDecimal(txtdiscount.Text);

                        decimal bill = Convert.ToDecimal(txtbill.Text);

                        decimal totall = (bill * d_per) / 100;

                        txtdiscount.Text = Convert.ToString(d_price + totall);
                    }
                    else
                    {
                        total2();
                    }
                }
                
            }
            catch 
            {
                
               
            }
        }



    }
}
