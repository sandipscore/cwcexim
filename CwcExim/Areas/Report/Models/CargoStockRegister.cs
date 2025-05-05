using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class CargoStockRegister
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public IList<exportCargoStock> exportCargoStocklst = new List<exportCargoStock>();

        public IList<importCargoStock> importCargoStocklst = new List<importCargoStock>();

        public IList<bondCargoStock> bondCargoStocklst = new List<bondCargoStock>();





        //public string value { get; set; }
    }
    public class exportCargoStock

    {
        public string shippingBillNo { get; set; }

        public string Date { get; set; }


        public string NoOfPackage { get; set; }

        public string Commodity { get; set; }


        public string Remarks { get; set; }
        
    }
    public class importCargoStock

    {
        public string BOE { get; set; }

        public string Date { get; set; }


        public string NoOfPackage { get; set; }

        public string Commodity { get; set; }


        public string Remarks { get; set; }

    }

    public class bondCargoStock

    {
        public string Warehouse { get; set; }

        public string Date { get; set; }


        public string NoOfPackage { get; set; }

        public string Commodity { get; set; }


        public string Remarks { get; set; }

    }



}