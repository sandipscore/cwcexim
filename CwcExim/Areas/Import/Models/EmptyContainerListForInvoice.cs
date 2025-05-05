using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class EmptyContainerListForInvoice
    {
        //ShippingLineId, ShippingLineName, GSTNo, CFSCode, ContainerNo, EmptyDate, Address, StateCode, StateName
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public string GSTNo { get; set; }
        public int EntryId { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string EmptyDate { get; set; }
        public string Address { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string ContainerInDate { get; set; }

        public string Size { get; set; }
    }
}