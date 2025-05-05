using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Master.Models
{
    public class DSRYardVM
    {
        private DSRYard _Yard;
        private DSRYard Yrd { get { return new DSRYard(); } }
        public DSRYardVM()
        {
            _Yard = Yrd;
        }
        public DSRYard MstYard { get { return _Yard; } set { _Yard = value; } }
        public IList<DSRYardWiseLocation> LstYard { get; set; } = new List<DSRYardWiseLocation>();
        public string LocationDetail { get; set; }
        public string DelLocationDetail { get; set; }
    }
}