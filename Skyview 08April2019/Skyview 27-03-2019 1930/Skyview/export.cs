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

    public partial class export : Form

    {

        ReportDocument rpt = new ReportDocument();

        public export(Object obj, String d1, String d2, String Waiter, decimal expense)
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
            decimal discount = 0;
            String s7 = "select sum(TotalDiscount) from tbl_Sale where Date between '" + d1 + "' and '" + d2 + "'"; DataTable fg = clsDataLayer.RetreiveQuery(s7);
            if (fg.Rows.Count > 0)
            {
                hyj = fg.Rows[0][0].ToString();
                discount = Convert.ToDecimal(hyj.ToString());
            }

            String hyj_strn = "";
           
            


            ParameterField p1 = new ParameterField();
            ParameterDiscreteValue s1 = new ParameterDiscreteValue();
            p1.ParameterFieldName = "TotalDis";
            s1.Value = hyj;
            p1.CurrentValues.Add(s1);



            pfield.Add(pdtFrom); pfield.Add(pf);
            pfield.Add(pfe);
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

            String query = "select * from SaleView where Date between '" + datee + "' and '" + datee + "' order by SaleCode asc";
            DataTable grab = clsDataLayer.RetreiveQuery(query);
            if (grab.Rows.Count > 0)
            {

            }


            rpt.Load(@"d:\codev_software\pos\Debug\Reports\Sale_Inventory.rpt");

            rpt.SetDataSource(grab);

            ExportOptions CrExportOptions;
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
            CrDiskFileDestinationOptions.DiskFileName = "d:\\codev_software\\backup_reports\\SampleReport.pdf";
            CrExportOptions = rpt.ExportOptions;
            {
                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                CrExportOptions.FormatOptions = CrFormatTypeOptions;
            }
            
            rpt.SetParameterValue("Waiter", Waiter);
            rpt.SetParameterValue("expense", Convert.ToInt32(expense));
            rpt.SetParameterValue("Date_From", d1);
            rpt.SetParameterValue("Date_To", d2);
            rpt.SetParameterValue("TotalDis", discount);
           
            rpt.Export();
            
            
        }

        

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }



   

        

    }

}


