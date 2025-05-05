using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinvoiceLibrary
{
   public class IrnModel
    {
        public string SupplierGstNo { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentType { get; set; }//INV,DBN,CRN
        public string DocumentDate { get; set; }
        
    }

    


}
