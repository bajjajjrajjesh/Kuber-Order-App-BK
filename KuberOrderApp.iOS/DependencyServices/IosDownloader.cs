using System;
using System.IO;
using System.Threading.Tasks;
using KuberOrderApp.Interfaces;
using KuberOrderApp.iOS.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(IosDownloader))]
namespace KuberOrderApp.iOS.DependencyServices
{
    public class IosDownloader : IDownloader
    {
        async public Task<string> CreateDownloadTask(byte[] arrayData, string folder, string FileName)
        {
            try
            {
                string pathToNewFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), folder);
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