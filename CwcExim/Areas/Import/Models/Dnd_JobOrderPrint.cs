using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Dnd_JobOrderPrint
    {
       
            public string mstcompany { get; set; }
            public List<Dnd_TotalJobOrder> LstTotal { get; set; } = new List<Dnd_TotalJobOrder>();


        }
        public class Dnd_TotalJobOrder
    {
       public string FormOneNo { get; set; }
        public string FormOneDate { get; set; } 
         public string TransportBy { get; set; } 
           public string CONTCBT { get; set; } 
           public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public string LCLFCL { get; set; }
        public string ShippingLineName { get; set; }
        public string ForeignLiner { get; set; }
        public string VesselName { get; set; }
        public string ViaNo { get; set; }
        public string Remarks { get; set; }
    }
    
}
