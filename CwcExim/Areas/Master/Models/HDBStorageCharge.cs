using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class HDBStorageCharge
    {
      
            public int StorageChargeId { get; set; }

            [Required(ErrorMessage = "Fill Out This Field")]
            [Display(Name = "Warehouse Type")]
            public int WarehouseType { get; set; }
            public string ContainerLoadType { get; set; }

            public string WarehouseTypeName { get; set; }

            [Required(ErrorMessage = "Fill Out This Field")]
            [Display(Name = "Charge Type")]
            public int ChargeType { get; set; }

            public string ChargeTypeName { get; set; }

            [Required(ErrorMessage = "Fill Out This Field")]
            [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
            [Display(Name = "Rate")]
            public decimal RateSqMPerWeek { get; set; }

            //[Required(ErrorMessage = "Fill Out This Field")]
            //[Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
            //[Display(Name = "Rate")]
            //public decimal RateSqMPerMonth { get; set; }

            //[Required(ErrorMessage = "Fill Out This Field")]
            //[Display(Name = "Rate")]
            //[Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
            //public decimal RateMeterPerDay { get; set; }

            [Required(ErrorMessage = "Fill Out This Field")]
            [Display(Name = "Rate")]
            [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
            public decimal RateCubMeterPerDay { get; set; }

            //[Required(ErrorMessage = "Fill Out This Field")]
            //[Display(Name = "Rate")]
            //[Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
            //public decimal RateCubMeterPerWeek { get; set; }

            //[Required(ErrorMessage = "Fill Out This Field")]
            //[Display(Name = "Rate")]
            //[Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
            //public decimal RateCubMeterPerMonth { get; set; }

            /*[Required(ErrorMessage = "Fill Out This Field")]
            [Display(Name = "Rate")]
            [Range(0.1, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
            public decimal RateSqMeterPerMonth { get; set; }*/

            [Required(ErrorMessage = "Fill Out This Field")]
            [Display(Name = "Effective Date")]
            public string EffectiveDate { get; set; }
            public int Uid { get; set; }
            [Display(Name = "Days Range From")]
            public int DaysRangeFrom { get; set; }
            [Display(Name = "Days Range To")]
            public int DaysRangeTo { get; set; }

            [Required(ErrorMessage = "Fill Out This Field")]
            [Display(Name = "Sac Code")]
            public string SacCode { get; set; }

            [Required(ErrorMessage = "Fill Out This Field")]
            [Display(Name = "Commodity Type")]
            public int CommodityType { get; set; }
            public string StorageType { get; set; }
            public string AreaType { get; set; }
        
    }
    }
