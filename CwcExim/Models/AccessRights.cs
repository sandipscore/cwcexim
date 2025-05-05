using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    public class AccessRights
    {
        [Display(Name = "Sl No.")]
        public int SrlNo { get; set; }
        public int MenuId { get; set; }

        [Display(Name ="Menu")]
        public string MenuName { get; set; }

        [Display(Name = "View and Add Rights")]
        public bool CanAdd { get; set; }

        [Display(Name = "View Rights")]
        public bool CanView { get; set; }
        [Display(Name = "Edit Rights")]
        public bool CanEdit { get; set; }

        [Display(Name = "Delete Rights")]
        public bool CanDelete { get; set; }

    }
}