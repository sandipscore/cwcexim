using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Dnd_ContainerList
    {
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }

        public int Size { get; set; }
        public string ShippingLine { get; set; }
        public int ShippingLineId { get; set; }
        public string ContainerClass { get; set; }
        public int CargoType { get; set; }
        public int ExportType { get; set; }

    }
}