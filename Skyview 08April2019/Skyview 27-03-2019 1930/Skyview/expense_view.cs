using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.Shared;
namespace Skyview
{
    public partial class expense_view : Form
    {
        public expense_view(Object obj, String d1, String d2)
        {
            InitializeComponent();

            ParameterFields pfield = new ParameterFields();

            //ParameterField pf = new ParameterField();
            //ParameterDiscreteValue pv = new ParameterDiscreteValue();
            //pf.ParameterFieldName = "total_sale";
            //pv.Value = sts;
            //pf.CurrentValues.Add(pv);


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

            //pfield.Add(pf); 
            pfield.Add(ptitle); pfield.Add(pdtFrom);

            crystalReportViewer1.ParameterFieldInfo = pfield;

            crystalReportViewer1.ReportSource = obj;
            crystalReportViewer1.Refresh();



        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
