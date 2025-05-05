using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Hdb_ListOfCustAppraismentAppr
    {
        public int CstmAppraiseWorkOrderId { get; set; }
        public string CstmAppraiseWorkOrderNo { get; set; }
        public int CustomAppraisementId { get; set; }
        public string AppraisementNo { get; set; }
        public string BOENo { get; set; }
        public string CHA { get; set; }
        public string Importer { get; set; }

        public string ContainerNo { get; set; }
        public string FormOneNo { get; set; }
        public string BL { get; set; }

    }
}