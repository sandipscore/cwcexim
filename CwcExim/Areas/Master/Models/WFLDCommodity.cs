using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class WFLDCommodity
    {
        public int CommodityId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Commodity")]
        [MaxLength(30, ErrorMessage = "Commodity Name Can Not Be No More Than 30 Characters In Length Including Spaces")]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Commodity Name can contain only alphabets and numbers")]
        public string CommodityName { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Alias")]
        [MaxLength(10, ErrorMessage = "Commodity Alias Can Not Be More Than 10 Characters In Length Including Spaces")]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Commodity Alias Can Contain Only Alphabets And Numbers")]
        public string CommodityAlias { get; set; }

        [Display(Name = "Tax Exempted")]
        public bool TaxExempted { get; set; }
        [Display(Name = "Fumigation Chemical")]
        public bool FumigationChemical { get; set; }
        public int Uid { get; set; }
        public int BranchId { get; set; }
        [Display(Name = "Commodity Type")]
        public int? CommodityType { get; set; }

        public bool ReadymadeGarments { get; set; }

    }
}