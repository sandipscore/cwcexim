using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Master.Models
{
    public class DNDBidder
    {
        public int BidderId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Name")]
        [StringLength(150, ErrorMessage = "Bidder Name Cannot Be More Than 150 Characters In Length")]
        [RegularExpression(@"^[a-zA-Z0-9.//\\_@#,&() -]*$", ErrorMessage = "Bidder Name Can Contain Only Alphabets And Numeric Digits")]
        public string BidderName { get; set; }

        [Display(Name = "Bidder Code")]
        [StringLength(50, ErrorMessage = "Bidder Alias Cannot Be More Than 50 Characters In Length")]
        [Required(ErrorMessage = "Fill Out This Field")]
        //[RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Exim Trader Alias Can Contain Only Alphabets And Numeric Digits")]
        [RegularExpression(@"^[a-zA-Z0-9.//\\_@#,&() -]*$", ErrorMessage = "Bidder Alias Can Contain Only Alphabets,Numeric Digits And Special Character")]
        public string BidderAlias { get; set; }


        [Display(Name = "Bidder Code")]
        //[StringLength(maximumLength: 10, MinimumLength = 4, ErrorMessage = "Bidder Code Must Be Minimum 4 Characters Long And Maximum 10 Characters Long.")]
        ////[RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Exim Trader Alias Can Contain Only Alphabets And Numeric Digits")]
        //[RegularExpression(@"^[a-zA-Z0-9.//\\_@#,&() -]*$", ErrorMessage = "Exim Trader Alias Can Contain Only Alphabets,Numeric Digits And Special Character")]
        public string BidderCode { get; set; }

        [Display(Name = "User Id")]
       // [StringLength(maximumLength: 10, MinimumLength = 4, ErrorMessage = "User ID Must Be Minimum 4 Characters Long And Maximum 10 Characters Long.")]
        [RegularExpression("^[a-zA-Z0-9._]*$", ErrorMessage = "User Id Can Contain Only Alphabets, Numeric Digits And Special Characters '.' And '_'")]
        public string UserId { get; set; }

        [Display(Name = "Password")]
        // [RegularExpression(@"^[a-zA-Z0-9\^&#._\\-$,]*$",ErrorMessage = "Password must contain at least : 1 Lowercase character,1 Upper Case character and 1 numeric digit. Special Character is optional and only specified ones are allowed : &amp; # . _ - $")]
        // [StringLength(20,MinimumLength=8,ErrorMessage = " Password should be minimum 8 characters and maximum 20 characters long.")]
        // [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare(otherProperty: "Password", ErrorMessage = "Password And Confirm Password Should Be Same")]
        public string ConfirmPassword { get; set; }
        public string HdnPassword { get; set; }

        //[Display(Name = "Importer")]
        //public bool Importer { get; set; }
        //[Display(Name = "Exporter")]
        //public bool Exporter { get; set; }

        //[Display(Name = "ShippingLine")]
        //public bool ShippingLine { get; set; }

        //[Display(Name = "CHA")]
        //public bool CHA { get; set; }

        //[Display(Name = "Rent")]
        //public bool Rent { get; set; }

        [Display(Name = "Address")]
        [StringLength(250, ErrorMessage = "Address Cannot Be More Than 250 Characters")]
        //  [Required(ErrorMessage = "Fill Out This Field")]
        //  [RegularExpression("^[a-zA-Z0-9./,\\-();\\r\\n: ]*$", ErrorMessage = "Address Can Contain Only Alphabets, Numeric Digits And Special Characters '.,/-();'")]
        public string Address { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int? CountryId { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int? StateId { get; set; }

        [Display(Name = "City")]
        public int? CityId { get; set; }

        [Display(Name = "Pincode")]
        [RegularExpression(@"^[0-9]{6}", ErrorMessage = "Pincode Should Be Of Numeric Digits And 6 Characters")]
        //[RegularExpression(@"^[0-9]+$", ErrorMessage = "Pincode Should Be Of Numeric Digits")]
        public int? PinCode { get; set; }

        [Display(Name = "Phone No")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Phone No Should Be Of Numeric Digits ")]
        [StringLength(10, MinimumLength = 8, ErrorMessage = "Phone No Should Be Within 8-10 Characters")]
        //[StringLength(15,ErrorMessage = "Phone No Cannot Be More Than 15 Characters")]
        public string PhoneNo { get; set; }

        [Display(Name = "Fax No")]
        [RegularExpression(@"^[0-9]{1,3}[0-9]{3}[0-9]{7}$", ErrorMessage = "Invalid Fax No")]

        //[RegularExpression(@"\[0-9]{1,3}\([0-9]{3}\)[0-9]{7}",ErrorMessage = "Invalid Fax No")]
        public string FaxNo { get; set; }

        //[Required(ErrorMessage ="Fill Out This Field")]
        [Display(Name = "Email Id")]
        //[EmailAddress(ErrorMessage ="Invalid Email Id")]
        [StringLength(2000, ErrorMessage = "Email Cannot Be More Than 2000 Characters")]
        public string Email { get; set; }

        [Display(Name = "Contact Person")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Contact Person Can Contain Only Alphabets")]
        [StringLength(30, ErrorMessage = "Contact Person Cannot Be More Than 30 Characters")]
        public string ContactPerson { get; set; }

        [Display(Name = "Mobile No")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile No Must Be Of 10 Digits")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Mobile No Should Be Of Numeric Digits ")]
        public string MobileNo { get; set; }

        [Display(Name = "PAN")]
        // [Required(ErrorMessage ="Fill Out This Field")]
        [RegularExpression(@"[A-Z]{5}\d{4}[A-Z]{1}", ErrorMessage = "Invalid PAN")]
        public string Pan { get; set; }

        [Display(Name = "Aadhaar No")]

        [RegularExpression(@"^\d{4}\d{4}\d{4}$", ErrorMessage = "Invalid Aadhaar No")]
        // [Required(ErrorMessage = "Fill Out This Field")]
        public string AadhaarNo { get; set; }

        [Display(Name = "GST No")]
        // [Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", ErrorMessage = "Invalid GST No")]
        public string GSTNo { get; set; }

        [Display(Name = "TAN")]
        [RegularExpression(@"^[A-Z]{4}[0-9]{5}[A-Z]{1}$", ErrorMessage = "Invalid TAN")]
        public string Tan { get; set; }
        public int Uid { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string Type { get; set; }
        public bool Forwarder { get; set; }

       
    }
}