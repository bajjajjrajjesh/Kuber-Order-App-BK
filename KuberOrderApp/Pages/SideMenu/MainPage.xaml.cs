using System;
using System.Collections.Generic;
using KuberOrderApp.Pages.Home;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.SideMenu
{
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
        {
            InitializeComponent();
            Flyout = new FlyoutMenuPage(this);
            Detail = new NavigationPage(new DashboardPage());
        }
    }
}
