using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Master.Models
{
    public class PPGGodownVM
    {
        private PPGGodown _Godown;
        private PPGGodown Gdwn { get { return new PPGGodown(); } }
        public PPGGodownVM()
        {
            _Godown = Gdwn;
        }
        public PPGGodown MstGodwon { get { return _Godown; } set { _Godown = value; } }
        public IList<PPGGodownWiseLocation> LstLocation { get; set; } = new List<PPGGodownWiseLocation>();
        public string LocationDetail { get; set; }

        public string DelLocationDetail { get; set; }
    }
}