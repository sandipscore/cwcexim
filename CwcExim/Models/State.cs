using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    public class State
    {
        public int StateId { get; set; }

        [Required(ErrorMessage ="Fill Out This Field")]
        [Display(Name ="Country")]
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        [Required(ErrorMessage ="Fill Out This Field")]
        [Display(Name = "State Name")]
        [RegularExpression(@"^[a-zA-Z ]+$",ErrorMessage = "State Name Can Contain Only Alphabets")]
        [MaxLength(30,ErrorMessage = "State Name Can Be No More Than 30 Characters In Length Including Spaces")]
        public string StateName { get; set; }

        [Required(ErrorMessage ="Fill Out This Field")]
        [Display(Name ="State Alias")]
        [MaxLength(5,ErrorMessage = "State Alias Can Be No More Than 5 Characters In Length Including Spaces")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "State Alias Can Contain Only Alphabets")]
        public string StateAlias { get; set; }
        public int Uid { get; set; }
        public int BranchId { get; set; }
        public IList<Country> LstCountry = new List<Country>();

        public string GstStateCode { get; set; }
        
    }
}