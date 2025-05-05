using CwcExim.Areas.CashManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Auction.Models
{
    public class HDB_AuctionInvoice
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
        public decimal EmdAdj { get; set; }
        public decimal AdvAdj { get; set; }
        public decimal Cess { get; set; }
        public string Remarks { get; set; }
        public decimal GstAmt { get; set; }

        public string ExportUnder { get; set; }
        public string SEZ { get; set; }
        public IList<CashReceipt> CashReceiptDetail { get; set; } = new List<CashReceipt>();
    }
    public class HDB_AuctionInvoicePrint
    {
        public string CompanyAddress { get; set; }
        public string CwcGstNo { get; set; }
        public string InvoiceNo { get; set; }
        public string TaxInvoiceDate { get; set; }
        public string PartyName { get; set; }
        public string PartyAddress { get; set; }
        public string PartyGstNo { get; set; }
        public string StateShortCode { get; set; }
        public string StateName { get; set; }
        public string AuctionAsmNo { get; set; }
        public string AuctionDate { get; set; }
        public string LotNo { get; set; }
        public string ShedNo { get; set; }
        public string SLine { get; set; }
        public string IgmNo { get; set; }
        public string OblNo { get; set; }
        public string ItemNo { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string BidderName { get; set; }
        public string BidderAddress { get; set; }
        public string BidderStateName { get; set; }
        public string Value { get; set; }
        public string Duty { get; set; }
        public string NatureOfCargo { get; set; }
        public string NoOfPkg { get; set; }
        public string TypeOfCargo { get; set; }
        public string TotalArea { get; set; }
        public string TotalWt { get; set; }

        public List<HDB_AuctionAccDetails> AuctionAccDetailsList { get; set; } = new List<HDB_AuctionAccDetails>();
        public List<HDB_ContainerDetails> ContainerDetailsList { get; set; } = new List<HDB_ContainerDetails>();
        public List<HDB_ConatinerCharge> ConatinerChargeList { get; set; } = new List<HDB_ConatinerCharge>();
    }

    public class HDB_AuctionAccDetails
    {
        public string BidAmount { get; set; }
        public string GSTPercectage { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string IGST { get; set; }
        public string TotalDues { get; set; }
        public string EMDPaid { get; set; }
        public string EMDPaidReceiptrNo { get; set; }
        public string AdvPaid { get; set; }
        public string AdvPaidReceiptrNo { get; set; }
        public string NetAmountPayable { get; set; }
    }

    public class HDB_ContainerDetails
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string DateOfArrival { get; set; }
        public string FreeDtFrom { get; set; }
        public string FreeDtUpto { get; set; }
        public string DateOfDelivery { get; set; }
        public string NoOfDays { get; set; }
        public string NoOfWeek { get; set; }
    }

    public class HDB_ConatinerCharge
    {
        public string ChargeCode { get; set; }
        public string Description { get; set; }
        public string SACode { get; set; }
        public string TaxableAmt { get; set; }
        public string CGSTPer { get; set; }
        public string CGSTAmt { get; set; }
        public string SGSTPer { get; set; }
        public string SGSTAmt { get; set; }
        public string IGSTPer { get; set; }
        public string IGSTAmt { get; set; }
        public string Total { get; set; }
    }
}