using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class Ppg_OBLSpilt
    {
        public int SpiltID { get; set; }
        public string SpiltNo { get; set; }
        public string SpiltDate { get; set; }

       
        public string OBLNo { get; set; }

        public string OBLNoDate { get; set; }

        public string SpiltOBLNo { get; set; }

        public string SpiltOBLDate { get; set; }

        public string CargoDesc { get; set; }

        public string CommodityName { get; set; }

        public string CommodityId { get; set; }

        public decimal NoOfPkg { get; set; }

        public decimal GRWT { get; set; }

        public int ImporterId { get; set; }

        public string ImporterName { get; set; }

        public string ContainerNo { get; set; }

        public string LineNo { get; set; }

        public string SMPTNo { get; set; }

        public string SMPTDate { get; set; }

        public string IGMNo { get; set; }

        public string IGMDate { get; set; }
        public int IsFCL { get; set; }

        public int HeaderId { get; set; }
        public int DetailsId { get; set; }

        public string PKG_Type { get; set; }

        public List<Ppg_OBLSpiltDetails> lstSpiltDetails { get; set; } = new List<Ppg_OBLSpiltDetails>();

    }

    public class Ppg_OBLSpiltDetails
    {
        public string SpiltOBL { get; set; }
        public string SpiltOBLDate { get; set; }
        public string SpiltCargoDesc { get; set; }
        public string SpiltCommodityName { get; set; }
        public int SpiltCommodityId { get; set; }
        public decimal SpiltPkg { get; set; }
        public decimal SpiltWT { get; set; }
        public string SpiltImporter { get; set; }
        public int SpiltImporterID { get; set; }
        public string SpiltLineNo { get; set; }
        public string SpiltSMPTNo { get; set; }
        public string SpiltSMPTDate { get; set; }
        public string SpiltIGMNo { get; set; }
        public string SpiltIGMDate { get; set; }
        
    }


    public class Ppg_OBLSpiltList
    {
        public string SpiltOBLNo { get; set; }

        public string SpiltOBLDate { get; set; }

        public int IsFCL { get; set; }
    }
}