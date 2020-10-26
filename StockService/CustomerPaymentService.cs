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
  public  class CustomerPaymentService
    {
        private StockModel db = new StockModel();
        public List<Customer> Customers()
        {
            return db.Customers.ToList();
        }
        public List<PaymentType> paymenttypss()
        {
            return db.PaymentTypes.ToList();
        }
        public List<Safe> Safes()
        {
            return db.Safes.ToList();
        }  public List<Bank> banks()
        {
            return db.Banks.ToList();
        }
        public List<CustomerPaymentVM> GetAll()
        {
        var List=    db.CustomerPayments.Where(x=>x.FromstockID==null&&x.ToStockID==null). Select(x => new CustomerPaymentVM
            {
                ID = x.ID,
                BankName = x.Bank.Name,

                CustomerName = x.Customer.cust_name,
                Notes = x.Notes,
                PaymentDate = (DateTime)x.PaymentDate,
                PaymentTypName = x.PaymentType.Name,
                PaymentValue = x.PaymentValue,
                SafeName = x.Safe.safe_name,
            PaymentNumber=x.PaymentNumber
        }).ToList();
          
            return List;
        }
        public CustomerPaymentVM GetByID(int id)
        {
            var custpayment=db.CustomerPayments.Find(id);
            CustomerPaymentVM customerPaymentVM = new CustomerPaymentVM();
            customerPaymentVM.ID = custpayment.ID;
            customerPaymentVM.Customer_ID = (int)custpayment.Customer_ID;

            customerPaymentVM.PaymentNumber = custpayment.PaymentNumber;
            customerPaymentVM.PaymentDate = (DateTime)custpayment.PaymentDate;
            customerPaymentVM.PaymentValue = custpayment.PaymentValue;

            customerPaymentVM.Safe_ID = (int)custpayment.Safe_ID;
            customerPaymentVM.Notes = custpayment.Notes;
 
            return customerPaymentVM;
        }
        public string SaveCreate(CustomerPaymentVM customerPaymentVM)
        {
            try
            {
                if (customerPaymentVM.PaymentValue < 0)
                {
                    var SAfeBlace = Convert.ToDouble(db.Safes.Find(customerPaymentVM.Safe_ID).safe_totalBalance);
                    if (SAfeBlace + customerPaymentVM.PaymentValue < 0)
                    {
                        return "رصيد الخزنه لا يسمح لاتمام العمليه";
                    }
                }
                CustomerPayment customerPayment = new CustomerPayment();
                customerPayment.Bank_ID = customerPaymentVM.Bank_ID;
                customerPayment.Customer_ID = customerPaymentVM.Customer_ID;
                customerPayment.PaymentTyp_ID = customerPaymentVM.PaymentTyp_ID;
                customerPayment.PaymentNumber = customerPaymentVM.PaymentNumber;
                customerPayment.PaymentDate = customerPaymentVM.PaymentDate;
                customerPayment.PaymentValue = Math.Round(Convert.ToDouble(customerPaymentVM.PaymentValue),2);
                customerPayment.Safe_ID = customerPaymentVM.Safe_ID;
                customerPayment.Notes = customerPaymentVM.Notes;
                customerPayment.OperationTyp = "+";
                db.CustomerPayments.Add(customerPayment);
                db.SaveChanges();
                SafeTransaction safeTransaction = new SafeTransaction();
                customerPayment.PaymentValue = Math.Round(Convert.ToDouble(customerPaymentVM.PaymentValue), 2);
                safeTransaction.TransactionDate = (DateTime)customerPayment.PaymentDate;
                safeTransaction.CustomerPaymentID = customerPayment.ID;
                safeTransaction.OperationType = "+";
                safeTransaction.SafeID = customerPayment.Safe_ID;
                db.SafeTransactions.Add(safeTransaction);
                db.SaveChanges();

                SafeTransactionService.EditBlanseInSafe((int)customerPayment.Safe_ID);

                return "true";
            }
            catch
            {
                return "false";
            }


        } public bool SaveEdit(CustomerPaymentVM customerPaymentVM)
        {
            try
            {

                CustomerPayment customerPayment = db.CustomerPayments.Find(customerPaymentVM.ID);
      
                customerPayment.Customer_ID = customerPaymentVM.Customer_ID;
                customerPayment.PaymentNumber = customerPaymentVM.PaymentNumber;
                customerPayment.PaymentDate = customerPaymentVM.PaymentDate;
                customerPayment.PaymentValue = Math.Round(Convert.ToDouble(customerPaymentVM.PaymentValue),2);
                customerPayment.Safe_ID = customerPaymentVM.Safe_ID;
                customerPayment.Notes = customerPaymentVM.Notes;
            
                db.Entry(customerPayment).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var safeTransaction= db.SafeTransactions.FirstOrDefault(x => x.CustomerPaymentID == customerPayment.ID);
                if (safeTransaction != null)
                {
                    safeTransaction.TransactionValue = Math.Round(Convert.ToDouble(customerPayment.PaymentValue), 2);
                    safeTransaction.TransactionDate = (DateTime)customerPayment.PaymentDate;
                    safeTransaction.SafeID = customerPayment.Safe_ID;
                    db.Entry(safeTransaction).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    SafeTransaction safeTransactionN = new SafeTransaction();
                    safeTransactionN.TransactionValue = Math.Round(Convert.ToDouble( customerPaymentVM.PaymentValue),2);
                    safeTransactionN.TransactionDate = (DateTime)customerPayment.PaymentDate;
                    safeTransactionN.CustomerPaymentID = customerPayment.ID;
                    safeTransactionN.OperationType = "+";
                    safeTransactionN.SafeID = customerPayment.Safe_ID;
                    db.SafeTransactions.Add(safeTransactionN);
                    db.SaveChanges();
                }
                SafeTransactionService.EditBlanseInSafe((int)customerPayment.Safe_ID);


                return true;
            }
            catch
            {
                return false;
            }


        }

        public string Delete(int id)
        {
            try
            {
                var safetransaction = db.SafeTransactions.FirstOrDefault(x => x.CustomerPaymentID == id);
                var CustPayments= db.CustomerPayments.Find(id);
                var SAfeID = CustPayments.Safe_ID;
                if (SafeBlanceCheck((int)CustPayments.Safe_ID, CustPayments.ID) < 0)
                {
                    return "رصيد الخزينه لا يسمح بعمليه الحذف ";
                }
                if (safetransaction != null)
                {

                    db.SafeTransactions.Remove(safetransaction);
                    db.SaveChanges();
                }
                db.CustomerPayments.Remove(CustPayments);
                db.SaveChanges();
                SafeTransactionService.EditBlanseInSafe((int)SAfeID);

                return "true";
            
            }
            catch
            {
                return "حدث خطأ اثناء الحذف تاكد من البيانات المعتمده علي عمليه الحذف";
            }
        }
        public static bool SaveReMotly(FromStock fromStock)
        {
            try
            {
                if (fromStock.FromStockTypeID != (int)FromStockTypeId.sales) return false;
                  StockModel DB = new StockModel();
                CustomerPayment customerPayment = DB.CustomerPayments.FirstOrDefault(x => x.FromStock.FromStockTypeID == (int)FromStockTypeId.sales && x.FromstockID == fromStock.ID);
                if (customerPayment == null) {
                    customerPayment = new CustomerPayment();
                customerPayment.PaymentDate = fromStock.TransDate;
                customerPayment.FromstockID = fromStock.ID;
                customerPayment.PaymentValue = fromStock.invoicecost;
                customerPayment.PaymentNumber = fromStock.Serial;
                customerPayment.OperationTyp = "-";
                customerPayment.Customer_ID = fromStock.DemondOrder.cust_id;
                DB.CustomerPayments.Add(customerPayment);
                DB.SaveChanges();
                }
                else
                {
                       customerPayment.FromstockID = fromStock.ID;
                customerPayment.PaymentValue = fromStock.invoicecost;
                customerPayment.PaymentNumber = fromStock.Serial;
                customerPayment.OperationTyp = "-";
                customerPayment.Customer_ID = fromStock.DemondOrder.cust_id;
                    DB.Entry(customerPayment).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();
                }
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
                if (toStock.ToStockTypeId != (int)ToStockTypeId.SalesReturns) return false;
                StockModel DB = new StockModel();

                CustomerPayment customerPayment = new CustomerPayment();
                customerPayment.PaymentDate = toStock.InvoiceDate;
                customerPayment.ToStockID = toStock.ID;
                customerPayment.PaymentValue = toStock.Price;
                customerPayment.PaymentNumber = toStock.InvoiceSerial;
                customerPayment.OperationTyp = "+";
                customerPayment.Customer_ID = toStock.StockTransactions.FirstOrDefault().DemondOrderDetail.DemondOrder.cust_id;
                DB.CustomerPayments.Add(customerPayment);
                DB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public double SafeBlanceCheck(int SafeID,int customerPaymentID)
        {
            var Val = db.SafeTransactions.Where(x => x.SafeID == SafeID && x.CustomerPaymentID != customerPaymentID).Sum(x => x.OperationType == "+" ? x.TransactionValue : -x.TransactionValue);
            return Val;
        }
        public int GetMaxSerial()
        {
          var TT=  db.CustomerPayments.OrderByDescending(x => x.ID).FirstOrDefault();
            return TT != null ? TT.ID + 1 : 1;
        }
    }
}
