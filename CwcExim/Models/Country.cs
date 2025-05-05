using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    public class Country
    {
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [MaxLength(50,ErrorMessage = "Country Name Can Be No More Than 50 Characters In Length Including Spaces")]
        [Display(Name ="Country Name:")]
        [RegularExpression(@"^[a-zA-Z ]+$",ErrorMessage ="Country Name Should Contain Only Alphabets")]
        public string CountryName { get; set;}
        [Required(ErrorMessage ="Fill Out This Field")]
        [MaxLength(5,ErrorMessage = "Country Alias Can Be No More Than 5 Characters In Length Including Spaces")]
        [Display(Name ="Country Alias:")]
        [RegularExpression(@"^[a-zA-Z ]+$",ErrorMessage ="Country Alias Should Contain Only Alphabets")]
        public string CountryAlias { get; set; }
        public int Uid { get; set; }
        public int BranchId { get; set; }
    }
}