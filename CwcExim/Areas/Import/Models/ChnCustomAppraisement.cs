using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class ChnCustomAppraisement
    {
        public string OBLNo { get; set; }

        public List<ChnCustomAppraisementDtl> LstAppraisement = new List<ChnCustomAppraisementDtl>();
        public List<ChnCustomAppraisementOrdDtl> LstCustomAppraisementOrdDtl { get; set; } = new List<ChnCustomAppraisementOrdDtl>();
        [MaxLength(45, ErrorMessage = "Rotation Cannot Be More Than 45 Characters")]
        public string Vessel { get; set; }
        [MaxLength(45, ErrorMessage = "Rotation Cannot Be More Than 45 Characters")]
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

        public List<ChnCustomAppraisementOBLCont> lstCustomAppraisementCont { get; set; } = new List<ChnCustomAppraisementOBLCont>();
        public int CustomAppraisementId { get; set; }
        public string ContainerNo { get; set; }
        public string AppraisementNo { get; set; }
        public string AppraisementDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ShippingLine { get; set; }
        public int ShippingLineId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHAName { get; set; }
        public int CHAId { get; set; }

        //  [Required(ErrorMessage = "Fill Out This Field")]
        [StringLength(45,ErrorMessage = "Rotation Cannot Be More Than 45 Characters")]
        public virtual string Rotation { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999999.99, ErrorMessage = "FOB Cannot Be More Than 99999999.99")]
        public decimal Fob { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999999.99, ErrorMessage = "Gross Duty Cannot Be More Than 99999999.99")]
        public decimal GrossDuty { get; set; }
        public int DeliveryType { get; set; }
        public int IsDO { get; set; }
        public string CustomAppraisementXML { get; set; }
        public int Uid { get; set; }
    }
    public class ChnCustomAppraisementOrdDtl
    {
        public int OrderId { get; set; }
        public int CustomAppraisementId { get; set; } = 0;
        public string IssuedBy { get; set; }
        public string DeliveredTo { get; set; }

        public string ValidType { get; set; }
        public string ValidDate { get; set; }

    }

    public class ChnCustomAppraisementOBLCont
    {
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public int NoOfPackages { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal CIFValue { get; set; }
        public string OBLNo { get; set; }
        public string OBLDate { get; set; }

        public int ImporterId { get; set; }
        public string Importer { get; set; }
        public string CargoDescription { get; set; }

        public string ShippingLine { get; set; }
        public int ShippingLineId { get; set; }
    }


    public class ChnCustomAppraisementBOECont
    {
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public decimal CIFValue { get; set; }
        public decimal Duty { get; set; }
    }
}
