using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMTRLibrary.Models
{
   public class ATModel
    {

        public headerField headerField { get; set; } = new headerField();

        public ATMaster master { get; set; } = new ATMaster();
    }
}
