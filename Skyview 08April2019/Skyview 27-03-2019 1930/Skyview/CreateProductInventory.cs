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
    public partial class CreateProductInventory : Form
    {
        TextBox tb = new TextBox();
        public CreateProductInventory()
        {
            InitializeComponent();
            refresh();


            String h3 = "select PICode from tbl_ProductByInventory";
            DataTable d3 = clsDataLayer.RetreiveQuery(h3);
            if (d3.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(txt_search, d3);
            }

            btnEdit.Enabled = false;
            btnUpdate.Enabled = false;
            btnsave.Enabled = false;
            grid.Enabled = false;

        }

        private void New_Click(object sender, EventArgs e)
        {
            grid.Enabled = true; btnsave.Enabled = true; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = false;
            grid.Rows.Clear();
            grid.Rows.Add();
            grid.Enabled = true;
            txtcode.Text = clsGeneral.getMAXCode("tbl_ProductByInventory", "PICode", "PC"); txt_search.Text = "-";
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
                for (int j = 0; j < 2; j++)
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
        for(int a=0;a<grid.Rows.Count;a++)
        {
            String sel = "select * from tbl_ProductByInventory where Product_Name='" + grid.Rows[a].Cells[0].Value + "'";
        DataTable gb = clsDataLayer.RetreiveQuery(sel);
        if(gb.Rows.Count > 0)
        {

        }
        else
        {
            String ins = "insert into tbl_ProductByInventory(PICode,Product_Name,Quantity,Purchase_Price,UserName,Date)values('" + txtcode.Text + "','" + grid.Rows[a].Cells[0].Value.ToString() + "', '0','" + grid.Rows[a].Cells[1].Value.ToString() + "','" + Login.UserID + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
            clsDataLayer.ExecuteQuery(ins);
            refresh();
        } 
        }
        }
        else
        {
            MessageBox.Show("Fill All Fields!");
        }
        }

         bool abc = false;
         private void CheckCategories()
         {
             abc = false;
         for (int a = 0; a < grid.Rows.Count; a++)
         {
         String quwe = "SELECT Categories_Name from product_categories where Categories_Name='"+grid.Rows[a].Cells[0].Value+"'";
         DataTable ds = clsDataLayer.RetreiveQuery(quwe);
         if (ds.Rows.Count > 0)
         {

         } else { abc = true; }
         }
         }
        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
 
            Save(); MessageBox.Show("Product Save Successfully!");
            dataGridView1.Refresh();
            grid.Enabled =false; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = true;
            }
            catch { }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckAllFields() && !CheckDataGridCells(grid))
            { 
                 String del = "delete from tbl_ProductByInventory where PICode='" + txtcode.Text + "'"; clsDataLayer.ExecuteQuery(del); Save(); MessageBox.Show("Product Update Successfully!");
                grid.Enabled = false; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = true;
                 
                //
            }
            else
            {
                MessageBox.Show("Fill All Fields!");
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            
            
            grid.Enabled = true; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = true; New.Enabled = false;

        }

        private void grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
        if (grid.CurrentCell.ColumnIndex == 0)
        {
        int yy = grid.CurrentCell.ColumnIndex;
        string columnHeaders = grid.Columns[yy].HeaderText;
        if (columnHeaders.Equals("Product Categories"))
        {
        tb = e.Control as TextBox;
        if (tb != null)
        {
        String get = "select Categories_Name from product_categories"; DataTable df = clsDataLayer.RetreiveQuery(get);
        if (df.Rows.Count > 0)
        {
        AutoCompleteStringCollection acsc = new AutoCompleteStringCollection(); 
        foreach (DataRow row in df.Rows) { acsc.Add(row[0].ToString()); }
        tb.AutoCompleteCustomSource = acsc;
        tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
        } } }
        } 
        }

        private void grid_KeyPress(object sender, KeyPressEventArgs e)
        {
        if (grid.CurrentCell.ColumnIndex == 1)
        {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        {   e.Handled = true; } }
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

                SqlDataAdapter sda = new SqlDataAdapter("SELECT distinct(PICode) FROM tbl_ProductByInventory Where PICode = '" + txt_search.Text + "'   ", con);
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
            SqlDataAdapter sda = new SqlDataAdapter("SELECT distinct(PICode) FROM tbl_ProductByInventory", con);
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
            SqlDataAdapter sda2 = new SqlDataAdapter("SELECT distinct(PICode) FROM tbl_ProductByInventory Where PICode ='" + txt_search.Text + "'", con);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            if (dt2.Rows.Count > 0)
            {
                txtcode.Text = dt2.Rows[0][0].ToString();
         
            }

            SqlDataAdapter sda = new SqlDataAdapter("SELECT PICode,Product_Name,Quantity,Purchase_Price,UserName,Date FROM tbl_ProductByInventory Where PICode ='" + txt_search.Text + "'", con);
            DataTable dt1 = new DataTable();
            sda.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
            grid.Rows.Clear();
            foreach (DataRow row in dt1.Rows)
            {
            int n = grid.Rows.Add();
            grid.Rows[n].Cells[0].Value = row[1].ToString();
            grid.Rows[n].Cells[1].Value = row[3].ToString();
            }
            String gb = "SELECT distinct(PICode) FROM tbl_ProductByInventory";
            DataTable dt = clsDataLayer.RetreiveQuery(gb); 
            if (dt.Rows.Count > 0)
            {
           dataGridView1.DataSource = null;   dataGridView1.DataSource = dt;
            }
            }

        }

      

    }
}
