using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Dnd_GateSummaryReport
    {
       public string mstcompany { get; set; }
            public string GodownName { get; set; }
            public int GodownId { get; set; }
            public List<Dnd_TotalEmptyIn> LstEmptyIn { get; set; } = new List<Dnd_TotalEmptyIn>();
        public List<Dnd_TotalEmptyOut> LstEmptyOut { get; set; } = new List<Dnd_TotalEmptyOut>();
        public List<Dnd_ShutOut> LstShutOut { get; set; } = new List<Dnd_ShutOut>();
        public List<Dnd_Hub> LstHub { get; set; } = new List<Dnd_Hub>();
        public List<Dnd_Others> LstOthers { get; set; } = new List<Dnd_Others>();
        public List<Dnd_Htc> LstHtc { get; set; } = new List<Dnd_Htc>();
        public List<Dnd_Pvt> LstPvt { get; set; } = new List<Dnd_Pvt>();
        public List<Dnd_CargoIn> LstCargoIn { get; set; } = new List<Dnd_CargoIn>();
        public List<Dnd_CargoOut> LstCargoOut { get; set; } = new List<Dnd_CargoOut>();
        public List<Dnd_SBDetails> LstSB { get; set; } = new List<Dnd_SBDetails>();
    }
    public class Dnd_TotalEmptyIn
    {

        public string EmptyIn20 { get; set; }
        public string EmptyIn40 { get; set; }
        public string EmptyIn45 { get; set; }
        public string EmptyInTeus { get; set; }
        public string EmptyInTotal { get; set; }
      

    }
    public class Dnd_TotalEmptyOut
    {

      
        public string EmptyOutTotal { get; set; }
        public string EmptyOut20 { get; set; }
        public string EmptyOut40 { get; set; }
        public string EmptyOut45 { get; set; }
        public string EmptyOutTeus { get; set; }

    }
    public class Dnd_ShutOut
    {


        public string ShutOutTotal { get; set; }
        public string ShutOut20 { get; set; }
        public string ShutOut40 { get; set; }
        public string ShutOut45 { get; set; }
        public string ShutOutTeus { get; set; }

    }
    public class Dnd_Hub
    {


        public string HubTotal { get; set; }
        public string Hub20 { get; set; }
        public string Hub40 { get; set; }
        public string Hub45 { get; set; }
        public string HubTeus { get; set; }

    }
    public class Dnd_Others
    {


        public string OthersTotal { get; set; }
        public string Others20 { get; set; }
        public string Others40 { get; set; }
        public string Others45 { get; set; }
        public string OthersTeus { get; set; }

    }
    public class Dnd_Htc
    {


        public string HtcTotal { get; set; }
        public string Htc20 { get; set; }
        public string Htc40 { get; set; }
        public string Htc45 { get; set; }
        public string HtcTeus { get; set; }

    }
    public class Dnd_Pvt
    {


        public string PvtTotal { get; set; }
        public string Pvt20 { get; set; }
        public string Pvt40 { get; set; }
        public string Pvt45 { get; set; }
        public string PvtTeus { get; set; }

    }
    public class Dnd_CargoIn
    {


        public string CargoInTotal { get; set; }
        public string CargoIn20 { get; set; }
        public string CargoIn40 { get; set; }
        public string CargoIn45 { get; set; }
        public string CargoInTeus { get; set; }

    }
    public class Dnd_CargoOut
    {


        public string CargoOutTotal { get; set; }
        public string CargoOut20 { get; set; }
        public string CargoOut40 { get; set; }
        public string CargoOut45 { get; set; }
        public string CargoOutTeus { get; set; }

    }
    public class Dnd_SBDetails
    {


        public string TotalSB { get; set; }
        public string TotalPkg { get; set; }
        public string TotalWeight { get; set; }
        public string TotalFOB{ get; set; }
      

    }

}