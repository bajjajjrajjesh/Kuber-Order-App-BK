using System;
using System.Collections.Generic;
using KuberOrderApp.ViewModels.Orders;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.Orders
{
    public partial class OrderReportPage : ContentPage
    {
        #region ReadOnly Section
        private readonly OrderReportViewModel _orderReportViewModel;
        #endregion

        public OrderReportPage()
        {
            InitializeComponent();
            BindingContext = _orderReportViewModel = new OrderReportViewModel();
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_orderReportViewModel._isFromPDF)
            {
                _orderReportViewModel._isFromPDF = false;
                return;
            }
            await _orderReportViewModel.GetOrderedList();
            if (XmlDataGrid.Columns == null || XmlDataGrid.Columns.Count == 0)
                return;

            XmlDataGrid.Columns[0].IsHidden = true;
        }

        void Entry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
           // _orderReportViewModel.Search();
        }

        void TappedXmlGrid(System.Object sender, Syncfusion.SfDataGrid.XForms.GridTappedEventArgs e)
        {
        }

        protected override bool OnBackButtonPressed()
        {
            App.CheckAutoLogin();
            return true;
        }
    }
}
