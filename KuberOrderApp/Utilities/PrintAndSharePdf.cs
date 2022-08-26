using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using KuberOrderApp.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KuberOrderApp.Utilities
{
    public class PrintAndSharePdf
    {
        async public static Task PrintPDF(string pdfURL, string title)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(pdfURL);
            MemoryStream stream = new MemoryStream(byteArray);

            await DependencyService.Get<IPrintService>().PrintPdfFile(stream, title);
        }

        async public static Task SharePDFFile(string pdfURL)
        {
            Device.BeginInvokeOnMainThread(async()=>
            {
                string contentType = "application/pdf";

                byte[] byteArray = Encoding.UTF8.GetBytes(pdfURL);
                MemoryStream stream = new MemoryStream(byteArray);

                ShareFile share = new ShareFile(pdfURL, contentType);
                ShareFileRequest request = new ShareFileRequest(Path.GetFileName(pdfURL), share);
                await Share.RequestAsync(request);
            });
        }
    }
}
