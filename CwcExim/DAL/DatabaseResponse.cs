using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwcExim.DAL
{
   public class DatabaseResponse
    {
       public int Status { get; set; }
       public string Message { get; set; }
       public Object Data { get; set; }
    }
}
