using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class LNSM_TDSReport
    {

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        // [Required(ErrorMessage = "Fill Out This Field")]
        public string PartyName { get; set; }
        public string PartyId { get; set; }
        public string ReportType { get; set; }


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

        public string ReceiptAmount { get; set; } = "";

        public string PartyCode { get; set; } = "";
        public string CertificateNo { get; set; } = "";
        public string FinancialYear { get; set; } = "";
        public string QUARTERMONTH { get; set; } = "";
        public string Remarks { get; set; } = "";



    }

    public class LNSM_TDSMain
    {
        public IList<LNSM_TDSReport> TDSReportLst = new List<LNSM_TDSReport>();

        public IList<TDSReporPartyWise> ObjTDSReporPartyWise = new List<TDSReporPartyWise>();

    }
}
