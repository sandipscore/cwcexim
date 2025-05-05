using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class BEODetails
    {
        public BeoNoDetails BeoNoDetails { get; set; } = new BeoNoDetails();

        public BeoOOCDetails BeoOOCDetails { get; set; } = new BeoOOCDetails();
        public List<BeoIGMDetails> BeoIGMDetails { get; set; } = new List<BeoIGMDetails>();

    }

    public class BeoNoDetails
    {
      
        public string BOE_NO { get; set; }
        public string BOE_DATE { get; set; }

        public string IMP_NAME { get; set; }
        public string IEC_CD { get; set; }
        public string IMP_ADD1 { get; set; }
        public string IMP_ADD2 { get; set; }
        public string CITY { get; set; }
        public string PIN { get; set; }
        public string C_OF_ORIGIN { get; set; }
        public string CHA_CODE { get; set; }
        public string NO_PKG { get; set; }
        public string PKG_TYPE { get; set; }
        public string GR_WT { get; set; }
        public string UNIT_TYPE { get; set; }
        public string CIF_VALUE { get; set; }
        public string DUTY { get; set; }
        public string CONTAINER_NO { get; set; }
      
    }

    public class BeoOOCDetails
    {
      
        public string OOC_TYPE { get; set; }
        public string OOC_NO { get; set; }
        public string OOC_DATE { get; set; }
        public string NO_PKG { get; set; }
        public string PKG_TYPE { get; set; }
        public string GR_WT { get; set; }
        public string UNIT_TYPE { get; set; }
        public string CIF_VALUE { get; set; }
        public string DUTY { get; set; }
    }

    public class BeoIGMDetails
    {
       
        public string OBL_NO { get; set; }
        public string OBL_DATE { get; set; }
        public string CONTAINER_NO { get; set; }
        public string PORT_OF_LOADING { get; set; }
        public string NATURE_OF_CARGO { get; set; }
        public string ITEM_TYPE { get; set; }
        public string NO_PKG { get; set; }
        public string PKG_TYPE { get; set; }
        public string GR_WT { get; set; }
        public string CARGO_DESC { get; set; }
        public string UNIT_OF_WEIGHT { get; set; }
        public string VOLUME { get; set; }
        public string UNIT_OF_VOLUME { get; set; }
        public string LOCAL_IGM_NO { get; set; }
        public string LOCAL_IGM_DATE { get; set; }
        public string LINE_NO { get; set; }
        public string SUB_LINE_NO { get; set; }
    }
}