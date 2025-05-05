using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Master.Models
{
    public class HDBGodownVM
    {
        private HDBGodown _Godown;
        private HDBGodown Gdwn{ get { return new HDBGodown(); } }
        public HDBGodownVM()
        {
            _Godown = Gdwn;
        }
        public HDBGodown MstGodwon { get { return _Godown; } set { _Godown = value; } }
        public IList<HDBGodownWiseLocation> LstLocation { get; set; } = new List<HDBGodownWiseLocation>();
        public string LocationDetail { get; set; }
       
        public string DelLocationDetail { get; set; }

    }
}