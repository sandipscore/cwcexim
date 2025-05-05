using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class WFLD_ApproveDeliveryOrder
    {
        public string ApproveDate { get; set; }

        public string OBLNo { get; set; }
        public string OBLDate { get; set; }
        public int DestuffingId { get; set; }

        [Required(ErrorMessage="Please Field OTP")]
       
        public int MobileGenerateCode { get; set; }

        public int Uid { get; set; }
    }

    public class WFLD_OBLStatusDetails
    {
        public string BOLNo { get; set; }
        public int NOPKG { get; set; }
        public decimal GRWT { get; set; }
        public string CARGO_DESC { get; set; }
        public string IMP_NAME { get; set; }
        public string CONTAINER_NO { get; set; }
        public decimal CIFValue { get; set; }
        public decimal Duty { get; set; }
    }
}