using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DSRTDSReport
    {

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

       // [Required(ErrorMessage = "Fill Out This Field")]
        public string PartyName { get; set; }
        public string PartyId { get; set; }


        //#for list
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string CRNo { get; set; }
        public string CRDate { get; set; }

        public string PartyTAN { get; set; }
        public string InvoiceValue { get; set; }

        
        public string TDS { get; set; }

        public string TDSPlus { get; set; }

        public string Amount { get; set; } = "";
        

        public string PartyCode { get; set; }="";
        public string CertificateNo { get; set; }="";
        public string FinancialYear { get; set; }="";
        public string QUARTERMONTH { get; set; }="";
        public string Remarks { get; set; }="";




    }

    public class DSRTDSMain
    {
     public   IList<DSRTDSReport> TDSReportLst = new List<DSRTDSReport>();

      public  IList<DSRTDSReporPartyWise> ObjTDSReporPartyWise = new List<DSRTDSReporPartyWise>();

    }

    public class DSRTDSReporPartyWise

    {

        public string Party { get; set; }
        public string Tan { get; set; }
        public string Value { get; set; }
        public string TDS { get; set; }

        public string TDSPlus { get; set; }

    }
}