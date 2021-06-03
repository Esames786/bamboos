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
    public partial class Pur_Vchr_Srch : Form
    {
        PurchaseVoucher1cs pv;
        public Pur_Vchr_Srch(PurchaseVoucher1cs p)
        {
            InitializeComponent();
            pv = p;
            string fetch = "SELECT distinct [Pur_Code],[VenderName],[BillAmount],[Discount],[NetAmount],[UserName],[Date] FROM [dbo].[Purchase_View]";
            DataTable ddt = clsDataLayer.RetreiveQuery(fetch);
            if (ddt.Rows.Count > 0)
            {
                foreach (DataRow dr in ddt.Rows)
                {
               int n = dataGridView1.Rows.Add();
               dataGridView1.Rows[n].Cells[0].Value = dr[0];
               dataGridView1.Rows[n].Cells[1].Value = dr["VenderName"];
               dataGridView1.Rows[n].Cells[2].Value = dr["Date"];
               dataGridView1.Rows[n].Cells[3].Value = dr["BillAmount"];
               dataGridView1.Rows[n].Cells[4].Value = dr["Discount"];
               dataGridView1.Rows[n].Cells[5].Value = dr["NetAmount"]; 
                }
            }
            
        }

        public Pur_Vchr_Srch()
        {
            // TODO: Complete member initialization
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    string q1 = "select [Pur_Code] from [Purchase_View]";
                    DataTable data = clsDataLayer.RetreiveQuery(q1);
                    if (data.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txt_srch_prod, data);
                    }
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    string q2 = "select [VenderName] from [Purchase_View]";
                    DataTable data1 = clsDataLayer.RetreiveQuery(q2);
                    if (data1.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txt_srch_prod, data1);
                    }
                }
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
 

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
            pv.txt_pur_code.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            this.Close();
            }
            catch { }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txt_srch_prod_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            if (comboBox1.SelectedIndex == 0)
            {
                string fetch = "SELECT distinct [Pur_Code],[VenderName],[BillAmount],[Discount],[NetAmount],[UserName],[Date] FROM [dbo].[Purchase_View] where [Pur_Code] = '" + txt_srch_prod.Text + "' ";
                DataTable ddt = clsDataLayer.RetreiveQuery(fetch);
                if (ddt.Rows.Count > 0)
                {
                    foreach (DataRow dr in ddt.Rows)
                    {
                        int n = dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells[0].Value = dr[0];
                        dataGridView1.Rows[n].Cells[1].Value = dr["VenderName"];
                        dataGridView1.Rows[n].Cells[2].Value = dr["Date"];
                        dataGridView1.Rows[n].Cells[3].Value = dr["BillAmount"];
                        dataGridView1.Rows[n].Cells[4].Value = dr["Discount"];
                        dataGridView1.Rows[n].Cells[5].Value = dr["NetAmount"];
                    }
                }
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                string fetch1 = "SELECT distinct [Pur_Code],[VenderName],[BillAmount],[Discount],[NetAmount],[UserName],[Date] FROM [dbo].[Purchase_View] where [VenderName] = '" + txt_srch_prod.Text + "' ";
                DataTable dt = clsDataLayer.RetreiveQuery(fetch1);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int n = dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells[0].Value = dr[0];
                        dataGridView1.Rows[n].Cells[1].Value = dr["VenderName"];
                        dataGridView1.Rows[n].Cells[2].Value = dr["Date"];
                        dataGridView1.Rows[n].Cells[3].Value = dr["BillAmount"];
                        dataGridView1.Rows[n].Cells[4].Value = dr["Discount"];
                        dataGridView1.Rows[n].Cells[5].Value = dr["NetAmount"];
                    }
                }
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    pv.txt_pur_code.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    this.Close();
                }
                catch { }

            }
        }
         
   
        }
    }
