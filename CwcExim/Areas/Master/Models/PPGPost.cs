using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class PPGPost
    {
        public int PortId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [MaxLength(30, ErrorMessage = "Port Name Cannot Be More Than 30 Characters")]
        [Display(Name = "Port Name")]
        [RegularExpression(@"^[a-zA-Z0-9-/ ]+$", ErrorMessage = "Port Name Can Contain Alphabets,Numeric Digits And Special Characters '-/'")]
        public string PortName { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [MaxLength(5, ErrorMessage = "Port Alias Cannot Be More Than 5 Characters")]
        [Display(Name = "Port Alias")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Port Alias Can Conatin Alphabets And Numeric Digits")]
        public string PortAlias { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }


        [Display(Name = "State")]
        public int StateId { get; set; } = 0;

        [Display(Name = "POD")]
        public bool POD { get; set; }
        public int Uid { get; set; }
        public int BranchId { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }

    }
}