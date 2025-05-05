using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class VRN_BondRegister
    {
        public List<VRN_SACDetails> lstSACDetails { get; set; } = new List<VRN_SACDetails>();
        public VRN_SACDetails SACDetails { get; set; } = new VRN_SACDetails();
        public List<VRN_SACAdvancePayment> lstAdvPayment { get; set; } = new List<VRN_SACAdvancePayment>();
        public List<VRN_UnloadingDetails> lstUnloadingDetails { get; set; } = new List<VRN_UnloadingDetails>();
        public List<VRN_DeliveryDetails> lstDeliveryDetails { get; set; } = new List<VRN_DeliveryDetails>();
        public List<VRN_DeliveryPayementDetails> lstDeliveryPaymentDet { get; set; } = new List<VRN_DeliveryPayementDetails>();
    }
    public class VRN_SACDetails
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
    public class VRN_SACAdvancePayment
    {
        public string SacNo { get; set; } = string.Empty;
        public decimal InvoiceAmt { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
    }
    public class VRN_UnloadingDetails
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
    public class VRN_DeliveryDetails
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
    public class VRN_DeliveryPayementDetails
    {
        public int DepositappId { get; set; } = 0;
        public decimal InvoiceAmt { get; set; }
        public string DeliveryDate { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
    }
    public class VRN_BondDetails
    {
        public int DepositAppId { get; set; }
        public string BondNo { get; set; }
    }
}