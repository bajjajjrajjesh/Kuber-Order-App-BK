using System;
using System.Collections.Generic;

namespace KuberOrderApp.Models.ResponseModels
{
    public class Payment
    {
        public string Col99 { get; set; }
        public DateTime Col98 { get; set; }
        public string Col90 { get; set; }
    }

    public class ReceiptPaymentDetailResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<Payment> data { get; set; }
    }
}
