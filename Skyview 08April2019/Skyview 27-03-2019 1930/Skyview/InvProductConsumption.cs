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
    public partial class InvProductConsumption : Form
    {
        TextBox tb = new TextBox();
        String btnstatus = "";
        public InvProductConsumption()
        {
            InitializeComponent(); refresh();
            String h3 = "select RCode from tbl_Consumption";
            DataTable d3 = clsDataLayer.RetreiveQuery(h3);
            if (d3.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(txt_search, d3);
            }

            btnEdit.Enabled = false;
            btnUpdate.Enabled = false;
            btnsave.Enabled = false;
            grid.Enabled = false;
            listBox1.Enabled = false;
            txt_cat.Enabled = false;
            chk_deal.Enabled = false;
            String quer = "select Categories_Name from product_categories where Categories_Name != 'DEALS'";
            DataTable ds = clsDataLayer.RetreiveQuery(quer);
            if (ds.Rows.Count > 0)
            { clsGeneral.SetAutoCompleteTextBox(txt_cat, ds); }

        }

        private void New_Click(object sender, EventArgs e)
        {
            grid.Enabled = true; btnsave.Enabled = true; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = false;
            grid.Rows.Clear();
            grid.Rows.Add();
            grid.Enabled = true;
            txtcode.Text = clsGeneral.getMAXCode("tbl_Consumption", "RCode", "RC"); txt_search.Text = "-";
            //String get = "select Product_Name from tbl_ProductBySale"; DataTable d2 = clsDataLayer.RetreiveQuery(get);
            //if (d2.Rows.Count > 0)
            //{
            //    clsGeneral.SetAutoCompleteTextBox(txtrawprod, d2);
            //}
            btnstatus = "new";
            //txtrawprod.Enabled = true;
           
            listBox1.Enabled = true;
            txt_cat.Enabled = true;
            chk_deal.Enabled = true;

        }

        private void grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (grid.CurrentCell.ColumnIndex == 1)
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        // same = 1;
                        grid.Rows.Add();
                        int yhs = grid.CurrentCell.RowIndex;
                        grid.CurrentCell = grid.Rows[yhs].Cells[0];
                        grid.BeginEdit(true);
                    }
                    else if (e.KeyCode == Keys.Delete)
                    {
                        if (grid.Rows.Count > 0)
                        {
                            int yh = grid.CurrentCell.RowIndex;
                            grid.Rows.RemoveAt(yh);
                        }
                    }
                }
                else
                {
                    if (e.KeyCode == Keys.Delete)
                    {
                        if (grid.Rows.Count > 0)
                        {
                            int yh = grid.CurrentCell.RowIndex;
                            grid.Rows.RemoveAt(yh);
                        }
                    }
                }
            }
            catch { }
        }

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

        private bool CheckDataGridCells(DataGridView dgv)
        {
            bool flag = false;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                for (int j = 0; j < 1; j++)
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
            return flag;
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                bool cm = false;
                txt_cat.Text = "-";

                for (int a1 = 0; a1 < mgrid.Rows.Count; a1++)
                {
                    String sel = "select * from tbl_Consumption where SaleProductName='" + mgrid.Rows[a1].Cells[0].Value.ToString() + "'";
                    DataTable gb = clsDataLayer.RetreiveQuery(sel);
                    if (gb.Rows.Count > 0)
                    {
                        cm = true;
                    }
                }

                if (cm == true)
                {
                    MessageBox.Show("Some Sale Product Already add!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    btnstatus = "save";
                    Save();
                }
            }
            catch { }
        }

        private void Save()
        {
            if (!CheckAllFields() && !CheckDataGridCells(grid))
            {
                for (int a1 = 0; a1 < mgrid.Rows.Count; a1++)
                {
                    String ins3 = "insert into tbl_Consumption(RCode,SaleProductName,Dates,UserName)values('" + txtcode.Text + "','" + mgrid.Rows[a1].Cells[0].Value.ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Login.UserID + "')";
                    clsDataLayer.ExecuteQuery(ins3);

                }

                for (int a = 0; a < grid.Rows.Count; a++)
                {
                    String ins = "insert into tbl_ConsumptionDetail(RCode,ReceipeProductName,Quantity)values('" + txtcode.Text + "','" + grid.Rows[a].Cells[0].Value.ToString() + "'," + grid.Rows[a].Cells[1].Value.ToString() + ")";
                    clsDataLayer.ExecuteQuery(ins);

                    String pd = grid.Rows[a].Cells[0].Value.ToString();
                    String he = "select Quantity from tblStockMaintain where ProductName='" + pd + "'"; DataTable d3 = clsDataLayer.RetreiveQuery(he);
                    if (d3.Rows.Count > 0)
                    {
                        decimal qs = Convert.ToDecimal(d3.Rows[0][0].ToString());
                        if (qs == 0)
                        {
                            String del3 = "delete from tblStockMaintain where ProductName='" + pd + "'"; clsDataLayer.ExecuteQuery(del3);
                            String ins2 = "insert into tblStockMaintain(RCode,SaleProductName,ProductName,Quantity,Dates,UserName)values('" + txtcode.Text + "','','" + grid.Rows[a].Cells[0].Value.ToString() + "',0,'" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Login.UserID + "')";
                            clsDataLayer.ExecuteQuery(ins2);
                        }
                    }
                    else
                    {
                        String ins23 = "insert into tblStockMaintain(RCode,SaleProductName,ProductName,Quantity,Dates,UserName)values('" + txtcode.Text + "','','" + grid.Rows[a].Cells[0].Value.ToString() + "',0,'" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Login.UserID + "')";
                        clsDataLayer.ExecuteQuery(ins23);
                    }

                }

                MessageBox.Show("Product Save Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.Refresh();
                grid.Enabled = false; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = true;
                refresh(); txt_cat.Clear(); listBox1.Items.Clear(); mgrid.Rows.Clear();
            }
            else
            {
                MessageBox.Show("Fill All Fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        bool abc = false;
        private void CheckCategories()
        {
            abc = false;
            for (int a = 0; a < grid.Rows.Count; a++)
            {
                String quwe = "SELECT Categories_Name from product_categories where Categories_Name='" + grid.Rows[a].Cells[0].Value + "'";
                DataTable ds = clsDataLayer.RetreiveQuery(quwe);
                if (ds.Rows.Count > 0)
                {

                }
                else { abc = true; }
            }
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckAllFields() && !CheckDataGridCells(grid))
            {
                
                bool cm = false;
                txt_cat.Text = "-";

                for (int a1 = 0; a1 < mgrid.Rows.Count; a1++)
                {
                    String sel = "select * from tbl_Consumption where SaleProductName='" + mgrid.Rows[a1].Cells[0].Value.ToString() + "' and RCode != '"+txtcode.Text+"'";
                    DataTable gb = clsDataLayer.RetreiveQuery(sel);
                    if (gb.Rows.Count > 0)
                    {
                        cm = true;
                    }
                }

                if (cm == true)
                {
                    MessageBox.Show("Some Sale Product Already add!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    String del = "delete from tbl_Consumption where RCode='" + txtcode.Text + "' delete from tbl_ConsumptionDetail where RCode='" + txtcode.Text + "'"; clsDataLayer.ExecuteQuery(del);

                    btnstatus = "save";
                    Save();

                    grid.Enabled = false; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = true;
                    btnstatus = "update";
                    txt_cat.Clear(); listBox1.Items.Clear(); mgrid.Rows.Clear();
                }

            }
            else
            {
                MessageBox.Show("Fill All Fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            grid.Enabled = true; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = true; New.Enabled = false;
            btnstatus = "edit"; mgrid.Enabled = true; listBox1.Enabled = true; chk_deal.Enabled = true; txt_cat.Enabled = true;
            //txtrawprod.Enabled = true;
        }

        private void grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (grid.CurrentCell.ColumnIndex == 0)
            {
                int yy = grid.CurrentCell.ColumnIndex;
                string columnHeaders = grid.Columns[yy].HeaderText;
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
            TextBox tb2 = null; tb2 = e.Control as TextBox;
            if (tb2 != null) { tb2.KeyPress += new KeyPressEventHandler(tb_KeyPress); }
        }

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (grid.CurrentCell.ColumnIndex != 0) //Desired Column
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

        private void grid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (grid.CurrentCell.ColumnIndex == 1)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                { e.Handled = true; }
            }
        }

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (txt_search.Text == "")
            {
                refresh();
            }
            else
            {

                SqlDataAdapter sda = new SqlDataAdapter("SELECT distinct(RCode) FROM tbl_Consumption Where RCode = '" + txt_search.Text + "'   ", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = null; dataGridView1.DataSource = dt;
                }
            }

        }

        public void refresh()
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT distinct(RCode) FROM tbl_Consumption", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnsave.Enabled = false;
            btnEdit.Enabled = true;
            btnUpdate.Enabled = false;
            New.Enabled = false;
            txt_search.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            SqlDataAdapter sda2 = new SqlDataAdapter("SELECT SaleProductName FROM tbl_Consumption Where RCode ='" + txt_search.Text + "'", con);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            if (dt2.Rows.Count > 0)
            {
                mgrid.Rows.Clear();
                foreach (DataRow dr in dt2.Rows)
                {
                    int n1 = mgrid.Rows.Add();
                    mgrid.Rows[n1].Cells[0].Value = dr[0].ToString();
                }
                //txtrawprod.Text = dt2.Rows[0][0].ToString();

            }
            txtcode.Text = txt_search.Text;
            SqlDataAdapter sda = new SqlDataAdapter("SELECT RCode,ReceipeProductName,Quantity FROM tbl_ConsumptionDetail Where RCode ='" + txt_search.Text + "'", con);
            DataTable dt1 = new DataTable();
            sda.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                grid.Rows.Clear();
                foreach (DataRow row in dt1.Rows)
                {
                    int n = grid.Rows.Add();
                    grid.Rows[n].Cells[0].Value = row[1].ToString();
                    grid.Rows[n].Cells[1].Value = row[2].ToString();
                }
                String gb = "SELECT distinct(RCode) FROM tbl_Consumption";
                DataTable dt = clsDataLayer.RetreiveQuery(gb);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = null; dataGridView1.DataSource = dt;
                }
            }

        }

        //private void txtrawprod_Leave(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (txtrawprod.Text.Length > 0)
        //        {

        //            if (btnstatus.Equals("new") || btnstatus.Equals("edit"))
        //            {
        //                int check = 0;
        //                String sel = "select * from tbl_Consumption where SaleProductName='" + txtrawprod.Text + "'";
        //                DataTable gb = clsDataLayer.RetreiveQuery(sel);
        //                if (gb.Rows.Count > 0)
        //                {
        //                    MessageBox.Show("Product is already available in data !", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); txtrawprod.Text = "";
        //                }

        //                String get = "select Product_Name from tbl_ProductBySale where Product_Name='" + txtrawprod.Text + "'"; DataTable d2 = clsDataLayer.RetreiveQuery(get);
        //                if (d2.Rows.Count > 0)
        //                {
        //                    check = 1;
        //                }
        //                else
        //                {
        //                    String get2 = "select Product_Name from tbl_deals where Product_Name='" + txtrawprod.Text + "'";
        //                    DataTable d22 = clsDataLayer.RetreiveQuery(get2);
        //                    if (d22.Rows.Count > 0)
        //                    {
        //                        check = 1;
        //                    }

        //                }
        //                if (check == 0)
        //                {
        //                    MessageBox.Show("Product is not available in data !", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); txtrawprod.Text = "";
        //                }
        //            }
        //        }
        //    }
        //    catch { }
        //}

        private void grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (grid.CurrentCell.ColumnIndex == 0)
                {
                    String get = "select distinct ProductName from vsreceipe where ProductName='" + grid.CurrentRow.Cells[0].Value.ToString() + "'"; DataTable df = clsDataLayer.RetreiveQuery(get);
                    if (df.Rows.Count < 1)
                    {
                        MessageBox.Show("Product is not available in data !", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); grid.CurrentRow.Cells[0].Value = "";
                    }
                    String Ggrade = grid.CurrentRow.Cells[0].Value.ToString(); int nn = 0;
                    for (int a = 0; a < grid.Rows.Count; a++)
                    {
                        String Gpname = grid.Rows[a].Cells[0].Value.ToString();
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
                        grid.CurrentRow.Cells[0].Value = "";
                    }

                }
            }
            catch { }
        }

        private void btnprint_Click(object sender, EventArgs e)
        {
            try
            {
                string q = "select * from vwconsumption where RCode='" + txtcode.Text + "'";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    rptProductconsumption pr = new rptProductconsumption();
                    pr.SetDataSource(purchase);
                    RepViewer pv = new RepViewer(pr);
                    pv.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            { }
        }

        private void chk_deals_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                sorting();
            }
            catch
            {


            }
        }

        public void sorting()
        {
            txt_cat.Focus();
            try
            {
                if (chk_deal.Checked == false)
                {

                    listBox1.Items.Clear();
                    String get = "select Product_Name from tbl_ProductBySale where Product_Categories = '" + txt_cat.Text + "'";

                    DataTable ds = clsDataLayer.RetreiveQuery(get);
                    if (ds.Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Rows.Count; i++)
                        {
                            listBox1.Items.Add(ds.Rows[i][0].ToString());
                        }
                    }
                }
                else
                {
                    listBox1.Items.Clear();
                    String get = "select distinct(Product_Name) from [tbl_deals] where Product_Categories = '" + txt_cat.Text + "'";

                    DataTable ds = clsDataLayer.RetreiveQuery(get);
                    if (ds.Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Rows.Count; i++)
                        {
                            listBox1.Items.Add(ds.Rows[i][0].ToString());
                        }
                    }

                }
            }
            catch { }
        }

        private void InvProductConsumption_Load(object sender, EventArgs e)
        {

        }

        private void mgrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (mgrid.CurrentCell.ColumnIndex == 0)
            {
                int yy = mgrid.CurrentCell.ColumnIndex;
                string columnHeaders = mgrid.Columns[yy].HeaderText;
                if (columnHeaders.Equals("Sale Product"))
                {
                    tb = e.Control as TextBox;
                    if (tb != null)
                    {
                        String get = "select distinct Product_Name from tbl_ProductBySale"; DataTable df = clsDataLayer.RetreiveQuery(get);
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
        }

        private void mgrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (mgrid.CurrentCell.ColumnIndex == 0)
                {
                    bool cm = false;

                    for (int a1 = 0; a1 < mgrid.Rows.Count; a1++)
                    {
                        String sel = "select * from tbl_Consumption where SaleProductName='" + mgrid.Rows[a1].Cells[0].Value.ToString() + "'";
                        DataTable gb = clsDataLayer.RetreiveQuery(sel);
                        if (gb.Rows.Count > 0)
                        {
                            cm = true;
                        }
                    }

                    if (cm == true)
                    {
                        MessageBox.Show("Sale Product Already add in Data!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        String Ggrade = mgrid.CurrentRow.Cells[0].Value.ToString(); int nn = 0;
                        for (int a = 0; a < mgrid.Rows.Count; a++)
                        {
                            String Gpname = mgrid.Rows[a].Cells[0].Value.ToString();
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
                            mgrid.CurrentRow.Cells[0].Value = "";
                        }
                        else
                        {
                            String get = "select distinct Product_Name from tbl_ProductBySale where Product_Name='" + mgrid.CurrentRow.Cells[0].Value.ToString() + "'"; DataTable df = clsDataLayer.RetreiveQuery(get);
                            if (df.Rows.Count > 0)
                            {

                            }
                            else
                            {
                                mgrid.CurrentRow.Cells[0].Value = ""; MessageBox.Show("Product not found!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void mgrid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (mgrid.CurrentCell.ColumnIndex == 0)
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        // same = 1;
                        mgrid.Rows.Add();
                        int yhs = mgrid.CurrentCell.RowIndex;
                        mgrid.CurrentCell = mgrid.Rows[yhs].Cells[0];
                        mgrid.BeginEdit(true);
                    }
                    else if (e.KeyCode == Keys.Delete)
                    {
                        if (mgrid.Rows.Count > 0)
                        {
                            int yh = mgrid.CurrentCell.RowIndex;
                            mgrid.Rows.RemoveAt(yh);
                        }
                    }
                }
                else
                {
                    if (e.KeyCode == Keys.Delete)
                    {
                        if (mgrid.Rows.Count > 0)
                        {
                            int yh = mgrid.CurrentCell.RowIndex;
                            mgrid.Rows.RemoveAt(yh);
                        }
                    }
                }
            }
            catch { }
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {

                for (int i = 0; i < listBox1.SelectedItems.Count; i++)
                {
                    string a = listBox1.SelectedItems[i].ToString();

                    string it = a;
                    int chk = 0;
                    for (int j = 0; j < mgrid.Rows.Count; j++)
                    {
                        string li_item = mgrid.Rows[j].Cells[0].Value.ToString();

                        if (li_item == it)
                        {
                            MessageBox.Show("ALREADY ADD");
                            chk = 1;
                        }

                    }
                    if (chk != 1)
                    {
                        int n = mgrid.Rows.Add();
                        mgrid.Rows[n].Cells[0].Value = it.ToString();
                    }
                }
            }
            catch
            {


            }
        }

        private void txt_cat_TextChanged(object sender, EventArgs e)
        {
            sorting();
        }

    }
}
