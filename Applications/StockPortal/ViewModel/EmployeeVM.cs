using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockPortal.ViewModel
{
    public class EmployeeVM
    {
        public int emp_id { get; set; }

        public string emp_name { get; set; }

        public string emp_mobile { get; set; }

        public string emp_mail { get; set; }

        public string emp_password { get; set; }

        public int login_status { get; set; }

        public int Job_id { get; set; }

        public string jobName { get; set; }
    }
}