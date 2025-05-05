using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{

    public class CHN_DailyTransactionReport
    {
        public string DTRDate { get; set; }
        public List<DTRAppeasementSummary> AppeasementSummaryList { get; set; }
        public List<DTRDeStuffingSummary> DeStuffingSummaryList { get; set; }
        public List<DTRCartingSummary> CartingSummaryList { get; set; }
        public List<DTRStuffingInportExportBONDSummary> StuffingSummaryList { get; set; }
        public List<DTRStuffingInportExportBONDSummary> InportInSummaryList { get; set; }
        public List<DTRStuffingInportExportBONDSummary> InportOutSummaryList { get; set; }
        public List<DTRStuffingInportExportBONDSummary> ExportInSummaryList { get; set; }
        public List<DTRStuffingInportExportBONDSummary> ExportOutSummaryList { get; set; }
        public List<DTRStuffingInportExportBONDSummary> BONDUnloadingSummaryList { get; set; }
        public List<DTRStuffingInportExportBONDSummary> BONDDeliverySummaryList { get; set; }
        public List<DTREmptyTransporterSummary> EmptyInTransporterSummaryList { get; set; }
        public List<DTREmptyTransporterSummary> EmptyOutTransporterSummaryList { get; set; }
    }

    public class DTRContainerInfo
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string MovementType { get; set; }
    }
    public class DTRStuffingInportExportBONDSummary
    {
        public DTRStuffingInportExportBONDSummary()
        {
            ContainerInfoList = new List<DTRContainerInfo>();
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
        public List<DTRContainerInfo> ContainerInfoList { get; set; }
    }


    public class DTRAppeasementSummary
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
    public class DTRDeStuffingSummary
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
    public class DTRCartingSummary
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


    public class DTREmptyTransporterSummary
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