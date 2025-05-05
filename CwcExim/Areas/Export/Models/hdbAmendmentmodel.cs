using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class hdbAmendmentmodel
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
        public string CargoType { get; set; }

    }

    public class HdbNewInfoSb
    {

        public string NewInfoPartyId { get; set; }
        public string NewInfoSBNo { get; set; }
        public DateTime NewInfoSBDate { get; set; }
        public string NewInfoCommodityID { get; set; }
        public string NewInfoWeight { get; set; }
        public string NewInfoPkg { get; set; }
        public string NewInfoFOB { get; set; }
        public string NewInfoArea { get; set; }

    }

    public class HdbOldInfoSb
    {
        public string OldShipBillNo { get; set; }
        public DateTime OldShipBillDate { get; set; }
        public string OldCartingAppId { get; set; }
        public string OldPartyId { get; set; }
        public string OldCommodityID { get; set; }
        public string OldWeight { get; set; }
        public string OldPkg { get; set; }
        public string OldFOB { get; set; }
        public string OldArea { get; set; }

    }


    public class HdbAmendmentViewModel
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

        public string CargoType { get; set; }

    }

}