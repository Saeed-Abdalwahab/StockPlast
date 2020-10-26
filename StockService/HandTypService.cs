using StockDB.Model;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService
{
    public class HandTypService
    {
        StockModel db = new StockModel();

        #region Get All HandTyps
        public List<HandTypVM> AllHandTyps()
        {
            var Model = new List<HandTypVM>();
            try
            {
                Model = (from HandTyp in db.HandTypes
                         select new HandTypVM()
                         {
                             HandType_id=HandTyp.HandType_id,
                             HandType_name = HandTyp.HandType_name

                         }).ToList();
            }
            catch
            {
                Model = null;
            }

            return Model;
        }

        #endregion

        #region Get HandTyp By ID

        public HandTypVM GetHandtypByid(int HandTyp_id)
        {
            HandTypVM Model = new HandTypVM();
            try
            {

                Model = (from HandTyp in db.HandTypes.Where(o => o.HandType_id == HandTyp_id)
                         select new HandTypVM()
                         {
                             HandType_id = HandTyp.HandType_id,
                             HandType_name = HandTyp.HandType_name
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
        public bool SaveinDatabase(HandTypVM handTypVM)
        {
            var result = false;
            try
            {
                
                if (db.HandTypes.FirstOrDefault(x => x.HandType_name == handTypVM.HandType_name)!=null) {

                    return false;
                }

                else if (handTypVM.HandType_id > 0)
                {
                    HandType handType = db.HandTypes.FirstOrDefault(x => x.HandType_id == handTypVM.HandType_id);
                    handType.HandType_name = handTypVM.HandType_name;
                    db.Entry(handType).State = EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    HandType handType = new HandType();
                    handType.HandType_name = handTypVM.HandType_name;
                    db.HandTypes.Add(handType);
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
