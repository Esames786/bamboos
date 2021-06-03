using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;



namespace Skyview

{

    public partial class expense_report_view : Form

    {

        ReportDocument rpt = new ReportDocument();

        public expense_report_view(Object obj, String d1, String d2)
        {
            InitializeComponent();
            ParameterFields pfield = new ParameterFields();


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

            string s7 = "Select * from expense where date between '" + d1 + "' and '" + d2 + "'";
            DataTable fg = clsDataLayer.RetreiveQuery(s7);
        
          
            if (fg.Rows.Count > 0)
            {
              
            }
            

            pfield.Add(pdtFrom); 
            pfield.Add(ptitle); 
            crystalReportViewer1.ParameterFieldInfo = pfield;
            crystalReportViewer1.ReportSource = obj;
            crystalReportViewer1.Refresh();


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

            string query = "Select * from expense where date between '" + datee + "' and '" + datee + "'";
            DataTable grab = clsDataLayer.RetreiveQuery(query);
            if (grab.Rows.Count > 0)
            {

            }


            rpt.Load(@"d:\codev_software\pos\Debug\Reports\expense_report.rpt");

            rpt.SetDataSource(grab);

            ExportOptions CrExportOptions;
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
            CrDiskFileDestinationOptions.DiskFileName = "d:\\codev_software\\backup_reports\\expense_report.pdf";
            CrExportOptions = rpt.ExportOptions;
            {
                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                CrExportOptions.FormatOptions = CrFormatTypeOptions;
            }
            
            
            rpt.SetParameterValue("Date_From", d1);
            rpt.SetParameterValue("Date_To", d2);
            rpt.Export();
            
            
        }

        

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }



   

        

    }

}


