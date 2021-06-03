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
    public partial class member : Form
    {
        public member()
        {
            InitializeComponent();
            textBox2.Enabled = false;
            textBox1.Enabled = false;
            btn_edit.Enabled = false;
            btn_update.Enabled = false;
            button1.Enabled = true;
            search();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.Length > 0)
                {


                    String sel = "select member_name from member where member_name='" + textBox1.Text + "'";
                    DataTable gb = clsDataLayer.RetreiveQuery(sel);
                    if (gb.Rows.Count == 0)
                    {
                        String insert = "insert into member (sale_code,member_name) values('" + textBox2.Text + "','" + textBox1.Text + "')";
                        clsDataLayer.ExecuteQuery(insert);
                        search();
                        btn_save.Enabled = false;
                        button1.Enabled = true;
                        textBox1.Enabled = false;
                        textBox1.Clear();
                        textBox2.Clear();
                        MessageBox.Show("Saved Successfully");

                    }
                    else
                    {
                        MessageBox.Show("This Name is already exist");
                    }
                }
                else
                {
                    MessageBox.Show("Fill all fields");
                }
            }
            catch 
            {
                
                
            }
        }

        public void search()
        {
            try
            {
                String sel = "select sale_code,member_name from member";
                DataTable gb = clsDataLayer.RetreiveQuery(sel);
                if (gb.Rows.Count > 0)
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = gb;

                }
                else
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                }
            }
            catch 
            {
                
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = clsGeneral.getMAXCode("member", "id", "MI"); 
            textBox1.Enabled = true;
            btn_save.Enabled = true;
            btn_update.Enabled = false;
            button1.Enabled = false;
            textBox1.Focus();
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            btn_save.Enabled = false;
            btn_edit.Enabled = false;
            btn_update.Enabled = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            btn_edit.Enabled = true;
            btn_save.Enabled = false;
            btn_update.Enabled = false;
            button1.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                String insert = "update member  set member_name = '" + textBox1.Text + "' where sale_code = '"+textBox2.Text+"'";
                clsDataLayer.ExecuteQuery(insert);
                search();
                btn_save.Enabled = false;
                btn_update.Enabled = false;
                button1.Enabled = true;
                textBox1.Enabled = false;
                textBox1.Clear();
                textBox2.Clear();
                MessageBox.Show("Updated Successfully");
                textBox1.Focus();
            }
            catch 
            {
                
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Enabled = false;
            textBox1.Enabled = false;
            btn_edit.Enabled = false;
            btn_update.Enabled = false;
            button1.Enabled = true;
            search();
        }
    }
}
