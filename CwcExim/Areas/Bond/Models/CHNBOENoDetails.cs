using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Bond.Models
{
    public class CHNBOENoDetails
    {
        public decimal AreaReserved { get; set; }
        public decimal SpaceReqReserved { get; set; }        
        public string BondNo { get; set; }
        public string BondDate { get; set; }

        public string DepositNo { get; set; }
        public string Remarks { get; set; }
        public int BondWOId { get; set; }
        public string BondBOENo { get; set; }
        public string BondBOEDate { get; set; }
        public string WRNo { get; set; }
        public string WRDate { get; set; }
        public int SpaceAppId { get; set; }
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public string CargoDescription { get; set; }
        public string SacNo { get; set; }
        public string SacDate { get; set; }
        public int Units { get; set; }
        public decimal Weight { get; set; }
        public string DepositDate { get; set; }

        public List<CHNGodownWiseDtl> LstGodownWiseDtl { get; set; } = new List<CHNGodownWiseDtl>();
    }
    public class CHNGodownWiseDtl
    {
        public string ReferenceNo { get; set; }
        public int ResvSpaceId { get; set; }
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public string SpaceUnreserved { get; set; }
        public string SpaceReserved { get; set; }
        public int Weight { get; set; }
        public int Units { get; set; }
        public decimal Value { get; set; }
        public decimal Duty { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public decimal UnloadedUnit { get; set; }
        public decimal BalancedUnit { get; set; }
        public decimal UnloadedWeight { get; set; }
        public decimal BalancedWeight { get; set; }
        public string ReservationTo { get; set; }

    }
}