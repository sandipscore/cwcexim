using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class SpecialOperationReq
    {
        public int SpclOprtnReqId { get; set; }
        public string SpclOprtnReqNo { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ContainerNo { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ReqDate { get; set; }
        public string CFSCode { get; set; }
        public int Size { get; set; }
        public string ShippingLine { get; set; }
        public int ShippingLineid { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PermissionDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public int oprtntypId { get; set; }       
        public int containerlassid { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public  string ContainerClass { get; set; }
        public int CargoType { get; set; }

        public string CargoTyp { get; set; }
        //[Required(ErrorMessage = "Fill Out This Field")]
        public int? cnttypeId { get; set; } = 0;
        public string Remarks { get; set; }
        
        //public int MyProperty { get; set; }
    }
}