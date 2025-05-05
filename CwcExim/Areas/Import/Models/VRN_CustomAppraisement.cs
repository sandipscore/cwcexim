using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
   
        public class VRN_CustomAppraisement : CustomAppraisement
        {
            public string OBLNo { get; set; }

            public List<VRN_CustomAppraisementDtl> LstAppraisementPpg = new List<VRN_CustomAppraisementDtl>();
            public List<VRN_CustomAppraisementOrdDtl> LstCustomAppraisementOrdDtl { get; set; } = new List<VRN_CustomAppraisementOrdDtl>();
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
           public bool IsConvert { get; set; }

            public List<VRN_CustomAppraisementOBLCont> CustomAppraisementCont { get; set; } = new List<VRN_CustomAppraisementOBLCont>();
        }
        public class VRN_CustomAppraisementOrdDtl
    {
            public int OrderId { get; set; }
            public int CustomAppraisementId { get; set; } = 0;
            public string IssuedBy { get; set; }
            public string DeliveredTo { get; set; }

            public string ValidType { get; set; }
            public string ValidDate { get; set; }

        }

        public class VRN_CustomAppraisementOBLCont
    {
            public string ContainerNo { get; set; }
            public string CFSCode { get; set; }
            public int NoOfPackages { get; set; }
            public decimal GrossWeight { get; set; }
            public string OBLNo { get; set; }
            public string OBLDate { get; set; }
            public decimal CIFValue { get; set; }
    }

        public class VRN_CustomAppraisementBOECont
    {
            public string ContainerNo { get; set; }
            public string CFSCode { get; set; }
            public string BOENo { get; set; }
            public string BOEDate { get; set; }
            public decimal CIFValue { get; set; }
            public decimal Duty { get; set; }
        }

    }
