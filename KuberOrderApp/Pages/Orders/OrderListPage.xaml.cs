using System;
using System.Collections;
using System.Collections.Generic;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.Models.ResponseModels;
using KuberOrderApp.Resources;
using KuberOrderApp.Utilities;
using KuberOrderApp.ViewModels.Orders;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.Orders
{
    public partial class OrderListPage : ContentPage
    {
        #region ReadOnly Section
        private readonly OrderListViewModel _orderListViewModel;
        #endregion

        public OrderListPage()
        {
            InitializeComponent();
            BindingContext = _orderListViewModel = new OrderListViewModel();
            _orderListViewModel.SelectedAccountName = null;
            _orderListViewModel.SelectedCategoryList = "";
            _orderListViewModel.SelectedGroupList = "";
            _orderListViewModel.SelectedMasterList = "";
            _orderListViewModel.SelectedTypeList = "";
            _orderListViewModel.AccountName = "";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _orderListViewModel.GetFiltersAndProducts();
        }

        async void Entry_Quantity_Unfocused(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            if (entry == null)
                return;

            if (_orderListViewModel.SelectedAccountName == null)
            {
                entry.Text = "0";
                Helper.DisplayAlert("Please select account name");
                return;
            }

            ProductList productList = entry.BindingContext as ProductList;
            if (productList == null)
                return;

            if (string.IsNullOrWhiteSpace(entry.Text) || Convert.ToDouble(entry.Text) == 0)
            {
                entry.Text = "0";
                productList.ColOrderedQty = 0;
                productList.ColMRP = productList.ColOrderedQty * productList.ColSaleRate;
                SetCart(productList);
                return;
            }

            productList.ColOrderedQty = Convert.ToDouble(entry.Text);
            productList.ColMRP = productList.ColOrderedQty * productList.ColSaleRate;
            SetCart(productList);
        }

        void Entry_Product_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            //_orderListViewModel.SearchProduct();
        }

        void TapGestureRecognizer_Add_Quantity_Tapped(System.Object sender, System.EventArgs e)
        {
            if (_orderListViewModel.SelectedAccountName == null)
            {
                Helper.DisplayAlert("Please select account name");
                return;
            }

            var stackLayout = (StackLayout)sender;
            if (stackLayout == null)
                return;

            ProductList productList = stackLayout.BindingContext as ProductList;
            if (productList == null)
                return;

            productList.ColOrderedQty++;
            productList.ColMRP = productList.ColOrderedQty * productList.ColSaleRate;
            SetCart(productList);
        }
        void TapGestureRecognizer_Remove_Quantity_Tapped(System.Object sender, System.EventArgs e)
        {
            if (_orderListViewModel.SelectedAccountName == null)
            {
                Helper.DisplayAlert("Please select account name");
                return;
            }

            var stackLayout = (StackLayout)sender;
            if (stackLayout == null)
                return;

            ProductList productList = stackLayout.BindingContext as ProductList;
            if (productList == null)
                return;

            productList.ColOrderedQty--;
            productList.ColMRP = productList.ColOrderedQty * productList.ColSaleRate;

            if (productList.ColOrderedQty <= 0)
            {
                productList.ColOrderedQty = 0;
                productList.ColMRP = productList.ColOrderedQty * productList.ColSaleRate;
                SetCart(productList);
                return;
            }
            SetCart(productList);
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
        }

        void Entry_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
        }

        void Entry_Focused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            Entry entry = (Entry)sender;
            entry.Unfocus();
            _orderListViewModel.NavigateToCommonFilterPage(entry.StyleId);
        }

        void Party_Entry_Focused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            Entry entry = (Entry)sender;
            entry.Unfocus();
            _orderListViewModel.NavigateToCommonSearchPage();
        }

        void ListView_ItemAppearing(System.Object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
        {
            if (_orderListViewModel.ProductList != null && e.Item == _orderListViewModel.ProductList[_orderListViewModel.ProductList.Count - 1])
            {
                if (_orderListViewModel.LoadMoreCommand != null && _orderListViewModel.LoadMoreCommand.CanExecute(null))
                    _orderListViewModel.LoadMoreCommand.Execute(null);
            }
        }


        #region Set Product To Cart
        async private void SetCart(ProductList productList)
        {
            List<ProductCart> productCarts = new List<ProductCart>();

            var cartProduct = await App.Database.GetCartProduct(productList.ColPK);
            if (cartProduct == null)
            {
                ProductCart productCart = new ProductCart()
                {
                    AccountColPK = _orderListViewModel.SelectedAccountName.ColPK,
                    ColPK = productList.ColPK,
                    ColOrderedQty = productList.ColOrderedQty,
                    ColSaleRate = productList.ColSaleRate,
                    ColMRP = productList.ColMRP,
                    ColName = productList.ColName,
                    AccountColName = _orderListViewModel.SelectedAccountName.ColName
                };
                productCarts.Add(productCart);

                await App.Database.Insert<ProductCart>(productCarts);
            }
            else
            {
                if (productList.ColOrderedQty == 0)
                    await App.Database.DeleteData<ProductCart>(cartProduct);
                else
                {
                    cartProduct.ColOrderedQty = productList.ColOrderedQty;
                    cartProduct.ColMRP = productList.ColMRP;
                    await App.Database.UpdateData<ProductCart>(cartProduct);
                }
            }

            _orderListViewModel.CartCount = await App.Database.GetCount<ProductCart>();
        }
        #endregion

        protected override bool OnBackButtonPressed()
        {
            App.CheckAutoLogin();
            return true;
        }
    }
}
