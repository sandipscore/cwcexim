using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_SBQuery
    {
        public string SBNO { get; set; }
        public int  Id { get; set; }
        public string Date { get; set; }
        public string PortOFLoad { get; set; }

        public string PortOFDischarge { get; set; }
        public string Comodity { get; set; }
        public string CHA { get; set; }
        public int Cargotype { get; set; }
        public int Package { get; set; }
        public int Vehicle { get; set; }
        public decimal Weight { get; set; }
        public decimal FOB { get; set; }
        public string ShippingLine { get; set; }

        public string Exporter { get; set; }
        public string GateinNo { get; set; }
        public string Country { get; set; }
        public IList<WFLD_SBQuery> CcinSBQList { get; set; }= new List<WFLD_SBQuery>();
        public IList<WFLD_CartingFORSB> CartingSBQList { get; set; } = new List<WFLD_CartingFORSB>();
        public IList<WFLD_DeliveryFORSBQuery> DeliverySBQList { get; set; } = new List<WFLD_DeliveryFORSBQuery>();
        public IList<WFLD_BTTFORSBQuery> BTTSBQList { get; set; } = new List<WFLD_BTTFORSBQuery>();
        public IList<WFLD_StockSBQuery> StockSBQList { get; set; } = new List<WFLD_StockSBQuery>();
        public IList<WFLD_DeliveryFORSBQuery> Pallatisation { get; set; } = new List<WFLD_DeliveryFORSBQuery>();

        public string SBNODate { get; set; }

        public string InvoiceNo { get; set; }
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
    public class WFLD_CartingFORSB
    {
      public  string CartingRegisterNo { get; set; }
        public string Date { get; set; }
        public string Godown { get; set; }
        public string Remarks { get; set; }

        public string Location { get; set; }
        public int NOOfPackages { get; set; }
        public decimal UnReserveCBM { get; set; }
        public decimal ReserveCBM { get; set; }
        public int excessbalancecargo { get; set; }
        public string ReceiptNo { get; set; }
        public string InvoiceNo { get; set; }
    }


    public class WFLD_DeliveryFORSBQuery
    {

        public string InvoiceNO { get; set; }
        public string Date { get; set; }
        public int  InvoiceId { get; set; }
        public string Exporter { get; set; }

        public string CHA { get; set; }
        public int NOOfPackages { get; set; }
        public int NOOfPallet { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public string StuReqNo { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string CfsCode { get; set; }
        public string Forwarder { get; set; }
        public string IwbNo { get; set; }
        public string Transporter { get; set; }
        public string SealNo { get; set; }
        public string SealDate { get; set; }
        public string PortOfLoading { get; set; }
        public string VehicleNo { get; set; }
        public string OutDate { get; set; }
    }

    public class WFLD_BTTFORSBQuery
    {

        public string InvoiceNO { get; set; }
        public string Date { get; set; }
        public int InvoiceId { get; set; }
        public string Exporter { get; set; }

        public string CHA { get; set; }
        public int NOOfPackages { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }

    }

    public class WFLD_StockSBQuery
    {

     
        public decimal SQM { get; set; }
      
        public int NOOfPackages { get; set; }
        public decimal Weight { get; set; }
       

    }
}