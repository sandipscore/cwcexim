using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class PpgCoreDataRptModel
    {
        public decimal ICDCurrMon { get; set; }
        public decimal ICDCommuMon { get; set; }
        public decimal ICDPreCurrMon { get; set; }
        public decimal ICDPreCommuCurrMon { get; set; }
        public decimal MFCurrMon { get; set; }
        public decimal MFCommuMon { get; set; }
        public decimal MFPreCurrMon { get; set; }
        public decimal MFPreCommuCurrMon { get; set; }
        public decimal CRTCurrMon { get; set; }
        public decimal CRTCommuMon { get; set; }
        public decimal CRTPreCurrMon { get; set; }
        public decimal CRTPreCommuMon { get; set; }
        public decimal PESTCurrMon { get; set; }
        public decimal PESTCommuMon { get; set; }

        public decimal PESTPreCurrMon { get; set; }

        public decimal PESTPreCommuCurrMon { get; set; }

        public decimal OtherCurrMon { get; set; }

        public decimal OtherCommuMon { get; set; }
        public decimal OtherPreCurrMon { get; set; }
        public decimal OtherPreCommuMon { get; set; }
        public decimal TotActMon { get; set; }
        public decimal TotCommuMon { get; set; }
        public decimal TotPreActMon { get; set; }
        public decimal TotPreCommMon { get; set; }
    }
}