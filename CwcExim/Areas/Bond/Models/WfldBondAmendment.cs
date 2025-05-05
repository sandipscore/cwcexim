using System.Collections.Generic;

namespace CwcExim.Areas.Bond.Models
{
    public class WfldBondAmendment
    {
        public int SpaceappId { get; set; }
        public string SACNo { get; set; }
        public string OldUOM { get; set; }
        public string NewUOM { get; set; }

    }
    public class WfldBondAmendmentDtl
    {
        public string DepositNo { get; set; }
        public string DepositDate { get; set; }
        public string BondNo { get; set; }
        public int OldUnits { get; set; }
        public int NewUnits { get; set; }
        public decimal OldWeight { get; set; }
        public decimal NewWeight { get; set; }
        public int UnloadingId { get; set; }
        public int DepositAppId { get; set; }
    }
    public class WfldBondAmendmentViewNodel
    {
        public string AmendNo { get; set; }
        public string AmendDate { get; set; }
        public WfldBondAmendment objBondAmend { get; set; } = new WfldBondAmendment();
        public List<WfldBondAmendmentDtl> lstBondAmendDtl { get; set; } = new List<WfldBondAmendmentDtl>();
    }
    public class WfldBondAmendmentList
    {
        public string AmendNo { get; set; }
        public string AmendDate { get; set; }
        public string SACNo { get; set; }
        public string SACDate { get; set; }
    }
}