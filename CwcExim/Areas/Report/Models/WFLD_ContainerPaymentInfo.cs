using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_ContainerList
    {
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }

    }
    public class WFLD_ContainerPaymentInfo
    {
        
        public string ContainerNo { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string CFSCode { get; set; }
        public string InDate { get; set; }
        public string Size { get; set; }
        public string EximTraderAlias { get; set; }
        public string OutDate { get; set; }

        public List<WFLD_ContainerPaymentDtl> lstContainerPaymentDtl = new List<WFLD_ContainerPaymentDtl>();
    }
    
    public class WFLD_ContainerPaymentDtl
    {
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public string PartyName { get; set; }
        public string ChargeType { get; set; }
        public decimal Amount { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string InvoiceType { get; set; }
    }
}