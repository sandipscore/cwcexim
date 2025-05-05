using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class EconomyReport
    {
        //IncomeExpHeadId, ItemType, FieldType, ItemHead, ItemLabel, ItemSortOrder, ParentId, Amount, CumAmount, ProCumAmount, IsTextBox
        public int Sr { get; set; }
        public int IncomeExpHeadId { get; set; }
        public string ItemType { get; set; }
        public string ItemHead { get; set; }
        public string ItemLabel { get; set; }
        public string Amount { get; set; }
        public string CumAmount { get; set; }
        public string ProCumAmount { get; set; }
        public int IsTextBox { get; set; }
        public string ItemCodeNo { get; set; } = "";
        public int ItemSortOrder { get; set; } = 0;
        public int PageNo { get; set; } = 0;
    }


    public class EconomyRptIncomeSummary
    {
        //Sr, IncomeExpHeadId, CodeNo, ItemLabel, Amount, CumAmount, ProCumAmount
        public int Sr { get; set; }
        public int IncomeExpHeadId { get; set; }
        public string CodeNo { get; set; }
        public string ItemLabel { get; set; }
        public string Amount { get; set; }
        public string CumAmount { get; set; }
        public string ProCumAmount { get; set; }
    }


    public class EconomyRptPrint
    {
        //FormDate, ToDate, SqmCovered, BagCovered, SqmOpen, BagOpen
        public int IsFound { get; set; }
        public string CreatedOn { get; set; }
        public string FormDate { get; set; }
        public string ToDate { get; set; }
        public string SqmCovered { get; set; }
        public string BagCovered { get; set; }
        public string SqmOpen { get; set; }
        public string BagOpen { get; set; }

        public List<EconomyReport> RptDetails = new List<EconomyReport>();
        public List<EconomyRptIncomeSummary> RptSummary = new List<EconomyRptIncomeSummary>();

    }
}