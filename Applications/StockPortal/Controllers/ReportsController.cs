//using DevExpress.XtraReports.UI;
//using StockPortal.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using DevExpress.Web.Mvc;


//namespace StockPortal.Controllers
//{
//    public class ReportsController : Controller
//    {

//        //protected SigmaScanWebContext db = new SigmaScanWebContext();

//        protected ReportInfo GetReport(string id)
//        {
//            if (string.IsNullOrWhiteSpace(id))
//                return null;

//            string typeName = "StockPortal.Reports." + id.Trim().Replace("-", ".");

//            Type type = Type.GetType(typeName, false, true);
//            if (type == null)
//                type = Type.GetType(typeName, false, true);

//            if (type == null)
//                return null;

//            XtraReport report = Activator.CreateInstance(type) as XtraReport;
//            if (report == null)
//                return null;

//            return new ReportInfo(id, report);
//        }
//        public ActionResult Index(string id/*, int costCenterID*/)
//        {
//            ReportInfo info = GetReport(id);
//            //this.Parameters["hdnCostCenterID"].Value = costCenterID;

//            if (info == null)
//                return HttpNotFound();

//            return View(info);
//        }
//        public ActionResult ViewReport(string id)
//        {
//            ReportInfo info = GetReport(id);
//            //var user = db.Users.Find(SiteInfo.UserID);
//            if (info == null)
//                return HttpNotFound();
//            FormCollection formCollection = new FormCollection();
//            formCollection = Session["parameters"] as FormCollection;
//            info.Report.Parameters["UserName"].Value = "ahmed ghazy";
//            info.Report.Parameters["CurrentDate"].Value = DateTime.Now;
//            info.Report.Parameters["UserName"].Visible = false;
//            info.Report.Parameters["CurrentDate"].Visible = false;

//            if (formCollection != null)
//            {
//                foreach (var key in formCollection.AllKeys)
//                {
//                    string name = key.ToString();
//                    if (info.Report.Parameters[key.ToString()] != null)
//                    {
//                        try
//                        {
//                            info.Report.Parameters[key.ToString()].Value = formCollection[key];
//                            if (info.Report.Parameters[key.ToString()].Type == typeof(DateTime))
//                                info.Report.Parameters[key.ToString()].Value = Convert.ToDateTime(formCollection[key]);

//                            if (info.Report.Parameters[key.ToString()].Type == typeof(DateTime?))
//                            {
//                                if (string.IsNullOrWhiteSpace(formCollection[key]))
//                                    info.Report.Parameters[key.ToString()].Value = null;
//                                else
//                                    info.Report.Parameters[key.ToString()].Value = Convert.ToDateTime(formCollection[key]);
//                            }

//                            if (info.Report.Parameters[key.ToString()].Type == typeof(int))
//                                info.Report.Parameters[key.ToString()].Value = Convert.ToInt32(formCollection[key]);

//                            if (info.Report.Parameters[key.ToString()].Type == typeof(int?))
//                            {
//                                if (string.IsNullOrWhiteSpace(formCollection[key]))
//                                    info.Report.Parameters[key.ToString()].Value = null;
//                                else
//                                    info.Report.Parameters[key.ToString()].Value = Convert.ToInt32(formCollection[key]);
//                            }

//                            if (info.Report.Parameters[key.ToString()].Type == typeof(string))
//                                info.Report.Parameters[key.ToString()].Value = formCollection[key].ToString();

//                            if (info.Report.Parameters[key.ToString()].Type == typeof(bool))
//                                info.Report.Parameters[key.ToString()].Value = Convert.ToBoolean(formCollection[key]);

//                            if (info.Report.Parameters[key.ToString()].Type == typeof(bool?))
//                            {
//                                if (string.IsNullOrWhiteSpace(formCollection[key]))
//                                    info.Report.Parameters[key.ToString()].Value = null;
//                                else
//                                    info.Report.Parameters[key.ToString()].Value = Convert.ToBoolean(formCollection[key]);
//                            }
//                        }
//                        catch
//                        {
//                            info.Report.Parameters[key.ToString()].Value = formCollection[key];
//                        }

//                        info.Report.Parameters[key.ToString()].Visible = false;
//                    }
//                }
//            }


//            return PartialView(info);
//        }
//        [ValidateInput(false)]
//        public ActionResult ExportReport(string id)
//        {
//            ReportInfo info = GetReport(id);
//            if (info == null)
//                return HttpNotFound();

//            if (Request == null || Request.Form == null)
//                return HttpNotFound();

//            //if (SiteInfo.Language == LanguageEnum.Arabic)
//            //    info.Report.RTL();

//            FileResult result = null;
//            char[] chars = { '=', ':', ';' };
//            if (Request.Form != null && Request.Form.AllKeys.Contains("DXArgument"))
//            {
//                string[] dxArguments = Request.Form.GetValues("DxArgument");

//                if (dxArguments.Length <= 0)
//                    return DocumentViewerExtension.ExportTo(info.Report, Request);

//                string[] arr = dxArguments[0].Split(chars, StringSplitOptions.RemoveEmptyEntries);

//                if (arr.Length <= 0)
//                    return  DocumentViewerExtension.ExportTo(info.Report, Request);

//                if (arr[2].ToLower().Equals("xls") || arr[2].ToLower().Equals("xlsx"))
//                {
//                    //info.Report.LTR();
//                    result = DocumentViewerExtension.ExportTo(info.Report, Request);
//                    //info.Report.RTL();
//                    return result;
//                }
//            }

//            return DocumentViewerExtension.ExportTo(info.Report, Request);
//        }





//    }
//}