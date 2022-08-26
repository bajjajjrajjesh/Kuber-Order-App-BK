using System;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using KuberOrderApp.Interfaces;
using UIKit;

namespace KuberOrderApp.iOS.DependencyServices
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
                var printInfo = UIPrintInfo.PrintInfo;
                printInfo.OutputType = UIPrintInfoOutputType.General;
                printInfo.JobName = title;

                //Get the path of the MyDocuments folder
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                //Get the path of the Library folder within the MyDocuments folder
                var library = Path.Combine(documents, "..", "Library");
                //Create a new file with the input file name in the Library folder
                var filepath = Path.Combine(library, "PrintSampleFile");

                //Write the contents of the input file to the newly created file
                using (MemoryStream tempStream = new MemoryStream())
                {
                    file.Position = 0;
                    file.CopyTo(tempStream);
                    File.WriteAllBytes(filepath, tempStream.ToArray());
                }

                var printer = UIPrintInteractionController.SharedPrintController;
                printInfo.OutputType = UIPrintInfoOutputType.General;

                printer.PrintingItem = NSUrl.FromFilename(filepath);
                printer.PrintInfo = printInfo;


                printer.ShowsPageRange = true;

                printer.Present(true, (handler, completed, err) =>
                {
                    if (!completed && err != null)
                    {
                        Console.WriteLine("error");
                    }
                });
                file.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
