using CrystalDecisions.Shared;
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
    public partial class Pur_inv_rpt_viewer : Form
    {
        public Pur_inv_rpt_viewer(Object obj, String d1, String d2, String Waiter, decimal expense)
        {
            InitializeComponent();
            ParameterFields pfield = new ParameterFields();

            ParameterField pf = new ParameterField();
            ParameterDiscreteValue pv = new ParameterDiscreteValue();
            pf.ParameterFieldName = "Waiter";
            pv.Value = Waiter;
            pf.CurrentValues.Add(pv);

            ParameterField pfe = new ParameterField();
            ParameterDiscreteValue pve = new ParameterDiscreteValue();
            pfe.ParameterFieldName = "expense";
            pve.Value = expense;
            pfe.CurrentValues.Add(pve);


            ParameterField ptitle = new ParameterField();
            ParameterDiscreteValue pvalue = new ParameterDiscreteValue();
            ptitle.ParameterFieldName = "Date_From";
            pvalue.Value = d1;
            ptitle.CurrentValues.Add(pvalue);

            ParameterField pdtFrom = new ParameterField();
            ParameterDiscreteValue dtFromValue = new ParameterDiscreteValue();
            pdtFrom.ParameterFieldName = "Date_To";
            dtFromValue.Value = d2;
            pdtFrom.CurrentValues.Add(dtFromValue);

            String hyj = "";
            String s7 = "select sum(TotalDiscount) from tbl_Sale where Date between '" + d1 + "' and '" + d2 + "'"; DataTable fg = clsDataLayer.RetreiveQuery(s7);
            if (fg.Rows.Count > 0)
            {
                hyj = fg.Rows[0][0].ToString();
            }

          

            ParameterField p1 = new ParameterField();
            ParameterDiscreteValue s1 = new ParameterDiscreteValue();
            p1.ParameterFieldName = "TotalDis";
            s1.Value = hyj;
            p1.CurrentValues.Add(s1);

         


            pfield.Add(pdtFrom); pfield.Add(pf);
            pfield.Add(pfe);
            pfield.Add(ptitle); pfield.Add(p1); 
            crystalReportViewer1.ParameterFieldInfo = pfield;
            crystalReportViewer1.ReportSource = obj;
            crystalReportViewer1.Refresh();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            //   ParameterField pdtFrom = new ParameterField();
            // ParameterDiscreteValue dtFromValue = new ParameterDiscreteValue();
            //pdtFrom.ParameterFieldName = "fromdate";
            //dtFromValue.Value = dtFrom;
            //pdtFrom.CurrentValues.Add(dtFromValue);



        }
    }
}
