using CwcExim.Areas.Import.Models;
using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class DSRInvoiceRCTSealing : DSRInvoiceBase
    {
        
        //--------------------------------------------------------------------------------------------------------------------
        public List<DSRPreInvoiceContainerRCTSealing> lstPrePaymentCont { get; set; } = new List<DSRPreInvoiceContainerRCTSealing>();
        public List<DSRPostPaymentContainerRCTSealing> lstPostPaymentCont { get; set; } = new List<DSRPostPaymentContainerRCTSealing>();
        public List<DSRPreInvoiceOBLRCTSealing> lstPrePaymentOBL { get; set; } = new List<DSRPreInvoiceOBLRCTSealing>();
        public List<DSRPostInvoiceOBLRCTSealing> lstPostPaymentOBL { get; set; } = new List<DSRPostInvoiceOBLRCTSealing>();
        public List<DSRPostPaymentChrgRCTSealing> lstPostPaymentChrg { get; set; } = new List<DSRPostPaymentChrgRCTSealing>();
        //public IList<DSRContainerWiseAmountRCTSealing> lstContWiseAmount { get; set; } = new List<DSRContainerWiseAmountRCTSealing>();
        //public List<DSROperationCFSCodeWiseAmountRCTSealing> lstOperationCFSCodeWiseAmount { get; set; } = new List<DSROperationCFSCodeWiseAmountRCTSealing>();
        //public List<DSRPreInvoiceCargoRCTSealing> lstPreInvoiceCargo { get; set; } = new List<DSRPreInvoiceCargoRCTSealing>();
        public List<DSRRCTSealingPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<DSRRCTSealingPostPaymentChargebreakupdate>();
        public IList<string> ActualApplicable { get; set; } = new List<string>();

        public int CHAId { get; set; }
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }
        public int ExporterId { get; set; }
        public string ExporterName { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
    }
    public class DSRPostPaymentContainerRCTSealing //: PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public int PortId { get; set; }
        public string PortName { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string ForeignLine { get; set; }
    }
    public class DSRPostPaymentChrgRCTSealing : PostPaymentCharge
    {
        //public int OperationId { get; set; }
    }

    public class DSRContainerWiseAmountRCTSealing : ContainerWiseAmount
    {

    }

    public class DSRPreInvoiceContainerRCTSealing //: PreInvoiceContainer
    {
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string ForeignLine { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public int PortId { get; set; }
        public string PortName { get; set; }
    }
    public class DSRPreInvoiceOBLRCTSealing 
    {
        public string OBLNo { get; set; }
        public string OBLDate { get; set; }
        public int NoOfPkg { get; set; }
        public decimal GrossWT { get; set; }
        public decimal FOBValue { get; set; }
    }

    public class DSRPostInvoiceOBLRCTSealing
    {
        public string OBLNo { get; set; }
        public string OBLDate { get; set; }
        public int NoOfPkg { get; set; }
        public decimal GrossWT { get; set; }
        public decimal FOBValue { get; set; }
    }

    public class DSROperationCFSCodeWiseAmountRCTSealing
    {
        public int InvoiceId { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public int OperationId { get; set; }
        public string ChargeType { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string Clause { get; set; }
    }

    public class DSRPreInvoiceCargoRCTSealing : PreInvoiceCargo {
    }



    public class DSRRCTSealingPostPaymentChargebreakupdate
    {
        public int ChargeId { get; set; } = 0;
        public string Clause { get; set; } = string.Empty;
        public int ClauseOrder { get; set; } = 0;
        public string ChargeName { get; set; } = string.Empty;
        public string ChargeType { get; set; } = string.Empty;
        public string SACCode { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public decimal Rate { get; set; } = 0M;
        public decimal Amount { get; set; } = 0M;

        public int OperationId { get; set; }
        public string CFSCode { get; set; }
        public string Startdate { get; set; }
        public string EndDate { get; set; }
    }

}