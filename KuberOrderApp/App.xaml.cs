using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KuberOrderApp.APIAndServices;
using KuberOrderApp.Interfaces;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.Models.ResponseModels;
using KuberOrderApp.Pages.Authentication;
using KuberOrderApp.Pages.Company;
using KuberOrderApp.Pages.Home;
using KuberOrderApp.Pages.SideMenu;
using KuberOrderApp.SQLiteDatabase;
using KuberOrderApp.Utilities;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace KuberOrderApp
{
    public partial class App : Application
    {
        public static List<AddPaymentAndReceiptRequest> AddPaymentAndReceiptRequestsList = new List<AddPaymentAndReceiptRequest>();
        public static KuberDatabase Database;
        public static MainPage mainPage;
        public static string TempString = "";
        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTI1NjUwQDMxMzkyZTMzMmUzMGhUNnRPdDBhazNsbG5sNzVmRTNRSFZZd1BVeFZWbVVzOTBnWTRZcUVwbW89;NTI1NjUxQDMxMzkyZTMzMmUzMERaSzVIYURpVzFwSC9VME9tRkErWCtwdkRBUmVCY0NkTGhHODY0ejgwTFE9;NTI1NjUyQDMxMzkyZTMzMmUzMEdvMGhzczZBUjR5SFBRa0hDNjBhdk1YbDZNcm5EY3BjOTUvS1c4MUhweXc9;NTI1NjUzQDMxMzkyZTMzMmUzMFo3aml2eEVFMXBtZ0tBcVZOR0ZIcjIrUVZJT3ZWMEc3VTFhdmpvdElNNmM9");
            App.Current.Resources.MergedDictionaries.Clear();

            App.Current.Resources.MergedDictionaries.Add(new Themes.LightTheme());
            Database = new KuberDatabase(DependencyService.Get<ISQLiteDBFilePath>().GetLocalDBFilePath("Kuber.db"));
           
            Connectivity.ConnectivityChanged += Connectivity_Changed;
            CheckAutoLogin();
            AutoSyncData.AutoSyncPendingData();
        }

        [Obsolete]
        async protected override void OnStart()
        {
            MessagingCenter.Unsubscribe<string, AddLocation>(this, "SetLocation");
            MessagingCenter.Subscribe<string, AddLocation>(this, "SetLocation", async (sender, arg) =>
            {
                await Task.Run(() =>
                {
                    SetUserLocation(arg);
                });
            });
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        #region Public Methods
        public static void CheckAutoLogin()
        {
            var loginResponse = (string)Helper.GetPreference("LoginResponse");
            bool? isLogin = (bool?)Helper.GetPreference("IsLoginComplete");
            if (string.IsNullOrWhiteSpace(loginResponse) || isLogin == null || !isLogin.Value)
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            else
                Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        public static void NavigateToFirstScreen()
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        public static void NavigateToCompanyListScreen(List<CompanyList> companyList, LoginRequest loginRequest)
        {
            Application.Current.MainPage = new NavigationPage(new CompanyListPage(companyList, loginRequest));
        }
        #endregion

        #region Private Methods
        private void Connectivity_Changed(object sender, ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess;
            if(access != NetworkAccess.None)
            {
                AutoSyncData.AutoSyncPendingData();
            }
        }

        async private static void SetUserLocation(AddLocation addLocation)
        {
            await ApiService.PostRequest<CommonResponseModel>(ApiPathString.SaveLocation, addLocation, null);
        }
        #endregion

    }
}
