using System;
using System.IO;
using System.Threading.Tasks;

namespace KuberOrderApp.Interfaces
{
    public interface IPrintService
    {
        Task PrintPdfFile(Stream file, string title);
    }
}
