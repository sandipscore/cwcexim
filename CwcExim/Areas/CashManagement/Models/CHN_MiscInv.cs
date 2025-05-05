namespace CwcExim.Areas.CashManagement.Models
{
    public class CHN_MiscInv
    {
        public decimal Amount { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal Total { get; set; }
        public decimal Round_up { get; set; }
        public string  SACCode { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal SGSTPer { get; set; }
        public decimal InvoiceAmt { get; set; }

    }
    public class CHNMiscPostModel:MiscInvModel
    {

        public decimal Taxable { get; set; }
        public decimal Discount { get; set; }

        public decimal? DiscountPer { get; set; }
        public decimal InvoiceAmt { get; set; }
        public string SACCode { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal SGSTPer { get; set; }
        public string   ChargeName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string SEZ { get; set; }
    }
}