using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class AccountLock
    {
        public int LockId { get; set; }
        public string SessionId { get; set; }
        public string IPAddress { get; set; }
        public string LoginId { get; set; }
        public int LockFrom { get; set; }
        public DateTime LockTill { get; set; }
        public int LockMinutes { get; set; }

    }
}