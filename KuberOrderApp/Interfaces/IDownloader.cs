using System;
using System.Threading.Tasks;

namespace KuberOrderApp.Interfaces
{
    public interface IDownloader
    {
        Task<string> CreateDownloadTask(byte[] arrayData, string folder, string FileName);
    }
}
