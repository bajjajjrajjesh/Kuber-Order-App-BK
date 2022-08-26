using System;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Xamarin.Forms;

namespace KuberOrderApp.Droid.Services
{
    [BroadcastReceiver]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class BootCompleteReceiver : BroadcastReceiver
    {
        public event Action<string> MessageChanged;
        private LocationManager locationManager;
        string locationProvider = null;
        public override void OnReceive(Context context, Intent intent)
        {
            //Toast.MakeText(context, "ONBOOT CALL With Location Enable", ToastLength.Long).Show();
            Intent service = new Intent(context, typeof(BackgroundLocationService));
            context.StartService(service);

        }
    }
}
