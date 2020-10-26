using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockPortal.Models;

namespace StockPortal.Controllers
{
    [SessionExpire]
    public class AccessDeniedController : BaseController
    {
        // GET: AccessDenied
        public ActionResult Index()
        {
            return View();
        }
    }
}