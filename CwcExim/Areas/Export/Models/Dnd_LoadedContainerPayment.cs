using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Areas.Import.Models;
namespace CwcExim.Areas.Export.Models
{
    public class Dnd_LoadedContainerPayment: Dnd_PostPaymentSheet
    {
        public string ExportUnder { get; set; } = string.Empty;
        public string LEODate { get; set; }
        public int POD { get; set; } = 0;
        public string Under { get; set; } = string.Empty;
    }
}