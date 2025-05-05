using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class DSRStorageCharge
    {
        public int StorageChargeId { get; set; }       
        public int ChargeId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Charge")]
        public string ChargeName { get; set; }        
        public int SACId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Sac Code")]
        public string SacCode { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Operation Type")]
        public string OperationType { get; set; }
        public string ContainerType { get; set; }
        public string ContainerLoadType { get; set; }        
        public string Size { get; set; }

        [Display(Name = "Is ODC")]
        public bool IsOdc { get; set; }       

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
        [Display(Name = "Rate")]
        public decimal Rate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Effective Date")]
        public string EffectiveDate { get; set; }      
        public int Uid { get; set; }
        [Display(Name = "Carting Type")]
        public string CartingType { get; set; }

        // new added
        [Display(Name = "Commodity Type")]
        public int CommodityType { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Warehouse Type")]
        public int WarehouseType { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Charge Type")]
        public int ChargeType { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
        [Display(Name = "Rate")]
        public decimal RateSqMPerWeek { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Rate")]
        [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
        public decimal RateCubMeterPerDay { get; set; }

        [Display(Name = "Days Range From")]
        public int DaysRangeFrom { get; set; }

        [Display(Name = "Days Range To")]
        public int DaysRangeTo { get; set; }

        public decimal SurCharge { get; set; }

        public string WarehouseTypeName { get; set; }
    }
}