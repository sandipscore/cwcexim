using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class DSRFumigationCharge
    {
        public int FumigationChargeId { get; set; }

        public string StringifyData { get; set; }

      // [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Cargo")]
        public string Cargo { get; set; }

        public string Type { get; set; }

       // [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Container")]
        public int Container { get; set; }

       // [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Container Size")]
        public string ContainerSize { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
       // [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
        [Display(Name = "Area From")]
        public decimal? FromWeight { get; set; }

      //  [Required(ErrorMessage = "Fill Out This Field")]
       // [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
        [Display(Name = "Area To")]
        public decimal? ToWeight { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
       // [Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
        [Display(Name = "Rate")]
        public decimal? WeightRate { get; set; }

       // [Required(ErrorMessage = "Fill Out This Field")]
       //[Range(0, 99999999.99, ErrorMessage = "Rate Should Be More Than 0 And Less Than Or Equal To 99999999.99")]
        [Display(Name = "Rate")]
        public decimal? SizeRate { get; set; }

        public decimal? Rate { get; set; }

        public string ChargesFor { get; set; }
        [Display(Name = "Effective Date")]
        public string EffectiveDate { get; set; }

        public IList<DSRFumigationChargeDetailsForCargo> lstChargeForCargo { get; set; } = new List<DSRFumigationChargeDetailsForCargo>();

        public IList<DSRFumigationChargeDetailsForContainer> lstChargeForContainer { get; set; } = new List<DSRFumigationChargeDetailsForContainer>();
    }

    public class DSRFumigationChargeDetailsForCargo
    {
        public decimal Fromweight { get; set; }

        public decimal Toweight { get; set; }

        public decimal WeightRate { get; set; }
        public string EffectiveDate { get; set; }


    }
    public class DSRFumigationChargeDetailsForContainer
    {
        public string ContainerSize { get; set; }

        public decimal SizeRate { get; set; }
        public string EffectiveDate { get; set; }
    }


}