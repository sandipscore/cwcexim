using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class CHnDtrOpningClosing
    {

        public Int64 Closingboe { get; set; } = 0;
        public Int64 Closingpkg { get; set; } = 0;
        public decimal Closingwgt { get; set; } = 0;
        public decimal Closingarea { get; set; } = 0;
        

    }
    public class DtrOpening
    {
        public int Openingboe { get; set; } = 0;
        public int Openingpkg { get; set; } = 0;
        public decimal Openingwgt { get; set; } = 0;
        public decimal Openingarea { get; set; } = 0;
    }
    public class DtrReciept
    {
        public int sumobl { get; set; } = 0;
        public int sumNoPkg { get; set; } = 0;
        public decimal sumGrWt { get; set; } = 0;
        public decimal sumArea { get; set; } = 0;
    }
    public class Dtrcargo
    {
        public int NoBoe { get; set; } = 0;
        public int NoPkg { get; set; } = 0;
        public decimal GrWt { get; set; } = 0;
        public decimal Area { get; set; } = 0;
    }



    public class Containermodel
    {
        public int SlNo { get; set; } = 0;
        public string ContainerNo { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;

        public string CFSCode { get; set; } = string.Empty;
        public string InDate { get; set; } = string.Empty;

        public string SlaCd { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public int NoObl { get; set; } = 0;
        public int NoPkg { get; set; } = 0;
        public decimal GrWt { get; set; } = 0;
        public int NoBoe { get; set; } = 0;
        public string Shed { get; set; } =string.Empty;
        public decimal Area { get; set; } = 0;
        public string SealCuttingDate { get; set; } = string.Empty;
        
    }
}