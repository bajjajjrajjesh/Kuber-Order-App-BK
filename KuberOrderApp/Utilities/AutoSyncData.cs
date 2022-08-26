using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KuberOrderApp.APIAndServices;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.Models.ResponseModels;

namespace KuberOrderApp.Utilities
{
    public class AutoSyncData
    {
        async public static void AutoSyncPendingData()
        {
            await SyncPendingOrders();
            await SyncPendingReceipt();
        }

        #region SyncReceiptData
        async private static Task SyncPendingReceipt()
        {
            List<AddPaymentAndReceiptRequest> addPaymentAndReceiptRequests = await App.Database.GetDataList<AddPaymentAndReceiptRequest>();
            if (addPaymentAndReceiptRequests != null && addPaymentAndReceiptRequests.Count > 0)
            {
                foreach (var receipt in addPaymentAndReceiptRequests)
                {
                    receipt.Col99 = "";
                    receipt.Col97 = "";
                    var response = await ApiService.PostRequest<CommonResponseModel>(ApiPathString.SaveReceiptPayment, receipt, null);
                    if (response != null && response.status)
                        await App.Database.DeleteData<AddPaymentAndReceiptRequest>(receipt);
                }
            }
        }

        async private static Task SyncPendingOrders()
        {
            List<SaveProductOrder> productOrders = await App.Database.GetDataList<SaveProductOrder>();
            if (productOrders != null && productOrders.Count > 0)
            {
                var response = await ApiService.PostRequest<CommonResponseModel>(ApiPathString.PlaceOrder, productOrders, null);
                if(response != null && response.status)
                {
                    foreach(var product in productOrders)
                        await App.Database.DeleteData<SaveProductOrder>(product);
                }
            }
        }
        #endregion
    }
}
