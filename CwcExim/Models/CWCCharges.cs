using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Models
{
    public class CWCCharges
    {
        public int CwcChargesId { get; set; }
        [Display(Name ="With Effect From")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string EffectiveDate { get; set; }
        /*Ground Rent*/
        
        /*Godown*/
        [Display(Name = "Warehouse Type")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int WarehouseType { get; set; }
        [Display(Name = "Charge Type")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int ChargesType { get; set; }
        [Display(Name ="Rate")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal SqMRate { get; set; }
        [Display(Name = "Rate")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal MPerDayRate { get; set; }
        [Display(Name = "Rate")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal CuMRate { get; set; }
        [Display(Name = "Rate")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal SqMeterPerMonthRate { get; set; }
        /*Weighment*/
        [Display(Name = "Container Rate")]
        [Required(ErrorMessage ="Fill Out This Field")]
        public decimal ContainerRate { get; set; }
        [Display(Name = "Truck Rate")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal TruckRate { get; set; }
        /******/
        /*Miscellaneous*/
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal Fumigation { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal Washing { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal Reworking { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal Bagging { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal Palletizing { get; set; }
        /***************/
        /*Entry Fees*/
        [Display(Name ="Container Type")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int EFContainerType1 { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public int EFContainerType2 { get; set; }
        [Display(Name ="Operation Type")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int EFOperationType { get; set; }
        [Display(Name ="Rate")]
        [Required(ErrorMessage ="Fill Out This Field")]
        public decimal EFRate { get; set; }
        [Display(Name ="Reefer")]
        public bool EFReefer { get; set; }
        /***************/
        /*Insurance Charge*/
        [Display(Name ="Charge")]
        [Required(ErrorMessage ="Fill Out This Field")]
        public decimal InsCharge { get; set; }
        /*****************/

    }
}