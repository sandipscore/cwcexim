using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class DECSignedModel
    {
        public string InJsonFile { get; set; }
        public string OUTJsonFile { get; set; }
        public string ArchiveInJsonFile { get; set; }
        public string ArchiveInJsonFilePath { get; set; }
        public string DSCPATH { get; set; }
        public string DSCPASSWORD { get; set; }

    }
}