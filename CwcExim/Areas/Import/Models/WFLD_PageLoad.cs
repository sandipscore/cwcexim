using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class WFLD_PageLoad
    {
        public IList<WFLDCHAForPage> lstCHA { get; set; } = new List<WFLDCHAForPage>();

        public IList<WFLDCommodityForPage> lstCommodity { get; set; } = new List<WFLDCommodityForPage>();

        public IList<WFLDShippingLineForPage> lstShippingLine { get; set; } = new List<WFLDShippingLineForPage>();

        public IList<WFLDImporterForPage> lstImporter { get; set; } = new List<WFLDImporterForPage>();

    }
    public class WFLDCHAForPage
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }

        public string PartyCode { get; set; }

        public bool BillToParty { get; set; }
        public bool IsInsured { get; set; }
        public string InsuredFrmdate { get; set; }
        public string InsuredTodate { get; set; }
        public bool IsTransporter { get; set; }
    }

    public class WFLDCommodityForPage
    {
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }

        public string PartyCode { get; set; }

        public string CommodityType { get; set; }

    }

    public class WFLDShippingLineForPage
    {
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }

        public string PartyCode { get; set; }
    }

    public class WFLDImporterForPage
    {
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }

        public string PartyCode { get; set; }
    }

    public class WFLDImpPartyForpage
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }

        public string PartyCode { get; set; }
        public bool IsInsured { get; set; }
        public bool IsTransporter { get; set; }
        public string  InsuredFrmDate { get; set; }
        public string InsuredToDate { get; set; }
    }

    public class WFLD_PartyForPage
    { 
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyCode { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
    }
}