using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android;
using Android.Locations;
using Xamarin.Essentials;
using System.Linq;
using Plugin.Permissions;
using KuberOrderApp.Droid.Services;
using Android.Content;
using Android.App.Job;
using Xamarin.Forms;
using KuberOrderApp.Pages.Home;

namespace KuberOrderApp.Droid
{
    [Activity(Label = "KuberOrderApp", Icon = "@mipmap/ic_launcher", Theme = "@style/MyTheme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        static readonly string[] ignoredProviders = { LocationManager.PassiveProvider, "local_database" };
        private static int MYJOBID = 1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate(savedInstanceState);


            Java.Lang.Class javaClass = Java.Lang.Class.FromType(typeof(JobSchedulerService));
            ComponentName obService = new ComponentName(".Service", JobSchedulerService);

            JobInfo.Builder builder1 = new JobInfo.Builder(MYJOBID, obService)
                                     .SetMinimumLatency(5000)   // Wait at least 5 second
                                     .SetOverrideDeadline(10000) // But no longer than 10 seconds
                                     .SetRequiredNetworkType(NetworkType.Unmetered);
            JobInfo jobInfo1 = builder1.Build();

            UserDialogs.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            LoadApplication(new App());
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) == Permission.Denied || ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessBackgroundLocation) == Permission.Denied || ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == Permission.Denied
                || ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == Permission.Denied
                || ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == Permission.Denied)
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessBackgroundLocation,
                Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage}, 1);
            }
            else
            {
                StartService(new Intent(this, typeof(BackgroundLocationService)));
                StartService(new Intent(this, typeof(BootCompleteReceiver)));
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            for (int i = 0; i < permissions.Count(); i++)
            {
                if (permissions[i].ToString() == "android.permission.ACCESS_FINE_LOCATION")
                {
                    if (grantResults.Length > 0 && grantResults[i] == Permission.Granted)
                    {
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            LocationManager locationManager = (LocationManager)GetSystemService(LocationService);
                            var enabledProviders = locationManager.GetProviders(true);
                            var hasProviders = enabledProviders.Any(p => !ignoredProviders.Contains(p));
                            if (hasProviders)
                            {
                                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                                var userLocation = await Geolocation.GetLastKnownLocationAsync();
                                userLocation = await Geolocation.GetLocationAsync(request);
                                StartService(new Intent(this, typeof(BackgroundLocationService)));
                            }
                        });
                    }
                }
            }
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        //public override void OnBackPressed()
        //{
        //    //  base.OnBackPressed ();            /*  Comment this base call to avoid calling Finish()  */
        //    //  Do nothing

        //    App.CheckAutoLogin();
        //}
    }
}