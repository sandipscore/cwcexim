using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class ExportDestuffing
    {
        public int ContainerStuffingId { get; set; }
        public string CFSCode { get; set; }
        public int ExportDestuffingId { get; set; }
        public string ExportDestuffingNo { get; set; }
        public string ExportDestuffingDate { get; set; }

        public List<ExportDestuffingCharges> lstCharges { get; set; }

        public decimal Total { get; set; }
    }
    public class ExportDestuffingCharges
    {
        public int PartyId { get; set; }        
        public String PartyName { get; set; }
        public int OperationId { get; set; }
        public String ChargeType { get; set; }
        public String ChargeName { get; set; }
        public decimal Charge { get; set; }
        public decimal CGSTCharge { get; set; }
        public decimal SGSTCharge { get; set; }
        public decimal IGSTCharge { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal SGSTPer { get; set; }
        public decimal Amount { get; set; }
        public decimal Taxable { get; set; }
        public String SACCode { get; set; }
        public decimal TotalAmount { get; set; }
    }
}