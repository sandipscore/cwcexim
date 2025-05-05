using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class OBLEntry
    {
        public int Id { get; set; }
        public string CONTCBT { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }

        ///[Required(ErrorMessage = "Fill Out This Field")]
        public string ContainerSize { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string IGM_No { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string IGM_Date { get; set; }
        public string TPNo { get; set; }
        public string TPDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string MovementType { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public int PortId { get; set; }
        public int SelectPortId { get; set; }
        public string PortName { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public int CountryId { get; set; }
        public int SelectCountryId { get; set; }
        public string CountryName { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }

        public string PartyCode { get; set; }

        public string StringifiedText { get; set; }
        public int IsAlreadyUsed { get; set; }
        public string OBLCreateDate { get; set; }
        
        public List<OblEntryDetails> OblEntryDetailsList = new List<OblEntryDetails>();
    }

    public class OblEntryDetails
    {
        public int icesContId { get; set; }
        public int CommodityId { get; set; }
        public string Commodity { get; set; }
        public int OBLEntryId { get; set; }
        public string OBL_No { get; set; }
        public string OBL_Date { get; set; }
        public string LineNo { get; set; }
        public string SMTPNo { get; set; }
        public string SMTP_Date { get; set; } = string.Empty;
        public string CargoDescription { get; set; }
        public int CargoType { get; set; }
        public string NoOfPkg { get; set; }
        public string PkgType { get; set; }
        public decimal GR_WT { get; set; }
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }
        public string IGM_IMPORTER { get; set; }
        public string MovementType { get; set; }
        public int PortId { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public int IsProcessed { get; set; }
        

    }

}