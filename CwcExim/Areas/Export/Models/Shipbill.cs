using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class Shipbill
    {
        [MaxLength(30,ErrorMessage = "Shipping Bill No.must be in 30 characters")]
        [Required(ErrorMessage ="Fill Out This Field")]
        public string OldShipbill { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [MaxLength(30, ErrorMessage = "Shipping Bill No.must be in 30 characters")]
        public string NewShipbill { get; set; }
    }
    public class ShippingBillList
    {
        public string OldShipbill { get; set; }
        public string NewShipbill { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
    }
}
