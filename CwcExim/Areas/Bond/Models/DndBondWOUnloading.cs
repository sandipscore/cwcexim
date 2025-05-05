using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Bond.Models
{
    public class DndBondWOUnloading: BondWOUnloading
    {
        public string BondNo { get; set; }

        public string BondDate { get; set; }
    }
    public class ListOfDnd_BOENo
    {
        //public string WorkOrderNo { get; set; }
        //public int BondWOId { get; set; }
        public string BondNo { get; set; }


        public int DepositAppId { get; set; }
    }


    public class DndBondWOUnloadingPDF
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