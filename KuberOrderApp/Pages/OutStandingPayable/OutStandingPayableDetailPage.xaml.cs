using System;
using System.Collections.Generic;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.ViewModels.OutStandingPayable;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.OutStandingPayable
{
    public partial class OutStandingPayableDetailPage : ContentPage
    {
        #region ReadOnly Section
        private readonly OutStandingPayableDetailViewModel _outStandingPayableDetailViewModel;
        #endregion

        public OutStandingPayableDetailPage(string selectedKey)
        {
            InitializeComponent();
            BindingContext = _outStandingPayableDetailViewModel = new OutStandingPayableDetailViewModel();
            _outStandingPayableDetailViewModel.SelectedKey = selectedKey;
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_outStandingPayableDetailViewModel._isFromPDF)
            {
                _outStandingPayableDetailViewModel._isFromPDF = false;
                return;
            }
            _outStandingPayableDetailViewModel._reportRequest = new ReportRequest()
            {
                OffsetFrom = 1,
                OffsetTo = 10,
                AccountFilter = _outStandingPayableDetailViewModel.SelectedKey
            };
            await _outStandingPayableDetailViewModel.GetPayableDetails();
            if (XmlDataGrid.Columns == null || XmlDataGrid.Columns.Count == 0)
                return;

            XmlDataGrid.Columns[0].IsHidden = true;
        }

        void Entry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            _outStandingPayableDetailViewModel.Search();
        }

        void BorderlessDatePicker_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            _outStandingPayableDetailViewModel.GetFilterData();
        }

        void TappedXmlGrid(System.Object sender, Syncfusion.SfDataGrid.XForms.GridTappedEventArgs e)
        {
        }
    }
}
