using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

  
namespace CwcExim.Areas.Import.Models
{
    public class Hdb_FormOneLclModel
    {
        public int FormOneId { get; set; }

        [DisplayName("Form 1 No.")]
        public string FormOneNo { get; set; }

        [DisplayName("Form 1 Date")]
        public string FormOneDate { get; set; }

        [DisplayName("BL No.")]
        public string BLNo { get; set; }
        [DisplayName("ICE Gate ContainerNo.")]
        public string IceGateContNo { get; set; }

        [DisplayName("ICE Gate ContainerSize.")]
        public string IcegateContSize { get; set; }

        public int? ShippingLineId { get; set; }

        [DisplayName("Shipping Line Name"), Required(ErrorMessage = "Fill Out This Field")]
        public string ShippingLineName { get; set; }

        [DisplayName("Vessel Name")]
        public string VesselName { get; set; }

        [DisplayName("Voyage No.")]
        public string VoyageNo { get; set; }

        [DisplayName("Rotation No.")]
        public string RotationNo { get; set; }

        public int PortOfDischargeId { get; set; }

        [DisplayName("Port of Discharge")]
        public string PortName { get; set; }

        [DisplayName("Port Charge"), Required(ErrorMessage = "Fill Out This Field")]
        public decimal PortCharge { get; set; }

        [DisplayName("Port Charge Amount")]
        public decimal PortChargeAmt { get; set; }

        [DisplayName("Cargo Type")]
        public int CargoType { get; set; }

    

        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public IList<Hdb_FormoneLclDetailModel> lstFormOneDetails { get; set; } = new List<Hdb_FormoneLclDetailModel>();
        public IList<ShippingLine> lstShippingLine { get; set; } = new List<ShippingLine>();
        public IList<PortOfDischarge> lstPOD { get; set; } = new List<PortOfDischarge>();
        public IList<CHA> lstCHA { get; set; } = new List<CHA>();
        public IList<Importer> lstImporter { get; set; } = new List<Importer>();
        public IList<TSAForwarder> lstTSAForwarder { get; set; } = new List<TSAForwarder>();
        public IList<Commodity> lstCommodity { get; set; } = new List<Commodity>();

        [Required(ErrorMessage = "Container Details Cannot Be Left Blank")]
        public string FormOneDetailsJS { get; set; }

        public string TrBondNo { get; set; }

        public string ISO { get; set; }

        public string ContainerNo { get; set; }

        public string GrossWeight { get; set; }

        public string CfsCode { get; set; }

        public string Pol { get; set; }

        public string SealCode { get; set; }
        public string CBT { get; set; }

        [DisplayName("CBT From")]
        public string CBTFrom { get; set; }

        public int GateId { get; set; }

        //public decimal CIFValue { get; set; }
        //public decimal GrossDuty { get; set; }
        //public string PackageType { get; set; }
        //public int Noofpkg { get; set; }
        //public string OBLNO { get; set; }


    }



}
