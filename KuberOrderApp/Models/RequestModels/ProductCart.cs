using System;
using KuberOrderApp.Models.Base;
using SQLite;

namespace KuberOrderApp.Models.RequestModels
{
    public class ProductCart : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public string AccountColPK { get; set; }
        public string AccountColName { get; set; }
        public string ColPK { get; set; }   //Col99
        public string ColName { get; set; } //Col98
        public string ColGrpCode { get; set; }  //Col89
        public string ColGrpName { get; set; }  //Col88
        public string ColCatCode { get; set; } //Col87
        public string ColCatName { get; set; } //Col86
        public string ColTypeCode { get; set; } //Col85
        public string ColTypeName { get; set; } //Col84//ColMastersCode,ColMastersName,
        //public double ColMRP { get; set; }  //Col93WR
        public double ColSaleRate { get; set; } //Col93

        public string ColMastersCode { get; set; }   //ColMastersCode
        public string ColMastersName { get; set; } // ColMastersName

        public double ColStock { get; set; } //    Col77
        //public double ColOrderedQty { get; set; } // total pending order quantity
        private double _colOrderedQty;
        public double ColOrderedQty
        {
            get { return _colOrderedQty; }
            set { SetProperty(ref _colOrderedQty, value); }
        }

        private double _colMRP;
        public double ColMRP
        {
            get { return _colMRP; }
            set { SetProperty(ref _colMRP, value); }
        }
        public double ColOrderLessStock { get; set; }
    }
}
