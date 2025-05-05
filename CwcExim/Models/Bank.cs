using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    public class Bank
    {
        public int BankId { get; set; }

        [Display(Name ="Bank")]
        [Required(ErrorMessage ="Fill Out This Field")]
        [StringLength(100,ErrorMessage = "Bank Name Cannot Be More Than 100 Characters")]
        [RegularExpression(@"^[a-zA-Z ]*$",ErrorMessage = "Bank Name Can Contain Only Alphabets")]
        public string BankName { get; set; }

        [Display(Name ="A/C No")]
        [Required(ErrorMessage ="Fill Out This Field")]
        [StringLength(18,MinimumLength =6,ErrorMessage = "Account No must be in 6 to 18 Characters")]
        [RegularExpression(@"^[0-9]*$",ErrorMessage = "Account No Can Contain Only Numeric Digits")]
        public string AccountNo { get; set; }

        [Display(Name = "Address")]
        [StringLength(100,ErrorMessage = "Address Cannot Be More Than 100 Characters")]
        [RegularExpression("^[a-zA-Z0-9.\\-/,();\\r\\n: ]*$", ErrorMessage = "Address Can Contain Only Alphabets, Numeric Digits And Special Characters '.,/-();'")]
       // [RegularExpression("^[0-9a-zA-Z.\\-,/;//r//n ]+$",ErrorMessage ="Address Can Contain Alphabets,Numeric Digits And Special Characters Like'.,;/-'")]
        public string Address { get; set; }

        [Display(Name = "IFSC Code")]
        [StringLength(11,ErrorMessage = "IFSC Code Cannot Be More Than 11 Characters")]
        [RegularExpression(@"^[A-Z0-9]+$",ErrorMessage = "IFSC Code Can Contain Alphabets And Numeric Digits")]
        public string IFSC { get; set; }

        [Display(Name = "Branch")]
        [Required(ErrorMessage = "Fill Out This Field")]
        [StringLength(30,ErrorMessage = "Branch Cannot Be More Than 30 Characters")]
        [RegularExpression(@"^[a-zA-Z ]*$",ErrorMessage = "Branch Can Contain Only Alphabets")]
        public string Branch { get; set; }

        [Display(Name = "Mobile No")]
        [Required(ErrorMessage ="Fill Out This Field")]
        [StringLength(10,MinimumLength =10,ErrorMessage = "Mobile No Should Of 10 Characters Only")]
        public string MobileNo { get; set; }

        [Display(Name = "Fax No")]
        [RegularExpression(@"^[0-9]{1,3}[0-9]{3}[0-9]{7}",ErrorMessage ="Invalid Fax No")]
        public string FaxNo { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage ="Fill Out This Field")]
        [EmailAddress(ErrorMessage ="Invalid Email")]
        [StringLength(50,ErrorMessage = "Email Cannot Be More than 50 Characters")]
        public string Email { get; set; }
        public int Uid { get; set; }
    }
}