using System;
using System.Collections.Generic;
using System.Data;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.ViewModels.Ledger;
using KuberOrderApp.ViewModels.OutStandingPayable;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.OutStandingPayable
{
    public partial class OutStandingPayablePage : ContentPage
    {
        #region ReadOnly Section
        private readonly OutStandingPayableViewModel _outStandingPayableViewModel;
        #endregion

        public OutStandingPayablePage()
        {
            InitializeComponent();
            BindingContext = _outStandingPayableViewModel = new OutStandingPayableViewModel();
            GetuotStandingPayableViewModel();
        }
        public async void GetuotStandingPayableViewModel()
        {
            if (_outStandingPayableViewModel._isFromPDF)
            {
                _outStandingPayableViewModel._isFromPDF = false;
                return;
            }
            _outStandingPayableViewModel._reportRequest = new ReportRequest()
            {
                OffsetFrom = 1,
                OffsetTo = 10,
                ProductFilter = ""
            };
            await _outStandingPayableViewModel.GetOutStandingPayale();
            if (XmlDataGrid == null || XmlDataGrid.Columns == null || XmlDataGrid.Columns.Count == 0)
                return;

            XmlDataGrid.Columns[0].IsHidden = true;
        }
        /*async protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_outStandingPayableViewModel._isFromPDF)
            {
                _outStandingPayableViewModel._isFromPDF = false;
                return;
            }
            _outStandingPayableViewModel._reportRequest = new ReportRequest()
            {
                OffsetFrom = 1,
                OffsetTo = 10,
                ProductFilter = ""
            };
            await _outStandingPayableViewModel.GetOutStandingPayale();
            if (XmlDataGrid == null || XmlDataGrid.Columns == null || XmlDataGrid.Columns.Count == 0)
                return;

            XmlDataGrid.Columns[0].IsHidden = true;
        }*/

        void Entry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
           // _outStandingPayableViewModel.Search();
        }

        void BorderlessDatePicker_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            _outStandingPayableViewModel.GetFilterData();
        }

        async void TappedXmlGrid(System.Object sender, Syncfusion.SfDataGrid.XForms.GridTappedEventArgs e)
        {
            DataRowView rowData = e.RowData as DataRowView;
            if (rowData == null)
                return;

            string keyId = rowData.Row.ItemArray[0].ToString();
            await App.Current.MainPage.Navigation.PushAsync(new OutStandingPayableDetailPage(keyId));
        }

        protected override bool OnBackButtonPressed()
        {
            App.CheckAutoLogin();
            return true;
        }
    }
}