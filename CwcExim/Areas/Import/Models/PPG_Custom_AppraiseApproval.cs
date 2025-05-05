using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class PPG_Custom_AppraiseApproval
    {
      //  public int CstmAppraiseWorkOrderId { get; set; }
        public int CustomAppraisementId { get; set; }
        public int CustomAppraisementDtlId { get; set; }
      //  public string CstmAppraiseWorkOrderNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string AppraisementNo { get; set; }
        public string AppraisementDate { get; set; }
        public string WorkOrderDate { get; set; }
        public string Remarks { get; set; }
        public int YardId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string YardName { get; set; }
        public string YardWiseLocationIds { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string YardWiseLctnNames { get; set; }
        public int Uid { get; set; }
        public string CustomAppraisementXML { get; set; }
        public string ShippingLine { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public int DeliveryType { get; set; }
        public string Rotation { get; set; }

        public List<PpgCustomAppraisementDtl> LstAppraisementDtl = new List<PpgCustomAppraisementDtl>();
        public int IsApproved { get; set; }
        public decimal Fob { get; set; }
        public decimal GrossDuty { get; set; }
    }
}