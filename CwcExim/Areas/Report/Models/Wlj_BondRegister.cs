using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Wlj_BondRegister
    {
        public List<Wlj_SACDetails> lstSACDetails { get; set; } = new List<Wlj_SACDetails>();
        public Wlj_SACDetails SACDetails { get; set; } = new Wlj_SACDetails();
        public List<Wlj_SACAdvancePayment> lstAdvPayment { get; set; } = new List<Wlj_SACAdvancePayment>();
        public List<Wlj_UnloadingDetails> lstUnloadingDetails { get; set; } = new List<Wlj_UnloadingDetails>();
        public List<Wlj_DeliveryDetails> lstDeliveryDetails { get; set; } = new List<Wlj_DeliveryDetails>();
        public List<Wlj_DeliveryPayementDetails> lstDeliveryPaymentDet { get; set; } = new List<Wlj_DeliveryPayementDetails>();
    }
    public class Wlj_SACDetails
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
        public string ExpDateofWarehouse { get; set; }
    }
    public class Wlj_SACAdvancePayment
    {
        public string SacNo { get; set; } = string.Empty;
        public decimal InvoiceAmt { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
    }
    public class Wlj_UnloadingDetails
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
    public class Wlj_DeliveryDetails
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
        public int DeliveryOrderDtlId { get; set; }
    }
    public class Wlj_DeliveryPayementDetails
    {
        public int DepositappId { get; set; } = 0;
        public decimal InvoiceAmt { get; set; }
        public string DeliveryDate { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
    }
    public class Wlj_BondDetails
    {
        public int DepositAppId { get; set; }
        public string BondNo { get; set; }
    }
}