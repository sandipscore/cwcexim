using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class WFLDContainerDetailsIRR
    {
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string DisplayCfs { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLine { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string Size { get; set; }
        public int CargoType { get; set; }
        public decimal GrossWt { get; set; } = 0;
        public decimal TareWt { get; set; } = 0;
        public decimal CargoWt { get; set; } = 0;
        public string WagonNo { get; set; }       
        public string Via { get; set; }
        public string PortOfOrigin { get; set; }

    }

    public class WFLDIrrInvoice
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public int PayeeId { get; set; }
        public string PayeeName { get; set; }
        public string GstNo { get; set; }
        public string Address { get; set; }
        public int StateCode { get; set; }
        public string StateName { get; set; }

        //----------------------------------
        public string CFSCode { get; set; } = "";
        public int CargoType { get; set; }
        public int OperationId { get; set; } = 0;
        public string OperationDesc { get; set; } = "";
        public string OperationSDesc { get; set; } = "";

        public decimal Quantity { get; set; } = 0;
        public decimal Rate { get; set; } = 0;
        public string SACCode { get; set; } = "";

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
        public string InvoiceType { get; set; } = "Tax";
        //------------------------------------------------
    }

    public class WFLDIrrTrains
    {
        public int TrainSummaryID { get; set; }
        public string TrainNo { get; set; }
        public string TrainNoDate { get; set; }
        public string TrainDate { get; set; } 
    }
   }