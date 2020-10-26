using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDB.DbOp
{
    public class AspNetUserWorkerDbOperation
    {
        private StockDB.Model.StockModel _SecurityEntities;

        public StockDB.Model.AspNetUserWorker GetRecordByUserId(string userId)
        {
            StockDB.Model.AspNetUserWorker aspNetUserWorker;
            using (_SecurityEntities = new Model.StockModel())
            {
                aspNetUserWorker = _SecurityEntities.AspNetUserWorkers.FirstOrDefault(b => b.UserId == userId);
            }

            return aspNetUserWorker;
        }
    }
}
