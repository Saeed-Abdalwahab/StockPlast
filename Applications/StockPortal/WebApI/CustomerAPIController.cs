using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StockPortal.WebApI
{
    public class CustomerAPIController : ApiController
    {
        #region notes 
        // created by mohamed ibrahim Galal 15/12/2019
        #endregion 
        [Route("api/GetAllCustomers")]
        [HttpGet]
        public List<ViewModel.CustomerVM> returnAllCustomers(string sText)
        {
            List<ViewModel.CustomerVM> returnSearchResults = returnCustSearch(sText);
            List<ViewModel.CustomerVM> returnSearchResultsWithNameWithTop30 = returnSearchResults.Take(15).ToList();
            return returnSearchResultsWithNameWithTop30; 
        }

        #region methods
        private List<ViewModel.CustomerVM> returnCustSearch(string searchText)
        {
            StockDB.Model.StockModel _stockModelEntities = new StockDB.Model.StockModel();
            var searchTextParameter = new SqlParameter("@SearchText", searchText);

            List<ViewModel.CustomerVM> result = _stockModelEntities.Database
               .SqlQuery<ViewModel.CustomerVM>("GetAllCustomers @SearchText", searchTextParameter)
               .ToList();

            _stockModelEntities.Dispose();
            List<ViewModel.CustomerVM> returnConcatnationList = new List<ViewModel.CustomerVM>();
            foreach (var item in result)
            {
                List<ViewModel.CustomerVM> checkList = returnConcatnationList.Where(ee => ee.cust_id == item.cust_id).ToList();
                if (checkList.Count() == 0)
                {
                    returnConcatnationList.Add(new ViewModel.CustomerVM
                    {
                        cust_id = item.cust_id,
                        cust_name = item.cust_name,
                        cust_address = item.cust_address

                    });
                }
            }
            return returnConcatnationList;

        }
        #endregion 
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}