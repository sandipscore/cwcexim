using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{
    public class VRN_ExpPvReport
    {

        public string CompanyLocation { get; set; }
        public string CompanyBranch { get; set; }
        public string ContainerNo { get; set; }
        public string EntryDateTime { get; set; }
        public string CFSCode { get; set; }
        public string Size { get; set; }
        public int Days { get; set; }
        public int NoOfUnitsRec { get; set; }
        public string EximTraderAlias { get; set; }
        public string TransportFrom { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
    }

    public class VRN_PVImpLoaded
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string AsOnDate { get; set; }
        public int GodownId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GodownName { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Module { get; set; }
    }

    public class VRN_PVImpLoadedModel
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string AsOnDate { get; set; }
    }


    public class VRN_PVReportImpEmptyCont
    {
        public string CompanyLocation { get; set; }
        public string CompanyBranch { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string EntryDateTime { get; set; }
        public string Size { get; set; }
        public string EximTraderAlias { get; set; }
        public string InDateEcy { get; set; }
        public string OutDateEcy { get; set; }
        public int Days { get; set; }
        public decimal Amount { get; set; }

    }
}