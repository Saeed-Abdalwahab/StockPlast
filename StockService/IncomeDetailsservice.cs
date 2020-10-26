using PlasticStatic;
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
    public class IncomeDetailsservice
    {
        StockModel db = new StockModel();

        #region Get All Expenses
        public List<IncomeDetailsVM> Getall()
        {
            IncomeDetailsStatus incomeDetailsStatus = new IncomeDetailsStatus();
            var d = incomeDetailsStatus.GetDisplayName();
            var Model = new List<IncomeDetailsVM>();
            try
            {
                Model = db.IncomeDetails.Select(x => new IncomeDetailsVM()
                {



                    incDet_id = x.incDet_id,
                    incDet_Date = x.incDet_Date,
                    safe_name = x.Safe.safe_name,
                    incomename = x.Income.inc_name,
                    incDet_value= x.incDet_value,
                    incDet_notes = x.incDet_notes,
                    IncDet_status = (IncomeDetailsStatus)x.incDet_status
                }).ToList();

                foreach (var item in Model)
                {
                    item.IncDet_statusName = (item.IncDet_status).GetDisplayName();
                }

            }
            catch
            {
                Model = null;
            }

            return Model;
        }

        #endregion

        #region Save In Data base
        public string SaveinDatabase(IncomeDetailsVM incomeDetailsVM)
        {
            var result = "false";
            try
            {
                if (incomeDetailsVM.incDet_id> 0)
                {
                    IncomeDetail incomeDetail = db.IncomeDetails.FirstOrDefault(x => x.incDet_id == incomeDetailsVM.incDet_id);
                    if (incomeDetail.incDet_status != (int)IncomeDetailsStatus.Finish)
                    {
                        incomeDetail.Income_inc_id = incomeDetailsVM.inc_id;
                        incomeDetail.incDet_Date = incomeDetailsVM.incDet_Date;
                        incomeDetail.safe_id = incomeDetailsVM.safe_id;
                        incomeDetail.incDet_notes = incomeDetailsVM.incDet_notes;
                        incomeDetail.incDet_value = Math.Round(incomeDetailsVM.incDet_value,2);
                        incomeDetail.incDet_status = (int)incomeDetailsVM.IncDet_status;
                    db.Entry(incomeDetail).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    }
                    if (incomeDetail.incDet_status == (int)IncomeDetailsStatus.Finish)
                    {
                        var SafeTransactonEdit = new SafeTransaction();

                        SafeTransactonEdit.IncomeDetailsID = incomeDetail.incDet_id;
                        SafeTransactonEdit.TransactionDate = incomeDetail.incDet_Date;
                        SafeTransactonEdit.TransactionValue = Math.Round(incomeDetail.incDet_value,2);
                        SafeTransactonEdit.SafeID = incomeDetail.safe_id;
                        SafeTransactonEdit.OperationType = "+";
                        db.SafeTransactions.Add(SafeTransactonEdit);
                        db.SaveChanges();
                        SafeTransactionService.EditBlanseInSafe((int)SafeTransactonEdit.SafeID);


                    }

                    result = "true";
                }
                else
                {
                    IncomeDetail incomeDetail = new IncomeDetail();
                    incomeDetail.Income_inc_id = incomeDetailsVM.inc_id;
                    incomeDetail.incDet_Date = incomeDetailsVM.incDet_Date;
                    incomeDetail.safe_id = incomeDetailsVM.safe_id;
                    incomeDetail.incDet_notes = incomeDetailsVM.incDet_notes;
                    incomeDetail.incDet_value = Math.Round(incomeDetailsVM.incDet_value,2);
                    incomeDetail.incDet_status = (int)incomeDetailsVM.IncDet_status;


                    db.IncomeDetails.Add(incomeDetail);
                    db.SaveChanges();
                    if (incomeDetail.incDet_status == (int)IncomeDetailsStatus.Finish) { 
                    SafeTransaction safeTransaction = new SafeTransaction();
                    safeTransaction.IncomeDetailsID = incomeDetail.incDet_id;
                    safeTransaction.TransactionDate = incomeDetail.incDet_Date;
                    safeTransaction.TransactionValue = Math.Round(incomeDetail.incDet_value);
                    safeTransaction.SafeID = incomeDetail.safe_id;
                    safeTransaction.OperationType = "+";
                    db.SafeTransactions.Add(safeTransaction);
                    db.SaveChanges();
                        SafeTransactionService.EditBlanseInSafe((int)safeTransaction.SafeID);

                    }
                    result = "true";

                }
            }
            catch 
            {
                return "false";
            }
            return result;
        }
        #endregion
        #region Get Expense By ID

        public IncomeDetailsVM GetByID(int incDet_id)
        {
            IncomeDetailsVM Model = new IncomeDetailsVM();
            try
            {

                Model = (from x in db.IncomeDetails.Where(o => o.incDet_id == incDet_id)
                         select new IncomeDetailsVM()
                         {

                             incDet_id = x.incDet_id,
                             incDet_Date = x.incDet_Date,
                             safe_id = x.safe_id,
                             inc_id = x.Income_inc_id,
                             incDet_value = x.incDet_value,
                             incDet_notes = x.incDet_notes,
                             IncDet_status = (IncomeDetailsStatus)x.incDet_status
                         }).FirstOrDefault();
            }
            catch (Exception)
            {

                Model = null;
            }
            return Model;
        }
        #endregion
        public string Delete(int id)
        {
            

            try
            {
                IncomeDetail incomeDetail = db.IncomeDetails.Find(id);
                var safeid = incomeDetail.safe_id;
                
                db.IncomeDetails.Remove(incomeDetail);
                db.SaveChanges();
                SafeTransactionService.EditBlanseInSafe((int)safeid);

                return "true";
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public double GetSafeBlance(int SafeID,int incomDetails)
        {
            var val = db.SafeTransactions.Where(x => x.SafeID == SafeID && x.IncomeDetailsID != incomDetails).Sum(x=>x.OperationType=="+"?x.TransactionValue:-x.TransactionValue);
            return val;
        }
    }
}
