using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Input;
using KuberOrderApp.APIAndServices;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.Models.ResponseModels;
using KuberOrderApp.Resources;
using KuberOrderApp.Utilities;
using KuberOrderApp.ViewModels.Base;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KuberOrderApp.ViewModels.Company
{
    public class CompanyListViewModel : BaseViewModel
    {
        #region Field Section.
        private CompanyList _selectedCompany;
        private string _mobileNo;
        private string _password;
        private List<CompanyList> _companyLists;
        private LoginRequest _loginRequest;
        #endregion

        #region Properties
        public CompanyList SelectedCompany
        {
            get { return _selectedCompany; }
            set { SetProperty(ref _selectedCompany, value); }
        }
        public List<CompanyList> CompanyLists
        {
            get { return _companyLists; }
            set { SetProperty(ref _companyLists, value); }
        }
        #endregion

        #region Commands
        public ICommand LogInCommand { private set; get; }
        public ICommand LogoutCommand { private set; get; }
        #endregion

        #region Constructor
        public CompanyListViewModel(List<CompanyList> companyList, LoginRequest loginRequest)
        {
            //_mobileNo = mobileNo;
            //_password = password;
            CompanyLists = new List<CompanyList>(companyList);
            this._loginRequest = loginRequest;
            LogoutCommand = new Command(async () => await OnLogOutClick());
            LogInCommand = new Command(async () => await LogIn());
        }
        #endregion

        #region Private Methods
        private async Task OnLogOutClick()
        {

        }

        private async Task LogIn()
        {
            if (SelectedCompany == null)
            {
                Helper.DisplayAlert(TextString.BlankCompany);
                return;
            }
            if (SelectedCompany == null && _loginRequest == null)
            {
                Helper.DisplayAlert(TextString.BlankCompany);
                return;
            }

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    string currentDevicePlatform = "";
                    if (Device.RuntimePlatform == Device.iOS)
                        currentDevicePlatform = "iOS";
                    else
                        currentDevicePlatform = "mobile-Android";

                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");

                    _loginRequest.SalManChildColID = SelectedCompany.ColID.ToString();

                   
                    var loginResponse = await ApiService.PostRequest<SalesManLoginResponse>(ApiPathString.LoginBySalesMan, _loginRequest, null);
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                   
                    if (loginResponse == null)
                        return;

                    if (!loginResponse.status)
                    {
                        Helper.DisplayAlert(loginResponse.message);
                        return;
                    }




                    if (loginResponse.data != null && loginResponse.data.ProductList != null && loginResponse.data.ProductList.Count > 0)
                    {
                        foreach (var product in loginResponse.data.ProductList)
                            product.ColImagePath = "https://sample-videos.com/img/Sample-png-image-500kb.png";

                        await App.Database.Insert<ProductList>(loginResponse.data.ProductList);
                        
                    }
                    if (loginResponse.data != null && loginResponse.data.PartyList != null && loginResponse.data.PartyList.Count > 0)
                        await App.Database.Insert<PartyList>(loginResponse.data.PartyList);


                    if (loginResponse.data != null && loginResponse.data.Settings != null && loginResponse.data.Settings.Count > 0)
                    {
                       
                    
                        await App.Database.Insert<SettingResponse>(loginResponse.data.Settings);
                        var datax = await App.Database.GetDataList<SettingResponse>();
                    }


                    //Helper.SavePreference("PartyList", JsonConvert.SerializeObject(loginResponse.data.PartyList));
                    Helper.SavePreference("SelectedCompany", SelectedCompany.ColCompYearList);
                    Helper.SavePreference("IsLoginComplete", true);
                    Helper.SavePreference("Token", loginResponse.message);
                    App.NavigateToFirstScreen();

                }
                catch (Exception ex)
                {
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                }
            }
        }
        #endregion

    }
}
