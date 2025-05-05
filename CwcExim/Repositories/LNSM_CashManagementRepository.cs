using CwcExim.Areas.CashManagement.Models;
using CwcExim.Areas.Import.Models;
using CwcExim.Areas.Report.Models;
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

namespace CwcExim.Repositories
{
    public class LNSM_CashManagementRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        #region Payment Receipt/Cash Receipt 
        public void ListOfPayeeForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfPayeeForPage", CommandType.StoredProcedure, Dparam);
            IList<LNSM_PayeeForPage> lstPayee = new List<LNSM_PayeeForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstPayee.Add(new LNSM_PayeeForPage
                    {
                        PayeeId = Convert.ToInt32(Result["PartyId"]),
                        PayeeName = Result["PartyName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstPayee, State };
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
        //public void GetPartyList()
        //{
        //    List<MySqlParameter> lstParam = new List<MySqlParameter>();
        //    IDataParameter[] DParam = { };
        //    DParam = lstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetAllPartyCashList", CommandType.StoredProcedure, DParam);
        //    var model = new Kol_CashReceiptModel();
        //    int Status = 0;
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            model.PartyDetail.Add(new Party { PartyId = Convert.ToInt32(Result["PartyId"]), PartyName = Convert.ToString(Result["PartyName"]) });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = model;
        //        }
        //        else
        //        {
        //            _DBResponse.Status = 0;
        //            _DBResponse.Message = "Error";
        //            _DBResponse.Data = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Status = 0;
        //        _DBResponse.Message = "Error";
        //        _DBResponse.Data = null;
        //    }
        //    finally
        //    {
        //        Result.Close();
        //        Result.Dispose();
        //    }
        //}


        public void GetCashRcptDetails(int PartyId, string PartyName, string Type = "INVOICE")
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = Type });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCshDtlsAgnstCashParty", CommandType.StoredProcedure, DParam);

            var model = new LNSM_CashReceiptModel();
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

                    model.PayByDetail.Add(new LNSM_PayBy
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
                        model.PdaAdjustdetail.Add(new LNSM_PdaAdjust
                        {
                            PayByPdaId = Convert.ToInt32(Result["PDAId"]),
                            EximTraderId = (Result["EximTraderId"] == System.DBNull.Value) ? 0 : Convert.ToInt32(Result["EximTraderId"]),
                            FolioNo = Result["FolioNo"].ToString(),
                            Opening = Convert.ToDecimal(Result["Balance"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.CashReceiptInvoiveMappingList.Add(new LNSM_CashReceiptInvoiveMapping
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
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.TdsBalanceAmount = Convert.ToDecimal(Result["TdsBalanceAmount"]);
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
        public void AddCashReceipt(LNSM_CashReceiptModel ObjCashRcpt, string xml)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = ObjCashRcpt.Type });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BalanceAmt", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.BalanceAmt) });
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

                    //try
                    //{
                    //    var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjCashRcpt.InvoiceHtml));
                    //    var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Cash Receipt No.<span>", "Cash Receipt No. <span>" + ReceiptNo)));
                    //    var LstParam1 = new List<MySqlParameter>();
                    //    LstParam1.Add(new MySqlParameter { ParameterName = "in_RcptNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = ReceiptNo });
                    //    LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                    //    LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                    //    IDataParameter[] DParam1 = LstParam1.ToArray();
                    //    var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                    //    DA1.ExecuteNonQuery("UpdateCashRcptHtmlReport", CommandType.StoredProcedure, DParam1);
                    //}
                    //catch { }
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = ReceiptNo;
                }
                else if (Result == -1)
                {
                    _DBResponse.Status = -1;
                    _DBResponse.Message = ReceiptNo;
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

        // Get Cash receipt Print
        public void GetCashRcptPrint(int CashReceiptId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.Int32, Value = CashReceiptId });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCashRcptMultiInvPrint", CommandType.StoredProcedure, DParam);
            var model = new LNSM_PostPaymentSheet();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    model.InvoiceNo = Result["InvoiceNo"].ToString();
                    model.InvoiceDate = Result["InvoiceDate"].ToString();
                    model.PartyName = Result["PartyName"].ToString();
                    model.PartyAddress = Result["PartyAddress"].ToString();
                    model.PartyState = Result["PartyState"].ToString();
                    model.PartyStateCode = Result["PartyStateCode"].ToString();
                    model.PartyGST = Result["PartyGST"].ToString();
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
                    model.ImporterExporterType = Result["ImporterExporterType"].ToString();
                    model.BillType = Result["BillType"].ToString();
                    model.StuffingDestuffDateType = Result["StuffingDestuffDateType"].ToString();
                    model.CWCTDS = Convert.ToDecimal(Result["CWCTDS"]);
                    model.HTTDS = Convert.ToDecimal(Result["HTTDS"]);
                    model.TDS = Convert.ToDecimal(Result["TDS"]);
                    model.TDSCol = Convert.ToDecimal(Result["TDSCol"]);
                    model.DeliveryDate = Result["DeliveryDate"].ToString();
                    model.ApproveOn = Result["ApproveOn"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.lstPostPaymentCont.Add(new LNSM_PostPaymentContainer
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
                        model.lstPostPaymentChrg.Add(new LNSM_PostPaymentCharge
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
                        model.CashReceiptDetails.Add(new LNSM_CashReceipt
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

        //public void UpdatePrintHtml(int CashReceiptId, string htmlPrint)
        //{
        //    List<MySqlParameter> lstParam = new List<MySqlParameter>();
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.Int32, Value = CashReceiptId });
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_Html", MySqlDbType = MySqlDbType.String, Value = htmlPrint });
        //    lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
        //    IDataParameter[] DParam = { };
        //    DParam = lstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    int Result = DataAccess.ExecuteNonQuery("UpdateCashRcptHtmlForPrint", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        if (Result == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = null;
        //        }
        //        else
        //        {
        //            _DBResponse.Status = 0;
        //            _DBResponse.Message = "Error";
        //            _DBResponse.Data = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Status = 0;
        //        _DBResponse.Message = "Error";
        //        _DBResponse.Data = null;
        //    }

        //}

        public void GetCashReceiptList(string ReceiptNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = ReceiptNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCashReceiptList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<LNSM_CashReceiptModel> lstCashReceipt = new List<LNSM_CashReceiptModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new LNSM_CashReceiptModel
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

        public void GetLoadMoreCashReceiptList(string ReceiptNo, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = ReceiptNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCashReceiptList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<LNSM_CashReceiptModel> lstCashReceipt = new List<LNSM_CashReceiptModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new LNSM_CashReceiptModel
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

        public void GetBulkCashreceipt(string FromDate, string ToDate, string ReceiptNo)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(FromDate == null ? "" : FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ToDate == null ? "" : ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ReceiptNo });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetBulkCashRecptForPrint", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result != null && Result.Tables[1].Rows.Count > 0)
                {
                    Status = 1;
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Result;
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

        #endregion

        #region ADD MONEY TO PD

        public void GetPartyDetails()
        {
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPDPartyDetails");
            IList<LNSM_PartyDetails> model = new List<LNSM_PartyDetails>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.Add(new LNSM_PartyDetails { Id = Convert.ToInt32(Result["Id"]), Name = Result["Name"].ToString(), Address = Result["Address"].ToString(), Folio = Result["Folio"].ToString(), Balance = Convert.ToDecimal(Result["Balance"]) });
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

        public void AddMoneyToPD(int partyId, DateTime transDate, string xml)
        {
            string GeneratedClientId = "0";
            string RecNo = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_PartyId", MySqlDbType = MySqlDbType.Int32, Value = partyId });
            LstParam.Add(new MySqlParameter { ParameterName = "In_TransDate", MySqlDbType = MySqlDbType.DateTime, Value = transDate });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Xml", MySqlDbType = MySqlDbType.Text, Value = xml });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "0" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddMoneyToPD", CommandType.StoredProcedure, DParam, out GeneratedClientId, out RecNo);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Money Added To SD Successfully";
                    _DBResponse.Data = RecNo;
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

        public void GetAddMoneyToPDList(int Page)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAddMoneyToPDList", CommandType.StoredProcedure, DParam);
            LNSM_AddMoneyToPDListModel ObjAddMoneyToPD = null;
            List<LNSM_AddMoneyToPDListModel> lstAddMoneyToPD = new List<LNSM_AddMoneyToPDListModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                string State = "0";
                while (Result.Read())
                {
                    Status = 1;
                    ObjAddMoneyToPD = new LNSM_AddMoneyToPDListModel();
                    ObjAddMoneyToPD.ReceiptNo = Convert.ToString(Result["ReceiptNo"]);
                    ObjAddMoneyToPD.ReceiptDate = Convert.ToString(Result["ReceiptDate"] == null ? "" : Result["ReceiptDate"]);
                    ObjAddMoneyToPD.Amount = Convert.ToString(Result["Amount"]);

                    lstAddMoneyToPD.Add(ObjAddMoneyToPD);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToString(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = State;
                    _DBResponse.Data = lstAddMoneyToPD;
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

        public void GetSearchAddMoneyToPDList(string ReceiptNo)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = ReceiptNo });
            DParam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAddMoneyToPDListByReceiptNo", CommandType.StoredProcedure, DParam);
            LNSM_AddMoneyToPDListModel ObjAddMoneyToPD = null;
            List<LNSM_AddMoneyToPDListModel> lstAddMoneyToPD = new List<LNSM_AddMoneyToPDListModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAddMoneyToPD = new LNSM_AddMoneyToPDListModel();
                    ObjAddMoneyToPD.ReceiptNo = Convert.ToString(Result["ReceiptNo"]);
                    ObjAddMoneyToPD.ReceiptDate = Convert.ToString(Result["ReceiptDate"] == null ? "" : Result["ReceiptDate"]);
                    ObjAddMoneyToPD.Amount = Convert.ToString(Result["Amount"]);

                    lstAddMoneyToPD.Add(ObjAddMoneyToPD);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstAddMoneyToPD;
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

        #region Payment Adjust Through SD

        public void GetPartyListSD()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPartySDList", CommandType.StoredProcedure, DParam);
            var model = new LNSM_CashReceiptModel();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.PartyDetail.Add(new LNSM_Party { PartyId = Convert.ToInt32(Result["PartyId"]), PartyName = Convert.ToString(Result["PartyName"]) });
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


        public void GetCashRcptDetailsSD(int PartyId, string PartyName, string Type = "")
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = Type });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCshDtlsAgnstSDParty", CommandType.StoredProcedure, DParam);

            var model = new LNSM_CashReceiptModel();
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

                    model.PayByDetail.Add(new LNSM_PayBy
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
                        model.PdaAdjustdetail.Add(new LNSM_PdaAdjust
                        {
                            PayByPdaId = Convert.ToInt32(Result["PDAId"]),
                            EximTraderId = (Result["EximTraderId"] == System.DBNull.Value) ? 0 : Convert.ToInt32(Result["EximTraderId"]),
                            FolioNo = Result["FolioNo"].ToString(),
                            Opening = Convert.ToDecimal(Result["Balance"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.CashReceiptInvoiveMappingList.Add(new LNSM_CashReceiptInvoiveMapping
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
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.TdsBalanceAmount = Result["TdsBalanceAmount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["TdsBalanceAmount"]);
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
        public void AddCashReceiptSD(LNSM_CashReceiptModel ObjCashRcpt, string xml)
        {
            string GeneratedClientId = "0";
            string ReceiptNo = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjCashRcpt.ReceiptDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjCashRcpt.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayByPdaId", MySqlDbType = MySqlDbType.Int32, Value = ObjCashRcpt.PayByPdaId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PdaAdjust", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PdaAdjustedAmount", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.Adjusted) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PdaClosing", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.Closing) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalPaymentReceipt", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.TotalPaymentReceipt) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TdsAmount", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.TdsAmount) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceValue", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjCashRcpt.InvoiceValue) });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjCashRcpt.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = ObjCashRcpt.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_xml", MySqlDbType = MySqlDbType.VarChar, Value = xml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = ObjCashRcpt.Type });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "0" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_cashreceiptinvdtlsxml", MySqlDbType = MySqlDbType.VarChar, Value = ObjCashRcpt.CashReceiptInvDtlsHtml });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("addcashreceiptSDmultiinvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReceiptNo);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    var data = new { ReceiptNo = ReceiptNo, CashReceiptId = Convert.ToInt32(GeneratedClientId) };
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Add Cash Receipt Successfully.";
                    _DBResponse.Data = data;

                    //try
                    //{
                    //    var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjCashRcpt.InvoiceHtml));
                    //    var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Cash Receipt No.<span>", "Cash Receipt No. <span>" + ReceiptNo)));
                    //    var LstParam1 = new List<MySqlParameter>();
                    //    LstParam1.Add(new MySqlParameter { ParameterName = "in_RcptNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = ReceiptNo });
                    //    LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                    //    LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                    //    IDataParameter[] DParam1 = LstParam1.ToArray();
                    //    var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                    //    DA1.ExecuteNonQuery("UpdateCashRcptHtmlReport", CommandType.StoredProcedure, DParam1);
                    //}
                    //catch { }
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = Convert.ToDecimal(ReceiptNo);
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
        public void GetCashRcptPrintSD(int CashReceiptId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.Int32, Value = CashReceiptId });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCashRcptMultiInvPrint", CommandType.StoredProcedure, DParam);
            var model = new LNSM_PostPaymentSheet();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    
                    model.InvoiceNo = Result["InvoiceNo"].ToString();                   
                    model.InvoiceDate = Result["InvoiceDate"].ToString();                    
                    model.PartyName = Result["PartyName"].ToString();
                    model.PartyAddress = Result["PartyAddress"].ToString();
                    model.PartyState = Result["PartyState"].ToString();
                    model.PartyStateCode = Result["PartyStateCode"].ToString();
                    model.PartyGST = Result["PartyGST"].ToString();
                   
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
                   
                    model.ImporterExporterType = Result["ImporterExporterType"].ToString();
                    model.BillType = Result["BillType"].ToString();
                    model.StuffingDestuffDateType = Result["StuffingDestuffDateType"].ToString();
                    model.CWCTDS = Convert.ToDecimal(Result["CWCTDS"]);
                    model.HTTDS = Convert.ToDecimal(Result["HTTDS"]);
                    model.TDS = Convert.ToDecimal(Result["TDS"]);
                    model.TDSCol = Convert.ToDecimal(Result["TDSCol"]);
                    model.DeliveryDate = Result["DeliveryDate"].ToString();
                    model.ApproveOn = Result["ApproveOn"].ToString();
                   
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.lstPostPaymentCont.Add(new LNSM_PostPaymentContainer
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
                        model.lstPostPaymentChrg.Add(new LNSM_PostPaymentCharge
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
                        model.CashReceiptDetails.Add(new LNSM_CashReceipt
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

        //public void UpdatePrintHtmlSD(int CashReceiptId, string htmlPrint)
        //{
        //    List<MySqlParameter> lstParam = new List<MySqlParameter>();
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.Int32, Value = CashReceiptId });
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_Html", MySqlDbType = MySqlDbType.String, Value = htmlPrint });
        //    lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
        //    IDataParameter[] DParam = { };
        //    DParam = lstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    int Result = DataAccess.ExecuteNonQuery("UpdateCashRcptHtmlForPrint", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        if (Result == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = null;
        //        }
        //        else
        //        {
        //            _DBResponse.Status = 0;
        //            _DBResponse.Message = "Error";
        //            _DBResponse.Data = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Status = 0;
        //        _DBResponse.Message = "Error";
        //        _DBResponse.Data = null;
        //    }

        //}

        public void GetCashReceiptSDList(string ReceiptNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = ReceiptNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCashReceiptSDList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<LNSM_CashReceiptModel> lstCashReceipt = new List<LNSM_CashReceiptModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new LNSM_CashReceiptModel
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

        public void GetLoadMoreCashReceiptSDList(string ReceiptNo, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = ReceiptNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCashReceiptSDList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<LNSM_CashReceiptModel> lstCashReceipt = new List<LNSM_CashReceiptModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new LNSM_CashReceiptModel
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

        #region Cheque Return

        public void GetPartyDetail(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetChequePartyDetails", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            LNSM_ChequeReturn LstEximTrader = new LNSM_ChequeReturn();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEximTrader.lstCheque.Add(new LNSM_ChequeDetails
                    {
                        PartyId = Convert.ToInt32(Result["Id"]),
                        PartyName = Result["Name"].ToString(),
                        PartyCode = Result["EximTraderAlias"].ToString(),
                        //  ChequeName = Result["Name"].ToString(),
                        ChequeBalance = Convert.ToDecimal(Result["Balance"]),
                        ChequeSdNo = Result["SdNo"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstEximTrader.State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEximTrader;
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
        public void GetChequeNo(int PartyId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "In_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetChequesForReturn", CommandType.StoredProcedure, DParam);
            IList<LNSM_ChequeDetail> model = new List<LNSM_ChequeDetail>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.Add(new LNSM_ChequeDetail
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        Cheque = Result["SdNo"].ToString(),

                    });

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
        public void GetChequeDetail(int partyid, string ChequeNo)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.VarChar, Value = partyid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Pda", MySqlDbType = MySqlDbType.VarChar, Value = ChequeNo });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfCheques", CommandType.StoredProcedure, DParam);
            IList<LNSM_ChequeDetail> model = new List<LNSM_ChequeDetail>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.Add(new LNSM_ChequeDetail
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        Cheque = Result["SdNo"].ToString(),

                    });

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
        public void GetChequeDetails(string ChequeNo)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "In_ChequeNo", MySqlDbType = MySqlDbType.VarChar, Value = ChequeNo });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetChequedetailsForReturn", CommandType.StoredProcedure, DParam);
            IList<LNSM_Cheques> model = new List<LNSM_Cheques>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.Add(new LNSM_Cheques
                    {
                        ChequeDate = Result["PdaChequeDate"].ToString(),
                        DraweeBank = Result["PdaDrawBank"].ToString(),
                        Amount = Result["Amount"].ToString()
                    });

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


        public void AddChequeReturn(int partyId, string ChequeReturnDate, string SdNo, string Balance, string ChequeNo, string DraweeBank, string Narration, string ChequeDate, decimal Amount, decimal AdjustedBalance)
        {
            string GeneratedClientId = "0";
            string RecNo = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_ChequeReturnDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ChequeReturnDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "In_PartyId", MySqlDbType = MySqlDbType.Int32, Value = partyId });
            LstParam.Add(new MySqlParameter { ParameterName = "In_SDNO", MySqlDbType = MySqlDbType.VarChar, Value = SdNo });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Balance", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDecimal(Balance) });
            LstParam.Add(new MySqlParameter { ParameterName = "In_ChequeNo", MySqlDbType = MySqlDbType.VarChar, Value = ChequeNo });
            LstParam.Add(new MySqlParameter { ParameterName = "In_DraweeBank", MySqlDbType = MySqlDbType.VarChar, Value = DraweeBank });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Narration", MySqlDbType = MySqlDbType.VarChar, Value = Narration });
            LstParam.Add(new MySqlParameter { ParameterName = "In_ChequeDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ChequeDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Amount", MySqlDbType = MySqlDbType.Decimal, Value = Amount });
            LstParam.Add(new MySqlParameter { ParameterName = "In_AdjustedBalance", MySqlDbType = MySqlDbType.Decimal, Value = AdjustedBalance });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "0" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddMoneyToDisHonourCheque", CommandType.StoredProcedure, DParam, out GeneratedClientId, out RecNo);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "DisHonour Cheque Saved Successfully";
                    _DBResponse.Data = RecNo;
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
        public void GetAllChequeReturn(string ReceiptNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = ReceiptNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllChequeReturn", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<LNSM_ChequeReturn> lstChequeReturn = new List<LNSM_ChequeReturn>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstChequeReturn.Add(new LNSM_ChequeReturn
                    {
                        DishonuredId = Convert.ToInt32(Result["DishonuredId"]),
                        AutoDisHonourRcptNo = Convert.ToString(Result["ReceiptNo"]),
                        ChequeReturnDate = Convert.ToString(Result["ChequeReturnDate"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        SdNo = Convert.ToString(Result["SDNo"]),
                        Balance = Convert.ToDecimal(Result["Balance"]),
                        ChequeNo = Convert.ToString(Result["ChequeNo"]),
                        DraweeBank = Convert.ToString(Result["DraweeBank"]),
                        ChequeDate = Convert.ToString(Result["ChequeDate"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        AdjustedBalance = Convert.ToDecimal(Result["AdjustedBalance"])
                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstChequeReturn;
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

        public void GetLoadMoreChequeReturnList(string ReceiptNo, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = ReceiptNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllChequeReturn", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<LNSM_ChequeReturn> lstChequeReturn = new List<LNSM_ChequeReturn>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstChequeReturn.Add(new LNSM_ChequeReturn
                    {
                        DishonuredId = Convert.ToInt32(Result["DishonuredId"]),
                        AutoDisHonourRcptNo = Convert.ToString(Result["ReceiptNo"]),
                        ChequeReturnDate = Convert.ToString(Result["ChequeReturnDate"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        SdNo = Convert.ToString(Result["SDNo"]),
                        Balance = Convert.ToDecimal(Result["Balance"]),
                        ChequeNo = Convert.ToString(Result["ChequeNo"]),
                        DraweeBank = Convert.ToString(Result["DraweeBank"]),
                        ChequeDate = Convert.ToString(Result["ChequeDate"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        AdjustedBalance = Convert.ToDecimal(Result["AdjustedBalance"])
                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstChequeReturn;
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

        #region Refund From SD
        public void RefundFromPDA(LNSM_PDARefundModel m, int Uid)
        {

            string BankBranch = m.Bank + '#' + m.Branch;
            string GeneratedClientId = "0";
            string RecNo = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_PartyId", MySqlDbType = MySqlDbType.Int32, Value = m.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "In_RefundAmount", MySqlDbType = MySqlDbType.Decimal, Value = m.RefundAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "In_OpeningAmt", MySqlDbType = MySqlDbType.Decimal, Value = m.OpBalance });
            LstParam.Add(new MySqlParameter { ParameterName = "In_ClosingAmt", MySqlDbType = MySqlDbType.Decimal, Value = m.closingBalance });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = m.Remarks });            
            LstParam.Add(new MySqlParameter { ParameterName = "In_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "In_BankBranch", MySqlDbType = MySqlDbType.Text, Value = BankBranch });
            LstParam.Add(new MySqlParameter { ParameterName = "In_ChequeNo", MySqlDbType = MySqlDbType.VarChar, Value = m.ChequeNo });
            LstParam.Add(new MySqlParameter { ParameterName = "In_ChequeDate", MySqlDbType = MySqlDbType.VarChar, Value = m.ChequeDate });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "0" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("RefundFromPDA", CommandType.StoredProcedure, DParam, out GeneratedClientId, out RecNo);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Refund From SD Successfully";
                    _DBResponse.Data = RecNo;
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

        public void GetPartyDetailsRefund()
        {
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPDPartyDetailsForRefund");
            IList<LNSM_RefundMoneyFromPD> model = new List<LNSM_RefundMoneyFromPD>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.Add(new LNSM_RefundMoneyFromPD
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        Name = Result["Name"].ToString(),
                        Address = Result["Address"].ToString(),
                        Folio = Result["Folio"].ToString(),
                        Balance = (Convert.ToDecimal(Result["Balance"])),
                        UnPaidAmount = (Convert.ToDecimal(Result["UnPaidAmount"])),
                        PName = Result["PName"].ToString(),
                        PartyAddress = Result["Address"].ToString()

                    });
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

        public void ViewSDRefund(int SDAcId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PdaId", MySqlDbType = MySqlDbType.Int32, Value = SDAcId });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ViewSDRefund", CommandType.StoredProcedure, Dparam);
            LNSM_ViewSDRefund ObjSD = new LNSM_ViewSDRefund();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    ObjSD.PDAACId = Convert.ToInt32(result["PdaAcId"]);
                    ObjSD.PartyName = result["Party"].ToString();
                    ObjSD.PartyAddress = result["PartyAddress"].ToString();
                    ObjSD.ClosureDate = result["ClosureDate"].ToString();
                    ObjSD.RecieptNo = result["ReceiptNo"].ToString();
                    ObjSD.OpeningAmt = Convert.ToDecimal(result["OpeningAmt"]);                    
                    ObjSD.RefundAmt = Convert.ToDecimal(result["RefundAmt"]);
                    ObjSD.ClosingAmt = Convert.ToDecimal(result["ClosingAmt"]);
                    ObjSD.BankName = result["Bank"].ToString();
                    ObjSD.Branch = result["Branch"].ToString();
                    ObjSD.ChqNo = (result["ChqNo"] == null ? "" : result["ChqNo"]).ToString();
                    ObjSD.ChqDate = (result["Chqdate"] == null ? "" : result["Chqdate"]).ToString();
                    ObjSD.Remarks = (result["Remarks"] == null ? "" : result["Remarks"]).ToString();
                }

                if (Status == 1)
                {
                    _DBResponse.Data = ObjSD;
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

        public void GetSDRefundList(string ReceiptNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = ReceiptNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("SDRefundList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();


            List<LNSM_SDRefundList> SdList = new List<LNSM_SDRefundList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    SdList.Add(new LNSM_SDRefundList
                    {
                        PDAACId = Convert.ToInt32(Result["PdaAcId"]),
                        ClosureDate = Result["ClosureDate"].ToString(),
                        PartyName = Result["Party"].ToString(),
                        RecieptNo = Result["ReceiptNo"].ToString(),
                        RefundAmt = (Convert.ToDecimal(Result["Amount"]))
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = SdList;
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

        public void GetLoadMoreSDRefundList(string ReceiptNo, int Page = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = ReceiptNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("SDRefundList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            List<LNSM_SDRefundList> SdList = new List<LNSM_SDRefundList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    SdList.Add(new LNSM_SDRefundList
                    {
                        PDAACId = Convert.ToInt32(Result["PdaAcId"]),
                        ClosureDate = Result["ClosureDate"].ToString(),
                        PartyName = Result["Party"].ToString(),
                        RecieptNo = Result["ReceiptNo"].ToString(),
                        RefundAmt = (Convert.ToDecimal(Result["Amount"]))
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = SdList;
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

        public void getCompanyDetails()
        {

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();


            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getCompanyDetails", CommandType.StoredProcedure, DParam);
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    objCompanyDetails.RoAddress = Convert.ToString(Result["ROAddress"]).Replace("<br/>", " ");
                    objCompanyDetails.CompanyName = Convert.ToString(Result["CompanyName"]);
                    objCompanyDetails.CompanyAddress = Convert.ToString(Result["CompanyAddress"]).Replace("<br/>", " ");

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objCompanyDetails;
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
                Result.Close();
                Result.Dispose();

            }
        }

        public void GetSDRefundDetails(string ReceiptNo)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = ReceiptNo });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetSDRefundDetails", CommandType.StoredProcedure, Dparam);
            LNSM_ViewSDRefund ObjSD = new LNSM_ViewSDRefund();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    ObjSD.PDAACId = Convert.ToInt32(result["PdaAcId"]);
                    ObjSD.PartyName = result["Party"].ToString();
                    ObjSD.PartyAddress = result["PartyAddress"].ToString();
                    ObjSD.ClosureDate = result["ClosureDate"].ToString();
                    ObjSD.RecieptNo = result["ReceiptNo"].ToString();
                    ObjSD.OpeningAmt = Convert.ToDecimal(result["OpeningAmt"]);
                    ObjSD.RefundAmt = Convert.ToDecimal(result["RefundAmt"]);
                    ObjSD.ClosingAmt = Convert.ToDecimal(result["ClosingAmt"]);
                    ObjSD.BankName = result["Bank"].ToString();
                    ObjSD.Branch = result["Branch"].ToString();
                    ObjSD.ChqNo = (result["ChqNo"] == null ? "" : result["ChqNo"]).ToString();
                    ObjSD.ChqDate = (result["Chqdate"] == null ? "" : result["Chqdate"]).ToString();
                    ObjSD.Remarks = (result["Remarks"] == null ? "" : result["Remarks"]).ToString();
                }

                if (Status == 1)
                {
                    _DBResponse.Data = ObjSD;
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
            List<LNSM_Party> model = new List<LNSM_Party>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.Add(new LNSM_Party { PartyId = Convert.ToInt32(Result["PartyId"]), PartyName = Convert.ToString(Result["PartyName"]) });
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

        public void AddEditPartyWiseTDSDeposit(LNSM_PartyWiseTDSDeposit objPartyWiseTDSDeposit)
        {
            string GeneratedClientId = "0";
            string ReceiptNo = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = objPartyWiseTDSDeposit.Id });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = objPartyWiseTDSDeposit.PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CertificateNo", MySqlDbType = MySqlDbType.String, Value = objPartyWiseTDSDeposit.CertificateNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CertificateDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objPartyWiseTDSDeposit.CertificateDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FinancialYearFrom", MySqlDbType = MySqlDbType.Int32, Value = objPartyWiseTDSDeposit.FinancialYear });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FinancialYearTo", MySqlDbType = MySqlDbType.Int32, Value = objPartyWiseTDSDeposit.FinancialYearNext });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TdsQuarter", MySqlDbType = MySqlDbType.String, Value = objPartyWiseTDSDeposit.TdsQuarter });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CertificateAmount", MySqlDbType = MySqlDbType.Decimal, Value = objPartyWiseTDSDeposit.Amount });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalTdsAmount", MySqlDbType = MySqlDbType.Decimal, Value = objPartyWiseTDSDeposit.TotalTDSDeducted });
            lstParam.Add(new MySqlParameter { ParameterName = "in_UnadjustedAmount", MySqlDbType = MySqlDbType.Decimal, Value = objPartyWiseTDSDeposit.UnadjustedAmount });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.String, Value = objPartyWiseTDSDeposit.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_tdscashreceiptxml", MySqlDbType = MySqlDbType.VarChar, Value = objPartyWiseTDSDeposit.CashReceiptInvDtlsHtml });
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
               
                else if (Result == -2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Certificate No Already Exist";
                    _DBResponse.Status = -2;
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
       
        public void GetAllPartyWiseTDSDeposit(string ReceiptNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = ReceiptNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPartyWiseTDSDeposit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<LNSM_PartyWiseTDSDeposit> lstTDSDeposit = new List<LNSM_PartyWiseTDSDeposit>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstTDSDeposit.Add(new LNSM_PartyWiseTDSDeposit
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        CertificateNo = Convert.ToString(Result["CirtificateNo"]),
                        CertificateDate = Convert.ToString(Result["CirtificateDate"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        TDSBalance = Convert.ToDecimal(Result["TDSBalance"]),
                        DepositDate = Convert.ToString(Result["ReceiptDate"]),
                        IsCan = Convert.ToString(Result["IsCan"]),
                        Remarks = Convert.ToString(Result["Remarks"]),
                        FinancialYear = Convert.ToInt32(Result["FinYarFrom"]),
                        FinancialYearNext = Convert.ToInt32(Result["FinYarTo"]),
                        TdsQuarter = Convert.ToString(Result["TdsQuarter"]),
                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstTDSDeposit;
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

        public void GetLoadMorePartyWiseTDSDeposit(string ReceiptNo, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = ReceiptNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPartyWiseTDSDeposit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<LNSM_PartyWiseTDSDeposit> lstTDSDeposit = new List<LNSM_PartyWiseTDSDeposit>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstTDSDeposit.Add(new LNSM_PartyWiseTDSDeposit
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        CertificateNo = Convert.ToString(Result["CirtificateNo"]),
                        CertificateDate = Convert.ToString(Result["CirtificateDate"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        TDSBalance = Convert.ToDecimal(Result["TDSBalance"]),
                        DepositDate = Convert.ToString(Result["ReceiptDate"]),
                        IsCan = Convert.ToString(Result["IsCan"]),
                        Remarks = Convert.ToString(Result["Remarks"]),
                        FinancialYear = Convert.ToInt32(Result["FinYarFrom"]),
                        FinancialYearNext = Convert.ToInt32(Result["FinYarTo"]),
                        TdsQuarter = Convert.ToString(Result["TdsQuarter"]),
                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstTDSDeposit;
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
        public void GetPartyWiseTDSDepositDetails(int PartyWiseTDSDepositId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyWiseTDSDepositId", MySqlDbType = MySqlDbType.Int32, Value = PartyWiseTDSDepositId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetPartyWiseTDSDepositDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            LNSM_PartyWiseTDSDeposit objPartyWiseTDSDeposit = new LNSM_PartyWiseTDSDeposit();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPartyWiseTDSDeposit.Id = Convert.ToInt32(Result["Id"]);
                    objPartyWiseTDSDeposit.PartyId = Convert.ToInt32(Result["PartyId"]);
                    objPartyWiseTDSDeposit.PartyName = Convert.ToString(Result["PartyName"]);
                    objPartyWiseTDSDeposit.CertificateNo = Convert.ToString(Result["CirtificateNo"]);
                    objPartyWiseTDSDeposit.CertificateDate = Convert.ToString(Result["CirtificateDate"]);
                    objPartyWiseTDSDeposit.Amount = Convert.ToDecimal(Result["Amount"]);
                    objPartyWiseTDSDeposit.TDSBalance = Convert.ToDecimal(Result["TDSBalance"]);
                    objPartyWiseTDSDeposit.Remarks = Convert.ToString(Result["Remarks"]);
                    objPartyWiseTDSDeposit.FinancialYear = Convert.ToInt32(Result["FinYarFrom"]);
                    objPartyWiseTDSDeposit.FinancialYearNext = Convert.ToInt32(Result["FinYarTo"]);
                    objPartyWiseTDSDeposit.TdsQuarter = Convert.ToString(Result["TdsQuarter"]);
                    objPartyWiseTDSDeposit.ReceiptNo = Convert.ToString(Result["ReceiptNo"]);
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

        public void GetPartyWiseTDSRcptDetails(int PartyId,string PartyName)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });            
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPartyWiseTDSDepositReceiptDetails", CommandType.StoredProcedure, DParam);

            var model = new LNSM_PartyWiseTDSDeposit();
            model.PartyId = PartyId;
            model.PartyName = PartyName;           

            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    model.CertificateMappingList.Add(new LNSM_TDSCertificateMapping
                    {
                        CashReceiptId = Convert.ToInt32(Result["CashReceiptId"]),
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        ReceiptDate = Convert.ToString(Result["ReceiptDate"]),                        
                        ReceiptAmount = Convert.ToDecimal(Result["ReceiptAmount"]),
                        TDSAmount = Convert.ToDecimal(Result["TDSAmount"]),
                        BalanceAmount = Convert.ToDecimal(Result["BalanceAmount"]),
                        DepositAmount = 0
                    });
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

        public void GetTDSBulkCashreceipt(string FromDate, string ToDate, string ReceiptNo)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(FromDate == null ? "" : FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ToDate == null ? "" : ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ReceiptNo });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetBulkTDSCashRecptForPrint", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result != null && Result.Tables[1].Rows.Count > 0)
                {
                    Status = 1;
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Result;
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

        #endregion

        #region Direct Online Payment
        public void AddDirectPaymentVoucher(LNSM_DirectOnlinePayment objDOP, int uid)
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
            List<LNSM_DirectOnlinePayment> objDOPList = new List<LNSM_DirectOnlinePayment>();
            LNSM_DirectOnlinePayment objDOP = new LNSM_DirectOnlinePayment();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objDOP = new LNSM_DirectOnlinePayment();
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
            List<LNSM_DirectOnlinePayment> objDOPList = new List<LNSM_DirectOnlinePayment>();
            LNSM_DirectOnlinePayment objDOP = new LNSM_DirectOnlinePayment();
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

        #region Online Payment Against Invoice
        public void ListOfPendingInvoice(int uid, string Type)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(Type) });
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
                        PartyName = (Result["PartyId"]).ToString(),
                        InvoiceDebitNo = (Result["InvoiceDebitNote"]).ToString(),


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

        public void AddEditOnlinePaymentAgainstInvoice(LNSM_OnlinePaymentAgainstInvoice objDOP, int uid, string XML)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = objDOP.Type });

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
            List<LNSM_OnlinePaymentAgainstInvoice> lstCashReceipt = new List<LNSM_OnlinePaymentAgainstInvoice>();
            int Status = 0;

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new LNSM_OnlinePaymentAgainstInvoice
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
            List<LNSM_OnlinePaymentReceipt> lstReceipt = new List<LNSM_OnlinePaymentReceipt>();
            while (Result.Read())
            {

                Status = 1;

                lstReceipt.Add(new LNSM_OnlinePaymentReceipt
                {
                    // PeriodFrom=Convert.ToString(Result["FromDate"])
                    ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                    PayerName = Convert.ToString(Result["PayerName"]),
                    PaymentDate = Convert.ToString(Result["PaymentDate"]),
                    ReferenceNo = Convert.ToString(Result["OrderId"]),
                    AmountPaid = Convert.ToDecimal(Result["TotalPayAmount"]),
                    PayerRemarks = Convert.ToString(Result["PayerRemarks"]),
                    PayerId = Convert.ToInt32(Result["PayerId"]),
                    PaymentAmount = Convert.ToDecimal(Result["PaymentAmount"]),
                    OnlineFacCharges = Convert.ToDecimal(Result["OnlineFacCharges"]),
                    RefPaymentDate = Convert.ToString(Result["RefPaymentDate"]),
                    
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

        public void GeneratedOnlinePaymentReceiptDetails(string OrderXML,int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_OrderXML", MySqlDbType = MySqlDbType.Text, Value = OrderXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddOnlineCashReceipt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstReceipt = new List<OnlinePaymentReceipt>();
           

                Status = 1;
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Saved Successfully";
                    _DBResponse.Data = Result;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Duplicate Entry";
                    _DBResponse.Data = Result;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Invalid Amount..";
                    _DBResponse.Data = Result;
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
            List<LNSM_OnlinePaymentReceipt> lstReceipt = new List<LNSM_OnlinePaymentReceipt>();
            while (Result.Read())
            {

                Status = 1;

                try
                {
                    lstReceipt.Add(new LNSM_OnlinePaymentReceipt
                    {
                        // PeriodFrom=Convert.ToString(Result["FromDate"])
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        PayerName = Convert.ToString(Result["PayerName"]),
                        PaymentDate = Convert.ToString(Result["PaymentDate"]),
                        ReferenceNo = Convert.ToString(Result["OrderId"]),
                        AmountPaid = Convert.ToDecimal(Result["TotalPayAmount"]),
                        PayerRemarks = Convert.ToString(Result["PayerRemarks"]),
                        RefPaymentDate = Convert.ToString(Result["RefPaymentDate"]),
                        PayerId = Convert.ToInt32(Result["PayerId"]),
                        OnlineFacCharges = Convert.ToDecimal(Result["OnlineFacCharges"]),
                        PaymentAmount = Convert.ToDecimal(Result["PaymentAmount"]),
                    });
                }
                catch(Exception ex)
                {

                }
             
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

        public void GeneratedOnlinePaymentReceiptDetailsAgainstInvoice(string OrderXML, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_OrderXML", MySqlDbType = MySqlDbType.Text, Value = OrderXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddOnlineCashReceiptAgainstInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstReceipt = new List<OnlinePaymentReceipt>();


            Status = 1;
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Saved Successfully";
                    _DBResponse.Data = Result;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Duplicate Entry";
                    _DBResponse.Data = Result;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Invalid Amount..";
                    _DBResponse.Data = Result;
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
            List<LNSM_OnlinePaymentReceipt> lstReceipt = new List<LNSM_OnlinePaymentReceipt>();
            while (Result.Read())
            {

                Status = 1;

                lstReceipt.Add(new LNSM_OnlinePaymentReceipt
                {
                    ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                    PayerName = Convert.ToString(Result["PayerName"]),
                    PaymentDate = Convert.ToString(Result["PaymentDate"]),
                    ReferenceNo = Convert.ToString(Result["OrderId"]),
                    AmountPaid = Convert.ToDecimal(Result["TotalPayAmount"]),
                    PayerRemarks = Convert.ToString(Result["PayerRemarks"]),
                    RefPaymentDate = Convert.ToString(Result["RefPaymentDate"]),
                    PayerId = Convert.ToInt32(Result["PayerId"]),
                    OnlineFacCharges = Convert.ToDecimal(Result["OnlineFacCharges"]),
                    PaymentAmount = Convert.ToDecimal(Result["PaymentAmount"]),
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

        public void GeneratedOnlinePaymentReceiptDetailsBRQ(string OrderXML, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_OrderXML", MySqlDbType = MySqlDbType.Text, Value = OrderXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddBQROnlineCashReceiptAgainstInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<OnlinePaymentReceipt> lstReceipt = new List<OnlinePaymentReceipt>();


            Status = 1;
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Saved Successfully";
                    _DBResponse.Data = Result;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Duplicate Entry";
                    _DBResponse.Data = Result;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Invalid Amount..";
                    _DBResponse.Data = Result;
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

        #region On Account Add Money
        public void OAGetEximTrader(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstEximTraderForMstOnAcAddMoney", CommandType.StoredProcedure, Dparam);
            LNSMOASearchEximTraderData objSD = new LNSMOASearchEximTraderData();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objSD.lstExim.Add(new LNSMOAListOfEximTrader
                    {
                        EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        EximTraderName = Convert.ToString(Result["EximTraderName"]),
                        PartyCode = Result["EximTraderAlias"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objSD.State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objSD;
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

        public void OASearchByPartyCode(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstEximTraderForMstOnAcAddMoney", CommandType.StoredProcedure, Dparam);
            LNSMOASearchEximTraderData objSD = new LNSMOASearchEximTraderData();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objSD.lstExim.Add(new LNSMOAListOfEximTrader
                    {
                        EximTraderId = Convert.ToInt32(Result["EximTraderId"]),
                        EximTraderName = Convert.ToString(Result["EximTraderName"]),
                        PartyCode = Result["EximTraderAlias"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objSD;
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

        public void AddOAAddMoney(LNSMOAAddMoney ObjSD, string xml)
        {
            string ReturnObj = "";
            string Param = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjSD.EximTraderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FolioNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjSD.FolioNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjSD.Date) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = ObjSD.Amount });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Xml", MySqlDbType = MySqlDbType.Text, Value = xml });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjSD.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjSD.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });


            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 60, Value = ObjSD.OnAcId, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            _DBResponse = new DatabaseResponse();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditOAAddMoney", CommandType.StoredProcedure, DParam, out Param, out ReturnObj);

            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = ReturnObj;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Add Money to On Account Saved Successfully";

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

        public void GetAllOAAddMoneyList(string ReceiptNo = "", int Page = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = ReceiptNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstOAAddMoney", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            //   WFLDOAAddMoney ObjAddMoneyToOA = null;
            List<LNSMOAAddMoney> lstAddMoneyToOA = new List<LNSMOAAddMoney>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstAddMoneyToOA.Add(new LNSMOAAddMoney
                    {

                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        TransDate = Convert.ToString(Result["ReceiptDate"] == null ? "" : Result["ReceiptDate"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstAddMoneyToOA;
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

                Result.Dispose();
                Result.Close();
            }
        }

        #endregion

        #region Adjustment of Advance Payment/ Credit Note/ TDS
        public void GetAdjustmentCashRcptDetails(int PartyId, string PartyName)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            //lstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = Type });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAdjustmentCshDtlsAgnstCashParty", CommandType.StoredProcedure, DParam);

            var model = new LNSM_AdjustmentCashReceiptModel();
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

                    model.AdjCashReceiptInvoiveMappingList.Add(new LNSM_AdjustmentCashReceiptInvoiveMapping
                    {
                        CashReceiptId = Convert.ToInt32(Result["CashReceiptId"]),
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                        DocType = Convert.ToString(Result["DocType"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        BalanceAmount = Convert.ToDecimal(Result["BalanceAmount"]),
                        AdjustAmount = 0
                    });
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        model.AdjustmentMappingList.Add(new LNSM_AdjustmentCashReceiptMapping
                        {
                            Id = Convert.ToInt32(Result["Id"]),
                            DocType = Convert.ToString(Result["DocType"]),
                            DocNo = Convert.ToString(Result["DocNo"]),
                            DocDate = Convert.ToString(Result["DocDate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            DueAmount = Convert.ToDecimal(Result["DueAmount"]),
                            AdjustAmount = 0                            
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

        // ADD Adjustment Cash Receipt
        public void AddAdjustmentCashReceipt(LNSM_AdjustmentCashReceiptModel ObjCashRcpt)
        {
            string GeneratedClientId = "0";
            string ReceiptNo = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();            
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjCashRcpt.ReceiptDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjCashRcpt.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAdjustedAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjCashRcpt.TotalPaymentReceipt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_cashreceiptOpenDocxml", MySqlDbType = MySqlDbType.VarChar, Value = ObjCashRcpt.CashReceiptInvDtlsHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_cashreceiptAdjustxml", MySqlDbType = MySqlDbType.VarChar, Value = ObjCashRcpt.AdjustReceiptInvDtlsHtml });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = ObjCashRcpt.Remarks });            
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });                       
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "0" });
            
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddAdjustAdvancePayment", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReceiptNo);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    var data = new { ReceiptNo = ReceiptNo, CashReceiptId = Convert.ToInt32(GeneratedClientId) };
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Adjustment Receipt Generated Successfully.";
                    _DBResponse.Data = data;                    
                   
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = ReceiptNo;
                }
                else if (Result == -1)
                {
                    _DBResponse.Status = -1;
                    _DBResponse.Message = ReceiptNo;
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

        public void GetAdjustBulkCashreceipt(string FromDate, string ToDate, string ReceiptNo)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(FromDate == null ? "" : FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ToDate == null ? "" : ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ReceiptNo });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetBulkAdjustCashRecptForPrint", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result != null && Result.Tables[1].Rows.Count > 0)
                {
                    Status = 1;
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Result;
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

        public void GetAdjustCashReceiptList(string ReceiptNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = ReceiptNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAdjustCashReceiptList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<LNSM_AdjustmentCashReceiptModel> lstCashReceipt = new List<LNSM_AdjustmentCashReceiptModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new LNSM_AdjustmentCashReceiptModel
                    {
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        TotalValue = Convert.ToDecimal(Result["Amount"]),
                        AdjCashReceiptId = Convert.ToInt32(Result["CashReceiptId"])

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

        public void GetLoadMoreAdjustCashReceiptList(string ReceiptNo, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNo", MySqlDbType = MySqlDbType.VarChar, Value = ReceiptNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAdjustCashReceiptList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<LNSM_AdjustmentCashReceiptModel> lstCashReceipt = new List<LNSM_AdjustmentCashReceiptModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCashReceipt.Add(new LNSM_AdjustmentCashReceiptModel
                    {
                        ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                        ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        TotalValue = Convert.ToDecimal(Result["Amount"]),
                        AdjCashReceiptId = Convert.ToInt32(Result["CashReceiptId"])

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

    }
}