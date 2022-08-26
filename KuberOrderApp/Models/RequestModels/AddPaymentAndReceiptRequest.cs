using System;
using SQLite;

namespace KuberOrderApp.Models.RequestModels
{
    public class AddPaymentAndReceiptRequest
    {
        //[PrimaryKey, AutoIncrement]
        //public int id { get; set; }

        public string Col99 { get; set; }   //ID
        public string Col90 { get; set; }// Receipt R or Payment P 1 Letter from Drop down of Receipt/Payment
        public string Col96 { get; set; } // Cash bank Account Code
        public string Col95 { get; set; }   //Party Accont Code
        public string Col97 { get; set; }    //Voucher No
        public string Col98 { get; set; }  //Voucher Date   //chng datetime to string
        public string Col91 { get; set; }    //Document No.
        public string Col92 { get; set; }  //Document Date //chng datetime to string
        public double Col94 { get; set; }    //Amount
        public string Col50 { get; set; }
    }
}
