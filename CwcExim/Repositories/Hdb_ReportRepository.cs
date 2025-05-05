using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using CwcExim.Areas.Report.Models;
using System.Data;
using MySql.Data.MySqlClient;
using System.Globalization;
using Newtonsoft.Json;
using CwcExim.Areas.Import.Models;
using System.Dynamic;
using CwcExim.Areas.CashManagement.Models;

namespace CwcExim.Repositories
{
    public class Hdb_ReportRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        public void GetInvoiceDetailsForPrint(int InvoiceId, string InvoiceType = "GE")
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = InvoiceType });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBulkInvoiceDetailsForPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PpgInvoiceGate objPaymentSheet = new PpgInvoiceGate();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheet.CompanyName = Result["CompanyName"].ToString();
                    objPaymentSheet.CompanyShortName = Result["CompanyShortName"].ToString();
                    objPaymentSheet.CompanyAddress = Result["CompanyAddress"].ToString();
                    objPaymentSheet.CompanyGstNo = Result["GstIn"].ToString();
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objPaymentSheet.InvoiceNo = Result["InvoiceNo"].ToString();
                        objPaymentSheet.InvoiceDate = Result["InvoiceDate"].ToString();
                        objPaymentSheet.PartyName = Result["PartyName"].ToString();
                        objPaymentSheet.PartyState = Result["PartyState"].ToString();
                        objPaymentSheet.PartyAddress = Result["PartyAddress"].ToString();
                        objPaymentSheet.PartyStateCode = Result["PartyStateCode"].ToString();
                        objPaymentSheet.PartyGstNo = Result["PartyGSTNo"].ToString();
                        objPaymentSheet.TotalTax = Convert.ToDecimal(Result["TotalTaxable"]);
                        objPaymentSheet.TotalCGST = Convert.ToDecimal(Result["TotalCGST"]);
                        objPaymentSheet.TotalSGST = Convert.ToDecimal(Result["TotalSGST"]);
                        objPaymentSheet.TotalIGST = Convert.ToDecimal(Result["TotalIGST"]);
                        objPaymentSheet.TotalAmt = Convert.ToDecimal(Result["InvoiceAmt"]);

                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objPaymentSheet.LstContainersGate.Add(new PpgInvoiceContainerGate()
                        {
                            CfsCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            FromDate = Convert.ToString(Result["FromDate"]),
                            ToDate = Convert.ToString(Result["ToDate"])
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objPaymentSheet.LstChargesGate.Add(new PpgInvoiceChargeGate()
                        {
                            ChargeSD = Convert.ToString(Result["OperationSDesc"]),
                            ChargeDesc = Convert.ToString(Result["OperationDesc"]),
                            HsnCode = Convert.ToString(Result["SACCode"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            TaxableAmt = Convert.ToDecimal(Result["Taxable"]),

                            CGSTRate = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTRate = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            IGSTRate = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),

                        });
                    }
                }

                //-------------------------------------------------------------------------
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentSheet;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
                //-----------------------------------------------------------------------
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
        public void GetInvoicePayeeList()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoicePayeeList", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<WFLDImpPartyForpage> objPayeeName = new List<WFLDImpPartyForpage>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPayeeName.Add(new WFLDImpPartyForpage()
                    {
                        PartyId = Convert.ToInt32(Result["PayeeId"]),
                        PartyName = Convert.ToString(Result["PayeeName"]),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPayeeName;
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
        public void GenericBulkInvoiceDetailsForPrint(BulkInvoiceReport ObjBulkInvoiceReport)
        {

            if (ObjBulkInvoiceReport.InvoiceNumber == null)
            {
                ObjBulkInvoiceReport.InvoiceNumber = "";
            }
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

            if (ObjBulkInvoiceReport.InvoiceModule == "All")
            {
                ObjBulkInvoiceReport.InvoiceModule = "";
            }

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = ObjBulkInvoiceReport.InvoiceNumber });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjBulkInvoiceReport.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjBulkInvoiceReport.PayeeId });
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
        public void GetIssueSlipForPreview(int IssueSlipId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Value = IssueSlipId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DA.ExecuteDataReader("GetIssueSlipForPreview", CommandType.StoredProcedure, DParam);
            HdbIssueSlip ObjIssueSlip = new HdbIssueSlip();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjIssueSlip.TotalCWCDues = Convert.ToDecimal(Result["TotalCWCDues"] == DBNull.Value ? 0 : Result["TotalCWCDues"]);
                    ObjIssueSlip.CRNoDate = Convert.ToString(Result["CRNoDate"] == null ? "" : Result["CRNoDate"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.IssueSlipNo = Result["IssueSlipNo"].ToString();
                        ObjIssueSlip.LstIssueSlipRpt.Add(new HdbIssueSlipReport
                        {
                            IssueDate = Result["IssueDt"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                            ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            MarksNo = Convert.ToString(Result["MarksNo"] == null ? "" : Result["MarksNo"]),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString(),
                            Weight = Convert.ToString(Result["Weight"] == null ? "" : Result["Weight"]),
                            ArrivalDate = (Result["ArrivalDate"] == null ? "" : Result["ArrivalDate"]).ToString(),
                            DestuffingDate = (Result["DestuffingDate"] == null ? "" : Result["DestuffingDate"]).ToString(),
                            Godown = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString(),
                            Location = (Result["Location"] == null ? "" : Result["Location"]).ToString(),
                            GodwonAndLocation = (Result["GodownLocation"] == null ? "" : Result["GodownLocation"]).ToString(),
                            NoOfpackage = Convert.ToInt32(Result["NoOfPackages"].ToString()),
                            OBL = Result["BOLNo"].ToString()
                        });
                    }

                }
                if (Status == 1)
                {
                    _DBResponse.Data = ObjIssueSlip;
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
                Result.Dispose();
                Result.Close();
            }
        }

        public List<PpgInvoiceGate> GetBulkInvoiceDetailsForPrint(BulkInvoiceReport ObjBulkInvoiceReport)
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

            List<PpgInvoiceGate> objPaymentSheetList = new List<PpgInvoiceGate>();
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = ObjBulkInvoiceReport.InvoiceModule });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjBulkInvoiceReport.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = ObjBulkInvoiceReport.InvoiceNumber });

            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetBulkInvoiceDetailsForPrint", CommandType.StoredProcedure, DParam);
                IList<string> htmls = new List<string>();
                _DBResponse = new DatabaseResponse();




                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    PpgInvoiceGate objPaymentSheetInvoiceHeader = new PpgInvoiceGate();
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        objPaymentSheetInvoiceHeader.CompanyName = dr["CompanyName"].ToString();
                        objPaymentSheetInvoiceHeader.CompanyShortName = dr["CompanyShortName"].ToString();
                        objPaymentSheetInvoiceHeader.CompanyAddress = dr["CompanyAddress"].ToString();
                        objPaymentSheetInvoiceHeader.CompanyGstNo = dr["GstIn"].ToString();
                    }

                    foreach (DataRow dr in Result.Tables[1].Rows)
                    {
                        PpgInvoiceGate objPaymentSheet = new PpgInvoiceGate();
                        objPaymentSheet.CompanyName = objPaymentSheetInvoiceHeader.CompanyName;
                        objPaymentSheet.CompanyShortName = objPaymentSheetInvoiceHeader.CompanyShortName;
                        objPaymentSheet.CompanyAddress = objPaymentSheetInvoiceHeader.CompanyAddress;
                        objPaymentSheet.CompanyGstNo = objPaymentSheetInvoiceHeader.CompanyGstNo;

                        objPaymentSheet.InvoiceId = Convert.ToInt32(dr["InvoiceId"]);
                        objPaymentSheet.InvoiceNo = dr["InvoiceNo"].ToString();
                        objPaymentSheet.InvoiceDate = dr["InvoiceDate"].ToString();
                        objPaymentSheet.PartyName = dr["PartyName"].ToString();
                        objPaymentSheet.PartyState = dr["PartyState"].ToString();
                        objPaymentSheet.PartyAddress = dr["PartyAddress"].ToString();
                        objPaymentSheet.PartyStateCode = dr["PartyStateCode"].ToString();
                        objPaymentSheet.PartyGstNo = dr["PartyGSTNo"].ToString();
                        objPaymentSheet.TotalTax = Convert.ToDecimal(dr["TotalTaxable"]);
                        objPaymentSheet.TotalCGST = Convert.ToDecimal(dr["TotalCGST"]);
                        objPaymentSheet.TotalSGST = Convert.ToDecimal(dr["TotalSGST"]);
                        objPaymentSheet.TotalIGST = Convert.ToDecimal(dr["TotalIGST"]);
                        objPaymentSheet.TotalAmt = Convert.ToDecimal(dr["InvoiceAmt"]);

                        foreach (DataRow drContainer in Result.Tables[2].Rows)
                        {
                            if (objPaymentSheet.InvoiceId == Convert.ToInt32(drContainer["InoviceId"]))
                            {
                                objPaymentSheet.LstContainersGate.Add(new PpgInvoiceContainerGate()
                                {
                                    CfsCode = Convert.ToString(drContainer["CFSCode"]),
                                    ContainerNo = Convert.ToString(drContainer["ContainerNo"]),
                                    Size = Convert.ToString(drContainer["Size"]),
                                    FromDate = Convert.ToString(drContainer["FromDate"]),
                                    ToDate = Convert.ToString(drContainer["ToDate"])
                                });
                            }
                        }

                        foreach (DataRow drCharges in Result.Tables[3].Rows)
                        {
                            if (objPaymentSheet.InvoiceId == Convert.ToInt32(drCharges["InoviceId"]))
                            {
                                objPaymentSheet.LstChargesGate.Add(new PpgInvoiceChargeGate()
                                {
                                    ChargeSD = Convert.ToString(drCharges["OperationSDesc"]),
                                    ChargeDesc = Convert.ToString(drCharges["OperationDesc"]),
                                    HsnCode = Convert.ToString(drCharges["SACCode"]),
                                    Rate = Convert.ToDecimal(drCharges["Rate"]),
                                    TaxableAmt = Convert.ToDecimal(drCharges["Taxable"]),

                                    CGSTRate = Convert.ToDecimal(drCharges["CGSTPer"]),
                                    CGSTAmt = Convert.ToDecimal(drCharges["CGSTAmt"]),
                                    SGSTRate = Convert.ToDecimal(drCharges["SGSTPer"]),
                                    SGSTAmt = Convert.ToDecimal(drCharges["SGSTAmt"]),
                                    IGSTRate = Convert.ToDecimal(drCharges["IGSTPer"]),
                                    IGSTAmt = Convert.ToDecimal(drCharges["IGSTAmt"]),
                                    Total = Convert.ToDecimal(drCharges["Total"]),

                                });
                            }
                        }

                        objPaymentSheetList.Add(objPaymentSheet);
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

            return objPaymentSheetList;
        }

        #region BulkDebit/CreaditNote
        public void PrintDetailsForBulkCRNote(string PeriodFrom, string PeriodTo, string CRDR)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Todate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Value = CRDR });

            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintOfBulkDRNote", CommandType.StoredProcedure, DParam);
            PrintModelOfBulkCrCompany objCR = new PrintModelOfBulkCrCompany();


            int Status = 0;
            _DBResponse = new DatabaseResponse();
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
                        Status = 1;
                        objCR.lstCrParty.Add(new PrintModelOfBulkCrParty
                        {
                            PartyName = Result["PartyName"].ToString(),
                            PartyAddress = Result["PartyAddress"].ToString(),
                            PartyCityName = Result["PartyCityName"].ToString(),
                            PartyStateName = Result["PartyStateName"].ToString(),
                            PartyStateCode = Result["PartyStateCode"].ToString(),
                            PartyGSTIN = Result["PartyGSTIN"].ToString(),
                            CRNoteNo = Result["CRNoteNo"].ToString(),
                            CRNoteDate = Result["CRNoteDate"].ToString(),
                            InvoiceNo = Result["InvoiceNo"].ToString(),
                            InvoiceDate = Result["InvoiceDate"].ToString(),
                            TotalAmt = Convert.ToDecimal(Result["TotalAmt"]),
                            RoundUp = Convert.ToDecimal(Result["RoundUp"]),
                            GrandTotal = Convert.ToDecimal(Result["GrandTotal"]),
                            CRNoteId = Convert.ToInt32(Result["CRNoteId"]),
                            Remarks = Result["Remarks"].ToString(),
                            irn = Result["irn"].ToString(),
                            SignedQRCode = Result["SignedQRCode"].ToString(),
                            SupplyType = Result["SupplyType"].ToString(),
                        });

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
                            CRNoteId = Convert.ToInt32(Result["CRNoteId"]),
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
        public void PrintDetailsForPortwise(string Fdt, String Tdt)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_Fdt", MySqlDbType = MySqlDbType.Date, Value = Fdt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Tdt", MySqlDbType = MySqlDbType.Date, Value = Tdt });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PortWiseReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PortWiseReport SDResult = new PortWiseReport();
            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    SDResult.PortWiseReportList.Add(new PortWiseReport
                    {

                        SlaCode = Result["SLACode"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        RLoni = Convert.ToInt32(Result["RLoni"]),
                        ROth = Convert.ToInt32(Result["ROth"]),
                        RTkd = Convert.ToInt32(Result["RTkd"]),
                        TLoni = Convert.ToInt32(Result["TLoni"]),
                        TOth = Convert.ToInt32(Result["TOth"]),
                        TTkd = Convert.ToInt32(Result["TTkd"]),

                    });

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        if (Result["ContainerLoadType"].ToString() == "FCL")
                        {
                            SDResult.TotalFCL = Convert.ToInt32(Result["ContainerLoadTypeCount"]);
                        }
                        else if (Result["ContainerLoadType"].ToString() == "LCL")
                        {
                            SDResult.TotalLCL = Convert.ToInt32(Result["ContainerLoadTypeCount"]);
                        }



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
        # endregion
        public void GetInvoiceDetailsPrintByNo(string InvoiceNo, string InvoiceType)
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = InvoiceType });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataSet Result = new DataSet();
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();

            Result = DataAccess.ExecuteDataSet("GetInvoiceDetailsPrintByNo", CommandType.StoredProcedure, DParam);
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



            //-----------------------------------------------------------------------
        }




        public void InvoiceDetailsForPrint(BulkInvoiceReport ObjBulkInvoiceReport)
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
                Result = DataAccess.ExecuteDataSet("GetInvoiceDetailsPrintByNo", CommandType.StoredProcedure, DParam);
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


        public void GetInvoiceList(string FromDate, string ToDate, string invoiceType)
        {

            DateTime dtfrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");


            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.String, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.String, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = invoiceType });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("InvoiceListWithDate", CommandType.StoredProcedure, DParam);
            IList<invoiceLIst> LstInvoice = new List<invoiceLIst>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstInvoice.Add(new invoiceLIst
                    {



                        InvoiceNumber = Result["InvoiceNumber"].ToString()



                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = JsonConvert.SerializeObject(LstInvoice);
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
                    objCompanyDetails.EffectVersion = Convert.ToDecimal(Result["Version"]);
                    objCompanyDetails.VersionLogoFile = Convert.ToString(Result["Effectlogofile"]);



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

        public void GetPartyList()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPartyList", CommandType.StoredProcedure, DParam);
            List<dynamic> lstParty = new List<dynamic>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstParty.Add(new { PartyId = Convert.ToInt32(Result["PartyId"]), PartyName = Convert.ToString(Result["PartyName"]) });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstParty;
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
            Hdb_SDStatement ObjSDStatement = new Hdb_SDStatement();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        ObjSDStatement.LstSD.Add(new Hdb_SDList
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
            DataSet ds = DataAccess.ExecuteDataSet("GetRegisterofOutwardSupply", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            List<RegisterOfOutwardSupplyModel> model = new List<RegisterOfOutwardSupplyModel>();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    model = (from DataRow dr in dt.Rows
                             select new RegisterOfOutwardSupplyModel()
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
                                 WH = dr["WH"].ToString(),
                                 CRNoDate = dr["CRNoDate"].ToString(),
                                 Amount = Convert.ToDecimal(dr["Amount"]),

                                 Received = Convert.ToDecimal(dr["Received"]),
                                 Adjustment = Convert.ToDecimal(dr["Adjustment"]),
                                 ChequeNoDate = dr["CHDD"].ToString(),
                                 Remarks = dr["Remarks"].ToString()
                             }).ToList();
                }
                decimal InvoiceAmount = 0, CRAmount = 0;
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]);
                    CRAmount = Convert.ToDecimal(dr["CRAmount"]);
                }
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
        private string RegisterofOutwardSupplyExcel(List<RegisterOfOutwardSupplyModel> model, decimal InvoiceAmount, decimal CRAmount)
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
                exl.AddCell("N1", "BILL REGISTER", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A2:A4", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B2:B4", "GSTIN", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C2:C4", "Place of Supply" + Environment.NewLine + "(Name of State)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D2:D4", "Name of Customer to whom Service rendered", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E2:E4", "Period of Invoice", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F2:F4", "Nature of Invoice" + Environment.NewLine + "(Resv./Initial Fumigatiom/General Basic/Over & Above)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G2:G4", "Rate per" + Environment.NewLine + "Bag/MT/Sqm", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H2:J2", "Invoice Details", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H3:H4", "Invoice No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I3:I4", "Date of Invoice", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J3:J4", "Value of Service" + Environment.NewLine + "(Before Tax)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K2:P2", "Rate of Tax", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K3:L3", "IGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M3:N3", "CGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O3:P3", "SGST", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("K4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("L4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("M4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("N4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("O4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("P4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Q2:Q4", "Total Invoice Value" + Environment.NewLine + "(14=(10+12 or 10+14+16))", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("R2:U2", "Perticulars of Payment Received", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("R3:R4", "Received" + Environment.NewLine + "At WH/RO", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("S3:S4", "C.R No. & Date", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("T3:T4", "Cheque/DD No. & Date", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("U3:U4", "Amount of DD/Cheque (Rs.)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("V2:V4", "Amount Received Against Bill (Rs.)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("W2:W4", "Adjustment/Deduction", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("X2:X4", "Balance", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Y2:Y4", "Remarks", DynamicExcel.CellAlignment.Middle);
                for (var i = 65; i < 90; i++)
                {
                    char character = (char)i;
                    string text = character.ToString();
                    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                }
                /*exl.AddTable<InvoiceData>("A", 6, model.InvoiceData, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16 });
                exl.AddTable<CashReceiptData>("R", 6, model.CashReceiptData, new[] { 12, 30, 14, 14, 14, 14, 14, 40 });*/
                exl.AddTable<RegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16, 12, 30, 14, 14, 14, 14, 14, 40 });
                var igstamt = model.Sum(o => o.ITaxAmount);
                var sgstamt = model.Sum(o => o.STaxAmount);
                var cgstamt = model.Sum(o => o.CTaxAmount);
                var totalamt = model.Sum(o => o.Total);
                exl.AddCell("L" + (model.Count + 6).ToString(), igstamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("N" + (model.Count + 6).ToString(), cgstamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 6).ToString(), sgstamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("Q" + (model.Count + 6).ToString(), totalamt, DynamicExcel.CellAlignment.CenterRight);
                /*exl.AddCell("O" + (model.Count + 7).ToString(), "Invoice Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 7).ToString(), InvoiceAmount, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 8).ToString(), "Cash Receipt Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 8).ToString(), CRAmount, DynamicExcel.CellAlignment.CenterRight);*/
                exl.Save();
            }
            return excelFile;
        }

        #endregion

        #region Party Wise Unpaid Invoice
        public void PartyWiseUnpaidAmout(int PartyId, string AsOnDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.String, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PartyWiseUnpaidAmt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PartyWiseUnpaidDtl ObjDet = new PartyWiseUnpaidDtl();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDet.AsOnDate = AsOnDate.ToString();
                    ObjDet.PartyName = Result["PartyName"].ToString();
                    ObjDet.lstDtl.Add(new InvoiceDtl
                    {
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDet;
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
                Result.Close();
                Result.Dispose();
            }
        }
        #endregion

        #region All Party Unpaid Invoice
        public void PartyUnpaidCreditStatus(string AsOnDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.String, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("AllPartyUnpaidAmt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PartyUnpaidDetails> lstDet = new List<PartyUnpaidDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstDet.Add(new PartyUnpaidDetails
                    {
                        PartyName = Convert.ToString(Result["PartyName"]),
                        UnpaidAmt = Convert.ToDecimal(Result["UnpaidAmt"])
                    });
                }
                if (lstDet.Count > 0)
                {
                    lstDet[0].AsOnDate = AsOnDate.ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstDet;
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

        public void GetBulkCashreceiptForSDOpening(string FromDate, string ToDate, string ReceiptNo)
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
                Result = DataAccess.ExecuteDataSet("GetBulkCashRecptForPrintForSDOpening", CommandType.StoredProcedure, DParam);
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
        #endregion

        #region ContainerwiseInvoiceReport

        public void ContainerInvoiceDetailsForPrint(PPGContainerInvoiceReport ObjBulkInvoiceReport)
        {

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjBulkInvoiceReport.InvoiceId });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("getContainerwiseInvoiceforprint", CommandType.StoredProcedure, DParam);
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


        public void GetContainer()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerInvoice", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<PPGContainerInvoiceReport> objContainerLst = new List<PPGContainerInvoiceReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objContainerLst.Add(new PPGContainerInvoiceReport()
                    {
                        InvoiceId = Convert.ToInt32(Result["InoviceId"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objContainerLst;
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
        #region Cargo Stuffing Request
        public void CargoStuffingRegister(Hdb_CargoStuffingRegister ObjCargoStuffingRegister)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjCargoStuffingRegister.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjCargoStuffingRegister.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            IDataReader Result = DataAccess.ExecuteDataReader("CargoStuffingRegister", CommandType.StoredProcedure, DParam);
            IList<Hdb_CargoStuffingRegister> LstCargoStuffingRegister = new List<Hdb_CargoStuffingRegister>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstCargoStuffingRegister.Add(new Hdb_CargoStuffingRegister
                    {
                        Date = Result["StuffingDate"].ToString(),
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
                        ShippingBillDate = Result["ShippingDate"].ToString(),
                        ShippingBillSeal = Result["ShippingSeal"].ToString(),
                        Party = Result["Party"].ToString(),
                        ContainerNO = Result["ContainerNo"].ToString(),
                        Commodity = Result["Commodity"].ToString(),
                        NoOfPackage = Result["NoOfUnits"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                        Fob = Result["Fob"].ToString(),
                        StuffWeight = Result["StuffWeight"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCargoStuffingRegister;
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
        #region DailyCashBook
        public void DailyCashBook(string PeriodFrom, string PeriodTo)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(PeriodFrom).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(PeriodTo).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyCashBookReport", CommandType.StoredProcedure, DParam);
            IList<Hdb_DailyCashBook> LstDailyCashBook = new List<Hdb_DailyCashBook>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstDailyCashBook.Add(new Hdb_DailyCashBook
                    {
                        CRNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),
                        Depositor = Result["Party"].ToString(),
                        ChqNo = Result["ChequeNo"].ToString(),
                        Area = Convert.ToDecimal(Result["GenSpace"]),
                        GRE = Convert.ToDecimal(Result["GRE"]),
                        GRL = Convert.ToDecimal(Result["GRL"]),
                        Reefer = Convert.ToDecimal(Result["Reefer"]),
                        INS = Convert.ToDecimal(Result["INS"]),
                        STO = Convert.ToDecimal(Result["STO"]),
                        EscCharge = Convert.ToDecimal(Result["EscCharge"]),
                        Print = Convert.ToDecimal(Result["Print"]),
                        Royality = Convert.ToDecimal(Result["Royality"]),
                        Franchiese = Convert.ToDecimal(Result["Franchiese"]),
                        HT = Convert.ToDecimal(Result["HT"]),
                        LWB = Convert.ToDecimal(Result["LWB"]),
                        Cgst = Result["CGSTAmt"].ToString(),
                        Sgst = Result["SGSTAmt"].ToString(),
                        Igst = Result["IGSTAmt"].ToString(),
                        TotalCash = Result["TotalCash"].ToString(),
                        TotalCheque = Result["TotalCheque"].ToString(),
                        TotalNEFT = Convert.ToDecimal(Result["TotalNEFT"]),
                        TotalOth = Convert.ToDecimal(Result["TotalOth"]),
                        Tds = Result["tdsCol"].ToString(),
                        CrTds = Result["crTDS"].ToString(),
                        Roundoff = Convert.ToDecimal(Result["RoundUp"]),
                        AddmonyToSd = Convert.ToString(Result["AddMonyAmount"]),
                        RefundFromSd = Convert.ToString(Result["RefundAmount"])
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
            List<Kol_loadstuf> Lststufack = new List<Kol_loadstuf>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Kol_loadstuf
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
            List<Kol_loadstufasr> Lststufack = new List<Kol_loadstufasr>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Kol_loadstufasr
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
            List<Kol_loadstufdp> Lststufack = new List<Kol_loadstufdp>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Kol_loadstufdp
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
            List<Kol_loadstufdt> Lststufack = new List<Kol_loadstufdt>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Kol_loadstufdt
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

        public void MOnthlyCashBook(Hdb_MonthlyCashBook ObjDailyCashBook)
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
            IList<Hdb_MonthlyCashBook> LstMonthlyCashBook = new List<Hdb_MonthlyCashBook>();
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

                    LstMonthlyCashBook.Add(new Hdb_MonthlyCashBook
                    {
                        // CRNo = Result["CRNo"].ToString(),
                        ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),
                        // Result["ReceiptDate"].ToString(),
                        //Depositor = Result["Depositor"].ToString(),
                        CwcChargeTAX = Result["CwcChargeTAX"].ToString(),
                        CwcChargeNonTAX = Result["CwcChargeNonTAX"].ToString(),
                        CustomRevenueTAX = Result["CustomRevenueTAX"].ToString(),
                        CustomRevenueNonTAX = Result["CustomRevenueNonTAX"].ToString(),
                        InsuranceTAX = Result["InsuranceTAX"].ToString(),
                        InsuranceNonTAX = Result["InsuranceNonTAX"].ToString(),
                        PortTAX = Result["PortTAX"].ToString(),
                        PortNonTAX = Result["PortNonTAX"].ToString(),
                        LWB = Result["LWB"].ToString(),
                        CWCCGST = Result["CWCCGST"].ToString(),
                        CWCSGST = Result["CWCSGST"].ToString(),
                        CWCISGT = Result["CWCISGT"].ToString(),
                        HtTax = Result["HtTax"].ToString(),
                        HtNonTax = Result["HtNonTax"].ToString(),
                        HtCGST = Result["HtCGST"].ToString(),
                        HtSGST = Result["HtSGST"].ToString(),
                        HtISGT = Result["HtISGT"].ToString(),
                        RoPdRefund = Result["RoPdRefund"].ToString(),
                        MISC = Result["MISC"].ToString(),
                        TDSPlus = Result["TDSPlus"].ToString(),
                        Exempted = Result["Exempted"].ToString(),
                        PdaPLus = Result["PdaPLus"].ToString(),
                        TDSMinus = Result["TDSMinus"].ToString(),
                        PdaMinus = Result["PdaMinus"].ToString(),
                        HtAdjust = Result["HtAdjust"].ToString(),
                        BankDeposit = Result["BankDeposit"].ToString(),
                        RoundOff = Result["RoundUp"].ToString(),
                        RowTotal = Result["Total"].ToString(),
                        AddMoneySD = Convert.ToString(Result["AddMoneySD"]),
                        withdralfromSD = Convert.ToString(Result["WithdrawlFromSD"])


                        //Party = Result["Party"].ToString(),
                        //Deposit = Result["Deposit"].ToString(),
                        //Withdraw = Result["Withdraw"].ToString()

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
                        Date = Result["Date"].ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString(),
                        Party = Result["Party"].ToString(),
                        Deposit = Result["Deposit"].ToString(),
                        Withdraw = Result["Withdraw"].ToString()

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


        public void PdSummaryReport(Hdb_PdSummary ObjPdSummaryReport)
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
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PdSummaryReport", CommandType.StoredProcedure, DParam);
            IList<Hdb_PdSummary> LstPdSummaryReport = new List<Hdb_PdSummary>();
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

                    LstPdSummaryReport.Add(new Hdb_PdSummary
                    {



                        PartyName = Result["PartyName"].ToString(),

                        Amount = Result["Amount"].ToString()

                        //ContainerNo = Result["ContainerNo"].ToString(),
                        //value = Result["value"].ToString()

                    });
                }
                if (LstPdSummaryReport.Count > 0)
                {
                    LstPdSummaryReport.Add(new Hdb_PdSummary
                    {
                        PartyName = "<b>Total : </b>",
                        Amount = "<b>" + LstPdSummaryReport.Sum(x => Convert.ToDecimal(x.Amount)).ToString() + "</b>"
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
        public void ServiceCodeWiseInvDtls(Hdb_ServiceCodeWiseInvDtls ObjServiceCodeWiseInvDtls)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjServiceCodeWiseInvDtls.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjServiceCodeWiseInvDtls.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Registered", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInvoiceReportDetails.Registered });

            //  LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ServiceCodeWiseInvDtls", CommandType.StoredProcedure, DParam);
            List<Hdb_ServiceCodeWiseInvDtls> LstInvoiceReportDetails = new List<Hdb_ServiceCodeWiseInvDtls>();

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

                    LstInvoiceReportDetails.Add(new Hdb_ServiceCodeWiseInvDtls
                    {
                        SAC = Result["SAC"].ToString(),
                        InvoiceNumber = Result["InvoiceNo"].ToString(),
                        Date = Result["Date"].ToString(),
                        Values = Result["Value"].ToString(),
                        CGST = Result["CGSTAmt"].ToString(),
                        SGST = Result["SGSTAmt"].ToString(),
                        IGST = Result["IGSTAmt"].ToString(),
                        TotalValue = Result["TotalValue"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInvoiceReportDetails;
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
        public void IssueSlipReport(Hdb_IssueSlipReport ObjIssueSlipReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjIssueSlipReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjIssueSlipReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            IDataReader Result = DataAccess.ExecuteDataReader("IssueSlipReport", CommandType.StoredProcedure, DParam);
            IList<Hdb_IssueSlipReport> LstIssueSlipReport = new List<Hdb_IssueSlipReport>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstIssueSlipReport.Add(new Hdb_IssueSlipReport
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ContainerSize = Result["Size"].ToString(),
                        VesselName = Result["Vessel"].ToString(),
                        BOENo = Result["BOENo"].ToString(),
                        BoeDate = Result["BOEDate"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        ImporterExporter = Result["Importer"].ToString(),
                        CargoDescription = Result["CargoDescription"].ToString(),
                        MarksAndNo = Result["MarksNo"].ToString(),
                        NoOfUnits = Result["NoOfUnits"].ToString(),
                        Weight = Result["Weight"].ToString(),
                        PackageNo = Convert.ToDecimal(Result["PackageNo"]),
                        OBLNo = Result["OBLNo"].ToString(),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        RotationNo = Result["Rotation"].ToString(),

                        DeliveryNo = Result["DeliveryNo"].ToString(),

                        DateOfReceiptOfCont = Convert.ToString(Result["ArrivalDate"] == DBNull.Value ? "N/A" : Result["ArrivalDate"]),
                        //Result["ArrivalDate"].ToString(),
                        DestuffingDate = Result["DestuffingDate"].ToString(),
                        GridNo = Result["Location"].ToString(),//Location
                        TotalCWCDues = Result["TotalCWCDues"].ToString(),

                        CRNo = Result["ReceiptNo"].ToString(),
                        CRDate = Result["CRNoDate"].ToString(),
                        ValidTillDate = Result["ValidTillDate"].ToString()




                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstIssueSlipReport;
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
        public void GetPayeeNameforUnpaidInvoice()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPayeeNameforUnpaidInv", CommandType.StoredProcedure, DParam);
            List<dynamic> lstParty = new List<dynamic>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstParty.Add(new { PartyId = Convert.ToInt32(Result["PartyId"]), PartyName = Convert.ToString(Result["PartyName"]) });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstParty;
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


        #region invoice details
        public void GetInvoiceReportDetails(Hdb_InvoiceReportDetails ObjInvoiceReportDetails)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjInvoiceReportDetails.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjInvoiceReportDetails.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            /* string Flag = "";
             if (ObjInvoiceReportDetails.Registered == 0)
             {
                 Flag = "All";
             }
             else if (ObjInvoiceReportDetails.Registered == 1)
             {
                 Flag = "Tax";
             }
             else
             {
                 Flag = "Bill";
             }*/
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Registered", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInvoiceReportDetails.Registered });

            // LstParam.Add(new MySqlParameter { ParameterName = "in_Flag", MySqlDbType = MySqlDbType.VarChar, Value = Flag });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceReportDetails", CommandType.StoredProcedure, DParam);
            List<Hdb_InvoiceReportDetails> LstInvoiceReportDetails = new List<Hdb_InvoiceReportDetails>();

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

                    LstInvoiceReportDetails.Add(new Hdb_InvoiceReportDetails
                    {
                        InvoiceNumber = Result["InvoiceNo"].ToString(),
                        Date = Result["Date"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        GSTNo = Result["GstNo"].ToString(),
                        SAC = Result["SAC"].ToString(),
                        Values = Result["Value"].ToString(),
                        CGST = Result["CGSTAmt"].ToString(),
                        SGST = Result["SGSTAmt"].ToString(),
                        IGST = Result["IGSTAmt"].ToString(),
                        TotalValue = Result["TotalValue"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInvoiceReportDetails;
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

        #region Invoice Report Summary
        public void GetInvoiceReportSummary(Hdb_InvoiceReportSummary ObjInvoiceReportSummary)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjInvoiceReportSummary.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjInvoiceReportSummary.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            /* string Flag = "";
             if (ObjInvoiceReportSummary.Registered == 0)
             {
                 Flag = "All";
             }
             else if (ObjInvoiceReportSummary.Registered == 1)
             {
                 Flag = "Tax";
             }
             else
             {
                 Flag = "Bill";
             }*/
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Registered", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInvoiceReportSummary.Registered });

            //  LstParam.Add(new MySqlParameter { ParameterName = "in_Flag", MySqlDbType = MySqlDbType.VarChar, Value = Flag });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceReportSummary", CommandType.StoredProcedure, DParam);
            List<Hdb_InvoiceReportSummary> LstInvoiceReportSummary = new List<Hdb_InvoiceReportSummary>();

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

                    LstInvoiceReportSummary.Add(new Hdb_InvoiceReportSummary
                    {
                        InvoiceNumber = Result["InvoiceNo"].ToString(),
                        Date = Result["Date"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        GSTNo = Result["GstNo"].ToString(),
                        Amount = Result["Amount"].ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Result["ReceiptDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInvoiceReportSummary;
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
        #region Exemoted Service
        public void GetExemptedService(Hdb_ExemptedService ObjExemptedService)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjExemptedService.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjExemptedService.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Registered", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInvoiceReportDetails.Registered });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_Flag", MySqlDbType = MySqlDbType.VarChar, Value = Flag });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetExemptedService", CommandType.StoredProcedure, DParam);
            List<Hdb_ExemptedService> LstInvoiceReportDetails = new List<Hdb_ExemptedService>();

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

                    LstInvoiceReportDetails.Add(new Hdb_ExemptedService
                    {
                        InvoiceNumber = Result["InvoiceNo"].ToString(),
                        Date = Result["Date"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        GSTNo = Result["GstNo"].ToString(),
                        SAC = Result["SAC"].ToString(),
                        Values = Result["Value"].ToString(),
                        CGST = Result["CGSTAmt"].ToString(),
                        SGST = Result["SGSTAmt"].ToString(),
                        IGST = Result["IGSTAmt"].ToString(),
                        TotalValue = Result["TotalValue"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInvoiceReportDetails;
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
        public void DailyPdaActivity(Hdb_DailyPdaActivityReport ObjDailyPdaActivityReport)
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
            IList<Hdb_DailyPdaActivityReport> LstDailyPdaActivityReport = new List<Hdb_DailyPdaActivityReport>();
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

                    LstDailyPdaActivityReport.Add(new Hdb_DailyPdaActivityReport
                    {
                        Date = Result["Date"].ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString(),
                        Party = Result["Party"].ToString(),
                        Deposit = Result["Deposit"].ToString(),
                        Withdraw = Result["Withdraw"].ToString()

                    });
                }
                if (LstDailyPdaActivityReport.Count > 0)
                {
                    LstDailyPdaActivityReport.Add(new Hdb_DailyPdaActivityReport
                    {
                        Date = "<b>Total : </b>",
                        ReceiptNo = "",
                        Party = "",
                        Deposit = "<b>" + LstDailyPdaActivityReport.Sum(x => Convert.ToDecimal(x.Deposit)).ToString() + "</b>",
                        Withdraw = "<b>" + LstDailyPdaActivityReport.Sum(x => Convert.ToDecimal(x.Withdraw)).ToString() + "</b>",
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

        #region PV Report
        //public void GetPVReport(Hdb_PV objPV)
        //{
        //    IDataParameter[] DParam = { };
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objPV.Module });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objPV.GodownId });
        //    DParam = LstParam.ToArray();
        //    IDataReader Result = DataAccess.ExecuteDataReader("PVReport", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    int Status = 0;
        //    IList<Hdb_PVReport> lstPV = new List<Hdb_PVReport>();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            lstPV.Add(new Hdb_PVReport
        //            {
        //                Stack = Result["Stack"].ToString(),
        //                CFSCode = Result["CFSCode"].ToString(),
        //                PartyName = Result["PartyName"].ToString(),
        //                CommodityName = Result["CommodityName"].ToString(),
        //                NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
        //                Weight = Convert.ToDecimal(Result["Weight"]),
        //                ReceiptDate = Result["ReceiptDate"].ToString(),
        //                TSA = Result["TSA"].ToString(),
        //                BLNo = Result["BLNo"].ToString()
        //            });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = lstPV;
        //        }
        //        else
        //        {
        //            _DBResponse.Status = 0;
        //            _DBResponse.Message = "No Data";
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

        public void GetPVReportImport(Hdb_PV objPV)
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
            IList<Hdb_ImpPVReport> lstPV = new List<Hdb_ImpPVReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Hdb_ImpPVReport
                    {
                        BOLNo = Result["BOLNo"].ToString(),
                        BOLDate = Result["BOLDate"].ToString(),
                        CargoDescription = Result["CargoDescription"].ToString(),
                        DestuffingEntryDate = Result["DestuffingEntryDate"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        CommodityAlias = Result["CommodityAlias"].ToString(),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                        NoOfUnitsRec = Convert.ToInt32(Result["NoOfUnitsRec"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        Area = Convert.ToDecimal(Result["Area"].ToString()),
                        CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                        Duty = Convert.ToDecimal(Result["Duty"].ToString()),
                        LocationName = Result["LocationName"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                        EntryDate = Result["EntryDate"].ToString(),
                        LCLFCL = Result["LCLFCL"].ToString(),
                        Importer = Result["Importer"].ToString()
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
        public void GetPVReportExport(Hdb_PV objPV)
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
            IList<Hdb_ExpPVReport> lstPV = new List<Hdb_ExpPVReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Hdb_ExpPVReport
                    {
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
                        ShippingBillDate = Result["ShippingBillDate"].ToString(),
                        EntryNo = Result["EntryNo"].ToString(),
                        EntryDate = Result["EntryDate"].ToString(),
                        RegisterDate = Result["RegisterDate"].ToString(),
                        Units = Convert.ToInt32(Result["Units"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        Fob = Convert.ToDecimal(Result["Fob"]),
                        Area = Convert.ToDecimal(Result["Area"]),
                        EximTraderName = Convert.ToString(Result["EximTraderName"]== DBNull.Value ? "" : Result["EximTraderName"]),           
                        EximTraderAlias = Convert.ToString(Result["EximTraderAlias"] == DBNull.Value ? "" : Result["EximTraderAlias"]),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ?0 : Result["ShippingLineId"]),
                        LocationName = Result["LocationName"].ToString(),
                        ChaName = Result["CHA"].ToString(),
                        ExporterName = Result["ExporterName"].ToString(),
                        CargoDescription = Result["CargoDescription"].ToString(),
                        Remarks = Convert.ToString(Result["Remarks"]),
                        CartingNo = Convert.ToString(Result["CartingNo"]),
                        PackageType = Convert.ToString(Result["PackageType"]),
                        Measurement = Convert.ToString(Result["Measurement"])
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

        public void GetPVReportBond(Hdb_PV objPV)
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
            IList<Hdb_BondPVReport> lstPV = new List<Hdb_BondPVReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Hdb_BondPVReport
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
                        CHA = Result["CHA"].ToString(),
                        Location = Result["LocationName"].ToString(),
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

        #region SAC Register
        public void GetSACDetails(string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("SACRegister", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Hdb_SACRegister> lstSAC = new List<Hdb_SACRegister>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstSAC.Add(new Hdb_SACRegister
                    {
                        SacNo = Result["SacNo"].ToString(),
                        SacDate = Result["SacDate"].ToString(),
                        ValidUpto = Result["ValidUpto"].ToString(),
                        IMPName = Result["IMPName"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        SealNo = Result["SealNo"].ToString(),
                        BOLAWBNo = Result["BOLAWBNo"].ToString(),
                        BOENo = Result["BOENo"].ToString(),
                        BOEDate = Result["BOEDate"].ToString(),
                        CargoDescription = Result["CargoDescription"].ToString(),
                        NoOfUnits = Result["NoOfUnits"].ToString(),
                        AreaReserved = Result["AreaReserved"].ToString(),
                        InvoiceAmt = Result["InvoiceAmt"].ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Result["ReceiptDate"].ToString(),
                        Remarks = Result["Remarks"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstSAC;
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

        #region Bond Register
        public void GetBondNo()
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.Date, Value = DBNull.Value });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.Date, Value = DBNull.Value });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("BondRegister", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            List<BondDetails> lstBond = new List<BondDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBond.Add(new BondDetails
                    {
                        BondNo = Result["BondNo"].ToString(),
                        DepositAppId = Convert.ToInt32(Result["DepositAppId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstBond;
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
        public void GetBondRegister(int DepositeAppId, string FromDate, string ToDate)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = DepositeAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.Date, Value = (FromDate == "" ? null : Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd")) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.Date, Value = (ToDate == "" ? null : Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd")) });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("BondRegister", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Hdb_BondRegister objBond = new Hdb_BondRegister();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objBond.lstSACDetails.Add(new Hdb_SACDetails
                    {
                        CompanyAdd = Convert.ToString(Result["CompanyAddress"]),
                        CompanyEmail = Convert.ToString(Result["CompananyEmail"]),
                        DepositappId = Convert.ToInt32(Result["DepositAppId"].ToString()),
                        IMPName = Result["IMPName"].ToString(),
                        IMPAdd = Result["IMPAdd"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        CHAdd = Result["CHAdd"].ToString(),
                        SacNo = Result["SacNo"].ToString(),
                        SacDate = Result["SacDate"].ToString(),
                        ValidUpto = Result["ValidUpto"].ToString(),
                        AreaReserved = Convert.ToDecimal(Result["AreaReserved"]),
                        GodownName = Result["GodownName"].ToString(),
                        BondNo = Result["BondNo"].ToString(),
                        BondDate = Result["BondDate"].ToString(),
                        BondBOENo = Result["BondBOENo"].ToString(),
                        BondBOEDate = Result["BondBOEDate"].ToString(),
                        BondAWBNo = Result["BondAWBNo"].ToString(),
                        BondAWBDate = Result["BondAWBDate"].ToString(),
                        ExpDateofWarehouse = Result["ExpDateofWarehouse"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objBond.lstAdvPayment.Add(new SACAdvancePayment
                        {
                            SacNo = Result["StuffingReqNo"].ToString(),
                            InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]),
                            ReceiptNo = Result["ReceiptNo"].ToString(),
                            ReceiptDate = Result["ReceiptDate"].ToString(),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objBond.lstUnloadingDetails.Add(new UnloadingDetails
                        {
                            DepositappId = Convert.ToInt32(Result["DepositAppId"].ToString()),
                            UnloadedDate = Convert.ToString(Result["UnloadedDate"]),
                            UnloadedUnits = Convert.ToInt32(Result["UnloadedUnits"]),
                            UnloadedWeights = Convert.ToDecimal(Result["UnloadedWeights"]),
                            AreaOccupied = Convert.ToDecimal(Result["AreaOccupied"]),
                            PackageCondition = Result["PackageCondition"].ToString(),
                            Value = Convert.ToDecimal(Result["Value"]),
                            Duty = Convert.ToDecimal(Result["Duty"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objBond.lstDeliveryDetails.Add(new DeliveryDetails
                        {
                            DepositappId = Convert.ToInt32(Result["DepositAppId"].ToString()),
                            DeliveryOrderNo = Result["DeliveryOrderNo"].ToString(),
                            Units = Convert.ToInt32(Result["Units"]),
                            Weight = Convert.ToDecimal(Result["Weight"]),
                            SQM = Convert.ToDecimal(Result["SQM"]),
                            Value = Convert.ToDecimal(Result["Value"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            BondBOENo = Result["BondBOENo"].ToString(),
                            BondBOEDate = Result["BondBOEDate"].ToString(),
                            DeliveryOrderDate = Result["DeliveryOrderDate"].ToString(),
                            DeliveryOrderDtlId = Convert.ToInt32(Result["DeliveryOrderDtlId"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objBond.lstDeliveryPaymentDet.Add(new DeliveryPayementDetails
                        {
                            DepositappId = Convert.ToInt32(Result["DepositAppId"].ToString()),
                            InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]),
                            DeliveryDate = Convert.ToString(Result["DeliveryDate"]),
                            ReceiptNo = Result["ReceiptNo"].ToString(),
                            ReceiptDate = Result["ReceiptDate"].ToString(),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objBond;
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

        #region Stack Report

        public void GetBondNoForStackReport()
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("BondRegisterForStackReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            List<BondDetails> lstBond = new List<BondDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBond.Add(new BondDetails
                    {
                        BondNo = Result["BondNo"].ToString(),
                        DepositAppId = Convert.ToInt32(Result["DepositAppId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstBond;
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

        public void GetBondStackReport(int DepositeAppId)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = DepositeAppId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("BondRegisterForStackReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Hdb_BondStackReport objStack = new Hdb_BondStackReport();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objStack.lstBondDetails.Add(new Hdb_BondDetails
                    {
                        DepositappId = Convert.ToInt32(Result["DepositAppId"].ToString()),
                        IMPName = Result["IMPName"].ToString(),
                        IMPAdd = Result["IMPAdd"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        CHAdd = Result["CHAdd"].ToString(),
                        SacNo = Result["SacNo"].ToString(),
                        SacDate = Result["SacDate"].ToString(),
                        ValidUpto = Result["ValidUpto"].ToString(),
                        AreaReserved = Convert.ToDecimal(Result["AreaReserved"]),
                        GodownName = Result["GodownName"].ToString(),
                        BondNo = Result["BondNo"].ToString(),
                        BondDate = Result["BondDate"].ToString(),
                        BondBOENo = Result["BondBOENo"].ToString(),
                        BondBOEDate = Result["BondBOEDate"].ToString(),
                        ExpDateofWarehouse = Result["ExpDateofWarehouse"].ToString(),
                        CargoDesc = Result["CargoDesc"].ToString()
                    });
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        objStack.lstAdvPayment.Add(new SACAdvancePayment
                //        {
                //            SacNo = Result["StuffingReqNo"].ToString(),
                //            InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]),
                //            ReceiptNo = Result["ReceiptNo"].ToString(),
                //            ReceiptDate = Result["ReceiptDate"].ToString(),
                //        });
                //    }
                //}
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objStack.lstUnloadingDetails.Add(new UnloadingDetailsForStack
                        {
                            DepositappId = Convert.ToInt32(Result["DepositAppId"].ToString()),
                            UnloadedDate = Convert.ToString(Result["UnloadedDate"]),
                            UnloadedUnits = Convert.ToInt32(Result["UnloadedUnits"]),
                            UnloadedWeights = Convert.ToDecimal(Result["UnloadedWeights"]),
                            AreaOccupied = Convert.ToDecimal(Result["AreaOccupied"]),
                            PackageCondition = Result["PackageCondition"].ToString(),
                            Value = Convert.ToDecimal(Result["Value"]),
                            Duty = Convert.ToDecimal(Result["Duty"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objStack.lstDeliveryDetails.Add(new DeliveryDetailsForStack
                        {
                            DepositappId = Convert.ToInt32(Result["DepositAppId"].ToString()),
                            DeliveryOrderNo = Result["DeliveryOrderNo"].ToString(),
                            Units = Convert.ToInt32(Result["Units"]),
                            Weight = Convert.ToDecimal(Result["Weight"]),
                            SQM = Convert.ToDecimal(Result["SQM"]),
                            Value = Convert.ToDecimal(Result["Value"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            BondBOENo = Result["BondBOENo"].ToString(),
                            BondBOEDate = Result["BondBOEDate"].ToString(),
                            EXBOENo = Result["ExBOENo"].ToString(),
                            ExBOEDate = Result["ExBOEDate"].ToString(),
                        });
                    }
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        objBond.lstDeliveryPaymentDet.Add(new DeliveryPayementDetails
                //        {
                //            DepositappId = Convert.ToInt32(Result["DepositAppId"].ToString()),
                //            InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]),
                //            DeliveryDate = Convert.ToString(Result["DeliveryDate"]),
                //            ReceiptNo = Result["ReceiptNo"].ToString(),
                //            ReceiptDate = Result["ReceiptDate"].ToString(),
                //        });
                //    }
                //}
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objStack;
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

        #region Accounts Report for Import Cargo
        public void GetImpCargo(string AsOnDate, string GodownId)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(GodownId) });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpCargoReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Hdb_ImpCargo> lstPV = new List<Hdb_ImpCargo>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Hdb_ImpCargo
                    {
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
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        TotWk = Convert.ToInt32(Result["TotWk"]),
                        EntryDate = Result["EntryDate"].ToString(),
                        StorageArea = Result["StorageType"].ToString(),
                        LCLFCL = Result["LCLFCL"].ToString(),
                        Size = Result["Size"].ToString(),
                        InsuranceAmount = Convert.ToDecimal(Result["InsuranceAmount"]),
                        AssessmentValue = Convert.ToDecimal(Result["CIFValue"]),
                        DutyValue = Convert.ToDecimal(Result["Duty"]),
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

        #region Accounts Report for Import Cargo In Excel
        public void GetImportCargoInExcel(string AsOnDate, string GodownId, string GodownName)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(GodownId) });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpCargoReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            List<Hdb_ImpCargo> lstPV = new List<Hdb_ImpCargo>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Hdb_ImpCargo
                    {
                        SlNo = Convert.ToInt32(Result["SlNo"]),
                        BOLNo = Result["BOLNo"].ToString(),
                        DestuffingEntryDate = Result["DestuffingEntryDate"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        EntryDate = Result["EntryDate"].ToString(),
                        Size = Result["Size"].ToString(),
                        LCLFCL = Result["LCLFCL"].ToString(),
                        ForwarderName = Result["ForwarderName"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        StorageArea = Result["StorageType"].ToString(),
                        CommodityAlias = Result["CommodityAlias"].ToString(),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                        NoOfUnitsRec = Convert.ToInt32(Result["NoOfUnitsRec"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        Area = Convert.ToDecimal(Result["Area"].ToString()),
                        LocationName = Result["LocationName"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                        TotWk = Convert.ToInt32(Result["TotWk"]),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        InsuranceAmount = Convert.ToDecimal(Result["InsuranceAmount"]),
                        AssessmentValue = Convert.ToDecimal(Result["CIFValue"]),
                        DutyValue = Convert.ToDecimal(Result["Duty"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ImportCargoInExcel(lstPV, AsOnDate, GodownName);
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

        private string ImportCargoInExcel(List<Hdb_ImpCargo> lstPV, string AsOnDate,string GodownName)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "(A Government of India Undertaking)"
                      ;

                var Titl1 = "Import Accrued Income Statement As on " + AsOnDate + "";
                
                exl.MargeCell("A1:T1", title, DynamicExcel.CellAlignment.TopCenter);
                exl.MargeCell("A2:T2", "Container Freight Station, Kukatpally", DynamicExcel.CellAlignment.TopCenter);
                exl.MargeCell("A3:T3", "IDPL Road, Hyderabad - 37", DynamicExcel.CellAlignment.TopCenter);
                exl.MargeCell("A4:T4", "Email - cfs.kukatpally@cewacor.nic.in", DynamicExcel.CellAlignment.TopCenter);
                exl.MargeCell("A5:T5", "As On Date - " + AsOnDate, DynamicExcel.CellAlignment.TopCenter);
                exl.MargeCell("A6:T6", GodownName, DynamicExcel.CellAlignment.TopCenter);
                exl.MargeCell("A7:T7", "", DynamicExcel.CellAlignment.TopCenter);
                exl.MargeCell("A8:P8", Titl1, DynamicExcel.CellAlignment.TopCenter);                
                exl.MargeCell("A9:A9", "Sl No", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B9:B9", "OBL NO", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C9:C9", "Destuffing DATE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D9:D9", "Entry No", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E9:E9", "Entry Date", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F9:F9", "Size", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G9:G9", "FCL / LCL", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H9:H9", "Forwarder Name", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I9:I9", "CHA Name", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J9:J9", "Storage Type", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K9:K9", "Item No", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L9:L9", "No Of Pkg", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M9:M9", "Received Pkg", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N9:N9", "Gross Weight", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O9:O9", "Area", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("P9:P9", "Slot No", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Q9:Q9", "Remarks", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("R9:R9", "Total Wk", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("S9:S9", "Storage Amount", DynamicExcel.CellAlignment.Middle);               
                exl.MargeCell("T9:T9", "Assessment Value", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("U9:U9", "Duty Value", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("V9:V9", "Insurance Amount", DynamicExcel.CellAlignment.Middle);

                exl.AddTable("A", 10, lstPV, new[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10,10,10,10,10,10,10 });
                
                var TotalUnits = lstPV.Sum(o => o.NoOfUnits);
                var TotalUnitsReceived = lstPV.Sum(o => o.NoOfUnitsRec);
                var Totalweight = lstPV.Sum(o => o.Weight);                
                var TotalArea = lstPV.Sum(o => o.Area);
                var TotalWk = lstPV.Sum(o => o.TotWk);
                var TotalStorageAmt = lstPV.Sum(o => o.Amount);
                var TotalInsuranceAmt = lstPV.Sum(o => o.InsuranceAmount);
                var AssessmentValue = lstPV.Sum(o => o.AssessmentValue);
                var DutyValue = lstPV.Sum(o => o.DutyValue);

                exl.AddCell("A" + (lstPV.Count + 10).ToString(), "TOTAL ", DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("L" + (lstPV.Count + 10).ToString(), TotalUnits.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("M" + (lstPV.Count + 10).ToString(), TotalUnitsReceived.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("N" + (lstPV.Count + 10).ToString(), Totalweight.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("O" + (lstPV.Count + 10).ToString(), TotalArea.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("R" + (lstPV.Count + 10).ToString(), TotalWk.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("S" + (lstPV.Count + 10).ToString(), TotalStorageAmt.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("T" + (lstPV.Count + 10).ToString(), AssessmentValue.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("U" + (lstPV.Count + 10).ToString(), DutyValue.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("V" + (lstPV.Count + 10).ToString(), TotalInsuranceAmt.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);

                exl.Save();
            }
            return excelFile;
        }
        #endregion

        #region Account Report Export Cargo In General Carting
        public void GetCargoExport(string Date)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(Date).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ExpCarGenCarReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Hdb_ExpCargo> lstPV = new List<Hdb_ExpCargo>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Hdb_ExpCargo
                    {
                        EntryNo = Result["EntryNo"].ToString(),
                        InDate = Result["InDate"].ToString(),
                        SbNo = (Result["SbNo"]).ToString(),
                        SbDate = Result["SbDate"].ToString(),
                        Shed = Result["Shed"].ToString(),
                        Area = Convert.ToDecimal(Result["Area"]),
                        NoOfDays = Convert.ToInt32(Result["NoOfDays"]),
                        NoOfWeek = Convert.ToInt32(Result["NoOfWeek"]),
                        GeneralAmount = Convert.ToDecimal(Result["GeneralAmount"]),
                        InsuranceAmount = Convert.ToDecimal(Result["InsuranceAmount"]),
                        StorageType = Result["StorageArea"].ToString(),
                        ForwarderName = Result["ForwarderName"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        FCLLCL = Result["FCLLCL"].ToString()
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

        #region Account Report Export Cargo In General Carting in Excel
        public void GetCargoExportInExcel(string Date)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(Date).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ExpCarGenCarReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            int srno = 0;
            List<Hdb_ExpCargo> lstPV = new List<Hdb_ExpCargo>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    srno = srno + 1;
                    lstPV.Add(new Hdb_ExpCargo
                    {
                        SlNo = srno,
                        EntryNo = Result["EntryNo"].ToString(),
                        InDate = Result["InDate"].ToString(),
                        SbNo = (Result["SbNo"]).ToString(),
                        SbDate = Result["SbDate"].ToString(),
                        GodownName = Result["GodownName"].ToString(),
                        Shed = Result["Shed"].ToString(),
                        StorageType = Result["StorageArea"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        ForwarderName = Result["ForwarderName"].ToString(),
                        FCLLCL = Result["FCLLCL"].ToString(),
                        Area = Convert.ToDecimal(Result["Area"]),
                        Fob = Convert.ToDecimal(Result["Fob"]),
                        NoOfDays = Convert.ToInt32(Result["NoOfDays"]),
                        NoOfWeek = Convert.ToInt32(Result["NoOfWeek"]),
                        GeneralAmount = Convert.ToDecimal(Result["GeneralAmount"]),
                        InsuranceAmount = Convert.ToDecimal(Result["InsuranceAmount"]),
                                                
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ExportCargoInExcel(lstPV, Date); ; ;
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

        private string ExportCargoInExcel(List<Hdb_ExpCargo> lstPV, string AsOnDate)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "(A Government of India Undertaking)"
                      ;

                var Titl1 = "Export Accrued Income Statement As on " + AsOnDate + "";

                exl.MargeCell("A1:O1", title, DynamicExcel.CellAlignment.TopCenter);
                exl.MargeCell("A2:O2", "Container Freight Station, Kukatpally", DynamicExcel.CellAlignment.TopCenter);
                exl.MargeCell("A3:O3", "IDPL Road, Hyderabad - 37", DynamicExcel.CellAlignment.TopCenter);
                exl.MargeCell("A4:O4", "Email - cfs.kukatpally@cewacor.nic.in", DynamicExcel.CellAlignment.TopCenter);
                exl.MargeCell("A5:O5", "As On Date - " + AsOnDate, DynamicExcel.CellAlignment.TopCenter);                
                exl.MargeCell("A6:O6", "", DynamicExcel.CellAlignment.TopCenter);
                exl.MargeCell("A7:O7", Titl1, DynamicExcel.CellAlignment.TopCenter);
                exl.MargeCell("A8:A8", "Sl No", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B8:B8", "Entry No", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C8:C8", "In Date", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D8:D8", "SB No", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E8:E8", "SB Date", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F8:F8", "Godown", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G8:G8", "Shed", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H8:H8", "Storage Type", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I8:I8", "CHA Name", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J8:J8", "Forwarder Name", DynamicExcel.CellAlignment.Middle);                
                exl.MargeCell("K8:K8", "FCL / LCL", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L8:L8", "Area", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M8:M8", "FOB", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N8:N8", "No Of Days", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O8:O8", "No Of Week", DynamicExcel.CellAlignment.Middle);                              
                exl.MargeCell("P8:P8", "Storage Amount", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Q8:Q8", "Insurance Amount", DynamicExcel.CellAlignment.Middle);

                exl.AddTable("A", 9, lstPV, new[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 ,10 ,10});

                var TotalArea = lstPV.Sum(o => o.Area);
                var TotalFob = lstPV.Sum(o => o.Fob);
                var TotalDays = lstPV.Sum(o => o.NoOfDays);
                var TotalWeeks = lstPV.Sum(o => o.NoOfWeek);                
                var TotalStorageAmt = lstPV.Sum(o => o.GeneralAmount);
                var TotalInsuranceAmt = lstPV.Sum(o => o.InsuranceAmount);

                exl.AddCell("A" + (lstPV.Count + 9).ToString(), "TOTAL ", DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("L" + (lstPV.Count + 9).ToString(), TotalArea.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("M" + (lstPV.Count + 9).ToString(), TotalFob.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("N" + (lstPV.Count + 9).ToString(), TotalDays.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("O" + (lstPV.Count + 9).ToString(), TotalWeeks.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("P" + (lstPV.Count + 9).ToString(), TotalStorageAmt.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("Q" + (lstPV.Count + 9).ToString(), TotalInsuranceAmt.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);                

                exl.Save();
            }
            return excelFile;
        }

        #endregion

        #region Abstract Report
        public void GetAbstractReport(int Month, int Year)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.Int32, Value = Month });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Year", MySqlDbType = MySqlDbType.Int32, Value = Year });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("AbstractReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Hdb_AbstractReport ObjAbsR = new Hdb_AbstractReport();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAbsR.GivenMnthFirstDate = Result["GivenMnthFirstDate"].ToString();
                    ObjAbsR.CurrMonthlastDate = Result["CurrMonthlastDate"].ToString();
                    ObjAbsR.PrevTotalUnpaidInvoice = Convert.ToInt32(Result["PrevTotalUnpaidInvoice"]);
                    ObjAbsR.PrevTotalUnpaidAmt = Convert.ToDecimal(Result["PrevTotalUnpaidAmt"]);
                    ObjAbsR.CurrMnthTotalInvoice = Convert.ToInt32(Result["CurrMnthTotalInvoice"]);
                    ObjAbsR.CurrMnthTotalInvoiceAmt = Convert.ToDecimal(Result["CurrMnthTotalInvoiceAmt"]);
                    ObjAbsR.TotalOutstandingInv = Convert.ToInt32(Result["TotalOutstandingInv"]);
                    ObjAbsR.TotalOutstandingAmt = Convert.ToDecimal(Result["TotalOutstandingAmt"]);
                    ObjAbsR.TotalPaidPreInv = Convert.ToInt32(Result["TotalPaidPreInv"]);
                    ObjAbsR.TotalPrevinvoicePaidAmt = Convert.ToDecimal(Result["TotalPrevinvoicePaidAmt"]);
                    ObjAbsR.TotalPaidCurrMnthInv = Convert.ToInt32(Result["TotalPaidCurrMnthInv"]);
                    ObjAbsR.TotalPaidCurrMnthInvAmt = Convert.ToDecimal(Result["TotalPaidCurrMnthInvAmt"]);
                    ObjAbsR.TotalPaidInv = Convert.ToInt32(Result["TotalPaidInv"]);
                    ObjAbsR.TotalIncome = Convert.ToDecimal(Result["TotalIncome"]);
                    ObjAbsR.PrevPendingInv = Convert.ToInt32(Result["PrevPendingInv"]);
                    ObjAbsR.PrevPendingInvAmt = Convert.ToDecimal(Result["PrevPendingInvAmt"]);
                    ObjAbsR.CurrMnthPendingInv = Convert.ToInt32(Result["CurrMnthPendingInv"]);
                    ObjAbsR.CurrMnthPendingAmt = Convert.ToDecimal(Result["CurrMnthPendingAmt"]);
                    ObjAbsR.TotalPendingInv = Convert.ToInt32(Result["TotalPendingInv"]);
                    ObjAbsR.TotalPendingAmt = Convert.ToDecimal(Result["TotalPendingAmt"]);
                    ObjAbsR.CurrMnthTotalCrNote = Convert.ToInt32(Result["CurrMnthTotalCrNote"]);
                    ObjAbsR.CurrMnthTotalCrAmt = Convert.ToDecimal(Result["CurrMnthTotalCrAmt"]);
                    ObjAbsR.TotalBalancedInv = Convert.ToInt32(Result["TotalBalancedInv"]);
                    ObjAbsR.TotalBalancedAmt = Convert.ToDecimal(Result["TotalBalancedAmt"]);





                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjAbsR;
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



        #region Work Slip Report
        public void GetWorkSlipReportList(HDB_WorkSlipReport ObjWorkSlipReport)
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
            LstParam.Add(new MySqlParameter { ParameterName = "repoType", MySqlDbType = MySqlDbType.String, Value = ObjWorkSlipReport.WorkSlipType });

            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("RptWorkSlip", CommandType.StoredProcedure, DParam);

            HDB_WorkSlipReport newObjWorkSlipReport = new HDB_WorkSlipReport();

            newObjWorkSlipReport.WorkSlipSummaryList = new List<HDB_WorkSlipSummary>();
            newObjWorkSlipReport.WorkSlipDetailList = new List<HDB_WorkSlipDetail>();

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
                        HDB_WorkSlipSummary objWorkSlipSummary = new HDB_WorkSlipSummary();
                        objWorkSlipSummary.SrNo = count;
                        objWorkSlipSummary.Particulars = Convert.ToString(item["Particulars"]);
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
                        newObjWorkSlipReport.WorkSlipDetailList.Add(new HDB_WorkSlipDetail
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

        #region Export RR report
        //public void GetContainerForExportRR()
        //{
        //    int Status = 0;
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetContainerForExportRR", CommandType.StoredProcedure);
        //    _DBResponse = new DatabaseResponse();
        //    List<HDBExportRRReport> LstStuffing = new List<HDBExportRRReport>();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;

        //            if (LstStuffing.Count <= 0)
        //            {
        //                LstStuffing.Add(new HDBExportRRReport
        //                {
        //                    ContainerNo = Result["ContainerNo"].ToString(),
        //                    CFSCode = Result["CFSCode"].ToString()
        //                });
        //            }

        //            else
        //            {
        //                int flag = 0;

        //                for (int i1 = 0; i1 < LstStuffing.Count; i1++)
        //                {
        //                    if (LstStuffing[i1].ContainerNo == Result["ContainerNo"].ToString())
        //                    {
        //                        flag = 1;
        //                        break;
        //                    }
        //                }

        //                if (flag == 0)
        //                {
        //                    LstStuffing.Add(new HDBExportRRReport
        //                    {
        //                        ContainerNo = Result["ContainerNo"].ToString(),
        //                        CFSCode = Result["CFSCode"].ToString()
        //                    });
        //                }
        //            }
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = LstStuffing;
        //        }
        //        else
        //        {
        //            _DBResponse.Status = 0;
        //            _DBResponse.Message = "No Data";
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
        //        Result.Dispose();
        //        Result.Close();
        //    }
        //}

        public void GetGatePassForExportRR(string GatePassDate)
        {
            int Status = 0;

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassDate", MySqlDbType = MySqlDbType.String, Value = (GatePassDate == "" ? "" : Convert.ToDateTime(GatePassDate).ToString("yyyy-MM-dd")) });

            IDataParameter[] DParam = LstParam.ToArray();
            DParam = LstParam.ToArray();

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGatePassForExportRR", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<HDBExportRRReport> LstGatePass = new List<HDBExportRRReport>();

            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstGatePass.Add(new HDBExportRRReport
                    {
                        GatePassId = Convert.ToInt32(Result["GatePassId"]),
                        GatePassNo = Result["GatePassNo"].ToString()
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstGatePass;
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



        public void PrintExportRR(int GatePassId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.VarChar, Value = GatePassId });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintExportRR", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            HDBPrintExportRRReport ObjDeliveryOrder = new HDBPrintExportRRReport();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDeliveryOrder.LstPartyDetails.Add(new HdbPartyDetails
                    {
                        PartyName = Result["PartyName"].ToString(),
                        PartyAddress = Result["PartyAddress"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),
                        PayeeAddress = Result["PayeeAddress"].ToString()

                    });
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        ObjDeliveryOrder.LstContDetails.Add(new HdbContDetails
                        {
                            GatePassDate = Result["GatePassDate"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            VehicleNo = Result["VehicleNo"].ToString(),
                            NoOfPackages = Convert.ToInt32(Result["NoOfUnits"]),
                            GrossWt = Convert.ToDecimal(Result["Weight"]),
                            CustomSeal = Result["CustomSealNo"].ToString()
                        });
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstChargesDetails.Add(new HdbChargesDetails
                        {
                            FR = Convert.ToDecimal(Result["FR"]),
                            HND = Convert.ToDecimal(Result["HND"]),
                            TPT = Convert.ToDecimal(Result["TPT"].ToString()),
                            MSC = Convert.ToDecimal(Result["MSC"].ToString())
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstContainerDetails.Add(new HdbContainerDetails
                        {
                            ExporterName = Result["ExporterName"].ToString(),
                            ShippingLine = Result["ShippingLine"].ToString(),
                            ContainereNo = Result["ContainereNo"].ToString()
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstShipBillDetails.Add(new HdbShipBillDetails
                        {
                            ShippingBill = Result["ShippingBill"].ToString()
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDeliveryOrder;
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

        #region Escalation Report

        public void GetEscalationReport(string PeriodFrom, string PeriodTo)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(PeriodFrom).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(PeriodTo).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("EscalationReport", CommandType.StoredProcedure, DParam);
            IList<Hdb_EscalationReport> LstEsrp = new List<Hdb_EscalationReport>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstEsrp.Add(new Hdb_EscalationReport
                    {
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToDateTime(Result["InvoiceDate"] == DBNull.Value ? "N/A" : Result["InvoiceDate"]).ToString("dd/MM/yyyy"),
                        Party = Result["PartyName"].ToString(),
                        TaxableAmt = Convert.ToDecimal(Result["Taxable"]),
                        IGSTAmt = Convert.ToDecimal(Result["IGST"]),
                        CGSTAmt = Convert.ToDecimal(Result["CGST"]),
                        SGSTAmt = Convert.ToDecimal(Result["SGST"]),
                        TotalAmt = Convert.ToDecimal(Result["TotalAmt"])

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEsrp;
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



        #region Party Wise Urealized Invoice

        public void GetPartyForUnrealizedInv()
        {
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetUnRealizedInvParty", CommandType.StoredProcedure);
            IList<Hdb_UnRealizedInvParty> lstParty = new List<Hdb_UnRealizedInvParty>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstParty.Add(new Hdb_UnRealizedInvParty
                    {
                        PartyId = Convert.ToInt32(result["PayeeId"]),
                        Partyname = result["PayeeName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstParty;
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
        public void GetPartywiseUnrealized(Hdb_UnRealizedInvRpt ObjPV)
        {
            string dt = "";
            string dt1 = "";
            if ((ObjPV.FromDate != null) && (ObjPV.ToDate != null))
            {
                dt = Convert.ToDateTime(ObjPV.FromDate).ToString("yyyy-MM-dd");
                dt1 = Convert.ToDateTime(ObjPV.ToDate).ToString("yyyy-MM-dd");
            }
            else
            {
                dt = "";
                dt1 = "";
            }

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjPV.AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PatyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPV.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = dt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = dt1 });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPV.InvoiceType });
            IDataParameter[] Dpram = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DA.ExecuteDataReader("PartyWiseUnrealizedInv", CommandType.StoredProcedure, Dpram);
            List<Hdb_PartyWiseUnrelized> PartyWiseUnrelized = new List<Hdb_PartyWiseUnrelized>();
            // List<OperationList> LstOperationName = new List<OperationList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                //while (Result.Read())
                //{
                //    LstOperationName.Add(new OperationList
                //    {
                //        OperationName = (Result["variable"] == null ? "" : Result["variable"]).ToString()
                //    });
                //}
                //if (Result.NextResult())
                //{ 
                while (Result.Read())
                {
                    Status = 1;
                    PartyWiseUnrelized.Add(new Hdb_PartyWiseUnrelized
                    {
                        PartyName = Result["PayeeName"].ToString(),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        PayeeId = Convert.ToInt32(Result["payeeId"]),

                    });
                }



                /*if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstJobOrder.Add(new PrintJobOrder
                        {
                            NoOfUnits = Result["NoOfUnits"].ToString(),
                            Location = Result["Location"].ToString(),
                            PartyName = Result["PartyName"].ToString(),
                            NatureOfOperation = Result["NatureOfOperation"].ToString(),
                            BOENo = Result["BOENo"].ToString()
                        });
                    }                   
                } */
                if (Status == 1)
                {
                    _DBResponse.Data = PartyWiseUnrelized;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "No Data Found As On Selected Date";
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
                Result.Dispose();
                Result.Close();
            }
        }
        #endregion

        #region Abstract Accured Report
        public void GetAbstractAccuredReport(int Month, int Year)
        {
            IDataParameter[] Dparam = { };
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.Int32, Value = Month });
            lstParam.Add(new MySqlParameter { ParameterName = "in_year", MySqlDbType = MySqlDbType.Int32, Value = Year });
            Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("AbstractAccuredReport", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Hdb_AbstractAccuredReport ObjAbR = new Hdb_AbstractAccuredReport();
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

                    ObjAbR.lstInv.Add(new Hdb_AbstractAccuredReport
                    {
                        InvType = Result["InvType"].ToString(),
                        Headname = Result["Chargename"].ToString(),
                        CGST = Convert.ToDecimal(Result["CGSTAmt"]),
                        SGST = Convert.ToDecimal(Result["SGSTAmt"]),
                        RoundOff = Convert.ToDecimal(Result["RoundUp"]),
                        SGSTCGST = Convert.ToDecimal(Result["CGSTSGST"]),
                        IGST = Convert.ToDecimal(Result["IGSTAmt"]),
                        Total = Convert.ToDecimal(Result["Total"])


                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAbR.lstCr.Add(new Hdb_AbstractAccuredReport
                        {
                            InvType = Result["InvType"].ToString(),
                            Headname = Result["Chargename"].ToString(),
                            CGST = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGST = Convert.ToDecimal(Result["SGSTAmt"]),
                            RoundOff = Convert.ToDecimal(Result["RoundUp"]),
                            SGSTCGST = Convert.ToDecimal(Result["CGSTSGST"]),
                            IGST = Convert.ToDecimal(Result["IGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"])


                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjAbR;
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

        #region FormOneReport

        public void GetFormOneReport(string fdt, string tdt)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();

            LstParam.Add(new MySqlParameter { ParameterName = "in_Todate", MySqlDbType = MySqlDbType.Date, Value = fdt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Fromdate", MySqlDbType = MySqlDbType.Date, Value = tdt });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllFormOne", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Hdb_FormOneReport> lstPV = new List<Hdb_FormOneReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Hdb_FormOneReport
                    {
                        Size = Convert.ToInt32(Result["Size"]),
                        CBTContainerNo = Result["ContainerNo"].ToString(),
                        FormOneDate = Result["FormOneDate"].ToString(),
                        LCLFCL = Result["LCLFCL"].ToString(),
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
                    _DBResponse.Status = 2;
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
            HdbSDDetailsStatement SDResult = new HdbSDDetailsStatement();
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
                            new HdbSDInvoiceDet
                            {
                                //Sl, InvoiceNo, InvoiceDate, ReceiptNo, ReceiptDate, ReceiptAmt, TranType, TranAmt
                                SL = Convert.ToInt32(Result["Sl"]),
                                InvoiceNo = Result["InvoiceNo"].ToString(),
                                InvoiceDate = Result["InvoiceDate"].ToString(),
                                ReceiptNo = Result["ReceiptNo"].ToString(),
                                Mode = Result["Modes"].ToString(),
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

        public void GetLedgerDetStatement(int PartyId, string Fdt, String Tdt)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SDPartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Fdt", MySqlDbType = MySqlDbType.Date, Value = Fdt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Tdt", MySqlDbType = MySqlDbType.Date, Value = Tdt });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLedgerStatement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            HdbSDDetailsStatement SDResult = new HdbSDDetailsStatement();
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
                            new HdbSDInvoiceDet
                            {
                                //Sl, InvoiceNo, InvoiceDate, ReceiptNo, ReceiptDate, ReceiptAmt, TranType, TranAmt
                                SL = Convert.ToInt32(Result["Sl"]),
                                InvoiceNo = Result["InvoiceNo"].ToString(),
                                InvoiceDate = Result["InvoiceDate"].ToString(),
                                ReceiptNo = Result["ReceiptNo"].ToString(),
                                ReceiptDate = Result["ReceiptDate"].ToString(),
                                RealisedInvoiceNo = Result["RealisedInv"].ToString(),
                                ReceiptAmt = Convert.ToDecimal(Result["ReceiptAmt"]),
                                TranType = Result["TranType"].ToString(),
                                TranAmt = Convert.ToDecimal(Result["TranAmt"]),
                                CrAdjust = Convert.ToInt32(Result["CrAdjust"])


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

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        SDResult.SDBalance = Convert.ToDecimal(Result["SDBalance"]);
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

        #region TDS Report

        public void GetEximTraderForPartyLedger()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPayeeForTDS", CommandType.StoredProcedure, DParam);
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

        public void TdsReport(Hdb_TDSReport ObjTDSReport)
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
            Hdb_TDSMain objTDSMain = new Hdb_TDSMain();
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

                    objTDSMain.TDSReportLst.Add(new Hdb_TDSReport
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
                        ReceiptAmount = Result["ReceiptAmt"].ToString(),


                    });
                }
                if (Status == 1)
                {
                    objTDSMain.TDSReportLst.Add(new Hdb_TDSReport
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
                        ReceiptAmount = "<strong>" + objTDSMain.TDSReportLst.Select(m => new { CRNo = m.CRNo, ReceiptAmount = m.ReceiptAmount }).ToList().Distinct().ToList().Sum(m => Convert.ToDecimal(m.ReceiptAmount)).ToString() + "</strong>"


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
                    if (Status == 1)
                    {
                        objTDSMain.ObjTDSReporPartyWise.Add(
                                                new TDSReporPartyWise
                                                {
                                                    Party = "<strong>Total</strong>",
                                                    Tan = string.Empty,
                                                    Value = "<strong>" + objTDSMain.ObjTDSReporPartyWise.ToList().Sum(m => Convert.ToDecimal(m.Value)).ToString() + "</strong>",
                                                    TDS = "<strong>" + objTDSMain.ObjTDSReporPartyWise.ToList().Sum(m => Convert.ToDecimal(m.TDS)).ToString() + "</strong>",
                                                    TDSPlus = "<strong>" + objTDSMain.ObjTDSReporPartyWise.ToList().Sum(m => Convert.ToDecimal(m.TDSPlus)).ToString() + "</strong>",
                                                });
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

        #region UnRealized Invoice Summary

        public void GetUnRealizedInvSummary(string AsOnDt, string InvoiceType, string FromDate, string ToDate)
        {
            int Status = 0;
            string dt = "";
            string dt1 = "";
            if ((FromDate != "") && (ToDate != ""))
            {
                dt = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
                dt1 = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd");
            }
            else
            {
                dt = "";
                dt1 = "";
            }
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(AsOnDt).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = dt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = dt1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("UnRealizedInvoiceSummary", CommandType.StoredProcedure, DParam);
            IList<UnrealizedInvSummary> LstEsrp = new List<UnrealizedInvSummary>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstEsrp.Add(new UnrealizedInvSummary
                    {
                        PartyName = Result["PayeeName"].ToString(),
                        Amount = Convert.ToDecimal(Result["Amount"]),

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEsrp;
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

        #region Register of Outward 
        public void GetRegisterofOutward(DateTime date1, DateTime date2)
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
            List<Hdb_RegisterOfOutwardSupply> model = new List<Hdb_RegisterOfOutwardSupply>();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    model = (from DataRow dr in dt.Rows
                             select new Hdb_RegisterOfOutwardSupply()
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
                                 RoundUp = Convert.ToDecimal(dr["RoundUp"]),
                                 Total = Convert.ToDecimal(dr["Total"]),
                                 WH = dr["WH"].ToString(),
                                 CRNoDate = dr["CRNoDate"].ToString(),
                                 Amount = Convert.ToDecimal(dr["Amount"]),

                                 Received = Convert.ToDecimal(dr["Received"]),
                                 Adjustment = Convert.ToDecimal(dr["Adjustment"]),
                                 ChequeNoDate = dr["CHDD"].ToString()
                             }).ToList();
                }
                decimal InvoiceAmount = 0, CRAmount = 0;
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]);
                    CRAmount = Convert.ToDecimal(dr["CRAmount"]);
                }
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = RegisterofOutwardExcel(model, InvoiceAmount, CRAmount);
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
        private string RegisterofOutwardExcel(List<Hdb_RegisterOfOutwardSupply> model, decimal InvoiceAmount, decimal CRAmount)
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
                exl.AddCell("N1", "BILL REGISTER", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A2:A4", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B2:B4", "GSTIN", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C2:C4", "Place of Supply" + Environment.NewLine + "(Name of State)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D2:D4", "Name of Customer to whom Service rendered", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E2:E4", "Period of Invoice", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F2:F4", "Nature of Invoice" + Environment.NewLine + "(Resv./Initial Fumigatiom/General Basic/Over & Above)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G2:G4", "Rate per" + Environment.NewLine + "Bag/MT/Sqm", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H2:J2", "Invoice Details", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H3:H4", "Invoice No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I3:I4", "Date of Invoice", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J3:J4", "Value of Service" + Environment.NewLine + "(Before Tax)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K2:P2", "Rate of Tax", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K3:L3", "IGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M3:N3", "CGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O3:P3", "SGST", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("K4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("L4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("M4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("N4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("O4", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("P4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("Q4", "RoundUp", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("R2:R4", "Total Invoice Value" + Environment.NewLine + "(18=(10+12+17 or 10+14+16+17))", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("S2:S2", "Perticulars of Payment Received", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("S3:S4", "Received" + Environment.NewLine + "At WH/RO", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("T3:T4", "C.R No. & Date", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("U3:U4", "Cheque/DD No. & Date", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("V3:V4", "Amount of DD/Cheque (Rs.)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("W2:W4", "Amount Received Against Bill (Rs.)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("X2:X4", "Adjustment/Deduction", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Y2:Y4", "Balance", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Z2:Z4", "Remarks", DynamicExcel.CellAlignment.Middle);
                for (var i = 65; i < 90; i++)
                {
                    char character = (char)i;
                    string text = character.ToString();
                    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                }
                /*exl.AddTable<InvoiceData>("A", 6, model.InvoiceData, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16 });
                exl.AddTable<CashReceiptData>("R", 6, model.CashReceiptData, new[] { 12, 30, 14, 14, 14, 14, 14, 40 });*/
                exl.AddTable<Hdb_RegisterOfOutwardSupply>("A", 6, model, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16, 12, 30, 14, 14, 14, 14, 14, 40, 40 });
                var igstamt = model.Sum(o => o.ITaxAmount);
                var sgstamt = model.Sum(o => o.STaxAmount);
                var cgstamt = model.Sum(o => o.CTaxAmount);
                var roundup = model.Sum(o => o.RoundUp);
                var totalamt = model.Sum(o => o.Total);
                var val = model.Sum(o => o.ServiceValue);
                //exl.AddCell("J" + (model.Count + 6).ToString(), igstamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("J" + (model.Count + 6).ToString(), val, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("L" + (model.Count + 6).ToString(), igstamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("N" + (model.Count + 6).ToString(), cgstamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 6).ToString(), sgstamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("Q" + (model.Count + 6).ToString(), roundup, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("R" + (model.Count + 6).ToString(), totalamt, DynamicExcel.CellAlignment.CenterRight);
                /*exl.AddCell("O" + (model.Count + 7).ToString(), "Invoice Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 7).ToString(), InvoiceAmount, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 8).ToString(), "Cash Receipt Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 8).ToString(), CRAmount, DynamicExcel.CellAlignment.CenterRight);*/
                exl.Save();
            }
            return excelFile;
        }

        #endregion

        #region Abstract Accured Report Taxable
        public void GetAbstractAccuredReportTaxable(int Month, int Year)
        {
            IDataParameter[] Dparam = { };
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.Int32, Value = Month });
            lstParam.Add(new MySqlParameter { ParameterName = "in_year", MySqlDbType = MySqlDbType.Int32, Value = Year });
            Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("AbstractAccuredReportTaxable", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Hdb_AbstractAccuredReport ObjAbR = new Hdb_AbstractAccuredReport();
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

                    ObjAbR.lstInv.Add(new Hdb_AbstractAccuredReport
                    {
                        InvType = Result["InvType"].ToString(),
                        Headname = Result["Chargename"].ToString(),
                        CGST = Convert.ToDecimal(Result["CGSTAmt"]),
                        SGST = Convert.ToDecimal(Result["SGSTAmt"]),
                        RoundOff = Convert.ToDecimal(Result["RoundUp"]),
                        SGSTCGST = Convert.ToDecimal(Result["CGSTSGST"]),
                        IGST = Convert.ToDecimal(Result["IGSTAmt"]),
                        Total = Convert.ToDecimal(Result["Total"]),
                        CGSTTaxable = Convert.ToDecimal(Result["CGSTTaxable"]),
                        IGSTTaxable = Convert.ToDecimal(Result["IGSTTaxable"]),
                        BillTaxable = Convert.ToDecimal(Result["BillTaxable"]),
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAbR.lstCr.Add(new Hdb_AbstractAccuredReport
                        {
                            InvType = Result["InvType"].ToString(),
                            Headname = Result["Chargename"].ToString(),
                            CGST = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGST = Convert.ToDecimal(Result["SGSTAmt"]),
                            RoundOff = Convert.ToDecimal(Result["RoundUp"]),
                            SGSTCGST = Convert.ToDecimal(Result["CGSTSGST"]),
                            IGST = Convert.ToDecimal(Result["IGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            CGSTTaxable = Convert.ToDecimal(Result["CGSTTaxable"]),
                            IGSTTaxable = Convert.ToDecimal(Result["IGSTTaxable"]),
                            BillTaxable = Convert.ToDecimal(Result["BillTaxable"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAbR.CGSTRoundUp = Convert.ToDecimal(Result["CGSTRoundUp"]);
                        ObjAbR.IGSTRoundUp = Convert.ToDecimal(Result["IGSTRoundUp"]);
                        ObjAbR.BillRoundUp = Convert.ToDecimal(Result["BillRoundUp"]);
                        ObjAbR.TotalRoundUp = Convert.ToDecimal(Result["TotalRoundUp"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjAbR;
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


        #region Abstract Realised (Annexure -III) Report
        public void GetAbstractRealisedReport(int Month, int Year)
        {
            IDataParameter[] Dparam = { };
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.Int32, Value = Month });
            lstParam.Add(new MySqlParameter { ParameterName = "in_year", MySqlDbType = MySqlDbType.Int32, Value = Year });
            Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("RealisedInvoiceReport", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Hdb_AbstracRealisedReport ObjAbR = new Hdb_AbstracRealisedReport();
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

                    ObjAbR.lstInv.Add(new Hdb_AbstracRealisedReport
                    {
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]),
                        ReceiptDate = Result["ReceiptDate"].ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString(),


                    });
                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjAbR;
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


        #region Abstract Raised (Annexure -IV) Report
        public void GetAbstractRaisedReport(int Month, int Year)
        {
            IDataParameter[] Dparam = { };
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.Int32, Value = Month });
            lstParam.Add(new MySqlParameter { ParameterName = "in_year", MySqlDbType = MySqlDbType.Int32, Value = Year });
            Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("RaisedInvoiceReport", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Hdb_AbstracRealisedReport ObjAbR = new Hdb_AbstracRealisedReport();
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

                    ObjAbR.lstInv.Add(new Hdb_AbstracRealisedReport
                    {
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]),
                        ReceiptDate = Result["ReceiptDate"].ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString(),


                    });
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAbR.InvoiceAmt = Convert.ToDecimal(Result["TotalAmt"]);

                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjAbR;
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

        #region Abstract Pending Raised (Annexure -V) Report
        public void GetAbstractPendingRaisedReport(int Month, int Year)
        {
            IDataParameter[] Dparam = { };
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.Int32, Value = Month });
            lstParam.Add(new MySqlParameter { ParameterName = "in_year", MySqlDbType = MySqlDbType.Int32, Value = Year });
            Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PriorPendingInvoiceReport", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Hdb_AbstracRealisedReport ObjAbR = new Hdb_AbstracRealisedReport();
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

                    ObjAbR.lstInv.Add(new Hdb_AbstracRealisedReport
                    {

                        PartyName = Result["PartyName"].ToString(),
                        InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]),



                    });
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAbR.InvoiceAmt = Convert.ToDecimal(Result["RaisedAmt"]);

                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjAbR;
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

        #region Abstract Pending Realisable (Annexure -V) Report
        public void GetAbstractPendingRealisableReport(int Month, int Year)
        {
            IDataParameter[] Dparam = { };
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.Int32, Value = Month });
            lstParam.Add(new MySqlParameter { ParameterName = "in_year", MySqlDbType = MySqlDbType.Int32, Value = Year });
            Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PendigRaisedInvoiceReport", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Hdb_AbstracRealisedReport ObjAbR = new Hdb_AbstracRealisedReport();
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

                    ObjAbR.lstInv.Add(new Hdb_AbstracRealisedReport
                    {

                        PartyName = Result["PartyName"].ToString(),
                        InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"]),



                    });
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAbR.InvoiceAmt = Convert.ToDecimal(Result["GrandTotal"]);

                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjAbR;
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




        #region VC Report
        public void VCCapacityDetails(string dtArray, string date1, string date2)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_Xml", MySqlDbType = MySqlDbType.Text, Value = dtArray });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(date1).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(date2).ToString("yyyy/MM/dd") });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //var Result1 = DataAccess.ExecuteDynamicSet<VCCapacityModel>("GetVCCapacityDetails", DParam);
            IDataReader Result = DataAccess.ExecuteDataReader("GetVCCapacityDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<VCCapacityModel> LstVCCapacity = new List<VCCapacityModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstVCCapacity.Add(new VCCapacityModel
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
                        LstVCCapacity.Add(new VCCapacityModel());
                    }
                }
                if (Result.NextResult())
                {
                    var row = 0;
                    while (Result.Read())
                    {
                        LstVCCapacity[row].OccupencyCFS = (Result["occupencycfs"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["occupencycfs"]));
                        LstVCCapacity[row].OccupencyBond = (Result["occupencybond"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["occupencybond"]));

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

                if (Result.NextResult())
                {
                    var row = 0;
                    while (Result.Read())
                    {
                        LstVCCapacity[row].CurrPCSIncome = (Result["CurrPCSIncome"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["CurrPCSIncome"]));
                        LstVCCapacity[row].PrevPCSIncome = (Result["PrevPCSIncome"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["PrevPCSIncome"]));
                        LstVCCapacity[row].YearPCSIncome = (Result["YearPCSIncome"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["YearPCSIncome"]));
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

        #endregion

        #region Economy Report

        public void SaveMonthlyReportDetails(int monthNo, int yearNo, string xml)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "repoYear", MySqlDbType = MySqlDbType.Int32, Value = yearNo });
            LstParam.Add(new MySqlParameter { ParameterName = "repoMonth", MySqlDbType = MySqlDbType.Int32, Value = monthNo });
            LstParam.Add(new MySqlParameter { ParameterName = "xmlMonthlyEconomyReport", MySqlDbType = MySqlDbType.Text, Value = xml });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("RptMonthlyEconomySave", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Data Saved Successfully";
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
        public List<Hdb_MonthlyEconomyReport> GetMonthlyEconomyReportDataToPrint(int monthNo, int yearNo, out int dataExistStatus)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "repoYear", MySqlDbType = MySqlDbType.Int32, Value = yearNo });
            LstParam.Add(new MySqlParameter { ParameterName = "repoMonth", MySqlDbType = MySqlDbType.Int32, Value = monthNo });
            LstParam.Add(new MySqlParameter { ParameterName = "dataExistStatus", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("RptMonthlyEconomyPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            dataExistStatus = Convert.ToInt32(DParam.Where(x => x.ParameterName == "dataExistStatus").Select(x => x.Value).FirstOrDefault());

            List<Hdb_MonthlyEconomyReport> LstMonthlyEconomyReport = new List<Hdb_MonthlyEconomyReport>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        Hdb_MonthlyEconomyReport objMonthlyEconomyReport = new Hdb_MonthlyEconomyReport();
                        objMonthlyEconomyReport.IncomeExpHeadId = Convert.ToInt32(item["IncomeExpHeadId"]);
                        objMonthlyEconomyReport.FieldType = Convert.ToInt32(item["FieldType"]);
                        objMonthlyEconomyReport.ItemType = Convert.ToString(item["ItemType"]);
                        objMonthlyEconomyReport.ItemLabel = Convert.ToString(item["ItemLabel"]);
                        objMonthlyEconomyReport.ItemSortOrder = Convert.ToInt32(item["ItemSortOrder"]);
                        objMonthlyEconomyReport.ItemCodeNo = Convert.ToString(item["ItemCodeNo"]);
                        objMonthlyEconomyReport.ParentId = Convert.ToInt32(item["ParentId"]);
                        objMonthlyEconomyReport.YearNo = yearNo;
                        objMonthlyEconomyReport.MonthNo = monthNo;
                        objMonthlyEconomyReport.Amount = Convert.ToString(item["Amount"]) == "" ? 0 : Convert.ToDecimal(item["Amount"]);
                        objMonthlyEconomyReport.CumAmountSinceApril = Convert.ToString(item["CumAmountSinceApril"]) == "" ? 0 : Convert.ToDecimal(item["CumAmountSinceApril"]);
                        objMonthlyEconomyReport.ProrataCumAmount = Convert.ToString(item["ProrataCumAmount"]) == "" ? 0 : Convert.ToDecimal(item["ProrataCumAmount"]);
                        objMonthlyEconomyReport.NoOfPosts = Convert.ToString(item["NoOfPosts"]) == "" ? 0 : Convert.ToInt32(item["NoOfPosts"]);
                        // objMonthlyEconomyReport.Remarks = Convert.ToString(item["Remarks"]);

                        LstMonthlyEconomyReport.Add(objMonthlyEconomyReport);
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

            return LstMonthlyEconomyReport;
        }
        public List<Hdb_MonthlyEconomyReport> GetMonthlyEconomyReportData(int monthNo, int yearNo, out int dataExistStatus)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "repoYear", MySqlDbType = MySqlDbType.Int32, Value = yearNo });
            LstParam.Add(new MySqlParameter { ParameterName = "repoMonth", MySqlDbType = MySqlDbType.Int32, Value = monthNo });
            LstParam.Add(new MySqlParameter { ParameterName = "dataExistStatus", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("RptMonthlyEconomyPreProcess", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            dataExistStatus = Convert.ToInt32(DParam.Where(x => x.ParameterName == "dataExistStatus").Select(x => x.Value).FirstOrDefault());

            List<Hdb_MonthlyEconomyReport> LstMonthlyEconomyReport = new List<Hdb_MonthlyEconomyReport>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        Hdb_MonthlyEconomyReport objMonthlyEconomyReport = new Hdb_MonthlyEconomyReport();
                        objMonthlyEconomyReport.IncomeExpHeadId = Convert.ToInt32(item["IncomeExpHeadId"]);
                        objMonthlyEconomyReport.FieldType = Convert.ToInt32(item["FieldType"]);
                        objMonthlyEconomyReport.ItemType = Convert.ToString(item["ItemType"]);
                        objMonthlyEconomyReport.ItemLabel = Convert.ToString(item["ItemLabel"]);
                        objMonthlyEconomyReport.ItemSortOrder = Convert.ToInt32(item["ItemSortOrder"]);
                        objMonthlyEconomyReport.ItemCodeNo = Convert.ToString(item["ItemCodeNo"]);
                        objMonthlyEconomyReport.ParentId = Convert.ToInt32(item["ParentId"]);
                        objMonthlyEconomyReport.YearNo = yearNo;
                        objMonthlyEconomyReport.MonthNo = monthNo;
                        objMonthlyEconomyReport.Amount = Convert.ToString(item["Amount"]) == "" ? 0 : Convert.ToDecimal(item["Amount"]);
                        // objMonthlyEconomyReport.CumAmountSinceApril = Convert.ToString(item["CumAmountSinceApril"]) == "" ? 0 : Convert.ToDecimal(item["CumAmountSinceApril"]);
                        // objMonthlyEconomyReport.ProrataCumAmount = Convert.ToString(item["ProrataCumAmount"]) == "" ? 0 : Convert.ToDecimal(item["ProrataCumAmount"]);
                        objMonthlyEconomyReport.NoOfPosts = Convert.ToString(item["NoOfPosts"]) == "" ? 0 : Convert.ToInt32(item["NoOfPosts"]);
                        // objMonthlyEconomyReport.Remarks = Convert.ToString(item["Remarks"]);
                        objMonthlyEconomyReport.Script = Convert.ToString(item["Script"]);

                        LstMonthlyEconomyReport.Add(objMonthlyEconomyReport);
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

            return LstMonthlyEconomyReport;
        }

        #endregion

        #region Payer Ledger
        public void GetAllPartyForPartyLedger(string PartyCode, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPartyForPayerLedger", CommandType.StoredProcedure, DParam);
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


        public void GetPayerLedgerStatement(int PartyId, string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = FromDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = ToDate });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPayerLedgerStatement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            HdbSDDetailsStatement SDResult = new HdbSDDetailsStatement();
            int isSDParty = 0;
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
                            new HdbSDInvoiceDet
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
                                CrAdjust = Convert.ToInt32(Result["CrAdjust"]),


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

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        SDResult.SDBalance = Convert.ToDecimal(Result["SDBalance"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        SDResult.IsPda = Convert.ToInt32(Result["isFlag"]);
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


        #region SD Refund Receipt

        public void GetSDRefundReceiptDetails(string JournalNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_JournalNo", MySqlDbType = MySqlDbType.String, Value = JournalNo });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetSdRefundReceiptDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            HdbSDDetailsStatement SDResult = new HdbSDDetailsStatement();
            int isSDParty = 0;
            try
            {
                Status = 1;



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

        }
        #endregion


        #region Bulk Cash receipt For External user
        public void GetBulkCashreceiptForUser(string FromDate, string ToDate, string ReceiptNo, int UserId)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(FromDate == null ? "" : FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ToDate == null ? "" : ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ReceiptNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = UserId });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetExternalUserBulkCashRecptForPrint", CommandType.StoredProcedure, DParam);
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


        public void GetReceiptListForExternalUser(string FromDate, string ToDate, int UserId)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = UserId });




            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetExternalReceiptListWithDate", CommandType.StoredProcedure, DParam);
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


        #region Bulk Invoice For External User

        public void GetInvoiceListForExternalUser(string FromDate, string ToDate, string invoiceType, int UserId)
        {

            DateTime dtfrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");


            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.String, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.String, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = invoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = UserId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("InvoiceListWithDateForExternalUser", CommandType.StoredProcedure, DParam);
            IList<invoiceLIst> LstInvoice = new List<invoiceLIst>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstInvoice.Add(new invoiceLIst
                    {



                        InvoiceNumber = Result["InvoiceNumber"].ToString()



                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = JsonConvert.SerializeObject(LstInvoice);
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
       
        public void ModuleListWithInvoice(BulkInvoiceReport ObjBulkInvoiceReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            int PartyId = ObjBulkInvoiceReport.PartyId;
            int PayeeId = ObjBulkInvoiceReport.PayeeId;
            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.String, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.String, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            DParam = LstParam.ToArray();
            DataSet LstInvoice = new DataSet();
            LstInvoice = DataAccess.ExecuteDataSet("ModuleListWithInvoice", CommandType.StoredProcedure, DParam);

            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                if (LstInvoice != null)
                {
                    Status = 1;
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInvoice;
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
                //Result.Close();
                //Result.Dispose();

            }
        }
        public void ModuleListWithInvoiceExternalUser(BulkInvoiceReport ObjBulkInvoiceReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            int PartyId = ObjBulkInvoiceReport.PartyId;
            int PayeeId = ObjBulkInvoiceReport.PayeeId;
            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.String, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.String, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            DParam = LstParam.ToArray();
            DataSet LstInvoice = new DataSet();
            LstInvoice = DataAccess.ExecuteDataSet("ModuleListWithInvoiceForExternalUser", CommandType.StoredProcedure, DParam);

            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                if (LstInvoice != null)
                {
                    Status = 1;
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInvoice;
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
                //Result.Close();
                //Result.Dispose();

            }
        }


       
        public void GenericBulkInvoiceDetailsForPrintForExternal(BulkInvoiceReport ObjBulkInvoiceReport)
        {

            if (ObjBulkInvoiceReport.InvoiceNumber == null)
            {
                ObjBulkInvoiceReport.InvoiceNumber = "";
            }
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjBulkInvoiceReport.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = ObjBulkInvoiceReport.InvoiceModule });

            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("getbulkinvoicedetailsforprintforexternaluser", CommandType.StoredProcedure, DParam);
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

        #region Bulk Debit Note For External User
        /*     public void GetBulkDebitNoteReportForExternaluser(string PeriodFrom, string PeriodTo, int UserId)
             {
                 int Status = 0;
                 List<MySqlParameter> LstParam = new List<MySqlParameter>();
                 LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
                 LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
                 LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = UserId });
                 DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                 IDataParameter[] DParam = { };
                 DParam = LstParam.ToArray();
                 IDataReader Result = DataAccess.ExecuteDataReader("BulkDRNoteForExternalUser", CommandType.StoredProcedure, DParam);
                 List<string> lststring = new List<string>();
                 _DBResponse = new DatabaseResponse();
                 try
                 {
                     while (Result.Read())
                     {
                         Status = 1;
                         lststring.Add(Result["CRNoteHtml"].ToString());
                     }
                     lststring.ToList().ForEach(item =>
                     {
                         if (item == "")
                         {
                             lststring.Remove(item);
                         }
                     });
                     if (Status == 1)
                     {
                         _DBResponse.Status = 1;
                         _DBResponse.Message = "Success";
                         _DBResponse.Data = lststring;
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
             }*/
        public void GetBulkDebitNoteReportForExternaluser(string PeriodFrom, string PeriodTo, int UserId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Todate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            //  lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Value = CRDR });
            lstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = UserId });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintOfBulkDRNoteForExternalUser", CommandType.StoredProcedure, DParam);
            PrintModelOfBulkCrCompany objCR = new PrintModelOfBulkCrCompany();


            int Status = 0;
            _DBResponse = new DatabaseResponse();
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
                        Status = 1;
                        objCR.lstCrParty.Add(new PrintModelOfBulkCrParty
                        {
                            PartyName = Result["PartyName"].ToString(),
                            PartyAddress = Result["PartyAddress"].ToString(),
                            PartyCityName = Result["PartyCityName"].ToString(),
                            PartyStateName = Result["PartyStateName"].ToString(),
                            PartyStateCode = Result["PartyStateCode"].ToString(),
                            PartyGSTIN = Result["PartyGSTIN"].ToString(),
                            CRNoteNo = Result["CRNoteNo"].ToString(),
                            CRNoteDate = Result["CRNoteDate"].ToString(),
                            InvoiceNo = Result["InvoiceNo"].ToString(),
                            InvoiceDate = Result["InvoiceDate"].ToString(),
                            TotalAmt = Convert.ToDecimal(Result["TotalAmt"]),
                            RoundUp = Convert.ToDecimal(Result["RoundUp"]),
                            GrandTotal = Convert.ToDecimal(Result["GrandTotal"]),
                            CRNoteId = Convert.ToInt32(Result["CRNoteId"]),
                            Remarks = Result["Remarks"].ToString(),
                            irn = Result["irn"].ToString(),
                            SignedQRCode = Result["SignedQRCode"].ToString(),
                            SupplyType = Result["SupplyType"].ToString(),
                        });

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
                            CRNoteId = Convert.ToInt32(Result["CRNoteId"]),
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
        #endregion



        #region Bulk CreditNote For External User

        public void GetBulkCreditNoteReportForExteranlUser(string PeriodFrom, string PeriodTo, int UserId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = UserId });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("BulkCRNoteForExternalUser", CommandType.StoredProcedure, DParam);
            List<string> lststring = new List<string>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lststring.Add(Result["CRNoteHtml"].ToString());
                }
                lststring.ToList().ForEach(item =>
                {
                    if (item == "")
                    {
                        lststring.Remove(item);
                    }
                });
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lststring;
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
        #endregion

        #region PartywiseSdStatement
        public void GetPartywiseSdStatement(int PartyId, string Fdt, String Tdt)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Partyid", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Fromdate", MySqlDbType = MySqlDbType.Date, Value = Fdt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Tdt });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPartyWiseSDDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            HDB_PartyWiseSdStatement SDResult = new HDB_PartyWiseSdStatement();
            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    SDResult.PartyName = Result["PartyName"].ToString();
                    SDResult.Opening = Convert.ToDecimal(Result["Amount"]);

                    //SDResult.UtilizationAmount = Convert.ToDecimal(Result["UtilizationAmount"]);

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        SDResult.LstSdStatement.Add(
                            new HDB_PartyWiseSdStatementDetails
                            {
                                //Sl, InvoiceNo, InvoiceDate, ReceiptNo, ReceiptDate, ReceiptAmt, TranType, TranAmt
                                SrNo = Convert.ToInt32(Result["SrNo"]),
                                ReceiptNo = Result["ReceiptNo"].ToString(),
                                ReceiptDate = Result["ReceiptDate"].ToString(),

                                Mode = Result["PayMode"].ToString(),
                                JVNo = Result["JVNo"].ToString(),
                                JVDate = Result["JVDate"].ToString(),
                                Amount = Convert.ToDecimal(Result["Amount"]),



                            });
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
        #region Daily Transaction 
        public void GetDailyTransactionReportList(Hdb_DailyTransactionReport ObjDailyTransactionReport)
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

            Hdb_DailyTransactionReport newObjDailyTransactionReport = new Hdb_DailyTransactionReport();

            newObjDailyTransactionReport.AppeasementSummaryList = new List<Hdb_AppeasementSummary>();
            newObjDailyTransactionReport.DeStuffingSummaryList = new List<Hdb_DeStuffingSummary>();
            newObjDailyTransactionReport.CartingSummaryList = new List<Hdb_CartingSummary>();
            newObjDailyTransactionReport.StuffingSummaryList = new List<Hdb_StuffingImportExportBONDSummary>();
            newObjDailyTransactionReport.InportInSummaryList = new List<Hdb_StuffingImportExportBONDSummary>();
            newObjDailyTransactionReport.InportOutSummaryList = new List<Hdb_StuffingImportExportBONDSummary>();
            newObjDailyTransactionReport.ExportInSummaryList = new List<Hdb_StuffingImportExportBONDSummary>();
            newObjDailyTransactionReport.ExportOutSummaryList = new List<Hdb_StuffingImportExportBONDSummary>();
            newObjDailyTransactionReport.BONDUnloadingSummaryList = new List<Hdb_StuffingImportExportBONDSummary>();
            newObjDailyTransactionReport.BONDDeliverySummaryList = new List<Hdb_StuffingImportExportBONDSummary>();
            newObjDailyTransactionReport.EmptyInTransporterSummaryList = new List<Hdb_EmptyTransporterSummary>();
            newObjDailyTransactionReport.EmptyOutTransporterSummaryList = new List<Hdb_EmptyTransporterSummary>();

            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    #region AppeasementSummary
                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        newObjDailyTransactionReport.AppeasementSummaryList.Add(new Hdb_AppeasementSummary
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
                        newObjDailyTransactionReport.DeStuffingSummaryList.Add(new Hdb_DeStuffingSummary
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
                        newObjDailyTransactionReport.CartingSummaryList.Add(new Hdb_CartingSummary
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
                            newObjDailyTransactionReport.StuffingSummaryList.Add(new Hdb_StuffingImportExportBONDSummary
                            {
                                ShippingBillNo = item["ShippingBillNo"].ToString(),
                                ShippingBillDate = item["ShippingBillDate"].ToString(),
                                ExporterName = item["ExporterName"].ToString(),
                                CHA = item["CHA"].ToString(),
                                ShippingLine = item["ShippingLine"].ToString(),
                                CartingType = item["CartingType"].ToString(),
                                Remarks = item["Remarks"].ToString(),
                            });

                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.StuffingSummaryList.Where(x => x.ShippingBillNo == item["ShippingBillNo"].ToString()).ToList())
                            {
                                Hdb_ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.StuffingSummaryList.Where(x => x.ShippingBillNo == item["ShippingBillNo"].ToString()).ToList())
                            {
                                Hdb_ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
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
                            newObjDailyTransactionReport.ExportInSummaryList.Add(new Hdb_StuffingImportExportBONDSummary
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
                                Hdb_ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.ExportInSummaryList.Where(x => x.ShippingBillNo == item["ShippingBillNo"].ToString()).ToList())
                            {
                                Hdb_ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
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
                            newObjDailyTransactionReport.ExportOutSummaryList.Add(new Hdb_StuffingImportExportBONDSummary
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
                                Hdb_ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.ExportOutSummaryList.Where(x => x.ShippingBillNo == item["ShippingBillNo"].ToString()).ToList())
                            {
                                Hdb_ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
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
                            newObjDailyTransactionReport.InportInSummaryList.Add(new Hdb_StuffingImportExportBONDSummary
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
                                Hdb_ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.InportInSummaryList.Where(x => x.ExporterName == item["ImporterName"].ToString()).ToList())
                            {
                                Hdb_ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
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
                            newObjDailyTransactionReport.InportOutSummaryList.Add(new Hdb_StuffingImportExportBONDSummary
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
                                Hdb_ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.InportOutSummaryList.Where(x => x.ExporterName == item["ImporterName"].ToString()).ToList())
                            {
                                Hdb_ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
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
                            newObjDailyTransactionReport.BONDUnloadingSummaryList.Add(new Hdb_StuffingImportExportBONDSummary
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
                                Hdb_ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.BONDUnloadingSummaryList.Where(x => x.ShippingBillNo == item["BOENo"].ToString()).ToList())
                            {
                                Hdb_ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
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
                            newObjDailyTransactionReport.BONDDeliverySummaryList.Add(new Hdb_StuffingImportExportBONDSummary
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
                                Hdb_ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString()));
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.BONDDeliverySummaryList.Where(x => x.ShippingBillNo == item["BOENo"].ToString()).ToList())
                            {
                                Hdb_ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }
                        }

                    }
                    #endregion

                    #region BONDDeliverySummary
                    foreach (DataRow item in Result.Tables[10].Rows)
                    {
                        newObjDailyTransactionReport.EmptyInTransporterSummaryList.Add(new Hdb_EmptyTransporterSummary
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

                    foreach (var item in newObjDailyTransactionReport.StuffingSummaryList)
                    {
                        newObjDailyTransactionReport.StuffingSummaryList[0].SizeTwenty += item.ContainerInfoList.Count(x => x.Size == "20");
                        newObjDailyTransactionReport.StuffingSummaryList[0].SizeForty += item.ContainerInfoList.Count(x => x.Size == "40");
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
                    // newObjDailyTransactionReport.EmptyOutTransporterSummaryList ;

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

        private Hdb_ContainerInfo GetContainerInfo(string containerNo, string size)
        {
            Hdb_ContainerInfo objContainerInfo = new Hdb_ContainerInfo();
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
        #endregion

        #region PDA Receipt Print#
        public void GetBulkPDACashreceipt(string FromDate, string ToDate, string ReceiptNo)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(FromDate == null ? "" : FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ToDate == null ? "" : ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = ReceiptNo });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetBulkPDACashRecptForPrint", CommandType.StoredProcedure, DParam);
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


        #region Monthly Performance Report
        public List<Hdb_MonthlyPerformaceReport> GetMonthlyPerformanceReportDataToPrint(int monthNo, int yearNo)
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

            List<Hdb_MonthlyPerformaceReport> LstMonthlyPerformaceReport = new List<Hdb_MonthlyPerformaceReport>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        Hdb_MonthlyPerformaceReport objMonthlyPerformaceReport = new Hdb_MonthlyPerformaceReport();
                        objMonthlyPerformaceReport.DescriptionId = Convert.ToInt32(item["SlNo"]);
                        objMonthlyPerformaceReport.MonthUnderReport = Convert.ToString(item["CurMonthAmt"]);
                        objMonthlyPerformaceReport.PrevMonth = Convert.ToString(item["PrevMonthAmt"]);
                        objMonthlyPerformaceReport.CorresMonthPrevYear = Convert.ToString(item["MonthPrevYearAmt"]);
                        objMonthlyPerformaceReport.MonthUnderReportMT = Convert.ToString(item["CurMonthMT"]);
                        objMonthlyPerformaceReport.PrevMonthMT = Convert.ToString(item["PrevMonthMT"]);
                        objMonthlyPerformaceReport.CorresMonthPrevYearMT = Convert.ToString(item["MonthPrevYearMT"]);

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
        #endregion

        #region LCL Statement Of Import Cargo Report
        public void GetLCLStatementOfImportCargo(string AsOnDate, int GodownId, string GodownName)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy-MM-dd HH:mm:ss") });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            DParam = LstParam.ToArray();
            //  DataTable dt = ds.Tables[0];
            //  DataSet Result = DataAccess.ExecuteDataReader("PVReport", CommandType.StoredProcedure, DParam);
            DataSet ds = DataAccess.ExecuteDataSet("LSCStatementofimportcargo", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            _DBResponse = new DatabaseResponse();
            int Status = 0;

            List<Hdb_LCLStatement> lstPV = new List<Hdb_LCLStatement>();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    lstPV = (from DataRow dr in dt.Rows
                             select new Hdb_LCLStatement()



                             {
                                 SlNo = Convert.ToInt32(dr["SlNo"]),
                                 ImporterName = Convert.ToString(dr["ImporterName"]),
                                 ImporterAddress = Convert.ToString(dr["ImporterAddress"]),
                                 TSANoDate = Convert.ToString(dr["TSANoDate"]),
                                 BOLNo = dr["BOLNo"].ToString(),
                                 EntryDate = Convert.ToString(dr["EntryDate"]),
                                 DestuffingEntryDate = Convert.ToString(dr["DestuffingEntryDate"]),
                                 NoOfUnitsRec = Convert.ToInt32(dr["NoOfUnitsRec"]),
                                 Weight = Convert.ToDecimal(dr["Weight"]),
                                 CargoDescription = dr["CargoDescription"].ToString(),
                                 CIF = Convert.ToDecimal(dr["CIFValue"]),
                                 Duty = Convert.ToDecimal(dr["Duty"]),
                                 CIFDuty = Convert.ToDecimal(dr["ValueDuty"]),
                                 Area = Convert.ToDecimal(dr["Area"]),
                                 CFSCode = dr["CFSCode"].ToString(),
                                 ForwarderName = dr["Forwarder"].ToString(),
                                 Remarks = dr["Remarks"].ToString(),

                             }).ToList();
                }

                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = LCLStatementImportExcel(lstPV, AsOnDate, GodownName);

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


        private string LCLStatementImportExcel(List<Hdb_LCLStatement> lstPV, string AsOnDate, string GodownName)
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
                var h2 = "CFS Hyderabad";
                var h3 = "LSC STATEMENT OF IMPORT CARGO As On Date - " + AsOnDate + "";
                var h4 = "GODOWN NUMBER -" + GodownName + "";
                exl.MargeCell("A1:K1", title, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A2:K2", h1, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A3:K3", h2, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A4:K4", h3, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A5:K5", h4, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                // exl.MargeCell("A6:K6", h5, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A6:A6", "SL. No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B6:B6", "Importer Name", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C6:C6", "Importer Address", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D6:D6", "TSA Number & Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E6:E6", "BL Number", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F6:F6", "Date of Arrival", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G6:G6", "Date of Destuffing", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H6:H6", "No. of Packages", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I6:I6", "Weight", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J6:J6", "Cargo Description", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K6:K6", "Value", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("L6:L6", "Duty", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("M6:M6", "Value+Duty", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("N6:N6", "Area occupied", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("O6:O6", "CFS CODE", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("P6:P6", "Forwarder Name", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("Q6:Q6", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                // exl.MargeCell("R7:R7", "DESC", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                // exl.MargeCell("S7:S7", "SLA", DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("T7:T7", "CFS/PORT NAME", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.AddTable<Hdb_LCLStatement>("A", 7, lstPV, new[] { 6, 20, 20, 20, 20, 20, 10, 15, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 });
                //   var NoOfUnits = lstPV.Sum(o => o.NoOfUnitsRec);
                var NoOfUnitsRec = lstPV.Sum(o => o.NoOfUnitsRec);
                var Weight = lstPV.Sum(o => o.Weight);
                var Area = lstPV.Sum(o => o.Area);
                var Value = lstPV.Sum(o => o.CIF);
                var Duty = lstPV.Sum(o => o.Duty);
                var ValueDuty = lstPV.Sum(o => o.CIFDuty);

                //var CBM = lstPV.Sum(o => o.CBM);

                // var CIF = lstPV.Sum(o => o.CIF);

                exl.MargeCell("E" + (lstPV.Count + 8).ToString() + ":G" + (lstPV.Count + 8).ToString() + "", "TOTAL", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("H" + (lstPV.Count + 8).ToString(), NoOfUnitsRec.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("I" + (lstPV.Count + 8).ToString(), Weight.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("K" + (lstPV.Count + 8).ToString(), Value.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("L" + (lstPV.Count + 8).ToString(), Duty.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("M" + (lstPV.Count + 8).ToString(), ValueDuty.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("N" + (lstPV.Count + 8).ToString(), Area.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //sexl.AddCell("P" + (lstPV.Count + 8).ToString(), CBM.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //  exl.AddCell("Q" + (lstPV.Count + 8).ToString(), CIF.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                // System.Web.UI.WebControls.TableCell cell7 = new System.Web.UI.WebControls.TableCell();
                // exl.MargeCell( "Signature of Godown Incharge " + "Signature of Import Incharge" + "Manager-CFS", DynamicExcel.CellAlignment.BottomLeft);

                exl.MargeCell("C" + (lstPV.Count + 16).ToString() + ":G" + (lstPV.Count + 8).ToString() + "", "Signature of Godown Incharge", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G" + (lstPV.Count + 16).ToString() + ":G" + (lstPV.Count + 8).ToString() + "", "Signature of Import Incharge", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("P" + (lstPV.Count + 16).ToString() + ":G" + (lstPV.Count + 8).ToString() + "", "Manager-CFS", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
                exl.Save();
            }
            return excelFile;
        }
        private string PVReportImportExcels(List<Hdb_LCLStatement> lstPV, string AsOnDate, string GodownName)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            System.Web.UI.WebControls.GridView Grid = new System.Web.UI.WebControls.GridView();


            System.Web.UI.WebControls.Table tb = new System.Web.UI.WebControls.Table();

            if (lstPV.Count > 0)
            {

                Grid.AllowPaging = false;
                Grid.DataSource = lstPV;
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


                    if (i % 2 != 0)
                    {
                        Grid.Rows[i].Attributes.Add("class", "textmode");
                    }
                    else
                    {
                        Grid.Rows[i].Attributes.Add("class", "textmode2");
                    }

                }
                var title = "CENTRAL WAREHOUSING CORPORATION </br>"
                          + "Principal Place of Business</br>"
                          + "CENTRAL WAREHOUSE</br>"
                          + "LSC STATEMENT OF IMPORT CARGO" + AsOnDate + "</br>";

                System.Web.UI.WebControls.TableCell cell1 = new System.Web.UI.WebControls.TableCell();
                cell1.Text = "<b> CENTRAL WAREHOUSING CORPORATION </b> ";
                cell1.ForeColor = System.Drawing.Color.Black;
                System.Web.UI.WebControls.TableRow tr1 = new System.Web.UI.WebControls.TableRow();
                tr1.Cells.Add(cell1);
                tr1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;

                //System.Web.UI.WebControls.TableCell cell2 = new System.Web.UI.WebControls.TableCell();
                //cell2.Text = "Principal Place of Business";
                //System.Web.UI.WebControls.TableRow tr2 = new System.Web.UI.WebControls.TableRow();
                //tr2.Cells.Add(cell2);
                //tr2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;

                System.Web.UI.WebControls.TableCell cell3 = new System.Web.UI.WebControls.TableCell();
                cell3.Text = "CENTRAL WAREHOUSE";
                cell3.ForeColor = System.Drawing.Color.Black;

                System.Web.UI.WebControls.TableRow tr3 = new System.Web.UI.WebControls.TableRow();
                tr3.Cells.Add(cell3);
                tr3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;


                System.Web.UI.WebControls.TableCell cell4 = new System.Web.UI.WebControls.TableCell();
                cell4.Text = "LSC STATEMENT OF IMPORT CARGO AS ON " + AsOnDate + "";
                System.Web.UI.WebControls.TableRow tr4 = new System.Web.UI.WebControls.TableRow();
                cell4.ForeColor = System.Drawing.Color.Black;

                tr4.Cells.Add(cell4);
                tr4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;



                System.Web.UI.WebControls.TableCell cell5 = new System.Web.UI.WebControls.TableCell();
                cell5.Controls.Add(Grid);
                System.Web.UI.WebControls.TableRow tr5 = new System.Web.UI.WebControls.TableRow();
                tr5.Cells.Add(cell5);
                System.Web.UI.WebControls.TableCell cell6 = new System.Web.UI.WebControls.TableCell();
                cell6.Text = "TOTAL " + AsOnDate + "";
                System.Web.UI.WebControls.TableRow tr6 = new System.Web.UI.WebControls.TableRow();
                cell6.ForeColor = System.Drawing.Color.Black;
                tr6.Cells.Add(cell6);
                tr6.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                System.Web.UI.WebControls.TableCell cell7 = new System.Web.UI.WebControls.TableCell();
                cell7.Text = "Signature of Godown Incharge " + "Signature of Import Incharge" + "Manager-CFS";
                System.Web.UI.WebControls.TableRow tr7 = new System.Web.UI.WebControls.TableRow();
                cell7.ForeColor = System.Drawing.Color.Black;
                tr7.Cells.Add(cell7);
                tr7.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tb.Rows.Add(tr7);
                //  tb.Rows.Add(tr2);
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
        }




        #endregion



        #region LSC Statement Long Standing Of Import Cargo Report
        public void GetLSCStatementLongStandingOfImportCargo(string AsOnDate, int GodownId, string GodownName)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy-MM-dd HH:mm:ss") });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            DParam = LstParam.ToArray();
            //  DataTable dt = ds.Tables[0];
            //  DataSet Result = DataAccess.ExecuteDataReader("PVReport", CommandType.StoredProcedure, DParam);
            DataSet ds = DataAccess.ExecuteDataSet("LSCStatementofimportcargoLognStandingCargo", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            _DBResponse = new DatabaseResponse();
            int Status = 0;

            List<Hdb_LCLStatement> lstPV = new List<Hdb_LCLStatement>();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    lstPV = (from DataRow dr in dt.Rows
                             select new Hdb_LCLStatement()



                             {
                                 SlNo = Convert.ToInt32(dr["SlNo"]),
                                 ImporterName = Convert.ToString(dr["ImporterName"]),
                                 ImporterAddress = Convert.ToString(dr["ImporterAddress"]),
                                 TSANoDate = Convert.ToString(dr["TSANoDate"]),
                                 BOLNo = dr["BOLNo"].ToString(),
                                 EntryDate = Convert.ToString(dr["EntryDate"]),
                                 DestuffingEntryDate = Convert.ToString(dr["DestuffingEntryDate"]),
                                 NoOfUnitsRec = Convert.ToInt32(dr["NoOfUnitsRec"]),
                                 Weight = Convert.ToDecimal(dr["Weight"]),
                                 CargoDescription = dr["CargoDescription"].ToString(),
                                 CIF = Convert.ToDecimal(dr["CIFValue"]),
                                 Duty = Convert.ToDecimal(dr["Duty"]),
                                 CIFDuty = Convert.ToDecimal(dr["ValueDuty"]),
                                 Area = Convert.ToDecimal(dr["Area"]),
                                 CFSCode = dr["CFSCode"].ToString(),
                                 ForwarderName = dr["Forwarder"].ToString(),
                                 Remarks = dr["Remarks"].ToString(),

                             }).ToList();
                }

                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = LCLStatementLongStandingImportExcel(lstPV, AsOnDate, GodownName);

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


        private string LCLStatementLongStandingImportExcel(List<Hdb_LCLStatement> lstPV, string AsOnDate, string GodownName)
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
                var h2 = "CFS Hyderabad";
                var h3 = "LSC STATEMENT OF  LONG STANDING IMPORT CARGO AS ON DATE - " + AsOnDate + "";
                var h4 = "GODOWN NUMBER -" + GodownName + "";
                exl.MargeCell("A1:K1", title, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A2:K2", h1, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A3:K3", h2, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A4:K4", h3, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A5:K5", h4, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                // exl.MargeCell("A6:K6", h5, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A6:A6", "SL. No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B6:B6", "Importer Name", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C6:C6", "Importer Address", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D6:D6", "TSA Number & Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E6:E6", "BL Number", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F6:F6", "Date of Arrival", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G6:G6", "Date of Destuffing", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H6:H6", "No. of Packages", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I6:I6", "Weight", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J6:J6", "Cargo Description", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K6:K6", "Value", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("L6:L6", "Duty", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("M6:M6", "Value+Duty", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("N6:N6", "Area occupied", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("O6:O6", "CFS CODE", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("P6:P6", "Forwarder Name", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("Q6:Q6", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                // exl.MargeCell("R7:R7", "DESC", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                // exl.MargeCell("S7:S7", "SLA", DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("T7:T7", "CFS/PORT NAME", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.AddTable<Hdb_LCLStatement>("A", 7, lstPV, new[] { 6, 20, 20, 20, 20, 20, 10, 15, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 });
                //   var NoOfUnits = lstPV.Sum(o => o.NoOfUnitsRec);
                var NoOfUnitsRec = lstPV.Sum(o => o.NoOfUnitsRec);
                var Weight = lstPV.Sum(o => o.Weight);
                var Area = lstPV.Sum(o => o.Area);
                var Value = lstPV.Sum(o => o.CIF);
                var Duty = lstPV.Sum(o => o.Duty);
                var ValueDuty = lstPV.Sum(o => o.CIFDuty);

                //var CBM = lstPV.Sum(o => o.CBM);

                // var CIF = lstPV.Sum(o => o.CIF);

                exl.MargeCell("E" + (lstPV.Count + 8).ToString() + ":G" + (lstPV.Count + 8).ToString() + "", "TOTAL", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("H" + (lstPV.Count + 8).ToString(), NoOfUnitsRec.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("I" + (lstPV.Count + 8).ToString(), Weight.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("K" + (lstPV.Count + 8).ToString(), Value.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("L" + (lstPV.Count + 8).ToString(), Duty.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("M" + (lstPV.Count + 8).ToString(), ValueDuty.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("N" + (lstPV.Count + 8).ToString(), Area.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //sexl.AddCell("P" + (lstPV.Count + 8).ToString(), CBM.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //  exl.AddCell("Q" + (lstPV.Count + 8).ToString(), CIF.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                // System.Web.UI.WebControls.TableCell cell7 = new System.Web.UI.WebControls.TableCell();
                // exl.MargeCell( "Signature of Godown Incharge " + "Signature of Import Incharge" + "Manager-CFS", DynamicExcel.CellAlignment.BottomLeft);

                exl.MargeCell("C" + (lstPV.Count + 16).ToString() + ":G" + (lstPV.Count + 8).ToString() + "", "Signature of Godown Incharge", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G" + (lstPV.Count + 16).ToString() + ":G" + (lstPV.Count + 8).ToString() + "", "Signature of Import Incharge", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("P" + (lstPV.Count + 16).ToString() + ":G" + (lstPV.Count + 8).ToString() + "", "Manager-CFS", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
                exl.Save();
            }
            return excelFile;
        }

        #endregion

        #region Import Insurance Register Report

        public void GetAllInsuranceType()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //    LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            //  DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllInsuranceType", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_ImportInsuranceType> lstInsuranceType = new List<Hdb_ImportInsuranceType>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstInsuranceType.Add(new Hdb_ImportInsuranceType
                    {
                        ValueName = Result["ValueName"].ToString(),
                        ValueType = Result["ValueType"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstInsuranceType;
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
        //public void GetImportInsuranceRegisterReport(DateTime date1, DateTime date2,string Type)
        //{
        //    var LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = date2 });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = Type });
        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    _DBResponse = new DatabaseResponse();
        //    DataSet ds = DataAccess.ExecuteDataSet("GetImportInsuranceRegister", CommandType.StoredProcedure, DParam);
        //    DataTable dt = ds.Tables[1];
        //    DataTable dt1 = ds.Tables[0];


        //    List<Hdb_ImportInsuranceRegister> model = new List<Hdb_ImportInsuranceRegister>();
        //    try
        //    {


        //        _DBResponse.Status = 0;
        //        _DBResponse.Message = "No Data";
        //        _DBResponse.Data = GetImportInsuranceRegisterExcel(model, dt,dt1, date1.ToString("dd/MM/yyyy"), date2.ToString("dd/MM/yyyy"));
        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Status = 0;
        //        _DBResponse.Message = "No Data";
        //        _DBResponse.Data = null;
        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //    }
        //}

        public void GetImportInsuranceRegisterReport(DateTime date1, DateTime date2, string Type)
        {
            int Status = 0;
            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = date2 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            DataSet ds = DataAccess.ExecuteDataSet("GetImportInsuranceRegister", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[1];
            DataTable dt1 = ds.Tables[0];

            string Godown = dt1.Rows[0]["GodownName"].ToString();

            List<Hdb_ImportInsuranceRegister> lstInsuranceRegister = new List<Hdb_ImportInsuranceRegister>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    Status = 1;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //OCIFValue,ODuty,InCIFValue,InDuty,OutCIFValue,OutDuty,CCIFValue,CDuty
                        Hdb_ImportInsuranceRegister objInsuranceRegister = new Hdb_ImportInsuranceRegister();
                        objInsuranceRegister.Date = dt.Rows[i]["Date"].ToString();
                        objInsuranceRegister.OCIFValue = Convert.ToDecimal(dt.Rows[i]["OCIFValue"].ToString());
                        objInsuranceRegister.OGrossDuty = Convert.ToDecimal(dt.Rows[i]["ODuty"].ToString());
                        objInsuranceRegister.RCIFValue = Convert.ToDecimal(dt.Rows[i]["InCIFValue"].ToString());
                        objInsuranceRegister.RGrossDuty = Convert.ToDecimal(dt.Rows[i]["InDuty"].ToString());
                        objInsuranceRegister.DCIFValue = Convert.ToDecimal(dt.Rows[i]["OutCIFValue"].ToString());
                        objInsuranceRegister.DGrossDuty = Convert.ToDecimal(dt.Rows[i]["OutDuty"].ToString());
                        objInsuranceRegister.CCIFValue = Convert.ToDecimal(dt.Rows[i]["CCIFValue"].ToString());
                        objInsuranceRegister.CGrossDuty = Convert.ToDecimal(dt.Rows[i]["CDuty"].ToString());
                        objInsuranceRegister.CTotal = Convert.ToDecimal(dt.Rows[i]["Total"].ToString());

                        lstInsuranceRegister.Add(objInsuranceRegister);
                    }

                }


                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = GetImportInsuranceRegisterExcel(lstInsuranceRegister, Godown, date1.ToString("dd/MM/yyyy"), date2.ToString("dd/MM/yyyy"));
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

        private string GetImportInsuranceRegisterExcel(List<Hdb_ImportInsuranceRegister> lstInsuranceRegister, string Godown, string date1, string date2)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {

                string typeOfValue = "";

                typeOfValue = "From " + date1 + " To " + date2;



                exl.MargeCell("A1:J1", "CENTRAL WAREHOUSING CORPORATION", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A2:J2", "(A Govt. of India Undertaking)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A3:J3", "CFS, Kukatpally, Hyderabad", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A5:B5", "Import Godown No.", DynamicExcel.CellAlignment.TopLeft);
                exl.MargeCell("C5:F5", Godown, DynamicExcel.CellAlignment.TopLeft);

                exl.MargeCell("H5:H5", "Date", DynamicExcel.CellAlignment.TopLeft);
                exl.MargeCell("I5:J5", DateTime.Today, DynamicExcel.CellAlignment.TopLeft);
                exl.MargeCell("A6:J6", "Insurance Statement (Import) " + typeOfValue, DynamicExcel.CellAlignment.Middle);

                // exl.MargeCell("A7:J5", typeOfValue, DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A7:J7", "Value & Customs Duty", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A8:A9", "Date", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B8:C8", "Opening Balance", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B9:B9", "Value", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C9:B9", "Customs Duty", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D8:E8", "Receipt / Destuffing", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D9:D9", "Value", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E9:E9", "Customs Duty", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("F8:G8", "Issue / Delivery / Transfer to Bond", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F9:F9", "Value", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G9:G9", "Customs Duty", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H8:J8", "Closing Balance", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("H9:H9", "Value", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I9:I9", "Customs Duty", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J9:J9", "Total", DynamicExcel.CellAlignment.Middle);


                exl.AddTable("A", 10, lstInsuranceRegister, new[] { 15, 20, 20, 20, 20, 20, 20, 20, 20, 20, }, DynamicExcel.CellAlignment.TopRight);

                var AvgCIFValue = lstInsuranceRegister.Sum(o=>o.CCIFValue) / (lstInsuranceRegister.Count);
                var AvgGrossDuty = lstInsuranceRegister.Sum(o=>o.CGrossDuty) / (lstInsuranceRegister.Count);
                var AvgTotal = lstInsuranceRegister.LastOrDefault().CTotal / (lstInsuranceRegister.Count);

                exl.AddCell("G" + (lstInsuranceRegister.Count + 10).ToString(), "Total", DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("H" + (lstInsuranceRegister.Count + 10).ToString(), lstInsuranceRegister.Sum(o=>o.CCIFValue), DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("I" + (lstInsuranceRegister.Count + 10).ToString(), lstInsuranceRegister.Sum(o=>o.CGrossDuty), DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("J" + (lstInsuranceRegister.Count + 10).ToString(), lstInsuranceRegister.LastOrDefault().CTotal, DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("G" + (lstInsuranceRegister.Count + 11).ToString(), "Average Value", DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("H" + (lstInsuranceRegister.Count + 11).ToString(), AvgCIFValue.ToString("0.00"), DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("I" + (lstInsuranceRegister.Count + 11).ToString(), AvgGrossDuty.ToString("0.00"), DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("J" + (lstInsuranceRegister.Count + 11).ToString(), AvgTotal.ToString("0.00"), DynamicExcel.CellAlignment.TopRight);


                string cellpos1 = "B" + (lstInsuranceRegister.Count + 15).ToString() + ":" + "D" + (lstInsuranceRegister.Count + 15).ToString();
                string cellpos2 = "G" + (lstInsuranceRegister.Count + 15).ToString() + ":" + "I" + (lstInsuranceRegister.Count + 15).ToString();

                exl.MargeCell(cellpos1, "Signature of Godown In-charge", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell(cellpos2, "Signature of Import In-charge", DynamicExcel.CellAlignment.Middle);

                exl.Save();
            }
            return excelFile;
        }


        #endregion

        #region Insurance Register Export
        public void GetInsuranceRegisterExport(DateTime date1, DateTime date2, Int32 Type)
        {
            string GType = Type == 3 ? "IIA" : Type == 4 ? "IIB" : Type == 5 ? "Open" : Type == 1 ? "VI" : "All";
            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = date2 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_godownId", MySqlDbType = MySqlDbType.Int32, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //var objRegOutwardSupply = (List<RegisterOfOutwardSupplyModel>)DataAccess.ExecuteDynamicSet<List<RegisterOfOutwardSupplyModel>>("GetRegisterofOutwardSupply", DParam);
            ; DataSet ds = DataAccess.ExecuteDataSet("ExportInsuranceRegisterReport", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            List<Hdb_InsuranceRegisterStockModel> model = new List<Hdb_InsuranceRegisterStockModel>();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    model = (from DataRow dr in dt.Rows
                             select new Hdb_InsuranceRegisterStockModel()
                             {
                                 //  SlNo = Convert.ToInt32(dr["SlNo"]),
                                 _date = dr["_date"].ToString(),

                                 openingfob = Convert.ToDecimal(dr["openingfob"]),
                                 receiptfob = Convert.ToDecimal(dr["receiptfob"]),
                                 issuefob = Convert.ToDecimal(dr["issuefob"]),
                                 closefob = Convert.ToDecimal(dr["closefob"]),
                                 closeweight = Convert.ToDecimal(dr["closeweight"]),

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
                _DBResponse.Data = dt;
                    //InsuranceRegisterExportExcel(model, date1.ToString("dd/MM/yyyy"), date2.ToString("dd/MM/yyyy"), GType);
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



        private string InsuranceRegisterExportExcel(List<Hdb_InsuranceRegisterStockModel> model,  string date1, string date2, string GType)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "(A Government of India Undertaking)"
                      ;

                var Addr = "CFS, Kukatpally, Hyderabad";
                var Titl1 = "Insurance Statement (Export) From " + date1 + " TO " + date2;
                var Godown = "Export Godown No. " + GType;
                exl.MargeCell("A1:G1", title, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A2:G2", Addr, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A3:G3", Titl1, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A4:C4", Godown, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A5:A7", "Date", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B5:F5", "FOB Value", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B6:B7", "Opening Balance", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C6:C7", "Receipt / Carting", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D6:D7", "Issue / Stuffing & BTT", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E6:F6", "Closing Balance", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E7:E7", "FOB Value", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F7:F7", "Weight", DynamicExcel.CellAlignment.Middle);

                //for (var i = 65; i < 85; i++)
                //{
                //    char character = (char)i;
                //    string text = character.ToString();
                //    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                //}
                /*exl.AddTable<InvoiceData>("A", 6, model.InvoiceData, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16 });
                exl.AddTable<CashReceiptData>("R", 6, model.CashReceiptData, new[] { 12, 30, 14, 14, 14, 14, 14, 40 });/
                exl.AddTable<PpgRegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16, 12, 30, 14, 14, 14, 14, 14, 40 });*/
                exl.AddTable<Hdb_InsuranceRegisterStockModel>("A", 8, model, new[] { 20, 20, 20, 20, 20, 20 });
                var receiptfob = model.Sum(o => o.receiptfob);
                var issuefob = model.Sum(o => o.issuefob);
                var closefob = model.Sum(o => o.closefob);
                var closeweight = model.Sum(o => o.closeweight);
                var AvgFob = closefob / (model.Count);
                String congn = "Total Number of Consignments received during the month:";
                congn = congn + " " + receiptfob;
                exl.AddCell("B" + (model.Count + 9).ToString(), "Total", DynamicExcel.CellAlignment.CenterRight);

                exl.AddCell("C" + (model.Count + 9).ToString(), receiptfob.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("D" + (model.Count + 9).ToString(), issuefob.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("E" + (model.Count + 9).ToString(), closefob.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("F" + (model.Count + 9).ToString(), closeweight.ToString(), DynamicExcel.CellAlignment.CenterRight);
                /*exl.AddCell("O" + (model.Count + 7).ToString(), "Invoice Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 7).ToString(), InvoiceAmount, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 8).ToString(), "Cash Receipt Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 8).ToString(), CRAmount, DynamicExcel.CellAlignment.CenterRight);*/
                exl.AddCell("D" + (model.Count + 10).ToString(), "Average  Value", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("E" + (model.Count + 10).ToString(), AvgFob.ToString("0.00"), DynamicExcel.CellAlignment.CenterRight);

                string cellpos = "B" + (model.Count + 12).ToString() + ":" + "D" + (model.Count + 12).ToString();

                //  exl.MargeCell("B"+ (model.Count + 11).ToString() ":"+, "Closing Balance", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell(cellpos, congn, DynamicExcel.CellAlignment.CenterRight);
                string cellpos1 = "A" + (model.Count + 15).ToString() + ":" + "C" + (model.Count + 15).ToString();
                string cellpos2 = "D" + (model.Count + 15).ToString() + ":" + "F" + (model.Count + 15).ToString();

                exl.MargeCell(cellpos1, "Signature of Godown In-charge", DynamicExcel.CellAlignment.CenterRight);
                exl.MargeCell(cellpos2, "Signature of Export In-charge", DynamicExcel.CellAlignment.CenterRight);

                exl.Save();
            }
            return excelFile;
        }


        #endregion



        #region Daily Transaction Report
        public void GetDailyTransactionReportImport(string FromDate, string ToDate, string Module, int GodownId, string GodownName)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            DParam = LstParam.ToArray();
            //  DataTable dt = ds.Tables[0];
            //  DataSet Result = DataAccess.ExecuteDataReader("PVReport", CommandType.StoredProcedure, DParam);
            DataSet ds = DataAccess.ExecuteDataSet("GetImportDailyTransactionReport_dtest", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            _DBResponse = new DatabaseResponse();
            int Status = 0;

            Hdb_ImportDailyTransaction lstImportDailyTransaction = new Hdb_ImportDailyTransaction();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstImportDailyTransaction.Dustuff.Add(new Hdb_ImportDustuffDailyTransaction
                        {
                            SLNO = Convert.ToString(dr["SlNo"]),
                            DestuffingNo = Convert.ToString(dr["DestuffingEntryNo"]),
                            DestuffingDate = Convert.ToString(dr["DestuffingEntryDate"]),
                            CBTContainerNo = Convert.ToString(dr["ContainerNo"]),
                            Size= Convert.ToString(dr["Size"]),
                            FCLLCL= Convert.ToString(dr["LCLFCL"]),
                            CBTfrom= Convert.ToString(dr["CbtForm"]),
                            PortofOrigin= Convert.ToString(dr["PortofOrigin"]),
                            TSANoDate= Convert.ToString(dr["TSANoDate"]),
                            BLNoDate= Convert.ToString(dr["BOLNoDate"]),
                            ImporterName = Convert.ToString(dr["ImporterName"]),
                            ForwarderName = Convert.ToString(dr["ForwarderName"]),
                            IGMNumber = Convert.ToString(dr["IGMNo"]),
                            CargoDescription= Convert.ToString(dr["CargoDescription"]),
                            CommodityType = Convert.ToString(dr["CommodityType"]),
                            NoOfPackages= Convert.ToDecimal(dr["Noofpkg"]),
                            Weight = Convert.ToDecimal(dr["Weight"]),
                            Value= Convert.ToDecimal(dr["Value"]),
                           Duty = Convert.ToDecimal(dr["Duty"]),
                           Area= Convert.ToDecimal(dr["Area"]),
                           Remarks= Convert.ToString(dr["Remarks"])













  
    });
                    }

                  /*  foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        lstImportDailyTransaction.Handling.Add(new Hdb_ImportHandlingDailyTransaction
                        {
                            ConatinerNo = Convert.ToString(dr["ContainerNo"]),
                            EmptyLoad = Convert.ToString(dr["EmptyLoad"]),
                            LiftOnOff = Convert.ToString(dr["LiffOnOff"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            Size = Convert.ToString(dr["Size"]),
                        });
                    }*/


                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        lstImportDailyTransaction.Delivery.Add(new Hdb_ImportDeliveryCargoDailyTransaction
                        {
                             SlNo= Convert.ToString(dr["SllNo"]),
                            IssueSlipNumber= Convert.ToString(dr["IssueslipNo"]),
                            Date= Convert.ToString(dr["IssueSlipDate"]),
                            VehicleNumber = Convert.ToString(dr["Vehicleno"]),
                            GatePassNoDate= Convert.ToString(dr["gatepassnodate"]),
                            FCLLCL = Convert.ToString(dr["LCLFCL"]),
                            ValueDuty= Convert.ToDecimal(dr["Total"]),
                            InvoiceNoDate = Convert.ToString(dr["InvoiceNoDate"]),
                            DeliveryorderNoDate = Convert.ToString(dr["DeliveryNoDate"]),
                            BLNoDate = Convert.ToString(dr["BLNoDate"]),
                            ImporterName = Convert.ToString(dr["ImporterName"]),
                            CHAName = Convert.ToString(dr["CHAName"]),
                            BENoDate= Convert.ToString(dr["Boenodate"]),
                            CargoDescription = Convert.ToString(dr["Cargodescription"]),
                            CommodityType = Convert.ToString(dr["CommodityType"]),
                            NoOfPackages = Convert.ToDecimal(dr["Noofpkg"]),
                            Weight = Convert.ToDecimal(dr["Weight"]),
                            Value = Convert.ToDecimal(dr["Value"]),
                            Duty = Convert.ToDecimal(dr["Duty"]),
                            Area = Convert.ToDecimal(dr["Area"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            Charges = Convert.ToString(dr["Charges"])



                        });
                    }
                }

                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = GetDailyTransactionReportImportExcel(lstImportDailyTransaction, FromDate, ToDate, GodownName);

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
        private string GetDailyTransactionReportImportExcel(Hdb_ImportDailyTransaction DailyTransactionReport, string FromDate, string ToDate, string GodownName)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION";

                exl.MargeCell("A1:N1", title, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A2:N2", "(A Govt.of India Undertaking)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A3:N3", "CFS, Kukatpally, Hyderabad", DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A4:N4", "From Date: " + FromDate, DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A5:N5", "To Date: " + ToDate, DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A6:N6", "Daily Transaction Report - Import Godown No." + GodownName + "", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A7:U7", "Deposit ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //exl.MargeCell("J7:N7", "Handling Details", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("A8:A8", "S.No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B8:B8", "Destuffing Entry Number", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C8:C8", "Destuffing Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D8:D8", "CBT/Container Number", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E8:E8", "Size", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F8:F8", "FCL/LCL", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G8:G8", "CBT from", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H8:H8", "Port of Discharge", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I8:I8", "TSA Number & Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J8:J8", "BL Number& Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K8:K8", "Importer Name", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("L8:L8", "Forwarder Name", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("M8:M8", "IGM Number", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("N8:N8", "Cargo Description", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("O8:O8", "Commodity Type", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("P8:P8", "No. Of Packages", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("Q8:Q8", "Weight", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("R8:R8", "Value", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("S8:S8", "Duty", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("T8:T8", "Area", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("U8:U8", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                try
                {
                   
                    exl.AddTable<Hdb_ImportDustuffDailyTransaction>("A", 9, DailyTransactionReport.Dustuff, new[] { 20, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 });
                    var Noofpkg = DailyTransactionReport.Dustuff.Sum(o => o.NoOfPackages);
                    var Weight = DailyTransactionReport.Dustuff.Sum(o => o.Weight);
                    var Value = DailyTransactionReport.Dustuff.Sum(o => o.Value);
                    var Duty = DailyTransactionReport.Dustuff.Sum(o => o.Duty);

                    var Area = DailyTransactionReport.Dustuff.Sum(o => o.Area);

                   

                    exl.MargeCell("A" + (DailyTransactionReport.Dustuff.Count + 9).ToString() + ":O" + (DailyTransactionReport.Dustuff.Count + 9).ToString() + "", "TOTAL", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
                    exl.AddCell("P" + (DailyTransactionReport.Dustuff.Count + 9).ToString(), Noofpkg.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                    exl.AddCell("Q" + (DailyTransactionReport.Dustuff.Count + 9).ToString(), Weight.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                    exl.AddCell("R" + (DailyTransactionReport.Dustuff.Count + 9).ToString(), Value.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                    exl.AddCell("S" + (DailyTransactionReport.Dustuff.Count + 9).ToString(), Duty.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                    exl.AddCell("T" + (DailyTransactionReport.Dustuff.Count + 9).ToString(), Area.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
          
                }
                catch (Exception e)
                {

                }


             /*   try
                {

                    exl.AddTable<Hdb_ImportHandlingDailyTransaction>("J", 9, DailyTransactionReport.Handling, new[] { 20, 10, 10, 10, 10 });


                }
                catch (Exception ex)
                {

                }*/


               var totalCount = (DailyTransactionReport.Dustuff.Count) ;

                var totalheaderRow = totalCount + 11;
                var totalCountrow = totalCount + 12;

                exl.MargeCell("A" + totalheaderRow + ":V" + totalheaderRow + "", "Delivery of Cargo / Container", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A" + totalCountrow + ":A" + totalCountrow + "", "S.No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B" + totalCountrow + ":B" + totalCountrow + "", "Issue Slip Number", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C" + totalCountrow + ":C" + totalCountrow + "", "Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D" + totalCountrow + ":D" + totalCountrow + "", "Vehicle Number", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E" + totalCountrow + ":E" + totalCountrow + "", "Gate Pass Number & Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F" + totalCountrow + ":F" + totalCountrow + "", "FCL/LCL", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G" + totalCountrow + ":G" + totalCountrow + "", "Value+Duty", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H" + totalCountrow + ":H" + totalCountrow + "", "Invoice No.& Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I" + totalCountrow + ":I" + totalCountrow + "", "Delivery order No. Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J" + totalCountrow + ":J" + totalCountrow + "", "BL Number& Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K" + totalCountrow + ":K" + totalCountrow + "", "Importer Name", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("L" + totalCountrow + ":L" + totalCountrow + "", "CHA Name", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("M" + totalCountrow + ":M" + totalCountrow + "", "BE No. & Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("N" + totalCountrow + ":N" + totalCountrow + "", "Cargo Description", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("O" + totalCountrow + ":O" + totalCountrow + "", "Commodity Type", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("P" + totalCountrow + ":P" + totalCountrow + "", "No. Of Packages", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("Q" + totalCountrow + ":Q" + totalCountrow + "", "Weight", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("R" + totalCountrow + ":R" + totalCountrow + "", "Value", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("S" + totalCountrow + ":S" + totalCountrow + "", "Duty", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("T" + totalCountrow + ":T" + totalCountrow + "", "Area", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("U" + totalCountrow + ":U" + totalCountrow + "", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("V" + totalCountrow + ":V" + totalCountrow + "", "Charges", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                try
                {
                    exl.AddTable<Hdb_ImportDeliveryCargoDailyTransaction>("A", (totalCountrow + 1), DailyTransactionReport.Delivery, new[] { 20, 20, 20, 20, 12, 20, 20, 20, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10,10 });
                    var Noofpkg = DailyTransactionReport.Delivery.Sum(o => o.NoOfPackages);
                    var Weight = DailyTransactionReport.Delivery.Sum(o => o.Weight);
                    var Value = DailyTransactionReport.Delivery.Sum(o => o.Value);
                    var Duty = DailyTransactionReport.Delivery.Sum(o => o.Duty);

                    var Area = DailyTransactionReport.Delivery.Sum(o => o.Area);



                    exl.MargeCell("A" + (totalCountrow + DailyTransactionReport.Delivery.Count +1).ToString() + ":O" + (totalCountrow + DailyTransactionReport.Delivery.Count + 1).ToString() + "", "TOTAL", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
                    exl.AddCell("P" + (totalCountrow + DailyTransactionReport.Delivery.Count +1).ToString(), Noofpkg.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                    exl.AddCell("Q" + (totalCountrow + DailyTransactionReport.Delivery.Count + 1).ToString(), Weight.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                    exl.AddCell("R" + (totalCountrow + DailyTransactionReport.Delivery.Count + 1).ToString(), Value.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                    exl.AddCell("S" + (totalCountrow + DailyTransactionReport.Delivery.Count + 1).ToString(), Duty.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                    exl.AddCell("T" + (totalCountrow + DailyTransactionReport.Delivery.Count + 1).ToString(), Area.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                }
                catch (Exception e)
                {

                }



                exl.MargeCell("B" + (totalCountrow + DailyTransactionReport.Delivery.Count + 15).ToString() + ":C" + (totalCountrow + DailyTransactionReport.Delivery.Count + 15).ToString() + "", "Signature of Godown No. I/C", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("J" + (totalCountrow + DailyTransactionReport.Delivery.Count + 15).ToString() + ":L" + (totalCountrow + DailyTransactionReport.Delivery.Count + 15).ToString() + "", "Signature of Import In-charge", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("R" + (totalCountrow + DailyTransactionReport.Delivery.Count + 15).ToString() + ":S" + (totalCountrow + DailyTransactionReport.Delivery.Count + 15).ToString() + "", "Manager-CFS", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);




                exl.Save();
            }
            return excelFile;
        }
        public void GetDailyTransactionReportExport(string FromDate, string ToDate, string Module, int GodownId, string GodownName)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            DParam = LstParam.ToArray();
            DataSet dss = DataAccess.ExecuteDataSet("GetExportDailyTransactionReport", CommandType.StoredProcedure, DParam);
            DataTable dt = dss.Tables[0];
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Hdb_ExportDailyTransaction lstexport = new Hdb_ExportDailyTransaction();




            try
            {
                if (dss.Tables.Count > 0)
                {
                    foreach (DataRow dr in dss.Tables[0].Rows)
                    {
                        lstexport.CartingDetails.Add(new Hdb_ExportDailyTransactionForCartingDetails
                        {
                            SlNo = Convert.ToInt32(dr["SlNo"]).ToString(),
                            CartingNo = Convert.ToString(dr["CartingNo"]),
                            CartingDate = Convert.ToString(dr["CartingDate"]),
                            SBNo = Convert.ToString(dr["SBNo"]),
                            ShippingBillDate = Convert.ToString(dr["ShippingBillDate"]),
                            ExporterName = Convert.ToString(dr["ExporterName"]),
                            CHAName = Convert.ToString(dr["CHAName"]),
                            CargoDescription = Convert.ToString(dr["CargoDescription"]),
                            CargoType = Convert.ToString(dr["CargoType"]),
                            PackageType = Convert.ToString(dr["PackageType"]),
                            NoOfPkg = Convert.ToDecimal(dr["NoOfUnits"]),
                            Weight = Convert.ToDecimal(dr["Weight"]),
                            FOB = Convert.ToDecimal(dr["FobValue"]),
                            Area = Convert.ToDecimal(dr["Area"]),
                            VehicleNo = Convert.ToString(dr["VehicleNo"]),                            
                            Remarks = Convert.ToString(dr["Remarks"]),
                        });
                    }

                    foreach (DataRow dr in dss.Tables[1].Rows)
                    {
                        lstexport.StuffingDetails.Add(new Hdb_ExportDailyTransactionForStuffingDetails
                        {
                            SlNo = Convert.ToInt32(dr["SlNo"]).ToString(),
                            StuffingNo = Convert.ToString(dr["StuffingNo"]),
                            StuffingDate = Convert.ToString(dr["StuffingDate"]),
                            SBNo = Convert.ToString(dr["SBNo"]),
                            ShippingBillDate = Convert.ToString(dr["ShippingBillDate"]),
                            ExporterName = Convert.ToString(dr["ExporterName"]),
                            CHAName = Convert.ToString(dr["CHAName"]),
                            CargoDescription = Convert.ToString(dr["CargoDescription"]),
                            CargoType = Convert.ToString(dr["CargoType"]),
                            PackageType = Convert.ToString(dr["PackageType"]),
                            NoOfPkg = Convert.ToDecimal(dr["NoOfUnits"]),
                            Weight = Convert.ToDecimal(dr["Weight"]),
                            FOB = Convert.ToDecimal(dr["FobValue"]),
                            Area = Convert.ToDecimal(dr["Area"]),
                            ContSize = Convert.ToString(dr["ContSize"]),
                            CBT = Convert.ToString(dr["CBT"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            ShippingLine = Convert.ToString(dr["ShippingLine"]),
                            Charges = Convert.ToString(dr["Charges"])
                        });
                    }

                    foreach (DataRow dr in dss.Tables[2].Rows)
                    {
                        lstexport.DeliveryDetails.Add(new Hdb_ExportDailyTransactionFroDeliveryDetails
                        {
                            Weight = Convert.ToDecimal(dr["BTTWeight"]),
                            FOB = Convert.ToDecimal(dr["Fob"]),
                            NoOfPKG = Convert.ToDecimal(dr["BTTQuantity"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            SBNo = Convert.ToString(dr["ShippingBillNo"])
                        });
                    }

                    foreach (DataRow dr in dss.Tables[3].Rows)
                    {
                        lstexport.BttContainer.Add(new Hdb_exportBTTDailyTransactionDetails
                        {
                            ContainerFOB = Convert.ToDecimal(dr["FOB"]),
                            ContainerNoCBT = Convert.ToString(dr["ContainerNo"]),
                            ContainerNoOfPkg = Convert.ToDecimal(dr["TotalNoOfPackages"]),
                            ContainerRemarks = Convert.ToString(dr["Remarks"]),
                            ContainerWeight = Convert.ToDecimal(dr["TotalGrossWt"]),
                            Size = Convert.ToInt32(dr["Size"])

                        });
                    }


                    foreach (DataRow dr in dss.Tables[4].Rows)
                    {
                        lstexport.Empty.Add(new Hdb_ExportDailyTransactionForEmpty
                        {
                            CloseBalance = Convert.ToDecimal(dr["CLOSING"]),
                            dry20 = Convert.ToDecimal(dr["DRY20"]),
                            dry40 = Convert.ToDecimal(dr["DRY40"]),
                            FOB = Convert.ToDecimal(dr["FOB"]),
                            Issue = Convert.ToDecimal(dr["ISSUE"]),
                            OpeningBalance = Convert.ToDecimal(dr["OPENING"]),
                            Rail20 = Convert.ToDecimal(dr["RAIL20"]),
                            Rail40 = Convert.ToDecimal(dr["RAIL40"]),
                            Receipt = Convert.ToDecimal(dr["RECEIPT"]),
                            Refer20 = Convert.ToDecimal(dr["REEFER20"]),
                            Refer40 = Convert.ToDecimal(dr["REEFER40"]),
                            Remarks = Convert.ToString(dr["REMARKS"]),
                            Remarks2 = Convert.ToString(dr["LOADREMARKS"]),
                            Teus = Convert.ToDecimal(dr["Teus"]),
                            Total20 = Convert.ToDecimal(dr["Total20"]),
                            Total40 = Convert.ToDecimal(dr["Total40"]),
                            Weight = Convert.ToDecimal(dr["WEIGHT"])
                        });
                    }


                }
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = GetDailyTransactionReportExportExcel(lstexport, FromDate, ToDate, GodownName);
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {

                dss.Dispose();

            }
        }
        private string GetDailyTransactionReportExportExcel(Hdb_ExportDailyTransaction ExportDailyTransaction, string FromDate, string ToDate, string GodownName)
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
                exl.MargeCell("A3:O3", "CFS, Kukatpally, Hyderabad", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A4:O4", "From Date: " + FromDate, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A5:O5", "To Date " + ToDate, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A6:O6", "Daily Transaction Report - Export Godown No." + GodownName + "", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A7:P7", "Carting", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);                

                exl.MargeCell("A8:A8", "Sl No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B8:B8", "Carting No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C8:C8", "Carting Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D8:D8", "Shipping Bill No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E8:E8", "Shipping Bill Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F8:F8", "Exporter Name", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G8:G8", "CHA Name", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H8:H8", "Cargo Description ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I8:I8", "Cargo Type", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J8:J8", "Package Type", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K8:K8", "No. Of Units", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("L8:L8", "Weight ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("M8:M8", "Fob Value ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("N8:N8", "Area ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("O8:O8", "Vehicle No. ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("P8:P8", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                try
                {
                    exl.AddTable<Hdb_ExportDailyTransactionForCartingDetails>("A", 9, ExportDailyTransaction.CartingDetails, new[] { 20,10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10,10,10,10 });

                    var Noofpkg = ExportDailyTransaction.CartingDetails.Sum(o => o.NoOfPkg);
                    var Weight = ExportDailyTransaction.CartingDetails.Sum(o => o.Weight);
                    var Value = ExportDailyTransaction.CartingDetails.Sum(o => o.FOB);                    
                    var Area = ExportDailyTransaction.CartingDetails.Sum(o => o.Area); 

                    exl.MargeCell("A" + (ExportDailyTransaction.CartingDetails.Count + 9).ToString() + ":J" + (ExportDailyTransaction.CartingDetails.Count + 9).ToString() + "", "TOTAL", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
                    exl.AddCell("K" + (ExportDailyTransaction.CartingDetails.Count + 9).ToString(), Noofpkg.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                    exl.AddCell("L" + (ExportDailyTransaction.CartingDetails.Count + 9).ToString(), Weight.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                    exl.AddCell("M" + (ExportDailyTransaction.CartingDetails.Count + 9).ToString(), Value.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                    exl.AddCell("N" + (ExportDailyTransaction.CartingDetails.Count + 9).ToString(), Area.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                }
                catch (Exception e)
                {

                }

                var totalCount = (ExportDailyTransaction.CartingDetails.Count);

                var totalheaderRow = totalCount + 11;
                var totalCountrow = totalCount + 12;

                exl.MargeCell("A" + totalheaderRow + ":S" + totalheaderRow + "", "Stuffing", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A" + totalCountrow + ":A" + totalCountrow + "", "S.No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B" + totalCountrow + ":B" + totalCountrow + "", "Stuffing No", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C" + totalCountrow + ":C" + totalCountrow + "", "Stuffing Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D" + totalCountrow + ":D" + totalCountrow + "", "Shipping Bill No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E" + totalCountrow + ":E" + totalCountrow + "", "Shipping Bill Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F" + totalCountrow + ":F" + totalCountrow + "", "Exporter Name", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G" + totalCountrow + ":G" + totalCountrow + "", "CHA Name", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H" + totalCountrow + ":H" + totalCountrow + "", "Cargo Description", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I" + totalCountrow + ":I" + totalCountrow + "", "Cargo Type", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J" + totalCountrow + ":J" + totalCountrow + "", "Package Type", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K" + totalCountrow + ":K" + totalCountrow + "", "No. Of Units", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("L" + totalCountrow + ":L" + totalCountrow + "", "Weight", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("M" + totalCountrow + ":M" + totalCountrow + "", "Fob Value", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("N" + totalCountrow + ":N" + totalCountrow + "", "Area", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("O" + totalCountrow + ":O" + totalCountrow + "", "Container / Size", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);    
                exl.MargeCell("P" + totalCountrow + ":P" + totalCountrow + "", "CBT / Size", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("Q" + totalCountrow + ":Q" + totalCountrow + "", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("R" + totalCountrow + ":R" + totalCountrow + "", "Shipping Line", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("S" + totalCountrow + ":S" + totalCountrow + "", "Charges", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                try
                {
                    exl.AddTable<Hdb_ExportDailyTransactionForStuffingDetails>("A", (totalCountrow + 1), ExportDailyTransaction.StuffingDetails, new[] { 20, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 ,20});
                    var Noofpkg = ExportDailyTransaction.StuffingDetails.Sum(o => o.NoOfPkg);
                    var Weight = ExportDailyTransaction.StuffingDetails.Sum(o => o.Weight);
                    var Value = ExportDailyTransaction.StuffingDetails.Sum(o => o.FOB);                   
                    var Area = ExportDailyTransaction.StuffingDetails.Sum(o => o.Area);



                    exl.MargeCell("A" + (totalCountrow + ExportDailyTransaction.StuffingDetails.Count + 1).ToString() + ":J" + (totalCountrow + ExportDailyTransaction.StuffingDetails.Count + 1).ToString() + "", "TOTAL", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
                    exl.AddCell("K" + (totalCountrow + ExportDailyTransaction.StuffingDetails.Count + 1).ToString(), Noofpkg.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                    exl.AddCell("L" + (totalCountrow + ExportDailyTransaction.StuffingDetails.Count + 1).ToString(), Weight.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                    exl.AddCell("M" + (totalCountrow + ExportDailyTransaction.StuffingDetails.Count + 1).ToString(), Value.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                    exl.AddCell("N" + (totalCountrow + ExportDailyTransaction.StuffingDetails.Count + 1).ToString(), Area.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                    
                }
                catch (Exception e)
                {

                }

                var totalCountStuf = (ExportDailyTransaction.StuffingDetails.Count);

                var totalheaderRowStuf = totalCount + totalCountStuf + 16;
                var totalCountrowStuf = totalCount + +totalCountStuf + 17;

                try
                {


                    exl.MargeCell("A" + totalheaderRowStuf + ":O" + totalheaderRowStuf + "", "Delivery of Cargo/Container/Back To Town", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                    exl.MargeCell("A" + totalCountrowStuf + ":A" + totalCountrowStuf + "", "Shipping Bill Number", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("B" + totalCountrowStuf + ":B" + totalCountrowStuf + "", "No.of units", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("C" + totalCountrowStuf + ":C" + totalCountrowStuf + "", "Weight", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("D" + totalCountrowStuf + ":D" + totalCountrowStuf + "", "FOB Value ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("E" + totalCountrowStuf + ":E" + totalCountrowStuf + "", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);


                    exl.AddTable<Hdb_ExportDailyTransactionFroDeliveryDetails>("A", (totalCountrowStuf + 1), ExportDailyTransaction.DeliveryDetails, new[] { 20, 10, 10, 10, 10 });

                }
                catch (Exception ex)
                {

                }
                try
                {

                    exl.MargeCell("F" + totalCountrowStuf + ":F" + totalCountrowStuf + "", "Container / CBT No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("G" + totalCountrowStuf + ":G" + totalCountrowStuf + "", "No. of units", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("H" + totalCountrowStuf + ":H" + totalCountrowStuf + "", "Weight", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("I" + totalCountrowStuf + ":I" + totalCountrowStuf + "", "FOB Value", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("J" + totalCountrowStuf + ":J" + totalCountrowStuf + "", "Size", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                    exl.MargeCell("K" + totalCountrowStuf + ":O" + totalCountrowStuf + "", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                    exl.AddTable<Hdb_exportBTTDailyTransactionDetails>("F", (totalCountrowStuf + 1), ExportDailyTransaction.BttContainer, new[] { 20, 10, 10, 10, 10, 10 });
                }
                catch (Exception ex)
                { }


                var total = 1 + ExportDailyTransaction.CartingDetails.Count + ExportDailyTransaction.StuffingDetails.Count + (ExportDailyTransaction.DeliveryDetails.Count > ExportDailyTransaction.BttContainer.Count ? ExportDailyTransaction.DeliveryDetails.Count : ExportDailyTransaction.BttContainer.Count);

                exl.MargeCell("A" + (total + 18).ToString() + ":E" + (total + 18).ToString() + "", "Empty Container", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F" + (total + 18).ToString() + ":Q" + (total + 18).ToString() + "", "FCL/Factory Stuffing/Franchise", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("A" + (total + 19).ToString() + ":A" + (total + 19).ToString() + "", "Opening Balance", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B" + (total + 19).ToString() + ":B" + (total + 19).ToString() + "", "Receipt", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C" + (total + 19).ToString() + ":C" + (total + 19).ToString() + "", "Issue", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D" + (total + 19).ToString() + ":D" + (total + 19).ToString() + "", "Closing Balance ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E" + (total + 19).ToString() + ":E" + (total + 19).ToString() + "", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);


                exl.MargeCell("F" + (total + 19).ToString() + ":G" + (total + 19).ToString() + "", "Dry", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H" + (total + 19).ToString() + ":I" + (total + 19).ToString() + "", "Reefer", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J" + (total + 19).ToString() + ":K" + (total + 19).ToString() + "", "Rail ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("L" + (total + 19).ToString() + ":L" + (total + 19).ToString() + "", "FOB Value", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("M" + (total + 19).ToString() + ":M" + (total + 19).ToString() + "", "Weight", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("N" + (total + 19).ToString() + ":O" + (total + 19).ToString() + "", "Total ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("P" + (total + 19).ToString() + ":P" + (total + 19).ToString() + "", "TEUs", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("Q" + (total + 19).ToString() + ":Q" + (total + 19).ToString() + "", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);



                exl.MargeCell("F" + (total + 20).ToString() + ":F" + (total + 20).ToString() + "", "20", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G" + (total + 20).ToString() + ":G" + (total + 20).ToString() + "", "40", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H" + (total + 20).ToString() + ":H" + (total + 20).ToString() + "", "20", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I" + (total + 20).ToString() + ":I" + (total + 20).ToString() + "", "40", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J" + (total + 20).ToString() + ":J" + (total + 20).ToString() + "", "20", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K" + (total + 20).ToString() + ":K" + (total + 20).ToString() + "", "40", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
            
                exl.MargeCell("N" + (total + 20).ToString() + ":N" + (total + 20).ToString() + "", "40", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("O" + (total + 20).ToString() + ":O" + (total + 20).ToString() + "", "40", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);


                exl.AddTable<Hdb_ExportDailyTransactionForEmpty>("A", (total + 21), ExportDailyTransaction.Empty, new[] { 20, 20, 20, 20, 12, 20, 20, 20, 20, 20, 12, 10, 10, 10, 10, 10, 20 });




                exl.MargeCell("A" + (total + 25).ToString() + ":C" + (total + 25).ToString() + "", "Signature of I/C Godown IIA & IIB", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
            
                exl.MargeCell("G" + (total + 25).ToString() + ":J" + (total + 25).ToString() + "", "Signature of Franchaise Asst/ I/C", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K" + (total + 25).ToString() + ":O" + (total + 25).ToString() + "", "Signature of Export I/C", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);





                //exl.MargeCell("A" + (BondDailyTransaction.lstBondDepositeTransaction.Count + BondDailyTransaction.lstBondDeliveryTransaction.Count + 13).ToString() + ":O" + (BondDailyTransaction.lstBondDepositeTransaction.Count + BondDailyTransaction.lstBondDeliveryTransaction.Count + 13).ToString() + "", " ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //exl.MargeCell("A" + (BondDailyTransaction.lstBondDepositeTransaction.Count + BondDailyTransaction.lstBondDeliveryTransaction.Count + 14).ToString() + ":O" + (BondDailyTransaction.lstBondDepositeTransaction.Count + BondDailyTransaction.lstBondDeliveryTransaction.Count + 14).ToString() + "", " ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);


                //exl.MargeCell("B" + (BondDailyTransaction.lstBondDepositeTransaction.Count + BondDailyTransaction.lstBondDeliveryTransaction.Count + 15).ToString() + ":C" + (BondDailyTransaction.lstBondDepositeTransaction.Count + BondDailyTransaction.lstBondDeliveryTransaction.Count + 15).ToString() + "", "Signature of I/C Godown No. ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //exl.MargeCell("M" + (BondDailyTransaction.lstBondDepositeTransaction.Count + BondDailyTransaction.lstBondDeliveryTransaction.Count + 15).ToString() + ":N" + (BondDailyTransaction.lstBondDepositeTransaction.Count + BondDailyTransaction.lstBondDeliveryTransaction.Count + 15).ToString() + "", "Signature of Bond I/C", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);


                exl.Save();
            }
            return excelFile;
        }
        public void GetDailyTransactionReportBond(string FromDate, string ToDate, string Module, int GodownId, string GodownName)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });

            DParam = LstParam.ToArray();
            DataSet ds = DataAccess.ExecuteDataSet("GetBondDailyTransactionReport", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Hdb_BondDailyTransactionReport BondDaily = new Hdb_BondDailyTransactionReport();

            try
            {
                int srno = 0;
                int depsrno = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    srno = srno + 1;
                    BondDaily.lstBondDepositeTransaction.Add(new Hdb_BondDepositeDailyTransactionReport
                    {
                        SrNo = srno,
                        GodownName = Convert.ToString(dr["GodownName"]),
                        DepositNo = Convert.ToString(dr["DepositNo"]),
                        DepositDate = Convert.ToString(dr["DepositDate"]),
                        Importer = Convert.ToString(dr["Importer"]),
                        CHAName = Convert.ToString(dr["CHAName"]),
                        CargoDescription = Convert.ToString(dr["CargoDescription"]),
                        BondNoDate = Convert.ToString(dr["BondNoDate"]),
                        BOENoDate = Convert.ToString(dr["BOENoDate"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        AREA = Convert.ToDecimal(dr["AREA"]),
                        Duty = Convert.ToDecimal(dr["Duty"]),
                        Noofpkg = Convert.ToDecimal(dr["Noofpkg"]),
                        Value = Convert.ToDecimal(dr["Value"]),
                        Weight = Convert.ToDecimal(dr["Weight"]),
                    });
                }

                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    depsrno = depsrno + 1;
                    BondDaily.lstBondDeliveryTransaction.Add(new Hdb_BondDeliverDailyTransactionReport
                    {
                        SrNo = depsrno,
                        GodownName = Convert.ToString(dr["GodownName"]),
                        DeliveryOrderNo = Convert.ToString(dr["DeliveryOrderNo"]),
                        DeliveryOrderDate = Convert.ToString(dr["DeliveryOrderDate"]),
                        Importer = Convert.ToString(dr["Importer"]),
                        CHAName = Convert.ToString(dr["CHAName"]),
                        CargoDescription = Convert.ToString(dr["CargoDescription"]),
                        BondNoDate = Convert.ToString(dr["BondNoDate"]),
                        BOENoDate = Convert.ToString(dr["BOENoDate"]),
                        AREA = Convert.ToDecimal(dr["Area"]),
                        Duty = Convert.ToDecimal(dr["Duty"]),
                        Noofpkg = Convert.ToDecimal(dr["Units"]),
                        Value = Convert.ToDecimal(dr["Value"]),
                        Weight = Convert.ToDecimal(dr["Weight"]),
                        InvoiceNoDate = Convert.ToString(dr["InvoiceNoDate"])
                    });
                }



                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = GetDailyTransactionReportBondExcel(BondDaily, FromDate, ToDate, GodownName);
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

        private string GetDailyTransactionReportBondExcel(Hdb_BondDailyTransactionReport BondDailyTransaction, string FromDate, string ToDate, string GodownName)
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
                exl.MargeCell("A3:O3", "CFS, Kukatpally, Hyderabad", DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A4:O4", "From Date: " + FromDate, DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A5:O5", "To Date " + ToDate, DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A6:O6", "Daily Transaction Report - Bond", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A7:O7", "Deposit", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("A8:A8", "S.No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B8:B8", "Godown No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C8:C8", "Deposit Application No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D8:D8", "Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E8:E8", "Importer", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F8:F8", "CHA", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G8:G8", "Description of Cargo", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H8:H8", "Bond Number / Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I8:I8", "Into Bond BE No. / Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J8:J8", "No. of Packages/Units", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K8:K8", "Weight (Kgs)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("L8:L8", "Value ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("M8:M8", "Duty", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("N8:N8", "Gross Area occupied in m2", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("O8:O8", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                try
                {
                    exl.AddTable<Hdb_BondDepositeDailyTransactionReport>("A", 9, BondDailyTransaction.lstBondDepositeTransaction, new[] { 6, 20, 20, 20, 12, 20, 10, 15, 20, 12, 12, 8, 14, 20, 10 });

                }
                catch (Exception ex)
                {

                }

                var Units = BondDailyTransaction.lstBondDepositeTransaction.Sum(o => o.Noofpkg);
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


                exl.Save();
            }



            return excelFile;
        }

        #endregion

        #region Bond Insurance Register Report

        public void GetAllInsuranceTypeBond()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //    LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            //  DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllInsuranceTypeBond", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_BondInsuranceType> lstInsuranceType = new List<Hdb_BondInsuranceType>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstInsuranceType.Add(new Hdb_BondInsuranceType
                    {
                        ValueName = Result["ValueName"].ToString(),
                        ValueType = Result["ValueType"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstInsuranceType;
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

        public void GetBondInsuranceRegisterReport(DateTime date1, DateTime date2, string Type)
        {
            int Status = 0;
            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = date2 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            DataSet ds = DataAccess.ExecuteDataSet("GetBondInsuranceRegister", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[1];
            DataTable dt1 = ds.Tables[0];

            string Godown = dt1.Rows[0]["GodownName"].ToString();

            List<Hdb_BondInsuranceRegister> lstInsuranceRegister = new List<Hdb_BondInsuranceRegister>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    Status = 1;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //OCIFValue,ODuty,InCIFValue,InDuty,OutCIFValue,OutDuty,CCIFValue,CDuty
                        Hdb_BondInsuranceRegister objInsuranceRegister = new Hdb_BondInsuranceRegister();
                        objInsuranceRegister.Date = dt.Rows[i]["Date"].ToString();
                        objInsuranceRegister.OCIFValue = Convert.ToDecimal(dt.Rows[i]["OCIFValue"].ToString());
                        objInsuranceRegister.OGrossDuty = Convert.ToDecimal(dt.Rows[i]["ODuty"].ToString());
                        objInsuranceRegister.RCIFValue = Convert.ToDecimal(dt.Rows[i]["InCIFValue"].ToString());
                        objInsuranceRegister.RGrossDuty = Convert.ToDecimal(dt.Rows[i]["InDuty"].ToString());
                        objInsuranceRegister.DCIFValue = Convert.ToDecimal(dt.Rows[i]["OutCIFValue"].ToString());
                        objInsuranceRegister.DGrossDuty = Convert.ToDecimal(dt.Rows[i]["OutDuty"].ToString());
                        objInsuranceRegister.CCIFValue = Convert.ToDecimal(dt.Rows[i]["CCIFValue"].ToString());
                        objInsuranceRegister.CGrossDuty = Convert.ToDecimal(dt.Rows[i]["CDuty"].ToString());
                        objInsuranceRegister.CTotal = Convert.ToDecimal(dt.Rows[i]["Total"].ToString());

                        lstInsuranceRegister.Add(objInsuranceRegister);
                    }

                }


                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = GetBondInsuranceRegisterExcel(lstInsuranceRegister, Godown, date1.ToString("dd/MM/yyyy"), date2.ToString("dd/MM/yyyy"));
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

        private string GetBondInsuranceRegisterExcel(List<Hdb_BondInsuranceRegister> lstInsuranceRegister, string Godown, string date1, string date2)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {

                string typeOfValue = "";

                typeOfValue = "From " + date1 + " To " + date2;



                exl.MargeCell("A1:J1", "CENTRAL WAREHOUSING CORPORATION", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A2:J2", "(A Govt. of India Undertaking)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A3:J3", "CFS, Kukatpally, Hyderabad", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A5:B5", "Bond Godown No.", DynamicExcel.CellAlignment.TopLeft);
                exl.MargeCell("C5:F5", Godown, DynamicExcel.CellAlignment.TopLeft);

                exl.MargeCell("H5:H5", "Date", DynamicExcel.CellAlignment.TopLeft);
                exl.MargeCell("I5:J5", DateTime.Today, DynamicExcel.CellAlignment.TopLeft);
                exl.MargeCell("A6:J6", "Insurance Statement (Bond) " + typeOfValue, DynamicExcel.CellAlignment.Middle);

                // exl.MargeCell("A7:J5", typeOfValue, DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A7:J7", "Value & Customs Duty", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A8:A9", "Date", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B8:C8", "Opening Balance", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B9:B9", "Value", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C9:B9", "Customs Duty", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D8:E8", "Receipt", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D9:D9", "Value", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E9:E9", "Customs Duty", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("F8:G8", "Issue / Delivery", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F9:F9", "Value", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G9:G9", "Customs Duty", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H8:J8", "Closing Balance", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("H9:H9", "Value", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I9:I9", "Customs Duty", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J9:J9", "Total", DynamicExcel.CellAlignment.Middle);


                exl.AddTable("A", 10, lstInsuranceRegister, new[] { 15, 20, 20, 20, 20, 20, 20, 20, 20, 20, }, DynamicExcel.CellAlignment.TopRight);

                var AvgCIFValue = lstInsuranceRegister.Sum(x => x.CCIFValue) / (lstInsuranceRegister.Count);
                var AvgGrossDuty = lstInsuranceRegister.Sum(x => x.CGrossDuty) / (lstInsuranceRegister.Count);
                var AvgTotal = lstInsuranceRegister.Sum(x => x.CTotal) / (lstInsuranceRegister.Count);

                exl.AddCell("G" + (lstInsuranceRegister.Count + 10).ToString(), "Total", DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("H" + (lstInsuranceRegister.Count + 10).ToString(), lstInsuranceRegister.LastOrDefault().CCIFValue, DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("I" + (lstInsuranceRegister.Count + 10).ToString(), lstInsuranceRegister.LastOrDefault().CGrossDuty, DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("J" + (lstInsuranceRegister.Count + 10).ToString(), lstInsuranceRegister.LastOrDefault().CTotal, DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("G" + (lstInsuranceRegister.Count + 11).ToString(), "Average Value", DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("H" + (lstInsuranceRegister.Count + 11).ToString(), AvgCIFValue.ToString("0.00"), DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("I" + (lstInsuranceRegister.Count + 11).ToString(), AvgGrossDuty.ToString("0.00"), DynamicExcel.CellAlignment.TopRight);
                exl.AddCell("J" + (lstInsuranceRegister.Count + 11).ToString(), AvgTotal.ToString("0.00"), DynamicExcel.CellAlignment.TopRight);


                string cellpos1 = "B" + (lstInsuranceRegister.Count + 15).ToString() + ":" + "D" + (lstInsuranceRegister.Count + 15).ToString();
                string cellpos2 = "G" + (lstInsuranceRegister.Count + 15).ToString() + ":" + "I" + (lstInsuranceRegister.Count + 15).ToString();

                exl.MargeCell(cellpos1, "Signature of Godown In-charge", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell(cellpos2, "Signature of Bond In-charge", DynamicExcel.CellAlignment.Middle);

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


            List<HDBRegisterOfEInvoiceModel> model = new List<HDBRegisterOfEInvoiceModel>();
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


        private string RegisterofEInvoiceExcel(List<HDBRegisterOfEInvoiceModel> model, DataTable dt)
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
            HDB_BulkIRN objInvoice = new HDB_BulkIRN();
            IDataReader Result = DataAccess.ExecuteDataReader("IRNNotgeneratedInvoiceList", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    objInvoice.lstPostPaymentChrg.Add(new HDB_BulkIRNDetails
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
        #region Bond Form-A Report
        public void GetBondFormAReport(DateTime date1, DateTime date2)
        {
            String PeriodFrom = date1.ToString("yyyy/MM/dd");
            String PeriodTo = date2.ToString("yyyy/MM/dd");

            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
            //  LstParam.Add(new MySqlParameter { ParameterName = "In_Type", MySqlDbType = MySqlDbType.VarChar, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //var objRegOutwardSupply = (List<RegisterOfOutwardSupplyModel>)DataAccess.ExecuteDynamicSet<List<RegisterOfOutwardSupplyModel>>("GetRegisterofOutwardSupply", DParam);
            DataSet ds = DataAccess.ExecuteDataSet("GetBondFormARpt", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            //  DataTable dt1 = ds.Tables[1];
            // DataTable dt2 = ds.Tables[2];


            List<HDBBondFormA> modelA = new List<HDBBondFormA>();
            //  List<HDBBondFormA> modelB = new List<HDBBondFormA>();
            // List<WFLDBondFormA> modelC = new List<WFLDBondFormA>();
            try
            {
                modelA = (from DataRow dr in dt.Rows
                          select new HDBBondFormA()
                          {
                              A = dr["A"].ToString(),
                              B = dr["B"].ToString(),
                              C = dr["C"].ToString(),
                              D = dr["D"].ToString(),
                              E = dr["E"].ToString(),
                              F = dr["F"].ToString(),

                              G = dr["G"].ToString(),
                              H = dr["H"].ToString(),
                              I = dr["I"].ToString(),
                              J = dr["J"].ToString(),
                              K = dr["K"].ToString(),
                              L = dr["L"].ToString(),

                              M = dr["M"].ToString(),
                              N = dr["N"].ToString(),
                              O = dr["O"].ToString(),
                              P = dr["P"].ToString(),
                              Q = dr["Q"].ToString(),
                              R = dr["R"].ToString(),

                              S = dr["S"].ToString(),
                              T = dr["T"].ToString(),
                              U = dr["U"].ToString(),
                              V = dr["V"].ToString(),
                              W = dr["W"].ToString(),
                              X = dr["X"].ToString(),

                              Y = dr["Y"].ToString(),
                              Z = dr["Z"].ToString(),
                              AA = dr["AA"].ToString(),
                              AB = dr["AB"].ToString(),
                              AC = dr["AC"].ToString(),
                              AD = dr["AD"].ToString(),

                              AE = dr["AE"].ToString(),
                              AF = dr["AF"].ToString()

                          }).ToList();

                /* modelB = (from DataRow dr in dt1.Rows
                           select new HDBBondFormA()
                           {
                               A = dr["A"].ToString(),
                               B = dr["B"].ToString(),
                               C = dr["C"].ToString(),
                               D = dr["D"].ToString(),
                               E = dr["E"].ToString(),
                               F = dr["F"].ToString(),

                               G = dr["G"].ToString(),
                               H = dr["H"].ToString(),
                               I = dr["I"].ToString(),
                               J = dr["J"].ToString(),
                               K = dr["K"].ToString(),
                               L = dr["L"].ToString(),

                               M = dr["M"].ToString(),
                               N = dr["N"].ToString(),
                               O = dr["O"].ToString(),
                             //  P = dr["P"].ToString(),
                             //  Q = dr["Q"].ToString(),
                              // R = dr["R"].ToString(),

                              // S = dr["S"].ToString(),
                              // T = dr["T"].ToString(),
                              // U = dr["U"].ToString(),
                              // V = dr["V"].ToString(),
                              // W = dr["W"].ToString(),
                              // X = dr["X"].ToString(),

                              // Y = dr["Y"].ToString(),
                              // Z = dr["Z"].ToString(),
                              // AA = dr["AA"].ToString(),
                              // AB = dr["AB"].ToString(),
                              // AC = dr["AC"].ToString(),
                              // AD = dr["AD"].ToString(),

                              // AE = dr["AE"].ToString(),
                              // AF = dr["AF"].ToString()

                           }).ToList();

              /*   modelC = (from DataRow dr in dt2.Rows
                           select new WFLDBondFormA()
                           {
                               A = dr["A"].ToString(),
                               B = dr["B"].ToString(),
                               C = dr["C"].ToString(),
                               D = dr["D"].ToString(),
                               E = dr["E"].ToString(),
                               F = dr["F"].ToString(),

                               G = dr["G"].ToString(),
                               H = dr["H"].ToString(),
                               I = dr["I"].ToString(),
                               J = dr["J"].ToString(),
                               K = dr["K"].ToString(),
                               L = dr["L"].ToString(),

                               M = dr["M"].ToString(),
                               N = dr["N"].ToString(),
                               O = dr["O"].ToString(),
                               P = dr["P"].ToString(),
                               Q = dr["Q"].ToString(),
                               R = dr["R"].ToString(),

                               S = dr["S"].ToString(),
                               T = dr["T"].ToString(),
                               U = dr["U"].ToString(),
                               V = dr["V"].ToString(),
                               W = dr["W"].ToString(),
                               X = dr["X"].ToString(),

                               Y = dr["Y"].ToString(),
                               Z = dr["Z"].ToString(),
                               AA = dr["AA"].ToString(),
                               AB = dr["AB"].ToString(),
                               AC = dr["AC"].ToString(),
                               AD = dr["AD"].ToString(),

                               AE = dr["AE"].ToString(),
                               AF = dr["AF"].ToString()

                           }).ToList();*/

                _DBResponse.Data = GetBondFormAExcel(modelA, date1.ToString("dd/MM/yyyy"), date2.ToString("dd/MM/yyyy"));

            }

            //foreach (DataRow dr in ds.Tables[1].Rows)
            //{
            //    InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]);
            //    CRAmount = Convert.ToDecimal(dr["CRAmount"]);
            //}

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

        private string GetBondFormAExcel(List<HDBBondFormA> modelA, string datevalue, string datevalueto)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            string month = "";
            string MonthPrint = "";
            string YearPrint = "";
            month = datevalueto.Split('/')[1].ToString();
            YearPrint = datevalueto.Split('/')[2].ToString();
            switch (month)
            {
                case "01":
                    MonthPrint = "JANUARY";
                    break;
                case "02":
                    MonthPrint = "FEBRUARY";
                    break;
                case "03":
                    MonthPrint = "MARCH";
                    break;
                case "04":
                    MonthPrint = "APRIL";
                    break;
                case "05":
                    MonthPrint = "MAY";
                    break;
                case "06":
                    MonthPrint = "JUNE";
                    break;
                case "07":
                    MonthPrint = "JULY";
                    break;
                case "08":
                    MonthPrint = "AUGUST";
                    break;
                case "09":
                    MonthPrint = "SEPTEMBER";
                    break;
                case "10":
                    MonthPrint = "OCTOBER";
                    break;
                case "11":
                    MonthPrint = "NOVEMBER";
                    break;
                case "12":
                    MonthPrint = "DECEMBER";
                    break;
            }


            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"Form - A";

                exl.MargeCell("A1:AF1", "Form - A", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A2:AF2", "Form to be maintained by the warehouse licensee of the receipt, handling, storing and removal of the warehoused goods.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A3:AF3", "(in terms of Circular No. 25 /2016-Customs dated 08.06.2016)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A4:AF4", "Warehouse code and address: CWC-CFS-Kukatpally-INSNF6U002", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("A5:R5", "Receipts", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("S5:X5", "Handling and storage", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("Y5:AF5", "Removal", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("A6:A9", "Sl No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B6:B9", "Bill of Entry No. and date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C6:C9", "Customs Station of import", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D6:D9", "Bond No.and date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E6:E9", "Description of goods", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("F6:F9", "Description and No.of packages", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G6:G9", "Marks and numbers on packages", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H6:H9", "Units Weight and quantity", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I6:I9", "Value", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J6:J9", "Duty assessed", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K6:K9", "Date of order under Section 60(1)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("L6:L9", "Warehouse code and address (in case of bond to bond transfer)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("M6:M9", "Registration No.of means of tranport", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("N6:N9", "OTL No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("O6:O9", "Quantity adviced", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("P6:P9", "Quantity received", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("Q6:Q9", "Breakage/damage", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("R6:R9", "shortage", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("S6:S9", "Sample drawn by government agencies", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("T6:T9", "Activities undertaken under section 64", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("U6:U9", "Date if expiry of initial Bonding period", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("V6:V9", "Period extended upto", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("W6:W9", "Details of Bank Guarantee", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("X6:X9", "Relinquishment", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("Y6:Y9", "Date and time of removal", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("Z6:Z9", "Purpose of removal (home consumption/ deposit in another warehouse/ export/ sold under Sec.72 (2)/ destruction etc). Give details.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("AA6:AA9", "Quantity cleared", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("AB6:AB9", "Value", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("AC6:AC9", "Duty", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("AD6:AD9", "Interest", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("AE6:AE9", "Balance quantity", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("AF6:AF9", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                int FirstLoop = Convert.ToInt32(modelA.Count.ToString());
                //int SecLoop = Convert.ToInt32(modelB.Count.ToString());

                FirstLoop = (FirstLoop + 10);
                //  SecLoop = (FirstLoop + SecLoop + 1);

                string fstInsert = "A" + FirstLoop.ToString();
                string lstInsert = "AF" + FirstLoop.ToString();

                string fstFinal = fstInsert + ":" + lstInsert;

                //   string scndInsertbegn = "A" + SecLoop.ToString();
                //   string scndlineInsertLast = "AF" + SecLoop.ToString();
                //   string scndFinal = scndInsertbegn + ":" + scndlineInsertLast;

                exl.AddTable("A", 10, modelA, new[] { 6, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 });

                //exl.Add(fstInsert:lstInsert, "Form - A", DynamicExcel.CellAlignment.Middle);

                exl.AddCell("A" + FirstLoop.ToString(), "", DynamicExcel.CellAlignment.Middle);
                //   exl.MargeCell(fstFinal, "Time Barred Bonds Transferred From CW, APMC Yard, Yeshwantpura and KR.Puram and lying in storage as on 01.02.2020 (OB)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                //  exl.AddTable("A", (FirstLoop + 1), modelB, new[] { 6, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 });

                //  exl.AddCell("A" + SecLoop.ToString(), "", DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell(scndFinal, "DELIVERY STATUS FOR THE MONTH '" + MonthPrint + "' '" + YearPrint + "", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                //  exl.AddTable("A", (SecLoop + 1), modelC, new[] { 6, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 });


                exl.Save();
            }
            return excelFile;
        }

        #endregion
        #region Bond Form-B Report
        public void GetBondFormBReport(DateTime date1, DateTime date2)
        {
            String PeriodFrom = date1.ToString("yyyy/MM/dd");
            String PeriodTo = date2.ToString("yyyy/MM/dd");

            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
            //  LstParam.Add(new MySqlParameter { ParameterName = "In_Type", MySqlDbType = MySqlDbType.VarChar, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //var objRegOutwardSupply = (List<RegisterOfOutwardSupplyModel>)DataAccess.ExecuteDynamicSet<List<RegisterOfOutwardSupplyModel>>("GetRegisterofOutwardSupply", DParam);
            DataSet ds = DataAccess.ExecuteDataSet("GetBondFormBRpt", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            //  DataTable dt1 = ds.Tables[1];
            // DataTable dt2 = ds.Tables[2];


            List<HDB_BondFormB> model = new List<HDB_BondFormB>();
            //  List<HDBBondFormA> modelB = new List<HDBBondFormA>();
            // List<WFLDBondFormA> modelC = new List<WFLDBondFormA>();
            try
            {
                model = (from DataRow dr in dt.Rows
                         select new HDB_BondFormB()
                         {
                             A = dr["A"].ToString(),
                             B = dr["B"].ToString(),
                             C = dr["C"].ToString(),
                             D = dr["D"].ToString(),
                             E = dr["E"].ToString(),
                             F = dr["F"].ToString(),

                             G = dr["G"].ToString(),
                             H = dr["H"].ToString(),
                             I = dr["I"].ToString(),
                             J = dr["J"].ToString(),
                             K = dr["K"].ToString(),
                             L = dr["L"].ToString(),

                             M = dr["M"].ToString(),
                             /*  N = dr["N"].ToString(),
                               O = dr["O"].ToString(),
                               P = dr["P"].ToString(),
                               Q = dr["Q"].ToString(),
                               R = dr["R"].ToString(),

                               S = dr["S"].ToString(),
                               T = dr["T"].ToString(),
                               U = dr["U"].ToString(),
                               V = dr["V"].ToString(),
                               W = dr["W"].ToString(),
                               X = dr["X"].ToString(),

                               Y = dr["Y"].ToString(),
                               Z = dr["Z"].ToString(),
                               AA = dr["AA"].ToString(),
                               AB = dr["AB"].ToString(),
                               AC = dr["AC"].ToString(),
                               AD = dr["AD"].ToString(),

                               AE = dr["AE"].ToString(),
                               AF = dr["AF"].ToString()
                               */
                         }).ToList();

                /* modelB = (from DataRow dr in dt1.Rows
                           select new HDBBondFormA()
                           {
                               A = dr["A"].ToString(),
                               B = dr["B"].ToString(),
                               C = dr["C"].ToString(),
                               D = dr["D"].ToString(),
                               E = dr["E"].ToString(),
                               F = dr["F"].ToString(),

                               G = dr["G"].ToString(),
                               H = dr["H"].ToString(),
                               I = dr["I"].ToString(),
                               J = dr["J"].ToString(),
                               K = dr["K"].ToString(),
                               L = dr["L"].ToString(),

                               M = dr["M"].ToString(),
                               N = dr["N"].ToString(),
                               O = dr["O"].ToString(),
                             //  P = dr["P"].ToString(),
                             //  Q = dr["Q"].ToString(),
                              // R = dr["R"].ToString(),

                              // S = dr["S"].ToString(),
                              // T = dr["T"].ToString(),
                              // U = dr["U"].ToString(),
                              // V = dr["V"].ToString(),
                              // W = dr["W"].ToString(),
                              // X = dr["X"].ToString(),

                              // Y = dr["Y"].ToString(),
                              // Z = dr["Z"].ToString(),
                              // AA = dr["AA"].ToString(),
                              // AB = dr["AB"].ToString(),
                              // AC = dr["AC"].ToString(),
                              // AD = dr["AD"].ToString(),

                              // AE = dr["AE"].ToString(),
                              // AF = dr["AF"].ToString()

                           }).ToList();

              /*   modelC = (from DataRow dr in dt2.Rows
                           select new WFLDBondFormA()
                           {
                               A = dr["A"].ToString(),
                               B = dr["B"].ToString(),
                               C = dr["C"].ToString(),
                               D = dr["D"].ToString(),
                               E = dr["E"].ToString(),
                               F = dr["F"].ToString(),

                               G = dr["G"].ToString(),
                               H = dr["H"].ToString(),
                               I = dr["I"].ToString(),
                               J = dr["J"].ToString(),
                               K = dr["K"].ToString(),
                               L = dr["L"].ToString(),

                               M = dr["M"].ToString(),
                               N = dr["N"].ToString(),
                               O = dr["O"].ToString(),
                               P = dr["P"].ToString(),
                               Q = dr["Q"].ToString(),
                               R = dr["R"].ToString(),

                               S = dr["S"].ToString(),
                               T = dr["T"].ToString(),
                               U = dr["U"].ToString(),
                               V = dr["V"].ToString(),
                               W = dr["W"].ToString(),
                               X = dr["X"].ToString(),

                               Y = dr["Y"].ToString(),
                               Z = dr["Z"].ToString(),
                               AA = dr["AA"].ToString(),
                               AB = dr["AB"].ToString(),
                               AC = dr["AC"].ToString(),
                               AD = dr["AD"].ToString(),

                               AE = dr["AE"].ToString(),
                               AF = dr["AF"].ToString()

                           }).ToList();*/

                _DBResponse.Data = GetBondFormBExcel(model, date1.ToString("dd/MM/yyyy"), date2.ToString("dd/MM/yyyy"));

            }

            //foreach (DataRow dr in ds.Tables[1].Rows)
            //{
            //    InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]);
            //    CRAmount = Convert.ToDecimal(dr["CRAmount"]);
            //}

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

        private string GetBondFormBExcel(List<HDB_BondFormB> model, string datevalue, string datevalueto)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            string month = "";
            string MonthPrint = "";
            string YearPrint = "";
            month = datevalueto.Split('/')[1].ToString();
            YearPrint = datevalueto.Split('/')[2].ToString();
            switch (month)
            {
                case "01":
                    MonthPrint = "JANUARY";
                    break;
                case "02":
                    MonthPrint = "FEBRUARY";
                    break;
                case "03":
                    MonthPrint = "MARCH";
                    break;
                case "04":
                    MonthPrint = "APRIL";
                    break;
                case "05":
                    MonthPrint = "MAY";
                    break;
                case "06":
                    MonthPrint = "JUNE";
                    break;
                case "07":
                    MonthPrint = "JULY";
                    break;
                case "08":
                    MonthPrint = "AUGUST";
                    break;
                case "09":
                    MonthPrint = "SEPTEMBER";
                    break;
                case "10":
                    MonthPrint = "OCTOBER";
                    break;
                case "11":
                    MonthPrint = "NOVEMBER";
                    break;
                case "12":
                    MonthPrint = "DECEMBER";
                    break;
            }


            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"Form - B";

                exl.MargeCell("A1:AF1", "Form - B", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A2:AF2", "(See Para 3 of Circular No 25 /2016-Customs dated 08.06.2016)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A3:AF3", "Details of goods stored in the warehouse where the period for which they may remain warehoused under section 61 is expiring in the following month.", DynamicExcel.CellAlignment.Middle);
                //    exl.MargeCell("A4:AF4", "Warehouse code and address: CWC-CFS-Kukatpally-INSNF6U002", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                //   exl.MargeCell("A5:R5", "Receipts", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //   exl.MargeCell("S5:X5", "Handling and storage", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //   exl.MargeCell("Y5:AF5", "Removal", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("A6:A9", "Sl No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B6:B9", "Bill of Entry No. and date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C6:C9", "Bond No. and date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D6:D9", "Date of order under Section 60(1)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E5:H5", "Balance goods in the warehouse", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E6:E9", "Invoice no.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F6:F9", "Sl.No", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G6:G9", "Description of goods", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H6:H9", "Quantity", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I6:I9", "Date of expiry of initial bonding period", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J6:J9", "Details of extensions(Period extended upto)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K6:K9", "Details of Bank Guarantee", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("L6:L9", "Date of expiry of Bonding period", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("M6:M9", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                // exl.MargeCell("N6:N9", "OTL No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                // exl.MargeCell("O6:O9", "Quantity adviced", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                //  exl.MargeCell("P6:P9", "Quantity received", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("Q6:Q9", "Breakage/damage", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("R6:R9", "shortage", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("S6:S9", "Sample drawn by government agencies", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("T6:T9", "Activities undertaken under section 64", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("U6:U9", "Date if expiry of initial Bonding period", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("V6:V9", "Period extended upto", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("W6:W9", "Details of Bank Guarantee", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("X6:X9", "Relinquishment", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("Y6:Y9", "Date and time of removal", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                //  exl.MargeCell("Z6:Z9", "Purpose of removal (home consumption/ deposit in another warehouse/ export/ sold under Sec.72 (2)/ destruction etc). Give details.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("AA6:AA9", "Quantity cleared", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("AB6:AB9", "Value", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("AC6:AC9", "Duty", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("AD6:AD9", "Interest", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("AE6:AE9", "Balance quantity", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("AF6:AF9", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                // int FirstLoop = Convert.ToInt32(modelA.Count.ToString());
                //int SecLoop = Convert.ToInt32(modelB.Count.ToString());

                //   FirstLoop = (FirstLoop + 10);
                //  SecLoop = (FirstLoop + SecLoop + 1);

                //    string fstInsert = "A" + FirstLoop.ToString();
                //       string lstInsert = "AF" + FirstLoop.ToString();

                //       string fstFinal = fstInsert + ":" + lstInsert;

                //   string scndInsertbegn = "A" + SecLoop.ToString();
                //   string scndlineInsertLast = "AF" + SecLoop.ToString();
                //   string scndFinal = scndInsertbegn + ":" + scndlineInsertLast;

                exl.AddTable("A", 10, model, new[] { 6, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 });

                //exl.Add(fstInsert:lstInsert, "Form - A", DynamicExcel.CellAlignment.Middle);

                //  exl.AddCell("A" + FirstLoop.ToString(), "", DynamicExcel.CellAlignment.Middle);
                //   exl.MargeCell(fstFinal, "Time Barred Bonds Transferred From CW, APMC Yard, Yeshwantpura and KR.Puram and lying in storage as on 01.02.2020 (OB)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                //  exl.AddTable("A", (FirstLoop + 1), modelB, new[] { 6, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 });

                //  exl.AddCell("A" + SecLoop.ToString(), "", DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell(scndFinal, "DELIVERY STATUS FOR THE MONTH '" + MonthPrint + "' '" + YearPrint + "", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                //  exl.AddTable("A", (SecLoop + 1), modelC, new[] { 6, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 });


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
            List<Hdb_E04Report> LstE04 = new List<Hdb_E04Report>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstE04.Add(new Hdb_E04Report
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
            Hdb_E04Report objE04Report = new Hdb_E04Report();
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
            List<Hdb_E04Report> LstE04Report = new List<Hdb_E04Report>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstE04Report.Add(new Hdb_E04Report
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
        #region Work Slip 
        public void GetWorkSlipList(HDB_WorkSlip ObjWorkSlipReport)
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
            LstParam.Add(new MySqlParameter { ParameterName = "repoType", MySqlDbType = MySqlDbType.String, Value = ObjWorkSlipReport.WorkSlipType });

            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("RptWorkSlip_newww", CommandType.StoredProcedure, DParam);







      //      int srno = 0;
         //   int depsrno = 0;
         //   foreach (DataRow dr in ds.Tables[0].Rows)
          //  {
           //     srno = srno + 1;
           //     BondDaily.lstBondDepositeTransaction.Add(new Hdb_BondDepositeDailyTransactionReport
              //  {
              //      SrNo = srno,

















                    DataTable dt = Result.Tables[0];
            _DBResponse = new DatabaseResponse();
                int srno = 0;

                List<HDB_WorkSlipDetails> lstPV = new List<HDB_WorkSlipDetails>();
            try
            {

                if (Result.Tables.Count > 0)
                {
                   // srno = srno + 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        srno = srno + 1;
                        lstPV.Add(new HDB_WorkSlipDetails
                        {


                            SerialNo = srno,
                            InvoiceNo = dr["Invoiceno"].ToString(),
                            ClauseNo = dr["Clause"].ToString(),
                            SAC = dr["SAC"].ToString(),
                            NoOfPackage = dr["NoOfPackage"].ToString(),
                            PortName = dr["PortName"].ToString(),
                            ContainerNo = dr["ContainerNo"].ToString(),
                            Size = dr["Size"].ToString(),
                           Weight = dr["GrossWeight1"].ToString(),
                           Distance= dr["Distance"].ToString(),
                           CFSCode= dr["CFSCode"].ToString(),
                            WorkOrderNo = dr["WorkOrderNo"].ToString(),
                            Remarks = dr["Remarks"].ToString(),

                        });
                    }

                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = GetWorkSlipReportExcel(lstPV,ObjWorkSlipReport.WorkSlipType,ObjWorkSlipReport.PeriodFrom,ObjWorkSlipReport.PeriodTo);
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

        private string GetWorkSlipReportExcel(List<HDB_WorkSlipDetails> lstPV,string WorkSlipType,string PeriodFrom,string PeriodTo)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION";
                string typeOfValue = "";

                typeOfValue = "From " + PeriodFrom + " To " + PeriodTo;
                exl.MargeCell("A1:N1", title, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A2:N2", "(A Govt.of India Undertaking)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A3:N3", "CFS, Kukatpally, Hyderabad", DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A4:N4", "Workslip No. 1", DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
               // exl.MargeCell("A5:N5", "Type of Workslip: "+ WorkslipType, DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A5:N5", "" + typeOfValue, DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A6:N6", "Type of Workslip: " + WorkSlipType, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
             //   exl.MargeCell("A7:I7", "Destuffing ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
              //  exl.MargeCell("J7:N7", "Handling Details", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("A7:A7", "S.N", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B7:B7", "Invoice /Bill of Supply No./ Debit Note No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C7:C7", "Clause No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D7:D7", "SAC", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E7:E7", "Container/CBT Number", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F7:F7", "Size", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G7:G7", "No.of Packages", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H7:H7", "Port Name ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I7:I7", "Weight(KG)/Weight Slab (MT)/ CBM", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J7:J7", "Distance Slab(if applicable)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K7:K7", "CFS Code", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("L7:L7", "WorkOrder No.& Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("M7:M7", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
               // exl.MargeCell("N7:N7", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.AddTable<HDB_WorkSlipDetails>("A", 8, lstPV, new[] { 6, 20, 20, 20, 20, 20, 10, 15, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 });
                //var NoOfUnits = lstPV.Sum(o => o.NoOfUnits);
                //var NoOfUnitsRec = lstPV.Sum(o => o.NoOfUnitsRec);
                //var Weight = lstPV.Sum(o => o.Weight);
                //var Area = lstPV.Sum(o => o.Area);

                //var CBM = lstPV.Sum(o => o.CBM);

                //var CIF = lstPV.Sum(o => o.CIF);

                //exl.MargeCell("A" + (lstPV.Count + 8).ToString() + ":J" + (lstPV.Count + 8).ToString() + "", "TOTAL", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("K" + (lstPV.Count + 8).ToString(), NoOfUnits.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("L" + (lstPV.Count + 8).ToString(), NoOfUnitsRec.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("M" + (lstPV.Count + 8).ToString(), Weight.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("O" + (lstPV.Count + 8).ToString(), Area.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("P" + (lstPV.Count + 8).ToString(), CBM.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("Q" + (lstPV.Count + 8).ToString(), CIF.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);

                string cellpos1 = "B" + (lstPV.Count + 15).ToString() + ":" + "D" + (lstPV.Count + 15).ToString();
                string cellpos2 = "G" + (lstPV.Count + 15).ToString() + ":" + "I" + (lstPV.Count + 15).ToString();
                string cellpos3 = "K" + (lstPV.Count + 15).ToString() + ":" + "M" + (lstPV.Count + 15).ToString();
                string cellpos4 = "O" + (lstPV.Count + 15).ToString() + ":" + "Q" + (lstPV.Count + 15).ToString();
                exl.MargeCell(cellpos1, "Name & Sign. of representative of HTC", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell(cellpos2, "Name & Sign. of Shed I/C", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell(cellpos3, "Name &Sign. Of Export I/C", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell(cellpos4, "Sign. Of Manager-CFS", DynamicExcel.CellAlignment.Middle);
                exl.Save(); 
            }
            return excelFile;
        }
        #endregion
        #region Work Slip detail
        public void GetWorkSlipdetailList(string FromDate, string ToDate, string WorkSlipType)
        {

            DateTime dtfrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "inv_StartDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "inv_EndDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "repoType", MySqlDbType = MySqlDbType.String, Value = WorkSlipType });

            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("RptWorkSlipDetail", CommandType.StoredProcedure, DParam);
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



        //      int srno = 0;
        //   int depsrno = 0;
        //   foreach (DataRow dr in ds.Tables[0].Rows)
        //  {
        //     srno = srno + 1;
        //     BondDaily.lstBondDepositeTransaction.Add(new Hdb_BondDepositeDailyTransactionReport
        //  {
        //      SrNo = srno,

















        /* DataTable dt = Result.Tables[0];
         _DBResponse = new DatabaseResponse();
         int srno = 0;

         List<HDB_WorkSlippDetails> lstPV = new List<HDB_WorkSlippDetails>();
         List<HDB_HTDetails> WorkSlipDetailList = new List<HDB_HTDetails>();
         try
         {

             if (Result.Tables.Count > 0)
             {
                 // srno = srno + 1;
                 foreach (DataRow dr in Result.Tables[0].Rows)
                 {
                     srno = srno + 1;
                     lstPV.Add(new HDB_WorkSlippDetails
                     {


                         SerialNo = srno,
                         invoiceid= dr["Invoiceid"].ToString(),
                         WorkOrderDate = dr["Invoicedate"].ToString(),
                         invoiceno = dr["invoiceno"].ToString(),
                         ContainerNo = dr["ContainerNo"].ToString(),


                     });
                 }
                 foreach (DataRow dr in Result.Tables[1].Rows)
                 {
                     srno = srno + 1;
                     WorkSlipDetailList.Add(new HDB_HTDetails
                     {
                         invoiceid = dr["Invoiceid"].ToString(),
                         invoiceno = dr["invoiceno"].ToString(),
                         clause = dr["clause"].ToString(),
                         ChargeName = dr["ChargeName"].ToString(),
                         GrossWeight = dr["GrossWeight"].ToString(),
                         Amount = dr["Amount"].ToString(),
                         CWCMargin = dr["CWCMargin"].ToString(),
                         HTCMargin = dr["HTCMargin"].ToString(),
                         CRNo = dr["crno"].ToString(),
                         CRAmount = dr["CRAmount"].ToString(),
                         HTCContractorRate = dr["HTCContractorRate"].ToString(),

                     });
                 }

                 _DBResponse.Status = 0;
                 _DBResponse.Message = "No Data";
                 _DBResponse.Data = GetWorkSlippReportExcel(lstPV,WorkSlipDetailList, ObjWorkSlipReport.WorkSlipType, ObjWorkSlipReport.PeriodFrom, ObjWorkSlipReport.PeriodTo);
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
     }*/

    /*    private string GetWorkSlippReportExcel(List<HDB_WorkSlippDetails> lstPV, List<HDB_HTDetails> WorkSlipDetailList, string WorkSlipType, string PeriodFrom, string PeriodTo)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION";
                string typeOfValue = "";

                typeOfValue = "From " + PeriodFrom + " To " + PeriodTo;
                exl.MargeCell("A1:N1", title, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A2:N2", "(A Govt.of India Undertaking)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A3:N3", "CFS, Kukatpally, Hyderabad", DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A4:N4", "Work-slip Report", DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                // exl.MargeCell("A5:N5", "Type of Workslip: "+ WorkslipType, DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A5:N5", "" + typeOfValue, DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A6:N6", "Type of Workslip: " + WorkSlipType, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //   exl.MargeCell("A7:I7", "Destuffing ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("J7:N7", "Handling Details", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("A7:A7", "Work-slip No. & Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B7:B7", "Date of Operation / Date of Work Order", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C7:C7", "Invoice / Bill of Supply No. & Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D7:D7", "Container No. & Size", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E7:E7", " H&T Clause ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F7:F7", " Description of clause", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G7:G7", " Cargo wieight ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H7:H7", "Amount as per Tariff against H&T clause", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I7:I7", "CWC margin in amount as per H&T tariff", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J7:J7", "HTC margin in amount as per H&T tariff ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K7:K7", "H&T rate per qtl ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("L7:L7", "CR No.& Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("M7:M7", "Amount Realized", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("K7:K7", "CFS Code", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("L7:L7", "WorkOrder No.& Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("M7:M7", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                // exl.MargeCell("N7:N7", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                // exl.AddTable<HDB_WorkSlippDetails>("A", 8, lstPV, new[] { 6, 20, 20, 20, 20, 20, 10, 15, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 });
                //var NoOfUnits = lstPV.Sum(o => o.NoOfUnits);
                //var NoOfUnitsRec = lstPV.Sum(o => o.NoOfUnitsRec);
                //var Weight = lstPV.Sum(o => o.Weight);
                //var Area = lstPV.Sum(o => o.Area);

                //var CBM = lstPV.Sum(o => o.CBM);

                //var CIF = lstPV.Sum(o => o.CIF);

                //exl.MargeCell("A" + (lstPV.Count + 8).ToString() + ":J" + (lstPV.Count + 8).ToString() + "", "TOTAL", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("K" + (lstPV.Count + 8).ToString(), NoOfUnits.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("L" + (lstPV.Count + 8).ToString(), NoOfUnitsRec.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("M" + (lstPV.Count + 8).ToString(), Weight.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("O" + (lstPV.Count + 8).ToString(), Area.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("P" + (lstPV.Count + 8).ToString(), CBM.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("Q" + (lstPV.Count + 8).ToString(), CIF.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                try
                {
                    var ln = 0;
                    int TCOBLNo = 0; int TCBOENo = 0; decimal TTStorageCharge = 0; decimal TTDocumentationCharge = 0; decimal TTFacilitationCharge = 0; decimal TTAggregationCharge = 0; decimal TTWeight = 0;
                    for (int i = 0; i < lstPV.Count(); i++)
                    {
                        var x = lstPV[i];
                        var y = lstPV[i - 1];

                        int COBLNo = 0; int CBOENo = 0; decimal TStorageCharge = 0; decimal TDocumentationCharge = 0; decimal TFacilitationCharge = 0; decimal TAggregationCharge = 0; decimal TWeight = 0;

                        exl.AddCell("A" + (9 + ln).ToString(), lstPV[i].SerialNo, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("B" + (9 + ln).ToString(), lstPV[i].WorkOrderDate, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("C" + (9 + ln).ToString(), lstPV[i].invoiceno, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("D" + (9 + ln).ToString(), lstPV[i].ContainerNo, DynamicExcel.CellAlignment.CenterRight);

                        // exl.AddCell("D" + (9 + ln).ToString(), DDetail[i].ArrivalDate, DynamicExcel.CellAlignment.CenterRight);
                        // exl.AddCell("E" + (9 + ln).ToString(), DDetail[i].DestuffingDate, DynamicExcel.CellAlignment.CenterRight);
                        for (int j = 0; j < WorkSlipDetailList.Count(); j++)
                        {
                            if (lstPV[i].invoiceid == WorkSlipDetailList[j].invoiceid)


                            {

                                exl.AddCell("E" + (9 + ln).ToString(), WorkSlipDetailList[j].clause, DynamicExcel.CellAlignment.CenterRight);
                                exl.AddCell("F" + (9 + ln).ToString(), WorkSlipDetailList[j].ChargeName, DynamicExcel.CellAlignment.CenterRight);
                                exl.AddCell("G" + (9 + ln).ToString(), WorkSlipDetailList[j].GrossWeight, DynamicExcel.CellAlignment.CenterRight);
                                exl.AddCell("H" + (9 + ln).ToString(), WorkSlipDetailList[j].Amount, DynamicExcel.CellAlignment.CenterRight);
                                exl.AddCell("I" + (9 + ln).ToString(), WorkSlipDetailList[j].CWCMargin, DynamicExcel.CellAlignment.CenterRight);
                                exl.AddCell("J" + (9 + ln).ToString(), WorkSlipDetailList[j].HTCMargin, DynamicExcel.CellAlignment.CenterRight);
                                exl.AddCell("K" + (9 + ln).ToString(), WorkSlipDetailList[j].HTCContractorRate, DynamicExcel.CellAlignment.CenterRight);
                                exl.AddCell("L" + (9 + ln).ToString(), WorkSlipDetailList[j].CRNo, DynamicExcel.CellAlignment.CenterRight);
                                exl.AddCell("M" + (9 + ln).ToString(), WorkSlipDetailList[j].CRAmount, DynamicExcel.CellAlignment.CenterRight);
                                // exl.AddCell("L" + (9 + ln).ToString(), WorkSlipDetailList[j].CRNo, DynamicExcel.CellAlignment.CenterRight);
                                // exl.AddCell("M" + (9 + ln).ToString(), WorkSlipDetailList[j].CRAmount, DynamicExcel.CellAlignment.CenterRight);



                                ln = ln + 1;

                            }
                            // exl.MargeCell(("A1:C4") + (9 + ln).ToString(),item.invoiceno, DynamicExcel.CellAlignment.CenterRight);
                            // exl.MargeCell(("A") + (9 + ln).ToString(), item.invoiceno, DynamicExcel.CellAlignment.CenterRight);
                            // exl.MargeCell(("B") + (9 + ln).ToString(), item.invoiceno, DynamicExcel.CellAlignment.CenterRight);
                            // exl.MargeCell(("C") + (9 + ln).ToString(), item.invoiceno, DynamicExcel.CellAlignment.CenterRight);
                            // exl.MargeCell(("D") + (9 + ln).ToString(), item.invoiceno, DynamicExcel.CellAlignment.CenterRight);

                        }

                        //-------------

                        //------------
                        // ln = ln + 1;
                    }
                    //-------------


                    //exl.AddTable<chn_test>("A", 9, DDetail, new[] { 6, 20, 20, 20, 12});
                    //exl.AddTable<CHN_DestuffingDetailReport>("F", 9, DestuffingDetail, new[] { 20,20, 10, 15, 20, 12, 12, 8, 14, 20, 10,20,20 });

                }
                // }
                catch (Exception ex)
                {

                }

                // string cellpos1 = "B" + (lstPV.Count + 15).ToString() + ":" + "D" + (lstPV.Count + 15).ToString();
                //  string cellpos2 = "G" + (lstPV.Count + 15).ToString() + ":" + "I" + (lstPV.Count + 15).ToString();
                //  string cellpos3 = "K" + (lstPV.Count + 15).ToString() + ":" + "M" + (lstPV.Count + 15).ToString();
                //   string cellpos4 = "O" + (lstPV.Count + 15).ToString() + ":" + "Q" + (lstPV.Count + 15).ToString();
                //   exl.MargeCell(cellpos1, "Name & Sign. of representative of HTC", DynamicExcel.CellAlignment.Middle);
                //   exl.MargeCell(cellpos2, "Name & Sign. of Shed I/C", DynamicExcel.CellAlignment.Middle);
                //   exl.MargeCell(cellpos3, "Name &Sign. Of Export I/C", DynamicExcel.CellAlignment.Middle);
                //   exl.MargeCell(cellpos4, "Sign. Of Manager-CFS", DynamicExcel.CellAlignment.Middle);
                exl.Save();
            }
            return excelFile;
        }*/
        #endregion

        #region Work Slip bond detail 
        public void GetWorkSlipBonddetailList(HDB_WorkSlipp ObjWorkSlipReport)
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
            DataSet Result = DataAccess.ExecuteDataSet("BondWorKSlip", CommandType.StoredProcedure, DParam);







            //      int srno = 0;
            //   int depsrno = 0;
            //   foreach (DataRow dr in ds.Tables[0].Rows)
            //  {
            //     srno = srno + 1;
            //     BondDaily.lstBondDepositeTransaction.Add(new Hdb_BondDepositeDailyTransactionReport
            //  {
            //      SrNo = srno,

















            DataTable dt = Result.Tables[0];
            _DBResponse = new DatabaseResponse();
            int srno = 0;

            List<HDB_BondWorkslip> lstPV = new List<HDB_BondWorkslip>();
           
            try
            {

                if (Result.Tables.Count > 0)
                {
                    // srno = srno + 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        srno = srno + 1;
                        lstPV.Add(new HDB_BondWorkslip
                        {


                            SerialNo = srno,
                            invoiceid = dr["Invoiceid"].ToString(),
                            WorkOrderNo = dr["WorkOrderNo"].ToString(),
                            invoiceno = dr["invoiceno"].ToString(),
                            Clause = dr["Clause"].ToString(),
                            ExBoeNo = dr["ExBoeNo"].ToString(),
                            Units = dr["Units"].ToString(),
                            Weight = dr["Weight"].ToString(),
                            BondNo = dr["BondNo"].ToString(),
                            SacNo = dr["SacNo"].ToString(),
                            ContainerNo = dr["ContainerNo"].ToString(),
                            Size = dr["Size"].ToString(),
                            Remarks = dr["Remarks"].ToString(),



                        });
                    }
                    

                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = GetWorkSlippBondReportExcel(lstPV,  ObjWorkSlipReport.PeriodFrom, ObjWorkSlipReport.PeriodTo);
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

        private string GetWorkSlippBondReportExcel(List<HDB_BondWorkslip> lstPV,   string PeriodFrom, string PeriodTo)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION";
                string typeOfValue = "";
                string distance = "";
                string port = "";

                typeOfValue = "From " + PeriodFrom + " To " + PeriodTo;
                exl.MargeCell("A1:N1", title, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A2:N2", "(A Govt.of India Undertaking)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A3:N3", "CFS, Kukatpally, Hyderabad", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A4:N4", "Bond-Work-slip Report", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                // exl.MargeCell("A5:N5", "Type of Workslip: "+ WorkslipType, DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A5:N5", "" + typeOfValue, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A6:N6", "Work-slip No. & Date", DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                //   exl.MargeCell("A6:N6", "Type of Workslip: " + WorkSlipType, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //   exl.MargeCell("A7:I7", "Destuffing ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("J7:N7", "Handling Details", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                // exl.MargeCell("A7:A7", "Work-slip No. & Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A7:A7", "SLNO", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B7:B7", "Invoice / Bill of Supply No. & Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
               
                exl.MargeCell("C7:C7", " H&T Clause ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D7:D7", "SAC", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E7:E7", "Bond Number & Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F7:F7", "Ex-BE No. Dt ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G7:G7", "Container/CBT No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H7:H7", "Size", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I7:I7", "Port Name", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J7:J7", "No.of Packages ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K7:K7", "Weight(KG)/Weight Slab (MT)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("L7:L7", "Distance Slab(if applicable)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                
                    exl.MargeCell("M7:M7", "WorkOrder No.& Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("N7:N7", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("I7:I7", "Work-slip No. & Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                try
                {
                    var ln = 0;
                    int TCOBLNo = 0; int TCBOENo = 0; decimal TTStorageCharge = 0; decimal TTDocumentationCharge = 0; decimal TTFacilitationCharge = 0; decimal TTAggregationCharge = 0; decimal TTWeight = 0;
                    for (int i = 0; i < lstPV.Count(); i++)
                    {
                        int COBLNo = 0; int CBOENo = 0; decimal TStorageCharge = 0; decimal TDocumentationCharge = 0; decimal TFacilitationCharge = 0; decimal TAggregationCharge = 0; decimal TWeight = 0;

                        exl.AddCell("A" + (9 + ln).ToString(), lstPV[i].SerialNo, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("B" + (9 + ln).ToString(), lstPV[i].invoiceno, DynamicExcel.CellAlignment.CenterRight);
                      
                        exl.AddCell("C" + (9 + ln).ToString(), lstPV[i].Clause, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("D" + (9 + ln).ToString(), lstPV[i].SacNo, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("E" + (9 + ln).ToString(), lstPV[i].BondNo, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("F" + (9 + ln).ToString(), lstPV[i].ExBoeNo, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("G" + (9 + ln).ToString(), lstPV[i].ContainerNo, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("H" + (9 + ln).ToString(), lstPV[i].Size, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("I" + (9 + ln).ToString(), port, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("J" + (9 + ln).ToString(), lstPV[i].Units, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("K" + (9 + ln).ToString(), lstPV[i].Weight, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("L" + (9 + ln).ToString(), distance, DynamicExcel.CellAlignment.CenterRight);
                        exl.AddCell("M" + (9 + ln).ToString(), lstPV[i].WorkOrderNo, DynamicExcel.CellAlignment.CenterRight);
                       
                        exl.AddCell("N" + (9 + ln).ToString(), lstPV[i].Remarks, DynamicExcel.CellAlignment.CenterRight);
                        // exl.AddCell("D" + (9 + ln).ToString(), lstPV[i].ExBoeNo, DynamicExcel.CellAlignment.CenterRight);
                        // exl.AddCell("D" + (9 + ln).ToString(), DDetail[i].ArrivalDate, DynamicExcel.CellAlignment.CenterRight);
                        // exl.AddCell("E" + (9 + ln).ToString(), DDetail[i].DestuffingDate, DynamicExcel.CellAlignment.CenterRight);






                        ln = ln + 1;


                        //-------------

                        //------------
                        // ln = ln + 1;
                    }
                    //-------------


                    //exl.AddTable<chn_test>("A", 9, DDetail, new[] { 6, 20, 20, 20, 12});
                    //exl.AddTable<CHN_DestuffingDetailReport>("F", 9, DestuffingDetail, new[] { 20,20, 10, 15, 20, 12, 12, 8, 14, 20, 10,20,20 });


                }
                catch (Exception ex)
                {

                }

                // string cellpos1 = "B" + (lstPV.Count + 15).ToString() + ":" + "D" + (lstPV.Count + 15).ToString();
                //  string cellpos2 = "G" + (lstPV.Count + 15).ToString() + ":" + "I" + (lstPV.Count + 15).ToString();
                //  string cellpos3 = "K" + (lstPV.Count + 15).ToString() + ":" + "M" + (lstPV.Count + 15).ToString();
                //   string cellpos4 = "O" + (lstPV.Count + 15).ToString() + ":" + "Q" + (lstPV.Count + 15).ToString();
                //   exl.MargeCell(cellpos1, "Name & Sign. of representative of HTC", DynamicExcel.CellAlignment.Middle);
                //   exl.MargeCell(cellpos2, "Name & Sign. of Shed I/C", DynamicExcel.CellAlignment.Middle);
                //   exl.MargeCell(cellpos3, "Name &Sign. Of Export I/C", DynamicExcel.CellAlignment.Middle);
                //   exl.MargeCell(cellpos4, "Sign. Of Manager-CFS", DynamicExcel.CellAlignment.Middle);
                exl.Save();
            }
            return excelFile;
        }
        #endregion

        #region Work Slip New
        public void WorkSlipReportNew(DateTime date1, DateTime date2)
        {

            /* DateTime dtfrom = DateTime.ParseExact(ObjWorkSlipReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
             String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
             DateTime dtTo = DateTime.ParseExact(ObjWorkSlipReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
             String PeriodTo = dtTo.ToString("yyyy/MM/dd");*/


            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = date2 });

            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("GetWorkslipReportNew", CommandType.StoredProcedure, DParam);


            DataTable dt = Result.Tables[0];
            _DBResponse = new DatabaseResponse();
            int srno = 0;

            List<HDB_WorkSlipNew> lstPV = new List<HDB_WorkSlipNew>();
            try
            {

                if (Result.Tables.Count > 0)
                {
                    // srno = srno + 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        srno = srno + 1;
                        lstPV.Add(new HDB_WorkSlipNew
                        {


                            SerialNo = srno,
                            //InvoiceId = Convert.ToInt32(dr["InvoiceId"].ToString()),
                            InvoiceNo = dr["Invoiceno"].ToString(),
                            InvoiceDate = dr["InvoiceDate"].ToString(),
                            ClauseNo = dr["Clause"].ToString(),
                            SAC = dr["SAC"].ToString(),
                            ContainerNo = dr["ContainerNo"].ToString(),
                            Size = dr["Size"].ToString(),
                            NoOfPackages = dr["NoOfPackage"].ToString(),
                            PortName = dr["PortName"].ToString(),
                            Weight = dr["GrossWeight1"].ToString(),
                            WeightQtl = dr["GrossWeightQuintal"].ToString(),
                            Distance = dr["Distance"].ToString(),
                            CFSCode = dr["CFSCode"].ToString(),
                            WorkOrderNo = dr["WorkOrderNo"].ToString(),
                            Remarks = dr["Remarks"].ToString(),

                        });
                    }

                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = GetWorkSlipReportNewExcel(lstPV, date1, date2);
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

        private string GetWorkSlipReportNewExcel(List<HDB_WorkSlipNew> lstPV, DateTime PeriodFrom, DateTime PeriodTo)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION";
                string typeOfValue = "";

                typeOfValue = "From " + PeriodFrom + " To " + PeriodTo;
                exl.MargeCell("A1:M1", title, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A2:M2", "(A Govt.of India Undertaking)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A3:M3", "CFS, Kukatpally, Hyderabad", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A4:M4", "Workslip No. 1", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                // exl.MargeCell("A5:N5", "Type of Workslip: "+ WorkslipType, DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A5:M5", "" + typeOfValue, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //exl.MargeCell("A6:N6", "Type of Workslip: " + WorkSlipType, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //   exl.MargeCell("A7:I7", "Destuffing ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("J7:N7", "Handling Details", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("A7:A7", "S.N", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B7:B7", "Invoice /Bill of Supply No./ Debit Note No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C7:C7", "Invoice /Bill of Supply No./ Debit Note Date.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D7:D7", "Clause No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E7:E7", "SAC", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F7:F7", "Container/CBT Number", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G7:G7", "Size", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H7:H7", "No.of Packages", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I7:I7", "Port Name ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J7:J7", "Weight(KG)/Weight Slab (MT)/ CBM", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K7:K7", "Weight In Quintals (roundoff)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("L7:L7", "Distance Slab(if applicable)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("M7:M7", "CFS Code", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("N7:N7", "WorkOrder No.& Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("O7:O7", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                // exl.MargeCell("N7:N7", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.AddTable<HDB_WorkSlipNew>("A", 8, lstPV, new[] { 6, 20, 20, 20, 20, 20, 10, 15, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 , 20, 20});
                //var NoOfUnits = lstPV.Sum(o => o.NoOfUnits);
                //var NoOfUnitsRec = lstPV.Sum(o => o.NoOfUnitsRec);
                //var Weight = lstPV.Sum(o => o.Weight);
                //var Area = lstPV.Sum(o => o.Area);

                //var CBM = lstPV.Sum(o => o.CBM);

                //var CIF = lstPV.Sum(o => o.CIF);

                //exl.MargeCell("A" + (lstPV.Count + 8).ToString() + ":J" + (lstPV.Count + 8).ToString() + "", "TOTAL", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("K" + (lstPV.Count + 8).ToString(), NoOfUnits.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("L" + (lstPV.Count + 8).ToString(), NoOfUnitsRec.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("M" + (lstPV.Count + 8).ToString(), Weight.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("O" + (lstPV.Count + 8).ToString(), Area.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("P" + (lstPV.Count + 8).ToString(), CBM.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("Q" + (lstPV.Count + 8).ToString(), CIF.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);

                string cellpos1 = "B" + (lstPV.Count + 15).ToString() + ":" + "D" + (lstPV.Count + 15).ToString();
                string cellpos2 = "E" + (lstPV.Count + 15).ToString() + ":" + "I" + (lstPV.Count + 15).ToString();
                string cellpos3 = "I" + (lstPV.Count + 15).ToString() + ":" + "M" + (lstPV.Count + 15).ToString();
                string cellpos4 = "L" + (lstPV.Count + 15).ToString() + ":" + "Q" + (lstPV.Count + 15).ToString();
                exl.MargeCell(cellpos1, "Name & Sign. of representative of HTC", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell(cellpos2, "Name & Sign. of Shed I/C", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell(cellpos3, "Name &Sign. Of Export I/C", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell(cellpos4, "Sign. Of Manager-CFS", DynamicExcel.CellAlignment.Middle);
                exl.Save();
            }
            return excelFile;
        }
        #endregion

        #region BIMonthly 
        public void GetBIMonthlyReport(int Month, int Year,int GodownId)
        {
            

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "repoYear", MySqlDbType = MySqlDbType.Int32, Value = Year });
            LstParam.Add(new MySqlParameter { ParameterName = "repoMonth", MySqlDbType = MySqlDbType.Int32, Value = Month });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });

            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("BIMonthlyReport", CommandType.StoredProcedure, DParam);


        




            //      int srno = 0;
            //   int depsrno = 0;
            //   foreach (DataRow dr in ds.Tables[0].Rows)
            //  {
            //     srno = srno + 1;
            //     BondDaily.lstBondDepositeTransaction.Add(new Hdb_BondDepositeDailyTransactionReport
            //  {
            //      SrNo = srno,

















            DataTable dt = Result.Tables[0];
            _DBResponse = new DatabaseResponse();
            int srno = 0;

            List<Hdb_BIMonthlyReport> lstPV = new List<Hdb_BIMonthlyReport>();
         
            try
            {

                if (Result.Tables.Count > 0)
                {
                    // srno = srno + 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        srno = srno + 1;
                        lstPV.Add(new Hdb_BIMonthlyReport
                        {


                            SerialNo = srno,
                            ImporterName = dr["importername"].ToString(),
                            DestuffingDate = dr["DestuffingDate"].ToString(),
                            InvoiceNo = dr["invoiceno"].ToString(),
                            InvoiceDate = dr["invoicedate"].ToString(),
                            BOENo= dr["BOENo"].ToString(),
                            BOEDate = dr["BOEDate"].ToString(),
                            BOLNo= dr["blno"].ToString(),
                            NoOfPKG=Convert.ToInt32(dr["noofpkgs"]),
                            Value=Convert.ToDecimal(dr["Value"]),
                            Duty = Convert.ToDecimal(dr["Duty"]),
                            ValueDuty = Convert.ToDecimal(dr["valdty"]),
                            LCLFCL= dr["lclfcl"].ToString(),
                            GodownName= dr["godownno"].ToString(),
                            DeliveryDate= dr["deliverydate"].ToString(),
                            PreviousAmount = Convert.ToInt32(dr["PreviousAmount"]),
                            CurrentAmount = Convert.ToInt32(dr["CurrentAmount"]),
                            TaxableAmount = Convert.ToInt32(dr["Taxable"]),
                        });
                    }
                  

                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = GetBIMonthlyReportExcel(lstPV,Year,Month,GodownId);
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
              //  Result.Close();
                Result.Dispose();

            }
        }
        private string GetBIMonthlyReportExcel(List<Hdb_BIMonthlyReport> ObjAbsR,int Year,int Month,int GodownId)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION";
                string typeOfValue = "";
                string MonthPrint = "";
                string PreMonthPrint = "";
                string Godown = "";
                string MonthYearPrint = "";
                string PrevMonthYearPrint = "";
                string PrevMonthYearrPrint = "";
                string Yearr = Convert.ToString(Year);
                var PrevYearr = Year - 1;
                var subyear = Yearr.Substring(Yearr.Length - 2);
                //  var Monthh=
                switch (Month)
                {
                    case 1:
                        MonthPrint = "January";
                        PreMonthPrint = "December";
                        MonthYearPrint = "`JAN"+'-'+subyear+"";
                        PrevMonthYearPrint = "`DEC"+'-'+PrevYearr+"";
                        PrevMonthYearrPrint = "December" + '-' + PrevYearr + "";
                        break;
                    case 2:
                        MonthPrint = "February";
                        PreMonthPrint = "January";
                        MonthYearPrint = "`FEB"+'-'+ Yearr + "";
                        PrevMonthYearPrint = "`JAN"+ '-' + Yearr + "";
                        PrevMonthYearrPrint = "January" + '-' + Yearr + "";
                        break;
                    case 3:
                        MonthPrint = "March";
                        PreMonthPrint = "February";
                        MonthYearPrint = "`MAR" + '-' + Yearr + "";
                        PrevMonthYearPrint = "`FEB" + '-' + Yearr + "";
                        PrevMonthYearrPrint = "February" + '-' + Yearr + "";
                        break;
                    case 4:
                        MonthPrint = "April";
                        PreMonthPrint = "March";
                        MonthYearPrint = "`APR" + '-' + Yearr + "";
                        PrevMonthYearPrint = "`MAR" + '-' + Yearr + "";
                        PrevMonthYearrPrint = "March" + '-' + Yearr + "";
                        break;
                    case 5:
                        MonthPrint = "MAY";
                        PreMonthPrint = "April";
                        MonthYearPrint = "`MAY" + '-' + Yearr + "";
                        PrevMonthYearPrint = "`APR" + '-' + Yearr + "";
                        PrevMonthYearrPrint = "April" + '-' + Yearr + "";
                        break;
                    case 6:
                        MonthPrint = "JUNE";
                        PreMonthPrint = "MAY";
                        MonthYearPrint = "`JUN" + '-' + Yearr + "";
                        PrevMonthYearPrint = "`MAY" + '-' + Yearr + "";
                        PrevMonthYearrPrint = "May" + '-' + Yearr + "";
                        break;
                    case 7:
                        MonthPrint = "JULY";
                        PreMonthPrint = "JUNE";
                        MonthYearPrint = "`JUL" + '-' + Yearr + "";
                        PrevMonthYearPrint = "`JUN" + '-' + Yearr + "";
                        PrevMonthYearrPrint = "June" + '-' + Yearr + "";
                        break;
                    case 8:
                        MonthPrint = "AUGUST";
                        PreMonthPrint = "JULY";
                        MonthYearPrint = "`AUG" + '-' + Yearr + "";
                        PrevMonthYearPrint = "`JUL" + '-' + Yearr + "";
                        PrevMonthYearrPrint = "July" + '-' + Yearr + "";
                        break;
                    case 9:
                        MonthPrint = "SEPTEMBER";
                        PreMonthPrint = "AUGUST";
                        MonthYearPrint = "`SEP" + '-' + Yearr + "";
                        PrevMonthYearPrint = "`AUG" + '-' + Yearr + "";
                        PrevMonthYearrPrint = "August" + '-' + Yearr + "";
                        break;
                    case 10:
                        MonthPrint = "OCTOBER";
                        PreMonthPrint = "SEPTEMBER";
                        MonthYearPrint = "`OCT" + '-' + Yearr + "";
                        PrevMonthYearPrint = "`SEP" + '-' + Yearr + "";
                        PrevMonthYearrPrint = "September" + '-' + Yearr + "";
                        break;
                    case 11:
                        MonthPrint = "NOVEMBER";
                        PreMonthPrint = "OCTOBER";
                        MonthYearPrint = "`NOV" + '-' + Yearr + "";
                        PrevMonthYearPrint = "`OCT" + '-' + Yearr + "";
                        PrevMonthYearrPrint = "October" + '-' + Yearr + "";
                        break;
                    case 12:
                        MonthPrint = "DECEMBER";
                        PreMonthPrint = "NOVEMBER";
                        MonthYearPrint = "`DEC" + '-' + Yearr + "";
                        PrevMonthYearPrint = "`NOV" + '-' + Yearr + "";
                        PrevMonthYearrPrint = "November" + '-' + Yearr + "";
                        break;
                }
                switch (GodownId)
                {
                    case 1:
                        Godown = "GODOWN 6";
                        break;
                    case 2:
                        Godown = "GODOWN 3";
                        break;
                    case 3:
                        Godown = "GODOWN 2A";
                        break;
                    case 4:
                        Godown = "GODOWN 2B";
                        break;
                    case 5:
                        Godown = "GODOWN 5";
                        break;
                    case 6:
                        Godown = "GODOWN 4";
                        
                        break;
                    case 7:
                        Godown = "GODOWN 1";
                        break;
                    case 8:
                        Godown = "GODOWN 7";
                        break;
                    
                }
                typeOfValue = "" + MonthPrint + "  " + Year+"";
               var  PreviousMonth = "" + PreMonthPrint+"  " ;
                var PreviousYear = + Year - 1 + "";
                string PreMonth = ""+PreviousMonth + "-" + PreviousYear +"" ;
                exl.MargeCell("A1:R1", title, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A2:R2", "(A Govt.of India Undertaking)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A3:R3", "CFS, Kukatpally, Hyderabad", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A4:R4", "Bi-monthly statement for the month of "+typeOfValue, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                // exl.MargeCell("A5:N5", "Type of Workslip: "+ WorkslipType, DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A5:R5", "GodownNumber:" + Godown, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A6:R6", "Accrued Income Statement for the Stocks Received During for the month of ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A7:R7", PrevMonthYearrPrint + " and Delivered during the month of " + typeOfValue, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("J7:N7", "Handling Details", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("A8:A8", "S.N.",  DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B8:B8", "Name of the Importer", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C8:C8", "Date of destuffing", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D8:D8", "Invoice Number", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E8:E8", "Invoice Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F8:F8", "BE Number", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G8:G8", "BE Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H8:H8", "BL Number", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I8:I8", "No Pkgs", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J8:J8", "Value in Rs.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K8:K8", "Duty in Rs.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("L8:L8", "Value+Duty in Rs.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("M8:M8", "LCL/FCL", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("N8:N8", "Godown Number", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("O8:O8", "Date of Delivery", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("P8:P8", PrevMonthYearPrint, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("Q8:Q8",MonthYearPrint, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("R8:R8", "Total Taxable Amount", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("N7:N7", , DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                // exl.AddTable<HDB_WorkSlippDetails>("A", 8, lstPV, new[] { 6, 20, 20, 20, 20, 20, 10, 15, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 });
                //var NoOfUnits = lstPV.Sum(o => o.NoOfUnits);
                //var NoOfUnitsRec = lstPV.Sum(o => o.NoOfUnitsRec);
                //var Weight = lstPV.Sum(o => o.Weight);
                //var Area = lstPV.Sum(o => o.Area);

                //var CBM = lstPV.Sum(o => o.CBM);

                //var CIF = lstPV.Sum(o => o.CIF);

                //exl.MargeCell("A" + (lstPV.Count + 8).ToString() + ":J" + (lstPV.Count + 8).ToString() + "", "TOTAL", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("K" + (lstPV.Count + 8).ToString(), NoOfUnits.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("L" + (lstPV.Count + 8).ToString(), NoOfUnitsRec.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("M" + (lstPV.Count + 8).ToString(), Weight.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                var PreviousAmount = ObjAbsR.Sum(o => o.PreviousAmount);
                var CurrentAmount = ObjAbsR.Sum(o => o.CurrentAmount);
                var Taxable = ObjAbsR.Sum(o => o.TaxableAmount);
                // var Area = lstPV.Sum(o => o.Area);

                // var CBM = lstPV.Sum(o => o.CBM);

                // var CIF = lstPV.Sum(o => o.CIF);


             //   exl.AddCell("O" + "", "TOTAL AMOUNT in Rs.", DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("P" + (ObjAbsR.Count + 9).ToString(),PreviousAmount.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("Q" + (ObjAbsR.Count + 9).ToString(), CurrentAmount.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("R" + (ObjAbsR.Count + 9).ToString(), Taxable.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);


                //exl.AddTable<chn_test>("A", 9, DDetail, new[] { 6, 20, 20, 20, 12});
               // exl.AddTable<Hdb_BIMonthlyReport>("F", 9,ObjAbsR , new[] { 20,20, 10, 15, 20, 12, 12, 8, 14, 20, 10,20,20 });
                 exl.AddTable<Hdb_BIMonthlyReport>("A", 9, ObjAbsR, new[] { 6, 20, 20, 20, 20, 20, 10, 15, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 });



                //ellpos3 = "K" + (lstPV.Count + 15).ToString() + ":" + "M" + (lstPV.Count + 15).ToString();
                //   string cellpos4 = "O" + (lstPV.Count + 15).ToString() + ":" + "Q" + (lstPV.Count + 15).ToString();
                //   exl.MargeCell(cellpos1, "Name & Sign. of representative of HTC", DynamicExcel.CellAlignment.Middle);
                //   exl.MargeCell(cellpos2, "Name & Sign. of Shed I/C", DynamicExcel.CellAlignment.Middle);
                //   exl.MargeCell(cellpos3, "Name &Sign. Of Export I/C", DynamicExcel.CellAlignment.Middle);
                //   exl.MargeCell(cellpos4, "Sign. Of Manager-CFS", DynamicExcel.CellAlignment.Middle);
                exl.Save();
            }
            return excelFile;
        }
        #endregion
        #region  LoadContRequestDTR
        public void GetLoadContReqDtrReport(DateTime date1, DateTime date2)
        {

            /* DateTime dtfrom = DateTime.ParseExact(ObjWorkSlipReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
             String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
             DateTime dtTo = DateTime.ParseExact(ObjWorkSlipReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
             String PeriodTo = dtTo.ToString("yyyy/MM/dd");*/
           // DateTime dtfrom = DateTime.ParseExact(date1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
           /// String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
          ////  DateTime dtTo = DateTime.ParseExact(date2, "dd/MM/yyyy", CultureInfo.InvariantCulture);
          //  String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = date2 });

            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("DtrExportLoadedContainers", CommandType.StoredProcedure, DParam);


            DataTable dt = Result.Tables[0];
            _DBResponse = new DatabaseResponse();
            int srno = 0;

            List<DailyTransactionExpLodRpt> lstPV = new List<DailyTransactionExpLodRpt>();
            try
            {

                if (Result.Tables.Count > 0)
                {
                    // srno = srno + 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        srno = srno + 1;
                        lstPV.Add(new DailyTransactionExpLodRpt
                        {


                            SerialNo = srno,
                            //InvoiceId = Convert.ToInt32(dr["InvoiceId"].ToString()),
                            GateInDate = dr["entrydate"].ToString(),
                            GateInTime = dr["entrytime"].ToString(),
                            RRDate = dr["RRDate"].ToString(),
                            GateOutDate = dr["GateExitDate"].ToString(),
                            GateOutTime = dr["GateExitTime"].ToString(),
                            Days = Convert.ToInt32(dr["noofdaysatcfs"]),
                            Exporter = dr["exporter"].ToString(),
                            CHA = dr["cha"].ToString(),
                            CustodianCode = dr["custodiancode"].ToString(),
                            VehicleNo = dr["vehicleno"].ToString(),
                            ContainerNo = dr["containerno"].ToString(),
                            ShippingBillNo = dr["shippingbillno"].ToString(),
                            ShippingBillDate = dr["shippingbilldate"].ToString(),
                            LEODate = dr["LEODate"].ToString(),
                            ShippingLineName = dr["shippinglinename"].ToString(),
                            NoOFPkg = Convert.ToInt32(dr["NoOfUnits"]),
                            GrossWeight= Convert.ToDecimal(dr["GrossWeight"]),
                            POC = dr["portorigincode"].ToString(),
                            PortName = dr["portname"].ToString(),
                            POD = dr["portofdischarge"].ToString(),
                            ExportUnder= dr["ExportUnder"].ToString(),
                            Size= Convert.ToInt32(dr["size"]),
                            Type= dr["type"].ToString(),
                            FOB = Convert.ToDecimal(dr["FobValue"]),
                            InvoiceAmt = Convert.ToDecimal(dr["InvoiceAmt"]),


                        });
                    }

                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = GetDTRLoadedExportExcel(lstPV, date1.ToString("dd/MM/yyyy"), date2.ToString("dd/MM/yyyy"));
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

        private string GetDTRLoadedExportExcel(List<DailyTransactionExpLodRpt> lstPV, string PeriodFrom, string PeriodTo)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION";
                string typeOfValue = "";

                typeOfValue = "From " + PeriodFrom + " To " + PeriodTo;
                exl.MargeCell("A1:M1", title, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A2:M2", "(A Govt.of India Undertaking)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A3:M3", "CFS, Kukatpally, Hyderabad", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A4:M4", "Daily Transaction Report(EXPORT LOADED CONTAINERS(FCL))", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                // exl.MargeCell("A5:N5", "Type of Workslip: "+ WorkslipType, DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A5:M5", "" + typeOfValue, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //exl.MargeCell("A6:N6", "Type of Workslip: " + WorkSlipType, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //   exl.MargeCell("A7:I7", "Destuffing ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                //  exl.MargeCell("J7:N7", "Handling Details", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                        
                exl.MargeCell("A7:A7", "Sl. No.", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("B7:B7", "Gate In Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("C7:C7", "Gate In Time", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D7:D7", "RR Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("E7:E7", "Gate Out Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("F7:F7", "Gate Out Time", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("G7:G7", "No Of Days at CFS", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("H7:H7", "EXPORTER", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("I7:I7", "CHA", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("J7:J7", "Custodian Code", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("K7:K7", "Road Transport Vehicle No", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("L7:L7", "Container Number", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("M7:M7", "Shipping Bill No", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("N7:N7", "Shipping Bill Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("O7:O7", "LEO Date", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("P7:P7", "Shipping Line Name", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("Q7:Q7", "No OF Packages", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("R7:R7", "Gross Wt", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("S7:S7", "Port Origin Code", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("T7:T7", "PORT NAME", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("U7:U7", "Port Of Discharge Code", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("V7:V7", "LUT/GST/SEZ", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("W7:W7", "Container Size", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("X7:X7", "Type", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("Y7:Y7", "FOB Value", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("Z7:Z7", "Invoice amount", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
              //  exl.MargeCell("Y7:Y7", "", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                // exl.MargeCell("N7:N7", "Remarks", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.AddTable<DailyTransactionExpLodRpt>("A", 8, lstPV, new[] { 6, 20, 20, 20, 20, 20, 10, 15, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,20,20,20,20,20,20,20 });
                //var NoOfUnits = lstPV.Sum(o => o.NoOfUnits);
                //var NoOfUnitsRec = lstPV.Sum(o => o.NoOfUnitsRec);
                //var Weight = lstPV.Sum(o => o.Weight);
                //var Area = lstPV.Sum(o => o.Area);

                //var CBM = lstPV.Sum(o => o.CBM);

                //var CIF = lstPV.Sum(o => o.CIF);

                //exl.MargeCell("A" + (lstPV.Count + 8).ToString() + ":J" + (lstPV.Count + 8).ToString() + "", "TOTAL", DynamicExcel.CellAlignment.BottomLeft, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("K" + (lstPV.Count + 8).ToString(), NoOfUnits.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("L" + (lstPV.Count + 8).ToString(), NoOfUnitsRec.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("M" + (lstPV.Count + 8).ToString(), Weight.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("O" + (lstPV.Count + 8).ToString(), Area.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("P" + (lstPV.Count + 8).ToString(), CBM.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //exl.AddCell("Q" + (lstPV.Count + 8).ToString(), CIF.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                var TotalGrossWeight = lstPV.Sum(o => o.GrossWeight);
                var TotalInvoiceAmount = lstPV.Sum(o => o.InvoiceAmt);
                //   var Taxable = ObjAbsR.Sum(o => o.TaxableAmount);
                // var Area = lstPV.Sum(o => o.Area);

                // var CBM = lstPV.Sum(o => o.CBM);

                // var CIF = lstPV.Sum(o => o.CIF);


                 exl.AddCell("O" +(lstPV.Count + 8).ToString(), "TOTAL ", DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("R" + (lstPV.Count + 8).ToString(), TotalGrossWeight.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("Z" + (lstPV.Count + 8).ToString(), TotalInvoiceAmount.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
              //  exl.AddCell("R" + (ObjAbsR.Count + 8).ToString(), Taxable.ToString(), DynamicExcel.CellAlignment.CenterRight, DynamicExcel.CellFontStyle.Bold);
                //  string cellpos1 = "P" + (lstPV.Count + 15).ToString() ;
                //  string cellpos2 = "X" + (lstPV.Count + 15).ToString() ;
                //  string cellpos3 = "I" + (lstPV.Count + 15).ToString() + ":" + "M" + (lstPV.Count + 15).ToString();
                //  string cellpos4 = "L" + (lstPV.Count + 15).ToString() + ":" + "Q" + (lstPV.Count + 15).ToString();
                //   exl.MargeCell(cellpos1, "Name & Sign. of representative of HTC", DynamicExcel.CellAlignment.Middle);
                //   exl.MargeCell(cellpos2, "Name & Sign. of Shed I/C", DynamicExcel.CellAlignment.Middle);
                //   exl.MargeCell(cellpos3, "Name &Sign. Of Export I/C", DynamicExcel.CellAlignment.Middle);
                //  exl.MargeCell(cellpos4, "Sign. Of Manager-CFS", DynamicExcel.CellAlignment.Middle);
                exl.Save();
            }
            return excelFile;
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
            List<Hdb_ContStufAckSearch> LstStuffing = new List<Hdb_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Hdb_ContStufAckSearch
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
            List<Hdb_ContStufAckSearch> LstStuff = new List<Hdb_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuff.Add(new Hdb_ContStufAckSearch
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
        public void  GetStufAckResult(string container, string shipbill, string cfscode)
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
            List<Hdb_ContStufAckRes> Lststufack = new List<Hdb_ContStufAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Hdb_ContStufAckRes
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
            List<Hdb_ContStufAckSearch> LstStuff = new List<Hdb_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuff.Add(new Hdb_ContStufAckSearch
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
        public void GetAllShippingBillNoForASRACK(string in_ShippingBillNo, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingBillNo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = in_ShippingBillNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetshippingbillNoForASRAckStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Hdb_ContStufAckSearch> LstStuff = new List<Hdb_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuff.Add(new Hdb_ContStufAckSearch
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
        public void GetASRAckResult(string shipbill, string CFSCode,string container)
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
            List<Hdb_ContStufAckRes> Lststufack = new List<Hdb_ContStufAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Hdb_ContStufAckRes
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
            List<Hdb_GatePassDPAckSearch> lstDPGPAck = new List<Hdb_GatePassDPAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDPGPAck.Add(new Hdb_GatePassDPAckSearch
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

            List<Hdb_ContDPAckSearch> lstDPContACK = new List<Hdb_ContDPAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDPContACK.Add(new Hdb_ContDPAckSearch
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
            List<Hdb_DPAckRes> Lststufack = new List<Hdb_DPAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Hdb_DPAckRes
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
            List<Hdb_GatePassDTAckSearch> lstDTGPAck = new List<Hdb_GatePassDTAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDTGPAck.Add(new Hdb_GatePassDTAckSearch
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
            
            List<Hdb_ContDTAckSearch> lstDTContACK = new List<Hdb_ContDTAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDTContACK.Add(new Hdb_ContDTAckSearch
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
            List<Hdb_DTAckRes> Lststufack = new List<Hdb_DTAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Hdb_DTAckRes
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

        #region Export Carting Register Report in Excel

        public void GetExportCartingReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetExportCartingReportExcel", CommandType.StoredProcedure, DParam);
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
        #region Stuffing Report
        public void StuffingReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("StuffingReport", CommandType.StoredProcedure, DParam);
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

        #region Tally response

        public void GetTallyResponse(TallyResponse vm)
        {

            DateTime dtfrom = DateTime.ParseExact(vm.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(vm.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("TallyWebApiResponse", CommandType.StoredProcedure, DParam);
            IList<TallyResponse> lstData = new List<TallyResponse>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstData.Add(new TallyResponse
                    {
                        Date = Result["Date"].ToString(),
                        Bill = Result["Bill"].ToString(),
                        Invoice = Result["Invoice"].ToString(),
                        Dr = Result["Dr"].ToString(),
                        Cr = Result["Cr"].ToString(),
                        Receipt = Result["Receipt"].ToString()

                    });
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



        #region Accrued Income for Import Cargo
        public void GetImpCargoInExcel(string AsOnDate, string GodownId)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(GodownId) });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpCargoReportHeaderInExcel", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            List<Hdb_ImpCargoInGodownInExcel> lstPV = new List<Hdb_ImpCargoInGodownInExcel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Hdb_ImpCargoInGodownInExcel
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        DOR = Result["DestuffingEntryDate"].ToString(),
                        DestuffingEntryDate = Result["DestuffingEntryDate"].ToString(),
                        EntryDate = Convert.ToDateTime(Result["EntryDate"].ToString()),
                        LCLFCL = Result["LCLFCL"].ToString(),
                        Size = Result["Size"].ToString(),
                        StorageType = Result["StorageType"].ToString(),
                        Area = Convert.ToDecimal(Result["Area"].ToString()),
                        CargoType = Convert.ToInt32(Result["CargoType"].ToString()),


                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ImpCargoInExcel(lstPV, AsOnDate);
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

        private string ImpCargoInExcel(List<Hdb_ImpCargoInGodownInExcel> model, string date1)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "(A Government of India Undertaking)"
                      ;

                var Addr = "CFS, Kukatpally, Hyderabad";
                var Titl1 = "Accrued Income Statement for The Month of FEB-2022";
                var Godown = "Godown No. ";
                exl.MargeCell("A1:Y1", title, DynamicExcel.CellAlignment.TopCenter);
                exl.MargeCell("A2:Y2", Addr, DynamicExcel.CellAlignment.TopCenter);
                exl.MargeCell("A3:Y3", Titl1, DynamicExcel.CellAlignment.TopCenter);
             //   exl.MargeCell("A4:C4", Godown, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A5:A5", "Sl No", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B5:B5", "CFS-CODE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C5:C5", "DOR", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D5:D5", "GRIDS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E5:E5", "06/01/2013", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F5:F5", "NO OF DAYS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G5:G5", "RATE@413", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H5:H5", "4/01/2016", DynamicExcel.CellAlignment.Middle);
                                   
                exl.MargeCell("I5:I5", "", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J5:J5", "RATE@456", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K5:K5", "14/02/2017", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L5:L5", "", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M5:M5", "RATE@500", DynamicExcel.CellAlignment.Middle);
                                  
                exl.MargeCell("N5:N5", "10/01/2018", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O5:O5", "", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("P5:P5", "RATE@550", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Q5:Q5", "11/2/2020", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("R5:R5", "", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("S5:S5", "RATE@625", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("T5:T5", "28/02/2022", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("U5:U5", "", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("V5:V5", "RATE@625", DynamicExcel.CellAlignment.Middle);
             //   exl.MargeCell("W5:W5", "", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("W5:W5", "Total upto  Feb 2022", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("X5:X5", "1-02-2022 to 28-02-2022", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Y5:Y5", "AMOUNT for 1-02-2022 to 28-02-2022", DynamicExcel.CellAlignment.Middle);


                List<Hdb_ImpCargoInExcel> lstHdb_ImpCargoInExcel = new List<Hdb_ImpCargoInExcel>();
                int j = 1;
                foreach(var i in model)
                {
                    lstHdb_ImpCargoInExcel.Add(new Hdb_ImpCargoInExcel
                    {
                        SLNo = j,
                        CFSCode = i.CFSCode,
                        DOR = i.DOR,
                        GRIDS = 1,
                        FirstEffective = "1/6/2013",
                        FirstNoOfDays = (Convert.ToInt32((Convert.ToDateTime("2013-06-01") - Convert.ToDateTime(i.EntryDate)).TotalDays) + 1) <= 0 ? 0 : (Convert.ToInt32((Convert.ToDateTime("2013-06-01") - Convert.ToDateTime(i.EntryDate)).TotalDays) + 1),
                        SecondEffective = "1/4/2016",
                        SecondNoOfDays = (Convert.ToInt32((Convert.ToDateTime("2016-04-01") - Convert.ToDateTime("2013-06-01")).TotalDays) + 1) <= 0 ? 0 : (Convert.ToInt32((Convert.ToDateTime("2016-04-01") - Convert.ToDateTime("2013-06-01")).TotalDays) + 1),
                        ThirdEffective = "14/02/2017",
                        ThirdNoOfDays = (Convert.ToInt32((Convert.ToDateTime("2017/02/14") - Convert.ToDateTime("2016-04-01")).TotalDays) + 1) <= 0 ? 0 : (Convert.ToInt32((Convert.ToDateTime("2017/02/14") - Convert.ToDateTime("2016-04-01")).TotalDays) + 1),
                        FourthEffective = "1/10/2018",
                        FourthNoOfDays = (Convert.ToInt32((Convert.ToDateTime("2018/10/01") - Convert.ToDateTime("2017/02/14")).TotalDays) + 1) <= 0 ? 0 : (Convert.ToInt32((Convert.ToDateTime("2018/10/01") - Convert.ToDateTime("2017/02/14")).TotalDays) + 1),
                        FifthEffective = "2/11/2020",
                        FifthNoOfDays = (Convert.ToInt32((Convert.ToDateTime("2020/11/02") - Convert.ToDateTime("2018/10/01")).TotalDays) + 1) <= 0 ? 0 : (Convert.ToInt32((Convert.ToDateTime("2020/11/02") - Convert.ToDateTime("2018/10/01")).TotalDays) + 1),
                        SixEffective = "28/02/2022",
                        SixNoOfDays = (Convert.ToInt32((Convert.ToDateTime("2022/02/28") - Convert.ToDateTime("2020/11/02")).TotalDays) + 1) <= 0 ? 0 : (Convert.ToInt32((Convert.ToDateTime("2022/02/28") - Convert.ToDateTime("2020/11/02")).TotalDays) + 1),

                        FirstRate = Convert.ToString(GetCalculateStorageCharge(Convert.ToDateTime("2013-06-01"), (Convert.ToInt32((Convert.ToDateTime("2013-06-01") - Convert.ToDateTime(i.EntryDate)).TotalDays) + 1) <= 0 ? 0 : (Convert.ToInt32((Convert.ToDateTime("2013-06-01") - Convert.ToDateTime(i.EntryDate)).TotalDays) + 1), i.LCLFCL, i.StorageType, i.CargoType, i.Size, i.CFSCode, Convert.ToDecimal(i.Area))),
                        SecondRate = Convert.ToString(GetCalculateStorageCharge(Convert.ToDateTime("2016-04-01"), (Convert.ToInt32((Convert.ToDateTime("2016-04-01") - Convert.ToDateTime("2013-06-01")).TotalDays) + 1) <= 0 ? 0 : (Convert.ToInt32((Convert.ToDateTime("2016-04-01") - Convert.ToDateTime("2013-06-01")).TotalDays) + 1), i.LCLFCL, i.StorageType, i.CargoType, i.Size, i.CFSCode, Convert.ToDecimal(i.Area))),
                        ThirdRate = Convert.ToString(GetCalculateStorageCharge(Convert.ToDateTime("2017/02/14"), (Convert.ToInt32((Convert.ToDateTime("2017/02/14") - Convert.ToDateTime("2016-04-01")).TotalDays) + 1) <= 0 ? 0 : (Convert.ToInt32((Convert.ToDateTime("2017/02/14") - Convert.ToDateTime("2016-04-01")).TotalDays) + 1), i.LCLFCL, i.StorageType, i.CargoType, i.Size, i.CFSCode, Convert.ToDecimal(i.Area))),
                        FourthRate = Convert.ToString(GetCalculateStorageCalCharge(Convert.ToDateTime("2018/10/01"), (Convert.ToInt32((Convert.ToDateTime("2018/10/01") - Convert.ToDateTime("2017/02/14")).TotalDays) + 1) <= 0 ? 0 : (Convert.ToInt32((Convert.ToDateTime("2018/10/01") - Convert.ToDateTime("2017/02/14")).TotalDays) + 1), i.LCLFCL, i.StorageType, i.CargoType, i.Size, i.CFSCode, Convert.ToDecimal(i.Area))),
                       // //  FourthRate = Convert.ToString(GetCalculateStorageCharge(Convert.ToDateTime("2018/10/01"), (Convert.ToInt32((Convert.ToDateTime("2018/10/01") - Convert.ToDateTime("2017/02/14")).TotalDays) + 1) <= 0 ? 0 : (Convert.ToInt32((Convert.ToDateTime("2018/10/01") - Convert.ToDateTime("2017/02/14")).TotalDays) + 1), i.LCLFCL, i.StorageType, i.CargoType, i.Size, i.CFSCode, Convert.ToDecimal(i.Area))),
                          FifthRate = Convert.ToString(GetCalculateStorageCalCharge(Convert.ToDateTime("2020/11/02"), (Convert.ToInt32((Convert.ToDateTime("2020/11/02") - Convert.ToDateTime("2018/10/01")).TotalDays) + 1) <= 0 ? 0 : (Convert.ToInt32((Convert.ToDateTime("2020/11/02") - Convert.ToDateTime("2018/10/01")).TotalDays) + 1), i.LCLFCL, i.StorageType, i.CargoType, i.Size, i.CFSCode, Convert.ToDecimal(i.Area))),
                         SixRate = Convert.ToString(GetCalculateStorageCalCharge(Convert.ToDateTime("2020/11/02"), (Convert.ToInt32((Convert.ToDateTime("2022/02/28") - Convert.ToDateTime("2020/11/02")).TotalDays) + 1) <= 0 ? 0 : (Convert.ToInt32((Convert.ToDateTime("2022/02/28") - Convert.ToDateTime("2020/11/02")).TotalDays) + 1), i.LCLFCL, i.StorageType, i.CargoType, i.Size, i.CFSCode, Convert.ToDecimal(i.Area))),
                        // TotalUpto = FirstRate + SecondRate + ThirdRate + FourthRate + FifthRate + SixRate,
                         DateDiff="28",
                         UptoAmount=Convert.ToDecimal(GetCalculateStorageCalCharge(Convert.ToDateTime("2020/11/02"), (Convert.ToInt32((Convert.ToDateTime("2022/02/28") - Convert.ToDateTime("2020/11/02")).TotalDays) + 1) <= 0 ? 0 : (Convert.ToInt32((Convert.ToDateTime("2022/02/28") - Convert.ToDateTime("2020/11/02")).TotalDays) + 1), i.LCLFCL, i.StorageType, i.CargoType, i.Size, i.CFSCode, Convert.ToDecimal(i.Area)))





                    });
                    j++;
                }


                foreach( var i in lstHdb_ImpCargoInExcel)
                {
                    i.TotalUpto =Convert.ToDecimal(i.FirstRate) + Convert.ToDecimal(i.SecondRate) + Convert.ToDecimal(i.ThirdRate) + Convert.ToDecimal(i.FourthRate) + Convert.ToDecimal(i.FifthRate) + Convert.ToDecimal(i.SixRate);
                }
            




                exl.AddTable<Hdb_ImpCargoInExcel>("A", 6, lstHdb_ImpCargoInExcel, new[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10,10,10,10,10 });
               

                exl.Save();
            }
            return excelFile;
        }


        public decimal GetCalculateStorageCharge(DateTime EffectiveDate, int Days, string FCLLCL,string StorageType,int CargoType,string Size,string CfsCode,decimal Area)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(EffectiveDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Days", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(Days) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(FCLLCL) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StorageType", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(StorageType) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(CargoType) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(Size) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CfsCode", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(CfsCode) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Area", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(Area) });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("CalculateStorageChargeImpStock", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
            int Status = 0;
            decimal Value = 0M;
           
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Value = Convert.ToDecimal(Result["Val2"].ToString());
                }
                return Value;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
               

            }

            
        }

        public decimal GetCalculateStorageCalCharge(DateTime EffectiveDate, int Days, string FCLLCL, string StorageType, int CargoType, string Size, string CfsCode, decimal Area)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EffectiveDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(EffectiveDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Days", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(Days) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(FCLLCL) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StorageType", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(StorageType) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(CargoType) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(Size) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CfsCode", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(CfsCode) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Area", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(Area) });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("CalculateStorageChargeCalImpStock", CommandType.StoredProcedure, DParam);
            //    _DBResponse = new DatabaseResponse();
            int Status = 0;
            decimal Value = 0M;

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Value = Convert.ToDecimal(Result["Val2"].ToString());
                }
                return Value;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                Result.Close();
                Result.Dispose();


            }


        }
        #endregion

        #region Add edit QR Code
        public void AddEditBQRCode(int InvoiceId, string FileName, int CreatedBy)
        {
            string Status = "0";
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FileName", MySqlDbType = MySqlDbType.VarChar, Value = FileName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.VarChar, Value = CreatedBy });
            LstParam.Add(new MySqlParameter { ParameterName = "@RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
            DParam = LstParam.ToArray();
            int Result = DataAccess.ExecuteNonQuery("AddEdithdfcbqrcode", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Saved Successfully.";
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

        #region Accounts Report for Bond Cargo
        public void GetBondCargo(string AsOnDate, string GodownId)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(GodownId) });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondCargoReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Hdb_BondCargo> lstPV = new List<Hdb_BondCargo>();
            try
            {
                
               
                    while (Result.Read())
                    {
                        Status = 1;
                        lstPV.Add(new Hdb_BondCargo
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
                            CHA = Result["CHA"].ToString(),
                            Location = Result["LocationName"].ToString(),
                            Remarks = Result["Remarks"].ToString(),
                            StorageAmount= Convert.ToDecimal(Result["StorageAmount"]),
                            InsuranceAmount = Convert.ToDecimal(Result["InsuranceAmount"]),
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

        #region ContainerSBWiseCharges
        public void GetContainerShipbillwiseChargesForPrint(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceId });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippbillwiseChargesForExp", CommandType.StoredProcedure, DParam);
            Hdb_ShipbillWiseStorageCharge ObjStuffing = new Hdb_ShipbillWiseStorageCharge();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjStuffing.StuffingNo= Convert.ToString(Result["StuffingNo"]);
                    ObjStuffing.StuffingDate = Convert.ToString(Result["StuffingDate"]);
                    ObjStuffing.Size = Convert.ToString(Result["Size"]);
                    ObjStuffing.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                    ObjStuffing.Forwarder = Convert.ToString(Result["Forwarder"]);
                }
                if (Result.NextResult())
                {
                    
                    while (Result.Read())
                    {
                        ObjStuffing.lstChargeDetails.Add(new Hdb_ShipbillWiseStorageChargeDetails
                        {
                            CargoDescription = Convert.ToString(Result["CargoDescription"]),
                            CartingRegisterNo = Convert.ToString(Result["CartingRegisterNo"]),
                            Exporter = Convert.ToString(Result["Exporter"]),
                            ExStorage = Convert.ToDecimal(Result["ExStorage"]),
                            Fob = Convert.ToDecimal(Result["Fob"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            Insurance = Convert.ToDecimal(Result["Insurance"]),
                            RegisterDate = Convert.ToString(Result["RegisterDate"]),
                            SBDate = Convert.ToString(Result["SBDate"]),
                            SBNo = Convert.ToString(Result["SBNo"]),
                            Storage = Convert.ToDecimal(Result["Storage"])
                        });
                    }
                   
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.FranchiseCharge.Carting = Convert.ToDecimal(Result["Carting"]);
                        ObjStuffing.FranchiseCharge.FranchaieseCharge = Convert.ToDecimal(Result["FranchaieseCharge"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjStuffing;
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

        
        #region Accrued Income for Bond Cargo
        public void GetBondCargoInExcel(string AsOnDate, string GodownId, string GodownName)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(GodownId) });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondCargoReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            List<Hdb_BondCargo> lstPV = new List<Hdb_BondCargo>();
            List<Hdb_BondCargoExcel> lstPVExcel = new List<Hdb_BondCargoExcel>();
            try
            {                
                int srno = 0;
                while (Result.Read())
                {
                    Status = 1;                    
                    lstPV.Add(new Hdb_BondCargo
                    {                        
                        BondNo = Result["BondNo"].ToString(),
                        BondDate = Result["BondDate"].ToString(),
                        Importer = Result["Importer"].ToString(),
                        ItemDesc = Result["ItemDesc"].ToString(),
                        Units = Convert.ToInt32(Result["Unit"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        Value = Convert.ToDecimal(Result["Value"]),
                        Duty = Convert.ToDecimal(Result["Duty"]),
                        TotalValueDuty = Convert.ToDecimal(Result["Value"]) + Convert.ToDecimal(Result["Duty"]),
                        StorageAmount = Convert.ToDecimal(Result["StorageAmount"]),
                        InsuranceAmount = Convert.ToDecimal(Result["InsuranceAmount"]),
                        Location = Result["LocationName"].ToString(),
                        Area = Convert.ToDecimal(Result["Area"]),
                        CHA = Result["CHA"].ToString(),                        
                        Remarks = Result["Remarks"].ToString(),                        
                        CompAddress = Result["CompanyAddress"].ToString(),
                        CompEmail = Result["CompanyEmail"].ToString(),
                    });

                    lstPVExcel.Add(new Hdb_BondCargoExcel
                    {
                        SlNo = Convert.ToInt32(Result["SrNo"]),
                        BondNo = Result["BondNo"].ToString(),
                        BondDate = Result["BondDate"].ToString(),
                        Importer = Result["Importer"].ToString(),
                        ItemDesc = Result["ItemDesc"].ToString(),
                        Units = Convert.ToInt32(Result["Unit"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        Value = Convert.ToDecimal(Result["Value"]),
                        Duty = Convert.ToDecimal(Result["Duty"]),
                        TotalValueDuty = Convert.ToDecimal(Result["Value"]) + Convert.ToDecimal(Result["Duty"]),
                        StorageAmount = Convert.ToDecimal(Result["StorageAmount"]),
                        InsuranceAmount = Convert.ToDecimal(Result["InsuranceAmount"]),
                        Location = Result["LocationName"].ToString(),
                        Area = Convert.ToDecimal(Result["Area"]),
                        CHA = Result["CHA"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = BondCargoInExcel(lstPV, lstPVExcel, AsOnDate, GodownName);
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

        private string BondCargoInExcel(List<Hdb_BondCargo> lstPV, List<Hdb_BondCargoExcel> lstPVExcel, string date1, string GodownName)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "(A Government of India Undertaking)"
                      ;

                var Addr = lstPV[0].CompAddress;
                var Titl1 = "Bond Accrued Income Statement As on " + date1 + "";                
                exl.MargeCell("A1:P1", title, DynamicExcel.CellAlignment.TopCenter);
                exl.MargeCell("A2:P2", Addr, DynamicExcel.CellAlignment.TopCenter);
                exl.MargeCell("A3:P3", Titl1, DynamicExcel.CellAlignment.TopCenter);
                exl.MargeCell("A4:P4", "GodownName - " + GodownName, DynamicExcel.CellAlignment.TopCenter);
                exl.MargeCell("A5:A5", "Sl No", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B5:B5", "BOND NO", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C5:C5", "BOND DATE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D5:D5", "NAME OF THE IMPORTER", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E5:E5", "DESCRIPTION OF GOODS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F5:F5", "NO OF PKGS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G5:G5", "WT.IN. KGS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H5:H5", "ACCESIBLE VALUE", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("I5:I5", "DUTY", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J5:J5", "TOTAL", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K5:K5", "Storage Amount", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L5:L5", "Insurance Amount", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M5:M5", "Location", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("N5:N5", "AREA IN SQ. MT", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O5:O5", "CHA", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("P5:P5", "Remarks", DynamicExcel.CellAlignment.Middle);               
               
                exl.AddTable("A", 7, lstPVExcel, new[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 });


                var TotalUnits = lstPVExcel.Sum(o => o.Units);
                var Totalweight = lstPVExcel.Sum(o => o.Weight);
                var TotalValue = lstPVExcel.Sum(o => o.Value);
                var TotalDuty = lstPVExcel.Sum(o => o.Duty);
                var TotalValueDuty = TotalValue + TotalDuty;
                var TotalStorageAmt = lstPVExcel.Sum(o => o.StorageAmount);
                var TotalInsuranceAmt = lstPVExcel.Sum(o => o.InsuranceAmount);
                var TotalArea = lstPVExcel.Sum(o => o.Area);

                exl.AddCell("A" + (lstPVExcel.Count + 7).ToString(), "TOTAL ", DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("F" + (lstPVExcel.Count + 7).ToString(), TotalUnits.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("G" + (lstPVExcel.Count + 7).ToString(), Totalweight.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("H" + (lstPVExcel.Count + 7).ToString(), TotalValue.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("I" + (lstPVExcel.Count + 7).ToString(), TotalDuty.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("J" + (lstPVExcel.Count + 7).ToString(), TotalValueDuty.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("K" + (lstPVExcel.Count + 7).ToString(), TotalStorageAmt.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("L" + (lstPVExcel.Count + 7).ToString(), TotalInsuranceAmt.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);
                exl.AddCell("N" + (lstPVExcel.Count + 7).ToString(), TotalArea.ToString(), DynamicExcel.CellAlignment.CenterLeft, DynamicExcel.CellFontStyle.Bold);

                exl.Save();
            }
            return excelFile;
        }

        #endregion

        #region Bulk Cash receipt UPI 
        public void GetBulkCashreceiptUPI(string FromDate, string ToDate, string ReceiptNo)
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
                Result = DataAccess.ExecuteDataSet("GetBulkCashRecptForPrintUPI", CommandType.StoredProcedure, DParam);
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

        #region Tds Deduction Report       

        public void TdsDeductionExcelRpt(string PeriodFrom, string PeriodTo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodFrom).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodTo).ToString("yyyy/MM/dd") });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetTDSDetailsRO", CommandType.StoredProcedure, DParam);
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


    

