using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMTRLibrary.Models
{
   public class DTModel
    {

        public headerField headerField { get; set; } = new headerField();
        public DTDetailsModel master { get; set; } = new DTDetailsModel();
    }




}
