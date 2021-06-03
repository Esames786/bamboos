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
    public partial class Add_User : Form
    {
        String UID = Login.UserID;
        public string GreenSignal = "";
        public string FormID = "";
        public Add_User()
        {
            InitializeComponent();
            this.BackColor = Color.SteelBlue;
        }
        public void UserNametesting()
        {
            string query = "select * from user_access where USENAME='" + UID + "' AND FORM_NAME='" + FormID + "'";
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
            con.Open();
            SqlCommand sc = new SqlCommand(query, con);
            SqlDataReader dr = sc.ExecuteReader();
            dr.Read();

            if (dr.HasRows == true)
            {
                if (dr["ACCESS"].ToString() == "Yes")
                {
                    GreenSignal = "YES";
                }
                else
                {
                    GreenSignal = "NO"; 
                }
            }
        }
         
        private void New()
        {
            FormID = "Add";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                txtEID.Text = clsGeneral.getMAXCode("login", "ID", "U");

                this.txtEID.Focus();
                txtEID.Enabled = true;
                txtName.Enabled = true;
                comboStatus.Enabled = true;
                txtPassword.Enabled = true;
                btnSave.Enabled = true;
                btnNew.Enabled = false;

                txtcpassword.Enabled = true;
                txtName.Focus();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to perform this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        private void btnNew_Click(object sender, EventArgs e)
        {
            New();    
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                FormID = "Save";
                UserNametesting();
                GreenSignal = "YES";
                if (GreenSignal == "YES")
                {
                    if (txtPassword.Text.Equals(txtcpassword.Text))
                    {
                        if (txtName.Text.Equals("") && txtPassword.Text.Equals(""))
                        {
                            MessageBox.Show("Fill All Fields!");
                        }
                        else
                        {
                            string query = "";
                            query = "INSERT INTO login(USERNAME, PASSWORD, ID, STATUS,CreatedBy) VALUES('" + txtName.Text + "','" + txtPassword.Text + "','" + txtEID.Text + "', '" + comboStatus.Text + "','" + Login.UserID + "')";
                            con.Open();
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                            MessageBox.Show(txtName.Text + " Add as a Employee!");
                            txtEID.Clear();
                            txtName.Clear();
                            txtPassword.Clear();
                            txtcpassword.Clear();
                            btnNew.Enabled = true;
                            txtEID.Enabled = false;
                            txtName.Enabled = false;
                            txtPassword.Enabled = false;
                            comboStatus.Enabled = false;
                            comboStatus.SelectedIndex = -1;
                            ShowAll();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Password and Confirm Password does not same!");
                    }
                }
                else
                {
                    MessageBox.Show("Sorry! You do not have permission to perform this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            
            }
            catch { }
        }

        private void Add_User_Load(object sender, EventArgs e)
        {
            ShowAll();
        }

        private void ShowAll()
        {
            try
            {
                String s = "SELECT * FROM login ORDER BY ID ASC";
                DataTable dt = clsDataLayer.RetreiveQuery(s);

                if (dt.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();

                    foreach (DataRow item in dt.Rows)
                    {
                        int n = dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells[0].Value = item["ID"].ToString();
                        dataGridView1.Rows[n].Cells[1].Value = item["USERNAME"].ToString();
                    }
                    dataGridView1.PerformLayout();
                }
                New();
            }
            catch   {   }
        } 
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtEmployeeID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        } 
        private void txtEmployeeID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String sel = "SELECT * FROM Login WHERE ID = '" + txtEmployeeID.Text + "'"; 
                DataTable dt = clsDataLayer.RetreiveQuery(sel);
                if (dt.Rows.Count > 0)
                {
                    Name = dt.Rows[0][0].ToString();
                    txtEmployeeID.Text = dt.Rows[0][2].ToString();
                    comboChngeStatus.Text = dt.Rows[0][3].ToString();
                }
            }
            catch { }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try 
            {
                FormID = "Update";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    if (txtEmployeeID.Text.Equals(""))
                    {
                        MessageBox.Show("Please Select Any Employee For Perform Update Function!");
                    }
                    else
                    { 
                        string query = "UPDATE Login SET STATUS = '" + comboChngeStatus.Text + "' WHERE ID = '" + txtEmployeeID.Text + "'";
                        clsDataLayer.ExecuteQuery(query);
                        MessageBox.Show(Name + " Employement Status is now " + comboChngeStatus.Text + ".", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                        txtEmployeeID.Clear();
                        comboChngeStatus.SelectedIndex = -1;
                    }
                }
                else
                {
                    MessageBox.Show("Sorry! You do not have permission to perform this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
              
            }
            catch { }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            FormID = "Delete";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                if (!txtEmployeeID.Text.Equals(""))
                {
                    String del = "delete from login where ID='" + txtEmployeeID.Text + "'";
                    if (clsDataLayer.ExecuteQuery(del) > 0)
                    {
                        MessageBox.Show("User Delete Successfully!");
                        ShowAll();
                        txtEmployeeID.Text = ""; 
                    }
                }
                else
                {
                    MessageBox.Show("Please Select Any Employee For Perform Delete Function!");
                }
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to perform this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Close(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
       
        }



 
    }
}
