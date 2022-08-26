using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using KuberOrderApp.APIAndServices;
using KuberOrderApp.Interfaces;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.Resources;
using Newtonsoft.Json;
using Plugin.XamarinFormsSaveOpenPDFPackage;
using Syncfusion.Data.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KuberOrderApp.Utilities
{
    public static class Helper
    {
        #region Display Alert
        public async static void DisplayAlert(string message)
        {
            await App.Current.MainPage.DisplayAlert(TextString.AppName, message, TextString.Ok);
        }
        #endregion

        #region SharedPreference 
        public static void SavePreference<T>(string property, T value)
        {
            Application.Current.Properties[property] = value;
            Application.Current.SavePropertiesAsync();
        }
        public static object GetPreference(string key)
        {
            if (Application.Current.Properties.ContainsKey(key))
            {
                return Application.Current.Properties[key];
            }
            else
                return null;
        }
        #endregion

        #region Search From DataTable
        public static DataTable SearchInAllColums(this DataTable table, string keyword, StringComparison comparison)
        {
           try
            {
                if (keyword.Equals(""))
                {
                    return table;
                }
                DataRow[] filteredRows = table.Rows
                       .Cast<DataRow>()
                       .Where(r => r.ItemArray.Any(
                       c => c.ToString().IndexOf(keyword, comparison) >= 0))
                       .ToArray();

                if (filteredRows.Length == 0)
                {
                    DataTable dtTemp = table.Clone();
                    dtTemp.Clear();
                    return dtTemp;
                }
                else
                {
                    return filteredRows.CopyToDataTable();
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Date Filter From DataTable
        public static DataTable FilterTable(DataTable table, DateTime startDate, DateTime endDate)
        {
            try
            {
                var filteredRows =
                from row in table.Rows.OfType<DataRow>()
                where DateTime.ParseExact(row[1].ToString(), "dd/MM/yyyy", null) >= startDate
                where DateTime.ParseExact(row[1].ToString(), "dd/MM/yyyy", null) <= endDate
                select row;


                var filteredTable = table.Clone();

                filteredRows.ToList().ForEach(r => filteredTable.ImportRow(r));

                return filteredTable;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public static DataTable dataTable = new DataTable();
        public static DataRow[] filteredRows;
        async public static Task<DataTable> FilterTableByTypes(DataTable table, string group, string category, string type, string master, StringComparison comparison)
        {
           try
            {
                dataTable = new DataTable();
                if (group.Equals("") && category.Equals("") && type.Equals("") && master.Equals(""))
                {
                    return table;
                }

                foreach (DataColumn column in table.Columns)
                    dataTable.Columns.Add(column.ColumnName, column.DataType);


                if (!string.IsNullOrWhiteSpace(group))
                {
                    var groupProducts = await App.Database.GetGroupProduct(group);
                    if(groupProducts != null && groupProducts.Count > 0)
                    {
                        var listOfColPK = groupProducts.Select(x => x.ColPK).ToList();
                        if(listOfColPK != null && listOfColPK.Count > 0)
                        {
                            var filter = table.AsEnumerable().Where(dr => listOfColPK.Contains(dr["Col99"].ToString())).CopyToDataTable();
                            if (filter != null && filter.Rows.Count > 0)
                            {
                                for (int i = 0; i < filter.Rows.Count; i++)
                                    dataTable.ImportRow(filter.Rows[i]);
                            }
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(category))
                {
                    var groupProducts = await App.Database.GetCategoryProduct(category);
                    if (groupProducts != null && groupProducts.Count > 0)
                    {
                        var listOfColPK = groupProducts.Select(x => x.ColPK).ToList();
                        if (listOfColPK != null && listOfColPK.Count > 0)
                        {
                            var filter = table.AsEnumerable().Where(dr => listOfColPK.Contains(dr["ColPK"].ToString())).CopyToDataTable();
                            if (filter != null && filter.Rows.Count > 0)
                            {
                                for (int i = 0; i < filter.Rows.Count; i++)
                                    dataTable.ImportRow(filter.Rows[i]);
                            }
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(type))
                {
                    var groupProducts = await App.Database.GetTypeProduct(type);
                    if (groupProducts != null && groupProducts.Count > 0)
                    {
                        var listOfColPK = groupProducts.Select(x => x.ColPK).ToList();
                        if (listOfColPK != null && listOfColPK.Count > 0)
                        {
                            var filter = table.AsEnumerable().Where(dr => listOfColPK.Contains(dr["ColPK"].ToString())).CopyToDataTable();
                            if (filter != null && filter.Rows.Count > 0)
                            {
                                for (int i = 0; i < filter.Rows.Count; i++)
                                    dataTable.ImportRow(filter.Rows[i]);
                            }
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(master))
                {
                    var groupProducts = await App.Database.GetMasterProduct(master);
                    if (groupProducts != null && groupProducts.Count > 0)
                    {
                        var listOfColPK = groupProducts.Select(x => x.ColPK).ToList();
                        if (listOfColPK != null && listOfColPK.Count > 0)
                        {
                            var filter = table.AsEnumerable().Where(dr => listOfColPK.Contains(dr["ColPK"].ToString())).CopyToDataTable();
                            if (filter != null && filter.Rows.Count > 0)
                            {
                                for (int i = 0; i < filter.Rows.Count; i++)
                                    dataTable.ImportRow(filter.Rows[i]);
                            }
                        }
                    }
                }

                if (dataTable != null && dataTable.Rows.Count > 0)
                    return dataTable;
                else
                {
                    return filteredRows.CopyToDataTable();
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        #endregion


        #region Order Quantity Filter
        public static DataTable FilterOrderReportByQuantity(DataTable table, string filter)
        {
            try
            {
                if (filter == "Pending")
                {
                    return table.AsEnumerable()
                                 .Where(r => Convert.ToDouble(r.Field<string>("QTY")) > 0)
                                 .CopyToDataTable();
                }
                else
                {
                    return table.AsEnumerable()
                                 .Where(r => Convert.ToDouble(r.Field<string>("QTY")) == 0)
                                 .CopyToDataTable();
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        #endregion


        #region API For PDF
        async public static Task GetPDFFileFromData(DateTime? fromDateTime = null, DateTime? toDateTime = null, int? reportId = null, string filterId = null, int? orderStatus = null, bool isFromShare = false)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");
                    MODGeneratePdf mODGeneratePdf = new MODGeneratePdf()
                    {
                        EntryORMasterID = filterId,
                    };
                    if (fromDateTime != null)
                        mODGeneratePdf.FromDate = fromDateTime.Value.ToString("dd/MM/yyyy");
                    if (toDateTime != null)
                        mODGeneratePdf.ToDate = fromDateTime.Value.ToString("dd/MM/yyyy");
                    if (reportId != null)
                        mODGeneratePdf.ReportID = reportId.Value;
                    if (orderStatus != null)
                        mODGeneratePdf.OrderStatus = orderStatus.Value;

                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Please wait...");

                    var pdfResponse = await ApiService.PostRequest<string>(ApiPathString.GenerateReportPDf, mODGeneratePdf, null);
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();

                    if (pdfResponse == null || string.IsNullOrWhiteSpace(pdfResponse))
                        return;


                    await DownloadPDFFile(pdfResponse);

                }
                catch (Exception ex)
                {
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                }
            }
        }


        async private static Task DownloadPDFFile(string pdfURL)
        {
            string filename = Path.GetFileName(pdfURL);

            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    WebClient webClient = new WebClient();
                    webClient.QueryString.Add("file", filename);
                    webClient.DownloadDataAsync(new Uri(pdfURL));
                    webClient.DownloadDataCompleted += async (s, e) =>
                    {
                        if (e.Error != null)
                        {
                            Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                            return;
                        }
                        else
                        {
                            string fileIdentifier = ((System.Net.WebClient)(s)).QueryString["file"];
                            Stream stream = new MemoryStream(e.Result);
                            await CrossXamarinFormsSaveOpenPDFPackage.Current.SaveAndView(fileIdentifier, "application/pdf", (MemoryStream)stream, PDFOpenContext.InApp);

                            Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                        }
                    };
                }
                catch (Exception ex)
                {
                    Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayAlert("Selected file is corrupted");
                    });
                    return;
                }
            });

        }
        #endregion
    }
}
