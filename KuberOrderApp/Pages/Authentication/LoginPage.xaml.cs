using System;
using System.Collections.Generic;
using KuberOrderApp.Pages.Base;
using KuberOrderApp.ViewModels.Authentication;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.Authentication
{
    public partial class LoginPage : ContentPage
    {
        #region ReadOnly Section
        private readonly LoginViewModel _loginViewModel;
        #endregion

        public LoginPage()
        {
            InitializeComponent();
            BindingContext = _loginViewModel = new LoginViewModel();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
