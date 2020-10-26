using StockDB.Model;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static PlasticStatic.Enums;
using static PlasticStatic.Extensions;

namespace StockService
{
   public class expireditemService
    {
        StockModel db = new StockModel();
        // function to determin name of expired
        private string ExpiredName(int? id)
        {
            if (id == null)
                return "";
            if (id == (int)expiredtype.printexpired)
            {
                return "تالف طباعه";
            }
            else if (id == (int)expiredtype.filmexpired)
            {
                return "تالف فيلم";

            }
            else if (id == (int)expiredtype.cutexpired)
            {
                return "تالف تقطيع";
            }
            else
                return "";
        }
        public string GetMaxSerial()
        {
            var tostock = db.ToStocks.OrderByDescending(x => x.ID).FirstOrDefault();
            return (tostock != null ? tostock.ID + 1 : 1).ToString();
        }
        public int getsupid(int itemsupid)
        {
         return (int)  db.ItemSuppliers.FirstOrDefault(x => x.ID == itemsupid).SupId;
        }
        public double? Calculatecurrentquantityfordemonorderdetails(int demondorderdetaisid, string tostockid=null,double totalbag = 0)
        {
            //first  catch all delivered count
            double? totaldeliver = null;
            if (tostockid == null) { 
            totaldeliver= db.StockTransactions.Where(x => x.DemondOrderDetialsId == demondorderdetaisid && (x.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.expired || x.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.finished)).
                Sum(x => x.Quantity);
            }
            else
            {
                var TostockID = Convert.ToInt32(tostockid);
                 totaldeliver = db.StockTransactions.Where(x => x.DemondOrderDetialsId == demondorderdetaisid && (x.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.expired || x.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.finished)&&x.ToStockId != TostockID).
              Sum(x => x.Quantity);
            }
            //

            var orignalcountfordemonorderdetails = db.StockTransactions.Where(x => x.DemondOrderDetialsId == demondorderdetaisid &&x.OperationType=="-"&&  x.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.Raw).
                 Sum(x => x.Quantity);
           
             
            var total= orignalcountfordemonorderdetails - (totaldeliver != null ? totaldeliver : 0) - totalbag;

            return total;
        }
        public AlertVM<PurchaseInvoiceViewModel> GetExpiredTostockList(int FromStockID,int Demondorderdetailsid)
        {
            AlertVM<PurchaseInvoiceViewModel> Alert = new AlertVM<PurchaseInvoiceViewModel>();
            var Model = new List<PurchaseInvoiceViewModel>();
            //var Model1 = new List<PurchaseInvoiceViewModel>();

            try
            {
                //Model = db.ToStocks.Where(T=>T.FromStockId==FromStockID && T.ToStockTypeId==(int)ToStockTypeId.ExpiredItem&&T.DemondOrderDetialsId==Demondorderdetailsid).Select(ts => new PurchaseInvoiceViewModel
                //{
                //    ID = ts.ID,
                //    InvoiceSerial = ts.InvoiceSerial,
                //    InvoiceDate = ts.InvoiceDate,
                //    Notes = ts.Notes,
                //    SupId = ts.SupId,
                //    SupName=ts.supplier.sup_name,
                //    demondordid=ts.DemondOrderDetialsId,
                //    StoreName = db.StockTransactions.FirstOrDefault(x => x.ToStockId == ts.ID).Store.st_name
                //}).ToList();
                Model = db.StockTransactions.Where(T => T.ToStock.FromStockId == FromStockID && T.ToStock.ToStockTypeId == (int)ToStockTypeId.ExpiredItem && T.DemondOrderDetialsId == Demondorderdetailsid)
                    .GroupBy(ts => new {
                        ID = (int)ts.ToStockId,
                        InvoiceSerial = ts.ToStock.InvoiceSerial,
                        InvoiceDate = ts.ToStock.InvoiceDate,
                        Notes = ts.ToStock.Notes,
                        SupId = ts.ToStock.SupId,
                        SupName = ts.ToStock.supplier.sup_name,
                        demondordid = ts.ToStock.DemondOrderDetialsId,
                        StoreName = ts.Store.st_name,
                        invoicestatus=ts.ToStock.InvoiceStatus,
                        expiredtype=ts.expiredtypid,
                        weight=ts.Weight,
                    })
                    .Select(ts => new PurchaseInvoiceViewModel
                {
                        ID = ts.Key.ID,
                        InvoiceSerial = ts.Key.InvoiceSerial,
                        InvoiceDate = ts.Key.InvoiceDate,
                        Notes = ts.Key.Notes,
                        SupId = ts.Key.SupId,
                        SupName = ts.Key.SupName,
                        demondordid = ts.Key.demondordid,
                        InvoiceStatus=ts.Key.invoicestatus,
                        StoreName = ts.Key.StoreName,
                        expiredType= (expiredtype)ts.Key.expiredtype,
                        Weight =ts.Key.weight

                }).ToList();
                foreach (var item in Model)
                {
                    item.expiredtypname = item.expiredType.GetDisplayName();
                }

                Alert.type = true;
                Alert.data = Model;
            }
            catch
            {
                Model = null;

                Alert.type = false;
                Alert.data = Model;
            }
            return Alert;
        }




        public AlertVM<PurchaseInvoiceViewModel> GetExpiredItemList(int ToStockID)
        {
            AlertVM<PurchaseInvoiceViewModel> Alert = new AlertVM<PurchaseInvoiceViewModel>();
            var Model = new List<PurchaseInvoiceViewModel>();
            
            try
            {

                Model = db.StockTransactions.Where(T => T.ToStockId == ToStockID && T.expiredtypid != null).Select(ts => new PurchaseInvoiceViewModel
                {
                    ID = ts.ID,
                    Weight = ts.Weight,
                    NoItem = ts.NoItem,
                    expiredtypname = ((expiredtype)ts.expiredtypid).ToString() /*Enum.GetName(typeof(expiredtype), (int)ts.expiredtypid)*/ /*ts.expiredtypid.ToString()*/
            }).ToList();

                Alert.type = true;
                Alert.data = Model;
            }
            catch
            {
                Model = null;

                Alert.type = false;
                Alert.data = Model;
            }
            return Alert;
        }

        public AlertVM<PurchaseInvoiceViewModel> GetCompletedItemList(int ToStockID)
        {
            AlertVM<PurchaseInvoiceViewModel> Alert = new AlertVM<PurchaseInvoiceViewModel>();
            var Model = new List<PurchaseInvoiceViewModel>();

            try
            {

                Model = db.StockTransactions.Where(T => T.ToStockId == ToStockID &&T.Store.StoreType_ID==(int?)PlasticStatic.Enums.StoreType.finished).Select(ts => new PurchaseInvoiceViewModel
                {
                    ID = ts.ID,
                    Weight = ts.Weight,
                    NoItem = ts.NoItem

                }).ToList();

                Alert.type = true;
                Alert.data = Model;
            }
            catch
            {
                Model = null;

                Alert.type = false;
                Alert.data = Model;
            }
            return Alert;
        }
        // For Returneditem list from to stock

        public AlertVM<PurchaseInvoiceViewModel> GetreturnedTostockList(int FromStockID, int Demondorderdetailsid)
        {
            AlertVM<PurchaseInvoiceViewModel> Alert = new AlertVM<PurchaseInvoiceViewModel>();
            var Model = new List<PurchaseInvoiceViewModel>();
            //var Model1 = new List<PurchaseInvoiceViewModel>();

            try
            {
                //Model = db.ToStocks.Where(T => T.FromStockId == FromStockID && T.ToStockTypeId == (int)ToStockTypeId.OperationReturns && T.DemondOrderDetialsId == Demondorderdetailsid).Select(ts => new PurchaseInvoiceViewModel
                //{
                //    ID = ts.ID,
                //    InvoiceSerial = ts.InvoiceSerial,
                //    InvoiceDate = ts.InvoiceDate,
                //    Notes = ts.Notes,
                //    SupId = ts.SupId,
                //    SupName = ts.supplier.sup_name,
                //    demondordid = ts.DemondOrderDetialsId,
                //    StoreName = db.StockTransactions.FirstOrDefault(x => x.ToStockId == ts.ID).Store.st_name

                //}).ToList();
                Model = db.StockTransactions.Where(T => T.ToStock.FromStockId == FromStockID && T.ToStock.ToStockTypeId == (int)ToStockTypeId.OperationReturns && T.ToStock.DemondOrderDetialsId == Demondorderdetailsid).Select(ts => new PurchaseInvoiceViewModel
                {
                    ID = ts.ToStock.ID,
                    InvoiceSerial = ts.ToStock.InvoiceSerial,
                    InvoiceDate = ts.ToStock.InvoiceDate,
                    Notes = ts.ToStock.Notes,
                    SupId = ts.ToStock.SupId,
                    SupName = ts.ToStock.supplier.sup_name,
                    demondordid = ts.ToStock.DemondOrderDetialsId,
                    StoreName=ts.Store.st_name,
                    Weight=ts.Weight
                    //StoreName = db.StockTransactions.FirstOrDefault(x => x.ToStockId == ts.ID).Store.st_name

                }).ToList();

                Alert.type = true;
                Alert.data = Model;
            }
            catch
            {
                Model = null;

                Alert.type = false;
                Alert.data = Model;
            }
            return Alert;
        }



        //eXPIRED ITEM eDIT
        public AlertVM<PurchaseInvoiceViewModel> GetTOstockExpiredByID(int ToStockID,int FromStockID ,int  ToStockTypeId )
        {
            AlertVM<PurchaseInvoiceViewModel> Alert = new AlertVM<PurchaseInvoiceViewModel>();

            var Model = new PurchaseInvoiceViewModel();


            try
            {
                PurchaseInvoiceViewModel purchaseInvoiceViewModel = new PurchaseInvoiceViewModel();
                purchaseInvoiceViewModel = db.StockTransactions.Where(x => x.ToStockId == ToStockID && x.FromStockId == FromStockID && x.expiredtypid != null).Select(ts=>new PurchaseInvoiceViewModel {
                    ID = (int)ts.ToStockId,
                    InvoiceSerial = ts.ToStock.InvoiceSerial,
                    InvoiceDate = ts.ToStock.InvoiceDate,
                    Notes = ts.ToStock.Notes,
                    SupId = ts.ToStock.SupId,
                    SupName = ts.ToStock.supplier.sup_name,
                    StoreName = ts.Store.st_name,
                    StoreId = ts.StoreId,
                    InvoiceStatus = ts.ToStock.InvoiceStatus,
                    cutexpired = ts.Weight 
                }).FirstOrDefault();
                Model = db.StockTransactions.Where(T => T.ToStockId == ToStockID && T.expiredtypid==(int)expiredtype.cutexpired&&T.FromStockId == FromStockID && T.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.expired && T.ToStock.ToStockTypeId == ToStockTypeId).Select(ts => new PurchaseInvoiceViewModel
                {
                    ID = (int)ts.ToStockId,
                    InvoiceSerial = ts.ToStock.InvoiceSerial,
                    InvoiceDate = ts.ToStock.InvoiceDate,
                    Notes = ts.ToStock.Notes,
                    SupId = ts.ToStock.SupId,
                    SupName = ts.ToStock.supplier.sup_name,
                    StoreName = ts.Store.st_name,
                    StoreId = ts.StoreId,
                    InvoiceStatus = ts.ToStock.InvoiceStatus,
                    cutexpired=ts.Weight

                }).FirstOrDefault();
                purchaseInvoiceViewModel.cutexpired = Model!=null? Model.cutexpired:null;
                Model = db.StockTransactions.Where(T => T.ToStockId == ToStockID && T.expiredtypid == (int)expiredtype.filmexpired && T.FromStockId == FromStockID && T.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.expired && T.ToStock.ToStockTypeId == ToStockTypeId).Select(ts => new PurchaseInvoiceViewModel
                {
                    ID = (int)ts.ToStockId,
                    InvoiceSerial = ts.ToStock.InvoiceSerial,
                    InvoiceDate = ts.ToStock.InvoiceDate,
                    Notes = ts.ToStock.Notes,
                    SupId = ts.ToStock.SupId,
                    SupName = ts.ToStock.supplier.sup_name,
                    StoreName = ts.Store.st_name,
                    StoreId = ts.StoreId,
                    InvoiceStatus = ts.ToStock.InvoiceStatus,
                    filmexpired = ts.Weight

                }).FirstOrDefault();
                purchaseInvoiceViewModel.filmexpired = Model !=null? Model.filmexpired:null;
                Model = db.StockTransactions.Where(T => T.ToStockId == ToStockID && T.expiredtypid == (int)expiredtype.printexpired && T.FromStockId == FromStockID && T.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.expired && T.ToStock.ToStockTypeId == ToStockTypeId).Select(ts => new PurchaseInvoiceViewModel
                {
                    ID = (int)ts.ToStockId,
                    InvoiceSerial = ts.ToStock.InvoiceSerial,
                    InvoiceDate = ts.ToStock.InvoiceDate,
                    Notes = ts.ToStock.Notes,
                    SupId = ts.ToStock.SupId,
                    SupName = ts.ToStock.supplier.sup_name,
                    StoreName = ts.Store.st_name,
                    StoreId = ts.StoreId,
                    InvoiceStatus = ts.ToStock.InvoiceStatus,
                    printexpired = ts.Weight

                }).FirstOrDefault();
                purchaseInvoiceViewModel.printexpired = Model!=null? Model.printexpired:null;

                Alert.type = true;
                Alert.Data = purchaseInvoiceViewModel;
            }
            catch
            {
                Model = null;

                Alert.type = false;
                Alert.Data = Model;
            }



            return Alert;
        }
        //Completed item for edit

        public AlertVM<PurchaseInvoiceViewModel> GetTOstockcompletedByID(int ToStockID, int FromStockID, int ToStockTypeId)
        {
            AlertVM<PurchaseInvoiceViewModel> Alert = new AlertVM<PurchaseInvoiceViewModel>();

            var Model = new PurchaseInvoiceViewModel();


            try
            {

                Model = db.StockTransactions.Where(T => T.ToStockId == ToStockID && T.FromStockId == FromStockID && T.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.finished&&T.ToStock.ToStockTypeId==ToStockTypeId).Select(ts => new PurchaseInvoiceViewModel
                {
                    ID = (int)ts.ToStockId,
                    InvoiceSerial = ts.ToStock.InvoiceSerial,
                    InvoiceDate = ts.ToStock.InvoiceDate,
                    Notes = ts.ToStock.Notes,
                    SupId = ts.ToStock.SupId,
                    SupName = ts.ToStock.supplier.sup_name,
                    StoreName = ts.Store.st_name,
                    StoreId = ts.StoreId,
                    InvoiceStatus = ts.ToStock.InvoiceStatus,
                    finishweight=(double)ts.Weight

                }).FirstOrDefault();

                Alert.type = true;
                Alert.Data = Model;
            }
            catch
            {
                Model = null;

                Alert.type = false;
                Alert.Data = Model;
            }



            return Alert;
        }
        //returned item for details

        public AlertVM<PurchaseInvoiceViewModel> GetReturnedtostockByID(int ToStockID, int FromStockID, int ToStockTypeId)
        {
            AlertVM<PurchaseInvoiceViewModel> Alert = new AlertVM<PurchaseInvoiceViewModel>();

            var Model = new PurchaseInvoiceViewModel();


            try
            {

                Model = db.StockTransactions.Where(T => T.ToStockId == ToStockID && T.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.Raw && T.ToStock.ToStockTypeId == ToStockTypeId).Select(ts => new PurchaseInvoiceViewModel
                {
                    ID = (int)ts.ToStockId,
                    InvoiceSerial = ts.ToStock.InvoiceSerial,
                    InvoiceDate = ts.ToStock.InvoiceDate,
                    Notes = ts.ToStock.Notes,
                    SupId = ts.ToStock.SupId,
                    SupName = ts.ToStock.supplier.sup_name,
                    StoreName = ts.Store.st_name,
                    StoreId = ts.StoreId,
                    InvoiceStatus = ts.ToStock.InvoiceStatus,
                    Weight = (double)ts.Weight

                }).FirstOrDefault();

                Alert.type = true;
                Alert.Data = Model;
            }
            catch
            {
                Model = null;

                Alert.type = false;
                Alert.Data = Model;
            }



            return Alert;
        }


        public bool AddPurchaseInvoice(PurchaseInvoiceViewModel vm, List<PurchaseInvoiceViewModel> insert, List<PurchaseInvoiceViewModel> Modified, List<PurchaseInvoiceViewModel> Deleted)
        {

            ToStock tostock = new ToStock();

            FromStockService fromStock = new FromStockService();
            try
            {
           

                //var Item=fromStock.GetStockTransactionByID(vm.StockTransactioID).Data;
                vm.ID=vm.ID == -1 ? 0 : vm.ID;
                tostock.InvoiceSerial = vm.InvoiceSerial;
                tostock.InvoiceDate = vm.InvoiceDate;
                tostock.SupId = /*db.ItemSuppliers.Find(Item.ItemSupID).SupId;*/vm.SupId;
                tostock.Notes = vm.Notes;
                tostock.ID = vm.ID;
                tostock.InvoiceStatus = vm.InvoiceStatus;
                tostock.ToStockTypeId = vm.ToStockTypeId;
                tostock.FromStockId = vm.FromStockID;
                tostock.DemondOrderDetialsId = vm.demondordid;
                if (vm.ID == 0)
                {
                    db.ToStocks.Add(tostock);
                    db.SaveChanges();
                }
                else
                {

                    var itemlist = db.StockTransactions.Where(s => s.ToStockId == vm.ID).ToList();
                    int? OldStoreID = itemlist.Select(o => o.StoreId).FirstOrDefault();

                    if (OldStoreID != vm.StoreId)
                    {
                        foreach (var item in itemlist)
                        {

                            item.StoreId = vm.StoreId;

                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                    db.Entry(tostock).State = EntityState.Modified;
                    db.SaveChanges();
                }

                if (AddItem((int)/*Item.ItemSupID*/vm.SupId,tostock.ID, vm.StoreId, insert, Modified, Deleted) == false)
                {
                    return false;
                }
                else
                {
                }



                return true;
            }

            catch (Exception)
            {
                return false;
            }

        }

        public bool Addpurchesesinvoiceexpired(PurchaseInvoiceViewModel vm)
        {

            ToStock tostock = new ToStock();

            FromStockService fromStock = new FromStockService();
            try
            {


                vm.ID = vm.ID == 0 ? 0 : vm.ID;
           

                if (vm.ID == 0)
                {
                    tostock.InvoiceSerial = vm.InvoiceSerial;
                    tostock.InvoiceDate = vm.InvoiceDate;
                    tostock.SupId = vm.SupId;
                    tostock.Notes = vm.Notes;
                    tostock.ID = vm.ID;
                    tostock.InvoiceStatus = (int)InvoiceStatus.NotFinish;
                    tostock.ToStockTypeId = vm.ToStockTypeId;
                    tostock.FromStockId = vm.FromStockID;
                    tostock.DemondOrderDetialsId = vm.demondordid;
                    db.ToStocks.Add(tostock);
                    db.SaveChanges();
                    if (vm.cutexpired != null)
                    { 
                        StockTransaction cutstoctr = new StockTransaction();
                        cutstoctr.ToStockId = tostock.ID;
                        cutstoctr.StoreId = vm.StoreId;
                        cutstoctr.NoItem = 1;
                        cutstoctr.Weight = vm.cutexpired;
                        cutstoctr.Quantity = vm.cutexpired;
                        cutstoctr.expiredtypid = (int)expiredtype.cutexpired;
                        cutstoctr.DemondOrderDetialsId = vm.demondordid;
                        cutstoctr.FromStockId = vm.FromStockID;
                        cutstoctr.OperationType = "+";
                        cutstoctr.ItemSupplierId = vm.itemsupid;
                        db.StockTransactions.Add(cutstoctr);
                        db.SaveChanges();
                    }
                    if (vm.printexpired != null)
                    {
                        StockTransaction printstoctr = new StockTransaction();
                        printstoctr.ToStockId = tostock.ID;
                        printstoctr.StoreId = vm.StoreId;
                        printstoctr.NoItem = 1;
                        printstoctr.Weight = vm.printexpired;
                        printstoctr.Quantity = vm.printexpired;
                        printstoctr.expiredtypid = (int)expiredtype.printexpired;
                        printstoctr.DemondOrderDetialsId = vm.demondordid;
                        printstoctr.FromStockId = vm.FromStockID;
                        printstoctr.OperationType = "+";
                        printstoctr.ItemSupplierId = vm.itemsupid;
                        db.StockTransactions.Add(printstoctr);
                        db.SaveChanges();
                    }
                    if (vm.filmexpired != null)
                    {
                        StockTransaction filmstoctr = new StockTransaction();
                        filmstoctr.ToStockId = tostock.ID;
                        filmstoctr.StoreId = vm.StoreId;
                        filmstoctr.NoItem = 1;
                        filmstoctr.Weight = vm.filmexpired;
                        filmstoctr.Quantity = vm.filmexpired;
                        filmstoctr.expiredtypid = (int)expiredtype.filmexpired;
                        filmstoctr.DemondOrderDetialsId = vm.demondordid;
                        filmstoctr.FromStockId = vm.FromStockID;
                        filmstoctr.OperationType = "+";
                        filmstoctr.ItemSupplierId = vm.itemsupid;
                        db.StockTransactions.Add(filmstoctr);
                        db.SaveChanges();
                    }
                   var demondorderdetails= db.DemondOrderDetails.Find(tostock.DemondOrderDetialsId);
                    demondorderdetails.status = (int)demondorderdetailstatus.Duringdelivery;
                    db.Entry(demondorderdetails).State = EntityState.Modified;
                    db.SaveChanges();


                }
                else
                {
                    tostock = db.ToStocks.Find(vm.ID);
                    tostock.InvoiceSerial = vm.InvoiceSerial;
                    tostock.InvoiceDate = vm.InvoiceDate;
                    tostock.SupId = vm.SupId;
                    tostock.Notes = vm.Notes;
                    tostock.InvoiceStatus = (int)InvoiceStatus.NotFinish;
                    tostock.ToStockTypeId = vm.ToStockTypeId;
                    tostock.FromStockId = vm.FromStockID;
                    tostock.DemondOrderDetialsId = vm.demondordid;

                    db.Entry(tostock).State = EntityState.Modified;
                    db.SaveChanges();
                    var allexpiredstocktr = db.StockTransactions.Where(x => x.DemondOrderDetialsId == vm.demondordid && x.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.expired).ToList();
                    var checkcutexpired=allexpiredstocktr.FirstOrDefault(x=>x.expiredtypid== (int)expiredtype.cutexpired);
                    var checkprintexpired=allexpiredstocktr.FirstOrDefault(x=>x.expiredtypid== (int)expiredtype.printexpired);
                    var checkfilmexpired=allexpiredstocktr.FirstOrDefault(x=>x.expiredtypid== (int)expiredtype.filmexpired);
                    if (checkcutexpired == null && vm.cutexpired != null)
                    {
                        StockTransaction cutstoctr = new StockTransaction();
                        cutstoctr.ToStockId = tostock.ID;
                        cutstoctr.StoreId = vm.StoreId;
                        cutstoctr.NoItem = 1;
                        cutstoctr.Weight = vm.cutexpired;
                        cutstoctr.Quantity = vm.cutexpired;
                        cutstoctr.expiredtypid = (int)expiredtype.cutexpired;
                        cutstoctr.DemondOrderDetialsId = vm.demondordid;
                        cutstoctr.FromStockId = vm.FromStockID;
                        cutstoctr.OperationType = "+";
                        cutstoctr.ItemSupplierId = vm.itemsupid;
                        db.StockTransactions.Add(cutstoctr);
                        db.SaveChanges();
                    }
                    if (checkfilmexpired == null && vm.filmexpired != null)
                    {
                        StockTransaction filmstoctr = new StockTransaction();
                        filmstoctr.ToStockId = tostock.ID;
                        filmstoctr.StoreId = vm.StoreId;
                        filmstoctr.NoItem = 1;
                        filmstoctr.Weight = vm.filmexpired;
                        filmstoctr.Quantity = vm.filmexpired;
                        filmstoctr.expiredtypid = (int)expiredtype.filmexpired;
                        filmstoctr.DemondOrderDetialsId = vm.demondordid;
                        filmstoctr.FromStockId = vm.FromStockID;
                        filmstoctr.OperationType = "+";
                        filmstoctr.ItemSupplierId = vm.itemsupid;
                        db.StockTransactions.Add(filmstoctr);
                        db.SaveChanges();
                    }
                    if (checkprintexpired == null && vm.printexpired != null)
                    {
                        StockTransaction printstoctr = new StockTransaction();
                        printstoctr.ToStockId = tostock.ID;
                        printstoctr.StoreId = vm.StoreId;
                        printstoctr.NoItem = 1;
                        printstoctr.Weight = vm.printexpired;
                        printstoctr.Quantity = vm.printexpired;
                        printstoctr.expiredtypid = (int)expiredtype.printexpired;
                        printstoctr.DemondOrderDetialsId = vm.demondordid;
                        printstoctr.FromStockId = vm.FromStockID;
                        printstoctr.OperationType = "+";
                        printstoctr.ItemSupplierId = vm.itemsupid;
                        db.StockTransactions.Add(printstoctr);
                        db.SaveChanges();
                    }

                    foreach (var item in allexpiredstocktr)
                    {

                        var stoctr = db.StockTransactions.Find(item.ID);
                        if (item.expiredtypid == (int)expiredtype.cutexpired)
                        {
                            if(vm.cutexpired != null)
                            {
                                stoctr.ToStockId = tostock.ID;
                                stoctr.StoreId = vm.StoreId;
                                stoctr.NoItem = 1;
                                stoctr.Weight = vm.cutexpired;
                                stoctr.Quantity = vm.cutexpired;
                                stoctr.expiredtypid = (int)expiredtype.cutexpired;
                                stoctr.DemondOrderDetialsId = vm.demondordid;
                                stoctr.FromStockId = vm.FromStockID;
                                stoctr.OperationType = "+";
                                stoctr.ItemSupplierId = vm.itemsupid;
                                db.Entry(stoctr).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else if(vm.cutexpired == null)
                            {
                                db.StockTransactions.Remove(stoctr);
                                db.SaveChanges();
                            }
                        }
                      else  if (item.expiredtypid == (int)expiredtype.printexpired)
                        {
                            if (vm.printexpired != null)
                            {
                                stoctr.ToStockId = tostock.ID;
                                stoctr.StoreId = vm.StoreId;
                                stoctr.NoItem = 1;
                                stoctr.Weight = vm.printexpired;
                                stoctr.Quantity = vm.printexpired;
                                stoctr.expiredtypid = (int)expiredtype.printexpired;
                                stoctr.DemondOrderDetialsId = vm.demondordid;
                                stoctr.FromStockId = vm.FromStockID;
                                stoctr.OperationType = "+";
                                stoctr.ItemSupplierId = vm.itemsupid;
                                db.Entry(stoctr).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else if (vm.printexpired == null)
                            {
                                db.StockTransactions.Remove(stoctr);
                                db.SaveChanges();
                            }
                        }
                     else   if (item.expiredtypid == (int)expiredtype.filmexpired)
                        {
                            if (vm.filmexpired != null)
                            {
                                stoctr.ToStockId = tostock.ID;
                                stoctr.StoreId = vm.StoreId;
                                stoctr.NoItem = 1;
                                stoctr.Weight = vm.filmexpired;
                                stoctr.Quantity = vm.filmexpired;
                                stoctr.expiredtypid = (int)expiredtype.filmexpired;
                                stoctr.DemondOrderDetialsId = vm.demondordid;
                                stoctr.FromStockId = vm.FromStockID;
                                stoctr.OperationType = "+";
                                stoctr.ItemSupplierId = vm.itemsupid;
                                db.Entry(stoctr).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else if (vm.filmexpired == null)
                            {
                                db.StockTransactions.Remove(stoctr);
                                db.SaveChanges();
                            }
                        }

                    }
                   
                }

                return true;
            }

            catch
            {
                return false;
            }
        }

        public bool Addpurchaseinvoisefinish(PurchaseInvoiceViewModel vm)
        {
            ToStock tostock = new ToStock();
            FromStockService fromStock = new FromStockService();
            try
            {


                //var Item=fromStock.GetStockTransactionByID(vm.StockTransactioID).Data;
                vm.ID = vm.ID == 0 ? 0 : vm.ID;

                if (vm.ID == 0)
                {
                    tostock.InvoiceSerial = vm.InvoiceSerial;
                    tostock.InvoiceDate = vm.InvoiceDate;
                    tostock.SupId = getsupid((int)vm.itemsupid);
                    tostock.Notes = vm.Notes;
                    tostock.ID = vm.ID;
                    tostock.InvoiceStatus = vm.InvoiceStatus;
                    tostock.ToStockTypeId = vm.ToStockTypeId;
                    tostock.FromStockId = vm.FromStockID;
                    tostock.InvoiceStatus = (int)InvoiceStatus.NotFinish;
                    tostock.DemondOrderDetialsId = vm.demondordid;
                    db.ToStocks.Add(tostock);
                    db.SaveChanges();
                    var demondorderdetails = db.DemondOrderDetails.Find(tostock.DemondOrderDetialsId);
                    demondorderdetails.status = (int)demondorderdetailstatus.Duringdelivery;
                    db.Entry(demondorderdetails).State = EntityState.Modified;
                    db.SaveChanges();

                    StockTransaction stoctr = new StockTransaction();
                    stoctr.ToStockId = tostock.ID;
                    stoctr.StoreId = vm.StoreId;
                    stoctr.NoItem = 1;
                    stoctr.Weight = vm.finishweight;
                    stoctr.Quantity = vm.finishweight;
                    stoctr.expiredtypid = vm.expiredtypid;
                    stoctr.DemondOrderDetialsId = vm.demondordid;
                    stoctr.FromStockId = vm.FromStockID;
                    stoctr.OperationType = "+";
                    stoctr.ItemSupplierId = vm.itemsupid;
                    db.StockTransactions.Add(stoctr);
                    db.SaveChanges();
                }
                else
                {
                    tostock = db.ToStocks.Find(vm.ID);
                    tostock.InvoiceSerial = vm.InvoiceSerial;
                    tostock.InvoiceDate = vm.InvoiceDate;
                    tostock.Notes = vm.Notes;
                    tostock.InvoiceStatus = (int)InvoiceStatus.NotFinish;
                    tostock.ToStockTypeId = vm.ToStockTypeId;
                    tostock.FromStockId = vm.FromStockID;
                    tostock.DemondOrderDetialsId = vm.demondordid;

                    db.Entry(tostock).State = EntityState.Modified;
                    db.SaveChanges();
                    StockTransaction stoctr = db.StockTransactions.FirstOrDefault(x => x.DemondOrderDetialsId == vm.demondordid && x.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.finished);
                    stoctr.ToStockId = tostock.ID;
                    stoctr.StoreId = vm.StoreId;
                    stoctr.NoItem = 1;
                    stoctr.Weight = vm.finishweight;
                    stoctr.Quantity = vm.finishweight;
                    stoctr.expiredtypid = vm.expiredtypid;
                    stoctr.DemondOrderDetialsId = vm.demondordid;
                    stoctr.FromStockId = vm.FromStockID;
                    stoctr.OperationType = "+";
                    stoctr.ItemSupplierId = vm.itemsupid;
                    db.Entry(stoctr).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return true;
            }

            catch 
            {
                return false;
            }

        }

        #region Add Item


        public bool AddItem(int itemsupid,int ToStockID, int? StoreID, List<PurchaseInvoiceViewModel> insert, List<PurchaseInvoiceViewModel> Modified, List<PurchaseInvoiceViewModel> Deleted)
        {
            try
            {

       
                foreach (var item in insert)
                {
                    StockTransaction stoctr = new StockTransaction();
                    stoctr.ToStockId = ToStockID;
                    stoctr.StoreId = StoreID;
                    stoctr.NoItem = 1;
                    stoctr.Weight = item.Weight;
                    stoctr.Quantity = item.Weight;
                    stoctr.expiredtypid = item.expiredtypid;
                    stoctr.DemondOrderDetialsId = item.demondordid;
                    stoctr.FromStockId = item.FromStockID;
                    stoctr.OperationType = "+";
                    stoctr.ItemSupplierId = itemsupid;
                    db.StockTransactions.Add(stoctr);
                    db.SaveChanges();
                }
                foreach (var item in Modified)
                {
                    StockTransaction stoctr = db.StockTransactions.Find(item.ID);
                    stoctr.ToStockId = ToStockID;
                    stoctr.StoreId = StoreID;
                    stoctr.NoItem = 1;
                    stoctr.Weight = item.Weight;
                    stoctr.Quantity = item.Weight;
                    stoctr.expiredtypid = item.expiredtypid;
                    stoctr.DemondOrderDetialsId = item.demondordid;
                    stoctr.OperationType = "+";
                    stoctr.ItemSupplierId = itemsupid;

                    db.Entry(stoctr).State = EntityState.Modified;
                    db.SaveChanges();

                }
                foreach (var item in Deleted)
                {
                    StockTransaction stoctr = db.StockTransactions.Find(item.ID);
                    if (stoctr == null)
                        continue;

                    db.StockTransactions.Remove(stoctr);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                var t = ex.Message;
                return false;
            }
        }



        #endregion

        public AlertVM<StoreViewModel> GetExpiredAllStore()
        {

            AlertVM<StoreViewModel> Alert = new AlertVM<StoreViewModel>();
            var Model = new List<StoreViewModel>();


            try
            {
           

                Model = (from ts in db.Stores.Where(o => o.StoreType_ID ==(int) PlasticStatic.Enums.StoreType.finished)
                         select new StoreViewModel()
                         {
                             st_id = ts.st_id,
                             st_name = ts.st_name


                         }).ToList();

                Alert.type = true;
                Alert.data = Model;
            }
            catch
            {
                Model = null;
                Alert.type = false;
                Alert.data = Model;
            }

            return Alert;

        }

        public AlertVM<StoreViewModel> GetexpiredStores()
        {

            AlertVM<StoreViewModel> Alert = new AlertVM<StoreViewModel>();
            var Model = new List<StoreViewModel>();


            try
            {
         
                Model = (from ts in db.Stores.Where(o => o.StoreType_ID == (int)PlasticStatic.Enums.StoreType.expired)
                         select new StoreViewModel()
                         {
                             st_id = ts.st_id,
                             st_name = ts.st_name


                         }).ToList();

                Alert.type = true;
                Alert.data = Model;
            }
            catch
            {
                Model = null;
                Alert.type = false;
                Alert.data = Model;
            }

            return Alert;

        }


        public AlertVM<StoreViewModel> GetRawStores()
        {

            AlertVM<StoreViewModel> Alert = new AlertVM<StoreViewModel>();
            var Model = new List<StoreViewModel>();


            try
            {

                Model = (from ts in db.Stores.Where(o => o.StoreType_ID == (int)PlasticStatic.Enums.StoreType.Raw)
                         select new StoreViewModel()
                         {
                             st_id = ts.st_id,
                             st_name = ts.st_name


                         }).ToList();

                Alert.type = true;
                Alert.data = Model;
            }
            catch
            {
                Model = null;
                Alert.type = false;
                Alert.data = Model;
            }

            return Alert;

        }
        public List<SelectListItem> GetallitemSuplierslist (int demondorddetid)
        {
            var list = db.StockTransactions.
                Where(x => x.DemondOrderDetialsId == demondorddetid && x.OperationType == "-").
                GroupBy(x => x.ItemSupplier).Select(x => new SelectListItem
            {
                Text = x.Key.supplier.sup_name,
                Value = x.Key.ID.ToString()
                
            }).ToList();
           
            return list;
        }
        public int GetallitemSupliersid(int demondorddetid)
        {
            //var list = db.StockTransactions.
            //    Where(x => x.DemondOrderDetialsId == demondorddetid && x.OperationType == "-").
            //    GroupBy(x => x.ItemSupplier).Select(x => new SelectListItem
            //{
            //    Text = x.Key.supplier.sup_name,
            //    Value = x.Key.ID.ToString()

            //}).ToList();
            var ItemSupplierID = db.StockTransactions.
                Where(x => x.DemondOrderDetialsId == demondorddetid && x.OperationType == "-").
                GroupBy(x => new
                {
                    itemsupplier = x.ItemSupplier,

                }).Select(xx => new
                {
                    itemsuplierid = xx.Key.itemsupplier.ID,
                    SumQuantity = xx.Sum(y => y.Quantity)
                }).ToList().OrderByDescending(x => x.SumQuantity).First().itemsuplierid;


            return ItemSupplierID;
        }


    }
}
