using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CwcExim.Models;

namespace CwcExim.Areas.Master.Models
{
    public class HTChargesPtyWise
    {
        public int HTChargesId { get; set; }
        [Display(Name = "Operation Type")]
        
        public int OperationType { get; set; }
        [Display(Name = "Operation Code")]
        
        public string OperationCode { get; set; }
        public int OperationId { get; set; }
        [Display(Name = "Container Type")]
        
        public int ContainerType { get; set; }
        [Display(Name = "Type")]
        
        public int Type { get; set; }
        
        [Display(Name = "Size")]

        public string Size { get; set; }
        [Display(Name = "Max Distance")]
        
        public decimal MaxDistance { get; set; }
        [Display(Name = "Rate CWC")]
        
        public decimal RateCWC { get; set; }


        

        [Display(Name = "Effective Date")]
        
        public string EffectiveDate { get; set; }
        
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
        

        public int PortId { get; set; }
        public string PortName { get; set; }

        public int PartyId { get; set; }

        public string PartyName { get; set; }
        public string CustomExam { get; set; }
        public IList<Operation> LstOperation { get; set; } = new List<Operation>();
        public IList<WeightSlabPtyWise> LstWeightSlab { get; set; } = new List<WeightSlabPtyWise>();
        public IList<DistanceSlabPtyWise> LstDistanceSlab { get; set; } = new List<DistanceSlabPtyWise>();

        public IList<ChargeListPtyWise> LstCharge { get; set; } = new List<ChargeListPtyWise>();

        public IList<Contractor> LstContractor { get; set; } = new List<Contractor>();

        public IList<ViewListPtyWise> LstviewList { get; set; } = new List<ViewListPtyWise>();
        public IList<CHNPort> LstPort = new List<CHNPort>();

    }
    public class WeightSlabPtyWise
    {
        public int WeightSlabId { get; set; }
        public int FromWeightSlab { get; set; }
        public int ToWeightSlab { get; set; }
        public bool chkWeightSlab { get; set; }
        public string Size { get; set; }
    }
    public class DistanceSlabPtyWise
    {
        public int DistanceSlabId { get; set; }
        public int FromDistanceSlab { get; set; }
        public int ToDistanceSlab { get; set; }
        public bool chkDistanceSlab { get; set; }
        public string Size { get; set; }
    }

    public class ChargeListPtyWise
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
        
        public int? PartyId { get; set; }

        public string PartyName { get; set; }
        public string CustomExam { get; set; }
    }
    public class ViewListPtyWise
    {
        public String OperationDesc { get; set; }
        public int HTChargesId { get; set; }
        public int OperationId { get; set; }
        public String EffectiveDate { get; set; }
        public String OperationCode { get; set; }
        
        public decimal RateCWC { get; set; }
        public String ChargesFor { get; set; }

        public int PartyId { get; set; }
        public string PartyName { get; set; }

    }
}