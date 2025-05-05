using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class ContainerMstReg
    {
        public string EntryDate { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string ContainerType { get; set; }
        public string ShippingLine { get; set; }
        public string DestuffingDate { get; set; }
        public string DeliveryDate { get; set; }
        public string  ReceivedDate { get; set; }
        public string Stuffingdate{ get; set; }
        public string EExitDate{ get; set; }
        public string OExitDate{ get; set; }
        public string Utilization{ get; set; }
}
}