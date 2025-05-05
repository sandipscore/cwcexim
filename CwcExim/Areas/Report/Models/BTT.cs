using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{
    public class BTT
    {
        public string ShippingBillNo { get; set; }
        public string BTTCargoEntryDate { get; set; }
        public string CommName { get; set; }
        public int NoOfUnits { get; set; }
        public decimal GrossWeight { get; set; }
        public int ENoOfUnits { get; set; }
        public decimal EGrossWeight { get; set; }
        public int BTTQuantity { get; set; }
        public decimal BTTWeight { get; set; }
    }
    public class BTTDate
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
    }
}