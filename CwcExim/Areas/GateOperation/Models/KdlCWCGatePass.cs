using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class kdlCWCGatePass:GatePass
    {
        public string Module { get; set; }
        //public List<PpgGatepassVehicle> lstVehicles { get; set; } = new List<PpgGatepassVehicle>();
        public string VehicleXml { get; set; }

        public string PortOfDispatch { get; set; }
    }

    public class KdlCWCInvoiceNoList : InvoiceNoList
    {
        public string Module { get; set; }

    }
    public class KdlCWCGatepassVehicle
    {
        public int Id { get; set; } = 0;
        public int GatePassId { get; set; } = 0;

        public string ContainerNo { get; set; }
        public string VehicleNo { get; set; }
        public decimal Package { get; set; } = 0;
        public decimal Weight { get; set; } = 0;

    }

    public class Kdl_CWCContainerDet
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
    }

    public class Kdl_CWCGPHdr
    {
        public string GatePassNo { get; set; }
        public string GatePassDate { get; set; }
        public string Remarks { get; set; }
        public string CHAName { get; set; }
        public string ImpExpName { get; set; }
        public string ShippingLineName { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string EntryDate { get; set; }
        public string InvoiceDate { get; set; }
        public IList<Kdl_CWCGPDet> lstDet { get; set; } = new List<Kdl_CWCGPDet>();

    }
    public class Kdl_CWCGPDet
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



    }

}
