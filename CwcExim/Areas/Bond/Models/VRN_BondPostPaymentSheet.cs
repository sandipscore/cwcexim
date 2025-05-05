using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;


namespace CwcExim.Areas.Bond.Models
{
    public class VRN_BondPostPaymentSheet
    {
        private decimal _CWCTotal = 0M;
        private decimal _HTTotal = 0M;
        private decimal _CWCTDS = 0M;
        private decimal _HTTDS = 0M;
        public IList<string> ActualApplicable { get; set; } = new List<string>();
        public string ApproveOn { get; set; } = string.Empty;
        public string ROAddress { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyAddress { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public int StateId { get; set; }
        public string StateCode { get; set; }
        public int CityId { get; set; }
        public string GstIn { get; set; }
        public string Pan { get; set; }
        public int InvoiceId { get; set; } = 0;
        public string InvoiceType { get; set; } = string.Empty;
        public string InvoiceNo { get; set; } = string.Empty;
        public string InvoiceDate { get; set; } = string.Empty;
        public string UptoDate { get; set; } = string.Empty;
        public int RequestId { get; set; } = 0;
        public string RequestNo { get; set; } = string.Empty;
        public string RequestDate { get; set; } = string.Empty;
        public int PartyId { get; set; } = 0;
        public string PartyName { get; set; } = string.Empty;
        public string PartyAddress { get; set; } = string.Empty;
        public string PartyState { get; set; } = string.Empty;
        public string PartyStateCode { get; set; } = string.Empty;
        public string PartyGST { get; set; } = string.Empty;
        public int PayeeId { get; set; } = 0;
        public string PayeeName { get; set; } = string.Empty;
        public decimal TotalAmt { get; set; } = 0M;
        public decimal TotalDiscount { get; set; } = 0M;
        public decimal TotalTaxable { get; set; } = 0M;
        public decimal TotalCGST { get; set; } = 0M;
        public decimal TotalSGST { get; set; } = 0M;
        public decimal TotalIGST { get; set; } = 0M;
        public decimal CWCAmtTotal { get; set; } = 10M;
        public decimal CWCTotal
        {
            get { return _CWCTotal; }
            set { _CWCTotal = value; }
        }
        public decimal CWCTDSPer { get; set; } = 10M;
        public decimal HTAmtTotal { get; set; } = 10M;
        public decimal HTTotal
        {
            get { return _HTTotal; }
            set { _HTTotal = value; }
        }
        public decimal HTTDSPer { get; set; } = 2M;
        public decimal CWCTDS
        {
            get { return _CWCTDS; }
            set { _CWCTDS = value; }
        }
        public decimal HTTDS
        {
            get { return _HTTDS; }
            set { _HTTDS = value; }
        }
        public decimal TDS { get; set; } = 0M;
        public decimal TDSCol { get; set; } = 0M;
        public decimal AllTotal { get; set; } = 0M;
        public decimal RoundUp { get; set; } = 0M;
        public decimal InvoiceAmt { get; set; } = 0M;
        public string CHAName { get; set; } = string.Empty;
        public string ImporterExporter { get; set; } = string.Empty;
        public int TotalNoOfPackages { get; set; } = 0;
        public decimal TotalGrossWt { get; set; } = 0M;
        public decimal TotalWtPerUnit { get; set; } = 0M;
        public decimal Area { get; set; } = 0M;
        public string TotalSpaceOccupiedUnit { get; set; } = "SQM";
        public string CompGST { get; set; } = string.Empty;
        public string CompPAN { get; set; } = string.Empty;
        public string CompStateCode { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public int DeliveryType { get; set; } = 0;
        public string BillType { get; set; } = string.Empty;
        public string InvoiceHtml { get; set; } = string.Empty;
        public decimal Weight { get; set; } = 0M;
        public decimal CIFValue { get; set; } = 0M;
        public decimal Duty { get; set; } = 0M;
        public int Units { get; set; } = 0;
        public int IsInsured { get; set; } = 0;

        public string DepositDate { get; set; } = string.Empty;
        public string BOENo { get; set; } = string.Empty;
        public string BOEDate { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string DeliveryDate { get; set; } = string.Empty;
        public string ExportUnder { get; set; } = string.Empty;
        public IList<VRN_PostPaymentCharge> lstPostPaymentChrg { get; set; } = new List<VRN_PostPaymentCharge>();       
        public string PaymentMode { get; set; }
    }

    public class VRN_ListOfBondInvoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PartyName { get; set; }
        public string Module { get; set; }
        public string ModuleName { get; set; }
    }
    public class BndPartyForpage
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }

        public string PartyCode { get; set; }
    }
}