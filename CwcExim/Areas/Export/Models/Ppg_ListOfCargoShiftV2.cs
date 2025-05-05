namespace CwcExim.Areas.Export.Models
{
    public class Ppg_ListOfCargoShiftV2
    {
        public string ShiftingNo { get; set; }
        public string ShiftingDt { get; set; }
        public string FromShipping { get; set; }
        public string ToShipping { get; set; }
        public string FromGodown { get; set; }
        public string ToGodown { get; set; }
        public string ShiftingType { get; set; }
        public int CargoShiftingId { get; set; }
    }
}