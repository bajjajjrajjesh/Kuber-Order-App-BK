using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using KuberOrderApp.APIAndServices;
using KuberOrderApp.Models.ResponseModels;
using KuberOrderApp.Pages.Authentication;
using KuberOrderApp.Pages.OutStandingPayable;
using KuberOrderApp.Pages.PaymentAndReceipt;
using KuberOrderApp.Pages.Receivable;
using KuberOrderApp.Pages.Stock;
using KuberOrderApp.Utilities;
using KuberOrderApp.ViewModels.Base;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KuberOrderApp.ViewModels.Home
{
    public class DashboardViewModel : BaseViewModel
    {
        #region Field Section.
        private DataTable _dataTableReceivable;
        private DataTable _dataTablePayable;
        private DataTable _dataTableStockReport;
        private DataTable _dataTableCashBank;
        private double _tableHeight = 0;
        private double _receivableTotal = 0;
        private double _payableTotal = 0;
        private double _stockTotal = 0;
        private double _cashBankTotal = 0;
        private double _cashBankDebitTotal = 0;
        private double _cashBankCreditTotal = 0;
        private string selectedCompany = "";
        #endregion

        #region Properties

        public string SelectedCompany
        {
            get { return selectedCompany; }
            set { SetProperty(ref selectedCompany, value); }
        }
        public DataTable DataTableReceivable
        {
            get { return _dataTableReceivable; }
            set { SetProperty(ref _dataTableReceivable, value); }
        }
        public DataTable DataTablePayable
        {
            get { return _dataTablePayable; }
            set { SetProperty(ref _dataTablePayable, value); }
        }
        public DataTable DataTableStockReport
        {
            get { return _dataTableStockReport; }
            set { SetProperty(ref _dataTableStockReport, value); }
        }
        public DataTable DataTableCashBank
        {
            get { return _dataTableCashBank; }
            set { SetProperty(ref _dataTableCashBank, value); }
        }
        public double TableHeight
        {
            get { return _tableHeight; }
            set { SetProperty(ref _tableHeight, value); }
        }
        public double ReceivableTotal
        {
            get { return _receivableTotal; }
            set { SetProperty(ref _receivableTotal, value); }
        }
        public double PayableTotal
        {
            get { return _payableTotal; }
            set { SetProperty(ref _payableTotal, value); }
        }
        public double StockTotal
        {
            get { return _stockTotal; }
            set { SetProperty(ref _stockTotal, value); }
        }
        public double CashBankDebitTotal
        {
            get { return _cashBankDebitTotal; }
            set { SetProperty(ref _cashBankDebitTotal, value); }
        }

        public double CashBankCreditTotal
        {
            get { return _cashBankCreditTotal; }
            set { SetProperty(ref _cashBankCreditTotal, value); }
        }
        #endregion

        #region Commands
        public ICommand ReceivableCommand { get; private set; }
        public ICommand PayableCommand { get; private set; }
        public ICommand StockCommand { get; private set; }
        public ICommand CashBankCommand { get; private set; }
        #endregion


        #region Constructor
        public DashboardViewModel()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            
            if(mainDisplayInfo.Height > 0)
            {
                TableHeight = (mainDisplayInfo.Height - 50) / 4;
            }
            SelectedCompany = (string)Helper.GetPreference("SelectedCompany");
            ReceivableCommand = new Command(OnReceivableSelected);
            PayableCommand = new Command(OnPayableSelected);
            StockCommand = new Command(OnStockSelected);
            CashBankCommand = new Command(OnCashBankSelected);
        }
        #endregion


        #region Public Methods
        async public Task GetDashboardData()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");

                    var dashboardResponse = await ApiService.GetRequest<CommonResponseModel>(ApiPathString.GetDashboardsData, null, null);
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();

                    if (dashboardResponse == null)
                        return;

                    if (!dashboardResponse.status)
                    {
                        Helper.DisplayAlert(dashboardResponse.message);
                        return;
                    }
                    App.TempString = dashboardResponse.data;
                    DataSet dataSets = JsonConvert.DeserializeObject<DataSet>(dashboardResponse.data);
                    if (dataSets == null)
                        return;

                    DataTableReceivable = dataSets.Tables[0];
                    ReceivableTotal = DataTableReceivable.AsEnumerable().Sum(row => row.Field<double>("ColAmount"));   
                    DataTablePayable = dataSets.Tables[1];
                    PayableTotal = DataTablePayable.AsEnumerable().Sum(row => row.Field<double>("ColAmount"));
                    DataTableStockReport = dataSets.Tables[2];
                    StockTotal = DataTableStockReport.AsEnumerable().Sum(row => row.Field<double>("ColAmount"));
                    DataTableCashBank = dataSets.Tables[3];
                    CashBankDebitTotal = DataTableCashBank.AsEnumerable().Sum(row => row.Field<double>("ColDebit"));
                    CashBankCreditTotal = DataTableCashBank.AsEnumerable().Sum(row => row.Field<double>("ColCredit"));



                }
                catch (Exception ex)
                {
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                    
                }
            }
        }
        #endregion

        #region Private Methods
        private void OnReceivableSelected(object obj)
        {
            App.mainPage.Detail = new NavigationPage(new ReceivablePage());
        }
        private void OnPayableSelected(object obj)
        {
            App.mainPage.Detail = new NavigationPage(new OutStandingPayablePage());
        }
        private void OnStockSelected(object obj)
        {
            App.mainPage.Detail = new NavigationPage(new StockPage());
        }
        private void OnCashBankSelected(object obj)
        {
            App.mainPage.Detail = new NavigationPage(new DisplayPaymentAndReceiptPage());
        }
        #endregion
    }
}
