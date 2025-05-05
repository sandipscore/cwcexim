using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class DSRGodownVM
    {
        private DSRGodown _Godown;
        private DSRGodown Gdwn { get { return new DSRGodown(); } }
        public DSRGodownVM()
        {
            _Godown = Gdwn;
        }
        public DSRGodown MstGodwon { get { return _Godown; } set { _Godown = value; } }
        public IList<DSRGodownWiseLocation> LstLocation { get; set; } = new List<DSRGodownWiseLocation>();
        public string LocationDetail { get; set; }

        public string DelLocationDetail { get; set; }

        public bool IsImport { get; set; }
        public bool IsExport { get; set; }
        public bool IsBond { get; set; }
        public bool IsOpen { get; set; }
    }
}