using System;
using System.Collections.Generic;
using System.Data;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.ViewModels.Ledger;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.Ledger
{
    public partial class LedgerPage : ContentPage
    {
        #region ReadOnly Section
        private readonly LedgerViewModel _ledgerViewModel;
        #endregion

        public LedgerPage()
        {
            InitializeComponent();
            BindingContext = _ledgerViewModel = new LedgerViewModel();
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_ledgerViewModel._isFromPDF)
            {
                _ledgerViewModel._isFromPDF = false;
                return;
            }
            _ledgerViewModel.DataTableCollection = null;
            await _ledgerViewModel.GetLedger();
            if (XmlDataGrid.Columns == null || XmlDataGrid.Columns.Count == 0)
                return;

            XmlDataGrid.Columns[0].IsHidden = true;
        }

        void Entry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            _ledgerViewModel.Search();
        }

        void BorderlessDatePicker_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            _ledgerViewModel.GetFilterData();
        }

        async void TappedXmlGrid(System.Object sender, Syncfusion.SfDataGrid.XForms.GridTappedEventArgs e)
        {
            DataRowView rowData = e.RowData as DataRowView;
            if (rowData == null)
                return;

            string keyId = rowData.Row.ItemArray[0].ToString();
            await App.Current.MainPage.Navigation.PushAsync(new LedgerDetailPage(keyId));
        }

        void XmlDataGrid_SelectionChanged(System.Object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
        }

        protected override bool OnBackButtonPressed()
        {
            App.CheckAutoLogin();
            return true;
        }
    }
}
