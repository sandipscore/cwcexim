using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    public class ExpenseCodeWiseHSN
    {
        public int ExpHSNId { get; set; }

        [Required(ErrorMessage ="Fill Out This Field")]
        public string HSNCode { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ExpenseCode { get; set; }
        public int Uid { get; set; }
        public IList<ExpenseCodeWiseHSN> LstHSN { get; set; } = new List<ExpenseCodeWiseHSN>();
        public IList<ExpenseHead> LstExpenseCode { get; set; } = new List<ExpenseHead>();
    }
}