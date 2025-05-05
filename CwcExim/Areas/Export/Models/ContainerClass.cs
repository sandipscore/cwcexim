using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class ContainerClass
    {
       public int containerlassid { get; set; }
        public string CntainerClass { get; set;}
        public class OperationType
        {
            public int oprtntypId { get; set; }
            public string OpertnType { get; set; }
        }
        public class ContainerType
        {
            public int cnttypeId { get; set; }
            public string ContType { get; set; }
        }
    
    
    }
}