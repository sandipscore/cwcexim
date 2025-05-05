using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class DSRCWCStorageCharge
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

        //[Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
        [Display(Name = "Rate")]        
        public decimal RateSqMPerWeek { get; set; }

        

        //[Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Rate")]
        [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]        
        public decimal RateCubMeterPerDay { get; set; }

        [Display(Name = "Reservation Basis")]
        [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]              
        public decimal RateSqMeterPerMonth { get; set; }

        [Display(Name = "Reservation Basis")]
        [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]        
        public decimal RateSqMeterPerWeek { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]        
        [Display(Name = "Rate")]
        public decimal RateSqMPerMonth { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Rate")]        
        [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
        public decimal RateMeterPerDay { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Rate")]        
        [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
        public decimal RateCubMeterPerWeek { get; set; }//Grid/Week

        //[Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Rate")]        
        [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
        public decimal RateCubMeterPerMonth { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Rate")]       
        [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
        public decimal ShutOutCargoRateSqMeterPerDay { get; set; }

        [Display(Name = "Size")]
        public string Size { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
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
        
        [Display(Name = "Rate")]
        [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
        public decimal RateSqMPerDay { get; set; }



        // 31.05.2019

        [Display(Name = "Rate")]
        [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
        //[RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Price can't have more than 2 decimal places")]
        public decimal RateTsaPerBe { get; set; }


        [Display(Name = "Rate")]
        [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]        
        public decimal RateMtPerDay { get; set; }

        [Display(Name = "Rate")]
        [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]        
        public decimal RateCbmPerDay { get; set; }

        [Display(Name = "Surcharge Rate")]
        [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]        
        public decimal RateSurcharge { get; set; }
        [Display(Name = "Additional Charge")]
        [Range(-100,100, ErrorMessage = "Value Should Be More Than -100 And Less Than Or Equal To 100")]
        public decimal AdditionalCharge { get; set; }
        



        [Display(Name = "Surcharge Duration")]
        public int SurchargeDuration { get; set; }

        [Display(Name = "Reservation Type")]
        public int ReservationType { get; set; }

        [Display(Name = "Is ODC")]
        public bool IsOdc { get; set; }

        [Display(Name = "Ready Made Garments")]
        public bool IsReadyMadeGarments { get; set; }

        [Display(Name = "High Secured Space")]
        public bool IsHighSecuredSpace { get; set; }

        [Display(Name = "Air Conditioned Space")]
        public bool IsAirConditionSpace { get; set; }



    }
}