using Microsoft.Win32.SafeHandles;
using StockDB.Model;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService
{
  public  class SafeReportService
    {
        StockModel db = new StockModel();

        public List<SafeReportVM> safeReportVMs(int safeid)
        {
            var list = db.SafeTransactions.Where(x => x.SafeID == safeid).Select(x => new SafeReportVM
            {
                ID = x.ID,
                Date = x.TransactionDate.ToString(),
                SafeName = x.Safe.safe_name,
                MoneyValue = x.TransactionValue,
                ActualyValue = x.OperationType == "+" ? x.TransactionValue : -x.TransactionValue,
                OperationType = x.OperationType == "+" ? "ايداع" : "سحب",
                OperationFor = (x.DemondorderID != null) ? "امر شغل" : (x.CustomerPaymentID != null) ? "دفعه عميل" : (x.ExpenseDetailsID != null) ? "مصروفات يوميه" : (x.IncomeDetailsID != null) ? "ايرادات يوميه" : (x.SupplierPaymentID != null) ? "دفعات مورد" : "رصيد افتتاحي"

            }).OrderBy(x => x.Date).ToList();
            foreach (var item in list)
            {
                item.ActualyValue = Math.Round(item.ActualyValue,2);
                item.MoneyValue = Math.Round(item.MoneyValue,2);

            }
            return list;

            
        }
    }
}
