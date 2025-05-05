using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Export.Models
{ 
    public class VIZ_LoadedContainerInvoice: VIZ_MovementInvoice
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

        public List<VIZ_PaySheetStuffingRequest> lstPaySheetStuffingRequest { get; set; } = new List<VIZ_PaySheetStuffingRequest>();
        public List<VIZ_PaymentSheetContainer> lstPaySheetContainer { get; set; } = new List<VIZ_PaymentSheetContainer>();
        public string GateEntryDate { get; set; }
    }

    public class VIZ_PaySheetStuffingRequest
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
        public string ReqContNo { get; set; }

        public string CustomSealNo { get; set; }

        public string ExamType { get; set; }

        public int BillToParty { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public int ExporterId { get; set; }
        public int ExpCount { get; set; }
    }
    public class VIZ_PaymentSheetContainer
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public bool Selected { get; set; }
        public string Size { get; set; }
        public string ArrivalDt { get; set; }
        public string IsHaz { get; set; }
        public string ExamType { get; set; }
    }
    public class VIZ_PaymentPartyName
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        
    }

    public class VIZ_BounceChk
    {
        public decimal BouncedCheque { get; set; }
    }
    public class VIZ_PaymentPartyNameGroup
    {
        public List<VIZ_PaymentPartyName> partylist { get; set; }
        public VIZ_BounceChk bouncecheck { get; set; }
    }
    }