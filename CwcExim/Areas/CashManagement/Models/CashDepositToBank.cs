using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class CashDepositToBank
    {
        public int Id { get; set; }
        public int BankId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string BankName { get; set; }
        public string BankAccountNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string DepositDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter more than 0")]
        public decimal DepositAmount { get; set; }
        public decimal BalanceAmount { get; set; }

    }
}