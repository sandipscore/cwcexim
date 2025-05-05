using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class WFLD_StuffingPlan
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

        public List<WFLD_StuffingPlanDtl> lstStuffingPlan { get; set; } = new List<WFLD_StuffingPlanDtl>();

    }

    public class WFLD_StuffingPlanDtl
    {
        public string CHA { get; set; }
        public int CHAId { get; set; }
        public int PlanID { get; set; }
        public string CargoDescription { get; set; }

        public int CargoType { get; set; }
        public int CartingRegisterDtlId { get; set; }
        public int CartingRegisterId { get; set; }

        public string CartingRegisterNo { get; set; }

        public string CommInvNo { get; set; }

        public string CommodityName { get; set; }

        public string ContainerNo { get; set; }
        public string Exporter { get; set; }
        public int ExporterId { get; set; }
        public decimal Fob { get; set; }
        public int GodownId { get; set; }
        public string GodownName  { get; set; }
        public int GrossWeight  { get; set; }
        public int NoOfUnits  { get; set; }
       public string PackUQCCode  { get; set; }
       public string PackUQCDescription  { get; set; }
       public int PortId  { get; set; }
       public string PortName  { get; set; }
       public int RQty  { get; set; }
       public decimal RWt  { get; set; }
       public string RegisterDate  { get; set; }
       public string ShippingBillNo  { get; set; }
       public string ShippingDate  { get; set; }
       public int ShortCargoDtlId  { get; set; }
      // public decimal StuffCBM  { get; set; }
    //   public int StuffQty  { get; set; }
     //  public decimal StuffWT  { get; set; }
      // public int StuffingReqContrId { get; set; }
     //  public int StuffingReqDtlId  { get; set; }
     //  public int StuffingReqId  { get; set; }
      public decimal TotalCBM  { get; set; }
       //public decimal TotalGossWeight  { get; set; }


    }
}