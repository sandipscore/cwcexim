using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
   
    public class DSR_ImpPVReport
    {
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
        public string PackageType { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string CHAName { get; set; }
        public string ImporterName { get; set; }
        public decimal CIFValue { get; set; }
        public decimal GrossDuty { get; set; }
        public int BalancePackages { get; set; }
        public string DestuffingEntryNo { get; set; }
    }
   

    public class DSR_ExpPVReport
    {
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
        public string CHAName { get; set; }
        public string ExporterName { get; set; }
       public string CargoDescription { get; set; }
    }

    public class DSR_BondPVReport
    {
        public string CompAddress { get; set; } = string.Empty;
        public string CompEmail { get; set; } = string.Empty;
        public string BondNo { get; set; }
        public string BondDate { get; set; }
        public string Importer { get; set; }
        public string CHA { get; set; }
        public int Units { get; set; }
        public decimal Weight { get; set; }
        public decimal Value { get; set; }
        public decimal Duty { get; set; }
        public decimal Area { get; set; }
        public string ItemDesc { get; set; }
    }
    public class DSR_PV 
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string AsOnDate { get; set; }
        public int GodownId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GodownName { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Module { get; set; }
    }

    public class DSR_PVReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string AsOnDate { get; set; }
    }
    public class DSR_PVReportImpLoadedCont
    {
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
        public string CHAName { get; set; }
        public string ImporterName { get; set; }

    }
    public class DSR_PVReportImpEmptyCont
    {
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

    public class DSR_LongStandingEmptyCont
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string EntryDateTime { get; set; }
        public string Size { get; set; }
        public string EximTraderAlias { get; set; }
        public string InDateEcy { get; set; }
        public string OutDateEcy { get; set; }
        public int Days { get; set; }
        public decimal Amount { get; set; }
        public  string ShippingLine { get; set; }
        public string Address { get; set; }

        public string Notices1 { get; set; }
        public string Notices2 { get; set; }
        public string AuctionNo { get; set; }
        public string NocNO { get; set; }
        public string NocDate { get; set; }
        public string Remarks { get; set; }
    }

    public class DSR_LongStandingImpCon
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public int days { get; set; }
    }
    public class DSR_LongStandingImpConDtl
    {
        public string BOLNo { get; set; }
        public string BOLDate { get; set; }
        public string IGMNo { get; set; }
        public string IGMDate { get; set; }
        public string ImporterName { get; set; }
        public string ImporterAddress { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string EntryDateTime { get; set; }
        public string Size { get; set; }
        public string EximTraderAlias { get; set; }
        public string InDate { get; set; }
        public string SlaCd { get; set; }
        public int Days { get; set; }
        public int NoOfPkg { get; set; }
        public decimal GrWt { get; set; }
        public string Commodity { get; set; }
        public string CargoType { get; set; }
        public string Notice1 { get; set; }
        public string Date1 { get; set; }
        public string Notice2 { get; set; }
        public string Date2 { get; set; }
        public string Nocr { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
    }
}