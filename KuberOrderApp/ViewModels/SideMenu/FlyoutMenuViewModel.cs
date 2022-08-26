using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using KuberOrderApp.Enums;
using KuberOrderApp.Extention;
using KuberOrderApp.Models.SideMenu;
using KuberOrderApp.Pages.AddressBook;
using KuberOrderApp.Pages.Authentication;
using KuberOrderApp.Pages.Home;
using KuberOrderApp.Pages.Ledger;
using KuberOrderApp.Pages.Orders;
using KuberOrderApp.Pages.OutStandingPayable;
using KuberOrderApp.Pages.PaymentAndReceipt;
using KuberOrderApp.Pages.Receivable;
using KuberOrderApp.Pages.SideMenu;
using KuberOrderApp.Pages.Stock;
using KuberOrderApp.Utilities;
using KuberOrderApp.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KuberOrderApp.ViewModels.SideMenu
{
    public class FlyoutMenuViewModel : BaseViewModel
    {
        #region ReadOnly Section
        private readonly MainPage _mainPage;
        #endregion

        #region Field Section
        private ObservableCollection<SideMenus> _menus;
        private SideMenus _selectedMenu;
        #endregion

        #region Properties
        public ObservableCollection<SideMenus> MenusList
        {
            get { return _menus; }
            set { SetProperty(ref _menus, value); }
        }
        public SideMenus SelectedMenu
        {
            get { return _selectedMenu; }
            set { SetProperty(ref _selectedMenu, value); }
        }
        #endregion

        #region Commands
        public ICommand MenuItemSelectedCommand { get; private set; }
        #endregion

        #region Constructor
        public FlyoutMenuViewModel(MainPage mainPage)
        {
            _mainPage = mainPage;
            App.mainPage = mainPage;
            MenuItemSelectedCommand = new Command(MenuItemSelected);
            SetMenus();
        }
        #endregion

        #region Private Methods
        private void SetMenus()
        {
            MenusList = new ObservableCollection<SideMenus>()
            {
                new SideMenus{ Title =  Menus.Home.GetDescription()},
                new SideMenus{Icon = "ic_outstanding", Title =  Menus.OutstandingReceivable.GetDescription()},
                new SideMenus{Icon = "ic_outstanding",  Title =  Menus.OutStandingPayable.GetDescription()},
                new SideMenus{Icon = "ic_receipt_entry", Title =  Menus.ReceiptPaymentAdd.GetDescription()},
                new SideMenus{Icon = "ic_receipt_entry", Title =  Menus.ReceiptPaymentDisplay.GetDescription()},
                new SideMenus{Icon = "ic_order_entry", Title =  Menus.OrderAdd.GetDescription()},
                new SideMenus{Icon = "ic_order_entry", Title =  Menus.OrderDisplay.GetDescription()},
                new SideMenus{Icon = "ic_purchase", Title =  Menus.Cart.GetDescription()},
                new SideMenus{Icon = "ic_order_report", Title =  Menus.OrderReport.GetDescription()},
                new SideMenus{Icon = "ic_ledger_book", Title =  Menus.Ledger.GetDescription()},
                new SideMenus{Icon = "ic_stock_report", Title =  Menus.Stock.GetDescription()},
                new SideMenus{Icon = "ic_address_book", Title =  Menus.AddressBook.GetDescription()},
                new SideMenus{Icon = "ic_share", Title =  Menus.ShareApplication.GetDescription()},
                new SideMenus{Icon = "ic_feedback", Title =  Menus.Feedback.GetDescription()},
                new SideMenus{Icon = "ic_contact", Title =  Menus.ContactUs.GetDescription()},
                new SideMenus{Icon = "ic_signout", Title =  Menus.LogOut.GetDescription()},
            };
        }

        async private void MenuItemSelected(object obj)
        {
            if (SelectedMenu == null)
                return;

            if (SelectedMenu.Title == Menus.Home.GetDescription())
                _mainPage.Detail = new NavigationPage(new DashboardPage());
            else if (SelectedMenu.Title == Menus.OrderAdd.GetDescription())
                _mainPage.Detail = new NavigationPage(new OrderListPage());
            else if (SelectedMenu.Title == Menus.OrderDisplay.GetDescription())
                _mainPage.Detail = new NavigationPage(new DisplayOrderPage());
            else if (SelectedMenu.Title == Menus.Cart.GetDescription())
                _mainPage.Detail = new NavigationPage(new CartOrderPage());
            else if (SelectedMenu.Title == Menus.OutstandingReceivable.GetDescription())
                _mainPage.Detail = new NavigationPage(new ReceivablePage());
            else if (SelectedMenu.Title == Menus.ReceiptPaymentAdd.GetDescription())
                _mainPage.Detail = new NavigationPage(new AddPaymentReceiptPage());
            else if (SelectedMenu.Title == Menus.ReceiptPaymentDisplay.GetDescription())
                _mainPage.Detail = new NavigationPage(new DisplayPaymentAndReceiptPage());
            else if (SelectedMenu.Title == Menus.OrderReport.GetDescription())
                _mainPage.Detail = new NavigationPage(new OrderReportPage());
            else if (SelectedMenu.Title == Menus.Ledger.GetDescription())
                _mainPage.Detail = new NavigationPage(new LedgerPage());
            else if (SelectedMenu.Title == Menus.Stock.GetDescription())
                _mainPage.Detail = new NavigationPage(new StockPage());
            else if (SelectedMenu.Title == Menus.OutStandingPayable.GetDescription())
                _mainPage.Detail = new NavigationPage(new OutStandingPayablePage());
            else if (SelectedMenu.Title == Menus.AddressBook.GetDescription())
                _mainPage.Detail = new NavigationPage(new AddressListPage());
            else if (SelectedMenu.Title == Menus.Feedback.GetDescription())
            {
                try
                {
                    await Browser.OpenAsync("https://kubererp.com/feedback", BrowserLaunchMode.SystemPreferred);
                }
                catch (Exception ex)
                {
                    // An unexpected error occured. No browser may be installed on the device.
                }
            }
            else if (SelectedMenu.Title == Menus.ContactUs.GetDescription())
            {
                try
                {
                    await Browser.OpenAsync("https://kubererp.com/contact-us", BrowserLaunchMode.SystemPreferred);
                }
                catch (Exception ex)
                {
                    // An unexpected error occured. No browser may be installed on the device.
                }
            }
            else if (SelectedMenu.Title == Menus.LogOut.GetDescription())
            {
                await App.Database.DropTableAndCreateNew();
                Helper.SavePreference("LoginResponse", "");
                Helper.SavePreference("IsLoginComplete", false);
                App.Current.MainPage = new NavigationPage(new LoginPage());
            }

            _mainPage.IsPresented = false;
            SelectedMenu = null;
        }
        #endregion
    }
}
