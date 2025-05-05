using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Wfld_ReportEngine
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ReportName { get; set; }
        public string MethodName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DownloadFilePath { get; set; }

        public string FileName { get; set; }
    }
}