using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class Dnd_StockRegisterReportViewModel
    {

        [Required]
        public string AsOnDate { get; set; }
        
        public int? GodownId { get; set; }
        public int? Shippingid { get; set; }
        public string ShippingName { get; set; }
        [Required]
        public string GodownName { get; set; }

        public List<Dnd_StockRegiterShippingLine> lstShipping = new List<Dnd_StockRegiterShippingLine>();
        public List<Dnd_StockRegister> StockDetails { get; set; } = new List<Dnd_StockRegister>();

    }

    public class Dnd_StockRegiterShippingLine
    {
        public int Shippingid { get; set; }

        public string ShippingName { get; set; }
        public string SLACode { get; set; }
    }
  
    public class Dnd_StockRegister
    {
        public int Shippingid { get; set; }

        public string ShippingName { get; set; }

        public string SbNo { get; set; }
        public string SbDate { get; set; }

        public string EntryNo { get; set; }

        public string CartingDate { get; set; }

        public decimal NoOfPKG { get; set; }

        public decimal GWT { get; set; }
        public decimal FOB { get; set; }
        public string SlotNo { get; set; }

        public decimal Area { get; set; }

        public string Crg { get; set; }

        public string CargoDesc { get; set; }

       
    }


}