using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    public class ExpenseHead
    {
        public int ExpenseHeadId { get; set; }

        [Display(Name="Expense Code")]
        [Required(ErrorMessage = "Fill Out This Field")]
        [StringLength(15,ErrorMessage = "Expense Code Cannot Be More Than 15 Characters In Length")]
        [RegularExpression("^[a-zA-Z0-9//-]+$",ErrorMessage ="Expense Code Can Contain Only Alphabets,Numeric Digits And Special Character '-'")]
        public string ExpenseCode { get; set; }

        [Display(Name = "Expense Head")]
        [Required(ErrorMessage = "Fill Out This Field")]
        [StringLength(30,ErrorMessage = "Expense Head Cannot Be More Than 30 Characters In Length")]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage ="Expense Head Can Conatin Only Alphabets")]
        public string ExpHead { get; set; }

      //  [Display(Name = "HSN Code")]
      //  [Required(ErrorMessage ="Fill Out This Field")]
       // [RegularExpression("^[0-9,.//-]+$",ErrorMessage = "HSN Code Can Contain Numeric Digits And Special Character Like , . -")]
      //  public string HSNCode { get; set; }

       // public int HSNId { get; set; }

      // public List<ExpenseHead> HSCCodeList = new List<ExpenseHead>();
        public int Uid { get; set; }
    }
}