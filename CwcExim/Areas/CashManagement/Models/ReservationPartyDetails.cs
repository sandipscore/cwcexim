using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class ReservationPartyDetails
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GstNo { get; set; }
        public string Address { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }

        //----------------------------------
        public decimal Amount { get; set; } = 0;
        public decimal CGSTPer { get; set; } = 0;
        public decimal CGST { get; set; } = 0;
        public decimal SGSTPer { get; set; } = 0;
        public decimal SGST { get; set; } = 0;
        public decimal IGSTPer { get; set; } = 0;
        public decimal IGST { get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public decimal RoundOff { get; set; } = 0;    
        public decimal InvoiceAmt { get; set; } = 0;
        public string Remarks { get; set; } = "";
        public string InvoiceDate { get; set; } = "";

        //------------------------------------------------
        public string InvoiceNo { get; set; } = "";
        public int InvoiceId { get; set; } = 0;

        //------------------------------------------------
        public int GodownId { get; set; }
        public string GodownName { get; set; }      
        public decimal GF { get; set; } = 0;
        public decimal MF { get; set; } = 0;
        public decimal TotalSpace { get; set; } = 0;

        //----------------------------------------------
        public string ComStateCode { get; set; }
        public int IsCashReceipt { get; set; } = 0;

        public string SEZ { get; set; }
    }


}