using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CwcExim.Models;

namespace CwcExim.Models
{
    public class AccessRightsVM
    {
        public AccessRightsVM()
        {
            LstPartyType = new List<PartyType>();
            LstPartyType.Add(new PartyType { Ptype = 1, PtypeName = "Licensee" });
            LstPartyType.Add(new PartyType { Ptype = 2, PtypeName = "Non Licensee" });

        }


        [Required(ErrorMessage = " ")]
        public int RoleId { get; set; }

        [Display(Name = "Role")]
        public string RoleName { get; set; }
        public IEnumerable<RoleMaster> RoleList { get; set; }

        [Required(ErrorMessage = " ")]
        public int ModuleId { get; set; }

        [Display(Name = "Module")]
        public string ModuleName { get; set; }
        public IEnumerable<Module> ModuleList { get; set; }
        public int Ptype { get; set; }
        public string PtypeName { get; set; }
        public IList<PartyType> LstPartyType { get; set; }
        public List<AccessRights> lstAccessRights { get; set; }


    }
    public class PartyType
    {
        public int Ptype { get; set; }
        public string PtypeName { get; set; }
    }

}