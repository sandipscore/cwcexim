using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class HDB_EntryThroughGate:EntryThroughGate 
    {
        public string FclorLcl { get; set; }
        public string SystemDateTime { get; set; }
        public bool CBT { get; set; } = false;
        public bool IsODC { get; set; } = false;
        public string LCLFCL { get; set; } = string.Empty;
        public string PrintSealCut { get; set; } = string.Empty;
        public string CBTContainer { get; set; } = string.Empty;
        public string SubCFSCode { get; set; }

       public string ExporterName { get; set; }

        [Display(Name = "Purpose ")]
        public string Purpose { get; set; }

        public int ForwarderId { get; set; }
        public string ForwarderName { get; set; }
        public string StuffType { get; set; }= string.Empty;
        public string OldCfsCode { get; set; }= string.Empty;

        public string CBTFrom { get; set; }= string.Empty;
        public string TransPortMode { get; set; } = string.Empty;

    }
    public class HDB_EntryThroughGateBond//: HDB_EntryThroughGate
    {
        public int EntryId { get; set; }
        public string OperationType { get; set; }
        public string ContainerType { get; set; }
        public string EntryDateTime { get; set; }
        public string SystemDateTime { get; set; }
        public string GateInNo { get; set; }
        public string CFSCode { get; set; }
        public int IsCBT { get; set; }
        public int ReferenceNoId { get; set; }
        public string ReferenceNo { get; set; }
        public string ReferenceDate { get; set; }
        public int IsVehicle { get; set; }
        public string GodownName { get; set; }
        public int GodownId { get; set; }
        public string TypeofPackage { get; set; }
        public string Others { get; set; }
        public string CBTNo { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string CustomSealNo { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string VehicleNo { get; set; }
        [Required(ErrorMessage ="Fill Out This Field")]
        public int CargoType { get; set; }
        public int NoOfPackages { get; set; }
        public decimal GrossWeight { get; set; }
        public string DepositorName { get; set; }
        public string Time { get; set; }
        public string CHAName { get; set; }
        public string CargoDescription { get; set; }
        public string Remarks { get; set; }
        public int Uid { get; set; }
        public int BranchId { get; set; }
        
    }
    public class HDB_EntryExport : GateEntryExport
    {
        public string DepositorName { get; set; }
        public string ForwarderName { get; set; }
    }

    public class HDB_LoadedContainerListWithData : LoadedContainerListWithData
    {
        public string DepositorName { get; set; }
        public string OldCfsCode { get; set; }

        public string CustomSealNo { get; set; }
        public string IsODC { get; set; }


    }
   
      public class HDB_StuffContainerListWithData
    {

        public string ContainerNo { get; set; } //ReferenceNo
        public string ExporterId { get; set; }
        public string ShippingLineId { get; set; }

        public string ShippingLineName { get; set; }
        public string Size { get; set; }
       // public string Reefer { get; set; }
        public string CargoType { get; set; }
        public string CargoDescription { get; set; }
        public string NoOfUnits { get; set; }
        public string GrossWt { get; set; }
        public string CHAName { get; set; }
        public string ReferenceDate { get; set; }
        public string CustomSealNo { get; set; }
        public string DepositorName { get; set; }
        public string OldCfsCode { get; set; }
    }



public class HDB_ReferenceNumber: ReferenceNumber
    {

        public IList<LoadContainerReferenceNumberList> listLoadContainer = new List<LoadContainerReferenceNumberList>();
        public IList<ChaNameList> listChaName = new List<ChaNameList>();
        public IList<HDB_EntryExport> listExports = new List<HDB_EntryExport>();
        public IList<ForwarderList> listForwarder { get; set; } = new List<ForwarderList>();


    }

    public class ForwarderList
    {
        public string ForwarderName { get; set; }

        public int ForwarderId { get; set; }
    }
    public class ChaNameList
    {
        public string CHAName { get; set; }
        public int CHAId { get; set; }


    }
  
    public class ReferenceNumList
    {
        public int Id { get; set; }
        public string CartingNo { get; set; }

    }

    public class GateEntrySealCutting
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string SealNo { get; set; }
        public string BLNo { get; set; }
        public string Importer { get; set; }
        public string LCLFCL { get; set; }
        public string ArrivalDate { get; set; }
        public string NoOfUnits { get; set; }
        public string GodownNo { get; set; }
        public string CargoDescription { get; set; }
        public string ReferenceNo { get; set; }
        public string CHA { get; set; }
        public string Exporter { get; set; }
        public string VehicleNo { get; set; }

        public string SACNo { get; set; } = string.Empty;

        public string SACDate { get; set; } = "";
        public string AWBNo { get; set; } = string.Empty;
        public string AWBDate { get; set; } = "";
        public string Purpose { get; set; }
        public string EntryDate { get; set; }

        public string CompanyAddress { get; set; } = string.Empty;
        public string CompanyEmail { get; set; } = string.Empty;
        public string CBTFrom { get; set; } = string.Empty;

    }
    public class StuffingReqList
    {
        public int StuffingReqId { get; set; }
        public string StuffingReqNo { get; set; }
    }

    //public class container
    //{
    //    public string ContainerName { get; set; }


    //}
    //public class containerAgainstGp
    //{
    //    public string ContainerName { get; set; }

    //    public int IsReefer { get; set; }


    //    public int size { get; set; }

    //    public string CargoDescription { get; set; }

    //    public string CargoType { get; set; }
    //    public string NoOfUnits { get; set; }

    //    public string VehicleNo { get; set; }

    //    public string Weight { get; set; }



    //    public string CFSCode { get; set; }
    //    public string shippingLine { get; set; }
    //    public string ShippingLineId { get; set; }

    //    public string OperationType { get; set; }

    //}


    //public class ReferenceNumberList
    //{
    //    public int CartingAppId { get; set; }
    //    public string CartingNo { get; set; }

    //}


    //public class BondReferenceNumberList
    //{
    //    public int SpaceappId { get; set; }
    //    public string ApplicationNo { get; set; }
    //    public string ApplicationDate { get; set; } 

    //}


    //public class LoadContainerReferenceNumberList
    //{
    //    public int LoadContReqId { get; set; }
    //    public string LoadContReqNo { get; set; }
    //    public string LoadContReqDate { get; set; }

    //}
    //public class ShippingLineList
    //{
    //    public  string ShippingLine { get; set; }
    //    public int ShippingLineId { get; set; }


    //}

    //public class LoadedContainer
    //{
    //    List<LoadContainerReferenceNumberList> listLoadContainer = new List<LoadContainerReferenceNumberList>();
    //    List<ShippingLineList> listShippingLine = new List<ShippingLineList>();

    //}
    //public class ReferenceNumber
    //{

    //    public IList<ReferenceNumberList> ReferenceList = new List<ReferenceNumberList>();

    //    public IList<GateEntryExport> listExport = new List<GateEntryExport>();

    //    public IList<ShippingLineList> listShippingLine = new List<ShippingLineList>();
    //}


    //public class GateEntryExport
    //{
    //    public string ContainerName { get; set; } //ReferenceNo
    //    public string ReferenceNo { get; set; }
    //    public string ReferenceDate { get; set; }
    //    public string ShippingLineId { get; set; }
    //    public string ShippingLineName { get; set; }
    //    public string ContainerNo { get; set; }
    //    public string ContainerSize { get; set; }
    //    public int CargoType { get; set; }
    //    public string CHAName { get; set; }

    //    public string CargoDescription { get; set; }
    //    //public string ContainerName { get; set; }
    //    //public string ContainerName { get; set; }
    //    //public string ContainerName { get; set; }
    //    //public string ContainerName { get; set; }
    //    //public string ContainerName { get; set; }

    //}


    //public class LoadedContainerListWithData
    //{

    //    public string ContainerNo { get; set; } //ReferenceNo
    //    public string ExporterId { get; set; }
    //    public string ShippingLineId { get; set; }

    //    public string ShippingLineName { get; set; }
    //    public string Size { get; set; }
    //    public string Reefer { get; set; }
    //    public string CargoType { get; set; }
    //    public string CargoDescription { get; set; }
    //    public string NoOfUnits { get; set; }

    //    public string GrossWt { get; set; }

    //}

    //public class ReferenceNumberCCIN
    //{

    //    public IList<CCINList> ReferenceList = new List<CCINList>();

    //   // public IList<GateEntryExport> listExport = new List<GateEntryExport>();

    //    public IList<ShippingLineList> listShippingLine = new List<ShippingLineList>();
    //}

    //public class CCINList
    //{
    //    public int CCINId { get; set; }
    //    public string CCINNo { get; set; }
    //    public string CCINDate { get; set; }
    //    public int ShippingLineId { get; set; }      
    //    public string Weight { get; set; }
    //    public string NoOfUnits { get; set; }

    //}


    //public class containerAgainstGp_PPG
    //{
    //    public string ContainerName { get; set; }

    //    public int IsReefer { get; set; }


    //    public int size { get; set; }

    //    public string CargoDescription { get; set; }

    //    public string CargoType { get; set; }
    //    public string NoOfUnits { get; set; }

    //    public string VehicleNo { get; set; }

    //    public string Weight { get; set; }



    //    public string CFSCode { get; set; }
    //    public string shippingLine { get; set; }
    //    public string ShippingLineId { get; set; }
    //    public string OperationType { get; set; }

    //    public string GPDate { get; set; }
    //    public string CHAName { get; set; }

    //}
}
