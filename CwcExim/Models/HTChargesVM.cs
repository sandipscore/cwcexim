using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class HTChargesVM:HTCharges
    {
        public IList<Contractor> LstContractor { get; set; } = new List<Contractor>();
        public IList<Operation> LstOperation { get; set; } = new List<Operation>();
    }
}