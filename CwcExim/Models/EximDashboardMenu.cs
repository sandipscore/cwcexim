using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class EximDashboardMenu
    {
        //public int ModuleId { get; set; }
        //public string ModuleName { get; set; }
        //public int MenuId { get; set; }
        //public string MenuName { get; set; }
        //public int ParentMenuId { get; set; }
        //public string ActionUrl { get; set; }

        public List<EximModule> lstEModule { get; set; }
        public List<EximMenu> lstEMenu { get; set; }
    }

    public class EximModule
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
    }
    public class EximMenu
    {
        public int ModuleId { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int ParentMenuId { get; set; }
        public string ActionUrl { get; set; }
    }
    
}