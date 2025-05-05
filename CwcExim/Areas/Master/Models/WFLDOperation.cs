using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class WFLDOperation
    {
        public int OperationId { get; set; }

        public int SacId { get; set; }

        [Display(Name = "Type")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int Type { get; set; }

        [Display(Name = "Code/Clause")]/*Code is actually containing the value for Clause*/
        [StringLength(20, ErrorMessage = "Code Cannot Be More Than 20 Characters")]
        [RegularExpression("^[a-zA-Z0-9//-]+$", ErrorMessage = "Code Can Contain Only Alphabets,Numeric Digits And Special Character Like '/-'")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        public string Code { get; set; }

        [Display(Name = "Short Description")]
        [StringLength(100, ErrorMessage = "Short Description Cannot Be More Than 100 Characters In Length")]
        [RegularExpression("^[a-zA-Z0-9.,;:\\-() ]+$", ErrorMessage = "Short Description Can Contain Only Alphabets,Numeric Digits And Special Character Like '.' , ',' , ';' , ':' , '-'")]
        public string ShortDescription { get; set; }

        [Display(Name = "Description")]
        [StringLength(500, ErrorMessage = "Description Cannot Be More Than 500 Characters In Length")]
        [RegularExpression("^[a-zA-Z0-9.,;:\\-\\r\\n ]+$", ErrorMessage = "Description Can Contain Only Alphabets,Numeric Digits And Special Character Like '.,;:-'")]
        public string Description { get; set; }
        public int Uid { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "SAC")]
        public string SacCode { get; set; }

        public List<WFLDSac> LstSac = new List<WFLDSac>();
    }
}