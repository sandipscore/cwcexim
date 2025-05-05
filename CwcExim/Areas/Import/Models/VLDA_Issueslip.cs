using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Import.Models
{
    public class VLDA_Issueslip
    {
        public int IssueSlipId { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceDate { get; set; }
        public string IssueSlipNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string IssueSlipDate { get; set; }

        [StringLength(1000, ErrorMessage = "Cargo Description Cannot Be More Than 1000 Characters")]
        public string CargoDescription { get; set; }
        public int Uid { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string InvoiceNo { get; set; }

        public List<VLDA_IssueSlipReport> LstIssueSlipRpt = new List<VLDA_IssueSlipReport>();

        public List<VLDA_IssueSlipContainer> LstContainer = new List<VLDA_IssueSlipContainer>();

        public List<VLDA_IssueSlipCargo> LstCargo = new List<VLDA_IssueSlipCargo>();
        public decimal TotalCWCDues { get; set; }
        public string CRNoDate { get; set; }
    }
    public class VLDA_IssueSlipContainer
    {
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string Size { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal CIFValue { get; set; }
        public decimal Duty { get; set; }
        public decimal Total { get; set; }
        public string BOEDate { get; set; }
        public string BOENo { get; set; }
    }

    public class VLDA_IssueSlipCargo
    {
        public string OBLNo { get; set; }
        public string CargoDescription { get; set; }
        public string GodownNo { get; set; }
        public string Location { get; set; }
        public string StackNo { get; set; }
        public string Area { get; set; }
        public decimal NetWeight { get; set; }
    }

    public class VLDA_IssueSlipReport
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string Vessel { get; set; }
        public string BOEDate { get; set; }
        public string BOENo { get; set; }
        public string CHA { get; set; }
        public string ShippingLine { get; set; }
        public string Importer { get; set; }
        public string CargoDescription { get; set; }
        public string MarksNo { get; set; }
        public string LineNo { get; set; }
        public string Rotation { get; set; }
        public string Weight { get; set; }
        public string ArrivalDate { get; set; }
        public string DestuffingDate { get; set; }
        public string Location { get; set; }

    }


}