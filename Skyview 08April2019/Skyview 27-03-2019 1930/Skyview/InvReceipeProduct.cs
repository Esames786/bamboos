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
    public partial class InvReceipeProduct : Form
    {
        String btnstatus = "";
        TextBox tb = new TextBox();
        public InvReceipeProduct()
        {
            InitializeComponent();
            refresh();

            String h3 = "select RCode from tbl_Receipe";
            DataTable d3 = clsDataLayer.RetreiveQuery(h3);
            if (d3.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(txt_search, d3);
            }
            String ck = "select ProductName from tbl_Rawprod";
            DataTable d1 = clsDataLayer.RetreiveQuery(ck);
            if (d1.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(txtrawprod, d1);
            }
            btnEdit.Enabled = false; btnUpdate.Enabled = false;
            btnsave.Enabled = false; grid.Enabled = false; txtrawprod.Enabled = false;
        }

        private void New_Click(object sender, EventArgs e)
        {
            grid.Enabled = true; btnsave.Enabled = true; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = false;
            grid.Rows.Clear();
            grid.Rows.Add();
            grid.Enabled = true;
            txtcode.Text = clsGeneral.getMAXCode("tbl_Receipe", "RCode", "RC"); txt_search.Text = "-";
            String ck = "select ProductName from tbl_Rawprod";
            DataTable d1 = clsDataLayer.RetreiveQuery(ck);
            if (d1.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(txtrawprod, d1);
            }
            btnstatus = "new"; txtrawprod.Enabled = true;
        }

        private void grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (grid.CurrentCell.ColumnIndex == 1 || grid.CurrentCell.ColumnIndex == 0)
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

        private void Save()
        {
            if (!CheckAllFields() && !CheckDataGridCells(grid))
            {
                String ins1 = "insert into tbl_Receipe(RCode,RawProductName,Dates,UserName)values('" + txtcode.Text + "','" + txtrawprod.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Login.UserID + "')";
                clsDataLayer.ExecuteQuery(ins1);
                for (int a = 0; a < grid.Rows.Count; a++)
                {
                    String ins = "insert into tbl_ReceipeDetail(RCode,ProductName,Quantity)values('" + txtcode.Text + "','" + grid.Rows[a].Cells[0].Value.ToString() + "'," + grid.Rows[a].Cells[1].Value.ToString() + ")";
                    clsDataLayer.ExecuteQuery(ins);

                    //insert into tblStockMaintain(RCode,SaleProductName,ProductName,Quantity,Dates,UserName)values('','','',3,'','')
                    refresh();

                }

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
                String quwe = "SELECT ProductName from tbl_Rawprod where ProductName='" + txtrawprod.Text + "'";
                DataTable ds = clsDataLayer.RetreiveQuery(quwe);
                if (ds.Rows.Count > 0)
                {

                }
                else { abc = true; }
            }
        }
        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckAllFields() && !CheckDataGridCells(grid))
                {
                    Save(); MessageBox.Show("Product Save Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridView1.Refresh(); btnstatus = "save";
                    grid.Enabled = false; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Fill All Fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckAllFields() && !CheckDataGridCells(grid))
            {
                String del = "delete from tbl_Receipe where RCode='" + txtcode.Text + "' delete from tbl_ReceipeDetail where RCode='" + txtcode.Text + "'"; clsDataLayer.ExecuteQuery(del); Save();
                MessageBox.Show("Product Update Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                grid.Enabled = false; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = true;
                btnstatus = "update";
                //
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
            btnstatus = "edit"; txtrawprod.Enabled = true;
        }

        private void grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb2 = null; tb2 = e.Control as TextBox;
            if (tb2 != null) { tb2.KeyPress += new KeyPressEventHandler(tb_KeyPress); }
        }

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell.ColumnIndex != 0) //Desired Column
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

                SqlDataAdapter sda = new SqlDataAdapter("SELECT distinct(RCode) FROM tbl_Receipe Where RCode = '" + txt_search.Text + "'   ", con);
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
            SqlDataAdapter sda = new SqlDataAdapter("SELECT distinct(RCode) FROM tbl_Receipe", con);
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
            SqlDataAdapter sda2 = new SqlDataAdapter("SELECT RawProductName FROM tbl_Receipe Where RCode ='" + txt_search.Text + "'", con);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            if (dt2.Rows.Count > 0)
            {
                txtrawprod.Text = dt2.Rows[0][0].ToString();

            }
            txtcode.Text = txt_search.Text;
            SqlDataAdapter sda = new SqlDataAdapter("SELECT RCode,ProductName,Quantity FROM tbl_ReceipeDetail Where RCode ='" + txt_search.Text + "'", con);
            DataTable dt1 = new DataTable();
            sda.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                grid.Rows.Clear();
                foreach (DataRow row in dt1.Rows)
                {
                    int n = grid.Rows.Add();
                    grid.Rows[n].Cells[0].Value = row[1].ToString(); grid.Rows[n].Cells[1].Value = row[2].ToString();
                }
                String gb = "SELECT distinct(RCode) FROM tbl_Receipe";
                DataTable dt = clsDataLayer.RetreiveQuery(gb);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = null; dataGridView1.DataSource = dt;
                }
            }

        }

        private void txtrawprod_Leave(object sender, EventArgs e)
        {
            if (!txtrawprod.Text.Equals(""))
            {
                if (btnstatus.Equals("new") || btnstatus.Equals("edit"))
                {
                    String sel = "select * from tbl_Receipe where RawProductName='" + txtrawprod.Text + "'";
                    DataTable gb = clsDataLayer.RetreiveQuery(sel);
                    if (gb.Rows.Count > 0)
                    {
                        MessageBox.Show("Product is already available in data !", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); txtrawprod.Text = "";
                    }

                    String ck = "select * from tbl_Rawprod where ProductName='" + txtrawprod.Text + "'";
                    DataTable d1 = clsDataLayer.RetreiveQuery(ck);
                    if (d1.Rows.Count < 1)
                    {
                        MessageBox.Show("Raw Product is not available in data !", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); txtrawprod.Text = "";
                    }
                }
            }
        }

        private void grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (grid.CurrentCell.ColumnIndex == 0)
                {
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
                string q = "select * from vsreceipe where RCode='" + txtcode.Text + "'";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    rptProductreceipe pr = new rptProductreceipe();
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
    }
}
