using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Master.Models
{
    public class WFLDYardVM
    {
        private WFLDYard _Yard;
        private WFLDYard Yrd { get { return new WFLDYard(); } }
        public WFLDYardVM()
        {
            _Yard = Yrd;
        }
        public WFLDYard MstYard { get { return _Yard; } set { _Yard = value; } }
        public IList<WFLDYardWiseLocation> LstYard { get; set; } = new List<WFLDYardWiseLocation>();
        public string LocationDetail { get; set; }
        public string DelLocationDetail { get; set; }
    }
}