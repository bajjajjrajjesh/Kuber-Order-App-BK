using System;
using System.Collections.Generic;
using System.Data;
using KuberOrderApp.ViewModels.PaymentAndReceipt;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.PaymentAndReceipt
{
    public partial class DisplayPaymentAndReceiptPage : ContentPage
    {
        #region ReadOnly Section
        private readonly DisplayPaymentAndReceiptViewModel _displayPaymentAndReceiptViewModel;
        #endregion
        public DisplayPaymentAndReceiptPage()
        {
            InitializeComponent();
            BindingContext = _displayPaymentAndReceiptViewModel = new DisplayPaymentAndReceiptViewModel();
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_displayPaymentAndReceiptViewModel._isFromPDF)
            {
                _displayPaymentAndReceiptViewModel._isFromPDF = false;
                return;
            }
            await _displayPaymentAndReceiptViewModel.GetPaymentAndReceiptData();
            if (XmlDataGrid.Columns == null || XmlDataGrid.Columns.Count == 0)
                return;

            XmlDataGrid.Columns[0].IsHidden = true;
        }

        async void TappedXmlGrid(System.Object sender, Syncfusion.SfDataGrid.XForms.GridTappedEventArgs e)
        {
            DataRowView rowData = e.RowData as DataRowView;
            if (rowData == null)
                return;

            string keyId = rowData.Row.ItemArray[0].ToString();
            await App.Current.MainPage.Navigation.PushAsync(new PaymentAndReceiptDetailPage(keyId));
        }

        void Entry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
          //  _displayPaymentAndReceiptViewModel.Search();
        }

        void BorderlessDatePicker_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            _displayPaymentAndReceiptViewModel.GetFilterData();
        }

        void XmlDataGrid_SelectionChanged(System.Object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
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

            await _displayPaymentAndReceiptViewModel.OnPrintClick(keyId);
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

            await _displayPaymentAndReceiptViewModel.OnShareClick(keyId);
        }

        protected override bool OnBackButtonPressed()
        {
            App.CheckAutoLogin();
            return true;
        }
    }
}
