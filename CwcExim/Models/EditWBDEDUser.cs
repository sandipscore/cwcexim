using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    [MetadataType(typeof(EditWBDEDUserMD))]
    public class EditWBDEDUser : UserBase
    {

    }
    public class SecondaryUser
    {
        [Display(Name = "User ID")]
        public int Uid { get; set; }
        // [Required(ErrorMessage ="*")]

        [Required(ErrorMessage = "Login Id is Required")]
        public string LoginId { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }
        public string HdnPassword { get; set; }

        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        public int? RoleId { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        public IEnumerable<RoleMaster> RoleList { get; set; }
        public int? DesignationId { get; set; }

        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Mobile No is Required")]
        [Display(Name = "Mobile No")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile No Must Be Of 10 Digits")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Mobile No Should Be Of Numeric Digits ")]
        public string MobileNo { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        [Required(ErrorMessage = "Email is Required")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "PAN No")]
        public string PanNo { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Pin Code")]
        public string PinCode { get; set; }
        public int DistrictId { get; set; }

        [Display(Name = "District")]
        public string DistrictName { get; set; }
        

        [Display(Name = "License No")]
        public string LicenseNo { get; set; }
        public bool EmailVerified { get; set; }
        public bool MobileVerified { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public string UpdatedOn { get; set; }

        [Display(Name = "Status")]
        public bool IsBlocked { get; set; }
        public bool Locked { get; set; }
        public int AttemptCount { get; set; }
        public bool IsSelected { get; set; }
        public int PartyType { get; set; }
        public bool Importer { get; set; }
        public bool Exporter { get; set; }
        public bool ShippingLine { get; set; }
        public bool CHA { get; set; }
        public int EximTraderId { get; set; }
    }
}