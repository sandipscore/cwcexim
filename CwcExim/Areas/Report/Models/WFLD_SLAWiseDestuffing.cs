using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_SLAWiseDestuffing
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ShippingLine { get; set; }
        public int ShippingLineId { get; set; }
        public string OBLNo { get; set; }
        public string OBLDate { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string DestuffingDate { get; set; }
        public string DestuffingSheetNo { get; set; }
        public string TSANo { get; set; }
        public string TSADate { get; set; }
        public string LineNo { get; set; }
        public string Importer { get; set; }
        public decimal NoOFPKG { get; set; }
        public decimal RECEIVEDPKG { get; set; }
        public decimal Area{ get; set; }
        public decimal CBM { get; set; }
        public string SLOT { get; set; }
        public string CFS { get; set; }
        public string REMARKS { get; set; }
        public decimal Weight { get; set; }
       
    }
}