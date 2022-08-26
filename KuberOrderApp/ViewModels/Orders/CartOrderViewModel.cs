using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using KuberOrderApp.APIAndServices;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.Models.ResponseModels;
using KuberOrderApp.Pages.Home;
using KuberOrderApp.Pages.Orders;
using KuberOrderApp.Resources;
using KuberOrderApp.Utilities;
using KuberOrderApp.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KuberOrderApp.ViewModels.Orders
{
    public class CartOrderViewModel : BaseViewModel
    {
        #region Field Section
        private ObservableCollection<ProductCart> _productCartList;
        private ObservableCollection<ProductCart> _duplicateProductCartList;
        private string _searchRecord;
        private double _total;
        private string _accountName;
        private ProductCart _selectedProduct;
        #endregion

        #region Properties
        public ProductCart SelectedProduct
        {
            get
            {
                return _selectedProduct = null;
            }
            set
            {
                SetProperty(ref _selectedProduct, value);
            }
        }
        public string AccountName
        {
            get { return _accountName; }
            set { SetProperty(ref _accountName, value); }
        }
        public ObservableCollection<ProductCart> ProductCartList
        {
            get { return _productCartList; }
            set { SetProperty(ref _productCartList, value); }
        }
        public ObservableCollection<ProductCart> DuplicateProductCartList
        {
            get { return _duplicateProductCartList; }
            set { SetProperty(ref _duplicateProductCartList, value); }
        }
        public string SearchRecord
        {
            get { return _searchRecord; }
            set { SetProperty(ref _searchRecord, value); }
        }
        public double Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }
        #endregion

        #region Commands
        public ICommand ConfirmOrderCommand { private set; get; }
        public ICommand SearchCommand { private set; get; }
        #endregion

        #region Constructor
        public CartOrderViewModel()
        {
            ConfirmOrderCommand = new Command(async () => await PlaceProductOrder());
            SearchCommand = new Command(async () => await SearchProduct());
        }
        #endregion

        #region Public Methods
        async public Task GetCartList()
        {
            try
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");
                var cartlist = await App.Database.GetDataList<ProductCart>();
                if (cartlist == null && cartlist.Count == 0)
                    return;

                ProductCartList = new ObservableCollection<ProductCart>(cartlist);
                DuplicateProductCartList = ProductCartList;
                Total = ProductCartList.Sum(x => x.ColMRP);
                AccountName = ProductCartList.Select(x => x.AccountColName).FirstOrDefault();
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
        }

        async public Task DeleteProductFromCart(ProductCart product)
        {
            try
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");
                //List<ProductCart> productCarts = new List<ProductCart>();
                //productCarts.Add(product);
                await App.Database.DeleteData<ProductCart>(product);
                ProductCartList.Remove(product);
                DuplicateProductCartList.Remove(product);
                Total = DuplicateProductCartList.Sum(x => x.ColMRP);
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
            catch(Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
        }

        async public Task SearchProduct()
        {
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");
            if (string.IsNullOrWhiteSpace(SearchRecord))
            {
                ProductCartList = DuplicateProductCartList;
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                return;
            }

            ProductCartList = new ObservableCollection<ProductCart>(DuplicateProductCartList.Where(x => (!string.IsNullOrEmpty(x.ColName) && x.ColName.ToLower().Contains(SearchRecord.ToLower()))).ToList());
            Acr.UserDialogs.UserDialogs.Instance.HideLoading();
        }
        #endregion

        #region Private Methods
        async private Task PlaceProductOrder()
        {
            if (DuplicateProductCartList == null || DuplicateProductCartList.Count == 0)
            {
                Helper.DisplayAlert("Please add product");
                return;
            }

            List<SaveProductOrder> cartProductList = new List<SaveProductOrder>();
            foreach (var product in DuplicateProductCartList)
            {
                SaveProductOrder saveProductOrder = new SaveProductOrder()
                {
                    Col79 = product.AccountColPK,
                    Col96 = product.ColPK,
                    Col95 = product.ColOrderedQty,
                    Col94 = product.ColSaleRate,
                    Col93 = product.ColMRP,
                    Col50 = "",
                    Col99 = "",
                    ColID = "",
                };
                cartProductList.Add(saveProductOrder);
            }

            try
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var orderPlaceResponse = await ApiService.PostRequest<CommonResponseModel>(ApiPathString.PlaceOrder, cartProductList, null);
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();

                    if (orderPlaceResponse == null)
                        return;

                    if (!orderPlaceResponse.status)
                    {
                        Helper.DisplayAlert(orderPlaceResponse.message);
                        return;
                    }
                    Helper.DisplayAlert(orderPlaceResponse.message);
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async() =>
                    {
                        await App.Database.Insert<SaveProductOrder>(cartProductList);
                        Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                    });
                }

                foreach (var product in DuplicateProductCartList)
                    await App.Database.DeleteData<ProductCart>(product);

                AccountName = "";
                Total = 0.00;
                DuplicateProductCartList = ProductCartList = new ObservableCollection<ProductCart>();

                var isContinue = await App.Current.MainPage.DisplayAlert(TextString.AppName, TextString.CartCompleteMessage, TextString.Continue, TextString.Dashboard);
                if (!isContinue)
                    App.mainPage.Detail = new NavigationPage(new DashboardPage());
                else
                    App.mainPage.Detail = new NavigationPage(new OrderListPage());

            }
            catch (Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
        }
        #endregion
    }
}
