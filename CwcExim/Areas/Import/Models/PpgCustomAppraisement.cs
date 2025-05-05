using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Import.Models
{
    public class PpgCustomAppraisement:CustomAppraisement
    {
        public string OBLNo { get; set; }

        public List<PpgCustomAppraisementDtl> LstAppraisementPpg = new List<PpgCustomAppraisementDtl>();
        public List<PPGCustomAppraisementOrdDtl> LstCustomAppraisementOrdDtl { get; set; } = new List<PPGCustomAppraisementOrdDtl>();
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string CFSCode { get; set; }
        public string Size { get; set; }
        public string ContainerLoadType { get; set; }
        public string CargoDescription { get; set; }
        public int NoOfPackages { get; set; }
        public decimal GrossWeight { get; set; }
        public int CargoType { get; set; }
        public int ApplicationForApp { get; set; }
        public int ImporterId { get; set; }
        public string Importer { get; set; }
        public string LineNo { get; set; }
        public int RMSValue { get; set; }
        public string GateEntryDate { get; set; }
        public string AppraisementDateCheck { get; set; }
        public string CAOrdDtlXml { get; set; }

        public List<PPGCustomAppraisementOBLCont> PPGCustomAppraisementCont { get; set; } = new List<PPGCustomAppraisementOBLCont>();
    }
    public class PPGCustomAppraisementOrdDtl
    {
        public int OrderId { get; set; }
        public int CustomAppraisementId { get; set; } = 0;
        public string IssuedBy { get; set; }
        public string DeliveredTo { get; set; }

        public string ValidType { get; set; }
        public string ValidDate { get; set; }

    }

    public class PPGCustomAppraisementOBLCont
    {
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public int NoOfPackages { get; set; }
        public decimal GrossWeight { get; set; }
        public string OBLNo { get; set; }
        public string OBLDate { get; set; }
        public string Importer { get; set; }
        public int ImporterId { get; set; }
    }


    public class PPGCustomAppraisementBOECont
    {
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public decimal CIFValue { get; set; }
        public decimal Duty { get; set; }
    }

   
}