using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CwcExim.Areas.CashManagement.Models;


namespace CwcExim.Areas.Auction.Models
{
    public class Kol_AuctionInvoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceNo { get; set; }

        [Required]
        public string AssesmentInvoiceNo { get; set; }

        public string AssesmentInvoiceID { get; set; }
        public string InvoiceDate { get; set; }
        [Required]
        public int BIDId { get; set; }
        [Required]
        public string BIDNo { get; set; }
        public string BidDate { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyAddress { get; set; }
        public string GSTNo { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string ReceiptDate { get; set; }
        public string DeliveryType { get; set; }
        public string DeliveryDate { get; set; }
        public string ContainerNo { get; set; }
        public string DeliveryDateUpto { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string AssesmentType { get; set; }
        public string OBL { get; set; }
        public string ShippingBill { get; set; }
        public string CargoDescription { get; set; }
        public string AssesmentSheetDate { get; set; }
        public string FreeUpto { get; set; }
        public Decimal? CustomDuty { get; set; }
        public Decimal? OT { get; set; }
        public Decimal? ValuersCharges { get; set; }
        public Decimal? AuctionCharges { get; set; }
        public Decimal? MISCExpenses { get; set; }
        public Decimal? CWCShare { get; set; }

        public Decimal? BidAmount { get; set; }
        public Decimal? EmdAmount { get; set; }
        public Decimal? AdvanceAmount { get; set; }
        public Decimal? NetPayable { get; set; }

        [Required]
        public string HSNCode { get; set; }

        [Required]
        public int GST { get; set; }

        public Decimal? TDSAmount { get; set; }
        public decimal PartySDBalance { get; set; }

        public string PaymentSheetModelJson { get; set; }

        public decimal TotalAmt { get; set; }
        public decimal AllTotal { get; set; }
        public decimal RoundUp { get; set; }
        public decimal InvoiceValue { get; set; }

        public decimal TotalCGST { get; set; }
        public decimal TotalSGST { get; set; }
        public decimal TotalIGST { get; set; }
        public int TotalNoOfPackages { get; set; }
        public decimal TotalGrossWt { get; set; }

        public string Remarks { get; set; }

        public string SEZ { get; set; }

        public string ServiceType { get; set; }
       
    }
}