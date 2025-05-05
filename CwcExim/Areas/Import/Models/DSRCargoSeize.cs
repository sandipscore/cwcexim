using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class DSRCargoSeize
    {
        public int Id { get; set; }
        public int DestuffingEntryDtlId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string OBLNo { get; set; }
        public string OBLDate { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string LineNo { get; set; }
        public string CargoDescription { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public string CFSCode { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; } 
        public int GodownId { get; set; }
        public string GodownNo { get; set; } 
        public int SeizeHoldStatus { get; set; } // 0= "Unhold"; 1="Hold"; 2="Seize";
        public bool IsSeize { get; set; }
        public bool IsHold { get; set; }
        public string Remarks { get; set; }
        public string OparationType { get; set; }

        
    }
}