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
using Microsoft.Reporting.WinForms;
using PlasticStatic;
using ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode;
using ReportViewer = Microsoft.Reporting.WebForms.ReportViewer;
using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WebForms.Internal.Soap.ReportingServices2005.Execution;
using IReportServerCredentials = Microsoft.Reporting.WebForms.IReportServerCredentials;

namespace StockPortal.Controllers
{
    public class SecurityReportsController : Controller
    {
        // GET: SecurityReports
        //public ActionResult Index()
        //{
        //    return View();
        //}
        // MyDataSet ds = new MyDataSet();
      

       

        public ActionResult UserActivity()
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = 900; //Unit.Percentage(900);
            reportViewer.Height = 900; //Unit.Percentage(900);

            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            reportViewer.ServerReport.ReportServerUrl =
                new Uri(ConfigurationManager.ConnectionStrings["ReportServerURL"].ConnectionString);

            IReportServerCredentials irsc = new Helpers.CustomReportCredentials(
                ConfigurationManager.ConnectionStrings["ReportServerUser"].ConnectionString,
                ConfigurationManager.ConnectionStrings["ReportServerPass"].ConnectionString,
                ConfigurationManager.ConnectionStrings["ReportServerDomin"].ConnectionString);

            reportViewer.ServerReport.ReportServerCredentials = irsc;
            reportViewer.ServerReport.ReportPath = "/Training/UserActivity";

            reportViewer.ShowParameterPrompts = true;
            reportViewer.ShowPrintButton = true;
            reportViewer.ShowRefreshButton = true;

            

            Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection =
                new Microsoft.Reporting.WebForms.ReportParameter[2];
            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter();
            reportParameterCollection[0].Name = "ManagementSpecializedName";
            //foreach (var managementSpecializedName in Sessions.ManagementSpecializedNameList)
            //{
            //    reportParameterCollection[0].Values.Add(managementSpecializedName);
            //}
            //*****
            reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter();
            //var ListOfAllComapany = ""; GetAllCompanyNames();
            //reportParameterCollection[1].Name = "CompanyName";
            //foreach (var CompanyName in ListOfAllComapany)
            //{
            //    reportParameterCollection[1].Values.Add(CompanyName);
            //}

            //  reportParameterCollection[1].Name = "CompanyName";
            // reportParameterCollection[1].Values.Add(Sessions.CompanyName);

            reportViewer.ServerReport.SetParameters(reportParameterCollection);

            reportViewer.ServerReport.Refresh();
            ViewBag.ReportViewer = reportViewer;




            return View();
        }

        public ActionResult ManagementSpecializedWorkFollow()
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = 900; //Unit.Percentage(900);
            reportViewer.Height = 900; //Unit.Percentage(900);

            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            reportViewer.ServerReport.ReportServerUrl =
                new Uri(ConfigurationManager.ConnectionStrings["ReportServerURL"].ConnectionString);
            IReportServerCredentials irsc = new Helpers.CustomReportCredentials(
                ConfigurationManager.ConnectionStrings["ReportServerUser"].ConnectionString,
                ConfigurationManager.ConnectionStrings["ReportServerPass"].ConnectionString,
                ConfigurationManager.ConnectionStrings["ReportServerDomin"].ConnectionString);

            reportViewer.ServerReport.ReportServerCredentials = irsc;
            reportViewer.ServerReport.ReportPath = "/Training/TrainingEventsByMangmentSNameTotals";

            reportViewer.ShowParameterPrompts = true;
            reportViewer.ShowPrintButton = true;
            reportViewer.ShowRefreshButton = true;

            Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection =
                new Microsoft.Reporting.WebForms.ReportParameter[2];
            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter();
            reportParameterCollection[0].Name = "ManagementSpecializedName";
            //foreach (var managementSpecializedName in Sessions.ManagementSpecializedNameList)
            //{
            //    reportParameterCollection[0].Values.Add(managementSpecializedName);
            //}


            //*****
            reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter();
           // var ListOfAllComapany = GetAllCompanyNames();
            reportParameterCollection[1].Name = "CompanyName";
            //foreach (var CompanyName in ListOfAllComapany)
            //{
            //    reportParameterCollection[1].Values.Add(CompanyName);
            //}

            // reportParameterCollection[1].Name = "CompanyName";
            // reportParameterCollection[1].Values.Add(Sessions.CompanyName);

            reportViewer.ServerReport.SetParameters(reportParameterCollection);

            reportViewer.ServerReport.Refresh();
            ViewBag.ReportViewer = reportViewer;




            return View();
        }

        public ActionResult UserWorkFollow()
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = 900; //Unit.Percentage(900);
            reportViewer.Height = 900; //Unit.Percentage(900);

             reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            reportViewer.ServerReport.ReportServerUrl =
                new Uri(ConfigurationManager.ConnectionStrings["ReportServerURL"].ConnectionString);
             reportViewer.ProcessingMode = ProcessingMode.Remote;




            //ReportExecutionService service = new ReportExecutionService();
            //service.Url = "/Training/TrainingEventsByMangmentSName";
            //service.Credentials = new System.Net.NetworkCredential("R.fanous", "", "EehcHosting.com");

            //Specify the server credentials
            //ReportServerCredentials reportCredentials = new ReportServerCredentials();
            // reportViewer.ServerReport.ReportServerCredentials = reportCredentials;


            IReportServerCredentials irsc = new Helpers.CustomReportCredentials(
                ConfigurationManager.ConnectionStrings["ReportServerUser"].ConnectionString,
                ConfigurationManager.ConnectionStrings["ReportServerPass"].ConnectionString,
            ConfigurationManager.ConnectionStrings["ReportServerDomin"].ConnectionString);

        reportViewer.ServerReport.ReportServerCredentials = irsc;


            reportViewer.ServerReport.ReportPath = "/Training/TrainingEventsByMangmentSName";
            reportViewer.ShowCredentialPrompts = true;
            reportViewer.ShowParameterPrompts = true;
            reportViewer.ShowPrintButton = true;
            reportViewer.ShowRefreshButton = true;

            Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection =new Microsoft.Reporting.WebForms.ReportParameter[2];
            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter();
            reportParameterCollection[0].Name = "ManagementSpecializedName";
            //foreach (var managementSpecializedName in Sessions.ManagementSpecializedNameList)
            //{
            //    reportParameterCollection[0].Values.Add(managementSpecializedName);
            //}
            
            //*****
            reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter();
            //var ListOfAllComapany = GetAllCompanyNames();
            reportParameterCollection[1].Name = "CompanyName";
            //foreach (var CompanyName in ListOfAllComapany)
            //{
            //    reportParameterCollection[1].Values.Add(CompanyName);
            //}


            // reportParameterCollection[1].Name = "CompanyName";
            //reportParameterCollection[1].Values.Add(string.Empty);

            reportViewer.ServerReport.SetParameters(reportParameterCollection);

            reportViewer.ServerReport.Refresh();
            ViewBag.ReportViewer = reportViewer;




            return View();
        }


        public ActionResult UserCharts()
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = 900; //Unit.Percentage(900);
            reportViewer.Height = 900; //Unit.Percentage(900);

            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            reportViewer.ServerReport.ReportServerUrl =
                new Uri(ConfigurationManager.ConnectionStrings["ReportServerURL"].ConnectionString);
            reportViewer.ProcessingMode = ProcessingMode.Remote;




            //ReportExecutionService service = new ReportExecutionService();
            //service.Url = "/Training/TrainingEventsByMangmentSName";
            //service.Credentials = new System.Net.NetworkCredential("R.fanous", "", "EehcHosting.com");

            //Specify the server credentials
            //ReportServerCredentials reportCredentials = new ReportServerCredentials();
            // reportViewer.ServerReport.ReportServerCredentials = reportCredentials;


            IReportServerCredentials irsc = new Helpers.CustomReportCredentials(
                ConfigurationManager.ConnectionStrings["ReportServerUser"].ConnectionString,
                ConfigurationManager.ConnectionStrings["ReportServerPass"].ConnectionString,
                ConfigurationManager.ConnectionStrings["ReportServerDomin"].ConnectionString);

            reportViewer.ServerReport.ReportServerCredentials = irsc;


            reportViewer.ServerReport.ReportPath = "/Training/SecurityReports/UserCharts";
            reportViewer.ShowCredentialPrompts = true;
            reportViewer.ShowParameterPrompts = true;
            reportViewer.ShowPrintButton = true;
            reportViewer.ShowRefreshButton = true;

            //Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[2];
            //reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter();
            //reportParameterCollection[0].Name = "ManagementSpecializedName";
            //foreach (var managementSpecializedName in Sessions.ManagementSpecializedNameList)
            //{
            //    reportParameterCollection[0].Values.Add(managementSpecializedName);
            //}

            ////*****
            //reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter();
            //var ListOfAllComapany = GetAllCompanyNames();
            //reportParameterCollection[1].Name = "CompanyName";
            //foreach (var CompanyName in ListOfAllComapany)
            //{
            //    reportParameterCollection[1].Values.Add(CompanyName);
            //}


            //// reportParameterCollection[1].Name = "CompanyName";
            ////reportParameterCollection[1].Values.Add(string.Empty);

            //reportViewer.ServerReport.SetParameters(reportParameterCollection);

            reportViewer.ServerReport.Refresh();
            ViewBag.ReportViewer = reportViewer;




            return View();
        }

        //************************************
        //[Serializable]
        //public sealed class MyReportServerCredentials :
        //    IReportServerCredentials
        //{
        //    public WindowsIdentity ImpersonationUser
        //    {
        //        get
        //        {
        //            // Use the default Windows user.  Credentials will be
        //            // provided by the NetworkCredentials property.
        //            return null;
        //        }
        //    }

        //    public ICredentials NetworkCredentials
        //    {
        //        get
        //        {
        //            // Read the user information from the Web.config file.  
        //            // By reading the information on demand instead of 
        //            // storing it, the credentials will not be stored in 
        //            // session, reducing the vulnerable surface area to the
        //            // Web.config file, which can be secured with an ACL.

        //            // User name
        //            string userName =
        //                ConfigurationManager.AppSettings
        //                    ["MyReportViewerUser"];

        //            if (string.IsNullOrEmpty(userName))
        //                throw new Exception(
        //                    "Missing user name from web.config file");

        //            // Password
        //            string password =
        //                ConfigurationManager.AppSettings
        //                    ["MyReportViewerPassword"];

        //            if (string.IsNullOrEmpty(password))
        //                throw new Exception(
        //                    "Missing password from web.config file");

        //            // Domain
        //            string domain =
        //                ConfigurationManager.AppSettings
        //                    ["MyReportViewerDomain"];

        //            if (string.IsNullOrEmpty(domain))
        //                throw new Exception(
        //                    "Missing domain from web.config file");

        //            return new NetworkCredential(userName, password, domain);
        //        }
        //    }

        //    public bool GetFormsCredentials(out Cookie authCookie,
        //        out string userName, out string password,
        //        out string authority)
        //    {
        //        authCookie = null;
        //        userName = null;
        //        password = null;
        //        authority = null;

        //        // Not using form credentials
        //        return false;
        //    }

        //}
        //***************




        //******************
    }


}