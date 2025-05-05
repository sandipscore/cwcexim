using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Bond.Models
{
    public class VRN_DeliveryOrder
    {
        public int DeliveryOrderId { get; set; }

        [Display(Name = "Delivery Order No")]
        public string DeliveryOrderNo { get; set; }

        [Display(Name = "Delivery Order Date")]
        public string DeliveryOrderDate { get; set; }
        public int SpaceAppId { get; set; }
        public string DeliveryOrderXml { get; set; }
        [Display(Name = "Bond No")]
        public string BondBOENo { get; set; }
        [Display(Name = "Bond Date")]
        public string BondBOEDate { get; set; }
        public string Importer { get; set; }
        [Display(Name = "Sac No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string SacNo { get; set; }

        [Display(Name = "Sac Date")]
        public string SacDate { get; set; }

        [StringLength(500, ErrorMessage = "Remarks Cannot Be More than 500 Characters In Length")]
        public string Remarks { get; set; }
        public int CHAId { get; set; }
        public string CargoDescription { get; set; }
        public string BondNo { get; set; }
        public string BondDate { get; set; }
        public Decimal Units_print { get; set; }
        public Decimal Weight_print { get; set; }
        public int GodownId_print { get; set; }
        public decimal TotalValue { get; set; } = 0;
        public decimal TotalDuty { get; set; } = 0;
        public int TotalUnits { get; set; } = 0;
        public string CompanyEmail { get; set; } = string.Empty;
        public string CompanyAddress { get; set; } = string.Empty;
        public string CompanyLoaction { get; set; } = string.Empty;
        public string LastDeliveryDate { get; set; } = string.Empty;
        public decimal TotalArea { get; set; } = 0;
        public decimal TotalWeight { get; set; } = 0;

        public List<VRN_DeliveryOrderforPrint> LstDeliveryPrintOrder { get; set; } = new List<VRN_DeliveryOrderforPrint>();
        public List<VRN_DeliveryOrderDtl> LstDeliveryOrderhdb { get; set; } = new List<VRN_DeliveryOrderDtl>();
        public List<VRN_DeliveryOrderPaymentPrint> LstDeliveryOrderPayment { get; set; } = new List<VRN_DeliveryOrderPaymentPrint>();

        public List<VRN_DeliveryGSTCharge> LstGst { get; set; } = new List<VRN_DeliveryGSTCharge>();

    }
        public class VRN_DeliveryOrderforPrint
        {
            public string Importer { get; set; }
            public string CHA { get; set; }
            public string CargoDesc { get; set; }
            public Decimal Units { get; set; }
            public Decimal Weight { get; set; }

            public string SacNo { get; set; }
            public string SacDate { get; set; }
            public string Remarks { get; set; }

            public string StorageUptoDate { get; set; }
            public string StorageToDate { get; set; }

            public string StorageFromDate { get; set; }

            public string StorageDays { get; set; }

            public string StorageWeek { get; set; }
            public decimal PaidStorage { get; set; }

            public string InsuranceUptoDate { get; set; }

            public string InsuranceFromDate { get; set; }
            public string InsuranceToDate { get; set; }
            public string InsuranceDays { get; set; }
            public string InsuranceWeek { get; set; }
            public decimal Paidinsurance { get; set; }

            public string Storagerate { get; set; }

        }


        public class VRN_DeliveryOrderDtl
        {
            public int DeliveryOrderDtlId { get; set; }
            public int DeliveryOrderId { get; set; }
            public int DepositAppId { get; set; }
            public string DepositNo { get; set; }
            public string DepositDate { get; set; }
            public string WRNo { get; set; }
            public string WRDate { get; set; }
            public int Units { get; set; }
            public decimal Weight { get; set; }
            public int ClosingUnits { get; set; }
            public decimal ClosingWeight { get; set; }
            public decimal Value { get; set; }
            public decimal Duty { get; set; }
            public decimal ClosingValue { get; set; }
            public decimal ClosingDuty { get; set; }
            public int GodownId { get; set; }
            public string CargoDescription { get; set; }
            public string Remarks { get; set; }
            public decimal Areas { get; set; }
            public decimal ClosingAreas { get; set; }
            public string ExBoeNo { get; set; }
            public string ExBoeDate { get; set; }
            public int UnloadedUnits { get; set; }
            public decimal UnloadedArea { get; set; }
            public decimal UnloadedWeight { get; set; }
            public decimal UnloadedValue { get; set; }
            public decimal UnloadedDuty { get; set; }
    }


        public class VRN_DeliveryOrderPaymentPrint
        {
            public string SacDate { get; set; }
            public Decimal SpaceReq { get; set; }
            public String DepositDate { get; set; }
            public string DepositNo { get; set; }
            public Decimal AreaReserved { get; set; }
            public string ReceiptNo { get; set; }
            public Decimal InsAmt { get; set; }
            public Decimal StoAmt { get; set; }
            public String FromDate { get; set; }
            public String ToDate { get; set; }
            public int days { get; set; }
            public int Weeks { get; set; }
            public decimal InvInsAmt { get; set; }
            public decimal InvStoAmt { get; set; }
            public decimal Tax { get; set; }
            public decimal InvoiceAmt { get; set; }
            public decimal TotalTaxable { get; set; }
            public string ReceiptDate { get; set; }

        }

        public class VRN_DeliveryGSTCharge
        {
            public decimal SGSTAmt { get; set; } = 0;
            public decimal CGSTAmt { get; set; } = 0;
            public decimal IGSTAmt { get; set; } = 0;
        }
        public class VRN_ListOfWorkOrderNo
    {
            public string SacNo { get; set; }
            public int SpaceappId { get; set; }
            public string BondNo { get; set; }
            public int DepositappId { get; set; }
        }
        public class VRN_WorkOrderDetails
        {
            public int BondWOId { get; set; }
            public string BondNo { get; set; }
            public string BondDate { get; set; }
            public string WRNo { get; set; }
            public string WRDate { get; set; }
            public int SpaceAppId { get; set; }
            public int GodownId { get; set; }
            public string WorkOrderDate { get; set; }
            public string SacNo { get; set; }
            public string SacDate { get; set; }
            public int ClosingUnits { get; set; }
            public decimal ClosingWeight { get; set; }
            public string Importer { get; set; }
            public int CHAId { get; set; }
            public string Remarks { get; set; }
        }
}