using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Bond.Models
{
    public class DndBondWODeli
    {
        public int BondWOId { get; set; }
        public int DepositAppId { get; set; }

        //[Display(Name = "Work Order No")]
        //public string DelWorkOrderNo { get; set; }

        //[Display(Name = "Work Order Date ")]
        //public string DelWorkOrderDate { get; set; }

        [Display(Name = "Work Order No")]
        public string WorkOrderNo { get; set; }

        [Display(Name = "Work Order Date ")]
        public string WorkOrderDate { get; set; }
        [Display(Name = "Bond No")]
        public string BondNo { get; set; }
        [Display(Name = "Bond Date")]
        public string BondDate { get; set; }

        [Display(Name = "WR No")]
        public string WRNo { get; set; }

        [Display(Name = "WR Date")]
        public string WRDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]

        [Display(Name = "Delivery No")]
        public string DeliveryNo { get; set; }

        [Display(Name = "Godown")]
        public string GodownName { get; set; }

        [Display(Name = "Cargo Description")]
        public string CargoDescription { get; set; }

        [Display(Name = "Sac No")]
        public string SacNo { get; set; }


        [Display(Name = "Sac Date")]
        public string SacDate { get; set; }

        [Display(Name = "Cargo Units")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Cargo Units Must Be Integer")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int Units { get; set; }

        [Display(Name = "Cargo Weight")]
        [RegularExpression("^[0-9.]+$", ErrorMessage = "Cargo Weight Must Be Decimal")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal Weight { get; set; }
        public string WorkOrderFor { get; set; }
    }
    public class ListOfDndBondWODeli
    {
        public int BondWOId { get; set; }
        public string BondBOENo { get; set; }
        public string BondBOEDate { get; set; }
        public string WorkOrderNo { get; set; }
        public string WorkOrderDate { get; set; }
        public string SacNo { get; set; }
    }
    public class ListOfDndBondNo
    {
        public string BondBOENo { get; set; }
        public int DepositAppId { get; set; }
    }
}