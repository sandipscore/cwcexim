using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Export.Models
{
    public class PPG_LoadedContainerInvoice: PPG_MovementInvoice
    {
        public int ExpPaySheetId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string InvoiceTypee { get; set; }

       

        [Required(ErrorMessage = "Fill Out This Field")]
        public string InvoiceDatee { get; set; }

        public int StuffingReqId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string StuffingReqNo { get; set; }

        public string StuffingReqDate { get; set; }

       

        public string GSTNo { get; set; }

        public List<PPG_PaySheetStuffingRequest> lstPaySheetStuffingRequest { get; set; } = new List<PPG_PaySheetStuffingRequest>();
        public List<PPG_PaymentSheetContainer> lstPaySheetContainer { get; set; } = new List<PPG_PaymentSheetContainer>();
        public string GateEntryDate { get; set; }
    }

    public class PPG_PaySheetStuffingRequest
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public string CHAGSTNo { get; set; }
        public int StuffingReqId { get; set; }
        public string StuffingReqNo { get; set; }
        public string StuffingReqDate { get; set; }
        public string ShippingLineName { get; set; }
        public string ExporterName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string GateEntryDateTime { get; set; }
    }
    public class PPG_PaymentSheetContainer
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public bool Selected { get; set; }
        public string Size { get; set; }
        public string ArrivalDt { get; set; }
        public string IsHaz { get; set; }
        public string IsOnWheel { get; set; }
    }
    public class PPG_PaymentPartyName
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
    }
}