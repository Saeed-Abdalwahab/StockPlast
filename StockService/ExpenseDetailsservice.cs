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
   public class ExpenseDetailsservice
    {
        StockModel db = new StockModel();

        #region Get All Expenses
        public List<ExpenseDetailsVM> Getall()
        {
            ExpensDetailsStatus expensDetailsStatus = new ExpensDetailsStatus();
            var d = expensDetailsStatus.GetDisplayName();
            var Model = new List<ExpenseDetailsVM>();
            try
            {
                Model = db.ExpenseDetails.Select(x => new ExpenseDetailsVM()
                {



                    ExpDet_id=x.ExpDet_id,
                    ExpDet_Date=x.ExpDet_Date,
                    safename=x.Safe.safe_name,
                    expensename=x.Expense.exp_name,
                    ExpDet_value=x.ExpDet_value,
                    ExpDet_notes=x.ExpDet_notes,
                    ExpDet_status = (ExpensDetailsStatus)x.ExpDet_status
                }).ToList();
     
                foreach (var item in Model)
                {
                    item.ExpDet_statusName = (item.ExpDet_status).GetDisplayName();
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
        public string SaveinDatabase(ExpenseDetailsVM expenseDetailsVM)
        {
            var result = "false";
            try
            {
                if (expenseDetailsVM.ExpDet_status == ExpensDetailsStatus.Finish)
                {
                    if (GetSafeBlance(expenseDetailsVM.safe_id) < expenseDetailsVM.ExpDet_value)
                    {
                        return "رصيد الخزينه لا يسمح باجراء العمله بشكل نهائي";
                    }
                }
                if (expenseDetailsVM.ExpDet_id > 0)
                {
                    ExpenseDetail  expenseDetail = db.ExpenseDetails.FirstOrDefault(x => x.ExpDet_id == expenseDetailsVM.ExpDet_id);
                    expenseDetail.exp_id = expenseDetailsVM.exp_id;
                    expenseDetail.ExpDet_Date = expenseDetailsVM.ExpDet_Date;
                    expenseDetail.safe_id = expenseDetailsVM.safe_id;
                    expenseDetail.ExpDet_notes = expenseDetailsVM.ExpDet_notes;
                    expenseDetail.ExpDet_value = Math.Round(expenseDetailsVM.ExpDet_value,2);
                    expenseDetail.ExpDet_status = (int)expenseDetailsVM.ExpDet_status;

        db.Entry(expenseDetail).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    if (expenseDetail.ExpDet_status == (int)ExpensDetailsStatus.Finish)
                    {
                        var SafeTransactonEdit = new SafeTransaction();

                        SafeTransactonEdit.ExpenseDetailsID = expenseDetail.ExpDet_id;
                        SafeTransactonEdit.TransactionDate = expenseDetail.ExpDet_Date;
                        SafeTransactonEdit.TransactionValue = Math.Round(expenseDetailsVM.ExpDet_value,2);
                        SafeTransactonEdit.SafeID = expenseDetail.safe_id;
                        SafeTransactonEdit.OperationType = "-";
                        db.SafeTransactions.Add(SafeTransactonEdit);
                        db.SaveChanges();
                        SafeTransactionService.EditBlanseInSafe((int)SafeTransactonEdit.SafeID);


                    }

                    result = "true";
                }
                else
                {
                    ExpenseDetail expenseDetail = new ExpenseDetail();
                    expenseDetail.exp_id = expenseDetailsVM.exp_id;
                    expenseDetail.ExpDet_Date = expenseDetailsVM.ExpDet_Date;
                    expenseDetail.safe_id = expenseDetailsVM.safe_id;
                    expenseDetail.ExpDet_notes = expenseDetailsVM.ExpDet_notes;
                    expenseDetail.ExpDet_value = Math.Round(expenseDetailsVM.ExpDet_value,2);
                    expenseDetail.ExpDet_status = (int)expenseDetailsVM.ExpDet_status;


                    db.ExpenseDetails.Add(expenseDetail);
                    db.SaveChanges();
                    if (expenseDetail.ExpDet_status == (int)ExpensDetailsStatus.Finish)
                    {
                        SafeTransaction SafeTransactonEdit = new SafeTransaction();

                        SafeTransactonEdit.ExpenseDetailsID = expenseDetail.ExpDet_id;
                        SafeTransactonEdit.TransactionDate = expenseDetail.ExpDet_Date;
                        SafeTransactonEdit.TransactionValue = expenseDetail.ExpDet_value;
                        SafeTransactonEdit.SafeID = expenseDetail.safe_id;
                        SafeTransactonEdit.OperationType = "-";
                        db.SafeTransactions.Add(SafeTransactonEdit);
                        db.SaveChanges();
                        SafeTransactionService.EditBlanseInSafe((int)SafeTransactonEdit.SafeID);


                    }
                    result = "true";

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion
        #region Get Expense By ID

        public ExpenseDetailsVM GetByID(int ExpDet_id)
        {
            ExpenseDetailsVM Model = new ExpenseDetailsVM();
            try
            {

                Model = (from x in db.ExpenseDetails.Where(o => o.ExpDet_id == ExpDet_id)
                         select new ExpenseDetailsVM()
                         {
                             
                             ExpDet_id = x.ExpDet_id,
                             ExpDet_Date = x.ExpDet_Date,
                             safe_id = x.safe_id,
                             exp_id = x.exp_id,
                             ExpDet_value = x.ExpDet_value,
                             ExpDet_notes = x.ExpDet_notes,
                             ExpDet_status = (ExpensDetailsStatus)x.ExpDet_status
                         }).FirstOrDefault();
            }
            catch (Exception)
            {

                Model = null;
            }
            return Model;
        }
        #endregion
        public bool Delete(int id)
        {
            try
            {
                ExpenseDetail expenseDetail = db.ExpenseDetails.Find(id);
                var safeid = expenseDetail.safe_id;
                db.ExpenseDetails.Remove(expenseDetail);
                db.SaveChanges();
                SafeTransactionService.EditBlanseInSafe((int)safeid);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public double GetSafeBlance(int safeID)
        {
            var val = db.SafeTransactions.Where(x => x.SafeID == safeID).Sum(x => x.OperationType == "+" ? x.TransactionValue : -x.TransactionValue);
            return val;
        }
    }
}
