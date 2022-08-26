using System;
using System.Collections.Generic;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.ViewModels.Ledger;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.Ledger
{
    public partial class LedgerDetailPage : ContentPage
    {
        #region ReadOnly Section
        private readonly LedgerDetailViewModel _ledgerDetailViewModel;
        #endregion

        public LedgerDetailPage(string selectedKey)
        {
            InitializeComponent();
            BindingContext = _ledgerDetailViewModel = new LedgerDetailViewModel();
            _ledgerDetailViewModel.SelectedKey = selectedKey;
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_ledgerDetailViewModel._isFromPDF)
            {
                _ledgerDetailViewModel._isFromPDF = false;
                return;
            }
            _ledgerDetailViewModel._reportRequest = new ReportRequest()
            {
                OffsetFrom = 1,
                OffsetTo = 10,
                AccountFilter = _ledgerDetailViewModel.SelectedKey,
            };
            await _ledgerDetailViewModel.GetLedgerDetails();
            if (XmlDataGrid.Columns == null || XmlDataGrid.Columns.Count == 0)
                return;

            XmlDataGrid.Columns[0].IsHidden = true;
        }

        void Entry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            _ledgerDetailViewModel.Search();
        }

        void BorderlessDatePicker_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            _ledgerDetailViewModel.GetFilterData();
        }

        void TappedXmlGrid(System.Object sender, Syncfusion.SfDataGrid.XForms.GridTappedEventArgs e)
        {
        }
    }
}
