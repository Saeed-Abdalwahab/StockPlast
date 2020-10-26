using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StockDB.Model;
using StockPortal.Helpers;
using StockPortal.Models;
using StockService;

namespace StockPortal.Controllers
{
    [HandleError]
    [CustomAuthorize("سريل")]

    [SessionExpire]
    public class ShaplonasController : BaseController
    {
        private StockModel db = new StockModel();

        // GET: Shaplonas
        public ActionResult Index()
        {
            var shaplonas = db.Shaplonas.Include(s => s.Customer);
            return View(shaplonas.ToList());
        }
        // GET: Shaplonas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shaplona shaplona = db.Shaplonas.Find(id);
            if (shaplona == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Views/Shaplonas/_Details.cshtml", shaplona);
        }

        // GET: Shaplonas/Create
        public ActionResult Create()
        {
            ViewBag.Customers = new SelectList(db.Customers,"cust_id", "cust_name");
            Shaplona shaplona = new Shaplona();
            var ship = db.Shaplonas.OrderByDescending(x => x.shap_id).FirstOrDefault();
            shaplona.ShapSerial = (ship!=null? ship.shap_id + 1:1).ToString() ;
            shaplona.shap_startDate = DateTime.Now;
            return View(shaplona);
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "shap_id,cust_id,shap_name,shap_pic,shap_startDate,ShapSerial")] Shaplona shaplona, HttpPostedFileBase shap_pic)
        {
            ViewBag.Customers = new SelectList(db.Customers, "cust_id", "cust_name", shaplona.cust_id);
            string pictureNAme = null;
            if (ModelState.IsValid)
            {
                
                //Check For Customer And ShapName
                if(check(shaplona.cust_id,shaplona.shap_name))
                {
                    ViewBag.ErrorExist = "هذا العميل له سريل بالفعل لا يمكن اضافه جديد ";
                    return View(shaplona);
                }
                if (CheckShapSerial(shaplona.ShapSerial))
                {
                    ViewBag.ErrorExist = "رقم سريل غير متاح برجاء ادخال رقم اخر";
                    return View(shaplona);
                }

                //Check For Img
                if (shap_pic != null) {
                    pictureNAme = shap_pic.FileName.Replace(" ", "");
                    shaplona.shap_pic = pictureNAme;
                
                db.Shaplonas.Add(shaplona);
                db.SaveChanges();
                    string folder = Server.MapPath(string.Format("~/images/shaplon/{0}/", shaplona.shap_id.ToString()));
                    DirectoryInfo di = Directory.CreateDirectory(folder);
                    if (shap_pic.ContentLength > 3*1024*1024)
                    {

                    ResizeImage(shap_pic, shaplona.shap_id);
                    }
                    else
                    {

                    shap_pic.SaveAs(folder + shap_pic.FileName.Replace(" ",""));
                    }
                }
                else
                {
                    db.Shaplonas.Add(shaplona);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.ErrorExist = "حدثه مشكله حاول مره اخري ";

            //ViewBag.cust_id = new SelectList(db.Customers, "cust_id", "cust_name", shaplona.cust_id);
            return View(shaplona);
        }


        public JsonResult AddShiplon(string cust_id,string Shap_nam,bool Check=false)
        {
            ShaplonService shaplonService = new ShaplonService();
            string Serial ;
            bool Result = shaplonService.SaveShiplon(int.Parse(cust_id), Shap_nam, Check,out Serial);
            return Result? Json(new { data = Serial }, JsonRequestBehavior.AllowGet): Json(false, JsonRequestBehavior.AllowGet);

        }

        // GET: Shaplonas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shaplona shaplona = db.Shaplonas.Find(id);
            if (shaplona == null)
            {
                return HttpNotFound();
            }
            ViewBag.Customers = new SelectList(db.Customers, "cust_id", "cust_name",shaplona.cust_id);

            //ViewBag.custName = db.Customers.FirstOrDefault(x => x.cust_id == shaplona.cust_id).cust_name.ToString();
            
            return View(shaplona);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "shap_pic,shap_id,cust_id,shap_name,shap_startDate,ShapSerial,shap_endDate")] Shaplona shaplona, HttpPostedFileBase shap_pic)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Customers = new SelectList(db.Customers, "cust_id", "cust_name", shaplona.cust_id);

                if (shaplona.shap_endDate != null)
                {
                    if (shaplona.shap_endDate < shaplona.shap_startDate)
                    {
                        ViewBag.ErrorExist = "لا يمكن ان يكون تاريخ الدخول اقل من الخروج!";

                        return View(shaplona);

                    }
                }
                try
                {
                    if (CheckShapSerial(shaplona.ShapSerial,shaplona.shap_id))
                    {
                        ViewBag.ErrorExist = "رقم سريل غير متاح برجاء ادخال رقم اخر";
                        ViewBag.Customers = new SelectList(db.Customers, "cust_id", "cust_name");

                        return View(shaplona);
                    }
                    if (check(shaplona.cust_id, shaplona.shap_name, shaplona.shap_id))
                    {
                        ViewBag.ErrorExist = "هذا العميل له نفس اسم السريل بالفعل لا يمكن اضافه جديد ";
                        return View(shaplona);
                    }
                    string folder = Server.MapPath(string.Format("~/images/shaplon/{0}/", shaplona.shap_id.ToString()));
                    if (shap_pic != null)
                    {
                        shaplona.shap_pic = shap_pic.FileName.Replace(" ","");
                        if (Directory.Exists(folder))
                        {
                            Directory.Delete(folder, true);

                        }
                        DirectoryInfo di = Directory.CreateDirectory(folder);
                        if(shap_pic.ContentLength > 3000)
                        {

                        ResizeImage(shap_pic, shaplona.shap_id);
                        }
                        else
                        {
                        shap_pic.SaveAs(folder + shaplona.shap_pic.ToString());

                        }
                    }

                    db.Entry(shaplona).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Customers = new SelectList(db.Customers, "cust_id", "cust_name");

                    //ViewBag.custName = db.Customers.FirstOrDefault(x => x.cust_id == shaplona.cust_id).cust_name.ToString();
                    ViewBag.Msg = "حدثت مشكله برجاء المحاوله مره اخري ";
                    return View(shaplona);
                }
            }
            else
            {

                //ViewBag.custName = db.Customers.FirstOrDefault(x => x.cust_id == shaplona.cust_id).cust_name.ToString();
                return View(shaplona);
            }
            //ViewBag.cust_id = new SelectList(db.Customers, "cust_id", "cust_name", shaplona.cust_id);
      
        }

        // GET: Shaplonas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shaplona shaplona = db.Shaplonas.Find(id);

            if (shaplona == null)
            {
                return HttpNotFound();
            }
            ViewBag.custName = db.Customers.FirstOrDefault(x => x.cust_id == shaplona.cust_id).cust_name;
            return View(shaplona);
        }

        // POST: Shaplonas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Shaplona shaplona = db.Shaplonas.Find(id);
                db.Shaplonas.Remove(shaplona);
                db.SaveChanges();
                string folder = Server.MapPath(string.Format("~/images/shaplon/{0}/", id.ToString()));
                if (Directory.Exists(folder))
                {
                    Directory.Delete(folder, true);

                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View("~/Views/Shaplonas/Delete.cshtml");
            }
        }



        //Function To Check Name Of Cust With Name Of Shaplon
        private bool check(int Custid,string shap_name,int? ID=null)
        {
            Shaplona shaplon = new Shaplona();

            if (ID != null)
            {
                shaplon = db.Shaplonas.FirstOrDefault(x => x.cust_id == Custid && x.shap_name == shap_name&&x.shap_id!=ID);
                if (shaplon != null)
                    return true;
                else
                    return false;
            }
            
             shaplon=  db.Shaplonas.FirstOrDefault(x => x.cust_id == Custid && x.shap_name == shap_name);
            if (shaplon != null)
                return true;
            else
                return false;
        }

        //Remot Validation for ShapSerial
        private bool CheckShapSerial(string shapserial, int? ID = null)
        {
            Shaplona shaplon = new Shaplona();

            if (ID != null)
            {
                shaplon = db.Shaplonas.FirstOrDefault(x => x.ShapSerial == shapserial &&  x.shap_id != ID);
                if (shaplon != null)
                    return true;
                else
                    return false;
            }
            else
            { 
            shaplon = db.Shaplonas.FirstOrDefault(x => x.ShapSerial == shapserial);
            if (shaplon != null)
                return true;
            else
                return false;
        }
        }
        private string ResizeImage(HttpPostedFileBase fileToUpload,int ShapID)
        {
            string name = Path.GetFileNameWithoutExtension(fileToUpload.FileName);
            var ext = Path.GetExtension(fileToUpload.FileName);
            string myfile = name.Replace(" ","") + ext;
            try
            {
                using (Image image = Image.FromStream(fileToUpload.InputStream, true, false))
                {
                    var path = Path.Combine(Server.MapPath(string.Format("~/images/shaplon/{0}/", ShapID.ToString())), myfile);

                    try
                    {
                        //Size can be change according to your requirement 
                        float thumbWidth = 1400F;
                        float thumbHeight = 1900F;
                        //calculate  image  size
                        if (image.Width > image.Height)
                        {
                            thumbHeight = ((float)image.Height / image.Width) * thumbWidth;
                        }
                        else
                        {
                            thumbWidth = ((float)image.Width / image.Height) * thumbHeight;
                        }

                        int actualthumbWidth = Convert.ToInt32(Math.Floor(thumbWidth));
                        int actualthumbHeight = Convert.ToInt32(Math.Floor(thumbHeight));
                        var thumbnailBitmap = new Bitmap(actualthumbWidth, actualthumbHeight);
                        var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                        thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                        thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                        thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        var imageRectangle = new Rectangle(0, 0, actualthumbWidth, actualthumbHeight);
                        thumbnailGraph.DrawImage(image, imageRectangle);
                        var ms = new MemoryStream();
                        //thumbnailBitmap.Save(path, ImageFormat.Jpeg);          
                        thumbnailBitmap.Save(path);
                        ms.Position = 0;
                        GC.Collect();
                        thumbnailGraph.Dispose();
                        thumbnailBitmap.Dispose();
                        image.Dispose();

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return myfile;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
