using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CwcExim.Areas.Auction.Models
{
    public class Dnd_Despatch
    {
        public int ID { get; set; }

        public int NoticeID { get; set; }

        [Required]
        [Display(Name ="Notice No")]
        public string NoticeNo { get; set; }

        [Required]
        [Display(Name = "Despatch No")]
        public string DespatchNo { get; set; }

        [Required]
        public string DespatchDate { get; set; }

        public string AuctionNo { get; set; }

    }
}