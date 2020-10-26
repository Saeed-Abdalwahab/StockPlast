using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using StockPortal.Helpers;
using Transport.App_Start;
using Microsoft.Owin.Security;
using System.Net;
using PlasticStatic;

namespace StockPortal
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {


            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
        }

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath != null && HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
        }
        private void Session_End(object sender, EventArgs e)
        {
            Sessions.FreeUpSessions();
        }


        //private static void FreeUpSessions()
        //{
        //    System.Web.HttpContext.Current.Session["UserRoles"] = null;
        //    System.Web.HttpContext.Current.Session["WorkerId"] = null;
        //    System.Web.HttpContext.Current.Session["EmployeeName"] = null;
        //    System.Web.HttpContext.Current.Session["EmployeeUserName"] = null;
        //    System.Web.HttpContext.Current.Session["UserId"] = null;
        //    System.Web.HttpContext.Current.Session["UserTypeId"] = null;
        //    System.Web.HttpContext.Current.Session["CompanyId"] = null;
        //    System.Web.HttpContext.Current.Session["CompanyName"] = null;
        //    System.Web.HttpContext.Current.Session["ManagementId"] = null;
        //    System.Web.HttpContext.Current.Session["ManagementName"] = null;
        //    System.Web.HttpContext.Current.Session["ManagementSpecializedId"] = null;
        //    System.Web.HttpContext.Current.Session["ManagementSpecializedName"] = null;
        //    System.Web.HttpContext.Current.Session["ManagementSpecializedIdList"] = null;
        //    System.Web.HttpContext.Current.Session["ManagementSpecializedNameList"] = null;
        //    System.Web.HttpContext.Current.Session["ManagementSpecializedAreaId"] = null;
        //    System.Web.HttpContext.Current.Session["ManagementSpecializedAreaName"] = null;


        //    System.Web.HttpContext.Current.Session["TransportCenterList"] = null;
        //}

    }
}
