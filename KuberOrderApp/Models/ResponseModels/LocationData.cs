using System;
namespace KuberOrderApp.Models.ResponseModels
{
    public class LocationData
    {
        private double latitude;
        private double longitude;
        private double speed_mps;
        private string provider;

        public LocationData()
        {
        }

        public LocationData(double longitude, double latitude, double speed_mps, string provider)
        {
            this.longitude = longitude;
            this.latitude = latitude;
            this.speed_mps = speed_mps;
            this.provider = provider;
        }

        public double Latitude
        {
            get
            {
                return latitude;
            }
        }

        public double Longitude
        {
            get
            {
                return longitude;
            }
        }

        public string Provider
        {
            get
            {
                return provider;
            }
        }

        public double Speed
        {
            get
            {
                return speed_mps;
            }
        }
    }
}
