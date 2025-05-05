using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Import.Models
{
    public class DSRCustomAppraisement
    {
        public string OBLNo { get; set; }
        [Required(ErrorMessage ="Fill Out This Field")]
        public string OBLDate { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }

        public List<DSRCustomAppraisementDtl> LstAppraisementWFLD = new List<DSRCustomAppraisementDtl>();
        public List<DSRCustomAppraisementOrdDtl> LstCustomAppraisementOrdDtl { get; set; } = new List<DSRCustomAppraisementOrdDtl>();
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
        public List<DSRCustomAppraisementOBLCont> WFLDCustomAppraisementCont { get; set; } = new List<DSRCustomAppraisementOBLCont>();

        public int CustomAppraisementId { get; set; }
        public string ContainerNo { get; set; }
        public string AppraisementNo { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string AppraisementDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ShippingLine { get; set; }
        public int ShippingLineId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHAName { get; set; }
        public int CHAId { get; set; }

        //  [Required(ErrorMessage = "Fill Out This Field")]
        //  [StringLength(45,ErrorMessage = "Rotation Cannot Be More Than 45 Characters")]
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
        public int ODCType { get; set; }
        public int InterShifting { get; set; }
        public int LiftOn { get; set; }
        public int LiftOff { get; set; }
        public int Reworking { get; set; }
        public int Weighment { get; set; }
        public string CargoDeliveryType { get; set; }
        [Range(0, 99999999.99, ErrorMessage = "Manual Weight Cannot Be More Than 99999999.99")]
        public decimal ManualWT { get; set; }
        [Range(0, 99999999.99, ErrorMessage = "Mechanical Weight Cannot Be More Than 99999999.99")]
        public decimal MechanicalWT { get; set; }

        public List<DSRCustomAppraisementDtl> LstAppraisement = new List<DSRCustomAppraisementDtl>();
        public int operationType { get; set; }
        public string ForeignLine { get; set; }

        public string WithoutDOSealNo { get; set; }

        public int? ContainerType { get; set; }

        public int CNoOfPackages { get; set; }
        public decimal CGrossWeight { get; set; }

    }
    public class DSRCustomAppraisementOrdDtl
    {
        public int OrderId { get; set; }
        public int CustomAppraisementId { get; set; } = 0;
        public string IssuedBy { get; set; }
        public string DeliveredTo { get; set; }

        public string ValidType { get; set; }
        public string ValidDate { get; set; }

    }

    public class DSRCustomAppraisementOBLCont
    {
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public int NoOfPackages { get; set; }
        public decimal GrossWeight { get; set; }
        public string OBLNo { get; set; }
        public string OBLDate { get; set; }

        public int ImporterId { get; set; }
        public string Importer { get; set; }
        public string CargoDescription { get; set; }
        public decimal CIFValue { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLine { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; }
    }


    public class DSRCustomAppraisementBOECont
    {
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public decimal CIFValue { get; set; }
        public decimal Duty { get; set; }
    }

   
}