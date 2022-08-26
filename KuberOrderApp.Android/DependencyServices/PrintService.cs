using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Print;
using KuberOrderApp.Droid.CustomFiles;
using KuberOrderApp.Interfaces;
using Plugin.CurrentActivity;

namespace KuberOrderApp.Droid.DependencyServices
{
    public class PrintService : IPrintService
    {
        public PrintService()
        {
        }

        async public Task PrintPdfFile(Stream file, string title)
        {
            try
            {
                if (file.CanSeek)
                    //Reset the position of PDF document stream to be printed
                    file.Position = 0;
                //Create a new file in the Personal folder with the given name
                string createdFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "PrintSampleFile");
                //Save the stream to the created file
                using (var dest = System.IO.File.OpenWrite(createdFilePath))
                    file.CopyTo(dest);
                string filePath = createdFilePath;
                PrintManager printManager = (PrintManager)CrossCurrentActivity.Current.Activity.GetSystemService(Context.PrintService);
                PrintDocumentAdapter pda = new CustomPrintDocumentAdapter(filePath);
                //Print with null PrintAttributes
                printManager.Print(title, pda, null);
                file.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
