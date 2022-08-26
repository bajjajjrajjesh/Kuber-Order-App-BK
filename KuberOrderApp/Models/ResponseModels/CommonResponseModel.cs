using System;
namespace KuberOrderApp.Models.ResponseModels
{
    public class CommonResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string data { get; set; }
    }
}
