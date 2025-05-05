using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.ExpSealCheking.Models
{
    public class CHN_SealChekingJobOrder
    {
        [Display(Name = "Job Order No")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        public string JobOrderNo { get; set; }

        public string JobOrderDate { get; set; }

        [Display(Name = "Truck Slip No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string TruckSlipNo { get; set; }

        public string TruckSlipDate { get; set; }

        [Display(Name = "Container/CBT No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ContainerNo { get; set; }

        [Display(Name = "Size")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Size { get; set; }


        public string StringifyXML { get; set; }
        public string StringifyClauseXML { get; set; }
       
        public string JobOrderDetailsJS { get; set; }
        public string JobOrderClauseJS { get; set; }

        public int ImpJobOrderId { get; set; }

        public int CustomId { get; set; }

        public int BranchId { get; set;}

        public string CFSCode { get; set; }
    }

    public class CHN_JobOrderClauseDtl
    {

        public int OperationId { get; set; }
        public string OperationCode { get; set; }

    }

    public class CHN_JobOrderList
    {
        public int JobOrderId { get; set; }
        public string JobOrderNo { get; set; }
        public string JobOrderDate { get; set; }
        public string TruckSlipNo { get; set; }

        public string TruckSlipDate { get; set; }

        public string Size { get; set; }
    }

    public class PrintJobOrderSealChecking
    {
        public int JobOrderId { get; set; }

        public string JobOrderNo { get; set; }

        public string JobOrderDate { get; set; }

        public string NameOfOperation { get; set; }

        public string Exporter { get; set; }

        public decimal weight { get; set; }

        public int NoOfUnits { get; set; }

        public string TruckSlip { get; set; }
    }
}