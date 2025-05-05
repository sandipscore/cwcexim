using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    public class GodownRight
    {
        public int UserId { get; set; }
        public static  int UId {get;set;}
        public int UserName { get; set; }
        public int RoleId { get; set; }
        public IEnumerable<User> lstUser { get; set; }

        public List<GodownRights> lstGodown { get; set; }
    }

    public class GodownRights
    {
        [Display(Name = "Sl No.")]
        public int SrlNo { get; set; }
        public int GodownId { get; set; }

        [Display(Name = "Godown Name")]
        public string GodownName { get; set; }

        [Display(Name = "View and Add Rights")]
        public bool IsAllowed { get; set; }
        public string Godown { get; set; }//Checking of Rights for godown/yard

    }
}