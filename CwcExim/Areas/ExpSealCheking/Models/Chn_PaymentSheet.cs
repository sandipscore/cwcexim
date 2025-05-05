using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.ExpSealCheking.Models
{
    public class Chn_PaymentSheet
    {
        public int ExpPaySheetId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string InvoiceType { get; set; }

        public string InvoiceNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string InvoiceDate { get; set; }

        public int EntryId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string TruckSlipNo { get; set; }

        public string TruckSlipDate { get; set; }

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

        IList<Chn_PaySheetContainer> lstPaySheetContainer { get; set; } = new List<Chn_PaySheetContainer>();
        public string PaymentSheetContainerJS { get; set; }

    }
    public class Chn_PaySheetContainer
    {
        public string TruckSlipNo { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string Reefer { get; set; }
        public bool MyProperty { get; set; }
        public string Insured { get; set; }
        public bool Insure { get; set; }
        public string DODate { get; set; }
    }
    public class Chn_CWCCharges
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
    public class Chn_HTCharges
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
    public class Chn_PaySheetTruckSlipRequest
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public string CHAGSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public int EntryId { get; set; }        
        public string TruckSlipNo { get; set; }
        public string TruckSlipDate { get; set; }
    }
    public class Chn_PaymentSheetContainer
    {
        public string TruckSlipNo { get; set; }
        public string ContainerNo { get; set; }
        public bool Selected { get; set; }
        public string Size { get; set; }
        public string ArrivalDt { get; set; }
        public string TruckSlipDate { get; set; }
        public string IsHaz { get; set; }
        public string LCLFCL { get; set; }
        public string LineNo { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string CFSCode { get; set; }       
    }
    public class Chn_PaymentPartyName
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
    }
    public class Chn_PaymentSheetBOE
    {
        public string CFSCode { get; set; }
        public string LineNo { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public bool Selected { get; set; }
    }
}