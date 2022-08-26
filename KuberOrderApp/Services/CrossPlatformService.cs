using System;
using KuberOrderApp.Interfaces;

namespace KuberOrderApp.Services
{
    public abstract class CrossPlatformService<T> where T : ICrossPlatformService
    {
        protected static T instance;

        public static T Instance
        {
            get
            {
                return instance;
            }
        }

        public static void Init<T1>() where T1 : T, new()
        {
            instance = new T1();
        }
    }
}
