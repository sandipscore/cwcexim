using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class ContainerStuffing
    {
        public int ContainerStuffingId { get; set; }
        public int StuffingReqId { get; set; }
        public string StuffingNo { get; set; }
        public string StuffingDate { get; set; }
        public string Remarks { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string StuffingReqNo { get; set; }
        public string RequestDate { get; set; }
        public int Uid { get; set; }
        public string StuffingXML { get; set; }
        public string Size { get; set; }
        public string GodownName { get; set; }
        public bool DirectStuffing { get; set; }

        public string Via { get; set; }
        public string ForeignLiner { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }

        public string SCMTRXML { get; set; }

        //------------------Invoice---------------------------
        public int GREPartyId { get; set; }
        [Display(Name = "Party Code")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public String GREPartyCode { get; set; }
        public int GREOperationId { get; set; }
        public String GREChargeType { get; set; }
        public String GREChargeName { get; set; }
        [Display(Name = "DSTF Charge")]
        public decimal GRECharge { get; set; }
        public decimal GRECGSTCharge { get; set; }
        public decimal GRESGSTCharge { get; set; }
        public decimal GREIGSTCharge { get; set; }
        public decimal GREIGSTPer { get; set; }
        public decimal GRECGSTPer { get; set; }
        public decimal GRESGSTPer { get; set; }
        public decimal GREAmount { get; set; }
        public decimal GRETaxable { get; set; }
        public String GRESACCode { get; set; }
        public decimal GRETotalAmount { get; set; }
        public string GREStartdate { get; set; }
        public string GREEndDate { get; set; }
        public string GRECFSCode { get; set; }

        public int INSPartyId { get; set; }
        [Display(Name = "Party Code")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public String INSPartyCode { get; set; }
        public int INSOperationId { get; set; }
        public String INSChargeType { get; set; }
        public String INSSACCode { get; set; }
        public String INSChargeName { get; set; }
        public decimal INSCharge { get; set; }
        public decimal INSCGSTCharge { get; set; }
        public decimal INSSGSTCharge { get; set; }
        public decimal INSIGSTCharge { get; set; }
        public decimal INSIGSTPer { get; set; }
        public decimal INSCGSTPer { get; set; }
        public decimal INSSGSTPer { get; set; }
        public decimal INSAmount { get; set; }
        public decimal INSTaxable { get; set; }
        public decimal INSTotalAmount { get; set; }
        public string INSStartdate { get; set; }
        public string INSEndDate { get; set; }
        public string INSCFSCode { get; set; }

        public int STOPartyId { get; set; }
        [Display(Name = "Party Code")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        public String STOPartyCode { get; set; }
        public int STOOperationId { get; set; }
        public String STOChargeType { get; set; }
        public String STOSACCode { get; set; }
        public String STOChargeName { get; set; }
        public decimal STOCharge { get; set; }
        public decimal STOCGSTCharge { get; set; }
        public decimal STOSGSTCharge { get; set; }
        public decimal STOIGSTCharge { get; set; }
        public decimal STOIGSTPer { get; set; }
        public decimal STOCGSTPer { get; set; }
        public decimal STOSGSTPer { get; set; }
        public decimal STOAmount { get; set; }
        public decimal STOTaxable { get; set; }
        public decimal STOTotalAmount { get; set; }
        public string STOStartdate { get; set; }
        public string STOEndDate { get; set; }
        public string STOCFSCode { get; set; }

        public int HandalingPartyId { get; set; }
        [Display(Name = "Party Code")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public String HandalingPartyCode { get; set; }
        public int HandalingOperationId { get; set; }
        public String HandalingChargeType { get; set; }
        public String HandalingSACCode { get; set; }
        public String HandalingChargeName { get; set; }
        public decimal HandalingCharge { get; set; }
        public decimal HandalingCGSTCharge { get; set; }
        public decimal HandalingSGSTCharge { get; set; }
        public decimal HandalingIGSTCharge { get; set; }
        public decimal HandalingIGSTPer { get; set; }
        public decimal HandalingCGSTPer { get; set; }
        public decimal HandalingSGSTPer { get; set; }
        public decimal HandalingAmount { get; set; }
        public decimal HandalingTaxable { get; set; }
        public decimal HandalingTotalAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public string HNDStartdate { get; set; }
        public string HNDEndDate { get; set; }
        public string HNDCFSCode { get; set; }

        public List<ContainerStuffingDtl> LstStuffingDtl = new List<ContainerStuffingDtl>();

        public List<Kol_ContainerStuffingSCMTR> LstSCMTRDtl = new List<Kol_ContainerStuffingSCMTR>();

        public String GREOperationCFSCodeWiseAmt { get; set; }
        public String GREContainerWiseAmt { get; set; }

        public String INSOperationCFSCodeWiseAmt { get; set; }
        public String INSContainerWiseAmt { get; set; }

        public String STOinvoicecargodtl { get; set; }
        public String STOOperationCFSCodeWiseAmt { get; set; }

        public String HNDOperationCFSCodeWiseAmt { get; set; }

        public String InvoiceNoGRE { get; set; }
        public String InvoiceNoINS { get; set; }
        public String InvoiceNoSTO { get; set; }
        public String InvoiceNoHND { get; set; }
        public int TransportMode { get; set; }

        public List<GREOperationCFSCodeWiseAmt> GREOperationCFSCodeWiseAmtLst = new List<GREOperationCFSCodeWiseAmt>();
        public List<GREContainerWiseAmt> GREContainerWiseAmtLst = new List<GREContainerWiseAmt>();

        public List<INSOperationCFSCodeWiseAmt> INSOperationCFSCodeWiseAmtLst = new List<INSOperationCFSCodeWiseAmt>();
        public List<INSContainerWiseAmt> INSContainerWiseAmtLst = new List<INSContainerWiseAmt>();

        public List<STOinvoicecargodtl> STOinvoicecargodtlLst = new List<STOinvoicecargodtl>();
        public List<STOOperationCFSCodeWiseAmt> STOOperationCFSCodeWiseAmtLst = new List<STOOperationCFSCodeWiseAmt>();

        public List<HNDOperationCFSCodeWiseAmt> HNDOperationCFSCodeWiseAmtLst = new List<HNDOperationCFSCodeWiseAmt>();
        public int FinalDestinationLocationId { get; set; }

        public string FinalDestinationLocation { get; set; }

    }
    public class ContainerDtl
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string CFSCode { get; set; }
        public decimal FOB { get; set; }
        public int StuffingReqId { get; set; }
        public decimal StuffWeight { get; set; }
        public int Insured { get; set; }

    }

    public class GREOperationCFSCodeWiseAmt
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string CFSCode { get; set; }
        public int OperationID { get; set; }
        public int Quantity { get; set; }
        public string ChargeType { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }

    }

    public class GREContainerWiseAmt
    {
        public string CFSCode { get; set; }
        public decimal GrEmpty { get; set; }

    }

    public class INSOperationCFSCodeWiseAmt
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string CFSCode { get; set; }
        public int OperationID { get; set; }
        public int Quantity { get; set; }
        public string ChargeType { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }

    }
    public class INSContainerWiseAmt
    {
        public string CFSCode { get; set; }
        public decimal InsuranceCharge { get; set; }

    }

    public class STOinvoicecargodtl
    {
        public string BOENo { get; set; }
        public DateTime BOEDate { get; set; }
        public string BOLNo { get; set; }
        public DateTime BOLDate { get; set; }
        public string CargoDescription { get; set; }
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public string GdnWiseLctnIds { get; set; }
        public string GdnWiseLctnNames { get; set; }
        public int CargoType { get; set; }
        public DateTime DestuffingDate { get; set; }
        public DateTime CartingDate { get; set; }
        public DateTime StuffingDate { get; set; }
        public int NoOfPackages { get; set; }
        public decimal GrossWt { get; set; }
        public decimal WtPerUnit { get; set; }
        public decimal SpaceOccupied { get; set; }
        public decimal SpaceOccupiedUnit { get; set; }
        public decimal CIFValue { get; set; }
        public decimal Duty { get; set; }

    }
    public class STOOperationCFSCodeWiseAmt
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string CFSCode { get; set; }
        public int OperationID { get; set; }
        public int Quantity { get; set; }
        public string ChargeType { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }

    }

    public class HNDOperationCFSCodeWiseAmt
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string CFSCode { get; set; }
        public int OperationID { get; set; }
        public int Quantity { get; set; }
        public string ChargeType { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }

    }



}