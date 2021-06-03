using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SqlServer;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Data.SqlClient;
using Skyview.bin.Debug.Reports;
using CrystalDecisions.Shared;
using System.Net.Mail;
using System.Net;

namespace Skyview
{
    public partial class Master_Form : Form
    {

        String serverName = ""; String userName = "";
        String path = ""; DataSet fds = new DataSet();
        String password = "";
        public Master_Form()
        {
            InitializeComponent();
            menuStrip1.Renderer = new MyRenderer();

        }

        private void btnGround_Click(object sender, EventArgs e)
        {
            try
            {
                FoodOrder f = new FoodOrder();
                FormID = "1016";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    f.Show();
                }
                else
                {
                    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }


        private void productCategoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                header_account st = new header_account();
                FormID = "1003";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    st.Show();
                }
                else
                {
                    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }

        private void addProductsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CreateProductInventory st = new CreateProductInventory();
                st.Show();
            }
            catch { }
        }

        private void addSaleProductsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CreateProductSale st = new CreateProductSale();
                FormID = "1006";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    st.Show();
                }
                else
                {
                    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }

        private void Master_Form_FormClosing(object sender, FormClosingEventArgs e)
        {

            DialogResult Result = MessageBox.Show("Mr. " + UID + "! Do want to send email?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (Result == DialogResult.Yes)
            {
                mail();
                Application.Exit(); Environment.Exit(0); Process.GetCurrentProcess().Kill();
            }
            else if (Result == DialogResult.No)
            {
                Application.Exit(); Environment.Exit(0); Process.GetCurrentProcess().Kill();
            }

        }

        private void addTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CreateTable st = new CreateTable();
                FormID = "1009";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    st.Show();
                }
                else
                {
                    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DirectOrder st = new DirectOrder();
                FormID = "1015";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    st.Show();
                }
                else
                {
                    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }

        private void purchaseInventoryReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Pur_inv_rpt_frm st = new Pur_inv_rpt_frm();
                st.Show();
            }
            catch { }
        }

        private void purchaseVoucherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                PurchaseVoucher1cs st = new PurchaseVoucher1cs();
                st.Show();
            }
            catch { }
        }

        private void Master_Form_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.B)
                { DirectOrder st = new DirectOrder(); st.Show(); }
                else if (e.KeyCode == Keys.T) { FoodOrder f = new FoodOrder(); f.Show(); }
            }
            catch { }
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.B)
                { DirectOrder st = new DirectOrder(); st.Show(); }
                else if (e.KeyCode == Keys.T) { FoodOrder f = new FoodOrder(); f.Show(); }
            }
            catch { }

        }

        private void saleReportTodayToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }



        private void serverconnect()
        {
            String re = "select ServerName,UserId,Passwords,Path from tbl_Connection"; DataTable d1 = clsDataLayer.RetreiveQuery(re); if (d1.Rows.Count > 0)
            {
                serverName = d1.Rows[0][0].ToString();
                userName = d1.Rows[0][1].ToString();
                password = d1.Rows[0][2].ToString();
                path = d1.Rows[0][3].ToString();
                string str = "Data Source=" + serverName + ";User ID=" + userName + ";Password=" + password + "";
                // string str = "Data Source=" + serverName + ";Integrated Security=True";

                SqlConnection con = new SqlConnection(str);
                try
                {
                    con.Open();
                    // MessageBox.Show("connection gets established");
                    SqlCommand cmd = new SqlCommand("SELECT  db.[name] as dbname FROM [master].[sys].[databases] db", con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);

                    sda.Fill(fds, "DatabaseName");
                    con.Close();
                    // MessageBox.Show("Server Connected!");
                }
                catch { }
            }
        }

        private void backupDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    FormID = "1017";
                    UserNametesting();
                    if (GreenSignal == "YES")
                    {
                        serverconnect();
                        String DbName = "bamboo"; String DestPath = "path";
                        if (DestPath == "" || DbName == "")
                        {
                            MessageBox.Show("Try to select Database and Destination Folder !");
                        }
                        else
                        {
                            string databaseName = DbName;//dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue.ToString();
                            databaseName = "bamboo";
                            DbName = "bamboo";
                            //Define a Backup object variable.
                            Backup sqlBackup = new Backup();

                            ////Specify the type of backup, the description, the name, and the database to be backed up.
                            sqlBackup.Action = BackupActionType.Database;
                            sqlBackup.BackupSetDescription = "BackUp of:" + databaseName + "on" + DateTime.Now.ToShortDateString();
                            sqlBackup.BackupSetName = "FullBackUp";
                            sqlBackup.Database = databaseName;

                            ////Declare a BackupDeviceItem
                            string destinationPath = DestPath;


                            string backupfileName = DbName + DateTime.Now.ToString("dd-MM-yyyy hh.MM.ss") + ".bak";
                            var y = @"" + path;
                            BackupDeviceItem deviceItem = new BackupDeviceItem(y + "\\" + backupfileName, DeviceType.File);
                            ServerConnection connection = new ServerConnection(serverName);

                            ////To Avoid TimeOut Exception
                            Server sqlServer = new Server(connection);
                            sqlServer.ConnectionContext.StatementTimeout = 60 * 60;

                            Microsoft.SqlServer.Management.Smo.Database db = sqlServer.Databases[databaseName];

                            sqlBackup.Initialize = true;
                            sqlBackup.Checksum = true;
                            sqlBackup.ContinueAfterError = true;

                            ////Add the device to the Backup object.
                            sqlBackup.Devices.Add(deviceItem);
                            ////Set the Incremental property to False to specify that this is a full database backup.
                            sqlBackup.Incremental = false;

                            sqlBackup.ExpirationDate = DateTime.Now.AddDays(3);
                            ////Specify that the log must be truncated after the backup is complete.
                            sqlBackup.LogTruncation = BackupTruncateLogType.Truncate;

                            sqlBackup.FormatMedia = false;
                            ////Run SqlBackup to perform the full database backup on the instance of SQL Server.
                            sqlBackup.SqlBackup(sqlServer);
                            ////Remove the backup device from the Backup object.
                            sqlBackup.Devices.Remove(deviceItem);
                            MessageBox.Show("Successful backup is created!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }


                }
                catch
                {
                    // MessageBox.Show(ex.Message);
                }
            }
            catch { }
        }

        private void serverConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }


        private class MyRenderer : ToolStripProfessionalRenderer
        {
            public MyRenderer() : base(new MyColors()) { }
        }

        private class MyColors : ProfessionalColorTable
        {
            public override Color MenuItemSelected
            {
                get { return Color.Black; }
            }
            public override Color MenuItemSelectedGradientBegin
            {
                get { return Color.Black; }
            }
            public override Color MenuItemSelectedGradientEnd
            {
                get { return Color.Black; }
            }
            public override Color MenuItemPressedGradientEnd
            {
                get { return Color.Black; }
            }
        }

        private Timer _Timer = new Timer();
        String UID = Login.UserID;
        public string GreenSignal = "";
        public string FormID = "";


        public void UserNametesting()
        {
            string query = "SELECT FORM_ID,ACCESS FROM user_access WHERE USENAME = '" + UID + "' AND FORM_ID = '" + FormID + "'";
            DataTable drs = clsDataLayer.RetreiveQuery(query);
            if (drs.Rows.Count > 0)
            {
                String frmid = drs.Rows[0]["FORM_ID"].ToString();
                String Acs = drs.Rows[0]["ACCESS"].ToString();
                if (Acs.Equals("Yes"))
                {
                    GreenSignal = "YES";
                }
                else
                {
                    GreenSignal = "NO";
                }
            }
            else
            {
                GreenSignal = "NO";
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                User_Access st = new User_Access();
                FormID = "1010";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    st.Show();
                }
                else
                {
                    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                Customer_info st = new Customer_info();
                st.Show();
            }
            catch { }
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        public void mail()
        {
            try
            {
                toolStripMenuItem3.Enabled = false;
                toolStripMenuItem3.Text = "Wait..Loading";
                string datee = "";
                string qq2 = "Select date from  date_checker";
                DataTable dtt2 = clsDataLayer.RetreiveQuery(qq2);
                if (dtt2.Rows.Count > 0)
                {
                    datee = dtt2.Rows[0][0].ToString();
                }
                else
                {
                    datee = DateTime.Now.ToString("yyyy-MM-dd");
                }


                decimal net = 0;
                string qq1 = "Select sum(amount) from expense where date between '" + datee + "' and '" + datee + "'";
                DataTable dtt1 = clsDataLayer.RetreiveQuery(qq1);
                if (dtt1.Rows.Count > 0)
                {
                    string check = dtt1.Rows[0][0].ToString();
                    if (check != "")
                    {
                        net = Convert.ToDecimal(dtt1.Rows[0][0].ToString());
                    }

                }

                string qq = "Select * from expense";
                DataTable dtt = clsDataLayer.RetreiveQuery(qq);
                expense_report sw = new expense_report();
               
                

                String query = "select * from SaleView where Date between '" + datee + "' and '" + datee + "' order by SaleCode asc";
                DataTable dt = clsDataLayer.RetreiveQuery(query);
                if (dt.Rows.Count > 0)
                {
                    Sale_Inventory pi = new Sale_Inventory();
                    pi.SetDataSource(dt);
                    expense_report_view sww =  new expense_report_view(sw, datee, datee);
                    export pv = new export(pi, datee, datee, "-", net);
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("codevsolution@gmail.com");
                    mail.Sender = new MailAddress("codevsolution@gmail.com");
                    mail.To.Add("dawoodkhan004@gmail.com");
                    mail.IsBodyHtml = true;
                    mail.Subject = "Dail Sale Report";
                    mail.Body = "Dear Concern download the follwing attachment in PDF format";                    
                    mail.Attachments.Add(new Attachment("d:\\codev_software\\backup_reports\\SampleReport.pdf"));
                    mail.Attachments.Add(new Attachment("d:\\codev_software\\backup_reports\\expense_report.pdf"));

                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.UseDefaultCredentials = false;

                    smtp.Credentials = new System.Net.NetworkCredential("codevsolution@gmail.com", "Sms03471220721");
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;

                    smtp.Timeout = 30000;
                    try
                    {

                        smtp.Send(mail);
                        MessageBox.Show("Success");
                        toolStripMenuItem3.Enabled = true;

                        toolStripMenuItem3.Text = "MAIL DATA";
                    }
                    catch
                    {

                    }


                }
                else
                {
                    MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {

            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            mail();
            toolStripMenuItem3.Enabled = true;

            toolStripMenuItem3.Text = "MAIL DATA";
        }



        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            try
            {
                Add_User st = new Add_User();
                FormID = "1000";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    st.Show();
                }
                else
                {
                    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            try
            {
                Change_Password st = new Change_Password();
                FormID = "1007";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    st.Show();
                }
                else
                {
                    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch
            {


            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            try
            {
                deal st = new deal();
                FormID = "1014";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    st.Show();
                }
                else
                {
                    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch
            {


            }
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            try
            {
                expense st = new expense();
                st.Show();
            }
            catch
            {


            }
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            try
            {
                InvRawProduct st = new InvRawProduct();
                st.Show();
            }
            catch { }

        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            try
            {
                InvReceipeProduct st = new InvReceipeProduct();
                st.Show();
            }
            catch { }

        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult Result = MessageBox.Show("Mr. " + UID + "! Do want to send email?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (Result == DialogResult.Yes)
                {
                    toolStripMenuItem10.Enabled = false;
                    toolStripMenuItem10.Text = "Waitt.....";
                    mail();
                    toolStripMenuItem10.Enabled = true;
                    toolStripMenuItem10.Text = "LogOut";

                    //run the program again and close this one
                    Process.Start(Application.StartupPath + "\\Skyview.exe");
                    //or you can use Application.ExecutablePath

                    //close this one
                    Process.GetCurrentProcess().Kill();

                }
                else if (Result == DialogResult.No)
                {
                    //run the program again and close this one
                    Process.Start(Application.StartupPath + "\\Skyview.exe");
                    //or you can use Application.ExecutablePath

                    //close this one
                    Process.GetCurrentProcess().Kill();
                }


              
               


            }
            catch
            { };
        }

        private void allReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChartofAccount frm = new frmChartofAccount(); frm.Show();
        }

        private void multipleProductReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddBankDetail frm = new AddBankDetail(); frm.Show();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            try
            {
                View_Product_Stock st = new View_Product_Stock();
                st.Show();
            }
            catch { }
        }

        private void purchaseInventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                PurchaseVoucher1cs st = new PurchaseVoucher1cs();
                st.Show();
            }
            catch { }
        }

        private void purchaseReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Purchase_Sale st = new Purchase_Sale();
                st.Show();
            }
            catch { }
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            try
            {
                InvProductConsumption st = new InvProductConsumption();
                st.Show();
            }
            catch { }
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            try
            {
                order_check ock = new order_check();

                FormID = "1008";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    ock.Show();
                }
                else
                {
                    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch
            { }
        }

        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {

            try
            {
                Pur_inv_rpt_frm st = new Pur_inv_rpt_frm();
                FormID = "1008";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    st.Show();
                }
                else
                {
                    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }

        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            try
            {
                find st = new find();
                st.Show();


            }
            catch { }
        }

        private void toolStripMenuItem17_Click(object sender, EventArgs e)
        {
            Receipt_voucher frm = new Receipt_voucher(); frm.Show();
        }

        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
            Payment_voucher frm = new Payment_voucher(); frm.Show();
        }

        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
            string q = "select * from PaymentDue where DueAmount!=0 and PartyCode!=528";
            DataTable purchase = clsDataLayer.RetreiveQuery(q);
            if (purchase.Rows.Count > 0)
            {
                RemainingPaymentDue rpt = new RemainingPaymentDue();
                rpt.SetDataSource(purchase);
                PaymentView pop = new PaymentView(rpt);
                pop.Show();
            }
            else
            {
                MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItem19_Click(object sender, EventArgs e)
        {
            AllReports frm = new AllReports(); frm.Show();
        }

        private void Master_Form_Load(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem20_Click(object sender, EventArgs e)
        {
            waiters ST = new waiters();
            ST.Show();
        }

        private void toolStripMenuItem21_Click(object sender, EventArgs e)
        {
            member ST = new member();
            ST.Show();
        }




    }
}
