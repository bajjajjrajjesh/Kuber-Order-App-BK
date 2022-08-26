using System;
using System.Threading.Tasks;
using System.Windows.Input;
using KuberOrderApp.Enums;
using KuberOrderApp.Models.ResponseModels;
using KuberOrderApp.Utilities;
using KuberOrderApp.ViewModels.Base;
using Xamarin.Forms;

namespace KuberOrderApp.ViewModels.AddressBook
{
    public class AddressDetailViewModel : BaseViewModel
    {
        #region Field Section.
        private PartyList _selectedAddress;
        private string _partyName;
        private string _mobileNo;
        private string _phoneNo;
        private string _address1;
        private string _address2;
        private string _address3;
        private string _city;
        private string _area;
        private string _state;
        private string _contactPerson;
        #endregion

        #region Properties
        public bool _isFromPDF = false;

        public PartyList SelectedParty
        {
            get { return _selectedAddress; }
            set { SetProperty(ref _selectedAddress, value); }
        }
        public string PartyName
        {
            get { return _partyName; }
            set { SetProperty(ref _partyName, value); }
        }
        public string MobileNo
        {
            get { return _mobileNo; }
            set { SetProperty(ref _mobileNo, value); }
        }
        public string PhoneNo
        {
            get { return _phoneNo; }
            set { SetProperty(ref _phoneNo, value); }
        }
        public string Address1
        {
            get { return _address1; }
            set { SetProperty(ref _address1, value); }
        }
        public string Address2
        {
            get { return _address2; }
            set { SetProperty(ref _address2, value); }
        }
        public string Address3
        {
            get { return _address3; }
            set { SetProperty(ref _address3, value); }
        }
        public string City
        {
            get { return _city; }
            set { SetProperty(ref _city, value); }
        }
        public string Area
        {
            get { return _area; }
            set { SetProperty(ref _area, value); }
        }
        public string State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }
        public string ContactPerson
        {
            get { return _contactPerson; }
            set { SetProperty(ref _contactPerson, value); }
        }
        #endregion

        #region Commands
        public ICommand PrintCommand { get; private set; }
        #endregion

        public AddressDetailViewModel()
        {
            PrintPDFCommand = new Command(async () => await OnPrintClick());
            ShareCommand = new Command(async () => await OnShareClick());
        }

        #region Public Methods
        public void SetAddressDetails()
        {
            if (SelectedParty == null)
                return;

            PartyName = SelectedParty.ColName;
            MobileNo = SelectedParty.ColPhoneO;
            PhoneNo = SelectedParty.ColPhoneR;
            Address1 = SelectedParty.ColAdd1;
            Address2 = SelectedParty.ColAdd2;
            Address3 = SelectedParty.ColAdd3;
            City = SelectedParty.ColCityName;
            State = SelectedParty.ColStateName;
            ContactPerson = SelectedParty.ColContectPerson;
            Area = SelectedParty.ColAreaName;


        }
        #endregion

        #region Private Methods
        async private Task OnPrintClick()
        {
            _isFromPDF = true;
            await Helper.GetPDFFileFromData(reportId: Convert.ToInt32(ReportType.AddressBookSingle), filterId: SelectedParty.ColPK.ToString());
        }

        async private Task OnShareClick()
        {
            _isFromPDF = true;
            await Helper.GetPDFFileFromData(reportId: Convert.ToInt32(ReportType.AddressBookSingle), filterId: SelectedParty.ColPK.ToString(), isFromShare: true);
        }
        #endregion
    }
}
