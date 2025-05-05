using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class VRN_Amendment
    {
        public string CCINId { get; set; }
        public string AmendmentNo { get; set; }
        public string AmendmentDate { get; set; }
        public string ShipBillNo { get; set; }
        public int ExporterId { get; set; }
        public int CommodityId { get; set; }
        public string ShipBillAmendmentDate { get; set; }
        public string Exporter { get; set; }
        public string Cargo { get; set; }
        public string Weight { get; set; }
        public string Pkg { get; set; }
        public string Area { get; set; }
        public string ShipBillDate { get; set; }
        public string FOBValue { get; set; }
        public string Duty { get; set; }
        public string Type { get; set; }
        public int IsApprove { get; set; }

        public int Cutting { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string ShortCargo { get; set; }
        public string CCINNO { get; set; }
        public string SBType { get; set; }
        public string ShippingLineName { get; set; }
        public string PortOfLoading { get; set; }

        public string PostOfDest { get; set; }

        public int ShippingLineId { get; set; }

        public int PortOfLoadingId { get; set; }

        public int PortofDestId { get; set; }

        public int VehicleNo { get; set; }

        public string CCINInvoiceNo { get; set; }
        public string CCINInvoiceDate { get; set; }

        public int NoofGround { get; set; }
    }
}