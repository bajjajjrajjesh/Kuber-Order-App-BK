using System;
using System.Collections.Generic;

namespace KuberOrderApp.Models.ResponseModels
{
    public class DtLedgerDatas
    {
        public string COL99 { get; set; }
        public string COL81 { get; set; }
        public string VOUDATE { get; set; }
        public string COL83 { get; set; }
        public object Column1 { get; set; }
        public string COL45 { get; set; }
        public string VOUDOCNO { get; set; }
        public string ACCNAME { get; set; }
        public string DEBIT { get; set; }
        public string CREDIT { get; set; }
    }

    public class DetailDatas
    {
        public string Name { get; set; }
        public double OpeningBal { get; set; }
        public double ClosingBal { get; set; }
        public List<DetailDatas> dtLedgerData { get; set; }
    }

    public class MODResponseSinglePayableReport
    {
        public bool status { get; set; }
        public string message { get; set; }
        public DetailDatas data { get; set; }
    }
}
