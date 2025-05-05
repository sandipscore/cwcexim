namespace CwcExim.Areas.Icegate.Models
{
    public class IcegateI02Model  
    {
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string SLA { get; set; }
        public string Importer { get; set; }
        public string SendOn { get; set; }
        public string DateofMsg { get; set; }
        public string FileName { get; set; }
        public string I02AFileName { get; set; }
        public string AckStatus { get; set; }
        public string ErrorCode { get; set; }
        public string FileCode { get; set; }
        public string OBLNo { get; set; }
    }
    public class IcegateI02ModelAck
    {
        public string AckRecvDate { get; set; }
        public string FileName { get; set; }
        public string ErrorCode { get; set; }
        public string FileCode { get; set; }
    }
    public class IcegateI02ExcelModel
    {
        public int SlNo { get; set; }
        public string Date { get; set; }
        public string ContainerNo { get; set; }
        public string OBLNo { get; set; }
        public string SLA { get; set; }
        public string Importer { get; set; }
        public string SendOn { get; set; }
        public string FileName { get; set; }
        public string AckStatus { get; set; }

        public string Remarks { get; set; }
    }
    public class IcegateI02ExcelAck
    {
        public string AckRecvDate { get; set; }
        public string FileName { get; set; }
        public string ErrorCode { get; set; }
    }
    public class IcegateE07Model
    {
        public int SlNo { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string SLA { get; set; }
        public string Importer { get; set; }
        public string SendOn { get; set; }
        public string DateofMsg { get; set; }
        public string FileName { get; set; }
        public string I02AFileName { get; set; }
        public string AckStatus { get; set; }
        public string ErrorCode { get; set; }
        public string FileCode { get; set; }
        public string OBLNo { get; set; }
        public string AckRecvDate { get; set; }
    }
    public class IcegateE07ExcelModel
    {
        public int SlNo { get; set; }
        public string Date { get; set; }
        public string ContainerNo { get; set; }
        public string OBLNo { get; set; }
        public string SLA { get; set; }
        public string Importer { get; set; }
        public string SendOn { get; set; }
        public string FileName { get; set; }
        public string AckStatus { get; set; }
        public string Remarks { get; set; }
    }
    public class IcegateE07ExcelAck
    {
        public string AckRecvDate { get; set; }
        public string FileName { get; set; }
        public string ErrorCode { get; set; }
    }
    public class IcegateE07ModelAck
    {
        public string AckRecvDate { get; set; }
        public string FileName { get; set; }
        public string ErrorCode { get; set; }
        public string FileCode { get; set; }
    }
}