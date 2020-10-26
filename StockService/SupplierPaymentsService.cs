using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockViewModel;
using StockDB.Model;
using static PlasticStatic.Enums;
using static PlasticStatic.Extensions;

namespace StockService
{
  public  class SupplierPaymentsService
    {
        private StockModel db = new StockModel();
        public List<supplier> Suppliers()
        {
            return db.suppliers.ToList();
        }
        public List<PaymentType> paymenttypss()
        {
            return db.PaymentTypes.ToList();
        }
        public List<Safe> Safes()
        {
            return db.Safes.ToList();
        }
        public List<Bank> banks()
        {
            return db.Banks.ToList();
        }
        public List<SupplierPaymentVM> GetAll()
        {
            var List = db.SupplierPayments.Where(x => x.FromstockID == null && x.ToStockID == null).Select(x => new SupplierPaymentVM
            {
                ID = x.ID,
                SupplierName = x.Supplier.sup_name,
                Notes = x.Notes,
                PaymentDate = (DateTime)x.PaymentDate,
                PaymentValue = x.PaymentValue,
                SafeName = x.Safe.safe_name,
                PaymentNumber = x.PaymentNumber
            }).ToList();
         
            return List;
        }
        public SupplierPaymentVM GetByID(int id)
        {
            var custpayment = db.SupplierPayments.Find(id);
            SupplierPaymentVM SupplierPaymentVM = new SupplierPaymentVM();
            SupplierPaymentVM.ID = custpayment.ID;
            SupplierPaymentVM.Supplier_ID = (int)custpayment.Supplier_ID;
            SupplierPaymentVM.PaymentNumber = custpayment.PaymentNumber;
            SupplierPaymentVM.PaymentDate = (DateTime)custpayment.PaymentDate;
            SupplierPaymentVM.PaymentValue = custpayment.PaymentValue;
            SupplierPaymentVM.Safe_ID = (int)custpayment.Safe_ID;
            SupplierPaymentVM.Notes = custpayment.Notes;
            return SupplierPaymentVM;
        }
        public bool SaveCreate(SupplierPaymentVM SupplierPaymentVM)
        {
            try
            {
                SupplierPayments SupplierPayment = new SupplierPayments();
                SupplierPayment.Supplier_ID = SupplierPaymentVM.Supplier_ID;
                SupplierPayment.PaymentNumber = SupplierPaymentVM.PaymentNumber;
                SupplierPayment.PaymentDate = SupplierPaymentVM.PaymentDate;
                SupplierPayment.PaymentValue = SupplierPaymentVM.PaymentValue;
                SupplierPayment.Safe_ID = SupplierPaymentVM.Safe_ID;
                SupplierPayment.Notes = SupplierPaymentVM.Notes;
                SupplierPayment.OperationTyp = "-";
                db.SupplierPayments.Add(SupplierPayment);
                db.SaveChanges();

                SafeTransaction safeTransaction = new SafeTransaction();
                safeTransaction.TransactionValue = (double)SupplierPaymentVM.PaymentValue;
                safeTransaction.TransactionDate = (DateTime)SupplierPayment.PaymentDate;
                safeTransaction.SupplierPaymentID = SupplierPayment.ID;
                safeTransaction.OperationType = "-";
                safeTransaction.SafeID = SupplierPayment.Safe_ID;
                db.SafeTransactions.Add(safeTransaction);
                db.SaveChanges();
                SafeTransactionService.EditBlanseInSafe((int)SupplierPaymentVM.Safe_ID);

                return true;
            }
            catch
            {
                return false;
            }


        }
        public bool SaveEdit(SupplierPaymentVM SupplierPaymentVM)
        {
            try
            {
                SupplierPayments SupplierPayment = db.SupplierPayments.Find(SupplierPaymentVM.ID);
               
                SupplierPayment.Supplier_ID = SupplierPaymentVM.Supplier_ID;
                SupplierPayment.PaymentNumber = SupplierPaymentVM.PaymentNumber;
                SupplierPayment.PaymentDate = SupplierPaymentVM.PaymentDate;
                SupplierPayment.PaymentValue = SupplierPaymentVM.PaymentValue;
                SupplierPayment.Safe_ID = SupplierPaymentVM.Safe_ID;
                SupplierPayment.Notes = SupplierPaymentVM.Notes;
                SupplierPayment.OperationTyp = "-";
                db.Entry(SupplierPayment).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var safeTransaction = db.SafeTransactions.FirstOrDefault(x => x.SupplierPaymentID == SupplierPayment.ID);
                if (safeTransaction != null)
                {
                    safeTransaction.TransactionValue = (double)SupplierPaymentVM.PaymentValue;
                    safeTransaction.TransactionDate = (DateTime)SupplierPaymentVM.PaymentDate;
                    safeTransaction.SafeID = SupplierPayment.Safe_ID;
                    db.Entry(safeTransaction).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    SafeTransaction safeTransactionN = new SafeTransaction();
                    safeTransactionN.TransactionValue = (double)SupplierPaymentVM.PaymentValue;
                    safeTransactionN.TransactionDate = (DateTime)SupplierPayment.PaymentDate;
                    safeTransactionN.SupplierPaymentID = SupplierPayment.ID;
                    safeTransactionN.OperationType = "-";
                    safeTransactionN.SafeID = SupplierPayment.Safe_ID;
                    db.SafeTransactions.Add(safeTransaction);
                    db.SaveChanges();
                }
                SafeTransactionService.EditBlanseInSafe((int)SupplierPaymentVM.Safe_ID);

                return true;
            }
            catch
            {
                return false;
            }


        }

        public bool Delete(int id)
        {
            try
            {
                var safetransaction = db.SafeTransactions.FirstOrDefault(x => x.SupplierPaymentID == id);
                var CustPayments = db.SupplierPayments.Find(id);
                var SafeId = CustPayments.Safe_ID;
                if (safetransaction != null)
                {

                    db.SafeTransactions.Remove(safetransaction);
                    db.SaveChanges();
                }
                db.SupplierPayments.Remove(CustPayments);
                db.SaveChanges();
                SafeTransactionService.EditBlanseInSafe((int)SafeId);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool SaveReMotly(FromStock fromStock)
        {
            try
            {
                if (fromStock.FromStockTypeID != (int)FromStockTypeId.purchesesreturn) return false;
                StockModel DB = new StockModel();
                SupplierPayments SupplierPayment = new SupplierPayments();
                SupplierPayment.PaymentDate = fromStock.TransDate;
                SupplierPayment.FromstockID = fromStock.ID;
                SupplierPayment.PaymentValue = fromStock.invoicecost;
                SupplierPayment.PaymentNumber = fromStock.Serial;
                SupplierPayment.OperationTyp = "-";
                SupplierPayment.Supplier_ID = DB.StockTransactions.Find(fromStock.StockTransactions.FirstOrDefault().ID).ItemSupplier.SupId;
                DB.SupplierPayments.Add(SupplierPayment);
                DB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool SaveReMotly(ToStock toStock)
        {
            try
            {
                if (toStock.ToStockTypeId != (int)ToStockTypeId.PurchaseInvoice) return false;
                StockModel DB = new StockModel();

                SupplierPayments SupplierPayment = new SupplierPayments();
                SupplierPayment.Supplier_ID = toStock.SupId;
                SupplierPayment.PaymentDate = toStock.InvoiceDate;
                SupplierPayment.ToStockID = toStock.ID;
                SupplierPayment.PaymentValue = toStock.Price;
                SupplierPayment.PaymentNumber = toStock.InvoiceSerial;
                SupplierPayment.OperationTyp = "+";
                DB.SupplierPayments.Add(SupplierPayment);
                DB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public double GetSafeBlance(int SafeID)
        {
            var t= db.SafeTransactions.Where(x => x.SafeID == SafeID).Sum(x => x.OperationType == "+" ? x.TransactionValue : -x.TransactionValue);
            return db.SafeTransactions.Where(x => x.SafeID == SafeID).Sum(x => x.OperationType == "+" ? x.TransactionValue : -x.TransactionValue);
        }
        public double GetSafeBlance(int SafeID,int PaymentID)
       
        {
            var t = db.SafeTransactions.Where(x => x.SafeID == SafeID && x.SupplierPaymentID != PaymentID).Sum(x => x.OperationType == "+" ? x.TransactionValue : -x.TransactionValue);

            return db.SafeTransactions.Where(x => x.SafeID == SafeID&&x.SupplierPaymentID!=PaymentID).Sum(x => x.OperationType == "+" ? x.TransactionValue : -x.TransactionValue);
        }
        public int GetMaxSerial()
        {
            var TT = db.SupplierPayments.OrderByDescending(x => x.ID).FirstOrDefault();
            return TT != null ? TT.ID + 1 : 1;
        }
    }
}
