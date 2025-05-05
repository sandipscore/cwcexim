using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.GateOperation.Models
{
    public class VRN_AddExportVehicle
    {
        public int DtlEntryId { get; set; }
        public int Uid { get; set; }
        public int EntryId { get; set; }
        public string ExportVehicleNo { get; set; }
        public int ExportNoOfPkg { get; set; }
        public decimal ExportGrWeight { get; set; }
        public string ExportCFSCode { get; set; }
        public string ExportReferenceNo { get; set; }
        public string VehicleEntryDt { get; set; }
        public string VehicleEntryTime { get; set; }
        public int TotalNoOfPkg { get; set; } = 0;
        public decimal TotalGrWt { get; set; } = 0;

    }
}