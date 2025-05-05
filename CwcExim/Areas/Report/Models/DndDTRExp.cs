using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DndDTRExp
    {
        public IList<CartingDetails> lstCartingDetails { get; set; } = new List<CartingDetails>();
        public IList<CartingDetails> lstShortCartingDetails { get; set; } = new List<CartingDetails>();
        public IList<CargoAcceptingDetails> lstCargoShifting { get; set; } = new List<CargoAcceptingDetails>();
        public IList<CargoAcceptingDetails> lstCargoAccepting { get; set; } = new List<CargoAcceptingDetails>();
        public IList<BTTDetails> lstBTTDetails { get; set; } = new List<BTTDetails>();
        public IList<StuffingDetails> lstStuffingDetails { get; set; } = new List<StuffingDetails>();
        public IList<StockDetails> StockOpening { get; set; } = new List<StockDetails>();
        public IList<StockDetails> StockClosing { get; set; } = new List<StockDetails>();
    }

}