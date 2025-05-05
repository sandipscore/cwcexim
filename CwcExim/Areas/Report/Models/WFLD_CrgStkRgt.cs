using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_CrgStkRgt
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public IList<WFLDexportCargoStock> ppgexportCargoStocklst = new List<WFLDexportCargoStock>();

        public IList<WFLDimportCargoStock> ppgimportCargoStocklst = new List<WFLDimportCargoStock>();

        //public IList<ppg_bondCargoStock> bondCargoStocklst = new List<ppbondCargoStock>();



    }

        //public string value { get; set; }
    
    public class WFLDexportCargoStock

    {
        public string shippingBillNo { get; set; }

        public string Date { get; set; }


        public string NoOfPackage { get; set; }

        public string Commodity { get; set; }


        public string Remarks { get; set; }

    }
    public class WFLDimportCargoStock

    {
        public string BOE { get; set; }

        public string Date { get; set; }


        public string NoOfPackage { get; set; }

        public string Commodity { get; set; }


        public string Remarks { get; set; }

    }

  /*  public class bondCargoStock

    {
        public string Warehouse { get; set; }

        public string Date { get; set; }


        public string NoOfPackage { get; set; }

        public string Commodity { get; set; }


        public string Remarks { get; set; }

    }*/
}














