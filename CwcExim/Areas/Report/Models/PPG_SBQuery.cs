using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class PPG_SBQuery
    {
        public string SBNO { get; set; }
        public int Id { get; set; }
        public string Date { get; set; }
        public string PortOFLoad { get; set; }

        public string PortOFDischarge { get; set; }
        public string Comodity { get; set; }
        public string CHA { get; set; }
        public int Cargotype { get; set; }
        public int Package { get; set; }
        public string Vehicle { get; set; }
        public decimal Weight { get; set; }
        public decimal FOB { get; set; }
        public string ShippingLine { get; set; }

        public string Exporter { get; set; }
        public string GateinNo { get; set; }
        public string Country { get; set; }
        public IList<PPG_SBQuery> CcinSBQList { get; set; } = new List<PPG_SBQuery>();
        public IList<PPG_CartingFORSB> CartingSBQList { get; set; } = new List<PPG_CartingFORSB>();
        public IList<PPG_DeliveryFORSBQuery> DeliverySBQList { get; set; } = new List<PPG_DeliveryFORSBQuery>();
        public IList<PPG_BTTFORSBQuery> BTTSBQList { get; set; } = new List<PPG_BTTFORSBQuery>();
        public IList<PPG_StockSBQuery> StockSBQList { get; set; } = new List<PPG_StockSBQuery>();
        public IList<PPG_DeliveryFORSBQuery> Pallatisation { get; set; } = new List<PPG_DeliveryFORSBQuery>();

        public string SBNODate { get; set; }
        public string PCIN { get; set; }
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
    public class PPG_CartingFORSB
    {
        public string CartingRegisterNo { get; set; }
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


    public class PPG_DeliveryFORSBQuery
    {

        public string InvoiceNO { get; set; }
        public string Date { get; set; }
        public int InvoiceId { get; set; }
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

    public class PPG_BTTFORSBQuery
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

    public class PPG_StockSBQuery
    {


        public decimal SQM { get; set; }

        public int NOOfPackages { get; set; }
        public decimal Weight { get; set; }


    }
}