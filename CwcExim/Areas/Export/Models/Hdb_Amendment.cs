using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Hdb_Amendment
    {
        public string CartingAppId { get; set; }
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

        public string CartingNo { get; set; }
        public string SBType { get; set; }
        public string ShippingLineName { get; set; }

        public string GodownName { get; set; }
        public string PortOfLoading { get; set; }

        public string PostOfDest { get; set; }

        public int ShippingLineId { get; set; }
        public int GodownId { get; set; }

        public int PortOfLoadingId { get; set; }

        public int PortofDestId { get; set; }
        public string PackUQCCode { get; set; }
        public string PackUQCDescription { get; set; }

    }



    public class Hdb_AmendmentViewModel
    {
        public int ID { get; set; }
        public string ShipBillNo { get; set; }
        public DateTime Date { get; set; }
        public string ExporterName { get; set; }
        public string ShippingLineName { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDest { get; set; }
        public string SBTypeName { get; set; }
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

        public int SBType { get; set; }
        public int ShippingLineId { get; set; }
        public int PortOfLoadingId { get; set; }
        public int PortOfDestId { get; set; }



        public string GodownName { get; set; }

        public int GodownId { get; set; }

        public string PackUQCCode { get; set; }
        public string PackUQCDescription { get; set; }


    }
}