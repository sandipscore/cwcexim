using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLDReeferContainerModel
    {
        public int SR { get; set; }
        public string EntryNo { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string ReceiptNo { get; set; }

        public string ReceiptDate { get; set; }

        public string Forwarder { get; set; }

        public string TransporterName { get; set; }
       
       
        public string Amount { get; set; }


    }
}