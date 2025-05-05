using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Icegate.Models
{
    public class WfldIcegateManualI02ViewModel
    {
        public int ID { get; set; }
        public string MessageType { get; set; } = "F";
        public string ModeOfTransport { get; set; }
        public string CustomHouseCode { get; set; } = "INPPG6";
        public string Train_VehicleNo { get; set; }
        public string Train_VehicleDate { get; set; }
        public string WagonNo { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string ShippingLineCode { get; set; }
        public decimal Weight { get; set; }
        public string PortCodeOrigin { get; set; }
        public string DestRailStationCode { get; set; }
        public string GatewayPortCode { get; set; }
        public string CarrierAgencyCode { get; set; }
        public string BondNo { get; set; }
        public string ISOCode { get; set; }
        public string ContainerStatus { get; set; }
        public int EntryId { get; set; }
        public string FileName { get; set; }
        public int FileCode { get; set; }
        public int CBT { get; set; }
        public int SerialNo { get; set; }
        public string SendOn { get; set; }
        public List<WfldSMTPDet> lstSMTP { get; set; } = new List<WfldSMTPDet>();
        
    }
    public class WfldSMTPDet
    {
        public string SMTPNo { get; set; }
        public string SMTPDate { get; set; }
        public string OriginPortCode { get; set; }
    }
}