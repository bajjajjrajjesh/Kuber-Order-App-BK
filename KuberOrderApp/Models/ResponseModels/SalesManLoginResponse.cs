using System;
using System.Collections.Generic;
using KuberOrderApp.Models.Base;
using Newtonsoft.Json;
using SQLite;

namespace KuberOrderApp.Models.ResponseModels
{
    public class PartyList
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public string ColPK { get; set; }   //Col99
        public string ColName { get; set; } //Col98
        public string ColAccGrpCode { get; set; }   //Col94
        public string ColAccType { get; set; }   //Col092
        public string ColGroupName { get; set; }    //Col95
        public string ColCityCode { get; set; } //Col89
        public string ColCityName { get; set; } //Col88


        public string ColAlias { get; set; }    // Col98A 
        public double ColBalance { get; set; } //    Col77
        public int ColCreditDays { get; set; }  //Col91
        public double ColCreditLimit { get; set; } //Col90
        public string ColPartyTypeCode { get; set; } //Col62
        public string ColPartyTypeName { get; set; } //Col61



        public string ColPartyCategoryCode { get; set; } //ColPartyCategoryCode
        public string ColPartyCategoryName { get; set; } //ColPartyCategoryName

        public string ColAdd1 { get; set; } //Col75
        public string ColAdd2 { get; set; }  //Col74
        public string ColAdd3 { get; set; } //Col73




        public string ColPinCode { get; set; } //Col72
        public string ColAreaName { get; set; } //Col86
        public string ColAreaCode { get; set; } //Col87
        public string ColZoneCode { get; set; } //Col60
        public string ColZoneName { get; set; } //Col59
        public string ColStateName { get; set; } //ColStateName
        public string ColStateCode { get; set; } //ColStateCode



        public string ColSalMan { get; set; } //ColSMCode
        public string ColGSTIN { get; set; } //ColGSTIN
        public string ColContectPerson { get; set; } //Col83
        public string ColAadharNo { get; set; } //ColAadharNo
        public string ColPhoneO { get; set; } //Col70
        public string ColPhoneR { get; set; } //Col71
        public string ColFax { get; set; } //Col69
        public string ColtxtEMail { get; set; } //Col68
        public string ColSite { get; set; } //Col67
    }

    public class ProductList : BaseModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore), PrimaryKey, AutoIncrement]
       
        public int id { get; set; }
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

        private byte[] _orderImage;
        public byte[] OrderImage
        {
            get { return _orderImage; }
            set { SetProperty(ref _orderImage, value); }
        }

        private double _colMRP;
        public double ColMRP
        {
            get { return _colMRP; }
            set { SetProperty(ref _colMRP, value); }
        }
        public double ColOrderLessStock { get; set; } //stock minus order qty
        //public bool IsStockVisible { get; set; } //Hide Stock In Offline Mode

        private bool _isStockVisible;
        public bool IsStockVisible
        {
            get { return _isStockVisible; }
            set { SetProperty(ref _isStockVisible, value); }
        }


        public string ColImagePath { get; set; }
    }

    public class Data
    {
        public List<PartyList> PartyList { get; set; }
        public List<ProductList> ProductList { get; set; }
        public List<SettingResponse> Settings{ get; set; }
    }

    public class SalesManLoginResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }
    public class SettingResponse
    {
       

        public string SetttingType { get; set; }
        public string SettingDesc { get; set; }
        public string SettingValue { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore), PrimaryKey, AutoIncrement]
        public long SettingSeq { get; set; }
    }


}
