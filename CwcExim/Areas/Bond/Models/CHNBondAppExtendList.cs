using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Bond.Models
{
    public class CHNBondAppExtendList
    {
        public int SpaceAvailCertExtId { get; set; }
        public string SacNo { get; set; }
        [Display(Name = "CHA")]
        public string CHAName { get; set; }
        [Display(Name = "Importer")]
        public string ImporterName { get; set; }
    }

    public class CHNBondAppExtendDetails
    {
        public int SpaceAvailCertExtId { get; set; }
        public decimal AreaReserved { get; set; }
        public int SpaceappId { get; set; }
        public string ExtendOn { get; set; }
        public string ExtendUpto { get; set; }
        public string SacNo { get; set; }
        public string ApplicationNo { get; set; }
        public string SacDate { get; set; }
        public string ApplicationDate { get; set; }
        public string ValidUpto { get; set; }
        public string CHAName { get; set; }
        public string ImporterName { get; set; }
        public int IsApproved { get; set; }
    }
    public class PrintChnSACExt
    {
        public string SacNo { get; set; }
        public string SacDate { get; set; }
        public string BOLAWBNo { get; set; }
        public string ImporterName { get; set; }
        public string CHAName { get; set; }
        public string BOE { get; set; }
        public decimal AreaReserved { get; set; }
        public string ExtendUpto { get; set; }
    }
}