using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class WFLDCargoShiftingShipBillDetails
    {
        public int CartingRegisterId { get; set; }
        public string CartingRegisterNo { get; set; }
        public string RegisterDate { get; set; }
        public int CartingRegisterDtlId { get; set; }
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public decimal ActualQty { get; set; }
        public decimal ActualWeight { get; set; }
        public bool IsChecked { get; set; } = false;
        public decimal SQM { get; set; }
    }
    public class WFLDInvoiceCargoShifting : WFLD_InvoiceBase
    {
        public List<WFLDPostPaymentChrgShifting> lstPostPaymentChrg { get; set; } = new List<WFLDPostPaymentChrgShifting>();
        public List<WFLDOperationCFSCodeWiseAmountCS> lstOperationCFSCodeWiseAmount { get; set; } = new List<WFLDOperationCFSCodeWiseAmountCS>();
        /******************************************/
        public string ShiftingNo { get; set; }
        public int CargoShiftingId { get; set; }
        public string ShiftingDt { get; set; }
        public string FromGodownName { get; set; }
        public string ToGodownName { get; set; }
        public int FromShippingId { get; set; }
        public string FromShippingLineName { get; set; }
        public int ToShippingId { get; set; }
        public string ToShippingLineName { get; set; }
        public List<WFLDCargoShiftingShipBillDetails> lstShippingDet { get; set; } = new List<WFLDCargoShiftingShipBillDetails>();
        public List<WFLDCargoPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<WFLDCargoPostPaymentChargebreakupdate>();

    }
    public class WFLDPostPaymentChrgShifting: PostPaymentCharge
    {

    }
    public class WFLDCargoPostPaymentChargebreakupdate
    {
        public int ChargeId { get; set; } = 0;
        public string Clause { get; set; } = string.Empty;
        public int ClauseOrder { get; set; } = 0;
        public string ChargeName { get; set; } = string.Empty;
        public string ChargeType { get; set; } = string.Empty;
        public string SACCode { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public decimal Rate { get; set; } = 0M;
        public decimal Amount { get; set; } = 0M;

        public int OperationId { get; set; }
        public string CFSCode { get; set; }
        public string Startdate { get; set; }
        public string EndDate { get; set; }
    }
    public class WFLDOperationCFSCodeWiseAmountCS
    {
        public int InvoiceId { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public int OperationId { get; set; }
        public string ChargeType { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
    }
}