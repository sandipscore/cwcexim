using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class CHN_GateEntryPrint
    {
        public string CompanyAdrress { get; set; }
        public string CompanyMail { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyLocation { get; set; }
        public string GateInNo { get; set; }
        public string EntryDate { get; set; }
        public string EntryTime { get; set; }
        public string ShippingLine { get; set; }
        public string CHAName { get; set; }
        public string ContainerNo { get; set; }
        public string CustomSealNo { get; set; }
        public string ShippingLineSealNo { get; set; }
        public string VehicleNo { get; set; }
        public string CargoDescription { get; set; }
        public string CargoType { get; set; }
        public string NoOfPackages { get; set; }
        public string GrossWeight { get; set; }
        public string ContainerLoadType { get; set; }
        public string ContainerType { get; set; }

        public string OperationType { get; set; }


        public string DisplayCfs { get; set; }

        public string Remarks { get; set; }
        public string Size { get; set; }

        public string ImporterName { get; set; }

        public string TerminalDate { get; set; }

        public string TerminalTime { get; set; }
    }
}