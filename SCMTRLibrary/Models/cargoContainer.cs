using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMTRLibrary.Models
{
    public class cargoContainer
    {
        public string messageType { get; set; }
        public Int32 equipmentSequenceNo { get; set; }
        public string equipmentID { get; set; }
        public string equipmentType { get; set; }
        public string equipmentSize { get; set; }
        public string equipmentLoadStatus { get; set; }
        public string finalDestinationLocation { get; set; }
        public string eventDate { get; set; }
        public string equipmentSealType { get; set; }
        public string equipmentSealNumber { get; set; }
        public string equipmentStatus { get; set; }
        //public string equipmentPkg { get; set; }
        public decimal equipmentQuantity { get; set; }
        // public string equipmentQUC { get; set; }
        public string EquipmentQUC { get; set; }
        public List<cargoDetails> cargoDetails { get; set; }

    }
}
