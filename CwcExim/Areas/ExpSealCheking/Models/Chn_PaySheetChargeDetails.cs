using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.ExpSealCheking.Models
{
        internal class Chn_PaySheetChargeDetails
        {
            public IList<Chn_PSContainer> lstPSContainer { get; set; } = new List<Chn_PSContainer>();
            public IList<Chn_PSEmptyGroudRent> lstEmptyGR { get; set; } = new List<Chn_PSEmptyGroudRent>();
            public IList<Chn_PSLoadedGroudRent> lstLoadedGR { get; set; } = new List<Chn_PSLoadedGroudRent>();
            public decimal InsuranceRate { get; set; }
            public IList<Chn_StorageRent> lstStorageRent { get; set; } = new List<Chn_StorageRent>();
            public decimal RateSQMPerWeek { get; set; }
            public decimal RateSQMPerMonth { get; set; }
            public decimal RateCUMPerWeek { get; set; }
            public decimal RateMTPerDay { get; set; }
            public IList<Chn_InsuranceCharge> lstInsuranceCharges { get; set; } = new List<Chn_InsuranceCharge>();
            public IList<Chn_PSHTCharges> lstPSHTCharge { get; set; } = new List<Chn_PSHTCharges>();
        }
        public class Chn_PSContainer
        {
            public string CFSCode { get; set; }
            public string ContainerNo { get; set; }
            public string Size { get; set; }
            public bool IsReefer { get; set; }
            public string Insured { get; set; }
        }
        public class Chn_PSEmptyGroudRent
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
        public class Chn_PSLoadedGroudRent
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
        public class Chn_StorageRent
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
        public class Chn_InsuranceCharge
        {
            public string CFSCode { get; set; }
            public int StorageWeeks { get; set; }
            public int IsInsured { get; set; }
            public decimal FOB { get; set; }
        }
        public class Chn_PSHTCharges
        {
            public int ChargeId { get; set; }
            public string ChargeName { get; set; }
            public decimal Charge { get; set; }
        }
        public class Chn_PaymentSheetFinalModel
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
            public IList<Chn_PSContainer> lstPSContainer { get; set; } = new List<Chn_PSContainer>();
            public decimal CWCTotal { get; set; } = 0;
            public IList<Chn_ChargesType> lstChargesType { get; set; } = new List<Chn_ChargesType>();
            public decimal HTTotal { get; set; } = 0;
            public decimal AllTotal { get; set; } = 0M;
            public decimal RoundUp { get; set; } = 0M;
            public decimal Invoice { get; set; } = 0M;
            public string Remarks { get; set; } = "";
        }
        public class Chn_ChargesType
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