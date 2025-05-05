using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class VLDA_OblEntry
    {
        public int Id { get; set; }
        public string CONTCBT { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }

        public string ContainerSize { get; set; }
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
        public string IGM_No { get; set; }
        public string IGM_Date { get; set; }
        public string GatewayPortCode { get; set; }

        public List<VLDA_OblEntryDetails> OblEntryDetailsList = new List<VLDA_OblEntryDetails>();
    }

    public class VLDA_OblEntryDetails
    {
        public int icesContId { get; set; }
        public int CommodityId { get; set; }
        public string Commodity { get; set; }
        public int OBLEntryId { get; set; }
        public string OBL_No { get; set; }
        public string OBL_Date { get; set; }
        public string ContainerNo { get; set; }
        public string ArrivalDate { get; set; } = string.Empty;
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

        public string TSANo { get; set; }
        public int IsProcessed { get; set; }
        public string TSA_Date { get; set; } = string.Empty;
        public decimal AreaCBM { get; set; } = 0;
        public decimal CIFValue { get; set; } = 0;

        public int PortCodeOriginId { get; set; }
        public string PortCodeOrigin { get; set; }       
        
    }
}