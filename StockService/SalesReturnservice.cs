using StockDB.Model;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlasticStatic.Enums;

namespace StockService
{
    public class SalesReturnservice
    {
        private StockModel db = new StockModel();
        public string GetMaxSerial()
        {
            var tostock = db.ToStocks.OrderByDescending(x => x.ID).FirstOrDefault();
            return (tostock != null ? tostock.ID + 1 : 1).ToString();
        }
        public List<Store> GetexpiredStores()
        {
            return db.Stores.Where(x => x.StoreType_ID == (int)PlasticStatic.Enums.StoreType.expired).ToList() ;
        }
        public List<SalesReturnVM> Index(int fromstockid)
        {
         var list=   db.ToStocks.Where(x => x.ToStockTypeId == (int)ToStockTypeId.SalesReturns&&x.FromStockId== fromstockid).Select(x => new SalesReturnVM
            {
                ID = x.ID,
                InvoiceSerial = x.InvoiceSerial,
                InvoiceDate = x.InvoiceDate,
                Price = x.Price,
                cutomername = x.FromStock.DemondOrder.Customer.cust_name,
                InvoiceStatus = (InvoiceStatus)x.InvoiceStatus,
                Notes=x.Notes,
                FromStockId=x.FromStockId
            }).ToList();

            return list;
        }
        public DateTime SalesDate(int fromstockid)
        {
            return (DateTime)db.FromStocks.FirstOrDefault(x => x.FromStockTypeID == (int)FromStockTypeId.sales && x.ID == fromstockid).TransDate;
        }
        public SalesReturnVM Edit(int tostockid)
        {
            var list = db.StockTransactions.Where(x => x.ToStockId == tostockid && x.ToStock.ToStockTypeId == (int)ToStockTypeId.SalesReturns).ToList();
            foreach (var item in list)
            {
                item.Quantity = db.StockTransactions.FirstOrDefault(x => x.DemondOrderDetialsId == item.DemondOrderDetialsId && x.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.finished && x.FromStockId == item.FromStockId).Quantity;
            }
            var stocktransaction=      actuallyQuantity(list,true);
            SalesReturnVM salesReturnVM = new SalesReturnVM();
            salesReturnVM = stocktransaction.Select(x => new SalesReturnVM
            {
                storeid = (int)x.StoreId,
                InvoiceSerial = x.ToStock.InvoiceSerial,
                InvoiceDate=x.ToStock.InvoiceDate,
                FromStockId= x.ToStock.FromStockId,
                Price= x.ToStock.Price,
                Notes= x.ToStock.Notes,
                ID=(int)x.ToStockId,
                InvoiceStatus= (InvoiceStatus)x.ToStock.InvoiceStatus,
                cutomername= x.FromStock.DemondOrder.Customer.cust_name,
                stocktransactions=stocktransaction
            }).FirstOrDefault();
       
            return salesReturnVM;
        }
        private List<StockTransaction> actuallyQuantity(List<StockTransaction> stockTransaction,bool edit=false)
        {
            foreach (var item in stockTransaction)
            {
                var returnedquantity = db.StockTransactions.Where(x => x.ToStock.ToStockTypeId == (int)ToStockTypeId.SalesReturns && x.DemondOrderDetialsId == item.DemondOrderDetialsId).Sum(x => x.Quantity);
                if (returnedquantity != null)
                {
                    item.Quantity= edit == false ?   item.Quantity - returnedquantity :  item.Quantity-returnedquantity+item.Weight;
                }
            }
            return stockTransaction;
        }
        public SalesReturnVM Create(int fromstockid)
        {
            var fromstock = db.FromStocks.Find(fromstockid);
            SalesReturnVM salesReturnVM = new SalesReturnVM();
            salesReturnVM.FromStockId = fromstockid;
            salesReturnVM.cutomername = fromstock.DemondOrder.Customer.cust_name;
            salesReturnVM.InvoiceDate = DateTime.Now.Date;
            salesReturnVM.InvoiceSerial = GetMaxSerial();
            salesReturnVM.stocktransactions = db.StockTransactions.Where(x => x.FromStockId == fromstockid && x.FromStock.FromStockTypeID == (int)FromStockTypeId.sales&&x.ToStock==null).ToList();
            salesReturnVM.stocktransactions = actuallyQuantity(salesReturnVM.stocktransactions);
            return salesReturnVM;
        }
       public bool SaveinDb(SalesReturnVM salesReturnVM)
        {
            try
            {
                ToStock toStock;
                if (salesReturnVM.ID == 0)
                {
                   toStock = new ToStock();
                    toStock.FromStockId = salesReturnVM.FromStockId;
                    toStock.Price = salesReturnVM.Price;
                    toStock.ToStockTypeId = (int)ToStockTypeId.SalesReturns;
                    toStock.InvoiceSerial = salesReturnVM.InvoiceSerial;
                    toStock.InvoiceDate = salesReturnVM.InvoiceDate;
                    toStock.Notes = salesReturnVM.Notes;
                    toStock.InvoiceStatus = (int)salesReturnVM.InvoiceStatus;
                    db.ToStocks.Add(toStock);
                    db.SaveChanges();
                    foreach (var item in salesReturnVM.stocktransactions)
                    {
                        var stocktransaction = db.StockTransactions.Find(item.ID);
                        stocktransaction.Quantity = item.Quantity;
                        stocktransaction.Weight = item.Quantity;
                        stocktransaction.OperationType = "+";
                        stocktransaction.StoreId = salesReturnVM.storeid;
                        stocktransaction.ToStockId = toStock.ID;
                        db.StockTransactions.Add(stocktransaction);
                        db.SaveChanges();
                    }
                }
                else
                {
                     toStock = db.ToStocks.Find(salesReturnVM.ID);
                    toStock.FromStockId = salesReturnVM.FromStockId;
                    toStock.Price = salesReturnVM.Price;
                    toStock.ToStockTypeId = (int)ToStockTypeId.SalesReturns;
                    toStock.InvoiceSerial = salesReturnVM.InvoiceSerial;
                    toStock.InvoiceDate = salesReturnVM.InvoiceDate;
                    toStock.Notes = salesReturnVM.Notes;
                    toStock.InvoiceStatus = (int)salesReturnVM.InvoiceStatus;
                    db.Entry(toStock).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    foreach (var item in salesReturnVM.stocktransactions)
                    {
                      if(item.Quantity==0||item.Quantity==null||double.IsNaN((double)item.Quantity))
                        {
                            var stocktransaction = db.StockTransactions.Find(item.ID);

                            db.StockTransactions.Remove(stocktransaction);
                            db.SaveChanges();
                        }
                        else
                        {
                            var stocktransaction = db.StockTransactions.Find(item.ID);
                            stocktransaction.Quantity = item.Quantity;
                            stocktransaction.Weight = item.Quantity;
                            stocktransaction.OperationType = "+";
                            stocktransaction.StoreId = salesReturnVM.storeid;
                            stocktransaction.ToStockId = toStock.ID;
                            db.Entry(stocktransaction).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }

                if(toStock.InvoiceStatus==(int)InvoiceStatus.Finish)
                {
                    CustomerPaymentService.SaveReMotly(toStock);

                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool Delete(int tostockid)
        {
            try
            {
                var stocktransactions = db.StockTransactions.Where(x => x.ToStockId == tostockid).ToList();
                db.StockTransactions.RemoveRange(stocktransactions);
                db.SaveChanges();
                var tostock = db.ToStocks.Find(tostockid);
                db.ToStocks.Remove(tostock);
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
