using StockDB.Model;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using PlasticStatic;
using static PlasticStatic.Enums;
using System.ComponentModel.DataAnnotations;

namespace StockService
{
   public class ExpenseService
    {
        StockModel db = new StockModel();

        #region Get All Expenses
        public List<ExpensesVM> Getall()
        {
            ExpensStatus expensStatus = new ExpensStatus();
           var d= expensStatus.GetDisplayName();
            var Model = new List<ExpensesVM>();
            try
            {
                Model = (from Expense in db.Expenses
                         select new ExpensesVM()
                         {
                             exp_id = Expense.exp_id,
                             exp_name = Expense.exp_name,
                             exp_ProfitStatus = (ExpensStatus)Expense.exp_ProfitStatus
                         }).ToList();
                foreach (var item in Model)
                {
                    item.exp_profitstautsname = (item.exp_ProfitStatus).GetDisplayName();
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
        public bool SaveinDatabase(ExpensesVM expensesVM)
        {
            var result = false;
            try
             {
                var test = db.Expenses.FirstOrDefault(x => x.exp_name == expensesVM.exp_name);
                if (test != null)
                {
                    if (test.exp_id != expensesVM.exp_id) { 
                    return false;
                    }
                }

                 if (expensesVM.exp_id > 0)
                {
                    Expense expense = db.Expenses.FirstOrDefault(x => x.exp_id == expensesVM.exp_id);
                    expense.exp_name = expensesVM.exp_name;
                    expense.exp_ProfitStatus = (int)expensesVM.exp_ProfitStatus;
                    db.Entry(expense).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    Expense expense = new Expense();
                    expense.exp_name = expensesVM.exp_name;
                    expense.exp_ProfitStatus = (int)expensesVM.exp_ProfitStatus;

                    db.Expenses.Add(expense);
                    db.SaveChanges();
                    result = true;

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

        public ExpensesVM GetByID(int exp_id)
        {
            ExpensesVM Model = new ExpensesVM();
            try
            {

                Model = (from expense in db.Expenses.Where(o => o.exp_id == exp_id)
                         select new ExpensesVM()
                         {
                             exp_id = expense.exp_id,
                             exp_name = expense.exp_name,
                             exp_ProfitStatus = (ExpensStatus)expense.exp_ProfitStatus
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
                Expense expense = db.Expenses.Find(id);
                db.Expenses.Remove(expense);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
