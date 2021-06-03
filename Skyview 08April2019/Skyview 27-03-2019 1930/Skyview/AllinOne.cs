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
    public partial class AllinOne : Form
    {
        public AllinOne(Object obj,decimal r_b)
        {
            InitializeComponent();
            ParameterFields pfield = new ParameterFields();

            ParameterField pf = new ParameterField();
            ParameterDiscreteValue pv = new ParameterDiscreteValue();
            pf.ParameterFieldName = "expense";
            pv.Value = r_b;
            pf.CurrentValues.Add(pv);

            pfield.Add(pf);

            crystalReportViewer1.ParameterFieldInfo = pfield;
            crystalReportViewer1.ReportSource = obj;
            crystalReportViewer1.Refresh();

        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
