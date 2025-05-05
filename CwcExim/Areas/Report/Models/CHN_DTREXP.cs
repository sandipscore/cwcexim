using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class CHN_DTREXP
    {

       
            public IList<CHN_CartingDetails> lstCartingDetails { get; set; } = new List<CHN_CartingDetails>();
            public IList<CHN_CartingDetails> lstShortCartingDetails { get; set; } = new List<CHN_CartingDetails>();
          
        public IList<CHN_CargoAcceptingDetails> lstCargoShifting { get; set; } = new List<CHN_CargoAcceptingDetails>();
            public IList<CHN_CargoAcceptingDetails> lstCargoAccepting { get; set; } = new List<CHN_CargoAcceptingDetails>();
            public IList<CHN_BTTDetails> lstBTTDetails { get; set; } = new List<CHN_BTTDetails>();
        //public IList<CHN_StuffingDetails> lstStuffingDetails { get; set; } = new List<CHN_StuffingDetails>();

            public IList<CHN_DTRStuffing> lstDTRStuffing { get; set; } = new List<CHN_DTRStuffing>();
            public IList<CHN_DRTSBDetails> lstDRTSBDetails { get; set; } = new List<CHN_DRTSBDetails>();
            public IList<CHN_StockDetails> StockOpening { get; set; } = new List<CHN_StockDetails>();
            public IList<CHN_StockDetails> StockClosing { get; set; } = new List<CHN_StockDetails>();

        
    }


    
    public class CHN_CartingDetails
    {

        public string RegisterNo { get; set; } 
        public string ShippingBillNo { get; set; } 
        public string ShippingBillDate { get; set; } 
        public string ExporterName { get; set; } 
        public string CargoDescription { get; set; } 
        public int ShippingLineId { get; set; } 
        public decimal ActualQty { get; set; } 
        public decimal ActualWeight { get; set; } 
        public decimal FobValue { get; set; }
        public string Slot { get; set; } 
        public string GR { get; set; } 
        public decimal Area { get; set; } 
        public string Remarks { get; set; }
        public string SLA { get; set; } 
        public string ShippingLineName { get; set; } 
        public string CFSCode { get; set; } 




    }
    public class CHN_DTRStuffing
    {
        public int StuffingReqId { get; set; } 
        public string StuffingReqNo { get; set; }
        public string PartyName { get; set; } 
        public string ContainerNo { get; set; } 
        public string CFSCode { get; set; } 
        public string Size { get; set; } 
        public string CustomSealNo { get; set; } 
        public string Remarks { get; set; } 
        public int ReqCount { get; set; } 

    }
    public class CHN_DRTSBDetails
    {
        public int StuffingReqId { get; set; } 
        public string StuffingReqNo { get; set; }
        public string CFSCode { get; set; } 
        public string SBNoDate { get; set; } 
        public string PartyName { get; set; } 

        public string CartingNoDate { get; set; }
        public string CargoDescription { get; set; }
        public string ExporterName { get; set; }
        public decimal StuffQuantity { get; set; }
        public decimal StuffWeight { get; set; }
        public decimal FobValue { get; set; } 
        public string SBNo { get; set; }
        public string ContainerNo { get; set; } 
        public string Size { get; set; }
        public string CustomSealNo { get; set; } 
        public string Remarks { get; set; } 

    }
    
    public class CHN_CargoAcceptingDetails
    {
        public string RegisterNo { get; set; } 

        public string ShippingBillNo { get; set; } 
        public string ShippingBillDate { get; set; } 

        public string ExporterName { get; set; } 
        public string CargoDescription { get; set; } 
        public decimal ActualQty { get; set; }
        public decimal ActualWeight { get; set; }
        public decimal FobValue { get; set; }
        public string LocationName { get; set; } 
        public string GR { get; set; } 
        public decimal SQM { get; set; } 
        public string FromGodown { get; set; }
        public string ToGodown { get; set; } 
        public string FromShippingLine { get; set; }
        public string ToShippingLine { get; set; }
        public string CFSCode { get; set; } 

    }


    
    public class CHN_BTTDetails
    {
        public string RegisterNo { get; set; } 

        public string ShippingBillNo { get; set; } 
        public string ShippingBillDate { get; set; } 
        public string ExporterName { get; set; } 
        public string CargoDescription { get; set; }
        public decimal BTTQuantity { get; set; } 
        public decimal BTTWeight { get; set; } 
        public decimal Fob { get; set; } 

        public string LocationName { get; set; } 
        public string GR { get; set; } 
        public decimal Area { get; set; }
        public string CFSCode { get; set; } 

    }

    

    
    public class CHN_StuffingDetails
    {
        public string StuffingNo { get; set; } 
        public string SLA { get; set; } 


        public string StuffingDate { get; set; } 
        public string ShippingBillNoDate { get; set; }
        public string CartingNo { get; set; } 
        public string EntryDateTime { get; set; } 
        public string CargoDescription { get; set; } 
        public decimal StuffQuantity { get; set; } 
        public decimal StuffWeight { get; set; } 
        public decimal Fob { get; set; } 

        public string ContainerNo { get; set; } 
        public string CustomSeal { get; set; } 
        public string Size { get; set; } 
        public string Remarks { get; set; } 
        public string CFSCode { get; set; } 










    }



  
    public class CHN_StockDetails
    {
        public string ShippingBillNo { get; set; }
        public decimal Units { get; set; }
        public decimal Weight { get; set; } 
        public decimal Fob { get; set; } 
    }

    public class CHN_StockDetailsEcel
    {
        public int noofsb { get; set; } = 0;
        public decimal Units { get; set; } = 0;
        public decimal Weight { get; set; } = 0;
        public decimal Fob { get; set; } = 0;
    }
    public class CHN_SummaryExcel
    {
        public string Desc { get; set; } = string.Empty;
        public int Countsb { get; set; } = 0;
        public decimal Units { get; set; } = 0;
        public decimal Weight { get; set; } = 0;
        public decimal Fob { get; set; } = 0;
    }
    public class CHN_StuffingDetailsExcel
    {
        public string StuffingNo { get; set; } = string.Empty;
        public string SLA { get; set; } = string.Empty;


        public string StuffingDate { get; set; } = string.Empty;
        public string ShippingBillNoDate { get; set; } = string.Empty;
        public string CartingNo { get; set; } = string.Empty;
        public string EntryDateTime { get; set; } = string.Empty;
        public string CargoDescription { get; set; } = string.Empty;
        public decimal StuffQuantity { get; set; } = 0;
        public decimal StuffWeight { get; set; } = 0;
        public decimal Fob { get; set; } = 0;

        public string ContainerNo { get; set; } = string.Empty;
        public string CustomSeal { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;











    }
    public class CHN_StockDetailsExcel
    {
        public string ShippingBillNo { get; set; } = string.Empty;
        public decimal Units { get; set; } = 0;
        public decimal Weight { get; set; } = 0;
        public decimal Fob { get; set; } = 0;
    }
    public class CHN_BTTDetailsExcel
    {
        public string RegisterNo { get; set; } = string.Empty;

        public string ShippingBillNo { get; set; } = string.Empty;
        public string ShippingBillDate { get; set; } = string.Empty;
        public string ExporterName { get; set; } = string.Empty;
        public string CargoDescription { get; set; } = string.Empty;
        public decimal BTTQuantity { get; set; } = 0;
        public decimal BTTWeight { get; set; } = 0;
        public decimal Fob { get; set; } = 0;

        public string LocationName { get; set; } = string.Empty;
        public string GR { get; set; } = string.Empty;
        public decimal Area { get; set; } = 0;


    }
    public class CHN_CargoAcceptingDetailsExcel
    {
        public string RegisterNo { get; set; } = string.Empty;

        public string ShippingBillNo { get; set; } = string.Empty;
        public string ShippingBillDate { get; set; } = string.Empty;

        public string ExporterName { get; set; } = string.Empty;
        public string CargoDescription { get; set; } = string.Empty;
        public decimal ActualQty { get; set; } = 0;
        public decimal ActualWeight { get; set; } = 0;
        public decimal FobValue { get; set; } = 0;
        public string LocationName { get; set; } = string.Empty;
        public string GR { get; set; } = string.Empty;
        public decimal SQM { get; set; } = 0;
        public string FromGodown { get; set; } = string.Empty;
        public string ToGodown { get; set; } = string.Empty;
        public string FromShippingLine { get; set; } = string.Empty;
        public string ToShippingLine { get; set; } = string.Empty;


    }
    public class CHN_DRTSBDetailsExcel
    {

        public string StuffingReqNo { get; set; } = string.Empty;
        public string PartyName { get; set; } = string.Empty;
        public string ExporterName { get; set; } = string.Empty;
        public string SBNoDate { get; set; } = string.Empty;
   
      
      

        public string CartingNoDate { get; set; } = string.Empty;
        public string CargoDescription { get; set; } = string.Empty;

        public decimal StuffQuantity { get; set; } = 0;
        public decimal StuffWeight { get; set; } = 0;
        public decimal FobValue { get; set; } = 0;
        
        public string ContainerNo { get; set; } = string.Empty;
        public string CustomSealNo { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
      
        public string Remarks { get; set; } = string.Empty;

    }
    public class CHN_CartingDetailsExcel
    {
        public string RegisterNo { get; set; } = string.Empty;
        public string ShippingBillNo { get; set; } = string.Empty;
        public string ShippingBillDate { get; set; } = string.Empty;
        public string ExporterName { get; set; } = string.Empty;
        public string ShippingLineName { get; set; } = string.Empty;
        public string CargoDescription { get; set; } = string.Empty;
        //public int ShippingLineId { get; set; } = 0;
        public decimal ActualQty { get; set; } = 0;
        public decimal ActualWeight { get; set; } = 0;
        public decimal FobValue { get; set; } = 0;
        public string Slot { get; set; } = string.Empty;
        public string GR { get; set; } = string.Empty;
        public decimal Area { get; set; } = 0;
        public string Remarks { get; set; } = "";
    }

}

