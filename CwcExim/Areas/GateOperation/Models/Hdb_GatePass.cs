using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class HDB_GatePass: GatePass
    {
        public string Module { get; set; }
        public int PortId { get; set; }
        public string PortName { get; set; }
    }

    public class HDB_GPHdr : GPHdr
    {
        public string ValidTill { get; set; }
        public string PortName { get; set; }
        public string Module { get; set; }
        public string BondNo { get; set; }

        public string GodownName { get; set; }

        public string BondDate { get; set; }

        public string VehicleNo { get; set; }

        public string CargoDescription { get; set; }

        public int NoOfUnits { get; set; }

        public decimal Weight { get; set; }
        public string CompanyAdrress { get; set; } = string.Empty;
        public string CompanyMail { get; set; } = string.Empty;
        public string CompanyShortName { get; set; } = string.Empty;
        public string CompanyLocation { get; set; } = string.Empty;
        public IList<HdbGPDet> lstDet { get; set; } = new List<HdbGPDet>();

    }
    public class HdbGPDet
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
        public string ShippingSealNo { get; set; }
        public string CustomSealNo { get; set; }
    }

    public class ListOfHdbGP
    {
        public int GatePassId { get; set; }
        public string GatePassNo { get; set; }
        public string GatePassDate { get; set; }
        public string InvoiceNo { get; set; }
        public string ContainerNo { get; set; } = string.Empty;
        public string VehicleNo { get; set; } = string.Empty;
        public string IsCancelled { get; set; }
    }
}