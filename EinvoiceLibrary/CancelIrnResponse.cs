using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinvoiceLibrary
{
    public class CancelIrnResponse
    {
        public int Status { get; set; }
        public string Irn { get; set; }
        public string CancelDate { get; set; }
        public ErrorDetails ErrorDetails { get; set; }
    }
}
