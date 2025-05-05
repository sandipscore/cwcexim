using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class HDB_PageLoad
    {
        public IList<HDB_CHAForPage> lstCHA { get; set; } = new List<HDB_CHAForPage>(); 
        public IList<HDB_ShippingLineForPage> lstShippingLine { get; set; } = new List<HDB_ShippingLineForPage>();
        public IList<HDB_ImporterForPage> lstImporter { get; set; } = new List<HDB_ImporterForPage>();
        public IList<HDB_PartyForPage> lstParty { get; set; } = new List<HDB_PartyForPage>();
        public IList<HDB_PayeeForPage> lstPayee { get; set; } = new List<HDB_PayeeForPage>();
    }

    public class HDB_CHAForPage
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }

        public string PartyCode { get; set; }
        public string PartyAddress { get; set; }
    }



    public class HDB_ShippingLineForPage
    {
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public string PartyCode { get; set; }
        public string PartyAddress { get; set; }
    }

    public class HDB_ImporterForPage
    {
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }
        public string ImporterCode { get; set; }
        public string ImporterGSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
    }

    public class HDB_PartyForPage
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyCode { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
    }
    public class HDB_PayeeForPage
    {
        public int PartyId { get; set; }
        public string PayeeName { get; set; }
        public string PayeeCode { get; set; }
        public string PayeeGSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
    }
    public class HDB_ImpPartyForpage
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string PartyCode { get; set; }
    }
}