using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Destuffing
    {
        public int DestuffingId { get; set; }
        public int ? CustomAppraisementId { get; set; }
        public string ContainerNo { get; set; }
        public string DestuffingNo { get; set; }
        public string DestuffingDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ShippingLine { get; set; }
        public int ShippingLineId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHAName { get; set; }
        public int CHAId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [StringLength(45, ErrorMessage = "Rotation Cannot Be More Than 45 Characters")]
        public string Rotation { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999999.99, ErrorMessage = "FOB Cannot Be More Than 99999999.99")]
        public decimal Fob { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999999.99, ErrorMessage = "Gross Duty Cannot Be More Than 99999999.99")]
        public decimal GrossDuty { get; set; }
        public int DeliveryType { get; set; }
        public int IsDO { get; set; }
        public string DestuffingXML { get; set; }
        public int Uid { get; set; }

        public List<DestuffingDtl> LstDestuffing = new List<DestuffingDtl>();
    }

    public class DestuffingAppList
    {
        public string DestuffingNo { get; set; }
        public string DestuffingDate { get; set; }
        public int DestuffingId { get; set; }
        public string BOENo { get; set; }
        public string Importer { get; set; }
        public string CHA { get; set; }
    }
}