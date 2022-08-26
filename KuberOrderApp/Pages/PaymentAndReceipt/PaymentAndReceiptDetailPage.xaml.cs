using System;
using System.Collections.Generic;
using KuberOrderApp.ViewModels.PaymentAndReceipt;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.PaymentAndReceipt
{
    public partial class PaymentAndReceiptDetailPage : ContentPage
    {
        #region ReadOnly Section
        private readonly PaymentAndReceiptDetailViewModel _paymentAndReceiptDetailViewModel;
        #endregion

        public PaymentAndReceiptDetailPage(string keyId)
        {
            InitializeComponent();
            BindingContext = _paymentAndReceiptDetailViewModel = new PaymentAndReceiptDetailViewModel();
            _paymentAndReceiptDetailViewModel.SelectedKey = keyId;
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            await _paymentAndReceiptDetailViewModel.GetPaymentAndReceiptDetailData();
            XmlDataGrid.Columns[0].IsHidden = true;
        }

        void Entry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
        }

        void BorderlessDatePicker_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
        }

        void XmlDataGrid_SelectionChanged(System.Object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
        }

        void TappedXmlGrid(System.Object sender, Syncfusion.SfDataGrid.XForms.GridTappedEventArgs e)
        {
        }
    }
}
