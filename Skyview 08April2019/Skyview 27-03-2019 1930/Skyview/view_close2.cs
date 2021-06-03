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
    public partial class view_close2 : Form
    {
        public view_close2(Object obj, decimal rec, decimal returnn, decimal add_dis, decimal service)
        {
            InitializeComponent();


            ParameterFields pfield = new ParameterFields();

            ParameterField ptitle = new ParameterField();
            ParameterDiscreteValue pvalue = new ParameterDiscreteValue();

            ptitle.ParameterFieldName = "cash_rev";
            pvalue.Value = rec;
            ptitle.CurrentValues.Add(pvalue);

            ParameterField p1title = new ParameterField();
            ParameterDiscreteValue p1value = new ParameterDiscreteValue();

            p1title.ParameterFieldName = "cash_return";
            p1value.Value = returnn;
            p1title.CurrentValues.Add(p1value);



            ParameterField ptitle2 = new ParameterField();
            ParameterDiscreteValue pvalue2 = new ParameterDiscreteValue();

            ptitle2.ParameterFieldName = "add_discount";
            pvalue2.Value = add_dis;
            ptitle2.CurrentValues.Add(pvalue2);


            ParameterField ptitle3 = new ParameterField();
            ParameterDiscreteValue pvalue3 = new ParameterDiscreteValue();
            ptitle3.ParameterFieldName = "service";
            pvalue3.Value = service;
            ptitle3.CurrentValues.Add(pvalue3);



            pfield.Add(p1title); pfield.Add(ptitle); pfield.Add(ptitle2); pfield.Add(ptitle3);
            crystalReportViewer1.ParameterFieldInfo = pfield;
            crystalReportViewer1.ReportSource = obj;
            crystalReportViewer1.Refresh();

        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
