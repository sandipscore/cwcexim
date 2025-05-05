using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class MonthlyEconomyReport
    {
        public int IncomeExpHeadId { get; set; }
        public int FieldType { get; set; }
        public string ItemType { get; set; }
        public string ItemHead { get; set; }
        public string ItemLabel { get; set; }
        public int ItemSortOrder { get; set; }
        public string ItemCodeNo { get; set; }
        public int ParentId { get; set; }
        public int BranchId { get; set; }

        public int MonthTransId { get; set; }
        public int YearNo { get; set; }
        public int MonthNo { get; set; }
        public decimal Amount { get; set; }
        public decimal CumAmountSinceApril { get; set; }
        public decimal ProrataCumAmount { get; set; }
        public int NoOfPosts { get; set; }
        public string Remarks { get; set; }
        public string Script { get; set; }
        public int CreatedBy { get; set; }

    }

    public enum FieldType
    {
        NotRequiredField = -1,
        NonValuedField = 0,
        TextBoxWithoutJs = 1,
        TextBoxWithJs = 2,
        ReadOnlyTextBoxWithJsResult = 3,
        ReadOnlyTextBoxWithPrevMonthData = 4,
        ReadOnlyTextBoxWithSqlData = 5,
    }

    public class ItemType
    {
        public const string Income = "I";
        public const string AnnexureHead = "AH";
        public const string AnnexureEstablishment = "AE";
        public const string AnnexureTotal = "AT";
        public const string Expense_Other = "EO";
        public const string Expense_Rent = "ER";
        public const string Expense_Print = "EP";
        public const string Expense_Total = "ET";
    }
}