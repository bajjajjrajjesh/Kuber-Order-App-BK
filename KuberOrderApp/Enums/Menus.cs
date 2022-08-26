using System;
using System.ComponentModel;

namespace KuberOrderApp.Enums
{
        public enum Menus
        {
            [Description("Dashboard")]
            Home,

            [Description("Order Add")]
            OrderAdd,

            [Description("Order Display")]
            OrderDisplay,

            [Description("Cart")]
            Cart,

            [Description("Receipt Add")]
            ReceiptPaymentAdd,

            [Description("Receipt Display")]
            ReceiptPaymentDisplay,

            [Description("Outstanding Receivable")]
            OutstandingReceivable,

            [Description("Order Report")]
            OrderReport,

            [Description("Account Ledger")]
            Ledger,

            [Description("Stock Reports")]
            Stock,

            [Description("Outstanding Payable")]
            OutStandingPayable,

            [Description("Address Book")]
            AddressBook,

            [Description("Share Application")]
            ShareApplication,

            [Description("Feedback")]
            Feedback,

            [Description("Contact Us")]
            ContactUs,

            [Description("LogOut")]
            LogOut,
        }
}
