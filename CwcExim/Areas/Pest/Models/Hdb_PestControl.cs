using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Pest.Models
{
    public class Hdb_PestControl:PestControlOperation
    {
        public string ExportUnder { get; set; }
        public decimal PestControlCGST { get; set; } = 0;
        public decimal PestControlSGST { get; set; } = 0;
        public decimal PestControlIGST { get; set; } = 0;
        public decimal HandlingAmount { get; set; } = 0;
        public decimal HandlingCGST { get; set; } = 0;
        public decimal HandlingSGST { get; set; } = 0;
        public decimal HandlingIGST { get; set; } = 0;
        public decimal NetAmt { get; set; } = 0;
        public decimal Totaltaxable { get; set; } = 0;
        public decimal IGSTPer { get; set; } = 0;
        public decimal CGSTPer { get; set; } = 0;
        public decimal SGSTPer { get; set; } = 0;
        public string HTClause { get; set; }
        public int HTClauseId { get; set; } = 0;
        
    }
}