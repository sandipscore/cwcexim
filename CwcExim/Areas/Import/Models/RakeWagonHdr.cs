using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class RakeWagonHdr
    {   
        public int RWHdrId { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string StationFrom { get; set; }
        public string StationTO { get; set; }
        public string RakeId { get; set; }
        public string RakeName { get; set; }
        public string NoOfWagon { get; set; }
        public string LstRelsTime { get; set; }
        public string FstOprPlcTime { get; set; }
        public int IsSend { get; set; }

        public IList<WgonDtl> lstWgon { get; set; } = new List<WgonDtl>();

        public string WgonDetailsJS { get; set; }

    }

    public class WgonDtl
    {
        public int ID { get; set; }
        public int ParentId { get; set; }
        public string WgonId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ContNo { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ContFlg { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ContSize { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ContPosn { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Commodity { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string TrafficType { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string CommodityCode { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ContLodWt { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ContTareWt { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string SmtpNo { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string SmtpDt { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string HsnCode { get; set; }

    }

    public class AllInvoiceList
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
    }

  }