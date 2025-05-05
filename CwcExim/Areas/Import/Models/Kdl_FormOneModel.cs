using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Kdl_FormOneModel
    {
        public int FormOneId { get; set; }

        [DisplayName("Form 1 No.")]
        public string FormOneNo { get; set; }

        [DisplayName("Form 1 Date")]
        public string FormOneDate { get; set; }

        [DisplayName("BL No.")]
        public string BLNo { get; set; }

        public int ShippingLineId { get; set; }

        [DisplayName("Shipping Line Name"), Required(ErrorMessage = "Fill Out This Field")]
        public string ShippingLineName { get; set; }

        [DisplayName("Vessel Name"),Required(ErrorMessage ="Fill Out This Field")]
        public string VesselName { get; set; }

        [DisplayName("Voyage No."),Required(ErrorMessage ="Fill Out This Field")]
        public string VoyageNo { get; set; }

        [DisplayName("Rotation No.")]
        public string RotationNo { get; set; }
        
        public int PortOfDischargeId { get; set; }

        [DisplayName("Port of Discharge"), Required(ErrorMessage = "Fill Out This Field")]
        public string PortName { get; set; }

        [DisplayName("Port Charge"), Required(ErrorMessage = "Fill Out This Field")]
        public decimal PortCharge { get; set; }

        [DisplayName("Port Charge Amount"), Required(ErrorMessage = "Fill Out This Field")]
        public decimal PortChargeAmt { get; set; }

        [DisplayName("Cargo Type"), Required(ErrorMessage = "Fill Out This Field")]
        public int CargoType { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string LCLFCL { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public IList<Kdl_FormOneDetailModel> lstFormOneDetail { get; set; } = new List<Kdl_FormOneDetailModel>();
        public IList<ShippingLine> lstShippingLine { get; set; } = new List<ShippingLine>();
        public IList<PortOfDischarge> lstPOD { get; set; } = new List<PortOfDischarge>();
        public IList<CHA> lstCHA { get; set; } = new List<CHA>();
        public IList<Importer> lstImporter { get; set; } = new List<Importer>();
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

    }



    public class Kdl_IALModel
    {


        [DisplayName("Cargo Type"), Required(ErrorMessage = "Fill Out This Field")]
        public int CargoType { get; set; }   

        

        public string ISO { get; set; }

        public string ContainerNo { get; set; }

        public string GrossWeight { get; set; }

        public string CfsCode { get; set; }

        public string Pol { get; set; }

        public string SealCode { get; set; }


    }
}

