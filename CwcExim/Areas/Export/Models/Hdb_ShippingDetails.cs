using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Hdb_ShippingDetails: Hdb_ShippingDetail
    {
        public bool IsReefer { get; set; }
        public bool IsSEZ { get; set; }

        public string CustomHouseCode { get; set; }

        public string IECCode { get; set; }

        public string PortOfDestination { get; set; }

        public string GSTNo { get; set; }

        public string PackageType { get; set; }

        public string OtherPkgType { get; set; }
        public string CommInvDate { get; set; }
        public string PackingListDate { get; set; }
        public string OldShippingBill { get; set; } = string.Empty;
        public int OldSBId { get; set; } = 0;
        public int SBId { get; set; } = 0;

        public string SCMTRPackageType { get; set; }

        public int PackUQCId { get; set; }
        public string PackUQCCode { get; set; }
        public string PackUQCDescription { get; set; }
    }
}