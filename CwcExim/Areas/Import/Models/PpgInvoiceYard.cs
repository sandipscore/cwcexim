using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class PpgInvoiceYard :PpgInvoiceBase
    {        

        //--------------------------------------------------------------------------------------------------------------------
        public List<PpgPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<PpgPreInvoiceContainer>();
        public List<PpgPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<PpgPostPaymentContainer>();
        public List<PpgPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<PpgPostPaymentChrg>();
        public IList<PpgContainerWiseAmount> lstContWiseAmount { get; set; } = new List<PpgContainerWiseAmount>();
        public List<ppgPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<ppgPostPaymentChargebreakupdate>();

        public List<PpgOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<PpgOperationCFSCodeWiseAmount>();

        public List<CwcExim.Areas.CashManagement.Models.Charge> lstCharge { get; set; } = new List<CwcExim.Areas.CashManagement.Models.Charge>();

        
        //--------------------------------------------------------------------------------------------------------------------

public string PaymentMode { get; set; }

        public string PlaceOfSupply { get; set; }

        public int FromShippingId { get; set; }
        public int ToShippingId { get; set; }

        public string FromShippingLine { get; set; }
        public string ToShippingLine { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public int ContainerClassId { get; set; }
        public string NDays { get; set; }

        public string FumigationType { get; set; }

        public int ChemicalId { get; set; }
        public string Chemical_Name { get; set; }

        public decimal Area { get; set; }
        public string GateInDate { get; set; }
        public string MonthValue { get; set; }
        public string YearValue { get; set; }
        public decimal GF { get; set; }
        public decimal MF { get; set; }
        public decimal TotalSpace { get; set; }
        public string GodownName { get; set; }
    }
    

    public class PpgPostPaymentContainer: PostPaymentContainer
    {

        public string OBLNo { get; set; }
        public string BOLDate { get; set; }
        public string SealCutDate { get; set; }
        public string DeliveryDate { get; set; }
    }
    public class PpgPostPaymentChrg : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class PpgContainerWiseAmount: ContainerWiseAmount
    {

    }

    public class PpgPreInvoiceContainer: PreInvoiceContainer
    {

        public string OBLNo { get; set; }
        public string SealCutDate { get; set; }

    }

    public class PpgOperationCFSCodeWiseAmount
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
    }



    public class ppgPostPaymentChargebreakupdate
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