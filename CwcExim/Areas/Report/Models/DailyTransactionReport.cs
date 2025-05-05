using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DailyTransactionReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string DTRDate { get; set; }
        public List<AppeasementSummary> AppeasementSummaryList { get; set; }
        public List<DeStuffingSummary> DeStuffingSummaryList { get; set; }
        public List<CartingSummary> CartingSummaryList { get; set; }
        public List<StuffingInportExportBONDSummary> StuffingSummaryList { get; set; }
        public List<StuffingInportExportBONDSummary> InportInSummaryList { get; set; }
        public List<StuffingInportExportBONDSummary> InportOutSummaryList { get; set; }
        public List<StuffingInportExportBONDSummary> ExportInSummaryList { get; set; } 
        public List<StuffingInportExportBONDSummary> ExportOutSummaryList { get; set; }
        public List<StuffingInportExportBONDSummary> BONDUnloadingSummaryList { get; set; }
        public List<StuffingInportExportBONDSummary> BONDDeliverySummaryList { get; set; }
        public List<EmptyTransporterSummary> EmptyInTransporterSummaryList { get; set; }
        public List<EmptyTransporterSummary> EmptyOutTransporterSummaryList { get; set; } 
    }

    public class ContainerInfo
    {
        public string CFSCode { get; set; } 
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string MovementType { get; set; }
    }

    public class AppeasementSummary
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
    public class DeStuffingSummary
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
    public class CartingSummary
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
    public class StuffingInportExportBONDSummary
    {
        public StuffingInportExportBONDSummary()
        {
            ContainerInfoList = new List<ContainerInfo>();
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
        public List<ContainerInfo> ContainerInfoList { get; set; }
    }

    public class EmptyTransporterSummary
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