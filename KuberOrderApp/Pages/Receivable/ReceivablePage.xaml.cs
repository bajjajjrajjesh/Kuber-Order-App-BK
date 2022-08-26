using System;
using System.Collections.Generic;
using System.Data;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.ViewModels.Receivable;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.Receivable
{
    public partial class ReceivablePage : ContentPage
    {
        #region ReadOnly Section
        private readonly ReceivableViewModel _receivableViewModel;
        #endregion

        public ReceivablePage()
        {
            InitializeComponent();
            BindingContext = _receivableViewModel = new ReceivableViewModel();
            GetOnLoad();
        }
        public async void GetOnLoad()
        {
             base.OnAppearing();


            if (_receivableViewModel._isFromPDF)
            {
                _receivableViewModel._isFromPDF = false;
                return;
            }
            _receivableViewModel._reportRequest = new ReportRequest()
            {
                OffsetFrom = 1,
                OffsetTo = 10,
                ProductFilter = ""
            };

            await _receivableViewModel.GetReceivable();
            if (XmlDataGrid.Columns == null || XmlDataGrid.Columns.Count == 0)
                return;

            XmlDataGrid.Columns[0].IsHidden = true;
        

   
        }
    
      /*  async protected override void OnAppearing()
        {
            base.OnAppearing();
            

            if (_receivableViewModel._isFromPDF)
            {
                _receivableViewModel._isFromPDF = false;
                return;
            }
            _receivableViewModel._reportRequest = new ReportRequest()
            {
                OffsetFrom = 1,
                OffsetTo = 10,
                ProductFilter = ""
            };
            
            await _receivableViewModel.GetReceivable();
            if (XmlDataGrid.Columns == null || XmlDataGrid.Columns.Count == 0)
                return;

            XmlDataGrid.Columns[0].IsHidden = true;
        }*/

        void Entry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
           // _receivableViewModel.Search();
        }

        void BorderlessDatePicker_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            _receivableViewModel.GetFilterData();
        }

        async void TappedXmlGrid(System.Object sender, Syncfusion.SfDataGrid.XForms.GridTappedEventArgs e)
        {
            DataRowView rowData = e.RowData as DataRowView;
            if (rowData == null)
                return;

            string keyId = rowData.Row.ItemArray[0].ToString();
            await App.Current.MainPage.Navigation.PushAsync(new ReceivableDetailPage(keyId));
        }

        void XmlDataGrid_QueryUnboundColumnValue(System.Object sender, Syncfusion.SfDataGrid.XForms.GridUnboundColumnEventArgs e)
        {
        }

        void XmlDataGrid_ColumnResizing(System.Object sender, Syncfusion.SfDataGrid.XForms.GridResizingEventArgs e)
        {
        }

        protected override bool OnBackButtonPressed()
        {
            App.CheckAutoLogin();
            return true;
        }
    }
}
