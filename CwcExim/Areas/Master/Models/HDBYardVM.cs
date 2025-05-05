using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Master.Models
{
    public class HDBYardVM
    {
      
            private HDBYard _Yard;
            private HDBYard Yrd { get { return new HDBYard(); } }
            public HDBYardVM()
            {
                _Yard = Yrd;
            }
            public HDBYard MstYard { get { return _Yard; } set { _Yard = value; } }
            public IList<HDBYardWiseLocation> LstYard { get; set; } = new List<HDBYardWiseLocation>();
            public string LocationDetail { get; set; }
            public string DelLocationDetail { get; set; }
        
    }
}