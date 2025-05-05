namespace CwcExim.Areas.Report.Models
{
    public class Hdb_ExpCargo
    {
        public int SlNo { get; set; }
        public string EntryNo { get; set; }
        public string InDate { get; set; }
        public string SbNo { get; set; }
        public string SbDate { get; set; }
        public string GodownName { get; set; }
        public string Shed { get; set; }
        public string StorageType { get; set; }
        public string CHAName { get; set; }
        public string ForwarderName { get; set; }
        public string FCLLCL { get; set; }
        public decimal Area { get; set; }
        public decimal Fob { get; set; }
        public int NoOfDays { get; set; }
        public int NoOfWeek { get; set; }
        public decimal GeneralAmount { get; set; }
        public decimal InsuranceAmount { get; set; }
        
    }
}