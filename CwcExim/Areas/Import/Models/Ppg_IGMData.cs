using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Ppg_IGMData
    { 
        public string Year { get; set; }
        public string ContainerNo { get; set; }
        public List<Ppg_ContainerInfo> lstContainerInfo { get; set; }
        public List<Ppg_CargoInfo> lstCargoInfo { get; set; }
    }
    public class Ppg_ContainerInfo
    {
        public string Size { get; set; }
        public string Status { get; set; }
        public string IGMNo { get; set; }
        public string IGMDate { get; set; }
        public string TPNo { get; set; }
        public string TPDate { get; set; }
        public string OBLNo { get; set; }
        public string NoOfPkg { get; set; }
        public string Weight { get; set; }
        public string SealNo { get; set; }
        public string ISOCode { get; set; }
    }
    public class Ppg_CargoInfo
    {
        public string OBLNo { get; set; }
        public string OBLDate { get; set; }
        public string ImporterName { get; set; }
        public string NoOfPkg { get; set; }
        public string PkgType { get; set; }
        public string Weight { get; set; }
        public string Unit { get; set; }
        public string CargoDesc { get; set; }
        public string HOUSE_BL_NO { get; set; }
    }

}
