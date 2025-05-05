using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class Hdb_ContainerIndent
    {
        public int IndentId { get; set; }
        public string IndentNo { get; set; }
        public string IndentDate { get; set; }
        public string TrailerNo { get; set; }
        public string ICDIn { get; set; }
        public string ICDOut { get; set; }
        public int Form1Id { get; set; }
        public string Form1No { get; set; }
        public string Remarks { get; set; }
        public List<Hdb_ContainerDetails> lstContainerDetails { get; set; } = new List<Hdb_ContainerDetails>();
    }
    public class Hdb_ListContainerIndent
    {
        public int IndentId { get; set; }
        public string IndentNo { get; set; }
        public string IndentDate { get; set; }
        public string TrailerNo { get; set; }
        public string FormOneNo { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
    }
    public class Hdb_ContainerDetails
    {
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public string CHAName { get; set; }
        public string IMPName { get; set; }
        public string CargoDesc { get; set; }
    }
    public class Hdb_Form1Det
    {
        public string FormOneNo { get; set; }
        public int FormOneId { get; set; }
    }
}