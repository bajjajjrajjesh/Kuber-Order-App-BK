using System;
using System.IO;
using KuberOrderApp.Interfaces;
using KuberOrderApp.iOS.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDBFilePath))]
namespace KuberOrderApp.iOS.DependencyServices
{
    public class SQLiteDBFilePath : ISQLiteDBFilePath
    {
        public string GetLocalDBFilePath(string filename)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }
            return Path.Combine(libFolder, filename);
        }
    }
}
