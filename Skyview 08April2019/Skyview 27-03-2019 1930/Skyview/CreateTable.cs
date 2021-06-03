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
    public partial class CreateTable : Form
    {
        TextBox tb = new TextBox();
        public CreateTable()
        {
            InitializeComponent(); SelectAll();
            String query = "select Table_Name from tbl_table";
            DataTable d1 = clsDataLayer.RetreiveQuery(query);
            if (d1.Rows.Count > 0)
            {        clsGeneral.SetAutoCompleteTextBox(txtsearch, d1);   }
            grid.Enabled = false;
            btnsave.Enabled = false;
            btnUpdate.Enabled = false;
            btnEdit.Enabled = false;
        }

        private void SelectAll()
        {
            String query = "select distinct TCode from tbl_table";
            DataTable d1 = clsDataLayer.RetreiveQuery(query);
            if (d1.Rows.Count > 0)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                foreach (DataRow dr in d1.Rows)
                {
                    int n = dataGridView1.Rows.Add(); dataGridView1.Rows[n].Cells[0].Value = dr["TCode"].ToString();
                } 
            }
        }

        private void New_Click(object sender, EventArgs e)
        {
            grid.Enabled = true; btnsave.Enabled = true; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = false;
            grid.Rows.Clear();
            grid.Rows.Add();
            txtcode.Text = clsGeneral.getMAXCode("tbl_table", "TCode", "TC"); txtsearch.Text = "-";
        }

        private void grid_KeyDown(object sender, KeyEventArgs e)
        {
        try
        {
        if (grid.CurrentCell.ColumnIndex == 0)
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
        for(int a=0;a<grid.Rows.Count;a++)
        {
            String sel = "select * from tbl_table where Table_Name='" + grid.Rows[a].Cells[0].Value + "'";
        DataTable gb = clsDataLayer.RetreiveQuery(sel);
        if(gb.Rows.Count > 0)
        {
            MessageBox.Show("Already Add!");
        }else{
            String ins = "insert into tbl_table(TCode,Table_Name,UserName,Date)values('" + txtcode.Text + "','" + grid.Rows[a].Cells[0].Value.ToString() + "','" + Login.UserID + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
            clsDataLayer.ExecuteQuery(ins);
        } 
        }
        }
        else
        {
            MessageBox.Show("Fill All Fields!");
        }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
 
            Save(); MessageBox.Show("Table Save Successfully!");     grid.Enabled =false; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = true;
            Search(); SelectAll();
            }
            catch { }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
        if (!CheckAllFields() && !CheckDataGridCells(grid))
            {
          String del = "delete from tbl_table where TCode='" + txtcode.Text + "'"; clsDataLayer.ExecuteQuery(del); Save(); MessageBox.Show("Product Update Successfully!");
           grid.Enabled = false; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = true;
           Search(); SelectAll();
        }
            else
            {
                MessageBox.Show("Fill All Fields!");
            }
        } 
        private void btnEdit_Click(object sender, EventArgs e)
        {
            grid.Enabled = true; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = true; New.Enabled = true; txtsearch.Text = "-";
        
       }

        private void Search()
        {
            try
            {
                String query = "select TCode from tbl_table where Table_Name Like '" + txtsearch.Text + "%'";
                DataTable d1 = clsDataLayer.RetreiveQuery(query);
                if (d1.Rows.Count > 0)
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        dataGridView1.Rows.Clear();
                    } 
                    foreach (DataRow dr in d1.Rows)
                    {
                        int n = dataGridView1.Rows.Add(); dataGridView1.Rows[n].Cells[0].Value = dr["TCode"].ToString();
                    }
                }
                else
                { 
                }
            }
            catch { }
        }
        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                String query = "select TCode,Table_Name from tbl_table where TCode='" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "'";
                DataTable d1 = clsDataLayer.RetreiveQuery(query);
                if (d1.Rows.Count > 0)
                {
                    txtcode.Text = d1.Rows[0]["TCode"].ToString(); grid.Rows.Clear();
                    foreach (DataRow dr in d1.Rows)
                    {
                        int n = grid.Rows.Add(); grid.Rows[n].Cells[0].Value = dr["Table_Name"].ToString();
                    }
                }
                btnEdit.Enabled = true;
            }
            catch { }
        }

        private void CreateTable_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

    }
}
