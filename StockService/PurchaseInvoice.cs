using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockDB.Model;
using StockViewModel;
using PlasticStatic;
using static PlasticStatic.Enums;
using System.Data.Entity;
using System.Data.SqlClient;

namespace StockService
{
    public class PurchaseInvoiceService
    {
        private StockModel db = new StockModel();

        #region Get All Ivoce item
        public List<PurchaseInvoiceViewModel> PurchaseInvoiceList()
        {
            var Model = new List<PurchaseInvoiceViewModel>();


            try
            {
                Model = db.ToStocks.Where(x => x.ToStockTypeId == (int)ToStockTypeId.PurchaseInvoice).Select(ts => new PurchaseInvoiceViewModel
                {
                    ID = ts.ID,
                    InvoiceSerial = ts.InvoiceSerial,
                    InvoiceDate = ts.InvoiceDate,
                    Notes = ts.Notes,
                    SupId = ts.SupId,
                    SupName = ts.supplier.sup_name,
                    StoreName = db.StockTransactions.FirstOrDefault(x => x.ToStockId == ts.ID).Store.st_name,
                    InvoiceStatus = ts.InvoiceStatus
                }).OrderBy(x => x.InvoiceStatus).ToList();
            }
            catch
            {
                Model = null;
            }

            return Model;
        }

        #endregion

        #region Get All Items
        public AlertVM<ItemViewModel> GetAllItems()
        {
            var Model = new List<ItemViewModel>();
            AlertVM<ItemViewModel> Alert = new AlertVM<ItemViewModel>();


            try
            {
                db = new StockModel();

                Model = (from ts in db.Items
                         select new ItemViewModel()
                         {

                             ID = ts.ID,
                             Name = ts.Name

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


        public ItemViewModel GetAllItemsByID(int ID)
        {
            var Model = new ItemViewModel();


            try
            {
                db = new StockModel();

                Model = (from ts in db.Items.Where(O => O.ID == ID)
                         select new ItemViewModel()
                         {

                             ID = ts.ID,
                             Name = ts.Name

                         }).FirstOrDefault();
            }
            catch
            {
                Model = null;
            }

            return Model;
        }
        #endregion

        #region Get Purchase Invoice By ID

        public PurchaseInvoiceViewModel GetPurchaseInvoiceByID(int id)
        {
            PurchaseInvoiceViewModel Model = new PurchaseInvoiceViewModel();

            string StoreName = GetStoreName(id);
            int StoreID = GetStoreID(id);
            try
            {
                db = new StockModel();

                Model = (from pr in db.ToStocks.Where(o => o.ID == id)
                         select new PurchaseInvoiceViewModel()
                         {
                             ID = pr.ID,
                             InvoiceDate = pr.InvoiceDate,
                             InvoiceSerial = pr.InvoiceSerial,
                             Notes = pr.Notes,
                             SupId = pr.SupId,
                             SupName = pr.supplier.sup_name,
                             InvoiceStatus = pr.InvoiceStatus,
                             InvoiceStatusName = pr.InvoiceStatus == (int)InvoiceStatus.Finish ? "تم الانتهاء" : "تحت الانشاء",
                             StoreName = StoreName,



                         }).FirstOrDefault();
                Model.StoreId = StoreID;
            }
            catch (Exception)
            {

                Model = null;
            }
            return Model;
        }


        private string GetStoreName(int StockID)
        {
            string StoreName;
            try
            {
                db = new StockModel();
                StoreName = db.StockTransactions.Where(o => o.ToStockId == StockID).Select(s => s.Store.st_name).FirstOrDefault();

            }
            catch (Exception)
            {

                StoreName = "";
            }
            return StoreName;
        }
        private int GetStoreID(int StockID)
        {
            int StoreID;
            try
            {
                db = new StockModel();
                StoreID = db.StockTransactions.Where(o => o.ToStockId == StockID).Select(s => s.Store.st_id).FirstOrDefault();

            }
            catch (Exception)
            {

                StoreID = 0;
            }
            return StoreID;
        }



        #endregion

        #region Add Ivoce 
        public bool AddPurchaseInvoice(PurchaseInvoiceViewModel vm, List<PurchaseInvoiceViewModel> insert, List<PurchaseInvoiceViewModel> Modified, List<PurchaseInvoiceViewModel> Deleted)
        {

            ToStock tostock = new ToStock();
            try
            {
                StockModel db = new StockModel();


                tostock.InvoiceSerial = vm.InvoiceSerial;
                tostock.InvoiceDate = vm.InvoiceDate;
                tostock.SupId = vm.SupId;
                tostock.Notes = vm.Notes;
                tostock.ID = vm.ID;
                tostock.InvoiceStatus = vm.InvoiceStatus;
                tostock.ToStockTypeId = (int?)ToStockTypeId.PurchaseInvoice;
                tostock.Price = vm.totalPurchesiescost;



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
                    int? OldSupID = itemlist.Select(o => o.ItemSupplier.SupId).FirstOrDefault();
                    if (OldSupID != vm.SupId)
                    {
                        foreach (var item in itemlist)
                        {
                            int? ItemID = db.ItemSuppliers.Find(item.ItemSupplierId).ItemId;
                            int NewItemSupID = AddItemSupplier(vm.SupId, ItemID);
                            item.ItemSupplierId = NewItemSupID;
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();

                        }
                    }


                    db.Entry(tostock).State = EntityState.Modified;
                    db.SaveChanges();
                    foreach (var item in itemlist)
                    {
                        updatePriceInItemTbl((int)item.ItemSupplier.ItemId);

                    }


                }

                if (tostock.InvoiceStatus == (int)InvoiceStatus.Finish)
                {
                    SupplierPaymentsService.SaveReMotly(tostock);
                }


                if (AddItem(tostock.ID, vm.StoreId, vm.SupId, insert, Modified, Deleted) == false)
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
        #endregion

        #region Add Item
        public bool AddItem(int ToStockID, int? StoreID, int? SupID, List<PurchaseInvoiceViewModel> insert, List<PurchaseInvoiceViewModel> Modified, List<PurchaseInvoiceViewModel> Deleted)
        {
            try
            {

                var dbb = new StockModel();

                //var lastinvoice = db.ToStocks.Where(x => x.SupId == SupID).OrderByDescending(x => x.InvoiceDate).FirstOrDefault();
                //var items = db.ItemSuppliers.Where(x => x.SupId == lastinvoice.SupId).Select(x => x.Item);
                //foreach (var i in items)
                //{
                //    var editeditem = db.Items.Find(i.ID);
                //    editeditem.PurchasingPrice = insert.FirstOrDefault(x => x.ItemID == editeditem.ID).itemprice;

                //    db.Entry(editeditem).State = EntityState.Modified;
                //    db.SaveChanges();

                //}
                foreach (var item in insert)
                {

                    StockTransaction stoctr = new StockTransaction();
                    stoctr.ToStockId = ToStockID;
                    stoctr.StoreId = StoreID;


                    int ItemSupID = AddItemSupplier(SupID, item.ItemID);

                    if (ItemSupID == 0)
                    {

                        return false;

                    }
                    else
                    {
                        stoctr.ItemSupplierId = ItemSupID;
                    }
                    stoctr.NoItem = item.NoItem;
                    stoctr.Weight = item.Weight;
                    stoctr.Quantity = item.NoItem * item.Weight;
                    stoctr.OperationType = "+";
                    stoctr.ItemPrice = Math.Round((double)item.itemprice,2);



                    dbb.StockTransactions.Add(stoctr);
                    dbb.SaveChanges();
                    updatePriceInItemTbl((int)item.ItemID);


                }
                foreach (var item in Modified)
                {
                    StockTransaction stoctredit = dbb.StockTransactions.Find(item.ID);
                    stoctredit.StoreId = StoreID;
                    stoctredit.ToStockId = ToStockID;
                    int ItemSupID = AddItemSupplier(SupID, item.ItemID);
                    if (ItemSupID == 0)
                    {
                        return false;
                    }
                    else
                    {
                        stoctredit.ItemSupplierId = ItemSupID;
                    }
                    stoctredit.NoItem = item.NoItem;
                    stoctredit.Weight = item.Weight;
                    stoctredit.Quantity = item.NoItem * item.Weight;

                    stoctredit.OperationType = "+";

                    stoctredit.ItemPrice = Math.Round((double)item.itemprice, 2);


                    dbb.Entry(stoctredit).State = EntityState.Modified;
                    dbb.SaveChanges();
                    updatePriceInItemTbl((int)item.ItemID);


                }
                foreach (var item in Deleted)
                {
                    StockTransaction stoctr = dbb.StockTransactions.Find(item.ID);
                    if (stoctr == null)
                        continue;

                    dbb.StockTransactions.Remove(stoctr);
                    dbb.SaveChanges();
                    updatePriceInItemTbl((int)item.ItemID);

                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        private int AddItemSupplier(int? SupID, int? ItemID)
        {
            int ID;

            try
            {
                db = new StockModel();
                ItemSupplier itemsup = new ItemSupplier();

                if (db.ItemSuppliers.Any(o => o.ItemId == ItemID && o.SupId == SupID))
                {
                    ID = (db.ItemSuppliers.Where(o => o.ItemId == ItemID && o.SupId == SupID).Select(o => o.ID).FirstOrDefault());
                }
                else
                {
                    itemsup.ItemId = ItemID;
                    itemsup.SupId = SupID;

                    db.ItemSuppliers.Add(itemsup);
                    db.SaveChanges();

                    ID = itemsup.ID;
                }
            }
            catch (Exception)
            {

                ID = 0;
            }

            return ID;
        }
        #endregion

        #region Edit Ivoce
        public List<PurchaseInvoiceViewModel> GetItemByInvoiceID(int ID)
        {
            var Model = new List<PurchaseInvoiceViewModel>();


            try
            {
                db = new StockModel();
                Model = (from ts in db.StockTransactions.Where(o => o.ToStockId == ID && o.ToStock.InvoiceStatus == 1)
                         select new PurchaseInvoiceViewModel()
                         {
                             ItemName = ts.ItemSupplier.Item.Name,
                             ItemID = ts.ItemSupplier.ItemId,
                             StoreId = ts.StoreId,
                             NoItem = ts.NoItem,
                             Weight = ts.Weight,
                             ID = ts.ID,
                             TransactionID = ts.ID,
                             itemprice = Math.Round((double)ts.ItemPrice,2),
                             StoreName = ts.Store.st_name,


                         }).ToList();
            }
            catch
            {
                Model = null;
            }

            return Model;
        }





        public List<PurchaseInvoiceViewModel> GetItemDetailsInvoiceID(int ID)
        {
            var Model = new List<PurchaseInvoiceViewModel>();


            try
            {
                StockModel db = new StockModel();
                Model = (from ts in db.StockTransactions.Where(o => o.ToStockId == ID)
                         select new PurchaseInvoiceViewModel()
                         {
                             ItemName = ts.ItemSupplier.Item.Name,
                             StoreId = ts.StoreId,
                             NoItem = ts.NoItem,
                             Weight = ts.Weight,
                             ID = ts.ID,
                             TransactionID = ts.ID,
                             StoreName = ts.Store.st_name,
                             itemprice = ts.ItemPrice

                         }).ToList();
            }
            catch
            {
                Model = null;
            }

            return Model;
        }
        #endregion

        #region Delete Ivoce.
        //De
        public bool DeletFinishinvoice(int ID)
        {
            try
            {
                //double? sum = 0;

                StockModel db = new StockModel();
                ToStock st = db.ToStocks.Find(ID);

                if (st == null) return true;
                var Demondorderdetailsid = st.DemondOrderDetialsId;
                List<StockTransaction> lst = db.StockTransactions.Where(o => o.ToStockId == st.ID && o.Store.StoreType_ID == (int)PlasticStatic.Enums.StoreType.finished).ToList();
                db.StockTransactions.RemoveRange(lst);
                db.ToStocks.Remove(st);
                db.SaveChanges();
                var Check = db.ToStocks.FirstOrDefault(x => x.DemondOrderDetialsId == Demondorderdetailsid && x.InvoiceStatus == (int)InvoiceStatus.NotFinish);
                if (Check == null)
                {
                    var demondorderdetails = db.DemondOrderDetails.Find(Demondorderdetailsid);
                    demondorderdetails.status = (int)demondorderdetailstatus.Underconstruction;
                    db.Entry(demondorderdetails).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }



        }

        public bool DeletePurchaseInvoice(int ID)
        {
            try
            {
                //double? sum = 0;

                StockModel db = new StockModel();
                ToStock st = db.ToStocks.Find(ID);
                var Demondorderdetailsid = st.DemondOrderDetialsId;
                if (st == null) return true;
                List<StockTransaction> lst = db.StockTransactions.Where(o => o.ToStockId == st.ID).ToList();
                db.StockTransactions.RemoveRange(lst);
                db.ToStocks.Remove(st);
                db.SaveChanges();
               var Check= db.ToStocks.FirstOrDefault(x => x.DemondOrderDetialsId == Demondorderdetailsid && x.InvoiceStatus == (int)InvoiceStatus.NotFinish);
                if (Check == null)
                {
                    var demondorderdetails = db.DemondOrderDetails.Find(Demondorderdetailsid);
                    demondorderdetails.status = (int)demondorderdetailstatus.Underconstruction;
                    db.Entry(demondorderdetails).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }



        }

        public double? Total(int ID)
        {
            double? sum = 0;

            try
            {

                StockModel db = new StockModel();
                ToStock st = db.ToStocks.Find(ID);
                List<StockTransaction> lst = db.StockTransactions.Where(o => o.ToStockId == st.ID).ToList();
                sum = lst.Sum(x => x.NoItem * x.Weight);
                return sum;
            }
            catch (Exception)
            {

                return sum = -1;
            }



        }
        #endregion

        #region Get Raw All Store
        public AlertVM<StoreViewModel> GetRawAllStore()
        {

            AlertVM<StoreViewModel> Alert = new AlertVM<StoreViewModel>();
            var Model = new List<StoreViewModel>();


            try
            {
                StockModel db = new StockModel();

                Model = (from ts in db.Stores.Where(o => o.StoreType_ID == 1)
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


        public StoreViewModel GetRawAllStoreByID(int ID)
        {

            var Model = new StoreViewModel();


            try
            {
                StockModel db = new StockModel();

                Model = (from ts in db.Stores.Where(o => o.StoreType_ID == 1 && o.st_id == ID)
                         select new StoreViewModel()
                         {
                             st_id = ts.st_id,
                             st_name = ts.st_name


                         }).FirstOrDefault();
            }
            catch
            {
                Model = null;
            }

            return Model;

        }
        #endregion

        #region Get Unit Type
      

        #endregion

        //function To Set Max Serial Of Purchesies
        public string GetMaxSerial()
        {
            var tostock = db.ToStocks.OrderByDescending(x => x.ID).FirstOrDefault();
            return (tostock != null ? (tostock.ID + 1) : 1).ToString();
        }

        public IEnumerable<Item> GetallItems()
        {
            return db.Items;
        }
        public void updatePriceInItemTbl(int itemId)
        {
            StockDB.Model.StockModel stockDB = new StockModel();
            var context = new StockModel();
            //var item = new SqlParameter("@Item", itemId);
            //var lastPrice = context.StockTransactions.Where(x => x.ItemSupplier.ItemId == itemId && x.ToStock.ToStockTypeId == (int)ToStockTypeId.PurchaseInvoice && x.ToStock.InvoiceStatus == (int)InvoiceStatus.Finish).OrderByDescending(x => x.ToStock.InvoiceDate).ThenByDescending(x => x.ToStock.ID).FirstOrDefault().ItemPrice;
            List<StockTransaction> stockTransactionList = context.StockTransactions.Where(x =>
       x.ItemSupplier.ItemId == itemId &&
       x.ToStock.ToStockTypeId == (int)ToStockTypeId.PurchaseInvoice &&
       x.ToStock.InvoiceStatus == (int)InvoiceStatus.Finish).OrderByDescending(x => x.ToStock.InvoiceDate).ThenByDescending(x => x.ToStock.ID).ToList();
            double? lastPrice = null;
            if (stockTransactionList.Count > 0)
            {
                lastPrice = stockTransactionList.FirstOrDefault().ItemPrice;
            }
            //var itemprice = context.Database.SqlQuery<itemprice>("getLastPriceForItem @Item", item).ToList();
            var itemedit = context.Items.Find(itemId);
            itemedit.PurchasingPrice = lastPrice;
            context.Entry(itemedit).State = EntityState.Modified;
            context.SaveChanges();
            context.Dispose();

        }
        public class itemprice
        {
            public double ItemPrice { get; set; }
        }

    }
}
