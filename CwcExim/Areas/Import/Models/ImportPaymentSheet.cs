using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class PaymentSheet
    {
        public int ExpPaySheetId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string InvoiceType { get; set; }

        public string InvoiceNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string InvoiceDate { get; set; }

        public int StuffingReqId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string StuffingReqNo { get; set; }

        public string StuffingReqDate { get; set; }

        public int PartyId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PartyName { get; set; }

        public int PayeeId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PayeeName { get; set; }

        public string GSTNo { get; set; }

        public string Remarks { get; set; }
        public decimal CWCTotCharge { get; set; }
        public decimal HTTotCharge { get; set; }
        public decimal AllTotal { get; set; }
        public decimal RoundUp { get; set; }
        public decimal InvoiceValue { get; set; }

        IList<PaySheetContainer> lstPaySheetContainer { get; set; } = new List<PaySheetContainer>();
        public string PaymentSheetContainerJS { get; set; }

    }
    public class PaySheetContainer
    {
        public string CFSNo { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string Reefer { get; set; }
        public bool MyProperty { get; set; }
        public string Insured { get; set; }
        public bool Insure { get; set; }
        public string DODate { get; set; }
    }
    public class CWCCharges
    {
        public string ChargeName { get; set; }
        public decimal Value { get; set; }
        public decimal IGST { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal CGST { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGST { get; set; }
        public decimal SGSTAmt { get; set; }
    }
    public class HTCharges
    {
        public string ChargeName { get; set; }
        public decimal Value { get; set; }
        public decimal IGST { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal CGST { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGST { get; set; }
        public decimal SGSTAmt { get; set; }
    }
    public class PaySheetStuffingRequest
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public string CHAGSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public int StuffingReqId { get; set; }
        public int DeliveryType { get; set; }
        public string StuffingReqNo { get; set; }
        public string StuffingReqDate { get; set; }

        public string DestuffingEntryDate { get; set; } = "";

        public string Importer { get; set; } = "";

        public int ForwarderId { get; set; } = 0;
        public string Forwarder { get; set; } = "";
        public string HBLNo { get; set; }

       

    }
    public class PaymentSheetContainer
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public bool Selected { get; set; }
        public string Size { get; set; }
        public string ArrivalDt { get; set; }
        public string IsHaz { get; set; }
        public string LCLFCL { get; set; }
        public string LineNo { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }

        public string OBLNo { get; set; }
        public string SealCutDate { get; set; }
        public int NoOfPkg { get; set; }
        public decimal GrWait { get; set; }

        public bool IsBond { get; set; }
        public int CargoType { get; set; }

    }
    public class PaymentPartyName
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string PartyCode { get; set; }
    }
    public class PaymentSheetBOE
    {
        public string CFSCode { get; set; }
        public string LineNo { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public bool Selected { get; set; }
    }
}