using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class VRN_AmendmentViewModel
    {
        public int ID { get; set; }
        public string ShipBillNo { get; set; }
        public DateTime Date { get; set; }
        public string ExporterName { get; set; }
        public string ExporterID { get; set; }
        public string Cargo { get; set; }

        public int CargoID { get; set; }
        public decimal Weight { get; set; }
        public decimal Pkg { get; set; }
        public string NewShipBillNo { get; set; }
        public string ShipbillDate { get; set; }
        public string OldShipBillDate { get; set; }
        public string FOB { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string AmendNo { get; set; }
        public string ShippingLineName { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDest { get; set; }
        public string SBTypeName { get; set; }

        public string CCINNo { get; set; }
        public int SBType { get; set; }
        public int ShippingLineId { get; set; }
        public int PortOfLoadingId { get; set; }
        public int PortOfDestId { get; set; }


        public decimal OldWeight { get; set; }
        public decimal OldPkg { get; set; }
        public string OldFOB { get; set; }
        public string GodownName { get; set; }
    }
}