using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class VRN_GodownVM
    {
        private VRN_Godown _Godown;
        private VRN_Godown Gdwn { get { return new VRN_Godown(); } }
        public VRN_GodownVM()
        {
            _Godown = Gdwn;
        }
        public VRN_Godown MstGodwon { get { return _Godown; } set { _Godown = value; } }
        public IList<VRN_GodownWiseLocation> LstLocation { get; set; } = new List<VRN_GodownWiseLocation>();
        public string LocationDetail { get; set; }

        public string DelLocationDetail { get; set; }
    }
}