using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.GateOperation.Models
{
    public class Dnd_GatePass:GatePass
    {
        public int PortId { get; set; }
        public string PortName { get; set; }
        public string DriverName { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Contact No Should Be Of Numeric Digits ")]
        [StringLength(10, MinimumLength = 8, ErrorMessage = "Contact No Should Be Within 8-10 Characters")]
        public string ContactNo { get; set; }
        public bool Risk { get; set; }
    }

    public class Dnd_GPHdr : GPHdr
    {
        public string ValidTill { get; set; }
        public string PortName { get; set; }
        public string Module { get; set; }
        public string BondNo { get; set; }

        public int GodownId { get; set; }

        public string BondDate { get; set; }

        public string VehicleNo { get; set; }

        public string CargoDescription { get; set; }

        public int NoOfUnits { get; set; }

        public decimal Weight { get; set; }
        public string CompanyAdrress { get; set; } = string.Empty;
        public string CompanyMail { get; set; } = string.Empty;
        public string CompanyShortName { get; set; } = string.Empty;
        public string CompanyLocation { get; set; } = string.Empty;
        public string DriverName { get; set; }
        public string ContactNo { get; set; }
      
        public IList<DndGPDet> lstDet { get; set; } = new List<DndGPDet>();
          public IList<ListOfContainer> lstCont { get; set; } = new List<ListOfContainer>();
    }
    public class DndGPDet
    {
        public string ContainerNo { get; set; }
        public decimal NoOfUnits { get; set; }
        public string VehicleNo { get; set; }
        public decimal Weight { get; set; }
        public decimal FOB { get; set; } = 0;
        public int CargoType { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string LineNo { get; set; }
        public string Rotation { get; set; }
        public string Location { get; set; }
        public string ShippingSealNo { get; set; }
        public string CustomSealNo { get; set; }
        public string Commodity { get; set; }
        public string Via { get; set; }
        public string POD { get; set; }
        public string CFSCode { get; set; }
        public string MoveNo { get; set; }
        public string POL { get; set; }
        public string EntryNo { get; set; }
        public string OBLDate { get; set; }
        public decimal FobValue { get; set; }

    }

    public class ListOfDndGP
    {
        public int GatePassId { get; set; }
        public string GatePassNo { get; set; }
        public string GatePassDate { get; set; }
        public string InvoiceNo { get; set; }
        public string ContainerNo { get; set; } = string.Empty;
        public string VehicleNo { get; set; } = string.Empty;
        public string IsCancelled { get; set; }
    }

    public class ListOfContainer
    {
        public string Container { get; set; }
        public string ShippingBillNo { get; set; }
     
    }
}