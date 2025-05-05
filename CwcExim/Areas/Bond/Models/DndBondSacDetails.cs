using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;

namespace CwcExim.Areas.Bond.Models
{
    public class DndBondSacDetails : BondSacDetails
    {
        public int UnloadingId { get; set; } = 0;

        public string BondNo { get; set; }
   
    }

    public class Dnd_PostPaymentChrg : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }
}