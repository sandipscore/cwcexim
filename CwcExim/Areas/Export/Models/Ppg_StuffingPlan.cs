using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Ppg_StuffingPlan
    {
        public int StuffingPlanId { get; set; }
        public string StuffingPlanNo { get; set; }
        public string StuffingPlanDate { get; set; }
        public string SBNo { get; set; }
        public string SBDate { get; set; }
        public string StuffingPlanDtl { get; set; }
        public int Uid { get; set; }
        public int isSubmit { get; set; }
        public string ExporterName { get; set; }
        public string CHAName { get; set; }

        public List<Ppg_StuffingPlanDtl> lstStuffingPlan { get; set; } = new List<Ppg_StuffingPlanDtl>();
    }
        
    public class Ppg_StuffingPlanDtl
    {
        public string CHA { get; set; }
        public int CHAId { get; set; }
        public int PlanID { get; set; }        
        public int CargoType { get; set; }        
        public int CartingRegisterId { get; set; }
        public string CartingRegisterNo { get; set; }        
        public string Exporter { get; set; }
        public int ExporterId { get; set; }
        public decimal Fob { get; set; }        
        public int GrossWeight { get; set; }
        public int NoOfUnits { get; set; }
        public string PackUQCCode { get; set; }
        public string PackUQCDescription { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingDate { get; set; }
        public int CartingRegisterDtlId { get; set; }


    }
}