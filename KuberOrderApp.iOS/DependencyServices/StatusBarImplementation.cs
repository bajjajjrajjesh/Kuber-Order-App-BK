using System;
using KuberOrderApp.Interfaces;
using KuberOrderApp.iOS.DependencyServices;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(StatusBarImplementation))]
namespace KuberOrderApp.iOS.DependencyServices
{
    public class StatusBarImplementation : IStatusBar
    {
        public StatusBarImplementation()
        {
        }

        #region IStatusBar implementation
        public void HideStatusBar()
        {
            UIApplication.SharedApplication.StatusBarHidden = true;
        }

        public void ShowStatusBar()
        {
            UIApplication.SharedApplication.StatusBarHidden = false;
        }
        #endregion
    }
}