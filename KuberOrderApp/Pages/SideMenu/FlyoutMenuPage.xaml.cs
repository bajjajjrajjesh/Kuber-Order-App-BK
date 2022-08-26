using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using KuberOrderApp.Pages.Base;
using KuberOrderApp.ViewModels.SideMenu;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.SideMenu
{
    public partial class FlyoutMenuPage : ContentPage
    {
        #region ReadOnly Section
        private readonly FlyoutMenuViewModel _flyoutMenuViewModel;
        #endregion

        public FlyoutMenuPage(MainPage mainPage)
        {
            InitializeComponent();
            BindingContext = _flyoutMenuViewModel  = new FlyoutMenuViewModel(mainPage); 
        }
    }
}
