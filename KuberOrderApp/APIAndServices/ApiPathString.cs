using System;
namespace KuberOrderApp.APIAndServices
{
    public class ApiPathString
    {
        public static string BaseUrl = "https://test.kubersoftware.in/api/";
        public static string LoginEndPoint = "Login/Login";
        public static string LoginBySalesMan = "Login/LoginBySalesMan";
        public static string GetOTP = "Login/GetOTP";
        public static string ForgotPassword = "Login/ForgotPassword";
        public static string SaveReceiptPayment = "ReceiptPayment/Save";
        public static string GetReceiptPayment = "ReceiptPayment/ReceiptDisplay";
        public static string GetReceiptPaymentDetail = "ReceiptPayment/GetByID/";
        public static string PlaceOrder = "Order/SaveOrder";
        public static string DisplayOrder = "Order/OrderDisplay";
        public static string DisplayOrderReport = "OrderReport/OrderReport";
        public static string DisplayOrderByID = "Order/GetByID";
        public static string GetOrderImages = "Order/GetProdImageAndStockQty";
        public static string GetLedgers = "AccountReport/AccountLedgerReport";
        public static string GetLedgerDetail = "AccountReport/AccountLedgerSingleReport";
        public static string GetStock = "ProductReport/ProductLedgerReport";
        public static string GetStockDetail = "ProductReport/ProductLedgerSingleReport";
        public static string GetReceivable = "OutstandingReport/ReceivableOutstandingReport";
        public static string GetReceivableDetail = "OutstandingReport/ReceivableOutstandingSingleReport";
        public static string GetPayable = "OutstandingReport/PayableOutstandingReport";
        public static string GetPayableDetail = "OutstandingReport/PayableOutstandingSingleReport";
        public static string SaveLocation = "Location/SaveLocation";
        public static string GetDashboardsData = "Dashboard/DashboardData";
        public static string GenerateReportPDf = "Report/GenerateReportPDF";

    }
}
