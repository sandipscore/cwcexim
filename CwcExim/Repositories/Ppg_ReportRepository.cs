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
using CwcExim.Areas.Auction.Models;

namespace CwcExim.Repositories
{ 
    public class Ppg_ReportRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }
        public void GatePassReport(Ppg_GatePassReport ObjGatePassReport, int UserId)
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

            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = ObjGatePassReport.Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = ObjGatePassReport.LCLFCL });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Location", MySqlDbType = MySqlDbType.VarChar, Value = ObjGatePassReport.GodownYard });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodName", MySqlDbType = MySqlDbType.VarChar, Value = ObjGatePassReport.LocationName });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GatePassReport", CommandType.StoredProcedure, DParam);
            IList<Ppg_GatePassReport> LstGatePassReport = new List<Ppg_GatePassReport>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstGatePassReport.Add(new Ppg_GatePassReport
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
                        LCLFCL = Result["LCLFCL"].ToString(),
                        BOENoOrSBNoOrWRNo = Result["BOENo"].ToString(),
                        Date = Result["BOEDate"].ToString(),
                        NoOfPackages = Result["NoOfPackages"].ToString(),

                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString(),
                        ICDCode= Result["CFSCode"].ToString(),
                        InDate= Result["InDate"].ToString(),
                        ContainerType= Result["TypeOfContainer"].ToString(),
                        SealCutingType= Result["RMS"].ToString(),
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = invoiceType });
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
                    _DBResponse.Data = JsonConvert.SerializeObject(LstInvoice); ;
                }
                else
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

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.String, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.String, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
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
            PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
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


        public void GetDaysWeeksForIMPYard(int invid, string CFSCode, int fvalue)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_invoiceId", MySqlDbType = MySqlDbType.Int32, Value = invid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_flag", MySqlDbType = MySqlDbType.Int32, Value = fvalue });

            //    lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetDaysWeeksForIMPYard", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            PpgDaysWeeks objDaysWeeks = new PpgDaysWeeks();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objDaysWeeks.Days = Convert.ToInt32(Result["Days"]);
                    //objDaysWeeks.Weeks = Convert.ToInt32(Result["Weeks"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objDaysWeeks;
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




                ObjSDStatement.LstSD.Add(new PPGSDList
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
                List<PpgRegisterOfOutwardSupplyModel> model = new List<PpgRegisterOfOutwardSupplyModel>();
                try
                {
                    if (ds.Tables.Count > 0)
                    {
                        model = (from DataRow dr in dt.Rows
                                 select new PpgRegisterOfOutwardSupplyModel()
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
                List<PpgRegisterOfOutwardSupplyModelCreditDebit> modelCreditDebit = new List<PpgRegisterOfOutwardSupplyModelCreditDebit>();
                try
                {
                    if (ds.Tables.Count > 0)
                    {
                        modelCreditDebit = (from DataRow dr in dt.Rows
                                            select new PpgRegisterOfOutwardSupplyModelCreditDebit()
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
        private string RegisterofOutwardSupplyExcel(List<PpgRegisterOfOutwardSupplyModel> model, decimal InvoiceAmount, decimal CRAmount)
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
                exl.AddTable<PpgRegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 20, 20, 20, 12, 20, 10, 15, 20, 12, 12, 8, 14, 8, 14, 8, 14, 16, 10, 30 });
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

        private string RegisterofOutwardSupplyExcelCreditDebit(List<PpgRegisterOfOutwardSupplyModelCreditDebit> modelCreditDebit, decimal InvoiceAmount, decimal CRAmount)
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


        public void GetBulkCashreceiptForExternalUser(string FromDate, string ToDate, string ReceiptNo,int PartyId)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(FromDate == null ? "" : FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ToDate == null ? "" : ToDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CashReceiptId", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ReceiptNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Size = 30, Value = PartyId });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetBulkCashRecptForPrintForExternalUser", CommandType.StoredProcedure, DParam);
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

        #region DailyCashBook
        public void DailyCashBook(DailyCashBookPpg ObjDailyCashBook)
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
            IDataReader Result = DataAccess.ExecuteDataReader("DailyCashBookReport", CommandType.StoredProcedure, DParam);
            IList<DailyCashBookPpg> LstDailyCashBook = new List<DailyCashBookPpg>();
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

                    LstDailyCashBook.Add(new DailyCashBookPpg
                    {

                        //ReceiptNo, ReceiptDate, Party, ChequeNo, CashReceiptId, GenSpace, sto, Insurance, GroundRentEmpty, GroundRentLoaded, Mf, EntCharge, 
                        //Fum, OtCharge, CGSTAmt, SGSTAmt, IGSTAmt, MISC, MiscExcess, TotalCash, TotalCheque, tdsCol, crTDS
                        /*CRNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),

                        Depositor = Result["Party"].ToString(),*/
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

        public void DailyCashBookCash(DailyCashBookPpg ObjDailyCashBook)
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
            IList<DailyCashBookPpg> LstDailyCashBook = new List<DailyCashBookPpg>();
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

                    LstDailyCashBook.Add(new DailyCashBookPpg
                    {

                        //ReceiptNo, ReceiptDate, Party, ChequeNo, CashReceiptId, GenSpace, sto, Insurance, GroundRentEmpty, GroundRentLoaded, Mf, EntCharge, 
                        //Fum, OtCharge, CGSTAmt, SGSTAmt, IGSTAmt, MISC, MiscExcess, TotalCash, TotalCheque, tdsCol, crTDS
                        /*  CRNo = Result["ReceiptNo"].ToString(),
                          ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),

                          Depositor = Result["Party"].ToString(),
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
                          Cgst = Result["CGSTAmt"].ToString(),
                          Sgst = Result["SGSTAmt"].ToString(),
                          Igst = Result["IGSTAmt"].ToString(),
                          Misc = Result["MISC"].ToString(),
                          MiscExcess = Result["MiscExcess"].ToString(),
                          TotalCash = Result["TotalCash"].ToString(),
                          TotalCheque = Result["TotalCheque"].ToString(),
                          Tds = Result["tdsCol"].ToString(),
                          CrTds = Result["crTDS"].ToString()*/
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
        #endregion
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
            IList<DailyCashBookPpg> LstMonthlyCashBook = new List<DailyCashBookPpg>();
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

                    LstMonthlyCashBook.Add(new DailyCashBookPpg
                    {
                        //CRNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),

                        //Depositor = Result["Party"].ToString(),
                        //ChqNo = Result["ChequeNo"].ToString(),
                        GenSpace = Result["GenSpace"].ToString(),
                        StorageCharge = Result["sto"].ToString(),
                        Insurance = Result["Insurance"].ToString(),
                        GroundRentEmpty = Result["GroundRentEmpty"].ToString(),
                        GroundRentLoaded = Result["GroundRentLoaded"].ToString(),
                        MfCharge = Result["Mf"].ToString(),
                        ThcCharge = Result["THC"].ToString(),
                        RRCharge = Result["RR"].ToString(),
                        FACCharge = Result["FAC"].ToString(),


                        EntryCharge = Result["EntCharge"].ToString(),
                        Fumigation = Result["Fum"].ToString(),
                        OtherCharge = Result["OtCharge"].ToString(),
                        Cgst = Result["CGSTAmt"].ToString(),
                        Sgst = Result["SGSTAmt"].ToString(),
                        Igst = Result["IGSTAmt"].ToString(),
                        Misc = Result["MISC"].ToString(),

                        MiscExcess = Result["MiscExcess"].ToString(),
                        TotalCash = Result["TotalCash"].ToString(),
                        TotalCheque = Result["TotalCheque"].ToString(),
                        TotalOthers = Result["TotalOther"].ToString(),
                        TotalPDA = Result["TotalPDA"].ToString(),
                        Tds = Result["tdsCol"].ToString(),
                        CrTds = Result["crTDS"].ToString(),
                        TFUCharge = Result["TFU"].ToString()

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
        public void DailyCashBooKXls(DailyCashBookPpg ObjDailyCashBook)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyCashBook.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyCashBook.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyCashBookReportXLS", CommandType.StoredProcedure, DParam);
            IList<DailyCashBookPpgXLS> LstDailyCashBook = new List<DailyCashBookPpgXLS>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    
                    LstDailyCashBook.Add(new DailyCashBookPpgXLS
                    {

                        //ReceiptNo, ReceiptDate, Party, ChequeNo, CashReceiptId, GenSpace, sto, Insurance, GroundRentEmpty, GroundRentLoaded, Mf, EntCharge, 
                        //Fum, OtCharge, CGSTAmt, SGSTAmt, IGSTAmt, MISC, MiscExcess, TotalCash, TotalCheque, tdsCol, crTDS
                        /*CRNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),
                        */
                       SLNO= Convert.ToInt32(Result["SLNO"]),
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
                        MiscExcess = Result["MiscExcess"].ToString(),
                        Cgst = Result["CGSTAmt"].ToString(),
                        Sgst = Result["SGSTAmt"].ToString(),
                        Igst = Result["IGSTAmt"].ToString(),

                        
                        TotalCash = Result["TotalCash"].ToString(),
                        TotalCheque = Result["TotalCheque"].ToString(),
                        TotalOthers = Result["TotalOther"].ToString(),
                        TotalPDA = Result["TotalPDA"].ToString(),
                        Tds = Result["tdsCol"].ToString(),
                        CrTds = Result["crTDS"].ToString(),
                        
                        Remarks = Result["Remarks"].ToString(),
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
                    DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                  //  decimal InvoiceAmount = 0, CRAmount = 0;

                    _DBResponse.Data = DailyCashBookWithSdDetailXls(LstDailyCashBook, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"));

                }

                //if (Status == 1)
                //{
                //    _DBResponse.Status = 1;
                //    _DBResponse.Message = "Success";
                //    _DBResponse.Data = LstDailyCashBook;
                //}
                //else
                //{
                //    _DBResponse.Status = 0;
                //    _DBResponse.Message = "No Data";
                //    _DBResponse.Data = null;
                //}
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

        private string DailyCashBookWithSdDetailXls(IList<DailyCashBookPpgXLS> OBJDailyCashBookPpg, string date1, string date2)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "(A Govt. of India Undertaking)"
                        + Environment.NewLine + "ICD Patparganj-Delhi"
                        + Environment.NewLine + Environment.NewLine
                        + "ExportContainer Income Detail";
                string typeOfValue = "";

                typeOfValue = "During Period Of " + date1 + " TO " + date2;



                exl.MargeCell("A1:H1", "CENTRAL WAREHOUSING CORPORATION", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A2:H2", "(A Govt. of India Undertaking)", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A3:H3", "ICD Patparganj-Delhi", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A4:H4", "Daily Cash Book With SD  " + typeOfValue, DynamicExcel.CellAlignment.Middle);
                /// exl.MargeCell("A5:H5", typeOfValue, DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("A4:F4", "DETAILS OF ORIGINAL INOICE", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("G4:O4", "DETAILS OF CREDIT NOTE ISSUED", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A5:A6", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B5:B6", "INVOICENO", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C5:C6", "INVOICE DATE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D5:D6", "INVOICE TYPE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E5:E6", "PARTY NAME.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F5:F6", "PAYEE NAME", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G5:G6", "MODE OF PAYMENT", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("H5:H6", "CHQ NO.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I5:I6", "GEN SPACE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J5:J6", "STO", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K5:K6", "INS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L5:L6", "GRE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M5:M6", "GRL", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N5:N6", "MF", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O5:O6", "ENT", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("P5:P6", "FUM ", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Q5:Q6", "OT CHARGE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("R5:R6", "MISC", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("S5:S6", "BILL", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("T5:T6", "CGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("U5:U6", "SGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("V5:V6", "IGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("W5:W6", "TOTAL CASH", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("X5:X6", "TOTAL CHQ", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Y5:Y6", "OTHERS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Z5:Z6", "TOTAL PDA", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AA5:AA6", "TDS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AB5:AB6", "CR TDS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AC5:AC6", "REMARKS", DynamicExcel.CellAlignment.Middle);

               

                exl.AddTable("A", 7, OBJDailyCashBookPpg, new[] { 6, 20, 20, 30, 12, 20, 20, 15, 15, 30, 15, 20, 20, 20, 15, 15, 15, 10, 12, 18, 18, 16, 12, 12, 15, 15, 17, 18, 15, 14, 15, 16, 16, 15, 15, 15, 15, 15, 15 });
                var GenSpace = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.GenSpace)).ToString();
                var StorageCharge = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.StorageCharge)).ToString();
                var Insurance = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.Insurance)).ToString();
                var GroundRentEmpty = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.GroundRentEmpty)).ToString();
                var GroundRentLoaded = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.GroundRentLoaded)).ToString();
                var MfCharge = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.MfCharge)).ToString();
                var EntryCharge = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.EntryCharge)).ToString();
                var Fumigation = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.Fumigation)).ToString();
                var OtherCharge = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.OtherCharge)).ToString();
                var Misc = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.Misc)).ToString();
                var Cgst = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.Cgst)).ToString();
                var Sgst = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.Sgst)).ToString();
                var Igst = OBJDailyCashBookPpg.ToList().Sum(o => Convert.ToDecimal(o.Igst)).ToString();

                var MiscExcess = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.MiscExcess)).ToString();
                var TotalCash = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.TotalCash)).ToString();
                var TotalCheque = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.TotalCheque)).ToString();
                var TotalOthers = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.TotalOthers)).ToString();
                var Tds = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.Tds)).ToString();
               var  CrTds = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.CrTds)).ToString();
               var  TotalPDA = OBJDailyCashBookPpg.Sum(o => Convert.ToDecimal(o.TotalPDA)).ToString();
  



                //var Total = PPGConIncomeDetail.Sum(o => o.Total);


                //var BOEValueDuty = PPGAssesmentSheetfcl.Sum(o => o.BOEValueDuty);
                exl.AddCell("H" + (OBJDailyCashBookPpg.Count + 7).ToString(), "Total", DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("I" + (OBJDailyCashBookPpg.Count + 7).ToString(), GenSpace.ToString(), DynamicExcel.CellAlignment.TopLeft);

                //exl.AddCell("W" + (PPGAssesmentSheetlcl.Count + 7).ToString(), Area.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("J" + (OBJDailyCashBookPpg.Count + 7).ToString(), StorageCharge.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("K" + (OBJDailyCashBookPpg.Count + 7).ToString(), Insurance.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("L" + (OBJDailyCashBookPpg.Count + 7).ToString(), GroundRentEmpty.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("M" + (OBJDailyCashBookPpg.Count + 7).ToString(), GroundRentLoaded.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("N" + (OBJDailyCashBookPpg.Count + 7).ToString(), MfCharge.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("O" + (OBJDailyCashBookPpg.Count + 7).ToString(),EntryCharge.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("AD" + (PPGAssesmentSheetfcl.Count + 7).ToString(), Haz.ToString(), DynamicExcel.CellAlignment.TopLeft);

                exl.AddCell("P" + (OBJDailyCashBookPpg.Count + 7).ToString(), Fumigation.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("Q" + (OBJDailyCashBookPpg.Count + 7).ToString(), OtherCharge.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("R" + (OBJDailyCashBookPpg.Count + 7).ToString(), Misc.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("S" + (OBJDailyCashBookPpg.Count + 7).ToString(), MiscExcess.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("T" + (OBJDailyCashBookPpg.Count + 7).ToString(), Cgst.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("S" + (OBJDailyCashBookPpg.Count + 7).ToString(), Sgst.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("U" + (OBJDailyCashBookPpg.Count + 7).ToString(), Igst.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("W" + (OBJDailyCashBookPpg.Count + 7).ToString(), TotalCash.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("X" + (OBJDailyCashBookPpg.Count + 7).ToString(), TotalCheque.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("Y" + (OBJDailyCashBookPpg.Count + 7).ToString(), TotalOthers.ToString(), DynamicExcel.CellAlignment.TopLeft);

                exl.AddCell("Z" + (OBJDailyCashBookPpg.Count + 7).ToString(), TotalPDA.ToString(), DynamicExcel.CellAlignment.TopLeft);

                exl.AddCell("AA" + (OBJDailyCashBookPpg.Count + 7).ToString(), Tds.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AB" + (OBJDailyCashBookPpg.Count + 7).ToString(), CrTds.ToString(), DynamicExcel.CellAlignment.TopLeft);
               

                ///exl.AddCell("AM" + (PPGConIncomeDetail.Count + 7).ToString(), TaxAmt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("AM" + (PPGConIncomeDetail.Count + 7).ToString(),Total.ToString(), DynamicExcel.CellAlignment.TopLeft);

                exl.Save();
            }
            return excelFile;
        }

        #region Monthly SD Book
        public void MonthSDBookReport(DailyCashBookPpg ObjDailyCashBook)
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
            IDataReader Result = DataAccess.ExecuteDataReader("MonthSDBookReport", CommandType.StoredProcedure, DParam);
            IList<DailyCashBookPpg> LstMonthlyCashBook = new List<DailyCashBookPpg>();
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

                    LstMonthlyCashBook.Add(new DailyCashBookPpg
                    {
                        //CRNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Convert.ToDateTime(Result["InvoiceDate"] == DBNull.Value ? "N/A" : Result["InvoiceDate"]).ToString("dd/MM/yyyy"),

                        //Depositor = Result["Party"].ToString(),
                        //ChqNo = Result["ChequeNo"].ToString(),
                        GenSpace = Result["GenSpace"].ToString(),
                        StorageCharge = Result["sto"].ToString(),
                        Insurance = Result["Insurance"].ToString(),
                        GroundRentEmpty = Result["GroundRentEmpty"].ToString(),
                        GroundRentLoaded = Result["GroundRentLoaded"].ToString(),
                        MfCharge = Result["Mf"].ToString(),
                        ThcCharge = Result["thc"].ToString(),

                        RRCharge = Result["rr"].ToString(),

                        FACCharge = Result["fac"].ToString(),
                        EntryCharge = Result["EntCharge"].ToString(),
                        Fumigation = Result["Fum"].ToString(),
                        OtherCharge = Result["OtCharge"].ToString(),
                        Cgst = Result["CGSTAmt"].ToString(),
                        Sgst = Result["SGSTAmt"].ToString(),
                        Igst = Result["IGSTAmt"].ToString(),
                        Misc = Result["MISC"].ToString(),
                        MiscExcess = Result["MiscExcess"].ToString(),
                        TotalCash = Result["TotalCash"].ToString(),
                        TotalCheque = Result["TotalCheque"].ToString(),
                        Tds = Result["tdsCol"].ToString(),
                        CrTds = Result["crTDS"].ToString(),
                        TFUCharge = Result["TFU"].ToString()

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


        public void PdSummaryReport(PdSummary ObjPdSummaryReport, int type = 1)
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

        public void PrintDetailsForBulkCRNoteForExternalUser(string PeriodFrom, string PeriodTo, string CRDR,int PartyId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Todate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Value = CRDR });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });

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

        #region WorkSlip New
        public void WorkSlipDetailsForPrintNew(string PeriodFrom, string PeriodTo, int CasualLabour, int Uid = 0)
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
                Result = DataAccess.ExecuteDataSet("GetWorkslipReportNew", CommandType.StoredProcedure, DParam);
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

        #region Account Report Export Cargo In General Carting
        public void GetCargoExport(Ppg_CarGenCar objPC)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPC.AsOnDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ExpCarGenCarReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Ppg_ExpCarGen> lstPV = new List<Ppg_ExpCarGen>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Ppg_ExpCarGen
                    {
                        EntryNo = Result["EntryNo"].ToString(),
                        InDate = Result["InDate"].ToString(),
                        SbNo = (Result["SbNo"]).ToString(),
                        SbDate = Result["SbDate"].ToString(),
                        Shed = Result["Shed"].ToString(),
                        Area = Convert.ToDecimal(Result["Area"]),
                        NoOfDays = Convert.ToInt32(Result["NoOfDays"]),
                        NoOfWeek = Convert.ToInt32(Result["NoOfWeek"]),
                        GeneralAmount = Convert.ToDecimal(Result["GeneralAmount"])
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

        #region PV Report
        public void GetPVReportImport(Ppg_PV objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objPV.Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objPV.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Range", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objPV.Ddlrange });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PVReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Ppg_ImpPVReport> lstPV = new List<Ppg_ImpPVReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Ppg_ImpPVReport
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
        public void GetPVReportExport(Ppg_PV objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objPV.Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objPV.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Range", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objPV.Ddlrange });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PVReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Ppg_ExpPVReport> lstPV = new List<Ppg_ExpPVReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Ppg_ExpPVReport
                    {
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
        public void DTRForExport(string DTRDate, string DTRToDate, int GodownId = 0)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DTRDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DTRToDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            _DBResponse = new DatabaseResponse();
            try
            {
                DParam = LstParam.ToArray();
                //Result = DataAccess.ExecuteDataSet("DailyTransactionExp", CommandType.StoredProcedure, DParam);
                PpgDTRExp obj = new PpgDTRExp();
                obj = (PpgDTRExp)DataAccess.ExecuteDynamicSet<PpgDTRExp>("DailyTransactionExp", DParam);
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

        #region SealCuttingReport
        public void SealCuttingReport(SealCuttingReport SealCuttingReport)
        {

            DateTime dtfrom = DateTime.ParseExact(SealCuttingReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(SealCuttingReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;
            DataSet Result = new DataSet();
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });

            DParam = LstParam.ToArray();
            Result = DataAccess.ExecuteDataSet("GetSealCuttingReport", CommandType.StoredProcedure, DParam);
            IList<SealCuttingReport> LstSealClosingReport = new List<SealCuttingReport>();
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result != null && Result.Tables[0].Rows.Count > 0)
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
        #endregion


        #region Collection Report
        public void CollectionReport(Ppg_CollectionReport ObjCollectionReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjCollectionReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjCollectionReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            IDataReader Result = DataAccess.ExecuteDataReader("CollectionReport", CommandType.StoredProcedure, DParam);
            Ppg_FinalCollectionReportTotal LstCollectionReport = new Ppg_FinalCollectionReportTotal();
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

                    LstCollectionReport.listCollectionReport.Add(new Ppg_CollectionReport
                    {
                        Date = Result["DateFormatted"].ToString(),
                        Cash = Result["Cash"].ToString(),
                        Bank = Result["Cheque"].ToString(),//bank has data for cheque see view
                        PO = Result["PO"].ToString(),
                        DD = Result["DRAFT"].ToString(),
                        Pd = Result["NEFT"].ToString(),//neft has data for pd see view
                        Chln = Result["CHALLAN"].ToString(),
                        Total = Result["Total"].ToString()

                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstCollectionReport.objCollectionReportTotal.TotalPDA = Convert.ToString(Result["TotalNEFT"]);
                        LstCollectionReport.objCollectionReportTotal.TotalCash = Convert.ToString(Result["TotalCash"]);
                        LstCollectionReport.objCollectionReportTotal.TotalBank = Convert.ToString(Result["TotalCheque"]);

                        LstCollectionReport.objCollectionReportTotal.TotalPO = Convert.ToString(Result["TotalPO"]);
                        LstCollectionReport.objCollectionReportTotal.TotalCHALLAN = Convert.ToString(Result["TotalCHALLAN"]);
                        LstCollectionReport.objCollectionReportTotal.TotalDraft = Convert.ToString(Result["TotalDRAFT"]);



                    }
                }
                if (Status == 1)
                {
                    if (LstCollectionReport.listCollectionReport.Count > 0)
                    {
                        LstCollectionReport.listCollectionReport.Add(new Ppg_CollectionReport
                        {
                            Date = "Total",
                            Cash = LstCollectionReport.listCollectionReport.Sum(o => Convert.ToDecimal(o.Cash)).ToString(),
                            Bank = LstCollectionReport.listCollectionReport.Sum(o => Convert.ToDecimal(o.Bank)).ToString(),
                            PO = LstCollectionReport.listCollectionReport.Sum(o => Convert.ToDecimal(o.PO)).ToString(),
                            DD = LstCollectionReport.listCollectionReport.Sum(o => Convert.ToDecimal(o.DD)).ToString(),
                            Pd = LstCollectionReport.listCollectionReport.Sum(o => Convert.ToDecimal(o.Pd)).ToString(),
                            Chln = LstCollectionReport.listCollectionReport.Sum(o => Convert.ToDecimal(o.Chln)).ToString(),
                            Total = LstCollectionReport.listCollectionReport.Sum(o => Convert.ToDecimal(o.Total)).ToString()

                        });
                    }
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCollectionReport;
                }
                else
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
        public void GetImpCargo(Ppg_PV objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objPV.GodownId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpCargoReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Ppg_ImpPVReport> lstPV = new List<Ppg_ImpPVReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Ppg_ImpPVReport
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

        #region Cheque Summary Report
        public void ChequeSummary(Ppg_ChequeSummary ObjChequeSummary)
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
            IList<Ppg_ChequeSummary> LstChequeSummary = new List<Ppg_ChequeSummary>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstChequeSummary.Add(new Ppg_ChequeSummary
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
                    LstChequeSummary.Add(new Ppg_ChequeSummary
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

        public void ChequeCashDDPOSummary(Ppg_CashChequeDDSummary ObjChequeSummary)
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
            IList<Ppg_CashChequeDDSummary> LstChequeSummary = new List<Ppg_CashChequeDDSummary>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstChequeSummary.Add(new Ppg_CashChequeDDSummary
                    {



                        Bank = Result["DraweeBank"].ToString(),
                        Type = Result["PayMode"].ToString(),
                        CashAmount = Convert.ToDecimal(Result["CASHAmount"]),
                        ChequeAmount = Convert.ToDecimal(Result["CHEQUEAmount"]),
                        POSAmount = Convert.ToDecimal(Result["POAmount"]),
                        Amount = Result["OtherAmount"].ToString(),
                        SDAmount = Convert.ToDecimal(Result["SDAmount"]),
                        OnlineAmount = Convert.ToDecimal(Result["OnlineAmount"]),

                        GCashAmount = Convert.ToDecimal(Result["CASHAmount"]),
                        GChequeAmount = Convert.ToDecimal(Result["CHEQUEAmount"]),
                        GPOSAmount = Convert.ToDecimal(Result["POAmount"]),
                        GOthersAmount = Convert.ToDecimal(Result["OtherAmount"]),
                        GOnlineAmount= Convert.ToDecimal(Result["OnlineAmount"]),
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
                    LstChequeSummary.Add(new Ppg_CashChequeDDSummary
                    {



                        Bank = string.Empty,

                        GOthersAmount = LstChequeSummary.ToList().Sum(m => Convert.ToDecimal(m.GOthersAmount)),
                        GCashAmount = LstChequeSummary.ToList().Sum(m => m.GCashAmount),
                        GChequeAmount = LstChequeSummary.ToList().Sum(m => m.ChequeAmount),
                        GPOSAmount = LstChequeSummary.ToList().Sum(m => m.GPOSAmount),
                        GOnlineAmount= LstChequeSummary.ToList().Sum(m => m.GOnlineAmount),
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

        #region PV Report Of Empty Container
        public void GetEmptyCont(Ppg_PVReport objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PVReportEmptyCont", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Ppg_PVReportImpEmptyCont> lstPV = new List<Ppg_PVReportImpEmptyCont>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Ppg_PVReportImpEmptyCont
                    {
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


        #region LongStandingEmptyCont Container
        public void LongStandingEmptyCont(Ppg_PVReport objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("LongStandingEmptyContiner", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Ppg_LongStandingEmptyCont> lstPV = new List<Ppg_LongStandingEmptyCont>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Ppg_LongStandingEmptyCont
                    {
                        CFSCode = Result["EntryNo"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        Address = Result["Address"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        EntryDateTime = Result["InDate"].ToString(),
                        Size = Result["Size"].ToString(),
                        EximTraderAlias = Result["SlaCd"].ToString(),
                        Days = Convert.ToInt32(Result["Days"]),
                        Amount = Convert.ToDecimal(Result["GRE"]),
                        InDateEcy = Result["InDateEcy"].ToString(),
                        OutDateEcy = Result["OutDate"].ToString(),
                        Notices1 = Result["NoticeDate1"].ToString(),
                        Notices2 = Result["NoticeDate2"].ToString(),
                        AuctionNo = Result["AuctionNoticeNo"].ToString(),
                        NocNO = Result["RefNo"].ToString(),
                        NocDate = Result["RefDate"].ToString(),
                        Remarks = Result["Remarks1"].ToString(),


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

        #region PV Report Of Import Loaded Container
        public void GetImpLoadedCont(Ppg_PVReport objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PVReportImpLoadedCont", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<Ppg_PVReportImpLoadedCont> lstPV = new List<Ppg_PVReportImpLoadedCont>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Ppg_PVReportImpLoadedCont
                    {
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

        #region Bulk Credit Note print
        public void BulkCreditNoteprint(string InvoiceNumber, int PartyId, DateTime FromDate, DateTime ToDate)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CRNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNumber });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("getBulkCRNoteDetailsForPrint", CommandType.StoredProcedure, DParam);
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

        #region Daily Invoice Report
        public void DailyInvoiceReport(Ppg_DailyInvReport ObjDailyReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyReport.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyReport.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.String, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.String, Value = PeriodTo });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjTDSReport.PartyId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyInvoiceReport", CommandType.StoredProcedure, DParam);
            IList<Ppg_DailyReport> lstPV = new List<Ppg_DailyReport>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new Ppg_DailyReport
                    {
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        Party = Result["PartyName"].ToString(),
                        TotalAmt = Convert.ToDecimal(Result["TotalAmt"]),
                        ReceiptNo = Result["ReceiptNo"].ToString(),
                        ReceiptAmt = Convert.ToDecimal(Result["Amount"])
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

        #region SDSummary
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
            IList<SDSummary> lstPV = new List<SDSummary>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new SDSummary
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        Date = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        Module = Result["InvoiceType"].ToString(),
                        EximTraderName = Result["PartyName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),

                        BILL = Convert.ToDecimal(Result["MiscExcess"]),
                        GEN = Convert.ToDecimal(Result["GenSpace"]),
                        STO = Convert.ToDecimal(Result["sto"]),
                        INS = Convert.ToDecimal(Result["Insurance"]),
                        GRE = Convert.ToDecimal(Result["GroundRentEmpty"]),
                        GRL = Convert.ToDecimal(Result["GroundRentLoaded"]),
                        MFCHRG = Convert.ToDecimal(Result["Mf"]),
                        //MFTAX = Convert.ToDecimal(Result["MFTAX"]),
                        PDA = Convert.ToDecimal(Result["TotalPDA"]),
                        ENT = Convert.ToDecimal(Result["EntCharge"]),
                        FUM = Convert.ToDecimal(Result["Fum"]),
                        OT = Convert.ToDecimal(Result["OtCharge"]),
                        CGST = Convert.ToDecimal(Result["CGSTAmt"]),
                        SGST = Convert.ToDecimal(Result["SGSTAmt"]),
                        IGST = Convert.ToDecimal(Result["IGSTAmt"]),
                        MISC = Convert.ToDecimal(Result["MISC"]),
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

        #region Satement Of Empty Container
        public void StatementOfEmptyContainer(PpgStatementOfEmptyContainer ObjStatementOfEmptyContainer)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjStatementOfEmptyContainer.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjStatementOfEmptyContainer.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            IDataReader Result = DataAccess.ExecuteDataReader("StatementOfEmptyContainer", CommandType.StoredProcedure, DParam);
            IList<PpgStatementOfEmptyContainer> LstStatementOfEmptyContainer = new List<PpgStatementOfEmptyContainer>();
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

                    LstStatementOfEmptyContainer.Add(new PpgStatementOfEmptyContainer
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        DateOfArrival = Result["DateOFarrival"].ToString(),
                        ImportExport = Result["OperationType"].ToString(),
                        dateofdetuffing = Result["DateOFDestuffing"] == DBNull.Value ? "" : Result["DateOFDestuffing"].ToString()

                        // Withdraw = Result["Withdraw"].ToString()//OperationType

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStatementOfEmptyContainer;
                }
                else
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

        #region GodownList according to Godown Rights
        public void GetGodownRightsWise(int Uid)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGodownListAccdRights", CommandType.StoredProcedure, DParam);
            IList<GodownList> lst = new List<GodownList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lst.Add(new GodownList
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
        #region Export RR report
        public void GetContainerForExportRR()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerForExportRR", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            List<PPGExportRRReport> LstStuffing = new List<PPGExportRRReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    if (LstStuffing.Count <= 0)
                    {
                        LstStuffing.Add(new PPGExportRRReport
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString()
                        });
                    }

                    else
                    {
                        int flag = 0;

                        for (int i1 = 0; i1 < LstStuffing.Count; i1++)
                        {
                            if (LstStuffing[i1].ContainerNo == Result["ContainerNo"].ToString())
                            {
                                flag = 1;
                                break;
                            }
                        }

                        if (flag == 0)
                        {
                            LstStuffing.Add(new PPGExportRRReport
                            {
                                ContainerNo = Result["ContainerNo"].ToString(),
                                CFSCode = Result["CFSCode"].ToString()
                            });
                        }
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
                }
                else
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



        public void PrintExportRR(String ContNo, String CfsCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContNo });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CfsCode });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintExportRR", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PrintExportRRReport ObjDeliveryOrder = new PrintExportRRReport();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDeliveryOrder.TrainNo = Result["TrainNo"].ToString();
                    ObjDeliveryOrder.TrainDate = Result["TrainDate"].ToString();
                    ObjDeliveryOrder.InvoiceNo = Convert.ToString(Result["InvoiceNo"]);
                    ObjDeliveryOrder.CFS_Port = Convert.ToString(Result["CFS_Port"]);
                    ObjDeliveryOrder.PortCode = Convert.ToString(Result["PortCode"]);
                    ObjDeliveryOrder.SLine = Convert.ToString(Result["SLine"]);

                    ObjDeliveryOrder.CustomSeal = Result["CustomSeal"].ToString();
                    ObjDeliveryOrder.TW = Convert.ToDecimal(Result["TW"].ToString());
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstPartyDetails.Add(new ppgPartyDetails
                        {
                            PartyName = Result["PartyName"].ToString(),
                            PartyAddress = Result["PartyAddress"].ToString(),
                            PayeeName = Result["PayeeName"].ToString(),
                            PayeeAddress = Result["PayeeAddress"].ToString()

                        });
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstContDetails.Add(new ppgContDetails
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            Loaded = Result["Loaded"].ToString(),
                            cType = Result["cType"].ToString(),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"])

                        });
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstChargesDetails.Add(new ppgChargesDetails
                        {
                            RR = Convert.ToDecimal(Result["RR"]),
                            THC = Convert.ToDecimal(Result["THC"]),
                            CGST = Convert.ToDecimal(Result["CGST"].ToString()),
                            SGST = Convert.ToDecimal(Result["SGST"].ToString()),
                            IGST = Convert.ToDecimal(Result["IGST"].ToString()),
                            Inv = Convert.ToDecimal(Result["Inv"].ToString())
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstCompDetails.Add(new ppgCompDetails
                        {
                            Pan = Result["Pan"].ToString(),
                            GstIn = Result["GstIn"].ToString()
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstReceiptDetails.Add(new ppgReceiptDetails
                        {
                            InvoiceNo = Result["InvoiceNo"].ToString(),
                            ReceiptNo = Result["ReceiptNo"].ToString(),
                            ReceiptDate = Result["ReceiptDate"].ToString(),
                            InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"].ToString()),
                            PayeeName = Result["PayeeName"].ToString(),

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
            IList<PpgVCCapacityModel> LstVCCapacity = new List<PpgVCCapacityModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstVCCapacity.Add(new PpgVCCapacityModel
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
                        LstVCCapacity.Add(new PpgVCCapacityModel());
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
            IList<PpgVCCapacityModel> LstVCCapacity = new List<PpgVCCapacityModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstVCCapacity.Add(new PpgVCCapacityModel
                    {
                        Occupency = (Result["occupency"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["occupency"])),
                        Income = (Result["income"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["income"]))
                    });
                }
                if (LstVCCapacity.Count() < 3)
                {
                    for (var i = LstVCCapacity.Count; i < 3; i++)
                    {
                        LstVCCapacity.Add(new PpgVCCapacityModel());
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
        #region-- LandingCertificate --
        public void GetLandingCertificateReport(String TrainNo, String TrainDate)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainNo", MySqlDbType = MySqlDbType.String, Value = TrainNo });
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(TrainDate) });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetLandingCertificateReport", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();
                TrainDetl trndtl = new TrainDetl();
                int Sr = 0;
                if (Result != null && Result.Tables.Count > 0)
                {
                    trndtl.TrainNo = Result.Tables[0].Rows[0]["TrainNo"].ToString();
                    trndtl.TrainDate = Result.Tables[0].Rows[0]["TrainDate"].ToString();
                    trndtl.PortName = Result.Tables[0].Rows[0]["PortName"].ToString();
                    foreach (DataRow dr in Result.Tables[1].Rows)
                    {
                        Status = 1;
                        trndtl.objTrainSummaryUpload.Add(new LandingCerificate
                        {
                            SrNo = Sr + 1,
                            Container_No = Convert.ToString(dr["Container_No"]),
                            CT_Size = Convert.ToString(dr["CT_Size"]),
                            S_Line = Convert.ToString(dr["S_Line"]),
                            TP_No = Convert.ToString(dr["Smtp_No"]),
                            TP_Date = Convert.ToString(dr["Smtp_Date"]),
                            ArrivalDate = Convert.ToString(dr["ArrivalDate"]),
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = trndtl;
                }
                else
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

        public void GetAllTrainNo()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            // lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetAllTrainNoForLC", CommandType.StoredProcedure, dpram);
            IList<PPG_TrainList> lstTrainNo = new List<PPG_TrainList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {

                    Status = 1;
                    lstTrainNo.Add(new PPG_TrainList { TrainNo = result["TrainNo"].ToString(), TrainSummaryID = Convert.ToInt32(result["TrainSummaryID"]) });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstTrainNo;
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

        public void GetTrainDateByTrainNo(String TrainNo)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = TrainNo });
            // lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetTrainDateByTrainNo", CommandType.StoredProcedure, dpram);
            IList<PPG_TrainDateList> lstTrainDate = new List<PPG_TrainDateList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {

                    Status = 1;
                    lstTrainDate.Add(new PPG_TrainDateList { TrainDate = result["TrainDate"].ToString() });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstTrainDate;
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

        #region Long Standing Report For Container
        public void GetLongStandingImpLoadedCont(Ppg_LongStandingImpCon ObjContainerBalanceInCFS)
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
            IList<Ppg_LongStandingImpConDtl> LstContainerBalanceInCFS = new List<Ppg_LongStandingImpConDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstContainerBalanceInCFS.Add(new Ppg_LongStandingImpConDtl
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

        #region Long Standing Report For Cargo
        public void GetLongStandingImpLoadedCargo(Ppg_LongStandingImpCargo ObjContainerBalanceInCFS)
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
            IList<Ppg_LongStandingImpCargoDtl> LstContainerBalanceInCFS = new List<Ppg_LongStandingImpCargoDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstContainerBalanceInCFS.Add(new Ppg_LongStandingImpCargoDtl
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

        #region Export Job Order report
        public void GetContainerForExportJobOrder(String Type)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = Type });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerForExportJobOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPGExportRRReport> LstStuffing = new List<PPGExportRRReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    if (LstStuffing.Count <= 0)
                    {
                        LstStuffing.Add(new PPGExportRRReport
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString()
                        });
                    }

                    else
                    {
                        int flag = 0;

                        for (int i1 = 0; i1 < LstStuffing.Count; i1++)
                        {
                            if (LstStuffing[i1].ContainerNo == Result["ContainerNo"].ToString())
                            {
                                flag = 1;
                                break;
                            }
                        }

                        if (flag == 0)
                        {
                            LstStuffing.Add(new PPGExportRRReport
                            {
                                ContainerNo = Result["ContainerNo"].ToString(),
                                CFSCode = Result["CFSCode"].ToString()
                            });
                        }
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
                }
                else
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
        public void PrintExportJobOrder(String PFrom, String PTo, String Ttype)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(PFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(PTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Ttype", MySqlDbType = MySqlDbType.VarChar, Value = Ttype });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("GetDetForPrntjoborder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CwcExim.Areas.Export.Models.PPGPrintJOModel objMdl = new CwcExim.Areas.Export.Models.PPGPrintJOModel();
            _DBResponse = new DatabaseResponse();
            List<CwcExim.Areas.Export.Models.PPGPrintJOModelDet> lstJobOrder = new List<CwcExim.Areas.Export.Models.PPGPrintJOModelDet>();
            try
            {

                while (result.Read())
                {
                    Status = 1;
                    lstJobOrder.Add(new CwcExim.Areas.Export.Models.PPGPrintJOModelDet
                    {
                        ContainerNo = result["ContainerNo"].ToString(),
                        ContainerSize = result["SZ"].ToString(),
                        ShippingLineName = result["ShippingLineName"].ToString(),
                        OnBehalf = result["OnBehalf"].ToString(),
                        CustomSealNo = result["CustomSeal"].ToString(),

                        Sline = result["LineSeal"].ToString(),
                        POL = result["POL"].ToString(),
                        POD = result["POD"].ToString(),
                        Ct_Tare = Convert.ToDecimal(result["TW"].ToString()),
                        Cargo_Wt = Convert.ToDecimal(result["CW"].ToString()),
                        CFSCode = result["CFSCode"].ToString(),


                        CargoType = result["CargoType"].ToString(),
                        ContainerLoadType = result["ContainerLoadType"].ToString(),
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstJobOrder;
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
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void PrintExportJoSum(String PFrom, String PTo, String Ttype)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(PFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(PTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Ttype", MySqlDbType = MySqlDbType.VarChar, Value = Ttype });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("GetDetForPrntjosummary", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CwcExim.Areas.Export.Models.PPGPrintJOModel objMdl = new CwcExim.Areas.Export.Models.PPGPrintJOModel();
            _DBResponse = new DatabaseResponse();
            List<CwcExim.Areas.Export.Models.ppgexportjobordersum> lstJobOrder = new List<CwcExim.Areas.Export.Models.ppgexportjobordersum>();
            try
            {

                while (result.Read())
                {
                    Status = 1;
                    lstJobOrder.Add(new CwcExim.Areas.Export.Models.ppgexportjobordersum
                    {

                        POL = result["POL"].ToString(),
                        SZ20 = result["SZ20"] == System.DBNull.Value ? 0 : Convert.ToInt32(result["SZ20"].ToString()),
                        SZ40 = result["SZ40"] == System.DBNull.Value ? 0 : Convert.ToInt32(result["SZ40"].ToString())
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstJobOrder;
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
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        //New format
        public void PrintExportJobOrderData(String PFrom, String PTo, String Ttype)
        {
            int Status = 0;
            String PeriodFrom = DateTime.ParseExact(PFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
            String PeriodTo = DateTime.ParseExact(PTo, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Ttype", MySqlDbType = MySqlDbType.VarChar, Value = Ttype });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("GetDetForPrntjoborder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            //CwcExim.Areas.Export.Models.PPGPrintJOModel objMdl = new CwcExim.Areas.Export.Models.PPGPrintJOModel();
            List<Ppg_ExportJobOrder> lstJobOrder = new List<Ppg_ExportJobOrder>();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstJobOrder.Add(new Ppg_ExportJobOrder
                    {
                        GatePassNo = result["GatePassNo"].ToString(),
                        CFSCode = result["CFSCode"].ToString(),
                        ContainerNo = result["ContainerNo"].ToString(),
                        Size = result["Size"].ToString(),
                        GatePassDate = result["GatePassDate"].ToString(),
                        SLA = result["SLA"].ToString(),
                        FRW = result["FRW"].ToString(),
                        CustomSeal = result["CustomSeal"].ToString(),
                        ShippingSeal = result["ShippingSeal"].ToString(),
                        POL = result["POL"].ToString(),
                        POD = result["POD"].ToString(),
                        TareWeight = result["TareWeight"].ToString(),
                        CargoWeight = result["CargoWeight"].ToString(),
                        Fob = result["Fob"].ToString(),
                        Via = result["Via"].ToString(),
                        TransportMode = result["TransportMode"].ToString(),
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstJobOrder;
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
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        #endregion

        #region core data report
        public void PrintCoreData(String Fdt, String ToDt)
        {


            int Status = 0;
            //string Flag = "";
            //if (ObjExemptedService.Registered == 0)
            //{
            //    Flag = "All";
            //}
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Fdt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = ToDt });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjTDSReport.PartyId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("CoreDataReport", CommandType.StoredProcedure, DParam);
            PPGCoreData objCoreData = new PPGCoreData();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objCoreData.ICDCash = Convert.ToDecimal(Result["ICDCash"].ToString());
                    objCoreData.ICDBill = Convert.ToDecimal(Result["ICDBill"].ToString());
                    objCoreData.ICDTotal = Convert.ToDecimal(Result["ICDTotal"].ToString());
                    objCoreData.DESSCash = Convert.ToDecimal(Result["DESSCash"].ToString());
                    objCoreData.DESSTotal = Convert.ToDecimal(Result["DESSTotal"].ToString());
                    objCoreData.CFSCash = Convert.ToDecimal(Result["CFSCash"].ToString());
                    objCoreData.IRRCash = Convert.ToDecimal(Result["IRRCash"].ToString());
                    objCoreData.IRRTotal = Convert.ToDecimal(Result["IRRTotal"].ToString());
                    objCoreData.GrossCash = Convert.ToDecimal(Result["GrossCash"].ToString());
                    objCoreData.GrossBill = Convert.ToDecimal(Result["GrossBill"].ToString());
                    objCoreData.GrossTotal = Convert.ToDecimal(Result["GrossTotal"].ToString());
                    objCoreData.CFSTotal = Convert.ToDecimal(Result["CFSTotal"].ToString());


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objCoreData;
                }
                else
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

        #region ContainerArrivalReport
        public void ContainerArrivalReport(ContainerArrivalReport ObjContainerArrivalReport)
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
            IDataReader Result = DataAccess.ExecuteDataReader("ContainerArrivalReport", CommandType.StoredProcedure, DParam);
            //IList<ContainerArrivalReport> LstContainerArrivalReport = new List<ContainerArrivalReport>();
            ContainerArrivalReport LstContainerArrivalReport = new ContainerArrivalReport();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            int SizeTwenty = 0;
            int SizeFourty = 0;
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

                    LstContainerArrivalReport.ListArrivalReport.Add(new ArrivalReportList
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        LoadOrEmpty = Result["LoadOrEmpty"].ToString(),
                        SealNo = Result["CustomSealNo"].ToString(),
                        Date = Result["Date"].ToString(),
                        Time = Result["Time"].ToString(),
                        Commodity = Result["Commodity"].ToString(),
                        ImportExport = Result["OperationType"].ToString(),
                        Size = Convert.ToString(Result["Size"] == null ? "" : Result["Size"]),
                        ICDCode = Result["ICDCode"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ModeOfTransport = Result["TransportMode"].ToString(),
                        JobDate = Result["formonedate"].ToString(),
                        ReceivedFrom = Result["terminallocation"].ToString(),
                        // Withdraw = Result["Withdraw"].ToString()

                    });
                }

                if (Status == 1)
                {
                    SizeTwenty = LstContainerArrivalReport.ListArrivalReport.Count(o => o.Size == "20");
                    SizeFourty = LstContainerArrivalReport.ListArrivalReport.Count(o => o.Size == "40");
                    LstContainerArrivalReport.SizeTewnty = Convert.ToString(SizeTwenty);
                    LstContainerArrivalReport.SizeFourty = Convert.ToString(SizeFourty);
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

        #region Container Payment Details
        public void ListOfContainerWithCFSCode(string CFSCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = CFSCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerWithCFSCode", CommandType.StoredProcedure, Dparam);
            IList<Ppg_ContainerList> lstContainer = new List<Ppg_ContainerList>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstContainer.Add(new Ppg_ContainerList
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"])
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
                    _DBResponse.Data = new { lstContainer, State };
                }
                else
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
        public void ContainerPaymentDetail(Ppg_ContainerPaymentInfo Obj)
        {
            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = Obj.CFSCode });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getContainerEnquiry", CommandType.StoredProcedure, DParam);
            //List<Ppg_ContainerPaymentDtl> LstMonthlyCashBook = new List<Ppg_ContainerPaymentDtl>();
            Ppg_ContainerPaymentInfo CPI = new Ppg_ContainerPaymentInfo();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    CPI.CFSCode = Convert.ToString(Result["CFSCode"]);
                    CPI.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                    CPI.InDate = Convert.ToString(Result["EntryDateTime"]);
                    CPI.Size = Convert.ToString(Result["Size"]);
                    CPI.EximTraderAlias = Convert.ToString(Result["EximTraderAlias"]);
                    CPI.OutDate = Convert.ToString(Result["GateExitDateTime"]);

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        CPI.lstContainerPaymentDtl.Add(new Ppg_ContainerPaymentDtl
                        {
                            ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),
                            ReceiptNo = Result["ReceiptNo"].ToString(),
                            PartyName = Result["PartyName"].ToString(),
                            ChargeType = Convert.ToString(Result["ChargeType"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            FromDate = Result["FromDate"].ToString(),
                            ToDate = Result["ToDate"].ToString(),
                            InvoiceType = Result["InvoiceType"].ToString()
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CPI;
                }
                else
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

        #region tds report
        public void TdsReport(TDSReport ObjTDSReport)
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
            TDSMain objTDSMain = new TDSMain();
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

                    objTDSMain.TDSReportLst.Add(new TDSReport
                    {
                        CRNo = Result["ReceiptNo"].ToString(),
                        CRDate = Result["ReceiptDate"].ToString(),
                        PartyCode = Result["partycode"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        CertificateNo = Result["CertificateNo"].ToString(),
                        FinancialYear = Result["FINANCIALYEAR"].ToString(),
                        QUARTERMONTH = Result["QUARTERMONTH"].ToString(),
                        Amount = Result["Amount"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                    });
                }

                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        objTDSMain.TDSReportLst.Add(
                //        new TDSReport
                //        {
                //            CRNo = Result["ReceiptNo"].ToString(),
                //            CRDate = Result["ReceiptDate"].ToString(),
                //            PartyCode = Result["partycode"].ToString(),
                //            PartyName = Result["PartyName"].ToString(),
                //            CertificateNo = Result["CertificateNo"].ToString(),
                //            FinancialYear = Result["FINANCIALYEAR"].ToString(),
                //            QUARTERMONTH = Result["QUARTERMONTH"].ToString(),
                //            Amount = Result["Amount"].ToString(),
                //        });

                //objTDSMain.ObjTDSReporPartyWise.Party = Convert.ToString(Result["Party"]);
                //objTDSMain.ObjTDSReporPartyWise.Tan = Convert.ToString(Result["Tan"]);
                //objTDSMain.ObjTDSReporPartyWise.Value = Convert.ToString(Result["Value"]);
                //objTDSMain.ObjTDSReporPartyWise.TDS = Convert.ToString(Result["TDS"]);



                //    }
                //}
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
        #endregion

        #region Import Con Income
        public void ImportConIncomeDetail(Ppg_ImportConIncome Obj)
        {
            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.VarChar, Value = Obj.FromDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.VarChar, Value = Obj.ToDate });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getImportContainerIncome", CommandType.StoredProcedure, DParam);
            List<Ppg_ImportConIncomeDtl> lstImportConIncomeDtl = new List<Ppg_ImportConIncomeDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstImportConIncomeDtl.Add(new Ppg_ImportConIncomeDtl
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        PartyName = Result["PartyName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        EntryNo = Result["GateInNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        MovementType = Result["MovementType"].ToString(),
                        TrainNo = Result["TrainNo"].ToString(),
                        TrainDate = Result["TrainDate"].ToString(),
                        SLACode = Convert.ToString(Result["SLACode"]),
                        PortCode = Result["PortCode"].ToString(),
                        DestuffingDate = Result["DestuffingDate"].ToString(),
                        CustomSealNo = Result["CustomSealNo"].ToString(),
                        ShedNo = Result["ShedNo"].ToString(),
                        THC = Convert.ToDecimal(Result["THC"]),
                        TPT = Convert.ToDecimal(Result["TPT"]),
                        FAC = Convert.ToDecimal(Result["FAC"]),
                        ECC = Convert.ToDecimal(Result["ECC"]),
                        DTF = Convert.ToDecimal(Result["DTF"]),
                        LOL = Convert.ToDecimal(Result["LOL"]),
                        IRR = Convert.ToDecimal(Result["IRR"]),
                        HAZ = Convert.ToDecimal(Result["HAZ"]),
                        TFU = Convert.ToDecimal(Result["TFU"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        Total = Convert.ToDecimal(Result["Total"])
                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstImportConIncomeDtl;
                }
                else
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
        public void ImportConIncomeDetailEXCEL(Ppg_ImportConIncome Obj)
        {
            int Status = 0;

            DateTime dtfrom = DateTime.ParseExact(Obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(Obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.VarChar, Value = Obj.FromDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.VarChar, Value = Obj.ToDate });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getImportContainerIncome", CommandType.StoredProcedure, DParam);
            List<Ppg_ImportConIncomeDtlEXCEL> lstImportConIncomeDtl = new List<Ppg_ImportConIncomeDtlEXCEL>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstImportConIncomeDtl.Add(new Ppg_ImportConIncomeDtlEXCEL
                    {
                        SLNO = Convert.ToInt32(Result["SLNO"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        PartyName = Result["PartyName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        EntryNo = Result["GateInNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        MovementType = Result["MovementType"].ToString(),
                        TrainNo = Result["TrainNo"].ToString(),
                        TrainDate = Result["TrainDate"].ToString(),
                        SLACode = Convert.ToString(Result["SLACode"]),
                        PortCode = Result["PortCode"].ToString(),
                        DestuffingDate = Result["DestuffingDate"].ToString(),
                        CustomSealNo = Result["CustomSealNo"].ToString(),
                        ShedNo = Result["ShedNo"].ToString(),
                        THC = Convert.ToDecimal(Result["THC"]),
                        TPT = Convert.ToDecimal(Result["TPT"]),
                        ECC = Convert.ToDecimal(Result["ECC"]),
                        DTF = Convert.ToDecimal(Result["DTF"]),
                        LOL = Convert.ToDecimal(Result["LOL"]),
                        IRR = Convert.ToDecimal(Result["IRR"]),
                        HAZ = Convert.ToDecimal(Result["HAZ"]),
                        TFU = Convert.ToDecimal(Result["TFU"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        Total = Convert.ToDecimal(Result["Total"])
                    });
                    DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    decimal InvoiceAmount = 0, CRAmount = 0;

                    _DBResponse.Data = ConIncomeDetail(lstImportConIncomeDtl, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"));

                }

                //if (Status == 1)
                //{
                //    _DBResponse.Status = 1;
                //    _DBResponse.Message = "Success";
                //    _DBResponse.Data = lstImportConIncomeDtl;
                //}
                //else
                //{
                //    _DBResponse.Status = 0;
                //    _DBResponse.Message = "No Data";
                //    _DBResponse.Data = null;
                //}
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

        private string ConIncomeDetail(List<Ppg_ImportConIncomeDtlEXCEL> PPGConIncomeDetail, string date1, string date2)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "(A Govt. of India Undertaking)"
                        + Environment.NewLine + "ICD Patparganj-Delhi"
                        + Environment.NewLine + Environment.NewLine
                        + "Container Income Detail";
                string typeOfValue = "";

                typeOfValue = "During Period Of " + date1 + " TO " + date2;



                exl.MargeCell("A1:H1", "CENTRAL WAREHOUSING CORPORATION", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A2:H2", "(A Govt. of India Undertaking)", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A3:H3", "ICD Patparganj-Delhi", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A4:H4", "Container Income Detail", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A5:H5", typeOfValue, DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("A4:F4", "DETAILS OF ORIGINAL INOICE", DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("G4:O4", "DETAILS OF CREDIT NOTE ISSUED", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A5:A6", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B5:B6", "InvoiceNo", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C5:C6", "InvoiceDate", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D5:D6", "PARTY NAME", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E5:E6", "PARTY CODE.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F5:F6", "CONTAINER NO", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G5:G6", "ICD CODE", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("H5:H6", "ENTRY NO.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I5:I6", "SIZE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J5:J6", "CONTAINER MOVEMENT TYPE(ByTrain / Road)", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("5:I6", tRAIN NO", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K5:K6", "TRAIN NO", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L5:L6", "TRAIN DATE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M5:M6", "SLA CODE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N5:N6", "PORT CODE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O5:O6", "DESTUFFING DATE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("P5:P6", "COSTUM SEAL NO", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Q5:Q6", "SHED NO", DynamicExcel.CellAlignment.Middle);


                exl.MargeCell("R5:Y5", "INCOME CHARGES CODE", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("R6:R6", "THC", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("S6:S6", "TPT.", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("P6:P6", "STORAGECHG", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("T6:T6", "ECC", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("U6:U6", "DTF", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("V6:V6", "IRR", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("W6:W6", "LOL", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("X6:X6", "HAZ", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Y6:Y6", "TFU", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("Z5:AB5", "TAXES", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Z6:Z6", "CGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AA6:AA6", "SGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AB6:AB6", "IGST", DynamicExcel.CellAlignment.Middle);


                exl.MargeCell("AC5:AC6", "Total", DynamicExcel.CellAlignment.Middle);


                //for (var i = 65; i < 86; i++)
                //{
                //    char character = (char)i;
                //    string text = character.ToString();
                //    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                //}

                exl.AddTable("A", 7, PPGConIncomeDetail, new[] { 6, 20, 20, 30, 12, 20, 20, 15, 15, 30, 15, 20, 20, 20, 15, 15, 15, 10, 12, 18, 18, 16, 12,12,12, 12, 15, 15, 17, 18 });
                var TaxAmt = PPGConIncomeDetail.Sum(o => o.Total);

                var igstamt = PPGConIncomeDetail.Sum(o => o.IGST);
                var sgstamt = PPGConIncomeDetail.Sum(o => o.SGST);
                var cgstamt = PPGConIncomeDetail.Sum(o => o.CGST);
                //var Area = PPGAssesmentSheetlcl.Sum(o => o.Area);
                // var Weight = PPGAssesmentSheetfcl.Sum(o => o.GrossWt);
                var THC = PPGConIncomeDetail.Sum(o => o.THC);
                var TPT = PPGConIncomeDetail.Sum(o => o.TPT);


                var ECC = PPGConIncomeDetail.Sum(o => o.ECC);
                var DTF = PPGConIncomeDetail.Sum(o => o.DTF);
                var LOL = PPGConIncomeDetail.Sum(o => o.LOL);
                var IRR = PPGConIncomeDetail.Sum(o => o.IRR);
                var Haz = PPGConIncomeDetail.Sum(o => o.HAZ);
                var TFU = PPGConIncomeDetail.Sum(o => o.TFU);
                //var BOEValueDuty = PPGAssesmentSheetfcl.Sum(o => o.BOEValueDuty);
                exl.AddCell("Q" + (PPGConIncomeDetail.Count + 7).ToString(), "Total", DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("R" + (PPGConIncomeDetail.Count + 7).ToString(), THC.ToString(), DynamicExcel.CellAlignment.TopLeft);

                //exl.AddCell("I" + (PPGAssesmentSheetlcl.Count + 7).ToString(), Area.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("S" + (PPGConIncomeDetail.Count + 7).ToString(), TPT.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("T" + (PPGConIncomeDetail.Count + 7).ToString(), ECC.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("U" + (PPGConIncomeDetail.Count + 7).ToString(), DTF.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("V" + (PPGConIncomeDetail.Count + 7).ToString(), LOL.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("W" + (PPGConIncomeDetail.Count + 7).ToString(), IRR.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("X" + (PPGConIncomeDetail.Count + 7).ToString(), Haz.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("Y" + (PPGConIncomeDetail.Count + 7).ToString(), TFU.ToString(), DynamicExcel.CellAlignment.TopLeft);

                // exl.AddCell("S" + (PPGAssesmentSheetfcl.Count + 7).ToString(), Haz.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("Z" + (PPGConIncomeDetail.Count + 7).ToString(), cgstamt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AA" + (PPGConIncomeDetail.Count + 7).ToString(), sgstamt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AB" + (PPGConIncomeDetail.Count + 7).ToString(), igstamt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AC" + (PPGConIncomeDetail.Count + 7).ToString(), TaxAmt.ToString(), DynamicExcel.CellAlignment.TopLeft);


                exl.Save();
            }
            return excelFile;
        }

        #endregion

        #region Export Con Income
        public void ExportConIncomeDetail(Ppg_ExportConIncome Obj)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(Obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(Obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.VarChar, Value = Obj.FromDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.VarChar, Value = Obj.ToDate });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getExportContainerIncome", CommandType.StoredProcedure, DParam);
            List<Ppg_ExportConIncomeDtl> lstExportConIncomeDtl = new List<Ppg_ExportConIncomeDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstExportConIncomeDtl.Add(new Ppg_ExportConIncomeDtl
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        PartyName = Result["PartyName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerType = Result["ContainerType"].ToString(),
                        Size = Result["Size"].ToString(),
                        MovementType = Result["MovementType"].ToString(),
                        CargoWeight = Convert.ToDecimal(Result["CargoWeight"]),
                        TareWeight = Convert.ToDecimal(Result["TareWeight"]),
                        TotalWeight = Convert.ToDecimal(Result["TotalWeight"]),
                        SLACode = Convert.ToString(Result["SLACode"]),
                        PortCode = Result["PortCode"].ToString(),
                        PortOfLoading = Result["PortOfLoading"].ToString(),
                        StuffingDate = Result["StuffingDate"].ToString(),
                        MovementDate = Result["MovementDate"].ToString(),
                        SealDate = Result["SealDate"].ToString(),
                        ShedNo = Result["ShedNo"].ToString(),
                        ShedArea = Result["ShedArea"].ToString(),
                        THC = Convert.ToDecimal(Result["THC"]),
                        HND = Convert.ToDecimal(Result["HND"]),
                        RR = Convert.ToDecimal(Result["RR"]),
                        FNC = Convert.ToDecimal(Result["FNC"]),
                        WHT = Convert.ToDecimal(Result["WHT"]),
                        GRL = Convert.ToDecimal(Result["GRL"]),
                        GRE = Convert.ToDecimal(Result["GRE"]),
                        MO = Convert.ToDecimal(Result["MO"]),
                        INS = Convert.ToDecimal(Result["INS"]),
                        GEN = Convert.ToDecimal(Result["GEN"]),
                        HAZ = Convert.ToDecimal(Result["HAZ"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        Total = Convert.ToDecimal(Result["Total"]),
                        FAC = Convert.ToDecimal(Result["FAC"])
                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstExportConIncomeDtl;
                }
                else
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

        public void ExportConIncomeDetailxls(Ppg_ExportConIncome Obj)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(Obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(Obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.VarChar, Value = Obj.FromDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.VarChar, Value = Obj.ToDate });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getExportContainerIncomeXls", CommandType.StoredProcedure, DParam);
            List<Ppg_ExportConIncomeDtlExcel> lstExportConIncomeDtl = new List<Ppg_ExportConIncomeDtlExcel>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstExportConIncomeDtl.Add(new Ppg_ExportConIncomeDtlExcel
                    {
                        SLNO = Convert.ToInt32(Result["SLNO"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        PartyName = Result["PartyName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerType = Result["ContainerType"].ToString(),
                        Size = Result["Size"].ToString(),
                        MovementType = Result["MovementType"].ToString(),
                        CargoWeight = Convert.ToDecimal(Result["CargoWeight"]),
                        TareWeight = Convert.ToDecimal(Result["TareWeight"]),
                        TotalWeight = Convert.ToDecimal(Result["TotalWeight"]),
                        SLACode = Convert.ToString(Result["SLACode"]),
                        PortCode = Result["PortCode"].ToString(),
                        PortOfLoading = Result["PortOfLoading"].ToString(),
                        StuffingDate = Result["StuffingDate"].ToString(),
                        MovementDate = Result["MovementDate"].ToString(),
                        SealDate = Result["SealDate"].ToString(),
                        ShedNo = Result["ShedNo"].ToString(),
                        ShedArea = Result["ShedArea"].ToString(),
                        Week = Convert.ToInt32(Result["week"]),
                        FOBValue = Convert.ToDecimal(Result["Fob"]),
                        HND = Convert.ToDecimal(Result["HND"]),
                        THC = Convert.ToDecimal(Result["THC"]),

                        RR = Convert.ToDecimal(Result["RR"]),
                        FNC = Convert.ToDecimal(Result["FNC"]),
                        WHT = Convert.ToDecimal(Result["WHT"]),
                        INS = Convert.ToDecimal(Result["INS"]),
                        GRE = Convert.ToDecimal(Result["GRE"]),
                        GRL = Convert.ToDecimal(Result["GRL"]),
                        GEN = Convert.ToDecimal(Result["GEN"]),
                        MO = Convert.ToDecimal(Result["MO"]),
                        HAZ = Convert.ToDecimal(Result["HAZ"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        Total = Convert.ToDecimal(Result["Total"])
                    });
                    DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    decimal InvoiceAmount = 0, CRAmount = 0;

                    _DBResponse.Data = ExpConIncomeDetail(lstExportConIncomeDtl, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"));


                }

            //    if (Status == 1)
            //    {
            //        _DBResponse.Status = 1;
            //        _DBResponse.Message = "Success";
            //        _DBResponse.Data = lstExportConIncomeDtl;
            //    }
            //    else
            //    {
            //        _DBResponse.Status = 0;
            //        _DBResponse.Message = "No Data";
            //        _DBResponse.Data = null;
            //    }
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






        private string ExpConIncomeDetail(List<Ppg_ExportConIncomeDtlExcel> PPGConIncomeDetail, string date1, string date2)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "(A Govt. of India Undertaking)"
                        + Environment.NewLine + "ICD Patparganj-Delhi"
                        + Environment.NewLine + Environment.NewLine
                        + "ExportContainer Income Detail";
                string typeOfValue = "";

                typeOfValue = "During Period Of " + date1 + " TO " + date2;



                exl.MargeCell("A1:H1", "CENTRAL WAREHOUSING CORPORATION", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A2:H2", "(A Govt. of India Undertaking)", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A3:H3", "ICD Patparganj-Delhi", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A4:H4", "Container Income Detail"+ typeOfValue, DynamicExcel.CellAlignment.Middle);
               /// exl.MargeCell("A5:H5", typeOfValue, DynamicExcel.CellAlignment.Middle);
               // exl.MargeCell("A4:F4", "DETAILS OF ORIGINAL INOICE", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("G4:O4", "DETAILS OF CREDIT NOTE ISSUED", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A5:A6", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B5:B6", "InvoiceNo", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C5:C6", "InvoiceDate", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D5:D6", "PARTY NAME", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E5:E6", "PARTY CODE.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F5:F6", "CONTAINER NO", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G5:G6", "ICD CODE", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("H5:H6", "SIZE.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I5:I6", "CONTAINER MOVEMENT TYPE(ByTrain / Road", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J5:J6", "CONTAINERTYPE(HAZ&NON-HAZ))", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K5:K6", "CARGO WEIGHT", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L5:L6", "TARE WEIGHT", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M5:M6", "TOTAL WEIGHT", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N5:N6", "SLA CODE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O5:O6", "PORT CODE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("P5:P6", "PORT OF LOADING ", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Q5:Q6", "STUFFING DATE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("R5:R6", "MOVEMENT DATE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("S5:S6", "SEAL DATE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("T5:T6", "SEAD NO", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("U5:U6", "SEAD AREA", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("V5:V6", "WEEK ", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("W5:W6", "FOB", DynamicExcel.CellAlignment.Middle);


                exl.MargeCell("X5:AG5", "INCOME CHARGES CODE", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("X6:X6", "HND", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Y6:Y6", "THC.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Z6:Z6", "RR", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AA6:AA6", "FNC", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AB6:AB6", "WEIGHMENT", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AC6:AC6", "INS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AD6:AD6", "GRE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AE6:AE6", "GE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AF6:AF6", "GRL", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AG6:AG6", "MOV", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AH6:AH6", "HAZ", DynamicExcel.CellAlignment.Middle);


                exl.MargeCell("AI5:AK5", "TAXES", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AI6:AI6", "CGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AJ6:AJ6", "SGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AK6:AK6", "IGST", DynamicExcel.CellAlignment.Middle);


                exl.MargeCell("AL5:AL6", "Total", DynamicExcel.CellAlignment.Middle);


                //for (var i = 65; i < 86; i++)
                //{
                //    char character = (char)i;
                //    string text = character.ToString();
                //    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                //}

                exl.AddTable("A", 7, PPGConIncomeDetail, new[] { 6, 20, 20, 30, 12, 20, 20, 15, 15, 30, 15, 20, 20, 20, 15, 15, 15, 10, 12, 18, 18, 16, 12, 12, 15, 15, 17, 18, 15, 14, 15, 16, 16, 15, 15, 15, 15, 15, 15 });
                var TaxAmt = PPGConIncomeDetail.Sum(o => o.Total);

                var igstamt = PPGConIncomeDetail.Sum(o => o.IGST);
                var sgstamt = PPGConIncomeDetail.Sum(o => o.SGST);
                var cgstamt = PPGConIncomeDetail.Sum(o => o.CGST);
                //var Area = PPGAssesmentSheetlcl.Sum(o => o.Area);
                //var Weight = PPGAssesmentSheetfcl.Sum(o => o.GrossWt);
                var THC = PPGConIncomeDetail.Sum(o => o.THC);
                var HND = PPGConIncomeDetail.Sum(o => o.HND);


                var RR = PPGConIncomeDetail.Sum(o => o.RR);
                var WHT = PPGConIncomeDetail.Sum(o => o.WHT);
                //var INS = PPGConIncomeDetail.Sum(o => o.INS);
                var GRE = PPGConIncomeDetail.Sum(o => o.GRE);
                var GEN = PPGConIncomeDetail.Sum(o => o.GEN);
                var INS = PPGConIncomeDetail.Sum(o => o.INS);
                var MO = PPGConIncomeDetail.Sum(o => o.MO);
                var HAZ = PPGConIncomeDetail.Sum(o => o.HAZ);
                var GRL = PPGConIncomeDetail.Sum(o => o.GRL);
                var FNC = PPGConIncomeDetail.Sum(o => o.FNC);
                //var Total = PPGConIncomeDetail.Sum(o => o.Total);


                //var BOEValueDuty = PPGAssesmentSheetfcl.Sum(o => o.BOEValueDuty);
                exl.AddCell("W" + (PPGConIncomeDetail.Count + 7).ToString(), "Total", DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("X" + (PPGConIncomeDetail.Count + 7).ToString(), HND.ToString(), DynamicExcel.CellAlignment.TopLeft);

                //exl.AddCell("W" + (PPGAssesmentSheetlcl.Count + 7).ToString(), Area.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("Y" + (PPGConIncomeDetail.Count + 7).ToString(), THC.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("Z" + (PPGConIncomeDetail.Count + 7).ToString(), RR.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AA" + (PPGConIncomeDetail.Count + 7).ToString(), FNC.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AB" + (PPGConIncomeDetail.Count + 7).ToString(), WHT.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AC" + (PPGConIncomeDetail.Count + 7).ToString(), INS.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AD" + (PPGConIncomeDetail.Count + 7).ToString(), GRE.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("AD" + (PPGAssesmentSheetfcl.Count + 7).ToString(), Haz.ToString(), DynamicExcel.CellAlignment.TopLeft);

                exl.AddCell("AE" + (PPGConIncomeDetail.Count + 7).ToString(), GEN.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AF" + (PPGConIncomeDetail.Count + 7).ToString(), GRL.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AG" + (PPGConIncomeDetail.Count + 7).ToString(), MO.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AH" + (PPGConIncomeDetail.Count + 7).ToString(), HAZ.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AI" + (PPGConIncomeDetail.Count + 7).ToString(), cgstamt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AJ" + (PPGConIncomeDetail.Count + 7).ToString(), sgstamt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AK" + (PPGConIncomeDetail.Count + 7).ToString(), igstamt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("AL" + (PPGConIncomeDetail.Count + 7).ToString(), TaxAmt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                ///exl.AddCell("AM" + (PPGConIncomeDetail.Count + 7).ToString(), TaxAmt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("AM" + (PPGConIncomeDetail.Count + 7).ToString(),Total.ToString(), DynamicExcel.CellAlignment.TopLeft);

                exl.Save();
            }
            return excelFile;
        }

        #endregion

        #region Assessment sheet LCL Report
        public void AssessmentSheetLCLDetail(Ppg_AssessmentSheetLCL Obj)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(Obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(Obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getAssessmentSheetLCL", CommandType.StoredProcedure, DParam);
            List<Ppg_AssessmentSheetLCLDtl> lstImportConIncomeDtl = new List<Ppg_AssessmentSheetLCLDtl>();
            int i = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstImportConIncomeDtl.Add(new Ppg_AssessmentSheetLCLDtl
                    {
                        SLNO = i + 1,
                        // InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        ImporterName = Result["ImporterName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),
                        PayeeCode = Result["PayeeCode"].ToString(),
                        BOENo = Result["BOENo"].ToString(),
                        BOEValueDuty = Convert.ToDecimal(Result["BOEValueDuty"]),
                        Area = Convert.ToDecimal(Result["Area"]),
                        GrossWt = Convert.ToDecimal(Result["GrossWt"]),
                        Week = Convert.ToInt32(Result["Week"]),
                        CargoType = Result["CargoType"].ToString(),
                        ENT = Convert.ToDecimal(Result["ENT"]),
                        HND = Convert.ToDecimal(Result["HND"]),
                        STO = Convert.ToDecimal(Result["STO"]),
                        INS = Convert.ToDecimal(Result["INS"]),
                        OTI = Convert.ToDecimal(Result["OTI"]),
                        HAZ = Convert.ToDecimal(Result["HAZ"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        Total = Convert.ToDecimal(Result["Total"])
                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstImportConIncomeDtl;
                }
                else
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



        public void AssessmentSheetLCLDetailEXL(Ppg_AssessmentSheetLCL Obj)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(Obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(Obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getAssessmentSheetLCL", CommandType.StoredProcedure, DParam);
            List<Ppg_AssessmentSheetLCLDtlXls> lstImportConIncomeDtl = new List<Ppg_AssessmentSheetLCLDtlXls>();
            int i = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstImportConIncomeDtl.Add(new Ppg_AssessmentSheetLCLDtlXls
                    {
                        SLNO = Convert.ToInt32(Result["SlNo"]),
                        // InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        ImporterName = Result["ImporterName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),
                        //PayeeCode = Result["PayeeCode"].ToString(),
                        BOENo = Result["BOENo"].ToString(),
                        BOEValueDuty = Convert.ToDecimal(Result["BOEValueDuty"]),
                        Week = Convert.ToInt32(Result["Week"]),
                        Area = Convert.ToDecimal(Result["Area"]),

                        GrossWt = Convert.ToDecimal(Result["GrossWt"]),

                        CargoType = Result["CargoType"].ToString(),
                        ENT = Convert.ToDecimal(Result["ENT"]),
                        HND = Convert.ToDecimal(Result["HND"]),
                        STO = Convert.ToDecimal(Result["STO"]),
                        INS = Convert.ToDecimal(Result["INS"]),
                        OTI = Convert.ToDecimal(Result["OTI"]),
                        HAZ = Convert.ToDecimal(Result["HAZ"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        Total = Convert.ToDecimal(Result["Total"])
                    });
                    DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    decimal InvoiceAmount = 0, CRAmount = 0;

                    _DBResponse.Data = RegisterofAssesmentSheetLcl(lstImportConIncomeDtl, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"));

                }
            }

            //    if (Status == 1)
            //    {
            //        _DBResponse.Status = 1;
            //        _DBResponse.Message = "Success";
            //        _DBResponse.Data = lstImportConIncomeDtl;
            //    }
            //    else
            //    {
            //        _DBResponse.Status = 0;
            //        _DBResponse.Message = "No Data";
            //        _DBResponse.Data = null;
            //    }
            //}
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

        private string RegisterofAssesmentSheetLcl(List<Ppg_AssessmentSheetLCLDtlXls> PPGAssesmentSheetlcl, string date1, string date2)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "(A Govt. of India Undertaking)"
                        + Environment.NewLine + "ICD Patparganj-Delhi"
                        + Environment.NewLine + Environment.NewLine
                        + "Assessment Sheet LCL Report";
                string typeOfValue = "";

                typeOfValue = "During Period Of " + date1 + " TO " + date2;



                exl.MargeCell("A1:H1", "CENTRAL WAREHOUSING CORPORATION", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A2:H2", "(A Govt. of India Undertaking)", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A3:H3", "ICD Patparganj-Delhi", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A4:H4", "Assessment Sheet LCL Report", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A5:H5", typeOfValue, DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("A4:F4", "DETAILS OF ORIGINAL INOICE", DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("G4:O4", "DETAILS OF CREDIT NOTE ISSUED", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A5:A6", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B5:B6", "InvoiceNo", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C5:C6", "InvoiceDate", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D5:D6", "IMPORTER NAME", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E5:E6", "PAYEE NAME.", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("F5:F6", "BOE NO.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G5:G6", "BOEVALUE(INCLUDINGDUTY)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H5:H6", "NO.OF WEEK", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I5:I6", "AREA.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J5:J6", "GROSS WEIGHT", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K5:K6", "CARGO TYPE", DynamicExcel.CellAlignment.Middle);


                exl.MargeCell("L5:P5", "INCOME CHARGES CODE", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("L6:L6", "ENTRY CHARGES", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M6:M6", "HANDLING CHARGES", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N6:N6", "STORAGECHG", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O6:O6", "INSURANCE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("P6:P6", "OVERTIMECHG", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Q6:Q6", "HAZ CHARGES", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("R5:T5", "TAXES", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("R6:R6", "CGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("S6:S6", "SGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("T6:T6", "IGST", DynamicExcel.CellAlignment.Middle);


                exl.MargeCell("U5:U6", "Total", DynamicExcel.CellAlignment.Middle);


                //for (var i = 65; i < 86; i++)
                //{
                //    char character = (char)i;
                //    string text = character.ToString();
                //    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                //}

                exl.AddTable("A", 7, PPGAssesmentSheetlcl, new[] { 6, 20, 20, 30, 12, 20, 20, 15, 15, 15, 14, 12, 12, 8, 14, 10, 10, 10, 12, 18, 18, });
                var TaxAmt = PPGAssesmentSheetlcl.Sum(o => o.Total);

                var igstamt = PPGAssesmentSheetlcl.Sum(o => o.IGST);
                var sgstamt = PPGAssesmentSheetlcl.Sum(o => o.SGST);
                var cgstamt = PPGAssesmentSheetlcl.Sum(o => o.CGST);
                var Area = PPGAssesmentSheetlcl.Sum(o => o.Area);
                var Weight = PPGAssesmentSheetlcl.Sum(o => o.GrossWt);
                var ENTRYCHG = PPGAssesmentSheetlcl.Sum(o => o.ENT);
                var HANDLING = PPGAssesmentSheetlcl.Sum(o => o.HND);


                var Sto = PPGAssesmentSheetlcl.Sum(o => o.STO);
                var INS = PPGAssesmentSheetlcl.Sum(o => o.INS);
                var Oti = PPGAssesmentSheetlcl.Sum(o => o.OTI);
                var Haz = PPGAssesmentSheetlcl.Sum(o => o.HAZ);
                var BOEValueDuty = PPGAssesmentSheetlcl.Sum(o => o.BOEValueDuty);
                exl.AddCell("F" + (PPGAssesmentSheetlcl.Count + 7).ToString(), "Total", DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("G" + (PPGAssesmentSheetlcl.Count + 7).ToString(), BOEValueDuty.ToString(), DynamicExcel.CellAlignment.TopLeft);

                exl.AddCell("I" + (PPGAssesmentSheetlcl.Count + 7).ToString(), Area.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("J" + (PPGAssesmentSheetlcl.Count + 7).ToString(), Weight.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("L" + (PPGAssesmentSheetlcl.Count + 7).ToString(), ENTRYCHG.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("M" + (PPGAssesmentSheetlcl.Count + 7).ToString(), HANDLING.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("N" + (PPGAssesmentSheetlcl.Count + 7).ToString(), Sto.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("O" + (PPGAssesmentSheetlcl.Count + 7).ToString(), INS.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("P" + (PPGAssesmentSheetlcl.Count + 7).ToString(), Oti.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("Q" + (PPGAssesmentSheetlcl.Count + 7).ToString(), Haz.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("R" + (PPGAssesmentSheetlcl.Count + 7).ToString(), cgstamt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("S" + (PPGAssesmentSheetlcl.Count + 7).ToString(), sgstamt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("T" + (PPGAssesmentSheetlcl.Count + 7).ToString(), igstamt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("U" + (PPGAssesmentSheetlcl.Count + 7).ToString(), TaxAmt.ToString(), DynamicExcel.CellAlignment.TopLeft);


                exl.Save();
            }
            return excelFile;
        }


        #endregion

        #region Seal Closing Report
        public void SealClosingReport(Ppg_SealClosingReport ObjSealClosingReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjSealClosingReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjSealClosingReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            IDataReader Result = DataAccess.ExecuteDataReader("SealClosingReport", CommandType.StoredProcedure, DParam);
            IList<Ppg_SealClosingReport> LstSealClosingReport = new List<Ppg_SealClosingReport>();
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

                    LstSealClosingReport.Add(new Ppg_SealClosingReport
                    {


                        Date = Result["Date"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        Commodity = Result["Commodity"].ToString(),
                        LCLFCL = Result["LCLFCL"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        CHAOrPort = Result["CHA"].ToString(),
                        Weight = Result["StuffWeight"].ToString(),

                        value = Result["value"].ToString()

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSealClosingReport;
                }
                else
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


        #region Empty Container Payment Report
        public void EmptyContainerPayment(Ppg_EmptyConPayRpt Obj)
        {
            DateTime dtfrom = DateTime.ParseExact(Obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(Obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getEmptyContainerPaymentReport", CommandType.StoredProcedure, DParam);
            List<Ppg_EmptyConPayRptDtl> lstEmptyContDtl = new List<Ppg_EmptyConPayRptDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstEmptyContDtl.Add(new Ppg_EmptyConPayRptDtl
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        ImporterName = Result["ImporterName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        TotalDays = Convert.ToInt32(Result["TotalDays"]),
                        LOE = Convert.ToDecimal(Result["LOE"]),
                        GRE = Convert.ToDecimal(Result["GRE"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        Total = Convert.ToDecimal(Result["Total"])
                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstEmptyContDtl;
                }
                else
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

        #region Reserve Space Income Report
        public void ReserveSpaceIncomeReport(DateTime FromDate, DateTime ToDate)
        {

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = FromDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = ToDate });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetReserveSpaceIncomeReport", CommandType.StoredProcedure, DParam);
            List<ReserveSpaceReport> lstEmptyContDtl = new List<ReserveSpaceReport>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstEmptyContDtl.Add(new ReserveSpaceReport
                    {

                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        //SLNo =Convert.ToInt32(Result["SlNo"]),
                        TotalAMount = Convert.ToDecimal(Result["InvoiceAmt"].ToString()),
                        SGST = Convert.ToDecimal(Result["TotalSGST"].ToString()),
                        ReservationAmount = Convert.ToDecimal(Result["TotalTaxable"].ToString()),
                        Remarks = Result["Remarks"].ToString(),
                        Rate = Convert.ToDecimal(Result["Rent"].ToString()),
                        PartyName = Result["PartyName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString(),
                        Month = Result["Month"].ToString(),
                        AmountReceivable = Convert.ToDecimal(Result["OutstandingAmt"].ToString()),
                        Area = Convert.ToDecimal(Result["Area"].ToString()),
                        BillingDate = "", //Result["InvoiceDate"].ToString(),
                        BillingNo = "",//Result["InvoiceNo"].ToString(),
                        CGST = Convert.ToDecimal(Result["TotalCGST"].ToString()),
                        IGST = Convert.ToDecimal(Result["TotalIGST"].ToString())

                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstEmptyContDtl;
                }
                else
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

        #region Outstanding Amount Report
        public void OutstandingAmountReport(DateTime FromDate, DateTime ToDate)
        {

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = FromDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = ToDate });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GETOutstandingAmountReport", CommandType.StoredProcedure, DParam);
            List<Ppg_OutstandingAmountReport> lstEmptyContDtl = new List<Ppg_OutstandingAmountReport>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstEmptyContDtl.Add(new Ppg_OutstandingAmountReport
                    {

                        BillingNo = Result["InvoiceNo"].ToString(),
                        BillingDate = Result["InvoiceDate"].ToString(),
                        //SLNo =Convert.ToInt32(Result["SlNo"]),
                        AmountReceivalbe = Convert.ToDecimal(Result["GrossTotal"].ToString()),
                        TotalAmount = Convert.ToDecimal(Result["Amount"].ToString()),
                        SGST = Convert.ToDecimal(Result["SGSTAmt"].ToString()),
                        Remarks = Result["Remarks"].ToString(),
                        SQM = Convert.ToDecimal(Result["Rate"].ToString()),
                        PartyName = Result["PartyName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString(),
                        Month = Result["Month"].ToString(),
                        Area = Convert.ToDecimal(Result["Area"].ToString()),
                        CGST = Convert.ToDecimal(Result["CGSTAmt"].ToString()),
                        IGST = Convert.ToDecimal(Result["IGSTAmt"].ToString())

                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstEmptyContDtl;
                }
                else
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


        #region Assessment sheet FCL Report
        public void AssessmentSheetFCLDetail(Ppg_AssessmentSheetFCL Obj)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(Obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(Obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAssessmentSheetFCL", CommandType.StoredProcedure, DParam);
            List<Ppg_AssessmentSheetFCLDtl> lstImportConIncomeDtl = new List<Ppg_AssessmentSheetFCLDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstImportConIncomeDtl.Add(new Ppg_AssessmentSheetFCLDtl
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        ImporterName = Result["ImporterName"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),
                        //PayeeCode = Result["PayeeCode"].ToString(),
                        BOENo = Result["BOENo"].ToString(),
                        BOEValueDuty = Convert.ToDecimal(Result["BEOValue"]),
                        //Area = Convert.ToDecimal(Result["Area"]),
                        GrossWt = Convert.ToDecimal(Result["TotalGrossWt"]),
                        Days = Convert.ToInt32(Result["Days"]),
                        CargoType = Result["CargoType"].ToString(),
                        ENT = Convert.ToDecimal(Result["ENT"]),
                        GRL = Convert.ToDecimal(Result["GRL"]),
                        MF = Convert.ToDecimal(Result["MF"]),
                        INS = Convert.ToDecimal(Result["INS"]),
                        OTI = Convert.ToDecimal(Result["OTI"]),
                        HAZ = Convert.ToDecimal(Result["HAZ"]),
                        TFU = Convert.ToDecimal(Result["TFU"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        Total = Convert.ToDecimal(Result["Total"])
                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstImportConIncomeDtl;
                }
                else
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



        public void AssessmentSheetFCLDetailExcel(Ppg_AssessmentSheetFCL Obj)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(Obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(Obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAssessmentSheetFCL", CommandType.StoredProcedure, DParam);
            List<Ppg_AssessmentSheetFCLDtlExcel> lstImportConIncomeDtl = new List<Ppg_AssessmentSheetFCLDtlExcel>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstImportConIncomeDtl.Add(new Ppg_AssessmentSheetFCLDtlExcel
                    {
                        SLNO = Convert.ToInt32(Result["SLNO"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        ImporterName = Result["ImporterName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),

                        //PayeeCode = Result["PayeeCode"].ToString(),
                        BOENo = Result["BOENo"].ToString(),
                        BOEValueDuty = Convert.ToDecimal(Result["BEOValue"]),
                        Days = Convert.ToInt32(Result["Days"]),
                        //Area = Convert.ToDecimal(Result["Area"]),
                        GrossWt = Convert.ToDecimal(Result["TotalGrossWt"]),

                        CargoType = Result["CargoType"].ToString(),
                        ENT = Convert.ToDecimal(Result["ENT"]),
                        GRL = Convert.ToDecimal(Result["GRL"]),

                        INS = Convert.ToDecimal(Result["INS"]),
                        OTI = Convert.ToDecimal(Result["OTI"]),
                        MF = Convert.ToDecimal(Result["MF"]),
                        HAZ = Convert.ToDecimal(Result["HAZ"]),
                        TFU = Convert.ToDecimal(Result["TFU"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        Total = Convert.ToDecimal(Result["Total"])
                    });
                    DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    decimal InvoiceAmount = 0, CRAmount = 0;

                    _DBResponse.Data = AssesmentSheetFCL(lstImportConIncomeDtl, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"));



                }

                //if (Status == 1)
                //{
                //    _DBResponse.Status = 1;
                //    _DBResponse.Message = "Success";
                //    _DBResponse.Data = lstImportConIncomeDtl;
                //}
                //else
                //{
                //    _DBResponse.Status = 0;
                //    _DBResponse.Message = "No Data";
                //    _DBResponse.Data = null;
                //}
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








        private string AssesmentSheetFCL(List<Ppg_AssessmentSheetFCLDtlExcel> PPGAssesmentSheetfcl, string date1, string date2)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "(A Govt. of India Undertaking)"
                        + Environment.NewLine + "ICD Patparganj-Delhi"
                        + Environment.NewLine + Environment.NewLine
                        + "Assessment Sheet FCL Report";
                string typeOfValue = "";

                typeOfValue = "During Period Of " + date1 + " TO " + date2;



                exl.MargeCell("A1:H1", "CENTRAL WAREHOUSING CORPORATION", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A2:H2", "(A Govt. of India Undertaking)", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A3:H3", "ICD Patparganj-Delhi", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A4:H4", "Assessment Sheet FCL Report", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A5:H5", typeOfValue, DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("A4:F4", "DETAILS OF ORIGINAL INOICE", DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("G4:O4", "DETAILS OF CREDIT NOTE ISSUED", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A5:A6", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B5:B6", "InvoiceNo", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C5:C6", "InvoiceDate", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D5:D6", "IMPORTER NAME", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E5:E6", "PAYEE NAME.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F5:F6", "CONTAINER NO", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G5:G6", "CONTAINER SIZE", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("H5:H6", "BOE NO.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I5:I6", "BOEVALUE(INCLUDINGDUTY)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J5:J6", "NO.OF Days", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("5:I6", "AREA.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K5:K6", "GROSS WEIGHT", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L5:L6", "CARGO TYPE", DynamicExcel.CellAlignment.Middle);


                exl.MargeCell("M5:Q5", "INCOME CHARGES CODE", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("M6:M6", "ENTRY CHARGES", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N6:N6", "GRLCHG.", DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("P6:P6", "STORAGECHG", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O6:O6", "INSURANCE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("P6:P6", "OVERTIMECHG", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Q6:Q6", "MF CHARGES", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("R6:R6", "HAZ CHARGES", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("S6:S6", "TFU CHARGES", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("S5:U5", "TAXES", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("T6:T6", "CGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("U6:U6", "SGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("V6:V6", "IGST", DynamicExcel.CellAlignment.Middle);


                exl.MargeCell("W5:W6", "Total", DynamicExcel.CellAlignment.Middle);


                //for (var i = 65; i < 86; i++)
                //{
                //    char character = (char)i;
                //    string text = character.ToString();
                //    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                //}

                exl.AddTable("A", 7, PPGAssesmentSheetfcl, new[] { 6, 20, 20, 30, 12, 20, 20, 15, 15, 15, 14, 12, 12, 8, 14, 10, 10, 10, 12, 18, 18, 16,16 });
                var TaxAmt = PPGAssesmentSheetfcl.Sum(o => o.Total);

                var igstamt = PPGAssesmentSheetfcl.Sum(o => o.IGST);
                var sgstamt = PPGAssesmentSheetfcl.Sum(o => o.SGST);
                var cgstamt = PPGAssesmentSheetfcl.Sum(o => o.CGST);
                //var Area = PPGAssesmentSheetlcl.Sum(o => o.Area);
                var Weight = PPGAssesmentSheetfcl.Sum(o => o.GrossWt);
                var ENTRYCHG = PPGAssesmentSheetfcl.Sum(o => o.ENT);
                var GRL = PPGAssesmentSheetfcl.Sum(o => o.GRL);


                var MF = PPGAssesmentSheetfcl.Sum(o => o.MF);
                var INS = PPGAssesmentSheetfcl.Sum(o => o.INS);
                var Oti = PPGAssesmentSheetfcl.Sum(o => o.OTI);
                var Haz = PPGAssesmentSheetfcl.Sum(o => o.HAZ);
                var TFU = PPGAssesmentSheetfcl.Sum(o => o.TFU);
                var BOEValueDuty = PPGAssesmentSheetfcl.Sum(o => o.BOEValueDuty);
                exl.AddCell("H" + (PPGAssesmentSheetfcl.Count + 7).ToString(), "Total", DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("I" + (PPGAssesmentSheetfcl.Count + 7).ToString(), BOEValueDuty.ToString(), DynamicExcel.CellAlignment.TopLeft);

                //exl.AddCell("I" + (PPGAssesmentSheetlcl.Count + 7).ToString(), Area.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("K" + (PPGAssesmentSheetfcl.Count + 7).ToString(), Weight.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("M" + (PPGAssesmentSheetfcl.Count + 7).ToString(), ENTRYCHG.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("N" + (PPGAssesmentSheetfcl.Count + 7).ToString(), GRL.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("O" + (PPGAssesmentSheetfcl.Count + 7).ToString(), INS.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("P" + (PPGAssesmentSheetfcl.Count + 7).ToString(), Oti.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("Q" + (PPGAssesmentSheetfcl.Count + 7).ToString(), MF.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("R" + (PPGAssesmentSheetfcl.Count + 7).ToString(), Haz.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("S" + (PPGAssesmentSheetfcl.Count + 7).ToString(), TFU.ToString(), DynamicExcel.CellAlignment.TopLeft);
                // exl.AddCell("S" + (PPGAssesmentSheetfcl.Count + 7).ToString(), Haz.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("T" + (PPGAssesmentSheetfcl.Count + 7).ToString(), cgstamt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("U" + (PPGAssesmentSheetfcl.Count + 7).ToString(), sgstamt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("V" + (PPGAssesmentSheetfcl.Count + 7).ToString(), igstamt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                exl.AddCell("W" + (PPGAssesmentSheetfcl.Count + 7).ToString(), TaxAmt.ToString(), DynamicExcel.CellAlignment.TopLeft);


                exl.Save();
            }
            return excelFile;
        }
        #endregion


        #region Rent Income Report
        public void RentIncomeReport(Ppg_RentIncomeReportViewModel Obj)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(Obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(Obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetRentIncomeReport", CommandType.StoredProcedure, DParam);
            List<Ppg_RentIncomeReportViewModel> lstImportConIncomeDtl = new List<Ppg_RentIncomeReportViewModel>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstImportConIncomeDtl.Add(new Ppg_RentIncomeReportViewModel
                    {
                        //i = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        PartyName = Result["EximTraderName"].ToString(),
                        PartyCode = Result["EximTraderAlias"].ToString(),
                        Month = Result["Month"].ToString(),
                        TDSCertification = Result["CirtificateNo"].ToString(),
                        RentReceived = Convert.ToDecimal(Result["Taxable"]),
                        TDSAmount = Convert.ToDecimal(Result["TDSAmount"]),
                        SGST = Convert.ToDecimal(Result["SGSTAmt"]),
                        CGST = Convert.ToDecimal(Result["CGSTAmt"]),
                        IGST = Convert.ToDecimal(Result["IGSTAmt"]),
                        TotalAmountReceived = Convert.ToDecimal(Result["Total"]),
                        AmountOutstanding = (Convert.ToDecimal(Result["Total"]) - Convert.ToDecimal(Result["ReceivedAmount"])),
                        Remarks = Result["Remarks"].ToString()

                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstImportConIncomeDtl;
                }
                else
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


        #region Cargo Stock Register
        public void CargoStockRegister(Ppg_CrgStkRgt ObjCargoStockRegister)
        {
            DateTime dtfrom = DateTime.ParseExact(ObjCargoStockRegister.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjCargoStockRegister.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("CargoStockRegister", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Ppg_CrgStkRgt _ObjCargoStockRegister = new Ppg_CrgStkRgt();
            IList<ppgexportCargoStock> ppgexportCargoStocklst = new List<ppgexportCargoStock>();

            IList<ppgimportCargoStock> ppgimportCargoStocklst = new List<ppgimportCargoStock>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    _ObjCargoStockRegister.ppgexportCargoStocklst.Add(new ppgexportCargoStock
                    {
                        shippingBillNo = Result["ShippingBillNo"].ToString().Trim(),
                        Date = (Result["RegisterDate"] == null ? "" : Result["RegisterDate"]).ToString(),
                        NoOfPackage = (Result["NoOfUnits"] == null ? "" : Result["NoOfUnits"]).ToString(),
                        Commodity = (Result["CommodityName"] == null ? "" : Result["CommodityName"]).ToString(),
                        Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString(),

                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        _ObjCargoStockRegister.ppgimportCargoStocklst.Add(new ppgimportCargoStock
                        {
                            BOE = Result["BOENo"].ToString(),
                            Date = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            NoOfPackage = (Result["NoOfUnits"] == null ? "" : Result["NoOfUnits"]).ToString(),
                            Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString(),

                        });
                    }



                }

                /*  if (Result.NextResult())
                  {
                      while (Result.Read())
                      {
                          Status = 1;
                          _ObjCargoStockRegister.bondCargoStocklst.Add(new bondCargoStock
                          {
                              Warehouse = Result["WRNo"].ToString(),
                              Date = (Result["WRDate"] == null ? "" : Result["WRDate"]).ToString(),
                              NoOfPackage = (Result["NoOfUnits"] == null ? "" : Result["NoOfUnits"]).ToString(),
                              Commodity = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                              Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString(),

                          });
                      }



                  }*/
                if (Status == 1)
                {
                    _ObjCargoStockRegister.ppgexportCargoStocklst.ToList().ForEach(m =>
                    {
                        if (m.shippingBillNo == "/")
                        {
                            m.shippingBillNo = "";
                        }

                    });

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = _ObjCargoStockRegister;
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


        #region Tax (0) Invoice Report
        public void TaxZeroInvoiceReport(DailyCashBookPpg ObjDailyCashBook)
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
            IDataReader Result = DataAccess.ExecuteDataReader("TaxZeroInvoiceReport", CommandType.StoredProcedure, DParam);
            IList<DailyCashBookPpg> LstDailyCashBook = new List<DailyCashBookPpg>();
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

                    LstDailyCashBook.Add(new DailyCashBookPpg
                    {

                        //ReceiptNo, ReceiptDate, Party, ChequeNo, CashReceiptId, GenSpace, sto, Insurance, GroundRentEmpty, GroundRentLoaded, Mf, EntCharge, 
                        //Fum, OtCharge, CGSTAmt, SGSTAmt, IGSTAmt, MISC, MiscExcess, TotalCash, TotalCheque, tdsCol, crTDS
                        /*CRNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),

                        Depositor = Result["Party"].ToString(),*/
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

        #endregion

        #region PDUtilizationSummary
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


                LstPdSummaryReport.Add(new PdSummary
                {



                    PartyName = "Total",

                    Amount = LstPdSummaryReport.Sum(x => Convert.ToDecimal(x.Amount)).ToString()

                    //ContainerNo = Result["ContainerNo"].ToString(),
                    //value = Result["value"].ToString()

                });

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

        #region Export THC RR
        public void ExportTHCRRReport(Ppg_ThcRrReport vm)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(vm.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(vm.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetTHCRRReport", CommandType.StoredProcedure, DParam);
            List<Ppg_ThcRrReport> lstExportTHCRRdtl = new List<Ppg_ThcRrReport>();

            _DBResponse = new DatabaseResponse();
            try
            {

                while (Result.Read())
                {
                    Status = 1;

                    lstExportTHCRRdtl.Add(new Ppg_ThcRrReport
                    {

                        Date = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLinaName"]),
                        PayeeCode = Convert.ToString(Result["EximTraderAlias"]),
                        DestinationPort = Convert.ToString(Result["PortName"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerSize = Convert.ToString(Result["Size"]),
                        TotalWeight = Convert.ToDecimal(Result["TotalGrossWt"]),
                        TareWeight = Convert.ToDecimal(Result["TareWeight"]),
                        RRAmount = Convert.ToDecimal(Result["RRAmount"]),
                        FACAmount = Convert.ToDecimal(Result["FACAmount"]),

                        TotalRRGstAmount = Convert.ToDecimal(Result["RRTotalGSTAmt"]),
                        THCAmount = Convert.ToDecimal(Result["THCAmount"]),
                        TotalTHGstAmount = Convert.ToDecimal(Result["THCTotalGSTAmt"]),
                        TotalFACGstAmount = Convert.ToDecimal(Result["FACTotalGSTAmt"]),

                        GrandTotal = Convert.ToDecimal(Result["Total"])
                    });

                }


                lstExportTHCRRdtl.Add(new Ppg_ThcRrReport
                {

                    Date = "Total",
                    InvoiceNo = "",
                    ShippingLineName = "",
                    PayeeCode = "",
                    DestinationPort = "",
                    ContainerNo = "",
                    CFSCode = "",
                    ContainerSize = "",
                    TotalWeight = lstExportTHCRRdtl.Sum(x => x.TotalWeight),
                    TareWeight = lstExportTHCRRdtl.Sum(x => x.TareWeight),
                    RRAmount = lstExportTHCRRdtl.Sum(x => x.RRAmount),
                    FACAmount = lstExportTHCRRdtl.Sum(x => x.FACAmount),

                    TotalRRGstAmount = lstExportTHCRRdtl.Sum(x => x.TotalRRGstAmount),
                    THCAmount = lstExportTHCRRdtl.Sum(x => x.THCAmount),
                    TotalTHGstAmount = lstExportTHCRRdtl.Sum(x => x.TotalTHGstAmount),
                    TotalFACGstAmount = lstExportTHCRRdtl.Sum(x => x.TotalFACGstAmount),

                    GrandTotal = lstExportTHCRRdtl.Sum(x => x.GrandTotal)

                });
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstExportTHCRRdtl;
                }
                else
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


        #region Import THC RR
        public void ImportTHCRRReport(Ppg_ThcRrReport vm)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(vm.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(vm.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImportTHCRRReport", CommandType.StoredProcedure, DParam);
            List<Ppg_ThcRrReport> lstExportTHCRRdtl = new List<Ppg_ThcRrReport>();

            _DBResponse = new DatabaseResponse();
            try
            {

                while (Result.Read())
                {
                    Status = 1;

                    lstExportTHCRRdtl.Add(new Ppg_ThcRrReport
                    {

                        Date = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLinaName"]),
                        PayeeCode = Convert.ToString(Result["EximTraderAlias"]),
                        DestinationPort = Convert.ToString(Result["PortName"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerSize = Convert.ToString(Result["Size"]),
                        TrainNo = Convert.ToString(Result["TrainNo"]),
                        TrainDate = Convert.ToString(Result["TrainDate"]),
                        TotalWeight = Convert.ToDecimal(Result["Gross_Wt"]),
                        TareWeight = Convert.ToDecimal(Result["Ct_Tare"]),
                        RRAmount = Convert.ToDecimal(Result["RRAmount"]),
                        FACAmount = Convert.ToDecimal(Result["FACAmount"]),

                        TotalRRGstAmount = Convert.ToDecimal(Result["RRTotalGSTAmt"]),
                        THCAmount = Convert.ToDecimal(Result["THCAmount"]),
                        TotalTHGstAmount = Convert.ToDecimal(Result["THCTotalGSTAmt"]),
                        TFUAmount = Convert.ToDecimal(Result["TFUAmount"]),
                        TotalTFUGstAmount = Convert.ToDecimal(Result["TFUTotalGSTAmt"]),
                        TotalFACGstAmount = Convert.ToDecimal(Result["FACTotalGSTAmt"]),

                        GrandTotal = Convert.ToDecimal(Result["Total"])
                    });

                }


                lstExportTHCRRdtl.Add(new Ppg_ThcRrReport
                {

                    Date = "Total",
                    InvoiceNo = "",
                    ShippingLineName = "",
                    PayeeCode = "",
                    DestinationPort = "",
                    ContainerNo = "",
                    CFSCode = "",
                    ContainerSize = "",
                    TrainNo = "",
                    TrainDate = "",
                    TareWeight = lstExportTHCRRdtl.Sum(x => x.TareWeight),
                    TotalWeight = lstExportTHCRRdtl.Sum(x => x.TotalWeight),
                    RRAmount = lstExportTHCRRdtl.Sum(x => x.RRAmount),
                    FACAmount = lstExportTHCRRdtl.Sum(x => x.FACAmount),

                    TotalRRGstAmount = lstExportTHCRRdtl.Sum(x => x.TotalRRGstAmount),
                    THCAmount = lstExportTHCRRdtl.Sum(x => x.THCAmount),
                    TotalTHGstAmount = lstExportTHCRRdtl.Sum(x => x.TotalTHGstAmount),
                    TFUAmount = lstExportTHCRRdtl.Sum(x => x.TFUAmount),
                    TotalTFUGstAmount = lstExportTHCRRdtl.Sum(x => x.TotalTFUGstAmount),
                    TotalFACGstAmount = lstExportTHCRRdtl.Sum(x => x.TotalFACGstAmount),

                    GrandTotal = lstExportTHCRRdtl.Sum(x => x.GrandTotal)

                });
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstExportTHCRRdtl;
                }
                else
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

        #region Long Standing Report For Cargo Export
        public void GetLongStandingExportLoadedCargo(Ppg_LongStandingExportCargo vm)
        {


            DateTime dtTo = DateTime.ParseExact(vm.AsOnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = vm.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_days", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = vm.days });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLongStandingExportCargo", CommandType.StoredProcedure, DParam);
            IList<Ppg_LongStandingExportCargoDtl> lstLongStandingExportCargoDetails = new List<Ppg_LongStandingExportCargoDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    lstLongStandingExportCargoDetails.Add(new Ppg_LongStandingExportCargoDtl
                    {
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
                        ShippingBillNoDate = Result["ShippingBillDate"].ToString(),
                        ShippingLineCode = Result["ShippingCode"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        StuffingDate = Result["StuffingDate"].ToString(),
                        CCINInvoiceDate = Result["InvoiceNo"].ToString(),
                        CCINInvoiceNo = Result["InvoiceDate"].ToString(),
                        ExporterName = Result["ExporterName"].ToString(),
                        ExporterAddress = Result["ExporterAddress"].ToString(),
                        StroageCharges = Convert.ToDecimal(Result["STO"]),
                        ChaName = Result["ChaName"].ToString(),
                        InDate = Result["EntryDate"].ToString(),
                        Fob = Convert.ToDecimal(Result["Fob"]),
                        NoOfPkg = Convert.ToInt32(Result["NoofPkg"].ToString()),
                        GrWt = Convert.ToDecimal(Result["Weight"].ToString()),
                        Area = Convert.ToDecimal(Result["Area"].ToString()),
                        Commodity = Result["CommodityName"].ToString(),
                        GH = Result["SpaceType"].ToString(),
                        Notice1 = Result["Notice1"].ToString(),
                        Notice2 = Result["Notice2"].ToString(),
                        Date1 = Result["Date1"].ToString(),
                        Date2 = Result["Date2"].ToString(),
                        Nocr = Result["SeizeDate"].ToString(),
                        SeizeDate = Result["Nocr"].ToString(),
                        Remarks1 = Result["Remarks1"].ToString(),
                        Remarks2 = Result["Remarks2"].ToString(),
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstLongStandingExportCargoDetails;
                }
                else
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




        #region Generating CashBookWithSD
        public void GetCashBookWithSD(DateTime date1, DateTime date2)
        {
            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = date2 });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //var objRegOutwardSupply = (List<RegisterOfOutwardSupplyModel>)DataAccess.ExecuteDynamicSet<List<RegisterOfOutwardSupplyModel>>("GetRegisterofOutwardSupply", DParam);
            DataSet ds = DataAccess.ExecuteDataSet("DailyCashBookReport", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];

            List<Ppg_CashBookWithSDExcel> model = new List<Ppg_CashBookWithSDExcel>();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    model = (from DataRow dr in dt.Rows
                             select new Ppg_CashBookWithSDExcel()
                             {
                                 // SlNo = Convert.ToInt32(dr["SlNo"]),


                                 InvoiceDate = dr["InvoiceDate"].ToString(),
                                 InvoiceNo = dr["InvoiceNo"].ToString(),
                                 InvoiceType = dr["InvoiceType"].ToString(),
                                 PartyName = dr["PartyName"].ToString(),
                                 PayeeName = dr["PayeeName"].ToString(),
                                 ModeOfPay = (dr["ModeOfPay"]).ToString(),
                                 ChequeNo = dr["ChequeNo"].ToString(),
                                 GenSpace = Convert.ToDecimal(dr["GenSpace"]),
                                 sto = Convert.ToDecimal(dr["sto"]),
                                 Insurance = Convert.ToDecimal(dr["Insurance"]),
                                 GroundRentLoaded = Convert.ToDecimal(dr["GroundRentLoaded"]),
                                 GroundRentEmpty = Convert.ToDecimal(dr["GroundRentEmpty"]),
                                 Mf = Convert.ToDecimal(dr["Mf"]),
                                 EntCharge = Convert.ToDecimal(dr["EntCharge"]),
                                 Fum = Convert.ToDecimal(dr["Fum"]),

                                 OtCharge = Convert.ToDecimal(dr["OtCharge"]),
                                 MISC = Convert.ToDecimal(dr["MISC"]),

                                 CGSTAmt = Convert.ToDecimal(dr["CGSTAmt"]),
                                 SGSTAmt = Convert.ToDecimal(dr["SGSTAmt"]),
                                 IGSTAmt = Convert.ToDecimal(dr["IGSTAmt"]),
                                 MiscExcess = Convert.ToDecimal(dr["MiscExcess"]),
                                 TotalCash = Convert.ToDecimal(dr["TotalCash"]),
                                 TotalCheque = Convert.ToDecimal(dr["TotalCheque"]),
                                 TotalOther = Convert.ToDecimal(dr["TotalOther"]),
                                 TotalPDA = Convert.ToDecimal(dr["TotalPDA"]),
                                 tdsCol = Convert.ToDecimal(dr["tdsCol"]),
                                 crTDS = Convert.ToDecimal(dr["crTDS"]),

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
                _DBResponse.Data = CashBookWithSDExcelExcel(model, InvoiceAmount, CRAmount);
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
        private string CashBookWithSDExcelExcel(List<Ppg_CashBookWithSDExcel> model, decimal InvoiceAmount, decimal CRAmount)
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
                        + "REGISTER OF CashBook With SD";
                exl.MargeCell("A1:M1", title, DynamicExcel.CellAlignment.Middle);

                //   exl.MargeCell("A2:A4", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("A2", "InvoiceDate", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("B2", "InvoiceNo", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("C2", "InvoiceType", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("D2", "PartyName", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("E2", "PayeeName", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("F2", "ModeOfPay", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("F2", "ChequeNo" + Environment.NewLine + "Bag/MT/Sqm", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("H2", "GenSpace", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("I2", "Sto", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("J2", "Insurance", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("K2", "GroundRentEmpty" + Environment.NewLine + "(Before Tax)", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("L2", "GroundRentLoaded", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("M2", "Mf", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("N2", "EntCharge", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("O2", "Fum", DynamicExcel.CellAlignment.Middle);
                //exl.AddCell("L4", "%", DynamicExcel.CellAlignment.Middle);
                //exl.AddCell("M4", "Amount", DynamicExcel.CellAlignment.Middle);
                //exl.AddCell("N4", "%", DynamicExcel.CellAlignment.Middle);
                //exl.AddCell("O4", "Amount", DynamicExcel.CellAlignment.Middle);
                //exl.AddCell("P4", "%", DynamicExcel.CellAlignment.Middle);
                //exl.AddCell("Q4", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("P2", "OtCharge", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("Q2", "MISC", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("R2", "CGSTAmt", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("S2", "SGSTAmt", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("T2", "IGSTAmt", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("U2", "MiscExcess", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("V2", "TotalCash", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("W2", "TotalCheque", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("X2", "TotalOther", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("Y2", "TotalPDA", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("Z2", "tdsCol", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("AA2", "crTDS", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("AB2", "Remarks", DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("T2:T4", "Remarks", DynamicExcel.CellAlignment.Middle);


                //for (var i = 65; i < 90; i++)
                //{
                //    char character = (char)i;
                //    string text = character.ToString();
                //    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                //}
                /*exl.AddTable<InvoiceData>("A", 6, model.InvoiceData, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16 });
                exl.AddTable<CashReceiptData>("R", 6, model.CashReceiptData, new[] { 12, 30, 14, 14, 14, 14, 14, 40 });/
                exl.AddTable<PpgRegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16, 12, 30, 14, 14, 14, 14, 14, 40 });*/
                exl.AddTable<Ppg_CashBookWithSDExcel>("A", 3, model, new[] { 6, 20, 20, 20, 12, 20, 10, 15, 20, 12, 12, 8, 14, 8, 14, 8, 14, 16, 10, 30, 14, 40, 14, 40, 14, 14, 12, 20, });
                var GenSpace = model.Sum(o => o.GenSpace);

                var sto = model.Sum(o => o.sto);
                var Insurance = model.Sum(o => o.Insurance);
                var GroundRentEmpty = model.Sum(o => o.GroundRentEmpty);
                var GroundRentLoaded = model.Sum(o => o.GroundRentLoaded);
                var Mf = model.Sum(o => o.Mf);
                var EntCharge = model.Sum(o => o.EntCharge);
                var Fum = model.Sum(o => o.Fum);
                var OtCharge = model.Sum(o => o.OtCharge);
                var MISC = model.Sum(o => o.MISC);
                var CGSTAmt = model.Sum(o => o.CGSTAmt);
                var SGSTAmt = model.Sum(o => o.SGSTAmt);
                var IGSTAmt = model.Sum(o => o.IGSTAmt);
                var MiscExcess = model.Sum(o => o.MiscExcess);
                var TotalCash = model.Sum(o => o.TotalCash);
                var TotalCheque = model.Sum(o => o.TotalCheque);
                var TotalOther = model.Sum(o => o.TotalOther);

                var TotalPDA = model.Sum(o => o.TotalPDA);
                var crTDS = model.Sum(o => o.crTDS);
                //var TotalCash = model.Sum(o => o.TotalCash);

                exl.AddCell("H" + (model.Count + 6).ToString(), GenSpace.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("I" + (model.Count + 6).ToString(), sto.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("J" + (model.Count + 6).ToString(), Insurance.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("K" + (model.Count + 6).ToString(), GroundRentEmpty.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("L" + (model.Count + 6).ToString(), GroundRentLoaded.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("M" + (model.Count + 6).ToString(), Mf.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("N" + (model.Count + 6).ToString(), EntCharge.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 6).ToString(), Fum.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 6).ToString(), OtCharge.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("Q" + (model.Count + 6).ToString(), MISC.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("R" + (model.Count + 6).ToString(), CGSTAmt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("S" + (model.Count + 6).ToString(), SGSTAmt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("T" + (model.Count + 6).ToString(), IGSTAmt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("U" + (model.Count + 6).ToString(), MiscExcess.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("V" + (model.Count + 6).ToString(), TotalCash.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("W" + (model.Count + 6).ToString(), TotalCheque.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("X" + (model.Count + 6).ToString(), TotalOther.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("Y" + (model.Count + 6).ToString(), TotalPDA.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("Z" + (model.Count + 6).ToString(), crTDS.ToString(), DynamicExcel.CellAlignment.CenterRight);
                //  exl.AddCell("Z" + (model.Count + 1).ToString(), totalamt.ToString(), DynamicExcel.CellAlignment.CenterRight);



                /*exl.AddCell("O" + (model.Count + 7).ToString(), "Invoice Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 7).ToString(), InvoiceAmount, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 8).ToString(), "Cash Receipt Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 8).ToString(), CRAmount, DynamicExcel.CellAlignment.CenterRight);*/

                exl.Save();
            }
            return excelFile;
        }

        //private string RegisterofOutwardSupplyExcelCreditDebit(List<PpgRegisterOfOutwardSupplyModelCreditDebit> modelCreditDebit, decimal InvoiceAmount, decimal CRAmount)
        //{
        //    if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
        //    {
        //        System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
        //    }
        //    var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
        //    using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
        //    {
        //        var title = @"CENTRAL WAREHOUSING CORPORATION"
        //                + Environment.NewLine + "Principal Place of Business"
        //                + Environment.NewLine + "CENTRAL WAREHOUSE"
        //                + Environment.NewLine + Environment.NewLine
        //                + "REGISTER OF OUTWARD SUPPLY (TAX INVOICE/BILL OF SUPPLY)";
        //        exl.MargeCell("A1:M1", title, DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("N1:O1", "BILL REGISTER", DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("A2:A4", "Sl.No.", DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("B2:B4", "GSTIN", DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("C2:C4", "Place" + Environment.NewLine + "(Name of State)", DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("D2:D4", "Name of Customer to whom Service rendered", DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("E2:E4", "Period of Invoice", DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("F2:F4", "Nature of Invoice" + Environment.NewLine + "(Resv./Initial Fumigatiom/General Basic/Over & Above)", DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("G2:G4", "HSN Code", DynamicExcel.CellAlignment.Middle);

        //        exl.MargeCell("H2:H4", "Rate per" + Environment.NewLine + "Bag/MT/Sqm", DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("I2:I4", "Credit / Debit Note No", DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("J2:J4", "Credit / Debit Note Date", DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("K2:M2", "Invoice Details", DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("K3:K4", "Invoice No.", DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("L3:L4", "Date of Invoice", DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("M3:M4", "Value of Service" + Environment.NewLine + "(Before Tax)", DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("N2:S2", "Rate of Tax", DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("N3:O3", "IGST", DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("P3:Q3", "CGST", DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("R3:S3", "SGST", DynamicExcel.CellAlignment.Middle);
        //        exl.AddCell("N4", "%", DynamicExcel.CellAlignment.Middle);
        //        exl.AddCell("O4", "Amount", DynamicExcel.CellAlignment.Middle);
        //        exl.AddCell("P4", "%", DynamicExcel.CellAlignment.Middle);
        //        exl.AddCell("Q4", "Amount", DynamicExcel.CellAlignment.Middle);
        //        exl.AddCell("R4", "%", DynamicExcel.CellAlignment.Middle);
        //        exl.AddCell("S4", "Amount", DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("T2:T4", "Total Invoice Value" + Environment.NewLine + "(14=(10+12 or 10+14+16))", DynamicExcel.CellAlignment.Middle);
        //        exl.MargeCell("U2:U4", "Remarks", DynamicExcel.CellAlignment.Middle);
        //        //exl.MargeCell("R2:U2", "Perticulars of Payment Received", DynamicExcel.CellAlignment.Middle);
        //        //exl.MargeCell("R3:R4", "Received" + Environment.NewLine + "At WH/RO", DynamicExcel.CellAlignment.Middle);
        //        //exl.MargeCell("S3:S4", "C.R No. & Date", DynamicExcel.CellAlignment.Middle);
        //        //exl.MargeCell("T3:T4", "Cheque/DD No. & Date", DynamicExcel.CellAlignment.Middle);
        //        //exl.MargeCell("T3:T4", "SD", DynamicExcel.CellAlignment.Middle);
        //        //exl.MargeCell("U3:U4", "Amount", DynamicExcel.CellAlignment.Middle);
        //        //exl.MargeCell("V3:V4", "TDS", DynamicExcel.CellAlignment.Middle);
        //        //exl.MargeCell("V2:V4", "Amount Received Against Bill (Rs.)", DynamicExcel.CellAlignment.Middle);
        //        //exl.MargeCell("W2:W4", "Adjustment/Deduction", DynamicExcel.CellAlignment.Middle);
        //        //exl.MargeCell("W2:W2", "Balance", DynamicExcel.CellAlignment.Middle);
        //        //exl.AddCell("W2", "Balance", DynamicExcel.CellAlignment.Middle);    

        //        for (var i = 65; i < 86; i++)
        //        {
        //            char character = (char)i;
        //            string text = character.ToString();
        //            exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
        //        }
        //        /*exl.AddTable<InvoiceData>("A", 6, model.InvoiceData, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16 });
        //        exl.AddTable<CashReceiptData>("R", 6, model.CashReceiptData, new[] { 12, 30, 14, 14, 14, 14, 14, 40 });/
        //        exl.AddTable<PpgRegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16, 12, 30, 14, 14, 14, 14, 14, 40 });*/
        //        exl.AddTable("A", 6, modelCreditDebit, new[] { 6, 20, 20, 20, 12, 20, 10, 15, 15, 15, 20, 12, 12, 8, 14, 8, 14, 8, 14, 16, 30 });
        //        var igstamt = modelCreditDebit.Sum(o => o.ITaxAmount);
        //        var sgstamt = modelCreditDebit.Sum(o => o.STaxAmount);
        //        var cgstamt = modelCreditDebit.Sum(o => o.CTaxAmount);
        //        var totalamt = modelCreditDebit.Sum(o => o.Total);
        //        exl.AddCell("O" + (modelCreditDebit.Count + 6).ToString(), igstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
        //        exl.AddCell("Q" + (modelCreditDebit.Count + 6).ToString(), cgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
        //        exl.AddCell("S" + (modelCreditDebit.Count + 6).ToString(), sgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
        //        exl.AddCell("T" + (modelCreditDebit.Count + 6).ToString(), totalamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
        //        /*exl.AddCell("O" + (model.Count + 7).ToString(), "Invoice Amount", DynamicExcel.CellAlignment.CenterRight);
        //        exl.AddCell("P" + (model.Count + 7).ToString(), InvoiceAmount, DynamicExcel.CellAlignment.CenterRight);
        //        exl.AddCell("O" + (model.Count + 8).ToString(), "Cash Receipt Amount", DynamicExcel.CellAlignment.CenterRight);
        //        exl.AddCell("P" + (model.Count + 8).ToString(), CRAmount, DynamicExcel.CellAlignment.CenterRight);*/

        //        exl.Save();
        //    }
        //    return excelFile;
        //}

        #endregion


        #region AuctionCashBook
        public void GetAuctionCashBook(string FromDate, string Todate)
        {

            DateTime dtfrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(Todate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");

            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "pFromDate", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "pTodate", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            DataSet ds = DataAccess.ExecuteDataSet("GetAuctionCashBook", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];

            List<AuctionCashBookViewModel> lstAuctionCashBook = new List<AuctionCashBookViewModel>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        lstAuctionCashBook.Add(new AuctionCashBookViewModel
                        {
                            ReceiptNo = Convert.ToString(dr["EmdRcvdNo"]),
                            ReceiptDate = Convert.ToString(dr["EmdRcvdDate"]),
                            ChqDDUTRNo = Convert.ToString(dr["InstrumentNo"]),
                            BidderName = Convert.ToString(dr["BidderName"]),
                            BidNo = Convert.ToString(dr["BidNo"]),
                            AdvanceAmountPaid = Convert.ToDecimal(dr["AdvanceAmount"]),
                            BidAmount = Convert.ToDecimal(dr["BidAmount"]),
                            EmdAmount = Convert.ToDecimal(dr["EmdAmount"]),
                            //  AdvanceAmountAdjust = Convert.ToDecimal(dr["AdvanceAdjust"]),
                            //  TotalPaid = Convert.ToDecimal(dr["TotalPaid"]),

                        });
                    }

                }
                if (lstAuctionCashBook.Count > 0)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = lstAuctionCashBook;
                }
                else
                {
                    _DBResponse.Status = 1;
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


        #region AuctionCashBook For Invoice 
        public void GetAuctionCashBookForInvoice(string FromDate, string Todate)
        {

            DateTime dtfrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(Todate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");

            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "pFromDate", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "pTodate", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            DataSet ds = DataAccess.ExecuteDataSet("GetAuctionCashBookForInvoice", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];

            List<AuctionCashBookViewModel> lstAuctionCashBook = new List<AuctionCashBookViewModel>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        lstAuctionCashBook.Add(new AuctionCashBookViewModel
                        {
                            ReceiptNo = Convert.ToString(dr["EmdRcvdNo"]),
                            ReceiptDate = Convert.ToString(dr["EmdRcvdDate"]),
                            ChqDDUTRNo = Convert.ToString(dr["InstrumentNo"]),
                            BidderName = Convert.ToString(dr["BidderName"]),
                            BidNo = Convert.ToString(dr["BidNo"]),
                            AdvanceAmountPaid = Convert.ToDecimal(dr["AdvanceAmount"]),
                            BidAmount = Convert.ToDecimal(dr["BidAmount"]),
                            IGSTAmount = Convert.ToDecimal(dr["IGSTAmount"]),

                            CGSTAmount = Convert.ToDecimal(dr["CGSTAmount"]),

                            SGSTAmount = Convert.ToDecimal(dr["SGSTAmount"]),

                            TotalPayable = Convert.ToDecimal(dr["TotalPayable"]),

                            AuctionCharges = Convert.ToDecimal(dr["AuctionCharges"]),
                            TDS = Convert.ToDecimal(dr["TDS"]),

                            //  EmdAmount = Convert.ToDecimal(dr["EmdAmount"]),
                            //  AdvanceAmountAdjust = Convert.ToDecimal(dr["AdvanceAdjust"]),
                            // TotalPaid = Convert.ToDecimal(dr["TotalPaid"]),
                            // EMDAmountAdjust = Convert.ToDecimal(dr["AdjustEmdAmount"]),
                            NetAmount = Convert.ToDecimal(dr["NetAmount"]),
                            TotalGST = Convert.ToDecimal(dr["TotalGST"])

                        });
                    }

                }
                if (lstAuctionCashBook.Count > 0)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = lstAuctionCashBook;
                }
                else
                {
                    _DBResponse.Status = 1;
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

        #region AuctionCashBook For Statement 
        public void GetAuctionStatement(string FromDate, string Todate)
        {

            DateTime dtfrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(Todate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");

            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "pFromDate", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "pTodate", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            DataSet ds = DataAccess.ExecuteDataSet("GetAuctionStatement", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];

            List<AuctionStatementViewModel> lstAuctionCashBook = new List<AuctionStatementViewModel>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        lstAuctionCashBook.Add(new AuctionStatementViewModel
                        {
                            EntryNo = Convert.ToString(dr["GateInNo"]),
                            EntryDate = Convert.ToString(dr["EntryDate"]),
                            Obl = Convert.ToString(dr["OBL"]),
                            ShippingBill = Convert.ToString(dr["SB"]),
                            ContainerNo = Convert.ToString(dr["Container"]),
                            CFSCode = Convert.ToString(dr["CFSCode"]),
                            HSNCode = Convert.ToString(dr["HSNCode"]),
                            Size = Convert.ToString(dr["Size"]),
                            InDate = Convert.ToString(dr["InDate"]),
                            Shed = Convert.ToString(dr["GodownName"]),
                            Area = Convert.ToDecimal(dr["Area"]),
                            Pkg = Convert.ToDecimal(dr["Noofpkg"]),
                            Weight = Convert.ToDecimal(dr["Weight"]),
                            Bidamount = Convert.ToDecimal(dr["BidAmount"]),
                            valueCharge = Convert.ToDecimal(dr["ValuesCharges"]),
                            AuctionCharge = Convert.ToDecimal(dr["AuctionCharges"]),
                            MiscCharge = Convert.ToDecimal(dr["MiscCharges"]),
                            CustomDuty = Convert.ToDecimal(dr["CustomDuty"]),
                            CwcShare = Convert.ToDecimal(dr["CWCShare"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            Commodity = Convert.ToString(dr["Commodity"])
                            //   TDSAmount=Convert.ToDecimal(dr["TDSAmount"])

                        });
                    }
                    lstAuctionCashBook.Add(new AuctionStatementViewModel
                    {
                        EntryNo = "<strong>Total</strong>",
                        EntryDate = "",
                        Obl = "",
                        ShippingBill = "",
                        ContainerNo = "",
                        Size = "",
                        InDate = "",
                        Shed = "",
                        Area = lstAuctionCashBook.Sum(x => x.Area),
                        Pkg = lstAuctionCashBook.Sum(x => x.Pkg),
                        Weight = lstAuctionCashBook.Sum(x => x.Weight),
                        Bidamount = lstAuctionCashBook.Sum(x => x.Bidamount),
                        valueCharge = lstAuctionCashBook.Sum(x => x.valueCharge),
                        AuctionCharge = lstAuctionCashBook.Sum(x => x.AuctionCharge),
                        MiscCharge = lstAuctionCashBook.Sum(x => x.MiscCharge),
                        CustomDuty = lstAuctionCashBook.Sum(x => x.CustomDuty),
                        CwcShare = lstAuctionCashBook.Sum(x => x.CwcShare),
                        Remarks = ""
                        // TDSAmount = lstAuctionCashBook.Sum(x => x.TDSAmount)
                    });

                }
                if (lstAuctionCashBook.Count > 0)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = lstAuctionCashBook;
                }
                else
                {
                    _DBResponse.Status = 1;
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

        #region PaymentVoucher Report (Imprest / Temporary Advanced)
        public void GetPaymentVoucherReport(string Fdt, string Tdt, string Purpose)
        {

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDt", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(Fdt).ToString("yyyyMMdd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDt", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(Tdt).ToString("yyyyMMdd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Value = Purpose });

            DParam = LstParam.ToArray();
            //IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentVoucherReport", CommandType.StoredProcedure, DParam);
            IList<Ppg_PaymentVoucherReport> LstRpt = new List<Ppg_PaymentVoucherReport>();
            LstRpt = (List<Ppg_PaymentVoucherReport>)DataAccess.ExecuteDynamicSet<Ppg_PaymentVoucherReport>("GetPaymentVoucherReport", DParam);
            _DBResponse = new DatabaseResponse();
            try
            {

                if (LstRpt.Count > 0)
                {
                    Status = 1;
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstRpt;
                }
                else
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
                /*
                Result.Close();
                Result.Dispose();
                */
            }
        }
        #endregion



        #region Import Container Income(Seci)
        public void ImportConIncomeSeciDetail(Ppg_ImportSeciIncRpt Obj)
        {

            DateTime dtfrom = DateTime.ParseExact(Obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(Obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getImportContainerSeciIncome", CommandType.StoredProcedure, DParam);
            List<Ppg_ImportSeciIncRpt> lstContDtl = new List<Ppg_ImportSeciIncRpt>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstContDtl.Add(new Ppg_ImportSeciIncRpt()
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ContainerType = Result["MovementType"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        ForeignLiner = Result["ForeignLiner"].ToString(),
                        VesselName = Result["VesselName"].ToString(),
                        VesselNo = Result["VesselNo"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceType = Convert.ToString(Result["InvoiceType"]),
                        PayeeName = Result["PayeeName"].ToString(),
                        GCD = Convert.ToDecimal(Result["GCD"].ToString()),
                        GDV = Convert.ToDecimal(Result["GDV"].ToString()),
                        THC = Convert.ToDecimal(Result["THC"]),
                        IRR = Convert.ToDecimal(Result["IRR"]),
                        CHT = Convert.ToDecimal(Result["TPT"]),
                        CH = Convert.ToDecimal(Result["HND"]),
                        SW = Convert.ToDecimal(Result["ST"]),
                        OSI = Convert.ToDecimal(Result["OT"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        Total = Convert.ToDecimal(Result["Total"])
                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstContDtl;
                }
                else
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
        #region Export Container Income(Seci)
        public void ExportConIncomeSeciDetail(Ppg_ExportSeciIncRpt Obj)
        {

            DateTime dtfrom = DateTime.ParseExact(Obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(Obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getExportContainerSeciIncome", CommandType.StoredProcedure, DParam);
            List<Ppg_ExportSeciIncRpt> lstContDtl = new List<Ppg_ExportSeciIncRpt>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstContDtl.Add(new Ppg_ExportSeciIncRpt()
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ContainerType = Result["MovementType"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        ForeignLiner = Result["ForeignLiner"].ToString(),
                        VesselName = Result["VesselName"].ToString(),
                        VesselNo = Result["VesselNo"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceType = Convert.ToString(Result["InvoiceType"]),
                        PayeeName = Result["PayeeName"].ToString(),
                        GCD = Convert.ToDecimal(Result["GCD"].ToString()),
                        GDV = Convert.ToDecimal(Result["GDV"].ToString()),
                        THC = Convert.ToDecimal(Result["THC"]),
                        IRR = Convert.ToDecimal(Result["IRR"]),
                        CHT = Convert.ToDecimal(Result["TPT"]),
                        CH = Convert.ToDecimal(Result["HND"]),
                        SW = Convert.ToDecimal(Result["ST"]),
                        OSI = Convert.ToDecimal(Result["OT"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        Total = Convert.ToDecimal(Result["Total"])
                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstContDtl;
                }
                else
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

        #region Seal Cutting Report (For FCL Container)

        public void GetSealCuttingReportForFCLCont(string FDt, string TDt)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDt", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FDt) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDt", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(TDt) });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetSealCuttingReportForFCLCont", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result != null && Result.Tables[0].Rows.Count > 0)
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
        #endregion
        public void GenericBulkInvoiceDetailsForPrintAuction(AuctionInvoiceViewModel ObjBulkInvoiceReport)
        {
            DateTime dtfrom = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            ObjBulkInvoiceReport.InvoiceModule = "Auc";
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
                Result = DataAccess.ExecuteDataSet("GetAuctioninvoicedetailsforprint", CommandType.StoredProcedure, DParam);
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

        #region Auction Outward Register Supply

        public void GetAuctionRegisterofOutwardSupply(DateTime date1, DateTime date2)
        {
            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.DateTime, Value = date2 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //var objRegOutwardSupply = (List<RegisterOfOutwardSupplyModel>)DataAccess.ExecuteDynamicSet<List<RegisterOfOutwardSupplyModel>>("GetRegisterofOutwardSupply", DParam);
            DataSet ds = DataAccess.ExecuteDataSet("GetAuctionRegisterOutwardSupply", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];

            List<PpgRegisterOfOutwardSupplyModel> model = new List<PpgRegisterOfOutwardSupplyModel>();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    model = (from DataRow dr in dt.Rows
                             select new PpgRegisterOfOutwardSupplyModel()
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
                _DBResponse.Data = AuctionRegisterofOutwardSupplyExcel(model, InvoiceAmount, CRAmount);
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

        private string AuctionRegisterofOutwardSupplyExcel(List<PpgRegisterOfOutwardSupplyModel> model, decimal InvoiceAmount, decimal CRAmount)
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
                exl.MargeCell("F2:F4", "Nature of Invoice" + Environment.NewLine + "(Assesment Type)", DynamicExcel.CellAlignment.Middle);
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
                exl.AddTable<PpgRegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 20, 20, 20, 12, 20, 10, 15, 20, 12, 12, 8, 14, 8, 14, 8, 14, 16, 10, 30 });
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


        #endregion

        #region Concor Ledger Sheet

        public void GetConcorLedgerSheet(string FromDate, string Todate)
        {
            DateTime dtfrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");

            DateTime dtTo = DateTime.ParseExact(Todate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");

            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "pFromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "pToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //var objRegOutwardSupply = (List<RegisterOfOutwardSupplyModel>)DataAccess.ExecuteDynamicSet<List<RegisterOfOutwardSupplyModel>>("GetRegisterofOutwardSupply", DParam);
            DataSet ds = DataAccess.ExecuteDataSet("ReportConcorLedgerSheet", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];

            List<PpgConcorLedgerSheetViewModel> lstPpgConcorLedgerSheetViewModel = new List<PpgConcorLedgerSheetViewModel>();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        lstPpgConcorLedgerSheetViewModel.Add(new PpgConcorLedgerSheetViewModel
                        {
                            Balance = Convert.ToDecimal(dr["Balance"]),
                            ConcorInvoiceNo = Convert.ToString(dr["ConcorInvoiceNo"]),
                            ContainerNo = Convert.ToString(dr["ContainerNo"]),
                            ContainerType = Convert.ToString(dr["ContainerType"]),
                            CreditAmount = Convert.ToDecimal(dr["CreditAmount"]),
                            Date = Convert.ToString(dr["Date"]),
                            DOC = Convert.ToDecimal(dr["Doc"]),
                            GrossWeight = Convert.ToDecimal(dr["Grossweight"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            IRR = Convert.ToDecimal(dr["IRR"]),
                            NetWeight = Convert.ToDecimal(dr["NetWeight"]),
                            OperationType = Convert.ToString(dr["OperationType"]),
                            OtherChg = Convert.ToDecimal(dr["OtherChg"]),
                            POLPOD = Convert.ToString(dr["PODPOL"]),
                            Size = Convert.ToInt32(dr["Size"]),
                            SlNo = Convert.ToString(dr["SlNo"]),
                            TareWeight = Convert.ToDecimal(dr["Tareweight"]),
                            THC = Convert.ToDecimal(dr["THC"]),
                            TrainDate = Convert.ToString(dr["TrainDate"]),
                            TrainNo = Convert.ToString(dr["TrainNo"])

                        });
                    }
                }

                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = GetConcorLedgerSheetExcel(lstPpgConcorLedgerSheetViewModel, FromDate, Todate);
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

        private string GetConcorLedgerSheetExcel(List<PpgConcorLedgerSheetViewModel> vm, string FromDate, string ToDate)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"GENERAL LEDGER STATEMENT (For ICD TKD)";
                exl.MargeCell("A1:U1", title, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B3:U3", "PARTY NAME: CONCOR", DynamicExcel.CellAlignment.TopLeft);
                exl.MargeCell("B4:U4", "PARTY ADDRESS: ICD TUGHLAKABAD", DynamicExcel.CellAlignment.TopLeft);
                exl.MargeCell("B5:U5", "PARTY GST NO:", DynamicExcel.CellAlignment.TopLeft);

                var FromRang = @"PERIOD W.E.F " + FromDate + " TO " + ToDate;

                exl.MargeCell("A7:U7", FromRang, DynamicExcel.CellAlignment.TopCenter);
                exl.AddCell("A9", "S.NO.", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("B9", "DATE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C9:N9", "DESCRIPTION", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O9:R9", "DEBIT AMOUNT", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("S9", "CREDIT AMOUNT", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("T9", "BALANCE", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("U9", "REMARKS", DynamicExcel.CellAlignment.Middle);

                exl.AddCell("A10", "", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("B10", "", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("C10", "CONCOR INVOICE NO.", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("D10", "INVOICE DATE", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("E10", "OPERATION TYPE", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("F10", "TRAIN NO.", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("G10", "TRAIN DATE", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("H10", "CONTAINER NO.", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("I10", "CONTAINER SIZE", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("J10", "POL/POD", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("K10", "NET WEIGHT", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("L10", "TARE WEIGHT", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("M10", "GROSS WEIGHT", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("N10", "CONTAINER TYPE", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("O10", "IRR/RR", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("P10", "THC", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("Q10", "DOCUMENTATION", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("R10", "OTHER CHG.", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("S10", "", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("T10", "", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("U10", "", DynamicExcel.CellAlignment.Middle);






                //for (var i = 65; i < 85; i++)
                //{
                //    char character = (char)i;
                //    string text = character.ToString();
                //    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                //}

                exl.AddTable<PpgConcorLedgerSheetViewModel>("A", 11, vm, new[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 6, 6, 6, 6, 20 });

                var Netweight = vm.Sum(x => x.NetWeight);
                var Tareweight = vm.Sum(x => x.TareWeight);
                var grossweight = vm.Sum(x => x.GrossWeight);
                var RR = vm.Sum(x => x.IRR);
                var Thc = vm.Sum(x => x.THC);
                var docu = vm.Sum(x => x.DOC);
                var other = vm.Sum(x => x.OtherChg);
                var Credit = vm.Sum(x => x.CreditAmount);
                var Balance = vm.Sum(x => x.Balance);



                exl.AddCell("K" + (vm.Count + 11).ToString(), Netweight.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("L" + (vm.Count + 11).ToString(), Tareweight.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("M" + (vm.Count + 11).ToString(), grossweight.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (vm.Count + 11).ToString(), RR.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (vm.Count + 11).ToString(), Thc.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("Q" + (vm.Count + 11).ToString(), docu.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("R" + (vm.Count + 11).ToString(), other.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("S" + (vm.Count + 11).ToString(), Credit.ToString(), DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("T" + (vm.Count + 11).ToString(), Balance.ToString(), DynamicExcel.CellAlignment.CenterRight);



                exl.Save();

            }
            return excelFile;
        }


        #endregion


        public void GenericBulkInvoiceDetailsForPrintAll(string month, string year)
        {
            //DateTime dtfrom = DateTime.ParseExact("01/10/2019", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            //DateTime dtTo = DateTime.ParseExact("31/10/2019", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //if (String.IsNullOrWhiteSpace(ObjBulkInvoiceReport.InvoiceModule))
            //{
            //    ObjBulkInvoiceReport.InvoiceModule = "";
            //}
            //if (String.IsNullOrWhiteSpace(ObjBulkInvoiceReport.InvoiceNumber))
            //{
            //    ObjBulkInvoiceReport.InvoiceNumber = "";
            //}
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = "" });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Monthval", MySqlDbType = MySqlDbType.VarChar, Value = month });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Yearval", MySqlDbType = MySqlDbType.VarChar, Value = year });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("getbulkinvoicedetailsforprintdownload", CommandType.StoredProcedure, DParam);
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
        public void ModuleListWithInvoiceALL(string month, string year)
        {

            //DateTime dtfrom = DateTime.ParseExact("01/10/2019", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            //DateTime dtTo = DateTime.ParseExact("31/10/2019", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            //int PartyId = ObjBulkInvoiceReport.PartyId;

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_month", MySqlDbType = MySqlDbType.VarChar, Value = month });
            LstParam.Add(new MySqlParameter { ParameterName = "in_year", MySqlDbType = MySqlDbType.VarChar, Value = year });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            DParam = LstParam.ToArray();
            DataSet LstInvoice = new DataSet();
            LstInvoice = DataAccess.ExecuteDataSet("ModuleListWithInvoiceALL", CommandType.StoredProcedure, DParam);

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





        #region Daily Valuation Report For Export Cargo
        public void DVRForPrint(string FromDate, string ToDate, int GodownId = 0)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate) });

            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("DailyValuationofExpCargo", CommandType.StoredProcedure, DParam);
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

        #endregion
        #region Stuffing Request Register
        public void StuffingRegister(Ppg_StuffingRegRpt ObjStuffingRegister)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjStuffingRegister.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjStuffingRegister.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            IDataReader Result = DataAccess.ExecuteDataReader("StuffingRegisterReport", CommandType.StoredProcedure, DParam);
            Ppg_StuffingRegRpt LstStuffingRegister = new Ppg_StuffingRegRpt();
            // IList<Ppg_StuffingDetail> LstStuffDetails = new List<Ppg_StuffingDetail> ();
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

                    LstStuffingRegister.LstStuff.Add(new Ppg_StuffingRegRpt
                    {



                        Date = Result["RequestDate"].ToString(),

                        CfsCode = Result["CFSCode"].ToString(),

                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),

                        //  ExporterName = Result["ExporterName"].ToString(),
                        ShippingLineName = Result["ShippingLine"].ToString(),
                        //  CHAName = Result["CHAName"].ToString(),
                        Cargo = Result["Cargo"].ToString(),
                        //  NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                        //     shippingBillNo = Result["ShippingBillNo"].ToString(),
                        //    shippingBillDate = Result["ShippingBillDate"].ToString(),

                        //shippingBillNo = Result["ShippingBillNo"].ToString(),
                        // pod = Result["POD"].ToString(),
                        Fob = Convert.ToDecimal(Result["Fob"]),
                        Weight = Convert.ToDecimal(Result["GrossWeight"]),
                        StfRegisterNo = Result["StuffingNo"].ToString(),
                        ForwarderName = Result["ForwarderName"].ToString(),
                        //  POL = Result["POL"].ToString(),
                        StuffType = Result["StuffType"].ToString(),
                        //   Area= Convert.ToDecimal(Result["Area"])
                        // StfRegisterDate = Result["POD"].ToString()
                        ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                        GodownNo= Result["GodownName"].ToString()


                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        LstStuffingRegister.LstStuffDetails.Add(new Ppg_StuffingDetail()
                        {
                            ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                            ExporterName = Result["ExporterName"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            CHA = Result["CHA"].ToString(),
                            NoOfUnit = Convert.ToInt32(Result["NoOfUnit"]),
                            SBNo = Result["SBNO"].ToString(),
                            SBDate = Result["SBDate"].ToString(),
                            Area = Convert.ToDecimal(Result["Area"]),
                            PortOfLoading = Result["PortOfLoading"].ToString(),


                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffingRegister;
                }
                else
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
        #region Export Loaded Container Out
        public void GetExpLoadedContrOut(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetExpLoadedContrOut", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_LoadedContrOutList> LstLoadedContr = new List<Ppg_LoadedContrOutList>();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstLoadedContr.Add(new Ppg_LoadedContrOutList
                    {
                        ICDCode = (Result["ICDCode"] == null ? "" : Result["ICDCode"]).ToString(),
                        ContainerNo = (Result["ContainerNo"] == null ? "" : Result["ContainerNo"]).ToString(),
                        Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                        ForwarderName = (Result["Forwarder"] == null ? "" : Result["Forwarder"]).ToString(),
                        ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                        Seal = (Result["Seal"] == null ? "" : Result["Seal"]).ToString(),
                        GatePassNo = (Result["GatePassNo"] == null ? "" : Result["GatePassNo"]).ToString(),
                        GatePassDate = (Result["GatePassDate"] == null ? "" : Result["GatePassDate"]).ToString(),
                        GateOutDate = (Result["GateOutDate"] == null ? "" : Result["GateOutDate"]).ToString(),
                        TransportMode = (Result["TransportMode"] == null ? "" : Result["TransportMode"]).ToString(),
                    });
                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstLoadedContr;
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
        public void DeStuffingReportBig(Ppg_DestuffingReport ObjDeStuffingReportBig)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDeStuffingReportBig.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDeStuffingReportBig.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = ObjDeStuffingReportBig.GodownId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ImportDeStuffingReport", CommandType.StoredProcedure, DParam);
            IList<Ppg_DestuffingReport> LstDeStuffingReportBig = new List<Ppg_DestuffingReport>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstDeStuffingReportBig.Add(new Ppg_DestuffingReport
                    {



                        DeStuffingNo = Result["DestuffingEntryNo"].ToString(),
                        DeStuffingDate = Result["DestuffingEntryDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ContainerSize = Result["Size"].ToString(),
                        GodownName = Result["GodownName"].ToString(),


                        CFSCode = Result["CFSCode"].ToString(),
                        ShippingLine = Result["Shipping Line"].ToString(),
                        VesselName = Result["Vessel"].ToString(),
                        VoyageNo = Result["Voyage"].ToString(),
                        Rotation = Result["Rotation"].ToString(),
                        SlSealNo = Result["SealNo"].ToString(),
                        CustomSealNO = Result["CustomSealNo"].ToString(),

                        LineNo = Result["LineNo"].ToString(),

                        BoeNo = Result["BOENo"].ToString(),
                        BoeDate = Result["BOEDate"].ToString(),
                        BolNo = Result["BOLNo"].ToString(),

                        BolDate = Result["BOLDate"].ToString(),
                        MarksNo = Result["MarksNo"].ToString(),
                        CHA = Result["CHA"].ToString(),
                        Importer = Result["Importer"].ToString(),

                        CargoDescription = Result["CargoDescription"].ToString(),
                        Commodity = Result["CommodityName"].ToString(),
                        CargoType = Result["Cargo Type"].ToString(),

                        NoOfPackets = Result["NoOfPackages"].ToString(),
                        YardLocation = Result["Yard/Location No"].ToString(),
                        Grossweight = Result["GrossWeight"].ToString(),
                        CIFValue = Result["CIFValue"].ToString(),
                        Duty = Result["Duty"].ToString()




                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDeStuffingReportBig;
                }
                else
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


        #region Core Data  REPORT New Format
        public void GetCoreDataReport(int month, int year)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Year", MySqlDbType = MySqlDbType.VarChar, Value = year });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.Int32, Value = month });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("RptCoreDataReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PpgCoreDataRptModel LstData = new PpgCoreDataRptModel();
            try
            {

                while (Result.Read())
                {
                    Status = 1;

                    LstData.ICDCurrMon = Convert.ToDecimal(Result["ICDCurrMon"]);
                    LstData.ICDCommuMon = Convert.ToDecimal(Result["ICDCommuMon"]);
                    LstData.ICDPreCurrMon = Convert.ToDecimal(Result["ICDPreCurrMon"]);
                    LstData.ICDPreCommuCurrMon = Convert.ToDecimal(Result["ICDPreCommuCurrMon"]);
                    LstData.MFCurrMon = Convert.ToDecimal(Result["MFCurrMon"]);
                    LstData.MFCommuMon = Convert.ToDecimal(Result["MFCommuMon"]);
                    LstData.MFPreCurrMon = Convert.ToDecimal(Result["MFPreCurrMon"]);
                    LstData.MFPreCommuCurrMon = Convert.ToDecimal(Result["MFPreCommuCurrMon"]);
                    LstData.CRTCurrMon = Convert.ToDecimal(Result["CRTCurrMon"]);
                    LstData.CRTCommuMon = Convert.ToDecimal(Result["CRTCommuMon"]);
                    LstData.CRTPreCurrMon = Convert.ToDecimal(Result["CRTPreCurrMon"]);

                    LstData.CRTPreCommuMon = Convert.ToDecimal(Result["CRTPreCommuMon"]);
                    LstData.PESTCurrMon = Convert.ToDecimal(Result["PESTCurrMon"]);
                    LstData.PESTCommuMon = Convert.ToDecimal(Result["PESTCommuMon"]);
                    LstData.PESTPreCurrMon = Convert.ToDecimal(Result["PESTPreCurrMon"]);
                    LstData.PESTPreCommuCurrMon = Convert.ToDecimal(Result["PESTPreCommuCurrMon"]);
                    LstData.OtherCurrMon = Convert.ToDecimal(Result["OtherCurrMon"]);
                    LstData.OtherCommuMon = Convert.ToDecimal(Result["OtherCommuMon"]);
                    LstData.OtherPreCurrMon = Convert.ToDecimal(Result["OtherPreCurrMon"]);
                    LstData.OtherPreCommuMon = Convert.ToDecimal(Result["OtherPreCommuMon"]);
                    LstData.TotActMon = Convert.ToDecimal(Result["TotActMon"]);
                    LstData.TotCommuMon = Convert.ToDecimal(Result["TotCommuMon"]);
                    LstData.TotPreActMon = Convert.ToDecimal(Result["TotPreActMon"]);
                    LstData.TotPreCommMon = Convert.ToDecimal(Result["TotPreCommMon"]);


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



        public void GetCoreDataForPrint(int month, int year)
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

        #region Contarrival
       
        public void GetImportContArrivalData(PPG_ContArrivalReport vm)
        {

            DateTime dtfrom = DateTime.ParseExact(vm.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(vm.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContSize", MySqlDbType = MySqlDbType.VarChar, Value = vm.ContainerSize });

            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("GetIMPContarrivalRpt", CommandType.StoredProcedure, DParam);
            List<PPG_ContArrivalReport> lstStockRegisterReport = new List<PPG_ContArrivalReport>();
            _DBResponse = new DatabaseResponse();
            try
            {
                foreach (DataRow dr in Result.Tables[0].Rows)
                {
                    Status = 1;
                    lstStockRegisterReport.Add(new PPG_ContArrivalReport
                    {
                        CFSCODE = Convert.ToString(dr["CFSCODE"]),
                        ContainerNo = Convert.ToString(dr["ContainerNo"]),
                        Size = Convert.ToString(dr["Size"]),
                        ShippingLine = Convert.ToString(dr["ShippingLine"]),
                        ModeOfTransport = Convert.ToString(dr["ModeOfTransport"]),
                        
                        Remarks = Convert.ToString(dr["Remarks"]),
                        CustomSealNo = Convert.ToString(dr["CustomSealNo"]),
                        GateInDateTime = Convert.ToString(dr["GateInDateTime"]),
                       
                       
                        VehicleNo = Convert.ToString(dr["VehicleNo"]),

                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstStockRegisterReport;
                }
                else
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
        public void GetExportEmptyContArrivalData(PPG_ContArrivalReport vm)
        {

            DateTime dtfrom = DateTime.ParseExact(vm.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(vm.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContSize", MySqlDbType = MySqlDbType.VarChar, Value = vm.ContainerSize });

            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("GetExpEmptyContarrivalRpt", CommandType.StoredProcedure, DParam);
            List<PPG_ContArrivalReport> lstExportEmptyArrivallist = new List<PPG_ContArrivalReport>();
            _DBResponse = new DatabaseResponse();
            try
            {
                foreach (DataRow dr in Result.Tables[0].Rows)
                {
                    Status = 1;
                    lstExportEmptyArrivallist.Add(new PPG_ContArrivalReport
                    {
                        CFSCODE = Convert.ToString(dr["CFSCODE"]),
                        ContainerNo = Convert.ToString(dr["ContainerNo"]),
                        Size = Convert.ToString(dr["Size"]),
                        ShippingLine = Convert.ToString(dr["ShippingLine"]),
                        ModeOfTransport = Convert.ToString(dr["ModeOfTransport"]),

                        Remarks = Convert.ToString(dr["Remarks"]),
                        CustomSealNo = Convert.ToString(dr["CustomSealNo"]),
                        GateInDateTime = Convert.ToString(dr["GateInDateTime"]),


                        VehicleNo = Convert.ToString(dr["VehicleNo"]),

                    });

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstExportEmptyArrivallist;
                }
                else
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

        #region Stock Register Report
        public void StockRegisterForPrint(string DTRDate, string DTRToDate, int GodownId = 0)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DTRDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "out_date", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(DTRToDate) });

            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("StockRegisterReport", CommandType.StoredProcedure, DParam);
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
        #endregion



        #region Export Load Container Report
        public void GetExportLoadedContReport(Ppg_ExportLoadContainer vm)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(vm.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(vm.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string PeriodTo = dtTo.ToString("yyyy/MM/dd");

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            DataSet ds = DataAccess.ExecuteDataSet("GetExportLoadContReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        #endregion

        #region ImportEmptyContainerOutReport 
        public void ImportEmptyContainerOutport(ImportEmptyContainerOut ObjContainerOutReport)
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
            //LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjContainerOutReport.ImportExport });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_LoadedEmpty", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjContainerOutReport.LoadedEmpty });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ImportEmptyContainerOutReport", CommandType.StoredProcedure, DParam);
            ImportEmptyContainerOut LstContainerOutReport = new ImportEmptyContainerOut();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstContainerOutReport.lstContainerOutReport.Add(new ImportEmptyContainerOut
                    {



                        //Date = Result["Date"].ToString(),
                        ICDCode= Result["CFSCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                        Time = Result["Time"].ToString(),
                        ShippingLine= Result["ShippingLine"].ToString(),
                        VehicleNo= Result["VehicleNo"].ToString(),

                        //LoadedEmpty = Result["LoadedEmpty"].ToString(),
                        //ImportExport = Result["ImportExport"].ToString()
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
        #region ImportLoadedContainerOutReport 
        public void ImportLoadedContainerOutReport(ImportLoadedContainerOutReport ObjContainerOutReport)
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
            //LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjContainerOutReport.ImportExport });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_LoadedEmpty", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjContainerOutReport.LoadedEmpty });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ImportLoadedContainerOutReport", CommandType.StoredProcedure, DParam);
            ImportLoadedContainerOutReport LstContainerOutReport = new ImportLoadedContainerOutReport();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstContainerOutReport.lstContainerOutReport.Add(new ImportLoadedContainerOutReport
                    {



                        //Date = Result["Date"].ToString(),
                        ICDCode = Result["CFSCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                        Time = Result["Time"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        CustomSealNo= Result["CustomSealNo"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),

                        //LoadedEmpty = Result["LoadedEmpty"].ToString(),
                        //ImportExport = Result["ImportExport"].ToString()
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
        #region ExportLoadedContainerOutReport 
        public void ExportLoadedContainerOutReport(ExportLoadedContainerOutReport ObjContainerOutReport)
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
            //LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjContainerOutReport.ImportExport });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_LoadedEmpty", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjContainerOutReport.LoadedEmpty });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ExportLoadedContainerOutReport", CommandType.StoredProcedure, DParam);
            ExportLoadedContainerOutReport LstContainerOutReport = new ExportLoadedContainerOutReport();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstContainerOutReport.lstContainerOutReport.Add(new ExportLoadedContainerOutReport
                    {



                        //Date = Result["Date"].ToString(),
                        ICDCode = Result["CFSCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                        Time = Result["Time"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        CustomSealNo = Result["CustomSealNo"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),

                        //LoadedEmpty = Result["LoadedEmpty"].ToString(),
                        //ImportExport = Result["ImportExport"].ToString()
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


        #region Invoice Details Credit Note wise
        public void GetInvoiceCreditNote(DateTime date1, DateTime date2)
        {
            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Fromdate", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = date2 });
              IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //var objRegOutwardSupply = (List<RegisterOfOutwardSupplyModel>)DataAccess.ExecuteDynamicSet<List<RegisterOfOutwardSupplyModel>>("GetRegisterofOutwardSupply", DParam);
            DataSet ds = DataAccess.ExecuteDataSet("GetInvoiceDetailsCreditNote", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
          






          //  List<WFLD_PartyLedCons> model = new List<WFLD_PartyLedCons>();
            try
            {


                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = GetInvoiceCreditNoteExcel(dt, date1.ToString("dd/MM/yyyy") , date2.ToString("dd/MM/yyyy"));
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



        private string GetInvoiceCreditNoteExcel( DataTable dt, string date1,string date2)
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
                          + "Invoice Details Credit Note wise from" + date1 + " "+" To "+ date2 +"</br>";

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
                cell4.Text = "Invoice Details Credit Note wise From " + date1 + " " + " To " + date2;
                System.Web.UI.WebControls.TableRow tr4 = new System.Web.UI.WebControls.TableRow();
                cell4.ForeColor = System.Drawing.Color.Black;

                tr4.Cells.Add(cell4);
                tr4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;



                System.Web.UI.WebControls.TableCell cell5 = new System.Web.UI.WebControls.TableCell();
                cell5.Controls.Add(Grid);
                System.Web.UI.WebControls.TableRow tr5 = new System.Web.UI.WebControls.TableRow();
                tr5.Cells.Add(cell5);

                tb.Rows.Add(tr1);
                //  tb.Rows.Add(tr2);
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


        #endregion

        #region Company details
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
                    objCompanyDetails.EffectVersion = Convert.ToDecimal(Result["Version"]);
                    objCompanyDetails.VersionLogoFile = Convert.ToString(Result["Effectlogofile"]);
                    objCompanyDetails.BranchName = Convert.ToString(Result["BranchName"]);


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

        #region Party Wise Tues Done Report
        public void GetPartyWiseTuesDone(Ppg_LongStandingImpCon ObjContainerBalanceInCFS)
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
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PartyWiseTuesDone", CommandType.StoredProcedure, DParam);
            IList<Ppg_PartyWiseTuesDone> LstContainerBalanceInCFS = new List<Ppg_PartyWiseTuesDone>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstContainerBalanceInCFS.Add(new Ppg_PartyWiseTuesDone
                    {                       
                        PartyName = Result["Party_Name"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        Size = Result["Size"].ToString(),
                        GateInDate = Result["GateInDate"].ToString()
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
    }
}
