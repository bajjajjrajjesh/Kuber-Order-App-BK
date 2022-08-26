using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using KuberOrderApp.APIAndServices;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.Models.ResponseModels;
using KuberOrderApp.Resources;
using KuberOrderApp.Utilities;
using KuberOrderApp.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KuberOrderApp.ViewModels.Authentication
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        #region Field Section.
        private string _mobileNo;
        private string _password;
        private string _confirmPassword;
        private string _otp;
        #endregion

        #region Properties
        public string MobileNo
        {
            get { return _mobileNo; }
            set { SetProperty(ref _mobileNo, value); }
        }
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { SetProperty(ref _confirmPassword, value); }
        }
        public string OTP
        {
            get { return _otp; }
            set { SetProperty(ref _otp, value); }
        }
        #endregion

        #region Commands
        public ICommand DoneCommand { private set; get; }
        public ICommand SendOTPCommand { private set; get; }
        #endregion

        #region Constructor
        public ForgotPasswordViewModel()
        {
            DoneCommand = new Command(() => OnDoneClick());
            SendOTPCommand = new Command(() => OnSendOTPClick());
        }
        #endregion

        #region Private Methods
        async private void OnSendOTPClick()
        {
            if(string.IsNullOrWhiteSpace(MobileNo))
            {
                Helper.DisplayAlert(TextString.BlankMobileNo);
                return;
            }

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");
                    string apiEndPoint = $"{ApiPathString.GetOTP}?MobileNo={MobileNo}&ECUserType=";
                    var otpResponse = await ApiService.GetRequest<CommonResponseModel>(apiEndPoint, null, null);
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();

                    if (otpResponse == null)
                        return;

                    if (!otpResponse.status)
                    {
                        Helper.DisplayAlert(otpResponse.message);
                        return;
                    }

                    Helper.DisplayAlert(otpResponse.message);
                }
                catch (Exception ex)
                {
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                }
            }
        }

        async private void OnDoneClick()
        {
            if (await CheckValidation())
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    try
                    {
                        ForgotPasswordRequest forgotPasswordRequest = new ForgotPasswordRequest()
                        {
                            ColMMobileNo = MobileNo,
                            ColCOTP = OTP,
                            ColPPassword = Password,
                            ColCKuberUserType = ""
                        };

                        Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");
                        var otpResponse = await ApiService.PostRequest<CommonResponseModel>(ApiPathString.ForgotPassword, forgotPasswordRequest, null);
                        Acr.UserDialogs.UserDialogs.Instance.HideLoading();

                        if (otpResponse == null)
                            return;

                        if (!otpResponse.status)
                        {
                            Helper.DisplayAlert(otpResponse.message);
                            return;
                        }

                        Helper.DisplayAlert(otpResponse.message);
                        await App.Current.MainPage.Navigation.PopAsync();
                    }
                    catch (Exception ex)
                    {
                        Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                    }
                }
            }
        }

        private async Task<bool> CheckValidation()
        {
            if (string.IsNullOrWhiteSpace(OTP))
            {
                Helper.DisplayAlert(TextString.BlankOTP);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(Password))
            {
                Helper.DisplayAlert(TextString.BlankPassword);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                Helper.DisplayAlert(TextString.BlankConfirmPassword);
                return false;
            }
            else if (Password.Length < 6)
            {
                Helper.DisplayAlert(TextString.PasswordLengthError);
                return false;
            }
            else if (ConfirmPassword.Length < 6)
            {
                Helper.DisplayAlert(TextString.ConfirmPasswordLengthError);
                return false;
            }
            else if (Password != ConfirmPassword)
            {
                Helper.DisplayAlert(TextString.PasswordLengthError);
                return false;
            }

            return true;
        }
        #endregion
    }
}
