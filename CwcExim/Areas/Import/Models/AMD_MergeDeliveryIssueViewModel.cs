using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class AMD_MergeDeliveryIssueViewModel
    {
        public int Movement { get; set; }

        public int? hdMovement { get; set; }
        public decimal Distance { get; set; }      
        public bool ConvertToBond { get; set; }
        public bool ConvertToBondFlag { get; set; }
        public VRN_DeliverApplication DeliApp { get; set; }
        public VRN_IssueSlip IssueSlip { get; set; }
        public int Uid { get; set; }
        public int OldPartyId { get; set; }
        public int OldPayeeId { get; set; }

        public decimal? RoundUp { get; set; }
        public string SEZ { get; set; }
        public string ExportUnder { get; set; }
        public string CstmExam { get; set; }
        public decimal PortDistanc { get; set; }
    }

    public class AMD_mergedet
    {
        public int Id { get; set; }
        public string AppNo { get; set; }
    }

    public class AMD_deliverydet
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public string GSTNo { get; set; }
        public string CFSCode { get; set; }
        public string BOENo { get; set; }
        public string LineNo { get; set; }
        public string Address { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public int CargoType { get; set; }
        public string DeliveryAppDate { get; set; }


    }
}
