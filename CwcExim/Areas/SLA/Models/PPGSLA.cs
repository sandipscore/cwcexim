using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.SLA.Models
{
    public class PPGSLA
    {
        public int SLARegistrationId { get; set; }
        public string RaisedBy { get; set; }
        public string RaisedOn { get; set; }

        [MaxLength(1000,ErrorMessage ="You have reached your maximum limit of characters allowed")]
        public string IssueDescription { get; set; }
        public string FileName { get; set; }
        public string ResolutionLevel { get; set; }
        public string IssueType { get; set; }
        public int Uid { get; set; }
        public int BranchId { get; set; }
    }

    public class PPGSLARegistrationList
    {
        public int TicketId { get; set; }
        public string TicketNo { get; set; }
        public string RaisedOn { get; set; }
        public string RaisedBy { get; set; }
        public string IssueDescription { get; set; }
        public string FileName { get; set; }
        public string ResolutionLevel { get; set; }
        public string IssueStatus { get; set; }
        public string IssueType { get; set; }
        public string ResolveDate { get; set; }
        public string ResolveTime { get; set; }

        [StringLength(1000,ErrorMessage = "You have reached your maximum limit of characters allowed")]
        public string Remarks { get; set; }
        public string ResolvedBy { get; set; }
        public string ResolutionHours { get; set; }

    }

    public class PPGSLAReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Year { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string Quarter { get; set; }
        public List<PPGSLAReportIncidents> lstIncidents { get; set; }
        //public List<PPGSLAReportDefects> lstDefects { get; set; }
    }

    public class PPGSLAReportDT
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Year { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string Quarter { get; set; }
        public List<PPGSLAReportIncidentsUPDT> lstIncidentsUP { get; set; }
        public List<PPGSLAReportIncidentsSHDT> lstIncidentsSH { get; set; }        
    }

    public class PPGSLAReportIncidents
    {
        public string M1 { get; set; }
        public string M2 { get; set; }
        public string M3 { get; set; }
        public string M4 { get; set; }
        public string M5 { get; set; }
    }

    public class PPGSLAReportIncidentsUPDT
    {
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public string Reason { get; set; }
        public string Duration { get; set; }        
    }

    public class PPGSLAReportIncidentsSHDT
    {
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public string Reason { get; set; }
        public string Duration { get; set; }
    }

    public class PPGDownTime
    {
        public int DTRegistrationIdCWC { get; set; }
        public string RaisedBy { get; set; }
        public string RaisedOn { get; set; }
        public string RemarksCwc { get; set; }        
        public string StartDateTime { get; set; }
        public string CompletionDateTime { get; set; }
        public int Uid { get; set; }
        public int BranchId { get; set; }
        public int TicketId { get; set; }
        public string TicketNo { get; set; }
        public string RemarksSitl { get; set; }
        public string CompletionStatus { get; set; }
    }

    public class PPGDownTimeSitl
    {
        public int DTRegistrationIdSitl { get; set; }
        public string RaisedBy { get; set; }
        public string RaisedOn { get; set; }
        public string Reason { get; set; }
        public string StartDateTime { get; set; }
        public string CompletionDateTime { get; set; }
        public int Uid { get; set; }
        public int BranchId { get; set; }
        public int TicketId { get; set; }
        public string TicketNo { get; set; }
    }
}