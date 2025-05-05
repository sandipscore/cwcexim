using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_DailyTransaction
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }
        public int GodownId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GodownName { get; set; }
        public string Module { get; set; }
    }


    public class Hdb_ExportDailyTransaction
    {
        public List<Hdb_ExportDailyTransactionForCartingDetails> CartingDetails { get; set; } = new List<Hdb_ExportDailyTransactionForCartingDetails>();
        public List<Hdb_ExportDailyTransactionForStuffingDetails> StuffingDetails { get; set; } = new List<Hdb_ExportDailyTransactionForStuffingDetails>();
        public List<Hdb_ExportDailyTransactionFroDeliveryDetails> DeliveryDetails { get; set; } = new List<Hdb_ExportDailyTransactionFroDeliveryDetails>();

        public List<Hdb_exportBTTDailyTransactionDetails> BttContainer { get; set; } = new List<Hdb_exportBTTDailyTransactionDetails>();
        public List<Hdb_ExportDailyTransactionForEmpty> Empty { get; set; } = new List<Hdb_ExportDailyTransactionForEmpty>();

        
    }

    public class Hdb_ExportDailyTransactionForCartingDetails
    {
        public string SlNo { get; set; }
        public string CartingNo { get; set; }
        public string CartingDate { get; set; }
        public string SBNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string ExporterName { get; set; }
        public string CHAName { get; set; }        
        public string CargoDescription { get; set; }
        public string CargoType { get; set; }
        public string PackageType { get; set; }
        public decimal NoOfPkg { get; set; }
        public decimal Weight { get; set; }
        public decimal FOB { get; set; }
        public decimal Area { get; set; }
        public string VehicleNo { get; set; }
        public string Remarks { get; set; }
        public string ShippingLine { get; set; }

    }

    public class Hdb_ExportDailyTransactionForStuffingDetails
    {
        public string SlNo { get; set; }
        public string StuffingNo { get; set; }
        public string StuffingDate { get; set; }
        public string SBNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string ExporterName { get; set; }
        public string CHAName { get; set; }        
        public string CargoDescription { get; set; }
        public string CargoType { get; set; }
        public string PackageType { get; set; }
        public decimal NoOfPkg { get; set; }
        public decimal Weight { get; set; }
        public decimal FOB { get; set; }
        public decimal Area { get; set; }
        public string ContSize { get; set; }
        public string CBT { get; set; }
        public string Remarks { get; set; }
        public string ShippingLine { get; set; }
        public string Charges { get; set; }

    }

    public class Hdb_exportBTTDailyTransactionDetails
    {
        public string ContainerNoCBT { get; set; }

        public decimal ContainerNoOfPkg { get; set; }

        public decimal ContainerWeight { get; set; }

        public decimal ContainerFOB { get; set; }

        public int Size { get; set; }

        public string ContainerRemarks { get; set; }
    }

    public class Hdb_ExportDailyTransactionFroDeliveryDetails
    {
        public string SBNo { get; set; }
        public decimal NoOfPKG { get; set; }
        public decimal Weight { get; set; }

        public decimal FOB { get; set; }

        public string Remarks { get; set; }

       
    }

    public class Hdb_ExportDailyTransactionForEmpty
    {
        public decimal OpeningBalance { get; set; }
        public decimal Receipt { get; set; }

        public decimal Issue { get; set; }
        public decimal CloseBalance { get; set; }

        public string Remarks { get; set; }

        public decimal dry20 { get; set; }
        public decimal dry40 { get; set; }

        public decimal Refer20 { get; set; }
        public decimal Refer40 { get; set; }

        public decimal Rail20 { get; set; }

        public decimal Rail40 { get; set; }

        public decimal FOB { get; set; }

        public decimal Weight { get; set; }

        public decimal Total20 { get; set; }

        public decimal Total40 { get; set; }

        public decimal Teus { get; set; }

        public string Remarks2 { get; set; }
    }



    public class Hdb_BondDailyTransactionReport
    {
        public List<Hdb_BondDepositeDailyTransactionReport> lstBondDepositeTransaction { get; set; } = new List<Hdb_BondDepositeDailyTransactionReport>();
        public List<Hdb_BondDeliverDailyTransactionReport> lstBondDeliveryTransaction { get; set; } = new List<Hdb_BondDeliverDailyTransactionReport>();

    }

    public class Hdb_BondDepositeDailyTransactionReport
    {
        public int SrNo { get; set; }
        public string GodownName { get; set; }
        public string DepositNo { get; set; }
        public string DepositDate { get; set; }
        public string Importer { get; set; }
        public string CHAName { get; set; }       
        public string CargoDescription { get; set; }
        public string BondNoDate { get; set; }
        public string BOENoDate { get; set; }
        public decimal Noofpkg { get; set; }
        public decimal Weight { get; set; }
        public decimal Value { get; set; }
        public decimal Duty { get; set; }
        public decimal AREA { get; set; }
        public string Remarks { get; set; }
    }

    public class Hdb_BondDeliverDailyTransactionReport

    {
        public int SrNo { get; set; }
        public string GodownName { get; set; }
        public string DeliveryOrderNo { get; set; }
        public string DeliveryOrderDate { get; set; }
        public string Importer { get; set; }
        public string CHAName { get; set; }
        public string CargoDescription { get; set; }
        public string BondNoDate { get; set; }
        public string BOENoDate { get; set; }
        public decimal Noofpkg { get; set; }
        public decimal Weight { get; set; }
        public decimal Value { get; set; }
        public decimal Duty { get; set; }
        public decimal AREA { get; set; }
      

        public string InvoiceNoDate { get; set; }
    }



    public class Hdb_ImportDailyTransaction
    {
        public List<Hdb_ImportDustuffDailyTransaction> Dustuff { get; set; } = new List<Hdb_ImportDustuffDailyTransaction>();
        public List<Hdb_ImportHandlingDailyTransaction> Handling { get; set; } = new List<Hdb_ImportHandlingDailyTransaction>();
        public List<Hdb_ImportDeliveryCargoDailyTransaction> Delivery { get; set; } = new List<Hdb_ImportDeliveryCargoDailyTransaction>();
    }

    public class Hdb_ImportDustuffDailyTransaction
    {
        public string SLNO { get; set; }
        public string DestuffingNo { get; set; }
        public string DestuffingDate { get; set; }

        public string CBTContainerNo { get; set; }
        public string Size { get; set; }
        public string FCLLCL { get; set; }
        public string CBTfrom { get; set; }

        public string PortofOrigin { get; set; }
        public string TSANoDate { get; set; }
        public string BLNoDate { get; set; }
        public string ImporterName { get; set; }

        public string ForwarderName { get; set; }
        public string IGMNumber { get; set; }
        public string CargoDescription { get; set; }
        public string CommodityType { get; set; }

        public decimal NoOfPackages { get; set; }
        public decimal Weight { get; set; }
        public decimal Value { get; set; }
        public decimal Duty { get; set; }
        public decimal Area { get; set; }
        public string Remarks { get; set; }

      
    }

    public class Hdb_ImportHandlingDailyTransaction
    {
        public string LiftOnOff { get; set; }
        public string ConatinerNo { get; set; }

        public string Size { get; set; }

        public string EmptyLoad { get; set; }

        public string Remarks { get; set; }
    }

    public class Hdb_ImportDeliveryCargoDailyTransaction
    {

        public string SlNo { get; set; }
        public string IssueSlipNumber { get; set; }
        public string Date { get; set; }

        public string VehicleNumber { get; set; }

        public string GatePassNoDate { get; set; }
        public string FCLLCL { get; set; }

        public decimal ValueDuty { get; set; }

        public string InvoiceNoDate { get; set; }
        public string DeliveryorderNoDate { get; set; }
        public string BLNoDate { get; set; }
        public string ImporterName { get; set; }

        public string CHAName { get; set; }

        public string BENoDate { get; set; }
        public string CargoDescription { get; set; }

        public string CommodityType { get; set; }

        public decimal NoOfPackages { get; set; }
     
        public decimal Weight { get; set; }

        public decimal Value { get; set; }

        public decimal Duty { get; set; }
        public decimal Area { get; set; }
        public string Remarks { get; set; }
        public string Charges { get; set; }
    }
}