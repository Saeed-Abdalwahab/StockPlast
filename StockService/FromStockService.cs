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
    public class FromStockService
    {
        public AlertVM<FromStockDetailsVM> GetFromStockByID(int ID)
        {
            var Model = new FromStockDetailsVM();

            AlertVM<FromStockDetailsVM> Alert = new AlertVM<FromStockDetailsVM>();


            try
            {
                StockModel db = new StockModel();
                Model = (from ts in db.FromStocks.Where(X => X.ID == ID)
                         select new FromStockDetailsVM()
                         {
                             ID=ts.ID,
                             Serial=ts.Serial,
                             cutTechName=ts.Employe1.emp_name,
                             printTechName=ts.Employe.emp_name,
                             Custname=ts.DemondOrder.Customer.cust_name,
                                                          TransDate=ts.TransDate,
                             demoOrd_serailNum=ts.DemondOrder.demoOrd_serailNum
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
        public AlertVM<StockTransactionVM> FromStockDetailsList(int ID)
        {
            var Model = new List<StockTransactionVM>();

            AlertVM<StockTransactionVM> Alert = new AlertVM<StockTransactionVM>();


            try
            {
                StockModel db = new StockModel();
                Model = (from ts in db.StockTransactions.Where(X => X.FromStockId == ID&&X.ToStock==null)
                         select new StockTransactionVM()
                         {
                             ID=ts.ID,
                             ItemName = ts.ItemSupplier.Item.Name,
                             NoItem =ts.NoItem,
                             StoreName = ts.Store.st_name,
                             SupName = ts.ItemSupplier.supplier.sup_name,
                             Weight = ts.Weight,
                             Total =ts.Weight*ts.NoItem,
                             FromStockId=ts.FromStockId,
                             Demondorderdetailsid=(int)ts.DemondOrderDetialsId,
                             Demonordname=ts.DemondOrderDetail.Shaplona.shap_name,
                             status=ts.DemondOrderDetail.status,
                             ItemSupID=ts.ItemSupplierId
                            
                             
                         }).ToList();

                //TO Colect Two Row in From Stock

                var Groupedlist = Model.GroupBy(ts => new
                {
                    ItemName = ts.ItemName,
                    FromStockId = ts.FromStockId,
                    Demondorderdetailsid = ts.Demondorderdetailsid,
                    Demonordname = ts.Demonordname,
                    status = ts.status,
                }).Select(ts => new StockTransactionVM
                {
                    ItemName = ts.Key.ItemName,
                    FromStockId = ts.Key.FromStockId,
                    Demondorderdetailsid = ts.Key.Demondorderdetailsid,
                    Demonordname = ts.Key.Demonordname,
                    status = ts.Key.status,
                    Weight = ts.Sum(x => x.Weight),
                    NoItem = ts.Sum(x => x.NoItem),
                    Total = ts.Sum(x => x.Total)

                }).ToList();

                Alert.type = true;
                Alert.data = Groupedlist;
            }
            catch
            {
                Model = null;
                Alert.type = false;
                Alert.data = Model;
            }

            return Alert;
        }


        public AlertVM<StockTransactionVM> GetStockTransactionByID(int? ID)
        {
            var Model = new StockTransactionVM();

            AlertVM<StockTransactionVM> Alert = new AlertVM<StockTransactionVM>();


            try
            {
                StockModel db = new StockModel();
                Model = (from ts in db.StockTransactions.Where(X => X.ID == ID)
                         select new StockTransactionVM()
                         {
                             ID = ts.ID,
                             ItemName = ts.ItemSupplier.Item.Name,
                             NoItem = ts.NoItem,
                             StoreName = ts.Store.st_name,
                             SupName = ts.ItemSupplier.supplier.sup_name,
                             Weight = ts.Weight,
                             Total = ts.Weight * ts.NoItem,
                             ItemSupID=ts.ItemSupplierId,
                             FromStockId = ts.FromStockId

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


        public double? TotalItem(int FromStockID)
        {
            var Model = new StockTransactionVM();

            double? total;
            double? TototalFinsh;
            double? Tototalreturn=null;

            try
            {
                StockModel db = new StockModel();
                int finishtostockID = db.ToStocks.Where(T => T.FromStockId == FromStockID && T.ToStockTypeId == (int)ToStockTypeId.FinishedItem).Select(o=>o.ID).FirstOrDefault();
                if (finishtostockID !=0)
                {
                    var resualt = db.StockTransactions.Where(s => s.ToStockId == finishtostockID).FirstOrDefault();
                    TototalFinsh = resualt.Weight * resualt.NoItem;
                }
                else
                {
                    TototalFinsh = 0;
                }

                int returntostockID = db.ToStocks.Where(T => T.FromStockId == FromStockID && T.ToStockTypeId == (int)ToStockTypeId.OperationReturns).Select(o => o.ID).FirstOrDefault();

                if (returntostockID !=0)
                {
                    Tototalreturn = db.StockTransactions.Where(s => s.ToStockId == returntostockID).Sum(o => o.NoItem * o.Weight);
                }
                if (Tototalreturn == null)
                {
                    Tototalreturn = 0;
                }
                else
                {
                }
                
                total = TototalFinsh + Tototalreturn;

            }
            catch
            {
                total = null;
            }

            return total;
        }
        public int? DemonndorderDetailsStatus(int DemondorderDetailsid)
        {
                StockModel db = new StockModel();
            return db.DemondOrderDetails.Find(DemondorderDetailsid).status;
        }
    }
}
