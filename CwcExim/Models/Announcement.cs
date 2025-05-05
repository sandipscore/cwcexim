using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Models
{
    public class Announcement
    {
       
        public int AnnounceId { get; set; }
        [Required(ErrorMessage ="Fill Out This Field")]
        [StringLength(500, ErrorMessage = "Title must be within 500 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Description { get; set; }
        [Display(Name ="Is Active")]
        public bool IsActive { get; set; }
        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string StartDate { get; set; }
        [Display(Name = "End Date")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string EndDate { get; set; }
        public int Uid { get; set; }
        public bool IsPublished { get; set; }
        public List<Announcement> LstAnnounce { get; set; } = new List<Announcement>();
        public string PublishedDate { get; set; }
    }
}