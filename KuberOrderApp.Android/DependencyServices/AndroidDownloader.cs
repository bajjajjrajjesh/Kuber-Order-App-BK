using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KuberOrderApp.Droid.DependencyServices;
using KuberOrderApp.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidDownloader))]
namespace KuberOrderApp.Droid.DependencyServices
{
    public class AndroidDownloader : IDownloader
    {
        async public Task<string> CreateDownloadTask(byte[] arrayData, string folder, string FileName)
        {
            try
            {
                string pathToNewFolder = Path.Combine(Android.OS.Environment.StorageDirectory.AbsolutePath, folder);
                string pathToNewFile = Path.Combine(pathToNewFolder, FileName);

                if (File.Exists(pathToNewFile))
                    File.Delete(pathToNewFile);

                if (!Directory.Exists(pathToNewFile))
                    Directory.CreateDirectory(pathToNewFolder);

                await File.WriteAllBytesAsync(pathToNewFile, arrayData);
                return pathToNewFile;

            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}