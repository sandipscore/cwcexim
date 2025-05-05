﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinvoiceLibrary
{
    public class EinvoicePayload
    {
        public string Version { get; set; }
       
        public TranDtls TranDtls { get; set; }

        public DocDtls DocDtls { get; set; }

        public SellerDtls SellerDtls { get; set; }

        public BuyerDtls BuyerDtls { get; set; }

        public DispDtls DispDtls { get; set; }

        public ShipDtls ShipDtls { get; set; }

        public IList<ItemList> ItemList = new List<ItemList>();


        public AttribDtls AttribDtls { get; set; }

        public ValDtls ValDtls { get; set; }
        public PayDtls PayDtls { get; set; }
        public RefDtls RefDtls { get; set; }

        public DocPerdDtls DocPerdDtls { get; set; }

        public PrecDocDtls PrecDocDtls { get; set; }
        public ContrDtls ContrDtls { get; set; }
        public AddlDocDtls AddlDocDtls { get; set; }

        public IList<ExpDtls> ExpDtls = new List<ExpDtls>();

        public EwbDtls EwbDtls { get; set; }


    }
    public class TranDtls
    {
        public string TaxSch { get; set; }
        public string SupTyp { get; set; }
        public string RegRev { get; set; }
        public string EcmGstin { get; set; }
        public string IgstOnIntra { get; set; }

    }
    public class DocDtls
    {
        public string Typ { get; set; }
        public string No { get; set; }
        public string Dt { get; set; }

    }
    public class SellerDtls
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public Int32 Pin { get; set; }
        public string Stcd { get; set; }
        public string Ph { get; set; }
        public string Em { get; set; }



    }
    public class BuyerDtls
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string Addr1 { get; set; }

        public string Pos { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public Int32 Pin { get; set; }
        public string Stcd { get; set; }
        public string Ph { get; set; }
        public string Em { get; set; }




    }
    public class DispDtls
    {

        public string Nm { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public Int32 Pin { get; set; }
        public string Stcd { get; set; }


    }
    public class ShipDtls
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public Int32 Pin { get; set; }
        public string Stcd { get; set; }


    }
    public class ItemList
    {
        public string SlNo { get; set; }
        public string PrdDesc { get; set; }
        public string IsServc { get; set; }
        public string HsnCd { get; set; }
        public string Barcde { get; set; }
        public decimal Qty { get; set; }
        public Int32 FreeQty { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotAmt { get; set; }
        public Int32 Discount { get; set; }
        public Int32 PreTaxVal { get; set; }
        public decimal AssAmt { get; set; }
        public decimal GstRt { get; set; }
        public decimal IgstAmt { get; set; }
        public decimal CgstAmt { get; set; }
        public decimal SgstAmt { get; set; }
        public Int32 CesRt { get; set; }
        public decimal CesAmt { get; set; }
        public Int32 CesNonAdvlAmt { get; set; }
        public Int32 StateCesRt { get; set; }
        public decimal StateCesAmt { get; set; }
        public Int32 StateCesNonAdvlAmt { get; set; }
        public Int32 OthChrg { get; set; }
        public decimal TotItemVal { get; set; }
        public string OrdLineRef { get; set; }
        public string OrgCntry { get; set; }
        public string PrdSlNo { get; set; }

        public BchDtls BchDtls { get; set; }
        public AttribDtls AttribDtls { get; set; }
    }
    public class BchDtls
    {
        public string Nm { get; set; }
        public string ExpDt { get; set; }
        public string WrDt { get; set; }
    }
    public class AttribDtls
    {
        public string Nm { get; set; }
        public string Val { get; set; }

    }
    public class ValDtls
    {

        public decimal AssVal { get; set; }
        public decimal CgstVal { get; set; }
        public decimal SgstVal { get; set; }
        public decimal IgstVal { get; set; }
        public decimal CesVal { get; set; }
        public decimal StCesVal { get; set; }
        public decimal Discount { get; set; }
        public decimal OthChrg { get; set; }
        public decimal RndOffAmt { get; set; }
        public decimal TotInvVal { get; set; }
        public decimal TotInvValFc { get; set; }



    }
    public class PayDtls
    {
        public string Nm { get; set; }
        public string AccDet { get; set; }
        public string Mode { get; set; }
        public string FinInsBr { get; set; }
        public string PayTerm { get; set; }
        public string PayInstr { get; set; }
        public string CrTrn { get; set; }
        public string DirDr { get; set; }
        public string CrDay { get; set; }
        public string PaidAmt { get; set; }
        public string PaymtDue { get; set; }

    }
    public class RefDtls
    {
        public string InvRm { get; set; }
       public DocPerdDtls DocPerdDtls { get; set; }
    }
    public class DocPerdDtls
    {
       public string InvStDt { get; set; }
        public string InvEndDt { get; set; }
    }
    public class PrecDocDtls
    {
        public string InvNo { get; set; }
        public string InvDt { get; set; }
        public string OthRefNo { get; set; }

    }
    public class ContrDtls
    {
        public string RecAdvRefr { get; set; }
        public string RecAdvDt { get; set; }
        public string TendRefr { get; set; }
        public string ContrRefr { get; set; }
        public string ExtRefr { get; set; }
        public string ProjRefr { get; set; }
        public string PORefr { get; set; }
        public string PORefDt { get; set; }

    }
    public class AddlDocDtls
    {
        public string Url { get; set; }
        public string Docs { get; set; }
        public string Info { get; set; }

    }
    public class ExpDtls
    {
        public string ShipBNo { get; set; }
        public string ShipBDt { get; set; }
        public string Port { get; set; }
        public string RefClm { get; set; }
        public string ForCur { get; set; }
        public string CntCode { get; set; }
        public string ExpDuty { get; set; }

    }
    public class EwbDtls
    {

        public string TransId { get; set; }
        public string TransName { get; set; }
        public string Distance { get; set; }
        public string TransDocNo { get; set; }
        public string TransDocDt { get; set; }
        public string VehNo { get; set; }
        public string VehType { get; set; }
        public string TransMode { get; set; }
    }
}
