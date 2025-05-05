using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Bond.Models
{
    public class BondSacDetails
    {
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string GSTNo { get; set; }
        public int DepositAppId { get; set; }
        public string DepositNo { get; set; }
        public string SacDate { get; set; }
        public string ValidUpto { get; set; }
        public string ShippingLineName { get; set; }
        public string CHAName { get; set; }
        public decimal AreaReserved { get; set; }
        public decimal ReservedArea { get; set; }
        public decimal Weight { get; set; } = 0M;
        public decimal CIFValue { get; set; } = 0M;
        public decimal Duty { get; set; } = 0M;
        public int Units { get; set; } = 0;
        public string Size { get; set; } = string.Empty;
        public int IsInsured { get; set; } = 0;
        public string DepositDate { get; set; } = string.Empty;
        public string BondBOENo { get; set; } = string.Empty;
        public string BondBOEDate { get; set; } = string.Empty;
        public string DeliveryDate { get; set; } = string.Empty;
        public string UnloadedDate { get; set; } = string.Empty;
        public decimal UnloadAreaOccupied { get; set; }
        public string LastSacDated { get; set; }
        public string SpaceappId { get; set; }
        public int BillToParty { get; set; } = 0;
        public int CHAId { get; set; } = 0;        
    }
    
}