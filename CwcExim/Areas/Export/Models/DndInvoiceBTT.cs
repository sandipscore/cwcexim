using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class DndInvoiceBTT: DNDExpInvoiceBase
    {
        public int Escort { get; set; } = 0;
        public List<DNDExpInvoiceContainerBase> lstPostPaymentCont { get; set; } = new List<DNDExpInvoiceContainerBase>();
        public List<DNDExpInvoiceChargeBase> lstPostPaymentChrg { get; set; } = new List<DNDExpInvoiceChargeBase>();
        public IList<DND_ExpContWiseAmount> lstContWiseAmount { get; set; } = new List<DND_ExpContWiseAmount>();
        public List<DND_ExpOperationContWiseCharge> lstOperationCFSCodeWiseAmount { get; set; } = new List<DND_ExpOperationContWiseCharge>();
        public List<DND_ExpCargoDtl> lstPreInvoiceCargo { get; set; } = new List<DND_ExpCargoDtl>();
        public IList<Dnd_PreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<Dnd_PreInvoiceContainer>();
        //public IList<DND_ExpContainer> lstPSCont { get; set; } = new List<DND_ExpContainer>();
        public IList<string> ActualApplicable { get; set; } = new List<string>();       
    }

}