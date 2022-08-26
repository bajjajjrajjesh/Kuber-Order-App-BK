using System;
namespace KuberOrderApp.Models.RequestModels
{
    public class AddLocation
    {
        public DateTime ColDDateTime { get; set; }
        public double ColNLongitude { get; set; }
        public double ColNLatitude { get; set; }
        public int ColCUserID { get; set; }
    }
}
