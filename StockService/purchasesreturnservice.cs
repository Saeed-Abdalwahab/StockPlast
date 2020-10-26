using StockDB.Model;
using StockPortal.ViewModel;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlasticStatic.Enums;

namespace StockService
{
public    class purchasesreturnservice
    {
        private StockModel db = new StockModel();

        public string GetMaxSerial()
        {
            var fromstock = db.FromStocks.OrderByDescending(x => x.ID).FirstOrDefault();
            return (fromstock != null ? fromstock.ID + 1 : 1).ToString();
        }

        public List<purchasesreturnVM> Index()
        {
            var list = db.FromStocks.Where(x => x.FromStockTypeID == (int)FromStockTypeId.purchesesreturn).Select(x => new purchasesreturnVM
            {
                ID = x.ID,
                Serial = x.Serial,
                TransDate = x.TransDate,
                invoicecost = x.invoicecost,
                notes = x.notes,
                toStockid = x.ToStockId,
                supname=x.ToStock.supplier.sup_name,
                InvoiceStatus=(InvoiceStatus)x.Status

            }).ToList();

            return list;
        }

        public purchasesreturnVM Edit(int fromstockid)
        {
            var list = db.StockTransactions.Where(x => x.FromStockId == fromstockid && x.FromStock.FromStockTypeID == (int)FromStockTypeId.purchesesreturn).Select(x=>new Avaliableiteminstore
            {
                ID=x.ID,
                itemsuplierid=x.ItemSupplierId,
                itemname=x.ItemSupplier.Item.Name,
                ReturnedItemPrice=x.ItemPrice,
                ReturnedNoitem=x.NoItem,
                weight=x.Weight,
                itemid =(int)x.ItemSupplier.ItemId,
                
            }).ToList();
            foreach (var item in list)
            {
                //-- To determin The Original No Of Item For Purcheses --
                item.ItemPrice = db.Items.Find(item.itemid).PurchasingPrice;
                item.Noitem = db.StockTransactions.Where(x => x.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.Raw && x.ItemSupplier.ItemId == item.itemid && ((x.FromStock.FromStockTypeID == (int)FromStockTypeId.purchesesreturn || x.FromStock.Status == (int)InvoiceStatus.Finish) || x.ToStock.InvoiceStatus == (int)InvoiceStatus.Finish) && x.ID != item.ID).Sum(x => x.OperationType == "+" ? x.NoItem : -x.NoItem);
            }
            purchasesreturnVM purchasesreturnVM = new purchasesreturnVM();
            var fromstock = db.FromStocks.Find(fromstockid);
            purchasesreturnVM.ID = fromstock.ID;
            purchasesreturnVM.Serial = fromstock.Serial;
            purchasesreturnVM.TransDate = fromstock.TransDate;
            purchasesreturnVM.notes = fromstock.notes;
            purchasesreturnVM.toStockid = fromstock.ToStockId;
            purchasesreturnVM.invoicecost = fromstock.invoicecost;
            purchasesreturnVM.InvoiceStatus = (InvoiceStatus)fromstock.Status;
            purchasesreturnVM.avaliableiteminstores = list ;

            return purchasesreturnVM;
        }

        private List<StockTransaction> actuallyQuantity(List<StockTransaction> stockTransaction, bool edit = false)
        {
            double? ReturnedNoItem = null;

            foreach (var item in stockTransaction)
            {
                if (edit == false) {
                     ReturnedNoItem = db.StockTransactions.Where(x => x.ItemSupplierId == item.ItemSupplierId && x.Weight == item.Weight && x.FromStock.FromStockTypeID == (int)FromStockTypeId.purchesesreturn && x.FromStock.ToStockId == item.ToStockId).Sum(x => x.NoItem);

                }
                else { 
                 ReturnedNoItem = db.StockTransactions.Where(x =>x.ItemSupplierId==item.ItemSupplierId && x.Weight==item.Weight&& x.FromStock.FromStockTypeID == (int)FromStockTypeId.purchesesreturn&&x.FromStock.ToStockId==item.FromStock.ToStockId ).Sum(x => x.NoItem);
                }
                if (ReturnedNoItem != null)
                {
                    item.Quantity = edit == false ? item.Quantity - ReturnedNoItem : item.Quantity - ReturnedNoItem + item.NoItem;
                }
            }
            return stockTransaction;
        }
        public List<Avaliableiteminstore> AvilableinStock(int supplierid, int StoreID)
        {
     var list=     db.StockTransactions.Where(x => x.ItemSupplier.SupId == supplierid && x.StoreId == StoreID && ((x.FromStock.FromStockTypeID == (int)FromStockTypeId.purchesesreturn||x.FromStock.Status == (int)InvoiceStatus.Finish) || x.ToStock.InvoiceStatus == (int)InvoiceStatus.Finish)).
              GroupBy(x => new {

                 
                  itemsuplierid = x.ItemSupplierId,
                  itemname = x.ItemSupplier.Item.Name,
                  itemId = x.ItemSupplier.Item.ID,
                  weight = x.Weight,
                  storeid=x.StoreId
              }
            ).Select(x => new Avaliableiteminstore
            {
                itemsuplierid = x.Key.itemsuplierid,
              storeid=(int)x.Key.storeid,
                itemid=x.Key.itemId,
                itemname = x.Key.itemname,
                weight = x.Key.weight,
                Noitem = x.Sum(xx => xx.OperationType == "+" ? xx.NoItem : -xx.NoItem)
            }).OrderBy(x => x.weight).ToList();
            foreach (var item in list)
            {
                item.ItemPrice = db.Items.Find(item.itemid).PurchasingPrice;
            }
            list.RemoveAll(x => x.Noitem <= 0 || x.weight <= 0);
            return list;
        }
        public DateTime PurchesesDate(int tostockid)
        {
            return (DateTime) db.ToStocks.FirstOrDefault(x => x.ToStockTypeId == (int) ToStockTypeId.PurchaseInvoice && x.ID == tostockid).InvoiceDate;
        }
        public List<supplier> Suppliers()
        {
            return db.suppliers.ToList();
        }
        public List<Store> RawStores()
        {
            return db.Stores.Where(x=>x.StoreType_ID==(int)PlasticStatic.Enums.StoreType.Raw).ToList();
        }
        //Create Purchesesreturn invoice

        public purchasesreturnVM Create(int tostockid)
        {
            var tostock = db.ToStocks.Find(tostockid);
            purchasesreturnVM purchasesreturnVM = new purchasesreturnVM();

            purchasesreturnVM.toStockid = tostock.ID;
            purchasesreturnVM.stocktransactions = db.StockTransactions.Where(x => x.ToStockId == tostockid && x.ToStock.ToStockTypeId == (int)ToStockTypeId.PurchaseInvoice &&x.ToStock.InvoiceStatus==(int)InvoiceStatus.Finish&&x.FromStockId==null).ToList();
            foreach (var item in purchasesreturnVM.stocktransactions)
            {
                //-- To determin The Original No Of Item For Purcheses --
                item.Quantity = item.NoItem;
            }
            purchasesreturnVM.stocktransactions = actuallyQuantity(purchasesreturnVM.stocktransactions);
            purchasesreturnVM.Serial = GetMaxSerial();
            //purchasesreturnVM.TransDate = DateTime.Now;
            return purchasesreturnVM;
        }

        public bool SaveinDb(purchasesreturnVM purchasesreturnVM)
        {
            try
            {
               

                FromStock fromStock;

                if (purchasesreturnVM.ID == 0)
                {
                    fromStock = new FromStock();
                    fromStock.ToStockId = purchasesreturnVM.toStockid;
                    fromStock.invoicecost = purchasesreturnVM.invoicecost;
                    fromStock.TransDate = purchasesreturnVM.TransDate;
                    fromStock.Serial = purchasesreturnVM.Serial;
                    fromStock.notes = purchasesreturnVM.notes;
                    fromStock.Status = (int)purchasesreturnVM.InvoiceStatus;
                    fromStock.FromStockTypeID =(int)FromStockTypeId.purchesesreturn;
                    db.FromStocks.Add(fromStock);
                    db.SaveChanges();
                    foreach (var item in purchasesreturnVM.stocktransactions)
                    {
                        StockTransaction stocktransaction = new StockTransaction();
                       
                        stocktransaction.NoItem = item.NoItem;
                        stocktransaction.Quantity = item.Weight * item.NoItem;
                        stocktransaction.OperationType = "-";
                        stocktransaction.StoreId = item.StoreId;
                        stocktransaction.ItemPrice = item.ItemPrice;
                        stocktransaction.Weight = item.Weight;
                        stocktransaction.ItemSupplierId = item.ItemSupplierId;
                        stocktransaction.FromStockId = fromStock.ID;
                        stocktransaction.ToStockId = null;
                        db.StockTransactions.Add(stocktransaction);
                        db.SaveChanges();
                    }
                }
                else
                {
                     fromStock = db.FromStocks.Find(purchasesreturnVM.ID);
                    fromStock.invoicecost = purchasesreturnVM.invoicecost;
                    fromStock.TransDate = purchasesreturnVM.TransDate;
                    fromStock.Serial = purchasesreturnVM.Serial;
                    fromStock.Status = (int)purchasesreturnVM.InvoiceStatus;

                    fromStock.notes = purchasesreturnVM.notes;
                    fromStock.FromStockTypeID = (int)FromStockTypeId.purchesesreturn;
                    db.Entry(fromStock).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    var oldInvoiceItems = db.StockTransactions.Where(x => x.FromStockId == fromStock.ID && x.FromStock.FromStockTypeID == (int)FromStockTypeId.purchesesreturn).ToList();
                    var DeltedItems = oldInvoiceItems.Where(xx => !purchasesreturnVM.stocktransactions.Select(x => x.ID).Contains(xx.ID)).ToList();
                    if (oldInvoiceItems.Count > purchasesreturnVM.stocktransactions.Count)
                    {
                        db.StockTransactions.RemoveRange(DeltedItems);
                        db.SaveChanges();
                    }
                    foreach (var item in purchasesreturnVM.stocktransactions)
                    {
                       
                      
                            var stocktransaction = db.StockTransactions.Find(item.ID);
                            stocktransaction.NoItem = item.NoItem;
                            stocktransaction.Quantity = stocktransaction.Weight * item.NoItem;
                            stocktransaction.FromStockId = fromStock.ID;
                        stocktransaction.Weight = item.Weight;
                            db.Entry(stocktransaction).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        
                    }
                }
                if (fromStock.Status == (int)InvoiceStatus.Finish)
                {

                SupplierPaymentsService.SaveReMotly(fromStock);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        //public string CheckAvilableinstore(int tostockid, int stocktransactionid, double ReturnedValue)
        //{
        //    var itemsuplierid = db.StockTransactions.Find(stocktransactionid).ItemSupplierId;
        //    db.StockTransactions.Where(x => x.ItemSupplierId == itemsuplierid);
        //}
        public bool Delete(int fromstockid)
        {
            try
            {
                var stocktransactions = db.StockTransactions.Where(x => x.FromStockId == fromstockid&& x.FromStock.FromStockTypeID==(int)FromStockTypeId.purchesesreturn).ToList();
                db.StockTransactions.RemoveRange(stocktransactions);
                db.SaveChanges();
                var fromStock = db.FromStocks.Find(fromstockid);
                db.FromStocks.Remove(fromStock);
                db.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }


    }
}
