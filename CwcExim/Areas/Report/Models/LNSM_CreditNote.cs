using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class UpiQRCodeInfoCR
    {
        public string ver { get; set; }
        public int orgId { get; set; }
        public int mode { get; set; }
        public string tid { get; set; }

        public string tr { get; set; }
        public string tn { get; set; }
        public string pa { get; set; }
        public string pn { get; set; }
        public string mc { get; set; }
        public decimal am { get; set; }
        public decimal mam { get; set; }
        public string mid { get; set; }
        public string msid { get; set; }
        public string mtid { get; set; }
        public string gstBrkUp { get; set; }
        public int qrMedium { get; set; }

        public string invoiceNo { get; set; }

        public string InvoiceDate { get; set; }
        public string InvoiceName { get; set; }
        public string QRexpire { get; set; }

        public int pinCode { get; set; }
        public string tier { get; set; }

        public string gstIn { get; set; }


        public decimal CGST { get; set; }
        public decimal SGST { get; set; }

        public decimal IGST { get; set; }
        public decimal CESS { get; set; }
        public decimal GSTIncentive { get; set; }
        public decimal GSTPCT { get; set; }

        public int merchant_id { get; set; }
        public int order_id { get; set; }


        public string redirect_url { get; set; }
        public string cancel_url { get; set; }
        public string language { get; set; }

        public string billing_name { get; set; }
        public string billing_address { get; set; }

        public string billing_city { get; set; }
        public string billing_state { get; set; }
        public string billing_zip { get; set; }
        public string billing_country { get; set; }
        public string billing_tel { get; set; }
        public string billing_email { get; set; }

        public string delivery_tel { get; set; }
        public int merchant_param1 { get; set; }

    }

    public class LNSM_CreditNote
    {

        public string InvoiceModule { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public int InvoiceId { get; set; }
        public int CRNoteId { get; set; }
        public string InvoiceNumber { get; set; }
        public string CRNoteNo { get; set; }
        public string RefInvoiceNo { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string InvoiceModuleName { get; set; }
        public int PayeeId { get; set; }
        public string PayeeName { get; set; }

        //****************************************************
        public string ShippingLine { get; set; } = "";
        public int ShippingLineId { get; set; } = 0;
        public string CHAName { get; set; } = "";
        public int CHAId { get; set; } = 0;
        public string All { get; set; }
        public string GSTNO { get; set; }
        public string PartyGSTNo { get; set; }
        public string InvoiceDate { get; set; }
        public string Amount { get; set; }
        public decimal TotalAmt { get; set; }
        public List<LNSM_CreditNote> ListInvc { get; set; }
    }

    public class PrintPDFModelOfCr
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompStateName { get; set; }
        public string CompStateCode { get; set; }
        public string CompCityName { get; set; }
        public string CompGstIn { get; set; }
        public string CompPan { get; set; }
        public string PartyName { get; set; }
        public string PartyAddress { get; set; }
        public string PartyCityName { get; set; }
        public string PartyStateName { get; set; }
        public string PartyStateCode { get; set; }
        public string PartyGSTIN { get; set; }
        public string CRNoteNo { get; set; }
        public string CRNoteDate { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal RoundUp { get; set; }
        public decimal GrandTotal { get; set; }
        public string Remarks { get; set; }
        public IList<ChargesModels> lstCharges { get; set; } = new List<ChargesModels>();

        //   #Added one column PayeeName (Whatsup group) for Print Developed by Dilip Samanta as on 03/06/2020,1430
        public string PayeeName { get; set; }
        public string irn { get; set; }
        public string SignedQRCode { get; set; }
        public string SupplyType { get; set; }


        public string ver { get; set; }
        public int orgId { get; set; }
        public int mode { get; set; }
        public string tid { get; set; }

        public string tr { get; set; }
        public string tn { get; set; }
        public string pa { get; set; }
        public string pn { get; set; }
        public string mc { get; set; }
        public decimal am { get; set; }
        public decimal mam { get; set; }
        public string mid { get; set; }
        public string msid { get; set; }
        public string mtid { get; set; }
        public string gstBrkUp { get; set; }
        public int qrMedium { get; set; }

        public string invoiceNo { get; set; }

        public string QRInvoiceDate { get; set; }
        public string InvoiceName { get; set; }
        public string QRexpire { get; set; }

        public int pinCode { get; set; }
        public string tier { get; set; }



        public decimal CGST { get; set; }
        public decimal SGST { get; set; }

        public decimal IGST { get; set; }
        public decimal CESS { get; set; }
        public decimal GSTIncentive { get; set; }
        public decimal GSTPCT { get; set; }
        public int CrNoteId { get; set; }

        public string gstIn { get; set; }

        public string AmountInWord { get; set; }
    }
    public class ChargesModels
    {
        public string ChargeName { get; set; }
        public decimal Taxable { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTPer { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal Total { get; set; }
        public string SACCode { get; set; }
        public int CRNoteId { get; set; }
    }

}