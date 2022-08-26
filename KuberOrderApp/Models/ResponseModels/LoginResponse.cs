using System;
using System.Collections.Generic;

namespace KuberOrderApp.Models.ResponseModels
{
    public class CompanyList
    {
        public int ColID { get; set; }
        public string ColCompYearList { get; set; }

       
    }

    public class LoginResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<CompanyList> data { get; set; }
    }
}
