using System;
using SQLite;

namespace KuberOrderApp.Models.RequestModels
{
    public class SaveProductOrder
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string Col99 { get; set; }   //Foreign Key of T51
        public string ColID { get; set; }   //ID or Primary Key
        public string Col79 { get; set; }    //Party Code 
        public string Col96 { get; set; }    //Product Code
        public double Col95 { get; set; }    //Quantity
        public double Col94 { get; set; }    //Rate
        public double Col93 { get; set; }    //Amount
        public string Col50 { get; set; }    //Remark
    }
}
