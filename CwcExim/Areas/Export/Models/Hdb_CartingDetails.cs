using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Export.Models
{
    public class Hdb_CartingDetails
    {
        public int CartingAppId { get; set; }
        public int TypeOfWork { get; set; }
        [Display(Name = "Application No.")]
        public string ApplicationNo { get; set; }
        [Display(Name = "Application Date")]
        public string ApplicationDate { get; set; }
        [Required(ErrorMessage ="Fill Out This Field")]
        public int CHAEximTraderId { get; set; }
        [MaxLength(1000,ErrorMessage = "Remarks must be in 1000 characters")]
        public string Remarks { get; set; }

    }
    public class Hdb_ShippingDetail
    {
        public int CartingAppDtlId { get; set; }
        public int CartingAppId { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingDate { get; set; }
        public string CommInvcNo { get; set; }
        public string PackingListNo { get; set; }
        public int ExporterId { get; set; }
        public string Exporter { get; set; }
        public string CargoDescription { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public int CargoType { get; set; }
        [RegularExpression("^[a-zA-Z0-9.,;:\\-() ]+$", ErrorMessage = "MarksAndNo Can Contain Only Alphabets,Numeric Digits And Special Character Like '.' , ',' , ';' , ':' , '-'")]
        public string MarksAndNo{ get; set; }
         public int NoOfUnits { get; set; }
        public decimal Weight { get; set; }
        public decimal FoBValue { get; set; }

        
    }
}