using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{
    public class WFLDChequeDD
    {
      
        public string DepositDate { get; set; }
        //  [Required(ErrorMessage = "Fill Out This Field")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ReceiptDate { get; set; }
        public string AccountNumber { get; set; }
        public string CompanyAddress { get; set; }
        public string EmailAddress { get; set; }
       // public string DepositDate { get; set; }
        public int Id { get; set; }
        public List<ChqDDDtl> ChqDetails { get; set; } = new List<ChqDDDtl>();
        public List<DepositDtl> DepositDetails { get; set; } = new List<DepositDtl>();
        public List<ReceiptDtl> ReceiptDetails { get; set; } = new List<ReceiptDtl>();

    }

    public class ChqDDDtl
    {
        public string PartyName;
        public string ChqNo;
        public string ChqDate;
        public string BankName;
        public decimal Amount;
    }

    public class DepositDtl
    {
        public int Id { get; set; }
        public string DepositDateValue { get; set; }
    }

    public class ReceiptDtl
    {
        public string ReceiptDateValue { get; set; }
        public string DepositDate { get; set; }
    }
}