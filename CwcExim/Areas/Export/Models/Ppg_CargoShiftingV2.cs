using System.Collections.Generic;

namespace CwcExim.Areas.Export.Models
{
    public class Ppg_CargoShiftingV2
    {
        public string ShiftingNo { get; set; }
        public int CargoShiftingId { get; set; }
        public string ShiftingDt { get; set; }
        public string FromGodownName { get; set; }
        public string ToGodownName { get; set; }
        public int FromShippingId { get; set; }
        public string FromShippingLineName { get; set; }
        public int ToShippingId { get; set; }
        public string ToShippingLineName { get; set; }
        public List<CargoShiftingShipBillDetails> lstShippingDet { get; set; } = new List<CargoShiftingShipBillDetails>();

    }
}