using System;
using System.Collections.Generic;
using KuberOrderApp.ViewModels.Common;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.CommonPages
{
    public partial class CommonSearchPage : ContentPage
    {
        #region ReadOnly Section
        private readonly CommonSearchViewModel _commonSearchViewModel;
        #endregion

        public CommonSearchPage(bool isFromDisplayReceipt = false,
            bool isFromAddAccount = false,
            bool isFromAddPayment = false,
                bool isFromAddOrder = false)
        {
            InitializeComponent();
            BindingContext = _commonSearchViewModel = new CommonSearchViewModel();
            _commonSearchViewModel.IsFromDisplayReceipt = isFromDisplayReceipt;
            _commonSearchViewModel.IsFromAddAccount = isFromAddAccount;
            _commonSearchViewModel.IsFromAddPayment = isFromAddPayment;
            _commonSearchViewModel.IsFromAddOrder = isFromAddOrder;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _commonSearchViewModel.GetPartyList();

        }

        void Entry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            _commonSearchViewModel.SearchParty();
        }
    }
}
