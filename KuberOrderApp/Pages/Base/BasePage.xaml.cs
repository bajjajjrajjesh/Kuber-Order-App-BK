using System;
using System.Collections.Generic;
using KuberOrderApp.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace KuberOrderApp.Pages.Base
{
    public partial class BasePage : ContentPage
    {
        public BasePage()
        {
            InitializeComponent();
            IStatusBar statusBar = DependencyService.Get<IStatusBar>();
            statusBar.HideStatusBar();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}
