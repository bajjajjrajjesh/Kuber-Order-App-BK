using System;
using System.Collections.Generic;
using System.Linq;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.ViewModels.Orders;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.Orders
{
    public partial class CartOrderPage : ContentPage
    {
        #region ReadOnly Section
        private readonly CartOrderViewModel _cartOrderViewModel;
        #endregion

        public CartOrderPage()
        {
            InitializeComponent();
            BindingContext = _cartOrderViewModel = new CartOrderViewModel();
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            await _cartOrderViewModel.GetCartList();
        }

        void Entry_Product_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            //_cartOrderViewModel.SearchProduct();
        }

        void Entry_Quantity_Unfocused(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            if (entry == null)
                return;

            ProductCart productList = entry.BindingContext as ProductCart;
            if (productList == null)
                return;

            if (string.IsNullOrWhiteSpace(entry.Text) || Convert.ToDouble(entry.Text) == 0)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    productList.ColOrderedQty = 0;
                    _cartOrderViewModel.DuplicateProductCartList.Remove(productList);
                    _cartOrderViewModel.ProductCartList.Remove(productList);
                    _cartOrderViewModel.Total = _cartOrderViewModel.DuplicateProductCartList.Sum(x => x.ColMRP);
                    SetCart(productList);
                });
                return;
            }
            productList.ColOrderedQty = Convert.ToDouble(entry.Text);
            productList.ColMRP = productList.ColOrderedQty * productList.ColSaleRate;
            SetCart(productList);
        }

        void TapGestureRecognizer_Add_Quantity_Tapped(System.Object sender, System.EventArgs e)
        {
            var stackLayout = (StackLayout)sender;
            if (stackLayout == null)
                return;

            ProductCart product = stackLayout.BindingContext as ProductCart;
            if (product == null)
                return;

            product.ColOrderedQty++;
            product.ColMRP = product.ColOrderedQty * product.ColSaleRate;

            var duplicateProduct = _cartOrderViewModel.DuplicateProductCartList.Where(x => x.id == product.id).FirstOrDefault();
            if (duplicateProduct == null)
                return;

            duplicateProduct.ColOrderedQty = product.ColOrderedQty;
            duplicateProduct.ColMRP = product.ColMRP;

            _cartOrderViewModel.Total = _cartOrderViewModel.DuplicateProductCartList.Sum(x => x.ColMRP);
            SetCart(product);
        }
        void TapGestureRecognizer_Remove_Quantity_Tapped(System.Object sender, System.EventArgs e)
        {
            var stackLayout = (StackLayout)sender;
            if (stackLayout == null)
                return;

            ProductCart product = stackLayout.BindingContext as ProductCart;
            if (product == null)
                return;

            product.ColOrderedQty--;
            product.ColMRP = product.ColOrderedQty * product.ColSaleRate;

            if (product.ColOrderedQty == 0)
            {
                _cartOrderViewModel.DuplicateProductCartList.Remove(product);
                _cartOrderViewModel.ProductCartList.Remove(product);
                _cartOrderViewModel.Total = _cartOrderViewModel.DuplicateProductCartList.Sum(x => x.ColMRP);
                SetCart(product);
                return;
            }

            var duplicateProduct = _cartOrderViewModel.DuplicateProductCartList.Where(x => x.id == product.id).FirstOrDefault();
            if (duplicateProduct == null)
                return;

            duplicateProduct.ColOrderedQty = product.ColOrderedQty;
            duplicateProduct.ColMRP = product.ColMRP;

            _cartOrderViewModel.Total = _cartOrderViewModel.DuplicateProductCartList.Sum(x => x.ColMRP);
            SetCart(product);
        }

        async void TapGestureRecognizer_Delete_Tapped(System.Object sender, System.EventArgs e)
        {
            var image = (Image)sender;
            if (image == null)
                return;

            ProductCart product = image.BindingContext as ProductCart;
            if (product == null)
                return;

            await _cartOrderViewModel.DeleteProductFromCart(product);
        }


        #region Set Product To Cart
        async private void SetCart(ProductCart productList)
        {
            try
            {
                List<ProductCart> productCarts = new List<ProductCart>();

                var cartProduct = await App.Database.GetCartProduct(productList.ColPK);
                if (cartProduct != null)
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
            }
            catch(Exception ex)
            {

            }
        }
        #endregion

        protected override bool OnBackButtonPressed()
        {
            App.CheckAutoLogin();
            return true;
        }

    }
}
