using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class WFLDRefundMoneyFromPD: RefundMoneyFromPD
    {
        public decimal UnPaidAmount { get; set; }
        public string PName { get; set; }

        public string PartyAddress { get; set; }
    }


    public class WFLDAddMoneyToPDModelRefund: AddMoneyToPDModelRefund
    {
        public decimal UnPaidAmount { get; set; }
        public string PName { get; set; }

        public string PartyAddress { get; set; }

        public string Remarks { get; set; }
        public bool IsCloser { get; set; } = true;
        public string CloserDate { get; set; }

    }

    public class WFLDSDRefundList
    {
        public int PDAACId { get; set; }
        public int SDCloserId { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string ClosureDate { get; set; }
        public string RecieptNo { get; set; }
        public decimal RefundAmt { get; set; }
    }

    //public class ViewSDRefund
    //{
    //    public int PDAACId { get; set; }
    //    public int PartyId { get; set; }
    //    public string PartyName { get; set; }
    //    public string PartyAddress { get; set; }
    //    public string ClosureDate { get; set; }
    //    public string RecieptNo { get; set; }
    //    public decimal RefundAmt { get; set; }
    //    public decimal OpeningAmt { get; set; }
    //    public decimal ClosingAmt { get; set; }
    //    public string ChqDate { get; set; }
    //    public string ChqNo { get; set; }
    //    public string BankName { get; set; }
    //    public string Branch { get; set; }
    //    public string Remarks { get; set; }
    //}


    public class WFLDViewSDRefund
    {
        public int PDAACId { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyAddress { get; set; }
        public string ClosureDate { get; set; }
        public string ISClosure { get; set; } = string.Empty;
        public string RecieptNo { get; set; }
        public decimal RefundAmt { get; set; }
        public decimal OpeningAmt { get; set; }
        public decimal ClosingAmt { get; set; }
        public string ChqDate { get; set; }
        public string ChqNo { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string Remarks { get; set; }
    }

}