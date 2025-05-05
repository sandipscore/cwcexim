using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_ImpCargoInExcel
    {
        public int SLNo { get; set; }

        public string CFSCode { get; set; }
        public string DOR { get; set; }

        public int GRIDS { get; set; }
        public string FirstEffective { get; set; }

        public int FirstNoOfDays { get; set; }

        public string FirstRate { get; set; }


        public string SecondEffective { get; set; }

        public int SecondNoOfDays { get; set; }

        public string SecondRate { get; set; }


        public string ThirdEffective { get; set; }

        public int ThirdNoOfDays { get; set; }

        public string ThirdRate { get; set; }

        public string FourthEffective { get; set; }

        public int FourthNoOfDays { get; set; }

        public string FourthRate { get; set; }

        public string FifthEffective { get; set; }

        public int FifthNoOfDays { get; set; }

        public string FifthRate { get; set; }


        public string SixEffective { get; set; }

        public int SixNoOfDays { get; set; }

        public string SixRate { get; set; }

        public decimal TotalUpto { get; set; }

        public string DateDiff { get; set; }

        public decimal UptoAmount { get; set; }

    }

    public class Hdb_ImpCargoInGodownInExcel
    {
        public string CFSCode { get; set; }

        public string DOR { get; set; }

        public string DestuffingEntryDate { get; set; }

        public DateTime EntryDate { get; set; }

        public string StorageType { get; set; }

        public string LCLFCL { get; set; }
        public string Size { get; set; }

        public decimal Area { get; set; }

        public int CargoType { get; set; }


    }


}