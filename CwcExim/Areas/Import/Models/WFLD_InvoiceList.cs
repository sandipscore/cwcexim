using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class WFLD_InvoiceList
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
    }
    public class WFLD_InvoiceDetails
    {
        public string InvoiceDate { get; set; }
        public string DeliveryDate { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public int PayeeId { get; set; }
        public string PayeeName { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public int StuffingReqId { get; set; }
        public string StuffingReqNo { get; set; }
        public string StuffingReqDate { get; set; }
        public int NoOfVehicle { get; set; }
        public int Distance { get; set; }
        public int OTHours { get; set; }
        public int SEZ { get; set; }
        public string InvoiceType { get; set; }
        public int IsInsured { get; set; }
        public int Billtoparty { get; set; }
        public int IsDirect { get; set; }
        public int IsBond { get; set; }
        public int IsTransporter { get; set; }
        public int DeliveryType { get; set; }
        public int VehicleNoUn { get; set; }
        public int Cargo { get; set; }
    }
}