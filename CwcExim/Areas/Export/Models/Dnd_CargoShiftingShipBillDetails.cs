using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class Dnd_CargoShiftingShipBillDetails
    {
        public int CartingRegisterId { get; set; }
        public string CartingRegisterNo { get; set; }
        public string RegisterDate { get; set; }
        public int CartingRegisterDtlId { get; set; }
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        [Required(ErrorMessage = "This field is Required")]
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public decimal ActualQty { get; set; }
        public decimal ActualWeight { get; set; }
        public bool IsChecked { get; set; } = false;
        public decimal SQM { get; set; }
        public int ShippinglineId { get; set; }
        public string ShippingLineName { get; set; }
        public int ToGodownId { get; set; }
        [Required(ErrorMessage ="This field is Required")]
        public string ToGodownName { get; set; }
        public int FromGodownId { get; set; }
        public string FromGodownName { get; set; }
        public string ToLocationId { get; set; }
        public string ToLocationName { get; set; }
        public string FromLocationId { get; set; }
        public string FromLocationName { get; set; }
        public string Remarks { get; set; }
        public int CargoShiftingId { get; set; }
        public string ShiftingDt { get; set; }
        public string ShiftingNo { get; set; }
        public decimal FOB { get; set; }
        public bool IsApproved { get; set; }
        public IList<Dnd_ApplicationNoDet> lstAppNo { get; set; } = new List<Dnd_ApplicationNoDet>();
    }

    public class Dnd_InvoiceCargoShifting : PPG_InvoiceBase
    {
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
        public List<Dnd_CargoShiftingShipBillDetails> lstShippingDet { get; set; } = new List<Dnd_CargoShiftingShipBillDetails>();

    }

    public class Dnd_ApplicationNoDet
    {
        public string ApplicationNo { get; set; }
        public int CartingAppId { get; set; }
        public int CargoShiftingId { get; set; }
    }
}