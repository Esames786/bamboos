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
    public partial class Change_Password : Form
    {
        String UID = Login.UserID;
        public string GreenSignal = "";
        public string FormID = "";
        public Change_Password()
        {
            InitializeComponent();
         }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        public void UserNametesting()
        {
            string query = "  select * from user_access where USENAME='" + UID + "' AND FORM_NAME='" + FormID + "'";
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

        private void Change_Password_Load(object sender, EventArgs e)
        {
            txtUser.Text = UID;
            FormID = "PasswordAccess";
            UserNametesting();
            this.txtOldPass.Focus();

            if (GreenSignal == "YES")
            { 
                    txtUser.Show();
                    label4.Show();
                  //  setcombo();
            }
            else
            {
                txtUser.Hide();
                label4.Hide();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            FormID = "Update";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                String s1 = "Select Count (*) From login where USERNAME = '" + txtUser.Text + "' And PASSWORD = '" + txtOldPass.Text + "'";
                 DataTable dt = clsDataLayer.RetreiveQuery(s1);  

                  errorProvider1.Clear();
               if (dt.Rows[0][0].ToString() == "1") 
                { 
                try
                {
                    if (txtNewPass.Text == txtVerifyPass.Text)
                    {
                        String upd = "UPDATE login Set PASSWORD = '" + txtNewPass.Text + "' Where USERNAME = '" + txtUser.Text + "' And PASSWORD = '" + txtOldPass.Text + "'"; clsDataLayer.ExecuteQuery(upd);
                      
                        MessageBox.Show("Your Password Has Been Changed!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtOldPass.ReadOnly = true;
                        txtNewPass.ReadOnly = true;
                        txtVerifyPass.ReadOnly = true;
                        checkBox1.Checked = false;
                        checkBox2.Checked = false;
                        checkBox3.Checked = false;
                    }
                    else
                    {
                        errorProvider1.SetError(txtNewPass, "Unmatch Password!");
                        errorProvider1.SetError(txtVerifyPass, "Unmatch Password!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                errorProvider1.SetError(txtUser, "Incorrect! Username");
                errorProvider1.SetError(txtOldPass, "Incorrect! Password");
            }
            //con.Close();
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void check()
        {
            if (checkBox1.Checked)
            {
                txtOldPass.Text = txtOldPass.Text;
            } 
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void setcombo()
        {
            string q = "select USERNAME from login";
            txtUser.DataSource = clsDataLayer.RetreiveQuery(q);
            DataTable dt = new DataTable();
            txtUser.DisplayMember = "USERNAME";
            txtUser.ValueMember = "USERNAME";
        
        }
        private void Change_Password_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked){
                txtOldPass.PasswordChar = '\0';
            }
            else
            {
                txtOldPass.PasswordChar = '*';
            }
        }

        private void close(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                txtNewPass.PasswordChar = '\0';
            }
            else
            {
                txtNewPass.PasswordChar = '*';
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                txtVerifyPass.PasswordChar = '\0';
            }
            else
            {
                txtVerifyPass.PasswordChar = '*';
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            txtOldPass.Clear();
            txtNewPass.Clear();
            txtVerifyPass.Clear();
            txtUser.Refresh();
        }

    }
}
