using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMTRLibrary.Models
{

    public class supportingDocuments
    {
        public string messageType { get; set; }
        public Int32 equipmentSerialNumber { get; set; }
        public Int32 documentSerialNumber { get; set; }
        public string icegateUserID { get; set; }
        public Int64 IRNNumber { get; set; }
        public string documentReferenceNumber { get; set; }
        public string documentTypeCode { get; set; }
        public string beneficiaryCode { get; set; }


    }
}
