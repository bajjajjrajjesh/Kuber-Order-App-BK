using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Input;
using KuberOrderApp.APIAndServices;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.Models.ResponseModels;
using KuberOrderApp.Utilities;
using KuberOrderApp.ViewModels.Base;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KuberOrderApp.ViewModels.PaymentAndReceipt
{
    public class PaymentAndReceiptDetailViewModel : BaseViewModel
    {
        #region Field Section.
        private DateTime _fromDate;
        private DateTime _toDate;
        private DataTable _backupDataTable;
        private DataTable _dataTableCollection;
        private DataTable _duplicateDataTableCollection;
        private DataTable _filteredDataTableCollection;
        private string _searchRecord;
        private bool _isLoaded;
        private string _selectedKey;
        #endregion

        #region Properties
        public bool _isFromPDF = false;

        public bool IsLoaded
        {
            get { return _isLoaded; }
            set { SetProperty(ref _isLoaded, value); }
        }
        public DateTime FromDate
        {
            get { return _fromDate; }
            set { SetProperty(ref _fromDate, value); }
        }
        public DateTime ToDate
        {
            get { return _toDate; }
            set { SetProperty(ref _toDate, value); }
        }
        public DataTable DataTableCollection
        {
            get { return _dataTableCollection; }
            set { SetProperty(ref _dataTableCollection, value); }
        }
        public DataTable DuplicateDataTableCollection
        {
            get { return _duplicateDataTableCollection; }
            set { SetProperty(ref _duplicateDataTableCollection, value); }
        }
        public DataTable FilteredDataTableCollection
        {
            get { return _filteredDataTableCollection; }
            set { SetProperty(ref _filteredDataTableCollection, value); }
        }
        public DataTable BackupDataTable
        {
            get { return _backupDataTable; }
            set { SetProperty(ref _backupDataTable, value); }
        }
        public string SearchRecord
        {
            get { return _searchRecord; }
            set { SetProperty(ref _searchRecord, value); }
        }
        public string SelectedKey
        {
            get { return _selectedKey; }
            set { SetProperty(ref _selectedKey, value); }
        }
        #endregion

        #region Commands
        public ICommand SearchCommand { private set; get; }
        #endregion

        #region Constructor
        public PaymentAndReceiptDetailViewModel()
        {
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
            PrintPDFCommand = new Command(async () => await OnPrintClick());
            ShareCommand = new Command(async () => await OnShareClick());
            //SearchCommand = new Command(async () => await Search());
        }
        #endregion

        #region Public Methods
        async public Task GetPaymentAndReceiptDetailData()
        {
            if (IsLoaded)
                return;

            IsLoaded = true;
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");

                    var receiptPaymentResponse = await ApiService.GetRequest<CommonResponseModel>($"{ApiPathString.GetReceiptPaymentDetail}{SelectedKey}", null);
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();

                    if (receiptPaymentResponse == null)
                        return;

                    if (!receiptPaymentResponse.status)
                    {
                        Helper.DisplayAlert(receiptPaymentResponse.message);
                        return;
                    }
                    DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(receiptPaymentResponse.data);

                    FilteredDataTableCollection = DuplicateDataTableCollection = DataTableCollection = dataTable;

                }
                catch (Exception ex)
                {
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                }
            }
        }
        #endregion

        #region Private Methods
        async private Task OnPrintClick()
        {

        }
        async private Task OnShareClick()
        {

        }
       
        #endregion
    }
}
