using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Kol_FormOneModel
    {
        public int FormOneId { get; set; }

        [DisplayName("Form 1 No.")]
        public string FormOneNo { get; set; }

        [DisplayName("Form 1 Date")]
        public string FormOneDate { get; set; }

        [DisplayName("BL No.")]
        public string BLNo { get; set; }

        [DisplayName("foreign liner name"), Required(ErrorMessage = "Fill Out This Field")]
        public string FlinerName { get; set; }

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
        public IList<Kol_FormOneDetailModel> lstFormOneDetail { get; set; } = new List<Kol_FormOneDetailModel>();
        public IList<ShippingLine> lstShippingLine { get; set; } = new List<ShippingLine>();
        public IList<ForeignLiner> lstForeignLiner { get; set; } = new List<ForeignLiner>();
        public IList<PortOfDischarge> lstPOD { get; set; } = new List<PortOfDischarge>();
        public IList<CHA> lstCHA { get; set; } = new List<CHA>();
        public IList<Importer> lstImporter { get; set; } = new List<Importer>();
        public IList<Commodity> lstCommodity { get; set; } = new List<Commodity>();

        [Required(ErrorMessage = "Container Details Cannot Be Left Blank")]
        public string FormOneDetailsJS { get; set; }

        public string TrBondNo { get; set; }

    }


    public class ForeignLiner
    {
       
        public string ForeignLinerName { get; set; }
    }
    public class ShippingLine
    {
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
    }
    public class PortOfDischarge
    {
        public int PODId { get; set; }
        public string PODName { get; set; }
    }
    public class CHA
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }

 
    }
    public class Importer
    {
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }
    }
    public class Commodity
    {
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
    }
}

