using System;
using System.Collections.Generic;
using System.Data;
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

namespace KuberOrderApp.ViewModels.OutStandingPayable
{
    public class OutStandingPayableDetailViewModel : BaseViewModel
    {
        #region Field Section.
        private DateTime _fromDate;
        private DateTime _toDate;
        private DataTable _dataTableCollection;
        private DataTable _duplicateDataTableCollection;
        private DataTable _filteredDataTableCollection;
        private string _searchRecord;
        public ReportRequest _reportRequest;
        private string _partyName;
        private string _openingBalance;
        private string _selectedKey;
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
        public string PartyName
        {
            get { return _partyName; }
            set { SetProperty(ref _partyName, value); }
        }
        public string OpeningBalance
        {
            get { return _openingBalance; }
            set { SetProperty(ref _openingBalance, value); }
        }
        public string SelectedKey
        {
            get { return _selectedKey; }
            set { SetProperty(ref _selectedKey, value); }
        }
        #endregion

        #region Constrcutor
        public OutStandingPayableDetailViewModel()
        {
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
            PrintPDFCommand = new Command(async () => await OnPrintClick());
            ShareCommand = new Command(async () => await OnShareClick());
            LoadMoreCommand = new Command(async () => await OnLoadMoreData());
        }
        #endregion

        #region Public Methods
        async public Task GetPayableDetails()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");

                    var payableDetailResponse = await ApiService.GetRequest<ReportDetailModel>($"{ApiPathString.GetPayableDetail}?OffsetFrom={_reportRequest.OffsetFrom}&OffsetTo={_reportRequest.OffsetTo}&AccountFilter={_reportRequest.AccountFilter}", null, null);
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();

                    if (payableDetailResponse == null)
                        return;

                    if (!payableDetailResponse.status)
                    {
                        Helper.DisplayAlert(payableDetailResponse.message);
                        return;
                    }
                    //if (payableDetailResponse.data.dtLedgerData == null)
                    //    return;

                    //var jsonData = JsonConvert.SerializeObject(payableDetailResponse.data.dtLedgerData);

                    DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(payableDetailResponse.data);
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
                        DataTableCollection = dataTable;
                        FilteredDataTableCollection = dataTable;
                        DuplicateDataTableCollection = dataTable;
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

        public void Search()
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
        #endregion

        #region Private Methods
        async private Task OnPrintClick()
        {
            _isFromPDF = true;
            await Helper.GetPDFFileFromData(fromDateTime: FromDate, toDateTime: ToDate, filterId: SelectedKey, reportId: Convert.ToInt32(ReportType.OutsrSinPayable));
        }
        async private Task OnShareClick()
        {
            _isFromPDF = true;
            await Helper.GetPDFFileFromData(fromDateTime: FromDate, toDateTime: ToDate, filterId: SelectedKey, reportId: Convert.ToInt32(ReportType.OutsrSinPayable), isFromShare: true);
        }
        async private Task OnLoadMoreData()
        {
            IsBusy = true;
            _reportRequest.OffsetFrom += 5;
            _reportRequest.OffsetTo += 5;
            await GetPayableDetails();
            IsBusy = false;
        }
        #endregion
    }
}
