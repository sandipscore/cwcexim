using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class VLDA_LorryReceipt
    {
        public int ID { get; set; }
        public string GRNo { get; set; }
        public string PartyName { get; set; }
        public int PartyID { get; set; }
        public string ShippingLine { get; set; }
        public string ShippingLineID { get; set; }
        public string LR_DATE { get; set; }
        public string StuffingPoint { get; set; }
        public string BookingNo { get; set; }
        public string TransportFrom { get; set; }
        public string Consignee { get; set; }
        public string TransportTo { get; set; }
        public string BOE { get; set; }
        public string InvoiceNo  { get; set; }
        public string Remarks { get; set; }
        public string Time { get; set; }
        public string EntryDateTime { get; set; }
        public string SystemDateTime { get; set; }
        public int Uid { get; set; }
        public int LorryId { get; set; }

        public string lstLorryDtl { get; set; }

        public List<VLDA_LorryReceiptDtl> lstLRDtl { get; set; } = new List<VLDA_LorryReceiptDtl>();
    }

}