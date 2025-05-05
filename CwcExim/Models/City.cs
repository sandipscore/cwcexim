using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    public class City
    {
        public int CityId { get; set; }

        [Required(ErrorMessage ="Fill Out This Field")]
        [StringLength(30,ErrorMessage = "City Name Can Be No More Than 30 Characters In Length Including Cpaces")]
        [RegularExpression(@"^[a-zA-Z ]+$",ErrorMessage = "City Name Should Contain Only Alphabets")]
        [Display(Name = "City Name")]
        public string CityName { get; set; }

        [Required(ErrorMessage ="Fill Out This Field")]
        [Display(Name = "City Alias")]
        [StringLength(5, ErrorMessage = "City Alias Can Be No More Than 5 Characters In Length Including Spaces")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "City Alias Should Contain Only Alphabets")]
        public string CityAlias { get; set; }

        [Required(ErrorMessage ="Fill Out This Field")]
        [Display(Name = "Country Name")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "State Name")]
        public int StateId { get; set; }
        public int Uid { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }

        public IList<Country> LstCountry = new List<Country>();
    }
}