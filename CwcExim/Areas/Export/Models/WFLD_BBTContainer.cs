using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class WFLD_BBTContainer : WFLD_InvoiceBase
    {
        public int InvoiceID { get; set; }        
        public string GSTNo { get; set; }
        public string City { get; set; }

        public List<WFLD_BTTContainerDetails> ContainerDetails { get; set; } = new List<WFLD_BTTContainerDetails>();
        public string ContainerDetailsXml { get; set; }
        public int hasSDAvailableBalance { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
        public List<WFLDPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<WFLDPreInvoiceContainer>();
        public IList<WFLD_ExpContainer> lstPSCont { get; set; } = new List<WFLD_ExpContainer>();
        public List<WFLDPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<WFLDPostPaymentContainer>();
        public List<WFLDPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<WFLDPostPaymentChrg>();
        public IList<WFLDContainerWiseAmount> lstContWiseAmount { get; set; } = new List<WFLDContainerWiseAmount>();
        public List<WFLDCMPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<WFLDCMPostPaymentChargebreakupdate>();
        public List<WFLDOperationCFSCodeWiseAmount_BTT> lstOperationCFSCodeWiseAmount { get; set; } = new List<WFLDOperationCFSCodeWiseAmount_BTT>();
        public IList<WFLD_ExpOperationContWiseCharge> lstOperationContwiseAmt { get; set; } = new List<WFLD_ExpOperationContWiseCharge>();
        public IList<WFLD_ExpContWiseAmount> lstContwiseAmt { get; set; } = new List<WFLD_ExpContWiseAmount>();
        public IList<string> ActualApplicable { get; set; } = new List<string>();

    }


    public class WFLD_BTTContainerDetails
    {
        public string ContrainerNo { get; set; }
        public int Size { get; set; }
        public string HazNonHaz { get; set; }
        public string SbNo { get; set; }
        public string SbDate { get; set; }
        public string CommodityName { get; set; }
        public string CFSCode { get; set; }

        public int isLoadedContrainer { get; set; }

        public string EntryDate { get; set; }
        public string IsOdc { get; set; }

        public decimal Weight { get; set; }

        public int ContainerStuffingId { get; set; }

        public int NoofPkg { get; set; }
        public int ShippingLineId { get; set; }
        public string ChargesFor { get; set; }
    }


    public class WFLD_BBTContainerPartyDetails
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyCode { get; set; }
        public string Address { get; set; }
        public string GST { get; set; }
        public string CityName { get; set; }

        public string StateName { get; set; }
        public string StateCode { get; set; }
    }

    public class WFLD_BTTContainerInvoiceList
    {
        public int InvoiceId { get; set; }
        public string Module { get; set; }
        public string ContainerNo { get; set; }
        public string InvoiceDate { get; set; }

        public string InvoiceNo { get; set; }
    }
    public class WFLDOperationCFSCodeWiseAmount_BTT
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
        public string Clause { get; set; }
    }





}