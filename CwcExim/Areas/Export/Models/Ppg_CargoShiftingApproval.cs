using System.Collections.Generic;

namespace CwcExim.Areas.Export.Models
{
    public class Ppg_CargoShiftingApproval
    {
        public int CargoShiftingId { get; set; }
        public string ShiftingNo { get; set; }
        public string ShiftingDt { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string GSTNo { get; set; }
        public int FromShippingId { get; set; }
        public string FromShippingName { get; set; }
        public int ToShippingId { get; set; }
        public string ToShippingName { get; set; }
        public int FromGodownId { get; set; }
        public string FromGodownName { get; set; }
        public int ToGodownId { get; set; }
        public string ToGodownName { get; set; }
        public int PayeeId { get; set; }
        public string PayeeName { get; set; }
        public string ShiftingType { get; set; }
        public List<Ppg_CartingRegisterDetail> lstCartingRegisterDtl { get; set; } = new List<Ppg_CartingRegisterDetail>();
        public int IsApproved { get; set; } = 0;
    }
    public class Ppg_CartingRegisterDetail
    {
        public int CartingRegisterDtlId { get; set; }
        public string CartingRegisterNo { get; set; }
        public string RegisterDate { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public decimal SQM { get; set; }
        public int ActualQty { get; set; }
        public decimal ActualWeight { get; set; }
    }
}