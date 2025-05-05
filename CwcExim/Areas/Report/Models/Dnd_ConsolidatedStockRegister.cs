using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class Dnd_ConsolidatedStockRegister
    {
        [Required]
        public string AsOnDate { get; set; }

       

        public List<Dnd_ConsolidatedStockRegiterGodown> lstgodown = new List<Dnd_ConsolidatedStockRegiterGodown>();
        public List<Dnd_ConsolidatedStockRegisterDetails> StockDetails { get; set; } = new List<Dnd_ConsolidatedStockRegisterDetails>();
    }

    public class Dnd_ConsolidatedStockRegiterGodown
    {
        public int GodownId { get; set; }

        public string GodownName { get; set; }
        public string SLACode { get; set; }
    }

    public class Dnd_ConsolidatedStockRegisterDetails
    {

        public int GodownId { get; set; }

        public string GodownName { get; set; }
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