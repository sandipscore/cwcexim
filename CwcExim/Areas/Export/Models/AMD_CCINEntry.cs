using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class AMD_CCINEntry
    {
        public int Id { get; set; }

        public string CCINNo { get; set; }

        public string nonApproval { get; set; }
        public string CCINDate { get; set; }

        public int SBId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string SBNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string SBDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public int SBType { get; set; } // 'SB Types 1. Baggage 2. Duty Free Goods 3. Cargo in Drawback',
        [Required(ErrorMessage = "Fill Out This Field")]
        public int PKType { get; set; }
        public int ExporterId { get; set; } // 'msteximtrader with Exporter 1',

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ExporterName { get; set; }
        public int ShippingLineId { get; set; } // 'msteximtrader with ShippingLine 1',
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ShippingLineName { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public int CHAId { get; set; } // 'msteximtrader with CHA 1',

        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHAName { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters allowed for Consignee.")]
        public string ConsigneeName { get; set; }

        [StringLength(200, ErrorMessage = "Maximum 200 Characters allowed for Consignee Address.")]
        public string ConsigneeAdd { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Country of destination :")]
        public int CountryId { get; set; }

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

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PortOfDischarge { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression(@"^[1-9][0-9]*$",
        ErrorMessage = "Value must be number greater than 0")]
        public int Package { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression(@"((\d+)((\.\d{1,2})?))$", ErrorMessage = "Valid Decimal number with maximum 2 decimal places.")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression(@"((\d+)((\.\d{1,2})?))$", ErrorMessage = "Valid Decimal number with maximum 2 decimal places.")]
        public decimal FOB { get; set; }
        public int CommodityId { get; set; } // 'CommodityId from mstcommodity',
        public string CommodityName { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public int CargoType { get; set; }

        public bool IsApproved { get; set; }

        // For Invoice

        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public int PartyId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PartyName { get; set; }
        public decimal PartySDBalance { get; set; }
        public bool IsPartyStateInCompState { get; set; }

        public string PaymentSheetModelJson { get; set; }

        public decimal TotalAmt { get; set; }
        public decimal AllTotal { get; set; }
        public decimal RoundUp { get; set; }
        public decimal InvoiceValue { get; set; }

        public decimal TotalCGST { get; set; }
        public decimal TotalSGST { get; set; }
        public decimal TotalIGST { get; set; }

        public string Remarks { get; set; }

        public int IsInGateEntry { get; set; }

        public string GodownName { get; set; }
        public int GodownId { get; set; }
        public int PortOfDestId { get; set; }
        public string PortOfDestName { get; set; }
        public string CartingType { get; set; }
        public int OTEHr { get; set; }
        public string PaymentMode { get; set; }
        public int EntryId { get; set; } = 0;
        public int PrevEntryId { get; set; } = 0;
        public string VehicleNo { get; set; } = string.Empty;
        public string Country { get; set; }
        public string Others { get; set; }
        public List<CwcExim.Models.PostPaymentCharge> lstPostPaymentChrg { get; set; } = new List<CwcExim.Models.PostPaymentCharge>();

        //[Required(ErrorMessage = "Fill Out This Field")]
        public string PackageType { get; set; }
        public int PackUQCId { get; set; }
        public string PackUQCCode { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PackUQCDescription { get; set; }
        public bool IsSEZ { get; set; }

    }
}