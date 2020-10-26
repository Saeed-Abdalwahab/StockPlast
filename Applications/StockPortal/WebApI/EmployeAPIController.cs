using PlasticStatic;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StockPortal.WebApI
{
    public class EmployeAPIController : ApiController
    {
        [Route("api/GetAllEmployees")]
        [HttpGet]
        public List<ViewModel.EmployeeVM> returnAllCustomers(string sText)
        {
            List<ViewModel.EmployeeVM> returnSearchResults = returnEmpSearch(sText);
            List<ViewModel.EmployeeVM> returnSearchResultsWithNameWithTop30 = returnSearchResults.Take(15).ToList();
            return returnSearchResultsWithNameWithTop30;
        }
        [Route("api/GetTechEmployees")]
        [HttpGet]
        public List<ViewModel.EmployeeVM> returnTechEmployees(string sText)
        {
            List<ViewModel.EmployeeVM> returnSearchResults = returnTechnichinEmpSearch(sText);
            List<ViewModel.EmployeeVM> returnSearchResultsWithNameWithTop30 = returnSearchResults.Take(15).ToList();
            return returnSearchResultsWithNameWithTop30;
        }

        #region methods
        private List<ViewModel.EmployeeVM> returnTechnichinEmpSearch(string searchText)
        {
            StockDB.Model.StockModel _stockModelEntities = new StockDB.Model.StockModel();
            var searchTextParameter = new SqlParameter("@SearchText", searchText);

            List<ViewModel.EmployeeVM> result = _stockModelEntities.Database
               .SqlQuery<ViewModel.EmployeeVM>("GetAllEmployee @SearchText", searchTextParameter)
               .ToList();
            int techEnum = int.Parse(Enums.EnumString.GetStringValue(Enums.job.Technician));
            var techemplist = result.Where(xx => xx.Job_id == techEnum).ToList();
            _stockModelEntities.Dispose();
            List<ViewModel.EmployeeVM> returnConcatnationList = new List<ViewModel.EmployeeVM>();
            foreach (var item in techemplist)
            {
                List<ViewModel.EmployeeVM> checkList = returnConcatnationList.Where(ee => ee.emp_id == item.emp_id).ToList();
                if (checkList.Count() == 0)
                {
                    returnConcatnationList.Add(new ViewModel.EmployeeVM
                    {
                        emp_id = item.emp_id,
                        emp_name = item.emp_name + " - " + item.jobName,

                    });
                }
            }
            return returnConcatnationList;

        }

        private List<ViewModel.EmployeeVM> returnEmpSearch(string searchText)
        {
            StockDB.Model.StockModel _stockModelEntities = new StockDB.Model.StockModel();
            var searchTextParameter = new SqlParameter("@SearchText", searchText);

            List<ViewModel.EmployeeVM> result = _stockModelEntities.Database
               .SqlQuery<ViewModel.EmployeeVM>("GetAllEmployee @SearchText", searchTextParameter)
               .ToList();

            _stockModelEntities.Dispose();
            List<ViewModel.EmployeeVM> returnConcatnationList = new List<ViewModel.EmployeeVM>();
            foreach (var item in result)
            {
                List<ViewModel.EmployeeVM> checkList = returnConcatnationList.Where(ee => ee.emp_id == item.emp_id).ToList();
                if (checkList.Count() == 0)
                {
                    returnConcatnationList.Add(new ViewModel.EmployeeVM
                    {
                        emp_id = item.emp_id,
                        emp_name = item.emp_name + " - " + item.jobName,

                    });
                }
            }
            return returnConcatnationList;

        }
        #endregion

    }
}