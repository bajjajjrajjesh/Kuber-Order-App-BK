using System;
using System.Collections.Generic;
using KuberOrderApp.Models.StaticModels;
using KuberOrderApp.ViewModels.Common;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.CommonPages
{
    public partial class CommonFilterPage : ContentPage
    {
        #region ReadOnly Section
        private readonly CommonFilterViewModel _commonFilterViewModel;
        #endregion

        public CommonFilterPage(bool isFromGroup = false, bool isFromCategory = false,
            bool isFromType = false, bool isFromMaster = false, List<string> filterList = null)
        {
            InitializeComponent();
            BindingContext = _commonFilterViewModel = new CommonFilterViewModel();
            _commonFilterViewModel.IsFromGroup = isFromGroup;
            _commonFilterViewModel.IsFromCategory = isFromCategory;
            _commonFilterViewModel.IsFromType = isFromType;
            _commonFilterViewModel.IsFromMaster = isFromMaster;
            _commonFilterViewModel.FilterList = filterList;
            _commonFilterViewModel.SetData();
        }

        void Entry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
        }
    }
}
