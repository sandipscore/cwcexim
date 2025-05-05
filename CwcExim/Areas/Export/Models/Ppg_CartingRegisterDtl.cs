using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Ppg_CartingRegisterDtl:CartingRegisterDtl
    {
        public decimal FoBValue1 { get; set; } = 0; //For Storing FOB Value from CCIN;
    }
}