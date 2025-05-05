using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class ListOfCustAppraismentAppr
    {
        public int CstmAppraiseWorkOrderId { get; set; }
        public string CstmAppraiseWorkOrderNo { get; set; }       
        public string AppraisementNo { get; set; }
        public string BOENo { get; set; }
        public string CHA { get; set; }
        public string Importer { get; set; }

    }
}