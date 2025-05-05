using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class VIZ_InvContainerStuffing
    {
        public int UId { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public int StuffingId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string StuffingNo { get; set; }
        public string StuffingDate { get; set; }
        public List<VIZ_InvContainerStuffingDtl> LstStuffingDtl = new List<VIZ_InvContainerStuffingDtl>();
        public string SEZ { get; set; }
        public string ShippingBillDtlXml { get; set; }
    }
    public class VIZ_InvContainerStuffingDtl
    {        
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public int ExporterId { get; set; }
        public string ExporterName { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public decimal StuffingWeight { get; set; }
        public decimal StuffingQuantity { get; set; }
        public decimal SQM { get; set; }
        public decimal CBM { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public int PayerId { get; set; }
        public string PayerName { get; set; }
    }
}