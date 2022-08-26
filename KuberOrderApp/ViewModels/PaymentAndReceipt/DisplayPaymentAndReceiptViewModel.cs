using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using KuberOrderApp.APIAndServices;
using KuberOrderApp.Enums;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.Models.ResponseModels;
using KuberOrderApp.Pages.CommonPages;
using KuberOrderApp.Pages.SideMenu;
using KuberOrderApp.Utilities;
using KuberOrderApp.ViewModels.Base;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KuberOrderApp.ViewModels.PaymentAndReceipt
{
    public class DisplayPaymentAndReceiptViewModel : BaseViewModel
    {
        #region Field Section.
        private DateTime _fromDate;
        private DateTime _toDate;
        private string _selectedReceiptOption;
        private List<AddPaymentAndReceiptRequest> _paymentAndReceiptList;
        private DataTable _backupDataTable;
        private DataTable _dataTableCollection;
        private DataTable _duplicateDataTableCollection;
        private DataTable _filteredDataTableCollection;
        private List<PartyList> _partyLists;
        private List<PartyList> _accountNameLists;
        private PartyList _selectedPaymentOption;
        private PartyList _selectedAccountName;
        private string _searchRecord;
        private string _selectedPartyName;
        private bool _isLoaded;
        private ReportRequest _reportRequest;
        #endregion

        #region Properties
        public bool _isFromPDF = false;

        public string SelectedPartyName
        {
            get { return _selectedPartyName; }
            set { SetProperty(ref _selectedPartyName, value); }
        }
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
        public PartyList SelectedPaymentOption
        {
            get { return _selectedPaymentOption; }
            set { SetProperty(ref _selectedPaymentOption, value); }
        }
        public PartyList SelectedAccountName
        {
            get { return _selectedAccountName; }
            set { SetProperty(ref _selectedAccountName, value); }
        }
        public List<AddPaymentAndReceiptRequest> PaymentAndReceiptList
        {
            get { return _paymentAndReceiptList; }
            set { SetProperty(ref _paymentAndReceiptList, value); }
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
        public List<PartyList> PartyLists
        {
            get { return _partyLists; }
            set { SetProperty(ref _partyLists, value); }
        }
        public List<PartyList> AccountNameList
        {
            get { return _accountNameLists; }
            set { SetProperty(ref _accountNameLists, value); }
        }
        public string SearchRecord
        {
            get { return _searchRecord; }
            set { SetProperty(ref _searchRecord, value); }
        }
        #endregion

        #region Commands
        public ICommand SearchCommand { private set; get; }
        #endregion

        #region Constructor
        public DisplayPaymentAndReceiptViewModel()
        {
            _reportRequest = new ReportRequest()
            {
                OffsetFrom = 1,
                OffsetTo = 10
            };
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
          
            SelectionCommand = new Command( () =>  NavigateToCommonSearchPage());
            LoadMoreCommand = new Command(async () => await OnLoadMoreData());
            SearchCommand = new Command(async () => await Search());
            MessagingCenter.Subscribe<string, PartyList>(this, "DisplayReceiptPartyList", async (sender, arg) =>
            {
                SelectedPaymentOption = arg;
                SelectedPartyName = SelectedPaymentOption.ColName;
            });
        }
        #endregion

        #region Public Methods
        async public Task GetPaymentAndReceiptData()
        {
            if (IsLoaded)
                return;

            IsLoaded = true;
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");

                    var receiptPaymentResponse = await ApiService.GetRequest<CommonResponseModel>(ApiPathString.GetReceiptPayment, _reportRequest, null);
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();


                    
                    if (receiptPaymentResponse == null)
                        return;

                    if (!receiptPaymentResponse.status)
                    {
                        Helper.DisplayAlert(receiptPaymentResponse.message);
                        return;
                    }
                    DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(receiptPaymentResponse.data);
                    //BackupDataTable = dataTable;
                   // dataTable.Columns.RemoveAt(0);
                    if (DataTableCollection != null && DataTableCollection.Rows.Count > 0)
                    {
                        DataTableCollection.BeginLoadData();
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                            DataTableCollection.ImportRow(dataTable.Rows[i]);
                        DataTableCollection.EndLoadData();
                        FilteredDataTableCollection = DuplicateDataTableCollection = DataTableCollection;
                    }
                    else
                    {
                        FilteredDataTableCollection = DuplicateDataTableCollection = DataTableCollection = dataTable;
                        string partyListStr = (string)Helper.GetPreference("PartyList");
                        string partyListStr1 = (string)Helper.GetPreference("ProductList");

                        if (string.IsNullOrWhiteSpace(partyListStr))
                            return;

                        var partyList = JsonConvert.DeserializeObject<List<PartyList>>(partyListStr);
                        if (partyList != null && partyList.Count > 0)
                        {
                            PartyLists = new List<PartyList>(partyList.Where(x => x.ColAccType == "BA" || x.ColAccType == "BO" || x.ColAccType == "CS").ToList());
                            AccountNameList = new List<PartyList>(partyList.Where(x => x.ColAccType != "BA" && x.ColAccType != "BO" && x.ColAccType != "CS").ToList());
                        }
                    }

                }
                catch (Exception ex)
                {
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                }
            }
        }

        public void GetFilterData()
        {
            DataTableCollection = FilteredDataTableCollection = Helper.FilterTable(DuplicateDataTableCollection, FromDate, ToDate);
        }

        async public Task Search()
        {
            try
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");
                if (string.IsNullOrWhiteSpace(SearchRecord))
                {
                    DataTableCollection = FilteredDataTableCollection;
                }
                else
                {
                    DataTableCollection = Helper.SearchInAllColums(FilteredDataTableCollection, SearchRecord, StringComparison.OrdinalIgnoreCase);
                }
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
            catch(Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
        }

        async public Task OnPrintClick(string key)
        {
            _isFromPDF = true;
            await Helper.GetPDFFileFromData(reportId: Convert.ToInt32(ReportType.ReceiptDisp),filterId: key, isFromShare: true);
        }
        async public Task OnShareClick(string key)
        {
            _isFromPDF = true;
            await Helper.GetPDFFileFromData(reportId: Convert.ToInt32(ReportType.ReceiptDisp), filterId: key, isFromShare: true);
        }
        #endregion

        #region Private Methods
        async private void NavigateToCommonSearchPage()
        {
            await App.Current.MainPage.Navigation.PushAsync(new CommonSearchPage(isFromDisplayReceipt: true));
        }
        async private Task OnLoadMoreData()
        {
            IsLoaded = false;
            IsBusy = true;
            _reportRequest.OffsetFrom += 5;
            _reportRequest.OffsetTo += 5;
            await GetPaymentAndReceiptData();
            IsBusy = false;
        }
        #endregion


    }
}
