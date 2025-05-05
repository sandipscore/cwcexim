using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class GenericChargesModel
    {
        public IList<EntryFees> EntryFees { get; set; }// = new List<EntryFees>();
        public IList<CustomRevenue> CustomRevenue { get; set; }// = new List<CustomRevenue>();
        public IList<EmptyGroundRent> EmptyGroundRent { get; set; }// = new List<EmptyGroundRent>();
        public IList<LoadedGroundRent> LoadedGroundRent { get; set; }// = new List<LoadedGroundRent>();
        public IList<Reefer> Reefer { get; set; }// = new List<Reefer>();
        public IList<StorageRent> StorageRent { get; set; }// = new List<StorageRent>();
        public IList<InsuranceRate> InsuranceRate { get; set; }// = new List<InsuranceRate>();
        public IList<OtherCharges> OtherCharges { get; set; }// = new List<OtherCharges>();
        public IList<PortCharges> PortCharges { get; set; }
        public IList<WeighmentCharge> WeighmentCharge { get; set; }// = new List<WeighmentCharge>();
        public IList<HTChargeRent> HTChargeRent { get; set; }// = new List<HTChargeRent>();
        public IList<SACGST> SACGST { get; set; }
        public IList<ApplicableHT> ApplicableHT { get; set; }
        public IList<CompanyDetails> CompanyDetails { get; set; }

        public IList<Deliverycharges> Deliverycharges { get; set; }
        public IList<Unloadingthecargo> Unloadingthecargo { get; set; }

        public IList<TransportationContainers> TransportationContainers { get; set; }
    }

    public class EntryFees
    {
        public string ContainerSize { get; set; }
        public decimal Rate { get; set; }
        public string SacCode { get; set; }
    }
    public class CustomRevenue
    {
        public decimal Charge { get; set; }
        public string SacCode { get; set; }
    }
    public class EmptyGroundRent
    {
        public string SacCode { get; set; }
        public int CommodityType { get; set; }
        public int DaysRangeFrom { get; set; }
        public int DaysRangeTo { get; set; }
        public string Size { get; set; }
        public decimal RentAmount { get; set; }
        public int ImpExp { get; set; } = 0;
    }
    public class LoadedGroundRent
    {
        public string SacCode { get; set; }
        public int CommodityType { get; set; }
        public int DaysRangeFrom { get; set; }
        public int DaysRangeTo { get; set; }
        public string Size { get; set; }
        public decimal RentAmount { get; set; }
        public int ImpExp { get; set; } = 0;
    }
    public class Reefer
    {
        public decimal ElectricityCharge { get; set; }
        public string ContainerSize { get; set; }
        public string SacCode { get; set; }
    }
    public class StorageRent
    {
        public int DaysRangeFrom { get; set; }
        public int DaysRangeTo { get; set; }
        public int CommodityType { get; set; }
        public int WarehouseType { get; set; }
        public decimal RateSqMPerWeek { get; set; }
        public decimal RateSqMeterPerMonth { get; set; }
        public decimal RateMetricTonePerDay { get; set; }
        public decimal RateCubMeterPerDay { get; set; }
        public decimal RateCubMeterPerWeek { get; set; }
        public decimal RateCubMeterPerMonth { get; set; }
        public string SacCode { get; set; }
        public string StorageType { get; set; }
    }
    public class InsuranceRate
    {
        public decimal ChargeInRs { get; set; }
        public string SacCode { get; set; }
    }
    public class OtherCharges
    {
        public decimal Fumigation { get; set; }
        public decimal Washing { get; set; }
        public decimal Reworking { get; set; }
        public decimal Bagging { get; set; }
        public decimal Palletizing { get; set; }
        public string SacCode { get; set; }
    }
    public class PortCharges
    {
        public decimal PortCharge { get; set; }
        public string SACCode { get; set; }
    }
    public class WeighmentCharge
    {
        public decimal ContainerRate { get; set; }
        public string ContainerSize { get; set; }
        public string SacCode { get; set; }
    }
    public class HTChargeRent
    {
        public string SacCode { get; set; }
        public int OperationType { get; set; }
        public string OperationCode { get; set; }
        public string OperationSDesc { get; set; }
        public int ClauseOrder { get; set; }
        public int ContainerType { get; set; }
        public string Size { get; set; }
        public int CommodityType { get; set; }
        public decimal RateCWC { get; set; }
        public int Type { get; set; }
    }
    public class SACGST
    {
        public string SacCode { get; set; }
        public decimal Gst { get; set; }
    }
    public class ApplicableHT
    {
        public string Clause { get; set; }
        public int ImportYardPS { get; set; }
        public int ImportYeardPSCD { get; set; }
        public int ImportGodownDSPS { get; set; }
        public int ImportGodownDelivery { get; set; }
        public int ExportPS { get; set; }
        public int EmptyContainerDelivery { get; set; }
        public int BTT { get; set; }
        public int LoadedExport { get; set; }
    }
    public class CompanyDetails
    {
        public string ROAddress { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyAddress { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public int? StateId { get; set; }
        public string StateCode { get; set; }
        public int? CityId { get; set; }
        public string GstIn { get; set; }
        public string Pan { get; set; }
    }

    public class TransportationContainers
    {

        public string SacCode { get; set; }
        public string Size { get; set; }
        public string RateCWC { get; set; }
        public string FromDisSlabCharge { get; set; }
        public string ToDisSlabCharge { get; set; }
      

    }

    public class Deliverycharges
    {
        public decimal RateCWC { get; set; }
        public string Size { get; set; }
        public string SacCode { get; set; }
    }

    public class Unloadingthecargo
    {
        public decimal RateCWC { get; set; }
        public string Size { get; set; }
        public string SacCode { get; set; }
    }
}