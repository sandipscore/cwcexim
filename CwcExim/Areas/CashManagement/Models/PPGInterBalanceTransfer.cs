using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.CashManagement.Models
{
    public class PPGInterBalanceTransfer
    {
        public Int32 ID { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [MaxLength(100, ErrorMessage = "Party Name Cannot Be More Than 100 Characters")]
        public string FromPartyName { get; set; }
        public Int32 FromPartyId { get; set; }
        public Int32 FromPDAId { get; set; }
        public decimal FromPartyBalance { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [MaxLength(100, ErrorMessage = "Party Name Cannot Be More Than 100 Characters")]
        public string ToPartyName { get; set; }

        public Int32 ToPartyId { get; set; }
        public Int32 ToPDAId { get; set; }
        public decimal TransferBalance { get; set; }

        public string TransferDate { get; set; }

    }
}