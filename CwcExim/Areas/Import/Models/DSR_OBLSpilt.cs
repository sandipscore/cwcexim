using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class DSR_OBLSpilt
    {
        public int SpiltID { get; set; }
        public string SpiltNo { get; set; }
        public string SpiltDate { get; set; }
        public string OBLNo { get; set; }

        public string OBLNoDate { get; set; }

        public string SpiltOBLNo { get; set; }

        public string SpiltOBLDate { get; set; }
      
        public decimal NoOfPkg { get; set; }

        public decimal GRWT { get; set; }

        public decimal Value { get; set; }
        public decimal Duty { get; set; }

        public int IsFCL { get; set; }

        public int HeaderId { get; set; }
        public int DetailsId { get; set; }

     

        public List<DSR_OBLSpiltDetails> lstSpiltOBLDetails { get; set; } = new List<DSR_OBLSpiltDetails>();
        public List<DSR_ContainerSpiltList> lstSpiltContainerDetails { get; set; } = new List<DSR_ContainerSpiltList>();

    }

    public class DSR_OBLSpiltDetails
    {
        public string SpiltOBL { get; set; }
        public string SpiltOBLDate { get; set; }       
        public decimal SpiltPkg { get; set; }
        public decimal SpiltWT { get; set; }
        public decimal SpiltValue { get; set; }
        public decimal SpiltDuty { get; set; }

    }


    public class DSR_OBLSpiltList
    {
        public string SpiltOBLNo { get; set; }

        public string SpiltOBLDate { get; set; }

        public int IsFCL { get; set; }

        public int Rows { get; set; }
    }


    public class DSR_ContainerSpiltList
    {
        public string SpiltOBLNO { get; set; }
        public string SpiltContainerNo { get; set; }

        public string SpiltCFSCode { get; set; }

        public int SpiltSize { get; set; }

        public int SpiltHeaderID { get; set; }

        public int SpiltDetailsId { get; set; }

        public bool Selected { get; set; }
     
    }
}