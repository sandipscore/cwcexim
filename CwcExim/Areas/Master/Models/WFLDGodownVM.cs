using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class WFLDGodownVM
    {
        private WFLDGodown _Godown;
        private WFLDGodown Gdwn { get { return new WFLDGodown(); } }
        public WFLDGodownVM()
        {
            _Godown = Gdwn;
        }
        public WFLDGodown MstGodwon { get { return _Godown; } set { _Godown = value; } }
        public IList<WFLDGodownWiseLocation> LstLocation { get; set; } = new List<WFLDGodownWiseLocation>();
        public string LocationDetail { get; set; }

        public string DelLocationDetail { get; set; }
    }
}