using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{
  //  Sr | SB No | SB Date | Entry No | Exporter | Carting Date | No of PKg | GR Wt | FOB | Slot No | CBM | Storage Charges
    public class WFLD_ShipBillAmndRpt
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        public string SBNo { get; set; }
        public string SBDate { get; set; }
        public string EntryNo { get; set; }
        public string Exporter { get; set; }
        public string CartingDate { get; set; }
        public decimal NoOfPKG { get; set; }
        public decimal GRWt { get; set; }
        public decimal FOB { get; set; }
        public string SlotNo { get; set; }
        public decimal CBM { get; set; }
        public decimal StorageCharge { get; set; }

        public string OldSBNo { get; set; }
        public string OldSBDate { get; set; }
        public decimal OldPkg { get; set; }
        public decimal OldGrossWeight { get; set; }
        public string AmndmntInvoiceNo { get; set; }
        public string AmndmntInvoiceDate { get; set; }

    }
}