using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.UI.WebControls;

using PlasticStatic;
using ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode;
using ReportViewer = Microsoft.Reporting.WebForms.ReportViewer;
using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WebForms.Internal.Soap.ReportingServices2005.Execution;
using IReportServerCredentials = Microsoft.Reporting.WebForms.IReportServerCredentials;
using ReportParameter = Microsoft.Reporting.WebForms.ReportParameter;
using StockDB.Model;

namespace StockPortal.Controllers
{
    public class StockReportsController : Controller
    {
        // GET: StockReports
        public ActionResult WorkerReportParam()
        {
            return View();
        }
        public ActionResult WorkingReportByIDs()
        {
            StockModel db = new StockModel();
            //ViewBag.FromStocks = new SelectList(db.FromStocks.ToList(), "ID", "Serial");

            var List = db.FromStocks.Where(x => x.FromStockTypeID == (int)PlasticStatic.Enums.FromStockTypeId.OperationOrder && x.DemondOrder.DemondOrderDetails.Any(y => y.status == (int)PlasticStatic.Enums.demondorderdetailstatus.Duringdelivery || y.status == (int)PlasticStatic.Enums.demondorderdetailstatus.Underconstruction/*|| y.status == (int)PlasticStatic.Enums.demondorderdetailstatus.NotStarted*/)).Select(x => new SelectListItem { Text = x.DemondOrder.Customer.cust_name+" - "+ x.Serial, Value = x.ID.ToString() }).ToList();

            ViewBag.Customers = new SelectList(List, "Value", "Text");

            return View();
        }
        [HttpPost]
        public ActionResult GetWorkingReportByIDs(List<string> CustomersID)
        {
            StockModel db = new StockModel();

          //var FromStockIDs= db.FromStocks.Where(x => CustomersID.Contains(x.DemondOrder.Customer.cust_id)).Select(x => x.ID.ToString()).ToList();

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = 2000; //Unit.Percentage(900);
            reportViewer.Height = 2000; //Unit.Percentage(900);
            reportViewer.SizeToReportContent = true;

            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            reportViewer.ServerReport.ReportServerUrl =
                new Uri(ConfigurationManager.ConnectionStrings["ReportServerURL"].ConnectionString);
            //IReportServerCredentials irsc = new Helpers.CustomReportCredentials(
            //    ConfigurationManager.ConnectionStrings["ReportServerUser"].ConnectionString,
            //    ConfigurationManager.ConnectionStrings["ReportServerPass"].ConnectionString,
            //    ConfigurationManager.ConnectionStrings["ReportServerDomin"].ConnectionString);

            //reportViewer.ServerReport.ReportServerCredentials = irsc;

            reportViewer.ServerReport.ReportPath = "/StockReports/WorkingOrderByIDs";


            //reportViewer.ServerReport.SetParameters(reportParameters);
            reportViewer.ShowParameterPrompts = true;
            reportViewer.ShowPrintButton = true;
            reportViewer.ShowRefreshButton = true;

            ReportParameter reportParameter = new ReportParameter();
            reportParameter.Values.AddRange(CustomersID.ToArray());
            reportParameter.Name = "FromStockIDs";

            reportViewer.ServerReport.SetParameters(reportParameter);


            reportViewer.ServerReport.Refresh();
            ViewBag.ReportViewer = reportViewer;
            return PartialView();
        }

        [HttpPost]
        public ActionResult WorkerReport(string StartDate, string EndDate, string Status)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = 2000; //Unit.Percentage(900);
            reportViewer.Height = 2000; //Unit.Percentage(900);
            reportViewer.SizeToReportContent = true;

            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            reportViewer.ServerReport.ReportServerUrl =
                new Uri(ConfigurationManager.ConnectionStrings["ReportServerURL"].ConnectionString);
            //IReportServerCredentials irsc = new Helpers.CustomReportCredentials(
            //    ConfigurationManager.ConnectionStrings["ReportServerUser"].ConnectionString,
            //    ConfigurationManager.ConnectionStrings["ReportServerPass"].ConnectionString,
            //    ConfigurationManager.ConnectionStrings["ReportServerDomin"].ConnectionString);

            //reportViewer.ServerReport.ReportServerCredentials = irsc;

            reportViewer.ServerReport.ReportPath = "/StockReports/WorkingOrder";


            //reportViewer.ServerReport.SetParameters(reportParameters);
            reportViewer.ShowParameterPrompts = true;
            reportViewer.ShowPrintButton = true;
            reportViewer.ShowRefreshButton = true;

            Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[3];
            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("StartDate", StartDate);
            reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter("EndDate", EndDate);
            reportParameterCollection[2] = new Microsoft.Reporting.WebForms.ReportParameter("DemondOrderDetailsStatus", Status);
            reportViewer.ServerReport.SetParameters(reportParameterCollection);


            reportViewer.ServerReport.Refresh();
            ViewBag.ReportViewer = reportViewer;
            return PartialView();
        }
        public ActionResult DemondOrderParam()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DemondOrderReport(string StartDate, string EndDate)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = 2000; //Unit.Percentage(900);
            reportViewer.Height = 2000; //Unit.Percentage(900);
            reportViewer.SizeToReportContent = true;

            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            reportViewer.ServerReport.ReportServerUrl =
                new Uri(ConfigurationManager.ConnectionStrings["ReportServerURL"].ConnectionString);

            //publish
            //IReportServerCredentials irsc = new Helpers.CustomReportCredentials(
            //    ConfigurationManager.ConnectionStrings["ReportServerUser"].ConnectionString,
            //    ConfigurationManager.ConnectionStrings["ReportServerPass"].ConnectionString,
            //    ConfigurationManager.ConnectionStrings["ReportServerDomin"].ConnectionString);
            //reportViewer.ServerReport.ReportServerCredentials = irsc;

            reportViewer.ServerReport.ReportPath = "/StockReports/DemonDorderReport";


            //reportViewer.ServerReport.SetParameters(reportParameters);
            reportViewer.ShowParameterPrompts = true;
            reportViewer.ShowPrintButton = true;
            reportViewer.ShowRefreshButton = true;

            Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[2];
            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("StartDate", StartDate);
            reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter("EndDate", EndDate);
            reportViewer.ServerReport.SetParameters(reportParameterCollection);


            reportViewer.ServerReport.Refresh();
            ViewBag.ReportViewer = reportViewer;
            return PartialView();
        }

        [HttpGet]
        public ActionResult DemondOrder(int DemondOrderID)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = 2000; //Unit.Percentage(900);
            reportViewer.Height = 2000; //Unit.Percentage(900);
            reportViewer.SizeToReportContent = true;

            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            reportViewer.ServerReport.ReportServerUrl =
                new Uri(ConfigurationManager.ConnectionStrings["ReportServerURL"].ConnectionString);

            //publish
            //IReportServerCredentials irsc = new Helpers.CustomReportCredentials(
            //    ConfigurationManager.ConnectionStrings["ReportServerUser"].ConnectionString,
            //    ConfigurationManager.ConnectionStrings["ReportServerPass"].ConnectionString,
            //    ConfigurationManager.ConnectionStrings["ReportServerDomin"].ConnectionString);
            //reportViewer.ServerReport.ReportServerCredentials = irsc;

            reportViewer.ServerReport.ReportPath = "/StockReports/DemondOrder";


            //reportViewer.ServerReport.SetParameters(reportParameters);
            reportViewer.ShowParameterPrompts = true;
            reportViewer.ShowPrintButton = true;
            reportViewer.ShowRefreshButton = true;

            Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[1];
            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("DemondOrderID", DemondOrderID.ToString());
            reportViewer.ServerReport.SetParameters(reportParameterCollection);


            reportViewer.ServerReport.Refresh();
            ViewBag.ReportViewer = reportViewer;
            return View();
        } [HttpGet]
        public ActionResult SalesReport(int FromStockID)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = 2000; //Unit.Percentage(900);
            reportViewer.Height = 2000; //Unit.Percentage(900);
            reportViewer.SizeToReportContent = true;

            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            reportViewer.ServerReport.ReportServerUrl =
                new Uri(ConfigurationManager.ConnectionStrings["ReportServerURL"].ConnectionString);

            //publish
            //IReportServerCredentials irsc = new Helpers.CustomReportCredentials(
            //    ConfigurationManager.ConnectionStrings["ReportServerUser"].ConnectionString,
            //    ConfigurationManager.ConnectionStrings["ReportServerPass"].ConnectionString,
            //    ConfigurationManager.ConnectionStrings["ReportServerDomin"].ConnectionString);
            //reportViewer.ServerReport.ReportServerCredentials = irsc;

            reportViewer.ServerReport.ReportPath = "/StockReports/SaleingReport";


            //reportViewer.ServerReport.SetParameters(reportParameters);
            reportViewer.ShowParameterPrompts = true;
            reportViewer.ShowPrintButton = true;
            reportViewer.ShowRefreshButton = true;

            Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[1];
            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter("FromStockID", FromStockID.ToString());
            reportViewer.ServerReport.SetParameters(reportParameterCollection);


            reportViewer.ServerReport.Refresh();
            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        [HttpGet]
        public ActionResult CustomersReport()
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = 2000; //Unit.Percentage(900);
            reportViewer.Height = 2000; //Unit.Percentage(900);
            reportViewer.SizeToReportContent = true;

            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            reportViewer.ServerReport.ReportServerUrl =
                new Uri(ConfigurationManager.ConnectionStrings["ReportServerURL"].ConnectionString);

            //publish
            //IReportServerCredentials irsc = new Helpers.CustomReportCredentials(
            //    ConfigurationManager.ConnectionStrings["ReportServerUser"].ConnectionString,
            //    ConfigurationManager.ConnectionStrings["ReportServerPass"].ConnectionString,
            //    ConfigurationManager.ConnectionStrings["ReportServerDomin"].ConnectionString);
            //reportViewer.ServerReport.ReportServerCredentials = irsc;

            reportViewer.ServerReport.ReportPath = "/StockReports/CustomerReport";


            reportViewer.ShowParameterPrompts = true;
            reportViewer.ShowPrintButton = true;
            reportViewer.ShowRefreshButton = true;

          


            reportViewer.ServerReport.Refresh();
            ViewBag.ReportViewer = reportViewer;
            return View();
        }    [HttpGet]
        public ActionResult SupplierReport()
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = 2000; //Unit.Percentage(900);
            reportViewer.Height = 2000; //Unit.Percentage(900);
            reportViewer.SizeToReportContent = true;

            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            reportViewer.ServerReport.ReportServerUrl =
                new Uri(ConfigurationManager.ConnectionStrings["ReportServerURL"].ConnectionString);



            //publish
            //IReportServerCredentials irsc = new Helpers.CustomReportCredentials(
            //    ConfigurationManager.ConnectionStrings["ReportServerUser"].ConnectionString,
            //    ConfigurationManager.ConnectionStrings["ReportServerPass"].ConnectionString,
            //    ConfigurationManager.ConnectionStrings["ReportServerDomin"].ConnectionString);
            //reportViewer.ServerReport.ReportServerCredentials = irsc;

            reportViewer.ServerReport.ReportPath = "/StockReports/SupplierReport";


            reportViewer.ShowParameterPrompts = true;
            reportViewer.ShowPrintButton = true;
            reportViewer.ShowRefreshButton = true;




            System.Drawing.Printing.PageSettings ps = new System.Drawing.Printing.PageSettings();
            ps.Landscape = true;
            ps.PaperSize = new System.Drawing.Printing.PaperSize("A4", 827, 1170);
            ps.PaperSize.RawKind = (int)System.Drawing.Printing.PaperKind.A4;
            reportViewer.SetPageSettings(ps);
            reportViewer.ServerReport.Refresh();
            ViewBag.ReportViewer = reportViewer;
            return View();
        }
    }
}