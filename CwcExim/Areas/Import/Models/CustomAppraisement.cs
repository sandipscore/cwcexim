using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class CustomAppraisement
    {
        public int CustomAppraisementId { get; set; }
        public string ContainerNo { get; set; }
        public string AppraisementNo { get; set; }
        public string AppraisementDate { get; set; }

        [Required(ErrorMessage ="Fill Out This Field")]
        public string ShippingLine { get; set; }
        public int ShippingLineId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHAName { get; set; }
        public int CHAId { get; set; }

      //  [Required(ErrorMessage = "Fill Out This Field")]
      //  [StringLength(45,ErrorMessage = "Rotation Cannot Be More Than 45 Characters")]
        public virtual string Rotation { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 99999999.99, ErrorMessage = "FOB Cannot Be More Than 99999999.99")]
        public decimal Fob { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0,99999999.99,ErrorMessage = "Gross Duty Cannot Be More Than 99999999.99")]
        public decimal GrossDuty { get; set; }
        public int DeliveryType { get; set; }
        public int IsDO { get; set; }
        public string CustomAppraisementXML { get; set; }
        public int Uid { get; set; }

        public List<CustomAppraisementDtl> LstAppraisement = new List<CustomAppraisementDtl>();
    }
}