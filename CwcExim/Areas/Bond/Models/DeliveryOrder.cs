using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Bond.Models
{
    public class DeliveryOrder
    {
        public int DeliveryOrderId { get; set; }

        [Display(Name = "Delivery Order No")]
        public string DeliveryOrderNo { get; set; }

        [Display(Name = "Delivery Order Date")]
        public string DeliveryOrderDate { get; set; }
        public int SpaceAppId { get; set; }
        public string DeliveryOrderXml { get; set; }
        [Display(Name = "Bond No")]
        public string BondBOENo { get; set; }

        [Display(Name = "Bond Date")]
        public string BondBOEDate { get; set; }
        public string Importer { get; set; }
        [Display(Name = "Sac No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string SacNo { get; set; }

        [Display(Name = "Sac Date")]
        public string SacDate { get; set; }

        [StringLength(500, ErrorMessage = "Remarks Cannot Be More than 500 Characters In Length")]
        public string Remarks { get; set; }
        public int CHAId { get; set; }
        public List<DeliveryOrderDtl> LstDeliveryOrder { get; set; } = new List<DeliveryOrderDtl>();
        public string CargoDescription { get; set; }
        public string GodownWiseLocXml { get; set; }
        public string GodownWiseStockXml { get; set; }        
        // [Display(Name = "Work Order No")]

        //[Required(ErrorMessage = "Fill Out This Field")]
        //public string WorkOrderNo { get; set; }

        //[Display(Name = "Work Order Date")]
        //public string WorkOrderDate { get; set; }



        //[Display(Name = "WR No")]
        //[StringLength(100, ErrorMessage = "WR No Cannot Be More Than 100 Characters In Length")]
        //public string WRNo { get; set; }

        //[Display(Name = "WR Date")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        //public string WRDate { get; set; }



        //[Required(ErrorMessage = "Fill Out This Field")]
        //public int Units { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        //[Range(0,9999999.999,ErrorMessage = "Weight Cannot Be More Than 9999999.99")]
        //public decimal Weight { get; set; }

        //[Display(Name = "Closing Units")]
        //public int ClosingUnits { get; set; }

        //[Display(Name = "Closing Weight")]
        //public decimal ClosingWeight { get; set; }


        //[Required(ErrorMessage = "Fill Out This Field")]
        //[Range(0, 9999999.999, ErrorMessage = "Value Cannot Be More Than 9999999.99")]
        //public decimal Value { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        //[Range(0, 9999999.999, ErrorMessage = "Duty Cannot Be More Than 9999999.99")]
        //public decimal Duty { get; set; }
        //public int GodownId { get; set; }

    }

    public class DeliveryOrderDtl
    {
        public int DeliveryOrderDtlId { get; set; }
        public int DeliveryOrderId { get; set; }
        public int DepositAppId { get; set; }
        public string DepositNo { get; set; }
        public string DepositDate { get; set; }
        public string WRNo { get; set; }
        public string WRDate { get; set; }
        public int Units { get; set; }
        public decimal Weight { get; set; }
        public int ClosingUnits { get; set; }
        public decimal ClosingWeight { get; set; }
        public decimal Value { get; set; }
        public decimal Duty { get; set; }
        public int GodownId { get; set; }
        public string CargoDescription { get; set; }
        public string Size { get; set; }

    }

    public class ListOfDeliveryOrder
    {
        public int DeliveryOrderId { get; set; }
        public string DeliveryOrderNo { get; set; }
        public string DeliveryOrderDate { get; set; }


        public string BondNo { get; set; }

        public string BondDate { get; set; }
        public string SACNo { get; set; }

        public string SACDate { get; set; }

        //[Display(Name = "Bond No")]
        //public string BondBOENo { get; set; }

        //[Display(Name = "Bond Date")]
        //public string BondBOEDate { get; set; }

        //[Display(Name = "Work Order No ")]
        //public string WorkOrderNo { get; set; }

        //[Display(Name = "Work Order Date ")]
        //public string WorkOrderDate { get; set; }

    }
    public class ListOfWorkOrderNo
    {
        public string SacNo { get; set; }
        public int SpaceappId { get; set; }
    }
    public class WorkOrderDetails
    {
        public int BondWOId { get; set; }
        public string BondBOENo { get; set; }
        public string BondBOEDate { get; set; }
        public string WRNo { get; set; }
        public string WRDate { get; set; }
        public int SpaceAppId { get; set; }
        public int GodownId { get; set; }
        public string WorkOrderDate { get; set; }
        public string SacNo { get; set; }
        public string SacDate { get; set; }
        public int ClosingUnits { get; set; }
        public decimal ClosingWeight { get; set; }
        public string Importer { get; set; }
        public int CHAId { get; set; }
    }

}