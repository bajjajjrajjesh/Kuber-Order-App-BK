using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KuberOrderApp.ViewModels.Home;
using Xamarin.Forms;

namespace KuberOrderApp.Pages.Home
{
    public partial class DashboardPage : ContentPage
    {
        #region ReadOnly Section
        private readonly DashboardViewModel _dashboardViewModel;
        #endregion

        public DashboardPage()
        {
            InitializeComponent();
            BindingContext = _dashboardViewModel = new DashboardViewModel();
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            await _dashboardViewModel.GetDashboardData();
        }

        
       // protected override bool OnBackButtonPressed() => true;
        protected override  bool OnBackButtonPressed()
        {
            
            var vm = (DashboardViewModel)BindingContext;
          /*  bool answer =  DisplayAlert("Question?", "Would you like to Close the app", "Yes", "No");
            //Debug.WriteLine("Answer: " + answer);
            if (answer == "yes")
            {
                System.Environment.Exit(0);
            }*/
            DisplayAlert("Warning!", "Would you like to Close the app", "Yes", "No")
       .ContinueWith(answer =>
       {
           if (answer.Result)
               System.Environment.Exit(0); // I'm not sure, but maybe you should wrap it on a 'BeginInvokeOnMainThread'
       });

           
            return true;
        }

    }
}
