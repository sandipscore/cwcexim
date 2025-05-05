using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Dnd_FormOneModel
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
        public IList<Dnd_FormOneDetailModel> lstFormOneDetail { get; set; } = new List<Dnd_FormOneDetailModel>();
        public IList<Dnd_ShippingLine> lstShippingLine { get; set; } = new List<Dnd_ShippingLine>();
        public IList<Dnd_ForeignLiner> lstForeignLiner { get; set; } = new List<Dnd_ForeignLiner>();
        public IList<Dnd_PortOfDischarge> lstPOD { get; set; } = new List<Dnd_PortOfDischarge>();
        public IList<Dnd_CHA> lstCHA { get; set; } = new List<Dnd_CHA>();
        public IList<Dnd_Importer> lstImporter { get; set; } = new List<Dnd_Importer>();
        public IList<Dnd_Commodity> lstCommodity { get; set; } = new List<Dnd_Commodity>();

        [Required(ErrorMessage = "Container Details Cannot Be Left Blank")]
        public string FormOneDetailsJS { get; set; }

        public string TrBondNo { get; set; }

    }


    public class Dnd_ForeignLiner
    {
       
        public string ForeignLinerName { get; set; }
    }
    public class Dnd_ShippingLine
    {
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
    }
    public class Dnd_PortOfDischarge
    {
        public int PODId { get; set; }
        public string PODName { get; set; }
    }
    public class Dnd_CHA
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }

 
    }
    public class Dnd_Importer
    {
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }
    }
    public class Dnd_Commodity
    {
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
    }
}

