using StockDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService
{
   public  class ShaplonService
    {
      private  StockModel db = new StockModel();

        public bool SaveShiplon(int Cust_Id,string shap_name,bool check,out string Serial )
        {
            DateTime ? Date= null;
            if (check)
            {
                Date = DateTime.Now;
            }
            Shaplona shaplona = new Shaplona();
            shaplona.shap_name = shap_name;
            shaplona.shap_startDate = Date;
            shaplona.cust_id = Cust_Id;
            var shap = db.Shaplonas.OrderByDescending(x => x.shap_id).FirstOrDefault();
            shaplona.ShapSerial = (shap!=null? shap.shap_id + 1:1).ToString();
            try
            {
                db.Shaplonas.Add(shaplona);
                db.SaveChanges();
                Serial = shaplona.ShapSerial;
                return true;
            }
            catch
            {
                Serial = "";
                return false;
            }
        }
    }
}
