using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Export.Models
{
    public class DSR_ContainerMovement
    {
        public int MovementId { get; set; }
        public string MovementNo { get; set; }
        public string MovementDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string Container { get; set; }
        public int LocationId { get; set; } 
        public string CFSCode { get; set; }
        public int ContainerStuffingDtlId { get; set; }
        public int ContainerStuffingId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string LocationName { get; set; }
        public string ShippingBillNo { get; set; }
        public int port;
        public IList<Charges_inv> Charges { get; set; } = new List<Charges_inv>();
        public decimal TotalCharges { get; set; } = 0;
        public string Party { get; set; }
        public int partyId { get; set; }

        public string GParty { get; set; }
        public int GpartyId { get; set; }

        public string RParty { get; set; }
        public int RpartyId { get; set; }

        public string Invoice { get; set; }
        public string Invoicee { get; set; }
        public string Invoiceee { get; set; }
        public string SealNo { get; set; }
        public string SealDate { get; set; }
        public string CustomSealNo { get; set; }
        public string CustomSealDate { get; set; }
        public string POD { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Sline { get; set; }
        public int sid { get; set; }
        public int TransportMode { get; set; }
        public IList<DSR_ShippingBill> ShipBill { get; set; } = new List<DSR_ShippingBill>();

        public decimal TareWeight { get; set; }
        public decimal CargoWeight { get; set; }

        public int Cargo { get; set; }
       

    }

    public class DSR_ShippingBill
    {
        public string shippingBillNo { get; set; }
        public decimal CargoWeight { get; set; }
        public int CargoType { get; set; }
    }
    public class DSR_GodownList
    {
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public string GodownNo { get; set; }
    }

    public class DSR_Charges_inv
    {
        // public int DBChargeID { get; set; }
        // public string ChargeId { get; set; }
        // public string ChargeType { get; set; }
        public string ChargeName { get; set; }
        public decimal Amount { get; set; } = 0;
        public decimal IGSTPer { get; set; } = 0;
        public decimal IGSTAmt { get; set; } = 0;
        public decimal CGSTPer { get; set; } = 0;
        public decimal CGSTAmt { get; set; } = 0;
        public decimal SGSTPer { get; set; } = 0;
        public decimal SGSTAmt { get; set; } = 0;
        public decimal Total { get; set; } = 0;
    }
}