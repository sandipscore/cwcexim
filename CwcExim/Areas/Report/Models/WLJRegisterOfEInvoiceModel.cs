using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WLJRegisterOfEInvoiceModel
    {

        /*****************Invoice Data***************/
        public int SlNo { get; set; }
        public string GSTIN { get; set; }
        public string Unit_Name { get; set; }
        public string Tran_SupTyp { get; set; }
        public string Tran_RegRev { get; set; }
        public string Tran_Typ { get; set; }
        public string Tran_IgstOnIntra { get; set; }
        public string Doc_Typ { get; set; }
        public string Doc_No { get; set; }
        public string Doc_Dt { get; set; }
        public string BillFrom_Gstin { get; set; }
        public string BillFrom_LglNm { get; set; }
        public string BillFrom_TrdNm { get; set; }
        public string BillFrom_Addr1 { get; set; }
        public string BillFrom_Addr2 { get; set; }
        public string BillFrom_Loc { get; set; }
        public string BillFrom_Pin { get; set; }
        public string BillFrom_Stcd { get; set; }
        public string BillFrom_Ph { get; set; }
        public string BillFrom_Em { get; set; }
        public string BillTo_Gstin { get; set; }
        public string BillTo_LglNm { get; set; }
        public string BillTo_TrdNm { get; set; }
        public string BillTo_Pos { get; set; }
        public string BillTo_Addr1 { get; set; }
        public string BillTo_Addr2 { get; set; }
        public string BillTo_Loc { get; set; }
        public string BillTo_Pin { get; set; }
        public string BillTo_Stcd { get; set; }
        public string BillTo_Ph { get; set; }
        public string BillTo_Em { get; set; }
        public string ShipFrom_Nm { get; set; }
        public string ShipFrom_Addr1 { get; set; }
        public string ShipFrom_Addr2 { get; set; }
        public string ShipFrom_Loc { get; set; }
        public string ShipFrom_Pin { get; set; }
        public string ShipFrom_Stcd { get; set; }
        public string ShipTo_Gstin { get; set; }
        public string ShipTo_LglNm { get; set; }
        public string ShipTo_TrdNm { get; set; }
        public string ShipTo_Addr1 { get; set; }
        public string ShipTo_Addr2 { get; set; }
        public string ShipTo_Loc { get; set; }
        public string ShipTo_Pin { get; set; }
        public string ShipTo_Stcd { get; set; }
        public string Item_SlNo { get; set; }
        public string Z { get; set; }
        public string Item_IsServc { get; set; }
        public string Item_HsnCd { get; set; }
        public string Item_Barcde { get; set; }
        public string Item_Qty { get; set; }
        public string Item_FreeQty { get; set; }
        public string Item_Unit { get; set; }
        public string Item_UnitPrice { get; set; }
        public string Item_TotAmt { get; set; }
        public string Item_Discount { get; set; }
        public string Item_PreTaxVal { get; set; }
        public string Item_AssAmt { get; set; }
        public string Item_GstRt { get; set; }
        public string Item_IgstAmt { get; set; }
        public string Item_CgstAmt { get; set; }
        public string Item_SgstAmt { get; set; }
        public string Item_CesRt { get; set; }
        public string Item_CesAmt { get; set; }
        public string Item_CesNonAdvlAmt { get; set; }
        public string Item_StateCesRt { get; set; }
        public string Item_StateCesAmt { get; set; }
        public string Item_StateCesNonAdvlAmt { get; set; }
        public string Item_OthChrg { get; set; }
        public string Item_TotItemVal { get; set; }
        public string Item_OrdLineRef { get; set; }
        public string Item_OrgCntry { get; set; }
        public string Item_PrdSlNo { get; set; }
        public string Item_Attrib_Nm { get; set; }
        public string Item_Attrib_Val { get; set; }
        public string Item_Bch_Nm { get; set; }
        public string Item_Bch_ExpDt { get; set; }
        public string Item_Bch_WrDt { get; set; }
        public string Val_AssVal { get; set; }
        public string Val_CgstVal { get; set; }
        public string Val_SgstVal { get; set; }
        public string Val_IgstVal { get; set; }
        public string Val_CesVal { get; set; }
        public string Val_StCesVal { get; set; }
        public string Val_RndOffAmt { get; set; }
        public string Val_TotInvVal { get; set; }
        public string Val_TotInvValFc { get; set; }
        public string Val_Discount { get; set; }
        public string Val_OthChrg { get; set; }
        public string Pay_Nm { get; set; }
        public string Pay_AccDet { get; set; }
        public string Pay_Mode { get; set; }
        public string Pay_FinInsBr { get; set; }
        public string Pay_PayTerm { get; set; }
        public string Pay_PayInstr { get; set; }
        public string Pay_CrTrn { get; set; }
        public string Pay_DirDr { get; set; }
        public string Pay_CrDay { get; set; }
        public string Pay_PaidAmt { get; set; }
        public string Pay_PaymtDue { get; set; }
        public string Ref_InvRm { get; set; }
        public string Ref_InvStDt { get; set; }
        public string Ref_InvEndDt { get; set; }
        public string Ref_PrecDoc_InvNo { get; set; }
        public string Ref_PrecDoc_InvDt { get; set; }
        public string Ref_PrecDoc_OthRefNo { get; set; }
        public string Ref_Contr_RecAdvRefr { get; set; }
        public string Ref_Contr_RecAdvDt { get; set; }
        public string Ref_Contr_TendRefr { get; set; }
        public string Ref_Contr_ContrRefr { get; set; }
        public string Ref_Contr_ExtRefr { get; set; }
        public string Ref_Contr_ProjRefr { get; set; }
        public string Ref_Contr_PORefr { get; set; }
        public string Ref_Contr_PORefDt { get; set; }
        public string AddlDoc_Url { get; set; }
        public string AddlDoc_Docs { get; set; }
        public string AddlDoc_Info { get; set; }
        public string Exp_ShipBNo { get; set; }
        public string Exp_ShipBDt { get; set; }
        public string Exp_Port { get; set; }
        public string Exp_RefClm { get; set; }
        public string Exp_ForCur { get; set; }
        public string Exp_CntCode { get; set; }
        public string Exp_ExpDuty { get; set; }
        public string Ewb_TransId { get; set; }
        public string Ewb_TransName { get; set; }
        public string Ewb_TransMode { get; set; }
        public string Ewb_Distance { get; set; }
        public string Ewb_TransDocNo { get; set; }
        public string Ewb_TransDocDt { get; set; }
        public string Ewb_VehNo { get; set; }
        public string Ewb_VehType { get; set; }
        public string CDKey { get; set; }
        public string EInvUserName { get; set; }
        public string EInvPassword { get; set; }
        public string EFUserName { get; set; }
        public string EFPassword { get; set; }
        public string GetQRImg { get; set; }
        public string GetSignedInvoice { get; set; }
        public string ImgSize { get; set; }
        public string RefNo{ get; set; }


}

}