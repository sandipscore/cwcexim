using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class DSR_Amendment
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
        public string CHAName { get; set; }
        public decimal UnReservedCBM { get; set; }
        public decimal ReservedCBM { get; set; }
        public string ManualWT { get; set; }
        public string MechanicalWT { get; set; }

    }

    public class DSR_AmendmentViewModel
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

        public string CHAName { get; set; }

        public string CCINNo { get; set; }
        public int SBType { get; set; }
        public int ShippingLineId { get; set; }
        public int PortOfLoadingId { get; set; }
        public int PortOfDestId { get; set; }


        public decimal OldWeight { get; set; }
        public decimal OldPkg { get; set; }
        public string OldFOB { get; set; }
        public decimal OldManualWT { get; set; }
        public decimal OldMechanicalWT { get; set; }
        public decimal ManualWT { get; set; }
        public decimal MechanicalWT { get; set; }
    }
    public class DSR_NewInfoSb
    {
        public string NewInfoPartyId { get; set; }
        public string NewInfoSBNo { get; set; }
        public DateTime NewInfoSBDate { get; set; }
        public string NewInfoCommodityID { get; set; }
        public string NewInfoWeight { get; set; }
        public string NewInfoPkg { get; set; }
        public string NewInfoFOB { get; set; }
        public string NewInfoArea { get; set; }

        public int? NewNoOfVehicle { get; set; }
        public string NewCargoInvoiceNo { get; set; }
        public DateTime? NewCargoInvoiceDate { get; set; }

        public string NewInfoUnReservedCBM { get; set; }
        public string NewInfoReservedCBM { get; set; }
        public string NewInfoManualWT { get; set; }
        public string NewInfoMechanicalWT { get; set; }



    }

    public class DSR_OldInfoSb
    {
        public string OldShipBillNo { get; set; }
        public DateTime OldShipBillDate { get; set; }
        public string OldCCINId { get; set; }
        public string OldPartyId { get; set; }
        public string OldCommodityID { get; set; }
        public string OldWeight { get; set; }
        public string OldPkg { get; set; }
        public string OldFOB { get; set; }
        public string OldArea { get; set; }

        public int? OldNoOfVehicle { get; set; }
        public string OldCargoInvoiceNo { get; set; }
        public DateTime? OldCargoInvoiceDate { get; set; }
        public string OldUnReservedCBM { get; set; }
        public string OldReservedCBM { get; set; }
        public string OldMechanicalWT { get; set; }
        public string OldManualWT { get; set; }
    }
}