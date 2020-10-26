using StockDB.Model;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService
{
    public class ItemService
    {

        #region Auto Comolet For Items
        public List<ItemVM> returnitemSearch(string searchText)
        {

            List<Item> result = ItemByName(searchText);
            List<ItemVM> returnConcatnationList = new List<ItemVM>();
            foreach (var item in result)
            {

                returnConcatnationList.Add(new ItemVM
                {
                    ID = item.ID,
                    Name = item.Name

                });

            }
            return returnConcatnationList;

        }


        private List<Item> ItemByName(string Name)
        {
            StockModel db = new StockModel();
            var Items = new List<Item>();
            int value;
            bool resualt = int.TryParse(Name, out value);

            if (resualt)
            {
                Items = db.Items.Where(s => s.SerialNum == Name).ToList();
            }
            else
            {
                Items = db.Items.Where(s => s.Name.Contains(Name)).ToList();
            }

            return Items;
        }
        #endregion
    }
}
