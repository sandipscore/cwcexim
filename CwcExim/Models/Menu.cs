using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class Menu
    {
        public int MenuId { get; set; }

        [Required(ErrorMessage = "Fill out this field")]
        //[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Menu Name must contain only alphabets")]
        [StringLength(maximumLength:120,ErrorMessage ="Menu Name must not be more than 120 characters")]
        [Display(Name ="Menu")]
        public string MenuName { get; set; }
        public int? ParentMenuId { get; set; }

        [Display(Name = "Parent Menu")]
        public string ParentMenuName { get; set; }
        public IEnumerable<Menu> ParentMenuList { get; set; }

        [Required(ErrorMessage = "Fill out this field")]
        public int? ModuleId { get; set; }

        [Display(Name = "Module")]
        public string ModuleName { get; set; }
        public IEnumerable<Module> ModuleList { get; set; }


        [Display(Name ="Action Url")]
        public string ActionUrl { get; set; }

        [Required(ErrorMessage = "Please enter Display Position")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Display Position must be numeric")]
        [Range(minimum:1,maximum:100,ErrorMessage = "Display position must be greater than 0")]
        [Display(Name = "Display Position")]
        public int DisplayPosition { get; set; }       
        public int CreatedBy { get; set; }

        [Display(Name = "Created By")]
        public string CreatedByName { get; set; }

        [Display(Name = "Created On")]
        public string CreatedOn { get; set; }
        public int UpdatedBy { get; set; }

        [Display(Name = "Updated By")]
        public string UpdatedByName { get; set; }

        [Display(Name = "Updated On")]
        public string UpdatedOn { get; set; }


    }
}