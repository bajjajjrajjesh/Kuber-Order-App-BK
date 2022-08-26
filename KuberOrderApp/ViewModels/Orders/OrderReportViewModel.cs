using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Input;
using KuberOrderApp.APIAndServices;
using KuberOrderApp.Enums;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.Models.ResponseModels;
using KuberOrderApp.Utilities;
using KuberOrderApp.ViewModels.Base;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KuberOrderApp.ViewModels.Orders
{
    public class OrderReportViewModel : BaseViewModel
    {
        #region Field Section
        private string _searchRecord;
        private DataTable _dataTableCollection;
        private DataTable _duplicateDataTableCollection;
        private DataTable _filteredDataTableCollection;
        private bool _isPending;
        private bool _isAll;
        private bool _isComplete;
        private ReportRequest _reportRequest;
        private int _orderStatus;
        #endregion

        #region Properties
        public bool _isFromPDF = false;

        public string SearchRecord
        {
            get { return _searchRecord; }
            set { SetProperty(ref _searchRecord, value); }
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
        public bool IsPending
        {
            get { return _isPending; }
            set { SetProperty(ref _isPending, value); }
        }
        public bool IsAll
        {
            get { return _isAll; }
            set { SetProperty(ref _isAll, value); }
        }
        public bool IsComplete
        {
            get { return _isComplete; }
            set { SetProperty(ref _isComplete, value); }
        }
        #endregion

        #region Commands
        public ICommand SelectedFilterCommand { private set; get; }
        public ICommand SearchCommand { private set; get; }
        #endregion

        #region Constructor
        public OrderReportViewModel()
        {
            _reportRequest = new ReportRequest()
            {
                OffsetFrom = 1,
                OffsetTo = 10,
                AccountFilter = "",
                ReportType = "S",
                ProductFilter = ""
            };
            IsAll = true;
            SelectedFilterCommand = new Command<string>((obj) => FilterData(obj));
            LoadMoreCommand = new Command(async () => await OnLoadMoreData());
            SearchCommand = new Command(async () => await Search());
            PrintPDFCommand = new Command(async () => await OnPrintClick());
            ShareCommand = new Command(async () => await OnShareClick());
        }
        #endregion

        #region Public Methods
        async public Task GetOrderedList()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");

                    var orderReportResponse = await ApiService.GetRequest<CommonResponseModel>(ApiPathString.DisplayOrderReport, _reportRequest, null);
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();

                    if (orderReportResponse == null)
                        return;

                    if (!orderReportResponse.status)
                    {
                        Helper.DisplayAlert(orderReportResponse.message);
                        return;
                    }
                    DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(orderReportResponse.data);
                    if (DataTableCollection != null && DataTableCollection.Rows.Count > 0)
                    {
                        DataTableCollection.BeginLoadData();
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                            DuplicateDataTableCollection.ImportRow(dataTable.Rows[i]);
                        DataTableCollection.EndLoadData();
                        FilteredDataTableCollection = DataTableCollection = DuplicateDataTableCollection;
                       
                    }
                    else
                        FilteredDataTableCollection = DuplicateDataTableCollection = DataTableCollection = dataTable;
                }
                catch (Exception ex)
                {
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                }
                FilterData("Pending");
            }
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
        #endregion

        #region Private Methods
        private void FilterData(string obj)
        {
            try
            {
                IsPending = false;
                IsAll = false;
                IsComplete = false;
                if (obj == "Pending")
                {
                    _orderStatus = 1;
                    IsPending = true;
                    DataTableCollection = FilteredDataTableCollection = Helper.FilterOrderReportByQuantity(DuplicateDataTableCollection, obj);
                }
                else if (obj == "All")
                {
                    _orderStatus = 2;
                    IsAll = true;
                    DataTableCollection = FilteredDataTableCollection = DuplicateDataTableCollection;
                }
                else if (obj == "Complete")
                {
                    _orderStatus = 0;
                    IsComplete = true;
                    DataTableCollection = FilteredDataTableCollection = Helper.FilterOrderReportByQuantity(DuplicateDataTableCollection, obj);
                }
            }
            catch(Exception ex)
            {
                DataTableCollection = FilteredDataTableCollection = null;
            }

        }

        async private Task OnLoadMoreData()
        {
            IsBusy = true;
            _reportRequest.OffsetFrom += 5;
            _reportRequest.OffsetTo += 5;
            await GetOrderedList();
            IsBusy = false;
        }

        async private Task OnPrintClick()
        {
            _isFromPDF = true;
            await Helper.GetPDFFileFromData(reportId: Convert.ToInt32(ReportType.OrderReport),orderStatus: _orderStatus);
        }
        async private Task OnShareClick()
        {
            _isFromPDF = true;
            await Helper.GetPDFFileFromData(reportId: Convert.ToInt32(ReportType.OrderReport), orderStatus: _orderStatus, isFromShare: true);
        }
        #endregion
    }
}
