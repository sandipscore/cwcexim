using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Master.Models
{
    public class CHNYardVM
    {
        private CHNYard _Yard;
        private CHNYard Yrd { get { return new CHNYard(); } }
        public CHNYardVM()
        {
            _Yard = Yrd;
        }
        public CHNYard MstYard { get { return _Yard; } set { _Yard = value; } }
        public IList<CHNYardWiseLocation> LstYard { get; set; } = new List<CHNYardWiseLocation>();
        public string LocationDetail { get; set; }
        public string DelLocationDetail { get; set; }
    }
}