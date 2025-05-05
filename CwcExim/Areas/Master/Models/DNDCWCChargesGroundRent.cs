using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class DNDCWCChargesGroundRent
    {
        public int GroundRentId { get; set; }
        [Display(Name = "Container Type")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int ContainerType { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public int CommodityType { get; set; }
        [Display(Name = "Days Range")]
        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(1, 9999999999, ErrorMessage = "Days Range must be greater then 1 and less then 9999999999")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Days Range From must be integer.")]
        public int DaysRangeFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 9999999999, ErrorMessage = "Days Range must be greater then 0 and less then 9999999999")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Days Range To must be integer.")]
        public int DaysRangeTo { get; set; }
        [Display(Name = "Rent Amount")]
        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999999.99, ErrorMessage = "Rent Amount must be  less then 99999999.99")]
        public decimal RentAmount { get; set; }
        /*[Display(Name = "Reefer")]
        public bool Reefer { get; set; }*/
        [Display(Name = "Size")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Size { get; set; }
        [Display(Name = "Operation Type")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int OperationType { get; set; }
        /*[Display(Name = "Electricity Charge")]
        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0.1, 99999999.99, ErrorMessage = "Electricity Charge must be  less then 99999999.99")]
        public decimal ElectricityCharge { get; set; }*/
        [Display(Name = "Effective Date")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string EffectiveDate { get; set; }
        [Display(Name = "SacCode")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string SacCode { get; set; }
        public bool IsODC { get; set; }
        public bool IsITP { get; set; }
    }
}