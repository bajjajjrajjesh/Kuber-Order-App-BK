using System;
using System.Collections.Generic;
using System.Data;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.ViewModels.Stock;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.Stock
{
    public partial class StockPage : ContentPage
    {
        #region ReadOnly Section
        private readonly StockViewModel _stockViewModel;
        #endregion

        public StockPage()
        {
            InitializeComponent();
            BindingContext = _stockViewModel = new StockViewModel();
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_stockViewModel._isFromPDF)
            {
                _stockViewModel._isFromPDF = false;
                return;
            }
            _stockViewModel._reportRequest = new ReportRequest()
            {
                OffsetFrom = 1,
                OffsetTo = 10,
                ProductFilter = ""
            };
            _stockViewModel.DataTableCollection = null;
            await _stockViewModel.GetStock();
            if (XmlDataGrid.Columns == null || XmlDataGrid.Columns.Count == 0)
                return;

            XmlDataGrid.Columns[0].IsHidden = true;
        }

        void Entry_Focused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            Entry entry = (Entry)sender;
            entry.Unfocus();
            _stockViewModel.NavigateToCommonFilterPage(entry.StyleId);
        }

        void Entry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
           // _stockViewModel.Search();
        }

        void BorderlessDatePicker_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            _stockViewModel.GetFilterData();
        }

        async void TappedXmlGrid(System.Object sender, Syncfusion.SfDataGrid.XForms.GridTappedEventArgs e)
        {
            DataRowView rowData = e.RowData as DataRowView;
            if (rowData == null)
                return;

            string keyId = rowData.Row.ItemArray[0].ToString();
            await App.Current.MainPage.Navigation.PushAsync(new StockDetailPage(keyId));
        }

        protected override bool OnBackButtonPressed()
        {
            App.CheckAutoLogin();
            return true;
        }
    }
}
