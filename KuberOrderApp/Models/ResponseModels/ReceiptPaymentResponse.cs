using System;
using System.Collections.Generic;

namespace KuberOrderApp.Models.ResponseModels
{
        public class Datum
        {
            public string Col99 { get; set; }
            public object Col81 { get; set; }
            public string VouDate { get; set; }
            public string VType { get; set; }
            public string VouDocNo { get; set; }
            public string AccountName { get; set; }
            public string RecPay { get; set; }
            public string Amount { get; set; }
        }

        public class ReceiptPaymentResponse
        {
            public bool status { get; set; }
            public string message { get; set; }
            public List<Datum> data { get; set; }
        }
}
