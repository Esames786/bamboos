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
    public partial class Purchase_Sale : Form
    {
        String UID = Login.UserID;
        String Code = "";
        public Purchase_Sale()
        {
            InitializeComponent();
            textBox1.Enabled = false;

            comboBox1.Text = "Date Wise";

            String h5 = "select VenderName from Purchase_View";
            DataTable d5 = clsDataLayer.RetreiveQuery(h5);
            if (d5.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(textBox1, d5);
            }
            String get = "select distinct Product_Name from Purchase_View"; DataTable ds3 = clsDataLayer.RetreiveQuery(get); if (ds3.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txtprd, ds3); }
        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        SqlCommand cmd;

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            print();
        }

        private void PartyCode()
        {
           
        }
         
        public void print()
        {
            try
            {
//                Date Wise
//Party Name
//Product Name
                if (comboBox1.SelectedIndex == 0)
                {

                    string q = "select * from Purchase_View where Date between '" + date1.Text + "' and '" + date2.Text + "' ";
                    DataTable purchase = clsDataLayer.RetreiveQuery(q);
                    if (purchase.Rows.Count > 0)
                    {
                        Purchase_report pr = new Purchase_report();
                        pr.SetDataSource(purchase);
                        RepViewer pv = new RepViewer(pr);
                        pv.Show();
                    }
                    else
                    {
                        MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    string q = "select * from Purchase_View where Date between '" + date1.Text + "' and '" + date2.Text + "'  and VenderName='" + textBox1.Text + "'";
                    DataTable purchase = clsDataLayer.RetreiveQuery(q);
                    if (purchase.Rows.Count > 0)
                    {
                        Purchase_report pr = new Purchase_report();
                        pr.SetDataSource(purchase);
                        RepViewer pv = new RepViewer(pr);
                        pv.Show();
                    }
                    else
                    {
                        MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                } 
                else if (comboBox1.SelectedIndex == 2)
                {
                    PartyCode();
                    string q = "";
    q = "select * from Purchase_View where VenderName='" + textBox1.Text+ "' and Product_Name='" + txtprd.Text+ "' and Date between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'";
                    DataTable purchase = clsDataLayer.RetreiveQuery(q);
                    if (purchase.Rows.Count > 0)
                    {
                        Purchase_report pr = new Purchase_report();
                        pr.SetDataSource(purchase);
                        RepViewer pv = new RepViewer(pr);
                        pv.Show();
                    }
                    else
                    {
                        MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (comboBox1.SelectedIndex == 3)
                {
                    PartyCode();
                    string q = "";
                    q = "select * from Purchase_View where Product_Name='" + txtprd.Text + "' and Date between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'";
                    DataTable purchase = clsDataLayer.RetreiveQuery(q);
                    if (purchase.Rows.Count > 0)
                    {
                        Purchase_report pr = new Purchase_report();
                        pr.SetDataSource(purchase);
                        RepViewer pv = new RepViewer(pr);
                        pv.Show();
                    }
                    else
                    {
                        MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch   { }
        }

        private void Return_Sale_Load(object sender, EventArgs e)
        {

        }

        private void Return_Sale_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
//            Date Wise
//Party Name
//Party Name  and Product Name
//Product Name
            if (comboBox1.SelectedIndex == 0)
            { 
                textBox1.Text = "";
                textBox1.Enabled = false; txtprd.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 1)
            { 
                textBox1.Text = "";
                textBox1.Enabled = true; txtprd.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                textBox1.Text = ""; txtprd.Enabled = true; textBox1.Enabled = true;
            }
            else
            {
                textBox1.Text = "";
                textBox1.Enabled = false; txtprd.Enabled = true;
                String get = "select distinct Product_Name from Purchase_View where VenderName='" + textBox1.Text+"'"; DataTable ds3 = clsDataLayer.RetreiveQuery(get); if (ds3.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txtprd,ds3); }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
             //   print();
            }
        }

        private void date1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                print();
            }
        }

        private void date2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                print();
            }
        }

        
    }
}
