using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class PpgGatePass:GatePass
    {
        public string Module { get; set; }
        //public List<PpgGatepassVehicle> lstVehicles { get; set; } = new List<PpgGatepassVehicle>();
        public string VehicleXml { get; set; } 

        public string PortOfDispatch { get; set; }
    }
    public class PpgInvoiceNoList: InvoiceNoList
    {
        public string Module { get; set; }
    }
   public class PpgGatepassVehicle
    {
        public int Id { get; set; } = 0;
        public int GatePassId { get; set; } = 0;

        public string ContainerNo { get; set; }
        public string VehicleNo { get; set; }
        public decimal Package { get; set; } = 0;
    }

    public class PPG_ContainerDet
    {
        public int GatePassDtlId { get; set; }
        public string ContainerNo { get; set; }
        public bool IsReefer { get; set; }
        public string Size { get; set; }
        /*public int CHAId { get; set; }
        public string CHAName { get; set; }
        public int ImpExpId { get; set; }
        public string ImpExpName { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }*/
        public string CargoDescription { get; set; }
        public int CargoType { get; set; }
        public int NoOfUnits { get; set; }
        public string VehicleNo { get; set; }
        public decimal Weight { get; set; }
        public string Location { get; set; }

        public string PortOfDispatch { get; set; }
        public decimal ElwbTareWeight { get; set; }
        public decimal ElwbCargoWeight { get; set; }
    }

    public class PPG_GPHdr
    {
        public string GatePassNo { get; set; }
        public string GatePassDate { get; set; }
        public string Remarks { get; set; }
        public string ExpiryDate { get; set; }
        public string CHAName { get; set; }
        public string ImpExpName { get; set; }
        public string ShippingLineName { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string EntryDate { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceType { get; set; } 
        public IList<PPG_GPDet> lstDet { get; set; } = new List<PPG_GPDet>();
        public IList<PPG_Header> lstHed { get; set; } = new List<PPG_Header>();
    }
    public class PPG_GPDet
    {
        public string ContainerNo { get; set; }
        public decimal NoOfUnits { get; set; }
        public string VehicleNo { get; set; }
        public decimal Weight { get; set; }
        public int CargoType { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string LineNo { get; set; }
        public string Rotation { get; set; }
        public string Location { get; set; }
        public string PortOfDispatch { get; set; }
        public string ICDCode { get; set; }
        public string ShippingLineNo { get; set; }
        public string CargoDescription { get; set; }

        public string OBLNO { get; set; }
        public string EntryDate { get; set; }
        public string IGMNo { get; set; }
        public string InDate { get; set; }
        public string DestuffingDate { get; set; }
        public string CustomSealNo { get; set; }

        public string InvoiceNo { get; set; }

        public string IssueSlipNo { get; set; }
        public string IssueSlipDate { get; set; }
        public string CreatedTime { get; set; }

        public decimal ElwbTareWeight { get; set; }
        public decimal ElwbCargoWeight { get; set; }


    }
    public class PPG_Header
    {
    
        public string InvoiceType { get; set; }
        public string CFSCode { get; set; }



    }


}