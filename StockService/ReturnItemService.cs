using StockDB.Model;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlasticStatic.Enums;

namespace StockService
{
  public  class ReturnItemService
    {
        public AlertVM<PurchaseInvoiceViewModel> GetfinishTostockList(int FromStockID,int Demondorderdetailsid)
        {
            AlertVM<PurchaseInvoiceViewModel> Alert = new AlertVM<PurchaseInvoiceViewModel>();
            var Model = new List<PurchaseInvoiceViewModel>();
            try
            {
                StockModel db = new StockModel();
                Model = db.StockTransactions.Where(T => T.ToStock.FromStockId == FromStockID && T.ToStock.ToStockTypeId == (int)ToStockTypeId.FinishedItem&&T.DemondOrderDetialsId==Demondorderdetailsid).Select(ts => new PurchaseInvoiceViewModel
                {
                    ID = (int)ts.ToStockId,
                    InvoiceSerial = ts.ToStock.InvoiceSerial,
                    InvoiceDate = ts.ToStock.InvoiceDate,
                    Notes = ts.ToStock.Notes,
                    SupId = ts.ToStock.SupId,
                    SupName = ts.ToStock.supplier.sup_name,
                    demondordid=ts.DemondOrderDetialsId,
                    StoreName = ts.Store.st_name,
                    InvoiceStatus=ts.ToStock.InvoiceStatus,
                    Weight=ts.Quantity
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
        public AlertVM<PurchaseInvoiceViewModel> GetExpiredItemList(int ToStockID)
        {
            AlertVM<PurchaseInvoiceViewModel> Alert = new AlertVM<PurchaseInvoiceViewModel>();
            var Model = new List<PurchaseInvoiceViewModel>();
            try
            {
                StockModel db = new StockModel();
                Model = db.StockTransactions.Where(T => T.ToStockId == ToStockID).Select(ts => new PurchaseInvoiceViewModel
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

        public AlertVM<PurchaseInvoiceViewModel> GetTOstockExpiredByID(int ToStockID, int FromStockID)
        {
            AlertVM<PurchaseInvoiceViewModel> Alert = new AlertVM<PurchaseInvoiceViewModel>();

            var Model = new PurchaseInvoiceViewModel();


            try
            {
                StockModel db = new StockModel();
                Model = db.ToStocks.Where(T => T.ID == ToStockID && T.FromStockId == FromStockID && T.ToStockTypeId == (int)ToStockTypeId.OperationReturns).Select(ts => new PurchaseInvoiceViewModel
                {
                    ID = ts.ID,
                    InvoiceSerial = ts.InvoiceSerial,
                    InvoiceDate = ts.InvoiceDate,
                    Notes = ts.Notes,
                    SupId = ts.SupId,
                    SupName = ts.supplier.sup_name,
                    StoreName = db.StockTransactions.FirstOrDefault(x => x.ToStockId == ts.ID).Store.st_name,
                    StoreId = db.StockTransactions.FirstOrDefault(x => x.ToStockId == ts.ID).StoreId,
                    InvoiceStatus = ts.InvoiceStatus

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
                StockModel db = new StockModel();

                var Item = fromStock.GetStockTransactionByID(vm.StockTransactioID).Data;
                vm.ID = vm.ID == -1 ? 0 : vm.ID;
                tostock.InvoiceSerial = vm.InvoiceSerial;
                tostock.InvoiceDate = vm.InvoiceDate;
                tostock.SupId = db.ItemSuppliers.Find(Item.ItemSupID).SupId;
                tostock.Notes = vm.Notes;
                tostock.ID = vm.ID;
                tostock.InvoiceStatus = vm.InvoiceStatus;
                tostock.ToStockTypeId = (int?)ToStockTypeId.OperationReturns;
                tostock.FromStockId = vm.FromStockID;

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

                if (AddItem(tostock.ID, vm.StoreId, insert, Modified, Deleted) == false)
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
        #region Add Item
        public bool AddItem(int ToStockID, int? StoreID, List<PurchaseInvoiceViewModel> insert, List<PurchaseInvoiceViewModel> Modified, List<PurchaseInvoiceViewModel> Deleted)
        {
            try
            {

                StockModel db = new StockModel();
                foreach (var item in insert)
                {
                    StockTransaction stoctr = new StockTransaction();
                    stoctr.ToStockId = ToStockID;
                    stoctr.StoreId = StoreID;
                    stoctr.NoItem = item.NoItem;
                    stoctr.Weight = item.Weight;
                    stoctr.OperationType = "+";


                    db.StockTransactions.Add(stoctr);
                    db.SaveChanges();
                }
                foreach (var item in Modified)
                {
                    StockTransaction stoctr = db.StockTransactions.Find(item.ID);
                    stoctr.StoreId = StoreID;
                    stoctr.ToStockId = ToStockID;
                    stoctr.NoItem = item.NoItem;
                    stoctr.Weight = item.Weight;
                    stoctr.OperationType = "+";
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
            catch (Exception)
            {

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
                StockModel db = new StockModel();

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



    }
}
