using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class DSR_RefundMoneyFromPD: RefundMoneyFromPD
    {
        public decimal UnPaidAmount { get; set; }
        public string PName { get; set; }
        public string PartyAddress { get; set; }
    }


    public class DSR_AddMoneyToPDModelRefund : AddMoneyToPDModelRefund
    {
        public decimal UnPaidAmount { get; set; }
        public string PName { get; set; }

        public string PartyAddress { get; set; }

        public string Remarks { get; set; }

    }

    public class DSR_SDRefundList
    {
        public int PDAACId { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string ClosureDate { get; set; }
        public string RecieptNo { get; set; }
        public decimal RefundAmt { get; set; }
    }

    public class DSR_ViewSDRefund
    {
        public int PDAACId { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyAddress { get; set; }
        public string ClosureDate { get; set; }
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