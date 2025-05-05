using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace CwcExim.Areas.Report.Models
{
    public class Chn_ExpPvReport
    {
        public string CompanyLocation { get; set; }
        public string CompanyBranch { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string EntryNo { get; set; }
        public string RegisterDate { get; set; }
        public int Units { get; set; }
        public decimal Weight { get; set; }
        public decimal Fob { get; set; }
        public string LocationName { get; set; }
        public decimal Area { get; set; }
        public string EximTraderName { get; set; }
        public string EximTraderAlias { get; set; }
        public int ShippingLineId { get; set; }
    }
    public class Chn_ExpPvReportExcel
    {
        public string SlNo { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string EntryNo { get; set; }
        public string RegisterDate { get; set; }
        public int Units { get; set; }
        public decimal Weight { get; set; }
        public decimal Fob { get; set; }
        public string LocationName { get; set; }
        public decimal Area { get; set; }
        public string EximTraderName { get; set; }
        public string EximTraderAlias { get; set; }
        public int ShippingLineId { get; set; }
        public IList<Chn_ExpPvReportExcelList> lstPVExcel { get; set; } = new List<Chn_ExpPvReportExcelList>();
    }
    public class Chn_ExpPvReportExcelList
    {
        public string SlNo { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string EntryNo { get; set; }
        public string RegisterDate { get; set; }
        public int Units { get; set; }
        public decimal Weight { get; set; }
        public decimal Fob { get; set; }
        public string LocationName { get; set; }
        public decimal Area { get; set; }
        public string EximTraderName { get; set; }
        public string EximTraderAlias { get; set; }
        public int ShippingLineId { get; set; }

    }

    public class Chn_PV
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string AsOnDate { get; set; }
        public int GodownId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GodownName { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Module { get; set; }
    }

    public class Chn_PVReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string AsOnDate { get; set; }
    }
}