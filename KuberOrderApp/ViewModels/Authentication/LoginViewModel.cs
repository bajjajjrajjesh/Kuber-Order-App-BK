using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using KuberOrderApp.APIAndServices;
using KuberOrderApp.Interfaces;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.Models.ResponseModels;
using KuberOrderApp.Pages.Authentication;
using KuberOrderApp.Resources;
using KuberOrderApp.Utilities;
using KuberOrderApp.ViewModels.Base;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KuberOrderApp.ViewModels.Authentication
{
    public class LoginViewModel : BaseViewModel
    {
        #region Field Section.
        IStatusBar _statusBar;
        private string _mobileNo;
        private string _password;
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
        #endregion

        #region Commands
        public ICommand LogInCommand { private set; get; }
        public ICommand ForgotPasswordCommand { private set; get; }
        #endregion

        #region Constructor
        public LoginViewModel()
        {
/*            MobileNo = "70467969501";
            Password = "123456";*/
            LogInCommand = new Command(async () => await LogIn());
            ForgotPasswordCommand = new Command(() =>  NavigateToForgotPassword());
        }
        #endregion

        #region Private Methods
        private async void NavigateToForgotPassword()
        {
            await App.Current.MainPage.Navigation.PushAsync(new ForgotPasswordPage());
        }

        private async Task LogIn()
        {
            if (await CheckValidation())
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    try
                    {
                        string currentDevicePlatform = "";
                        if (Device.RuntimePlatform == Device.iOS)
                            currentDevicePlatform = "mobile-iOS";
                        else
                            currentDevicePlatform = "mobile-Android";

                        Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");
                        LoginRequest loginRequest = new LoginRequest()
                        {
                            ColMMobileNo = MobileNo,
                            ColPPassword = Password,
                            ColIPAddress = "1",
                            ColDClientDate = DateTime.Now.ToString("MM/dd/yyyy"),
                            ColDeviceInfo = currentDevicePlatform,
                        };
                        var loginResponse = await ApiService.PostRequest<LoginResponse>(ApiPathString.LoginEndPoint, loginRequest, null);
                        Acr.UserDialogs.UserDialogs.Instance.HideLoading();

                        if (loginResponse == null)
                        {
                            
                            return;

                        }
                        if (!loginResponse.status)
                        {
                            Helper.DisplayAlert(loginResponse.message);
                            return;
                        }

                        Helper.SavePreference("LoginResponse", JsonConvert.SerializeObject(loginResponse));
                        App.NavigateToCompanyListScreen(loginResponse.data, loginRequest);

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
            if (string.IsNullOrWhiteSpace(MobileNo))
            {
                Helper.DisplayAlert(TextString.BlankMobileNo);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(Password))
            {
                Helper.DisplayAlert(TextString.BlankPassword);
                return false;
            }
            else if (Password.Length < 6)
            {
                Helper.DisplayAlert(TextString.PasswordLengthError);
                return false;
            }

            return true;
        }
        #endregion
    }
}
