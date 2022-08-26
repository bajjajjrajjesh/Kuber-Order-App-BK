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
using KuberOrderApp.Models.StaticModels;
using KuberOrderApp.Pages.CommonPages;
using KuberOrderApp.Utilities;
using KuberOrderApp.ViewModels.Base;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KuberOrderApp.ViewModels.Stock
{
    public class StockViewModel : BaseViewModel
    {
        #region Field Section.
        private List<string> _groupList;
        private string _selectedGroupList;
        private List<string> _categoryList;
        private string _selectedCategoryList;
        private List<string> _typeList;
        private string _selectedTypeList;
        private List<string> _masterList;
        private string _selectedMasterList;
        private DateTime _fromDate;
        private DateTime _toDate;
        private string _selectedReceiptOption;
        private DataTable _dataTableCollection;
        private DataTable _duplicateDataTableCollection;
        private DataTable _filteredDataTableCollection;
        private List<PartyList> _partyLists;
        private List<PartyList> _accountNameLists;
        private PartyList _selectedPaymentOption;
        private PartyList _selectedAccountName;
        private string _searchRecord;
        public ReportRequest _reportRequest;
        private bool _isLoaded;
        #endregion

        #region Properties
        public bool _isFromPDF = false;

        public List<string> GroupList
        {
            get { return _groupList; }
            set { SetProperty(ref _groupList, value); }
        }
        public string SelectedGroupList
        {
            get { return _selectedGroupList; }
            set { SetProperty(ref _selectedGroupList, value); }
        }
        public List<string> CategoryList
        {
            get { return _categoryList; }
            set { SetProperty(ref _categoryList, value); }
        }
        public string SelectedCategoryList
        {
            get { return _selectedCategoryList; }
            set { SetProperty(ref _selectedCategoryList, value); }
        }
        public List<string> TypeList
        {
            get { return _typeList; }
            set { SetProperty(ref _typeList, value); }
        }
        public string SelectedTypeList
        {
            get { return _selectedTypeList; }
            set { SetProperty(ref _selectedTypeList, value); }
        }
        public List<string> MasterList
        {
            get { return _masterList; }
            set { SetProperty(ref _masterList, value); }
        }
        public string SelectedMasterList
        {
            get { return _selectedMasterList; }
            set { SetProperty(ref _selectedMasterList, value); }
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
        public ICommand SelectionAccountCommand { private set; get; }
        public ICommand SearchCommand { private set; get; }
        #endregion

        #region Constructor
        public StockViewModel()
        {
            MessagingCenter.Unsubscribe<string, string>(this, "GroupFilter");
            MessagingCenter.Subscribe<string, string>(this, "GroupFilter", async (sender, arg) =>
            {
                SelectedGroupList = arg;
                await FilterProduct();
            });
            MessagingCenter.Unsubscribe<string, string>(this, "CategoryFilter");
            MessagingCenter.Subscribe<string, string>(this, "CategoryFilter", async (sender, arg) =>
            {
                SelectedCategoryList = arg;
                await FilterProduct();
            });
            MessagingCenter.Unsubscribe<string, string>(this, "TypeFilter");
            MessagingCenter.Subscribe<string, string>(this, "TypeFilter", async (sender, arg) =>
            {
                SelectedTypeList = arg;
                await FilterProduct();
            });
            MessagingCenter.Unsubscribe<string, string>(this, "MasterFilter");
            MessagingCenter.Subscribe<string, string>(this, "MasterFilter", async (sender, arg) =>
            {
                SelectedMasterList = arg;
                await FilterProduct();
            });

            _reportRequest = new ReportRequest()
            {
                OffsetFrom = 1,
                OffsetTo = 10
            };
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
            PrintPDFCommand = new Command(async () => await OnPrintClick());
            ShareCommand = new Command(async () => await OnShareClick());
            LoadMoreCommand = new Command(async () => await OnLoadMoreData());
            SelectionCommand = new Command<string>((obj) => NavigateToCommonFilterPage(obj));
            SearchCommand = new Command(async () => await Search());
        }
        #endregion

        #region Public Methods
        async public Task GetStock()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");

                    var stockResponse = await ApiService.GetRequest<CommonResponseModel>(ApiPathString.GetStock, _reportRequest, null);
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();

                    if (stockResponse == null)
                        return;

                    if (!stockResponse.status)
                    {
                        Helper.DisplayAlert(stockResponse.message);
                        return;
                    }
                    DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(stockResponse.data);

                    //dataTable.Columns.RemoveAt(0);
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
                        var ProductList = await App.Database.GetDataList<ProductList>();
                        if (ProductList != null && ProductList.Count > 0)
                        {

                            GroupList = new List<string>(ProductList.Select(o => o.ColGrpName).Distinct());
                            CategoryList = new List<string>(ProductList.Select(o => o.ColCatName).Distinct());
                            TypeList = new List<string>(ProductList.Select(o => o.ColTypeName).Distinct());
                            MasterList = new List<string>(ProductList.Select(o => o.ColMastersName).Distinct());
                        }
                    }



                    var partyList = await App.Database.GetDataList<PartyList>();
                    if (partyList != null && partyList.Count > 0)
                    {
                        PartyLists = new List<PartyList>(partyList.Where(x => x.ColAccType == "BA" || x.ColAccType == "BO" || x.ColAccType == "CS").ToList());
                        AccountNameList = new List<PartyList>(partyList.Where(x => x.ColAccType != "BA" && x.ColAccType != "BO" && x.ColAccType != "CS").ToList());
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
        async public void NavigateToCommonFilterPage(object strFilter)
        {
            _isLoaded = true;
            if (strFilter.ToString() == "Group")
                await App.Current.MainPage.Navigation.PushAsync(new CommonFilterPage(isFromGroup: true, filterList: GroupList));
            else if (strFilter.ToString() == "Category")
                await App.Current.MainPage.Navigation.PushAsync(new CommonFilterPage(isFromCategory: true, filterList: CategoryList));
            else if (strFilter.ToString() == "Type")
                await App.Current.MainPage.Navigation.PushAsync(new CommonFilterPage(isFromType: true, filterList: TypeList));
            else if (strFilter.ToString() == "Master")
                await App.Current.MainPage.Navigation.PushAsync(new CommonFilterPage(isFromMaster: true, filterList: MasterList));
        }
        #endregion

        #region Private Methods
        async private Task FilterProduct()
        {
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading();
            DataTableCollection = FilteredDataTableCollection = await Helper.FilterTableByTypes(FilteredDataTableCollection, SelectedGroupList, SelectedCategoryList, SelectedTypeList, SelectedMasterList, StringComparison.OrdinalIgnoreCase);
            Acr.UserDialogs.UserDialogs.Instance.HideLoading();
        }
        async private Task OnPrintClick()
        {
            _isFromPDF = true;
            await Helper.GetPDFFileFromData(fromDateTime: FromDate, toDateTime: ToDate, reportId: Convert.ToInt32(ReportType.MultipleStock));
        }
        async private Task OnShareClick()
        {
            _isFromPDF = true;
            await Helper.GetPDFFileFromData(fromDateTime: FromDate, toDateTime: ToDate, reportId: Convert.ToInt32(ReportType.MultipleStock), isFromShare: true);
        }
        async private Task OnLoadMoreData()
        {
            IsBusy = true;
            _reportRequest.OffsetFrom += 5;
            _reportRequest.OffsetTo += 5;
            await GetStock();
            IsBusy = false;
        }
        #endregion
    }
}
