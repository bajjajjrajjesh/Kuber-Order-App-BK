using System;
using System.Threading.Tasks;
using CoreLocation;
using Foundation;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.Models.ResponseModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;

namespace KuberOrderApp.iOS.Services
{
    public class LocationManager
    {
        protected CLLocationManager locMgr;

        // event for the location changing
        public event EventHandler<LocationUpdatedEventArgs> LocationUpdated = delegate { };
        private CLLocation mLastLocation;
        int Count;
        double NewSpeed, OldSpeed;
        public LocationManager()
        {
            this.locMgr = new CLLocationManager();

            this.locMgr.PausesLocationUpdatesAutomatically = false;

            // iOS 8 has additional permissions requirements
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                locMgr.RequestAlwaysAuthorization(); // works in background
                                                     //locMgr.RequestWhenInUseAuthorization (); // only in foreground
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                locMgr.AllowsBackgroundLocationUpdates = true;
            }

            locMgr.DesiredAccuracy = 1000;
            LocationUpdated += PrintLocation;
        }

        public CLLocationManager LocMgr
        {
            get { return this.locMgr; }
        }


        public void StartLocationUpdates()
        {
            // We need the user's permission for our app to use the GPS in iOS. This is done either by the user accepting
            // the popover when the app is first launched, or by changing the permissions for the app in Settings

            if (CLLocationManager.LocationServicesEnabled)
            {

                LocMgr.DesiredAccuracy = 1; // sets the accuracy that we want in meters

                // Location updates are handled differently pre-iOS 6. If we want to support older versions of iOS,
                // we want to do perform this check and let our LocationManager know how to handle location updates.

                if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
                {

                    LocMgr.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) => {
                        // fire our custom Location Updated event
                        this.LocationUpdated(this, new LocationUpdatedEventArgs(e.Locations[e.Locations.Length - 1]));
                    };
                }
                else
                {
                    // this won't be called on iOS 6 (deprecated). We will get a warning here when we build.
                    LocMgr.UpdatedLocation += (object sender, CLLocationUpdatedEventArgs e) => {
                        this.LocationUpdated(this, new LocationUpdatedEventArgs(e.NewLocation));
                    };
                }

                // Start our location updates
                LocMgr.StartUpdatingLocation();

                // Get some output from our manager in case of failure
                LocMgr.Failed += (object sender, NSErrorEventArgs e) => {
                    Console.WriteLine(e.Error);
                };
            }
            else
            {
                //Let the user know that they need to enable LocationServices
                Console.WriteLine("Location services not enabled, please enable this in your Settings");
                string msg = "Location services not enabled, please enable this in your Settings";
            }
        }



        //This will keep going in the background and the foreground
        async public void PrintLocation(object sender, LocationUpdatedEventArgs e)
        {
            CLLocation Currentlocation = e.Location;
            await Task.Run(() =>
            {
                if (Currentlocation == null)
                    return;

                AddLocation addLocation = new AddLocation()
                {
                    ColDDateTime = DateTime.Now,
                    ColNLatitude = Currentlocation.Coordinate.Latitude,
                    ColNLongitude = Currentlocation.Coordinate.Longitude,
                };
                MessagingCenter.Send<string, AddLocation>("True", "SetLocation", addLocation);
            });

            UIApplication.SharedApplication.EndBackgroundTask(1);

        }
    }
}
