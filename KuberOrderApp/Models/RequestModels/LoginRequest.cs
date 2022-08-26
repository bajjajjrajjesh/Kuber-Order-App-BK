using System;
using Newtonsoft.Json;

namespace KuberOrderApp.Models.RequestModels
{
    public class LoginRequest
    {
        public string SalManChildColID { get; set; }
        public string ColMMobileNo { get; set; }
        public string ColPPassword { get; set; }
        public string ColIPAddress { get; set; }
        public string ColDClientDate { get; set; }
        public string ColDeviceInfo { get; set; }
    }
}
