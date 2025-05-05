namespace CwcExim.Areas.Export.Models
{
    public class Ppg_ContStuffChargesV2
    {
        public int OperationId { get; set; }
        public string ChargeType { get; set; }
        public string ChargeName { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTPer { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal Taxable { get; set; }
        public decimal Total { get; set; }
        public string SACCode { get; set; }
    }
}