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
    public class IncomeService
    {

        StockModel db = new StockModel();

        #region Get All Incomes
        public List<IncomeVM> Getall()
        {
            IncomeStatus incomeStatus = new IncomeStatus();
            var d = incomeStatus.GetDisplayName();
            var Model = new List<IncomeVM>();
            try
            {
                Model = (from Income in db.Incomes
                         select new IncomeVM()
                         {
                             inc_id = Income.inc_id,
                             inc_name = Income.inc_name,
                             inc_ProfitStatus = (IncomeStatus)Income.inc_ProfitStatus
                         }).ToList();
                foreach (var item in Model)
                {
                    item.inc_ProfitStatusname = (item.inc_ProfitStatus).GetDisplayName();
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
        public bool SaveinDatabase(IncomeVM incomeVM)
        {
            var result = false;
            try
            {
                var test = db.Incomes.FirstOrDefault(x => x.inc_name == incomeVM.inc_name);
                if (test != null)
                {
                    if (test.inc_id != incomeVM.inc_id)
                    {
                        return false;
                    }
                }

                if (incomeVM.inc_id > 0)
                {
                    Income  income = db.Incomes.FirstOrDefault(x => x.inc_id == incomeVM.inc_id);
                    income.inc_name = incomeVM.inc_name;
                    income.inc_ProfitStatus = (int)incomeVM.inc_ProfitStatus;
                    db.Entry(income).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    Income income = new Income();
                    income.inc_name = incomeVM.inc_name;
                    income.inc_ProfitStatus = (int)incomeVM.inc_ProfitStatus;

                    db.Incomes.Add(income);
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

        public IncomeVM GetByID(int inc_id)
        {
            IncomeVM Model = new IncomeVM();
            try
            {

                Model = (from income in db.Incomes.Where(o => o.inc_id == inc_id)
                         select new IncomeVM()
                         {
                             inc_id = income.inc_id,
                             inc_name = income.inc_name,
                             inc_ProfitStatus = (IncomeStatus)income.inc_ProfitStatus
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
                Income income = db.Incomes.Find(id);
                db.Incomes.Remove(income);
                
                db.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
                //throw ex;
            }
        }
    }
}
