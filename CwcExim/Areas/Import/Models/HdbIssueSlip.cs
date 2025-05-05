using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class HdbIssueSlip
    {
        public int IssueSlipId { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceDate { get; set; }
        public string IssueSlipNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string IssueSlipDate { get; set; }

        [StringLength(1000,ErrorMessage = "Cargo Description Cannot Be More Than 1000 Characters")]
        public string CargoDescription { get; set; }
        public int Uid { get; set; }

        [Required(ErrorMessage ="Fill Out This Field")]
        public string InvoiceNo { get; set; }

        public List<HdbIssueSlipReport> LstIssueSlipRpt= new List<HdbIssueSlipReport>();

        public List<HdbIssueSlipContainer> LstContainer = new List<HdbIssueSlipContainer>();

        public List<HdbIssueSlipCargo> LstCargo = new List<HdbIssueSlipCargo>();
        public decimal TotalCWCDues { get; set; }
        public string CRNoDate { get; set; }
    }

    public class HdbIssueSlipContainer
    {
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string Size { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal CIFValue { get; set; }
        public string BOEDate { get; set; }
        public string BOENo { get; set; }
    }

    public class HdbIssueSlipCargo
    {
        public string OBLNo { get; set; }
        public string CargoDescription { get; set; }
        public string GodownNo { get; set; }
        public string Location { get; set; }
        public string StackNo { get; set; }
        public string Area { get; set; }
        public decimal NetWeight { get; set; }
    }

    public class HdbIssueSlipReport
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
        public string IssueDate { get; set; }
        public int NoOfpackage { get; set; }
        public string OBL { get; set; }
        public string Godown { get; set; } = string.Empty;
        public string GodwonAndLocation { get; set; } = string.Empty;

    }


}