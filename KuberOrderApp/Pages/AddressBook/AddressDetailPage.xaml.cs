using System;
using System.Collections.Generic;
using KuberOrderApp.Models.ResponseModels;
using KuberOrderApp.ViewModels.AddressBook;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.AddressBook
{
    public partial class AddressDetailPage : ContentPage
    {
        #region ReadOnly Section
        private readonly AddressDetailViewModel _addressDetailViewModel;
        #endregion

        public AddressDetailPage(PartyList selectedParty)
        {
            InitializeComponent();
            BindingContext = _addressDetailViewModel = new AddressDetailViewModel();
            _addressDetailViewModel.SelectedParty = selectedParty;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_addressDetailViewModel._isFromPDF)
            {
                _addressDetailViewModel._isFromPDF = false;
                return;
            }
            _addressDetailViewModel.SetAddressDetails();
        }
    }
}
