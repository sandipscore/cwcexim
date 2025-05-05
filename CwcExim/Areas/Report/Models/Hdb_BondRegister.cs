using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_BondRegister
    {
        public List<Hdb_SACDetails> lstSACDetails { get; set; } = new List<Hdb_SACDetails>();
        public Hdb_SACDetails SACDetails { get; set; } = new Hdb_SACDetails();
        public List<SACAdvancePayment> lstAdvPayment { get; set; } = new List<SACAdvancePayment>();
        public List<UnloadingDetails> lstUnloadingDetails { get; set; } = new List<UnloadingDetails>();
        public List<DeliveryDetails> lstDeliveryDetails { get; set; } = new List<DeliveryDetails>();
        public List<DeliveryPayementDetails> lstDeliveryPaymentDet { get; set; } = new List<DeliveryPayementDetails>();
    }
    public class Hdb_SACDetails
    {
        public string CompanyAdd { get; set; } = string.Empty;
        public string CompanyEmail { get; set; } = string.Empty;
        public int DepositappId { get; set; } = 0;
        public string IMPName { get; set; }
        public string IMPAdd { get; set; }
        public string CHAName { get; set; }
        public string CHAdd { get; set; }
        public string SacNo { get; set; }
        public string SacDate { get; set; }
        public string ValidUpto { get; set; }
        public decimal AreaReserved { get; set; }
        public string GodownName { get; set; }
        public string BondNo { get; set; }
        public string BondDate { get; set; }
        public string BondBOENo { get; set; }
        public string BondBOEDate { get; set; }
        public string BondAWBNo { get; set; }
        public string BondAWBDate { get; set; }
        public string DeliveryOrderDate { get; set; }
        public string ExpDateofWarehouse { get; set; }
    }
    public class SACAdvancePayment
    {
        public string SacNo { get; set; } = string.Empty;
        public decimal InvoiceAmt { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
    }
    public class UnloadingDetails
    {
        public int DepositappId { get; set; } = 0;
        public string UnloadedDate { get; set; }
        public int UnloadedUnits { get; set; }
        public decimal UnloadedWeights { get; set; }
        public decimal AreaOccupied { get; set; }
        public string PackageCondition { get; set; }
        public decimal Value { get; set; }
        public decimal Duty { get; set; }
    }
    public class DeliveryDetails
    {
        public int DepositappId { get; set; } = 0;
        public string DeliveryOrderNo { get; set; }
        public int Units { get; set; }
        public decimal Weight { get; set; }
        public decimal SQM { get; set; }
        public decimal Value { get; set; }
        public decimal Duty { get; set; }
        public string BondBOENo { get; set; }
        public string BondBOEDate { get; set; }
        public string DeliveryOrderDate { get; set; }
        public int DeliveryOrderDtlId { get; set; }
    }
    public class DeliveryPayementDetails
    {
        public int DepositappId { get; set; } = 0;
        public decimal InvoiceAmt { get; set; }
        public string DeliveryDate { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
    }
    public class BondDetails
    {
        public int DepositAppId { get; set; }
        public string BondNo { get; set; }
    }
}