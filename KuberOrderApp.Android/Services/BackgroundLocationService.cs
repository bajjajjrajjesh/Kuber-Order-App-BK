using System;
using Android.Content;
using Android.Locations;
using Android.Provider;
using Xamarin.Forms;
using System.Diagnostics;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Xamarin.Forms.Internals;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android.Views;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Android.App;
using KuberOrderApp.Models.ResponseModels;
using Android;
using System.Threading.Tasks;
using KuberOrderApp.Models.RequestModels;

[assembly: UsesPermission(Manifest.Permission.AccessFineLocation)]
[assembly: UsesPermission(Manifest.Permission.AccessCoarseLocation)]
[assembly: UsesPermission(Manifest.Permission.AccessLocationExtraCommands)]
namespace KuberOrderApp.Droid.Services
{
    [Service]
    public class BackgroundLocationService : Service, ILocationListener
    {
        private static bool startLocationUpdatesOnResume;
        private LocationManager locationManager;
        double NewSpeed, OldSpeed;
        int Count;
        Notification notification;
        private Location mLastLocation;
        public Location GetAddressLocation;
        bool useMockLocation = false;//NOTE: for true, uncomment AccessMockLocation above
        const string MockProvider = "mock";// LocationManager.GpsProvider;
        public static BackgroundLocationService CurrentInstanceLocation { get; set; }
        LocationData objLocationData = new LocationData();
        public List<double> arrLatitude = new List<double>();
        public List<double> arrLongitude = new List<double>();
        public class LocationServiceBinder : Binder
        {
            BackgroundLocationService service;

            public LocationServiceBinder(BackgroundLocationService service)
            {
                try
                {
                    this.service = service;
                }
                catch (Exception ex) { }
            }

            public BackgroundLocationService GetLocationServiceBinder()
            {
                try
                {
                    return service;
                }
                catch (Exception ex) { return null; }
            }
        }


        public static bool StartLocationUpdatesOnResume
        {
            get
            {
                return startLocationUpdatesOnResume;
            }
        }

        public event Action<LocationData> LocationChanged;
        public event Action<string> MessageChanged;



        protected LocationManager LocMgr = Android.App.Application.Context.GetSystemService("location") as LocationManager;

        readonly string logTag = "LocationService";
        IBinder binder;

        public override void OnCreate()
        {
            base.OnCreate();

        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            CurrentInstanceLocation = this;
            StartLocationUpdates();
            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            try
            {
                if (intent != null)
                {
                    binder = new LocationServiceBinder(this);
                    return binder;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public override void OnDestroy()
        {
             Toast.MakeText(Forms.Context, "OnLocation D ", ToastLength.Long).Show();
            Intent intent = new Intent("com.android.techtrainner");
           intent.PutExtra("yourvalue", "torestore");
            SendBroadcast(intent);
        }



        async public void OnLocationChanged(Location locationData)
        {
            //Toast.MakeText(this, "OnLocation change ", ToastLength.Long).Show();
            GetAddressLocation = locationData;
            Count++;
            //this.mLastLocation.Latitude

            await Task.Run(() =>
            {
                if (locationData == null)
                    return;

                AddLocation addLocation = new AddLocation()
                {
                    ColDDateTime = DateTime.Now,
                    ColNLatitude = locationData.Latitude,
                    ColNLongitude = locationData.Longitude,
                };
                MessagingCenter.Send<string, AddLocation>("True", "SetLocation", addLocation);
            });

            //if (Count == 1)
            //{
            //    Count = 0;

            //    double speed = 0;

            //    if (this.mLastLocation != null)
            //    {
            //        double distance = DistanceCalculate(mLastLocation.Latitude, mLastLocation.Longitude, locationData.Latitude, locationData.Longitude);
            //        if (Helpers.Settings.isMilePerHour == "False")
            //        {
            //            distance = distance * 1.60934;
            //        }
            //        double time = locationData.Time - mLastLocation.Time;
            //        double hours = TimeSpan.FromMilliseconds(time).TotalHours;
            //        speed = distance / hours;
            //    }
            //    else
            //    {
            //        speed = locationData.Speed;
            //    }

            //    this.mLastLocation = locationData;

            //    if (App.CurrentInstanceDashboad != null)
            //    {
            //        string strSpeedDash = speed.ToString("F");
            //        if (Helpers.Settings.isMilePerHour == "True")
            //        {
            //            if (speed > 0)
            //            {
            //                App.CurrentInstanceDashboad.getSpeedValue(strSpeedDash + " " + "mph");
            //            }
            //            else
            //            {
            //                App.CurrentInstanceDashboad.getSpeedValue("0.00" + " " + "mph");
            //            }
            //        }
            //        else
            //        {
            //            if (speed > 0)
            //            {
            //                App.CurrentInstanceDashboad.getSpeedValue(strSpeedDash + " " + "kph");
            //            }
            //            else
            //            {
            //                App.CurrentInstanceDashboad.getSpeedValue("0.00" + " " + "kph");
            //            }
            //        }
            //    }

            //    if (MainActivity.Instance != null)
            //    {
            //        string strSpeed1 = speed.ToString("F");
            //        if (Helpers.Settings.isMilePerHour == "True")
            //        {
            //            if (speed > 0)
            //            {
            //                MainActivity.Instance.getSpeedValue(strSpeed1 + " " + "mph");
            //            }
            //            else
            //            {
            //                MainActivity.Instance.getSpeedValue("0.00" + " " + "mph");
            //            }
            //        }
            //        else
            //        {
            //            if (speed > 0)
            //            {
            //                MainActivity.Instance.getSpeedValue(strSpeed1 + " " + "kph");
            //            }
            //            else
            //            {
            //                MainActivity.Instance.getSpeedValue("0.00" + " " + "kph");
            //            }

            //        }
            //    }

            //    App.LocationAllDetails = new LocationData(locationData.Longitude, locationData.Latitude, speed, locationData.Provider);

            //    //if (Forms.Context != null)
            //    //{
            //    //Toast.MakeText(this, "OnContext Call Thay.", ToastLength.Long).Show();
            //    string strSpeed = speed.ToString("F");
            //    speed = Convert.ToDouble(strSpeed);
            //    double exactSpeed = OldSpeed - speed;

            //    int SensitiveSpeed = Convert.ToInt32(Helpers.Settings.SensitiveSpeed);

            //    if (exactSpeed > SensitiveSpeed && Helpers.Settings.isOpenApp == "False")
            //    {
            //        try
            //        {
            //            App.isEditWitnessOpen = true;
            //            var intent = new Intent(this.BaseContext, typeof(MainActivity));
            //            intent.SetFlags(ActivityFlags.NewTask);
            //            this.BaseContext.StartActivity(intent);
            //        }
            //        catch (Exception e)
            //        {
            //            Toast.MakeText(this, "ExactSpeed Condition" + e.ToString(), ToastLength.Long).Show();
            //        }
            //    }
            //    OldSpeed = speed;
            //    //}
            //}
        }

        public override void OnTaskRemoved(Intent rootIntent)
        {

            //App.isEditWitnessOpen = true;
            //var intent = new Intent(this.BaseContext, typeof(MainActivity));
            //intent.SetFlags(ActivityFlags.NewTask);
            //this.BaseContext.StartActivity(intent);



        }

        public void OnMessageChanged(string message)
        {
            MessageChanged?.Invoke(message);
        }

        private void GetMockLocation()
        {
            try
            {
                locationManager.RemoveTestProvider(MockProvider);
            }
            catch (Exception e)
            {
                //Debug.WriteLine("RemoveTestProvider: " + e);
            }

            locationManager.AddTestProvider
            (
              MockProvider,
              "requiresNetwork" == "",
              "requiresSatellite" == "",
              "requiresCell" == "",
              "hasMonetaryCost" == "",
              "supportsAltitude" == "",
              "supportsSpeed" == "",
              "supportsBearing" == "",
              Power.Low,
              Android.Hardware.SensorStatus.AccuracyHigh
            );

            locationManager.SetTestProviderEnabled
            (
              MockProvider,
              true
            );

            locationManager.SetTestProviderStatus
            (
               MockProvider,
               Availability.Available,
               null,
               Java.Lang.JavaSystem.CurrentTimeMillis()
            );

            SetMockLocation();
            StartRandomMockLocations(10);
        }

        public void SetMockLocation(bool useRandomLocation = false)
        {
            Random random = new Random();

            Location newLocation = new Location(MockProvider)
            {

                Longitude = random.NextDouble() * 360 - 180,
                Latitude = random.NextDouble() * 180 - 90,
                Accuracy = 100.0f,
                Time = DateTime.Now.Ticks,
                Altitude = 1.0,
                Speed = 0.0f,
                Bearing = 0.0f,
                Provider = MockProvider,
                ElapsedRealtimeNanos = (long)(TimeSpan.FromTicks(DateTime.Now.Ticks).TotalMilliseconds * 1000000)
            };

            if (useRandomLocation)
            {
                newLocation.Longitude = -118.2437;//Los Angeles
                newLocation.Latitude = 34.0522;
            }

            locationManager.SetTestProviderLocation
        (
          MockProvider,
          newLocation
        );
        }

        private void StartRandomMockLocations(double secInterval)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000 * secInterval;
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            SetMockLocation(true);
        }

        async public void StartLocationUpdates()
        {
            startLocationUpdatesOnResume = false;
            locationManager = locationManager ?? Application.ApplicationContext.GetSystemService(Context.LocationService) as LocationManager;
            if (locationManager == null)
            {
                return;
            }

            string locationProvider = null;

            if (useMockLocation)
            {
                GetMockLocation();
                locationProvider = MockProvider;
            }
            else
            {
                if (locationManager.IsProviderEnabled(LocationManager.GpsProvider))
                {
                    locationProvider = LocationManager.GpsProvider;
                }
                else if (locationManager.IsProviderEnabled(LocationManager.NetworkProvider))
                {
                    locationProvider = LocationManager.NetworkProvider;
                }
            }

            if (string.IsNullOrEmpty(locationProvider) == false)
            {
               // App.isLocationOn = true;
                //Toast.MakeText(this, "Get Location Provider: "+locationProvider, ToastLength.Long).Show();
                Location lastKnownLocation = locationManager.GetLastKnownLocation(locationProvider);
                if (lastKnownLocation != null)
                {
                    OnLocationChanged(lastKnownLocation);
                }

                locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);

                OnMessageChanged(string.Format("Finding location via the {0} provider...", locationProvider));
            }
            else
            {
                //Toast.MakeText(this, "Not Get Location Provider: " + locationProvider, ToastLength.Long).Show();
                //if (App.isAppOpen == true)
                //{
                //    App.isLocationOn = false;
                //    ShowLocationSettingsIntent();
                //}
                //else
                //{

                //    if (App.isReboot == true)
                //    {
                //        App.isReboot = false;
                //        Toast.MakeText(this, "We are unable to get your location. Please open MyCrash Application to Track your location." + locationProvider, ToastLength.Long).Show();
                //    }

                //}
            }
        }

        public void StopLocationUpdates()
        {
            locationManager?.RemoveUpdates(this);
        }

        private void ShowLocationSettingsIntent()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(Forms.Context);
            builder.SetTitle("No GPS detected");
            builder.SetMessage("Would you like to adjust your location service settings?");
            builder.SetCancelable(false);
            builder.SetNegativeButton("NO", (sender, args) =>
            {
                OnMessageChanged("Unable to find an enabled provider.");
            });
            builder.SetPositiveButton("YES", (sender, args) =>
            {
                StartActivity(new Intent(Settings.ActionLocationSourceSettings));
                startLocationUpdatesOnResume = true;
            }).Show();
        }

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            // throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            // throw new NotImplementedException();
        }


    }
}
