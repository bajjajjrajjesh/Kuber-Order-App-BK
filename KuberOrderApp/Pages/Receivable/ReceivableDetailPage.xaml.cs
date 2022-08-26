using System;
using System.Collections.Generic;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.ViewModels.Receivable;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.Receivable
{
    public partial class ReceivableDetailPage : ContentPage
    {
        #region ReadOnly Section
        private readonly ReceivableDetailViewModel _receivableDetailViewModel;
        #endregion

        public ReceivableDetailPage(string selectedKey)
        {
            InitializeComponent();
            BindingContext = _receivableDetailViewModel = new ReceivableDetailViewModel();
            _receivableDetailViewModel.SelectedKey = selectedKey;
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            if(_receivableDetailViewModel._isFromPDF)
            {
                _receivableDetailViewModel._isFromPDF = false;
                return;
            }
            _receivableDetailViewModel._reportRequest = new ReportRequest()
            {
                OffsetFrom = 1,
                OffsetTo = 10,
                AccountFilter = _receivableDetailViewModel.SelectedKey
            };
            await _receivableDetailViewModel.GetReceivableDetail();
            if (XmlDataGrid.Columns == null || XmlDataGrid.Columns.Count == 0)
                return;

            XmlDataGrid.Columns[0].IsHidden = true;
        }

        void Entry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            //_receivableDetailViewModel.Search();
        }

        void BorderlessDatePicker_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            _receivableDetailViewModel.GetFilterData();
        }

        void TappedXmlGrid(System.Object sender, Syncfusion.SfDataGrid.XForms.GridTappedEventArgs e)
        {
        }
    }
}
