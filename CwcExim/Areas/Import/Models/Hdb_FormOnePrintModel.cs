using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Hdb_FormOnePrintModel
    {
        public string ShippingLineNo { get; set; }
        public string FormOneNo { get; set; }
        public string CHAName { get; set; }
        public string CHAAddress { get; set; }
        public string CHAPhoneNo { get; set; }
        public string FormOneDate { get; set; }
        public IList<Hdb_FormOnePrintDetailModel> lstFormOnePrintDetail { get; set; } = new List<Hdb_FormOnePrintDetailModel>();
    }

    public class Hdb_FormOnePrintDetailModel
    {
        public string VesselName { get; set; }
        public string VoyageNo { get; set; }
        public string RotationNo { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public string SealNo { get; set; }
        public string ImpName { get; set; }
        public string ImpAddress { get; set; }
        public string ImpName2 { get; set; }
        public string ImpAddress2 { get; set; }
        public string Type { get; set; }
        public string LineNo { get; set; }
        public string CargoDesc { get; set; }
        public string DateOfLanding { get; set; }
        public string HazType { get; set; }
        public string ReferType { get; set; }
        public string FormOneDate { get; set; }
        public string ShippingLineName { get; set; }
        public string ForwarderName { get; set; }



    }
}