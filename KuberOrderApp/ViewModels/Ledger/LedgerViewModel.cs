using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using KuberOrderApp.APIAndServices;
using KuberOrderApp.Enums;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.Models.ResponseModels;
using KuberOrderApp.Utilities;
using KuberOrderApp.ViewModels.Base;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KuberOrderApp.ViewModels.Ledger
{
    public class LedgerViewModel : BaseViewModel
    {
        #region Field Section.
        private DateTime _fromDate;
        private DateTime _toDate;
        private DataTable _dataTableCollection;
        private DataTable _duplicateDataTableCollection;
        private DataTable _filteredDataTableCollection;
        private string _searchRecord;
        public ReportRequest _reportRequest;
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
        public string SearchRecord
        {
            get { return _searchRecord; }
            set { SetProperty(ref _searchRecord, value); }
        }
        #endregion

        #region Constructor
        public LedgerViewModel()
        {
            _reportRequest = new ReportRequest()
            {
                OffsetFrom = 1,
                OffsetTo = 10,
                ProductFilter = ""
            };
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
            PrintPDFCommand = new Command(async () => await OnPrintClick());
            ShareCommand = new Command(async () => await OnShareClick());
            LoadMoreCommand = new Command(async () => await OnLoadMoreData());
        }
        #endregion

        #region Public Methods
        async public Task GetLedger()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");

                    var ledgersResponse = await ApiService.GetRequest<CommonResponseModel>(ApiPathString.GetLedgers, _reportRequest, null);
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();

                    if (ledgersResponse == null)
                        return;

                    if (!ledgersResponse.status)
                    {
                        Helper.DisplayAlert(ledgersResponse.message);
                        return;
                    }
                    App.TempString = ledgersResponse.data;
                    DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(ledgersResponse.data);
                    if (DataTableCollection != null && DataTableCollection.Rows.Count > 0)
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

        public void Search()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchRecord))
                {
                    DataTableCollection = FilteredDataTableCollection;
                }
                else
                {
                    DataTableCollection = Helper.SearchInAllColums(FilteredDataTableCollection, SearchRecord, StringComparison.OrdinalIgnoreCase);
                }
            }
            catch(Exception ex)
            {

            }
        }
        #endregion

        #region Private Methods
        async private Task OnPrintClick()
        {
            _isFromPDF = true;
            await Helper.GetPDFFileFromData(reportId: Convert.ToInt32(ReportType.MultipleLedger));
        }

        async private Task OnShareClick()
        {
            _isFromPDF = true;
            await Helper.GetPDFFileFromData(reportId: Convert.ToInt32(ReportType.MultipleLedger), isFromShare: true);
        }
        async private Task OnLoadMoreData()
        {
            IsBusy = true;
            _reportRequest.OffsetFrom += 5;
            _reportRequest.OffsetTo += 5;
            await GetLedger();
            IsBusy = false;
        }
        #endregion
    }
}
