using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class VRNRegisterOfOutwardSupplyModelCreditDebit
    {
        public int SlNo { get; set; }
        public string GST { get; set; }
        public string Place { get; set; }
        public string Name { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string GSTC { get; set; }
        public string PlaceC { get; set; }

        public string CreditNoteNo { get; set; }
        public string CRNoteDate { get; set; }
        public decimal ServiceValue { get; set; }
        //  public string ITaxPercent { get; set; }
        public decimal ITaxAmount { get; set; }
        //   public string CTaxPercent { get; set; }
        public decimal CTaxAmount { get; set; }
        //    public string STaxPercent { get; set; }
        public decimal STaxAmount { get; set; }
        public decimal RoundUp { get; set; }
        public decimal Total { get; set; }

      
    }
}