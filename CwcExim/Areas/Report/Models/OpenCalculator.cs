using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class OpenCalculator
    {
        public string OperationType { get; set; }

        [Required]
        public string EntryDate { get; set; }

        [Required]
        public string DeliveryDate { get; set; }

        public string CargoType { get; set; }
        public string Size { get; set; }

        public string CBM { get; set; }

        public string MovementType { get; set; }

        [Required]
        public string CIFValue { get; set; }
        [Required]
        public string Duty { get; set; }

        public string Carting { get; set; }
        public string CCIN { get; set; }

    }

    public class ChargesList
    {
        public string ChargeName { get; set; }
        public decimal Amount { get; set; }
    }
}