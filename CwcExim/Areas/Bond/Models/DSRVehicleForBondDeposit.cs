using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Bond.Models
{
    public class DSRVehicleForBondDeposit
    {
        public string VehicleNo { get; set; }
        public int EntryId { get; set; }
        public int SpaceappId { get; set; }
        public decimal NoOfPackages { get; set; }
        public decimal GrossWeight { get; set; }
    }
}