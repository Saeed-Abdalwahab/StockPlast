using StockDB.Model;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService
{
   public class CustomerAccountReportService
    {
        StockModel db = new StockModel(); 

        public  Customer customer (int cust_id)
        {
            return db.Customers.Find(cust_id);
        }
        public List<CustomerAccountReportVM> customerAccountReports(int Cust_ID)
        {
            double? Total = 0;
            try { 
           var List= db.CustomerPayments.Where(x => x.Customer_ID == Cust_ID).OrderBy(x => x.PaymentDate).ThenBy(x=>x.ID).Select(x => new CustomerAccountReportVM
            {
                Date = x.PaymentDate,
                Discount = x.FromstockID != null ? x.FromStock.sale_discount : 0,
                Sales = x.FromstockID != null ? (x.FromStock.invoicecost) + (x.FromStock.sale_discount) : 0,
                SalesReturn = x.ToStockID != null ? (x.ToStock.Price) : 0,
                Payment =(x.FromstockID==null&&x.ToStockID==null)? x.PaymentValue:0,
            }).ToList();
            if (List != null)
            {
                if (List[0].Sales != 0)
                {
                        List[0].Sales = Math.Round((double)List[0].Sales, 2);
                    Total = -(List[0].Sales - List[0].Discount);

                }
                else if (List[0].SalesReturn != 0)
                {
                        List[0].SalesReturn = Math.Round((double)List[0].SalesReturn, 2);
                    Total = List[0].SalesReturn;
                }
                else
                {
                        List[0].Payment = Math.Round((double)List[0].Payment, 2);
                    Total = List[0].Payment;
                        
                }

                List[0].TotalBalance = Total;
                for (int i = 1; i < List.Count; i++)
                {
                    if (List[i].Sales != 0)
                    {
                           
                        Total = -(List[i].Sales - List[i].Discount);
                            List[i].Sales = Math.Round((double)List[i].Sales, 2);
                    }
                    else if (List[i].SalesReturn != 0)
                    {
                        Total = List[i].SalesReturn;
                            List[i].SalesReturn = Math.Round((double)List[i].SalesReturn, 2);

                        }
                        else
                    {
                        Total = List[i].Payment;
                    }
                    List[i].TotalBalance = Math.Round((double)(Total + List[i - 1].TotalBalance),2);

                }
            }
            return List;
            }
            catch
            {
                return null;
            }



        }
    }
}
