using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using System.Data;
using MySql.Data.MySqlClient;
using CwcExim.Areas.Export.Models;
using System.Configuration;
using CwcExim.Models;
using CwcExim.Areas.Import.Models;
using System.Globalization;
using CwcExim.Areas.Report.Models;
using EinvoiceLibrary;
using System.IO;
using CwcExim.UtilityClasses;

namespace CwcExim.Repositories
{
    public class CHN_ReportRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        #region CashStatement
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


        public void GetUserForCashStatement()
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetUserForCashStatement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            List<Chn_UserDetails> lstUser = new List<Chn_UserDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstUser.Add(new Chn_UserDetails
                    {
                        UserName = Result["UserName"].ToString(),
                        UId = Convert.ToInt32(Result["Uid"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstUser;
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
        public void PrintCashStatement(int UserId, string ReceiptDate)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = UserId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = (ReceiptDate == "" ? null : Convert.ToDateTime(ReceiptDate).ToString("yyyy-MM-dd")) });
            IDataParameter[] Dpram = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DA.ExecuteDataReader("GetCashStatementForPrint", CommandType.StoredProcedure, Dpram);
            _DBResponse = new DatabaseResponse();
            CHN_CashStatement ObjCash = new CHN_CashStatement();
            int Status = 0;
            try
            {
                while (Result.Read())
                {

                    ObjCash.CompanyAddress = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        ObjCash.LstCash.Add(new CHN_CashStatementDetail
                        {
                            UserId = Convert.ToInt32(Result["UserId"]),
                            ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                            StartCR = Result["StartCR"].ToString(),
                            EndCR = Convert.ToString(Result["EndCR"]),
                            UserName = Result["UserName"].ToString(),
                            CashAmt = Convert.ToDecimal(Result["CashAmt"]),
                            ChequeAmt = Convert.ToDecimal(Result["ChequeAmt"]),
                            NEFTAmt = Convert.ToDecimal(Result["NEFTAmt"]),
                            TotalCR = Convert.ToInt32(Result["TotalCR"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        ObjCash.LstChq.Add(new CHN_ChequeStatemnt
                        {
                            UserId = Convert.ToInt32(Result["UserId"]),
                            ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                            ChequeDate = Result["ChequeDate"].ToString(),
                            ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                            ChequeNo = Result["chequeNumber"].ToString(),
                            ChequeAmt = Convert.ToDecimal(Result["Amount"])
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = ObjCash;
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
        public void PrintCashStatementAdmin(int UserId, string ReceiptDate)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = UserId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = (ReceiptDate == "" ? null : Convert.ToDateTime(ReceiptDate).ToString("yyyy-MM-dd")) });
            IDataParameter[] Dpram = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DA.ExecuteDataReader("GetCashStatementForPrintAdmin", CommandType.StoredProcedure, Dpram);
            _DBResponse = new DatabaseResponse();
            CHN_CashStatement ObjCash = new CHN_CashStatement();
            int Status = 0;
            try
            {
                while (Result.Read())
                {

                    ObjCash.CompanyAddress = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        ObjCash.LstCash.Add(new CHN_CashStatementDetail
                        {
                            UserId = Convert.ToInt32(Result["UserId"]),
                            ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                            StartCR = Result["StartCR"].ToString(),
                            EndCR = Convert.ToString(Result["EndCR"]),
                            UserName = Result["UserName"].ToString(),
                            CashAmt = Convert.ToDecimal(Result["CashAmt"]),
                            ChequeAmt = Convert.ToDecimal(Result["ChequeAmt"]),
                            NEFTAmt = Convert.ToDecimal(Result["NEFTAmt"]),
                            TotalCR = Convert.ToInt32(Result["TotalCR"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        ObjCash.LstChq.Add(new CHN_ChequeStatemnt
                        {
                            UserId = Convert.ToInt32(Result["UserId"]),
                            ReceiptDate = Convert.ToString(Result["ReceiptDate"]),
                            ChequeDate = Result["ChequeDate"].ToString(),
                            ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                            ChequeNo = Result["chequeNumber"].ToString(),
                            ChequeAmt = Convert.ToDecimal(Result["Amount"])
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = ObjCash;
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

        #endregion

        #region Bulk Report
        public void GenericBulkInvoiceDetailsForPrint(BulkInvoiceReport ObjBulkInvoiceReport)
        {
            DateTime dtfrom = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            if (String.IsNullOrWhiteSpace(ObjBulkInvoiceReport.InvoiceModule))
            {
                ObjBulkInvoiceReport.InvoiceModule = "";
            }
            if (String.IsNullOrWhiteSpace(ObjBulkInvoiceReport.InvoiceNumber))
            {
                ObjBulkInvoiceReport.InvoiceNumber = "";
            }
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = ObjBulkInvoiceReport.InvoiceNumber });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjBulkInvoiceReport.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = ObjBulkInvoiceReport.InvoiceModule });

            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetBulkInvoiceDetailsForPrint", CommandType.StoredProcedure, DParam);
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

        #region SD Details Statement
        public void GetAllPartyForSDDet(string PartyCode, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPartyForSDDet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PartyForChnSDDet> LstParty = new List<PartyForChnSDDet>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstParty.Add(new PartyForChnSDDet
                    {
                        Party = Result["Party"].ToString(),
                        PartyId = Convert.ToInt32(Result["PartyId"]),
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
                    _DBResponse.Data = new { LstParty, State };
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

        public void GetSDDetStatement(int PartyId, string Fdt, String Tdt)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SDPartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Fdt", MySqlDbType = MySqlDbType.Date, Value = Fdt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Tdt", MySqlDbType = MySqlDbType.Date, Value = Tdt });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSDDetailsStatement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            ChnSDDetailsStatement SDResult = new ChnSDDetailsStatement();
            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    SDResult.PartyName = Result["PartyName"].ToString();
                    SDResult.PartyCode = Result["PartyCode"].ToString();
                    SDResult.PartyGst = Result["PartyGst"].ToString();
                    SDResult.CompanyGst = Result["CompanyGst"].ToString();
                    SDResult.CompanyAddress = Result["CompanyAddress"].ToString();
                    SDResult.FolioNo= Result["FolioNo"].ToString();
                    //SDResult.UtilizationAmount = Convert.ToDecimal(Result["UtilizationAmount"]);

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        SDResult.lstInvc.Add(
                            new ChnSDInvoiceDet
                            {
                                //Sl, InvoiceNo, InvoiceDate, ReceiptNo, ReceiptDate, ReceiptAmt, TranType, TranAmt
                                SL = Convert.ToInt32(Result["Sl"]),
                                InvoiceNo = Result["InvoiceNo"].ToString(),
                                InvoiceDate = Result["InvoiceDate"].ToString(),
                                ReceiptNo = Result["ReceiptNo"].ToString(),
                                ReceiptDate = Result["ReceiptDate"].ToString(),
                                ReceiptAmt = Convert.ToDecimal(Result["ReceiptAmt"]),
                                TranType = Result["TranType"].ToString(),
                                TranAmt = Convert.ToDecimal(Result["TranAmt"]),


                            });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        SDResult.UtilizationAmount = Convert.ToDecimal(Result["UtilizationAmount"]);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = SDResult;
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

        #region Bill Cum SD Ledger
        public void GetBillCumSDLedgerReport(int partyId, string fromdate, string todate, string comname, string address)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = partyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = fromdate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = todate });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("BillCumSDLedger", CommandType.StoredProcedure, DParam);
            BillCumSDLedger LedgerObj = new BillCumSDLedger();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {

                    LedgerObj.OpenningBalance = Convert.ToDecimal(Result["OpenningBalance"]);
                    LedgerObj.EximTraderName = Convert.ToString(Result["EximTraderName"]);
                    LedgerObj.EximTraderAlias = Convert.ToString(Result["EximTraderAlias"]);

                    LedgerObj.COMGST = Convert.ToString(Result["COMGST"]);
                    LedgerObj.COMPAN = Convert.ToString(Result["COMPAN"]);
                    LedgerObj.CurDate = DateTime.Now.ToString("dd-MMM-yyyy");

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        LedgerObj.lstDetails.Add(new BillCumSDLedgerDetails
                        {
                            Sr = Convert.ToInt32(Result["Sr"]),
                            Col1 = Convert.ToString(Result["Col1"]),
                            Col2 = Convert.ToString(Result["Col2"]),
                            Col3 = Convert.ToString(Result["Col3"]),
                            Col4 = Convert.ToString(Result["Col4"]),
                            Col5 = Convert.ToString(Result["Col5"]),
                            Col6 = Convert.ToString(Result["Col6"]),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        LedgerObj.ClosingBalance = Convert.ToDecimal(Result["ClosingBalance"]);
                    }
                }


                if (Status == 1)
                {
                    LedgerObj.CompanyName = comname;
                    LedgerObj.CompanyAddress = address;


                    /*
                    CrInvLedgerObj.ClosingBalance = (CrInvLedgerObj.OpenningBalance + (CrInvLedgerObj.lstLedgerSummary.Sum(o => o.Debit)))
                                                    - (CrInvLedgerObj.lstLedgerSummary.Sum(o => o.Credit));
                    */

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LedgerObj;
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
        #endregion

        #region CashReceiptInvoiceLedgerReport Partywise
        public void GetCrInvLedgerReport(int partyId, string fromdate, string todate, string comname, string address)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = partyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = fromdate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = todate });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("CashReceiptInvoiceLedger", CommandType.StoredProcedure, DParam);
            CashReceiptInvoiceLedger CrInvLedgerObj = new CashReceiptInvoiceLedger();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {

                    CrInvLedgerObj.OpenningBalance = Convert.ToDecimal(Result["OpenningBalance"]);
                    CrInvLedgerObj.EximTraderName = Convert.ToString(Result["EximTraderName"]);
                    CrInvLedgerObj.Address = Convert.ToString(Result["Address"]);
                    CrInvLedgerObj.City = Convert.ToString(Result["City"]);
                    CrInvLedgerObj.State = Convert.ToString(Result["State"]);
                    CrInvLedgerObj.GSTNo = Convert.ToString(Result["GSTNo"]);
                    CrInvLedgerObj.PinCode = Convert.ToString(Result["PinCode"]);
                    CrInvLedgerObj.COMGST = Convert.ToString(Result["COMGST"]);
                    CrInvLedgerObj.COMPAN = Convert.ToString(Result["COMPAN"]);
                    CrInvLedgerObj.CurDate = DateTime.Now.ToString("dd-MMM-yyyy");

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        CrInvLedgerObj.lstLedgerSummary.Add(new CrInvLedgerSummary
                        {
                            InvCr = Convert.ToInt32(Result["InvCr"]),
                            InvCrId = Convert.ToInt32(Result["InvCrId"]),
                            InvCrNo = Convert.ToString(Result["InvCrNo"]),
                            InvCrDate = Convert.ToString(Result["InvCrDate"]),
                            Debit = Convert.ToDecimal(Result["Debit"]),
                            Credit = Convert.ToDecimal(Result["Credit"]),
                            CreatedOn = Convert.ToString(Result["CreatedOn"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        CrInvLedgerObj.lstLedgerDetails.Add(new CrInvLedgerDetails
                        {
                            InvCr = Convert.ToInt32(Result["InvCr"]),
                            InvCrId = Convert.ToInt32(Result["InvCrId"]),
                            Description = Convert.ToString(Result["Description"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        CrInvLedgerObj.lstLedgerDetailsFull.Add(new CrInvLedgerFullDetails
                        {
                            Sr = Convert.ToInt32(Result["Sr"]),
                            //Sr, ReceiptDt, ReceiptNo, ChargeCode, ContNo, Size, Debit, Credit, Balance, GroupSr, InvCr, InvCrId
                            ReceiptDt = Convert.ToString(Result["ReceiptDt"]),
                            ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                            ChargeCode = Convert.ToString(Result["ChargeCode"]),
                            ContNo = Convert.ToString(Result["ContNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            Debit = Convert.ToDecimal(Result["Debit"]),
                            Credit = Convert.ToDecimal(Result["Credit"]),
                            Balance = Convert.ToDecimal(Result["Balance"]),
                            GroupSr = Convert.ToString(Result["GroupSr"]),
                        });
                    }
                }

                if (Status == 1)
                {
                    CrInvLedgerObj.CompanyName = comname;
                    CrInvLedgerObj.CompanyAddress = address;

                    CrInvLedgerObj.lstLedgerSummary.ForEach(item =>
                    {
                        var dtls = CrInvLedgerObj.lstLedgerDetails.Where(o => o.InvCr == item.InvCr && o.InvCrId == item.InvCrId).ToList();
                        dtls.ForEach(d =>
                        {
                            item.LedgerDetails.Add(d);
                        });
                    });

                    CrInvLedgerObj.TotalDebit = CrInvLedgerObj.lstLedgerDetailsFull.Sum(x => x.Debit);
                    CrInvLedgerObj.TotalCredit = CrInvLedgerObj.lstLedgerDetailsFull.Sum(x => x.Credit);
                    CrInvLedgerObj.ClosingBalance = CrInvLedgerObj.OpenningBalance + CrInvLedgerObj.TotalCredit - CrInvLedgerObj.TotalDebit;

                    /*
                    CrInvLedgerObj.ClosingBalance = (CrInvLedgerObj.OpenningBalance + (CrInvLedgerObj.lstLedgerSummary.Sum(o => o.Debit)))
                                                    - (CrInvLedgerObj.lstLedgerSummary.Sum(o => o.Credit));
                    */

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CrInvLedgerObj;
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
        #endregion

        #region Daily PDA Activity Report
        public void DailyPdaActivity(DailyPdaActivityReport ObjDailyPdaActivityReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyPdaActivityReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyPdaActivityReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyPdActivityReport", CommandType.StoredProcedure, DParam);
            IList<ChnDailyPdaActivityReport> LstDailyPdaActivityReport = new List<ChnDailyPdaActivityReport>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //ObjStatusDtl = new ConsumerList();
                    //ObjStatusDtl.RegistrationNo = Convert.ToString(Result["RegistrationNo"]);
                    //ObjStatusDtl.Name = Convert.ToString(Result["CompanyName"]);
                    //ObjStatusDtl.Address = Convert.ToString(Result["CompanyAddress"]);
                    //ObjStatusDtl.IssueDate = Convert.ToString(Result["IssuedOn"]);


                    //                 InvoiceDate DATE,
                    //         InvoiceNumber       VARCHAR(30),
                    //ReceiptAmount DECIMAL(18, 3),
                    //InvoiceAmount DECIMAL(18, 3),
                    //Value DECIMAL(18, 3),
                    //OpeningBalance DECIMAL(18, 3),
                    //ClosingBalance DECIMAL(18, 3)
                    if (Result["Deposit"].ToString().Equals("0.00") && Result["Withdraw"].ToString().Equals("0.00"))
                    {

                    }
                    else
                    {
                        LstDailyPdaActivityReport.Add(new ChnDailyPdaActivityReport
                        {
                            Date = Result["Date"].ToString(),
                            ReceiptNo = Result["ReceiptNo"].ToString(),
                            Party = Result["Party"].ToString(),
                            Deposit = Result["Deposit"].ToString(),
                            Withdraw = Result["Withdraw"].ToString()

                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDailyPdaActivityReport;
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

        #endregion

        #region PDSummary
        public void PdSummaryReport(ChnPdSummary ObjPdSummaryReport, int type = 1)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjPdSummaryReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            // DateTime dtTo = DateTime.ParseExact(ObjCargoInStockReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_asOndate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.Int32, Value = type });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PdSummaryReport", CommandType.StoredProcedure, DParam);
            IList<ChnPdSummary> LstPdSummaryReport = new List<ChnPdSummary>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //ObjStatusDtl = new ConsumerList();
                    //ObjStatusDtl.RegistrationNo = Convert.ToString(Result["RegistrationNo"]);
                    //ObjStatusDtl.Name = Convert.ToString(Result["CompanyName"]);
                    //ObjStatusDtl.Address = Convert.ToString(Result["CompanyAddress"]);
                    //ObjStatusDtl.IssueDate = Convert.ToString(Result["IssuedOn"]);


                    //                 InvoiceDate DATE,
                    //         InvoiceNumber       VARCHAR(30),
                    //ReceiptAmount DECIMAL(18, 3),
                    //InvoiceAmount DECIMAL(18, 3),
                    //Value DECIMAL(18, 3),
                    //OpeningBalance DECIMAL(18, 3),
                    //ClosingBalance DECIMAL(18, 3)

                    LstPdSummaryReport.Add(new ChnPdSummary
                    {



                        PartyName = Result["PartyName"].ToString(),

                        Amount = Result["Amount"].ToString()

                        //ContainerNo = Result["ContainerNo"].ToString(),
                        //value = Result["value"].ToString()

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPdSummaryReport;
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

        #endregion

        #region  SD A/c Statement
        public void GetPDAStatement(int Month, int Year)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.Int32, Value = Month });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Year", MySqlDbType = MySqlDbType.Int32, Value = Year });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("RptSdStatement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            ChnSDStatement ObjSDStatement = new ChnSDStatement();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        ObjSDStatement.LstSD.Add(new ChnSDList
                        {
                            PartyName = Convert.ToString(dr["PartyName"]),
                            SDAmount = Convert.ToString(dr["SDAmount"]),
                            UnpaidAmount = Convert.ToString(dr["UnpaidAmount"]),
                            BalanceAmount = Convert.ToString(dr["BalanceAmount"]),
                            AdjustAmount = Convert.ToDecimal(dr["AdjustAmount"]),
                            UtilizationAmount = Convert.ToDecimal(dr["UtilizationAmount"]),
                            RefundAmount = Convert.ToDecimal(dr["RefundAmount"])



                        });
                    }
                }

                ObjSDStatement.LstSD.Add(new ChnSDList
                {
                    PartyName = "Total",
                    SDAmount = Convert.ToString(ObjSDStatement.LstSD.Sum(x => Convert.ToDecimal(x.SDAmount))),
                    UnpaidAmount = Convert.ToString(ObjSDStatement.LstSD.Sum(x => Convert.ToDecimal(x.UnpaidAmount))),
                    BalanceAmount = Convert.ToString(ObjSDStatement.LstSD.Sum(x => Convert.ToDecimal(x.BalanceAmount))),
                    AdjustAmount = Convert.ToDecimal(ObjSDStatement.LstSD.Sum(x => x.AdjustAmount)),
                    UtilizationAmount = Convert.ToDecimal(ObjSDStatement.LstSD.Sum(x => x.UtilizationAmount)),
                    RefundAmount = Convert.ToDecimal(ObjSDStatement.LstSD.Sum(x => x.RefundAmount))



                });

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSDStatement;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data Found";
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

        #region WorkSlip
        public void GetWorkSlipReportList(CHN_WorkSlipReport ObjWorkSlipReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjWorkSlipReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjWorkSlipReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "inv_StartDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "inv_EndDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            // LstParam.Add(new MySqlParameter { ParameterName = "repoType", MySqlDbType = MySqlDbType.String, Value = ObjWorkSlipReport.WorkSlipType });

            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("RptWorkSlip", CommandType.StoredProcedure, DParam);

            CHN_WorkSlipReport newObjWorkSlipReport = new CHN_WorkSlipReport();

            newObjWorkSlipReport.WorkSlipSummaryList = new List<CHN_WorkSlipSummary>();
            newObjWorkSlipReport.WorkSlipDetailList = new List<CHN_WorkSlipDetail>();

            _DBResponse = new DatabaseResponse();
            try
            {
                int count = 0;
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        count++;
                        //newObjWorkSlipReport.WorkSlipSummaryList.Add(new WorkSlipSummary
                        //{
                        //    SrNo = count,
                        //    Particulars = item["Particulars"].ToString(),
                        //    Clause = item["Clause"].ToString(),
                        //    SAC = item["SAC"].ToString(),
                        //    NoOfPackages = item["NoOfPackages"].ToString(),
                        //    GrossWeight = item["GrossWeight"].ToString(),
                        //    Count_20 = item["Count_20"] == null ? (int?)null : Convert.ToInt32(item["Count_20"]),
                        //    Count_40 = item["Count_40"] == null ? (int?)null : Convert.ToInt32(item["Count_40"])

                        //});
                        CHN_WorkSlipSummary objWorkSlipSummary = new CHN_WorkSlipSummary();
                        objWorkSlipSummary.SrNo = count;
                        objWorkSlipSummary.Particulars = Convert.ToString(item["ChargeName"]);
                        objWorkSlipSummary.Clause = Convert.ToString(item["Clause"]);
                        objWorkSlipSummary.SAC = Convert.ToString(item["SAC"]);
                        objWorkSlipSummary.NoOfPackages = Convert.ToString(item["NoOfPackages"]);
                        objWorkSlipSummary.GrossWeight = Convert.ToString(item["GrossWeight"]);

                        if (item["Count_20"] != System.DBNull.Value)
                        {
                            objWorkSlipSummary.Count_20 = Convert.ToInt32(item["Count_20"]);
                        }
                        if (item["Count_40"] != System.DBNull.Value)
                        {
                            objWorkSlipSummary.Count_40 = Convert.ToInt32(item["Count_40"]);
                        }

                        newObjWorkSlipReport.WorkSlipSummaryList.Add(objWorkSlipSummary);

                    }

                    foreach (DataRow item in Result.Tables[1].Rows)
                    {
                        newObjWorkSlipReport.WorkSlipDetailList.Add(new CHN_WorkSlipDetail
                        {
                            Clause = item["Clause"].ToString(),
                            ContainerSize = item["ContainerSize"].ToString(),
                            Containers = item["Containers"].ToString(),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = newObjWorkSlipReport;
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

        #region PV Report

        public void GetGodownRightsWise(int Uid)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGodownListAccdRights", CommandType.StoredProcedure, DParam);
            IList<Areas.Export.Models.GodownList> lst = new List<Areas.Export.Models.GodownList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lst.Add(new Areas.Export.Models.GodownList
                    {
                        GodownName = Result["GodownName"].ToString(),
                        GodownId = Convert.ToInt32(Result["GodownId"])
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lst;
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
        public void GetPVReportImport(string AsOnDate, string Module, int GodownId)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PVReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Chn_ImpPVReport> lstPV = new List<Chn_ImpPVReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Chn_ImpPVReport
                    {
                        CompanyLocation = Result["CompanyLocation"].ToString(),
                        CompanyBranch = Result["CompanyBranch"].ToString(),
                        BOLNo = Result["BOLNo"].ToString(),
                        DestuffingEntryDate = Result["DestuffingEntryDate"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        CommodityAlias = Result["CommodityAlias"].ToString(),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                        NoOfUnitsRec = Convert.ToInt32(Result["NoOfUnitsRec"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        Area = Convert.ToDecimal(Result["Area"].ToString()),
                        LocationName = Result["LocationName"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPV;
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
        public void GetPVReportExport(string AsOnDate, string Module, int GodownId)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PVReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Chn_ExpPvReport> lstPV = new List<Chn_ExpPvReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Chn_ExpPvReport
                    {
                        CompanyLocation = Result["CompanyLocation"].ToString(),
                        CompanyBranch = Result["CompanyBranch"].ToString(),
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
                        ShippingBillDate = Result["ShippingBillDate"].ToString(),
                        EntryNo = Result["EntryNo"].ToString(),
                        RegisterDate = Result["RegisterDate"].ToString(),
                        Units = Convert.ToInt32(Result["Units"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        Fob = Convert.ToDecimal(Result["Fob"]),
                        Area = Convert.ToDecimal(Result["Area"]),
                        EximTraderName = Result["EximTraderName"].ToString(),
                        EximTraderAlias = Result["EximTraderAlias"].ToString(),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        LocationName = Result["LocationName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPV;
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
        public void GetPVReportImportExcel(string AsOnDate, string Module, int GodownId, string GodownName)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            DParam = LstParam.ToArray();
            DataSet ds = DataAccess.ExecuteDataSet("PVReportExcel", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            List<Chn_ImpPVReportExcel> lstPV = new List<Chn_ImpPVReportExcel>();
            try
            {



                if (ds.Tables.Count > 0)
                {
                    lstPV = (from DataRow dr in dt.Rows
                             select new Chn_ImpPVReportExcel()
                             {
                                 // CompanyLocation = dr["CompanyLocation"].ToString(),
                                 // CompanyBranch =dr["CompanyBranch"].ToString(),
                                 SlNo = Convert.ToInt32(dr["SlNo"]),
                                 BOLNo = dr["BOLNo"].ToString(),
                                 DestuffingEntryDate = dr["DestuffingEntryDate"].ToString(),
                                 CFSCode = dr["CFSCode"].ToString(),
                                 CommodityAlias = dr["CommodityAlias"].ToString(),
                                 NoOfUnits = Convert.ToInt32(dr["NoOfUnits"]),
                                 NoOfUnitsRec = Convert.ToInt32(dr["NoOfUnitsRec"]),
                                 Weight = Convert.ToDecimal(dr["Weight"]),
                                 Area = Convert.ToDecimal(dr["Area"].ToString()),
                                 LocationName = dr["LocationName"].ToString(),
                                 Remarks = dr["Remarks"].ToString(),
                             }).ToList();
                }

                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = PVReportImportExcel(lstPV, AsOnDate, GodownName);
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Close();
                ds.Dispose();

            }
        }
        private string PVReportImportExcel(List<Chn_ImpPVReportExcel> lstPV, string AsOnDate, string GodownName)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = "CENTRAL WAREHOUSING CORPORATION";
                var h1 = "(A Govt.of India Undertaking)";
                var h2 = "CFS Madhavaram-Chennai";
                var h3 = "As On Date -" + AsOnDate + "";
                var h4 = "Shed Cd -" + GodownName + "";
                var h5 = "Physical Verification Report for Import Cargo";



                exl.MargeCell("A1:K1", title, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A2:K2", h1, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A3:K3", h2, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A4:K4", h3, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A5:K5", h4, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A6:K6", h5, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("A7:A7", "SL. No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B7:B7", "OBL No", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C7:C7", "Dstf Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D7:D7", "ICD Code", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E7:E7", "Item No", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F7:F7", "No Pkg", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G7:G7", "Rcvd Pkg", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H7:H7", "Gr W", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I7:I7", "Area", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J7:J7", "SlotNo", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K7:K7", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);


                exl.AddTable<Chn_ImpPVReportExcel>("A", 8, lstPV, new[] { 6, 20, 20, 20, 20, 20, 10, 15, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 });
                var NoOfUnits = lstPV.Sum(o => o.NoOfUnits);
                var NoOfUnitsRec = lstPV.Sum(o => o.NoOfUnitsRec);
                var Weight = lstPV.Sum(o => o.Weight);
                var Area = lstPV.Sum(o => o.Area);



                exl.MargeCell("A" + (lstPV.Count + 8).ToString() + ":E" + (lstPV.Count + 8).ToString() + "", "TOTAL", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("F" + (lstPV.Count + 8).ToString(), NoOfUnits.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("G" + (lstPV.Count + 8).ToString(), NoOfUnitsRec.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("H" + (lstPV.Count + 8).ToString(), Weight.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("I" + (lstPV.Count + 8).ToString(), Area.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                // exl.AddCell("O" + (lstPV.Count + 8).ToString(), Area.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("P" + (lstPV.Count + 8).ToString(), CBM.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("Q" + (lstPV.Count + 8).ToString(), CIF.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);



                exl.Save();
            }
            return excelFile;
        }
        public void GetPVReportExportExcel(string AsOnDate, string Module, int GodownId, string GodownName)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            DParam = LstParam.ToArray();
            DataSet ds = DataAccess.ExecuteDataSet("PVReportExcel", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            List<Chn_ExpPvReportExcel> obj = new List<Chn_ExpPvReportExcel>();
          //  DataTable dt = ds.Tables[0];
            try
            {



               
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = GeneratingExcelForExportPv(obj, dt,AsOnDate, GodownName);
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Close();
                ds.Dispose();

            }
        }
        private string GeneratingExcelForExportPv(List<Chn_ExpPvReportExcel> obj, DataTable dt, string AsOnDate, string GodownName)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            System.Web.UI.WebControls.GridView Grid = new System.Web.UI.WebControls.GridView();


            System.Web.UI.WebControls.Table tb = new System.Web.UI.WebControls.Table();

            if (dt.Rows.Count > 0)
            {

                Grid.AllowPaging = false;
                Grid.DataSource = dt;
                Grid.DataBind();


                for (int i = 0; i < Grid.HeaderRow.Cells.Count; i++)
                {
                    Grid.HeaderRow.Cells[i].Style.Add("background-color", "#edebeb");
                    Grid.HeaderRow.Cells[i].Attributes.Add("class", "Headermode");

                }

                for (int i = 0; i < Grid.Rows.Count; i++)
                {

                    Grid.Rows[i].BackColor = System.Drawing.Color.White;


                    if (i % 2 != 0)
                    {
                        Grid.Rows[i].Attributes.Add("class", "textmode");
                    }
                    else
                    {
                        Grid.Rows[i].Attributes.Add("class", "textmode2");
                    }

                }


                System.Web.UI.WebControls.TableCell cell1 = new System.Web.UI.WebControls.TableCell();
                cell1.Text = "<b> CENTRAL WAREHOUSING CORPORATION </b> ";
                cell1.ForeColor = System.Drawing.Color.Black;
                System.Web.UI.WebControls.TableRow tr1 = new System.Web.UI.WebControls.TableRow();
                tr1.Cells.Add(cell1);
                tr1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;

                System.Web.UI.WebControls.TableCell cell2 = new System.Web.UI.WebControls.TableCell();
                cell2.Text = "(A Govt.of India Undertaking) " ;
                cell2.ForeColor = System.Drawing.Color.Black;

                System.Web.UI.WebControls.TableRow tr2 = new System.Web.UI.WebControls.TableRow();
                tr2.Cells.Add(cell2);
                tr2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                System.Web.UI.WebControls.TableCell cell3= new System.Web.UI.WebControls.TableCell();
                cell3.Text = "CFS Madhavaram-Chennai";
                cell3.ForeColor = System.Drawing.Color.Black;

                System.Web.UI.WebControls.TableRow tr3 = new System.Web.UI.WebControls.TableRow();
                tr3.Cells.Add(cell3);
                tr3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;


                System.Web.UI.WebControls.TableCell cell4 = new System.Web.UI.WebControls.TableCell();
                cell4.Text = "Stock Register of Shed No... " +GodownName+"";
                System.Web.UI.WebControls.TableRow tr4 = new System.Web.UI.WebControls.TableRow();
                cell4.ForeColor = System.Drawing.Color.Black;

                tr4.Cells.Add(cell4);
                tr4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;

                System.Web.UI.WebControls.TableCell cell5 = new System.Web.UI.WebControls.TableCell();
                cell5.Text = "As on Date... " + AsOnDate + "";
                System.Web.UI.WebControls.TableRow tr5 = new System.Web.UI.WebControls.TableRow();
                cell5.ForeColor = System.Drawing.Color.Black;

                tr5.Cells.Add(cell5);
                tr5.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                System.Web.UI.WebControls.TableCell cell6 = new System.Web.UI.WebControls.TableCell();
                cell6.Text = "Physical Verification Report for Export Cargo";
                System.Web.UI.WebControls.TableRow tr6 = new System.Web.UI.WebControls.TableRow();
                cell6.ForeColor = System.Drawing.Color.Black;

                tr6.Cells.Add(cell6);
                tr6.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;

                System.Web.UI.WebControls.TableCell cell7 = new System.Web.UI.WebControls.TableCell();
                cell7.Controls.Add(Grid);
                System.Web.UI.WebControls.TableRow tr7 = new System.Web.UI.WebControls.TableRow();
                tr7.Cells.Add(cell7);

                tb.Rows.Add(tr1);
                tb.Rows.Add(tr2);
                tb.Rows.Add(tr3);
                tb.Rows.Add(tr4);
                tb.Rows.Add(tr5);
                tb.Rows.Add(tr6);
                tb.Rows.Add(tr7);
               // tb.Rows.Add(tr8);

            }
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                using (System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw))
                {
                    System.IO.StreamWriter writer = System.IO.File.AppendText(excelFile);
                    tb.RenderControl(hw);
                    writer.WriteLine(sw.ToString());
                    writer.Close();
                }
            }



            return excelFile;
        }

     /*   private string GeneratingExcelForExportPv(List<Chn_ExpPvReportExcel> lstPV, string AsOnDate, string GodownName)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = "CENTRAL WAREHOUSING CORPORATION";
                var h1 = "(A Govt.of India Undertaking)";
                var h2 = "CFS Madhavaram-Chennai";
                var h3 = "As On Date -" + AsOnDate + "";
                var h4 = "Shed Cd -" + GodownName + "";
                var h5 = "Physical Verification Report for Import Cargo";



                exl.MargeCell("A1:K1", title, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A2:K2", h1, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A3:K3", h2, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A4:K4", h3, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A5:K5", h4, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A6:K6", h5, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("A7:A7", "SL. No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B7:B7", "OBL No", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C7:C7", "Dstf Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D7:D7", "ICD Code", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E7:E7", "Item No", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F7:F7", "No Pkg", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G7:G7", "Rcvd Pkg", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H7:H7", "Gr W", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I7:I7", "Area", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J7:J7", "SlotNo", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K7:K7", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);


                exl.AddTable<Chn_ExpPvReportExcel>("A", 8, lstPV, new[] { 6, 20, 20, 20, 20, 20, 10, 15, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 });
               // var NoOfUnits = lstPV.Sum(o => o.NoOfUnits);
               // var NoOfUnitsRec = lstPV.Sum(o => o.NoOfUnitsRec);
                var Weight = lstPV.Sum(o => o.Weight);
                var Area = lstPV.Sum(o => o.Area);



                exl.MargeCell("A" + (lstPV.Count + 8).ToString() + ":E" + (lstPV.Count + 8).ToString() + "", "TOTAL", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
             //   exl.AddCell("F" + (lstPV.Count + 8).ToString(), NoOfUnits.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
             //   exl.AddCell("G" + (lstPV.Count + 8).ToString(), NoOfUnitsRec.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("H" + (lstPV.Count + 8).ToString(), Weight.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("I" + (lstPV.Count + 8).ToString(), Area.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                // exl.AddCell("O" + (lstPV.Count + 8).ToString(), Area.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("P" + (lstPV.Count + 8).ToString(), CBM.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("Q" + (lstPV.Count + 8).ToString(), CIF.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);



                exl.Save();
            }
            return excelFile;
        }*/
        /*   private string GeneratingExcelForExportPv(List<Chn_ExpPvReportExcel> obj, string AsOnDate, string GodownName)
           {


               //  DataTable dts = dt.Tables[0];



               //  List<CHN_PVShippingLineExcel> CHN = new List<CHN_PVShippingLineExcel>();
               //  List<CHN_ExportPVDet> lstExppvdtl = new List<CHN_ExportPVDet>();
               String Sla = "";
               var ShippingLineId = 0;
               var EximtraderName = "";
               var EximtraderAlias = "";
               var ShippingBillNo = "";
               var ShippingBillDate = "";
               var EntryNo = "";
               List<CHN_PVShippingLineExcel> lstBTTDetailsExcel = new List<CHN_PVShippingLineExcel>();
               List<CHN_ExportPVDetExcel> lstBTTExcel = new List<CHN_ExportPVDetExcel>();
              // var data = "";
               if (obj.Count > 0)
               {
                   //  obj.lstShpDtl.Select(x => new { ShippingLineId = x.ShippingLineId, EximTraderName = x.EximTraderName, EximTraderAlias = x.EximTraderAlias }).Distinct().ToList().ForEach(x =>
                  obj.Select(x => new { ShippingLineId = x.ShippingLineId, EximTraderName = x.EximTraderName, EximTraderAlias = x.EximTraderAlias }).Distinct().ToList().ForEach(x =>
                   {
                       EximtraderName = x.EximTraderName;
                       obj.Where(y => y.ShippingLineId == x.ShippingLineId).ToList().ForEach(y =>
                       {
                           ShippingBillNo = y.ShippingBillNo;
                           ShippingBillDate = y.ShippingBillDate;
                           EntryNo = y.EntryNo;


                       });

                       // List < CHN_ExportPVDet > lstExppvdtl = new List<CHN_ExportPVDet>();

                   });

               };
           /*   var data = obj;
               lstBTTDetailsExcel = obj.Select(x => new CHN_PVShippingLineExcel

               {
                   EximtraderName = x.EximTraderName,

                   // ShippingBillNo = x.ShippingBillNo,
                   obj.Where(y => y.ShippingLineId == x.ShippingLineId).ToList().ForEach(y =>
                   {
                       ShippingBillNo = y.ShippingBillNo;
                   });
                  });

               //  obj.CopyTo=lstBTTDetailsExcel;
               //List<Chn_ExpPvReportExcelList> lstBTTDetailsExcel = new List<Chn_ExpPvReportExcelList>();
               /*       lstBTTDetailsExcel = (from rs in obj
                                            select new Chn_ExpPvReportExcel

                                            {
                                                ShippingBillNo = y.ShippingBillNo;
                      ShippingBillDate = y.ShippingBillDate;
                      EntryNo = y.EntryNo;


                  });*/

        //})
        /* List<CHN_PVShippingLineExcel> lstBTTDetailsExcel = new List<CHN_PVShippingLineExcel>();
          lstBTTDetailsExcel = obj.
Select(x => new ChnExportExcel
         {

             obj.Where(y => y.ShippingLineId == x.ShippingLineId).ToList().ForEach(y =>
             {
                 ShippingBillNo = y.ShippingBillNo;
                 ShippingBillDate = y.ShippingBillDate;
                 EntryNo = y.EntryNo;



      })
             })


      };*/

        /*     if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
             {
                 System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
             }



             var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");


             using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
             {
                 var title = @"CENTRAL WAREHOUSING CORPORATION"
                         + Environment.NewLine + "(A Govt. of India Undertaking)"
                         + Environment.NewLine + "CFS Madhavaram-Chennai"
                         + Environment.NewLine + Environment.NewLine
                         + "Physical Verification Report for Export Cargo";
                 string typeOfValue = "";

                 typeOfValue = "During Period" + AsOnDate + "  " + "GodownName (" + GodownName + ")";

                 exl.MargeCell("A1:H1", "CENTRAL WAREHOUSING CORPORATION", DynamicExcel.CellAlignment.Middle);

                 exl.MargeCell("A2:H2", "(A Govt. of India Undertaking)", DynamicExcel.CellAlignment.Middle);

                 exl.MargeCell("A3:H3", "CFS Madhavaram-Chennai", DynamicExcel.CellAlignment.Middle);

                 exl.MargeCell("A4:H4", "Physical Verification Report for Export Cargo", DynamicExcel.CellAlignment.Middle);
                 exl.MargeCell("A5:H5", typeOfValue, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                 // exl.MargeCell("A6:D4", "GodwonName :" + GodownName, DynamicExcel.CellAlignment.Middle);
                 // exl.MargeCell("A4:F4", "DETAILS OF ORIGINAL INOICE", DynamicExcel.CellAlignment.Middle);
                 // exl.MargeCell("G4:O4", "DETAILS OF CREDIT NOTE ISSUED", DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("A6:O6", "CARGO CARTING DETAILS", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                 exl.MargeCell("A7:A8", "Sr.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                 try
                 {
                  //  exl.AddTable("A", 1, obj.GroupBy(x=>x.EximTraderName).ToList(), new[] { 6 });
                  //   exl.AddTable("A", 9, obj, new[] { 20 });



                     exl.MargeCell("B7:B8", "Sb No", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                 exl.MargeCell("C7:C8", "Sb Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                 exl.MargeCell("D7:D8", "Entry No", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                 exl.MargeCell("E7:E8", "Carting Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                 exl.MargeCell("F7:F8", "No Pkg", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                 exl.MargeCell("G7:G8", "Gr Wt", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                 exl.MargeCell("H7:H8", "Fob", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                 exl.MargeCell("I7:I8", "Slot No", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                 exl.MargeCell("J7:J8", "Area", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);



                // exl.MargeCell("K7:K8", "G/R", DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("L7:L8", "Area", DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("M7:M8", "Remarks", DynamicExcel.CellAlignment.Middle);



                 //for (var i = 65; i < 86; i++)
                 //{
                 //    char character = (char)i;
                 //    string text = character.ToString();
                 //    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                 //}
                 try
                 {
                     exl.AddTable("A", 7, obj.GroupBy(x => x.ShippingLineId).ToList(), new[] { 6, 20, 20, 30, 12, 20, 20, 15, 15, 15, 14, 14, 14 });
                         { 
 }
                     }
                 catch (Exception ex)
                 {

                 }
                     try
                     {

                     }
                     catch(Exception ex)
                     {

                     }
                 }
                 catch (Exception ex)
                 {

                 }
                 exl.Save();
             }
             return excelFile;
         }*/

        /*private string PVReportExportExcel(List<Chn_ExpPvReportExcel> model, DataSet ds, DataTable dt, string AsOnDate, string GodownName)
        {
            DataTable dts = ds.Tables[0];



            List<dynamic> lstCont = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            String Sla = "";
            var ShippingLineId = 0;
            var EximtraderName = "";
            var EximtraderAlias = "";
            var ShippingBillNo = "";
            var ShippingBillDate = "";

            lstCont.Select(x => new { ShippingLineId = x.ShippingLineId, EximTraderName = x.EximTraderName, EximTraderAlias = x.EximTraderAlias }).Distinct().ToList().ForEach(x =>
            {
                EximtraderName = x.EximTraderName;
                EximtraderAlias = x.EximTraderAlias;
                int i = 1;

                List<Chn_ExpPvReportExcel> lstShortCartingDetailsExcel = new List<Chn_ExpPvReportExcel>();
                lstShortCartingDetailsExcel.Where(y => y.ShippingLineId == x.ShippingLineId).ToList().ForEach(y =>
                 {


                     ShippingBillNo = y.ShippingBillNo;
                     ShippingBillDate = y.ShippingBillDate;

                 });
            });

           /* if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }

            var excelFile = Server.MapPath("~/Docs/" + Session.SessionID + "/" + dt + ".xlsx");


            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "(A Govt. of India Undertaking)"
                        + Environment.NewLine + "ICD Patparganj-Delhi"
                        + Environment.NewLine + Environment.NewLine
                        + "Assessment Sheet LCL Report";
                string typeOfValue = "";

                typeOfValue = "During Period" + dt + "  " + "GodownName (" + GodownName + ")";

                exl.MargeCell("A1:H1", "CENTRAL WAREHOUSING CORPORATION", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A2:H2", "(A Govt. of India Undertaking)", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A3:H3", "CFS Madhavaram-Chennai", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A4:H4", "Daily Transaction Report", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A5:H5", typeOfValue, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                // exl.MargeCell("A6:D4", "GodwonName :" + GodownName, DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("A4:F4", "DETAILS OF ORIGINAL INOICE", DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("G4:O4", "DETAILS OF CREDIT NOTE ISSUED", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A6:O6", "CARGO CARTING DETAILS", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A7:A8", "EntryNo", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B7:B8", "SB No", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C7:C8", "SB Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D7:D8", "Exporter", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("E7:E8", "ShippingLine", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F7:F8", "Cargo ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G7:G8", "No Of Package", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H7:H8", "GROSS WEIGHT", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I7:I8", "FOB", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J7:J8", "Slot", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);



                exl.MargeCell("K7:K8", "G/R", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L7:L8", "Area", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M7:M8", "Remarks", DynamicExcel.CellAlignment.Middle);



                //for (var i = 65; i < 86; i++)
                //{
                //    char character = (char)i;
                //    string text = character.ToString();
                //    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                //}
                try
                {
                    exl.AddTable("A", 9, lstCont.OrderBy(x => x.ShippingLineName).ToList(), new[] { 6, 20, 20, 30, 12, 20, 20, 15, 15, 15, 14, 14, 14 });

                }
                catch (Exception ex)
                {

                }
                ///return "/Docs/" + Session.SessionID + "/" + dt + ".xlsx"; ;

            }
        }*/
    /*    private string PVReportExportExcel(ChnExportExcel model, DataTable dt, string   AsOnDate, string GodownName)
{
    if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
    {
        System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
    }
    var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
    System.Web.UI.WebControls.GridView Grid = new System.Web.UI.WebControls.GridView();


            System.Web.UI.WebControls.Table tb = new System.Web.UI.WebControls.Table();

            if (dt.Rows.Count > 0)
            {

                Grid.AllowPaging = false;
                Grid.DataSource = dt;
                Grid.DataBind();


                for (int i = 0; i < Grid.HeaderRow.Cells.Count; i++)
                {
                    Grid.HeaderRow.Cells[i].Style.Add("background-color", "#edebeb");
                    Grid.HeaderRow.Cells[i].Attributes.Add("class", "Headermode");


                }

                for (int i = 0; i < Grid.Rows.Count; i++)
                {

                    Grid.Rows[i].BackColor = System.Drawing.Color.White;


                    if (i % 2 != 0)
                    {
                        Grid.Rows[i].Attributes.Add("class", "textmode");
                    }
                    else
                    {
                        Grid.Rows[i].Attributes.Add("class", "textmode2");
                    }

                }


                System.Web.UI.WebControls.TableCell cell1 = new System.Web.UI.WebControls.TableCell();
                cell1.Text = "<b> CENTRAL WAREHOUSING CORPORATION </b> ";
                cell1.ForeColor = System.Drawing.Color.Black;
                System.Web.UI.WebControls.TableRow tr1 = new System.Web.UI.WebControls.TableRow();
                tr1.Cells.Add(cell1);
                tr1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;

                System.Web.UI.WebControls.TableCell cell2 = new System.Web.UI.WebControls.TableCell();
                cell2.Text = "(A Govt. of India Undertaking)";
                cell2.ForeColor = System.Drawing.Color.Black;

                System.Web.UI.WebControls.TableRow tr2 = new System.Web.UI.WebControls.TableRow();
                tr2.Cells.Add(cell2);
                tr2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;

                System.Web.UI.WebControls.TableCell cell3 = new System.Web.UI.WebControls.TableCell();
                cell3.Text = "CMDA Trunk Terminal,No.8, GNT Road,Ponniammanmedu (PO), Madhavaram,Chennai-600110";
                cell3.ForeColor = System.Drawing.Color.Black;

                System.Web.UI.WebControls.TableRow tr3 = new System.Web.UI.WebControls.TableRow();
                tr3.Cells.Add(cell3);
                tr3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;


                System.Web.UI.WebControls.TableCell cell4 = new System.Web.UI.WebControls.TableCell();
                cell4.Text = "Physical Verification Report Export Cargo AsOnDate " + AsOnDate ;
                System.Web.UI.WebControls.TableRow tr4 = new System.Web.UI.WebControls.TableRow();
                cell4.ForeColor = System.Drawing.Color.Black;

                tr4.Cells.Add(cell4);
                tr4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;



                System.Web.UI.WebControls.TableCell cell5 = new System.Web.UI.WebControls.TableCell();
                cell5.Controls.Add(Grid);
                System.Web.UI.WebControls.TableRow tr5 = new System.Web.UI.WebControls.TableRow();
                tr5.Cells.Add(cell5);
                System.Web.UI.WebControls.TableCell cell6 = new System.Web.UI.WebControls.TableCell();
                cell5.Controls.Add(Grid);
                System.Web.UI.WebControls.TableRow tr6 = new System.Web.UI.WebControls.TableRow();
                tr5.Cells.Add(cell5);
                tb.Rows.Add(tr1);
                tb.Rows.Add(tr2);
                tb.Rows.Add(tr3);
                tb.Rows.Add(tr4);
                tb.Rows.Add(tr5);
                tb.Rows.Add(tr6);
            }
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                using (System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw))
                {
                    System.IO.StreamWriter writer = System.IO.File.AppendText(excelFile);
                    tb.RenderControl(hw);
                    writer.WriteLine(sw.ToString());
                    writer.Close();
                }
            }



    return excelFile;
}*/

        #endregion

        #region PV Report Of Import Loaded Container
        public void GetImpLoadedCont(Chn_PVReport objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PVReportImpLoadedCont", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Chn_PVReportImpLoadedCont> lstPV = new List<Chn_PVReportImpLoadedCont>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Chn_PVReportImpLoadedCont
                    {
                        CompanyLocation=Result["CompanyLocation"].ToString(),
                        CompanyBranch=Result["CompanyBranch"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        EximTraderAlias = Result["EximTraderAlias"].ToString(),
                        Days = Convert.ToInt32(Result["Days"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        TransportFrom = Result["TransportFrom"].ToString(),
                        Size = Result["Size"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPV;
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
        #endregion

        #region PV Report Of Empty Container
        public void GetEmptyCont(Chn_PVReport objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PVReportEmptyCont", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Chn_PVReportImpEmptyCont> lstPV = new List<Chn_PVReportImpEmptyCont>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Chn_PVReportImpEmptyCont
                    {
                        CompanyLocation=Result["CompanyLocation"].ToString(),
                        CompanyBranch=Result["CompanyBranch"].ToString(),
                        CFSCode = Result["EntryNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        EntryDateTime = Result["InDate"].ToString(),
                        Size = Result["Size"].ToString(),
                        EximTraderAlias = Result["SlaCd"].ToString(),
                        Days = Convert.ToInt32(Result["Days"]),
                        Amount = Convert.ToDecimal(Result["GRE"]),
                        InDateEcy = Result["InDateEcy"].ToString(),
                        OutDateEcy = Result["OutDate"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPV;
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
        #endregion

        #region Long Standing Report For Container
        public void GetLongStandingImpLoadedCont(Chn_LongStandingImpCon ObjContainerBalanceInCFS)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjContainerBalanceInCFS.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(ObjContainerBalanceInCFS.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_days", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjContainerBalanceInCFS.days });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("LongStandingImpLoadedCont", CommandType.StoredProcedure, DParam);
            IList<Chn_LongStandingImpConDtl> LstContainerBalanceInCFS = new List<Chn_LongStandingImpConDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstContainerBalanceInCFS.Add(new Chn_LongStandingImpConDtl
                    {
                        BOLNo = Result["OBL_NO"].ToString(),
                        IGMNo = Result["IGM_No"].ToString(),
                        IGMDate = Result["IGM_Date"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        ImporterName = Result["ImporterName"].ToString(),
                        ImporterAddress = Result["ImporterAddress"].ToString(),
                        InDate = Result["EntryDateTime"].ToString(),
                        CargoType = Result["CargoType"].ToString(),
                        SlaCd = Result["SlaCd"].ToString(),
                        NoOfPkg = Convert.ToInt32(Result["NO_PKG"].ToString()),
                        GrWt = Convert.ToDecimal(Result["GR_WT"].ToString()),
                        Commodity = Result["CommodityName"].ToString(),
                        Notice1 = Result["Notice1"].ToString(),
                        Notice2 = Result["Notice2"].ToString(),
                        Date1 = Result["Date1"].ToString(),
                        Date2 = Result["Date2"].ToString(),
                        Nocr = Result["Nocr"].ToString(),
                        Remarks1 = Result["Remarks1"].ToString(),
                        Remarks2 = Result["Remarks2"].ToString(),
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstContainerBalanceInCFS;
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
        #endregion

        #region Long Standing Report For Cargo
        public void GetLongStandingImpLoadedCargo(Chn_LongStandingImpCargo ObjContainerBalanceInCFS)
        {


            DateTime dtTo = DateTime.ParseExact(ObjContainerBalanceInCFS.AsOnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = ObjContainerBalanceInCFS.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_days", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjContainerBalanceInCFS.days });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("LongStandingImpLoadedCargo", CommandType.StoredProcedure, DParam);
            IList<Chn_LongStandingImpCargoDtl> LstContainerBalanceInCFS = new List<Chn_LongStandingImpCargoDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstContainerBalanceInCFS.Add(new Chn_LongStandingImpCargoDtl
                    {
                        BOLNo = Result["OBL_NO"].ToString(),
                        IGMNo = Result["IGM_No"].ToString(),
                        IGMDate = Result["IGM_Date"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        DstfDate = Result["DestuffingEntryDate"].ToString(),
                        ImporterName = Result["ImporterName"].ToString(),
                        ImporterAddress = Result["ImporterAddress"].ToString(),
                        InDate = Result["EntryDateTime"].ToString(),
                        CargoType = Result["CargoType"].ToString(),
                        SlaCd = Result["SlaCd"].ToString(),
                        NoOfPkg = Convert.ToInt32(Result["NO_PKG"].ToString()),
                        GrWt = Convert.ToDecimal(Result["GR_WT"].ToString()),
                        Area = Convert.ToDecimal(Result["Area"].ToString()),
                        Commodity = Result["CommodityName"].ToString(),
                        Notice1 = Result["Notice1"].ToString(),
                        Notice2 = Result["Notice2"].ToString(),
                        Date1 = Result["Date1"].ToString(),
                        Date2 = Result["Date2"].ToString(),
                        Nocr = Result["Nocr"].ToString(),
                        Remarks1 = Result["Remarks1"].ToString(),
                        Remarks2 = Result["Remarks2"].ToString(),
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstContainerBalanceInCFS;
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
        #endregion


        #region-- VC REPORT --
        public void VCCapacityDetails(string dtArray)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_Xml", MySqlDbType = MySqlDbType.Text, Value = dtArray });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //var Result1 = DataAccess.ExecuteDynamicSet<VCCapacityModel>("GetVCCapacityDetails", DParam);
            IDataReader Result = DataAccess.ExecuteDataReader("GetVCCapacityDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<ChnVCCapacityModel> LstVCCapacity = new List<ChnVCCapacityModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstVCCapacity.Add(new ChnVCCapacityModel
                    {
                        //vccapid = (Result["vccapid"] == DBNull.Value ? 0 : Convert.ToInt32(Result["vccapid"])),
                        //vcoptdate = (Result["vcoptdate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(Result["vcoptdate"])),
                        cfscap = (Result["cfscap"] == DBNull.Value ? 496034M : Convert.ToDecimal(Result["cfscap"])),
                        bndcap = (Result["bndcap"] == DBNull.Value ? 8766M : Convert.ToDecimal(Result["bndcap"])),
                        cfsutlz = (Result["cfsutlz"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["cfsutlz"])),
                        bndutlz = (Result["bndultz"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["bndultz"])),
                        //weekid = Result["weekid"].ToString()
                    });
                }
                if (LstVCCapacity.Count() < 3)
                {
                    for (var i = LstVCCapacity.Count; i < 3; i++)
                    {
                        LstVCCapacity.Add(new ChnVCCapacityModel());
                    }
                }
                if (Result.NextResult())
                {
                    var row = 0;
                    while (Result.Read())
                    {
                        LstVCCapacity[row].Occupency = (Result["occupency"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["occupency"]));
                        LstVCCapacity[row].Income = (Result["income"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["income"]));
                        row += 1;
                    }
                }
                if (Result.NextResult())
                {
                    var row = 0;
                    while (Result.Read())
                    {
                        LstVCCapacity[row].Import = (Result["Import"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Import"]));
                        LstVCCapacity[row].Export = (Result["Export"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Export"]));
                        LstVCCapacity[row].Empty = (Result["Empty"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Empty"]));
                        row += 1;
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstVCCapacity;
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

        public void AddVCDetails(DateTime date, decimal cfscap, decimal bndcap, decimal cfsutl, decimal bndutl)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_Date", MySqlDbType = MySqlDbType.DateTime, Value = date });
            LstParam.Add(new MySqlParameter { ParameterName = "In_CfsCap", MySqlDbType = MySqlDbType.Decimal, Value = cfscap });
            LstParam.Add(new MySqlParameter { ParameterName = "In_BndCap", MySqlDbType = MySqlDbType.Decimal, Value = bndcap });
            LstParam.Add(new MySqlParameter { ParameterName = "In_CfsUtl", MySqlDbType = MySqlDbType.Decimal, Value = cfsutl });
            LstParam.Add(new MySqlParameter { ParameterName = "In_BndUtl", MySqlDbType = MySqlDbType.Decimal, Value = bndutl });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            _DBResponse = new DatabaseResponse();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddVCDetails", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Bank Details Saved Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Bank And Branch Combination Already Exist";
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

        public void VCCapacityReport(string dtArray)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_Xml", MySqlDbType = MySqlDbType.Text, Value = dtArray });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //var Result1 = DataAccess.ExecuteDynamicSet<VCCapacityModel>("GetVCCapacityDetails", DParam);
            IDataReader Result = DataAccess.ExecuteDataReader("GetVCCapacityReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<ChnVCCapacityModel> LstVCCapacity = new List<ChnVCCapacityModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstVCCapacity.Add(new ChnVCCapacityModel
                    {
                        Occupency = (Result["occupency"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["occupency"])),
                        Income = (Result["income"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["income"]))
                    });
                }
                if (LstVCCapacity.Count() < 3)
                {
                    for (var i = LstVCCapacity.Count; i < 3; i++)
                    {
                        LstVCCapacity.Add(new ChnVCCapacityModel());
                    }
                }
                /*if (Result.NextResult())
                {
                    var row = 0;
                    while (Result.Read())
                    {
                        LstVCCapacity[row].Occupency = (Result["occupency"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["occupency"]));
                        LstVCCapacity[row].Income = (Result["income"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["income"]));
                        row += 1;
                    }
                }*/
                if (Result.NextResult())
                {
                    var row = 0;
                    while (Result.Read())
                    {
                        LstVCCapacity[row].Import = (Result["Import"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Import"]));
                        LstVCCapacity[row].Export = (Result["Export"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Export"]));
                        LstVCCapacity[row].Empty = (Result["Empty"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Empty"]));
                        row += 1;
                    }
                }
                if (Result.NextResult())
                {
                    var row = 0;
                    while (Result.Read())
                    {
                        LstVCCapacity[row].CurYear = (Result["CurYear"] == DBNull.Value ? 0 : Convert.ToInt32(Result["CurYear"]));
                        LstVCCapacity[row].PreYear = (Result["PreYear"] == DBNull.Value ? 0 : Convert.ToInt32(Result["PreYear"]));
                        row += 1;
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstVCCapacity;
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

        #region Gate Pass
        public void GatePassReport(ChnGatePassReport ObjGatePassReport, int UserId)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjGatePassReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjGatePassReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = UserId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GatePassReport", CommandType.StoredProcedure, DParam);
            IList<ChnGatePassReport> LstGatePassReport = new List<ChnGatePassReport>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstGatePassReport.Add(new ChnGatePassReport
                    {



                        GatePassNo = Result["GatePassNo"].ToString(),
                        GatePassDate = Result["GatePassDate"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),


                        ContainerSize = Result["Size"].ToString(),
                        VesselName = Result["Vessel"].ToString(),
                        VoyageNo = Result["Voyage"].ToString(),
                        RotationNo = Result["Rotation"].ToString(),
                        LineNo = Result["LineNo"].ToString(),
                        ShippingSealLineNo = Result["SealNo"].ToString(),
                        CustomSealLineNo = Result["CustomSealNo"].ToString(),

                        ImporterExporter = Result["ImpExpName"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        ShippingLine = Result["ShippingLineName"].ToString(),
                        Weight = Result["Weight"].ToString(),
                        LocationName = Result["Location"].ToString(),
                        NatureOfGoods = Result["Nature of goods"].ToString(),

                        BOENoOrSBNoOrWRNo = Result["BOENo"].ToString(),
                        Date = Result["BOEDate"].ToString(),
                        NoOfPackages = Result["NoOfPackages"].ToString(),

                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstGatePassReport;
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

        #endregion

        public void DailyCashBook(Chn_DailyCashBookDtl ObjDailyCashBook)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyCashBook.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyCashBook.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyCashBookReportCash", CommandType.StoredProcedure, DParam);
            IList<Chn_DailyCashBookDtl> LstDailyCashBook = new List<Chn_DailyCashBookDtl>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //ObjStatusDtl = new ConsumerList();
                    //ObjStatusDtl.RegistrationNo = Convert.ToString(Result["RegistrationNo"]);
                    //ObjStatusDtl.Name = Convert.ToString(Result["CompanyName"]);
                    //ObjStatusDtl.Address = Convert.ToString(Result["CompanyAddress"]);
                    //ObjStatusDtl.IssueDate = Convert.ToString(Result["IssuedOn"]);


                    //                 InvoiceDate DATE,
                    //         InvoiceNumber       VARCHAR(30),
                    //ReceiptAmount DECIMAL(18, 3),
                    //InvoiceAmount DECIMAL(18, 3),
                    //Value DECIMAL(18, 3),
                    //OpeningBalance DECIMAL(18, 3),
                    //ClosingBalance DECIMAL(18, 3)

                    LstDailyCashBook.Add(new Chn_DailyCashBookDtl
                    {

                        //ReceiptNo, ReceiptDate, Party, ChequeNo, CashReceiptId, GenSpace, sto, Insurance, GroundRentEmpty, GroundRentLoaded, Mf, EntCharge, 
                        //Fum, OtCharge, CGSTAmt, SGSTAmt, IGSTAmt, MISC, MiscExcess, TotalCash, TotalCheque, tdsCol, crTDS
                        CRNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),
                        CRId=Convert.ToInt32(Result["CashReceiptId"]),
                        Depositor = Result["Party"].ToString(),
                        ChqNo = Result["ChequeNo"].ToString(),
                        GenSpace = Result["GenSpace"].ToString(),
                        StorageCharge = Result["sto"].ToString(),
                        Insurance = Result["Insurance"].ToString(),
                        GroundRent = Result["GroundRent"].ToString(),
                        RFIDCharge = Result["RFID"].ToString(),
                        WeighmentCharge = Result["Wgmnt"].ToString(),
                        FacilitationCharge = Result["FcChrg"].ToString(),
                        DocumentCharge = Result["DocChrg"].ToString(),
                        AggregationCharge = Result["AggChrg"].ToString(),
                        HTCharge = Result["HT"].ToString(),
                        OtherCharge = Result["OtCharge"].ToString(),
                        Cgst = Result["CGSTAmt"].ToString(),
                        Sgst = Result["SGSTAmt"].ToString(),
                        Igst = Result["IGSTAmt"].ToString(),
                        Misc = Result["MISC"].ToString(),
                        //MiscExcess = Result["MiscExcess"].ToString(),
                        TotalCash = Result["TotalCash"].ToString(),
                        TotalCheque = Result["TotalCheque"].ToString(),
                        Tds = Result["tdsCol"].ToString(),
                        CrTds = Result["crTDS"].ToString()
                        //TDSPlus = Result["TDSPlus"].ToString(),
                        //Exempted = Result["Exempted"].ToString(),
                        //PdaPLus = Result["PdaPLus"].ToString(),
                        //TDSMinus = Result["TDSMinus"].ToString(),
                        //PdaMinus = Result["PdaMinus"].ToString(),
                        //HtAdjust = Result["HtAdjust"].ToString(),
                        //RoundOff = Result["RoundUp"].ToString(),
                        //RowTotal = Result["Total"].ToString()


                        //Party = Result["Party"].ToString(),
                        //Deposit = Result["Deposit"].ToString(),
                        //Withdraw = Result["Withdraw"].ToString()

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDailyCashBook;
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
       
        public void MonthlyCashBook(Chn_DailyCashBookDtl ObjDailyCashBook)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyCashBook.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyCashBook.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("MonthlyCashBookReport", CommandType.StoredProcedure, DParam);
            IList<Chn_DailyCashBookDtl> LstMonthlyCashBook = new List<Chn_DailyCashBookDtl>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //ObjStatusDtl = new ConsumerList();
                    //ObjStatusDtl.RegistrationNo = Convert.ToString(Result["RegistrationNo"]);
                    //ObjStatusDtl.Name = Convert.ToString(Result["CompanyName"]);
                    //ObjStatusDtl.Address = Convert.ToString(Result["CompanyAddress"]);
                    //ObjStatusDtl.IssueDate = Convert.ToString(Result["IssuedOn"]);


                    //                 InvoiceDate DATE,
                    //         InvoiceNumber       VARCHAR(30),
                    //ReceiptAmount DECIMAL(18, 3),
                    //InvoiceAmount DECIMAL(18, 3),
                    //Value DECIMAL(18, 3),
                    //OpeningBalance DECIMAL(18, 3),
                    //ClosingBalance DECIMAL(18, 3)

                    LstMonthlyCashBook.Add(new Chn_DailyCashBookDtl
                    {
                        //CRNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),

                        //Depositor = Result["Party"].ToString(),
                        //ChqNo = Result["ChequeNo"].ToString(),
                        GenSpace = Result["GenSpace"].ToString(),
                        StorageCharge = Result["sto"].ToString(),
                        Insurance = Result["Insurance"].ToString(),
                        GroundRent = Result["GroundRent"].ToString(),
                        RFIDCharge = Result["RFID"].ToString(),
                        WeighmentCharge = Result["Weighment"].ToString(),
                        FacilitationCharge = Result["Facilitation"].ToString(),
                        DocumentCharge = Result["DocChrg"].ToString(),
                        AggregationCharge = Result["AggChrg"].ToString(),
                        HTCharge = Result["HT"].ToString(),
                        OtherCharge = Result["OtCharge"].ToString(),
                        Cgst = Result["CGSTAmt"].ToString(),
                        Sgst = Result["SGSTAmt"].ToString(),
                        Igst = Result["IGSTAmt"].ToString(),
                        Misc = Result["MISC"].ToString(),
                        //MiscExcess = Result["MiscExcess"].ToString(),
                        TotalCash = Result["TotalCash"].ToString(),
                        TotalCheque = Result["TotalCheque"].ToString(),
                        Tds = Result["tdsCol"].ToString(),
                        CrTds = Result["crTDS"].ToString()

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstMonthlyCashBook;
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

        #region Time Barred Report

        public void GetTimeBarredReport(string AsOnDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy-MM-dd") });
           // LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objPV.Module });
           // LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objPV.GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("TimeBarredReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Chn_TimeBarredReport> lstPV = new List<Chn_TimeBarredReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Chn_TimeBarredReport
                    {
                        BondNo = Result["BondNo"].ToString(),
                        BondDate = Result["BondDate"].ToString(),
                        Importer = Result["Importer"].ToString(),
                        ItemDesc = Result["ItemDesc"].ToString(),
                        Units = Convert.ToInt32(Result["Unit"]),
                        Value = Convert.ToDecimal(Result["Value"]),
                        Duty = Convert.ToDecimal(Result["Duty"]),
                        Area = Convert.ToDecimal(Result["Area"]),
                        CHA = Result["CHA"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPV;
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
        #endregion

        #region Live Bond Report
        public void GetLiveBondReport(string AsOnDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy-MM-dd") });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objPV.Module });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objPV.GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("LiveBondReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Chn_TimeBarredReport> lstPV = new List<Chn_TimeBarredReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Chn_TimeBarredReport
                    {
                        BondNo = Result["BondNo"].ToString(),
                        BondDate = Result["BondDate"].ToString(),
                        Importer = Result["Importer"].ToString(),
                        ItemDesc = Result["ItemDesc"].ToString(),
                        Units = Convert.ToInt32(Result["Unit"]),
                        Value = Convert.ToDecimal(Result["Value"]),
                        Duty = Convert.ToDecimal(Result["Duty"]),
                        Area = Convert.ToDecimal(Result["Area"]),
                        CHA = Result["CHA"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPV;
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
        #endregion
        public void AgeWiseBreakUp(Chn_AgewiseBreakUp ObjDailyCashBook)
        {


            DateTime dtfrom = DateTime.ParseExact(ObjDailyCashBook.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyCashBook.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAgewisebreaup", CommandType.StoredProcedure, DParam);
            IList<Chn_AgewiseBreakUp> LstAgeWiseBreakUp = new List<Chn_AgewiseBreakUp>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstAgeWiseBreakUp.Add(new Chn_AgewiseBreakUp
                    {

                        //ReceiptNo, ReceiptDate, Party, ChequeNo, CashReceiptId, GenSpace, sto, Insurance, GroundRentEmpty, GroundRentLoaded, Mf, EntCharge, 
                        //Fum, OtCharge, CGSTAmt, SGSTAmt, IGSTAmt, MISC, MiscExcess, TotalCash, TotalCheque, tdsCol, crTDS
                        opnNOOfBound = Convert.ToInt32(Result["opnNOOfBound"]),
                        opnDuty = Convert.ToDecimal(Result["opnDuty"]),// == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),

                        ReciptNOOfBound = Convert.ToInt32(Result["ReciptNOOfBound"]),
                        ReciptDuty = Convert.ToDecimal(Result["ReciptDuty"]),
                        DipNOOfBound = Convert.ToInt32(Result["DipNOOfBound"]),
                        DipDuty = Convert.ToDecimal(Result["DipDuty"]),
                        NoFBond = Convert.ToInt32(Result["NoFBond"]),
                        DutyAmount = Convert.ToDecimal(Result["DutyAmount"]),


                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstAgeWiseBreakUp;
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

        #region Monthly Time Barred Report

        public void GetMonthlyTimeBarred(int Month,int Year)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.Int32, Value = Month });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Year", MySqlDbType = MySqlDbType.Int32, Value = Year });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("MonthlyTimeBarred", CommandType.StoredProcedure, dpram);
            Chn_MonthlyTimeBarredReport objTR = new Chn_MonthlyTimeBarredReport();
            //IList<UptoLastMonthTimeBarred> LstPrevMnth = new List<UptoLastMonthTimeBarred>();
           // IList<CurrMonthTimeBarred> LstCurrMnth = new List<CurrMonthTimeBarred>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objTR.NoOfStatrtTbb = Convert.ToInt32(result["NoOfStrtTbb"]);
                    objTR.NoOfAddedTbb = Convert.ToInt32(result["NoOfAddedTbb"]);
                    objTR.StartUnlddate = result["StartUnloadDt"].ToString();
                    objTR.Area=Convert.ToDecimal(result["TotalArea"]);
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objTR.LstPrevMnth.Add(new UptoLastMonthTimeBarred
                        {
                            LstMnthBondNo = result["BondNo"].ToString(),
                            LstMnthStAmt = Convert.ToDecimal(result["StAmt"])
                        });
                    }
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objTR.LstCurrMnth.Add(new CurrMonthTimeBarred
                        {
                           CurrMnthBondNo=result["BondNo"].ToString(),
                           CurrMnthStAmt= Convert.ToDecimal(result["StAmt"])

                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objTR;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }

        #endregion


        #region ECONOMY REPORT
        public void GetEconomyReport(int month, int year)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "repoYear", MySqlDbType = MySqlDbType.VarChar, Value = year });
            LstParam.Add(new MySqlParameter { ParameterName = "repoMonth", MySqlDbType = MySqlDbType.Int32, Value = month });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMonthlyEconomyRpt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ChnEconomyReport> LstData = new List<ChnEconomyReport>();
            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    LstData.Add(new ChnEconomyReport
                    {
                        IncomeExpHeadId = Convert.ToInt32(Result["IncomeExpHeadId"]),
                        ItemType = Result["ItemType"].ToString(),
                        ItemHead = Result["ItemHead"].ToString(),
                        ItemLabel = Result["ItemLabel"].ToString(),
                        Amount = Result["Amount"].ToString(),
                        CumAmount = Result["CumAmount"].ToString(),
                        ProCumAmount = Result["ProCumAmount"].ToString(),
                        IsTextBox = Convert.ToInt32(Result["IsTextBox"])

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstData;
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

        public void SaveEconomyReport(int Month, int Year, string xmlMonthlyEconomyReport, int Uid)
        {
            string GenId = "0";
            string RetObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "repoYear", MySqlDbType = MySqlDbType.Int32, Value = Year });
            LstParam.Add(new MySqlParameter { ParameterName = "repoMonth", MySqlDbType = MySqlDbType.Int32, Value = Month });
            LstParam.Add(new MySqlParameter { ParameterName = "uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "xmlMonthlyEconomyReport", MySqlDbType = MySqlDbType.LongText, Value = xmlMonthlyEconomyReport });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GenId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.Text, Direction = ParameterDirection.Output, Value = RetObj });
            _DBResponse = new DatabaseResponse();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("RptMonthlyEconomySave", CommandType.StoredProcedure, DParam, out GenId, out RetObj);
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = RetObj;
                    _DBResponse.Data = RetObj;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = RetObj;
                    _DBResponse.Data = RetObj;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = RetObj;
                    _DBResponse.Data = RetObj;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }


        public void GetEconomyReportForPrint(int month, int year)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "repoYear", MySqlDbType = MySqlDbType.VarChar, Value = year });
            LstParam.Add(new MySqlParameter { ParameterName = "repoMonth", MySqlDbType = MySqlDbType.Int32, Value = month });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMonthlyEconomyRptForPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            ChnEconomyRptPrint PrintData = new ChnEconomyRptPrint();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    PrintData.FormDate = Result["FormDate"].ToString();
                    PrintData.ToDate = Result["ToDate"].ToString();
                    PrintData.SqmCovered = Result["SqmCovered"].ToString();
                    PrintData.BagCovered = Result["BagCovered"].ToString();
                    PrintData.SqmOpen = Result["SqmOpen"].ToString();
                    PrintData.BagOpen = Result["BagOpen"].ToString();
                    PrintData.CreatedOn = Result["CreatedOn"].ToString();
                    PrintData.IsFound = Convert.ToInt32(Result["IsFound"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        PrintData.RptDetails.Add(new ChnEconomyReport
                        {
                            Sr = Convert.ToInt32(Result["Sr"]),
                            IncomeExpHeadId = Convert.ToInt32(Result["IncomeExpHeadId"]),
                            ItemType = Result["ItemType"].ToString(),
                            ItemHead = Result["ItemHead"].ToString(),
                            ItemLabel = Result["ItemLabel"].ToString(),
                            Amount = Result["Amount"].ToString(),
                            CumAmount = Result["CumAmount"].ToString(),
                            ProCumAmount = Result["ProCumAmount"].ToString(),
                            ItemCodeNo = Result["ItemCodeNo"].ToString(),
                            ItemSortOrder = Convert.ToInt32(Result["ItemSortOrder"]),
                            PageNo = Convert.ToInt32(Result["PageNo"]),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        PrintData.RptSummary.Add(new ChnEconomyRptIncomeSummary
                        {
                            Sr = Convert.ToInt32(Result["Sr"]),
                            IncomeExpHeadId = Convert.ToInt32(Result["IncomeExpHeadId"]),
                            ItemLabel = Result["ItemLabel"].ToString(),
                            Amount = Result["Amount"].ToString(),
                            CumAmount = Result["CumAmount"].ToString(),
                            ProCumAmount = Result["ProCumAmount"].ToString(),
                            CodeNo = Result["CodeNo"].ToString(),

                        });
                    }
                }

                if (PrintData.IsFound == 0)
                {
                    Status = 0;
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = PrintData;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data saved for this month and year";
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

        #region Daily Transaction Report
        public void DTRForPrint(string DTRDate, int GodownId = 0)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DTRDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("DailyTransactionReport", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result != null && Result.Tables[0].Rows.Count > 0)
                {
                    Status = 1;
                }
                else if (Result != null && Result.Tables[1].Rows.Count > 0)
                {
                    Status = 1;
                }
                else
                {
                    Status = 0;
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
        public void DTRForExport(string DTRDate, int GodownId = 0)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DTRDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DTRDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            _DBResponse = new DatabaseResponse();
            try
            {
                DParam = LstParam.ToArray();
                //Result = DataAccess.ExecuteDataSet("DailyTransactionExp", CommandType.StoredProcedure, DParam);
                CHN_DTREXP obj = new CHN_DTREXP();
                obj = (CHN_DTREXP)DataAccess.ExecuteDynamicSet<CHN_DTREXP>("DailyTransactionExp", DParam);
                if (obj.lstBTTDetails.Count > 0 || obj.lstCargoAccepting.Count > 0 || obj.lstCargoShifting.Count > 0 || obj.lstCartingDetails.Count > 0 ||
                    obj.lstDTRStuffing.Count > 0||obj.lstDRTSBDetails.Count>0 || obj.StockOpening.Count > 0 || obj.StockClosing.Count > 0 || obj.lstShortCartingDetails.Count > 0)
                    _DBResponse.Status = 1;
                else
                    _DBResponse.Status = 0;
                _DBResponse.Message = "Data";
                _DBResponse.Data = obj;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        public void DTRForImportFCL(string DTRDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "As_On", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DTRDate) });
            _DBResponse = new DatabaseResponse();
            try
            {
                DParam = LstParam.ToArray();
                DataSet Result = DataAccess.ExecuteDataSet("GetImpDTRFCL", CommandType.StoredProcedure, DParam);
                if (Result.Tables[0].Rows.Count > 0)
                    _DBResponse.Status = 1;
                else
                    _DBResponse.Status = 0;
                _DBResponse.Message = "Data";  
                _DBResponse.Data = Result;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        #endregion

        #region Insurance Register Report
        public void GetInsuranceRegister(string Module,DateTime date1, DateTime date2)
        {
            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = date2 });
           
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
           
            DataSet ds = DataAccess.ExecuteDataSet("GetInsuranceRegisterExcel", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];


            List<CHN_InsuranceRegister> model = new List<CHN_InsuranceRegister>();
            try
            {


                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = GetInsuranceRegisterExcel(model, dt, date1.ToString("dd/MM/yyyy"), date2.ToString("dd/MM/yyyy"));
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                ds.Dispose();
            }
        }


        private string GetInsuranceRegisterExcel(List<CHN_InsuranceRegister> model, DataTable dt, string datevalue, string datevalueto)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            System.Web.UI.WebControls.GridView Grid = new System.Web.UI.WebControls.GridView();


            System.Web.UI.WebControls.Table tb = new System.Web.UI.WebControls.Table();

            if (dt.Rows.Count > 0)
            {

                Grid.AllowPaging = false;
                Grid.DataSource = dt;
                Grid.DataBind();


                for (int i = 0; i < Grid.HeaderRow.Cells.Count; i++)
                {
                    Grid.HeaderRow.Cells[i].Style.Add("background-color", "#edebeb");
                    Grid.HeaderRow.Cells[i].Attributes.Add("class", "Headermode");


                }

                for (int i = 0; i < Grid.Rows.Count; i++)
                {

                    Grid.Rows[i].BackColor = System.Drawing.Color.White;


                    if (i % 2 != 0)
                    {
                        Grid.Rows[i].Attributes.Add("class", "textmode");
                    }
                    else
                    {
                        Grid.Rows[i].Attributes.Add("class", "textmode2");
                    }

                }
                

                System.Web.UI.WebControls.TableCell cell1 = new System.Web.UI.WebControls.TableCell();
                cell1.Text = "<b> CENTRAL WAREHOUSING CORPORATION </b> ";
                cell1.ForeColor = System.Drawing.Color.Black;
                System.Web.UI.WebControls.TableRow tr1 = new System.Web.UI.WebControls.TableRow();
                tr1.Cells.Add(cell1);
                tr1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;

                System.Web.UI.WebControls.TableCell cell2 = new System.Web.UI.WebControls.TableCell();
                cell2.Text = "(A Govt. of India Undertaking)";
                cell2.ForeColor = System.Drawing.Color.Black;

                System.Web.UI.WebControls.TableRow tr2 = new System.Web.UI.WebControls.TableRow();
                tr2.Cells.Add(cell2);
                tr2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;

                System.Web.UI.WebControls.TableCell cell3 = new System.Web.UI.WebControls.TableCell();
                cell3.Text = "CMDA Trunk Terminal,No.8, GNT Road,Ponniammanmedu (PO), Madhavaram,Chennai-600110";
                cell3.ForeColor = System.Drawing.Color.Black;

                System.Web.UI.WebControls.TableRow tr3 = new System.Web.UI.WebControls.TableRow();
                tr3.Cells.Add(cell3);
                tr3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;


                System.Web.UI.WebControls.TableCell cell4 = new System.Web.UI.WebControls.TableCell();
                cell4.Text = "Insurance Register Report From " + datevalue + " TO " + datevalueto;
                System.Web.UI.WebControls.TableRow tr4 = new System.Web.UI.WebControls.TableRow();
                cell4.ForeColor = System.Drawing.Color.Black;

                tr4.Cells.Add(cell4);
                tr4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;



                System.Web.UI.WebControls.TableCell cell5 = new System.Web.UI.WebControls.TableCell();
                cell5.Controls.Add(Grid);
                System.Web.UI.WebControls.TableRow tr5 = new System.Web.UI.WebControls.TableRow();
                tr5.Cells.Add(cell5);

                tb.Rows.Add(tr1);
                tb.Rows.Add(tr2);
                tb.Rows.Add(tr3);
                tb.Rows.Add(tr4);
                tb.Rows.Add(tr5);

            }
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                using (System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw))
                {
                    System.IO.StreamWriter writer = System.IO.File.AppendText(excelFile);
                    tb.RenderControl(hw);
                    writer.WriteLine(sw.ToString());
                    writer.Close();
                }
            }



            return excelFile;
        }

        public void GetInsuranceRegisterPDF(string Module, DateTime date1, DateTime date2)
        {
            int Status = 0;
            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = date2 });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            DataSet ds = DataAccess.ExecuteDataSet("GetInsuranceRegister", CommandType.StoredProcedure, DParam);
            //DataTable dt = ds.Tables[0];

            List<CHN_InsuranceRegister> model = new List<CHN_InsuranceRegister>();
            try
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    Status = 1;
                }
                else
                {
                    Status = 0;
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ds;
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
                ds.Dispose();
            }
        }

        #endregion
        #region Carting Order Register
        public void CartingOrderRegister(CHN_CartingOrderRegister ObjCartingOrderRegister)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjCartingOrderRegister.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjCartingOrderRegister.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("CartingOrderRegister", CommandType.StoredProcedure, DParam);
            IList<CHN_CartingOrderRegister> LstCartingOrderRegister = new List<CHN_CartingOrderRegister>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                int i = 1;
                while (Result.Read())
                {
                    Status = 1;
                    //ObjStatusDtl = new ConsumerList();
                    //ObjStatusDtl.RegistrationNo = Convert.ToString(Result["RegistrationNo"]);
                    //ObjStatusDtl.Name = Convert.ToString(Result["CompanyName"]);
                    //ObjStatusDtl.Address = Convert.ToString(Result["CompanyAddress"]);
                    //ObjStatusDtl.IssueDate = Convert.ToString(Result["IssuedOn"]);


                    //                 InvoiceDate DATE,
                    //         InvoiceNumber       VARCHAR(30),
                    //ReceiptAmount DECIMAL(18, 3),
                    //InvoiceAmount DECIMAL(18, 3),
                    //Value DECIMAL(18, 3),
                    //OpeningBalance DECIMAL(18, 3),
                    //ClosingBalance DECIMAL(18, 3)

                    LstCartingOrderRegister.Add(new CHN_CartingOrderRegister
                    {


                        SlNo = i,
                        Date = Result["Date"].ToString(),

                        ExporterName = Result["ExporterName"].ToString(),

                        ChaName = Result["ChaName"].ToString(),
                        Cargo = Result["Cargo"].ToString(),

                        NoOfUnits = Result["NoOfUnits"].ToString(),
                        Weight = Result["Weight"].ToString(),
                        FobValue = Result["FobValue"].ToString(),
                        Destination = Result["Destination"].ToString(),
                        vessel = Result["vessel"].ToString(),
                        ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"]).ToString(),
                         ShippingBillDate = (Result["ShippingBillDate"] == null ? "" : Result["ShippingBillDate"]).ToString(),
                          GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString(),
                          RegisterNo= (Result["RegisterNo"] == null ? "" : Result["RegisterNo"]).ToString(),
                          Location= (Result["Location"] == null ? "" : Result["Location"]).ToString(),











                        //value = Result["value"].ToString()

                    });
                    i += 1;
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCartingOrderRegister;
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
        #endregion
        #region Register of Outward Supply
        public void GetRegisterofOutwardSupply(DateTime date1, DateTime date2, string Type)
        {
            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.DateTime, Value = date2 });
            LstParam.Add(new MySqlParameter { ParameterName = "In_Type", MySqlDbType = MySqlDbType.VarChar, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //var objRegOutwardSupply = (List<RegisterOfOutwardSupplyModel>)DataAccess.ExecuteDynamicSet<List<RegisterOfOutwardSupplyModel>>("GetRegisterofOutwardSupply", DParam);
            DataSet ds = DataAccess.ExecuteDataSet("GetRegisterofOutwardSupply", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            if (Type == "Inv" || Type == "Unpaid" || Type == "CancelInv")
            {
                List<CHN_RegisterOfOutwardSupplyModel> model = new List<CHN_RegisterOfOutwardSupplyModel>();
                try
                {
                    if (ds.Tables.Count > 0)
                    {
                        model = (from DataRow dr in dt.Rows
                                 select new CHN_RegisterOfOutwardSupplyModel()
                                 {
                                     SlNo = Convert.ToInt32(dr["SlNo"]),
                                     GST = dr["GST"].ToString(),
                                     Place = dr["Place"].ToString(),
                                     Name = dr["Name"].ToString(),
                                     Period = dr["Period"].ToString(),

                                     Nature = dr["Nature"].ToString(),
                                     Rate = Convert.ToDecimal(dr["Rate"]),
                                     InvoiceNo = dr["InvoiceNo"].ToString(),
                                     InvoiceDate = dr["InvoiceDate"].ToString(),
                                     ServiceValue = Convert.ToDecimal(dr["ServiceValue"]),

                                     ITaxPercent = dr["ITaxPercent"].ToString(),
                                     ITaxAmount = Convert.ToDecimal(dr["ITaxAmount"]),
                                     CTaxPercent = dr["CTaxPercent"].ToString(),
                                     CTaxAmount = Convert.ToDecimal(dr["CTaxAmount"]),
                                     STaxPercent = dr["STaxPercent"].ToString(),
                                     STaxAmount = Convert.ToDecimal(dr["STaxAmount"]),

                                     Total = Convert.ToDecimal(dr["Total"]),
                                     //WH = dr["WH"].ToString(),
                                     //CRNoDate = dr["CRNoDate"].ToString(),
                                     //SD = Convert.ToDecimal(dr["SD"]),
                                     //Amount = Convert.ToDecimal(dr["Amount"]),
                                     //TDS = Convert.ToDecimal(dr["TDS"]),
                                     //ChequeNoDate = dr["CHDD"].ToString(),
                                     PaymentMode = dr["PaymentMode"].ToString(),
                                     Remarks = dr["Remarks"].ToString()
                                     ,
                                     HSNCode = dr["HSNCode"].ToString()
                                 }).ToList();
                    }
                    decimal InvoiceAmount = 0, CRAmount = 0;
                    //foreach (DataRow dr in ds.Tables[1].Rows)
                    //{
                    //    InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]);
                    //    CRAmount = Convert.ToDecimal(dr["CRAmount"]);
                    //}
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = RegisterofOutwardSupplyExcel(model, InvoiceAmount, CRAmount);
                }
                catch (Exception ex)
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
                finally
                {
                    ds.Dispose();
                }
            }
            else
            {
                List<CHN_RegisterOfOutwardSupplyModelCreditDebit> modelCreditDebit = new List<CHN_RegisterOfOutwardSupplyModelCreditDebit>();
                try
                {
                    if (ds.Tables.Count > 0)
                    {
                        modelCreditDebit = (from DataRow dr in dt.Rows
                                            select new CHN_RegisterOfOutwardSupplyModelCreditDebit()
                                            {
                                                SlNo = Convert.ToInt32(dr["SlNo"]),
                                                GST = dr["GST"].ToString(),
                                                Place = dr["Place"].ToString(),
                                                Name = dr["Name"].ToString(),
                                                Period = dr["Period"].ToString(),

                                                Nature = dr["Nature"].ToString(),
                                                HSNCode = dr["HSNCode"].ToString(),
                                                Rate = Convert.ToDecimal(dr["Rate"]),
                                                CreditNoteNo = dr["CreditNoteNo"].ToString(),
                                                CRNoteDate = dr["CRNoteDate"].ToString(),
                                                InvoiceNo = dr["InvoiceNo"].ToString(),
                                                InvoiceDate = dr["InvoiceDate"].ToString(),
                                                ServiceValue = Convert.ToDecimal(dr["ServiceValue"]),

                                                ITaxPercent = dr["ITaxPercent"].ToString(),
                                                ITaxAmount = Convert.ToDecimal(dr["ITaxAmount"]),
                                                CTaxPercent = dr["CTaxPercent"].ToString(),
                                                CTaxAmount = Convert.ToDecimal(dr["CTaxAmount"]),
                                                STaxPercent = dr["STaxPercent"].ToString(),
                                                STaxAmount = Convert.ToDecimal(dr["STaxAmount"]),

                                                Total = Convert.ToDecimal(dr["Total"]),
                                                Remarks = dr["Remarks"].ToString()

                                            }).ToList();
                    }
                    decimal InvoiceAmount = 0, CRAmount = 0;
                    //foreach (DataRow dr in ds.Tables[1].Rows)
                    //{
                    //    InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]);
                    //    CRAmount = Convert.ToDecimal(dr["CRAmount"]);
                    //}
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = RegisterofOutwardSupplyExcelCreditDebit(modelCreditDebit, InvoiceAmount, CRAmount);
                }
                catch (Exception ex)
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
                finally
                {
                    ds.Dispose();
                }
            }


        }
        private string RegisterofOutwardSupplyExcel(List<CHN_RegisterOfOutwardSupplyModel> model, decimal InvoiceAmount, decimal CRAmount)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "Principal Place of Business"
                        + Environment.NewLine + "CENTRAL WAREHOUSE"
                        + Environment.NewLine + Environment.NewLine
                        + "REGISTER OF OUTWARD SUPPLY (TAX INVOICE/BILL OF SUPPLY)";
                exl.MargeCell("A1:M1", title, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N1:O1", "BILL REGISTER", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A2:A4", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B2:B4", "GSTIN", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C2:C4", "Place" + Environment.NewLine + "(Name of State)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D2:D4", "Name of Customer to whom Service rendered", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E2:E4", "Period of Invoice", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F2:F4", "Nature of Invoice" + Environment.NewLine + "(Resv./Initial Fumigatiom/General Basic/Over & Above)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G2:G4", "HSN Code", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H2:H4", "Rate per" + Environment.NewLine + "Bag/MT/Sqm", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I2:K2", "Invoice Details", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I3:I4", "Invoice No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J3:J4", "Date of Invoice", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K3:K4", "Value of Service" + Environment.NewLine + "(Before Tax)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L2:Q2", "Rate of Tax", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L3:M3", "IGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N3:O3", "CGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("P3:Q3", "SGST", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("L4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("M4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("N4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("O4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("P4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("Q4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("R2:R4", "Total Invoice Value" + Environment.NewLine + "(18=(11+13 or 11+15+17))", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("R2:U2", "Perticulars of Payment Received", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("R3:R4", "Received" + Environment.NewLine + "At WH/RO", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("S3:S4", "C.R No. & Date", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("T3:T4", "Cheque/DD No. & Date", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("T3:T4", "SD", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("U3:U4", "Amount", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("V3:V4", "TDS", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("V2:V4", "Amount Received Against Bill (Rs.)", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("W2:W4", "Adjustment/Deduction", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("W2:W2", "Balance", DynamicExcel.CellAlignment.Middle);
                //exl.AddCell("W2", "Balance", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("S2:S4", "PaymentMode", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("T2:T4", "Remarks", DynamicExcel.CellAlignment.Middle);


                for (var i = 65; i < 85; i++)
                {
                    char character = (char)i;
                    string text = character.ToString();
                    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                }
                /*exl.AddTable<InvoiceData>("A", 6, model.InvoiceData, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16 });
                exl.AddTable<CashReceiptData>("R", 6, model.CashReceiptData, new[] { 12, 30, 14, 14, 14, 14, 14, 40 });/
                exl.AddTable<PpgRegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16, 12, 30, 14, 14, 14, 14, 14, 40 });*/
                exl.AddTable<CHN_RegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 20, 20, 20, 12, 20, 10, 15, 20, 12, 12, 8, 14, 8, 14, 8, 14, 16, 10, 30 });
                var igstamt = model.Sum(o => o.ITaxAmount);
                var sgstamt = model.Sum(o => o.STaxAmount);
                var cgstamt = model.Sum(o => o.CTaxAmount);
                var totalamt = model.Sum(o => o.Total);
                exl.AddCell("M" + (model.Count + 6).ToString(), igstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 6).ToString(), cgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("Q" + (model.Count + 6).ToString(), sgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("R" + (model.Count + 6).ToString(), totalamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                /*exl.AddCell("O" + (model.Count + 7).ToString(), "Invoice Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 7).ToString(), InvoiceAmount, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 8).ToString(), "Cash Receipt Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 8).ToString(), CRAmount, DynamicExcel.CellAlignment.CenterRight);*/

                exl.Save();
            }
            return excelFile;
        }

        private string RegisterofOutwardSupplyExcelCreditDebit(List<CHN_RegisterOfOutwardSupplyModelCreditDebit> modelCreditDebit, decimal InvoiceAmount, decimal CRAmount)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "Principal Place of Business"
                        + Environment.NewLine + "CENTRAL WAREHOUSE"
                        + Environment.NewLine + Environment.NewLine
                        + "REGISTER OF OUTWARD SUPPLY (TAX INVOICE/BILL OF SUPPLY)";
                exl.MargeCell("A1:M1", title, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N1:O1", "BILL REGISTER", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A2:A4", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B2:B4", "GSTIN", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C2:C4", "Place" + Environment.NewLine + "(Name of State)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D2:D4", "Name of Customer to whom Service rendered", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E2:E4", "Period of Invoice", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F2:F4", "Nature of Invoice" + Environment.NewLine + "(Resv./Initial Fumigatiom/General Basic/Over & Above)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G2:G4", "HSN Code", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("H2:H4", "Rate per" + Environment.NewLine + "Bag/MT/Sqm", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I2:I4", "Credit / Debit Note No", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J2:J4", "Credit / Debit Note Date", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K2:M2", "Invoice Details", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K3:K4", "Invoice No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L3:L4", "Date of Invoice", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M3:M4", "Value of Service" + Environment.NewLine + "(Before Tax)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N2:S2", "Rate of Tax", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N3:O3", "IGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("P3:Q3", "CGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("R3:S3", "SGST", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("N4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("O4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("P4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("Q4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("R4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("S4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("T2:T4", "Total Invoice Value" + Environment.NewLine + "(14=(10+12 or 10+14+16))", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("U2:U4", "Remarks", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("R2:U2", "Perticulars of Payment Received", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("R3:R4", "Received" + Environment.NewLine + "At WH/RO", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("S3:S4", "C.R No. & Date", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("T3:T4", "Cheque/DD No. & Date", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("T3:T4", "SD", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("U3:U4", "Amount", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("V3:V4", "TDS", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("V2:V4", "Amount Received Against Bill (Rs.)", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("W2:W4", "Adjustment/Deduction", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("W2:W2", "Balance", DynamicExcel.CellAlignment.Middle);
                //exl.AddCell("W2", "Balance", DynamicExcel.CellAlignment.Middle);    

                for (var i = 65; i < 86; i++)
                {
                    char character = (char)i;
                    string text = character.ToString();
                    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                }
                /*exl.AddTable<InvoiceData>("A", 6, model.InvoiceData, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16 });
                exl.AddTable<CashReceiptData>("R", 6, model.CashReceiptData, new[] { 12, 30, 14, 14, 14, 14, 14, 40 });/
                exl.AddTable<PpgRegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16, 12, 30, 14, 14, 14, 14, 14, 40 });*/
                exl.AddTable("A", 6, modelCreditDebit, new[] { 6, 20, 20, 20, 12, 20, 10, 15, 15, 15, 20, 12, 12, 8, 14, 8, 14, 8, 14, 16, 30 });
                var igstamt = modelCreditDebit.Sum(o => o.ITaxAmount);
                var sgstamt = modelCreditDebit.Sum(o => o.STaxAmount);
                var cgstamt = modelCreditDebit.Sum(o => o.CTaxAmount);
                var totalamt = modelCreditDebit.Sum(o => o.Total);
                exl.AddCell("O" + (modelCreditDebit.Count + 6).ToString(), igstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("Q" + (modelCreditDebit.Count + 6).ToString(), cgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("S" + (modelCreditDebit.Count + 6).ToString(), sgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("T" + (modelCreditDebit.Count + 6).ToString(), totalamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                /*exl.AddCell("O" + (model.Count + 7).ToString(), "Invoice Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 7).ToString(), InvoiceAmount, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 8).ToString(), "Cash Receipt Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 8).ToString(), CRAmount, DynamicExcel.CellAlignment.CenterRight);*/

                exl.Save();
            }
            return excelFile;
        }

        #endregion

        #region Daily Transaction Diary
        public void GetDailyTransactionReportList(CHN_DailyTransactionReport ObjDailyTransactionReport)
        {

            DateTime dtDTR = DateTime.ParseExact(ObjDailyTransactionReport.DTRDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String DTRDate = dtDTR.ToString("yyyy/MM/dd");

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "repoDate", MySqlDbType = MySqlDbType.DateTime, Value = DTRDate });

            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("RptDailyTranDtls", CommandType.StoredProcedure, DParam);

            CHN_DailyTransactionReport newObjDailyTransactionReport = new CHN_DailyTransactionReport();

            newObjDailyTransactionReport.AppeasementSummaryList = new List<DTRAppeasementSummary>();
            newObjDailyTransactionReport.DeStuffingSummaryList = new List<DTRDeStuffingSummary>();
            newObjDailyTransactionReport.CartingSummaryList = new List<DTRCartingSummary>();
            newObjDailyTransactionReport.StuffingSummaryList = new List<DTRStuffingInportExportBONDSummary>();
            newObjDailyTransactionReport.InportInSummaryList = new List<DTRStuffingInportExportBONDSummary>();
            newObjDailyTransactionReport.InportOutSummaryList = new List<DTRStuffingInportExportBONDSummary>();
            newObjDailyTransactionReport.ExportInSummaryList = new List<DTRStuffingInportExportBONDSummary>();
            newObjDailyTransactionReport.ExportOutSummaryList = new List<DTRStuffingInportExportBONDSummary>();
            newObjDailyTransactionReport.BONDUnloadingSummaryList = new List<DTRStuffingInportExportBONDSummary>();
            newObjDailyTransactionReport.BONDDeliverySummaryList = new List<DTRStuffingInportExportBONDSummary>();
            newObjDailyTransactionReport.EmptyInTransporterSummaryList = new List<DTREmptyTransporterSummary>();
            newObjDailyTransactionReport.EmptyOutTransporterSummaryList = new List<DTREmptyTransporterSummary>();

            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    #region AppeasementSummary
                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        newObjDailyTransactionReport.AppeasementSummaryList.Add(new DTRAppeasementSummary
                        {
                            BOENo = item["BOENo"].ToString(),
                            BOEDate = item["BOEDate"].ToString(),
                            Importer = item["Importer"].ToString(),
                            CHA = item["CHA"].ToString(),
                            Cargo = item["Cargo"].ToString(),
                            ContainerNo = item["ContainerNo"].ToString(),
                            Size = item["Size"].ToString(),
                            NoOfPackages = item["NoOfPackages"].ToString(),
                            GrossWeight = item["GrossWeight"].ToString(),
                            Remarks = item["Remarks"].ToString(),

                        });
                    }

                    #endregion

                    #region DeStuffingSummary
                    foreach (DataRow item in Result.Tables[1].Rows)
                    {
                        newObjDailyTransactionReport.DeStuffingSummaryList.Add(new DTRDeStuffingSummary
                        {
                            BOENo = item["BOENo"].ToString(),
                            BOEDate = item["BOEDate"].ToString(),
                            Importer = item["Importer"].ToString(),
                            CHA = item["CHA"].ToString(),
                            Cargo = item["Cargo"].ToString(),
                            GodownId = item["GodownId"].ToString(),
                            ContainerNo = item["ContainerNo"].ToString(),
                            Size = item["Size"].ToString(),
                            NoOfPackages = item["NoOfPackages"].ToString(),
                            DestuffingWeight = item["DestuffingWeight"].ToString(),
                            Remarks = item["Remarks"].ToString(),
                        });
                    }
                    #endregion

                    #region CartingSummary
                    foreach (DataRow item in Result.Tables[2].Rows)
                    {
                        newObjDailyTransactionReport.CartingSummaryList.Add(new DTRCartingSummary
                        {
                            ShippingBillNo = item["ShippingBillNo"].ToString(),
                            ShippingBillDate = item["ShippingBillDate"].ToString(),
                            ExporterName = item["ExporterName"].ToString(),
                            CHA = item["CHA"].ToString(),
                            Cargo = item["Cargo"].ToString(),
                            Units = item["Units"].ToString(),
                            Weight = item["Weight"].ToString(),
                            CartingType = item["CartingType"].ToString(),
                            Remarks = item["Remarks"].ToString(),
                        });
                    }
                    #endregion

                    #region StuffingSummary
                    foreach (DataRow item in Result.Tables[3].Rows)
                    {
                        var objStuffingSummaryIsExists = newObjDailyTransactionReport.StuffingSummaryList.Where(x => x.ShippingBillNo == item["ShippingBillNo"].ToString()).FirstOrDefault();
                        if (objStuffingSummaryIsExists == null)
                        {
                            newObjDailyTransactionReport.StuffingSummaryList.Add(new DTRStuffingInportExportBONDSummary
                            {
                                ShippingBillNo = item["ShippingBillNo"].ToString(),
                                ShippingBillDate = item["ShippingBillDate"].ToString(),
                                ExporterName = item["ExporterName"].ToString(),
                                CHA = item["CHA"].ToString(),
                                ShippingLine = item["ShippingLine"].ToString(),
                                CartingType = item["CartingType"].ToString(),
                                Remarks = item["Remarks"].ToString(),
                                SizeForty = Convert.ToInt32(item["SizeForty"].ToString()),
                                SizeTwenty = Convert.ToInt32(item["SizeTwenty"].ToString()),

                            });

                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.StuffingSummaryList.Where(x => x.ShippingBillNo == item["ShippingBillNo"].ToString()).ToList())
                            {
                                DTRContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);                                
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.StuffingSummaryList.Where(x => x.ShippingBillNo == item["ShippingBillNo"].ToString()).ToList())
                            {
                                DTRContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);                                
                            }
                        }

                    }
                    #endregion

                    #region ExportInSummary
                    foreach (DataRow item in Result.Tables[4].Rows)
                    {
                        var objStuffingSummaryIsExists = newObjDailyTransactionReport.ExportInSummaryList.Where(x => x.ShippingBillNo == item["ShippingBillNo"].ToString()).FirstOrDefault();
                        if (objStuffingSummaryIsExists == null)
                        {
                            newObjDailyTransactionReport.ExportInSummaryList.Add(new DTRStuffingInportExportBONDSummary
                            {
                                ShippingBillNo = item["ShippingBillNo"].ToString(),
                                ShippingBillDate = item["ShippingDate"].ToString(),
                                ExporterName = item["ExporterName"].ToString(),
                                CHA = item["CHA"].ToString(),
                                ShippingLine = item["ShippingLine"].ToString(),
                                CartingType = item["Transporter"].ToString(),
                                Remarks = item["Remarks"].ToString(),
                            });

                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.ExportInSummaryList.Where(x => x.ShippingBillNo == item["ShippingBillNo"].ToString()).ToList())
                            {
                                DTRContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.ExportInSummaryList.Where(x => x.ShippingBillNo == item["ShippingBillNo"].ToString()).ToList())
                            {
                                DTRContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }
                        }

                    }
                    #endregion

                    #region ExportOutSummary
                    foreach (DataRow item in Result.Tables[5].Rows)
                    {
                        var objStuffingSummaryIsExists = newObjDailyTransactionReport.ExportOutSummaryList.Where(x => x.ShippingBillNo == item["ShippingBillNo"].ToString()).FirstOrDefault();
                        if (objStuffingSummaryIsExists == null)
                        {
                            newObjDailyTransactionReport.ExportOutSummaryList.Add(new DTRStuffingInportExportBONDSummary
                            {
                                ShippingBillNo = item["ShippingBillNo"].ToString(),
                                ShippingBillDate = item["ShippingDate"].ToString(),
                                ExporterName = item["ExporterName"].ToString(),
                                CHA = item["CHA"].ToString(),
                                ShippingLine = item["ShippingLine"].ToString(),
                                CartingType = item["Transporter"].ToString(),
                                Remarks = item["Remarks"].ToString(),                                
                            });

                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.ExportOutSummaryList.Where(x => x.ShippingBillNo == item["ShippingBillNo"].ToString()).ToList())
                            {
                                DTRContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);                                
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.ExportOutSummaryList.Where(x => x.ShippingBillNo == item["ShippingBillNo"].ToString()).ToList())
                            {
                                DTRContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }
                        }

                    }
                    #endregion

                    #region ImportInSummary
                    foreach (DataRow item in Result.Tables[6].Rows)
                    {
                        var objStuffingSummaryIsExists = newObjDailyTransactionReport.InportInSummaryList.Where(x => x.ExporterName == item["ImporterName"].ToString()).FirstOrDefault();
                        if (objStuffingSummaryIsExists == null)
                        {
                            newObjDailyTransactionReport.InportInSummaryList.Add(new DTRStuffingInportExportBONDSummary
                            {
                                ShippingBillNo = item["BOENo"].ToString(),
                                ShippingBillDate = item["BOEDate"].ToString(),
                                ExporterName = item["ImporterName"].ToString(),
                                CHA = item["CHA"].ToString(),
                                ShippingLine = item["ShippingLine"].ToString(),
                                CartingType = item["Transporter"].ToString(),
                                Remarks = item["Remarks"].ToString(),
                            });

                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.InportInSummaryList.Where(x => x.ExporterName == item["ImporterName"].ToString()).ToList())
                            {
                                DTRContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.InportInSummaryList.Where(x => x.ExporterName == item["ImporterName"].ToString()).ToList())
                            {
                                DTRContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }
                        }

                    }
                    #endregion

                    #region ImportOutSummary
                    foreach (DataRow item in Result.Tables[7].Rows)
                    {
                        var objStuffingSummaryIsExists = newObjDailyTransactionReport.InportOutSummaryList.Where(x => x.ExporterName == item["ImporterName"].ToString()).FirstOrDefault();
                        if (objStuffingSummaryIsExists == null)
                        {
                            newObjDailyTransactionReport.InportOutSummaryList.Add(new DTRStuffingInportExportBONDSummary
                            {
                                ShippingBillNo = item["BOENo"].ToString(),
                                ShippingBillDate = item["BOEDate"].ToString(),
                                ExporterName = item["ImporterName"].ToString(),
                                CHA = item["CHA"].ToString(),
                                ShippingLine = item["ShippingLine"].ToString(),
                                CartingType = item["Transporter"].ToString(),
                                Remarks = item["Remarks"].ToString(),
                            });

                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.InportOutSummaryList.Where(x => x.ExporterName == item["ImporterName"].ToString()).ToList())
                            {
                                DTRContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.InportOutSummaryList.Where(x => x.ExporterName == item["ImporterName"].ToString()).ToList())
                            {
                                DTRContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }
                        }

                    }
                    #endregion

                    #region BONDUnloadingSummary
                    foreach (DataRow item in Result.Tables[8].Rows)
                    {
                        var objStuffingSummaryIsExists = newObjDailyTransactionReport.BONDUnloadingSummaryList.Where(x => x.ShippingBillNo == item["BOENo"].ToString()).FirstOrDefault();
                        if (objStuffingSummaryIsExists == null)
                        {
                            newObjDailyTransactionReport.BONDUnloadingSummaryList.Add(new DTRStuffingInportExportBONDSummary
                            {
                                ShippingBillNo = item["BOENo"].ToString(),
                                ShippingBillDate = item["BOEDate"].ToString(),
                                ExporterName = item["ImporterName"].ToString(),
                                CHA = item["CHA"].ToString(),
                                ShippingLine = item["ShippingLine"].ToString(),
                                CartingType = item["Transporter"].ToString(),
                                Remarks = item["Remarks"].ToString(),
                            });

                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.BONDUnloadingSummaryList.Where(x => x.ShippingBillNo == item["BOENo"].ToString()).ToList())
                            {
                                DTRContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.BONDUnloadingSummaryList.Where(x => x.ShippingBillNo == item["BOENo"].ToString()).ToList())
                            {
                                DTRContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }
                        }

                    }
                    #endregion

                    #region BONDDeliverySummary
                    foreach (DataRow item in Result.Tables[9].Rows)
                    {
                        var objStuffingSummaryIsExists = newObjDailyTransactionReport.BONDDeliverySummaryList.Where(x => x.ShippingBillNo == item["BOENo"].ToString()).FirstOrDefault();
                        if (objStuffingSummaryIsExists == null)
                        {
                            newObjDailyTransactionReport.BONDDeliverySummaryList.Add(new DTRStuffingInportExportBONDSummary
                            {
                                ShippingBillNo = item["BOENo"].ToString(),
                                ShippingBillDate = item["BOEDate"].ToString(),
                                ExporterName = item["ImporterName"].ToString(),
                                CHA = item["CHA"].ToString(),
                                ShippingLine = item["ShippingLine"].ToString(),
                                CartingType = item["Transporter"].ToString(),
                                Remarks = item["Remarks"].ToString(),
                            });

                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.BONDDeliverySummaryList.Where(x => x.ShippingBillNo == item["BOENo"].ToString()).ToList())
                            {
                                DTRContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString()));
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.BONDDeliverySummaryList.Where(x => x.ShippingBillNo == item["BOENo"].ToString()).ToList())
                            {
                                DTRContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }
                        }

                    }
                    #endregion

                    #region EmptyInTransporterSummary
                    foreach (DataRow item in Result.Tables[10].Rows)
                    {
                        newObjDailyTransactionReport.EmptyInTransporterSummaryList.Add(new DTREmptyTransporterSummary
                        {
                            FromDate = item["FromLocation"].ToString(),
                            ToDate = item["ToLocation"].ToString(),
                            ContainerNo = item["ContainerNo"].ToString(),
                            Size = item["Size"].ToString(),
                            ShippingLine = item["ShippingLine"].ToString(),
                            TransportedBy = item["Transporter"].ToString(),
                            Remarks = item["Remarks"].ToString(),
                        });

                    }
                    #endregion

                    #region EmptyOutTransporterSummary
                    foreach (DataRow item in Result.Tables[11].Rows)
                    {
                        newObjDailyTransactionReport.EmptyOutTransporterSummaryList.Add(new DTREmptyTransporterSummary
                        {
                            FromDate = item["FromLocation"].ToString(),
                            ToDate = item["ToLocation"].ToString(),
                            ContainerNo = item["ContainerNo"].ToString(),
                            Size = item["Size"].ToString(),
                            ShippingLine = item["ShippingLine"].ToString(),
                            TransportedBy = item["Transporter"].ToString(),
                            Remarks = item["Remarks"].ToString(),
                        });

                    }
                    #endregion

                    #region Assign Size 20 and 40

                    //foreach (var item in newObjDailyTransactionReport.StuffingSummaryList)
                    //{
                    //    newObjDailyTransactionReport.StuffingSummaryList[0].SizeTwenty += item.ContainerInfoList.Count(x => x.Size == "20");
                    //    newObjDailyTransactionReport.StuffingSummaryList[0].SizeForty += item.ContainerInfoList.Count(x => x.Size == "40");
                    //}
                    foreach (var item in newObjDailyTransactionReport.StuffingSummaryList)
                    {
                        newObjDailyTransactionReport.StuffingSummaryList[0].SizeTwenty=item.SizeTwenty;
                        newObjDailyTransactionReport.StuffingSummaryList[0].SizeForty = item.SizeForty;
                    }


                    foreach (var item in newObjDailyTransactionReport.InportInSummaryList)
                    {
                        newObjDailyTransactionReport.InportInSummaryList[0].SizeTwenty += item.ContainerInfoList.Count(x => x.Size == "20");
                        newObjDailyTransactionReport.InportInSummaryList[0].SizeForty += item.ContainerInfoList.Count(x => x.Size == "40");
                    }

                    foreach (var item in newObjDailyTransactionReport.InportOutSummaryList)
                    {
                        newObjDailyTransactionReport.InportOutSummaryList[0].SizeTwenty += item.ContainerInfoList.Count(x => x.Size == "20");
                        newObjDailyTransactionReport.InportOutSummaryList[0].SizeForty += item.ContainerInfoList.Count(x => x.Size == "40");
                    }

                    foreach (var item in newObjDailyTransactionReport.ExportInSummaryList)
                    {
                        newObjDailyTransactionReport.ExportInSummaryList[0].SizeTwenty += item.ContainerInfoList.Count(x => x.Size == "20");
                        newObjDailyTransactionReport.ExportInSummaryList[0].SizeForty += item.ContainerInfoList.Count(x => x.Size == "40");
                    }

                    foreach (var item in newObjDailyTransactionReport.ExportOutSummaryList)
                    {
                        newObjDailyTransactionReport.ExportOutSummaryList[0].SizeTwenty += item.ContainerInfoList.Count(x => x.Size == "20");
                        newObjDailyTransactionReport.ExportOutSummaryList[0].SizeForty += item.ContainerInfoList.Count(x => x.Size == "40");
                    }

                    foreach (var item in newObjDailyTransactionReport.BONDUnloadingSummaryList)
                    {
                        newObjDailyTransactionReport.BONDUnloadingSummaryList[0].SizeTwenty += item.ContainerInfoList.Count(x => x.Size == "20");
                        newObjDailyTransactionReport.BONDUnloadingSummaryList[0].SizeForty += item.ContainerInfoList.Count(x => x.Size == "40");
                    }

                    foreach (var item in newObjDailyTransactionReport.BONDDeliverySummaryList)
                    {
                        newObjDailyTransactionReport.BONDDeliverySummaryList[0].SizeTwenty += item.ContainerInfoList.Count(x => x.Size == "20");
                        newObjDailyTransactionReport.BONDDeliverySummaryList[0].SizeForty += item.ContainerInfoList.Count(x => x.Size == "40");
                    }

                    if (newObjDailyTransactionReport.EmptyInTransporterSummaryList.Count > 0)
                    {
                        newObjDailyTransactionReport.EmptyInTransporterSummaryList[0].SizeTwenty = newObjDailyTransactionReport.EmptyInTransporterSummaryList.Count(x => x.Size == "20");
                        newObjDailyTransactionReport.EmptyInTransporterSummaryList[0].SizeForty = newObjDailyTransactionReport.EmptyInTransporterSummaryList.Count(x => x.Size == "40");
                    }

                    if (newObjDailyTransactionReport.EmptyOutTransporterSummaryList.Count > 0)
                    {
                        newObjDailyTransactionReport.EmptyOutTransporterSummaryList[0].SizeTwenty = newObjDailyTransactionReport.EmptyOutTransporterSummaryList.Count(x => x.Size == "20");
                        newObjDailyTransactionReport.EmptyOutTransporterSummaryList[0].SizeForty = newObjDailyTransactionReport.EmptyOutTransporterSummaryList.Count(x => x.Size == "40");
                    }


                    #endregion

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = newObjDailyTransactionReport;
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

        private DTRContainerInfo GetContainerInfo(string containerNo, string size)
        {
            DTRContainerInfo objContainerInfo = new DTRContainerInfo();
            if (!String.IsNullOrWhiteSpace(containerNo))
            {
                objContainerInfo.ContainerNo = containerNo;
            }
            else
            {
                objContainerInfo.ContainerNo = "-";
            }

            if (!String.IsNullOrWhiteSpace(size))
            {
                objContainerInfo.Size = size;
            }
            else
            {
                objContainerInfo.Size = "-";
            }

            return objContainerInfo;
        }




        #region Consolidated GST Register
        public void GetConsolidatedGST(DateTime date1, DateTime date2)
        {
            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.DateTime, Value = date2 });
         //   LstParam.Add(new MySqlParameter { ParameterName = "In_Type", MySqlDbType = MySqlDbType.VarChar, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //var objRegOutwardSupply = (List<RegisterOfOutwardSupplyModel>)DataAccess.ExecuteDynamicSet<List<RegisterOfOutwardSupplyModel>>("GetRegisterofOutwardSupply", DParam);
            DataSet ds = DataAccess.ExecuteDataSet("GetConsolidatedGSTRegister", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
                List<CHN_ConsolidatedGSTRegisterModel> model = new List<CHN_ConsolidatedGSTRegisterModel>();
                try
                {
                    if (ds.Tables.Count > 0)
                    {
                        model = (from DataRow dr in dt.Rows
                                 select new CHN_ConsolidatedGSTRegisterModel()
                                 {
                                     SlNo = Convert.ToInt32(dr["SlNo"]),
                                     InvoiceNo = dr["InvoiceNo"].ToString(),
                                     InvoiceDate = dr["InvoiceDate"].ToString(),
                                     Name = dr["Name"].ToString(),
                                     GST = dr["GST"].ToString(),
                                     Nature = dr["Nature"].ToString(),
                                                                        
                                     ServiceValue = Convert.ToDecimal(dr["ServiceValue"]),
                                     Excempted = Convert.ToDecimal(dr["Excempted"]),

                                     ITaxAmount = Convert.ToDecimal(dr["ITaxAmount"]),
                                   CTaxAmount = Convert.ToDecimal(dr["CTaxAmount"]),
                                   STaxAmount = Convert.ToDecimal(dr["STaxAmount"]),
                                         Total = Convert.ToDecimal(dr["Total"]),
                                    
                                 }).ToList();
                    }
                    decimal InvoiceAmount = 0, CRAmount = 0;
                    //foreach (DataRow dr in ds.Tables[1].Rows)
                    //{
                    //    InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]);
                    //    CRAmount = Convert.ToDecimal(dr["CRAmount"]);
                    //}
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = RegisterofConsolidatedGSTExcel(model,date1.ToString("dd/MM/yyyy"),date2.ToString("dd/MM/yyyy"));
                }
                catch (Exception ex)
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
                finally
                {
                    ds.Dispose();
                }
            }
                     
        private string RegisterofConsolidatedGSTExcel(List<CHN_ConsolidatedGSTRegisterModel> model, string date1,string date2)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "Principal Place of Business"
                        + Environment.NewLine + "CENTRAL WAREHOUSE"
                        + Environment.NewLine + Environment.NewLine
                        + "CONSOLIDATED GST SUMMARY From "+ date1 +" TO " + date2;
                exl.MargeCell("A1:M1", title, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("N1:O1", "BILL REGISTER", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A2:A4", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B2:B4", "Invoice No. / Bill Of Supply No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C2:C4", "Date", DynamicExcel.CellAlignment.Middle);
                    //+ Environment.NewLine + "(Name of State)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D2:D4", "Name of the Party", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E2:E4", "GST Number of the Party", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F2:F4", "Nature of Service" , DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G2:H2", "Value", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G3:G4", "TAXABLE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H3:H4", "EXEMPTED", DynamicExcel.CellAlignment.Middle);
               
                exl.MargeCell("I2:K2", "TAXES", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I3:I4", "SGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J3:J4", "CGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K3:K4", "IGST", DynamicExcel.CellAlignment.Middle);
               exl.MargeCell("L2:L4", "Total Tax Collected", DynamicExcel.CellAlignment.Middle);
            
                //for (var i = 65; i < 85; i++)
                //{
                //    char character = (char)i;
                //    string text = character.ToString();
                //    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                //}
                /*exl.AddTable<InvoiceData>("A", 6, model.InvoiceData, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16 });
                exl.AddTable<CashReceiptData>("R", 6, model.CashReceiptData, new[] { 12, 30, 14, 14, 14, 14, 14, 40 });/
                exl.AddTable<PpgRegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16, 12, 30, 14, 14, 14, 14, 14, 40 });*/
                exl.AddTable<CHN_ConsolidatedGSTRegisterModel>("A", 6, model, new[] { 6, 20, 20, 20, 12, 20, 10, 15, 20, 12, 12, 8 });
                var igstamt = model.Sum(o => o.ITaxAmount);
                var sgstamt = model.Sum(o => o.STaxAmount);
                var cgstamt = model.Sum(o => o.CTaxAmount);
                var Service = model.Sum(o => o.ServiceValue);
                var Excempted = model.Sum(o => o.Excempted);
                var Total= model.Sum(o => o.Total);
                exl.AddCell("G" + (model.Count + 6).ToString(), Service.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("H" + (model.Count + 6).ToString(), Excempted.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("I" + (model.Count + 6).ToString(), sgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("J" + (model.Count + 6).ToString(), cgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("K" + (model.Count + 6).ToString(), igstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("L" + (model.Count + 6).ToString(), Total.ToString(), DynamicExcel.CellAlignment.CenterRight);

                /*exl.AddCell("O" + (model.Count + 7).ToString(), "Invoice Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 7).ToString(), InvoiceAmount, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 8).ToString(), "Cash Receipt Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 8).ToString(), CRAmount, DynamicExcel.CellAlignment.CenterRight);*/

                exl.Save();
            }
            return excelFile;
        }

        #endregion

        #region Register of E-Invoice 
        public void GetRegisterofEInvoice(DateTime date1, DateTime date2)
        {
            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.DateTime, Value = date2 });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //var objRegOutwardSupply = (List<RegisterOfOutwardSupplyModel>)DataAccess.ExecuteDynamicSet<List<RegisterOfOutwardSupplyModel>>("GetRegisterofOutwardSupply", DParam);
            DataSet ds = DataAccess.ExecuteDataSet("GetRegisterofEInvoice", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];


            List<CHNRegisterOfEInvoiceModel> model = new List<CHNRegisterOfEInvoiceModel>();
            try
            {

                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = RegisterofEInvoiceExcel(model, dt);
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                ds.Dispose();
            }

        }


        private string RegisterofEInvoiceExcel(List<CHNRegisterOfEInvoiceModel> model, DataTable dt)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            System.Web.UI.WebControls.GridView Grid = new System.Web.UI.WebControls.GridView();


            System.Web.UI.WebControls.Table tb = new System.Web.UI.WebControls.Table();

            if (dt.Rows.Count > 0)
            {

                Grid.AllowPaging = false;
                Grid.DataSource = dt;
                Grid.DataBind();


                for (int i = 0; i < Grid.HeaderRow.Cells.Count; i++)
                {
                    Grid.HeaderRow.Cells[i].Style.Add("background-color", "#edebeb");
                    Grid.HeaderRow.Cells[i].Attributes.Add("class", "Headermode");


                }

                for (int i = 0; i < Grid.Rows.Count; i++)
                {
                    //Apply text style to each Row

                    Grid.Rows[i].BackColor = System.Drawing.Color.White;                   

                }                

                System.Web.UI.WebControls.TableCell cell5 = new System.Web.UI.WebControls.TableCell();
                cell5.Controls.Add(Grid);
                System.Web.UI.WebControls.TableRow tr5 = new System.Web.UI.WebControls.TableRow();
                tr5.Cells.Add(cell5);
                                
                tb.Rows.Add(tr5);

            }
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                using (System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw))
                {
                    System.IO.StreamWriter writer = System.IO.File.AppendText(excelFile);
                    tb.RenderControl(hw);
                    writer.WriteLine(sw.ToString());
                    writer.Close();
                }
            }
            
            return excelFile;
        }


        #endregion

        #region Bulk EInvoice Generation

        public void GetBulkIrnDetails()
        {
            int Status = 0;

            IDataParameter[] DParam = { };

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            CHN_BulkIRN objInvoice = new CHN_BulkIRN();
            IDataReader Result = DataAccess.ExecuteDataReader("IRNNotgeneratedInvoiceList", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    objInvoice.lstPostPaymentChrg.Add(new CHN_BulkIRNDetails
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        GstNo = Result["GstNo"].ToString(),
                        SupplyType = Result["SupplyType"].ToString(),
                        InvoiceType = Result["InvoiceType"].ToString(),
                        OperationType = Result["InvType"].ToString()
                    });
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

        public void AddEditIRNErrorResponse(string InvoiceNo, string ErrorMessage, int ErrorCode)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ErrorMessage", MySqlDbType = MySqlDbType.String, Value = ErrorMessage });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ErrorCode", MySqlDbType = MySqlDbType.Int32, Value = ErrorCode });


            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddeditirnErrorResponce", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = (Result == 1) ? "IRN Generate Successfully" : "IRN Generate Successfully";
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
        #endregion

        #region Party Wise Ledger
        public void GetPartyWiseLedger(string FromDate, string ToDate, int PartyId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetRptPartyWiseLedger", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PartyWiseLedgerList> LstPartyLedger = new List<PartyWiseLedgerList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPartyLedger.Add(new PartyWiseLedgerList
                    {
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        ReceiptDate = Result["ReceiptDate"].ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString(),
                        Amount = Convert.ToDecimal(Result["Amount"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPartyLedger;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data Found";
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
        public void GetEximTraderForPartyLedger()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetEximTraderForPartyLedger", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHN_PartyLedgerList> LstParty = new List<CHN_PartyLedgerList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstParty.Add(new CHN_PartyLedgerList
                    {
                        PartyId = Convert.ToInt32(Result["EximTraderId"].ToString()),
                        PartyName = Result["EximTraderName"].ToString(),
                        GSTNo= Result["GSTNo"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstParty;
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

        public void GetSAC()
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllSAC", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_SACList> LstSAC = new List<Chn_SACList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSAC.Add(new Chn_SACList
                    {
                        SAC = Result["SAC"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSAC;
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

        public void ServiceCodeWiseInvDtls( string date1, string date2, string SAC)
        {



            List<MySqlParameter> LstParam = new List<MySqlParameter>();
          
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(date1).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(date2).ToString("yyyy-MM-dd") });


            LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = SAC });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
          

            DataSet ds = DataAccess.ExecuteDataSet("ServiceCodeWiseInvDtls", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            _DBResponse = new DatabaseResponse();
            int Status = 0;

            List<Chn_ServiceCodeWiseInvDtlsRpt> model = new List<Chn_ServiceCodeWiseInvDtlsRpt>();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    model = (from DataRow dr in dt.Rows
                             select new Chn_ServiceCodeWiseInvDtlsRpt()
                             {
                                

                                 SAC = dr["SAC"].ToString(),
                                 InvoiceNumber = dr["Invoice"].ToString(),
                                 Date = dr["Date"].ToString(),
                                 CGSTAmt = Convert.ToDecimal(dr["CGSTAmt"]),
                                 SGSTAmt =Convert.ToDecimal( dr["SGSTAmt"]),
                                 IGSTAmt = Convert.ToDecimal(dr["IGSTAmt"]),
                                 TotalValue = Convert.ToDecimal(dr["TotalValue"])
                                
                             }).ToList();
                }

                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = ServiceCodeWiseInvDtlsExcel(model, date1, date2);
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                ds.Dispose();
            }
        }

     

        private string ServiceCodeWiseInvDtlsExcel(List<Chn_ServiceCodeWiseInvDtlsRpt> model,  string date1, string date2)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION";
                                    
                var h1 = "(A Govt.of India Undertaking)";
                var h2 = "Service Code Wise Invoice Details From  " + date1+ " To " + date2;

                exl.MargeCell("A1:G1", title, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A2:G2", h1, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A3:G3", h2, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

              

                exl.MargeCell("A4:A4", "SAC", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B4:B4", "Invoice/GSTNo", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C4:C4", "Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D4:D4", "CGSTAmt", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E4:E4", "SGSTAmt", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F4:F4", "IGSTAmt", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G4:G4", "TotalValue", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
            

                exl.AddTable<Chn_ServiceCodeWiseInvDtlsRpt>("A", 5, model, new[] { 6, 20, 10,10,10,10,10,10 });

                var CGSTAmt = model.Sum(o => o.CGSTAmt);
                var SGSTAmt = model.Sum(o => o.SGSTAmt);
                var IGSTAmt = model.Sum(o => o.IGSTAmt);
                var TotalValue = model.Sum(o => o.TotalValue);
                exl.MargeCell("A" + (model.Count + 5).ToString() + ":C" + (model.Count + 5).ToString() + "", "TOTAL", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("D" + (model.Count + 5).ToString(), CGSTAmt.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("E" + (model.Count + 5).ToString(), SGSTAmt.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("F" + (model.Count + 5).ToString(), IGSTAmt.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("G" + (model.Count + 5).ToString(), TotalValue.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
             

                exl.Save();
            }
            return excelFile;

        }

        public void ServiceCodeWiseInvDtlsPDF( DateTime date1, DateTime date2, string SAC)
        {
            int Status = 0;
            var LstParam = new List<MySqlParameter>();
           
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = date2 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = SAC });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            DataSet ds = DataAccess.ExecuteDataSet("ServiceCodeWiseInvDtls", CommandType.StoredProcedure, DParam);
            //DataTable dt = ds.Tables[0];

            List<Chn_ServiceCodeWiseInvDtls> model = new List<Chn_ServiceCodeWiseInvDtls>();
            try
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    Status = 1;
                }
                else
                {
                    Status = 0;
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ds;
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
                ds.Dispose();
            }
        }

        public void ChequeSummary(Chn_ChequeSummary ObjChequeSummary)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjChequeSummary.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjChequeSummary.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = ObjChequeSummary.Type });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ChequeSummaryReport", CommandType.StoredProcedure, DParam);
            IList<Chn_ChequeSummary> LstChequeSummary = new List<Chn_ChequeSummary>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstChequeSummary.Add(new Chn_ChequeSummary
                    {


                        ReceiptDate = Result["ReceiptDate"].ToString(),
                        Bank = Result["DraweeBank"].ToString(),

                        Amount = Result["Amount"].ToString(),

                        ChequeNumber = Result["chequeNumber"].ToString(),
                        Date = Result["Date"].ToString(),
                        Party = Result["Party"].ToString(),
                        InvoiceNumber = Result["InvoiceNo"].ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString()
                        //value = Result["value"].ToString()

                    });
                }
                if (Status == 1)
                {
                    LstChequeSummary.Add(new Chn_ChequeSummary
                    {


                        ReceiptDate = "<strong>Total</strong>",
                        Bank = string.Empty,

                        Amount = LstChequeSummary.ToList().Sum(m => Convert.ToDecimal(m.Amount)).ToString(),

                       // ChequeNumber = "<strong>Total</strong>",
                        ChequeNumber = string.Empty,
                        Date = string.Empty,
                        Party = string.Empty,
                        InvoiceNumber = string.Empty,
                        ReceiptNo = string.Empty
                        //value = Result["value"].ToString()

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstChequeSummary;
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

        public void ChequeCashDDPOSummary(Chn_CashChequeDDSummary ObjChequeSummary)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjChequeSummary.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjChequeSummary.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = ObjChequeSummary.Type });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("CashCqeDdPoSdStmtRpt", CommandType.StoredProcedure, DParam);
            IList<Chn_CashChequeDDSummary> LstChequeSummary = new List<Chn_CashChequeDDSummary>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstChequeSummary.Add(new Chn_CashChequeDDSummary
                    {



                        Bank = Result["DraweeBank"].ToString(),
                        Type = Result["PayMode"].ToString(),
                        CashAmount = Convert.ToDecimal(Result["CASHAmount"]),
                        ChequeAmount = Convert.ToDecimal(Result["CHEQUEAmount"]),
                        POSAmount = Convert.ToDecimal(Result["POAmount"]),
                        Amount = Result["OtherAmount"].ToString(),
                        SDAmount = Convert.ToDecimal(Result["SDAmount"]),


                        GCashAmount = Convert.ToDecimal(Result["CASHAmount"]),
                        GChequeAmount = Convert.ToDecimal(Result["CHEQUEAmount"]),
                        GPOSAmount = Convert.ToDecimal(Result["POAmount"]),
                        GOthersAmount = Convert.ToDecimal(Result["OtherAmount"]),

                        ChequeNumber = Result["chequeNumber"].ToString(),
                        Date = Result["Date"].ToString(),
                        Party = Result["Party"].ToString(),
                        InvoiceNumber = Result["InvoiceNo"].ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString()
                        //value = Result["value"].ToString()

                    });
                }
                if (Status == 1)
                {
                    LstChequeSummary.Add(new Chn_CashChequeDDSummary
                    {



                        Bank = string.Empty,

                        GOthersAmount = LstChequeSummary.ToList().Sum(m => Convert.ToDecimal(m.GOthersAmount)),
                        GCashAmount = LstChequeSummary.ToList().Sum(m => m.GCashAmount),
                        GChequeAmount = LstChequeSummary.ToList().Sum(m => m.ChequeAmount),
                        GPOSAmount = LstChequeSummary.ToList().Sum(m => m.GPOSAmount),
                        SDAmount = LstChequeSummary.ToList().Sum(m => m.SDAmount),

                        Amount = string.Empty,


                        ChequeNumber = "<strong>Total</strong>",
                        Date = string.Empty,
                        Party = string.Empty,
                        InvoiceNumber = string.Empty,
                        ReceiptNo = string.Empty
                        //value = Result["value"].ToString()

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstChequeSummary;
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

        #region ContainerOutReport
        public void ContainerOutport(ContainerOutReport ObjContainerOutReport)
        {
            int sizeTwenty = 0;
            int sizeFourty = 0;
            DateTime dtfrom = DateTime.ParseExact(ObjContainerOutReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjContainerOutReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjContainerOutReport.ImportExport });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadedEmpty", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjContainerOutReport.LoadedEmpty });

  
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ContainerOutReport", CommandType.StoredProcedure, DParam);
            ContainerOutReport LstContainerOutReport = new ContainerOutReport();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstContainerOutReport.lstContainerOutReport.Add(new ContainerOutReportList
                    {



                        Date = Result["Date"].ToString(),

                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                        Time = Result["Time"].ToString(),
                        LoadedEmpty = Result["LoadedEmpty"].ToString(),
                        ImportExport = Result["ImportExport"].ToString()
                    });
                }

                if (Status == 1)
                {

                    if (LstContainerOutReport.lstContainerOutReport.Count > 0)
                    {
                        sizeTwenty = LstContainerOutReport.lstContainerOutReport.Count(o => o.Size == "20");
                        sizeFourty = LstContainerOutReport.lstContainerOutReport.Count(o => o.Size == "40");
                        //sizeTwenty = 0;
                        //sizeFourty = 0;

                        //LstContainerOutReport.lstContainerOutReport.ToList().ForEach(m =>
                        //{
                        //    if (m.Size == "20" && m.Size != null && m.Size != "")
                        //    {
                        //        sizeTwenty += 1;
                        //    }
                        //    if (m.Size == "40" && m.Size != null && m.Size != "")
                        //    {
                        //        sizeFourty += 1;
                        //    }

                        //});
                    }

                    LstContainerOutReport.SizeTwenty = Convert.ToString(sizeTwenty);
                    LstContainerOutReport.SizeFouirty = Convert.ToString(sizeFourty);

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstContainerOutReport;
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
        #endregion
      
        #region LCL Destuffing Register
        public void GetLCLDestuffingRegister(Chn_LCLDestuffingRegister obj)
        {

            DateTime dtfrom = DateTime.ParseExact(obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
           // LstParam.Add(new MySqlParameter { ParameterName = "in_days", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjContainerBalanceInCFS.days });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getLclDestuffingRegister", CommandType.StoredProcedure, DParam);
            IList<Chn_LCLDestuffingRegister> LstDestuff = new List<Chn_LCLDestuffingRegister>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstDestuff.Add(new Chn_LCLDestuffingRegister
                    {
                        DestuffingEntryDate = Result["DestuffingEntryDate"].ToString(),
                        IGMNo = Result["IGM_No"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        LineNo = Result["LineNo"].ToString(),
                        GodownName = Result["GodownName"].ToString(),
                        CargoDescription = Result["CargoDescription"].ToString(),
                        Importer = Result["Importer"].ToString(),
                        DeliveryDate = Result["Deliverydate"].ToString(),
                        IssueSlipNo = Result["issueslipno"].ToString(),
                        GatepassNo = Result["GatepassNo"].ToString(),
                        NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                        GrossWeight=Convert.ToDecimal(Result["DestuffWeight"]),
                        Grid = Convert.ToDecimal(Result["Grid"]),
                        NoOfPkgdel = Convert.ToInt32(Result["NoOfPkgdel"]),
                        CB = Result["CB"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                       
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDestuff;
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
        #endregion


        public List<MonthlyPerformaceReport> GetMonthlyPerformanceReportDataToPrintCHN(int monthNo, int yearNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "repoYear", MySqlDbType = MySqlDbType.Int32, Value = yearNo });
            LstParam.Add(new MySqlParameter { ParameterName = "repoMonth", MySqlDbType = MySqlDbType.Int32, Value = monthNo });

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("RptMonthlyPerformance", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            List<MonthlyPerformaceReport> LstMonthlyPerformaceReport = new List<MonthlyPerformaceReport>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        MonthlyPerformaceReport objMonthlyPerformaceReport = new MonthlyPerformaceReport();
                        objMonthlyPerformaceReport.DescriptionId = Convert.ToInt32(item["SlNo"]);
                        objMonthlyPerformaceReport.MonthUnderReport = Convert.ToString(item["CurMonthAmt"]);
                        objMonthlyPerformaceReport.PrevMonth = Convert.ToString(item["PrevMonthAmt"]);
                        objMonthlyPerformaceReport.CorresMonthPrevYear = Convert.ToString(item["MonthPrevYearAmt"]);

                        LstMonthlyPerformaceReport.Add(objMonthlyPerformaceReport);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data Found";
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

            return LstMonthlyPerformaceReport;
        }


        #region Daily CashBook with SD

        public void DailyCashBookWithSD(Chn_DailyCashBookDtl ObjDailyCashBook)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyCashBook.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyCashBook.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyCashBookReportSD", CommandType.StoredProcedure, DParam);
            IList<Chn_DailyCashBookDtl> LstDailyCashBook = new List<Chn_DailyCashBookDtl>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //ObjStatusDtl = new ConsumerList();
                    //ObjStatusDtl.RegistrationNo = Convert.ToString(Result["RegistrationNo"]);
                    //ObjStatusDtl.Name = Convert.ToString(Result["CompanyName"]);
                    //ObjStatusDtl.Address = Convert.ToString(Result["CompanyAddress"]);
                    //ObjStatusDtl.IssueDate = Convert.ToString(Result["IssuedOn"]);


                    //                 InvoiceDate DATE,
                    //         InvoiceNumber       VARCHAR(30),
                    //ReceiptAmount DECIMAL(18, 3),
                    //InvoiceAmount DECIMAL(18, 3),
                    //Value DECIMAL(18, 3),
                    //OpeningBalance DECIMAL(18, 3),
                    //ClosingBalance DECIMAL(18, 3)

                    LstDailyCashBook.Add(new Chn_DailyCashBookDtl
                    {

                        //ReceiptNo, ReceiptDate, Party, ChequeNo, CashReceiptId, GenSpace, sto, Insurance, GroundRentEmpty, GroundRentLoaded, Mf, EntCharge, 
                        //Fum, OtCharge, CGSTAmt, SGSTAmt, IGSTAmt, MISC, MiscExcess, TotalCash, TotalCheque, tdsCol, crTDS
                        /*    CRNo = Result["ReceiptNo"].ToString(),
                            ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),
                            CRId = Convert.ToInt32(Result["CashReceiptId"]),
                            Depositor = Result["Party"].ToString(),
                            ChqNo = Result["ChequeNo"].ToString(),
                            GenSpace = Result["GenSpace"].ToString(),
                            StorageCharge = Result["sto"].ToString(),
                            Insurance = Result["Insurance"].ToString(),
                            GroundRent = Result["GroundRent"].ToString(),
                            RFIDCharge = Result["RFID"].ToString(),
                            WeighmentCharge = Result["Wgmnt"].ToString(),
                            FacilitationCharge = Result["FcChrg"].ToString(),
                            DocumentCharge = Result["DocChrg"].ToString(),
                            AggregationCharge = Result["AggChrg"].ToString(),
                            HTCharge = Result["HT"].ToString(),
                            OtherCharge = Result["OtCharge"].ToString(),
                            Cgst = Result["CGSTAmt"].ToString(),
                            Sgst = Result["SGSTAmt"].ToString(),
                            Igst = Result["IGSTAmt"].ToString(),
                            Misc = Result["MISC"].ToString(),
                            //MiscExcess = Result["MiscExcess"].ToString(),
                            TotalCash = Result["TotalCash"].ToString(),
                            TotalCheque = Result["TotalCheque"].ToString(),
                            Tds = Result["tdsCol"].ToString(),
                            CrTds = Result["crTDS"].ToString()*/
                        //TDSPlus = Result["TDSPlus"].ToString(),
                        //Exempted = Result["Exempted"].ToString(),
                        //PdaPLus = Result["PdaPLus"].ToString(),
                        //TDSMinus = Result["TDSMinus"].ToString(),
                        //PdaMinus = Result["PdaMinus"].ToString(),
                        //HtAdjust = Result["HtAdjust"].ToString(),
                        //RoundOff = Result["RoundUp"].ToString(),
                        //RowTotal = Result["Total"].ToString()


                        //Party = Result["Party"].ToString(),
                        //Deposit = Result["Deposit"].ToString(),
                        //Withdraw = Result["Withdraw"].ToString()
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        InvoiceType = Result["InvoiceType"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),
                        ModeOfPay = Result["ModeOfPay"].ToString(),

                        ChqNo = Result["ChequeNo"].ToString(),
                        GenSpace = Result["GenSpace"].ToString(),
                        StorageCharge = Result["sto"].ToString(),
                        Insurance = Result["Insurance"].ToString(),
                        GroundRentEmpty = Result["GroundRentEmpty"].ToString(),
                        GroundRentLoaded = Result["GroundRentLoaded"].ToString(),
                        MfCharge = Result["Mf"].ToString(),
                        EntryCharge = Result["EntCharge"].ToString(),
                        Fumigation = Result["Fum"].ToString(),
                        OtherCharge = Result["OtCharge"].ToString(),
                        Misc = Result["MISC"].ToString(),
                        Cgst = Result["CGSTAmt"].ToString(),
                        Sgst = Result["SGSTAmt"].ToString(),
                        Igst = Result["IGSTAmt"].ToString(),

                        MiscExcess = Result["MiscExcess"].ToString(),
                        TotalCash = Result["TotalCash"].ToString(),
                        TotalCheque = Result["TotalCheque"].ToString(),
                        TotalOthers = Result["TotalOther"].ToString(),
                        Tds = Result["tdsCol"].ToString(),
                        CrTds = Result["crTDS"].ToString(),
                        TotalPDA = Result["TotalPDA"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDailyCashBook;
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

        #endregion

        #region Monthly Cash Book With SD

        public void MonthlyCashBookWithSD(Chn_DailyCashBookDtl ObjDailyCashBook)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyCashBook.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyCashBook.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("MonthlyCashBookReportWithSD", CommandType.StoredProcedure, DParam);
            IList<Chn_DailyCashBookDtl> LstMonthlyCashBook = new List<Chn_DailyCashBookDtl>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //ObjStatusDtl = new ConsumerList();
                    //ObjStatusDtl.RegistrationNo = Convert.ToString(Result["RegistrationNo"]);
                    //ObjStatusDtl.Name = Convert.ToString(Result["CompanyName"]);
                    //ObjStatusDtl.Address = Convert.ToString(Result["CompanyAddress"]);
                    //ObjStatusDtl.IssueDate = Convert.ToString(Result["IssuedOn"]);


                    //                 InvoiceDate DATE,
                    //         InvoiceNumber       VARCHAR(30),
                    //ReceiptAmount DECIMAL(18, 3),
                    //InvoiceAmount DECIMAL(18, 3),
                    //Value DECIMAL(18, 3),
                    //OpeningBalance DECIMAL(18, 3),
                    //ClosingBalance DECIMAL(18, 3)

                    LstMonthlyCashBook.Add(new Chn_DailyCashBookDtl
                    {
                        //CRNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),

                        //Depositor = Result["Party"].ToString(),
                        //ChqNo = Result["ChequeNo"].ToString(),
                        GenSpace = Result["GenSpace"].ToString(),
                        StorageCharge = Result["sto"].ToString(),
                        Insurance = Result["Insurance"].ToString(),
                        GroundRent = Result["GroundRent"].ToString(),
                        RFIDCharge = Result["RFID"].ToString(),
                        WeighmentCharge = Result["Weighment"].ToString(),
                        FacilitationCharge = Result["Facilitation"].ToString(),
                        DocumentCharge = Result["DocChrg"].ToString(),
                        AggregationCharge = Result["AggChrg"].ToString(),
                        HTCharge = Result["HT"].ToString(),
                        OtherCharge = Result["OtCharge"].ToString(),
                        Cgst = Result["CGSTAmt"].ToString(),
                        Sgst = Result["SGSTAmt"].ToString(),
                        Igst = Result["IGSTAmt"].ToString(),
                        Misc = Result["MISC"].ToString(),
                        //MiscExcess = Result["MiscExcess"].ToString(),
                        TotalCash = Result["TotalCash"].ToString(),
                        TotalCheque = Result["TotalCheque"].ToString(),
                        Tds = Result["tdsCol"].ToString(),
                        CrTds = Result["crTDS"].ToString()

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstMonthlyCashBook;
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

        #endregion
        #region Destuffing Detail Report
        public void GetAllPartyForDetsuffing(string PartyCode, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPartyForDestuffingReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PartyForChnSDDet> LstParty = new List<PartyForChnSDDet>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstParty.Add(new PartyForChnSDDet
                    {
                        Party = Result["Party"].ToString(),
                        PartyId = Convert.ToInt32(Result["PartyId"]),
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
                    _DBResponse.Data = new { LstParty, State };
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

        public void GetDestuffingDetailReport(string FromDate, string ToDate, int PartyId, string PartyName)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });

            DParam = LstParam.ToArray();
            DataSet ds = DataAccess.ExecuteDataSet("DestuffingReportDetail", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            CHN_DestuffingDetail BondDaily = new CHN_DestuffingDetail();

            try
            {
                int srno = 0;
                int depsrno = 0;
                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    srno = srno + 1;
                    BondDaily.lstchn_destuffingrpt.Add(new chn_destuffingrpt
                    {
                        serialNo = srno,
                        ContainerNo = Convert.ToString(dr1["ContainerNo"]),
                        Size = Convert.ToString(dr1["Size"]),
                        ArrivalDate = Convert.ToString(dr1["ArrivalDate"]),
                        DestuffingDate = Convert.ToString(dr1["DestuffingDate"]),
                       
                    });
                }
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    srno = srno + 1;
                    BondDaily.lstDestuffingdetail.Add(new CHN_DestuffingDetailReport
                    {
                        //serialNo = srno,
                        ContainerNo = Convert.ToString(dr["ContainerNo"]),
                       // Size = Convert.ToString(dr["Size"]),
                        //ArrivalDate = Convert.ToString(dr["ArrivalDate"]),
                       // DestuffingDate = Convert.ToString(dr["DestuffingDate"]),
                        OBLNo = Convert.ToString(dr["OBLNo"]),
                        LineNo = Convert.ToString(dr["LineNo"]),
                        BOENo = Convert.ToString(dr["BOENo"]),
                        DeliveryDate = Convert.ToString(dr["DeliveryDate"]),
                        NoOfGrid = Convert.ToInt32(dr["NoOfGrid"]),
                        StorageDays = Convert.ToInt32(dr["StorageDays"]),
                        InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                        StorageCharge = Convert.ToDecimal(dr["StorageCharge"]),
                        DocumentationCharge = Convert.ToDecimal(dr["DocumentationCharge"]),
                        FacilitationCharge = Convert.ToDecimal(dr["FacilitationCharge"]),
                        AggregationCharge = Convert.ToDecimal(dr["AggregationCharge"]),
                        Weight = Convert.ToDecimal(dr["Weight"]),
                    });
                }

            



                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = GetDailyTransactionReportBondExcel(BondDaily.lstchn_destuffingrpt, BondDaily.lstDestuffingdetail, FromDate, ToDate,PartyName);
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {

                ds.Dispose();

            }
        }

        private string GetDailyTransactionReportBondExcel(List<chn_destuffingrpt> DDetail,List<CHN_DestuffingDetailReport> DestuffingDetail, string FromDate, string ToDate, string PartyName)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");

            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION";
             
                exl.MargeCell("A1:O1", title, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A2:O2", "(A Govt.of India Undertaking)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A3:O3", "CFS,Madhavaram-Chennai", DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A4:O4", "From Date: " + FromDate, DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A5:O5", "To Date " + ToDate, DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A6:O6", "Party Name"+ PartyName, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A7:O7", "Destuffing Report", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("A8:A8", "S.No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B8:B8", "ContainerNumber/ CBT No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C8:C8", "Size", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D8:D8", "Date of Arrival", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E8:E8", "Date of destuffing", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F8:F8", "OBL NO.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G8:G8", "Line Number", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H8:H8", "BoE No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I8:I8", "Date of Delivery", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J8:J8", "No. of Grid", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K8:K8", "Storage days", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("L8:L8", "Invoice Number ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("M8:M8", "Storage charge", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("N8:N8", "Documentation Charges", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("O8:O8", "Facilitation Charges", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("P8:P8", "Aggregation Charges", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("Q8:Q8", "Weight(Kgs)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                try
                {
                    var ln = 0;
                    int TCOBLNo = 0; int TCBOENo = 0; decimal TTStorageCharge = 0; decimal TTDocumentationCharge = 0; decimal TTFacilitationCharge = 0; decimal TTAggregationCharge = 0; decimal TTWeight = 0;
                    for (int i = 0; i < DDetail.Count(); i++)
                    {
                        int COBLNo = 0; int CBOENo= 0; decimal TStorageCharge = 0; decimal TDocumentationCharge = 0; decimal TFacilitationCharge = 0; decimal TAggregationCharge = 0; decimal TWeight = 0;

                        exl.AddCell("A" + (9+ln).ToString(), DDetail[i].serialNo, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("B" + (9+ln).ToString(), DDetail[i].ContainerNo, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("C" + (9+ln).ToString(), DDetail[i].Size, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("D" + (9+ln).ToString(), DDetail[i].ArrivalDate, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("E" + (9+ln).ToString(), DDetail[i].DestuffingDate, DynamicExcel.CellAlignment.CenterRight);
                        for (int j = 0; j < DestuffingDetail.Count(); j++)
                        {
                            if (DDetail[i].ContainerNo == DestuffingDetail[j].ContainerNo)
                            {
                                exl.AddCell("F" + (9 + ln).ToString(), DestuffingDetail[j].OBLNo, DynamicExcel.CellAlignment.CenterRight);
                                exl.AddCell("G" + (9 + ln).ToString(), DestuffingDetail[j].LineNo, DynamicExcel.CellAlignment.CenterRight);
                                exl.AddCell("H" + (9 + ln).ToString(), DestuffingDetail[j].BOENo, DynamicExcel.CellAlignment.CenterRight);
                                exl.AddCell("I" + (9 + ln).ToString(), DestuffingDetail[j].DeliveryDate, DynamicExcel.CellAlignment.CenterRight);
                                exl.AddCell("J" + (9 + ln).ToString(), DestuffingDetail[j].NoOfGrid, DynamicExcel.CellAlignment.CenterRight);
                                exl.AddCell("K" + (9 + ln).ToString(), DestuffingDetail[j].StorageDays, DynamicExcel.CellAlignment.CenterRight);
                                exl.AddCell("L" + (9 + ln).ToString(), DestuffingDetail[j].InvoiceNo, DynamicExcel.CellAlignment.CenterRight);
                                exl.AddCell("M" + (9 + ln).ToString(), DestuffingDetail[j].StorageCharge, DynamicExcel.CellAlignment.CenterRight);
                                exl.AddCell("N" + (9 + ln).ToString(), DestuffingDetail[j].DocumentationCharge, DynamicExcel.CellAlignment.CenterRight);
                                exl.AddCell("O" + (9 + ln).ToString(), DestuffingDetail[j].FacilitationCharge, DynamicExcel.CellAlignment.CenterRight);
                                exl.AddCell("P" + (9 + ln).ToString(), DestuffingDetail[j].AggregationCharge, DynamicExcel.CellAlignment.CenterRight);
                                exl.AddCell("Q" + (9 + ln).ToString(), DestuffingDetail[j].Weight, DynamicExcel.CellAlignment.CenterRight);

                                COBLNo += 1;
                                if (DestuffingDetail[j].BOENo != string.Empty)
                                CBOENo += 1;
                                TStorageCharge += DestuffingDetail[j].StorageCharge;
                                TDocumentationCharge += DestuffingDetail[j].DocumentationCharge;
                                TFacilitationCharge += DestuffingDetail[j].FacilitationCharge;
                                TAggregationCharge += DestuffingDetail[j].AggregationCharge;
                                TWeight += DestuffingDetail[j].Weight;
                            
                                TCOBLNo += 1;
                                if (DestuffingDetail[j].BOENo != string.Empty)
                                TCBOENo += 1;
                                TTStorageCharge += DestuffingDetail[j].StorageCharge;
                                TTDocumentationCharge += DestuffingDetail[j].DocumentationCharge;
                                TTFacilitationCharge += DestuffingDetail[j].FacilitationCharge;
                                TTAggregationCharge += DestuffingDetail[j].AggregationCharge;
                                TTWeight += DestuffingDetail[j].Weight;

                                ln = ln + 1;
                            }
                        }

                        //-------------
                        exl.AddCell("E" + (9 + ln).ToString(), "No of OBL / BOE", DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("F" + (9 + ln).ToString(), COBLNo, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("G" + (9 + ln).ToString(), "", DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("H" + (9 + ln).ToString(), CBOENo, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("I" + (9 + ln).ToString(), "Total:", DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("J" + (9 + ln).ToString(), "", DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("K" + (9 + ln).ToString(), "", DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("L" + (9 + ln).ToString(), "", DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("M" + (9 + ln).ToString(), TStorageCharge, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("N" + (9 + ln).ToString(), TDocumentationCharge, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("O" + (9 + ln).ToString(), TFacilitationCharge, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("P" + (9 + ln).ToString(), TAggregationCharge, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("Q" + (9 + ln).ToString(), TWeight, DynamicExcel.CellAlignment.CenterRight);
                        //------------
                        ln = ln + 1;
                    }
                    //-------------
                    exl.AddCell("E" + (9 + ln).ToString(), "No of OBL / BOE", DynamicExcel.CellAlignment.CenterRight);
                    exl.AddCell("F" + (9 + ln).ToString(), TCOBLNo, DynamicExcel.CellAlignment.CenterRight);
                    exl.AddCell("G" + (9 + ln).ToString(), "", DynamicExcel.CellAlignment.CenterRight);
                    exl.AddCell("H" + (9 + ln).ToString(), TCBOENo, DynamicExcel.CellAlignment.CenterRight);
                    exl.AddCell("I" + (9 + ln).ToString(), "Gross Total:", DynamicExcel.CellAlignment.CenterRight);
                    exl.AddCell("J" + (9 + ln).ToString(), "", DynamicExcel.CellAlignment.CenterRight);
                    exl.AddCell("K" + (9 + ln).ToString(), "", DynamicExcel.CellAlignment.CenterRight);
                    exl.AddCell("L" + (9 + ln).ToString(), "", DynamicExcel.CellAlignment.CenterRight);
                    exl.AddCell("M" + (9 + ln).ToString(), TTStorageCharge, DynamicExcel.CellAlignment.CenterRight);
                    exl.AddCell("N" + (9 + ln).ToString(), TTDocumentationCharge, DynamicExcel.CellAlignment.CenterRight);
                    exl.AddCell("O" + (9 + ln).ToString(), TTFacilitationCharge, DynamicExcel.CellAlignment.CenterRight);
                    exl.AddCell("P" + (9 + ln).ToString(), TTAggregationCharge, DynamicExcel.CellAlignment.CenterRight);
                    exl.AddCell("Q" + (9 + ln).ToString(), TTWeight, DynamicExcel.CellAlignment.CenterRight);
                    //------------
                    ln = ln + 1;

                    //exl.AddTable<chn_test>("A", 9, DDetail, new[] { 6, 20, 20, 20, 12});
                    //exl.AddTable<CHN_DestuffingDetailReport>("F", 9, DestuffingDetail, new[] { 20,20, 10, 15, 20, 12, 12, 8, 14, 20, 10,20,20 });


                }
                catch (Exception ex)
                {

                }

               /* var Units = BondDailyTransaction.lstBondDepositeTransaction.Sum(o => o.Noofpkg);
                var Weight = BondDailyTransaction.lstBondDepositeTransaction.Sum(o => o.Weight);
                exl.AddCell("I" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 9).ToString(), "TOTAL", DynamicExcel.CellAlignment.CenterLeft);

                exl.AddCell("J" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 9).ToString(), Units.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("K" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 9).ToString(), Weight.ToString(), DynamicExcel.CellAlignment.CenterRight);

                try
                {


                    exl.MargeCell("A" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 10).ToString() + ":O" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 10).ToString() + "", "Delivery", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                    exl.MargeCell("A" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + ":A" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + "", "S.No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("B" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + ":B" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + "", "Godown No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("C" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + ":C" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + "", "Delivery Order No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("D" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + ":D" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + "", "Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("E" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + ":E" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + "", "Importer", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("F" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + ":F" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + "", "CHA", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("G" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + ":G" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + "", "Description of Cargo", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("H" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + ":H" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + "", "Bond Number / Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("I" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + ":I" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + "", "Ex Bond BOE No. / Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("J" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + ":J" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + "", "No. of Packages/Units", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("K" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + ":K" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + "", "Weight (Kgs)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("L" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + ":L" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + "", "Value ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("M" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + ":M" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + "", "Duty", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("N" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + ":N" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + "", "Gross Area occupied in m2", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("O" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + ":O" + (BondDailyTransaction.lstBondDepositeTransaction.Count + 11).ToString() + "", "Invoice No. & Date / Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);


                    exl.AddTable<Hdb_BondDeliverDailyTransactionReport>("A", BondDailyTransaction.lstBondDepositeTransaction.Count + 12, BondDailyTransaction.lstBondDeliveryTransaction, new[] { 6, 20, 20, 20, 12, 20, 10, 15, 20, 12, 12, 8, 14, 20, 10 });

                    var deliUnits = BondDailyTransaction.lstBondDeliveryTransaction.Sum(o => o.Noofpkg);
                    var deliWeight = BondDailyTransaction.lstBondDeliveryTransaction.Sum(o => o.Weight);
                    exl.AddCell("I" + (BondDailyTransaction.lstBondDepositeTransaction.Count + BondDailyTransaction.lstBondDeliveryTransaction.Count + 12).ToString(), "TOTAL", DynamicExcel.CellAlignment.CenterLeft);

                    exl.AddCell("J" + (BondDailyTransaction.lstBondDepositeTransaction.Count + BondDailyTransaction.lstBondDeliveryTransaction.Count + 12).ToString(), Units.ToString(), DynamicExcel.CellAlignment.CenterRight);
                    exl.AddCell("K" + (BondDailyTransaction.lstBondDepositeTransaction.Count + BondDailyTransaction.lstBondDeliveryTransaction.Count + 12).ToString(), Weight.ToString(), DynamicExcel.CellAlignment.CenterRight);
                }
                catch (Exception ex)
                {

                }
                exl.MargeCell("A" + (BondDailyTransaction.lstBondDepositeTransaction.Count + BondDailyTransaction.lstBondDeliveryTransaction.Count + 13).ToString() + ":O" + (BondDailyTransaction.lstBondDepositeTransaction.Count + BondDailyTransaction.lstBondDeliveryTransaction.Count + 13).ToString() + "", " ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A" + (BondDailyTransaction.lstBondDepositeTransaction.Count + BondDailyTransaction.lstBondDeliveryTransaction.Count + 14).ToString() + ":O" + (BondDailyTransaction.lstBondDepositeTransaction.Count + BondDailyTransaction.lstBondDeliveryTransaction.Count + 14).ToString() + "", " ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);


                exl.MargeCell("B" + (BondDailyTransaction.lstBondDepositeTransaction.Count + BondDailyTransaction.lstBondDeliveryTransaction.Count + 15).ToString() + ":C" + (BondDailyTransaction.lstBondDepositeTransaction.Count + BondDailyTransaction.lstBondDeliveryTransaction.Count + 15).ToString() + "", "Signature of I/C Godown No. ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("M" + (BondDailyTransaction.lstBondDepositeTransaction.Count + BondDailyTransaction.lstBondDeliveryTransaction.Count + 15).ToString() + ":N" + (BondDailyTransaction.lstBondDepositeTransaction.Count + BondDailyTransaction.lstBondDeliveryTransaction.Count + 15).ToString() + "", "Signature of Bond I/C", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

    */
                exl.Save();
            }



            return excelFile;
        }

        #endregion

        #region E04 Report

        public void ListofE04Report(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofE04Report", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_E04Report> LstE04 = new List<Chn_E04Report>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstE04.Add(new Chn_E04Report
                    {
                        ID = Convert.ToInt32(Result["ID"]),
                        CUSTOM_CD = Result["CUSTOM_CD"].ToString(),
                        SB_NO = Result["SB_NO"].ToString(),
                        SB_DATE = Result["SB_DATE"].ToString(),
                        IEC_CD = Result["IEC_CD"].ToString(),
                        BI_NO = Result["BI_NO"].ToString(),
                        EXP_NAME = Result["EXP_NAME"].ToString(),
                        Address = Result["Address"].ToString(),
                        CHA_CODE = Result["CHA_CODE"].ToString(),
                        FOB = Result["FOB"].ToString(),
                        POD = Result["POD"].ToString(),
                        LEO_NO = Result["LEO_NO"].ToString(),
                        LEO_DATE = Result["LEO_DATE"].ToString(),
                        ENTRY_NO = Result["ENTRY_NO"].ToString(),
                        G_DATE = Result["G_DATE"].ToString(),
                        TRANSHIPPER_CODE = Result["TRANSHIPPER_CODE"].ToString(),
                        GATEWAY_PORT = Result["GATEWAY_PORT"].ToString(),
                        PCIN = Result["PCIN"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstE04;
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

        public void GetE04DetailById(int ID)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ID", MySqlDbType = MySqlDbType.Int32, Value = ID });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("ViewE04Details", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Chn_E04Report objE04Report = new Chn_E04Report();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    objE04Report.ID = Convert.ToInt32(Result["ID"]);
                    objE04Report.CUSTOM_CD = Result["CUSTOM_CD"].ToString();
                    objE04Report.SB_NO = Result["SB_NO"].ToString();
                    objE04Report.SB_DATE = Result["SB_DATE"].ToString();
                    objE04Report.IEC_CD = Result["IEC_CD"].ToString();
                    objE04Report.BI_NO = Result["BI_NO"].ToString();
                    objE04Report.EXP_NAME = Result["EXP_NAME"].ToString();
                    objE04Report.EXP_ADD1 = Result["EXP_ADD1"].ToString();
                    objE04Report.EXP_ADD2 = Result["EXP_ADD2"].ToString();
                    objE04Report.PIN = Result["PIN"].ToString();
                    objE04Report.CITY = Result["CITY"].ToString();
                    objE04Report.CHA_CODE = Result["CHA_CODE"].ToString();
                    objE04Report.FOB = Result["FOB"].ToString();
                    objE04Report.POD = Result["POD"].ToString();
                    objE04Report.LEO_NO = Result["LEO_NO"].ToString();
                    objE04Report.LEO_DATE = Result["LEO_DATE"].ToString();
                    objE04Report.ENTRY_NO = Result["ENTRY_NO"].ToString();
                    objE04Report.G_DATE = Result["G_DATE"].ToString();
                    objE04Report.TRANSHIPPER_CODE = Result["TRANSHIPPER_CODE"].ToString();
                    objE04Report.GATEWAY_PORT = Result["GATEWAY_PORT"].ToString();
                    objE04Report.PCIN = Result["PCIN"].ToString();
                }

                if (Status == 1)
                {
                    _DBResponse.Data = objE04Report;
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

        public void GetE04DetailSearch(string SB_No, string SB_Date, string Exp_Name)
        {
            //string SBDate ="";
            //if (SB_Date != null && SB_Date != "" )
            //{
            //    DateTime SBDt = DateTime.ParseExact(SB_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //    SBDate = SBDt.ToString("yyyy-MM-dd");
            //}            

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SBNo", MySqlDbType = MySqlDbType.VarChar, Value = SB_No });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Exporter", MySqlDbType = MySqlDbType.VarChar, Value = Exp_Name });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SBDate", MySqlDbType = MySqlDbType.VarChar, Value = SB_Date });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofE04Search", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_E04Report> LstE04Report = new List<Chn_E04Report>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstE04Report.Add(new Chn_E04Report
                    {
                        ID = Convert.ToInt32(Result["ID"]),
                        CUSTOM_CD = Result["CUSTOM_CD"].ToString(),
                        SB_NO = Result["SB_NO"].ToString(),
                        SB_DATE = Result["SB_DATE"].ToString(),
                        IEC_CD = Result["IEC_CD"].ToString(),
                        BI_NO = Result["BI_NO"].ToString(),
                        EXP_NAME = Result["EXP_NAME"].ToString(),
                        Address = Result["Address"].ToString(),
                        CHA_CODE = Result["CHA_CODE"].ToString(),
                        FOB = Result["FOB"].ToString(),
                        POD = Result["POD"].ToString(),
                        LEO_NO = Result["LEO_NO"].ToString(),
                        LEO_DATE = Result["LEO_DATE"].ToString(),
                        ENTRY_NO = Result["ENTRY_NO"].ToString(),
                        G_DATE = Result["G_DATE"].ToString(),
                        TRANSHIPPER_CODE = Result["TRANSHIPPER_CODE"].ToString(),
                        GATEWAY_PORT = Result["GATEWAY_PORT"].ToString(),
                        PCIN = Result["PCIN"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstE04Report;
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
        #region Stuffing Acknowledgement Search       

        public void GetAllContainerNoForContstufserach(string cont, int Page = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_cont", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = cont });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = '0' });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerNoForstufack", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            // List<StuffingRequestDtl> LstStuffing = new List<StuffingRequestDtl>();
            List<Chn_ContStufAckSearch> LstStuffing = new List<Chn_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Chn_ContStufAckSearch
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
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
                    _DBResponse.Data = new { LstStuffing, State };
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
        public void GetAllShippingBillNoForContstufserach(string shipbill, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_shipbill", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = shipbill });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = '0' });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetshippingbillNoForstufack", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            // List<StuffingRequestDtl> LstStuffing = new List<StuffingRequestDtl>();
            List<Chn_ContStufAckSearch> LstStuff = new List<Chn_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuff.Add(new Chn_ContStufAckSearch
                    {
                        shippingbillno = Result["ShippingBillNo"].ToString(),
                        shippingbilldate = Result["ShippingBillDate"].ToString(),
                        // ShippingLine = Result["ShippingLine"].ToString()
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
                    _DBResponse.Data = new { LstStuff, State };
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
        public void GetStufAckResult(string container, string shipbill, string cfscode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_container", MySqlDbType = MySqlDbType.VarChar, Value = container });
            LstParam.Add(new MySqlParameter { ParameterName = "in_shipbill", MySqlDbType = MySqlDbType.VarChar, Value = shipbill });
            LstParam.Add(new MySqlParameter { ParameterName = "in_cfscode", MySqlDbType = MySqlDbType.VarChar, Value = cfscode });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStufAckResult", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_ContStufAckRes> Lststufack = new List<Chn_ContStufAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Chn_ContStufAckRes
                    {
                        shipbill = Result["shipbill"].ToString(),
                        reason = Result["reason"].ToString(),
                        status = Result["status"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lststufack;
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

        #region SBQueryReport
        public void GetAllSB()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShipBillListForSBQuery", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHN_SBQuery> LstSB = new List<CHN_SBQuery>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new CHN_SBQuery
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        SBNODate = Result["SBNODate"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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
                Result.Dispose();
                Result.Close();
            }

        }

        public void SBQueryReport(int id, string sbno, string sbdate)
        {


            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SBNO", MySqlDbType = MySqlDbType.VarChar, Value = sbno });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SBDate", MySqlDbType = MySqlDbType.Date, Value = sbdate });

            int Status = 0;

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSBQueryDetails", CommandType.StoredProcedure, DParam);
            CHN_SBQuery LstSBQueryReport = new CHN_SBQuery();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSBQueryReport.Id = Convert.ToInt32(Result["Id"]);
                    LstSBQueryReport.SBNO = Convert.ToString(Result["SBNO"]);
                    LstSBQueryReport.PortOFDischarge = Convert.ToString(Result["PortOfDischarge"]);
                    LstSBQueryReport.PortOFLoad = Convert.ToString(Result["PortName"]);
                    LstSBQueryReport.ShippingLine = Convert.ToString(Result["ShippingLine"]);
                    LstSBQueryReport.Comodity = Convert.ToString(Result["Comodity"]);
                    LstSBQueryReport.CHA = Convert.ToString(Result["CHA"]);
                    LstSBQueryReport.Date = Convert.ToString(Result["Date"]);
                    //LstSBQueryReport.PortOFDischarge = Convert.ToString(Result["Id"]);
                    LstSBQueryReport.Package = Convert.ToInt32(Result["Package"]);
                    LstSBQueryReport.Weight = Convert.ToDecimal(Result["Weight"]);
                    LstSBQueryReport.FOB = Convert.ToDecimal(Result["FOB"]);
                    LstSBQueryReport.Cargotype = Convert.ToInt32(Result["CargoType"]);
                    LstSBQueryReport.Vehicle = Convert.ToString(Result["NoOfVehicle"]);
                    LstSBQueryReport.Exporter = Convert.ToString(Result["Exporter"]);
                    LstSBQueryReport.Country = Convert.ToString(Result["Country"]);
                    LstSBQueryReport.GateinNo = Convert.ToString(Result["GateInNo"]);
                    LstSBQueryReport.PCIN = Convert.ToString(Result["PCIN"]);
                    //LstSBQueryReport.InvoiceNo = Convert.ToString(Result["InvoiceNo"]);

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSBQueryReport;
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
        #endregion
        #region Stuffing ASR Acknowledgement Search       

        public void GetCotainerNoForASRAck(string in_cont, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_cont", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = in_cont });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCotainerNoForASRAckStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_ContStufAckSearch> LstStuff = new List<Chn_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuff.Add(new Chn_ContStufAckSearch
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
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
                    _DBResponse.Data = new { LstStuff, State };
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
        public void GetAllShippingBillNoForASRACK(string shipbill, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingBillNo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = shipbill });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetshippingbillNoForASRAckStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_ContStufAckSearch> LstStuff = new List<Chn_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuff.Add(new Chn_ContStufAckSearch
                    {
                        shippingbillno = Result["ShippingBillNo"].ToString(),
                        shippingbilldate = Result["ShippingBillDate"].ToString(),
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
                    _DBResponse.Data = new { LstStuff, State };
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
        public void GetASRAckResult(string shipbill, string CFSCode, string container)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_container", MySqlDbType = MySqlDbType.VarChar, Value = container });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_shipbill", MySqlDbType = MySqlDbType.VarChar, Value = shipbill });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetASRAckResult", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_ContStufAckRes> Lststufack = new List<Chn_ContStufAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Chn_ContStufAckRes
                    {
                        shipbill = Result["shipbill"].ToString(),
                        reason = Result["reason"].ToString(),
                        status = Result["status"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lststufack;
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

        #region DP Acknowledment Serach  

        public void GetGatePassNoDPForAckSearch(string GatePassNo, int Page = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassNo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = GatePassNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGatePassNoDPForAckSearch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_GatePassDPAckSearch> lstDPGPAck = new List<Chn_GatePassDPAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDPGPAck.Add(new Chn_GatePassDPAckSearch
                    {
                        GatePassId = Convert.ToInt32(Result["GatePassId"]),
                        GatePassNo = Convert.ToString(Result["GatePassNo"])
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
                    _DBResponse.Data = new { lstDPGPAck, State };
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
        public void GetContainerNoForDPAckSearch(string ContainerNo, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerNoForDPAckSearch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            List<Chn_ContDPAckSearch> lstDPContACK = new List<Chn_ContDPAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDPContACK.Add(new Chn_ContDPAckSearch
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        GatePassdtlId = Convert.ToInt32(Result["GatePassdtlId"]),
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
                    _DBResponse.Data = new { lstDPContACK, State };
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
        public void GetDPAckSearch(int GatePassId, string ContainerNo, int GatePassDetailId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.VarChar, Value = GatePassId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassDetailId", MySqlDbType = MySqlDbType.VarChar, Value = GatePassDetailId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDPAckResult", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_DPAckRes> Lststufack = new List<Chn_DPAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Chn_DPAckRes
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Path = Result["Path"].ToString(),
                        Reason = Result["Reason"].ToString(),
                        Status = Result["Status"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lststufack;
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

        #region DT Acknowledment Serach  

        public void GetGatePassNoDTForAckSearch(string GatePassNo, int Page = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassNo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = GatePassNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGatePassNoDTForAckSearch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_GatePassDTAckSearch> lstDTGPAck = new List<Chn_GatePassDTAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDTGPAck.Add(new Chn_GatePassDTAckSearch
                    {
                        GatePassId = Convert.ToInt32(Result["GatePassId"]),
                        GatePassNo = Convert.ToString(Result["GatePassNo"])
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
                    _DBResponse.Data = new { lstDTGPAck, State };
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
        public void GetContainerNoForDTAckSearch(string ContainerNo, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerNoForDTAckSearch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            List<Chn_ContDTAckSearch> lstDTContACK = new List<Chn_ContDTAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDTContACK.Add(new Chn_ContDTAckSearch
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
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
                    _DBResponse.Data = new { lstDTContACK, State };
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
        public void GetDTAckSearch(int GatePassId, string ContainerNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = GatePassId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDTAckResult", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Chn_DTAckRes> Lststufack = new List<Chn_DTAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Chn_DTAckRes
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Reason = Result["Reason"].ToString(),
                        Status = Result["Status"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lststufack;
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

        #region Stuffing Loaded Search       


        public void GetStufloadResult(string jobno)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_jobno", MySqlDbType = MySqlDbType.VarChar, Value = jobno });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_shipbill", MySqlDbType = MySqlDbType.VarChar, Value = shipbill });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_cfscode", MySqlDbType = MySqlDbType.VarChar, Value = cfscode });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getstufloadno", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHN_loadstuf> Lststufack = new List<CHN_loadstuf>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new CHN_loadstuf
                    {
                        loadstufreqno = Result["loadreqno"].ToString(),
                        expstufreqno = Result["stufreqno"].ToString(),
                        // reason = Result["reason"].ToString(),
                        // status = Result["status"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lststufack;
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
        public void GetStufloadasrResult(string jobasrno)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_asrjobno", MySqlDbType = MySqlDbType.VarChar, Value = jobasrno });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_shipbill", MySqlDbType = MySqlDbType.VarChar, Value = shipbill });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_cfscode", MySqlDbType = MySqlDbType.VarChar, Value = cfscode });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getstufloadasrno", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHN_loadstufasr> Lststufack = new List<CHN_loadstufasr>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new CHN_loadstufasr
                    {
                        loadstufasrreqno = Result["loadasrreqno"].ToString(),
                        expstufasrreqno = Result["stufasrreqno"].ToString(),
                        // reason = Result["reason"].ToString(),
                        // status = Result["status"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lststufack;
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
        public void GetStufloaddpResult(string jobdpno)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_dpjobno", MySqlDbType = MySqlDbType.VarChar, Value = jobdpno });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_shipbill", MySqlDbType = MySqlDbType.VarChar, Value = shipbill });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_cfscode", MySqlDbType = MySqlDbType.VarChar, Value = cfscode });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getstufloaddpno", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHN_loadstufdp> Lststufack = new List<CHN_loadstufdp>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new CHN_loadstufdp
                    {
                        loadstufdpreqno = Result["loaddpreqno"].ToString(),
                        expstufdpreqno = Result["stufdpreqno"].ToString(),
                        // reason = Result["reason"].ToString(),
                        // status = Result["status"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lststufack;
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
        public void GetStufloaddtResult(string jobdtno)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_dtjobno", MySqlDbType = MySqlDbType.VarChar, Value = jobdtno });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_shipbill", MySqlDbType = MySqlDbType.VarChar, Value = shipbill });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_cfscode", MySqlDbType = MySqlDbType.VarChar, Value = cfscode });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getstufloaddtno", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHN_loadstufdt> Lststufack = new List<CHN_loadstufdt>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new CHN_loadstufdt
                    {
                        loadstufdtreqno = Result["loaddtreqno"].ToString(),
                        expstufdtreqno = Result["stufdtreqno"].ToString(),
                        // reason = Result["reason"].ToString(),
                        // status = Result["status"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lststufack;
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

        #region Bond Storage Collection Register
        public void BondStorageCollectionReport(string FromDate, string ToDate)
        {

            FromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            ToDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"); ;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDt", MySqlDbType = MySqlDbType.DateTime, Value = FromDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDt", MySqlDbType = MySqlDbType.DateTime, Value = ToDate });
            DParam = LstParam.ToArray();
            DataSet ds = DataAccess.ExecuteDataSet("getBondStorageCollectionRpt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ds;
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
        }
        #endregion

        #region Import Laden Container Arrival Report
        public void ImportLadenContainerArrivalexcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetImportLadenContainerArrivalReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Status = 1;
            try
            {

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Result;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data Found";
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
                //  Result.Close();
            }
        }
        #endregion

        #region Empty Container Despatch Report
        public void EmptyContainerDespatchReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetEmptyContainerDespatchReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Status = 1;
            try
            {

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Result;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data Found";
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
                //  Result.Close();
            }
        }
        #endregion

        #region Factory Destuffing Report
        public void FactoryDestuffingexcelReport(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetFactoryDestuffingReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Status = 1;
            try
            {

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Result;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data Found";
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
                //  Result.Close();
            }
        }
        #endregion

        #region Import Destuff Cargo Report
        public void ImportDestuffCargoexcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetImportDestuffCargoReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Status = 1;
            try
            {

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Result;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data Found";
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
                //  Result.Close();
            }
        }
        #endregion

        #region Import YardStock Report
        public void ImportYardStockexcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetImportYardStockReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Status = 1;
            try
            {

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Result;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data Found";
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
                //  Result.Close();
            }
        }
        #endregion
    }
}