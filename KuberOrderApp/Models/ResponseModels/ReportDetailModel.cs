using System;
namespace KuberOrderApp.Models.ResponseModels
{
    public class ReportDetailModel
    {
        public string Name { get; set; }
        public double OpeningBal { get; set; }
        public double ClosingBal { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string data { get; set; }
    }
}
