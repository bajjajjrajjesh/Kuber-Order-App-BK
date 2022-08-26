using CoreLocation;
using System;
using System.Diagnostics;
using UIKit;
using KuberOrderApp.Services;
using KuberOrderApp.Models.ResponseModels;
using KuberOrderApp.Interfaces;

namespace KuberOrderApp.iOS.Services
{
    public class iOSLocationService : CrossPlatformService<ILocationService>, Interfaces.ILocationService
    {
        public event Action<LocationData> LocationChanged;
        public event Action<string> MessageChanged;

        public void OnLocationChanged(LocationData locationData)
        {

            LocationChanged?.Invoke(locationData);
        }

        public void OnMessageChanged(string message)
        {
            MessageChanged?.Invoke(message);
        }

        public void StartLocationUpdates()
        {
            var mgr = new CLLocationManager();
            MyLocationDelegate locationDelegate = new MyLocationDelegate();
            mgr.Delegate = locationDelegate;
            mgr.DesiredAccuracy = CLLocation.AccuracyBest;
            mgr.StartUpdatingLocation();

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                mgr.RequestWhenInUseAuthorization();
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                mgr.AllowsBackgroundLocationUpdates = true;
            }

            //OnLocationChanged(new LocationData(-118.2437, 34.0522, ""));
        }

        public void StopLocationUpdates()
        {
        }

        public class MyLocationDelegate : CLLocationManagerDelegate
        {
            public override void AuthorizationChanged(CLLocationManager manager, CLAuthorizationStatus status)
            {
                Debug.WriteLine("LocationManager: AuthorizationChanged: {0}", status.ToString());
            }

            public override void LocationsUpdated(CLLocationManager manager, CLLocation[] locations)
            {
                foreach (var loc in locations)
                {
                    instance.OnLocationChanged(new LocationData(loc.Coordinate.Longitude, loc.Coordinate.Latitude, loc.Speed, "GPS"));
                }
            }
        }
    }
}
