using StockDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService
{
   public class SafeTransactionService
    {
     private static   StockModel db = new StockModel();
        public static void EditBlanseInSafe(int safeid)
        {
            var safetransactions = db.SafeTransactions.Where(x => x.SafeID == safeid).ToList();
           var AvilableInSafe= safetransactions.Sum(x => x.OperationType == "+" ? x.TransactionValue : -x.TransactionValue);
            var LastDate = safetransactions.OrderByDescending(x => x.TransactionDate).First().TransactionDate;
            var safe =db.Safes.Find(safeid);
            safe.safe_totalBalance = Math.Round(AvilableInSafe,2);
            safe.safe_date = LastDate;
            db.Entry(safe).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

        }
    }
}
