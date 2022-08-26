using System;
using System.Collections.Generic;
using KuberOrderApp.ViewModels.AddressBook;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.AddressBook
{
    public partial class AddressListPage : ContentPage
    {
        #region ReadOnly Section
        private readonly AddressListViewModel _addressListViewModel;
        #endregion

        public AddressListPage()
        {
            InitializeComponent();
            BindingContext = _addressListViewModel = new AddressListViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_addressListViewModel._isFromPDF)
            {
                _addressListViewModel._isFromPDF = false;
                return;
            }
            _addressListViewModel.GetPartyList();
        }

        void Entry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            _addressListViewModel.SearchParty();
        }

        protected override bool OnBackButtonPressed()
        {
            App.CheckAutoLogin();
            return true;
        }
    }
}
