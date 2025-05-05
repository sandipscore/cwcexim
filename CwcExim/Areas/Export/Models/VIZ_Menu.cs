using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class VIZ_Menu
    {
        public int CanAdd { get; set; }
        public int CanEdit { get; set; }
        public int CanDelete { get; set; }
        public int CanView { get; set; }
    }
}