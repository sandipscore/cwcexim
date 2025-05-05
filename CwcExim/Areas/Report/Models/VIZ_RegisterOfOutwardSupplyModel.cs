using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class VIZ_RegisterOfOutwardSupplyModel
    {
        
        /*****************Invoice Data***************/
        public int SlNo { get; set; }
        public string GST { get; set; }
        public string Place { get; set; }
        public string Name { get; set; }
        public string Period { get; set; }
        public string Nature { get; set; }
        public string HSNCode { get; set; }
        public decimal Rate { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public decimal ServiceValue { get; set; }
        public string ITaxPercent { get; set; }
        public decimal ITaxAmount { get; set; }
        public string CTaxPercent { get; set; }
        public decimal CTaxAmount { get; set; }
        public string STaxPercent { get; set; }
        public decimal STaxAmount { get; set; }
        public decimal Total { get; set; }
        /*****************Invoice Data***************/
       
        public string PaymentMode { get; set; }
        public string Remarks { get; set; } 


        /*****************CashReceipt Data***************/

    }

    public class VIZ_RegisterOfOutwardSupplyModelCreditDebit
    {

        public int SlNo { get; set; }
        public string GST { get; set; }
        public string Place { get; set; }
        public string Name { get; set; }
        public string Period { get; set; }
        public string Nature { get; set; }
        public string HSNCode { get; set; }
        public decimal Rate { get; set; }
        public string CreditNoteNo { get; set; }
        public string CRNoteDate { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public decimal ServiceValue { get; set; }
        public string ITaxPercent { get; set; }
        public decimal ITaxAmount { get; set; }
        public string CTaxPercent { get; set; }
        public decimal CTaxAmount { get; set; }
        public string STaxPercent { get; set; }
        public decimal STaxAmount { get; set; }
        public decimal Total { get; set; }
       
        public string Remarks { get; set; }



    }

}