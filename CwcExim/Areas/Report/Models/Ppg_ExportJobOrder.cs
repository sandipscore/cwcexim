namespace CwcExim.Areas.Report.Models
{
    public class Ppg_ExportJobOrder
    {
        public string GatePassNo { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string GatePassDate { get; set; }
        public string SLA { get; set; }
        public string FRW { get; set; }
        public string CustomSeal { get; set; }
        public string ShippingSeal { get; set; }
        public string POL { get; set; }
        public string POD { get; set; }
        public string TareWeight { get; set; }
        public string CargoWeight { get; set; }
        public string Fob { get; set; }
        public string Via { get; set; }
        public string TransportMode { get; set; }
    }
}