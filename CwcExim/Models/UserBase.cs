using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    public abstract class UserBase
    {
        [Display(Name = "User ID")]
        public int Uid { get; set; }
       // [Required(ErrorMessage ="*")]
        public string LoginId { get; set; }

        [Display(Name ="Name")]
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
        public IEnumerable<DesignationMaster> DesignationList { get; set; }

        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }

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
        public IEnumerable<District> DistrictList { get; set; }

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

        [Display(Name ="Status")]
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