using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using KuberOrderApp.APIAndServices;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.Models.ResponseModels;
using KuberOrderApp.Pages.CommonPages;
using KuberOrderApp.Resources;
using KuberOrderApp.Utilities;
using KuberOrderApp.ViewModels.Base;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KuberOrderApp.ViewModels.PaymentAndReceipt
{
    public class AddPaymentReceiptViewModel : BaseViewModel
    {
        #region Field Section.
        private int _voucherNo;
        private string _challanNo;
        private DateTime _voucherDate;
        private DateTime _challanDate;
        private PartyList _selectedPaymentOption;
        private string _selectedPaymentPartyName;
        private PartyList _selectedAccountName;
        private string _selectedPartyAccountName;
        private string _accountName;
        private double _amount;
        private string _remarks;
        #endregion

        #region Properties
        public int VoucherNo
        {
            get { return _voucherNo; }
            set { SetProperty(ref _voucherNo, value); }
        }
        public string ChallanNo
        {
            get { return _challanNo; }
            set { SetProperty(ref _challanNo, value); }
        }
        public string SelectedPaymentPartyName
        {
            get { return _selectedPaymentPartyName; }
            set { SetProperty(ref _selectedPaymentPartyName, value); }
        }
        public string SelectedPartyAccountName
        {
            get { return _selectedPartyAccountName; }
            set { SetProperty(ref _selectedPartyAccountName, value); }
        }
        public DateTime ChallanDate
        {
            get { return _challanDate; }
            set { SetProperty(ref _challanDate, value); }
        }
        public DateTime VoucherDate
        {
            get { return _voucherDate; }
            set { SetProperty(ref _voucherDate, value); }
        }
        public PartyList SelectedPaymentOption
        {
            get { return _selectedPaymentOption; }
            set { SetProperty(ref _selectedPaymentOption, value); }
        }
        public PartyList SelectedAccountName
        {
            get { return _selectedAccountName; }
            set { SetProperty(ref _selectedAccountName, value); }
        }
        public string AccountName
        {
            get { return _accountName; }
            set { SetProperty(ref _accountName, value); }
        }
        public double Amount
        {
            get { return _amount; }
            set { SetProperty(ref _amount, value); }
        }
        public string Remarks
        {
            get { return _remarks; }
            set { SetProperty(ref _remarks, value); }
        }
        #endregion

        #region Commands
        public ICommand SaveCommand { protected set; get; }
        public ICommand SaveAndPrintCommand { protected set; get; }
        public ICommand SelectionAccountCommand { protected set; get; }
        public ICommand SelectionPaymentCommand { protected set; get; }
        #endregion

        #region Constructor
        public AddPaymentReceiptViewModel()
        {
            PrintPDFCommand = new Command(async () => await OnPrintClick());
            ShareCommand = new Command(async () => await OnShareClick());
            SaveCommand = new Command(async () => await OnSaveClick());
            SaveAndPrintCommand = new Command(async () => await OnSaveAndPrintClick());
            SelectionAccountCommand = new Command( () => NavigateToCommonSearchPage(true));
            SelectionPaymentCommand = new Command( () => NavigateToCommonSearchPage(false));
            VoucherDate = DateTime.Now;
            ChallanDate = DateTime.Now;


            MessagingCenter.Subscribe<string, PartyList>(this, "AddPaymentPartyList", async (sender, arg) =>
            {
                SelectedPaymentOption = arg;
                SelectedPaymentPartyName = SelectedPaymentOption.ColName;
            });

            MessagingCenter.Subscribe<string, PartyList>(this, "AddAccountPartyList", async (sender, arg) =>
            {
                SelectedAccountName = arg;
                SelectedPartyAccountName = SelectedAccountName.ColName;
            });
        }
        #endregion

        #region Public Methods
        async public void NavigateToCommonSearchPage(bool isAccountName)
        {
            if (isAccountName)
                await App.Current.MainPage.Navigation.PushAsync(new CommonSearchPage(isFromAddAccount: true));
            else
                await App.Current.MainPage.Navigation.PushAsync(new CommonSearchPage(isFromAddPayment: true));
        }
        #endregion

        #region Private Methods
        async private Task OnPrintClick()
        {
            
        }
        async private Task OnShareClick()
        {

        }
        async private Task OnSaveClick()
        {
            if (!await CheckValidation())
                return;

            await AddPaymentReceiptAPI();
            //App.AddPaymentAndReceiptRequestsList.Add(addPaymentAndReceiptRequest);
        }
        async private Task OnSaveAndPrintClick()
        {
            if (!await CheckValidation())
                return;
        }

        async private Task AddPaymentReceiptAPI()
        {
            AddPaymentAndReceiptRequest addPaymentAndReceiptRequest = new AddPaymentAndReceiptRequest()
            {
                Col90 = SelectedPaymentOption.ColName.Substring(0, 1),
                Col96 = SelectedPaymentOption.ColPK,
                Col95 = SelectedAccountName.ColPK,
                Col98 = VoucherDate.ToString("dd/MM/yyyy"),
                Col91 = ChallanNo,
                Col92 = ChallanDate.ToString("dd/MM/yyyy"),
                Col94 = Amount,
                Col50 = Remarks,
                Col97 = "",
                Col99 = "",
            };

            try
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var loginResponse = await ApiService.PostRequest<CommonResponseModel>(ApiPathString.SaveReceiptPayment, addPaymentAndReceiptRequest, null);
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();

                    if (loginResponse == null)
                        return;

                    if (!loginResponse.status)
                    {
                        Helper.DisplayAlert(loginResponse.message);
                        return;
                    }
                    Helper.DisplayAlert(loginResponse.message);
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async() =>
                    {
                        List<AddPaymentAndReceiptRequest> paymentAndReceiptRequests = new List<AddPaymentAndReceiptRequest>();
                        paymentAndReceiptRequests.Add(addPaymentAndReceiptRequest);
                        await App.Database.Insert<AddPaymentAndReceiptRequest>(paymentAndReceiptRequests);
                        Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                    });
                }

                VoucherNo = 0;
                VoucherDate = DateTime.Now;
                ChallanDate = DateTime.Now;

                AccountName = "";
                SelectedPaymentOption = null;
                SelectedAccountName = null;
                ChallanNo = "";
                Remarks = "";
                Amount = 0;
                Remarks = "";

                Helper.DisplayAlert("Entry Saved");
            }
            catch (Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
        }

        async private Task<bool> CheckValidation()
        {
            if (SelectedPaymentOption == null)
            {
                Helper.DisplayAlert(TextString.BlankPaymentOption);
                return false;
            }
            else if (SelectedAccountName == null)
            {
                Helper.DisplayAlert(TextString.BlankReceiptOption);
                return false;
            }
            else if (Amount <= 0)
            {
                Helper.DisplayAlert(TextString.BlankAmount);
                return false;
            }
            //else if (string.IsNullOrWhiteSpace(Remarks))
            //{
            //    Helper.DisplayAlert(TextString.BlankRemarks);
            //    return false;
            //}

            return true;
        }
        #endregion
    }
}
