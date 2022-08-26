using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using KuberOrderApp.Models.ResponseModels;
using KuberOrderApp.Pages.SideMenu;
using KuberOrderApp.Resources;
using KuberOrderApp.ViewModels.Base;
using Xamarin.Forms;

namespace KuberOrderApp.ViewModels.Common
{
    public class CommonSearchViewModel : BaseViewModel
    {
        #region Field Section
        private bool _isFromDisplayReceipt;
        private bool _isFromAddAccount;
        private bool _isFromAddPayment;
        private bool _isFromAddOrder;
        private List<PartyList> _partyLists;
        private List<PartyList> _duplicateLists;
        private PartyList _selectedParty;
        private string _searchRecord;
        private string _title;
        #endregion

        #region Properties
        public bool IsFromDisplayReceipt
        {
            get { return _isFromDisplayReceipt; }
            set { SetProperty(ref _isFromDisplayReceipt, value); }
        }
        public bool IsFromAddAccount
        {
            get { return _isFromAddAccount; }
            set { SetProperty(ref _isFromAddAccount, value); }
        }
        public bool IsFromAddPayment
        {
            get { return _isFromAddPayment; }
            set { SetProperty(ref _isFromAddPayment, value); }
        }
        public bool IsFromAddOrder
        {
            get { return _isFromAddOrder; }
            set { SetProperty(ref _isFromAddOrder, value); }
        }
        public List<PartyList> PartyLists
        {
            get { return _partyLists; }
            set { SetProperty(ref _partyLists, value); }
        }
        public List<PartyList> DuplicateLists
        {
            get { return _duplicateLists; }
            set { SetProperty(ref _duplicateLists, value); }
        }
        public PartyList SelectedParty
        {
            get { return _selectedParty; }
            set { SetProperty(ref _selectedParty, value); }
        }
        public string SearchRecord
        {
            get { return _searchRecord; }
            set { SetProperty(ref _searchRecord, value); }
        }
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        #endregion

        #region Commands
        public ICommand ItemSelectedCommand { protected set; get; }
        #endregion

        #region Constructor
        public CommonSearchViewModel()
        {
            ItemSelectedCommand = new Command(() => OnSelected());
        }
        #endregion

        #region Public Methods
        async public void GetPartyList()
        {
            try
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");
                var partyList = await App.Database.GetDataList<PartyList>();
                if (partyList != null && partyList.Count > 0)
                {
                    if (IsFromDisplayReceipt || IsFromAddPayment)
                    {
                        Title = TextString.PaymentOption;
                        PartyLists = new List<PartyList>(partyList.Where(x => x.ColAccType == "BA" || x.ColAccType == "BO" || x.ColAccType == "CS").ToList());
                    }
                    else
                    {
                        Title = TextString.AccountName;
                        PartyLists = new List<PartyList>(partyList.Where(x => x.ColAccType.ToLower() == "sc" || x.ColAccType.ToLower() == "sd").ToList());
                    }

                    DuplicateLists = PartyLists;
                }
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
        }

        public void SearchParty()
        {
            if (string.IsNullOrWhiteSpace(SearchRecord))
            {
                PartyLists = new List<PartyList>(_duplicateLists);
                return;
            }

            if (_duplicateLists != null && _duplicateLists.Count > 0)
                PartyLists = new List<PartyList>(_duplicateLists.Where(x => (!string.IsNullOrEmpty(x.ColName) && x.ColName.ToLower().Contains(SearchRecord.ToLower()))));
        }
        #endregion

        #region Private Methods
        async private void OnSelected()
        {
            if (SelectedParty == null)
                return;

            if (IsFromDisplayReceipt)
                MessagingCenter.Send<string, PartyList>("True", "DisplayReceiptPartyList", SelectedParty);
            else if (IsFromAddPayment)
                MessagingCenter.Send<string, PartyList>("True", "AddPaymentPartyList", SelectedParty);
            else if (IsFromAddAccount)
                MessagingCenter.Send<string, PartyList>("True", "AddAccountPartyList", SelectedParty);
            else if (IsFromAddOrder)
                MessagingCenter.Send<string, PartyList>("True", "AddOrderAccountList", SelectedParty);

            await App.Current.MainPage.Navigation.PopAsync();
            SelectedParty = null;
        }
        #endregion
    }
}
