using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class LoginAuditTrail
    {
        public int TrailId { get; set; }
        public string LoginId { get; set; }
        public string LoginTime { get; set; }
        public string LogOutTime { get; set; }
        public string SessionId { get; set; }
        public int Status { get; set; }
        public bool Offline { get; set; }
        public string IPAddress { get; set; }
    }
}