using System;
namespace KuberOrderApp.Models.RequestModels
{
    public class MODGeneratePdf
    {
        public int ReportID { get; set; }   //case 1=order,case 2=ledger
        public string EntryORMasterID { get; set; } //EntryID=Col99 
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int OrderStatus { get; set; }    //all,pending,clear etc.
    }
}
