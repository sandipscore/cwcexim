using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class LoginAttempt
    {
        public string LoginId { get; set; }
        public string SessionId { get; set; }
        public int FailureAttemptCount { get; set; }
    }
}