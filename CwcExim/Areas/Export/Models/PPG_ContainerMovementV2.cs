using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class PPG_ContainerMovementV2
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

        [Required(ErrorMessage = "Fill Out This Field")]
        public string Party { get; set; }
        public int partyId { get; set; }

        public string PayeeID { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PayeeName { get; set; }

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
        public IList<PPG_ShippingBillV2> ShipBill { get; set; } = new List<PPG_ShippingBillV2>();

        public decimal TareWeight { get; set; }
        public decimal CargoWeight { get; set; }

        public int Cargo { get; set; }
        public decimal ElwbCargoWeight { get; set; }
        public decimal ElwbTareWeight { get; set; }
    }
}