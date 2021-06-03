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
    public partial class expense : Form
    {
        public expense()
        {
            InitializeComponent();
            disable();
            btn_disable();
            btn_new.Enabled = true;
            refresh();
            textBox4.Enabled = false;
            gett();
            
        }

        public void gett()
        {
            String quer = "select name from expense";
            DataTable ds = clsDataLayer.RetreiveQuery(quer);
            if (ds.Rows.Count > 0)
            { clsGeneral.SetAutoCompleteTextBox(txt_name, ds); }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
        if (!CheckAllFields())
        { 
        String queey = "Select e_code,name,amount from expense where e_code = '" + txt_code.Text + "'";
        DataTable dt = clsDataLayer.RetreiveQuery(queey);
        if (dt.Rows.Count == 0)
        {

        String s10 = "insert into expense (e_code,name,amount,date,username,qty) values ('" + txt_code.Text + "','" + txt_name.Text + "','" + txt_price.Text + "','" + dateTimePicker1.Text + "','" + Login.UserID + "','"+Convert.ToDecimal(txtqty.Text).ToString()+"')";
        clsDataLayer.ExecuteQuery(s10); MessageBox.Show("Record Saved !!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); clear(); disable(); btn_disable(); btn_new.Enabled = true; refresh(); gett();
        }
        else
        {
            MessageBox.Show("Data Already Exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
        }
        else
        {
       MessageBox.Show("Kindly Fill All Fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
        }

        private void disable()
        {
            txt_price.Enabled = false;
            txt_name.Enabled = false;
            txt_code.Enabled = false;
            txtqty.Enabled = false;
        }

        private void enable()
        {
            txt_price.Enabled = true;
            txt_name.Enabled = true;
            txt_code.Enabled = false;
            txtqty.Enabled = true;
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

                else if (c is RichTextBox)
                {
                    if (((RichTextBox)c).Text == "")
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }

        public void clear()
        {
            txt_code.Text = "";
            txt_name.Text = "";
            txt_price.Text = "";
            txtqty.Text = "";
        }

        public void btn_disable()
        {
            btn_new.Enabled = false;
            btn_save.Enabled = false;
            btn_update.Enabled = false;
            btn_edit.Enabled = false;
            

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            clear();
            txt_code.Text = clsGeneral.getMAXCode("expense", "id", "Ex");
            btn_disable();
            enable();
            btn_save.Enabled = true;

        }

        public void refresh()
        {
            string query = "Select e_code,name,amount,date,qty from expense";
            DataTable dt = clsDataLayer.RetreiveQuery(query);
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string query = "Select e_code,name,amount,date,qty from expense";
            DataTable dt = clsDataLayer.RetreiveQuery(query);
            if (dt.Rows.Count > 0)
            {
                string check = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                txt_code.Text = check.ToString();
                txt_name.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                decimal price= Convert.ToDecimal(dataGridView1.CurrentRow.Cells[2].Value.ToString());
                txt_price.Text = price.ToString();
                dateTimePicker1.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                txtqty.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                btn_disable();
                btn_edit.Enabled = true;
            }

        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            btn_disable();
            enable();
            btn_update.Enabled = true;

        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (!CheckAllFields())
            {
                String queey = "Select e_code from expense where e_code = '" +txt_code.Text + "' ";
                DataTable dt = clsDataLayer.RetreiveQuery(queey);
                if (dt.Rows.Count > 0)
                {
                    
                    String s10 = "update expense set e_code = '" + txt_code.Text+ "',name = '" + txt_name.Text + "', amount = '" + txt_price.Text + "',date ='" + dateTimePicker1.Text+ "' ,qty = '"+Convert.ToDecimal(txtqty.Text).ToString()+"' where e_code = '" + txt_code.Text + "'";
                    clsDataLayer.ExecuteQuery(s10);
                    MessageBox.Show("Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear(); disable(); disable(); btn_new.Enabled = true; refresh(); gett();
                }
                else
                {
                    MessageBox.Show("Phone Number Already Exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
            {
                MessageBox.Show("Kindly Fill All Fields");
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                if (textBox4.Text.Length > 0)
                {


                    string query = "Select e_code,name,amount,date,qty from expense where e_code like '" + textBox4.Text + "'";
                    DataTable dt = clsDataLayer.RetreiveQuery(query);
                    if (dt.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = dt;
                    }
                }
                else
                {
                    refresh();
                }
            }
            else
            {
                if (textBox4.Text.Length > 0)
                {


                    string query = "Select e_code,name,amount,date,qty from expense where name like '" + textBox4.Text + "'";
                    DataTable dt = clsDataLayer.RetreiveQuery(query);
                    if (dt.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = dt;
                    }
                }
                else
                {
                    refresh();
                }
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            btn_disable();
            disable();
            refresh();
            clear();
            btn_new.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txt_code.Text.Length == 0)
            {

                string dd = "";

                string qq = "Select * from expense";
                DataTable dtt = clsDataLayer.RetreiveQuery(qq);
                if (dtt.Rows.Count > 0)
                {
                    expense_report sw = new expense_report();
                    sw.SetDataSource(dtt);
                    expense_view wv = new expense_view(sw, dd, dd);
                    wv.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                string dd = "";

                string qq = "Select * from expense where e_code = '"+txt_code.Text+"'";
                DataTable dtt = clsDataLayer.RetreiveQuery(qq);
                if (dtt.Rows.Count > 0)
                {
                    expense_report sw = new expense_report();
                    sw.SetDataSource(dtt);
                    expense_view wv = new expense_view(sw, dd, dd);
                    wv.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txt_price_KeyPress(object sender, KeyPressEventArgs e)
        {
              if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                { e.Handled = true; }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                label4.Text = "Name";
                String quer = "select name from expense";
                DataTable ds = clsDataLayer.RetreiveQuery(quer);
                if (ds.Rows.Count > 0)
                { clsGeneral.SetAutoCompleteTextBox(textBox4, ds); }
                textBox4.Enabled = true;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                String quer = "select e_code from expense";
                DataTable ds = clsDataLayer.RetreiveQuery(quer);
                if (ds.Rows.Count > 0)
                { clsGeneral.SetAutoCompleteTextBox(textBox4, ds); }
                label4.Text = "Code";
                textBox4.Enabled = true;
            }
            else
            {
                textBox4.Enabled = false;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            { e.Handled = true; }
        }
    }
}
