using StockDB.Model;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace StockService
{
   public class JobService
    {
        StockModel db = new StockModel();

        #region Get All 
        public List<JobVM> GettAll()
        {
            var Model = new List<JobVM>();
            try
            {
                Model = (from j in db.Jobs
                         select new JobVM()
                         {
                             job_id = j.job_id,
                             job_name = j.job_name

                         }).ToList();
            }
            catch
            {
                Model = null;
            }

            return Model;
        }

        #endregion
        #region Get  By ID

        public JobVM GetByID(int id)
        {
            JobVM Model = new JobVM();
            try
            {

                Model = (from j in db.Jobs.Where(o => o.job_id == id)
                         select new JobVM()
                         {
                             job_id = j.job_id,
                             job_name = j.job_name
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
        public bool SaveinDatabase(JobVM JobVM)
        {
            var result = false;
            try
            {

                if (db.Jobs.FirstOrDefault(x => x.job_name == JobVM.job_name) != null)
                {

                    return false;
                }

                else if (JobVM.job_id > 0)
                {
                    Job job = db.Jobs.FirstOrDefault(x => x.job_id == JobVM.job_id);
                    job.job_name = JobVM.job_name;
                    db.Entry(job).State = EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    Job job = new Job();
                    job.job_name = JobVM.job_name;
                    db.Jobs.Add(job);
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
