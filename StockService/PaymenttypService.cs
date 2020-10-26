using StockDB.Model;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace StockService
{
   public class PaymenttypService
    {
        StockModel db = new StockModel();
        public List<PaymentTypVM> GettAll()
        {
            var Model = new List<PaymentTypVM>();
            try
            {
                Model = (from j in db.PaymentTypes
                         select new PaymentTypVM()
                         {
                             ID = j.ID,
                             Name = j.Name

                         }).ToList();
            }
            catch
            {
                Model = null;
            }

            return Model;
        }

        #region Get  By ID

        public PaymentTypVM GetByID(int id)
        {
            PaymentTypVM Model = new PaymentTypVM();
            try
            {

                Model = (from j in db.PaymentTypes.Where(o => o.ID == id)
                         select new PaymentTypVM()
                         {
                             ID = j.ID,
                             Name = j.Name
                         }).FirstOrDefault();
            }
            catch (Exception)
            {

                Model = null;
            }
            return Model;
        }
        #endregion
        #region Save In Data base
        public bool SaveinDatabase(PaymentTypVM PaymentTypVM)
        {
            var result = false;
            try
            {

                if (db.PaymentTypes.FirstOrDefault(x => x.Name == PaymentTypVM.Name) != null)
                {

                    return false;
                }

                else if (PaymentTypVM.ID > 0)
                {
                    var paymentType = db.PaymentTypes.FirstOrDefault(x => x.ID == PaymentTypVM.ID);
                    paymentType.Name = PaymentTypVM.Name;
                    db.Entry(paymentType).State = EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    PaymentType paymentType = new PaymentType();
                    paymentType.Name = PaymentTypVM.Name;
                    db.PaymentTypes.Add(paymentType);
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

    }
}
