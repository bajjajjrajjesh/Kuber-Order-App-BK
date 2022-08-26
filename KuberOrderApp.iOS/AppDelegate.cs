using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using KuberOrderApp.iOS.Services;
using Syncfusion.ListView.XForms.iOS;
using UIKit;
using Xamarin.Forms;

namespace KuberOrderApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //

        public static LocationManager Manager = null;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            Syncfusion.SfDataGrid.XForms.iOS.SfDataGridRenderer.Init();
            Syncfusion.XForms.iOS.ComboBox.SfComboBoxRenderer.Init();
            SfListViewRenderer.Init();
            LoadApplication(new App());
            UIBarButtonItem.Appearance.SetBackButtonTitlePositionAdjustment(new UIOffset(-100, -60), UIBarMetrics.Default);
            Manager = new LocationManager();
            Manager.StartLocationUpdates();
            return base.FinishedLaunching(app, options);
        }

        #region Change StatusBar Color
        public void SetLightTheme()
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.DarkContent, false);
                    var currentViewController = GetCurrentViewController();
                    if (currentViewController != null)
                        currentViewController.SetNeedsStatusBarAppearanceUpdate();
                });
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.Message);
            }
        }
        UIViewController GetCurrentViewController()
        {
            try
            {
                var window = UIApplication.SharedApplication.KeyWindow;
                var vc = window.RootViewController;
                while (vc.PresentedViewController != null)
                    vc = vc.PresentedViewController;
                return vc;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.Message);
                return null;
            }
        }
        #endregion
    }
}
