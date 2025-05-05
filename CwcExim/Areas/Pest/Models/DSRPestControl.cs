using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Pest.Models
{
    public class DSRPestControl: DSRPestControlModel
    {

      
        public string FumiType { get; set; }
        public string IssueDate { get; set; }
        public string FumigationDate { get; set; }
        public string OutsideContainerXml { get; set; }
        public IList<outsidecontainer> OutsideContDetails { get; set; } = new List<outsidecontainer>();

        public string InsideContainerXml { get; set; }
        public IList<insidecontainer> InsideContDetails { get; set; } = new List<insidecontainer>();
        public string Place { get; set; }
        public string CType { get; set; }
        public string Pkg { get; set; }
        public string InvSplNo { get; set; }
        public string VesselNo { get; set; }
        public string Dosages { get; set; }
        public int PortOfLoadingId { get; set; }
        public string PortOfLoadingName { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ChemicalFumigation { get; set; }
        public string Temperature { get; set; }
        public string CountryId { get; set; }
        public string Exposure { get; set; }
        public string Distance { get; set; }
       public int PortOfDestId { get; set; }
        public string PortOfDestName { get; set; }
        public string GasTight { get; set; }
        public string Pressor { get; set; }
        public string PackageType { get; set; }
        public string Canes { get; set; }
        public string CargoDesc { get; set; }
        public int ExporterId { get; set; }
        public string ExporterName { get; set; }
        public string Consignee { get; set; }
        public string Remarks { get; set; }
        public string AsmType { get; set; }
        public string ExportUnder { get; set; }
        public decimal PestControlCGST { get; set; } = 0;
        public decimal PestControlSGST { get; set; } = 0;
        public decimal PestControlIGST { get; set; } = 0;
        public decimal HandlingAmount { get; set; } = 0;
        public decimal HandlingCGST { get; set; } = 0;
        public decimal HandlingSGST { get; set; } = 0;
        public decimal HandlingIGST { get; set; } = 0;
        public decimal NetAmt { get; set; } = 0;
        public decimal Totaltaxable { get; set; } = 0;
        public decimal IGSTPer { get; set; } = 0;
        public decimal CGSTPer { get; set; } = 0;
        public decimal SGSTPer { get; set; } = 0;
        public string ChemicalInvXML { get; set; }
        public IList<DSRChemicalConsump> ChemicalInvDetails { get; set; } = new List<DSRChemicalConsump>();
        public string SacCode { get; set; }
        public string Pallets { get; set; }

    }


    public class outsidecontainer
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string Amount { get; set; }

    }

    public class insidecontainer
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string Amount { get; set; }
        public string CfsCode { get; set; }

        public string Slacd { get; set; }


    }
}