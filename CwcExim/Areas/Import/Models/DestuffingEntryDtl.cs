using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class DestuffingEntryDtl
    {
        public int DestuffingEntryDtlId { get; set; }
        public int DestuffingEntryId { get; set; }
        public string LineNo { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string BOLNo { get; set; }
        public string BOLDate { get; set; }
        public string MarksNo { get; set; }
        public int CHAId { get; set; }
        public string CHA { get; set; }
        public int ImporterId { get; set; }
        public string Importer { get; set; }
        public string CargoDescription { get; set; }
        public int CargoType { get; set; }
        public int CommodityId { get; set; }
        public string Commodity { get; set; }
        public int NoOfPackages { get; set; }
        public decimal CUM { get; set; }
        public decimal SQM { get; set; }
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal DestuffingWeight { get; set; }
        public decimal CIFValue { get; set; }
        public decimal Duty { get; set; }
        public int AppraisementStatus { get; set; }
        public string GodownWiseLocationIds { get; set; }
        public string GodownWiseLctnNames { get; set; }
    }
}