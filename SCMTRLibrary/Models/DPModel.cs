using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMTRLibrary.Models
{
   public class DPModel
    {
        public headerField headerField { get; set; } = new headerField();

        public DPDetails master { get; set; } = new DPDetails();
    }
}
