using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_EmpContReg
    {
        public string mstcompany { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public List<WFLD_EmpCont> LstEmpty { get; set; } = new List<WFLD_EmpCont>();
        public class WFLD_EmpCont
        {
            public string CFSCode { get; set; }
            public string InDateTime { get; set; }
            public string ContainerNo { get; set; }
            public string Size { get; set; }
            public string TransPort { get; set; }
            public string ShippingLine { get; set; }
            public string Forwarder { get; set; }
            public string Status { get; set; }
            public string ContainerClass { get; set; }
            public string VehicleNo { get; set; }
            public string Remarks { get; set; }
            public string DateOfArrival { get; set; }
            public string ImportExport { get; set; }
            public int SlNo { get; set; }



        }
    }
}