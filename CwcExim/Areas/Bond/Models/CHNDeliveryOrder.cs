using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Bond.Models
{
    public class CHNDeliveryOrder:DeliveryOrder
    {
        public string BondNo { get; set; }
        public string BondDate { get; set; }
        public Decimal Units_print { get; set; }
        public Decimal Weight_print { get; set; }
        public int GodownId_print { get; set; }

        public List<CHNDeliveryOrderforPrint> LstDeliveryPrintOrder { get; set; } = new List<CHNDeliveryOrderforPrint>();
        public List<CHNDeliveryOrderDtl> LstDeliveryOrderhdb { get; set; } = new List<CHNDeliveryOrderDtl>();
        public List<CHNDeliveryOrderPaymentPrint> LstDeliveryOrderPayment { get; set; } = new List<CHNDeliveryOrderPaymentPrint>();

    }

    public class CHNDeliveryOrderforPrint
    {
        public string Importer { get; set; }
        public string CHA { get; set; }
        public string CargoDesc { get; set; }
        public Decimal Units { get; set; }
        public Decimal Weight { get; set; }

        public string SacNo { get; set; }
        public string SacDate { get; set; }
        public string Remarks { get; set; }

    }


    public class CHNDeliveryOrderDtl
    {
        public int DeliveryOrderDtlId { get; set; }
        public int DeliveryOrderId { get; set; }
        public int DepositAppId { get; set; }
        public string DepositNo { get; set; }
        public string DepositDate { get; set; }
        public string WRNo { get; set; }
        public string WRDate { get; set; }
        public int Units { get; set; }
        public decimal Weight { get; set; }
        public int ClosingUnits { get; set; }
        public decimal ClosingWeight { get; set; }
        public decimal Value { get; set; }
        public decimal Duty { get; set; }
        public int GodownId { get; set; }
        public string CargoDescription { get; set; }
        public string Remarks { get; set; }

    }


    public class CHNDeliveryOrderPaymentPrint
    {
        public string SacDate { get; set; }
        public Decimal SpaceReq { get; set; }
        public String DepositDate { get; set; }
        public string DepositNo { get; set; }
        public Decimal AreaReserved { get; set; }
        public string ReceiptNo { get; set; }
        public Decimal InsAmt { get; set; }
        public Decimal StoAmt { get; set; }
        public String FromDate { get; set; }
        public String ToDate { get; set; }
        public int days { get; set; }
        public int Weeks { get; set; }
        public decimal InvInsAmt { get; set; }
        public decimal InvStoAmt { get; set; }
        public decimal Tax { get; set; }
        public decimal InvoiceAmt { get; set; }
        public decimal TotalTaxable { get; set; }
        public string ReceiptDate { get; set; }
    }
    public class ListOfCHNWorkOrderNo
    {
        public string SacNo { get; set; }
        public int SpaceappId { get; set; }
        public string BondNo { get; set; }
        public int DepositappId { get; set; }
    }
    public class CHNWorkOrderDetails
    {
        public int BondWOId { get; set; }
        public string BondNo { get; set; }
        public string BondDate { get; set; }
        public string WRNo { get; set; }
        public string WRDate { get; set; }
        public int SpaceAppId { get; set; }
        public int GodownId { get; set; }
        public string WorkOrderDate { get; set; }
        public string SacNo { get; set; }
        public string SacDate { get; set; }
        public int ClosingUnits { get; set; }
        public decimal ClosingWeight { get; set; }
        public string Importer { get; set; }
        public int CHAId { get; set; }

        public string Remarks { get; set; }
    }
}