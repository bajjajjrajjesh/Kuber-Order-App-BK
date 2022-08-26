using System;
using KuberOrderApp.Models.ResponseModels;

namespace KuberOrderApp.Interfaces
{
    public interface ILocationService : ICrossPlatformService
    {
        event Action<LocationData> LocationChanged;
        event Action<string> MessageChanged;
        void OnLocationChanged(LocationData locationData);
        void OnMessageChanged(string message);
        void StartLocationUpdates();
        void StopLocationUpdates();

    }
}