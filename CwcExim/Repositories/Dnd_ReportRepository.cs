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
using Newtonsoft.Json;


namespace CwcExim.Repositories
{
    public class Dnd_ReportRepository
    {

        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

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

        #region Daily Pda Activity Report
        public void DailyPdaActivity(DailyPdaActivityReport ObjDailyPdaActivityReport,bool Active)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_Active", MySqlDbType = MySqlDbType.Int32, Value = Active ? 1 : 0 });//in_OperationType

            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailySdActivityReport", CommandType.StoredProcedure, DParam);
            IList<DailyPdaActivityReport> LstDailyPdaActivityReport = new List<DailyPdaActivityReport>();
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

                    LstDailyPdaActivityReport.Add(new DailyPdaActivityReport
                    {
                        //  Date = Result["Date"].ToString(),
                        // ReceiptNo = Result["ReceiptNo"].ToString(),
                        partycode = Result["PartyCode"].ToString(),
                        Party = Result["PartyName"].ToString(),
                        Opening = Convert.ToDecimal(Result["OpeningAmount"]),
                        Deposit = Result["DebitAmount"].ToString(),
                        Withdraw = Result["AdjustAmount"].ToString(),
                        Closing = Convert.ToDecimal(Result["UtilizationAmount"]),


                    });
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

        #region SD Summary
        public void PdSummaryUtilizationReport(PdSummary ObjPdSummaryReport, int type = 1)
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
            IDataReader Result = DataAccess.ExecuteDataReader("PdSummaryUtilizationReport", CommandType.StoredProcedure, DParam);
            IList<PdSummary> LstPdSummaryReport = new List<PdSummary>();
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

                    LstPdSummaryReport.Add(new PdSummary
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
            SDStatement ObjSDStatement = new SDStatement();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        ObjSDStatement.LstSD.Add(new PPGSDList
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

        #region Party Wise SD Statement
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
            List<PartyForSDDet> LstParty = new List<PartyForSDDet>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstParty.Add(new PartyForSDDet
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
            PpgSDDetailsStatement SDResult = new PpgSDDetailsStatement();
            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    SDResult.PartyName = Result["PartyName"].ToString();
                    SDResult.PartyCode = Result["PartyCode"].ToString();
                    SDResult.PartyGst = Result["PartyGst"].ToString();
                    SDResult.CompanyGst = Result["CompanyGst"].ToString();
                    //SDResult.UtilizationAmount = Convert.ToDecimal(Result["UtilizationAmount"]);

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        SDResult.lstInvc.Add(
                            new PpgSDInvoiceDet
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

        #region SD Summary Details Report
        public void SDSummary(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("SDSummary", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Dnd_SDSummary> lstPV = new List<Dnd_SDSummary>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Dnd_SDSummary
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        Date = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        Module = Result["InvoiceType"].ToString(),
                        EximTraderName = Result["PartyName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),

                     //   BILL = Convert.ToDecimal(Result["MiscExcess"]),
                     //   GEN = Convert.ToDecimal(Result["GenSpace"]),
                        STO = Convert.ToDecimal(Result["sto"]),
                        INS = Convert.ToDecimal(Result["Insurance"]),
                        GRE = Convert.ToDecimal(Result["GroundRentEmpty"]),
                        GRL = Convert.ToDecimal(Result["GroundRentLoaded"]),
                        MFCHRG = Convert.ToDecimal(Result["Mf"]),
                        //MFTAX = Convert.ToDecimal(Result["MFTAX"]),
                      //  PDA = Convert.ToDecimal(Result["TotalPDA"]),
                        ENT = Convert.ToDecimal(Result["EntCharge"]),
                      //  FUM = Convert.ToDecimal(Result["Fum"]),
                      //  OT = Convert.ToDecimal(Result["OtCharge"]),
                        CGST = Convert.ToDecimal(Result["CGSTAmt"]),
                        SGST = Convert.ToDecimal(Result["SGSTAmt"]),
                        IGST = Convert.ToDecimal(Result["IGSTAmt"]),
                        RoundUp = Convert.ToDecimal(Result["RoundUp"]),
                       // MISC = Convert.ToDecimal(Result["MISC"]),
                        Total = Convert.ToDecimal(Result["TotalPDA"]),
                        TDS = Convert.ToString(Result["tdsCol"]),
                        CRTDS = Convert.ToDecimal(Result["crTDS"]),
                        PaymentType = Result["ModeOfPay"].ToString(),
                        Remarks = Result["Remarks"].ToString()
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

        #region Party Wise Invoice and Receipt Ledger
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
            Dnd_CashReceiptInvoiceLedger CrInvLedgerObj = new Dnd_CashReceiptInvoiceLedger();

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
                        CrInvLedgerObj.lstDndLedgerSummary.Add(new Dnd_CrInvLedgerSummary
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
                        CrInvLedgerObj.lstDndLedgerDetails.Add(new Dnd_CrInvLedgerDetails
                        {
                            InvCr = Convert.ToInt32(Result["InvCr"]),
                            InvCrId = Convert.ToInt32(Result["InvCrId"]),
                           // Description = Convert.ToString(Result["Description"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                          
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        CrInvLedgerObj.lstDndLedgerDetailsFull.Add(new Dnd_CrInvLedgerFullDetails
                        {
                            Sr = Convert.ToInt32(Result["Sr"]),
                            //Sr, ReceiptDt, ReceiptNo, ChargeCode, ContNo, Size, Debit, Credit, Balance, GroupSr, InvCr, InvCrId
                            ReceiptDt = Convert.ToString(Result["ReceiptDt"]),
                            ReceiptNo = Convert.ToString(Result["ReceiptNo"]),
                            // ChargeCode = Convert.ToString(Result["ChargeCode"]),
                            ChargeName = Convert.ToString(Result["ChargeName"]),
                            HSNCode = Convert.ToString(Result["SAC"]),
                            ContNo = Convert.ToString(Result["ContNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            Debit = Convert.ToDecimal(Result["Debit"]),
                            Credit = Convert.ToDecimal(Result["Credit"]),
                            Balance = Convert.ToDecimal(Result["Balance"]),
                            GroupSr = Convert.ToString(Result["GroupSr"]),


                        });
                    }
                }
               if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        CrInvLedgerObj.lstDndLedgerTotal.Add(new Dnd_CrInvLedgerTotal
                        {
                           // Sr = Convert.ToInt32(Result["Sr"]),
                            //Sr, ReceiptDt, ReceiptNo, ChargeCode, ContNo, Size, Debit, Credit, Balance, GroupSr, InvCr, InvCrId
                            Charge = Convert.ToString(Result["ChargeName"]),
                            Debit = Convert.ToDecimal(Result["Debit"]),
                            Credit = Convert.ToDecimal(Result["Credit"]),
                            // ChargeCode = Convert.ToString(Result["ChargeCode"]),
                            //  IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            // CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            // SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"])


                        });
                    }
                }

                if (Status == 1)
                {
                    CrInvLedgerObj.CompanyName = comname;
                    CrInvLedgerObj.CompanyAddress = address;

                    CrInvLedgerObj.lstDndLedgerSummary.ForEach(item =>
                    {
                        var dtls = CrInvLedgerObj.lstDndLedgerDetails.Where(o => o.InvCr == item.InvCr && o.InvCrId == item.InvCrId).ToList();
                        dtls.ForEach(d =>
                        {
                            item.LedgerDetails.Add(d);
                        });
                    });
                 
                    CrInvLedgerObj.TotalDebit = CrInvLedgerObj.lstDndLedgerDetailsFull.Sum(x => x.Debit);
                    CrInvLedgerObj.TotalCredit = CrInvLedgerObj.lstDndLedgerDetailsFull.Sum(x => x.Credit);
                    CrInvLedgerObj.ClosingBalance = CrInvLedgerObj.OpenningBalance + CrInvLedgerObj.TotalCredit - CrInvLedgerObj.TotalDebit;
                    /* CrInvLedgerObj.ClosingBalance = CrInvLedgerObj.lstLedgerBalance.Sum(x => x.Total);*/


                    /*  CrInvLedgerObj.ClosingBalance = (CrInvLedgerObj.OpenningBalance + (CrInvLedgerObj.lstLedgerSummary.Sum(o => o.Debit)))
                                                      - (CrInvLedgerObj.lstLedgerSummary.Sum(o => o.Credit));*/


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
            DndBillCumSDLedger LedgerObj = new DndBillCumSDLedger();

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

                        LedgerObj.lstDetails.Add(new DndBillCumSDLedgerDetails
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

                        LedgerObj.lstSummary.Add(new DndBillCumSDLedgerSummary
                        {
                            Sr = Convert.ToInt32(Result["Sr"]),
                            Col7 = Convert.ToString(Result["Col7"]),
                            Col8 = Convert.ToString(Result["Col8"]),
                            Col9 = Convert.ToString(Result["Col9"]),
                            Col10 = Convert.ToString(Result["Col10"]),

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

        #region Daily cash book
        public void DailyCashBook(string PeriodFrom, string PeriodTo)
        {
            PeriodFrom = DateTime.ParseExact(PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            PeriodTo = DateTime.ParseExact(PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            int Status = 0;

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });

            IDataParameter[] DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyCashBookReport", CommandType.StoredProcedure, DParam);
            IList<Dnd_DailyCashbook> LstDailyCashBook = new List<Dnd_DailyCashbook>();

            _DBResponse = new DatabaseResponse();
            try
            {
                int count = 0;
                while (Result.Read())
                {
                    Status = 1;
                    LstDailyCashBook.Add(new Dnd_DailyCashbook()
                    {
                        Sr = (++count).ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Result["ReceiptDate"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        ModeOfPay = Result["ModeOfPay"].ToString(),
                        ENT =Convert.ToDecimal(Result["ENT"]),
                        GRE =Convert.ToDecimal(Result["GRE"]), 
                        GRL = Convert.ToDecimal(Result["GRL"]),
                        Reefer = Convert.ToDecimal(Result["Reefer"]),
                        Monitoring = Convert.ToDecimal(Result["Monitoring"]),
                        STO = Convert.ToDecimal(Result["STO"]),
                        INS = Convert.ToDecimal(Result["INS"]),
                        WHT = Convert.ToDecimal(Result["WHT"]),
                        OTH = Convert.ToDecimal(Result["OTH"]),
                        HT = Convert.ToDecimal(Result["HT"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        RoundUp = Convert.ToDecimal(Result["RoundUp"]),
                        CHQNo = Result["CHQNo"].ToString(),
                        TotalCASH = Convert.ToDecimal(Result["TotalCASH"]),
                        TotalCHQ = Convert.ToDecimal(Result["TotalCHQ"]),
                        Others = Convert.ToDecimal(Result["Others"]),
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
                DndDTRExp obj = new DndDTRExp();
                obj = (DndDTRExp)DataAccess.ExecuteDynamicSet<DndDTRExp>("DailyTransactionExp", DParam);
                if (obj.lstBTTDetails.Count > 0 || obj.lstCargoAccepting.Count > 0 || obj.lstCargoShifting.Count > 0 || obj.lstCartingDetails.Count > 0 ||
                    obj.lstStuffingDetails.Count > 0 || obj.StockOpening.Count > 0 || obj.StockClosing.Count > 0 || obj.lstShortCartingDetails.Count > 0)
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
        #endregion

        #region TDS Report

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
            List<PartyLedgerList> LstParty = new List<PartyLedgerList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstParty.Add(new PartyLedgerList
                    {
                        PartyId = Convert.ToInt32(Result["EximTraderId"].ToString()),
                        PartyName = Result["EximTraderName"].ToString()

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

        public void TdsReport(Dnd_TDSReport ObjTDSReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjTDSReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjTDSReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjTDSReport.PartyId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("TDSReport", CommandType.StoredProcedure, DParam);
            Dnd_TDSMain objTDSMain = new Dnd_TDSMain();
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

                    objTDSMain.TDSReportLst.Add(new Dnd_TDSReport
                    {
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        CRNo = Result["ReceiptNo"].ToString(),
                        CRDate = Result["ReceiptDate"].ToString(),
                        PartyTAN = Result["TanNumber"].ToString(),
                        InvoiceValue = Result["InvoiceValue"].ToString(),
                        TDS = Result["TDS"].ToString(),
                        TDSPlus = Result["TDSPlus"].ToString(),
                        Amount = Result["Amount"].ToString(),
                        ReceiptAmount= Result["ReceiptAmt"].ToString(),


                    });
                }
                if (Status == 1)
                {
                    objTDSMain.TDSReportLst.Add(new Dnd_TDSReport
                    {
                        InvoiceNo = "<strong>Total</strong>",
                        InvoiceDate = string.Empty,
                        CRNo = string.Empty,
                        CRDate = string.Empty,
                        PartyTAN = string.Empty,
                        InvoiceValue = "<strong>" + objTDSMain.TDSReportLst.ToList().Sum(m => Convert.ToDecimal(m.InvoiceValue)).ToString() + "</strong>",
                        TDS = "<strong>" + objTDSMain.TDSReportLst.ToList().Sum(m => Convert.ToDecimal(m.TDS)).ToString() + "</strong>",
                        TDSPlus = "<strong>" + objTDSMain.TDSReportLst.ToList().Sum(m => Convert.ToDecimal(m.TDSPlus)).ToString() + "</strong>",
                        Amount = "<strong>" + objTDSMain.TDSReportLst.ToList().Sum(m => Convert.ToDecimal(m.Amount)).ToString() + "</strong>",
                        ReceiptAmount = "<strong>" + objTDSMain.TDSReportLst.ToList().Sum(m => Convert.ToDecimal(m.ReceiptAmount)).ToString() + "</strong>"


                    });

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objTDSMain.ObjTDSReporPartyWise.Add(
                        new TDSReporPartyWise
                        {
                            Party = Convert.ToString(Result["Party"]),
                            Tan = Convert.ToString(Result["Tan"]),
                            Value = Convert.ToString(Result["Value"]),
                            TDS = Convert.ToString(Result["TDS"]),
                            TDSPlus = Result["TDSPlus"].ToString()
                        });

                        //objTDSMain.ObjTDSReporPartyWise.Party = Convert.ToString(Result["Party"]);
                        //objTDSMain.ObjTDSReporPartyWise.Tan = Convert.ToString(Result["Tan"]);
                        //objTDSMain.ObjTDSReporPartyWise.Value = Convert.ToString(Result["Value"]);
                        //objTDSMain.ObjTDSReporPartyWise.TDS = Convert.ToString(Result["TDS"]);



                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objTDSMain;
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

        #region Bulk Cash receipt 
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

        public void GetChequeBounceCashreceipt(string ReceiptNo)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //    LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(FromDate == null ? "" : FromDate).ToString("yyyy-MM-dd") });
            //     LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ToDate == null ? "" : ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ReceiptNo });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetCashRecptForCCPrint", CommandType.StoredProcedure, DParam);
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
        public void GetReceiptList(string FromDate, string ToDate)
        {

            DateTime dtfrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");


            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });




            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ReceiptListWithDate", CommandType.StoredProcedure, DParam);
            IList<ReceiptList> LstReceiptList = new List<ReceiptList>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstReceiptList.Add(new ReceiptList
                    {



                        ReceiptNumber = Result["ReceiptNo"].ToString()



                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = JsonConvert.SerializeObject(LstReceiptList); ;
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
        #region MonthlyCashBook 
        public void MonthlyCashBook(DailyCashBookPpg ObjDailyCashBook)
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
            IList<Dnd_DailyCashbook> LstMonthlyCashBook = new List<Dnd_DailyCashbook>();
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

                    LstMonthlyCashBook.Add(new Dnd_DailyCashbook
                    {
                        //CRNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),

                        //Depositor = Result["Party"].ToString(),
                        //ChqNo = Result["ChequeNo"].ToString(),
                        // GenSpace = Result["GenSpace"].ToString(),
                        ENT = Convert.ToDecimal(Result["ENT"]),
                        GRE = Convert.ToDecimal(Result["GRE"]),
                        GRL = Convert.ToDecimal(Result["GRL"]),
                        Reefer = Convert.ToDecimal(Result["Reefer"]),
                        Monitoring = Convert.ToDecimal(Result["Monitoring"]),
                        STO = Convert.ToDecimal(Result["STO"]),
                        INS = Convert.ToDecimal(Result["INS"]),
                        WHT = Convert.ToDecimal(Result["WHT"]),
                        OTH = Convert.ToDecimal(Result["OTH"]),
                        HT = Convert.ToDecimal(Result["HT"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        RoundUp = Convert.ToDecimal(Result["RoundUp"]),
                      //  CHQNo = Result["CHQNo"].ToString(),
                        TotalCASH = Convert.ToDecimal(Result["TotalCASH"]),
                        TotalCHQ = Convert.ToDecimal(Result["TotalCHQ"]),
                        Others = Convert.ToDecimal(Result["TotalOther"]),
                        Remarks = Result["Remarks"].ToString(),
                      //  C = Result["CGSTAmt"].ToString(),
                      //  Sgst = Result["SGSTAmt"].ToString(),
                      //  Igst = Result["IGSTAmt"].ToString(),
                      // // Misc = Result["MISC"].ToString(),
                      ////  MiscExcess = Result["MiscExcess"].ToString(),
                      //  TotalCash = Result["TotalCash"].ToString(),
                      //  TotalCheque = Result["TotalCheque"].ToString(),
                      //  TotalOthers = Result["TotalOther"].ToString(),
                        TotalPDA = Convert.ToDecimal(Result["TotalPDA"]),
                        Tds = Convert.ToDecimal(Result["tdsCol"]),
                        CrTds = Convert.ToDecimal(Result["crTDS"])

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

        #region Track Your Container
        public void GetShippingLine()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLine", CommandType.StoredProcedure, DParam);
            List<DND_ShippingLineList> LstShippingLine = new List<DND_ShippingLineList>();
            // ShippingLine LstShippingLine = new ShippingLine();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstShippingLine.Add(new DND_ShippingLineList
                    {
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Result["ShippingLine"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstShippingLine;
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
        public void GetContainerNoForContStatus(int ShippingLineId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = "0" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerForContrStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DND_TrackContainer> LstContainer = new List<DND_TrackContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstContainer.Add(new DND_TrackContainer
                    {
                        ContainerNo = (Result["ContainerNo"] == null ? "" : Result["ContainerNo"]).ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstContainer;
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
        public void GetContainerDetForContStatus(int ShippingLineId, string ContainerNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerForContrStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DND_TrackContainerStatusList> LstContainer = new List<DND_TrackContainerStatusList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstContainer.Add(new DND_TrackContainerStatusList
                    {
                        AppraisementDate = Convert.ToString(Result["AppraisementDate"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerClass = Convert.ToString(Result["ContainerClass"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        CustomSeal = Convert.ToString(Result["CustomSealNo"]),
                        DestuffingDate = Convert.ToString(Result["DestuffingEntryDate"]),
                        ExportType = Convert.ToString(Result["ExportType"]),
                        GateEntryDate = Convert.ToString(Result["EntryDateTime"]),
                        GateExitDate = Convert.ToString(Result["GateExitDateTime"]),
                        GatePassDate = Convert.ToString(Result["GatePassDate"]),
                        GatePassNo = Convert.ToString(Result["GatePassNo"]),
                        GodownName = Convert.ToString(Result["GodownName"]),
                        JobOrderDate = Convert.ToString(Result["JobOrderDate"]),
                        LineNo = Convert.ToString(Result["LineNo"]),
                        Location = Convert.ToString(Result["Location"]),
                        PODestination = Convert.ToString(Result["POD"]),
                        POL = Convert.ToString(Result["POL"]),
                        Rotation = Convert.ToString(Result["RotationNo"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLine"]),
                        ShippingSeal = Convert.ToString(Result["ShippingLineSealNo"]),
                        Size = Convert.ToString(Result["Size"]),
                        StuffingDate = Convert.ToString(Result["StuffingDate"]),
                        VehicleNo = Convert.ToString(Result["VehicleNo"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstContainer;
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

        public void GetContainerNoForTrackContStatus()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ICDCode", MySqlDbType = MySqlDbType.VarChar, Value = "0" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerForContrStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Areas.Export.Models.ContainerList> LstContainer = new List<Areas.Export.Models.ContainerList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstContainer.Add(new Areas.Export.Models.ContainerList
                    {
                        ContainerNo = (Result["ContainerNo"] == null ? "" : Result["ContainerNo"]).ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstContainer;
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

        public void GetContWiseICDList(string ContainerNo)
        {
            int Status = 0;
            List<MySqlParameter> Lstparam = new List<MySqlParameter>();
            Lstparam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ContainerNo });
            IDataParameter[] DParam = { };
            DParam = Lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DA.ExecuteDataReader("GetICDListForTrackCont", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ICDList> LstICD = new List<ICDList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstICD.Add(new ICDList
                    {
                        ICDCode = Convert.ToString(Result["CFSCode"])

                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Data = LstICD;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Data = null;
                    _DBResponse.Message = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetContWiseLatestICD(string ContainerNo)
        {
            int Status = 0;
            List<MySqlParameter> Lstparam = new List<MySqlParameter>();
            Lstparam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ContainerNo });
            IDataParameter[] DParam = { };
            DParam = Lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DA.ExecuteDataReader("GetContWiseLatestICDForTrackCont", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            ICDList ObjIcd = new ICDList();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjIcd.ICDCode = Convert.ToString(Result["CFSCode"]);


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Data = ObjIcd;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Data = null;
                    _DBResponse.Message = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetContainerDetForTrackContStatus(string ICDCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ICDCode", MySqlDbType = MySqlDbType.VarChar, Value = ICDCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerForContrStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DND_TrackContainerStatusList> LstContainer = new List<DND_TrackContainerStatusList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstContainer.Add(new DND_TrackContainerStatusList
                    {
                        DestuffingDate = (Result["DestuffingDate"] == null ? "" : Result["DestuffingDate"]).ToString(),
                        StuffingDate = (Result["StuffingDate"] == null ? "" : Result["StuffingDate"]).ToString(),
                        AppraisementDate = (Result["AppraisementAppDate"] == null ? "" : Result["AppraisementAppDate"]).ToString(),
                        GatePassNo = (Result["GatePassNo"] == null ? "" : Result["GatePassNo"]).ToString(),
                        GateEntryDate = (Result["GateEntryDateTime"] == null ? "" : Result["GateEntryDateTime"]).ToString(),
                        Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString(),
                        GateExitDate = (Result["GateExitDateTime"] == null ? "" : Result["GateExitDateTime"]).ToString(),
                        GatePassDate = (Result["GatePassDate"] == null ? "" : Result["GatePassDate"]).ToString(),
                        LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                        JobOrderDate = (Result["JobOrderDate"] == null ? "" : Result["JobOrderDate"]).ToString(),
                        GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString(),
                        Location = (Result["Location"] == null ? "" : Result["Location"]).ToString(),
                        ShippingLineName = (Result["shippingLine"] == null ? "" : Result["shippingLine"]).ToString(),
                        Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),                        
                        CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                        ContainerClass = (Result["ContainerClass"] == null ? "" : Result["ContainerClass"]).ToString(),                       
                        ExportType = (Result["ExportType"] == null ? "" : Result["ExportType"]).ToString(),
                        VehicleNo = (Result["VehicleNo"] == null ? "" : Result["VehicleNo"]).ToString(),
                        CustomSeal = (Result["CustomSeal"] == null ? "" : Result["CustomSeal"]).ToString(),
                        ShippingSeal = (Result["ShippingSeal"] == null ? "" : Result["ShippingSeal"]).ToString(),                        
                        POL = (Result["POL"] == null ? "" : Result["POL"]).ToString(),
                        PODestination = (Result["PODestination"] == null ? "" : Result["PODestination"]).ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstContainer;
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

        public void GetContainerForContTracking(string ContainerNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();            
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });           
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerForContTracking", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DND_TrackContainer> LstContainer = new List<DND_TrackContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstContainer.Add(new DND_TrackContainer
                    {
                        ContainerNo = (Result["ContainerNo"] == null ? "" : Result["ContainerNo"]).ToString(),
                      
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstContainer;
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

        public void GetContainerDetForContTracking(string ContainerNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();            
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });          
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerForContTracking", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DND_TrackContainerStatusList> LstContainer = new List<DND_TrackContainerStatusList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstContainer.Add(new DND_TrackContainerStatusList
                    {
                        AppraisementDate = Convert.ToString(Result["AppraisementDate"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerClass = Convert.ToString(Result["ContainerClass"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        CustomSeal = Convert.ToString(Result["CustomSealNo"]),
                        DestuffingDate = Convert.ToString(Result["DestuffingEntryDate"]),
                        ExportType = Convert.ToString(Result["ExportType"]),
                        GateEntryDate = Convert.ToString(Result["EntryDateTime"]),
                        GateExitDate = Convert.ToString(Result["GateExitDateTime"]),
                        GatePassDate = Convert.ToString(Result["GatePassDate"]),
                        GatePassNo = Convert.ToString(Result["GatePassNo"]),
                        GodownName = Convert.ToString(Result["GodownName"]),
                        JobOrderDate = Convert.ToString(Result["JobOrderDate"]),
                        LineNo = Convert.ToString(Result["LineNo"]),
                        Location = Convert.ToString(Result["Location"]),
                        PODestination = Convert.ToString(Result["POD"]),
                        POL = Convert.ToString(Result["POL"]),
                        Rotation = Convert.ToString(Result["RotationNo"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLine"]),
                        ShippingSeal = Convert.ToString(Result["ShippingLineSealNo"]),
                        Size = Convert.ToString(Result["Size"]),
                        StuffingDate = Convert.ToString(Result["StuffingDate"]),
                        VehicleNo = Convert.ToString(Result["VehicleNo"]),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstContainer;
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
        #endregion

        #region ICE Details

        public void ListOfICEGateOBLNo(string SearchBy, string OBLNo, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLNo", MySqlDbType = MySqlDbType.VarChar, Value = OBLNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            lstParam.Add(new MySqlParameter { ParameterName = "In_SearchBy", MySqlDbType = MySqlDbType.VarChar, Value = SearchBy });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOBLNoForOBLSearch", CommandType.StoredProcedure, Dparam);
            IList<OBLNoForPage> LstObl = new List<OBLNoForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstObl.Add(new OBLNoForPage
                    {
                        OBLNo = Result["OBLNo"].ToString()
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
                    _DBResponse.Data = new { LstObl, State };
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
        public void GetICEGateDetail(string OBLNo, string SearchBy)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_OBLNo", MySqlDbType = MySqlDbType.String, Value = OBLNo });
                LstParam.Add(new MySqlParameter { ParameterName = "In_SearchBy", MySqlDbType = MySqlDbType.String, Value = SearchBy });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetICEGateData", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                OBLWiseContainerEntry objOBLEntry = new OBLWiseContainerEntry();

                //List<OblEntryDetails> OblEntryDetailsList = new List<OblEntryDetails>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        objOBLEntry.OBL_No = Convert.ToString(Result.Tables[0].Rows[0]["OBL_NO"]);
                        objOBLEntry.OBL_Date = Convert.ToString(Result.Tables[0].Rows[0]["OBL_DATE"]);
                        objOBLEntry.IGM_No = Convert.ToString(Result.Tables[0].Rows[0]["IGM_NO"]);
                        objOBLEntry.IGM_Date = Convert.ToString(Result.Tables[0].Rows[0]["IGM_DATE"]);
                        objOBLEntry.TPNo = Convert.ToString(Result.Tables[0].Rows[0]["TP_NO"]);
                        objOBLEntry.TPDate = Convert.ToString(Result.Tables[0].Rows[0]["TP_DATE"]);
                        objOBLEntry.MovementType = Convert.ToString(Result.Tables[0].Rows[0]["MovementType"]);
                        objOBLEntry.PortId = Convert.ToInt32(Result.Tables[0].Rows[0]["PortId"]);
                        objOBLEntry.LineNo = Convert.ToString(Result.Tables[0].Rows[0]["LINE_NO"].ToString());
                        objOBLEntry.CargoDescription = Convert.ToString(Result.Tables[0].Rows[0]["CargoDescription"]);
                        objOBLEntry.NoOfPkg = Convert.ToString(Result.Tables[0].Rows[0]["NO_PKG"]);
                        objOBLEntry.PkgType = Convert.ToString(Result.Tables[0].Rows[0]["PKG_TYPE"]);
                        objOBLEntry.GR_WT = Convert.ToDecimal(Result.Tables[0].Rows[0]["GR_WT"]);
                        objOBLEntry.ImporterId = Convert.ToInt32(Result.Tables[0].Rows[0]["ImporterId"]);
                        objOBLEntry.ImporterName = Convert.ToString(Result.Tables[0].Rows[0]["ImporterName"]);
                        objOBLEntry.ImporterAddress = Convert.ToString(Result.Tables[0].Rows[0]["ImporterAddress"]);
                        objOBLEntry.ImporterAddress1 = Convert.ToString(Result.Tables[0].Rows[0]["ImporterAddress1"]);
                        objOBLEntry.CargoType = Convert.ToInt32(Result.Tables[0].Rows[0]["CargoType"]);
                        objOBLEntry.SMTPNo = Convert.ToString(Result.Tables[0].Rows[0]["SMTPNo"]);
                    }

                    foreach (DataRow dr in Result.Tables[1].Rows)
                    {
                        OBLWiseContainerEntryDetails objOBLEntryDetails = new OBLWiseContainerEntryDetails();
                        objOBLEntryDetails.ShippingLineName = Convert.ToString(dr["ShippingLineName"]);
                        objOBLEntryDetails.ShippingLineId = Convert.ToInt32(dr["ShippingLineId"]);
                        objOBLEntryDetails.ContainerSize = Convert.ToString(dr["ContainerSize"]);
                        objOBLEntryDetails.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                        objOBLEntryDetails.NoOfPkg = Convert.ToString(dr["NO_PKG"]);
                        objOBLEntryDetails.GR_WT = Convert.ToDecimal(dr["GR_WT"]);
                        objOBLEntry.OblEntryDetailsList.Add(objOBLEntryDetails);
                    }
                }

                if (Status == 1)
                {
                    //if (OblEntryDetailsList.Count > 0)
                    //{
                    //    objOBLEntry.StringifiedText = Newtonsoft.Json.JsonConvert.SerializeObject(OblEntryDetailsList);
                    //}

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objOBLEntry;
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

        public void DeleteOBLWiseContainer(int OblEntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OblEntryId", MySqlDbType = MySqlDbType.Int32, Value = OblEntryId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("DeleteOBLWiseContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Cannot Delete As It Exists In Seal Cutting";
                    _DBResponse.Status = 2;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Cannot Delete  As It Exists In Job Order By Train";
                    _DBResponse.Status = 3;
                }
                //else if (Result == -1)
                //{
                //    _DBResponse.Data = null;
                //    _DBResponse.Message = "Cannot Delete As It Exists In Another Page";
                //    _DBResponse.Status = -1;
                //}
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
            List<DND_SBQuery> LstSB = new List<DND_SBQuery>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new DND_SBQuery
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        SBNO = Result["SBNO"].ToString()
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

        public void SBQueryReport(int id, string sbno)
        {


            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = id });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SBNO", MySqlDbType = MySqlDbType.VarChar, Value = sbno });

            int Status = 0;

            IDataParameter[] DParam = { };



            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSBQueryDetails", CommandType.StoredProcedure, DParam);
            DND_SBQuery LstSBQueryReport = new DND_SBQuery();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSBQueryReport.Id = Convert.ToInt32(Result["Id"]);
                    LstSBQueryReport.SBNO = Result["SBNO"].ToString();
                    LstSBQueryReport.ShippingBillDate = Result["SBDate"].ToString();
                    LstSBQueryReport.ShippingLine = Result["ShippingLine"].ToString();

                    LstSBQueryReport.CFSCode = Result["CFSCode"].ToString();
                    LstSBQueryReport.EntryDateTime = Result["EntryDateTime"].ToString();
                    LstSBQueryReport.CHA = Result["CHA"].ToString();

                    LstSBQueryReport.PortOFLoad = Convert.ToString(Result["PortOfLoading"]);
                    LstSBQueryReport.PortOFDischarge = Convert.ToString(Result["PortOfDischarge"]);
                    LstSBQueryReport.Godown = Convert.ToString(Result["GodownName"]);

                    LstSBQueryReport.Package = Result["Package"].ToString();
                    LstSBQueryReport.Weight = Result["Weight"].ToString();
                    LstSBQueryReport.FOB = Result["FOB"].ToString();                    
                    
                    LstSBQueryReport.Comodity = Result["Commodity"].ToString();                    
                    LstSBQueryReport.Cargotype = Result["CargoType"].ToString();
                    LstSBQueryReport.BAL = Result["BAL"].ToString();
                    
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        LstSBQueryReport.CartingSBQList.Add(new DND_CartingFORSB
                        {
                            Date = Convert.ToString(Result["Date"]),
                            CartingRegisterNo = Result["CartingRegisterNo"].ToString(),
                            Godown = Convert.ToString(Result["Godown"]),
                            Location = Result["Location"].ToString(),
                            NOOfPackages = Convert.ToInt32(Result["NoOFPackage"])
                        });



                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        LstSBQueryReport.DeliverySBQList.Add(new DND_DeliveryFORSBQuery
                        {
                            Date = Convert.ToString(Result["Date"]),
                            InvoiceNO = Result["InvoiceNo"].ToString(),
                            Exporter = Convert.ToString(Result["Exporter"]),
                            CHA = Result["CHA"].ToString(),
                            NOOfPackages = Convert.ToInt32(Result["NoOFPackage"])
                        });



                    }
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
        #region TotalContainerReport
        public void TotalContainerReportPrint(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("TotalContainerReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();

            //while (Result.Read())
            //{
            //   
            //    lstPV.Add(new Dnd_TotalContainerReport
            //    {
            //        CFSCode = Result["CFSCode"].ToString(),
            //        ContainerNo= Result["ContainerNo"].ToString(),
            //        InDateTime = Convert.ToString(Result["InDateTime"]),
            //        Size = Result["Size"].ToString(),
            //        ShippingLine = Result["ShippingLine"].ToString(),
            //        Origin = Result["Origin"].ToString(),
            //        Status = Result["Status"].ToString(),
            //        ContClass = Result["ContClass"].ToString(),
            //        VehicleNo = Result["VehicleNo"].ToString(),
            //        Remarks =Result["Remarks"].ToString(),
            //        OutDate = Result["Outdate"].ToString(),
            //        ContainerType= Result["ContainerType"].ToString(),

            //    });
            //}
            try
            {
                while (Result.Read())
                {
                    
                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            InDateTime = Convert.ToString(Result["InDateTime"]),
                            Size = Result["Size"].ToString(),
                            ShippingLine = Result["ShippingLine"].ToString(),
                            Origin = Result["Origin"].ToString(),
                            Status = Result["Status"].ToString(),
                            ContClass = Result["ContClass"].ToString(),
                            VehicleNo = Result["VehicleNo"].ToString(),
                            Remarks = Result["Remarks"].ToString(),
                            OutDate = Result["Outdate"].ToString(),
                            ContainerType = Result["ContainerType"].ToString(),

                            //    });
                        });
                    }
                }


                /*
                        CrInvLedgerObj.ClosingBalance = (CrInvLedgerObj.OpenningBalance + (CrInvLedgerObj.lstLedgerSummary.Sum(o => o.Debit)))
                                                        - (CrInvLedgerObj.lstLedgerSummary.Sum(o => o.Credit));
                        */


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
        #region TotalEmptyReport
        public void TotalEmptyContainerReportPrint(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("TotalContainerEmptyReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
           Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
           
                //while (Result.Read())
                //{
                //    Status = 1;
                //    lstPV.Add(new Dnd_TotalContainerReport
                //    {
                //        CFSCode = Result["CFSCode"].ToString(),
                //        ContainerNo = Result["ContainerNo"].ToString(),
                //        InDateTime = Convert.ToString(Result["InDateTime"]),
                //        Size = Result["Size"].ToString(),
                //        ShippingLine = Result["ShippingLine"].ToString(),
                //        Origin = Result["Origin"].ToString(),
                //        Status = Result["Status"].ToString(),
                //        ContClass = Result["ContClass"].ToString(),
                //        VehicleNo = Result["VehicleNo"].ToString(),
                //        Remarks = Result["Remarks"].ToString(),
                //        OutDate = Result["Outdate"].ToString(),

                //    });
                //}
                try
                {
                    while (Result.Read())
                    {
                   
                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                    }
                    if (Result.NextResult())
                    {
                        while (Result.Read())
                        {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                            {
                                CFSCode = Result["CFSCode"].ToString(),
                                ContainerNo = Result["ContainerNo"].ToString(),
                                InDateTime = Convert.ToString(Result["InDateTime"]),
                                Size = Result["Size"].ToString(),
                                ShippingLine = Result["ShippingLine"].ToString(),
                                Origin = Result["Origin"].ToString(),
                                Status = Result["Status"].ToString(),
                                ContClass = Result["ContClass"].ToString(),
                                VehicleNo = Result["VehicleNo"].ToString(),
                                Remarks = Result["Remarks"].ToString(),
                                OutDate = Result["Outdate"].ToString(),
                                //ContainerType = Result["ContainerType"].ToString(),

                                //    });
                            });
                        }
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
        #region TotalLoadedReport
        public void TotalLoadedReportPrint(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("TotalContainerLoadedReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
            ////try
            ////{
            ////    while (Result.Read())
            ////    {
            ////        Status = 1;
            ////    //    lstPV.Add(new Dnd_TotalContainerReport
            ////    //    {
            ////    //        CFSCode = Result["CFSCode"].ToString(),
            ////    //        ContainerNo = Result["ContainerNo"].ToString(),
            ////    //        InDateTime = Convert.ToString(Result["InDateTime"]),
            ////    //        Size = Result["Size"].ToString(),
            ////    //        ShippingLine = Result["ShippingLine"].ToString(),
            ////    //        Origin = Result["Origin"].ToString(),
            ////    //        Status = Result["Status"].ToString(),
            ////    //        ContClass = Result["ContClass"].ToString(),
            ////    //        VehicleNo = Result["VehicleNo"].ToString(),
            ////    //        Remarks = Result["Remarks"].ToString(),
            ////    //        OutDate = Result["Outdate"].ToString(),

            ////    //    });
            ////    //}
            try
            {
                while (Result.Read())
                {
                   
                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            InDateTime = Convert.ToString(Result["InDateTime"]),
                            Size = Result["Size"].ToString(),
                            ShippingLine = Result["ShippingLine"].ToString(),
                            Origin = Result["Origin"].ToString(),
                            Status = Result["Status"].ToString(),
                            ContClass = Result["ContClass"].ToString(),
                            VehicleNo = Result["VehicleNo"].ToString(),
                            Remarks = Result["Remarks"].ToString(),
                            OutDate = Result["Outdate"].ToString(),
                            //ContainerType = Result["ContainerType"].ToString(),

                            //    });
                        });
                    }
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
        #region TotalHubReport
        public void TotalHubReportPrint(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("TotalContainerHubReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
            //try
            //{
                //while (Result.Read())
                //{
                //    Status = 1;
                //    lstPV.Add(new Dnd_TotalContainerReport
                //    {
                //        CFSCode = Result["CFSCode"].ToString(),
                //        ContainerNo = Result["ContainerNo"].ToString(),
                //        InDateTime = Convert.ToString(Result["InDateTime"]),
                //        Size = Result["Size"].ToString(),
                //        ShippingLine = Result["ShippingLine"].ToString(),
                //        Origin = Result["Origin"].ToString(),
                //        Status = Result["Status"].ToString(),
                //        ContClass = Result["ContClass"].ToString(),
                //        VehicleNo = Result["VehicleNo"].ToString(),
                //        Remarks = Result["Remarks"].ToString(),
                //        OutDate = Result["Outdate"].ToString(),
                //        ContainerType= Result["ContainerType"].ToString(),

                //    });
                //}
                try
                {
                    while (Result.Read())
                    {
                    
                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                    }
                    if (Result.NextResult())
                    {
                        while (Result.Read())
                        {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                            {
                                CFSCode = Result["CFSCode"].ToString(),
                                ContainerNo = Result["ContainerNo"].ToString(),
                                InDateTime = Convert.ToString(Result["InDateTime"]),
                                Size = Result["Size"].ToString(),
                                ShippingLine = Result["ShippingLine"].ToString(),
                                Origin = Result["Origin"].ToString(),
                                Status = Result["Status"].ToString(),
                                ContClass = Result["ContClass"].ToString(),
                                VehicleNo = Result["VehicleNo"].ToString(),
                                Remarks = Result["Remarks"].ToString(),
                                OutDate = Result["Outdate"].ToString(),
                                ContainerType = Result["ContainerType"].ToString(),

                                //    });
                            });
                        }
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
        #region TotalContainerWheelReport
        public void TotalWheelReportPrint(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("TotalContainerWheelReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
           Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
            //try
            //{
            //while (Result.Read())
            //{
            //    Status = 1;
            //    lstPV.Add(new Dnd_TotalContainerReport
            //    {
            //        CFSCode = Result["CFSCode"].ToString(),
            //        ContainerNo = Result["ContainerNo"].ToString(),
            //        InDateTime = Convert.ToString(Result["InDateTime"]),
            //        Size = Result["Size"].ToString(),
            //        ShippingLine = Result["ShippingLine"].ToString(),
            //        Origin = Result["Origin"].ToString(),
            //        Status = Result["Status"].ToString(),
            //        ContClass = Result["ContClass"].ToString(),
            //        VehicleNo = Result["VehicleNo"].ToString(),
            //        Remarks = Result["Remarks"].ToString(),
            //        OutDate = Result["Outdate"].ToString(),
            //        ContainerType= Result["ContainerType"].ToString(),
            //        CustomSealNo = Result["CustomSealNo"].ToString(),

            //    });
            //}
            try
            {
                while (Result.Read())
                {
                 
                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            InDateTime = Convert.ToString(Result["InDateTime"]),
                            Size = Result["Size"].ToString(),
                            ShippingLine = Result["ShippingLine"].ToString(),
                            Origin = Result["Origin"].ToString(),
                            Status = Result["Status"].ToString(),
                            ContClass = Result["ContClass"].ToString(),
                            VehicleNo = Result["VehicleNo"].ToString(),
                            Remarks = Result["Remarks"].ToString(),
                            OutDate = Result["Outdate"].ToString(),
                            ContainerType = Result["ContainerType"].ToString(),
                            CustomSealNo = Result["CustomSealNo"].ToString(),

                            //    });
                        });
                    }
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
        #region EmptyContainerOutReport
        public void EmptyContainerOutPrint(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("EmptyContainerOutReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),
            //            Size = Result["Size"].ToString(),
            //            OutDate = Result["Outdate"].ToString(),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //            InDateTime = Convert.ToString(Result["InDateTime"]),
            //            DestuffingDate= Result["DestuffingDate"].ToString(),
            //            GatePassNo = Result["GatePassNo"].ToString(),
            //           ExitDateTime = Result["ExitDateTime"].ToString(),
            //          });
            //    
            //}
            try
            {
                while (Result.Read())
                {
                  
                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            OutDate = Result["Outdate"].ToString(),
                            ShippingLine = Result["ShippingLine"].ToString(),
                            InDateTime = Convert.ToString(Result["InDateTime"]),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            GatePassNo = Result["GatePassNo"].ToString(),
                            ExitDateTime = Result["ExitDateTime"].ToString(),

                            //    });
                        });
                    }
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

        #region Fresh Movement Received
        public void FreshMovReceived(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("FreshMovRecRpt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),                        
            //            Size = Result["Size"].ToString(),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //            MovementNo = Result["MovementNo"].ToString(),
            //            MovementDate = Result["MovementDate"].ToString(),
            //            OutDate = Result["Outdate"].ToString(),
            //            ViaNo = Result["ViaNo"].ToString(),
            //            Vessel = Result["Vessel"].ToString(),
            //        });
            //    }
            try
            {
                while (Result.Read())
                {
                   
                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            ShippingLine = Result["ShippingLine"].ToString(),
                            MovementNo = Result["MovementNo"].ToString(),
                            MovementDate = Result["MovementDate"].ToString(),
                            OutDate = Result["Outdate"].ToString(),
                            ViaNo = Result["ViaNo"].ToString(),
                            Vessel = Result["Vessel"].ToString(),

                            //    });
                        });
                    }
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
        #region ExportContainerOutReport
        public void ExportContainerOutPrint(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ExportContainerOutReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),
            //            Size = Result["Size"].ToString(),
            //            InDateTime = Convert.ToString(Result["InDateTime"]),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //           CustomSealNo= Result["CustomSealNo"].ToString(),
            //            POD= Result["POD"].ToString(),
            //            Vessel= Result["Vessel"].ToString(),
            //            MovementNo= Result["MovementNo"].ToString(),
            //            GatePassNo = Result["GatePassNo"].ToString(),
            //            VehicleNo = Result["VehicleNo"].ToString(),
            //            ContClass = Result["TypeClass"].ToString(),
            //            ExitDateTime = Result["ExitDateTime"].ToString(),
            //        });
            //    }
            try
            {
                while (Result.Read())
                {
                  
                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            InDateTime = Convert.ToString(Result["InDateTime"]),
                            ShippingLine = Result["ShippingLine"].ToString(),
                            CustomSealNo = Result["CustomSealNo"].ToString(),
                            POD = Result["POD"].ToString(),
                            Vessel = Result["Vessel"].ToString(),
                            MovementNo = Result["MovementNo"].ToString(),
                            GatePassNo = Result["GatePassNo"].ToString(),
                            VehicleNo = Result["VehicleNo"].ToString(),
                            ContClass = Result["TypeClass"].ToString(),
                            ExitDateTime = Result["ExitDateTime"].ToString(),

                            //    });
                        });
                    }
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
        #region LoadedContainerOutReport
        public void LoadedContainerOutPrint(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("LoadedContainerOutReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),
            //            Size = Result["Size"].ToString(),
            //            InDateTime = Convert.ToString(Result["InDateTime"]),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //            CustomSealNo = Result["CustomSealNo"].ToString(),
            //            POD = Result["POD"].ToString(),
            //            Vessel = Result["Vessel"].ToString(),
            //            MovementNo = Result["MovementNo"].ToString(),
            //            GatePassNo = Result["GatePassNo"].ToString(),
            //            VehicleNo = Result["VehicleNo"].ToString(),
            //            ContClass = Result["TypeClass"].ToString(),
            //            ExitDateTime = Result["ExitDateTime"].ToString(),
            //        });
            //    }
            try
            {
                while (Result.Read())
                {
                   
                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            InDateTime = Convert.ToString(Result["InDateTime"]),
                            ShippingLine = Result["ShippingLine"].ToString(),
                            CustomSealNo = Result["CustomSealNo"].ToString(),
                            POD = Result["POD"].ToString(),
                            Vessel = Result["Vessel"].ToString(),
                            MovementNo = Result["MovementNo"].ToString(),
                            GatePassNo = Result["GatePassNo"].ToString(),
                            VehicleNo = Result["VehicleNo"].ToString(),
                            ContClass = Result["TypeClass"].ToString(),
                            ExitDateTime = Result["ExitDateTime"].ToString(),

                            //    });
                        });
                    }
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
        #region PrivateMovementDetailReport
        public void PrivateMovementDetailPrint(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PrivateMovementDetailReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),
            //            Size = Result["Size"].ToString(),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //            ExitDateTime=Result["Outdate"].ToString(),
            //            POL=Result["POL"].ToString(),
            //            MovementNo = Result["MovementNo"].ToString(),
            //            MovementDate= Result["MovementDate"].ToString(),
            //            GatePassNo = Result["GatePassNo"].ToString(),
            //            ViaNo = Result["ViaNo"].ToString(),
            //            Vessel = Result["Vessel"].ToString(),
            //        });
            //    }
            try
            {
                while (Result.Read())
                {
               
                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            ShippingLine = Result["ShippingLine"].ToString(),
                            ExitDateTime = Result["Outdate"].ToString(),
                            POL = Result["POL"].ToString(),
                            MovementNo = Result["MovementNo"].ToString(),
                            MovementDate = Result["MovementDate"].ToString(),
                            GatePassNo = Result["GatePassNo"].ToString(),
                            ViaNo = Result["ViaNo"].ToString(),
                            Vessel = Result["Vessel"].ToString(),
                            //    });
                        });
                    }
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

        #region Fresh Movement Received Vessel Wise
        public void VesselWiseFreshMovReceived(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("FreshMovRecRptVessel", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),
            //            Size = Result["Size"].ToString(),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //            MovementNo = Result["MovementNo"].ToString(),
            //            MovementDate = Result["MovementDate"].ToString(),
            //            OutDate = Result["Outdate"].ToString(),
            //            ViaNo = Result["ViaNo"].ToString(),
            //            Vessel = Result["Vessel"].ToString(),
            //        });
            //    }
            try
            {
                while (Result.Read())
                {
                  
                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            ShippingLine = Result["ShippingLine"].ToString(),
                            MovementNo = Result["MovementNo"].ToString(),
                            MovementDate = Result["MovementDate"].ToString(),
                            OutDate = Result["Outdate"].ToString(),
                            ViaNo = Result["ViaNo"].ToString(),
                            Vessel = Result["Vessel"].ToString(),

                            //    });
                        });
                    }
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
        #region Pendency Report
        public void PendencyReportPrint(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PendencyReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
          Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),
            //            Size = Result["Size"].ToString(),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //            MovementNo = Result["MovementNo"].ToString(),
            //            MovementDate = Result["MovementDate"].ToString(),
            //            ViaNo = Result["ViaNo"].ToString(),
            //            Vessel = Result["Vessel"].ToString(),
            //        });
            //    }
            try
            {
                while (Result.Read())
                {
                  
                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            ShippingLine = Result["ShippingLine"].ToString(),
                            MovementNo = Result["MovementNo"].ToString(),
                            MovementDate = Result["MovementDate"].ToString(),
                           // OutDate = Result["Outdate"].ToString(),
                            ViaNo = Result["ViaNo"].ToString(),
                            Vessel = Result["Vessel"].ToString(),

                            //    });
                        });
                    }
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
        #region Vessel Wise Pendency Report
        public void VesselWisePendencyReportPrint(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PendencyReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),
            //            Size = Result["Size"].ToString(),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //            MovementNo = Result["MovementNo"].ToString(),
            //            MovementDate = Result["MovementDate"].ToString(),
            //            ViaNo = Result["ViaNo"].ToString(),
            //            Vessel = Result["Vessel"].ToString(),
            //        });
            //    }
            try
            {
                while (Result.Read())
                {
                   
                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            ShippingLine = Result["ShippingLine"].ToString(),
                            MovementNo = Result["MovementNo"].ToString(),
                            MovementDate = Result["MovementDate"].ToString(),
                            ViaNo = Result["ViaNo"].ToString(),
                            Vessel = Result["Vessel"].ToString(),

                            //    });
                        });
                    }
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
        #region Pendency Query
        public void GetAllVia()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //    LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            //  DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetViaForPendency", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Dnd_TotalContainer> LstVia = new List<Dnd_TotalContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstVia.Add(new Dnd_TotalContainer
                    {
                        Via = Result["Via"].ToString(),
                        ViaId = (Result["ViaId"]).ToString(),
                     
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstVia;
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
        public void VesselWisePendencyPrint(string ViaId)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ViaId", MySqlDbType = MySqlDbType.VarChar, Value = ViaId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PendencyQuery", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),
            //            Size = Result["Size"].ToString(),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //            MovementNo = Result["MovementNo"].ToString(),
            //            MovementDate = Result["MovementDate"].ToString(),

            //        });
            //    }
            try
            {
                while (Result.Read())
                {
                 
                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            ShippingLine = Result["ShippingLine"].ToString(),
                            MovementNo = Result["MovementNo"].ToString(),
                            MovementDate = Result["MovementDate"].ToString(),
                            ViaNo = Result["Via"].ToString(),
                            Vessel = Result["Vessel"].ToString(),
                            //    });
                        });
                    }
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
        #region  BackToTownCargoReport
        public void BackToTownCargoPrint(string FromDate,string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("BackToTownCargoPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();           
                try
                {
                    while (Result.Read())
                    {
                
                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                    }
                    if (Result.NextResult())
                    {
                        while (Result.Read())
                        {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                            GatePassNo = Result["GatePassNo"].ToString(),
                            GatePassDate = Result["GatePassDate"].ToString(),
                            CFSCode = Result["EntryNo"].ToString(),
                            ShippingBillNo = Result["SbNo"].ToString(),
                            ShippingBillDate = Result["SbDate"].ToString(),
                            ShippingLine = Result["ShippingLine"].ToString(),
                            NoOfPkg = Convert.ToDecimal(Result["NoPkg"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            Fob = Convert.ToDecimal(Result["Fob"]),
                            VehicleNo = Result["VehicleNo"].ToString(),
                            ShedNo = Result["ShedNo"].ToString(),
                            Remarks = Convert.ToString(Result["FileNo"]),
                            InDateTime = Convert.ToString(Result["InDate"])

                                
                            });
                        }
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
        #region Buffer Out Report
        public void BufferContainerOutReportPrint(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("BufferContainerOutReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
            //try
            //{
                //while (Result.Read())
                //{
                //    Status = 1;
                //    lstPV.Add(new Dnd_TotalContainerReport
                //    {
                //        CFSCode = Result["CFSCode"].ToString(),
                //        ContainerNo = Result["ContainerNo"].ToString(),
                //        Size = Result["Size"].ToString(),
                //        ShippingLine = Result["ShippingLine"].ToString(),
                //        InDateTime = Result["InDate"].ToString(),
                //        CustomSealNo = Result["CustomSealNo"].ToString(),
                //        SlaSealNo = Result["ShippingLineSealNo"].ToString(),
                //        MovementNo = Result["MovementNo"].ToString(),
                //        MovementDate = Result["MovementDate"].ToString(),
                //        GatePassNo= Result["GatePassNo"].ToString(),
                //        OutDate= Result["OutDate"].ToString(),
                //    });
                //}
                try
                {
                    while (Result.Read())
                    {
                 
                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                    }
                    if (Result.NextResult())
                    {
                        while (Result.Read())
                        {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                            {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            ShippingLine = Result["ShippingLine"].ToString(),
                            InDateTime = Result["InDate"].ToString(),
                            CustomSealNo = Result["CustomSealNo"].ToString(),
                            SlaSealNo = Result["ShippingLineSealNo"].ToString(),
                            MovementNo = Result["MovementNo"].ToString(),
                            MovementDate = Result["MovementDate"].ToString(),
                            GatePassNo = Result["GatePassNo"].ToString(),
                            OutDate = Result["OutDate"].ToString(),

                            //    });
                        });
                        }
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
        #region OnWheel Out Report
        public void OnWheelOutReportPrint(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("OnWheelOutReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),
            //            Size = Result["Size"].ToString(),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //            GatePassNo = Result["GatePassNo"].ToString(),
            //            GatePassDate = Result["GatePassDate"].ToString(),
            //            VehicleNo = Result["VehicleNo"].ToString(),
            //            OutDate = Result["OutDate"].ToString(),
            //        });
            //    }
            try
            {
                while (Result.Read())
                {
                   
                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            ShippingLine = Result["ShippingLine"].ToString(),
                            GatePassNo = Result["GatePassNo"].ToString(),
                            GatePassDate = Result["GatePassDate"].ToString(),
                            VehicleNo = Result["VehicleNo"].ToString(),
                            OutDate = Result["OutDate"].ToString(),

                            //    });
                        });
                    }
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
        #region Stuffing Request Reprint
        public void GetAllStuff()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //    LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            //  DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingNoForReprint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Dnd_TotalContainer> LstVia = new List<Dnd_TotalContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstVia.Add(new Dnd_TotalContainer
                    {
                        Stuff = Result["Stuff"].ToString(),
                        StuffId = Convert.ToInt32(Result["StuffId"]),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstVia;
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
        #region Cargo Carting Register Report
        public void CargoCartingRegisterReportPrint(Dnd_TotalContainerReport obj,string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
           // Dnd_TotalContainerReport obj = new Dnd_TotalContainerReport();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = obj.GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("CargoCartingRegisterReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),
            //            Size = Result["Size"].ToString(),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //            MovementNo = Result["MovementNo"].ToString(),
            //            MovementDate = Result["MovementDate"].ToString(),
            //            ViaNo = Result["ViaNo"].ToString(),
            //            Vessel = Result["Vessel"].ToString(),
            //        });
            //    }
            try
            {
                while (Result.Read())
                {

                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                           EntryNo  = Result["EntryNo"].ToString(),
                            CartingDate = Result["CartingDate"].ToString(),
                            ShippingBillNo = Result["ShippingBillNo"].ToString(),
                            ShippingBillDate = Result["ShippingBillDate"].ToString(),
                            ExpCode = Result["ExpCode"].ToString(),
                            CHA = Result["Cha"].ToString(),
                            Commodity = Result["Commodity"].ToString(),
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            TotalPkg = Convert.ToDecimal(Result["TotalPkg"]),
                            ReceivedPkg = Convert.ToDecimal(Result["PkgRcvd"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            Balance = Convert.ToDecimal(Result["Balance"]),
                            Fob = Convert.ToDecimal(Result["Fob"]),
                            SlotNo= Result["SlotNo"].ToString(),
                            GenReserved = Result["GenReserved"].ToString(),
                            Area = Convert.ToDecimal(Result["Area"]),
                            ShippingLineId= Convert.ToInt32(Result["ShippingLineId"]),
                            Remarks= Result["Remarks"].ToString(),
                            //    });
                        });
                    }
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
        public void GetGodownForCargoCarting()
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
           // LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGodownListCargoCarting", CommandType.StoredProcedure, DParam);
            IList<CwcExim.Areas.Import.Models.GodownList> lst = new List<CwcExim.Areas.Import.Models.GodownList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lst.Add(new CwcExim.Areas.Import.Models.GodownList
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
        #endregion
        #region Daily Transaction Carting
        public void DailyTransactionCartingPrint(Dnd_TotalContainerReport obj,string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = obj.GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyTransactionCarting", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),
            //            Size = Result["Size"].ToString(),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //            GatePassNo = Result["GatePassNo"].ToString(),
            //            GatePassDate = Result["GatePassDate"].ToString(),
            //            VehicleNo = Result["VehicleNo"].ToString(),
            //            OutDate = Result["OutDate"].ToString(),
            //        });
            //    }
            try
            {
                while (Result.Read())
                {

                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                            ShippingBillNo = Result["ShippingBillNo"].ToString(),
                            TotalPkg = Convert.ToDecimal(Result["TotalPkg"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            Fob = Convert.ToDecimal(Result["Fob"]),
                            ShippingLine = Result["ShippingLineCode"].ToString(),
                            Godown = Result["Godown"].ToString(),
                            Remarks= Result["Remarks"].ToString(),


                            //    });
                        });
                    }
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
        #region Stuffing Request Register
        public void StuffingRequestRegisterPrint(Dnd_TotalContainerReport obj, string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = obj.GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("StuffingRequestRegister", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),
            //            Size = Result["Size"].ToString(),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //            GatePassNo = Result["GatePassNo"].ToString(),
            //            GatePassDate = Result["GatePassDate"].ToString(),
            //            VehicleNo = Result["VehicleNo"].ToString(),
            //            OutDate = Result["OutDate"].ToString(),
            //        });
            //    }
            try
            {
                while (Result.Read())
                {

                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                            StuffingReqNo = Result["StuffingReqNo"].ToString(),
                            RequestDate = Result["RequestDate"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo= Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            ShippingLineName = Result["ShippingLineCode"].ToString(),
                            InDateTime = Result["InDate"].ToString(),
                            NoOfSbs = Convert.ToInt32(Result["NoOfSbs"]),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                            Via = Result["ViaNo"].ToString(),
                            Vessel = Result["Vessel"].ToString(),
                            Godown= Result["Godown"].ToString(),
                            //    });
                        });
                    }
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
        #region  Stuffing Daily Transaction
        public void StuffingDailyTransactionPrint(Dnd_TotalContainerReport obj, string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = obj.GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("StuffingDailyTransaction", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),
            //            Size = Result["Size"].ToString(),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //            GatePassNo = Result["GatePassNo"].ToString(),
            //            GatePassDate = Result["GatePassDate"].ToString(),
            //            VehicleNo = Result["VehicleNo"].ToString(),
            //            OutDate = Result["OutDate"].ToString(),
            //        });
            //    }
            try
            {
                while (Result.Read())
                {

                    lstPV.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                            StuffingReqNo = Result["StuffingReqNo"].ToString(),
                            RequestDate = Result["RequestDate"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            ShippingLineName = Result["ShippingLineCode"].ToString(),
                            InDateTime = Result["InDate"].ToString(),
                            NoOfSbs = Convert.ToInt32(Result["NoOfSbs"]),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            Fob = Convert.ToDecimal(Result["Fob"]),
                            Via = Result["ViaNo"].ToString(),
                            Vessel = Result["Vessel"].ToString(),
                            Godown= Result["Godown"].ToString(),
                            //    });
                        });
                    }
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
        public void GetPVReportImport(Dnd_PV objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objPV.Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objPV.GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PVReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Dnd_ImpPVReport> lstPV = new List<Dnd_ImpPVReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Dnd_ImpPVReport
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
        public void GetPVReportExport(Dnd_PV objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objPV.Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objPV.GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PVReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            List<Dnd_ExpPVReport> lstPV = new List<Dnd_ExpPVReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Dnd_ExpPVReport
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
        public void GetPVReportBond(Dnd_PV objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objPV.Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objPV.GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PVReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Dnd_BondPVReport> lstPV = new List<Dnd_BondPVReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Dnd_BondPVReport
                    {
                        CompAddress = Result["CompanyAddress"].ToString(),
                        CompEmail = Result["CompanyEmail"].ToString(),
                        BondNo = Result["BondNo"].ToString(),
                        BondDate = Result["BondDate"].ToString(),
                        Importer = Result["Importer"].ToString(),
                        ItemDesc = Result["ItemDesc"].ToString(),
                        Units = Convert.ToInt32(Result["Unit"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
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
        public void GetPVReportImportITP(Dnd_PV objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objPV.Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objPV.GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PVReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Dnd_ImpPVReport> lstPV = new List<Dnd_ImpPVReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Dnd_ImpPVReport
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
        #endregion

        #region General Space Utilisation Register
        public void GeneralSpaceUtilisationRegister(Dnd_TotalContainerReport obj, string AsOnDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            // Dnd_TotalContainerReport obj = new Dnd_TotalContainerReport();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = obj.GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GeneralSpaceUtilisationRegister", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstData = new Dnd_TotalContainerReport();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),
            //            Size = Result["Size"].ToString(),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //            MovementNo = Result["MovementNo"].ToString(),
            //            MovementDate = Result["MovementDate"].ToString(),
            //            ViaNo = Result["ViaNo"].ToString(),
            //            Vessel = Result["Vessel"].ToString(),
            //        });
            //    }
            try
            {
                while (Result.Read())
                {

                    lstData.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstData.LstTotal.Add(new Dnd_TotalContainer
                        {
                            EntryNo = Result["EntryNo"].ToString(),
                            CartingDate = Result["CartingDate"].ToString(),
                            ShippingBillNo = Result["SBNo"].ToString(),
                            ShippingBillDate = Result["SBDate"].ToString(),
                            NoOfUnits = Convert.ToDecimal(Result["ActualQty"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            Fob = Convert.ToDecimal(Result["FOB"].ToString()),
                            SQM = Convert.ToDecimal(Result["SQM"].ToString()),
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                            ShippingLineAlias = Result["ShippingLineCode"].ToString(),
                            //    });
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstData;
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
        #region Carting Allow Register
        public void CartingAllowRegister(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            // Dnd_TotalContainerReport obj = new Dnd_TotalContainerReport();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("CartingAllowRegister", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstData = new Dnd_TotalContainerReport();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),
            //            Size = Result["Size"].ToString(),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //            MovementNo = Result["MovementNo"].ToString(),
            //            MovementDate = Result["MovementDate"].ToString(),
            //            ViaNo = Result["ViaNo"].ToString(),
            //            Vessel = Result["Vessel"].ToString(),
            //        });
            //    }
            try
            {
                while (Result.Read())
                {

                    lstData.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstData.LstTotal.Add(new Dnd_TotalContainer
                        {
                            VehicleNo = Result["TruckNo"].ToString(),
                            InDateTime = Result["indate"].ToString(),
                            TotalSbs = Convert.ToInt32(Result["Totsbs"].ToString()),
                            TotalPkg = Convert.ToInt32(Result["TotalPkgs"].ToString()),
                            ShippingLineAlias = Result["SlaCode"].ToString(),
                            ShedNo =Result["ShedNo"].ToString()

                            //    });
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstData;
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
        #region Empty Vehicle in/out Register
        public void EmptyVehicleinoutRegister(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            // Dnd_TotalContainerReport obj = new Dnd_TotalContainerReport();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("EmptyVehicleInoutRegister", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstData = new Dnd_TotalContainerReport();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),
            //            Size = Result["Size"].ToString(),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //            MovementNo = Result["MovementNo"].ToString(),
            //            MovementDate = Result["MovementDate"].ToString(),
            //            ViaNo = Result["ViaNo"].ToString(),
            //            Vessel = Result["Vessel"].ToString(),
            //        });
            //    }
            try
            {
                while (Result.Read())
                {

                    lstData.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstData.LstTotal.Add(new Dnd_TotalContainer
                        {
                            VehicleNo = Result["TruckNo"].ToString(),
                            InDateTime = Result["indate"].ToString(),
                            LoadedInDate = Result["LoadedInDate"].ToString(),
                            ExitDateTime = Result["OutDate"].ToString(),
                            Remarks = Result["Remarks"].ToString()

                            //    });
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstData;
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
        #region Miscelleneout entry Register
        public void MiscelleneoutentryRegister(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            // Dnd_TotalContainerReport obj = new Dnd_TotalContainerReport();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("CartingAllowRegister", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstData = new Dnd_TotalContainerReport();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),
            //            Size = Result["Size"].ToString(),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //            MovementNo = Result["MovementNo"].ToString(),
            //            MovementDate = Result["MovementDate"].ToString(),
            //            ViaNo = Result["ViaNo"].ToString(),
            //            Vessel = Result["Vessel"].ToString(),
            //        });
            //    }
            try
            {
                while (Result.Read())
                {

                    lstData.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstData.LstTotal.Add(new Dnd_TotalContainer
                        {
                            VehicleNo = Result["TruckNo"].ToString(),
                            InDateTime = Result["indate"].ToString(),
                            TotalSbs = Convert.ToInt32(Result["Totsbs"].ToString()),
                            TotalPkg = Convert.ToInt32(Result["TotalPkgs"].ToString())

                            //    });
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstData;
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
        #region Daily Gate Summary
        public void DailyGateSummary(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            // Dnd_TotalContainerReport obj = new Dnd_TotalContainerReport();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyGateSummaryReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_GateSummaryReport lstData = new Dnd_GateSummaryReport();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),
            //            Size = Result["Size"].ToString(),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //            MovementNo = Result["MovementNo"].ToString(),
            //            MovementDate = Result["MovementDate"].ToString(),
            //            ViaNo = Result["ViaNo"].ToString(),
            //            Vessel = Result["Vessel"].ToString(),
            //        });
            //    }
            try
            {
                while (Result.Read())
                {

                    lstData.mstcompany = Result["CompanyAddress"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstData.LstEmptyIn.Add(new Dnd_TotalEmptyIn
                        {
                            EmptyIn20 = Result["EmptyIn20"].ToString(),
                            EmptyIn40 = Result["EmptyIn40"].ToString(),
                            EmptyIn45 = Result["EmptyIn45"].ToString(),
                            EmptyInTeus = Result["EmptyInTeus"].ToString(),
                            EmptyInTotal = Result["EmptyInTotal"].ToString(),
                            //    });
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstData.LstEmptyOut.Add(new Dnd_TotalEmptyOut
                        {
                            EmptyOut20 = Result["EmptyOut20"].ToString(),
                            EmptyOut40 = Result["EmptyOut40"].ToString(),
                            EmptyOut45 = Result["EmptyOut45"].ToString(),
                            EmptyOutTeus = Result["EmptyOutTeus"].ToString(),
                            EmptyOutTotal = Result["EmptyOutTotal"].ToString(),
                            //    });
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstData.LstShutOut.Add(new Dnd_ShutOut
                        {
                            ShutOut20 = Result["ShutOut20"].ToString(),
                            ShutOut40 = Result["ShutOut40"].ToString(),
                            ShutOut45 = Result["ShutOut45"].ToString(),
                            ShutOutTeus = Result["ShutOutTeus"].ToString(),
                            ShutOutTotal = Result["ShutOutTotal"].ToString(),
                            //    });
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstData.LstHub.Add(new Dnd_Hub
                        {
                            Hub20 = Result["Hub20"].ToString(),
                            Hub40 = Result["Hub40"].ToString(),
                            Hub45 = Result["Hub45"].ToString(),
                            HubTeus = Result["HubTeus"].ToString(),
                            HubTotal = Result["HubTotal"].ToString(),
                            //    });
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstData.LstOthers.Add(new Dnd_Others
                        {
                            Others20 = Result["Others20"].ToString(),
                            Others40 = Result["Others40"].ToString(),
                            Others45 = Result["Others45"].ToString(),
                            OthersTeus = Result["OthersTeus"].ToString(),
                            OthersTotal = Result["OthersTotal"].ToString(),
                            //    });
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstData.LstHtc.Add(new Dnd_Htc
                        {
                            Htc20 = Result["Htc20"].ToString(),
                            Htc40 = Result["Htc40"].ToString(),
                            Htc45 = Result["Htc45"].ToString(),
                            HtcTeus = Result["HtcTeus"].ToString(),
                            HtcTotal = Result["HtcTotal"].ToString(),
                            //    });
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstData.LstPvt.Add(new Dnd_Pvt
                        {
                            Pvt20 = Result["Pvt20"].ToString(),
                            Pvt40 = Result["Pvt40"].ToString(),
                            Pvt45 = Result["Pvt45"].ToString(),
                            PvtTeus = Result["PvtTeus"].ToString(),
                            PvtTotal = Result["PvtTotal"].ToString(),
                            //    });
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstData.LstCargoIn.Add(new Dnd_CargoIn
                        {
                            CargoIn20 = Result["CargoIn20"].ToString(),
                            //CargoIn40 = Result["CargoIn40"].ToString(),
                            //CargoIn45 = Result["CargoIn45"].ToString(),
                            //CargoInTeus = Result["CargoInTeus"].ToString(),
                            //CargoInTotal = Result["CargoInTotal"].ToString(),
                            //    });
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstData.LstCargoOut.Add(new Dnd_CargoOut
                        {
                            CargoOut20 = Result["CargoOut20"].ToString(),
                            //CargoOut40 = Result["CargoOut40"].ToString(),
                            //CargoOut45 = Result["CargoOut45"].ToString(),
                            //CargoOutTeus = Result["CargoOutTeus"].ToString(),
                            //CargoOutTotal = Result["CargoOutTotal"].ToString(),
                            //    });
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstData.LstSB.Add(new Dnd_SBDetails
                        {
                            TotalSB = Result["SBNo"].ToString(),
                            TotalPkg = Result["TotalPkg"].ToString(),
                            TotalWeight = Result["TotalWeight"].ToString(),
                            TotalFOB = Result["TotalFOB"].ToString(),
                         
                            //    });
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstData;
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
        public void GetRegisterofOutwardSupply(DateTime date1, DateTime date2)
        {

            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.DateTime, Value = date2 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //var objRegOutwardSupply = (List<RegisterOfOutwardSupplyModel>)DataAccess.ExecuteDynamicSet<List<RegisterOfOutwardSupplyModel>>("GetRegisterofOutwardSupply", DParam);
            DataSet ds = DataAccess.ExecuteDataSet("GetRegisterofOutward", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            List<Dnd_RegisterOfOutward> model = new List<Dnd_RegisterOfOutward>();

            try
            {
                if (ds.Tables.Count > 0)
                {
                    model = (from DataRow dr in dt.Rows
                             select new Dnd_RegisterOfOutward()
                             {
                                 SlNo = Convert.ToInt32(dr["SlNo"]),
                                 InvoiceType = dr["InvoiceType"].ToString(),
                                 GST = dr["GST"].ToString(),
                                 Name = dr["Name"].ToString(),
                                 Place = dr["Place"].ToString(),
                                 InvoiceNo = dr["InvoiceNo"].ToString(),
                                 InvoiceDate = dr["InvoiceDate"].ToString(),
                                 SACCode = dr["SACCode"].ToString(),
                                 description = dr["Description"].ToString(),
                                 Rate = dr["Rate"].ToString(),
                                 ServiceValue = Convert.ToDecimal(dr["ServiceValue"]),
                                 ITaxAmount = Convert.ToDecimal(dr["ITaxAmount"]),
                                 CTaxAmount = Convert.ToDecimal(dr["CTaxAmount"]),
                                 STaxAmount = Convert.ToDecimal(dr["STaxAmount"]),
                                 Cess = Convert.ToDecimal(dr["Cess"]),
                                 RoundUp = Convert.ToDecimal(dr["RoundUp"]),
                                 Total = Convert.ToDecimal(dr["Total"])
                             }).ToList();
                }

                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = RegisterofOutwardSupplyExcel(model, date1);
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
        private string RegisterofOutwardSupplyExcel(List<Dnd_RegisterOfOutward> model, DateTime date1)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var date = date1.ToString();
            var PeriodFromMonth = date.Split('/')[1];


            var printMonth = "";
            var printYear = "";


            switch (PeriodFromMonth)
            {
                //Here ds is list of invoice of a module between two dates 
                case "01":
                    printMonth = "JANUARY";

                    break;
                case "02":
                    printMonth = "FEBRUARY";

                    break;
                case "03":
                    printMonth = "MARCH";

                    break;
                case "04":
                    printMonth = "APRIL";

                    break;
                case "05":
                    printMonth = "MAY";

                    break;
                case "06":
                    printMonth = "JUNE";

                    break;
                case "07":
                    printMonth = "JULY";

                    break;
                case "08":
                    printMonth = "AUGUST";

                    break;
                case "09":
                    printMonth = "SEPTEMBER";

                    break;
                case "10":
                    printMonth = "OCTOBER";

                    break;
                case "11":
                    printMonth = "NOVEMBER";

                    break;
                case "12":
                    printMonth = "DECEMBER";

                    break;
            }


            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = "OUTWARD SUPPLY REGISTER OF CW CFS D'Node";
                exl.MargeCell("A1:M1", title, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A2:M2", "TAX INVOICE / BILL OF SUPPLY FOR THE MONTH OF " + printMonth + " -2019", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A3:A4", "SlNo", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B3:B4", "Outward Type", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C3:C4", "GSTIN of depositor/ customer", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D3:D4", "Name & Address of Customer to whom Service Rendered", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E3:E4", "Place of Supply", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F3:G3", "Invoice Details", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F4:F4", "Number", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G4:G4", "Date", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H3:H4", "HSN Code", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I3:I4", "Description of Supply", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J3:J4", "Rate of Tax", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K3:k4", "Total Value of Supply Before Tax ", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L3:O3", "Amount of Tax", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L4:L4", "IGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M4:M4", "Central Tax", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N4:N4", "State Tax", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O4:O4", "Cess", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("P3:P4", "RoundUp", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Q3:Q4", "Total Amount" + Environment.NewLine + "(Taxable Value+ Taxes+ Cess+RoundUp)", DynamicExcel.CellAlignment.Middle);
                for (var i = 65; i < 80; i++)
                {
                    char character = (char)i;
                    string text = character.ToString();
                    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                }
                /*exl.AddTable<InvoiceData>("A", 6, model.InvoiceData, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16 });
                exl.AddTable<CashReceiptData>("R", 6, model.CashReceiptData, new[] { 12, 30, 14, 14, 14, 14, 14, 40 });*/
                exl.AddTable<Dnd_RegisterOfOutward>("A", 5, model, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16, 12, 30, 14, 14, 14, 14, 14, 40 });
                var servicevalue = model.Sum(o => o.ServiceValue);
                var igstamt = model.Sum(o => o.ITaxAmount);
                var sgstamt = model.Sum(o => o.STaxAmount);
                var cgstamt = model.Sum(o => o.CTaxAmount);
                var totalamt = model.Sum(o => o.Total);
                exl.AddCell("K" + (model.Count + 5).ToString(), servicevalue, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("L" + (model.Count + 5).ToString(), igstamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("M" + (model.Count + 5).ToString(), sgstamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("N" + (model.Count + 5).ToString(), cgstamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("Q" + (model.Count + 5).ToString(), totalamt, DynamicExcel.CellAlignment.CenterRight);
                /*exl.AddCell("O" + (model.Count + 7).ToString(), "Invoice Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 7).ToString(), InvoiceAmount, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 8).ToString(), "Cash Receipt Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 8).ToString(), CRAmount, DynamicExcel.CellAlignment.CenterRight);*/
                exl.Save();
            }
            return excelFile;
        }

        #endregion

        #region SDBalance
        public void GetSDBalanceforReport(int PayeeId, int invid)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_invoiceId", MySqlDbType = MySqlDbType.Int32, Value = invid });

            //    lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("getSDBalanceForprint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            DndSDBalancePrint objSDBalance = new DndSDBalancePrint();
            try
            {


                while (Result.Read())
                {
                    Status = 1;
                    objSDBalance.SDBalance = Convert.ToDecimal(Result["SDBalanceAmount"]);

                }


                if (Status == 1)
                {
                    _DBResponse.Data = objSDBalance;
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
       
        public string GetPreviousInvDate(String DelAppNo)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_DeliApp", MySqlDbType = MySqlDbType.VarChar, Value = DelAppNo });

            //    lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetPreviousInvoiceDate", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            // PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
            try
            {


                while (Result.Read())
                {
                    Status = 1;
                    ReturnObj = Result["PDate"].ToString();

                }


                if (Status == 1)
                {
                    _DBResponse.Data = ReturnObj;
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
            return ReturnObj;
        }

        #endregion

        #region Cheque Summary Report
        public void ChequeSummary(Dnd_ChequeSummary ObjChequeSummary)
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
            IList<Dnd_ChequeSummary> LstChequeSummary = new List<Dnd_ChequeSummary>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstChequeSummary.Add(new Dnd_ChequeSummary
                    {



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
                    LstChequeSummary.Add(new Dnd_ChequeSummary
                    {



                        Bank = string.Empty,

                        Amount = LstChequeSummary.ToList().Sum(m => Convert.ToDecimal(m.Amount)).ToString(),

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

        public void ChequeCashDDPOSummary(Dnd_ChequeSummary ObjChequeSummary)
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
            IList<Dnd_ChequeSummary> LstChequeSummary = new List<Dnd_ChequeSummary>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstChequeSummary.Add(new Dnd_ChequeSummary
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
                    LstChequeSummary.Add(new Dnd_ChequeSummary
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



        #endregion

        #region Stuffing Pendency Report
        public void StuffingPendencyReportPrint(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("StuffingPendencyReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            List<Dnd_StuffingPendency> lstPV = new List<Dnd_StuffingPendency>();
            //try
            //{
            //    while (Result.Read())
            //    {
            //        Status = 1;
            //        lstPV.Add(new Dnd_TotalContainerReport
            //        {
            //            CFSCode = Result["CFSCode"].ToString(),
            //            ContainerNo = Result["ContainerNo"].ToString(),
            //            Size = Result["Size"].ToString(),
            //            ShippingLine = Result["ShippingLine"].ToString(),
            //            MovementNo = Result["MovementNo"].ToString(),
            //            MovementDate = Result["MovementDate"].ToString(),
            //            ViaNo = Result["ViaNo"].ToString(),
            //            Vessel = Result["Vessel"].ToString(),
            //        });
            //    }
            try
            {
               
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.Add(new Dnd_StuffingPendency
                        {
                            StuffingReqNo = Result["StuffingReqNo"].ToString(),
                            StuffingReqDate = Result["RequestDate"].ToString(),
                            ContainerNo= Result["ContainerNo"].ToString(),
                            CFSCode= Result["CFSCode"].ToString(),
                            Size = Result["Size"].ToString(),
                            ShippingLine = Result["ShippingLine"].ToString(),
                            InDate = Result["EntryDateTime"].ToString(),
                            NoOfSbs = Convert.ToInt32(Result["ShippingBillNo"]),
                            NoOfUnits =Convert.ToDecimal(Result["NoOfUnits"]),
                            GrossWeight = Convert.ToDecimal( Result["GrossWeight"]),
                            FOB = Convert.ToDecimal(Result["Fob"]),
                            Via= Result["Via"].ToString(),
                            Vessel= Result["Vessel"].ToString(),
                            Godown= Result["Godown"].ToString()
                            //    });
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
                    objCompanyDetails.EmailAddress = Convert.ToString(Result["EmailAddress"]).Replace("<br/>", " ");



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

        #endregion

        #region Container Arrival Report

        public void ContainerArrivalReport(Dnd_ContainerArrivalReport ObjContainerArrivalReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjContainerArrivalReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjContainerArrivalReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjContainerArrivalReport.ImportExport });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EmptyLoaded", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjContainerArrivalReport.EmptyLoaded });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerArrivalReport", CommandType.StoredProcedure, DParam);
            Dnd_ContainerArrivalReport LstContainerArrivalReport = new Dnd_ContainerArrivalReport();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            int SizeTwenty = 0;
            int SizeFourty = 0;
            int SizeFourtyFive = 0;
            try
            {
                while (Result.Read())
                {
                    Status = 1;                    

                    LstContainerArrivalReport.ListArrivalReport.Add(new Dnd_ArrivalReportList
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        LoadOrEmpty = Result["LoadOrEmpty"].ToString(),
                        SealNo = Result["CustomSealNo"].ToString(),
                        Date = Result["Date"].ToString(),
                        Time = Result["Time"].ToString(),
                        Commodity = Result["Commodity"].ToString(),
                        ImportExport = Result["OperationType"].ToString(),
                        Size = Convert.ToString(Result["Size"] == null ? "" : Result["Size"])
                        // Withdraw = Result["Withdraw"].ToString()

                    });
                }

                if (Status == 1)
                {
                    SizeTwenty = LstContainerArrivalReport.ListArrivalReport.Count(o => o.Size == "20");
                    SizeFourty = LstContainerArrivalReport.ListArrivalReport.Count(o => o.Size == "40");
                    SizeFourtyFive = LstContainerArrivalReport.ListArrivalReport.Count(o => o.Size == "45");
                    LstContainerArrivalReport.SizeTewnty = Convert.ToString(SizeTwenty);
                    LstContainerArrivalReport.SizeFourty = Convert.ToString(SizeFourty);
                    LstContainerArrivalReport.SizeFourtyFive = Convert.ToString(SizeFourtyFive);

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstContainerArrivalReport;
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

        #region Daily Transaction Cargo Shifting
        public void DailyTransactionShiftingPrint(Dnd_TotalContainerReport obj, string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = obj.GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyTransactionShifting", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();
           
            try
            {
                while (Result.Read()){
                    lstPV.mstcompany = Result["CompanyAddress"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                            EntryNo = Result["EntryNo"].ToString(),
                            ShippingBillNo = Result["ShippingBillNo"].ToString(),
                            ShippingBillDate = Result["ShippingBillDate"].ToString(),
                            TotalPkg = Convert.ToDecimal(Result["TotalPkg"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            Fob = Convert.ToDecimal(Result["Fob"]),
                            GodownName = Result["GodownName"].ToString(),
                            ShippingLineName = Result["ShippingLine"].ToString(),
                            ShippingLineAlias = Result["ShippingLineCode"].ToString(),
                            MovementNo = Result["ShiftNo"].ToString(),
                            MovementDate = Result["ShiftDate"].ToString(),
                            //    });
                        });
                    }
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

        #region Daily Transaction Cargo Receiving
        public void DailyTransactionReceivingPrint(Dnd_TotalContainerReport obj, string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = obj.GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyTransactionReceiving", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_TotalContainerReport lstPV = new Dnd_TotalContainerReport();

            try
            {
                while (Result.Read())
                {
                    lstPV.mstcompany = Result["CompanyAddress"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstTotal.Add(new Dnd_TotalContainer
                        {
                            EntryNo = Result["EntryNo"].ToString(),
                            ShippingBillNo = Result["ShippingBillNo"].ToString(),
                            ShippingBillDate = Result["ShippingBillDate"].ToString(),
                            TotalPkg = Convert.ToDecimal(Result["TotalPkg"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            Fob = Convert.ToDecimal(Result["Fob"]),
                            GodownName = Result["GodownName"].ToString(),
                            ShippingLineName = Result["ShippingLine"].ToString(),
                            ShippingLineAlias = Result["ShippingLineCode"].ToString(),
                            MovementNo = Result["ShiftNo"].ToString(),
                            MovementDate = Result["ShiftDate"].ToString(),
                            //    });
                        });
                    }
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
            List<EconomyReport> LstData = new List<EconomyReport>();
            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    LstData.Add(new EconomyReport
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
            EconomyRptPrint PrintData = new EconomyRptPrint();
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
                        PrintData.RptDetails.Add(new EconomyReport
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
                        PrintData.RptSummary.Add(new EconomyRptIncomeSummary
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

        #region WorkSlip
        public void WorkSlipDetailsForPrint(string PeriodFrom, string PeriodTo, int CasualLabour, int Uid = 0)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodFrom) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodTo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Labour", MySqlDbType = MySqlDbType.Int32, Value = CasualLabour });
            LstParam.Add(new MySqlParameter { ParameterName = "in_uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetWorkslipReport", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result != null && Result.Tables[0].Rows.Count > 0)
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

        #region Stock Register Report
        public void StockRegisterReport(Dnd_StockRegisterReportViewModel vm)
        {

            DateTime dtfrom = DateTime.ParseExact(vm.AsOnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
          

            int Status = 0;
            
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = vm.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom }); 
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStockRegisterReport", CommandType.StoredProcedure, DParam);
            Dnd_StockRegisterReportViewModel lstStockRegisterReportViewModel = new Dnd_StockRegisterReportViewModel();
          
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    lstStockRegisterReportViewModel.lstShipping.Add(new Dnd_StockRegiterShippingLine
                    {
                        Shippingid = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingName = Convert.ToString(Result["ShippingLineName"]),
                        SLACode = Convert.ToString(Result["EximTraderAlias"]),
                    });
                }
                if(Result.NextResult())
                {
                    while(Result.Read())
                    {
                        lstStockRegisterReportViewModel.StockDetails.Add(new Dnd_StockRegister
                        {
                            Area = Convert.ToDecimal(Result["Area"]),
                            CargoDesc = Convert.ToString(Result["CommodityName"]),
                            CartingDate = Convert.ToString(Result["RegisterDate"]),
                            Crg = Convert.ToString(Result["CRG"]),
                            EntryNo = Convert.ToString(Result["EntryNo"]),
                            FOB = Convert.ToDecimal(Result["Fob"]),
                            GWT = Convert.ToDecimal(Result["Weight"]),
                            NoOfPKG = Convert.ToDecimal(Result["Units"]),
                            SbDate = Convert.ToString(Result["ShippingBillDate"]),
                            SbNo = Convert.ToString(Result["ShippingBillNo"]),
                            Shippingid = Convert.ToInt32(Result["ShippingLineId"]),
                            ShippingName = Convert.ToString(Result["EximTraderName"]),
                            SlotNo=Convert.ToString(Result["LocationName"]),
                          

                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstStockRegisterReportViewModel;
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

        #region Stock Register Report SLA Wise
        public void StockRegisterReportSLAWise(Dnd_StockRegisterReportViewModel vm)
        {

            DateTime dtfrom = DateTime.ParseExact(vm.AsOnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");


            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = vm.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = vm.Shippingid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStockRegisterReportSLAWise", CommandType.StoredProcedure, DParam);
            Dnd_StockRegisterReportViewModel lstStockRegisterReportViewModel = new Dnd_StockRegisterReportViewModel();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    lstStockRegisterReportViewModel.lstShipping.Add(new Dnd_StockRegiterShippingLine
                    {
                        Shippingid = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingName = Convert.ToString(Result["ShippingLineName"]),
                        SLACode = Convert.ToString(Result["EximTraderAlias"]),
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstStockRegisterReportViewModel.StockDetails.Add(new Dnd_StockRegister
                        {
                            Area = Convert.ToDecimal(Result["Area"]),
                            CargoDesc = Convert.ToString(Result["CommodityName"]),
                            CartingDate = Convert.ToString(Result["RegisterDate"]),
                            Crg = Convert.ToString(Result["CRG"]),
                            EntryNo = Convert.ToString(Result["EntryNo"]),
                            FOB = Convert.ToDecimal(Result["Fob"]),
                            GWT = Convert.ToDecimal(Result["Weight"]),
                            NoOfPKG = Convert.ToDecimal(Result["Units"]),
                            SbDate = Convert.ToString(Result["ShippingBillDate"]),
                            SbNo = Convert.ToString(Result["ShippingBillNo"]),
                            Shippingid = Convert.ToInt32(Result["ShippingLineId"]),
                            ShippingName = Convert.ToString(Result["EximTraderName"]),
                            SlotNo = Convert.ToString(Result["LocationName"]),


                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstStockRegisterReportViewModel;
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

        #region Consolidated Stock Register Report
        public void ConsolidatedStockRegisterReport(Dnd_ConsolidatedStockRegister vm)
        {

            DateTime dtfrom = DateTime.ParseExact(vm.AsOnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");


            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();          
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetConsolidatedStockRegisterReport", CommandType.StoredProcedure, DParam);
            Dnd_ConsolidatedStockRegister lstStockRegisterReportViewModel = new Dnd_ConsolidatedStockRegister();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    lstStockRegisterReportViewModel.lstgodown.Add(new Dnd_ConsolidatedStockRegiterGodown
                    {
                           GodownId= Convert.ToInt32(Result["GodownId"]),
                         GodownName = Convert.ToString(Result["GodownName"])
                       
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstStockRegisterReportViewModel.StockDetails.Add(new Dnd_ConsolidatedStockRegisterDetails
                        {
                            Area = Convert.ToDecimal(Result["Area"]),
                            CargoDesc = Convert.ToString(Result["CommodityName"]),
                            CartingDate = Convert.ToString(Result["RegisterDate"]),
                            Crg = Convert.ToString(Result["CRG"]),
                            EntryNo = Convert.ToString(Result["EntryNo"]),
                            FOB = Convert.ToDecimal(Result["Fob"]),
                            GWT = Convert.ToDecimal(Result["Weight"]),
                            NoOfPKG = Convert.ToDecimal(Result["Units"]),
                            SbDate = Convert.ToString(Result["ShippingBillDate"]),
                            SbNo = Convert.ToString(Result["ShippingBillNo"]),
                            Shippingid = Convert.ToInt32(Result["ShippingLineId"]),
                            ShippingName = Convert.ToString(Result["EximTraderName"]),
                            SlotNo = Convert.ToString(Result["LocationName"]),
                            GodownName = Convert.ToString(Result["GodownName"]),
                            GodownId = Convert.ToInt32(Result["GodownId"]),

                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstStockRegisterReportViewModel;
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
        #region VESSEL CUT OFF REPORT
        public void GetAllPort()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //    LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            //  DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PortForVesselCutOff", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Dnd_VesselCutOffRpt> LstPort = new List<Dnd_VesselCutOffRpt>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new Dnd_VesselCutOffRpt
                    {
                       PortName = Result["PortName"].ToString(),
                        PortId = Convert.ToInt32(Result["PortId"]),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPort;
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
        public void VesselCutOffPrint(Dnd_VesselCutOffRpt ObjStuffing)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.PortId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjStuffing.AsOnDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("VesselCutOffReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Dnd_VesselCutOffRpt lstPV = new Dnd_VesselCutOffRpt();

            //while (Result.Read())
            //{
            //   
            //    lstPV.Add(new Dnd_TotalContainerReport
            //    {
            //        CFSCode = Result["CFSCode"].ToString(),
            //        ContainerNo= Result["ContainerNo"].ToString(),
            //        InDateTime = Convert.ToString(Result["InDateTime"]),
            //        Size = Result["Size"].ToString(),
            //        ShippingLine = Result["ShippingLine"].ToString(),
            //        Origin = Result["Origin"].ToString(),
            //        Status = Result["Status"].ToString(),
            //        ContClass = Result["ContClass"].ToString(),
            //        VehicleNo = Result["VehicleNo"].ToString(),
            //        Remarks =Result["Remarks"].ToString(),
            //        OutDate = Result["Outdate"].ToString(),
            //        ContainerType= Result["ContainerType"].ToString(),

            //    });
            //}
            try
            {
               
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.LstVessel.Add(new Dnd_VesselCutOffRpt
                        {
                            ViaNo = Result["VIA"].ToString(),
                            VesselName = Result["Vessel"].ToString(),
                            CutOffDate = Result["CutOffDate"].ToString(),
                            CutOffTime = Result["CutOffTime"].ToString(),
                            OpenDate= Result["OpenDate"].ToString(),
                            DepartureDate= Result["CutOffDate"].ToString(),
                            PortName= Result["PortOfLoadingName"].ToString(),



                            //    });
                        });
                    }
                


                /*
                        CrInvLedgerObj.ClosingBalance = (CrInvLedgerObj.OpenningBalance + (CrInvLedgerObj.lstLedgerSummary.Sum(o => o.Debit)))
                                                        - (CrInvLedgerObj.lstLedgerSummary.Sum(o => o.Credit));
                        */


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
    }
}