using Skyview;
using Skyview.bin.Debug.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyview
{
    public partial class PurchaseVoucher1cs : Form
    {
        String Code = "";
        TextBox tb = new TextBox(); decimal old = 0;
        public PurchaseVoucher1cs()
        {
            InitializeComponent();

            //clsGeneral.SetAutoCompleteTextBox(txt_pur_code, clsDataLayer.RetreiveQuery("select pur_code from tbl_purchase"));

            txt_vend_name.Enabled = false;
            dataGridView1.Enabled = false;
            txt_bill_amt.Enabled = false;
            txt_discnt.Enabled = false;
            txt_net_amt.Enabled = false;
            btn_upd.Enabled = false;
            btn_edt.Enabled = false;
            btn_rpt.Enabled = true;
            btn_sv.Enabled = false;
            btn_upd.Enabled = false;
            txt_pur_code.Enabled = true; btn_add.Focus();
            clsGeneral.SetAutoCompleteTextBox(txt_vend_name, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 14 and HeaderActCode='20101' Order BY ID DESC"));

        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            txt_pur_code.Text = clsGeneral.getMAXCode("tbl_purchase", "Pur_Code", "PR");
            txt_pur_code.Enabled = false;
            txt_vend_name.Enabled = true;
            dataGridView1.Enabled = true;  txt_bill_amt.Enabled = true;
            txt_discnt.Enabled = true;
            txt_net_amt.Enabled = true;
            dataGridView1.Rows.Add();
            txt_discnt.Text = "0";
            btn_sv.Enabled = true; txt_vend_name.Focus();
            btn_upd.Enabled = false; btn_sv.Enabled = true;
            btn_edt.Enabled = false; btn_add.Enabled = true; txt_vend_name.Text = "";grid2.Rows.Clear();
            txt_bill_amt.Text = "";  txt_discnt.Text = "";   txt_net_amt.Text = ""; txt_discnt.Text = "0";
        }

        private void btn_edt_Click(object sender, EventArgs e)
        {
            txt_pur_code.Enabled = false;
            txt_vend_name.Enabled = true;
            dataGridView1.Enabled = true;
            txt_bill_amt.Enabled = true;
            txt_discnt.Enabled = true;
            txt_net_amt.Enabled = true;
            btn_add.Enabled = true;
            btn_sv.Enabled = false;
            btn_upd.Enabled = true; 
            btn_edt.Enabled = false;   old = Convert.ToDecimal(txt_net_amt.Text);
        }

        private void btn_rpt_Click(object sender, EventArgs e)
        {
            string rptq = "select * from Purchase_View where Pur_Code = '"+txt_pur_code.Text+"' ";
            DataTable dt = clsDataLayer.RetreiveQuery(rptq);
            if(dt.Rows.Count > 0)
            {
                Purchase_report pr = new Purchase_report();
                pr.SetDataSource(dt);
                RepViewer pv = new RepViewer(pr);
                pv.Show();
            }
            else
            {
                MessageBox.Show("No Records Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
        }

        private void btn_clr_Click(object sender, EventArgs e)
        {
            txt_pur_code.Clear();
            txt_vend_name.Clear();
            txt_bill_amt.Clear();
            txt_discnt.Clear();
            txt_net_amt.Clear();
            dataGridView1.Rows.Clear();
            btn_add.Enabled = true;
            txt_pur_code.Enabled = true;
        }

        private void stockmanage()
        {
        for (int a1 = 0; a1 < grid2.Rows.Count; a1++)
        {
        String pd = grid2.Rows[a1].Cells[1].Value.ToString();decimal stockhand = 0;
                decimal inputstock = Convert.ToDecimal(grid2.Rows[a1].Cells[2].Value.ToString());
                String get = "select Quantity from tblStockMaintain where ProductName='" + pd + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(get);
        if (d1.Rows.Count > 0)
        {   stockhand = Convert.ToDecimal(d1.Rows[0][0].ToString());    }

                decimal tblstock = 0;
                String get2 = "select Quantity from tblPurchaseStock where Product_Name='" + pd + "' and RPur_Code='" + txt_pur_code.Text+"'";
                DataTable d12 = clsDataLayer.RetreiveQuery(get2);
                if (d12.Rows.Count > 0)
                { tblstock = Convert.ToDecimal(d12.Rows[0][0].ToString()); }
                decimal final = stockhand - tblstock + inputstock;
                String upd = "update tblStockMaintain set Quantity=" + final + " where ProductName='" + pd + "'"; clsDataLayer.ExecuteQuery(upd);
            }
        }

        decimal balance = 0;
        public decimal PartyBalance()
        {
            try
            {
                String get = "select Party_Balance from LedgerPayment where RefCode='" + Code + "' order by ID desc";
                DataTable dfr = clsDataLayer.RetreiveQuery(get);
                if (dfr.Rows.Count > 0)
                {
                    balance = decimal.Parse(dfr.Rows[0]["Party_Balance"].ToString());
                }
                else
                {
                    balance = 0;
                }

            }
            catch { }
            return balance;

        }

        private void PartyCode()
        {
            String sel = "select ActCode from Accounts where ActTitle = '" + txt_vend_name.Text + "'";
            DataTable dc = clsDataLayer.RetreiveQuery(sel);
            if (dc.Rows.Count > 0)
            {
                Code = dc.Rows[0][0].ToString();
            }
        }
        private void PaymentDue()
        {
            PartyCode();
            String rec = "select * from PaymentDue where PartyCode = '" + Code + "'";
            DataTable d = clsDataLayer.RetreiveQuery(rec);
            if (d.Rows.Count > 0)
            {
                decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
                decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
                decimal received = decimal.Parse(d.Rows[0]["PaidAmount"].ToString());
                decimal bill = decimal.Parse(txt_net_amt.Text);
                total += bill;
                due += bill;
                string updateblnc = "update PaymentDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
                clsDataLayer.ExecuteQuery(updateblnc);
            }
            else
            {
                String ii = @"insert into PaymentDue (PayCode,PartyName,PartyCode,TotalAmount,DueAmount,PaidAmount,Company_Name)
                values('" + txt_pur_code.Text + "','" + txt_vend_name.Text + "','" + Code + "'," + txt_net_amt.Text + "," + txt_net_amt.Text + ",0,'Delizia')";
                clsDataLayer.ExecuteQuery(ii);
            }
        }
        private void SaveClick()
        {
            try
            {
                string s1 = "insert into [dbo].[tbl_purchase] values ('" + txt_pur_code.Text + "','" + txt_vend_name.Text + "','" + txt_bill_amt.Text + "','" + txt_discnt.Text + "','" + txt_net_amt.Text + "','" + Login.UserID + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                clsDataLayer.ExecuteQuery(s1);

                stockmanage();
                for (int a = 0; a < dataGridView1.Rows.Count; a++)
                {
                    String s2 = "insert into [dbo].[tbl_purchase_detail](Pur_Code,Product_Name,QuantityType,Quantity,Sell_Price,Total_Amount,UserName) values ('" + txt_pur_code.Text + "','" + dataGridView1.Rows[a].Cells[0].Value + "','" + dataGridView1.Rows[a].Cells[1].Value + "'," + dataGridView1.Rows[a].Cells[2].Value + ",'" + dataGridView1.Rows[a].Cells[3].Value + "'," + dataGridView1.Rows[a].Cells[4].Value + ",'" + Login.UserID + "')";
                    clsDataLayer.ExecuteQuery(s2);
                }

                for (int a1 = 0; a1 < grid2.Rows.Count; a1++)
                {
  String ins3 = "insert into tblPurchaseStock(RPur_Code,RawProduct,Product_Name,Quantity,UserName)values('" + txt_pur_code.Text + "','" + grid2.Rows[a1].Cells[0].Value + "','" + grid2.Rows[a1].Cells[1].Value + "'," + grid2.Rows[a1].Cells[2].Value + ",'" + Login.UserID + "')";
                    clsDataLayer.ExecuteQuery(ins3);
                }
                PartyCode();
                Decimal a5 = PartyBalance();
                Decimal b = Convert.ToDecimal(txt_net_amt.Text);
                Decimal c = a5 + b;
                String ins = @"insert into LedgerPayment(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                             " Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) values('Purchase','" + txt_pur_code.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','01010204 ','" + Code + "','" + txt_pur_code.Text + "','Purchase'," + txt_net_amt.Text + ",0.00,'OnPayabale ' ," + c + ",'Bamboo')  insert into LedgerPayment(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                             " values('Purchase','" + txt_pur_code.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txt_pur_code.Text + "','" + txt_vend_name.Text + "',0.00," + txt_net_amt.Text + ",'OnPayabale '," + c + ",'Bamboo') ";

                clsDataLayer.ExecuteQuery(ins);
                PaymentDue();
                MessageBox.Show("Saved Successfully !!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btn_sv.Enabled = false;
                txt_pur_code.Enabled = false;
                btn_rpt.Enabled = true; dataGridView1.Enabled = false; txt_vend_name.Enabled = false;
                btn_upd.Enabled = false; btn_sv.Enabled = false;
                btn_edt.Enabled = true; btn_add.Enabled = true;
            }
            catch { }
        }

        //        //same chize do bar nahi ho
        //        waha pe show hojaye
        //save pe jo phele se add nahi hai wo insert hojaye

        private bool CheckAllFields()
        {
            bool flag = false;
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
                if (c is TextBox)
                {
                    if (((TextBox)c).Text == "")
                    {
                        flag = true;
                        break;
                    }
                }
                else if (c is ComboBox)
                {

                    if (((ComboBox)c).Text == "")
                    {
                        flag = true;
                        break;
                    }

                }
            }
            return flag;
        }

        private bool CheckDataGridCells(DataGridView dgv, DataGridView dgv1)
        {
            bool flag = false;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (dgv.Rows[i].Cells[j].Value == null)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag == true)
                {
                    break;
                }
            }

            for (int i = 0; i < dgv1.Rows.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (dgv1.Rows[i].Cells[j].Value == null)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag == true)
                {
                    break;
                }
            }
            return flag;
        }

        private void btn_sv_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckAllFields() && !CheckDataGridCells(dataGridView1, grid2))
                {
                    String getname = "select RemainingLimit,OpeningBalance from Accounts where ActTitle='" + txt_vend_name.Text + "'";
                        DataTable de = clsDataLayer.RetreiveQuery(getname);
                        if (de.Rows.Count < 1)
                        {
                            MessageBox.Show("UserName dont have a chart Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            SaveClick();
                        }
                    //for (int a = 0; a < grid2.Rows.Count; a++)
                    //{
                    //    String prodname = grid2.Rows[a].Cells[0].Value.ToString(); String prodname2 = grid2.Rows[a].Cells[1].Value.ToString();
                    //    String fet = "select ProductName from vsreceipe where RawProductName='" + prodname + "' ProductName='" + prodname2 + "'";
                    //    DataTable d3 = clsDataLayer.RetreiveQuery(fet);
                    //    if (d3.Rows.Count < 1)
                    //    {
                    //        String cc = clsGeneral.getMAXCode("tbl_Receipe", "RCode", "RC");
                    //        String ins1 = "insert into tbl_Receipe(RCode,RawProductName,Dates,UserName)values('" + cc + "','" + prodname + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Login.UserID + "')";
                    //        clsDataLayer.ExecuteQuery(ins1);
                    //        String ins = "insert into tbl_ReceipeDetail(RCode,ProductName,Quantity)values('" + cc + "','" + prodname2 + "'," + grid2.Rows[a].Cells[2].Value.ToString() + ")";
                    //        clsDataLayer.ExecuteQuery(ins);
                    //    }
                    //}
                }
                else
                {
                    MessageBox.Show("Fill All Fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell.ColumnIndex == 0)
                {
                    String Ggrade = dataGridView1.CurrentRow.Cells[0].Value.ToString(); int nn = 0;
                    for (int a = 0; a < dataGridView1.Rows.Count; a++)
                    {
                        String Gpname = dataGridView1.Rows[a].Cells[0].Value.ToString();
                        if (Gpname.Equals(""))
                        {

                        }
                        else if (Ggrade.Equals(Gpname))
                        {
                            nn++;
                        }
                    }
                    if (nn > 1)
                    {
                        MessageBox.Show("Same Product Cant Add Multiple Time", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dataGridView1.CurrentRow.Cells[0].Value = "";
                    }
                    //
                    String pd = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    String query = "select QuantityType,PurchasPrice from tbl_Rawprod where ProductName='" + pd + "'"; DataTable ds = clsDataLayer.RetreiveQuery(query);
                    if (ds.Rows.Count > 0)
                    {
                        dataGridView1.CurrentRow.Cells[1].Value = ds.Rows[0][0].ToString(); dataGridView1.CurrentRow.Cells[3].Value = ds.Rows[0][1].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Product not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dataGridView1.CurrentRow.Cells[0].Value = "";
                    }
                }
                else if (dataGridView1.CurrentCell.ColumnIndex == 2 || dataGridView1.CurrentCell.ColumnIndex == 3)
                {
                    decimal a = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[2].Value);
                    decimal b = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[3].Value);
                    dataGridView1.CurrentRow.Cells[4].Value = a * b;

                    totalgrid();
                }
            }
            catch { }
        }

         private void totalgrid()
        {
            decimal total = 0;
            for (int a = 0; a < dataGridView1.Rows.Count; a++)
            {
                total += Convert.ToDecimal(dataGridView1.Rows[a].Cells[4].Value.ToString());
            }
            txt_bill_amt.Text = total.ToString();
        }

        private void PaymentUpdateDue()
        {
            PartyCode();
            String rec = "select * from PaymentDue where PartyCode = '" + Code + "'";
            DataTable d = clsDataLayer.RetreiveQuery(rec);
            if (d.Rows.Count > 0)
            {
                decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
                decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
                decimal received = decimal.Parse(d.Rows[0]["PaidAmount"].ToString());
                decimal bill = decimal.Parse(txt_net_amt.Text);
                total = (total - old) + bill;
                due = (due - old) + bill;

                string updateblnc = "update PaymentDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
                clsDataLayer.ExecuteQuery(updateblnc);
            }
            else
            {
                String ii = @"insert into PaymentDue (PayCode,PartyName,PartyCode,TotalAmount,DueAmount,PaidAmount,Company_Name)
                values('" + txt_pur_code.Text + "','" + txt_vend_name.Text + "','" + Code + "'," + txt_net_amt.Text + "," + txt_net_amt.Text + ",0,'Bamboo')";
                clsDataLayer.ExecuteQuery(ii);
            }
        }

        private void btn_upd_Click(object sender, EventArgs e)
         {
            if (!CheckAllFields() && !CheckDataGridCells(dataGridView1, grid2))
            {
                for (int a = 0; a < grid2.Rows.Count; a++)
                {
                    String prodname = grid2.Rows[a].Cells[0].Value.ToString(); String prodname2 = grid2.Rows[a].Cells[1].Value.ToString();
                    String fet = "select ProductName from vsreceipe where RawProductName='" + prodname + "' and ProductName='" + prodname2 + "'";
                    DataTable d3 = clsDataLayer.RetreiveQuery(fet);
                    if (d3.Rows.Count < 1)
                    {
                        String cc = clsGeneral.getMAXCode("tbl_Receipe", "RCode", "RC");
                        String ins1 = "insert into tbl_Receipe(RCode,RawProductName,Dates,UserName)values('" + cc + "','" + prodname + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Login.UserID + "')";
                        clsDataLayer.ExecuteQuery(ins1);
                        String ins = "insert into tbl_ReceipeDetail(RCode,ProductName,Quantity)values('" + cc + "','" + prodname2 + "'," + grid2.Rows[a].Cells[2].Value.ToString() + ")";
                        clsDataLayer.ExecuteQuery(ins);
                    }
                }
                txt_pur_code.Enabled = false;
                btn_rpt.Enabled = true;
                stockmanage();

                string del1 = "delete from [dbo].[tbl_purchase] where Pur_Code = '" + txt_pur_code.Text + "' ";
                clsDataLayer.ExecuteQuery(del1);

                string del2 = "delete from [dbo].[tbl_purchase_detail] where Pur_Code = '" + txt_pur_code.Text + "' delete from tblPurchaseStock where RPur_Code = '" + txt_pur_code.Text + "'";
                clsDataLayer.ExecuteQuery(del2);



                string s1 = "insert into [dbo].[tbl_purchase] values ('" + txt_pur_code.Text + "','" + txt_vend_name.Text + "','" + txt_bill_amt.Text + "','" + txt_discnt.Text + "','" + txt_net_amt.Text + "','" + Login.UserID + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                clsDataLayer.ExecuteQuery(s1);

                for (int a = 0; a < dataGridView1.Rows.Count; a++)
                {
                    String s2 = "insert into [dbo].[tbl_purchase_detail](Pur_Code,Product_Name,QuantityType,Quantity,Sell_Price,Total_Amount,UserName) values ('" + txt_pur_code.Text + "','" + dataGridView1.Rows[a].Cells[0].Value + "','" + dataGridView1.Rows[a].Cells[1].Value + "'," + dataGridView1.Rows[a].Cells[2].Value + ",'" + dataGridView1.Rows[a].Cells[3].Value + "'," + dataGridView1.Rows[a].Cells[4].Value + ",'" + Login.UserID + "')";
                    clsDataLayer.ExecuteQuery(s2);
                }
                for (int a1 = 0; a1 < grid2.Rows.Count; a1++)
                {
                    String ins3 = "insert into tblPurchaseStock(RPur_Code,RawProduct,Product_Name,Quantity,UserName)values('" + txt_pur_code.Text + "','" + grid2.Rows[a1].Cells[0].Value + "','" + grid2.Rows[a1].Cells[1].Value + "'," + grid2.Rows[a1].Cells[2].Value + ",'" + Login.UserID + "')";
                    clsDataLayer.ExecuteQuery(ins3);
                }
                PartyCode();
                PaymentUpdateDue();
                int index = GetRowIndex(txt_pur_code.Text);
                DataTable dt = SetDT();
                ShowDt(dt);
                Console.WriteLine("Index=" + index);
                DeleteRecord();
                dt.Rows[index][7] = Convert.ToDecimal(txt_net_amt.Text);
                dt.Rows[index + 1][8] = Convert.ToDecimal(txt_net_amt.Text);
                dt = SomeOperation(dt); InsertRecord(dt);
                MessageBox.Show("Update Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btn_sv.Enabled = false;
                txt_pur_code.Enabled = false;
                btn_rpt.Enabled = true;
                //
                btn_upd.Enabled = false; btn_sv.Enabled = false;
                btn_edt.Enabled = true; btn_add.Enabled = true; 
            }
            else
            {
                MessageBox.Show("Fill All Fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);

        //Ledger Update
        #region LedgerUpdate
        private int GetRowIndex(string input)
        {
            int index = 0;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT V_No FROM LedgerPayment where RefCode = '" + Code + "' order by ID asc", con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetValue(0).ToString() == input)
                    {
                        break;
                    }
                    index++;
                }
                con.Close();
            }
            catch { }
            return index;
        }
        private void DeleteRecord()
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM LedgerPayment where RefCode = '" + Code + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch { }
        }
        private DataTable SetDT()
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlDataAdapter cmd = new SqlDataAdapter("select V_No, Datetime, AccountCode, RefCode, PaymentType, InvoiceNo, Particulars, Debit, Credit, Party_Balance,Company_Name,Description from LedgerPayment where RefCode = '" + Code + "' order by ID asc", con);
                cmd.Fill(dt);
                con.Close();
            }
            catch { }
            return dt;
        }
        private DataTable SomeOperation(DataTable dt)
        {
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Console.WriteLine("Payment Type" + dt.Rows[i].ItemArray[4]);
                    String type = "";
                    type = dt.Rows[i].ItemArray[4].ToString();
                    if (type.Equals("OnCredit"))
                    {
                        Console.WriteLine("True");
                    }
                    else
                    {
                        Console.WriteLine("False");
                    }
                    if (i == 0)
                    {
                        dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[7]);
                    }
                    else if (i == 1)
                    {
                        dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[8]);
                    }
                    else
                    {
                        if (type.Equals("OnPayabale"))
                        {
                            dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[7]) + Convert.ToDecimal(dt.Rows[i - 1].ItemArray[9]);
                        }
                        else
                        {
                            dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i - 1].ItemArray[9]) - Convert.ToDecimal(dt.Rows[i].ItemArray[7]);
                        }
                    }
                }
            }
            catch { }
            return dt;
        }
        private void InsertRecord(DataTable dt)
        {
            try
            {
                con.Open();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i].ItemArray[8].ToString().Equals(""))
                    {
                        dt.Rows[i].ItemArray[8] = "0.00";
                    }
                    else
                    {
                        dt.Rows[i].ItemArray[8].ToString();
                    }
                    String ins = "INSERT INTO LedgerPayment VALUES('" + dt.Rows[i].ItemArray[0].ToString() + "','" + dt.Rows[i].ItemArray[1].ToString() + "','" + dt.Rows[i].ItemArray[2].ToString() + "','" + dt.Rows[i].ItemArray[3].ToString() + "','" + dt.Rows[i].ItemArray[4].ToString() + "','" + dt.Rows[i].ItemArray[5].ToString() + "','" + dt.Rows[i].ItemArray[6].ToString() + "'," + dt.Rows[i].ItemArray[7].ToString() + "," + dt.Rows[i].ItemArray[8].ToString() + "," + dt.Rows[i].ItemArray[9].ToString() + ",'" + dt.Rows[i].ItemArray[10].ToString() + "','" + dt.Rows[i].ItemArray[11].ToString() + "')";
                    SqlCommand cmd = new SqlCommand(ins, con); cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            catch { }
        }
        private void ShowDt(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Console.Write("  " + dt.Rows[i].ItemArray[j].ToString());
                }
                Console.WriteLine();
            }
        }
        #endregion LedgerUpdate





        private void txt_pur_code_TextChanged(object sender, EventArgs e)
         {
             btn_edt.Enabled = true;
             try
             {
                 dataGridView1.Rows.Clear();
                 grid2.Rows.Clear();
                 string sel = "SELECT [Pur_Code],[VenderName],[BillAmount],[Discount],[NetAmount],[UserName],[Date],QuantityType,[Product_Name],[Quantity],[Sell_Price],[Total_Amount] FROM [dbo].[Purchase_View] where Pur_Code = '" + txt_pur_code.Text + "' ";
                 DataTable dt = clsDataLayer.RetreiveQuery(sel);
                 if (dt.Rows.Count > 0)
                 {
                 for (int a = 0; a < dt.Rows.Count; a++)
                 {
                     txt_vend_name.Text = dt.Rows[a][1].ToString(); 
                     txt_bill_amt.Text = dt.Rows[a][2].ToString();
                     txt_discnt.Text = dt.Rows[a][3].ToString();
                     txt_net_amt.Text = dt.Rows[a][4].ToString();
                 }

                 foreach (DataRow row in dt.Rows)
                 {
                     int n = dataGridView1.Rows.Add();
                     dataGridView1.Rows[n].Cells[0].Value = row["Product_Name"].ToString();
                     dataGridView1.Rows[n].Cells[1].Value = row["QuantityType"].ToString();
                     dataGridView1.Rows[n].Cells[2].Value = row["Quantity"].ToString();
                     dataGridView1.Rows[n].Cells[3].Value = row["Sell_Price"].ToString();
                        dataGridView1.Rows[n].Cells[4].Value = row["Total_Amount"].ToString();
                    }
                 }

                String get = "select RawProduct,Product_Name,Quantity from tblPurchaseStock where RPur_Code='" + txt_pur_code.Text+"'";
                DataTable d1 = clsDataLayer.RetreiveQuery(get);
                if(d1.Rows.Count > 0)
                {
                    foreach (DataRow row in d1.Rows)
                    {
                        int n = grid2.Rows.Add();
                        grid2.Rows[n].Cells[0].Value = row["RawProduct"].ToString();
                        grid2.Rows[n].Cells[1].Value = row["Product_Name"].ToString();
                        grid2.Rows[n].Cells[2].Value = row["Quantity"].ToString(); 
                    }
                }

             }
             catch { }
         }

         private void txt_discnt_TextChanged(object sender, EventArgs e)
         {
             try
             {
                 decimal bill = Convert.ToDecimal(txt_bill_amt.Text);
                 decimal dis = Convert.ToDecimal(txt_discnt.Text);
                 decimal net = bill - dis;
                 txt_net_amt.Text = net.ToString();
             }
             catch 
             { }
         }

         private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
         {
             if (dataGridView1.CurrentCell.ColumnIndex == 3 || dataGridView1.CurrentCell.ColumnIndex == 4)
             {
                 if (e.KeyCode == Keys.Enter)
                 {
                     int y2 = dataGridView1.CurrentCell.RowIndex;
                     dataGridView1.Rows.Add();
                     int yhs = dataGridView1.CurrentCell.RowIndex;
                     dataGridView1.CurrentCell = dataGridView1.Rows[yhs].Cells[0]; 
                     dataGridView1.BeginEdit(true);

                 }
             }
             else
             {
                 if (e.KeyCode == Keys.Delete)
                 {
                     if (dataGridView1.Rows.Count > 0)
                     {
                         int yh = dataGridView1.CurrentCell.RowIndex;
                         dataGridView1.Rows.RemoveAt(yh);
                     }
                 }
             }
         }

         private void button1_Click(object sender, EventArgs e)
         {
             Pur_Vchr_Srch ps = new Pur_Vchr_Srch(this);
             ps.Show();
         }

         private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
         {
             if (dataGridView1.CurrentCell.ColumnIndex == 0)
             {
                 int yy = dataGridView1.CurrentCell.ColumnIndex;
                 string columnHeaders = dataGridView1.Columns[yy].HeaderText;
                 if (columnHeaders.Equals("Raw Product"))
                 {
                     tb = e.Control as TextBox;
                     if (tb != null)
                     { 
                        String hname = "select ProductName from tbl_Rawprod"; DataTable df = clsDataLayer.RetreiveQuery(hname);
                         if (df.Rows.Count > 0)
                         {
                             AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();

                             foreach (DataRow row in df.Rows)
                             {
                                 acsc.Add(row[0].ToString());
                             }

                             tb.AutoCompleteCustomSource = acsc;
                             tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                             tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                         }
                     }
                 }
             }
            TextBox tb2 = null; tb2 = e.Control as TextBox;
            if (tb2 != null) { tb2.KeyPress += new KeyPressEventHandler(tb_KeyPress2); }
        }

        private void tb_KeyPress2(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell.ColumnIndex != 0 && dataGridView1.CurrentCell.ColumnIndex != 1) //Desired Column
                {
                    if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.'))
                    { e.Handled = true; }
                    TextBox txtDecimal = sender as TextBox;
                    if (e.KeyChar == '.' && txtDecimal.Text.Contains("."))
                    {
                        e.Handled = true;
                    }
                }
            }
            catch { }
        }

        private void txt_discnt_KeyDown(object sender, KeyEventArgs e)
         {
             if (e.KeyCode == Keys.Enter)
             {
                 SaveClick();
             }
         }

  

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                grid2.Rows.Clear();
               for(int a=0;a<dataGridView1.Rows.Count;a++)
                {
             String prodname = dataGridView1.Rows[a].Cells[0].Value.ToString();
                    decimal input =Convert.ToDecimal(dataGridView1.Rows[a].Cells[2].Value.ToString());
                    String fet = "select ProductName,Quantity from vsreceipe where RawProductName='"+prodname+"'";
                    DataTable d1 = clsDataLayer.RetreiveQuery(fet);
                    if(d1.Rows.Count > 0)
                    {
                    foreach(DataRow dr in d1.Rows)
                        {
           int n = grid2.Rows.Add(); grid2.Rows[n].Cells[0].Value = prodname;
                            grid2.Rows[n].Cells[1].Value = dr["ProductName"];
                            decimal db = Convert.ToDecimal(dr["Quantity"]);decimal final = db * input;
                            grid2.Rows[n].Cells[2].Value = final.ToString();
                        }
                    }
                }

            }
            catch { }
        }

        private void grid2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //if (grid2.CurrentCell.ColumnIndex == 2 || grid2.CurrentCell.ColumnIndex == 1 || grid2.CurrentCell.ColumnIndex == 0)
                //{
                //    if (e.KeyCode == Keys.Enter)
                //    {
                //        // same = 1;
                //        grid2.Rows.Add();
                //        int yhs = grid2.CurrentCell.RowIndex;
                //        grid2.CurrentCell = grid2.Rows[yhs].Cells[0];
                //        grid2.BeginEdit(true);
                //    }
                //    else if (e.KeyCode == Keys.Delete)
                //    {
                //        if (grid2.Rows.Count > 0)
                //        {
                //            int yh = grid2.CurrentCell.RowIndex;
                //            grid2.Rows.RemoveAt(yh);
                //        }
                //    }
                //}
                //else
                //{
                //    if (e.KeyCode == Keys.Delete)
                //    {
                //        if (grid2.Rows.Count > 0)
                //        {
                //            int yh = grid2.CurrentCell.RowIndex;
                //            grid2.Rows.RemoveAt(yh);
                //        }
                //    }
                //}
            }
            catch { }
        }

        private void grid2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (grid2.CurrentCell.ColumnIndex == 1)
            {
                int yy = grid2.CurrentCell.ColumnIndex;
                string columnHeaders = grid2.Columns[yy].HeaderText;
                if (columnHeaders.Equals("Kitchen Product"))
                {
                    tb = e.Control as TextBox;
                    if (tb != null)
                    {
                        String get = "select distinct ProductName from vsreceipe"; DataTable df = clsDataLayer.RetreiveQuery(get);
                        if (df.Rows.Count > 0)
                        {
                            AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
                            foreach (DataRow row in df.Rows) { acsc.Add(row[0].ToString()); }
                            tb.AutoCompleteCustomSource = acsc;
                            tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                            tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                    }
                }
            }
            else if (grid2.CurrentCell.ColumnIndex == 0)
            {
                int yy = grid2.CurrentCell.ColumnIndex;
                string columnHeaders = grid2.Columns[yy].HeaderText;
                if (columnHeaders.Equals("RawProduct"))
                {
                    tb = e.Control as TextBox;
                    if (tb != null)
                    {
                        String hname = "select ProductName from tbl_Rawprod"; DataTable df = clsDataLayer.RetreiveQuery(hname);
                        if (df.Rows.Count > 0)
                        {
                            AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();

                            foreach (DataRow row in df.Rows)
                            {
                                acsc.Add(row[0].ToString());
                            }

                            tb.AutoCompleteCustomSource = acsc;
                            tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                            tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                    }
                }
            }
            TextBox tb2 = null; tb2 = e.Control as TextBox;
            if (tb2 != null) { tb2.KeyPress += new KeyPressEventHandler(tb_KeyPress); }
        }

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (grid2.CurrentCell.ColumnIndex == 2) //Desired Column
                {
                    if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.'))
                    { e.Handled = true; }
                    TextBox txtDecimal = sender as TextBox;
                    if (e.KeyChar == '.' && txtDecimal.Text.Contains("."))
                    {
                        e.Handled = true;
                    }
                }
            }
            catch { }
        }

        private void grid2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (grid2.CurrentCell.ColumnIndex == 1)
                {
                    String Ggrade = grid2.CurrentRow.Cells[1].Value.ToString(); int nn = 0;
                    for (int a = 0; a < grid2.Rows.Count; a++)
                    {
                        String Gpname = grid2.Rows[a].Cells[1].Value.ToString();
                        if (Gpname.Equals(""))
                        {

                        }
                        else if (Ggrade.Equals(Gpname))
                        {
                            nn++;
                        }
                    }
                    if (nn > 1)
                    {
                        MessageBox.Show("Same Product Cant Add Multiple Time", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        grid2.CurrentRow.Cells[1].Value = "";
                    }
                }
                else if (grid2.CurrentCell.ColumnIndex == 0)
                { 
                String hname = "select ProductName from tbl_Rawprod where ProductName='"+grid2.CurrentRow.Cells[0].Value.ToString()+"'"; DataTable df = clsDataLayer.RetreiveQuery(hname);
                if (df.Rows.Count > 0)
                { }
                    else {
                        MessageBox.Show("Product not available!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        grid2.CurrentRow.Cells[0].Value = "";
                    }
                }


                }
            catch { }
        }

        private void txt_vend_name_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
