using System;
using System.Runtime.Remoting.Contexts;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Widget;

namespace KuberOrderApp.Droid.Services
{
    public class JobSchedulerService : JobService
    {
        public JobSchedulerService()
        {
        }

        public override bool OnStartJob(JobParameters @params)
        {
            Intent service = new Intent(this, typeof(BackgroundLocationService));
            this.StartService(service);
            return true;
        }

        public override bool OnStopJob(JobParameters @params)
        {
            return false;
        }
    }
}