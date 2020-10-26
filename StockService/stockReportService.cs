using StockViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockDB.Model;
using StockPortal.ViewModel;
using static PlasticStatic.Enums;

namespace StockService
{
    public class stockReportService
    {
        StockModel db = new StockModel(); 
        public List<stockReportVM> avilableinstore(int storetypid)
        {
            List<stockReportVM> avilableinstock = new List<stockReportVM>();
            if (storetypid == (int)PlasticStatic.Enums.StoreType.expired)
            {
                avilableinstock = db.StockTransactions.Where(x => x.Store.StoreType_ID == storetypid && x.ToStock.InvoiceStatus == (int)PlasticStatic.Enums.InvoiceStatus.Finish&&(x.ToStock.ToStockTypeId == (int)ToStockTypeId.SalesReturns|| x.ToStock.ToStockTypeId == (int)ToStockTypeId.ExpiredItem) ).GroupBy(x => new
                {
                    storeName = x.Store.st_name,
                    itemname = x.ItemSupplier.Item.Name,
                    supliername = x.ItemSupplier.supplier.sup_name,
                    Weight = x.Weight,
                    StoreTypID = x.Store.StoreType_ID

                }).Select(x => new stockReportVM
                {
                    storeName = x.Key.storeName,
                    itemname = x.Key.itemname,
                    supliername = x.Key.StoreTypID != (int)PlasticStatic.Enums.StoreType.Raw ? null : x.Key.supliername,
                    Weight = x.Key.Weight,
                    NoItem = x.Sum(xx => xx.OperationType == "+" ? xx.NoItem : -xx.NoItem),
                    Quantity = x.Sum(xx => xx.OperationType == "+" ? xx.Quantity : -xx.Quantity)

                }).OrderBy(x => x.storeName).ThenByDescending(x => x.Quantity).ToList();


            }
            else
            {
                avilableinstock = db.StockTransactions.Where(x => x.Store.StoreType_ID == storetypid && (x.ToStock.InvoiceStatus == (int)PlasticStatic.Enums.InvoiceStatus.Finish || x.FromStock.Status == (int)PlasticStatic.Enums.InvoiceStatus.Finish || x.FromStock.FromStockTypeID == (int)FromStockTypeId.purchesesreturn)).GroupBy(x => new
                {
                    storeName = x.Store.st_name,
                    itemname = x.ItemSupplier.Item.Name,
                    supliername = x.ItemSupplier.supplier.sup_name,
                    Weight = x.Weight,
                    StoreTypID = x.Store.StoreType_ID

                }).Select(x => new stockReportVM
                {
                    storeName = x.Key.storeName,
                    itemname = x.Key.itemname,
                    supliername = x.Key.StoreTypID != (int)PlasticStatic.Enums.StoreType.Raw ? null : x.Key.supliername,
                    Weight = x.Key.Weight,
                    NoItem = x.Sum(xx => xx.OperationType == "+" ? xx.NoItem : -xx.NoItem),
                    Quantity = x.Sum(xx => xx.OperationType == "+" ? xx.Quantity : -xx.Quantity)

                }).OrderBy(x => x.storeName).ThenByDescending(x => x.Quantity).ToList();


            }
            avilableinstock.RemoveAll(x => x.NoItem <= 0 || x.Weight <= 0);
            return avilableinstock;
        }
        public List<stockReportVM> avilableinstore(int storetypid,int itemid)
        {
            var avilableinstock = db.StockTransactions.Where(x => x.Store.StoreType_ID == storetypid&&x.ItemSupplier.ItemId==itemid).GroupBy(x => new
            {
                storeName = x.Store.st_name,
                itemname = x.ItemSupplier.Item.Name,
                supliername = x.ItemSupplier.supplier.sup_name,
          
                Weight = x.Weight,
                StoreTypID=x.Store.StoreType_ID

            }).Select(x => new stockReportVM
            {
                storeName = x.Key.storeName,
                itemname = x.Key.itemname,
                supliername = x.Key.StoreTypID != (int)PlasticStatic.Enums.StoreType.Raw ? null : x.Key.supliername,
                Weight = x.Key.Weight,
                NoItem = x.Sum(xx => xx.OperationType == "+" ? xx.NoItem : -xx.NoItem),
                Quantity = x.Sum(xx => xx.OperationType == "+" ? xx.Quantity : -xx.Quantity)

            }).OrderBy(x => x.storeName).ThenByDescending(x => x.Quantity).ToList();
            avilableinstock.RemoveAll(x => x.NoItem <= 0 || x.Weight <= 0);

            return avilableinstock;
        }


        public List<stockReportVM> avilableinFinishstore()
        {
            var avilableinstock = db.StockTransactions.Where(x => x.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.finished&& x.FromStock.Status == (int)PlasticStatic.Enums.InvoiceStatus.Finish).GroupBy(x => new
            {
                storeName = x.Store.st_name,
                DemondorderDetailsName = x.DemondOrderDetail.Shaplona.shap_name,
                CustomerName = x.DemondOrderDetail.DemondOrder.Customer.cust_name,

                quantity = x.Quantity,


            }).Select(x => new stockReportVM
            {
                storeName = x.Key.storeName,
                supliername = x.Key.CustomerName,
                itemname = x.Key.DemondorderDetailsName,
                Quantity = x.Sum(xx => xx.OperationType == "+" ? xx.Quantity : -xx.Quantity)

            }).OrderBy(x => x.storeName).ThenByDescending(x => x.Quantity).ToList();
            avilableinstock.RemoveAll(x => x.NoItem <= 0 || x.Weight <= 0);

            return avilableinstock.Where(x=>x.Quantity>0).ToList();
        }
        public List<stockReportVM> avilableinFinishstore(int itemid)
        {
            var avilableinstock = db.StockTransactions.Where(x => x.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.finished && x.ItemSupplier.ItemId == itemid&&x.FromStock.FromStockTypeID==(int)FromStockTypeId.sales&&x.FromStock.Status==(int)InvoiceStatus.Finish).GroupBy(x => new
            {
                storeName = x.Store.st_name,
                DemondorderDetailsName = x.DemondOrderDetail.Shaplona.shap_name,
                CustomerName = x.DemondOrderDetail.DemondOrder.Customer.cust_name,

                quantity = x.Quantity,


            }).Select(x => new stockReportVM
            {
                storeName = x.Key.storeName,
                supliername=x.Key.CustomerName,
                itemname = x.Key.DemondorderDetailsName,
                Quantity = x.Sum(xx => xx.OperationType == "+" ? xx.Quantity : -xx.Quantity)

            }).OrderBy(x => x.storeName).ThenByDescending(x => x.Quantity).ToList();
            avilableinstock.RemoveAll(x => x.NoItem <= 0 || x.Weight <= 0);

            return avilableinstock.Where(x=>x.Quantity>0).ToList();
        }


        public List<Item> Items()
        {
            return db.Items.ToList();
        }        

    }
}
