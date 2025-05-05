using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_FactoryStuffIWBRegister
    {
        public string EntryNo { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string SlaCd { get; set; }
        public string PortLoad { get; set; }
        public string POD { get; set; }
        public string Transporter { get; set; }
        public string CHACd { get; set; }
        public int NoPkg { get; set; }
        public string ReceiptNo { get; set; }
        public string PartyName { get; set; }
        public string GatePassNo { get; set; }
        public string GatePassDate { get; set; }
    }
}