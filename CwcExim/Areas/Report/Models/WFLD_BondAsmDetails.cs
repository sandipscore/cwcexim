using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_BondAsmDetails
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }


        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }

        public string AsmNo { get; set; }

        public string AsmDate { get; set; }

        public string SacNo { get; set; }

        public string SacDate { get; set; }
        public string BondNo { get; set; }

        public string BondDate { get; set; }

        public string AsmType { get; set; }

        public string PartyCode { get; set; }
    }

    public class WFLD_BondBoeQuery
    { 
        public string SacNO { get; set; }
        public string SacDate { get; set; }

        public string BonderName { get; set; }

        public string CHAName { get; set; }

        public string inBondNo { get; set; }

        public string InBondDate { get; set; }

        public string BondedOn { get; set; }

        public string InBondBOENo { get; set; }

        public string InBondBOEDate { get; set; }

        public string CIFValue { get; set; }

        public string NoOFPKG { get; set; }

        public string Weight { get; set; }

        public string Duty { get; set; }

        public string GodownNo { get; set; }
        public string StackNo { get; set; }

        public string HazardousCD { get; set; }
    }
}