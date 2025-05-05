using CwcExim.Areas.Import.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Hdb_BttContPS : Hdb_InvoiceBase
    {
        public string ExportUnder { get; set; }
        public int Distance { get; set; }
        public int IsLocalGST { get; set; }
        public IList<Hdb_BttContainer> lstConatiner { get; set; } = new List<Hdb_BttContainer>();
        public IList<Hdb_BttContCharges> lstContCharges { get; set; } = new List<Hdb_BttContCharges>();
        public IList<Hdb_BttContCharges> lstContHTCharges { get; set; } = new List<Hdb_BttContCharges>();
        public IList<Hdb_BttContwiseCharges> lstContWiseAmt { get; set; } = new List<Hdb_BttContwiseCharges>();
        public IList<Hdb_BttContOperationCFSCodeWiseAmt> lstOperationCode { get; set; } = new List<Hdb_BttContOperationCFSCodeWiseAmt>();
        public IList<string> ActualApplicable { get; set; } = new List<string>();
        public IList<Hdb_BttContCharges> lstPostContCharges { get; set; } = new List<Hdb_BttContCharges>();
    }
    public class Hdb_BttContainer
    {
        public string CFSCode { get; set; } = string.Empty;
        public string ContainerNo { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public int Reefer { get; set; } = 0;
        public int Insured { get; set; } = 0;
        public int RMS { get; set; } = 0;
        public int HeavyScrap { get; set; } = 0;
        public int AppraisementPerct { get; set; } = 0;
        public int CargoType { get; set; } = 0;
        public string ArrivalDate { get; set; } = string.Empty;
        public string ArrivalTime { get; set; } = string.Empty;
        public string LineNo { get; set; } = string.Empty;
        public string BOENo { get; set; } = string.Empty;
        public DateTime? CartingDate { get; set; } = null;
        public DateTime? StuffingDate { get; set; } = null;
        public DateTime? DestuffingDate { get; set; } = null;
        public int NoOfPackages { get; set; } = 0;
        public decimal GrossWt { get; set; } = 0M;
        public decimal WtPerUnit { get; set; } = 0M;
        public decimal SpaceOccupied { get; set; } = 0M;
        public string SpaceOccupiedUnit { get; set; } = string.Empty;
        public decimal CIFValue { get; set; } = 0M;
        public decimal Duty { get; set; } = 0M;
        public int DeliveryType { get; set; } = 0;
        public decimal StuffCUM { get; set; } = 0M;
        public string LCLFCL { get; set; }
        public string DeliveryDate { get; set; }
        public int ISODC { get; set; } = 0;
    }
    public class Hdb_BttContCharges 
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
        public decimal Discount { get; set; } = 0M;
        public decimal Taxable { get; set; } = 0M;
        public decimal IGSTPer { get; set; } = 0M;
        public decimal IGSTAmt { get; set; } = 0M;
        public decimal CGSTPer { get; set; } = 0M;
        public decimal CGSTAmt { get; set; } = 0M;
        public decimal SGSTPer { get; set; } = 0M;
        public decimal SGSTAmt { get; set; } = 0M;
        public decimal Total { get; set; } = 0M;
        public decimal ActualFullCharge { get; set; } = 0M;
        public int OperationId { get; set; }
        public bool Selected { get; set; }
    }
    public class Hdb_BttContOperationCFSCodeWiseAmt
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
    public class Hdb_BttContwiseCharges
    {
        public int InvoiceId { get; set; }
        public int ContainerId { get; set; }
        public string CFSCode { get; set; } = string.Empty;
        public string LineNo { get; set; } = string.Empty;
        public decimal EntryFee { get; set; } = 0M;
        public decimal CstmRevenue { get; set; } = 0M;
        public decimal GrEmpty { get; set; } = 0M;
        public decimal GrLoaded { get; set; } = 0M;
        public decimal ReeferCharge { get; set; } = 0M;
        public decimal StorageCharge { get; set; } = 0M;
        public decimal InsuranceCharge { get; set; } = 0M;
        public decimal PortCharge { get; set; } = 0M;
        public decimal WeighmentCharge { get; set; } = 0M;
    }
    public class Hdb_BttCont
    {
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string OldCFSCode { get; set; }
        public string StuffType { get; set; }
    }
}