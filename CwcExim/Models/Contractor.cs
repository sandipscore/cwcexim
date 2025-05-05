using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    public class Contractor
    {
        public int ContractorId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Contractor Name")]
        [StringLength(30,ErrorMessage = "Contractor Name Cannot Be More Than 30 Characters")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Contractor Name Can Contain Alphabets Only")]
        public string ContractorName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Contractor Alias Can Contain Alphabets And Numeric Digits Only")]
        [StringLength(5,ErrorMessage = "Contractor Alias  Cannot Be More Than 5 Characters")]
        public string ContractorAlias { get; set; }

        [Display(Name = "Address")]
        [StringLength(50, ErrorMessage = "Address Should Be Of 50 Characters Only")]
        [RegularExpression("^[a-zA-Z0-9.\\-/,();\\r\\n: ]*$", ErrorMessage = "Address Can Contain Only Alphabets, Numeric Digits And Special Characters '.,/-();'")]
        public string Address { get; set; }

        [Display(Name = "Country")]
        public int? CountryId { get; set; }

        [Display(Name = "State")]
        public int? StateId { get; set; }

        [Display(Name = "City")]
        public int? CityId { get; set; }

        [Display(Name = "Pincode")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Pincode Should Be Of Numeric Digits")]
        public int? PinCode { get; set; }

        [Display(Name = "Phone No")]
        [StringLength(11,MinimumLength =8,ErrorMessage = "Phone No Should Be Within 8-10 Characters")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Phone No Should Be Of Numeric Digits")]
        public string PhoneNo { get; set; }

        // +895(194)6165163
        // +09(155)2541778
        [Display(Name = "Fax No")]
        [RegularExpression(@"^[0-9]{1,3}[0-9]{3}[0-9]{7}$", ErrorMessage = "Invalid Fax No")]
        //[RegularExpression(@"\+[0-9]{1,3}\([0-9]{3}\)[0-9]{7}",ErrorMessage ="Invalid Fax No")]
        //[RegularExpression(@"\[0-9]{1,3}\([0-9]{3}\)[0-9]{7}", ErrorMessage = "Invalid Fax No")]
        public string FaxNo { get; set; }

        [Display(Name = "Email Id")]
        [Required(ErrorMessage = "Fill Out This Field")]
        [EmailAddress(ErrorMessage ="Invalid Email Id")]
        [StringLength(50, ErrorMessage = "Email Id Cannot Be More Than 50 Characters")]
        public string Email { get; set; }


        [Display(Name = "Contact Person")]
        [StringLength(30,ErrorMessage = "Contact Person Cannot Be More Than 30 Characters")]
        [RegularExpression(@"^[a-zA-Z ]+$",ErrorMessage = "Contact Person Can Contain Only Alphabets")]
        public string ContactPerson { get; set; }

        [Display(Name = "Mobile No")]
        [StringLength(10,MinimumLength =10,ErrorMessage = "Mobile No Must Be Of 10 Digits")]
        [RegularExpression(@"^[0-9]+$",ErrorMessage = "Mobile No Should Be Of Numeric Digits")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "PAN")]
        [RegularExpression(@"[A-Z]{5}\d{4}[A-Z]{1}",ErrorMessage ="Invalid PAN")]
        public string Pan { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Aadhaar No")]
        [RegularExpression(@"^\d{4}\d{4}\d{4}$", ErrorMessage = "Invalid Aadhaar No")]
        //[RegularExpression(@"^\d{4}\s\d{4}\s\d{4}$",ErrorMessage ="Invalid Aadhaar No")]
        public string AadhaarNo { get; set; }

        // eg.11ABCDE1234L1Z1 digits
        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "GST No")]
        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", ErrorMessage = "Invalid GST No")]
        //[RegularExpression(@"[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$",ErrorMessage ="Invalid GSTNo")]
        public string GSTNo { get; set; }
        public int Uid { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
    }
}