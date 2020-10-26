using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlasticStatic
{
    public class Sessions
    {
        public static string[] UserRoles
        {
            get
            {
                if (HttpContext.Current.Session["UserRoles"] != null)
                {
                    return (string[])HttpContext.Current.Session["UserRoles"];
                }

                return null;
            }
            set
            {
                HttpContext.Current.Session["UserRoles"] = value;

            }
        }
        public static int WorkerId
        {
            get
            {
                if (HttpContext.Current.Session["WorkerId"] != null)
                {
                    return int.Parse(HttpContext.Current.Session["WorkerId"].ToString());
                }

                return 0;
            }
            set
            {
                HttpContext.Current.Session["WorkerId"] = value;

            }
        }
        public static string EmployeeName
        {
            get
            {
                if (HttpContext.Current.Session["EmployeeName"] != null)
                {
                    return HttpContext.Current.Session["EmployeeName"].ToString();
                }

                return "";
            }
            set
            {
                HttpContext.Current.Session["EmployeeName"] = value;

            }
        }
        public static string EmployeeUserName
        {
            get
            {
                if (HttpContext.Current.Session["EmployeeUserName"] != null)
                {
                    return HttpContext.Current.Session["EmployeeUserName"].ToString();
                }

                return "";
            }
            set
            {
                HttpContext.Current.Session["EmployeeUserName"] = value;

            }
        }
        public static string UserId
        {
            get
            {
                if (HttpContext.Current.Session["UserId"] != null)
                {
                    return HttpContext.Current.Session["UserId"].ToString();
                }

                return "";
            }
            set
            {
                HttpContext.Current.Session["UserId"] = value;

            }
        }
        public static int UserTypeId
        {
            get
            {
                if (HttpContext.Current.Session["UserTypeId"] != null)
                {
                    return int.Parse(HttpContext.Current.Session["UserTypeId"].ToString());
                }

                return 0;
            }
            set
            {
                HttpContext.Current.Session["UserTypeId"] = value;

            }
        }
        public static int CompanyId
        {
            get
            {
                if (HttpContext.Current.Session["CompanyId"] != null)
                {
                    return int.Parse(HttpContext.Current.Session["CompanyId"].ToString());
                }

                return 0;
            }
            set
            {
                HttpContext.Current.Session["CompanyId"] = value;

            }
        }
        public static string CompanyName
        {
            get
            {
                if (HttpContext.Current.Session["CompanyName"] != null)
                {
                    return HttpContext.Current.Session["CompanyName"].ToString();
                }

                return "";
            }
            set
            {
                HttpContext.Current.Session["CompanyName"] = value;

            }
        }
        public static int ManagementId
        {
            get
            {
                if (HttpContext.Current.Session["ManagementId"] != null)
                {
                    return int.Parse(HttpContext.Current.Session["ManagementId"].ToString());
                }

                return 0;
            }
            set
            {
                HttpContext.Current.Session["ManagementId"] = value;

            }
        }
        public static string ManagementName
        {
            get
            {
                if (HttpContext.Current.Session["ManagementName"] != null)
                {
                    return HttpContext.Current.Session["ManagementName"].ToString();
                }

                return "";
            }
            set
            {
                HttpContext.Current.Session["ManagementName"] = value;

            }
        }
        
        public static void FreeUpSessions()
        {
            HttpContext.Current.Session["UserRoles"] = null;
            HttpContext.Current.Session["WorkerId"] = null;
            HttpContext.Current.Session["EmployeeName"] = null;
            HttpContext.Current.Session["EmployeeUserName"] = null;
            HttpContext.Current.Session["UserId"] = null;
            HttpContext.Current.Session["UserTypeId"] = null;
            HttpContext.Current.Session["CompanyId"] = null;
            HttpContext.Current.Session["CompanyName"] = null;
            HttpContext.Current.Session["ManagementId"] = null;
            HttpContext.Current.Session["ManagementName"] = null;
     
        }
    }
}
