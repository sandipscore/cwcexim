using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    internal class PaySheetChargeDetails
    {
        public IList<PSContainer> lstPSContainer { get; set; } = new List<PSContainer>();
        public IList<PSEmptyGroudRent> lstEmptyGR { get; set; } = new List<PSEmptyGroudRent>();
        public IList<PSLoadedGroudRent> lstLoadedGR { get; set; } = new List<PSLoadedGroudRent>();
        public decimal InsuranceRate { get; set; }
        public IList<StorageRent> lstStorageRent { get; set; } = new List<StorageRent>();
        public decimal RateSQMPerWeek { get; set; }
        public decimal RateSQMPerMonth { get; set; }
        public decimal RateCUMPerWeek { get; set; }
        public decimal RateMTPerDay { get; set; }
        public IList<InsuranceCharge> lstInsuranceCharges { get; set; } = new List<InsuranceCharge>();
        public IList<PSHTCharges> lstPSHTCharge { get; set; } = new List<PSHTCharges>();
    }
    public class PSContainer
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public bool IsReefer { get; set; }
        public string Insured { get; set; }
    }
    public class PSEmptyGroudRent
    {
        public string ContainerType { get; set; }
        public string CommodityType { get; set; }
        public bool IsReefer { get; set; }
        public string Size { get; set; }
        public int DaysRangeFrom { get; set; }
        public int DaysRangeTo { get; set; }
        public decimal RentAmount { get; set; }
        public decimal ElectricityCharge { get; set; }
        public int GroundRentPeriod { get; set; }
        public string CFSCode { get; set; }
        public decimal FOBValue { get; set; }
        public int IsInsured { get; set; }
    }
    public class PSLoadedGroudRent
    {
        public string ContainerType { get; set; }
        public string CommodityType { get; set; }
        public bool IsReefer { get; set; }
        public string Size { get; set; }
        public int DaysRangeFrom { get; set; }
        public int DaysRangeTo { get; set; }
        public decimal RentAmount { get; set; }
        public decimal ElectricityCharge { get; set; }
        public int GroundRentPeriod { get; set; }
        public string CFSCode { get; set; }
        public decimal FOBValue { get; set; }
        public int IsInsured { get; set; }
    }
    public class StorageRent
    {
        public string CFSCode { get; set; }
        public decimal ActualCUM { get; set; }
        public decimal ActualSQM { get; set; }
        public decimal ActualWeight { get; set; }
        public decimal StuffCUM { get; set; }
        public decimal StuffSQM { get; set; }
        public decimal StuffWeight { get; set; }
        public int StorageDays { get; set; }
        public int StorageWeeks { get; set; }
        public int StorageMonths { get; set; }
        public int StorageMonthWeeks { get; set; }
    }
    public class InsuranceCharge
    {
        public string CFSCode { get; set; }
        public int StorageWeeks { get; set; }
        public int IsInsured { get; set; }
        public decimal FOB { get; set; }
    }
    public class PSHTCharges
    {
        public int ChargeId { get; set; }
        public string ChargeName { get; set; }
        public decimal Charge { get; set; }
    }
    public class PaymentSheetFinalModel
    {
        public int InvoiceId { get; set; } = 0;
        public string InvoiceType { get; set; } = "";
        public string InvoiceNo { get; set; } = "";
        public string InvoiceDate { get; set; } = "";
        public int StuffingReqId { get; set; } = 0;
        public string StuffingReqNo { get; set; } = "";
        public string StuffingDate { get; set; } = "";
        public int PartyId { get; set; } = 0;
        public string PartyName { get; set; } = "";
        public int PayeeId { get; set; } = 0;
        public string PayeeName { get; set; } = "";
        public string GSTNo { get; set; } = "";
        public IList<PSContainer> lstPSContainer { get; set; } = new List<PSContainer>();
        public decimal CWCTotal { get; set; } = 0;
        public IList<ChargesType> lstChargesType { get; set; } = new List<ChargesType>();
        public decimal HTTotal { get; set; } = 0;
        public decimal AllTotal { get; set; } = 0M;
        public decimal RoundUp { get; set; } = 0M;
        public decimal Invoice { get; set; } = 0M;
        public string Remarks { get; set; } = "";
    }
    public class ChargesType
    {
        public int DBChargeID { get; set; }
        public string ChargeId { get; set; }
        public string ChargeType { get; set; }
        public string ChargeName { get; set; }
        public decimal Amount { get; set; } = 0;
        public decimal IGSTPer { get; set; } = 0;
        public decimal IGSTAmt { get; set; } = 0;
        public decimal CGSTPer { get; set; } = 0;
        public decimal CGSTAmt { get; set; } = 0;
        public decimal SGSTPer { get; set; } = 0;
        public decimal SGSTAmt { get; set; } = 0;
        public decimal Total { get; set; } = 0;
    }
}