using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace CwcExim.Areas.Report.Models
{
    public class Chn_ImpPVReport
    {
        public string CompanyLocation { get; set; }
        public string CompanyBranch { get; set; }
        public string BOLNo { get; set; }
        public string DestuffingEntryDate { get; set; }
        public string CFSCode { get; set; }
        public string CommodityAlias { get; set; }
        public int NoOfUnits { get; set; }
        public int NoOfUnitsRec { get; set; }
        public decimal Weight { get; set; }
        public decimal Area { get; set; }
        public string LocationName { get; set; }
        public string Remarks { get; set; }
        public decimal Amount { get; set; }
        public int TotWk { get; set; }
        
    }
    public class Chn_ImpPVReportExcel
    {
       
        public int SlNo { get; set; }
        public string BOLNo { get; set; }
        public string DestuffingEntryDate { get; set; }
        public string CFSCode { get; set; }
        public string CommodityAlias { get; set; }
        public int NoOfUnits { get; set; }
        public int NoOfUnitsRec { get; set; }
        public decimal Weight { get; set; }
        public decimal Area { get; set; }
        public string LocationName { get; set; }
        public string Remarks { get; set; }
        

    }

    public class Chn_PVReportImpLoadedCont
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


    public class Chn_PVReportImpEmptyCont
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