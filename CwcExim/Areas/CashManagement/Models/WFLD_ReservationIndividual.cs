using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.CashManagement.Models
{
    public class WFLD_ReservationIndividual
    {
        public string GstIn { get; set; }

        public int InvoiceId { get; set; } = 0;
        public string InvoiceType { get; set; } = string.Empty;
        public string InvoiceNo { get; set; } = string.Empty;

        public string DeliveryDate { get; set; } = string.Empty;
        public int PartyId { get; set; } = 0;
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PartyName { get; set; } = string.Empty;

        public string PartyGST { get; set; } = string.Empty;
        public string StateCode { get; set; } = string.Empty;
        public string GSTNo { get; set; } = string.Empty;
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GodownName { get; set; } = string.Empty;
        public int GodownId { get; set; }

        public string GodownType { get; set; } = string.Empty;

        
        public string OperationType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; } = string.Empty;
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; } = string.Empty;
        public int PayeeId { get; set; } = 0;
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PayeeName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal SQM { get; set; } = 0;



        public decimal CGST { get; set; } = 0;

        public decimal SGST { get; set; } = 0;
        public decimal IGST { get; set; } = 0;
        public decimal CGSTPER { get; set; } = 0;

        public decimal SGSTPER { get; set; } = 0;
        public decimal IGSTPER { get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public decimal Round_up { get; set; } = 0;
        public decimal InvoiceAmt { get; set; } = 0;
        public string Remarks { get; set; } = string.Empty;
        public string CargoType { get; set; }

        public bool Term { get; set; }

        public decimal TotalChrgAmount { get; set; } = 0;
        public IList<charges> ReservationCharges {get; set; }=new List<charges>();

        public string ReservationChargesXML { get; set; }

        public string SpaceType { get; set; }

        public string PlaceOfSupply { get; set; }
        public string SezValue { get; set; }
    }



    public class WFLD_Godown
    {
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public string OperationType { get; set; }
        

    }


    public class charges
    {


        public string chargeName { get; set; }
      public decimal Amount { get; set; }

        public string SacCode { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal CGST { get; set; }
        public decimal SGSTPer { get; set; }
        public decimal SGST { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal IGST { get; set; }
        public decimal Total { get; set; }


                               
    }
}