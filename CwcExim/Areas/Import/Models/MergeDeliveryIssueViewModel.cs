using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class MergeDeliveryIssueViewModel
    {
        public PPG_DeliverApplication DeliApp { get; set; }
        public PPG_Issueslip IssueSlip { get; set; }
        public int Uid { get; set; }
        public int OldPartyId { get; set; }
        public int OldPayeeId { get; set; }
    }

    public class ppgmergedet
    {
       public int Id { get; set; }
        public string AppNo { get; set; }
    }


    public class ppgdeliverydet
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