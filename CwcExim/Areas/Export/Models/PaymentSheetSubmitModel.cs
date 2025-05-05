using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class PaymentSheetSubmitModel
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerState { get; set; }
        public int CustomerStateId { get; set; }
        public string CustomerStateCode { get; set; }
        public string GSTIn { get; set; }
        public decimal ReverseChargeAmt { get; set; }
        public string MyProperty { get; set; }
        public int IsRegistered { get; set; }
        public decimal TotalValue { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalTaxableAmt { get; set; }
        public decimal TotalCGSTAmt { get; set; }
        public decimal TotalSGSTAmt { get; set; }
        public decimal TotaIGSTAmt { get; set; }
        public decimal TotalInvoiceAmt { get; set; }

        IList<PaymentSheetSubmitDtl> lstPaysheetSubmitDtl { get; set; } = new List<PaymentSheetSubmitDtl>();
    }

    public class PaymentSheetSubmitDtl
    {
        public int InvoiceDtlId { get; set; }
        public int InvoiceId { get; set; }
        public int InvoiceDtlSerial { get; set; }
        public string ServCode { get; set; }
        public string ServiceDexc { get; set; }
        public string AccountingCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Value { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal TaxableAmt { get; set; }
        public decimal CGSTRate { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTRate { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTRate { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal TotalValue { get; set; }
    }
}
