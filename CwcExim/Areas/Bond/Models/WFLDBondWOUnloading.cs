using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Bond.Models
{
    public class WFLDBondWOUnloading: BondWOUnloading
    {
        public string BondNo { get; set; }

        public string BondDate { get; set; }
        public decimal AppliedArea { get; set; }
        public decimal? AppliedAreaReserved { get; set; }
        public decimal RemArea { get; set; }
        public decimal RemAreaReserved { get; set; }
        public decimal? AreaOccupiedReserved { get; set; }
        public decimal? AreaOccupiedReservedOrg { get; set; }
        public decimal NetSpaceReq { get; set; }        
        public decimal NetSpaceRemaining { get; set; }
        public decimal NetSpaceRemainingCalc { get; set; }
        public decimal SpaceReservedCHA { get; set; }
        public decimal SpaceReservedIMP { get; set; }
        public decimal NetSpaceReservedCHA { get; set; }
        public decimal NetSpaceReservedIMP { get; set; }

    }
    public class ListOfWFLD_BOENo
    {
        //public string WorkOrderNo { get; set; }
        //public int BondWOId { get; set; }
        public string BondNo { get; set; }


        public int DepositAppId { get; set; }
    }


    public class WFLDBondWOUnloadingPDF
    {
        public string DepositNo { get; set; }
        public string UnloadingDate { get; set; }
        public int UnloadingId { get; set; }
        public string GodownNo { get; set; }
        public string CargoDescription { get; set; }
        public int NoOfUnits { get; set; }
        public decimal Weight { get; set; }
        public decimal Area { get; set; }
        public string PkgCondition { get; set; }
        public string Remarks { get; set; }
        public string CompanyAddress { get; set; }
        public string EmailAddress { get; set; }
        public string Location { get; set; }

    }
}