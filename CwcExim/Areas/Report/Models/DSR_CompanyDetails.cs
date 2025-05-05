using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DSRCompanyDetailsForReport
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string GSTIN { get; set; }
        public decimal EffectVersion { get; set; }

        public string VersionLogoFile { get; set; }

        public string RoAddress { get; set; }



    }
}