using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMTRLibrary.Models
{
    public class master
    {
        public declaration declaration { get; set; } = new declaration();
        public location location { get; set; } = new location();

        public List<cargoContainer> cargoContainer { get; set; } = new List<cargoContainer>();
    }
}
