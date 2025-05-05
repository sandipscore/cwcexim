using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CwcExim.Models;

namespace CwcExim.Areas.Master.Models
{
    public class DNDHTCharges
    {
        public int HTChargesId { get; set; }
        [Display(Name = "Operation Type")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        public int OperationType { get; set; }
        [Display(Name = "Operation Code")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        public string OperationCode { get; set; }
        public int OperationId { get; set; }
        [Display(Name = "Container Type")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        public int ContainerType { get; set; }
        [Display(Name = "Type")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        public int Type { get; set; }
        /*[Display(Name = "Description")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Description { get; set; }*/
        [Display(Name = "Size")]

        public string Size { get; set; }
        [Display(Name = "Max Distance")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        public decimal MaxDistance { get; set; }
        [Display(Name = "Rate CWC")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        public decimal RateCWC { get; set; }


        /*[Display(Name = "Contractor")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int ContractorId { get; set; }*/

        [Display(Name = "Effective Date")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        public string EffectiveDate { get; set; }
        /*public string ContractorName { get; set; }*/
        [Display(Name = "Contractor Rate")]
        public decimal ContractorRate { get; set; } = 0M;

        public decimal RoundTripRate { get; set; }
        public decimal EmptyRate { get; set; }
        public string OperationDesc { get; set; }
        public string ContainerLoadType { get; set; }
        public string TransportFrom { get; set; }
        public string EximType { get; set; }
        public String ChargeList { get; set; }
        public int SlabType { get; set; }
        public string ChargesFor { get; set; }

        [Display(Name = "Commodity Type")]
        public int CommodityType { get; set; }


        public int WeightSlab { get; set; }
        public int DistanceSlab { get; set; }

        public decimal AddlWtCharges { get; set; }
        public decimal AddlDisCharges { get; set; }
        public int IsODC { get; set; }
        //public decimal PortToDistance { get; set; }

        public int PortId { get; set; }
        public string PortName { get; set; }
        public IList<Operation> LstOperation { get; set; } = new List<Operation>();
        public IList<DNDWeightSlab> LstWeightSlab { get; set; } = new List<DNDWeightSlab>();
        public IList<DNDDistanceSlab> LstDistanceSlab { get; set; } = new List<DNDDistanceSlab>();

        public IList<DNDChargeList> LstCharge { get; set; } = new List<DNDChargeList>();

        public IList<Contractor> LstContractor { get; set; } = new List<Contractor>();

        public IList<DNDViewList> LstviewList { get; set; } = new List<DNDViewList>();
        public IList<DNDPort> LstPort = new List<DNDPort>();
        public bool IsITP { get; set; }

    }
    public class DNDWeightSlab
    {
        public int WeightSlabId { get; set; }
        public int FromWeightSlab { get; set; }
        public int ToWeightSlab { get; set; }
        public bool chkWeightSlab { get; set; }
        public string Size { get; set; }
    }
    public class DNDDistanceSlab
    {
        public int DistanceSlabId { get; set; }
        public int FromDistanceSlab { get; set; }
        public int ToDistanceSlab { get; set; }
        public bool chkDistanceSlab { get; set; }
        public string Size { get; set; }
    }

    public class DNDChargeList
    {
        public int ID { get; set; }
        public int WtSlabId { get; set; }
        public int FromWtSlabCharge { get; set; }
        public int ToWtSlabCharge { get; set; }
        public int DisSlabId { get; set; }
        public int FromDisSlabCharge { get; set; }
        public int ToDisSlabCharge { get; set; }
        public decimal CwcRate { get; set; }
        public decimal ContractorRate { get; set; }
        public int SlabType { get; set; }
        public int WeightSlab { get; set; }
        public int DistanceSlab { get; set; }
        public decimal RoundTripRate { get; set; }
        public decimal EmptyRate { get; set; }
        public decimal AddlWtCharges { get; set; }
        public decimal AddlDisCharges { get; set; }
        public int PortId { get; set; }
        public string PortName { get; set; }
        //public decimal PortFromDistance { get; set; }
        //public decimal PortToDistance { get; set; }
    }
    public class DNDViewList
    {
        public String OperationDesc { get; set; }
        public int HTChargesId { get; set; }
        public int OperationId { get; set; }
        public String EffectiveDate { get; set; }
        public String OperationCode { get; set; }
        //public int WeightSlab { get; set; }
        public decimal RateCWC { get; set; }
        public String ChargesFor { get; set; }


    }
}