using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class DSRPrintJOModel
    {
        public string ContainerType { get; set; }
        public string JobOrderNo { get; set; }
        public string JobOrderDate { get; set; }
        public string ShippingLineName { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string TrainNo { get; set; }
        public string TrainDate { get; set; }
        public IList<DSRPrintJOModelDet> lstDet { get; set; } = new List<DSRPrintJOModelDet>();
    }
    public class DSRPrintJOModelDet
    {
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public string Sline { get; set; }
        public string ChaName { get; set; }
        public string ImporterName { get; set; }
        public string CargoType { get; set; }
        public string ContainerLoadType { get; set; }

    }
}