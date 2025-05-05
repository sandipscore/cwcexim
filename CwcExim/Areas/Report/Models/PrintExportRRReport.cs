using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class PrintExportRRReport
    {
        public string TrainNo { get; set; }
        public string TrainDate { get; set; }
        public string InvoiceNo { get; set; }
        public string CFS_Port { get; set; }
        public string PortCode { get; set; }
        public string SLine { get; set; }
        public string CustomSeal { get; set; }
        public decimal TW { get; set; }

        public List<ppgPartyDetails> LstPartyDetails { get; set; } = new List<ppgPartyDetails>();
        public List<ppgContDetails> LstContDetails { get; set; } = new List<ppgContDetails>();
        public List<ppgChargesDetails> LstChargesDetails { get; set; } = new List<ppgChargesDetails>();
        public List<ppgCompDetails> LstCompDetails { get; set; } = new List<ppgCompDetails>();

        public List<ppgReceiptDetails> LstReceiptDetails { get; set; } = new List<ppgReceiptDetails>();
    }
    public class ppgPartyDetails
    {
        public string PartyName { get; set; }
        public string PartyAddress { get; set; }
        public string PayeeName { get; set; }
       
        public string PayeeAddress { get; set; }

    }

    public class ppgContDetails
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string Loaded { get; set; }
        public string cType { get; set; }
        public Int32 NoOfPackages { get; set; }
        public Decimal GrossWt { get; set; }

       

    }


    public class ppgChargesDetails
    {
      
        public decimal RR { get; set; }
        public decimal THC { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal Inv { get; set; }
       

    }


    public class ppgCompDetails
    {
        public string Pan { get; set; }
        public string GstIn { get; set; }
      
    }
    public class ppgReceiptDetails
    {
        public string InvoiceNo { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public decimal InvoiceAmt { get; set; }

        public string PayeeName { get; set; }
    }
  
}