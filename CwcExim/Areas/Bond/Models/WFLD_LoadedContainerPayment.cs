using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Areas.Import.Models;
namespace CwcExim.Areas.Bond.Models
{
    public class WFLD_LoadedContainerPayment : WFLD_PostPaymentSheet
    {
        public string ExportUnder { get; set; } = string.Empty;
        public string LEODate { get; set; }
        public int POD { get; set; } = 0;
    }
}