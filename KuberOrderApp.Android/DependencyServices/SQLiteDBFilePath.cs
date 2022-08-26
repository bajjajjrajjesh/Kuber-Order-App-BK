using System;
using System.IO;
using KuberOrderApp.Droid.DependencyServices;
using KuberOrderApp.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDBFilePath))]
namespace KuberOrderApp.Droid.DependencyServices
{
    public class SQLiteDBFilePath : ISQLiteDBFilePath
    {
        public string GetLocalDBFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}
