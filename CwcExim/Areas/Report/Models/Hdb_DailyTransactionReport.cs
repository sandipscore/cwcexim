using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_DailyTransactionReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string DTRDate { get; set; }
        public List<Hdb_AppeasementSummary> AppeasementSummaryList { get; set; }
        public List<Hdb_DeStuffingSummary> DeStuffingSummaryList { get; set; }
        public List<Hdb_CartingSummary> CartingSummaryList { get; set; }
        public List<Hdb_StuffingImportExportBONDSummary> StuffingSummaryList { get; set; }
        public List<Hdb_StuffingImportExportBONDSummary> InportInSummaryList { get; set; }
        public List<Hdb_StuffingImportExportBONDSummary> InportOutSummaryList { get; set; }
        public List<Hdb_StuffingImportExportBONDSummary> ExportInSummaryList { get; set; }
        public List<Hdb_StuffingImportExportBONDSummary> ExportOutSummaryList { get; set; }
        public List<Hdb_StuffingImportExportBONDSummary> BONDUnloadingSummaryList { get; set; }
        public List<Hdb_StuffingImportExportBONDSummary> BONDDeliverySummaryList { get; set; }
        public List<Hdb_EmptyTransporterSummary> EmptyInTransporterSummaryList { get; set; }
        public List<Hdb_EmptyTransporterSummary> EmptyOutTransporterSummaryList { get; set; }
    }

    public class Hdb_ContainerInfo
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string MovementType { get; set; }
    }

    public class Hdb_AppeasementSummary
    {
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string Importer { get; set; }
        public string CHA { get; set; }
        public string Cargo { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string NoOfPackages { get; set; }
        public string GrossWeight { get; set; }
        public string Remarks { get; set; }
    }
    public class Hdb_DeStuffingSummary
    {
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string Importer { get; set; }
        public string CHA { get; set; }
        public string Cargo { get; set; }
        public string GodownId { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string NoOfPackages { get; set; }
        public string DestuffingWeight { get; set; }
        public string Remarks { get; set; }
    }
    public class Hdb_CartingSummary
    {
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string ExporterName { get; set; }
        public string CHA { get; set; }
        public string Cargo { get; set; }
        public string Units { get; set; }
        public string Weight { get; set; }
        public string CartingType { get; set; }
        public string Remarks { get; set; }
    }
    public class Hdb_StuffingImportExportBONDSummary
    {
        public Hdb_StuffingImportExportBONDSummary()
        {
            ContainerInfoList = new List<Hdb_ContainerInfo>();
        }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string ExporterName { get; set; }
        public string CHA { get; set; }
        public string ShippingLine { get; set; }
        public string CartingType { get; set; }
        public int SizeTwenty { get; set; }
        public int SizeForty { get; set; }
        public string Remarks { get; set; }
        public List<Hdb_ContainerInfo> ContainerInfoList { get; set; }
    }

    public class Hdb_EmptyTransporterSummary
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string ShippingLine { get; set; }
        public string TransportedBy { get; set; }
        public string Remarks { get; set; }
        public int SizeTwenty { get; set; }
        public int SizeForty { get; set; }

    }
}
