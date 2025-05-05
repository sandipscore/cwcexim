using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class Chn_ContainerStuffing : PPG_ContainerStuffing
    {
        public string POLName { get; set; }
        public string CompanyAddress { get; set; } = "";
        public string Vessel { get; set; } = string.Empty;
        public string Voyage { get; set; } = string.Empty;
        public string Via { get; set; } = string.Empty;
        public string ShipBillNo { get; set; } = string.Empty;
        public string ContainerNo { get; set; } = string.Empty;
        public string SCMTRXML { get; set; }


        //public List<CHN_ContainerStuffingDtl> LstStuffingDtl = new List<CHN_ContainerStuffingDtl>();
        public List<CHN_ContainerStuffingSCMTR> LstSCMTRDtl = new List<CHN_ContainerStuffingSCMTR>();

        //public List<CHN_ContainerStuffingDtl> LstStuffing { get; set; } = new List<CHN_ContainerStuffingDtl>();
        //public List<CHN_ContainerStuffingSCMTR> LstSCMTR { get; set; } = new List<CHN_ContainerStuffingSCMTR>();
        public int FinalDestinationLocationId { get; set; }

        public string FinalDestinationLocation { get; set; }
        public string AmendmentDate { get; set; }
    }
}

