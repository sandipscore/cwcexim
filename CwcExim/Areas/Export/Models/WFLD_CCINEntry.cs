using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class WFLD_CCINEntry
    {
        public int Id { get; set; }

        public string CCINNo { get; set; }

        public string nonApproval { get; set; }
        public string CCINDate { get; set; }

        public int SBId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [MaxLength(8,ErrorMessage ="Shipping Bill No Cannot Be Of More Than 8 Digits")]
        public string SBNo { get; set; }

       

        public string OldSBDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string SBDate { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        public int? SBType { get; set; } // 'SB Types 1. Baggage 2. Duty Free Goods 3. Cargo in Drawback',
        public int ExporterId { get; set; } // 'msteximtrader with Exporter 1',

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ExporterName { get; set; }
        public int ShippingLineId { get; set; } // 'msteximtrader with ShippingLine 1',
        public string ShippingLineName { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public int CHAId { get; set; } // 'msteximtrader with CHA 1',

        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHAName { get; set; }

        [StringLength(100, ErrorMessage = "Maximum 100 Characters allowed for Consignee.")]
        public string ConsigneeName { get; set; }

        [StringLength(200, ErrorMessage = "Maximum 200 Characters allowed for Consignee Address.")]
        public string ConsigneeAdd { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Country of destination :")]
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Port of destination :")]
        public int StateId { get; set; }
        public int SelectStateId { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "City")]
        public int CityId { get; set; }
        public int SelectCityId { get; set; }
        public int PortOfLoadingId { get; set; } // 'PortMaster with POD 1',
        public string PortOfLoadingName { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        public string PortOfDischarge { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression(@"^[1-9][0-9]*$",
        ErrorMessage = "Value must be number greater than 0")]
        public int Package { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal FOB { get; set; }
        public int CommodityId { get; set; } // 'CommodityId from mstcommodity',
        public string CommodityName { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public int CargoType { get; set; }

        public bool IsApproved { get; set; }
        public string GodownName { get; set; }
        public int GodownId { get; set; }
        public int PortOfDestId { get; set; }
        public string PortOfDestName { get; set; }
        public int NoOfVehicles { get; set; }
        public string InvoiceNo { get; set; }
        public string CartingType { get; set; }
        public int InvoiceId { get; set; }
        public string Remarks { get; set; }

        public int PartyId { get; set; }

        public string PartyName { get; set; }
        [StringLength(maximumLength:30,ErrorMessage = "Cargo Invoice No. should be less then or equal to 30 characters.")]
        public string CargoInvNo { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        public string PackageType { get; set; }
        public int PackUQCId { get; set; }
        public string PackUQCCode { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PackUQCDescription { get; set; }


        public bool IsSEZ { get; set; }

    }



    public class WFLD_CCINInvoice
    {
          // For Invoice

        public int InvoiceId { get; set; }
    public string InvoiceNo { get; set; }

        public string InvoiceType { get; set; }

        public string InvoiceDate { get; set; }
        public int RequestId { get; set; }

        public string RequestNo { get; set; }

        public string RequestDate { get; set; }
        public int PartyId { get; set; }

    [Required(ErrorMessage = "Fill Out This Field")]
    public string PartyName { get; set; }
    public decimal PartySDBalance { get; set; }
    public bool IsPartyStateInCompState { get; set; }

    public string PaymentSheetModelJson { get; set; }
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
        public decimal CWCTotal { get; set; } = 0M;
        public decimal CWCTDSPer { get; set; } = 10M;
        public decimal HTAmtTotal { get; set; } = 10M;
        public decimal HTTotal { get; set; } = 0M;
        public decimal HTTDSPer { get; set; } = 2M;
        public decimal CWCTDS { get; set; } = 0M;
        public decimal HTTDS { get; set; } = 0M;
        public decimal TDS { get; set; } = 0M;
        public decimal TDSCol { get; set; } = 0M;
        public decimal AllTotal { get; set; } = 0M;
        public decimal RoundUp { get; set; } = 0M;
        public decimal InvoiceAmt { get; set; } = 0M;
        public string ShippingLineName { get; set; } = string.Empty;
        public string CHAName { get; set; } = string.Empty;
        public string ImporterExporter { get; set; } = string.Empty;
        public string BOENo { get; set; } = string.Empty;
        public string BOEDate { get; set; } = string.Empty;
        public string CFSCode { get; set; } = string.Empty;
        public string DestuffingDate { get; set; } = string.Empty;
        public string StuffingDate { get; set; } = string.Empty;
        public string CartingDate { get; set; } = string.Empty;
        public string ArrivalDate { get; set; } = string.Empty;
        public int TotalNoOfPackages { get; set; } = 0;
        public decimal TotalGrossWt { get; set; } = 0M;
        public decimal TotalWtPerUnit { get; set; } = 0M;
        public decimal TotalSpaceOccupied { get; set; } = 0M;
        public string TotalSpaceOccupiedUnit { get; set; } = string.Empty;
        public decimal TotalValueOfCargo { get; set; } = 0M;
        public decimal PdaAdjustedAmount { get; set; } = 0M;
        public string CompGST { get; set; } = string.Empty;
        public string CompPAN { get; set; } = string.Empty;
        public string CompStateCode { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public int DeliveryType { get; set; }
        public string SEZ = string.Empty;
        public int BillToParty { get; set; } = 0;

        public int IsInGateEntry { get; set; }

   
   
    public int OTEHr { get; set; }
    public string PaymentMode { get; set; }
 public int Pos { get; set; }
        public int NoOfVehicle { get; set; }
        public List<PostPaymentCharge> lstPostPaymentChrg { get; set; } = new List<PostPaymentCharge>();
        public IList<string> ActualApplicable { get; set; } = new List<string>();
    }


    public class CCINForInvoice
    {
        public int Id { get; set; }
        public string CCINNo { get; set; }
        public string CCINDate { get; set; }
        public string SBNo { get; set; }
        public string SBDate { get; set; }
        public int ExporterId { get; set; }

        public string ExporterName { get; set; }

        public int NoOfVehicle { get; set; }

        public decimal package { get; set; }
        public decimal Weight { get; set; }
    }
    public class PackUQCForPage
    {
        public int PackUQCId { get; set; }
        public string PackUQCDescription { get; set; }
        public string PackUQCCode { get; set; }
    }
}