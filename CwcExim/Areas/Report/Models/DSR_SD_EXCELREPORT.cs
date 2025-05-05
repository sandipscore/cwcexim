using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DSR_SD_EXCELREPORT
    {
        public int SLno { get; set; } = 0;
      //  public int PayeeId { get; set; }
        public string PayeeName { get; set; }
       public decimal Opening { get; set; }
 
        public decimal SDBalance { get; set; }
        public decimal Creadit { get; set; }



      
        public decimal IMPSTO { get; set; }
        public decimal IMPINS { get; set; }
        public decimal IMPHT { get; set; }
        public decimal IMPGRL { get; set; }
       
      // public decimal IMPOTHS { get; set; }
        public decimal EXPSTO { get; set; }
        public decimal EXPINS { get; set; }
        public decimal EXPHTT { get; set; }
        public decimal EXPGRE { get; set; }

        public decimal BNDSTO { get; set; }
      
        public decimal BNDINS { get; set; }
        public decimal BNDHT { get; set; }



        public decimal WET { get; set; }
        public decimal DOC { get; set; }
        public decimal MISCT { get; set; }
        public decimal PCS { get; set; }
      
        public decimal RCTSEAL { get; set; }
        public decimal OTHERS { get; set; }
        public decimal TotalTexable { get; set; }
        //  public decimal ADM { get; set; }

        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }






        public decimal RoundUp { get; set; }
        public decimal Total { get; set; }
        public decimal Closing { get; set; }

        //  public decimal MISCN { get; set; }

        // public decimal OTHS { get; set; }
        // public decimal MPCS { get; set; }

        //   public decimal EXPOTHS { get; set; }
        // public decimal EXPHTN { get; set; }




    }
}