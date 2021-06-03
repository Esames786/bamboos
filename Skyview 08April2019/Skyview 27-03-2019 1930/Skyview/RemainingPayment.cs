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
    public partial class RemainingPayment : Form
    {
        public RemainingPayment()
        {
            InitializeComponent(); show();
        }

        private void show()
        {
            grid.Rows.Clear(); decimal y = 0;
            String Scode = ""; String Table = ""; String Floor = ""; String Waiter = "";
            String h = "select distinct SaleCode from tbl_SaleByTableQ where TableStatus='Active'"; DataTable ds = clsDataLayer.RetreiveQuery(h);
            if (ds.Rows.Count > 0)
            { }

            //

            //

            for (int b = 0; b < ds.Rows.Count; b++)
            {
                Scode = ds.Rows[b][0].ToString();
                String h2 = "select distinct SaleCode,TableNo,FloorNo,Waiter from tbl_SaleByTableQ where SaleCode='" + Scode + "'"; DataTable ds2 = clsDataLayer.RetreiveQuery(h2);
                if (ds2.Rows.Count > 0)
                {
                    Table = ds2.Rows[0]["TableNo"].ToString(); Floor = ds2.Rows[0]["FloorNo"].ToString(); Waiter = ds2.Rows[0]["Waiter"].ToString();
                }
                String tt = "select Gross_Amount,TotalDiscount,Net_Amount from tbl_Sale where DSaleCode='" + Scode + "'"; DataTable gg = clsDataLayer.RetreiveQuery(tt);
                if (gg.Rows.Count > 0)
                {
                    foreach (DataRow dr in gg.Rows)
                    {
                        int n = grid.Rows.Add();
                        grid.Rows[n].Cells[0].Value = Scode; grid.Rows[n].Cells[1].Value = Floor; grid.Rows[n].Cells[2].Value = Table;
                        grid.Rows[n].Cells[3].Value = dr["Gross_Amount"].ToString(); grid.Rows[n].Cells[4].Value = dr["TotalDiscount"].ToString(); grid.Rows[n].Cells[5].Value = dr["Net_Amount"].ToString();
                    }
                    y += Convert.ToDecimal(gg.Rows[0][2].ToString());
                    //txttotal.Text = jk.ToString();
                }
            }
            txttotal.Text = y.ToString();
        }

        private void txttotal_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
