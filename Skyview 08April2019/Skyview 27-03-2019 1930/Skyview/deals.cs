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
    public partial class deal : Form
    {
        TextBox tb = new TextBox();
        public deal()
        {
            InitializeComponent();
            
            refresh();


            String h3 = "select Product_Name from tbl_deals";
            DataTable d3 = clsDataLayer.RetreiveQuery(h3);
            if (d3.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(txt_search, d3);
            }
            String h4 = "select Categories_Name from product_categories";
            DataTable d4 = clsDataLayer.RetreiveQuery(h4);
            if (d4.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(txtcat, d4);
            }
            btnEdit.Enabled = false;
            btnUpdate.Enabled = false;
            btnsave.Enabled = false;
            grid.Enabled = false;
            txtcode.Enabled = false;
            txt_deal.Enabled = false;
            textBox1.Enabled = false;
            txtcat.Enabled = false;
            txtpname.Enabled = false;
            txtprice.Enabled = false;
            txtqty.Enabled = false;
        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);

        public void refresh()
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT distinct(PICode) FROM tbl_deals", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = dt;
            }
        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            
        }

        private void New_Click(object sender, EventArgs e)
        {
            grid.Enabled = true; btnsave.Enabled = true; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = false;
            grid.Rows.Clear();
            
            grid.Enabled = true;
            txt_deal.Enabled = true;
            txtpname.Enabled = true;
            txtprice.Enabled = true;
            txtcat.Enabled = true;
            txtqty.Enabled = true;
            txtcode.Text = clsGeneral.getMAXCode("tbl_deals", "PICode", "PI"); 
        }

        private bool CheckDataGridCells(DataGridView dgv)
        {
            bool flag = false;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                for (int j = 0; j < 3; j++)
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

        private void grid_KeyDown(object sender, KeyEventArgs e)
        {
            {
                try
                {
                    if (grid.CurrentCell.ColumnIndex == 2)
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
        }

        public void Save()
        {
           
                        String sel = "select * from tbl_deals where d_name='" + txt_deal.Text + "'";
                        DataTable gb = clsDataLayer.RetreiveQuery(sel);
                        if (gb.Rows.Count > 0)
                        {
                            MessageBox.Show("Already Add", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            for (int a = 0; a < grid.Rows.Count; a++)
                            {
                                    string checking = "ON";
                                    string mention = "";
                                    string mention1 = grid.Rows[a].Cells[1].Value.ToString();
                                    String sel3 = "select status from tbl_deals where Product_Name='" + mention1 + "' and PICode = '"+txtcode.Text+"' and status = 'ON' ";
                                    DataTable gb3 = clsDataLayer.RetreiveQuery(sel3);
                                    if (gb3.Rows.Count == 0)
                                    {
                                        string str2 = mention1.Replace(txt_deal.Text, "").Replace("  ", "");
                                        mention = str2+ " " + txt_deal.Text;
                                    }
                                    else
                                    {
                                        mention = mention1;
                                    }
                                    
                                    String insert = "insert into tbl_deals (PICode,Product_Name,Product_Categories,Sell_Price,UserName,Date,d_name,status)values('" + txtcode.Text + "','" + mention + "','" + grid.Rows[a].Cells[0].Value.ToString() + "','" + grid.Rows[a].Cells[2].Value.ToString() + "','" + Login.UserID + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + txt_deal.Text + "','"+checking+"')";
                                    clsDataLayer.ExecuteQuery(insert);
                                    refresh();
                                    btnsave.Enabled = false;
                                    New.Enabled = true;
                                    
                        
                            }
                            grid.Rows.Clear();
                            txt_deal.Clear();
                            textBox1.Clear();
                        }
               
        }

        
        private void btnsave_Click(object sender, EventArgs e)
        {

            try
            {   if (!CheckDataGridCells(grid))
                {
                     if (txt_deal.Text != "")
                     {
                
                    Save(); MessageBox.Show("Product Save Successfully!");
                    grid.Enabled = false; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = true;
                    dataGridView2.Refresh();
                }
                else
                {
                    MessageBox.Show("Fill All Fields!");
                }

            }
            else
            {
                MessageBox.Show("Fill All Fields!");
            }
                
            }
            catch { }

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            btnsave.Enabled = false;
            btnEdit.Enabled = true;
            btnUpdate.Enabled = false;
            New.Enabled = false;
            txt_search.Text = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            SqlDataAdapter sda2 = new SqlDataAdapter("SELECT distinct(PICode) FROM tbl_deals Where PICode ='" + txt_search.Text + "'", con);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            if (dt2.Rows.Count > 0)
            {
                txtcode.Text = dt2.Rows[0][0].ToString();

            }

            SqlDataAdapter sda = new SqlDataAdapter("SELECT PICode,Product_Name,Product_Categories,Sell_Price,UserName,Date,d_name FROM tbl_deals Where PICode ='" + txt_search.Text + "'", con);
            DataTable dt1 = new DataTable();
            sda.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                string ck = "";
                grid.Rows.Clear();
                foreach (DataRow row in dt1.Rows)
                {   
                    int n = grid.Rows.Add();
                    ck = row[1].ToString();
                    grid.Rows[n].Cells[0].Value = row[2].ToString();
                    grid.Rows[n].Cells[2].Value = row[3].ToString();
                    txt_deal.Text = row[6].ToString();
                    grid.Rows[n].Cells[1].Value = ck.Replace(txt_deal.Text, "").Replace("  ", "");
                    string tm = grid.Rows[n].Cells[1].Value.ToString();
                    grid.Rows[n].Cells[1].Value = tm.Trim();
                   
                }
                refresh();
                
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            grid.Enabled = true; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = true; New.Enabled = false; txt_deal.Enabled = true;
            txtpname.Enabled = true;
            txtprice.Enabled = true;
            txtcat.Enabled = true;
            txtqty.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckDataGridCells(grid))
            {
                if (txtcode.Text != "")
                {
                    //


                    String del = "delete from tbl_deals where PICode='" + txtcode.Text + "'"; clsDataLayer.ExecuteQuery(del); Save(); MessageBox.Show("Product Update Successfully!");
                    grid.Enabled = false; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = true; txt_deal.Enabled = false;

                }
                else
                {
                    MessageBox.Show("Fill All Fields!");
                }
                //
            }
            else
            {
                MessageBox.Show("Fill All Fields!");
            }
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
                        String get2 = "select Categories_Name from product_categories where Categories_Name != 'DEALS'  "; DataTable df2 = clsDataLayer.RetreiveQuery(get2);
                        if (df2.Rows.Count > 0)
                        {
                            AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
                            foreach (DataRow row in df2.Rows) { acsc.Add(row[0].ToString()); }
                            tb.AutoCompleteCustomSource = acsc;
                            tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                            tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                    }
                }
            }else if (grid.CurrentCell.ColumnIndex == 1)
            
            {
                int yy = grid.CurrentCell.ColumnIndex;
                string columnHeaders = grid.Columns[yy].HeaderText;
                if (columnHeaders.Equals("Product Name"))
                {
                        tb = e.Control as TextBox;
                        if (tb != null)
                        {
                            string pp_name = grid.CurrentRow.Cells[0].Value.ToString();
                            String get = "select Product_Name from tbl_ProductBySale where Product_Categories = '" + pp_name + "' "; DataTable df = clsDataLayer.RetreiveQuery(get);
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
            else 
            {
                tb.AutoCompleteCustomSource = null;
                tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }
            



        }

        private void grid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (grid.CurrentCell.ColumnIndex == 2)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                { e.Handled = true; }
                
            }
        }

        public void total_c()
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
             refresh();

            btnEdit.Enabled = false;
            btnUpdate.Enabled = false;
            btnsave.Enabled = false;
            grid.Enabled = false;
            txtcode.Enabled = false;
            txt_deal.Enabled = false;
            New.Enabled = true;
            txt_deal.Clear();
            grid.Rows.Clear();
            textBox1.Clear();
            txtqty.Clear();
            txtpname.Clear();
            txtprice.Clear();
            txtcat.Clear();
            txtcat.Enabled = false;
            txtpname.Enabled = false;
            txtprice.Enabled = false;
            txtqty.Enabled = false;
        
        }

        private void grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
                textBox1.Text = CellSum().ToString();
        }
        private double CellSum()
        {
            double sum = 0;
            try
            {
                 sum = 0;
                for (int i = 0; i < grid.Rows.Count; ++i)
                {
                    double d = 0;
                    Double.TryParse(grid.Rows[i].Cells[2].Value.ToString(), out d);
                    sum += d;
                }
                
            }
            catch 
            {    
            }
            return sum;
        }

        private void txtpname_TextChanged(object sender, EventArgs e)
        {
            try
            {
                
                String h5 = "select Sell_Price from tbl_ProductBySale where Product_Name = '" + txtpname.Text + "'";
                DataTable d5 = clsDataLayer.RetreiveQuery(h5);
                if (d5.Rows.Count > 0)
                {
                    int no= Convert.ToInt32(d5.Rows[0][0].ToString());
                    txtprice.Text = no.ToString();
                }
                else
                {
                    txtprice.Text = "";
                }
            }
            catch 
            {
                
               
            }
            
        }

        private void txtcat_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String h4 = "select Product_Name from tbl_ProductBySale where Product_Categories = '" + txtcat.Text + "'";
                DataTable d4 = clsDataLayer.RetreiveQuery(h4);
                if (d4.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtpname, d4);
                }
                else
                {
                    txtpname.Text = "";
                }
            }
            catch         
            {
                
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int qtyy = Convert.ToInt32(txtqty.Text);
                for (int i = 1; i <= qtyy; i++)
                {

                    int n = grid.Rows.Add();
                    grid.Rows[n].Cells[0].Value = txtcat.Text;
                    grid.Rows[n].Cells[1].Value = txtpname.Text;
                    grid.Rows[n].Cells[2].Value = txtprice.Text;
                }
                txtcat.Focus();
                txtcat.Clear();
                txtpname.Clear();
                txtqty.Clear();
            }
            catch 
            {
                
                
            }
            
        }

     
    }
}
