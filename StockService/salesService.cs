using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockDB.Model;
using StockViewModel;
using static PlasticStatic.Enums;

namespace StockService
{
   public class salesService
    {
       private StockModel db = new StockModel();
        public string GetMaxSerial()
        {
            var fromStock = db.FromStocks.OrderByDescending(x => x.ID).FirstOrDefault();
            return (fromStock   != null ? fromStock.ID + 1 : 1).ToString();
        }


        //For Retrive all finished Demond Order Details  list 
        public List<SalesVM> Getsalesinvoice()
        {
            var salesinvoice = db.FromStocks.Where(x => x.FromStockTypeID == (int)FromStockTypeId.sales).
                Select(x => new SalesVM
                {
                    Sale_ID=x.ID,
                    cust_name = x.DemondOrder.Customer.cust_name,
                    Cust_phone = x.DemondOrder.Customer.cust_mobile,
                    sale_charge = x.sale_charge,
                    sale_tax = x.sale_tax,
                    sale_discount = x.sale_discount,
                    invoicecost=x.invoicecost,
                    invoicestatus=(InvoiceSSAlestatus)x.Status
                 
                }).ToList();

            return salesinvoice;
        }

        public List<StockTransaction> GetfinishedDemondorderdetails(List<int> ids)
        {
            List<StockTransaction> StockTransactionList = new List<StockTransaction>();
            foreach (var item in ids)
            {

               var tt= db.StockTransactions.Where(x => x.DemondOrderDetialsId == item && x.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.finished&&x.OperationType=="+"&&x.DemondOrderDetail.status==(int)demondorderdetailstatus.Completed).FirstOrDefault();
                if(tt!=null)
                StockTransactionList.Add(tt);

            }
            return StockTransactionList;
        }
        public List<StockTransaction> GetfinishedDemondorderdetailsForEdit(List<int> ids,int fromstockid)
        {
            List<StockTransaction> StockTransactionList = new List<StockTransaction>();
            foreach (var item in ids)
            {
                var tt = db.StockTransactions.Where(x => x.DemondOrderDetialsId == item && x.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.finished && x.OperationType == "-"&&x.FromStockId==fromstockid&&x.FromStock.FromStockTypeID==(int)FromStockTypeId.sales&&x.DemondOrderDetail.status==(int)demondorderdetailstatus.SaleingProcess).FirstOrDefault();
                StockTransactionList.Add(tt);
            }
            return StockTransactionList;
        }
        public List<StockTransaction> GetfinishedDemondorderdetailsForDetails(List<int> ids, int fromstockid)
        {
            List<StockTransaction> StockTransactionList = new List<StockTransaction>();
            foreach (var item in ids)
            {
                var tt = db.StockTransactions.Where(x => x.DemondOrderDetialsId == item && x.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.finished && x.OperationType == "-" && x.FromStockId == fromstockid && x.FromStock.FromStockTypeID == (int)FromStockTypeId.sales && x.DemondOrderDetail.status == (int)demondorderdetailstatus.Delivery).FirstOrDefault();
                StockTransactionList.Add(tt);
            }
            return StockTransactionList;
        }

        public List<DemondOrderDetail> GetfinishedDemondorderdetails(int ID)
        {
            var DemondorderDetailsList = db.DemondOrderDetails.Where(x => x.DemondOrder.cust_id == ID&&x.status==(int)demondorderdetailstatus.Completed).ToList();
            return DemondorderDetailsList;
        }
        public List<DemondOrderDetail> GetfinishedDemondorderdetailsForEdit(int ID)
        {
            var DemondorderDetailsList = db.DemondOrderDetails.Where(x => x.DemondOrder.cust_id == ID && x.status == (int)demondorderdetailstatus.SaleingProcess).ToList();
            return DemondorderDetailsList;
        }
        public List<DemondOrderDetail> GetfinishedDemondorderdetailsFodetails(int ID)
        {
            var DemondorderDetailsList = db.DemondOrderDetails.Where(x => x.DemondOrder.cust_id == ID && x.status == (int)demondorderdetailstatus.Delivery).ToList();
            return DemondorderDetailsList;
        }
        public Customer Custinfo(int CustID)
        {
            return db.Customers.Find(CustID);
        }
        public bool SaveinDb(SalesVM salesVM)
        {
            bool result = true;
            if (salesVM.stocktranslist.Count == 0) return false;
            try
            {
                FromStock fromStock = new FromStock();
                if(salesVM.TotalCost<0)
                {
                    return false;
                }
                fromStock.FromStockTypeID = (int)FromStockTypeId.sales;
                fromStock.notes = salesVM.notes;
                fromStock.sale_charge = salesVM.sale_charge;
                fromStock.sale_discount = salesVM.sale_discount;
                fromStock.sale_tax = salesVM.sale_tax;
                fromStock.Serial = salesVM.Serial;
                fromStock.TransDate = salesVM.TransDate;
                fromStock.invoicecost = Math.Round(Convert.ToDouble(salesVM.TotalCost),2);
                fromStock.Status = (int)salesVM.invoicestatus;
                fromStock.DemondOrderId = db.DemondOrders.FirstOrDefault(x => x.cust_id == salesVM.cust_ID).demoOrd_id;
                db.FromStocks.Add(fromStock);
                db.SaveChanges();

                foreach (var item in salesVM.stocktranslist)
                {
                    if (item.ID <= 0) continue;
                    var oldstocktransaction = db.StockTransactions.Find(item.ID);
                    StockTransaction stockTransaction = new StockTransaction();
                    stockTransaction.FromStockId = fromStock.ID;
                    stockTransaction.NoItem = 1;
                    stockTransaction.OperationType = "-";
                    stockTransaction.Quantity = oldstocktransaction.Quantity;
                    stockTransaction.Weight = oldstocktransaction.Weight;
                    stockTransaction.ItemPrice = item.SelingPrice;
                    stockTransaction.StoreId = oldstocktransaction.StoreId;
                    stockTransaction.ItemSupplierId = oldstocktransaction.ItemSupplierId;
                    stockTransaction.DemondOrderDetialsId = oldstocktransaction.DemondOrderDetialsId;
                    db.StockTransactions.Add(stockTransaction);
                    db.SaveChanges();
               var demondOrderDetail = db.DemondOrderDetails.Find(oldstocktransaction.DemondOrderDetialsId);
                demondOrderDetail.status = salesVM.invoicestatus==InvoiceSSAlestatus.Finish? (int)demondorderdetailstatus.Delivery: (int)demondorderdetailstatus.SaleingProcess;
                    demondOrderDetail.Selling_Price = item.SelingPrice;
                    demondOrderDetail.ActualQuantity = item.ActualQuantity;
                    demondOrderDetail.Hand_Price = item.HandPrice;
                    demondOrderDetail.Hand_Count = item.HandCount;
                    
                db.Entry(demondOrderDetail).State= EntityState.Modified;
                db.SaveChanges();
                    updatePriceInItemTbl((int)demondOrderDetail.Item_id,(double)stockTransaction.ItemPrice);

                }
               
                    CustomerPaymentService.SaveReMotly(fromStock);
                
            }

            catch 
            {
                result = false;
            }
            return result;
        }

        public bool SaveEditinDb(SalesVM salesVM)
        {
            bool result = true;
            if (salesVM.stocktranslist.Count == 0) return false;
            try
            {
                FromStock fromStock = db.FromStocks.Find(salesVM.Sale_ID);
                if (salesVM.TotalCost < 0)
                {
                    return false;
                }
                fromStock.FromStockTypeID = (int)FromStockTypeId.sales;
                fromStock.notes = salesVM.notes;
                fromStock.sale_charge = salesVM.sale_charge;
                fromStock.sale_discount = salesVM.sale_discount;
                fromStock.sale_tax = salesVM.sale_tax;
                fromStock.Serial = salesVM.Serial;
                fromStock.TransDate = salesVM.TransDate;
                fromStock.invoicecost = Math.Round(Convert.ToDouble(salesVM.TotalCost),2);
                fromStock.Status = (int)salesVM.invoicestatus;
                db.Entry(fromStock).State = EntityState.Modified;
                db.SaveChanges();
                var OldStockTransaction = db.StockTransactions.Where(x => x.FromStockId == salesVM.Sale_ID && x.FromStock.FromStockTypeID == (int)FromStockTypeId.sales && x.DemondOrderDetail.status == (int)demondorderdetailstatus.SaleingProcess).ToList();

                foreach (var item in salesVM.stocktranslist)
                {
                    if (item.ID <= 0) continue;
                    StockTransaction stockTransaction = db.StockTransactions.Find(item.ID);
                  stockTransaction.ItemPrice = item.SelingPrice;

                    db.Entry(stockTransaction).State = EntityState.Modified;
                    db.SaveChanges();
                    var demondOrderDetail = db.DemondOrderDetails.Find(stockTransaction.DemondOrderDetialsId);
                    demondOrderDetail.status = salesVM.invoicestatus == InvoiceSSAlestatus.Finish ? (int)demondorderdetailstatus.Delivery : (int)demondorderdetailstatus.SaleingProcess;

                    demondOrderDetail.Selling_Price = item.SelingPrice;
                    demondOrderDetail.ActualQuantity = item.ActualQuantity;
                    demondOrderDetail.Hand_Price = item.HandPrice;
                    demondOrderDetail.Hand_Count = item.HandCount;
                    db.Entry(demondOrderDetail).State = EntityState.Modified;
                    db.SaveChanges();
                    updatePriceInItemTbl((int)demondOrderDetail.Item_id,(double)stockTransaction.ItemPrice);


                }
                var DeletedStockTransaction=OldStockTransaction.Where(x => !salesVM.stocktranslist.Select(xx => xx.ID).Contains(x.ID)).ToList();
                foreach (var item in DeletedStockTransaction)
                {
                    var demondOrderDetail = db.DemondOrderDetails.Find(item.DemondOrderDetialsId);
                    demondOrderDetail.status = (int)demondorderdetailstatus.Completed;

                    db.Entry(demondOrderDetail).State = EntityState.Modified;
                    db.SaveChanges();
                    db.StockTransactions.Remove(item);
                    db.SaveChanges();
                }

                
                    CustomerPaymentService.SaveReMotly(fromStock);
                
            }

            catch
            {
                result = false;
            }
            return result;
        }

        public void updatePriceInItemTbl(int itemId,double CurentPrice)
        {
            StockDB.Model.StockModel stockDB = new StockModel();
            var context = new StockModel();
            var lastPrice = context.StockTransactions.Where(x => x.ItemSupplier.ItemId == itemId && x.FromStock.FromStockTypeID == (int)FromStockTypeId.sales && x.ToStock.InvoiceStatus == (int)InvoiceStatus.Finish).OrderByDescending(x => x.FromStock.TransDate).ThenBy(x => x.FromStock).FirstOrDefault();

            var itemedit = context.Items.Find(itemId);

            itemedit.SellingPrice = lastPrice==null?CurentPrice:lastPrice.ItemPrice;
            context.Entry(itemedit).State = EntityState.Modified;
            context.SaveChanges();
            context.Dispose();

        }


        public bool Delete(int FromStockid)
        {
            try
            {

                var stockTransactions = db.StockTransactions.Where(x => x.FromStockId == FromStockid && x.FromStock.FromStockTypeID == (int)FromStockTypeId.sales && x.DemondOrderDetail.status == (int)demondorderdetailstatus.SaleingProcess).ToList() ;
                foreach (var item in stockTransactions)
                {
                    var demondOrderDetail = db.DemondOrderDetails.Find(item.DemondOrderDetialsId);
                    demondOrderDetail.status = (int)demondorderdetailstatus.Completed;
                    db.Entry(demondOrderDetail).State = EntityState.Modified;
                    db.SaveChanges();
                    db.StockTransactions.Remove(db.StockTransactions.Find(item.ID));
                    db.SaveChanges();
                }
                var CustomerPayment = db.CustomerPayments.FirstOrDefault(x => x.FromStock.FromStockTypeID == (int)FromStockTypeId.sales && x.FromstockID == FromStockid);
                if (CustomerPayment != null)
                {
                    db.CustomerPayments.Remove(CustomerPayment);
                    db.SaveChanges();
                }
                db.FromStocks.Remove(db.FromStocks.Find(FromStockid));
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
