using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class GodownVM
    {
        private Godown _Godown;
        private Godown Gdwn{ get { return new Godown(); } }
        public GodownVM()
        {
            _Godown = Gdwn;
        }
        public Godown MstGodwon { get { return _Godown; } set { _Godown = value; } }
        public IList<GodownWiseLocation> LstLocation { get; set; } = new List<GodownWiseLocation>();
        public string LocationDetail { get; set; }
       
        public string DelLocationDetail { get; set; }

    }
}