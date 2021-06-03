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
    public partial class Customer_info : Form
    {
        String UID = Login.UserID;
        public string GreenSignal = "";
        public string FormID = "";
        public Customer_info()
        {
     InitializeComponent();  disable();    txt_Add.Enabled = false;     refresh(); btn_add.Focus();
        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
         
        public void disable()
        {
            txt_srch.Enabled = false;
            //textBox1.Enabled = false;
            txt_name.Enabled = false;
            txt_cntct.Enabled = false;
           
            txt_email.Enabled = false;
            txt_nic.Enabled = false;
            //txt_RBalnce.Enabled = false;

            btn_save.Enabled = false;
            btn_upd.Enabled = false;
            btn_edit.Enabled = false;
            txt_id.Enabled = false;
            txt_Add.Enabled = false;
            
        }
        public void refresh()
        {
            string query = "Select c_id,c_name,c_phone from customer_info";
            DataTable dt = clsDataLayer.RetreiveQuery(query);
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
            }
        }

        private void Customer_info_Load(object sender, EventArgs e)
        {
            setcombo();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (txt_Add.Text == "")
            {
                txt_Add.Text = "-";
            }
            if (txt_email.Text == "")
            {
                txt_email.Text = "-";
            }
            if (txt_nic.Text == "")
            {
                txt_nic.Text = "-";
            }
            
            if (!CheckAllFields())
            {
                String queey = "Select c_name,c_phone from customer_info where c_name = '" + txt_name.Text + "' and c_phone = '" + txt_cntct.Text + "'";
                DataTable dt = clsDataLayer.RetreiveQuery(queey);
                if (dt.Rows.Count == 0)
                {
                    String queey3 = "Select c_phone from customer_info where  c_phone = '" + txt_cntct.Text + "'";
                    DataTable dt3 = clsDataLayer.RetreiveQuery(queey3);
                    if (dt3.Rows.Count == 0)
                    {

                    String s1 = "INSERT INTO [dbo].[customer_info](c_id,[c_name],[c_phone],[c_address],[c_email],[nic],[datee]) VALUES ('"+txt_id.Text+"','" + txt_name.Text + "','" + txt_cntct.Text + "','" + txt_Add.Text + "','" + txt_email.Text + "','" + txt_nic.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    clsDataLayer.ExecuteQuery(s1);
                    MessageBox.Show("Record Saved !!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearr();
                    disable();
                    btn_add.Enabled = true;
                    refresh();

                    }
                    else
                    {
                        MessageBox.Show("The Contact Number Already Taken","Error",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    }

                }
                else
                {
                    MessageBox.Show("Data Already Exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
            {
                MessageBox.Show("Fill Atleast name and contact");
            }
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearr();
           
        }

        public void clearr()
        {
            txt_cntct.Clear();
            txt_Add.Clear();
            txt_email.Clear();
            txt_name.Clear();
            txt_nic.Clear();
            //txt_RBalnce.Clear();
            txt_id.Clear();
           // textBox1.Clear();
            disable();
            btn_add.Enabled = true;
        }


        public void setcombo()
        {
            if (comboBox1.SelectedIndex == 0)
            {
                string q1 = "Select c_name from customer";
                DataTable dt = clsDataLayer.RetreiveQuery(q1);
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                string q2 = "Select c_phone from customer";
                DataTable dt2 = clsDataLayer.RetreiveQuery(q2);
            }

        }

        //private void Change_Password_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == (char)Keys.Escape)
        //    {
        //        this.Close();
        //    }
        //}

        private void close(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {

            //txtUser.Refresh();
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_upd_Click(object sender, EventArgs e)
        { 
            try
            {
                if (txt_Add.Text == "")
                {
                    txt_Add.Text = "-";
                }
                if (txt_email.Text == "")
                {
                    txt_email.Text = "-";
                }
                if (txt_nic.Text == "")
                {
                    txt_nic.Text = "-";
                }
            
                if (!CheckAllFields())
                { 
                    String queey = "Select c_phone from customer_info where c_phone = '" + txt_cntct.Text + "' and c_id != '" + txt_id.Text + "'";
                    DataTable dt = clsDataLayer.RetreiveQuery(queey);
                    if (dt.Rows.Count == 0)
                    {
                        String s1 = "update customer_info set c_id = '" + txt_id.Text + "',c_name = '" + txt_name.Text + "', c_phone = '" + txt_cntct.Text + "',c_address ='" + txt_Add.Text + "' , c_email = '" + txt_email.Text + "',nic ='" + txt_nic.Text + "'  ,datee =  '" + DateTime.Now.ToString("yyyy-MM-dd") + "' where c_id = '"+txt_id.Text+"'";
                        clsDataLayer.ExecuteQuery(s1); 
                        MessageBox.Show("Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clearr();  disable();   btn_add.Enabled = true;  refresh(); btn_upd.Enabled = false; 
                    }
                    else
                    {
                        MessageBox.Show("Phone Number Already Exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                else
                {
                    MessageBox.Show("Fill Atleast name and contact");
                }
            }
            catch { }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            txt_name.Enabled = true;
            txt_cntct.Enabled = true;
            txt_Add.Enabled = true;
            txt_email.Enabled = true;
            txt_nic.Enabled = true;
           // txt_RBalnce.Enabled = false;
            btn_edit.Enabled = false;
            btn_upd.Enabled = true;
            btn_save.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                String quer = "select c_name from customer_info";
                DataTable ds = clsDataLayer.RetreiveQuery(quer);
                if (ds.Rows.Count > 0)
                { clsGeneral.SetAutoCompleteTextBox(txt_srch, ds); }
                txt_srch.Enabled = true;
            }

            else if (comboBox1.SelectedIndex == 1)
            {
                String quer = "select c_phone from customer_info";
                DataTable ds = clsDataLayer.RetreiveQuery(quer);
                if (ds.Rows.Count > 0)
                { clsGeneral.SetAutoCompleteTextBox(txt_srch, ds); }
                txt_srch.Enabled = true;
            }
            else
            {
                txt_srch.Enabled = false;
            }
        }

        public void getdata()
        {
            if (comboBox1.SelectedIndex == 0)
            { 
                string query = "Select c_id,c_name,c_phone from customer_info where c_name Like '" + txt_srch.Text + "%'";
                DataTable dt = clsDataLayer.RetreiveQuery(query);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dt;
                }
            }
            else if (comboBox1.SelectedIndex == 1)
            {

                string query = "Select c_id,c_name,c_phone from customer_info where c_phone Like '" + txt_srch.Text + "%'";
                DataTable dt = clsDataLayer.RetreiveQuery(query);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dt;
                }
            }


        }

        private void txt_srch_TextChanged(object sender, EventArgs e)
        {
            if (txt_srch.Text.Length > 0)
            {
                getdata();
            }
            else
            {
                refresh();
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btn_edit.Enabled = true;
            string check = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            string check_no = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            string query = "Select c_id,c_name,c_phone,c_address,c_email,nic from customer_info where c_id= '"+check.ToString()+"'";
            DataTable dt = clsDataLayer.RetreiveQuery(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    txt_id.Text = dr[0].ToString();
                    txt_name.Text = dr[1].ToString();
                    txt_cntct.Text = dr[2].ToString();
                    txt_Add.Text = dr[3].ToString();
                    txt_email.Text = dr[4].ToString();
                    txt_nic.Text = dr[5].ToString();
                    
                }
            }
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
        private bool CheckDataGridCells(DataGridView dgv)
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
            return flag;
        }

        private void btn_add_Click(object sender, EventArgs e)
        { 
            clearr();
            txt_id.Text = clsGeneral.getMAXCode("customer_info", "c_id", "CD");
            txt_name.Enabled = true;  txt_cntct.Enabled = true;
            txt_Add.Enabled = true;  txt_email.Enabled = true;
            txt_nic.Enabled = true;
           // txt_RBalnce.Enabled = true;
            btn_save.Enabled = true;
            btn_add.Enabled = false;
            btn_upd.Enabled = false;
        }

        private void txt_cntct_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) { e.Handled = true; }
        }

        private void txt_nic_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) { e.Handled = true; }
        }

        private void txt_RBalnce_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) { e.Handled = true; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txt_name.Text.Length > 0 && txt_cntct.Text.Length > 0)
            {

                String qq = "Select * from customer_info where c_phone = '" + txt_cntct.Text + "' and c_name = '" + txt_name.Text + "' ";
                DataTable dtt = clsDataLayer.RetreiveQuery(qq);
                if (dtt.Rows.Count > 0)
                {
                    CustomerrInfo cis = new CustomerrInfo();
                    cis.SetDataSource(dtt);
                    CIrptViewer crr = new CIrptViewer(cis);
                    crr.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found");
                }
            }
            else
            {
                String qq = "Select * from customer_info ";
                DataTable dtt = clsDataLayer.RetreiveQuery(qq);
                if (dtt.Rows.Count > 0)
                {
                    CustomerrInfo cis = new CustomerrInfo();
                    cis.SetDataSource(dtt);
                    CIrptViewer crr = new CIrptViewer(cis);
                    crr.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found");
                }
            }
        }
            

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txt_name_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txt_cntct_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_Add_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void txt_email_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_nic_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void txt_RBalnce_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        }
    }

