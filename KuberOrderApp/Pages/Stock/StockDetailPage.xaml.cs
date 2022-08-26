using System;
using System.Collections.Generic;
using System.Data;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.ViewModels.Stock;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.Stock
{
    public partial class StockDetailPage : ContentPage
    {
        #region ReadOnly Section
        private readonly StockDetailViewModel _stockDetailViewModel;
        #endregion

        public StockDetailPage(string selectedKeyId)
        {
            InitializeComponent();
            BindingContext = _stockDetailViewModel = new StockDetailViewModel();
            _stockDetailViewModel.SelectedKey = selectedKeyId;
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_stockDetailViewModel._isFromPDF)
            {
                _stockDetailViewModel._isFromPDF = false;
                return;
            }
            _stockDetailViewModel._reportRequest = new ReportRequest()
            {
                OffsetFrom = 1,
                OffsetTo = 10,
                ProductFilter = _stockDetailViewModel.SelectedKey
            };
            await _stockDetailViewModel.GetStockDetail();
            if (XmlDataGrid.Columns == null || XmlDataGrid.Columns.Count == 0)
                return;

            XmlDataGrid.Columns[0].IsHidden = true;
        }

        void Entry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            _stockDetailViewModel.Search();
        }

        void BorderlessDatePicker_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            _stockDetailViewModel.GetFilterData();
        }

        async void TappedXmlGrid(System.Object sender, Syncfusion.SfDataGrid.XForms.GridTappedEventArgs e)
        {
            
        }

        void XmlDataGrid_SelectionChanged(System.Object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
        }
    }
}
