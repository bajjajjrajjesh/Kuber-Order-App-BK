using System;
using System.Collections.Generic;
using KuberOrderApp.ViewModels.Authentication;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.Authentication
{
    public partial class ForgotPasswordPage : ContentPage
    {
        #region ReadOnly Section
        private readonly ForgotPasswordViewModel _forgotPasswordViewModel;
        #endregion

        public ForgotPasswordPage()
        {
            InitializeComponent();
            BindingContext = _forgotPasswordViewModel = new ForgotPasswordViewModel();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
