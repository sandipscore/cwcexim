using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class YardVM
    {
        private Yard _Yard;
        private Yard Yrd { get { return new Yard(); } }
        public YardVM()
        {
            _Yard=Yrd;
        }
        public Yard MstYard { get { return _Yard; } set { _Yard = value; } }
        public IList<YardWiseLocation> LstYard { get; set; } = new List<YardWiseLocation>();
        public string LocationDetail { get; set; }
        public string DelLocationDetail { get; set; }
    }
}