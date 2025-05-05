using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Ppg_ApproveDeliveryOrder
    {
        public string ApproveDate { get; set; }
        public string OBLNo { get; set; }
        public string OBLDate { get; set; }
        public int DestuffingId { get; set; }

        [Required(ErrorMessage = "Please Field OTP")]
        public int MobileGenerateCode { get; set; }
        public int Uid { get; set; }
        public string ContainerNo { get; set; }
    }
}