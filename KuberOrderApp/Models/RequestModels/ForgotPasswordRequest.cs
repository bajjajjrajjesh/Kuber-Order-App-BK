using System;
namespace KuberOrderApp.Models.RequestModels
{
    public class ForgotPasswordRequest
    {
        public string ColMMobileNo { get; set; }
        public string ColCOTP { get; set; }
        public string ColPPassword { get; set; }
        public string ColCKuberUserType { get; set; }
    }
}
