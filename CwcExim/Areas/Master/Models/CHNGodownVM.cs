using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class CHNGodownVM
    {
        private CHNGodown _Godown;
        private CHNGodown Gdwn { get { return new CHNGodown(); } }
        public CHNGodownVM()
        {
            _Godown = Gdwn;
        }
        public CHNGodown MstGodwon { get { return _Godown; } set { _Godown = value; } }
        public IList<CHNGodownWiseLocation> LstLocation { get; set; } = new List<CHNGodownWiseLocation>();
        public string LocationDetail { get; set; }

        public string DelLocationDetail { get; set; }
    }
}