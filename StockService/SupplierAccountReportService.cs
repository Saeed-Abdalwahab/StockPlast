using StockDB.Model;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService
{
    public class SupplierAccountReportService
    {
        StockModel db = new StockModel();

        public supplier supplier(int sup_id)
        {
            return db.suppliers.Find(sup_id);
        }
        public List<SupplierAccountReportVM> SupplierAccountReports(int sup_id)
        {
            double? Total = 0;
            try
            {
                var List = db.SupplierPayments.Where(x => x.Supplier_ID == sup_id).OrderBy(x => x.PaymentDate).ThenBy(x=>x.ID).Select(x => new SupplierAccountReportVM
                {
                    Date = x.PaymentDate,
                    Purchases = x.ToStockID != null ? (x.ToStock.Price)  : 0,
                    PurchasesReturn = x.FromstockID != null ? (x.FromStock.invoicecost) : 0,
                    Payment = (x.FromstockID == null && x.ToStockID == null) ? x.PaymentValue : 0,
                }).ToList();

                if (List != null)
                {
                    if (List[0].Purchases != 0)
                    {
                        List[0].Purchases = Math.Round((double)List[0].Purchases, 2);
                        Total = (List[0].Purchases);
                    }
                    else if (List[0].PurchasesReturn != 0)
                    {
                        List[0].PurchasesReturn = Math.Round((double)List[0].PurchasesReturn, 2);
                        Total = -(List[0].PurchasesReturn);
                    }
                    else
                    {
                        Total = -(List[0].Payment);
                    }

                    List[0].TotalBalance = Total;
                    for (int i = 1; i < List.Count; i++)
                    {
                        if (List[i].Purchases != 0)
                        {
                            Total = (List[i].Purchases);
                            List[i].Purchases = Math.Round((double)List[i].Purchases, 2);
                        }
                        else if (List[i].PurchasesReturn != 0)
                        {
                            Total = -(List[i].PurchasesReturn);
                            List[i].PurchasesReturn = Math.Round((double)List[i].PurchasesReturn, 2);
                        }
                        else
                        {
                            Total = -(List[i].Payment);
                        }
                        List[i].TotalBalance = Math.Round(Convert.ToDouble(Total + List[i - 1].TotalBalance),2);
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
