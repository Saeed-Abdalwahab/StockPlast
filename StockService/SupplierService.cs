using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockViewModel;
using StockDB.Model;

namespace StockService
{
    public class SupplierService
    {
       
        #region Get All Suppliers
        public AlertVM<SupplierViewModel> SupplierList()
        {
            AlertVM<SupplierViewModel> alert = new AlertVM<SupplierViewModel>();
            var Model = new List<SupplierViewModel>();


            try
            {
                StockModel db = new StockModel();
                Model = (from ts in db.suppliers
                         select new SupplierViewModel()
                         {
                             sup_id=ts.sup_id,
                             contact_name=ts.contact_name,
                             sup_mail=ts.sup_mail,
                             sup_mobile=ts.sup_mobile,
                             sup_name=ts.sup_name,
                             WhatsApp=ts.WhatsApp,
                            

                         }).ToList();

                alert.data = Model;
                alert.type = true;
            }
            catch
            {
                
                Model = null;
                alert.data = Model;
                alert.type = false;


            }

            return alert ;
        }



        #endregion

        #region Auto Comolet For Suppliers

        public List<SupplierViewModel> returnSupSearch(string searchText)
        {

            List<supplier> result = SupplierByName(searchText);
            List<SupplierViewModel> returnConcatnationList = new List<SupplierViewModel>();
            foreach (var item in result)
            {
                
                    returnConcatnationList.Add(new SupplierViewModel
                    {
                       sup_name=item.sup_name,
                       sup_id=item.sup_id

                    });
                
            }
            return returnConcatnationList;

        }


        private List<supplier> SupplierByName(string Name)
        {
            StockModel db = new StockModel();

            var suppliers = db.suppliers.Where(s => s.sup_name.Contains(Name)).ToList();

            return suppliers;
        }

        #endregion        
    }
}
