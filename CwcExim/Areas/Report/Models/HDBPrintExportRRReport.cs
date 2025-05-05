using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class HDBPrintExportRRReport
    {       
        public string InvoiceNo { get; set; }
        public string CFS_Port { get; set; }
        public string PortCode { get; set; }
        public string SLine { get; set; }        
        public decimal TW { get; set; }

        public List<HdbPartyDetails> LstPartyDetails { get; set; } = new List<HdbPartyDetails>();
        public List<HdbContDetails> LstContDetails { get; set; } = new List<HdbContDetails>();
        public List<HdbChargesDetails> LstChargesDetails { get; set; } = new List<HdbChargesDetails>();
        public List<HdbShipBillDetails> LstShipBillDetails { get; set; } = new List<HdbShipBillDetails>();

        public List<HdbContainerDetails> LstContainerDetails { get; set; } = new List<HdbContainerDetails>();
    }
    public class HdbPartyDetails
    {
        public string PartyName { get; set; }
        public string PartyAddress { get; set; }
        public string PayeeName { get; set; }
       
        public string PayeeAddress { get; set; }

    }

    public class HdbContDetails
    {
        public String GatePassDate { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string VehicleNo { get; set; }
        public Int32 NoOfPackages { get; set; }
        public Decimal GrossWt { get; set; }
        public string CustomSeal { get; set; }

    }


    public class HdbChargesDetails
    {      
        public decimal FR { get; set; }
        public decimal HND { get; set; }
        public decimal TPT { get; set; }
        public decimal MSC { get; set; }      

    }


    public class HdbShipBillDetails
    {
        public string ShippingBill { get; set; }      
      
    }
    public class HdbReceiptDetails
    {
        public string InvoiceNo { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public decimal InvoiceAmt { get; set; }

        public string PayeeName { get; set; }
    }
    public class HdbContainerDetails
    {
        public string ExporterName { get; set; }
        public string ShippingLine { get; set; }
        public string ContainereNo { get; set; }
    }
  
}