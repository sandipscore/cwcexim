using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class VLDA_InvoiceList
    {
        public string invoiceno { get; set; }
        public string invoicenoo { get; set; }
        public string invoicenooo { get; set; }
        public string MovementNo { get; set; }

    }

    public class VLDA_ExpInvoiceList
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
    }
    public class VLDA_ExpInvoiceDetails
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
        public int TransporterId { get; set; }
        public string TransporterName { get; set; }
        public int PortId { get; set; }
        public string PortName { get; set; }
        public int CountryId { get; set; }
        public string Countryname { get; set; }
        public int OnWheel { get; set; }
        public int Shifting { get; set; }
        public string CustomSealNo { get; set; }
        public string LinerSeal { get; set; }
        public string VehicleNumber { get; set; }
        public string Remarks { get; set; }
    }
}