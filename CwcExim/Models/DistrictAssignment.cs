using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class DistrictAssignment
    {
        public int AssignmentId { get; set; }
        public int DistrictId { get; set; }
        public int Uid { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public List<UserForDistAssignment> UserList { get; set; }
        public List<DistrictForAssignment> DistrictList { get; set; }
    }
}