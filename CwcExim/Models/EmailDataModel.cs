using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class EmailDataModel
    {
        public string ReceiverEmail { get; set; }
        public string Subject { get; set; }
        public string MailBody { get; set; }
    }
}