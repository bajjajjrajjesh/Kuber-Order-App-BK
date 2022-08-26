using System;
using System.Collections.Generic;
using KuberOrderApp.ViewModels.PaymentAndReceipt;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.PaymentAndReceipt
{
    public partial class AddPaymentReceiptPage : ContentPage
    {
        #region ReadOnly Section
        private readonly AddPaymentReceiptViewModel _addPaymentReceiptViewModel;
        #endregion

        public AddPaymentReceiptPage()
        {
            InitializeComponent();
            BindingContext = _addPaymentReceiptViewModel = new AddPaymentReceiptViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        void CashBank_Entry_Focused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            Entry entry = (Entry)sender;
            entry.Unfocus();
            _addPaymentReceiptViewModel.NavigateToCommonSearchPage(false);
        }

        void Account_Entry_Focused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            Entry entry = (Entry)sender;
            entry.Unfocus();
            _addPaymentReceiptViewModel.NavigateToCommonSearchPage(true);
        }

        protected override bool OnBackButtonPressed()
        {
            App.CheckAutoLogin();
            return true;
        }
    }
}
