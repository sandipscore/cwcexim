using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DND_SBQuery
    {
        public string SBNO { get; set; }
        public int  Id { get; set; }
        public string ShippingBillDate { get; set; }
        public string ShippingLine { get; set; }
        public string CFSCode  { get; set; }
        public string EntryDateTime { get; set; }
        public string CHA { get; set; }
        public string PortOFLoad { get; set; }
        public string PortOFDischarge { get; set; }
        public string Godown { get; set; }
        public string Package { get; set; }
        public string Weight { get; set; }
        public string FOB { get; set; }
        public string Comodity { get; set; }       
        public string Cargotype { get; set; }
        public string BAL { get; set; }
        public IList<DND_SBQuery> CcinSBQList { get; set; }= new List<DND_SBQuery>();
        public IList<DND_CartingFORSB> CartingSBQList { get; set; } = new List<DND_CartingFORSB>();
        public IList<DND_DeliveryFORSBQuery> DeliverySBQList { get; set; } = new List<DND_DeliveryFORSBQuery>();


    }
    //public class WFLD_CCINFORSB
    //{
    //    public string SBNO { get; set; }
    //    public string Id { get; set; }
    //    public string Date { get; set; }
    //    public string PortOFLoad { get; set; }

    //    public string PortOFDischarge { get; set; }
    //    public string Comodity { get; set; }
    //    public int  Cargotype { get; set; }
    //    public string ShippingLine  { get; set; }
    //}
    public class DND_CartingFORSB
    {
      public  string CartingRegisterNo { get; set; }
        public string Date { get; set; }
        public string Godown { get; set; }
       
        public string Location { get; set; }
        public int NOOfPackages { get; set; }
    }


    public class DND_DeliveryFORSBQuery
    {

        public string InvoiceNO { get; set; }
        public string Date { get; set; }
        public int  InvoiceId { get; set; }
        public string Exporter { get; set; }

        public string CHA { get; set; }
        public int NOOfPackages { get; set; }
    }
}