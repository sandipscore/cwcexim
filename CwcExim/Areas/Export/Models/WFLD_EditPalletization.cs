using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class WFLD_EditPalletization
    {
        public int Id { get; set; }
        public int CartingRegisterId { get; set; }
        public string SBNo { get; set; }
        public string SBDate { get; set; }
        public int GodownNewId { get; set; }
        public string GodownNewName { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public int PayeeId { get; set; }
        public string PayeeName { get; set; }
        public string NoOfPallet { get; set; }
        public string PkgType { get; set; }

        public List<WLFD_PallatisationSBDetails> PallatisationSBDetails { get; set; }
        public List<WLFD_PallatisationSBDetails> PallatisationNewSBDetails { get; set; }
        public List<WFLD_PallasationChargeBase> PallatisationChargeDetails { get; set; }
        public string PallatisationCharge { get; set; }
        public string PalletSBDetails { get; set; }
        public string PalletSBNewDetails { get; set; }

        public string InvoiceNo { get; set; }
        public int InvoiceId { get; set; }

        public decimal Total { get; set; }
        public decimal TotalTaxable { get; set; }

        public decimal TotalCGST { get; set; }
        public decimal TotalSGST { get; set; }

        public string Remarks { get; set; }
        public decimal TotalIGST { get; set; }

        public decimal ReserveCBM { get; set; }
        public decimal GeneralCBM { get; set; }
        public string SEZ { get; set; }
        public string InvoiceDate { get; set; }
        public string DeliveryDate { get; set; }
        public string GSTNo { get; set; }
        public string InvoiceType { get; set; }
    }
}

