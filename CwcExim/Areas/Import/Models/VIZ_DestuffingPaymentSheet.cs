using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class VIZ_DestuffingPaymentSheet
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public string CHAGSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public int StuffingReqId { get; set; }
        public int DeliveryType { get; set; }
        public string StuffingReqNo { get; set; }
        public string StuffingReqDate { get; set; }

        public string DestuffingEntryDate { get; set; } = "";

        public string Importer { get; set; } = "";

        public int ForwarderId { get; set; } = 0;
        public string Forwarder { get; set; } = "";
        public string HBLNo { get; set; }
    }

    public class VIZ_PaymentSheetContainer
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public bool Selected { get; set; }
        public string Size { get; set; }
        public string ArrivalDt { get; set; }
        public string IsHaz { get; set; }
        public string LCLFCL { get; set; }
        public string LineNo { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }

        public string OBLNo { get; set; }
        public string SealCutDate { get; set; }
        public int NoOfPkg { get; set; }
        public decimal GrWait { get; set; }

        public bool IsBond { get; set; }
        public int CargoType { get; set; }

    }

    public class VIZ_DestuffingConntainer
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public bool Selected { get; set; }
        public string Size { get; set; }
        public string ArrivalDt { get; set; }
        public string IsHaz { get; set; }
    }
}