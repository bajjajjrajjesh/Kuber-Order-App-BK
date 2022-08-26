using System;
using System.Collections.Generic;
using System.Data;
using KuberOrderApp.ViewModels.Orders;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.Orders
{
    public partial class DisplayOrderPage : ContentPage
    {
        #region ReadOnly Section
        private readonly DisplayOrderViewModel _displayOrderViewModel;
        #endregion

        public DisplayOrderPage()
        {
            InitializeComponent();
            BindingContext = _displayOrderViewModel = new DisplayOrderViewModel();
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_displayOrderViewModel._isFromPDF)
            {
                _displayOrderViewModel._isFromPDF = false;
                return;
            }
            await _displayOrderViewModel.GetOrderedList();

            if (XmlDataGrid.Columns == null || XmlDataGrid.Columns.Count == 0)
                return;

            XmlDataGrid.Columns[0].IsHidden = true;
        }

        void TappedXmlGrid(System.Object sender, Syncfusion.SfDataGrid.XForms.GridTappedEventArgs e)
        {
        }

        void Entry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            //_displayOrderViewModel.Search();
        }

        void BorderlessDatePicker_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            _displayOrderViewModel.GetFilterData();
        }

        async void TapGestureRecognizer_Print_Tapped(System.Object sender, System.EventArgs e)
        {
            Image image = (Image)sender;
            if (image == null)
                return;

            DataRowView rowData = image.BindingContext as DataRowView;
            if (rowData == null)
                return;

            string keyId = rowData.Row.ItemArray[0].ToString();

            await _displayOrderViewModel.OnPrintClick(keyId);
        }

        async void TapGestureRecognizer_Share_Tapped(System.Object sender, System.EventArgs e)
        {
            Image image = (Image)sender;
            if (image == null)
                return;

            DataRowView rowData = image.BindingContext as DataRowView;
            if (rowData == null)
                return;

            string keyId = rowData.Row.ItemArray[0].ToString();

            await _displayOrderViewModel.OnShareClick(keyId);
        }

        protected override bool OnBackButtonPressed()
        {
            App.CheckAutoLogin();
            return true;
        }
    }
}
