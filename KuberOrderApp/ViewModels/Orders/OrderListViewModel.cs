using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using KuberOrderApp.APIAndServices;
using KuberOrderApp.Interfaces;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.Models.ResponseModels;
using KuberOrderApp.Models.StaticModels;
using KuberOrderApp.Pages.CommonPages;
using KuberOrderApp.Pages.Orders;
using KuberOrderApp.Pages.SideMenu;
using KuberOrderApp.Utilities;
using KuberOrderApp.ViewModels.Base;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KuberOrderApp.ViewModels.Orders
{
    public class OrderListViewModel : BaseViewModel
    {
        #region Field Section
        private List<string> _groupList;
        private string _selectedGroupList;
        private List<string> _categoryList;
        private string _selectedCategoryList;
        private List<string> _typeList;
        private string _selectedTypeList;
        private List<string> _masterList;
        private string _selectedMasterList;
        private PartyList _selectedAccountName;
        private string _accountName;
        private ProductList _selectedProduct;
        private ObservableCollection<ProductList> _productList;
        private ObservableCollection<ProductList> _duplicateProductList;
        private ObservableCollection<ProductList> _filteredProductList;
        private bool _isLoaded;
        private string _searchRecord;
        private int _cartCount;
        private int _offSet;
        private IDownloader _downloader;
        #endregion

        #region Properties
        public List<string> GroupList
        {
            get { return _groupList; }
            set { SetProperty(ref _groupList, value); }
        }
        public string SelectedGroupList
        {
            get { return _selectedGroupList; }
            set { SetProperty(ref _selectedGroupList, value); }
        }
        public List<string> CategoryList
        {
            get { return _categoryList; }
            set { SetProperty(ref _categoryList, value); }
        }
        public string SelectedCategoryList
        {
            get { return _selectedCategoryList; }
            set { SetProperty(ref _selectedCategoryList, value); }
        }
        public List<string> TypeList
        {
            get { return _typeList; }
            set { SetProperty(ref _typeList, value); }
        }
        public string SelectedTypeList
        {
            get { return _selectedTypeList; }
            set { SetProperty(ref _selectedTypeList, value); }
        }
        public List<string> MasterList
        {
            get { return _masterList; }
            set { SetProperty(ref _masterList, value); }
        }
        public string SelectedMasterList
        {
            get { return _selectedMasterList; }
            set { SetProperty(ref _selectedMasterList, value); }
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
        public ProductList SelectedProduct
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
        public ObservableCollection<ProductList> ProductList
        {
            get { return _productList; }
            set { SetProperty(ref _productList, value); }
        }
        public string SearchRecord
        {
            get { return _searchRecord; }
            set { SetProperty(ref _searchRecord, value); }
        }
        public int CartCount
        {
            get { return _cartCount; }
            set { SetProperty(ref _cartCount, value); }
        }
        #endregion

        #region Commands
        public ICommand SelectionAccountCommand { private set; get; }
        public ICommand PlaceOrderCommand { private set; get; }
        public ICommand CartCommand { private set; get; }
        public ICommand LoadMoreCommandAuto { private set; get; }
        public ICommand SearchCommand { private set; get; }
        #endregion

        #region Constructr
        public OrderListViewModel()
        {
            MessagingCenter.Unsubscribe<string, PartyList>(this, "AddOrderAccountList");
            MessagingCenter.Subscribe<string, PartyList>(this, "AddOrderAccountList", async (sender, arg) =>
            {
                SelectedAccountName = arg;
                AccountName = SelectedAccountName.ColName;
            });
            MessagingCenter.Unsubscribe<string, string>(this, "GroupFilter");
            MessagingCenter.Subscribe<string, string>(this, "GroupFilter", async (sender, arg) =>
            {
                SelectedGroupList = arg;
                await FilterProduct();
            });
            MessagingCenter.Unsubscribe<string, string>(this, "CategoryFilter");
            MessagingCenter.Subscribe<string, string>(this, "CategoryFilter", async (sender, arg) =>
            {
                SelectedCategoryList = arg;
                await FilterProduct();
            });
            MessagingCenter.Unsubscribe<string, string>(this, "TypeFilter");
            MessagingCenter.Subscribe<string, string>(this, "TypeFilter", async (sender, arg) =>
            {
                SelectedTypeList = arg;
                await FilterProduct();
            });
            MessagingCenter.Unsubscribe<string, string>(this, "MasterFilter");
            MessagingCenter.Subscribe<string, string>(this, "MasterFilter", async (sender, arg) =>
            {
                SelectedMasterList = arg;
                await FilterProduct();
            });
            SelectionAccountCommand = new Command(() => NavigateToCommonSearchPage());
            SelectionCommand = new Command<string>((obj) => NavigateToCommonFilterPage(obj));
            PlaceOrderCommand = new Command(async () => await PlaceProductOrder());
            CartCommand = new Command(() => NavigateToCommonCartPage());
            LoadMoreCommandAuto = new Command<object>(LoadMoreOrder);
            SearchCommand = new Command(async () => await SearchProduct());
            _offSet = 0;
        }
        #endregion

        #region Public Methods
        async public Task GetFiltersAndProducts()
        {

            if (_isLoaded)
                return;

            _isLoaded = true;

            try
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");
                var productList = await App.Database.GetOrderDataList<ProductList>(_offSet);
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                if ((ProductList != null && ProductList.Count > 0) && (productList != null && productList.Count > 0))
                {
                    await GetOrderedListImages(productList);
                    foreach (var product in productList)
                    {
                        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                            product.IsStockVisible = false;
                        else
                            product.IsStockVisible = true;

                        if (product.OrderImage == null)
                        {
                            product.OrderImage = await DownloadImageAsync(product.ColImagePath);
                            await App.Database.UpdateData<ProductList>(product);
                        }
                        ProductList.Add(product);
                        _duplicateProductList.Add(product);
                        _filteredProductList.Add(product);
                    }

                }
                else
                {
                    if (ProductList != null && ProductList.Count > 0)
                        return;

                    ProductList = new ObservableCollection<ProductList>(productList);
                    _duplicateProductList = new ObservableCollection<ProductList>(productList);
                    _filteredProductList = new ObservableCollection<ProductList>(productList);
                    if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                    {
                        ProductList = new ObservableCollection<ProductList>(ProductList.Select(y => { y.IsStockVisible = false; return y; }).ToList());
                        _duplicateProductList = new ObservableCollection<ProductList>(_duplicateProductList.Select(y => { y.IsStockVisible = false; return y; }).ToList());
                        _filteredProductList = new ObservableCollection<ProductList>(_filteredProductList.Select(y => { y.IsStockVisible = false; return y; }).ToList());
                    }
                    else
                    {
                        ProductList = new ObservableCollection<ProductList>(ProductList.Select(y => { y.IsStockVisible = true; return y; }).ToList());
                        _duplicateProductList = new ObservableCollection<ProductList>(_duplicateProductList.Select(y => { y.IsStockVisible = true; return y; }).ToList());
                        _filteredProductList = new ObservableCollection<ProductList>(_filteredProductList.Select(y => { y.IsStockVisible = true; return y; }).ToList());
                    }

                    if (ProductList != null && ProductList.Count > 0)
                    {
                        if (ProductList != null && ProductList.Count > 0)
                        {
                            GroupList = new List<string>(ProductList.Select(o => o.ColGrpName).Distinct());
                            CategoryList = new List<string>(ProductList.Select(o => o.ColCatName).Distinct());
                            TypeList = new List<string>(ProductList.Select(o => o.ColTypeName).Distinct());
                            MasterList = new List<string>(ProductList.Select(o => o.ColMastersName).Distinct());
                        }
                    }
                }

               

                CartCount = await App.Database.GetCount<ProductCart>();
                if (productList != null && productList.Count > 0)
                {
                    await GetOrderedListImages(productList);
                    foreach(var product in productList)
                    {
                        if (product.OrderImage == null)
                        {
                            product.OrderImage = await DownloadImageAsync(product.ColImagePath);
                            await App.Database.UpdateData<ProductList>(product);
                        }
                    }
                }
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
        }

        async public Task SearchProduct()
        {
            try
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");
                if (string.IsNullOrWhiteSpace(SearchRecord))
                {
                    ProductList = _filteredProductList;
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                    return;
                }

                ProductList = new ObservableCollection<ProductList>((_filteredProductList.Where(x => (!string.IsNullOrEmpty(x.ColName) && x.ColName.ToLower().Contains(SearchRecord.ToLower())))).ToList());
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
            catch(Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
        }

        async public void NavigateToCommonFilterPage(object strFilter)
        {
            if (strFilter.ToString() == "Group")
                await App.Current.MainPage.Navigation.PushAsync(new CommonFilterPage(isFromGroup: true, filterList: GroupList));
            else if (strFilter.ToString() == "Category")
                await App.Current.MainPage.Navigation.PushAsync(new CommonFilterPage(isFromCategory: true, filterList: CategoryList));
            else if (strFilter.ToString() == "Type")
                await App.Current.MainPage.Navigation.PushAsync(new CommonFilterPage(isFromType: true, filterList: TypeList));
            else if (strFilter.ToString() == "Master")
                await App.Current.MainPage.Navigation.PushAsync(new CommonFilterPage(isFromMaster: true, filterList: MasterList));
        }

        async public void NavigateToCommonSearchPage()
        {
            await App.Current.MainPage.Navigation.PushAsync(new CommonSearchPage(isFromAddOrder: true));
        }
        #endregion

        #region Private Methods
        private async void LoadMoreOrder(object obj)
        {
           /* if (_filteredProductList.Count <= 10)
                return;*/

            _isLoaded = false;
            _offSet += 10;
            await GetFiltersAndProducts();
        }
        async private void NavigateToCommonCartPage()
        {
            if(CartCount > 0)
                App.mainPage.Detail = new NavigationPage(new CartOrderPage());
        }

        async private Task FilterProduct()
        {
            _filteredProductList = ProductList = new ObservableCollection<ProductList>((_filteredProductList.Where(x => (!string.IsNullOrWhiteSpace(x.ColGrpName) && x.ColGrpName == SelectedGroupList)
            || (!string.IsNullOrWhiteSpace(x.ColCatName) && x.ColCatName == SelectedCategoryList)
            || (!string.IsNullOrWhiteSpace(x.ColTypeName) && x.ColTypeName == SelectedTypeList)
            || (!string.IsNullOrWhiteSpace(x.ColMastersName) && x.ColMastersName == SelectedMasterList)).ToList()));
        }

        async private Task PlaceProductOrder()
        {
            var productList = ProductList.Where(x => x.ColOrderedQty > 0).ToList();

            if (SelectedAccountName == null)
            {
                Helper.DisplayAlert("Please select account name");
                return;
            }
            if (productList == null || productList.Count == 0)
            {
                Helper.DisplayAlert("Please add quantity for product");
                return;
            }
            try
            {
               
                App.mainPage.Detail = new NavigationPage(new CartOrderPage());
            }
            catch (Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.HideLoading();
            }
        }

        async private Task GetOrderedListImages(List<ProductList> listProducts)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    string strIds = "";
                    List<string> listIds = new List<string>(listProducts.Select(x => x.ColPK).ToList());
                    foreach(var id in listIds)
                    {
                        if (string.IsNullOrWhiteSpace(strIds))
                            strIds = id;
                        else
                            strIds = $"{strIds},{id}";
                    }
                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");

                    var orderImageResponse = await ApiService.GetRequest<List<string>>($"{ApiPathString.GetOrderImages}?ProdId={strIds}", null, null);
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();

                    if (orderImageResponse == null && orderImageResponse.Count == 0)
                        return;

                    for(int i = 0; i < orderImageResponse.Count; i++)
                    {
                        var orderImageList = orderImageResponse[i];
                        ProductList[i].ColOrderedQty = Convert.ToDouble(orderImageList[0]);
                    }

                }
                catch (Exception ex)
                {
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                }
            }
        }


        async Task<byte[]> DownloadImageAsync(string imageUrl)
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    const int _downloadImageTimeoutInSeconds = 15;
                    HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(_downloadImageTimeoutInSeconds) };

                    using (var httpResponse = await _httpClient.GetAsync(imageUrl))
                    {
                        if (httpResponse.StatusCode == HttpStatusCode.OK)
                        {
                            return await httpResponse.Content.ReadAsByteArrayAsync();
                        }
                        else
                        {
                            //Url is Invalid
                            return null;
                        }
                    }
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                //Handle Exception
                return null;
            }
        }
        #endregion
    }
}
