using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using KuberOrderApp.Enums;
using KuberOrderApp.Models.ResponseModels;
using KuberOrderApp.Pages.AddressBook;
using KuberOrderApp.Utilities;
using KuberOrderApp.ViewModels.Base;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace KuberOrderApp.ViewModels.AddressBook
{
    public class AddressListViewModel : BaseViewModel
    {
        #region Field Section.
        private string _searchAddress;
        private List<PartyList> _partyLists;
        private List<PartyList> _duplicatePartyLists;
        private PartyList _selectedParty;
        #endregion

        #region Properties
        public bool _isFromPDF = false;

        public string SearchAddress
        {
            get { return _searchAddress; }
            set { SetProperty(ref _searchAddress, value); }
        }
        public List<PartyList> PartyLists
        {
            get { return _partyLists; }
            set { SetProperty(ref _partyLists, value); }
        }
        public PartyList SelectedParty
        {
            get { return _selectedParty; }
            set { SetProperty(ref _selectedParty, value); }
        }
        #endregion

        #region Commands
        public ICommand ItemSelectedCommand { get; private set; }
        #endregion

        #region Constructor
        public AddressListViewModel()
        {
            ItemSelectedCommand = new Command(OnItemSelected);
            PrintPDFCommand = new Command(async () => await OnPrintClick());
            ShareCommand = new Command(async () => await OnShareClick());
        }
        #endregion

        #region Public Methods
        async public void GetPartyList()
        {
            try
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");
                _duplicatePartyLists = new List<PartyList>();
                var partyList = await App.Database.GetDataList<PartyList>();
                if (partyList != null && partyList.Count > 0)
                {
                    PartyLists = new List<PartyList>(partyList.Where(x => x.ColAccType.ToLower() == "sc" || x.ColAccType.ToLower() == "sd").ToList());
                    _duplicatePartyLists = new List<PartyList>(partyList);
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
            if (string.IsNullOrWhiteSpace(SearchAddress))
            {
                PartyLists = new List<PartyList>(_duplicatePartyLists);
                return;
            }

            if (_duplicatePartyLists != null && _duplicatePartyLists.Count > 0)
                PartyLists = new List<PartyList>(_duplicatePartyLists.Where(x => (!string.IsNullOrEmpty(x.ColName) && x.ColName.ToLower().Contains(SearchAddress.ToLower()))));
        }
        #endregion



        #region Private Methods
        async private Task OnPrintClick()
        {
            _isFromPDF = true;
            await Helper.GetPDFFileFromData(reportId: Convert.ToInt32(ReportType.AddressBookAll));
        }

        async private Task OnShareClick()
        {
            _isFromPDF = true;
            await Helper.GetPDFFileFromData(reportId: Convert.ToInt32(ReportType.AddressBookAll), isFromShare: true);
        }

        async private void OnItemSelected(object obj)
        {
            if (SelectedParty == null)
                return;

            await App.Current.MainPage.Navigation.PushAsync(new AddressDetailPage(SelectedParty));

            SelectedParty = null;
        }
        #endregion
    }
}
