using System;
namespace KuberOrderApp.Models.RequestModels
{
    public class ReportRequest
    {
        public int OffsetFrom { get; set; }
        public int OffsetTo { get; set; }
        public string ProductFilter { get; set; }
        public string AccountFilter { get; set; }
        public string ReportType { get; set; }
    }
}
