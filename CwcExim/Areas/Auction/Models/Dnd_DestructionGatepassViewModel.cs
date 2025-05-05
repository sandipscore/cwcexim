using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Auction.Models
{
    public class Dnd_DestructionGatepassViewModel
    {
        public string GatePassNo { get; set; }
        public string GatePassDate { get; set; }
        public string VehicleNo { get; set; }
        public string ContainerNoAndSize { get; set; }
        public string ImporterExporter { get; set; }
        public string DestructionAgencyName { get; set; }
        public string ShippingLine { get; set; }
        public string OBLNoShippbillNo { get; set; }
        public string DestructionDate { get; set; }
        public string NoOfPkg { get; set; }
        public string Weight { get; set; }
        public string LocationName { get; set; }
        public string ICDCode { get; set; }
        public string CargoDesc { get; set; }
        public string GatepassTime { get; set; }
        public string GatepassExpiryDateAndTime { get; set; }

        public string Remarks { get; set; }
    }
}