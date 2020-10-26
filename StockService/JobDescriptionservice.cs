
using StockDB.Model;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using PlasticStatic;
using System.Web.Mvc;

namespace StockService
{
  public  class JobDescriptionservice
    {
        StockModel db = new StockModel();

        #region Get All 
        public List<JobDescriptionVM> Getall()
        {
            var Model = new List<JobDescriptionVM>();
            try
            {
                Model = (from JD in db.JobDescriptions
                         select new JobDescriptionVM()
                         {
                             ID = JD.ID,
                             Name = JD.Name,
                             Jobname= JD.Job.job_name,
                             JobID=JD.JobID
                         }).ToList();
            }
            catch
            {
                Model = null;
            }

            return Model;
        }

        #endregion
        #region Save In Data base
        public bool SaveinDatabase(JobDescriptionVM JobDescriptionVM)
        {
            var result = false;
            try
            {
                //var test = db.JobDescriptions.FirstOrDefault(x => x.Name == JobDescriptionVM.Name );
                //if (test != null)
                //{
                //    if (test.ID != JobDescriptionVM.ID)
                //    {
                //        return false;
                //    }
                //}

                if (JobDescriptionVM.ID > 0)
                {
                    var JD = db.JobDescriptions.FirstOrDefault(x => x.ID == JobDescriptionVM.ID);
                    JD.Name = JobDescriptionVM.Name;
                    JD.JobID =JobDescriptionVM.JobID;
                    db.Entry(JD).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    JobDescription JD = new JobDescription();
                    JD.Name = JobDescriptionVM.Name;
                    JD.JobID = JobDescriptionVM.JobID;

                    db.JobDescriptions.Add(JD);
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

        public JobDescriptionVM GetByID(int ID)
        {
            JobDescriptionVM Model = new JobDescriptionVM();
            try
            {

                Model = (from JD in db.JobDescriptions.Where(o => o.ID == ID)
                         select new JobDescriptionVM()
                         {
                             ID = JD.ID,
                             Name = JD.Name,
                             JobID = JD.JobID,
                             Jobname= JD.Job.job_name
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
                var JD = db.JobDescriptions.Find(id);
                db.JobDescriptions.Remove(JD);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        //Method

        public List<Job> Jobes()
        {
            return db.Jobs.ToList();
        }
        public List<JobDescriptionVM> GetJobDescriptionByJobID(int id)
        {
            return db.JobDescriptions.Where(x => x.JobID == id).Select(x=>new JobDescriptionVM { ID=x.ID,Name=x.Name}).ToList();
        }
    }
}
