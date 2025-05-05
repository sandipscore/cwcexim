using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Master.Models
{
    public class VRN_PartyModel
    {
        public IList<VRN_CHAForPage> lstCHA { get; set; } = new List<VRN_CHAForPage>();
        public IList<VRN_ShippingLineForPage> lstShippingLine { get; set; } = new List<VRN_ShippingLineForPage>();
        public IList<VRN_ImporterForPage> lstImporter { get; set; } = new List<VRN_ImporterForPage>();
        public IList<VRN_PartyForPage> lstParty { get; set; } = new List<VRN_PartyForPage>();
        public IList<VRN_PayeeForPage> lstPayee { get; set; } = new List<VRN_PayeeForPage>();
    }

    public class VRN_CHAForPage
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }

        public string PartyCode { get; set; }
        public string PartyAddress { get; set; }
    }


    public class VRN_ShippingLineForPage
    {
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public string PartyCode { get; set; }
        public string PartyAddress { get; set; }
    }

    public class VRN_ImporterForPage
    {
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }
        public string ImporterCode { get; set; }
        public string ImporterGSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
    }

    public class VRN_PartyForPage
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyCode { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
    }
    public class VRN_PayeeForPage
    {
        public int PartyId { get; set; }
        public string PayeeName { get; set; }
        public string PayeeCode { get; set; }
        public string PayeeGSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
    }
    public class VRN_ImpPartyForpage
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