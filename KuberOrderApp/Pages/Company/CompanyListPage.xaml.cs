using System;
using System.Collections.Generic;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.Models.ResponseModels;
using KuberOrderApp.ViewModels.Company;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.Company
{
    public partial class CompanyListPage : ContentPage
    {
        #region ReadOnly Section
        private readonly CompanyListViewModel _companyListViewModel;
        #endregion
        ViewCell lastCell;
        public CompanyListPage(List<CompanyList> companyList, LoginRequest loginRequest)
        {
            InitializeComponent();
            BindingContext = _companyListViewModel = new CompanyListViewModel(companyList, loginRequest);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.FromHex("e29152");
                lastCell = viewCell;
            }
        }
    }
}
