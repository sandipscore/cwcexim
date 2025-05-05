using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_ShipbillWiseStorageCharge
    {
        public string StuffingNo { get; set; }

        public string StuffingDate { get; set; }

        public string Forwarder { get; set; }

        public string ContainerNo { get; set; }

        public string Size { get; set; }

        public List<Hdb_ShipbillWiseStorageChargeDetails> lstChargeDetails { get; set; } = new List<Hdb_ShipbillWiseStorageChargeDetails>();
        public Hdb_ShipbillWiseFranchiseCharge FranchiseCharge { get; set; } = new Hdb_ShipbillWiseFranchiseCharge();
    }

    public class Hdb_ShipbillWiseStorageChargeDetails
    {
        public string SBNo { get; set; }
        public string SBDate { get; set; }
        public string CartingRegisterNo { get; set; }
        public string RegisterDate { get; set; }
        public string CargoDescription { get; set; }
        public string Exporter { get; set; }


        public decimal GrossWeight { get; set; }
        public decimal Fob { get; set; }
        public decimal Storage { get; set; }

        public decimal ExStorage { get; set; }
        public decimal Insurance { get; set; }
    }


    public class Hdb_ShipbillWiseFranchiseCharge
    {
        public decimal FranchaieseCharge { get; set; }
        public decimal Carting { get; set; }
    }
}