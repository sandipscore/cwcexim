using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        [Display(Name = "Company Name")]
        [MaxLength(100,ErrorMessage = "Company Name must be in 100 charcters")]
        [Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression("^[A-Za-z,.& ]*$",ErrorMessage = "Company Name can contain alphabets only")]
        public string CompanyName { get; set; }
        [Display(Name = "Company Short Name")]
        [MaxLength(50,ErrorMessage = "Company Short Name must be in 50 characters")]
        [RegularExpression("^[A-Za-z ]*$", ErrorMessage = "Company Short Name can contain alphabets only")]
        public string CompanyShortName { get; set; }
        [Display(Name = "CompanyAddress")]
        [Required(ErrorMessage = "Fill Out This Field")]
        [MaxLength(500,ErrorMessage = "Company Address must be in 500 characters")]
        [RegularExpression("^[A-Za-z0-9\\r\\n\\- ,./]*$",ErrorMessage = "Company Address can contain alphabets and ' , ',' . ',' / ',' - '")]
        public string CompanyAddress { get; set; }
        [Display(Name = "Phone No")]
        [StringLength(11,MinimumLength =10,ErrorMessage ="Phone No Should Be Be Within 10 To 11 Characters")]
       // [RegularExpression("^[0-9]{10}$",ErrorMessage = "Phone No. can contain 10 digits only")]
        public string PhoneNo { get; set; }

        [Display(Name = "Fax Number")]
        [MaxLength(13,ErrorMessage = "Fax Number must be in 13 characters")]
        [RegularExpression("^[0-9]{1,3}[0-9]{3}[0-9]{7}$", ErrorMessage = "Invalid Fax Number")]
        public string FaxNumber { get; set; }
        [Display(Name = "Email Address")]
        [MaxLength(50,ErrorMessage = "Email Address must be in 50 characters")]
        [Required(ErrorMessage = "Fill Out This Field")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "State")]
        public int? StateId { get; set; }

        [Display(Name = "State Code")]
        [RegularExpression(@"^[0-9]{6}", ErrorMessage = "State Code Should Be Of Numeric Digits And 6 Characters")]
        public string StateCode { get; set; }

        [Display(Name = "City")]
        public int? CityId { get; set; }

        [Display(Name = "GST No")]
        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", ErrorMessage = "Invalid GST No")]
        public string GstIn { get; set; }

        [Display(Name = "PAN")]
        [RegularExpression(@"[A-Z]{5}\d{4}[A-Z]{1}", ErrorMessage = "Invalid PAN")]
        public string Pan { get; set; }
        public int Uid { get; set; }

    
        [Display(Name = "CFS Format")]
        [StringLength(20,ErrorMessage = "CFS Format Cannot Be More Than 20 Characters In Length")]
        public string CFSFormat { get; set; }


        //Add on 21st JAN 2019
        public string LocationUrl { get; set; }

        public string ROAddress { get; set; }
        public string ContactAddress { get; set; }
        public string ContactPhone { get; set; }
        public string BranchType { get; set; }

        public string BranchName { get; set; }

        //Add on 21st JAN 2019


    }
}