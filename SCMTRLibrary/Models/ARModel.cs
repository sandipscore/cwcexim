using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMTRLibrary.Models
{
   public class ARModel
    {
        public headerField headerField { get; set; } = new headerField();

        public ARDetails master { get; set; } = new ARDetails();
    }
}
