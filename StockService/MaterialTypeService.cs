using StockDB.Model;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace StockService
{
 public   class MaterialTypeService
    {
        StockModel db = new StockModel();

        #region Get All MaterialType
        public List<MaterialTypeVM> GettAll()
        {
            var Model = new List<MaterialTypeVM>();
            try
            {
                Model = (from Materialtype in db.Material_Type
                         select new MaterialTypeVM()
                         {
                             mat_id = Materialtype.mat_id,
                             mat_name = Materialtype.mat_name

                         }).ToList();
            }
            catch
            {
                Model = null;
            }

            return Model;
        }

        #endregion
        #region Get Materialtype By ID

        public MaterialTypeVM GetByID(int id)
        {
            MaterialTypeVM Model = new MaterialTypeVM();
            try
            {

                Model = (from Materialtype in db.Material_Type.Where(o => o.mat_id == id)
                         select new MaterialTypeVM()
                         {
                             mat_id = Materialtype.mat_id,
                             mat_name = Materialtype.mat_name
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
        public bool SaveinDatabase(MaterialTypeVM materialTypeVM)
        {
            var result = false;
            try
            {

                if (db.Material_Type.FirstOrDefault(x => x.mat_name == materialTypeVM.mat_name) != null)
                {

                    return false;
                }

                else if (materialTypeVM.mat_id > 0)
                {
                    Material_Type materialType = db.Material_Type.FirstOrDefault(x => x.mat_id == materialTypeVM.mat_id);
                    materialType.mat_name = materialTypeVM.mat_name;
                    db.Entry(materialType).State = EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    Material_Type materialType = new Material_Type();
                    materialType.mat_name = materialTypeVM.mat_name;
                    db.Material_Type.Add(materialType);
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
