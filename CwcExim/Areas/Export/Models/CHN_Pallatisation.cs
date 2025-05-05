using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class CHN_Pallatisation
    {
        public int Id { get; set; }

        public int CartingRegisterId { get; set; }

        public string SBNo { get; set; }
        public string SBDate { get; set; }
        public int GodownNewID { get; set; }

        public string GodownNewName { get; set; }

        public string LocationId { get; set; }
        public string LocationName { get; set; }

        public int PartyID { get; set; }

        public string PartyName { get; set; }

        public int PayeeID { get; set; }

        public string PayeeName { get; set; }

        public string NoOfPallet { get; set; }

        public string PkgType { get; set; }

        public List<CHN_PallatisationSBDetails> PallatisationSBDetails { get; set; }
        public List<CHN_PallatisationSBDetails> PallatisationNewSBDetails { get; set; }

        public List<CHN_PallasationChargeBase> PallatisationChargeDetails { get; set; }

        public string PallatisationCharge { get; set; }
        public string PalletSBDetails { get; set; }
        public string PalletSBNewDetails { get; set; }

        public string Invoice { get; set; }
        public string InvoiceID { get; set; }

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

    }


    public class CHN_PallatisationSBDetails
    {
        public int GodownID { get; set; }
        public string GodownName { get; set; }
        public string SBNo { get; set; }
        public string SBDate { get; set; }

        public int Qty { get; set; }
        public decimal Weight { get; set; }
        public decimal SQM { get; set; }
        public decimal FOB { get; set; }

        public decimal Rate { get; set; }
        public string LocationName { get; set; }
        public string LocationId { get; set; }


        public int PartyId { get; set; }

        public string ComodityId { get; set; }

        public string Item { get; set; }
        public int RefID { get; set; }
        public decimal Vol { get; set; }

        public string PartyName { get; set; }
        public int ShippingLineId { get; set; }

        public string PkgType { get; set; }
        //public decimal ReserveCBM { get; set; }
        //public decimal GeneralCBM { get; set; }
    }

    public class CHN_PallasationChargeBase
    {
        public int ChargeId { get; set; }
        public string OperationId { get; set; }
        public string Clause { get; set; }
        public string ChargeType { get; set; }
        public string ChargeName { get; set; }
        public string SACCode { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal Taxable { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTPer { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal Total { get; set; }
    }
    public class CHN_PostPalletizationCBM
    {
        public string UpdateDate { get; set; }
        public int PalletizationId { get; set; }
        public int CartingdtlId { get; set; }
        public string SBNo { get; set; }
        public string SBDate { get; set; }
        public int Qty { get; set; }
        public decimal ReservedCBM { get; set; }
        public decimal GeneralCBM { get; set; }
        public int StockId { get; set; }
    }
}