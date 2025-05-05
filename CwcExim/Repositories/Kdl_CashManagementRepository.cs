using CwcExim.Areas.CashManagement.Models;
using CwcExim.Areas.Import.Models;
using CwcExim.DAL;
using CwcExim.Models;
using CwcExim.UtilityClasses;
using EinvoiceLibrary;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EinvoiceLibrary;
using CwcExim.Areas.Master.Models;
using CwcExim.Models;

namespace CwcExim.Repositories
{
    public class Kdl_CashManagementRepository
    {
        private DatabaseResponse _DBResponse;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        #region Cash Receipt

        public void GetPartyList()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPartyList", CommandType.StoredProcedure, DParam);
            var model = new Kol_CashReceiptModel();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.PartyDetail.Add(new Party { PartyId = Convert.ToInt32(Result["PartyId"]), PartyName = Convert.ToString(Result["PartyName"]) });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = model;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }


        public void GetCashRcptDetails(int PartyId, string PartyName)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCshDtlsAgnstParty", CommandType.StoredProcedure, DParam);

            var model = new Kol_CashReceiptModel();
            model.PartyId = PartyId;
            model.PartyName = PartyName;
            model.PayBy = PartyName;

            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    model.PayByDetail.Add(new PayBy
                    {
                        PayByEximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        PayByName = Convert.ToString(Result["PayBy"]),
                        Address = Convert.ToString(Result["Address"]),
                        StateName = Convert.ToString(Result["StateName"]),
                        CityName = Convert.ToString(Result["CityName"]),
                        CountryName = Convert.ToString(Result["CountryName"])
                    });
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.PdaAdjustdetail.Add(new PdaAdjust
                        {
                            PayByPdaId = Convert.ToInt32(Result["PDAId"]),
                            EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                            FolioNo = Result["FolioNo"].ToString(),
                            Opening = Convert.ToDecimal(Result["Balance"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.CashReceiptInvoiveMappingList.Add(new CashReceiptInvoiveMapping
                        {
                            InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                            InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                            AllTotalAmt = Convert.ToDecimal(Result["AllTotalAmt"]),
                            RoundUp = Convert.ToDecimal(Result["RoundUp"]),
                            InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]),
                            DueAmt = Convert.ToDecimal(Result["DueAmt"]),
                            AdjustmentAmt = Convert.ToDecimal(Result["AdjustmentAmt"])
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = model;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }

        // ADD Cash Receipt
        public void AddCashReceipt(Kol_CashReceiptModel ObjCashRcpt, string xml)
        {
            string GeneratedClientId = "0";
            string ReceiptNo = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjCashRcpt.ReceiptDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjCashRcpt.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayByPdaId", MySqlDbType = MySqlDbType.Int32, Value = ObjCashRcpt.PayByPdaId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PdaAdjust", MySqlDbType = MySqlDbType.Int16, Value = ObjCashRcpt.PdaAdjust == true ? 1 : 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PdaAdjustedAmount", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.Adjusted) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PdaClosing", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.Closing) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalPaymentReceipt", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.TotalPaymentReceipt) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TdsAmount", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.TdsAmount) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceValue", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.InvoiceValue) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjCashRcpt.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = ObjCashRcpt.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_xml", MySqlDbType = MySqlDbType.VarChar, Value = xml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "0" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_cashreceiptinvdtlsxml", MySqlDbType = MySqlDbType.VarChar, Value = ObjCashRcpt.CashReceiptInvDtlsHtml });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddCashReceiptMultiInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReceiptNo);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    var data = new { ReceiptNo = ReceiptNo, CashReceiptId = Convert.ToInt32(GeneratedClientId) };
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Add Cash Receipt Successfully.";
                    _DBResponse.Data = data;

                    try
                    {
                        var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjCashRcpt.InvoiceHtml));
                        var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Cash Receipt No.<span>", "Cash Receipt No. <span>" + ReceiptNo)));
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_RcptNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = ReceiptNo });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                        var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA1.ExecuteNonQuery("UpdateCashRcptHtmlReport", CommandType.StoredProcedure, DParam1);
                    }
                    catch { }
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }

        // Get Cash receipt Print
        public void GetCashRcptPrint(int CashReceiptId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.Int32, Value = CashReceiptId });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCashRcptMultiInvPrint", CommandType.StoredProcedure, DParam);
            var model = new PostPaymentSheet();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    // model.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    model.InvoiceNo = Result["InvoiceNo"].ToString();
                    // model.InvoiceType = Result["InvoiceType"].ToString();
                    model.InvoiceDate = Result["InvoiceDate"].ToString();
                    //  model.RequestId = Convert.ToInt32(Result["RequestId"]);
                    //  model.RequestNo = Result["RequestNo"].ToString();
                    //  model.RequestDate = Result["RequestDate"].ToString();
                    //  model.PartyId = Convert.ToInt32(Result["PartyId"]);
                    model.PartyName = Result["PartyName"].ToString();
                    model.PartyAddress = Result["PartyAddress"].ToString();
                    model.PartyState = Result["PartyState"].ToString();
                    model.PartyStateCode = Result["PartyStateCode"].ToString();
                    model.PartyGST = Result["PartyGST"].ToString();
                    //   model.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                    //   model.PayeeName = Result["PayeeName"].ToString();
                    model.TotalAmt = Convert.ToDecimal(Result["TotalAmt"]);
                    model.TotalDiscount = Convert.ToDecimal(Result["TotalDiscount"]);
                    model.TotalTaxable = Convert.ToDecimal(Result["TotalTaxable"]);
                    model.TotalCGST = Convert.ToDecimal(Result["TotalCGST"]);
                    model.TotalSGST = Convert.ToDecimal(Result["TotalSGST"]);
                    model.TotalIGST = Convert.ToDecimal(Result["TotalIGST"]);
                    model.CWCTotal = Convert.ToDecimal(Result["CWCTotal"]);
                    model.HTTotal = Convert.ToDecimal(Result["HTTotal"]);
                    model.AllTotal = Convert.ToDecimal(Result["AllTotal"]);
                    model.RoundUp = Convert.ToDecimal(Result["RoundUp"]);
                    model.InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]);
                    model.ShippingLineName = Result["ShippingLineName"].ToString();
                    model.CHAName = Result["CHAName"].ToString();
                    model.ImporterExporter = Result["ImporterExporter"].ToString();
                    model.BOENo = Result["BOENo"].ToString();
                    model.CFSCode = Result["CFSCode"].ToString();
                    // model.DestuffingDate = Result["DestuffingDate"].ToString();
                    model.StuffingDestuffingDate = Result["StuffingDestuffingDate"].ToString();
                    model.ArrivalDate = Result["ArrivalDate"].ToString();
                    model.TotalNoOfPackages = Convert.ToInt32(Result["TotalNoOfPackages"]);
                    model.TotalGrossWt = Convert.ToDecimal(Result["TotalGrossWt"]);
                    model.TotalWtPerUnit = Convert.ToDecimal(Result["TotalWtPerUnit"]);
                    model.TotalSpaceOccupied = Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                    model.TotalSpaceOccupiedUnit = Result["TotalSpaceOccupiedUnit"].ToString();
                    model.TotalValueOfCargo = Convert.ToDecimal(Result["TotalValueOfCargo"]);
                    model.PdaAdjustedAmount = Convert.ToDecimal(Result["PdaAdjustedAmount"]);
                    model.Remarks = Result["Remarks"].ToString();
                    // model.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    model.ImporterExporterType = Result["ImporterExporterType"].ToString();
                    model.BillType = Result["BillType"].ToString();
                    model.StuffingDestuffDateType = Result["StuffingDestuffDateType"].ToString();
                    model.CWCTDS = Convert.ToDecimal(Result["CWCTDS"]);
                    model.HTTDS = Convert.ToDecimal(Result["HTTDS"]);
                    model.TDS = Convert.ToDecimal(Result["TDS"]);
                    model.TDSCol = Convert.ToDecimal(Result["TDSCol"]);
                    model.DeliveryDate = Result["DeliveryDate"].ToString();
                    model.ApproveOn = Result["ApproveOn"].ToString();
                    // model.InvoiceHtml = Result["InvoiceHtml"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.lstPostPaymentCont.Add(new PostPaymentContainer
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            ArrivalDate = Convert.ToString(Result["ArrivalDate"]),
                            CartingDate = Result["CartingDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Result["CartingDate"]),
                            StuffingDate = Result["StuffingDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Result["StuffingDate"]),
                            DestuffingDate = Result["DestuffingDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Result["DestuffingDate"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Convert.ToString(Result["SpaceOccupiedUnit"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.lstPostPaymentChrg.Add(new PostPaymentCharge
                        {
                            Clause = Convert.ToString(Result["Clause"]),
                            ChargeName = Convert.ToString(Result["ChargeName"]),
                            ChargeType = Convert.ToString(Result["ChargeType"]),
                            SACCode = Convert.ToString(Result["SACCode"]),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.CompGST = Result["CompGST"].ToString();
                        model.CompPAN = Result["CompPAN"].ToString();
                        model.CompStateCode = Result["CompStateCode"].ToString();
                        model.ROAddress = Result["ROAddress"].ToString();
                        model.CompanyAddress = Result["CompanyAddress"].ToString();
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.CashReceiptDetails.Add(new CashReceipt
                        {
                            PaymentMode = Convert.ToString(Result["PayMode"]),
                            DraweeBank = Convert.ToString(Result["DraweeBank"]),
                            InstrumentNo = Convert.ToString(Result["InstrumentNo"]),
                            Date = Convert.ToString(Result["PayDate"]),
                            Amount = Convert.ToDecimal(Result["Amount"])
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = model;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }

        public void UpdatePrintHtml(int CashReceiptId, string htmlPrint)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.Int32, Value = CashReceiptId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Html", MySqlDbType = MySqlDbType.String, Value = htmlPrint });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("UpdateCashRcptHtmlForPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            
        }

        public void GetCashReceiptList()
        {

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCashReceiptList");
            List<Kdl_CashReceiptModel> lstCashReceipt = new List<Kdl_CashReceiptModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new Kdl_CashReceiptModel
                    {
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        TotalValue = Convert.ToDecimal(Result["Amount"]),
                        InvoiceId = Convert.ToInt32(Result["CashReceiptId"])

                    });


                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCashReceipt;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        #endregion

        #region Invoice Edit
        public bool HasColumn(IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        public void GetInvoiceForEdit(string Module)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = Module });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceForEdit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<EditInvoiceList> objInvoiceList = new List<EditInvoiceList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objInvoiceList.Add(new EditInvoiceList()
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objInvoiceList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        
        public void GetInvoiceDetailsForEdit(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceDetailsForEdit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PostPaymentSheet objPostPaymentSheet = new PostPaymentSheet();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPostPaymentSheet.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    objPostPaymentSheet.InvoiceType = Convert.ToString(Result["InvoiceType"]);
                    objPostPaymentSheet.DeliveryDate = Convert.ToString(Result["DeliveryDate"]);
                    objPostPaymentSheet.InvoiceDate = Convert.ToString(Result["InvoiceDate"]);
                    objPostPaymentSheet.RequestId = Convert.ToInt32(Result["StuffingReqId"]);
                    objPostPaymentSheet.RequestNo = Convert.ToString(Result["StuffingReqNo"]);
                    objPostPaymentSheet.RequestDate = Convert.ToString(Result["StuffingReqDate"]);
                    objPostPaymentSheet.PartyId = Convert.ToInt32(Result["PartyId"]);
                    objPostPaymentSheet.PartyName = Convert.ToString(Result["PartyName"]);
                    objPostPaymentSheet.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                    objPostPaymentSheet.PayeeName = Convert.ToString(Result["PayeeName"]);
                    objPostPaymentSheet.PartyGST = Convert.ToString(Result["PartyGSTNo"]);
                    objPostPaymentSheet.PartyAddress = Convert.ToString(Result["PartyAddress"]);
                    objPostPaymentSheet.PartyState = Convert.ToString(Result["PartyState"]);
                    objPostPaymentSheet.PartyStateCode = Convert.ToString(Result["PartyStateCode"]);
                    objPostPaymentSheet.TotalAmt = Convert.ToDecimal(Result["TotalAmt"]);
                    objPostPaymentSheet.TotalDiscount = Convert.ToDecimal(Result["TotalDiscount"]);
                    objPostPaymentSheet.TotalTaxable = Convert.ToDecimal(Result["TotalTaxable"]);
                    objPostPaymentSheet.TotalCGST = Convert.ToDecimal(Result["TotalCGST"]);
                    objPostPaymentSheet.TotalSGST = Convert.ToDecimal(Result["TotalSGST"]);
                    objPostPaymentSheet.TotalIGST = Convert.ToDecimal(Result["TotalIGST"]);
                    objPostPaymentSheet.CWCTotal = Convert.ToDecimal(Result["CWCTotal"]);
                    objPostPaymentSheet.HTTotal = Convert.ToDecimal(Result["HTTotal"]);
                    objPostPaymentSheet.CWCTDSPer = Convert.ToDecimal(Result["CWCTDSPerc"]);
                    objPostPaymentSheet.HTTDSPer = Convert.ToDecimal(Result["HTTDSPerc"]);
                    objPostPaymentSheet.CWCTDS = Convert.ToDecimal(Result["CWCTDS"]);
                    objPostPaymentSheet.HTTDS = Convert.ToDecimal(Result["HTTDS"]);
                    objPostPaymentSheet.TDS = Convert.ToDecimal(Result["TDS"]);
                    objPostPaymentSheet.TDSCol = Convert.ToDecimal(Result["TDSCol"]);
                    objPostPaymentSheet.AllTotal = Convert.ToDecimal(Result["AllTotal"]);
                    objPostPaymentSheet.RoundUp = Convert.ToDecimal(Result["RoundUp"]);
                    objPostPaymentSheet.InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]);
                    objPostPaymentSheet.ShippingLineName = Convert.ToString(Result["ShippingLinaName"]);
                    objPostPaymentSheet.CHAName = Convert.ToString(Result["CHAName"]);
                    objPostPaymentSheet.ImporterExporter = Convert.ToString(Result["ExporterImporterName"]);
                    objPostPaymentSheet.BOENo = Convert.ToString(Result["BOENo"]);
                    objPostPaymentSheet.BOEDate = Convert.ToString(Result["BOEDate"]);
                    objPostPaymentSheet.TotalNoOfPackages = Convert.ToInt32(Result["TotalNoOfPackages"]);
                    objPostPaymentSheet.TotalGrossWt = Convert.ToDecimal(Result["TotalGrossWt"]);
                    objPostPaymentSheet.TotalWtPerUnit = Convert.ToDecimal(Result["TotalWtPerUnit"]);
                    objPostPaymentSheet.TotalSpaceOccupied = Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = Convert.ToString(Result["TotalSpaceOccupiedUnit"]);
                    objPostPaymentSheet.TotalValueOfCargo = Convert.ToDecimal(Result["TotalValueOfCargo"]);
                    objPostPaymentSheet.CompGST = Convert.ToString(Result["CompGST"]);
                    objPostPaymentSheet.CompPAN = Convert.ToString(Result["CompPAN"]);
                    objPostPaymentSheet.CompStateCode = Convert.ToString(Result["CompStateCode"]);
                    objPostPaymentSheet.ApproveOn = Convert.ToString(Result["CstmExaminationDate"]);
                    objPostPaymentSheet.DestuffingDate = Convert.ToString(Result["DestuffingDate"]);
                    objPostPaymentSheet.StuffingDate = Convert.ToString(Result["StuffingDate"]);
                    objPostPaymentSheet.CartingDate = Convert.ToString(Result["CartingDate"]);
                    objPostPaymentSheet.ArrivalDate = Convert.ToString(Result["ArrivalDate"]);
                    objPostPaymentSheet.CFSCode = Convert.ToString(Result["CFSCode"]);
                    objPostPaymentSheet.Remarks = Convert.ToString(Result["Remarks"]);
                    objPostPaymentSheet.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    objPostPaymentSheet.Module = Convert.ToString(Result["Module"]);

                    if (Result.NextResult())
                    {
                        while (Result.Read())
                        {
                            objPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                            {
                                ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                                Clause = Convert.ToString(Result["Clause"]),
                                ChargeType = Convert.ToString(Result["ChargeType"]),
                                ChargeName = Convert.ToString(Result["ChargeName"]),
                                SACCode = Convert.ToString(Result["SACCode"]),
                                Quantity = Convert.ToInt32(Result["Quantity"]),
                                Rate = Convert.ToDecimal(Result["Rate"]),
                                Amount = Convert.ToDecimal(Result["Amount"]),
                                Discount = Convert.ToDecimal(Result["Discount"]),
                                Taxable = Convert.ToDecimal(Result["Taxable"]),
                                IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                                IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                                CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                                CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                                SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                                SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                                Total = Convert.ToDecimal(Result["Total"])
                            });
                        }
                    }
                    if (Result.NextResult())
                    {
                        while (Result.Read())
                        {
                            objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                            {
                                CFSCode = Convert.ToString(Result["CFSCode"]),
                                ContainerNo = Convert.ToString(Result["ContainerNo"]),
                                Size = Convert.ToString(Result["Size"]),
                                Reefer = Convert.ToInt16(Result["IsReefer"]),
                                Insured = Convert.ToInt16(Result["Insured"]),
                                RMS = Convert.ToInt16(Result["RMS"]),
                                CargoType = Convert.ToInt16(Result["CargoType"]),
                                ArrivalDate = Convert.ToString(Result["ArrivalDate"]),
                                DestuffingDate = Convert.ToDateTime(Result["DestuffingDate"]),
                                CartingDate = Convert.ToDateTime(Result["CartingDate"]),
                                StuffingDate = Convert.ToDateTime(Result["StuffingDate"]),
                                NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                                GrossWt = Convert.ToDecimal(Result["GrossWt"]),
                                WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                                SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                                SpaceOccupiedUnit = Convert.ToString(Result["SpaceOccupiedUnit"]),
                                CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                                Duty = Convert.ToDecimal(Result["Duty"]),
                                LCLFCL = HasColumn(Result, "LCLFCL")? Convert.ToString(Result["LCLFCL"]):""
                            });
                        }
                    }
                    if (Result.NextResult())
                    {
                        while (Result.Read())
                        {
                            objPostPaymentSheet.lstContWiseAmount.Add(new ContainerWiseAmount()
                            {
                                CFSCode = Convert.ToString(Result["CFSCode"]),
                                EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                                CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                                GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                                GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                                ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                                StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                                InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                                PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                                WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"]),
                                LineNo=HasColumn(Result, "LineNo")? Result["LineNo"].ToString():""
                            });
                        }
                    }
                    if (Result.NextResult())
                    {
                        while (Result.Read())
                        {
                            /*
                            PreInvoiceCargo objPre = new PreInvoiceCargo();
                            objPre.BOENo = Convert.ToString(Result["BOENo"]);
                            objPre.BOEDate = Convert.ToString(Result["BOEDate"]);
                            objPre.BOLNo = Convert.ToString(Result["BOLNo"]);
                            objPre.BOLDate = Convert.ToString(Result["BOLDate"]);
                            objPre.CargoDescription = Convert.ToString(Result["BOLDate"]);
                            objPre.GodownId = Convert.ToInt32(Result["GodownId"]);
                            objPre.GodownName = Convert.ToString(Result["GodownName"]);
                            objPre.GodownWiseLocationIds = Convert.ToString(Result["GdnWiseLctnIds"]);
                            objPre.GodownWiseLctnNames = Convert.ToString(Result["GdnWiseLctnNames"]);
                            objPre.CargoType = Convert.ToInt32(Result["CargoType"]);
                            objPre.DestuffingDate = Convert.ToString(Result["DestuffingDate"]);
                            objPre.CartingDate = Convert.ToString(Result["CartingDate"]);
                            objPre.StuffingDate = Convert.ToString(Result["StuffingDate"]);
                            objPre.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]);
                            objPre.GrossWeight = Convert.ToDecimal(Result["GrossWt"]);
                            objPre.WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]);
                            objPre.SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]);
                            objPre.SpaceOccupiedUnit = Convert.ToString(Result["SpaceOccupiedUnit"]);
                            objPre.CIFValue = Convert.ToDecimal(Result["CIFValue"]);
                            objPre.Duty = Convert.ToDecimal(Result["Duty"]);
                            objPostPaymentSheet.lstPreInvoiceCargo.Add(objPre);
                            */
                            
                            objPostPaymentSheet.lstPreInvoiceCargo.Add(new PreInvoiceCargo()
                            {
                                BOENo = Convert.ToString(Result["BOENo"]),
                                BOEDate = Convert.ToString(Result["BOEDate"]),
                                BOLNo = Convert.ToString(Result["BOLNo"]),
                                BOLDate = Convert.ToString(Result["BOLDate"]),
                                CargoDescription = Convert.ToString(Result["BOLDate"]),
                                GodownId = Convert.ToInt32(Result["GodownId"]),
                                GodownName = Convert.ToString(Result["GodownName"]),
                                GodownWiseLocationIds = Convert.ToString(Result["GdnWiseLctnIds"]),
                                GodownWiseLctnNames = Convert.ToString(Result["GdnWiseLctnNames"]),
                                CargoType = Convert.ToInt32(Result["CargoType"]),
                                DestuffingDate = Convert.ToString(Result["DestuffingDate"]),
                                CartingDate = Convert.ToString(Result["CartingDate"]),
                                StuffingDate = Convert.ToString(Result["StuffingDate"]),
                                NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                                GrossWeight = Convert.ToDecimal(Result["GrossWt"]),
                                WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                                SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                                SpaceOccupiedUnit = Convert.ToString(Result["SpaceOccupiedUnit"]),
                                CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                                Duty = Convert.ToDecimal(Result["Duty"])
                            });
                        }
                    }
                    objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").ToList().ForEach(item =>
                    {
                        if (!objPostPaymentSheet.ActualApplicable.Any(i => i == item.Clause))
                            objPostPaymentSheet.ActualApplicable.Add(item.Clause);
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPostPaymentSheet;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetBondInvoiceDetailsForEdit(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceDetailsForEdit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            BondPostPaymentSheet objBondPostPaymentSheet = new BondPostPaymentSheet();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objBondPostPaymentSheet.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    objBondPostPaymentSheet.InvoiceType = Convert.ToString(Result["InvoiceType"]);
                   // objBondPostPaymentSheet.DeliveryDate = Convert.ToString(Result["DeliveryDate"]);
                    objBondPostPaymentSheet.InvoiceDate = Convert.ToString(Result["InvoiceDate"]);
                    objBondPostPaymentSheet.RequestId = Convert.ToInt32(Result["StuffingReqId"]);
                    objBondPostPaymentSheet.RequestNo = Convert.ToString(Result["StuffingReqNo"]);
                    objBondPostPaymentSheet.RequestDate = Convert.ToString(Result["StuffingReqDate"]);
                    objBondPostPaymentSheet.PartyId = Convert.ToInt32(Result["PartyId"]);
                    objBondPostPaymentSheet.PartyName = Convert.ToString(Result["PartyName"]);
                    objBondPostPaymentSheet.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                    objBondPostPaymentSheet.PayeeName = Convert.ToString(Result["PayeeName"]);
                    objBondPostPaymentSheet.PartyGST = Convert.ToString(Result["PartyGSTNo"]);
                    objBondPostPaymentSheet.PartyAddress = Convert.ToString(Result["PartyAddress"]);
                    objBondPostPaymentSheet.PartyState = Convert.ToString(Result["PartyState"]);
                    objBondPostPaymentSheet.PartyStateCode = Convert.ToString(Result["PartyStateCode"]);
                    objBondPostPaymentSheet.TotalAmt = Convert.ToDecimal(Result["TotalAmt"]);
                    objBondPostPaymentSheet.TotalDiscount = Convert.ToDecimal(Result["TotalDiscount"]);
                    objBondPostPaymentSheet.TotalTaxable = Convert.ToDecimal(Result["TotalTaxable"]);
                    objBondPostPaymentSheet.TotalCGST = Convert.ToDecimal(Result["TotalCGST"]);
                    objBondPostPaymentSheet.TotalSGST = Convert.ToDecimal(Result["TotalSGST"]);
                    objBondPostPaymentSheet.TotalIGST = Convert.ToDecimal(Result["TotalIGST"]);
                    objBondPostPaymentSheet.CWCTotal = Convert.ToDecimal(Result["CWCTotal"]);
                    objBondPostPaymentSheet.HTTotal = Convert.ToDecimal(Result["HTTotal"]);
                    objBondPostPaymentSheet.CWCTDSPer = Convert.ToDecimal(Result["CWCTDSPerc"]);
                    objBondPostPaymentSheet.HTTDSPer = Convert.ToDecimal(Result["HTTDSPerc"]);
                    objBondPostPaymentSheet.CWCTDS = Convert.ToDecimal(Result["CWCTDS"]);
                    objBondPostPaymentSheet.HTTDS = Convert.ToDecimal(Result["HTTDS"]);
                    objBondPostPaymentSheet.TDS = Convert.ToDecimal(Result["TDS"]);
                    objBondPostPaymentSheet.TDSCol = Convert.ToDecimal(Result["TDSCol"]);
                    objBondPostPaymentSheet.AllTotal = Convert.ToDecimal(Result["AllTotal"]);
                    objBondPostPaymentSheet.RoundUp = Convert.ToDecimal(Result["RoundUp"]);
                    objBondPostPaymentSheet.InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]);
                  //  objBondPostPaymentSheet.ShippingLineName = Convert.ToString(Result["ShippingLinaName"]);
                    objBondPostPaymentSheet.CHAName = Convert.ToString(Result["CHAName"]);
                    objBondPostPaymentSheet.ImporterExporter = Convert.ToString(Result["ExporterImporterName"]);
                    objBondPostPaymentSheet.BOENo = Convert.ToString(Result["BOENo"]);
                    objBondPostPaymentSheet.BOEDate = Convert.ToString(Result["BOEDate"]);
                    objBondPostPaymentSheet.TotalNoOfPackages = Convert.ToInt32(Result["TotalNoOfPackages"]);
                    objBondPostPaymentSheet.TotalGrossWt = Convert.ToDecimal(Result["TotalGrossWt"]);
                    objBondPostPaymentSheet.TotalWtPerUnit = Convert.ToDecimal(Result["TotalWtPerUnit"]);
                  //  objBondPostPaymentSheet.TotalSpaceOccupied = Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                    objBondPostPaymentSheet.TotalSpaceOccupiedUnit = Convert.ToString(Result["TotalSpaceOccupiedUnit"]);
                  //  objBondPostPaymentSheet.TotalValueOfCargo = Convert.ToDecimal(Result["TotalValueOfCargo"]);
                    objBondPostPaymentSheet.CompGST = Convert.ToString(Result["CompGST"]);
                    objBondPostPaymentSheet.CompPAN = Convert.ToString(Result["CompPAN"]);
                    objBondPostPaymentSheet.CompStateCode = Convert.ToString(Result["CompStateCode"]);
                    objBondPostPaymentSheet.ApproveOn = Convert.ToString(Result["CstmExaminationDate"]);
                    //objBondPostPaymentSheet.DestuffingDate = Convert.ToString(Result["DestuffingDate"]);
                    //objBondPostPaymentSheet.StuffingDate = Convert.ToString(Result["StuffingDate"]);
                    //objBondPostPaymentSheet.CartingDate = Convert.ToString(Result["CartingDate"]);
                    //objBondPostPaymentSheet.ArrivalDate = Convert.ToString(Result["ArrivalDate"]);
                    //objBondPostPaymentSheet.CFSCode = Convert.ToString(Result["CFSCode"]);
                    objBondPostPaymentSheet.Remarks = Convert.ToString(Result["Remarks"]);
                    objBondPostPaymentSheet.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                  //  objBondPostPaymentSheet.Module = Convert.ToString(Result["Module"]);

                    if (Result.NextResult())
                    {
                        while (Result.Read())
                        {
                            objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                            {
                                ChargeId = objBondPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                                Clause = Convert.ToString(Result["Clause"]),
                                ChargeType = Convert.ToString(Result["ChargeType"]),
                                ChargeName = Convert.ToString(Result["ChargeName"]),
                                SACCode = Convert.ToString(Result["SACCode"]),
                                Quantity = Convert.ToInt32(Result["Quantity"]),
                                Rate = Convert.ToDecimal(Result["Rate"]),
                                Amount = Convert.ToDecimal(Result["Amount"]),
                                Discount = Convert.ToDecimal(Result["Discount"]),
                                Taxable = Convert.ToDecimal(Result["Taxable"]),
                                IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                                IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                                CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                                CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                                SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                                SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                                Total = Convert.ToDecimal(Result["Total"])
                            });
                        }
                    }
                    
                    objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").ToList().ForEach(item =>
                    {
                        if (!objBondPostPaymentSheet.ActualApplicable.Any(i => i == item.Clause))
                            objBondPostPaymentSheet.ActualApplicable.Add(item.Clause);
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objBondPostPaymentSheet;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        #endregion

        #region New Empty Container Paymentsheet New

        public void GetApplicationForEmptyContainerNew( int ApplicationId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ApplicationId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ApplicationFor });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetApplicationForEmptyContainerNew", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PaySheetStuffingRequest> objPaySheetStuffing = new List<PaySheetStuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new PaySheetStuffingRequest()
                    {
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHAName = Convert.ToString(Result["CHAName"]),
                        CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                        StuffingReqId = Convert.ToInt32(Result["DestuffingId"]),
                        StuffingReqNo = Convert.ToString(Result["DestuffingNo"]),
                        StuffingReqDate = Convert.ToString(Result["DestuffingDate"]),
                        Address = Convert.ToString(Result["Address"]),
                        DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        #endregion
        #region Rent Invoice
        public void GetPaymentParty()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetRentPaymentParty", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<Kdl_RentParty> objPaymentPartyName = new List<Kdl_RentParty>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new Kdl_RentParty()
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        Address = Convert.ToString(Result["Address"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        logic1 = Convert.ToInt32(Result["logic1"]),
                        logic2 = Convert.ToInt32(Result["logic2"]),
                        logic3 = Convert.ToInt32(Result["logic3"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentPartyName;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }


        public void AddEditRentInv(KDL_RentInvoice ObjPostPaymentSheet, String RentData, String ChargeData, int BranchId, int Uid, string month, int year, string Module,string ExportUnder)
        {
            string GeneratedClientId = "0";
            String mon = month.Substring(0, 3);
            int yr = year;
            // string dt = Convert.ToDateTime(ObjPostPaymentSheet.Date).ToString("yyyy-MM-dd hh:mm");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RentlXML", MySqlDbType = MySqlDbType.Text, Value = RentData });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChargeXML", MySqlDbType = MySqlDbType.Text, Value = ChargeData });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.VarChar, Value = mon });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Year", MySqlDbType = MySqlDbType.Int32, Value = yr });
            LstParam.Add(new MySqlParameter { ParameterName = "in_flag", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.addeditflg });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceXML", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditRentInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;

                    try
                    {
                        var Inv = GeneratedClientId.Split(',');
                        int i = 0;
                        foreach (var item in ObjPostPaymentSheet.lstPrePaymentContt)
                        {
                            var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(item.InvoiceHtml));
                            var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Invoice No. <span>", "Invoice No. <span>" + Inv[i])));
                            var LstParam1 = new List<MySqlParameter>();
                            LstParam1.Add(new MySqlParameter { ParameterName = "in_InvNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = Inv[i] });
                            LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                            LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                            IDataParameter[] DParam1 = LstParam1.ToArray();
                            var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                            DA1.ExecuteNonQuery("UpdateInvoiceHtmlReport", CommandType.StoredProcedure, DParam1);

                            i++;
                        }
                    }
                    catch { }


                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Rent Invoice Saved Successfully";
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;


                    try
                    {
                        var Inv = GeneratedClientId.Split(',');
                        int i = 0;
                        foreach (var item in ObjPostPaymentSheet.lstPrePaymentContt)
                        {
                            var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(item.InvoiceHtml));
                            var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Invoice No. <span>", "Invoice No. <span>" + Inv[i])));
                            var LstParam1 = new List<MySqlParameter>();
                            LstParam1.Add(new MySqlParameter { ParameterName = "in_InvNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = Inv[i] });
                            LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                            LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                            IDataParameter[] DParam1 = LstParam1.ToArray();
                            var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                            DA1.ExecuteNonQuery("UpdateInvoiceHtmlReport", CommandType.StoredProcedure, DParam1);

                            i++;
                        }
                    }
                    catch { }


                    _DBResponse.Message = "Rent Invoice Updated Successfully";
                }

                else if (Result == 3)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Update Rent Invoice Details As It Already Exist In Another Page";
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "SD Balance is not sufficient";
                }

                else if (Result == 5)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Zero Value Invoice can not be generated";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Duplicate Rent Invoice ";
                }

                else if (Result == -1)
                {
                    GeneratedClientId = " ";
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Rent Invoice Entry For this Month already exists. Populate data and Try to Update.";
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



        public void GetRentDet(string Mon, int Yr, int flg, int PartyCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.VarChar, Value = Mon });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Year", MySqlDbType = MySqlDbType.Int32, Value = Yr });
            LstParam.Add(new MySqlParameter { ParameterName = "flag", MySqlDbType = MySqlDbType.Int32, Value = flg });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.Int32, Value = PartyCode });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            KDL_RentInvoice objInvoice = new KDL_RentInvoice();
            IDataReader Result = DataAccess.ExecuteDataReader("GetRentInvoices", CommandType.StoredProcedure, DParam);
            try
            {

                _DBResponse = new DatabaseResponse();


                while (Result.Read())
                {
                    objInvoice.lstPrePaymentCont.Add(new WFLDRentDetails
                    {
                        Date = Result["InvoiceDate"].ToString(),
                        PartyId = Convert.ToInt32(Result["PartyId"].ToString()),
                        PartyName = Result["PartyName"].ToString(),
                        amount = Convert.ToDecimal(Result["Amount"].ToString()),
                        cgst = Convert.ToDecimal(Result["CGST"]),
                        sgst = Convert.ToDecimal(Result["SGST"]),
                        igst = Convert.ToDecimal(Result["IGST"]),
                        round_up = Convert.ToDecimal(Result["Round_Up"]),
                        total = Convert.ToDecimal(Result["Total"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),

                        GSTNo = Result["GstNo"].ToString(),

                    });

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPpgRentInvoiceCharge.Add(new WFLD_RentInvoiceCharge
                        {
                            Date = Result["InvoiceDate"].ToString(),
                            amount = Result["Amount"].ToString(),
                            cgst = Result["CGSTAmt"].ToString(),
                            sgst = Result["SGSTAmt"].ToString(),
                            igst = Result["IGSTAmt"].ToString(),
                            round_up = Result["Round_Up"].ToString(),
                            total = Result["Total"].ToString(),
                            ChargeHead = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            GSTNo = Result["GstNo"].ToString(),

                        });
                    }
                }

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objInvoice;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }


        public void PurposeListForChargeInvc()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "TruckSlipNo" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("RentChargeLst", CommandType.StoredProcedure, DParam);
            //IList<PurposeListForInvc> lstPurpose = new List<PurposeListForInvc>();
            List<SelectListItem> lstPurpose = new List<SelectListItem>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstPurpose.Add(new SelectListItem { Text = result["Exporter"].ToString(), Value = result["OperationSDesc"].ToString() });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstPurpose;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
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








        public void ListOfExpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            if (InvoiceDate != null) InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd");
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceDate", MySqlDbType = MySqlDbType.Date, Value = InvoiceDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Module });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofexpInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Kdl_ListOfInvoice> lstExpInvoice = new List<Kdl_ListOfInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstExpInvoice.Add(new Kdl_ListOfInvoice()
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        Module = Convert.ToString(Result["Module"]),
                        ModuleName = Convert.ToString(Result["ModuleName"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstExpInvoice;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        #endregion
        #region Misc. Invoice

        public void GetMiscInvoiceAmount(string purpose, string InvoiceType, int PartyId,string SEZ, decimal Amount)
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Value = purpose });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = Amount });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getMiscpaymentsheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Kdl_MiscInvoice ObjEntryThroughGate = new Kdl_MiscInvoice();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjEntryThroughGate.Amount = Convert.ToDecimal(Result["SUM"]);
                    ObjEntryThroughGate.CGST = Convert.ToDecimal(Result["CGSTAmt"]);
                    ObjEntryThroughGate.SGST = Convert.ToDecimal(Result["SGSTAmt"]);
                    ObjEntryThroughGate.IGST = Convert.ToDecimal(Result["IGSTAmt"]);
                    ObjEntryThroughGate.Total = Convert.ToDecimal(Result["NetAmt"]);
                    ObjEntryThroughGate.Round_up = Convert.ToDecimal(Result["Round_Up"]);
              


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void PurposeListForMiscInvc()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "TruckSlipNo" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("IncomecodeForMisleniousinvoice", CommandType.StoredProcedure, DParam);
            //IList<PurposeListForInvc> lstPurpose = new List<PurposeListForInvc>();
            List<SelectListItem> lstPurpose = new List<SelectListItem>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstPurpose.Add(new SelectListItem { Text = result["Exporter"].ToString(), Value = result["OperationSDesc"].ToString() });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstPurpose;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
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
        public void GetPaymentPartyMisc()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyfoMisc", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<PaymentPartyName> objPaymentPartyName = new List<PaymentPartyName>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new PaymentPartyName()
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentPartyName;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void AddMiscInv(MiscInvModel ObjPostPaymentSheet, int BranchId, int Uid, string Module,string ExportUnder)
        {
            string GeneratedClientId = "0";
            string dt = Convert.ToDateTime(ObjPostPaymentSheet.DeliveryDate).ToString("yyyy-MM-dd hh:mm");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(dt) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Purpose });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Amount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.SGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.IGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Round_up", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Round_up });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Total", MySqlDbType = MySqlDbType.Decimal, Value = (ObjPostPaymentSheet.Amount+ ObjPostPaymentSheet.CGST+ ObjPostPaymentSheet.SGST+ ObjPostPaymentSheet.IGST ) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_net", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Total });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Naration", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Naration });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditMiscInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    var Inv = GeneratedClientId.Split(',');


                    _DBResponse.Data = GeneratedClientId;   

                    try
                    {
                        int i = 0;
                            var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjPostPaymentSheet.InvoiceHtml));
                        strHtmlOriginal = strHtmlOriginal.Replace("996929", Inv[1]);

                        var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Invoice No. <span>", "Invoice No. <span>" + Inv[0])));
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = Inv[0] });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                            var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                            DA1.ExecuteNonQuery("UpdateInvoiceHtmlReport", CommandType.StoredProcedure, DParam1);

                        
                    }
                    catch { }


                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Misc. Invoice Saved Successfully";
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Misc. Invoice Updated Successfully";
                }

                else if (Result == 4)
                {
                    _DBResponse.Data = GeneratedClientId;






                    _DBResponse.Status = 4;
                    _DBResponse.Message = "SD Balance is not sufficient";
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

        #endregion

        #region Cancel Invoice
        public void GetHeaderIRNForInvoice()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            IDataReader Result = DataAccess.ExecuteDataReader("GetIRNheaderDetails", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            try
            {


                while (Result.Read())
                {
                    hp.ClientID = Result["ClientID"].ToString();
                    hp.ClientSecret = Result["ClientSecret"].ToString();
                    hp.GSTIN = Result["GSTIN"].ToString();
                    hp.UserName = Result["UserName"].ToString();
                    hp.Password = Result["Password"].ToString();
                    hp.AppKey = "";


                }

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = hp;

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }

        public void ListOfCancleInvoice(string InvoiceNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfCancleInvoiceNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CancelInvoice> lstInvoice = new List<CancelInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstInvoice.Add(new CancelInvoice()
                    {

                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = (Result["InvoiceNo"]).ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstInvoice;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void DetailsOfCancleInvoice(int invoiceid = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = invoiceid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("DetailsOfCancleInvoiceNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CancelInvoice objcancelinv = new CancelInvoice();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objcancelinv.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    objcancelinv.InvoiceNo = (Result["InvoiceNo"]).ToString();
                    objcancelinv.InvoiceDate = (Result["InvoiceDate"]).ToString();
                    objcancelinv.PartyName = (Result["PartyName"]).ToString();
                    objcancelinv.Amount = Convert.ToDecimal(Result["Amount"]);
                    objcancelinv.Irn = Convert.ToString(Result["irn"]);
                    objcancelinv.SupplyType = Convert.ToString(Result["SupplyType"]);
                    objcancelinv.AckNo = Convert.ToString(Result["AckNo"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objcancelinv;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void ViewDetailsOfCancleInvoice(int invoiceid = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = invoiceid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ViewDetailsOfCancleInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CancelInvoice objcancelinv = new CancelInvoice();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objcancelinv.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    objcancelinv.InvoiceNo = (Result["InvoiceNo"]).ToString();
                    objcancelinv.InvoiceDate = (Result["InvoiceDate"]).ToString();
                    objcancelinv.PartyName = (Result["PartyName"]).ToString();
                    objcancelinv.Amount = Convert.ToDecimal(Result["Amount"]);
                    objcancelinv.Irn = Convert.ToString(Result["irn"]);
                    objcancelinv.CancelRemarks = Convert.ToString(Result["CancelRemarks"]);
                    objcancelinv.CancelReason = Convert.ToString(Result["CancelReason"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objcancelinv;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void LstOfCancleInvoice(string InvoiceNo, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("CancleInvoiceList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CancelInvoice> lstInvoice = new List<CancelInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstInvoice.Add(new CancelInvoice()
                    {


                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = (Result["InvoiceNo"]).ToString(),
                        InvoiceDate = (Result["InvoiceDate"]).ToString(),
                        PartyName = (Result["PartyName"]).ToString(),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        CancelDate = (Result["CancelDate"]).ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstInvoice;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }


        public void AddEditCancleInvoice(CancelInvoice ObjCancelInvoice, int uid)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjCancelInvoice.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CancelDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjCancelInvoice.CancelDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CancelReason", MySqlDbType = MySqlDbType.VarChar, Value = ObjCancelInvoice.CancelReason });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CancelRemarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjCancelInvoice.CancelRemarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SupplyType", MySqlDbType = MySqlDbType.VarChar, Value = ObjCancelInvoice.SupplyType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            //LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditCancleInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Invoice Cancelled Saved Successfully-" + GeneratedClientId;
                    _DBResponse.Data = GeneratedClientId;
                }


                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        #endregion

        #region Credit Note
        public void GetInvoiceNoForCreaditNote(string CRDR)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = CRDR });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfInvcNofrCr", CommandType.StoredProcedure, DParam);
            List<ListOfInvoiceNo> lstInvcNo = new List<ListOfInvoiceNo>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstInvcNo.Add(new ListOfInvoiceNo { InvoiceId = Convert.ToInt32(Result["InvoiceId"]), InvoiceNo = Result["InvoiceNo"].ToString() });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstInvcNo;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public void GetInvoiceDetailsForCreaditNote(int InvoiceId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = "" });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfInvcNofrCr", CommandType.StoredProcedure, DParam);
            InvoiceDetails objInv = new InvoiceDetails();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objInv.Module = Result["Module"].ToString();
                    objInv.InvoiceType = Result["InvoiceType"].ToString();
                    objInv.InvoiceDate = Result["InvoiceDate"].ToString();
                    objInv.PartyId = Convert.ToInt32(Result["PartyId"]);
                    objInv.PartyName = Result["PartyName"].ToString();
                    objInv.PartyGSTNo = Result["PartyGSTNo"].ToString();
                    objInv.PartyAddress = Result["PartyAddress"].ToString();
                    objInv.PartyState = Result["PartyState"].ToString();
                    objInv.PartyStateCode = Result["PartyStateCode"].ToString();
                    objInv.SupplyType = Result["SupplyType"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInv.lstInvoiceCarges.Add(new InvoiceCarges
                        {
                            ChargesTypeId = Convert.ToInt32(Result["ChargesTypeId"]),
                            Clause = Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            RetValue = 0,
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            Total = 0
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objInv;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public void AddCreditNote(CreditNote objCR, string XML, string CRDR)
        {
            string ReturnObj = "";
            string Id = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRNoteId", MySqlDbType = MySqlDbType.Int32, Value = objCR.CRNoteId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRNoteDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objCR.CRNoteDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = objCR.InvoiceId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = objCR.InvoiceType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = objCR.InvoiceNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objCR.InvoiceDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = objCR.PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Size = 250, Value = objCR.PartyName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyGSTNo", MySqlDbType = MySqlDbType.VarChar, Size = 250, Value = objCR.PartyGSTNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Size = 250, Value = objCR.PartyAddress });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = objCR.PartyState });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = objCR.PartyStateCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = objCR.TotalAmt });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = objCR.RoundUp });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GrandTotal", MySqlDbType = MySqlDbType.Decimal, Value = objCR.GrandTotal });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objCR.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = objCR.Module });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRNoteHtml", MySqlDbType = MySqlDbType.Text, Value = "" });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ChargesXML", MySqlDbType = MySqlDbType.Text, Value = XML });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = CRDR });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddCRNote", CommandType.StoredProcedure, DParam, out Id, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    string html = GenerateHtmlForCRNote(Convert.ToInt32(Id), CRDR);
                    html = html.Replace("<br/>", "|_br_|");
                    string base64str = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(html));
                    try
                    {
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_CRNoteId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(Id) });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_CRNoteHtml", MySqlDbType = MySqlDbType.Text, Value = base64str });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                        var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA1.ExecuteNonQuery("UpdateCRNoteHtml", CommandType.StoredProcedure, DParam1);
                    }
                    catch { }
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Credit Note Saved Successfully";
                    _DBResponse.Data = ReturnObj;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        public void ListOfCRNote(string CRDR)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = CRDR });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfCRNote", CommandType.StoredProcedure, DParam);
            List<ListOfCRNote> lstCR = new List<ListOfCRNote>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCR.Add(new ListOfCRNote
                    {
                        CRNoteId = Convert.ToInt32(Result["CRNoteId"]),
                        CRNoteNo = Result["CRNoteNo"].ToString(),
                        CRNoteDate = Result["CRNoteDate"].ToString(),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCR;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public void PrintDetailsForCRNote(int CRNoteId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRNoteId", MySqlDbType = MySqlDbType.Int32, Value = CRNoteId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintOfCRNote", CommandType.StoredProcedure, DParam);
            PrintModelOfCr objCR = new PrintModelOfCr();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objCR.CompanyName = Result["CompanyName"].ToString();
                    objCR.CompanyAddress = Result["CompanyAddress"].ToString();
                    objCR.CompStateName = Result["CompStateName"].ToString();
                    objCR.CompStateCode = Result["CompStateCode"].ToString();
                    objCR.CompCityName = Result["CompCityName"].ToString();
                    objCR.CompGstIn = Result["CompGstIn"].ToString();
                    objCR.CompPan = Result["CompPan"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objCR.PartyName = Result["PartyName"].ToString();
                        objCR.PartyAddress = Result["PartyAddress"].ToString();
                        objCR.PartyCityName = Result["PartyCityName"].ToString();
                        objCR.PartyStateName = Result["PartyStateName"].ToString();
                        objCR.PartyStateCode = Result["PartyStateCode"].ToString();
                        objCR.PartyGSTIN = Result["PartyGSTIN"].ToString();
                        objCR.CRNoteNo = Result["CRNoteNo"].ToString();
                        objCR.CRNoteDate = Result["CRNoteDate"].ToString();
                        objCR.InvoiceNo = Result["InvoiceNo"].ToString();
                        objCR.InvoiceDate = Result["InvoiceDate"].ToString();
                        objCR.TotalAmt = Convert.ToDecimal(Result["TotalAmt"]);
                        objCR.RoundUp = Convert.ToDecimal(Result["RoundUp"]);
                        objCR.Remarks = Result["Remarks"].ToString();
                        objCR.GrandTotal = Convert.ToDecimal(Result["GrandTotal"]);

                        objCR.irn = Result["irn"].ToString();
                        objCR.SignedQRCode = Result["SignedQRCode"].ToString();
                        objCR.SupplyType = Result["SupplyType"].ToString();
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objCR.lstCharges.Add(new ChargesModel
                        {
                            ChargeName = Result["ChargeName"].ToString(),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            SACCode = Result["SACCode"].ToString()
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objCR;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public string GenerateHtmlForCRNote(int CRNoteId, string CRDR)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRNoteId", MySqlDbType = MySqlDbType.Int32, Value = CRNoteId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintOfCRNote", CommandType.StoredProcedure, DParam);
            PrintModelOfCr objCR = new PrintModelOfCr();
            _DBResponse = new DatabaseResponse();
            string html = "";
            try
            {
                while (Result.Read())
                {
                    objCR.CompanyName = Result["CompanyName"].ToString();
                    objCR.CompanyAddress = Result["CompanyAddress"].ToString();
                    objCR.CompStateName = Result["CompStateName"].ToString();
                    objCR.CompStateCode = Result["CompStateCode"].ToString();
                    objCR.CompCityName = Result["CompCityName"].ToString();
                    objCR.CompGstIn = Result["CompGstIn"].ToString();
                    objCR.CompPan = Result["CompPan"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objCR.PartyName = Result["PartyName"].ToString();
                        objCR.PartyAddress = Result["PartyAddress"].ToString();
                        objCR.PartyCityName = Result["PartyCityName"].ToString();
                        objCR.PartyStateName = Result["PartyStateName"].ToString();
                        objCR.PartyStateCode = Result["PartyStateCode"].ToString();
                        objCR.PartyGSTIN = Result["PartyGSTIN"].ToString();
                        objCR.CRNoteNo = Result["CRNoteNo"].ToString();
                        objCR.CRNoteDate = Result["CRNoteDate"].ToString();
                        objCR.InvoiceNo = Result["InvoiceNo"].ToString();
                        objCR.InvoiceDate = Result["InvoiceDate"].ToString();
                        objCR.TotalAmt = Convert.ToDecimal(Result["TotalAmt"]);
                        objCR.RoundUp = Convert.ToDecimal(Result["RoundUp"]);
                        objCR.GrandTotal = Convert.ToDecimal(Result["GrandTotal"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objCR.lstCharges.Add(new ChargesModel
                        {
                            ChargeName = Result["ChargeName"].ToString(),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            SACCode = Result["SACCode"].ToString()
                        });
                    }
                }
                string note = "";
                if (CRDR == "C")
                    note = "CREDIT NOTE";
                else
                    note = "DEBIT NOTE";
                string SACCode = "";
                objCR.lstCharges.Select(x => new { SACCode = x.SACCode }).Distinct().ToList().ForEach(item =>
                {
                    if (SACCode == "")
                        SACCode = item.SACCode;
                    else
                        SACCode = SACCode + "," + item.SACCode;
                });
                html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th colspan='2' style='text-align:center;padding:8px;'>Principle Place of Business: <span style='border-bottom:1px solid #000;'>______________________</span><br/>" + note + "</th></tr><tr><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Provider</th><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Receiver</th></tr></thead><tbody><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: " + objCR.CompanyName + "</td><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: <span>" + objCR.PartyName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Warehouse Address: <span>" + objCR.CompanyAddress + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>Address: <span>" + objCR.PartyAddress + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + objCR.CompCityName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + objCR.PartyCityName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + objCR.CompStateName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + objCR.PartyStateName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + objCR.CompStateCode + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + objCR.PartyStateCode + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>GSTIN: <span>" + objCR.CompGstIn + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'><span>GSTIN(if registered):" + objCR.PartyGSTIN + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>PAN:<span>" + objCR.CompPan + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'><span></span></td></tr><tr><td style='text-align:left;padding:8px;'>Debit/Credit Note Serial No: <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteNo + "</span><br/><br/>Date of Issue: <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteDate + "</span></td><td style='text-align:left;padding:8px;'>Accounting Code of <span>" + SACCode + "</span><br/><br/>Description of Services: <span>Other Storage & Warehousing Services</span></td></tr><tr><td colspan='2' style='text-align:left;padding:8px;'>Original Bill of Supply/Tax Invoice No: <span style='border-bottom:1px solid #000;'>" + objCR.InvoiceNo + "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Date: <span style='border-bottom:1px solid #000;'>" + objCR.InvoiceDate + "</span></td></tr><tr><td colspan='2'>";
                string htmltable = "<table style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;font-size:7pt;'><thead><tr><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Sl No.</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:20%;'>Particulars</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:7%;'>Taxable Value</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>CGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>SGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>IGST</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Total Amount</th></tr><tr><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Reasons for increase / decrease in original invoice</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th></tr></thead><tbody>";
                string tr = "";
                int Count = 1;
                decimal IGSTAmt = 0, CGSTAmt = 0, SGSTAmt = 0;
                objCR.lstCharges.ToList().ForEach(item =>
                {
                    tr += "<tr><td style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>" + Count + "</td><td style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'>" + item.ChargeName + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Taxable + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Total + "</td></tr>";
                    IGSTAmt += item.IGSTAmt;
                    CGSTAmt += item.CGSTAmt;
                    SGSTAmt += item.SGSTAmt;
                    Count++;
                });
                string AmountInWord = ConvertNumbertoWords((long)objCR.GrandTotal);
                string tfoot = "<tr><td style='border:1px solid #000;text-align:center;padding:5px;'></td><td style='border:1px solid #000;text-align:left;padding:5px;'></td><td style='border:1px solid #000;text-align:center;padding:5px;font-weight:600;'>Total</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + objCR.TotalAmt + "</td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in figure)</span> <span>" + objCR.GrandTotal + "</span></td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in words)</span> <span>" + AmountInWord + "</span></td></tr></tbody></table></td></tr><tr><td colspan='2' style='text-align:left;padding:5px;'>Note:<br/><span style='padding:8px;'>1. The invoice issued earlier can be modified/cancelled by way of Debit Note/Credit Note.</span><br/><span style='padding:8px;'>2. Credit Note is to be issued where excess amount cliamed in original invoice.</span><br/><span style='padding:8px;'>3. Debit Note is to be issued where less amount claimed in original invoice.</span></td></tr><tr><td></td><td style='text-align:left;padding:8px;font-weight:600;'>Signature: ____________________________<br/><br/>Name of the Signatory: __________________ <br/><br/>Designation/Status: ____________________ </td></tr><tr><td style='text-align:left;padding:5px;'>To,<br/><span style='border-bottom:1px solid #000;'>____________________________ <br/>____________________________<br/>____________________________<br/></span><br/><br/>Copy To:<br/>1. Duplicate Copy for RM, CWC,RO -<br/>2. Triplicate Copy for Warehouse</td></tr></tbody></table>";
                html = html + htmltable + tr + tfoot;
            }
            catch (Exception ex)
            {

            }
            return html;
        }
        public string ConvertNumbertoWords(long number)
        {
            if (number == 0) return "ZERO";
            if (number < 0) return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            if ((number / 1000000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " LAKHS ";
                number %= 1000000;
            }
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                number %= 100;
            }
            //if ((number / 10) > 0)  
            //{  
            // words += ConvertNumbertoWords(number / 10) + " RUPEES ";  
            // number %= 10;  
            //}  
            if (number > 0)
            {
                //if (words != "") words += "AND ";
                var unitsMap = new[]
                {
            "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
        };
                var tensMap = new[]
                {
            "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
        };
                if (number < 20) words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }
        #endregion
        #region IRN for Debit & Credit Note
        public void GetIRNForDebitCredit(String CRNoteNo, String TypeOfInv, String CRDR)
        {
            EinvoicePayload Obj = new EinvoicePayload();

            TranDtls tr = new TranDtls();
            DocDtls doc = new DocDtls();
            SellerDtls seller = new SellerDtls();
            BuyerDtls buyer = new BuyerDtls();
            DispDtls disp = new DispDtls();

            ShipDtls ship = new ShipDtls();
            BchDtls btc = new BchDtls();
            AttribDtls attr = new AttribDtls();
            ValDtls vald = new ValDtls();
            PayDtls payd = new PayDtls();
            RefDtls refd = new RefDtls();
            PrecDocDtls pred = new PrecDocDtls();
            ContrDtls Cont = new ContrDtls();
            AddlDocDtls addl = new AddlDocDtls();
            ExpDtls expd = new ExpDtls();
            EwbDtls ewb = new EwbDtls();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            LstParam.Add(new MySqlParameter { ParameterName = "in_CrNoteNo", MySqlDbType = MySqlDbType.VarChar, Value = CRNoteNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_type", MySqlDbType = MySqlDbType.VarChar, Value = TypeOfInv });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Value = CRDR });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetIRNDetailsForCreditDebitNote", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {


                while (Result.Read())
                {
                    Obj.Version = Result["Version"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        tr.TaxSch = Result["TaxSch"].ToString();
                        tr.SupTyp = Result["SupTyp"].ToString();
                        tr.RegRev = Result["RegRev"].ToString();
                        tr.EcmGstin = null;

                        tr.IgstOnIntra = Result["IgstOnIntra"].ToString();

                    }
                }




                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        doc.Typ = Result["Typ"].ToString();
                        doc.No = Result["No"].ToString();
                        doc.Dt = Result["Dt"].ToString();


                    }
                }



                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        seller.Gstin = Result["Gstin"].ToString();
                        seller.LglNm = Result["LglNm"].ToString();
                        seller.TrdNm = Result["TrdNm"].ToString() == "" ? null : Result["TrdNm"].ToString();
                        seller.Addr1 = Result["Addr1"].ToString();
                        seller.Addr2 = null;
                        seller.Loc = Result["Loc"].ToString();
                        seller.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        seller.Stcd = Result["Stcd"].ToString();
                        seller.Ph = null;// Result["Ph"].ToString().Length > 12 ? Result["Ph"].ToString().Substring(3, Result["Ph"].ToString().Length) : Result["Ph"].ToString();
                        seller.Em = null;//Result["Em"].ToString();
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        buyer.Gstin = Result["Gstin"].ToString();
                        buyer.LglNm = Result["LglNm"].ToString();
                        buyer.TrdNm = Result["TrdNm"].ToString() == "" ? null : Result["TrdNm"].ToString();
                        buyer.Addr1 = Result["Addr1"].ToString();
                        buyer.Addr2 = null;
                        buyer.Pos = Convert.ToString(Result["Stcd"]);
                        buyer.Loc = Result["Loc"].ToString();
                        buyer.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        buyer.Stcd = Result["Stcd"].ToString();
                        buyer.Ph = null;//Result["Ph"].ToString().Length > 12 ? Result["Ph"].ToString().Substring(3, Result["Ph"].ToString().Length) : Result["Ph"].ToString();
                        buyer.Em = null; //Result["Em"].ToString();
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        disp.Nm = Result["Nm"].ToString();
                        disp.Addr1 = Result["Addr1"].ToString();
                        disp.Addr2 = null;
                        disp.Loc = Result["Loc"].ToString();
                        disp.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        disp.Stcd = Result["Stcd"].ToString();

                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        ship.Gstin = Result["Gstin"].ToString();
                        ship.LglNm = Result["LglNm"].ToString();
                        ship.TrdNm = Result["TrdNm"].ToString() == "" ? null : Result["TrdNm"].ToString();
                        ship.Addr1 = Result["Addr1"].ToString();
                        ship.Addr2 = null;
                        ship.Loc = Result["Loc"].ToString();
                        ship.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        ship.Stcd = Result["Stcd"].ToString();

                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        btc.Nm = Result["Nm"].ToString();
                        btc.ExpDt = Result["ExpDt"].ToString();
                        btc.WrDt = Result["WrDt"].ToString();
                        // attr.Nm = Result["Nmm"].ToString();
                        // attr.Val= Result["Val"].ToString();
                        attr = null;
                        Obj.ItemList.Add(new ItemList
                        {
                            SlNo = Result["SlNo"].ToString(),
                            PrdDesc =null,// Result["PrdDesc"].ToString(),
                            IsServc = Result["IsServc"].ToString(),
                            HsnCd = Result["HsnCd"].ToString(),
                            Barcde = Result["Barcde"].ToString(),
                            Qty = Convert.ToDecimal(Result["Qty"].ToString()),
                            FreeQty = Convert.ToInt32(Result["FreeQty"].ToString()),
                            Unit =null,// Result["Unit"].ToString(),
                            UnitPrice = Convert.ToDecimal(Result["UnitPrice"].ToString()),
                            TotAmt = Convert.ToDecimal(Result["TotAmt"].ToString()),
                            Discount = Convert.ToInt32(Result["Discount"].ToString()),
                            PreTaxVal = Convert.ToInt32(Result["PreTaxVal"].ToString()),
                            AssAmt = Convert.ToDecimal(Result["AssAmt"].ToString()),
                            GstRt = Convert.ToDecimal(Result["GstRt"].ToString()),
                            IgstAmt = Convert.ToDecimal(Result["IgstAmt"].ToString()),
                            CgstAmt = Convert.ToDecimal(Result["CgstAmt"].ToString()),
                            SgstAmt = Convert.ToDecimal(Result["SgstAmt"].ToString()),
                            CesRt = Convert.ToInt32(Result["CesRt"].ToString()),
                            CesAmt = Convert.ToDecimal(Result["CesAmt"].ToString()),
                            CesNonAdvlAmt = Convert.ToInt32(Result["CesNonAdvlAmt"].ToString()),
                            StateCesRt = Convert.ToInt32(Result["StateCesRt"].ToString()),
                            StateCesAmt = Convert.ToDecimal(Result["StateCesAmt"].ToString()),
                            StateCesNonAdvlAmt = Convert.ToInt32(Result["StateCesNonAdvlAmt"].ToString()),
                            OthChrg = Convert.ToInt32(Result["OthChrg"].ToString()),
                            TotItemVal = Convert.ToDecimal(Result["TotItemVal"].ToString()),
                            OrdLineRef = Convert.ToString(Result["OrdLineRef"].ToString()),
                            OrgCntry = Result["OrgCntry"].ToString(),
                            PrdSlNo = Convert.ToString(Result["PrdSlNo"].ToString()),
                            BchDtls = null,
                            AttribDtls = null,

                        });



                    }


                }



                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        vald.AssVal = Convert.ToDecimal(Result["AssVal"].ToString());
                        vald.CgstVal = Convert.ToDecimal(Result["CgstVal"].ToString());
                        vald.SgstVal = Convert.ToDecimal(Result["SgstVal"].ToString());
                        vald.IgstVal = Convert.ToDecimal(Result["IgstVal"].ToString());
                        vald.CesVal = Convert.ToDecimal(Result["CesVal"].ToString());
                        vald.StCesVal = Convert.ToDecimal(Result["StCesVal"].ToString());
                        vald.Discount = Convert.ToDecimal(Result["Discount"].ToString());
                        vald.OthChrg = Convert.ToDecimal(Result["OthChrg"].ToString());
                        vald.RndOffAmt = Convert.ToDecimal(Result["RndOffAmt"].ToString());
                        vald.TotInvVal = Convert.ToDecimal(Result["TotInvVal"].ToString());
                        vald.TotInvValFc = Convert.ToDecimal(Result["TotInvValFc"].ToString());


                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        payd.Nm = Result["Nm"].ToString();
                        payd.AccDet = Result["AccDet"].ToString();
                        payd.Mode = Result["Mode"].ToString();
                        payd.FinInsBr = Result["FinInsBr"].ToString();
                        payd.PayTerm = Result["PayTerm"].ToString();
                        payd.PayInstr = Result["PayInstr"].ToString();
                        payd.CrTrn = Result["CrTrn"].ToString();
                        payd.DirDr = Result["DirDr"].ToString();
                        payd.CrDay = Result["CrDay"].ToString();
                        payd.PaidAmt = Result["PaidAmt"].ToString();
                        payd.PaymtDue = Result["PaymtDue"].ToString();


                    }
                }


                if (Result.NextResult())
                {
                    DocPerdDtls docp = new DocPerdDtls();
                    while (Result.Read())
                    {
                        refd.InvRm = Result["InvRm"].ToString();
                        docp.InvStDt = Result["InvStDt"].ToString();
                        docp.InvEndDt = Result["InvEndDt"].ToString();
                        refd.DocPerdDtls = docp;


                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        pred.InvNo = Result["InvNo"].ToString();
                        pred.InvDt = Result["InvDt"].ToString();
                        pred.OthRefNo = Result["OthRefNo"].ToString();


                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Cont.RecAdvRefr = Result["RecAdvRefr"].ToString();
                        Cont.RecAdvDt = Result["RecAdvDt"].ToString();
                        Cont.TendRefr = Result["TendRefr"].ToString();
                        Cont.ContrRefr = Result["ContrRefr"].ToString();
                        Cont.ExtRefr = Result["ExtRefr"].ToString();
                        Cont.ProjRefr = Result["ProjRefr"].ToString();
                        Cont.PORefr = Result["PORefr"].ToString();
                        Cont.PORefDt = Result["PORefDt"].ToString();



                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        addl.Url = Result["Url"].ToString();
                        addl.Docs = Result["Docs"].ToString();
                        addl.Info = Result["Info"].ToString();




                    }
                }

                if (Result.NextResult())
                {
                    // while (Result.Read())
                    // {



                    // Obj.ExpDtls.Add(new ExpDtls
                    // {
                    // ShipBNo = Result["ShipBNo"].ToString(),
                    // ShipBDt = Result["ShipBDt"].ToString(),
                    // Port = Result["Port"].ToString(),
                    // RefClm = Result["RefClm"].ToString(),
                    // ForCur = Result["ForCur"].ToString(),
                    // CntCode = Result["CntCode"].ToString(),
                    // ExpDuty = Result["ExpDuty"].ToString(),


                    // });












                    // }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ewb.TransId = Result["TransId"].ToString();
                        ewb.TransName = Result["TransName"].ToString();
                        ewb.Distance = Result["Distance"].ToString();
                        ewb.TransDocNo = Result["TransDocNo"].ToString();
                        ewb.TransDocDt = Result["TransDocDt"].ToString();
                        ewb.VehNo = Result["VehNo"].ToString();
                        ewb.VehType = Result["VehType"].ToString();
                        ewb.TransMode = Result["TransMode"].ToString();




                    }
                }

                //if (Result.NextResult())
                //{
                // while (Result.Read())
                // {
                // hp.ClientID = Result["ClientID"].ToString();
                // hp.ClientSecret = Result["ClientSecret"].ToString();
                // hp.GSTIN = Result["GSTIN"].ToString();
                // hp.UserName = Result["UserName"].ToString();
                // hp.Password = Result["Password"].ToString();
                // hp.AppKey = "";



                // }
                //}
                Obj.TranDtls = tr;
                Obj.DocDtls = doc;
                Obj.SellerDtls = seller;
                Obj.BuyerDtls = buyer;
                Obj.DispDtls = disp;
                Obj.ShipDtls = ship;
                Obj.AttribDtls = attr;
                Obj.ValDtls = vald;
                Obj.PayDtls = null;
                Obj.RefDtls = null;
                Obj.PrecDocDtls = pred;
                Obj.ContrDtls = Cont;
                Obj.AddlDocDtls = null;
                Obj.ExpDtls = null;
                // Obj.ExpDtls = expd;
                Obj.EwbDtls = null;

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Obj;

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public void GetIRNForB2CInvoiceForCreditDebitNote(String CrNoteNo, String TypeOfInv, String CRDR)
        {
            QrCodeData Obj = new QrCodeData();

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            LstParam.Add(new MySqlParameter { ParameterName = "in_CrNoteNo", MySqlDbType = MySqlDbType.VarChar, Value = CrNoteNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_type", MySqlDbType = MySqlDbType.VarChar, Value = TypeOfInv });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Value = CRDR });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetIrnB2CDetailsForCreditDebitNote", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {


                while (Result.Read())
                {
                    Obj.SellerGstin = Result["SellerGST"].ToString();
                    Obj.BuyerGstin = Result["BuyerGstin"].ToString();
                    Obj.DocNo = Result["DocNo"].ToString();
                    Obj.DocTyp = Result["DocTyp"].ToString();
                    Obj.DocDt = Result["DocDt"].ToString();
                    Obj.TotInvVal = (Int32)Convert.ToDecimal(Result["TotInvVal"].ToString());
                    Obj.ItemCnt = Convert.ToInt32(Result["ItemCnt"].ToString());
                    Obj.MainHsnCode = Result["MainHsnCode"].ToString();
                    //  Obj.Irn = Result["Irn"].ToString();
                    //   Obj.IrnDt = Result["IrnDt"].ToString();
                    //  Obj.iss = Result["iss"].ToString();


                }
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Obj;

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public void GetIRNForB2CInvoiceForCreditDebitNoteDR(String CrNoteNo, String TypeOfInv, String CRDR)
        {
            Kdl_IrnB2CDetails Obj = new Kdl_IrnB2CDetails();

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            LstParam.Add(new MySqlParameter { ParameterName = "in_CrNoteNo", MySqlDbType = MySqlDbType.VarChar, Value = CrNoteNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_type", MySqlDbType = MySqlDbType.VarChar, Value = TypeOfInv });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Value = CRDR });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetIrnB2CDetailsForCreditDebitNote", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {


                while (Result.Read())
                {
                    Obj.SellerGstin = Result["SellerGST"].ToString();
                    Obj.BuyerGstin = Result["BuyerGstin"].ToString();
                    Obj.DocNo = Result["DocNo"].ToString();
                    Obj.DocTyp = Result["DocTyp"].ToString();
                    Obj.DocDt = Result["DocDt"].ToString();
                    Obj.TotInvVal = (Int32)Convert.ToDecimal(Result["TotInvVal"].ToString());
                    Obj.ItemCnt = Convert.ToInt32(Result["ItemCnt"].ToString());
                    Obj.MainHsnCode = Result["MainHsnCode"].ToString();
                    Obj.ver = Convert.ToString(Result["ver"]);

                    Obj.tier = Convert.ToString(Result["tier"]);
                    Obj.tid = Convert.ToString(Result["tid"]);
                    //Obj.sign = Convert.ToString(Result["sign"]);
                    Obj.SGST = Convert.ToDecimal(Result["SGSTAmt"]);
                    Obj.CGST = Convert.ToDecimal(Result["CGSTAmt"]);
                    Obj.IGST = Convert.ToDecimal(Result["IGSTAmt"]);
                    Obj.CESS = 0;

                    Obj.qrMedium = Convert.ToInt32(Result["qrMedium"]);
                    Obj.QRexpire = Convert.ToString(Result["QRexpireDays"]);

                    Obj.pinCode = Convert.ToInt32(Result["pinCode"]);
                    Obj.pa = Convert.ToString(Result["pa"]);
                    Obj.orgId = Convert.ToInt32(Result["orgId"]);
                    Obj.mtid = Convert.ToString(Result["mtid"]);
                    Obj.msid = Convert.ToString(Result["msid"]);
                    Obj.mode = Convert.ToInt32(Result["mode"]);
                    Obj.mc = Convert.ToString(Result["mc"]);
                    Obj.mam = Convert.ToString(Result["InvoiceAmt"]);
                    Obj.GSTPCT = Convert.ToInt32(Result["IGSTPer"]);
                    Obj.GSTIncentive = 0;
                    Obj.mid = Convert.ToString(Result["mid"]);
                    Obj.InvoiceName = Convert.ToString(Result["PartyName"]);
                    Obj.tr = Convert.ToString(Result["tr"]);
                    Obj.pn = Convert.ToString(Result["pn"]);
                    Obj.gstIn = Convert.ToString(Result["SellerGST"]);
                    //  Obj.Irn = Result["Irn"].ToString();
                    //   Obj.IrnDt = Result["IrnDt"].ToString();
                    //  Obj.iss = Result["iss"].ToString();


                }
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Obj;

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public void AddEditIRNResponsecForCreditDebitNote(IrnResponse objOBL, string CrNoteNo, string CRDR)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CrNoteNo", MySqlDbType = MySqlDbType.VarChar, Value = CrNoteNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AckNo", MySqlDbType = MySqlDbType.String, Value = objOBL.AckNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AckDt", MySqlDbType = MySqlDbType.String, Value = objOBL.AckDt });
            lstParam.Add(new MySqlParameter { ParameterName = "in_irn", MySqlDbType = MySqlDbType.String, Value = objOBL.irn }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SignedInvoice", MySqlDbType = MySqlDbType.String, Value = objOBL.SignedInvoice });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SignedQRCode", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.SignedQRCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_QRCodeImageBase64", MySqlDbType = MySqlDbType.LongText, Value = objOBL.QRCodeImageBase64 });// Value = Convert.ToDateTime(objOBL.IGM_Date).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IrnStatus", MySqlDbType = MySqlDbType.String, Value = objOBL.IrnStatus });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EwbNo", MySqlDbType = MySqlDbType.DateTime, Value = objOBL.EwbNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EwbDt", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.EwbDt });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EwbValidTill", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.EwbValidTill });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IrnResponsecol", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Value = CRDR });

            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("Addeditirnresponcecrdr", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = (Result == 1) ? "IRN Saved Successfully" : "IRN Updated Successfully";
                    _DBResponse.Status = Result;
                }
                else
                {

                    log.Info("After Calling Stored Procedure Error" + " Credit Note No " + CrNoteNo + " signed Invoice: " + objOBL.SignedInvoice + " SignedQRCode " + objOBL.SignedQRCode);

                    _DBResponse.Data = null;
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
        }

        public void AddEditIRNB2CCreditDebitNote(String IRN, B2cQRCodeResponse QrCode, string CrNoteNo, String CRDR)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CrNoteNo", MySqlDbType = MySqlDbType.VarChar, Value = CrNoteNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_irn", MySqlDbType = MySqlDbType.String, Value = IRN });
            lstParam.Add(new MySqlParameter { ParameterName = "in_QRCodesigned", MySqlDbType = MySqlDbType.LongText, Value = QrCode.QrCodeJson });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Value = CRDR });

            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddeditirnB2CresponceCreditDebit", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = (Result == 1) ? "OBL Entry Saved Successfully" : "OBL Entry Updated Successfully";
                    _DBResponse.Status = Result;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "This Container information is already exists!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Can not update as seal cutting done!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Can not update as job order by road done!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 6)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Duplicate OBL No.!";
                    _DBResponse.Status = Result;
                }
                else
                {
                    _DBResponse.Data = null;
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
        }

        public void GetHeaderIRNForCreditDebitNote()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            IDataReader Result = DataAccess.ExecuteDataReader("GetIRNheaderDetails", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            try
            {


                while (Result.Read())
                {
                    hp.ClientID = Result["ClientID"].ToString();
                    hp.ClientSecret = Result["ClientSecret"].ToString();
                    hp.GSTIN = Result["GSTIN"].ToString();
                    hp.UserName = Result["UserName"].ToString();
                    hp.Password = Result["Password"].ToString();
                    hp.AppKey = "";


                }



                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = hp;

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        #endregion

        #region Direct Online Payment
        public void AddDirectPaymentVoucher(Kdl_DirectOnlinePayment objDOP, int uid)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_OnlinePayAckId", MySqlDbType = MySqlDbType.Int32, Value = objDOP.OnlinePayAckId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransId", MySqlDbType = MySqlDbType.Decimal, Value = objDOP.TransId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OrderId", MySqlDbType = MySqlDbType.Int64, Value = objDOP.OrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PaymentAmount", MySqlDbType = MySqlDbType.Decimal, Value = objDOP.PaymentAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OnlineFacilitationCharges", MySqlDbType = MySqlDbType.Decimal, Value = objDOP.OnlineFacilitationCharges });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = objDOP.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalPaymentAmount", MySqlDbType = MySqlDbType.Decimal, Value = objDOP.TotalPaymentAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Response", MySqlDbType = MySqlDbType.VarChar, Value = objDOP.Response });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Area", MySqlDbType = MySqlDbType.VarChar, Value = objDOP.Area });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditOnlinePayACK", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Saved Successfully-" + GeneratedClientId;
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "User not an SD Party!";
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        public void GetOnlinePayACK(long OrderId, int uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_OrderId", MySqlDbType = MySqlDbType.Int64, Value = OrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = uid });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOnlinePayACK", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PaymentGatewayRequest objPayRequest = new PaymentGatewayRequest();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    objPayRequest.billing_name = Convert.ToString(Result["billing_name"]);
                    objPayRequest.billing_address = Convert.ToString(Result["billing_address"]);
                    objPayRequest.billing_city = Convert.ToString(Result["billing_city"]);
                    objPayRequest.billing_state = Convert.ToString(Result["billing_state"]);
                    objPayRequest.billing_zip = Convert.ToString(Result["billing_zip"]);
                    objPayRequest.billing_country = Convert.ToString(Result["billing_country"]);
                    objPayRequest.billing_tel = Convert.ToDecimal(Result["billing_tel"]);
                    objPayRequest.billing_email = Convert.ToString(Result["billing_email"]);
                    objPayRequest.tid = Convert.ToDecimal(Result["tid"]);
                    objPayRequest.order_id = Convert.ToInt64(Result["order_id"]);
                    objPayRequest.amount = Convert.ToDecimal(Result["amount"]);
                    objPayRequest.merchant_id = Convert.ToDecimal(Result["merchant_id"]);
                    objPayRequest.cancel_url = Convert.ToString(Result["ccavenueCancelURL"]);
                    objPayRequest.redirect_url = Convert.ToString(Result["ccavenueRedirectURL"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPayRequest;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void AddPaymentGatewayRequest(PaymentGatewayRequest objPGR)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_tid", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.tid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_merchant_id", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.merchant_id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_id", MySqlDbType = MySqlDbType.Int64, Value = objPGR.order_id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_amount", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.amount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_currency", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.currency });
            LstParam.Add(new MySqlParameter { ParameterName = "in_redirect_url", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.redirect_url });
            LstParam.Add(new MySqlParameter { ParameterName = "in_cancel_url", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.cancel_url });
            LstParam.Add(new MySqlParameter { ParameterName = "in_language", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.language });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_name", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_name });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_address", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_city", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_city });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_state", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_state });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_zip", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_zip });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_country });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_tel", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_tel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_email", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_email });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_name", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_name });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_address", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_city", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_city });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_state", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_state });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_zip", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_zip });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_country });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_tel", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.delivery_tel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_issuing_bank", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.issuing_bank });
            LstParam.Add(new MySqlParameter { ParameterName = "in_mobile_number", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.mobile_number });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddPaymentGatewayRequest", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Request Saved Successfully-" + GeneratedClientId;
                    _DBResponse.Data = GeneratedClientId;
                }


                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }

        public void AddPaymentGatewayResponse(PaymentGatewayResponse objPGR)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_tracking_id", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.tracking_id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_id", MySqlDbType = MySqlDbType.Int64, Value = objPGR.order_id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_bank_ref_no", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.bank_ref_no });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_status", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_status });
            LstParam.Add(new MySqlParameter { ParameterName = "in_failure_message", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.failure_message });
            LstParam.Add(new MySqlParameter { ParameterName = "in_payment_mode", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.payment_mode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_status_code", MySqlDbType = MySqlDbType.Int16, Value = objPGR.status_code });
            LstParam.Add(new MySqlParameter { ParameterName = "in_status_message", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.status_message });
            LstParam.Add(new MySqlParameter { ParameterName = "in_amount", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.amount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_currency", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.currency });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_name", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_name });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_address", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_city", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_city });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_state", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_state });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_zip", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_zip });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_country });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_tel", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.billing_tel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_email", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_email });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_name", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_name });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_address", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_city", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_city });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_state", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_state });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_zip", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_zip });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.delivery_country });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_tel", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.delivery_tel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_vault", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.vault });
            LstParam.Add(new MySqlParameter { ParameterName = "in_offer_type", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.offer_type });
            LstParam.Add(new MySqlParameter { ParameterName = "in_offer_code", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.offer_code });
            LstParam.Add(new MySqlParameter { ParameterName = "in_discount_value", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.discount_value });
            LstParam.Add(new MySqlParameter { ParameterName = "in_mer_amount", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.mer_amount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_eci_value", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.eci_value });
            LstParam.Add(new MySqlParameter { ParameterName = "in_retry", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.retry });
            LstParam.Add(new MySqlParameter { ParameterName = "in_response_code", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.response_code });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_notes", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.billing_notes });
            LstParam.Add(new MySqlParameter { ParameterName = "in_trans_date", MySqlDbType = MySqlDbType.DateTime, Value = objPGR.trans_date });
            LstParam.Add(new MySqlParameter { ParameterName = "in_bin_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.bin_country });


            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddPaymentGatewayResponse", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Response Saved Successfully-" + GeneratedClientId;
                    _DBResponse.Data = GeneratedClientId;
                }


                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Duplicate Entry";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }

        public void GetOnlinePayAckList(int uid, long OrderId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OrderId", MySqlDbType = MySqlDbType.Int64, Value = OrderId });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOnlinePayAckList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Kdl_DirectOnlinePayment> objDOPList = new List<Kdl_DirectOnlinePayment>();
            Kdl_DirectOnlinePayment objDOP = new Kdl_DirectOnlinePayment();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objDOP = new Kdl_DirectOnlinePayment();
                    objDOP.OrderId = Convert.ToInt64(Result["OrderId"]);
                    objDOP.PaymentAmount = Convert.ToDecimal(Result["PaymentAmount"]);
                    objDOP.OnlineFacilitationCharges = Convert.ToDecimal(Result["OnlineFacilitationCharges"]);
                    objDOP.TotalPaymentAmount = Convert.ToDecimal(Result["TotalPaymentAmount"]);
                    objDOP.Response = Convert.ToString(Result["Response"]);
                    objDOP.CreatedOn = Convert.ToString(Result["TransDate"]);

                    objDOPList.Add(objDOP);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objDOPList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }


        public void GetOnlineConfirmPayment(decimal TotalAmt, long OrderId)
        {
            int Status = 0;
            string GeneratedClientId = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_OrderId", MySqlDbType = MySqlDbType.Int64, Value = OrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalPaymentAmount", MySqlDbType = MySqlDbType.Decimal, Value = TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ConfirmDirectOnlinePayment", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            List<Kdl_DirectOnlinePayment> objDOPList = new List<Kdl_DirectOnlinePayment>();
            Kdl_DirectOnlinePayment objDOP = new Kdl_DirectOnlinePayment();
            try
            {

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = GeneratedClientId;

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        #endregion

        #region Online Payment Receipt
        public void OnlinePaymentReceiptDetails(string FromDate, string ToDate, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOnlinePaymentReceiptDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstReceipt = new List<OnlinePaymentReceipt>();
            while (Result.Read())
            {

                Status = 1;

                lstReceipt.Add(new OnlinePaymentReceipt
                {
                    // PeriodFrom=Convert.ToString(Result["FromDate"])
                    ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                    PayerName = Convert.ToString(Result["PayerName"]),
                    PaymentDate = Convert.ToString(Result["PaymentDate"]),
                    ReferenceNo = Convert.ToString(Result["OrderId"]),
                    AmountPaid = Convert.ToDecimal(Result["TotalPayAmount"]),
                    PayerRemarks = Convert.ToString(Result["PayerRemarks"])
                });
            }

            if (Status == 1)
            {
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = lstReceipt;
            }
            else
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;

            }
        }

        public void GetOnlinePaymentReceiptList(string SearchValue = "", int Pages = 0)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Pages", MySqlDbType = MySqlDbType.Int32, Value = Pages });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOnlinePaymentReceiptList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstCashReceipt = new List<OnlinePaymentReceipt>();
            int Status = 0;

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new OnlinePaymentReceipt
                    {
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                        PayerName = Convert.ToString(Result["PartyName"]),
                        AmountPaid = Convert.ToDecimal(Result["Amount"]),
                        CashReceiptId = Convert.ToInt32(Result["CashReceiptId"])

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCashReceipt;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        #endregion

        #region Online Payment Against Invoice
        public void ListOfPendingInvoice(int uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPendingInvoiceOnlinePtm", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CancelInvoice> lstInvoice = new List<CancelInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstInvoice.Add(new CancelInvoice()
                    {

                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = (Result["InvoiceNo"]).ToString(),
                        Amount = Convert.ToDecimal(Result["InvoiceAmt"]),
                        InvoiceDate = (Result["InvoiceDate"]).ToString(),
                        PayeeName = (Result["PayeeId"]).ToString(),
                        PartyName = (Result["PartyId"]).ToString()


                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstInvoice;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void AddEditOnlinePaymentAgainstInvoice(Kdl_OnlinePaymentAgainstInvoice objDOP, int uid, string XML)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_OnlinePayAckId", MySqlDbType = MySqlDbType.Int32, Value = objDOP.OnlinePayAckId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransId", MySqlDbType = MySqlDbType.Decimal, Value = objDOP.TransId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OrderId", MySqlDbType = MySqlDbType.Int64, Value = objDOP.OrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PaymentAmount", MySqlDbType = MySqlDbType.Decimal, Value = objDOP.Total });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OnlineFacilitationCharges", MySqlDbType = MySqlDbType.Decimal, Value = objDOP.OnlineFacilitaionCharge });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalPaymentAmount", MySqlDbType = MySqlDbType.Decimal, Value = objDOP.TotalPayAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Response", MySqlDbType = MySqlDbType.VarChar, Value = objDOP.Response });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceXML", MySqlDbType = MySqlDbType.LongText, Value = XML });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditOnlinePayInvoiceAgainst", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Saved Successfully-" + GeneratedClientId;
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Duplicate Entry";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Invalid Amount..";
                    _DBResponse.Data = GeneratedClientId;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }

        public void GetOnlinePaymentAgainstInvoice(string SearchValue = "", int Pages = 0)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Pages", MySqlDbType = MySqlDbType.Int32, Value = Pages });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOnlinePaymentInvoiceAgainstList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Kdl_OnlinePaymentAgainstInvoice> lstCashReceipt = new List<Kdl_OnlinePaymentAgainstInvoice>();
            int Status = 0;

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new Kdl_OnlinePaymentAgainstInvoice
                    {
                        OrderId = Convert.ToInt64(Result["OrderId"]),
                        TotalPayAmount = Convert.ToDecimal(Result["TotalPaymentAmount"]),
                        Response = Convert.ToString(Result["Response"]),
                        lstInvoiceDetails = Convert.ToString(Result["InvoiceNo"]),
                        OnlinePayAckId = Convert.ToInt32(Result["OnlinePayAckId"])

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCashReceipt;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        #endregion

        #region Online Payment Receipt Against Invoice
        public void OnlinePaymentReceiptDetailsAgainstInvoice(string FromDate, string ToDate, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOnlinePaymentReceiptDetailsAgainstInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstReceipt = new List<OnlinePaymentReceipt>();
            while (Result.Read())
            {

                Status = 1;

                lstReceipt.Add(new OnlinePaymentReceipt
                {
                    // PeriodFrom=Convert.ToString(Result["FromDate"])
                    ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                    PayerName = Convert.ToString(Result["PayerName"]),
                    PaymentDate = Convert.ToString(Result["PaymentDate"]),
                    ReferenceNo = Convert.ToString(Result["OrderId"]),
                    AmountPaid = Convert.ToDecimal(Result["TotalPayAmount"]),
                    PayerRemarks = Convert.ToString(Result["PayerRemarks"])
                });
            }

            if (Status == 1)
            {
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = lstReceipt;
            }
            else
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;

            }
        }

        public void GetOnlinePaymentReceiptListAgainstInvoice(string SearchValue = "", int Pages = 0)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Pages", MySqlDbType = MySqlDbType.Int32, Value = Pages });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOnlinePaymentReceiptListAgainstInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstCashReceipt = new List<OnlinePaymentReceipt>();
            int Status = 0;

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new OnlinePaymentReceipt
                    {
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                        PayerName = Convert.ToString(Result["PartyName"]),
                        AmountPaid = Convert.ToDecimal(Result["Amount"]),
                        CashReceiptId = Convert.ToInt32(Result["CashReceiptId"])

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCashReceipt;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        #endregion

        #region Online On Account Payment Receipt
        public void OnlineOAPaymentReceiptDetails(string FromDate, string ToDate, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOnlineOnAccountPaymentReceipt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstReceipt = new List<OnlinePaymentReceipt>();
            while (Result.Read())
            {

                Status = 1;

                lstReceipt.Add(new OnlinePaymentReceipt
                {
                    // PeriodFrom=Convert.ToString(Result["FromDate"])
                    ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                    PayerName = Convert.ToString(Result["PayerName"]),
                    PaymentDate = Convert.ToString(Result["PaymentDate"]),
                    ReferenceNo = Convert.ToString(Result["OrderId"]),
                    AmountPaid = Convert.ToDecimal(Result["TotalPayAmount"]),
                    PayerRemarks = Convert.ToString(Result["PayerRemarks"])
                });
            }

            if (Status == 1)
            {
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = lstReceipt;
            }
            else
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;

            }
        }

        public void GetOnlineOAPaymentReceiptList(string SearchValue = "", int Pages = 0)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Pages", MySqlDbType = MySqlDbType.Int32, Value = Pages });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOnlineOAPaymentReceiptList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstCashReceipt = new List<OnlinePaymentReceipt>();
            int Status = 0;

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new OnlinePaymentReceipt
                    {
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                        PayerName = Convert.ToString(Result["PartyName"]),
                        AmountPaid = Convert.ToDecimal(Result["Amount"]),
                        CashReceiptId = Convert.ToInt32(Result["CashReceiptId"])

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCashReceipt;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        #endregion

        #region TransactionStatusEnquiry


        public void GetInvoiceNoForTransactionStatusEnquiry(string InvoiceNo, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceNoForTransactionStatusEnquiry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<TransactionStatusEnquiry> lstInvoice = new List<TransactionStatusEnquiry>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstInvoice.Add(new TransactionStatusEnquiry()
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = (Result["InvoiceNo"]).ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstInvoice;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetQRRequestDetails()
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("GetQRRequestDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Result;

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Dispose();
            }
        }

        public void AddQRTransactionAck(string JSON_XML, int InvoiceId)
        {

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_JSON_XML", MySqlDbType = MySqlDbType.LongText, Value = JSON_XML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddQRTransactionAck", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Save Successfully.";
                    _DBResponse.Data = null;
                }

                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }

        }



        public void GetOrderNoForTransactionStatusEnquiry(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOrderNoByInvoiceId", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            string OiderNo = "";
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    OiderNo = Convert.ToString(Result["OrderId"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = OiderNo;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }


        public void AddCcAvnueTransactionUpdate(CcAvnResponseJsonModel vm)
        {

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_amt", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(vm.order_amt) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_date_time", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(vm.order_date_time) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_bill_name", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.order_bill_name) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_bill_address", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.order_bill_address) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_bill_zip", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.order_ship_zip) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_bill_tel", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.order_ship_tel) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_bill_email", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.order_ship_email) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_ip", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.order_ip) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_card_name", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.order_card_name) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_device_type", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.order_device_type) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_no", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.order_no) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_reference_no", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.reference_no) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_enquiry_by", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("UpdateCCAvenueTransactionStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Save Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Duplicate Entry ";
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }

        }


        public void CheckResponceOrderIdAndAmount(string OrderId, decimal Amount)
        {

            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_OrderId", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(OrderId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToString(Amount) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("CheckOrderIdAndAmount", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Save Successfully.";
                    _DBResponse.Data = Result;
                }

                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = Result;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }

        }

        #endregion


        #region Acknowledgement View List
        public void AcknowledgementViewList(string FromDate, string ToDate, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = FromDate == null ? null : Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = ToDate == null ? null : Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAcknowledgementViewList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Kdl_AcknowledgementViewList> lstReceipt = new List<Kdl_AcknowledgementViewList>();


            try
            {
                while (Result.Read())
                {

                    Status = 1;

                    lstReceipt.Add(new Kdl_AcknowledgementViewList
                    {
                        // PeriodFrom=Convert.ToString(Result["FromDate"])
                        TransId = Convert.ToString(Result["TransId"]),
                        OrderId = Convert.ToString(Result["OrderId"]),
                        OnlineFacilitationCharges = Convert.ToDecimal(Result["OnlineFacilitationCharges"]),
                        PaymentAmount = Convert.ToDecimal(Result["PaymentAmount"]),
                        TotalPaymentAmount = Convert.ToDecimal(Result["TotalPaymentAmount"]),
                        Response = Convert.ToString(Result["Response"]),
                        TransactionDate = Convert.ToString(Result["TransactionDate"]),
                        BankRef = Convert.ToString(Result["reference_no"]),
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstReceipt;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;

                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }

        }
        #endregion


        #region BQR Payment Receipt Against Invoice
        public void BQRPaymentReceiptDetailsAgainstInvoice(string FromDate, string ToDate, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBRQPaymentReceiptDetailsAgainstInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstReceipt = new List<OnlinePaymentReceipt>();
            while (Result.Read())
            {

                Status = 1;

                lstReceipt.Add(new OnlinePaymentReceipt
                {
                    // PeriodFrom=Convert.ToString(Result["FromDate"])
                    ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                    PayerName = Convert.ToString(Result["PayerName"]),
                    PaymentDate = Convert.ToString(Result["PaymentDate"]),
                    ReferenceNo = Convert.ToString(Result["OrderId"]),
                    AmountPaid = Convert.ToDecimal(Result["TotalPayAmount"]),
                    PayerRemarks = Convert.ToString(Result["PayerRemarks"])
                });
            }

            if (Status == 1)
            {
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = lstReceipt;
            }
            else
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;

            }
        }

        public void GetBQRPaymentReceiptListAgainstInvoice(string SearchValue = "", int Pages = 0)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Pages", MySqlDbType = MySqlDbType.Int32, Value = Pages });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBQRPaymentReceiptListAgainstInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstCashReceipt = new List<OnlinePaymentReceipt>();
            int Status = 0;

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new OnlinePaymentReceipt
                    {
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                        PayerName = Convert.ToString(Result["PartyName"]),
                        AmountPaid = Convert.ToDecimal(Result["Amount"]),
                        CashReceiptId = Convert.ToInt32(Result["CashReceiptId"])

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCashReceipt;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        #endregion


        #region Pull
        public void BQRPaymentReceiptPullData()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBRQInvoiceStatusList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstReceipt = new List<OnlinePaymentReceipt>();
            while (Result.Read())
            {

                Status = 1;

                lstReceipt.Add(new OnlinePaymentReceipt
                {


                    ReferenceNo = Convert.ToString(Result["InvoiceId"]),

                });
            }

            if (Status == 1)
            {
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = lstReceipt;
            }
            else
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;

            }
        }


        public void AddPaymentGatewayResponseBQR(CcAvnResponseJsonModel objPGR)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_tracking_id", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.reference_no });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_id", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_no });
            LstParam.Add(new MySqlParameter { ParameterName = "in_bank_ref_no", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bank_ref_no });
            LstParam.Add(new MySqlParameter { ParameterName = "in_order_status", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_status });
            LstParam.Add(new MySqlParameter { ParameterName = "in_failure_message", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_notes });
            LstParam.Add(new MySqlParameter { ParameterName = "in_payment_mode", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_option_type });
            LstParam.Add(new MySqlParameter { ParameterName = "in_status_code", MySqlDbType = MySqlDbType.Int16, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_status_message", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bank_response });
            LstParam.Add(new MySqlParameter { ParameterName = "in_amount", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.order_amt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_currency", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_currncy });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_name", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_name });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_address", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_city", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_city });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_state", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_state });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_zip", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_zip });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_country });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_tel", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_tel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_email", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_email });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_name", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_name });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_address", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_city", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_city });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_state", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_state });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_zip", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_zip });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_country });
            LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_tel", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_tel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_vault", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_offer_type", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_offer_code", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_discount_value", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_mer_amount", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.order_capt_amt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_eci_value", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_retry", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_response_code", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_billing_notes", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_notes });
            LstParam.Add(new MySqlParameter { ParameterName = "in_trans_date", MySqlDbType = MySqlDbType.DateTime, Value = objPGR.order_date_time });
            LstParam.Add(new MySqlParameter { ParameterName = "in_bin_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_country });


            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddPaymentGatewayResponseTranBqr", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Response Saved Successfully-" + GeneratedClientId;
                    _DBResponse.Data = GeneratedClientId;
                }


                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Duplicate Entry";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        #endregion
        #region Party Wise TDS Deposit
        public void GetAllEximTraderFilterWise(string FilterText)
        {
            // Type of Text for the parameter
            // 'All' For No Filter
            // 'Importer' For Importer=1
            // 'Exporter' For Exporter=1
            // 'ShippingLine' For ShippingLine=1
            // 'CHA' For CHA=1

            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_FilterText", MySqlDbType = MySqlDbType.String, Value = FilterText });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEximTrader", CommandType.StoredProcedure, DParam);
            List<Party> model = new List<Party>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.Add(new Party { PartyId = Convert.ToInt32(Result["PartyId"]), PartyName = Convert.ToString(Result["PartyName"]) });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = model;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }

        public void AddEditPartyWiseTDSDeposit(PartyWiseTDSDeposit objPartyWiseTDSDeposit)
        {
            string GeneratedClientId = "0";
            string ReceiptNo = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = objPartyWiseTDSDeposit.Id });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = objPartyWiseTDSDeposit.PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CirtificateNo", MySqlDbType = MySqlDbType.String, Value = objPartyWiseTDSDeposit.CirtificateNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.String, Value = objPartyWiseTDSDeposit.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CirtificateDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objPartyWiseTDSDeposit.CirtificateDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FinancialYearFrom", MySqlDbType = MySqlDbType.Int32, Value = objPartyWiseTDSDeposit.FinancialYear });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FinancialYearTo", MySqlDbType = MySqlDbType.Int32, Value = objPartyWiseTDSDeposit.FinancialYearNext });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TdsQuarter", MySqlDbType = MySqlDbType.String, Value = objPartyWiseTDSDeposit.TdsQuarter });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = objPartyWiseTDSDeposit.Amount });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "0" });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditPartyWiseTDSDeposit", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReceiptNo);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = (Result == 1) ? "Party Wise TDS Deposit Saved Successfully" : "Party Wise TDS Deposit Updated Successfully";
                    _DBResponse.Status = Result;

                    var data = new { ReceiptNo = ReceiptNo, CashReceiptId = Convert.ToInt32(GeneratedClientId) };
                    _DBResponse.Data = data;


                }
                else if (Result == -1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "SD Balance will be negative for this Party. You cannot save this.";
                    _DBResponse.Status = -1;
                }

                else if (Result == -2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Please select SD Party. You can not deposit TDS in Cash Party";
                    _DBResponse.Status = -2;
                }
                else if (Result == -4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Certificate No Already Exist";
                    _DBResponse.Status = -4;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "You can not Update ! Its already deleted";
                    _DBResponse.Status = 3;
                }
                else
                {
                    _DBResponse.Data = null;
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
        }

        public void GetAllPartyWiseTDSDeposit()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllPartyWiseTDSDeposit", CommandType.StoredProcedure);
                _DBResponse = new DatabaseResponse();

                List<PartyWiseTDSDeposit> PartyWiseTDSDepositList = new List<PartyWiseTDSDeposit>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        PartyWiseTDSDeposit objPartyWiseTDSDeposit = new PartyWiseTDSDeposit();
                        objPartyWiseTDSDeposit.Id = Convert.ToInt32(dr["Id"]);
                        objPartyWiseTDSDeposit.ReceiptNo = Convert.ToString(dr["ReceiptNo"]);
                        objPartyWiseTDSDeposit.PartyName = Convert.ToString(dr["PartyName"]);
                        objPartyWiseTDSDeposit.CirtificateNo = Convert.ToString(dr["CirtificateNo"]);
                        objPartyWiseTDSDeposit.CirtificateDate = Convert.ToString(dr["CirtificateDate"]);
                        objPartyWiseTDSDeposit.Amount = Convert.ToDecimal(dr["Amount"]);
                        objPartyWiseTDSDeposit.TDSBalance = Convert.ToDecimal(dr["TDSBalance"]);
                        objPartyWiseTDSDeposit.DepositDate = Convert.ToString(dr["ReceiptDate"]);
                        objPartyWiseTDSDeposit.IsCan = Convert.ToString(dr["IsCan"]);
                        objPartyWiseTDSDeposit.Remarks = Convert.ToString(dr["Remarks"]);
                        objPartyWiseTDSDeposit.FinancialYear = Convert.ToInt32(dr["FinYarFrom"]);
                        objPartyWiseTDSDeposit.FinancialYearNext = Convert.ToInt32(dr["FinYarTo"]);
                        objPartyWiseTDSDeposit.TdsQuarter = Convert.ToString(dr["TdsQuarter"]);
                        PartyWiseTDSDepositList.Add(objPartyWiseTDSDeposit);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = PartyWiseTDSDepositList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
            }
        }

        public void GetPartyWiseTDSDepositDetails(int PartyWiseTDSDepositId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyWiseTDSDepositId", MySqlDbType = MySqlDbType.Int32, Value = PartyWiseTDSDepositId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetPartyWiseTDSDepositDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            PartyWiseTDSDeposit objPartyWiseTDSDeposit = new PartyWiseTDSDeposit();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPartyWiseTDSDeposit.Id = Convert.ToInt32(Result["Id"]);
                    objPartyWiseTDSDeposit.PartyId = Convert.ToInt32(Result["PartyId"]);
                    objPartyWiseTDSDeposit.PartyName = Convert.ToString(Result["PartyName"]);
                    objPartyWiseTDSDeposit.CirtificateNo = Convert.ToString(Result["CirtificateNo"]);
                    objPartyWiseTDSDeposit.CirtificateDate = Convert.ToString(Result["CirtificateDate"]);
                    objPartyWiseTDSDeposit.Amount = Convert.ToDecimal(Result["Amount"]);
                    objPartyWiseTDSDeposit.TDSBalance = Convert.ToDecimal(Result["TDSBalance"]);
                    objPartyWiseTDSDeposit.Remarks = Convert.ToString(Result["Remarks"]);
                    objPartyWiseTDSDeposit.FinancialYear = Convert.ToInt32(Result["FinYarFrom"]);
                    objPartyWiseTDSDeposit.FinancialYearNext = Convert.ToInt32(Result["FinYarTo"]);
                    objPartyWiseTDSDeposit.TdsQuarter = Convert.ToString(Result["TdsQuarter"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Data = objPartyWiseTDSDeposit;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
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
                Result.Dispose();
                Result.Close();
            }
        }

        public void DeletePartyWiseTDSDeposit(int PartyWiseTDSDepositId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyWiseTDSDepositId", MySqlDbType = MySqlDbType.Int32, Value = PartyWiseTDSDepositId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("DeletePartyWiseTDSDeposit", CommandType.StoredProcedure, Dparam);
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Party Wise TDS Deposit Deleted Successfully.";
                    _DBResponse.Status = 1;
                }
                else if (result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "SD Balance will be negative for this Party. You cannot Delete this.";
                    _DBResponse.Status = 2;
                }

                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "You can not delete ! Its already deleted";
                    _DBResponse.Status = 3;
                }
                else
                {
                    _DBResponse.Data = null;
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
        }

        #endregion

        #region Pull CCAvenue
        public void CCAvenuePaymentReceiptPullData()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCCAvenueOrderStatusList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstReceipt = new List<OnlinePaymentReceipt>();
            while (Result.Read())
            {

                Status = 1;

                lstReceipt.Add(new OnlinePaymentReceipt
                {


                    ReferenceNo = Convert.ToString(Result["OrderId"]),

                });
            }

            if (Status == 1)
            {
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = lstReceipt;
            }
            else
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;

            }
        }


        public void AddPaymentGatewayResponseCCAvenue(CcAvnResponseJsonModel objPGR)
        {
            try
            {
                log.Error("Start Saving CCAvenue Pull  :");
                string GeneratedClientId = "";
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_tracking_id", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.reference_no });
                LstParam.Add(new MySqlParameter { ParameterName = "in_order_id", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_no });
                LstParam.Add(new MySqlParameter { ParameterName = "in_bank_ref_no", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bank_ref_no });
                LstParam.Add(new MySqlParameter { ParameterName = "in_order_status", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_status });
                LstParam.Add(new MySqlParameter { ParameterName = "in_failure_message", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_notes });
                LstParam.Add(new MySqlParameter { ParameterName = "in_payment_mode", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_option_type });
                LstParam.Add(new MySqlParameter { ParameterName = "in_status_code", MySqlDbType = MySqlDbType.Int16, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "in_status_message", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bank_response });
                LstParam.Add(new MySqlParameter { ParameterName = "in_amount", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.order_amt });
                LstParam.Add(new MySqlParameter { ParameterName = "in_currency", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_currncy });
                LstParam.Add(new MySqlParameter { ParameterName = "in_billing_name", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_name });
                LstParam.Add(new MySqlParameter { ParameterName = "in_billing_address", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_address });
                LstParam.Add(new MySqlParameter { ParameterName = "in_billing_city", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_city });
                LstParam.Add(new MySqlParameter { ParameterName = "in_billing_state", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_state });
                LstParam.Add(new MySqlParameter { ParameterName = "in_billing_zip", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_zip });
                LstParam.Add(new MySqlParameter { ParameterName = "in_billing_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_country });
                LstParam.Add(new MySqlParameter { ParameterName = "in_billing_tel", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_tel });
                LstParam.Add(new MySqlParameter { ParameterName = "in_billing_email", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_email });
                LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_name", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_name });
                LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_address", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_address });
                LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_city", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_city });
                LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_state", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_state });
                LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_zip", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_zip });
                LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_country });
                LstParam.Add(new MySqlParameter { ParameterName = "in_delivery_tel", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_ship_tel });
                LstParam.Add(new MySqlParameter { ParameterName = "in_vault", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "in_offer_type", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "in_offer_code", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "in_discount_value", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "in_mer_amount", MySqlDbType = MySqlDbType.Decimal, Value = objPGR.order_capt_amt });
                LstParam.Add(new MySqlParameter { ParameterName = "in_eci_value", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "in_retry", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "in_response_code", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "in_billing_notes", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_notes });
                LstParam.Add(new MySqlParameter { ParameterName = "in_trans_date", MySqlDbType = MySqlDbType.DateTime, Value = objPGR.order_date_time });
                LstParam.Add(new MySqlParameter { ParameterName = "in_bin_country", MySqlDbType = MySqlDbType.VarChar, Value = objPGR.order_bill_country });


                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                int Result = DataAccess.ExecuteNonQuery("AddPaymentGatewayResponse", CommandType.StoredProcedure, DParam, out GeneratedClientId);
                _DBResponse = new DatabaseResponse();

                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Response Saved Successfully-" + GeneratedClientId;
                    _DBResponse.Data = GeneratedClientId;
                }


                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Duplicate Entry";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Start Saving CCAvenue Error  :" + ex.StackTrace);
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            log.Error("Start Saving CCAvenue END  :");
        }
        #endregion
    }
}