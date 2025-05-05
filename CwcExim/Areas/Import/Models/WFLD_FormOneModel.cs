using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Import.Models
{
    public class WFLD_FormOneModel
    {
        public int FormOneId { get; set; }

        [DisplayName("Form 1 No.")]
        public string FormOneNo { get; set; }

        [DisplayName("Form 1 Date")]
        public string FormOneDate { get; set; }

       
        public int ShippingLineId { get; set; }

        [DisplayName("Shipping Line Name"), Required(ErrorMessage = "Fill Out This Field")]
        public string ShippingLineName { get; set; }

        [DisplayName("Vessel Name"), Required(ErrorMessage = "Fill Out This Field")]
        public string VesselName { get; set; }

        [DisplayName("Voyage No."), Required(ErrorMessage = "Fill Out This Field")]
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

      

        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public IList<WFLD_FormOneDetail> lstFormOneDetail { get; set; } = new List<WFLD_FormOneDetail>();
        public IList<ShippingLine> lstShippingLine { get; set; } = new List<ShippingLine>();
        public IList<PortOfDischarge> lstPOD { get; set; } = new List<PortOfDischarge>();
        public IList<CHA> lstCHA { get; set; } = new List<CHA>();
        public IList<Importer> lstImporter { get; set; } = new List<Importer>();
        public IList<WFLD_Commodity> lstCommodity { get; set; } = new List<WFLD_Commodity>();

        [Required(ErrorMessage = "Container Details Cannot Be Left Blank")]
        public string FormOneDetailsJS { get; set; }

        public string TrBondNo { get; set; }

    }
}