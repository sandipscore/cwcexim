using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Kol_RakeLoadingDetails
    {
        public RakeAuthentication auth { get; set; }
        public RakeHeader rakehdr { get; set; }
        public List<RakeDetails> wgondtls { get; set; }
    }
   
    public class RakeAuthentication
    {
        public string org { get; set; }
        public string userid { get; set; }
        public string pswd { get; set; }
    }

    public class RakeHeader
    {
        public string sttnfrom { get; set; }
        public string sttnto { get; set; }
        public string invcid { get; set; }
        public string rakeid { get; set; }
        public string rakename { get; set; }
        public string oprgplcttime { get; set; }
        public string relstime { get; set; }
        public string noofwgon { get; set; }
    }

    public class RakeDetails
    {
        public string wgonid { get; set; }
	    public string sttnfrom { get; set; }
        public string sttnto { get; set; }
        public string contnumb { get; set; }
        public string contleflag { get; set; }
        public string contsize { get; set; }
        public string contposn { get; set; }
        public string cmdtload { get; set; }
        public string trfctype { get; set; }
        public string cmdtstatcode { get; set; }
        public string conttarewght { get; set; }
        public string contloadwght { get; set; }
        public string smtpno { get; set; }
        public string smtpdate { get; set; }
        public string hsncode { get; set; }
    }

}