using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class ChnExportExcel
    {
       

        //  public IList<CHN_PVShippingLine> lstShpDtl { get; set; } = new List<CHN_PVShippingLine>();
        public IList<CHN_ExportPVDet> lstExppvdtl { get; set; } = new List<CHN_ExportPVDet>();
    }
    public class CHN_PVShippingLine
    {

        public string EximTraderName { get; set; }
        public string EximTraderAlias { get; set; }
        public int ShippingLineId { get; set; }
    }
    
 
        public class  CHN_PVShippingLineExcel
        {
             public int SlNo { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string EntryNo { get; set; }
        public string RegisterDate { get; set; }
        public decimal Units { get; set; }
        public decimal Weight { get; set; }
        public decimal Fob { get; set; }
        public string LocationName { get; set; }
        public decimal Area { get; set; }
        public int ShippingLineId { get; set; }
        public string EximtraderName { get; set; }
    }
    
    public class CHN_ExportPVDet
        {
        public int SlNo { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string EntryNo { get; set; }
        public string RegisterDate { get; set; }
        public decimal Units { get; set; }
        public decimal Weight { get; set; }
        public decimal Fob { get; set; }
        public string LocationName { get; set; }
       public decimal Area { get; set; }
        public int ShippingLineId { get; set; }
        }
    public class CHN_ExportPVDetExcel
    {
        public int SlNo { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string EntryNo { get; set; }
        public string RegisterDate { get; set; }
        public decimal Units { get; set; }
        public decimal Weight { get; set; }
        public decimal Fob { get; set; }
        public string LocationName { get; set; }
        public decimal Area { get; set; }
        public int ShippingLineId { get; set; }
    }


}
