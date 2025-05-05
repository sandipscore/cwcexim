using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class TallySheet
    {
        public string TallySheetNo { get; set; }
        public string TallySheetDate { get; set; }
        public int TallySheetId { get; set; }
        [Display(Name = "ICD Code")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string CFSCode { get; set; }
        public int ContainerId { get; set; }
        [Display(Name = "Container No")]
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        [Display(Name = "Godown No.")]
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        [Display(Name = "Gate In No.")]
        public string GateInNo { get; set; }
        public int SealCuttingId { get; set; }
        public List<TallySheetDtl> lstDtl { get; set; } = new List<TallySheetDtl>();
        public string StringifyXML { get; set; }
        public string SealCuttingDt { get; set; }
    }
    public class TallySheetDtl
    {
        public int TallySheetDtlId { get; set; }
        public string OblNo { get; set; }
        public string OblDate { get; set; }
        public string IGMNo { get; set; }
        public string LineNo { get; set; }
        public string Cargo { get; set; }
        public int Pkg { get; set; }
        public decimal Wt { get; set; }
        public string UOM { get; set; }
        public decimal Area { get; set; }
        public int Serial { get; set; }
    }
    public class TallySheetList
    {
        public int TallySheetId { get; set; }
        [Display(Name = "CFS Code")]
        public string CFSCode { get; set; }
        [Display(Name = "Container No")]
        public string ContainerNo { get; set; }
        [Display(Name = "Godown No")]
        public string GodownName { get; set; }
        [Display(Name = "Gate In No")]
        public string GateInNo { get; set; }
        [Display(Name = "Tally Sheet No.")]
        public string TellySheetNo { get; set; }
        [Display(Name = "Tally Sheet Date")]
        public string TellySheetDate { get; set; }
    }
    public class ContainerList
    {
        public string CONTAINERNO { get; set; }
        public int SealCuttingId { get; set; }
    }
    public class TallySheetPrintHeader
    {
        public string TallySheetNo { get; set; }
        public string TallySheetDate { get; set; }
        public string TallySheetDateTime { get; set; }
        public string GodownNo { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string GateInDate { get; set; }
        public string CustomSealNo { get; set; }
        public string SlaSealNo { get; set; }
        public string CFSCode { get; set; }
        public string IGM_No { get; set; }
        public string MovementType { get; set; }
        public string ShippingLine { get; set; }
        public string POL { get; set; }
        public string POD { get; set; }
        public List<TallySheetPrintDetails> lstDetaiils { get; set; } = new List<TallySheetPrintDetails>();
    }
    public class TallySheetPrintDetails
    {
        public string  SMTPNo { get;set;}
        public string OBL_No { get;set;}
        public string Importer { get;set;}
        public string Cargo { get;set;}
        public string Type { get;set;}
        public int NoOfPkg { get;set;}
        public int PkgRec { get;set;}
        public decimal Weight { get;set;}
        public decimal Area { get;set;}
    }
}