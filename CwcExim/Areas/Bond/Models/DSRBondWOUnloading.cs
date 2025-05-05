using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Bond.Models
{
    public class DSRBondWOUnloading : BondWOUnloading
    {
        public string BondNo { get; set; }
        public string BondDate { get; set; }
        [Display(Name = "Nature of Material")]
        public int NatureOfMaterial { get; set; }
    }
    public class ListOfDSR_BOENo
    {
        public string BondNo { get; set; }
        public int DepositAppId { get; set; }
    }


    public class DSRBondWOUnloadingPDF
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