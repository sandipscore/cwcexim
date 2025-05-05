using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_BondStackReport
    {
        public List<Hdb_BondDetails> lstBondDetails { get; set; } = new List<Hdb_BondDetails>();
        public Hdb_BondDetails StackBondDetails { get; set; } = new Hdb_BondDetails();
        public List<UnloadingDetailsForStack> lstUnloadingDetails { get; set; } = new List<UnloadingDetailsForStack>();
        public List<DeliveryDetailsForStack> lstDeliveryDetails { get; set; } = new List<DeliveryDetailsForStack>();

    }

    public class Hdb_BondDetails
    {
        public int DepositappId { get; set; } = 0;
        public string IMPName { get; set; }
        public string IMPAdd { get; set; }
        public string CHAName { get; set; }
        public string CHAdd { get; set; }
        public string SacNo { get; set; }
        public string SacDate { get; set; }
        public string ValidUpto { get; set; }
        public decimal AreaReserved { get; set; }
        public string GodownName { get; set; }
        public string BondNo { get; set; }
        public string BondDate { get; set; }
        public string BondBOENo { get; set; }
        public string BondBOEDate { get; set; }
        public string ExpDateofWarehouse { get; set; }
        public string CargoDesc { get; set; }
    }

    public class UnloadingDetailsForStack
    {
        public int DepositappId { get; set; } = 0;
        public string UnloadedDate { get; set; }
        public int UnloadedUnits { get; set; }
        public decimal UnloadedWeights { get; set; }
        public decimal AreaOccupied { get; set; }
        public string PackageCondition { get; set; }
        public decimal Value { get; set; }
        public decimal Duty { get; set; }
    }

    public class DeliveryDetailsForStack
    {
        public int DepositappId { get; set; } = 0;
        public string DeliveryOrderNo { get; set; }
        public int Units { get; set; }
        public decimal Weight { get; set; }
        public decimal SQM { get; set; }
        public decimal Value { get; set; }
        public decimal Duty { get; set; }
        public string BondBOENo { get; set; }
        public string BondBOEDate { get; set; }
        public string EXBOENo { get; set; }
        public string ExBOEDate { get; set; }
    }
}