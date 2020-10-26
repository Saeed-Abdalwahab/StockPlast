using StockPortal.ViewModel;
using StockDB.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using static PlasticStatic.Enums;

namespace StockService
{
  public  class StockTransaction_service
    {
        StockModel db = new StockModel();
        //Count Available
        public List<Avaliableiteminstore> GetAvlibleQuntityandNoiteminstock(int itemId, int? storeId,int fromstockid=0)
        {
            List<Avaliableiteminstore> Avaliableiteminstore = new List<Avaliableiteminstore>();
            List<StockTransaction> StockTransaction = new List<StockTransaction>();
            if (storeId == null||storeId==0)
            {
                StockTransaction = db.StockTransactions.Where(x => x.ItemSupplier.ItemId == itemId&&(x.ToStock==null||x.ToStock.InvoiceStatus==(int)InvoiceStatus.Finish)).ToList();
            }
            else
            {
                StockTransaction = db.StockTransactions.Where(x => x.StoreId == storeId && x.ItemSupplier.ItemId == itemId&&x.Store.StoreType_ID==(int?)PlasticStatic.Enums.StoreType.Raw && (x.ToStock.InvoiceStatus == (int)PlasticStatic.Enums.InvoiceStatus.Finish || x.FromStock.Status == (int)PlasticStatic.Enums.InvoiceStatus.Finish || x.FromStock.FromStockTypeID == (int)FromStockTypeId.purchesesreturn)).ToList();
                
            }
            
            if (StockTransaction == null)
            {
                return Avaliableiteminstore = null;
            }
            Avaliableiteminstore = StockTransaction.Where(x=>x.Store.StoreType_ID== (int)PlasticStatic.Enums.StoreType.Raw).
                GroupBy(x => new {

                storeid = x.Store.st_id,
                itemsuplierid=x.ItemSupplierId,
                itemname = x.ItemSupplier.Item.Name,
                supliername = x.ItemSupplier.supplier.sup_name,
                storename = x.Store.st_name,
                itemId=x.ItemSupplier.Item.ID,
                weight = x.Weight
            }
            ).Select(x => new Avaliableiteminstore
            {
                itemsuplierid = x.Key.itemsuplierid,
                storeid = x.Key.storeid,
               itemid=x.Key.itemId,
                itemname = x.Key.itemname,
                supliername = x.Key.supliername,
                storename = x.Key.storename,
                weight = x.Key.weight,
                Noitem = x.Sum(xx => xx.OperationType=="+"?xx.NoItem:-xx.NoItem)
            }).OrderBy(x=>x.weight).ToList();


            return Avaliableiteminstore.Where(x=>x.Noitem!=0&&x.weight!=0).ToList();
        }

        //Get Exactily Amount Avilable For Item Suplier and Weight  in Store 
        public double Avilableamount(int itemid,int itemsuplierid,int storeid,double weghit,double curent=0)
        {

            var Avilable= GetAvlibleQuntityandNoiteminstock(itemid, storeid).FirstOrDefault(xx => xx.itemsuplierid == itemsuplierid && xx.storeid == storeid && xx.weight == weghit);
            var x = Avilable == null ? 0.0 : Avilable.Noitem;
            if (x > 0)
            {
                return (double)x;
            }
            else
                return 0;
        }

        public bool saveinDatabase(string itemsuplierid, string storeid, string Noitem, string fromstockid, string wegiht,string Demondorderdetailsid,string itemid)
        {
            int iteemid = int.Parse(itemid);
            var result = false;


            try
            {
                if (double.Parse(Noitem) <= 0 || double.Parse(Noitem) > Avilableamount(iteemid, int.Parse(itemsuplierid), int.Parse(storeid), double.Parse(wegiht)))
                {
                    result = false;
                }
                

              
                else
                {
                    
                    StockTransaction stockTransaction = new StockTransaction();
                    stockTransaction.OperationType = "-";
                    stockTransaction.ItemSupplierId = int.Parse(itemsuplierid);
                    stockTransaction.StoreId = int.Parse(storeid);
                    stockTransaction.NoItem = double.Parse(Noitem);
                    stockTransaction.FromStockId = int.Parse(fromstockid);
                    stockTransaction.Weight = double.Parse(wegiht);
                    stockTransaction.DemondOrderDetialsId = int.Parse(Demondorderdetailsid);
                    stockTransaction.Quantity = double.Parse(wegiht) * double.Parse(Noitem);
                    db.StockTransactions.Add(stockTransaction);
                db.SaveChanges();
                    var DemondorderDetails = db.DemondOrderDetails.Find((int)stockTransaction.DemondOrderDetialsId);
                    DemondorderDetails.status = (int)demondorderdetailstatus.Underconstruction;
                    db.Entry(DemondorderDetails).State = EntityState.Modified;
                    db.SaveChanges();
                    
                result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public bool SaveEditInDatabase(string StockTransactionId,string NewNoItem)
        {
            var result = false;
            try
            {
                
                int ID = int.Parse(StockTransactionId);
                double Noitem = double.Parse(NewNoItem);
                StockTransaction stockTransaction = db.StockTransactions.FirstOrDefault(x => x.ID == ID);
                if (stockTransaction == null || Noitem <= 0 || Noitem > Avilableamount(stockTransaction.ItemSupplier.Item.ID, (int)stockTransaction.ItemSupplierId, (int)stockTransaction.StoreId, (int)stockTransaction.Weight,Noitem))
                {
                    result = false;
                   
                }
           
                else
                {
                    stockTransaction.NoItem = Noitem;
                    stockTransaction.Quantity = Noitem * stockTransaction.Weight;
                    db.Entry(stockTransaction).State = EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public bool SaveDelet(string stockTransactionId)
        {
            var result = false;
            try{
            int Id = int.Parse(stockTransactionId);
            StockTransaction stockTransaction = db.StockTransactions.FirstOrDefault(x => x.ID == Id);
                var DemonDorderDetailsID = stockTransaction.DemondOrderDetialsId;
            db.StockTransactions.Remove(stockTransaction);
            db.SaveChanges();
                //If I Delet All Items From Stock Return status For Demond Order Details Be Not Started
            var stockTest=    db.StockTransactions.FirstOrDefault(x => x.DemondOrderDetialsId == DemonDorderDetailsID && x.ToStock==null&&x.OperationType=="-"&&x.FromStockId!=null);
                if(stockTest == null)
                {
                    var DemondorderDetails = db.DemondOrderDetails.Find(DemonDorderDetailsID);
                    DemondorderDetails.status = (int)demondorderdetailstatus.NotStarted;
                    db.Entry(DemondorderDetails).State = EntityState.Modified;
                    db.SaveChanges();
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;

        }



    }
}
