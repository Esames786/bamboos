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
    public partial class header_account : Form
    {
        String UID = Login.UserID;
        public string GreenSignal = "";
        public string FormID = "";
        public header_account()
        {
            InitializeComponent();
            Disable();
            refresh(); 

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
                        //if (((TextBox)c).Name == "search_rcpt") { }
                        //else
                        //{
                        flag = true;
                        break;
                        //}
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
        private void Clears()
        {
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
                if (c is TextBox)
                {
                    if (((TextBox)c).Text != "")
                    {
                        ((TextBox)c).Text = "";
                    }
                }
                else if (c is ComboBox)
                {
                    if (((ComboBox)c).Text != "")
                    {
                        ((ComboBox)c).Text = "";

                    }
                }
                else if (c is RichTextBox)
                {
                    if (((RichTextBox)c).Text != "")
                    {
                        ((RichTextBox)c).Text = "";

                    }
                }



            }
        }
        private bool Enable()
        {
            bool flag = false;
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
                if (c is TextBox)
                {
                    if (((TextBox)c).Enabled == false)
                    {
                        ((TextBox)c).Enabled = true;
                        flag = true;
                    }
                }
                else if (c is ComboBox)
                {
                    if (((ComboBox)c).Enabled == false)
                    {
                        ((ComboBox)c).Enabled = true;
                        flag = true;
                    }
                }
                else if (c is MaskedTextBox)
                {
                    if (((MaskedTextBox)c).Enabled == false)
                    {
                        ((MaskedTextBox)c).Enabled = true;
                        flag = true;
                    }
                } 
            }
             

            return flag;
        }
        private bool Disable()
        {
            bool flag = false;
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
                if (c is TextBox)
                {
                    if (((TextBox)c).Enabled == true)
                    {
                        ((TextBox)c).Enabled = false;
                        flag = true;

                    }
                }
                else if (c is ComboBox)
                {
                    if (((ComboBox)c).Enabled == true)
                    {
                        ((ComboBox)c).Enabled = false;
                        flag = true;
                    }
                }
               

                btn_edit.Enabled = false;
                btn_save.Enabled = false;
                
                btn_update.Enabled = false;

                btn_add.Enabled = true;

            }

        


            return flag;
        }

        private void button1_Click(object sender, EventArgs e)
        {
        //FormID = "Add";
        //UserNametesting();
        //if (GreenSignal == "YES")
        //{
        Clears();
        Enable();
        txt_trc_code.Text = clsGeneral.getMAXCode("product_categories", "Catecode", "HC");
        btn_save.Enabled = true;
        btn_update.Enabled = false;   btn_edit.Enabled = false; txt_header.Focus();
        //}
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            //FormID = "Update";
            //UserNametesting();
            //if (GreenSignal == "YES")
            //{
                try
                { 
                    if (!CheckAllFields())
                    {
                        update();
                    }
                    else
                    {
                        MessageBox.Show("Fill All Fields!");
                    }
                }
                catch (Exception)
                { throw; }

            //}
        }

        public void update()
        { 
            String queryya = "Delete From product_categories Where Catecode = '" + txt_trc_code.Text + "'";
        clsDataLayer.ExecuteQuery(queryya);

        string query_grid = "";
        query_grid = "insert into product_categories (Catecode,Categories_Name)values ('" + txt_trc_code.Text + "','" + txt_header.Text + "')";
        if (clsDataLayer.ExecuteQuery(query_grid) > 0)
        {
        MessageBox.Show("Update Succesfully");

        btn_save.Enabled = false;
        btn_update.Enabled = false;
        Clears();
        Disable();
        refresh();
        }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            //FormID = "Save";
            //UserNametesting();
            //if (GreenSignal == "YES")
            //{
                try
                {
                    if (!CheckAllFields())
                    {
                        Save();
                    }
                    else
                    {
                        MessageBox.Show("Fill All Fields!");
                    }
                }
                catch   {   }
          //  }
        }
        public void Save()
        {
            string query_grid = "";
            query_grid = "insert into product_categories (Catecode,Categories_Name)values ('" + txt_trc_code.Text + "','" + txt_header.Text + "')";
            if (clsDataLayer.ExecuteQuery(query_grid) > 0)
            {
                MessageBox.Show("Saved Succesfully");
                
                btn_save.Enabled = false;
                btn_update.Enabled = false;
                Clears();
                Disable();
                refresh();
            }

        }
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        public void refresh()
        {

            String hy = "SELECT Catecode,Categories_Name FROM product_categories";
            DataTable dt = clsDataLayer.RetreiveQuery(hy);
             if (dt.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    int n1 = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n1].Cells[0].Value = dr[0].ToString(); 
                    dataGridView1.Rows[n1].Cells[1].Value = dr[1].ToString(); 
                }  
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
                    Enable();
                    btn_add.Enabled = false;
                    btn_edit.Enabled = false;
                    btn_save.Enabled = false;
                    btn_update.Enabled = true;
               
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_trc_code.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            
            String cg = "SELECT Catecode,Categories_Name from product_categories where Catecode ='" + txt_trc_code.Text + "'";
            DataTable dt1 = clsDataLayer.RetreiveQuery(cg);
            if (dt1.Rows.Count > 0)
            {
                txt_trc_code.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                txt_header.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                 btn_add.Enabled = false;
                btn_edit.Enabled = true;
            }
        }
        private void txt_header_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

      }
}
