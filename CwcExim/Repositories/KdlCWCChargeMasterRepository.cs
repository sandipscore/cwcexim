using CwcExim.DAL;
using CwcExim.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CwcExim.Repositories
{
    public class KdlCWCChargeMasterRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }
        public void GetAllCharges()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();

            _DBResponse = new DatabaseResponse();
            try
            {
                var ChargeName = DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = ChargeName;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
        public void GetBOLForDeliverApp(int DeliveryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Value = DeliveryId });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetBOLFrDeli", CommandType.StoredProcedure, dparam);
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            var BOL = "";
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    BOL = result["BOLNo"].ToString() + ":" + result["BOLDate"].ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Data = BOL;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = BOL;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void AddEditInvoice(KdlPostPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, int BranchId, int Uid,
           string Module, string CargoXML = "")
        {
            var cfsCodeWiseHtRate = HttpContext.Current.Session["lstCfsCodewiseRateHT"];

            string cfsCodeWiseHtRateXML = cfsCodeWiseHtRate == null ? "" : UtilityClasses.Utility.CreateXML(cfsCodeWiseHtRate);
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSpaceOccupied });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalValueOfCargo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditExpInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully";
                    try
                    {
                        var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjPostPaymentSheet.InvoiceHtml));
                        var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Invoice No. <span>", "Invoice No. <span>" + GeneratedClientId)));
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                        var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA1.ExecuteNonQuery("UpdateInvoiceHtmlReport", CommandType.StoredProcedure, DParam1);
                    }
                    catch { }
                    try
                    {
                        var LstParam2 = new List<MySqlParameter>();
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_cfsCodeWiseHtRateXML", MySqlDbType = MySqlDbType.Text, Value = cfsCodeWiseHtRateXML });
                        LstParam2.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam2 = LstParam2.ToArray();
                        var DA2 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA2.ExecuteNonQuery("cfsCodeWiseHtRate", CommandType.StoredProcedure, DParam2);
                    }
                    catch { }
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Payment Invoice Updated Successfully";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        public void GetDeliveryPaymentSheet_Kandla(string InvoiceDate, int DestuffingAppId, int DeliveryType, string DestuffingAppNo, string DestuffingAppDate, int PartyId, string PartyName,
         string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName,
         string InvoiceType, string LineXML, int InvoiceId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "LineXML", MySqlDbType = MySqlDbType.Text, Value = LineXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (KdlPrePaymentSheet)DataAccess.ExecuteDynamicSet<KdlPrePaymentSheet>("GetDeliveryPaymentSheet", DParam);

                var objPostPaymentSheet = new KdlPostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = DestuffingAppId;
                objPostPaymentSheet.RequestNo = DestuffingAppNo;
                objPostPaymentSheet.RequestDate = DestuffingAppDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode;
                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));
                objPostPaymentSheet.lstPreInvoiceCargo = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<KdlCWCPreInvoiceCargo>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.KdlCWClstPreInvoiceCargo));


                objPrePaymentSheet.KdlCWClstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + " , ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";

                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new KdlCWCPostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            LineNo = item.LineNo,
                            BOENo = item.BOENo,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,

                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new KdlCWCPostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        LineNo = item.LineNo,
                        BOENo = item.BOENo,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.KdlCWClstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.KdlCWClstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.KdlCWClstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.KdlCWClstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.KdlCWClstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.KdlCWClstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.KdlCWClstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");

                //******************************************************************************************************
                //Get Godown Type From Godown Master By GodownId
                if (Convert.ToInt32(HttpContext.Current.Session["BranchId"]) == 1)
                {
                    List<MySqlParameter> LstParam2 = new List<MySqlParameter>();
                    LstParam2.Add(new MySqlParameter
                    {
                        ParameterName = "in_godownid",
                        MySqlDbType = MySqlDbType.Int32,
                        Value = objPostPaymentSheet.lstPreInvoiceCargo.Count > 0 ? objPostPaymentSheet.lstPreInvoiceCargo[0].GodownId : 0
                    });

                    IDataParameter[] DParam2 = { };
                    DParam2 = LstParam2.ToArray();

                    var GodowntypeId = Convert.ToInt32(DataAccess.ExecuteScalar("getgodowntypeid", CommandType.StoredProcedure, DParam2));
                    objPostPaymentSheet.CalculateCharges(5, ChargeName, GodowntypeId);
                }
                else
                {
                    objPostPaymentSheet.CalculateCharges(5, ChargeName);
                }
                //*******************************************************************************************************
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
    }
}