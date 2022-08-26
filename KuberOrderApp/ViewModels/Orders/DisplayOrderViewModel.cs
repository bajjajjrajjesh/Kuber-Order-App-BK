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
    public class DisplayOrderViewModel : BaseViewModel
    {
        #region Field Section
        private DateTime _fromDate;
        private DateTime _toDate;
        private string _searchRecord;
        private DataTable _dataTableCollection;
        private DataTable _duplicateDataTableCollection;
        private DataTable _filteredDataTableCollection;
        private ReportRequest _reportRequest;
        #endregion

        #region Properties
        public bool _isFromPDF = false;

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
        #endregion

        #region Commands
        public ICommand SearchCommand { private set; get; }
        #endregion

        #region Constructor
        public DisplayOrderViewModel()
        {
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
            _reportRequest = new ReportRequest()
            {
                OffsetFrom = 1,
                OffsetTo = 10
            };

          
            LoadMoreCommand = new Command(async () => await OnLoadMoreData());
            SearchCommand = new Command(async () => await Search());
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

                    var displayOrderResponse = await ApiService.GetRequest<CommonResponseModel>(ApiPathString.DisplayOrder, _reportRequest, null);
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading(); 
                    if (displayOrderResponse == null)
                        return;

                    if (!displayOrderResponse.status)
                    {
                        Helper.DisplayAlert(displayOrderResponse.message);
                        return;
                    }
                    DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(displayOrderResponse.data);
                    if(DataTableCollection != null && DataTableCollection.Rows.Count > 0)
                    {
                        DataTableCollection.BeginLoadData();
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                            DataTableCollection.ImportRow(dataTable.Rows[i]);
                        DataTableCollection.EndLoadData();
                        FilteredDataTableCollection = DuplicateDataTableCollection = DataTableCollection;
                    }
                    else
                        FilteredDataTableCollection = DuplicateDataTableCollection = DataTableCollection = dataTable;
                   

                }
                catch (Exception ex)
                {
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                }
            }
        }

        public void GetFilterData()
        {
            try
            {
                DataTableCollection = FilteredDataTableCollection = Helper.FilterTable(DuplicateDataTableCollection, FromDate, ToDate);
            }
            catch(Exception ex)
            {

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

        async public Task OnPrintClick(string key)
        {
            _isFromPDF = true;
            await Helper.GetPDFFileFromData(reportId: Convert.ToInt32(ReportType.OrderDisp), filterId: key, isFromShare: true);
        }
        async public Task OnShareClick(string key)
        {
            _isFromPDF = true;
            await Helper.GetPDFFileFromData(reportId: Convert.ToInt32(ReportType.OrderDisp), filterId: key, isFromShare: true);
        }
        #endregion

        #region Private Methods
        async private Task OnLoadMoreData()
        {
            IsBusy = true;
            _reportRequest.OffsetFrom += 5;
            _reportRequest.OffsetTo += 5;
            await GetOrderedList();
            IsBusy = false;
        }
        #endregion
    }
}
