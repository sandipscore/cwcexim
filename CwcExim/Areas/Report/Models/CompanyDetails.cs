using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class CompanyDetailsForReport
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }

        public string RoAddress { get; set; }

        public string EmailAddress { get; set; }

        public decimal EffectVersion { get; set; }

        public string VersionLogoFile { get; set; }
        public string BranchName { get; set; }

    }
}