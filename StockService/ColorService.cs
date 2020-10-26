using StockDB.Model;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;


namespace StockService
{
  public  class ColorService
    {
        StockModel db = new StockModel();
        #region GetAllColors
        public List<ColorVM> AllColors()
        {
            var Model = new List<ColorVM>();
            try
            {
                Model = (from color in db.colors
                         select new ColorVM()
                         {
                             color_id = color.color_id,
                             color_name = color.color_name
                         }).ToList();

            }
            catch
            {

                Model = null;
            }
            return Model;
        }
        #endregion

        #region Get Color By ID
        public ColorVM GetColorById (int color_id)
        {
            ColorVM Model = new ColorVM();

            try
            {
                Model = (from color in db.colors.Where(m => m.color_id == color_id)
                        select new ColorVM()
                        {
                            color_id = color.color_id,
                            color_name = color.color_name

                        }).FirstOrDefault();
            }
            catch (Exception)
            {

                Model = null;
            }
            return Model;
        }
        #endregion



        public bool SaveinDatabase(ColorVM colorVM)
        {
            var result = false;

            try
            {
                if (colorVM.color_id> 0)
                {
                    color color = db.colors.FirstOrDefault(x => x.color_id == colorVM.color_id);
                    color.color_name = colorVM.color_name;
                    db.Entry(color).State = EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                }
             
                else
                {
                    color color= new color();
                    color.color_name= colorVM.color_name;
                    db.colors.Add(color);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {

                throw ex; ;
            }
            return result;
        }
    }
}
