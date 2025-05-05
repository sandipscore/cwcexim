using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace EinvoiceLibrary
{
   public class TokenResponse
    {
    public int Status { get; set; }
       public ErrorDetails ErrorDetails { get; set; }
        public JObject Data  { get; set; }
        public string AuthToken { get; set; }
        public string Sek { get; set; }

    }
}
