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
    public class DSR_ReportRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }
        public void GatePassReport(GatePassReport ObjGatePassReport, int UserId)
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
            IList<GatePassReport> LstGatePassReport = new List<GatePassReport>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstGatePassReport.Add(new GatePassReport
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
            DSRInvoiceGate objPaymentSheet = new DSRInvoiceGate();
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
                        objPaymentSheet.LstContainersGate.Add(new DSRInvoiceContainerGate()
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
                        objPaymentSheet.LstChargesGate.Add(new DSRInvoiceChargeGate()
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

        public List<DSRInvoiceGate> GetBulkInvoiceDetailsForPrint(DSRBulkInvoiceReport ObjBulkInvoiceReport)
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

            List<DSRInvoiceGate> objPaymentSheetList = new List<DSRInvoiceGate>();
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
                    DSRInvoiceGate objPaymentSheetInvoiceHeader = new DSRInvoiceGate();
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        objPaymentSheetInvoiceHeader.CompanyName = dr["CompanyName"].ToString();
                        objPaymentSheetInvoiceHeader.CompanyShortName = dr["CompanyShortName"].ToString();
                        objPaymentSheetInvoiceHeader.CompanyAddress = dr["CompanyAddress"].ToString();
                        objPaymentSheetInvoiceHeader.CompanyGstNo = dr["GstIn"].ToString();
                    }

                    foreach (DataRow dr in Result.Tables[1].Rows)
                    {
                        DSRInvoiceGate objPaymentSheet = new DSRInvoiceGate();
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
                                objPaymentSheet.LstContainersGate.Add(new DSRInvoiceContainerGate()
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
                                objPaymentSheet.LstChargesGate.Add(new DSRInvoiceChargeGate()
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

        public void GenericBulkInvoiceDetailsForPrint(DSRBulkInvoiceReport ObjBulkInvoiceReport)
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
                if (ObjBulkInvoiceReport.InvoiceModule.ToLower() == "bndadv" || ObjBulkInvoiceReport.InvoiceModule.ToLower() == "bnd"
                    || ObjBulkInvoiceReport.InvoiceModule.ToLower() == "bndadvance")
                {
                    Result = DataAccess.ExecuteDataSet("GetBulkInvoiceDetailsForBondPrint", CommandType.StoredProcedure, DParam);
                }
                else
                {
                    Result = DataAccess.ExecuteDataSet("GetBulkInvoiceDetailsForPrint", CommandType.StoredProcedure, DParam);
                }
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
        public void GetInvoiceListByStuffingId(string StuffingId)
        {



            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingId", MySqlDbType = MySqlDbType.Int32, Value = StuffingId });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("InvoiceListWithStuffingId", CommandType.StoredProcedure, DParam);
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
                    _DBResponse.Data = LstInvoice; ;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void GetFactoryInvoiceListByStuffingId(string StuffingId)
        {



            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingId", MySqlDbType = MySqlDbType.Int32, Value = StuffingId });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("InvoiceListWithStuffingIdForFactory", CommandType.StoredProcedure, DParam);
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
                    _DBResponse.Data = LstInvoice; ;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
        public void GetInvoiceList(string FromDate, string ToDate, string invoiceType)
        {

            DateTime dtfrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            if (invoiceType == "All")
            {
                invoiceType = "";
            }

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

        public void ModuleListWithInvoice(DSRBulkInvoiceReport ObjBulkInvoiceReport)
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

        public void getCompanyDetails()
        {

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();


            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getCompanyDetails", CommandType.StoredProcedure, DParam);
            DSRCompanyDetailsForReport objCompanyDetails = new DSRCompanyDetailsForReport();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    objCompanyDetails.RoAddress = Convert.ToString(Result["ROAddress"]).Replace("<br/>", " ");
                    objCompanyDetails.CompanyName = Convert.ToString(Result["CompanyName"]);
                    objCompanyDetails.GSTIN = Convert.ToString(Result["GSTIN"]);
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
            DSRSDBalancePrint objSDBalance = new DSRSDBalancePrint();
            try
            {


                while (Result.Read())
                {
                    Status = 1;
                    objSDBalance.SDBalance = Convert.ToDecimal(Result["SDBalanceAmount"]);
                    objSDBalance.PortName = Result["PortName"].ToString();
                    objSDBalance.EmptyFrom = Result["EmptyFrom"].ToString();
                    objSDBalance.EmptyTo = Result["EmptyTo"].ToString();
                    objSDBalance.LoadFrom = Result["LoadFrom"].ToString();
                    objSDBalance.LoadTo = Result["LoadTo"].ToString();
                    objSDBalance.MovementType = Result["Movement"].ToString();
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
            // DSRSDBalancePrint objSDBalance = new DSRSDBalancePrint();
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
            DSRDaysWeeks objDaysWeeks = new DSRDaysWeeks();
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
            DSRSDStatement ObjSDStatement = new DSRSDStatement();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        ObjSDStatement.LstSD.Add(new DSRSDList
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

        #region  On Account Statement
        public void GetOnAccountStatement(string Fdt, string Tdt)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Fdt", MySqlDbType = MySqlDbType.VarString, Value = Fdt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Tdt", MySqlDbType = MySqlDbType.VarString, Value = Tdt });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("RptOAStatement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSROAStatement ObjOAStatement = new DSROAStatement();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        ObjOAStatement.LstOnAccount.Add(new DSROAList
                        {
                            PartyName = Convert.ToString(dr["PartyName"]),
                            OpeningAmount = Convert.ToDecimal(dr["OpeningBalance"]),
                            BalanceAmount = Convert.ToDecimal(dr["BalanceAmount"]),
                            AdjustAmount = Convert.ToDecimal(dr["AdjustAmount"]),
                            ReceiptAmount = Convert.ToDecimal(dr["ReceiptAmount"]),

                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjOAStatement;
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
        #region Party Wise On Account Statement Details Statement
        public void GetAllPartyForOADet(string PartyCode, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllPartyForPartyWiseOADet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSRPartyForOADet> LstParty = new List<DSRPartyForOADet>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstParty.Add(new DSRPartyForOADet
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

        public void GetPartyWiseOnAccountStatement(int PartyId, string Fdt, string Tdt)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_OAPartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Fdt", MySqlDbType = MySqlDbType.VarString, Value = Fdt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Tdt", MySqlDbType = MySqlDbType.VarString, Value = Tdt });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("RptOAPartyWiseStatement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSROADtlStatement OAResult = new DSROADtlStatement();
            try
            {

                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        OAResult.PartyName = Convert.ToString(dr["PartyName"]);
                        OAResult.PartyCode = Convert.ToString(dr["PartyCode"]);
                        OAResult.PartyGst = Convert.ToString(dr["PartyGst"]);
                        //OAResult.CompanyGst = "";//Result["CompanyGst"].ToString();
                    }

                    foreach (DataRow dr in Result.Tables[1].Rows)
                    {
                        OAResult.LstOnAccountDtl.Add(new DSROADtlStatementList
                        {
                            Particular = Convert.ToString(dr["ReceiptNo"]),
                            ReceivedDate = Convert.ToString(dr["ReceivedDate"]),
                            AdjustAmount = Convert.ToDecimal(dr["AdjustAmount"]),
                            ReceiptAmount = Convert.ToDecimal(dr["ReceiptAmount"]),
                            ClosingBalance = Convert.ToDecimal(dr["ClosingBalance"]),

                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = OAResult;
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
                List<DSRRegisterOfOutwardSupplyModel> model = new List<DSRRegisterOfOutwardSupplyModel>();
                try
                {

                    decimal InvoiceAmount = 0, CRAmount = 0;

                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = RegisterofOutwardSupplyExcel(model, InvoiceAmount, CRAmount, dt);
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
                List<DSRRegisterOfOutwardSupplyModelCreditDebit> modelCreditDebit = new List<DSRRegisterOfOutwardSupplyModelCreditDebit>();
                try
                {
                    if (ds.Tables.Count > 0)
                    {
                        modelCreditDebit = (from DataRow dr in dt.Rows
                                            select new DSRRegisterOfOutwardSupplyModelCreditDebit()
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
                                                Remarks = dr["Remarks"].ToString(),
                                                PaymentMode= dr["PaymentMode"].ToString(),

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
        private string RegisterofOutwardSupplyExcel(List<DSRRegisterOfOutwardSupplyModel> model, decimal InvoiceAmount, decimal CRAmount, DataTable dt)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xls");
            System.Web.UI.WebControls.GridView Grid = new System.Web.UI.WebControls.GridView();


            System.Web.UI.WebControls.Table tb = new System.Web.UI.WebControls.Table();

            if (dt.Rows.Count > 0)
            {

                Grid.AllowPaging = false;
                Grid.DataSource = dt;
                Grid.DataBind();


                //for (int i = 0; i < Grid.HeaderRow.Cells.Count; i++)
                //{
                //    Grid.HeaderRow.Cells[i].Style.Add("background-color", "#edebeb");
                //    Grid.HeaderRow.Cells[i].Attributes.Add("class", "Headermode");


                //}

                //for (int i = 0; i < Grid.Rows.Count; i++)
                //{
                //    //Apply text style to each Row

                //    Grid.Rows[i].BackColor = System.Drawing.Color.White;


                //    if (i % 2 != 0)
                //    {
                //        Grid.Rows[i].Attributes.Add("class", "textmode");
                //    }
                //    else
                //    {
                //        Grid.Rows[i].Attributes.Add("class", "textmode2");
                //    }

                //}

                //var title = "CENTRAL WAREHOUSING CORPORATION </br>"
                //          +"Principal Place of Business</br>"
                //          + "CENTRAL WAREHOUSE</br>"                    
                //          + "REGISTER OF OUTWARD SUPPLY (TAX INVOICE/BILL OF SUPPLY)</br>";

                System.Web.UI.WebControls.TableCell cell1 = new System.Web.UI.WebControls.TableCell();
                cell1.Text = "<b> CENTRAL WAREHOUSING CORPORATION </b> ";
                System.Web.UI.WebControls.TableRow tr1 = new System.Web.UI.WebControls.TableRow();
                tr1.Cells.Add(cell1);
                tr1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;

                System.Web.UI.WebControls.TableCell cell2 = new System.Web.UI.WebControls.TableCell();
                cell2.Text = "Principal Place of Business";
                System.Web.UI.WebControls.TableRow tr2 = new System.Web.UI.WebControls.TableRow();
                tr2.Cells.Add(cell2);
                tr2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;

                System.Web.UI.WebControls.TableCell cell3 = new System.Web.UI.WebControls.TableCell();
                cell3.Text = "CENTRAL WAREHOUSE";
                System.Web.UI.WebControls.TableRow tr3 = new System.Web.UI.WebControls.TableRow();
                tr3.Cells.Add(cell3);
                tr3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;


                System.Web.UI.WebControls.TableCell cell4 = new System.Web.UI.WebControls.TableCell();
                cell4.Text = "REGISTER OF OUTWARD SUPPLY (TAX INVOICE/BILL OF SUPPLY)";
                System.Web.UI.WebControls.TableRow tr4 = new System.Web.UI.WebControls.TableRow();
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



            //-----------------------------------
            //DataTable table = new DataTable();
            //for (int i = 0; i < Grid.HeaderRow.Cells.Count; i++)
            //    table.Columns.Add(Grid.HeaderRow.Cells[i].Text);
            //// fill rows             
            //foreach (System.Web.UI.WebControls.GridViewRow row in Grid.Rows)
            //{
            //    DataRow dr;
            //    dr = table.NewRow();
            //    for (int i = 0; i < row.Cells.Count; i++)
            //    {
            //        dr[i] = row.Cells[i].Text.Replace("&nbsp;", "");
            //    }
            //    table.Rows.Add(dr);
            //}
            //using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            //{
            //    var title = @"CENTRAL WAREHOUSING CORPORATION"
            //            + Environment.NewLine + "Principal Place of Business"
            //            + Environment.NewLine + "CENTRAL WAREHOUSE"
            //            + Environment.NewLine + Environment.NewLine
            //            + "REGISTER OF OUTWARD SUPPLY (TAX INVOICE/BILL OF SUPPLY)";
            //    exl.MargeCell(title, DynamicExcel.CellAlignment.Middle);

            //}
            //-------------------------------------





            return excelFile;
        }
        
        private string RegisterofOutwardSupplyExcel(List<DSRRegisterOfOutwardSupplyModel> model, decimal InvoiceAmount, decimal CRAmount)
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
                exl.MargeCell("F2:F4", "Nature of Invoice" + Environment.NewLine + "(Resv./Initial Fumigation/General Basic/Over & Above)", DynamicExcel.CellAlignment.Middle);
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

                exl.MargeCell("S2:S4", "PaymentMode", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("T2:T4", "Remarks", DynamicExcel.CellAlignment.Middle);


                for (var i = 65; i < 85; i++)
                {
                    char character = (char)i;
                    string text = character.ToString();
                    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                }

                exl.AddTable<DSRRegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 20, 20, 20, 12, 20, 10, 15, 20, 12, 12, 8, 14, 8, 14, 8, 14, 16, 10, 30 });
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
        private string RegisterofOutwardSupplyExcelCreditDebit(List<DSRRegisterOfOutwardSupplyModelCreditDebit> modelCreditDebit, decimal InvoiceAmount, decimal CRAmount)
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
                exl.MargeCell("F2:F4", "Nature of Invoice" + Environment.NewLine + "(Resv./Initial Fumigation/General Basic/Over & Above)", DynamicExcel.CellAlignment.Middle);
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
                exl.MargeCell("V2:V4", "PaymentMode", DynamicExcel.CellAlignment.Middle);
                    for (var i = 65; i < 87; i++)
                    {
                        char character = (char)i;
                        string text = character.ToString();
                        exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                    }

                    exl.AddTable("A", 6, modelCreditDebit, new[] { 6, 20, 20, 20, 12, 20, 10, 15, 15, 15, 20, 12, 12, 8, 14, 8, 14, 8, 14, 16, 30,32 });
                    var igstamt = modelCreditDebit.Sum(o => o.ITaxAmount);
                    var sgstamt = modelCreditDebit.Sum(o => o.STaxAmount);
                    var cgstamt = modelCreditDebit.Sum(o => o.CTaxAmount);
                    var totalamt = modelCreditDebit.Sum(o => o.Total);
                    exl.AddCell("O" + (modelCreditDebit.Count + 6).ToString(), igstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                    exl.AddCell("Q" + (modelCreditDebit.Count + 6).ToString(), cgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                    exl.AddCell("S" + (modelCreditDebit.Count + 6).ToString(), sgstamt.ToString(), DynamicExcel.CellAlignment.CenterRight);
                    exl.AddCell("T" + (modelCreditDebit.Count + 6).ToString(), totalamt.ToString(), DynamicExcel.CellAlignment.CenterRight);

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

        public void ContainerInvoiceDetailsForPrint(DSRContainerInvoiceReport ObjBulkInvoiceReport)
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
            IList<DSRContainerInvoiceReport> objContainerLst = new List<DSRContainerInvoiceReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objContainerLst.Add(new DSRContainerInvoiceReport()
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

        #region Job Order
        public void JobOrderContainerInvoiceDetailsForPrint(string InvoiceId,int ImpJobOrderId)
        {

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = ImpJobOrderId });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetJobOrderInvoiceDetails", CommandType.StoredProcedure, DParam);
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

        public void GetImportJOInvoiceDetailsForPrint(int ImpJobOrderId)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderId", MySqlDbType = MySqlDbType.Int32, Value = ImpJobOrderId });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("getjoborderinvoicesforPrint", CommandType.StoredProcedure, DParam);
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




        #region DailyCashBook
        public void DailyCashBook(DSR_DailyCashBook ObjDailyCashBook)
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
            IList<DSR_DailyCashBook> LstDailyCashBook = new List<DSR_DailyCashBook>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    

                    LstDailyCashBook.Add(new DSR_DailyCashBook
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
                        TotalOA = Result["TotalOA"].ToString(),
                        
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

        public void DailyCashBookCash(DSR_DailyCashBookCash ObjDailyCashBook)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyCashBook.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyCashBook.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            
            int Status = 0;
           
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyCashBookReportCash", CommandType.StoredProcedure, DParam);
            IList<DSR_DailyCashBookCash> LstDailyCashBook = new List<DSR_DailyCashBookCash>();
            
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstDailyCashBook.Add(new DSR_DailyCashBookCash
                    {

                        ReceiptNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Result["ReceiptDate"].ToString(),
                        PartyName = Result["Party"].ToString(),
                        CodeNo = Result["CodeNo"].ToString(),
                        AdvDeposit = Result["AdvDeposit"].ToString(),
                        ChequeNo = Result["ChequeNo"].ToString(),
                        CashReceiptId = Result["CashReceiptId"].ToString(),
                        IMPSTO = Result["IMPSTO"].ToString(),
                        IMPINS = Result["IMPINS"].ToString(),
                        IMPGRL = Result["IMPGRL"].ToString(),
                        IMPHT = Result["IMPHT"].ToString(),
                        EXPSTO = Result["EXPSTO"].ToString(),
                        EXPINS = Result["EXPINS"].ToString(),
                        EXPGRE = Result["EXPGRE"].ToString(),
                        EXPHTT = Result["EXPHTT"].ToString(),
                        EXPHTN = Result["EXPHTN"].ToString(),
                        BNDSTO = Result["BNDSTO"].ToString(),
                        BNDINS = Result["BNDINS"].ToString(),
                        BNDHT = Result["BNDHT"].ToString(),
                        OTHS = Result["OTHS"].ToString(),
                        MISCT = Result["MISCT"].ToString(),
                        MISCN = Result["MISCN"].ToString(),
                        PCS = Result["PCS"].ToString(),
                        DOC = Result["DOC"].ToString(),
                        ADM = Result["ADM"].ToString(),
                        WET = Result["WET"].ToString(),
                        tdsCol = Result["tdsCol"].ToString(),
                        TaxableAmt = Result["TaxableAmt"].ToString(),
                        CGSTAmt = Result["CGSTAmt"].ToString(),
                        SGSTAmt = Result["SGSTAmt"].ToString(),
                        IGSTAmt = Result["IGSTAmt"].ToString(),
                        RoundUp = Result["RoundUp"].ToString(),
                        GrossAmt = Result["GrossAmt"].ToString(),
                        RecCash = Result["RecCash"].ToString(),
                        RecCheque = Result["RecCheque"].ToString(),
                        RecOnline = Result["RecOnline"].ToString(),
                        TDS = Result["TDS"].ToString(),
                        ADJ = Result["ADJ"].ToString(),
                        RecCrNote = Result["RecCrNote"].ToString(),
                        NetAmt = Result["NetAmt"].ToString(),
                        OpeningBalance = Result["OpeningBalance"].ToString(),
                        BankDeposit = Result["BankDeposit"].ToString(),                        
                        OpeningRSQty = Result["OpeningRSQty"].ToString(),
                        CPurchaseRSQty = Result["CPurchaseRSQty"].ToString(),                       
                        CConsumRSQty = Result["CConsumRSQty"].ToString(),
                        OConsumRSQty = Result["OConsumRSQty"].ToString()

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
        public void MonthlyCashBook(DSR_DailyCashBookCash ObjDailyCashBook)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyCashBook.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyCashBook.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            
            int Status = 0;
           
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
           

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("MonthlyCashBookReport", CommandType.StoredProcedure, DParam);
            IList<DSR_DailyCashBookCash> LstMonthlyCashBook = new List<DSR_DailyCashBookCash>();
            
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstMonthlyCashBook.Add(new DSR_DailyCashBookCash
                    {
                        
                        ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),
                       
                        AdvDeposit = Result["AdvDeposit"].ToString(),                                                
                        IMPSTO = Result["IMPSTO"].ToString(),
                        IMPINS = Result["IMPINS"].ToString(),
                        IMPGRL = Result["IMPGRL"].ToString(),
                        IMPHT = Result["IMPHT"].ToString(),
                        EXPSTO = Result["EXPSTO"].ToString(),
                        EXPINS = Result["EXPINS"].ToString(),
                        EXPGRE = Result["EXPGRE"].ToString(),
                        EXPHTT = Result["EXPHTT"].ToString(),
                        EXPHTN = Result["EXPHTN"].ToString(),
                        BNDSTO = Result["BNDSTO"].ToString(),
                        BNDINS = Result["BNDINS"].ToString(),
                        BNDHT = Result["BNDHT"].ToString(),
                        OTHS = Result["OTHS"].ToString(),
                        MISCT = Result["MISCT"].ToString(),
                        MISCN = Result["MISCN"].ToString(),
                        PCS = Result["PCS"].ToString(),
                        DOC = Result["DOC"].ToString(),
                        ADM = Result["ADM"].ToString(),
                        WET = Result["WET"].ToString(),
                        tdsCol = Result["tdsCol"].ToString(),
                        TaxableAmt = Result["TaxableAmt"].ToString(),
                        CGSTAmt = Result["CGSTAmt"].ToString(),
                        SGSTAmt = Result["SGSTAmt"].ToString(),
                        IGSTAmt = Result["IGSTAmt"].ToString(),
                        RoundUp = Result["RoundUp"].ToString(),
                        GrossAmt = Result["GrossAmt"].ToString(),
                        RecCash = Result["RecCash"].ToString(),
                        RecCheque = Result["RecCheque"].ToString(),
                        RecOnline = Result["RecOnline"].ToString(),
                        TDS = Result["TDS"].ToString(),
                        ADJ = Result["ADJ"].ToString(),
                        RecCrNote = Result["RecCrNote"].ToString(),
                        NetAmt = Result["NetAmt"].ToString(),
                       
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

        #region Monthly SD Book
        public void MonthSDBookReport(DSR_DailyCashBook ObjDailyCashBook)
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
            IList<DSR_DailyCashBook> LstMonthlyCashBook = new List<DSR_DailyCashBook>();
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

                    LstMonthlyCashBook.Add(new DSR_DailyCashBook
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


        public void DailyPdaActivity(DSRDailyPdaActivityReport ObjDailyPdaActivityReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyPdaActivityReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyPdaActivityReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailySdActivityReport", CommandType.StoredProcedure, DParam);
            DSRDailyPdaActivityReport lstRptDailyPdaActivity = new DSRDailyPdaActivityReport();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    lstRptDailyPdaActivity.LstDailyPdaActivityReport.Add(new DSRDailyPdaActivityList
                    {
                        Party = Result["PartyName"].ToString(),
                        partycode = Result["PartyCode"].ToString(),
                        Opening = Convert.ToDecimal(Result["OpeningAmount"]),
                        Deposit = Convert.ToDecimal(Result["DebitAmount"]),
                        Withdraw = Convert.ToDecimal(Result["AdjustAmount"]),
                        Closing = Convert.ToDecimal(Result["UtilizationAmount"]),
                    });
                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstRptDailyPdaActivity;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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


        public void PdSummaryReport(DSRPdSummary ObjPdSummaryReport, int type = 1)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjPdSummaryReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
          
            int Status = 0;
           
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_asOndate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.Int32, Value = type });
            
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PdSummaryReport", CommandType.StoredProcedure, DParam);
            IList<DSRPdSummary> LstPdSummaryReport = new List<DSRPdSummary>();
            
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                   

                    LstPdSummaryReport.Add(new DSRPdSummary
                    {
                        PartyName = Result["PartyName"].ToString(),
                        Amount = Result["Amount"].ToString()

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
            DSRCashReceiptInvoiceLedger CrInvLedgerObj = new DSRCashReceiptInvoiceLedger();

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
                        CrInvLedgerObj.lstLedgerSummary.Add(new DSRCrInvLedgerSummary
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
                        CrInvLedgerObj.lstLedgerDetails.Add(new DSRCrInvLedgerDetails
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
                        CrInvLedgerObj.lstLedgerDetailsFull.Add(new DSRCrInvLedgerFullDetails
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
        public void WorkSlipDetailsForPrintMisc(string PeriodFrom, string PeriodTo, string ImportExport, int Uid = 0)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodFrom) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodTo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Operation", MySqlDbType = MySqlDbType.VarChar, Value = ImportExport });
            LstParam.Add(new MySqlParameter { ParameterName = "in_uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetWorkslipReportMisc", CommandType.StoredProcedure, DParam);
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
        public void WorkSlipDetailsForPrintExport(string PeriodFrom, string PeriodTo, string ImportExport, int Uid = 0)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodFrom) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodTo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Operation", MySqlDbType = MySqlDbType.VarChar, Value = ImportExport });
            LstParam.Add(new MySqlParameter { ParameterName = "in_uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetWorkslipReportExport", CommandType.StoredProcedure, DParam);
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
        public void WorkSlipDetailsForPrint(string PeriodFrom, string PeriodTo, string ImportExport, int Uid = 0)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodFrom) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(PeriodTo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Operation", MySqlDbType = MySqlDbType.VarChar, Value = ImportExport });
            LstParam.Add(new MySqlParameter { ParameterName = "in_uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetWorkslipReportImport", CommandType.StoredProcedure, DParam);
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
        //#region Account Report Export Cargo In General Carting
        //public void GetCargoExport(DSR_CarGenCar objPC)
        //{
        //    IDataParameter[] DParam = { };
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPC.AsOnDate).ToString("yyyy-MM-dd") });
        //    DParam = LstParam.ToArray();
        //    IDataReader Result = DataAccess.ExecuteDataReader("ExpCarGenCarReport", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    int Status = 0;
        //    IList<DSR_ExpCarGen> lstPV = new List<DSR_ExpCarGen>();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            lstPV.Add(new DSR_ExpCarGen
        //            {
        //                EntryNo = Result["EntryNo"].ToString(),
        //                InDate = Result["InDate"].ToString(),
        //                SbNo = (Result["SbNo"]).ToString(),
        //                SbDate = Result["SbDate"].ToString(),
        //                Shed = Result["Shed"].ToString(),
        //                Area = Convert.ToDecimal(Result["Area"]),
        //                NoOfDays = Convert.ToInt32(Result["NoOfDays"]),
        //                NoOfWeek = Convert.ToInt32(Result["NoOfWeek"]),
        //                GeneralAmount = Convert.ToDecimal(Result["GeneralAmount"])
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

        //#endregion

        #region PV Report
        public void GetPVReportImport(DSR_PV objPV)
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

            IList<DSR_ImpPVReport> lstPV = new List<DSR_ImpPVReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new DSR_ImpPVReport
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
                        PackageType= Result["PkgType"].ToString(),
                        DestuffingEntryNo= Result["DestuffingEntryNo"].ToString(),
                        ContainerNo= Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        ImporterName = Result["ImporterName"].ToString(),
                        CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                        GrossDuty = Convert.ToDecimal(Result["GrossDuty"]),
                        BalancePackages = Convert.ToInt32(Result["BalancePackages"])

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
        public void GetPVReportExport(DSR_PV objPV)
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
            IList<DSR_ExpPVReport> lstPV = new List<DSR_ExpPVReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new DSR_ExpPVReport
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
                        LocationName = Result["LocationName"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        ExporterName = Result["ExporterName"].ToString(),
                        CargoDescription = Result["CargoDescription"].ToString(),
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

        public void GetPVReportBond(DSR_PV objPV)
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
            IList<DSR_BondPVReport> lstPV = new List<DSR_BondPVReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new DSR_BondPVReport
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
                DSRDTRExp obj = new DSRDTRExp();
                obj = (DSRDTRExp)DataAccess.ExecuteDynamicSet<DSRDTRExp>("DailyTransactionExp", DParam);
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
        public void CollectionReport(DSR_CollectionReport ObjCollectionReport)
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
            DSR_FinalCollectionReportTotal LstCollectionReport = new DSR_FinalCollectionReportTotal();
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

                    LstCollectionReport.listCollectionReport.Add(new DSR_CollectionReport
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
                        LstCollectionReport.listCollectionReport.Add(new DSR_CollectionReport
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

        //#region Accounts Report for Import Cargo
        //public void GetImpCargo(DSR_PV objPV)
        //{
        //    IDataParameter[] DParam = { };
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = "" });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objPV.GodownId });
        //    DParam = LstParam.ToArray();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetImpCargoReport", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    int Status = 0;
        //    IList<DSR_ImpPVReport> lstPV = new List<DSR_ImpPVReport>();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            lstPV.Add(new DSR_ImpPVReport
        //            {
        //                BOLNo = Result["BOLNo"].ToString(),
        //                DestuffingEntryDate = Result["DestuffingEntryDate"].ToString(),
        //                CFSCode = Result["CFSCode"].ToString(),
        //                CommodityAlias = Result["CommodityAlias"].ToString(),
        //                NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
        //                NoOfUnitsRec = Convert.ToInt32(Result["NoOfUnitsRec"]),
        //                Weight = Convert.ToDecimal(Result["Weight"]),
        //                Area = Convert.ToDecimal(Result["Area"].ToString()),
        //                LocationName = Result["LocationName"].ToString(),
        //                Remarks = Result["Remarks"].ToString(),
        //                Amount = Convert.ToDecimal(Result["Amount"]),
        //                TotWk = Convert.ToInt32(Result["TotWk"]),
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
        //#endregion

        #region Cheque Summary Report
        public void ChequeSummary(DSR_ChequeSummary ObjChequeSummary)
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
            IList<DSR_ChequeSummary> LstChequeSummary = new List<DSR_ChequeSummary>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstChequeSummary.Add(new DSR_ChequeSummary
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
                    LstChequeSummary.Add(new DSR_ChequeSummary
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

        public void ChequeCashDDPOSummary(DSR_CashChequeDDSummary ObjChequeSummary)
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
            IList<DSR_CashChequeDDSummary> LstChequeSummary = new List<DSR_CashChequeDDSummary>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstChequeSummary.Add(new DSR_CashChequeDDSummary
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
                    LstChequeSummary.Add(new DSR_CashChequeDDSummary
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

        #region PV Report Of Empty Container
        public void GetEmptyCont(DSR_PVReport objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PVReportEmptyCont", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<DSR_PVReportImpEmptyCont> lstPV = new List<DSR_PVReportImpEmptyCont>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new DSR_PVReportImpEmptyCont
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
        public void LongStandingEmptyCont(DSR_PVReport objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("LongStandingEmptyContiner", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<DSR_LongStandingEmptyCont> lstPV = new List<DSR_LongStandingEmptyCont>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new DSR_LongStandingEmptyCont
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
        public void GetImpLoadedCont(DSR_PVReport objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("PVReportImpLoadedCont", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<DSR_PVReportImpLoadedCont> lstPV = new List<DSR_PVReportImpLoadedCont>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new DSR_PVReportImpLoadedCont
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
                        CHAName = Result["CHAName"].ToString(),
                        ImporterName = Result["ImporterName"].ToString(),
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
        public void DailyInvoiceReport(DSR_DailyInvReport ObjDailyReport)
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
            IList<DSR_DailyReport> lstPV = new List<DSR_DailyReport>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new DSR_DailyReport
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
            IList<DSRSDSummary> lstPV = new List<DSRSDSummary>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new DSRSDSummary
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        Date = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        Module = Result["InvoiceType"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString(),
                        //BILL = Convert.ToDecimal(Result["MiscExcess"]),
                        GEN = Convert.ToDecimal(Result["GenSpace"]),
                        STO = Convert.ToDecimal(Result["STO"]),
                        INS = Convert.ToDecimal(Result["INS"]),
                        GRE = Convert.ToDecimal(Result["GRE"]),
                        GRL = Convert.ToDecimal(Result["GRL"]),
                        HT = Convert.ToDecimal(Result["HT"]),
                        OTHS = Convert.ToDecimal(Result["OTHS"]),
                        WET = Convert.ToDecimal(Result["WET"]),
                        RCTSEAL = Convert.ToDecimal(Result["RCTSEAL"]),
                        MISC = Convert.ToDecimal(Result["MISC"]),
                        PCS = Convert.ToDecimal(Result["PCS"]),                       
                        CGST = Convert.ToDecimal(Result["CGSTAmt"]),
                        SGST = Convert.ToDecimal(Result["SGSTAmt"]),
                        IGST = Convert.ToDecimal(Result["IGSTAmt"]),
                        RoundUp = Convert.ToDecimal(Result["RoundUp"]),
                        
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
        public void StatementOfEmptyContainer(DSRStatementOfEmptyContainer ObjStatementOfEmptyContainer)
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
            IList<DSRStatementOfEmptyContainer> LstStatementOfEmptyContainer = new List<DSRStatementOfEmptyContainer>();
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

                    LstStatementOfEmptyContainer.Add(new DSRStatementOfEmptyContainer
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
            List<DSRExportRRReport> LstStuffing = new List<DSRExportRRReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    if (LstStuffing.Count <= 0)
                    {
                        LstStuffing.Add(new DSRExportRRReport
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
                            LstStuffing.Add(new DSRExportRRReport
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
            DSRPrintExportRRReport ObjDeliveryOrder = new DSRPrintExportRRReport();
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
                        ObjDeliveryOrder.LstPartyDetails.Add(new DSRPartyDetails
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
                        ObjDeliveryOrder.LstContDetails.Add(new DSRContDetails
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            Loaded = Result["Loaded"].ToString(),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"])

                        });
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstChargesDetails.Add(new DSRChargesDetails
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
                        ObjDeliveryOrder.LstCompDetails.Add(new DSRCompDetails
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
                        ObjDeliveryOrder.LstReceiptDetails.Add(new DSRReceiptDetails
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
            List<DSRPartyForSDDet> LstParty = new List<DSRPartyForSDDet>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstParty.Add(new DSRPartyForSDDet
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
            DSRSDDetailsStatement SDResult = new DSRSDDetailsStatement();
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
                            new DSRSDInvoiceDet
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
            IList<DSRVCCapacityModel> LstVCCapacity = new List<DSRVCCapacityModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstVCCapacity.Add(new DSRVCCapacityModel
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
                        LstVCCapacity.Add(new DSRVCCapacityModel());
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
                        LstVCCapacity[row].Import = 0;//(Result["Import"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Import"]));
                        LstVCCapacity[row].Export = 0; //(Result["Export"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Export"]));
                        LstVCCapacity[row].Empty = 0;// (Result["Empty"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Empty"]));
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
            IList<DSRVCCapacityModel> LstVCCapacity = new List<DSRVCCapacityModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstVCCapacity.Add(new DSRVCCapacityModel
                    {
                        Occupency = (Result["occupency"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["occupency"])),
                        Income = (Result["income"] == DBNull.Value ? 0 : Convert.ToDecimal(Result["income"]))
                    });
                }
                if (LstVCCapacity.Count() < 3)
                {
                    for (var i = LstVCCapacity.Count; i < 3; i++)
                    {
                        LstVCCapacity.Add(new DSRVCCapacityModel());
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
            IList<DSR_TrainList> lstTrainNo = new List<DSR_TrainList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {

                    Status = 1;
                    lstTrainNo.Add(new DSR_TrainList { TrainNo = result["TrainNo"].ToString(), TrainSummaryID = Convert.ToInt32(result["TrainSummaryID"]) });
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
            IList<DSR_TrainDateList> lstTrainDate = new List<DSR_TrainDateList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {

                    Status = 1;
                    lstTrainDate.Add(new DSR_TrainDateList { TrainDate = result["TrainDate"].ToString() });
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
        public void GetLongStandingImpLoadedCont(DSR_LongStandingImpCon ObjContainerBalanceInCFS)
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
            IList<DSR_LongStandingImpConDtl> LstContainerBalanceInCFS = new List<DSR_LongStandingImpConDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstContainerBalanceInCFS.Add(new DSR_LongStandingImpConDtl
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
        public void GetLongStandingImpLoadedCargo(DSR_LongStandingImpCargo ObjContainerBalanceInCFS)
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
            IList<DSR_LongStandingImpCargoDtl> LstContainerBalanceInCFS = new List<DSR_LongStandingImpCargoDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstContainerBalanceInCFS.Add(new DSR_LongStandingImpCargoDtl
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
            List<DSRExportRRReport> LstStuffing = new List<DSRExportRRReport>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    if (LstStuffing.Count <= 0)
                    {
                        LstStuffing.Add(new DSRExportRRReport
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
                            LstStuffing.Add(new DSRExportRRReport
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
            CwcExim.Areas.Export.Models.DSRPrintJOModel objMdl = new CwcExim.Areas.Export.Models.DSRPrintJOModel();
            _DBResponse = new DatabaseResponse();
            List<CwcExim.Areas.Export.Models.DSRPrintJOModelDet> lstJobOrder = new List<CwcExim.Areas.Export.Models.DSRPrintJOModelDet>();
            try
            {

                while (result.Read())
                {
                    Status = 1;
                    lstJobOrder.Add(new CwcExim.Areas.Export.Models.DSRPrintJOModelDet
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
            CwcExim.Areas.Export.Models.DSRPrintJOModel objMdl = new CwcExim.Areas.Export.Models.DSRPrintJOModel();
            _DBResponse = new DatabaseResponse();
            List<CwcExim.Areas.Export.Models.DSRexportjobordersum> lstJobOrder = new List<CwcExim.Areas.Export.Models.DSRexportjobordersum>();
            try
            {

                while (result.Read())
                {
                    Status = 1;
                    lstJobOrder.Add(new CwcExim.Areas.Export.Models.DSRexportjobordersum
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
            DSRCoreData objCoreData = new DSRCoreData();
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
                        LedgerObj.ClosingBalance = Convert.ToDecimal(Result["ClosingBalance"] == DBNull.Value ? 0 : Result["ClosingBalance"]);
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
            IList<DSR_ContainerList> lstContainer = new List<DSR_ContainerList>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstContainer.Add(new DSR_ContainerList
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
        public void ContainerPaymentDetail(DSR_ContainerPaymentInfo Obj)
        {
            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = Obj.CFSCode });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getContainerEnquiry", CommandType.StoredProcedure, DParam);
            //List<DSR_ContainerPaymentDtl> LstMonthlyCashBook = new List<DSR_ContainerPaymentDtl>();
            DSR_ContainerPaymentInfo CPI = new DSR_ContainerPaymentInfo();
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
                        CPI.lstContainerPaymentDtl.Add(new DSR_ContainerPaymentDtl
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
        /* public void TdsReport(TDSReport ObjTDSReport)
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
         }*/
        public void TdsReport(DSR_TDSReport ObjTDSReport)
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
            DSR_TDSMain objTDSMain = new DSR_TDSMain();
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

                    objTDSMain.TDSReportLst.Add(new DSR_TDSReport
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
                    objTDSMain.TDSReportLst.Add(new DSR_TDSReport
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
        public void ImportConIncomeDetail(DSR_ImportConIncome Obj)
        {
            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.VarChar, Value = Obj.FromDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.VarChar, Value = Obj.ToDate });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getImportContainerIncome", CommandType.StoredProcedure, DParam);
            List<DSR_ImportConIncomeDtl> lstImportConIncomeDtl = new List<DSR_ImportConIncomeDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstImportConIncomeDtl.Add(new DSR_ImportConIncomeDtl
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
                        ECC = Convert.ToDecimal(Result["ECC"]),
                        DTF = Convert.ToDecimal(Result["DTF"]),
                        LOL = Convert.ToDecimal(Result["LOL"]),
                        IRR = Convert.ToDecimal(Result["IRR"]),
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
        #endregion

        #region Export Con Income
        public void ExportConIncomeDetail(DSR_ExportConIncome Obj)
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
            List<DSR_ExportConIncomeDtl> lstExportConIncomeDtl = new List<DSR_ExportConIncomeDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstExportConIncomeDtl.Add(new DSR_ExportConIncomeDtl
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
                        Total = Convert.ToDecimal(Result["Total"])
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
        #endregion

        #region Assessment sheet LCL Report
        public void AssessmentSheetLCLDetail(DSR_AssessmentSheetLCL Obj)
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
            List<DSR_AssessmentSheetLCLDtl> lstImportConIncomeDtl = new List<DSR_AssessmentSheetLCLDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstImportConIncomeDtl.Add(new DSR_AssessmentSheetLCLDtl
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
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
        #endregion

        #region Seal Closing Report
        public void SealClosingReport(DSR_SealClosingReport ObjSealClosingReport)
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
            IList<DSR_SealClosingReport> LstSealClosingReport = new List<DSR_SealClosingReport>();
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

                    LstSealClosingReport.Add(new DSR_SealClosingReport
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
        public void EmptyContainerPayment(DSR_EmptyConPayRpt Obj)
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
            List<DSR_EmptyConPayRptDtl> lstEmptyContDtl = new List<DSR_EmptyConPayRptDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstEmptyContDtl.Add(new DSR_EmptyConPayRptDtl
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
            List<DSRReserveSpaceReport> lstEmptyContDtl = new List<DSRReserveSpaceReport>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstEmptyContDtl.Add(new DSRReserveSpaceReport
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
            List<DSR_OutstandingAmountReport> lstEmptyContDtl = new List<DSR_OutstandingAmountReport>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstEmptyContDtl.Add(new DSR_OutstandingAmountReport
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
        public void AssessmentSheetFCLDetail(DSR_AssessmentSheetFCL Obj)
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
            List<DSR_AssessmentSheetFCLDtl> lstImportConIncomeDtl = new List<DSR_AssessmentSheetFCLDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstImportConIncomeDtl.Add(new DSR_AssessmentSheetFCLDtl
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
        #endregion


        #region Rent Income Report
        public void RentIncomeReport(DSR_RentIncomeReportViewModel Obj)
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
            List<DSR_RentIncomeReportViewModel> lstImportConIncomeDtl = new List<DSR_RentIncomeReportViewModel>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstImportConIncomeDtl.Add(new DSR_RentIncomeReportViewModel
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
        public void CargoStockRegister(DSR_CrgStkRgt ObjCargoStockRegister)
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
            DSR_CrgStkRgt _ObjCargoStockRegister = new DSR_CrgStkRgt();
            IList<DSRexportCargoStock> ppgexportCargoStocklst = new List<DSRexportCargoStock>();

            IList<DSRimportCargoStock> ppgimportCargoStocklst = new List<DSRimportCargoStock>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    _ObjCargoStockRegister.ppgexportCargoStocklst.Add(new DSRexportCargoStock
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
                        _ObjCargoStockRegister.ppgimportCargoStocklst.Add(new DSRimportCargoStock
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
        public void TaxZeroInvoiceReport(DSR_DailyCashBook ObjDailyCashBook)
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
            IList<DSR_DailyCashBook> LstDailyCashBook = new List<DSR_DailyCashBook>();
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

                    LstDailyCashBook.Add(new DSR_DailyCashBook
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
        public void ExportTHCRRReport(DSR_ThcRrReport vm)
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
            List<DSR_ThcRrReport> lstExportTHCRRdtl = new List<DSR_ThcRrReport>();

            _DBResponse = new DatabaseResponse();
            try
            {

                while (Result.Read())
                {
                    Status = 1;

                    lstExportTHCRRdtl.Add(new DSR_ThcRrReport
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
                        TotalRRGstAmount = Convert.ToDecimal(Result["RRTotalGSTAmt"]),
                        THCAmount = Convert.ToDecimal(Result["THCAmount"]),
                        TotalTHGstAmount = Convert.ToDecimal(Result["THCTotalGSTAmt"]),
                        GrandTotal = Convert.ToDecimal(Result["Total"])
                    });

                }


                lstExportTHCRRdtl.Add(new DSR_ThcRrReport
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
                    TotalRRGstAmount = lstExportTHCRRdtl.Sum(x => x.TotalRRGstAmount),
                    THCAmount = lstExportTHCRRdtl.Sum(x => x.THCAmount),
                    TotalTHGstAmount = lstExportTHCRRdtl.Sum(x => x.TotalTHGstAmount),
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
        public void ImportTHCRRReport(DSR_ThcRrReport vm)
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
            List<DSR_ThcRrReport> lstExportTHCRRdtl = new List<DSR_ThcRrReport>();

            _DBResponse = new DatabaseResponse();
            try
            {

                while (Result.Read())
                {
                    Status = 1;

                    lstExportTHCRRdtl.Add(new DSR_ThcRrReport
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
                        TotalRRGstAmount = Convert.ToDecimal(Result["RRTotalGSTAmt"]),
                        THCAmount = Convert.ToDecimal(Result["THCAmount"]),
                        TotalTHGstAmount = Convert.ToDecimal(Result["THCTotalGSTAmt"]),
                        GrandTotal = Convert.ToDecimal(Result["Total"])
                    });

                }


                lstExportTHCRRdtl.Add(new DSR_ThcRrReport
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
                    TotalRRGstAmount = lstExportTHCRRdtl.Sum(x => x.TotalRRGstAmount),
                    THCAmount = lstExportTHCRRdtl.Sum(x => x.THCAmount),
                    TotalTHGstAmount = lstExportTHCRRdtl.Sum(x => x.TotalTHGstAmount),
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
        public void GetLongStandingExportLoadedCargo(DSR_LongStandingExportCargo vm)
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
            IList<DSR_LongStandingExportCargoDtl> lstLongStandingExportCargoDetails = new List<DSR_LongStandingExportCargoDtl>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    lstLongStandingExportCargoDetails.Add(new DSR_LongStandingExportCargoDtl
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

        #region  Excel SD reports

        public void PartyWiseSdReportSummary(string FromDate,  string ToDate)
        {
            int Status = 0;
            DateTime dtfrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("SDSummaryExcel", CommandType.StoredProcedure, DParam);
            List<DSR_SD_EXCELREPORT> lstSdexcellist = new List<DSR_SD_EXCELREPORT>();
            int i = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstSdexcellist.Add(new DSR_SD_EXCELREPORT
                    {
                          SLno = Convert.ToInt32(Result["SlNo"]),
                        // InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                     //   PayeeId = Convert.ToInt32 (Result["PayeeId"]),
                        PayeeName = Convert.ToString(Result["PayeeName"]),
                        Opening = Convert.ToDecimal(Result["Opening"]),
                        SDBalance = Convert.ToDecimal(Result["SDBalance"]),
                        Creadit = Convert.ToDecimal(Result["Creadit"]),


                        IMPSTO = Convert.ToDecimal(Result["IMPSTO"]),
                        IMPINS = Convert.ToDecimal(Result["IMPINS"]),
                        IMPHT = Convert.ToDecimal(Result["IMPHT"]),
                        IMPGRL = Convert.ToDecimal(Result["IMPGRL"]),
                  


                        EXPSTO = Convert.ToDecimal(Result["EXPSTO"]),
                        EXPINS = Convert.ToDecimal(Result["EXPINS"]),
                        EXPHTT = Convert.ToDecimal(Result["EXPHTT"]),
                        EXPGRE = Convert.ToDecimal(Result["EXPGRE"]),
                      


                        BNDSTO = Convert.ToDecimal(Result["BNDSTO"]),                        
                        BNDINS = Convert.ToDecimal(Result["BNDINS"]),
                        BNDHT = Convert.ToDecimal(Result["BNDHT"]),

                        WET = Convert.ToDecimal(Result["WET"]),
                        DOC = Convert.ToDecimal(Result["DOC"]),
                        MISCT = Convert.ToDecimal(Result["MISCT"]),

                        PCS = Convert.ToDecimal(Result["PCS"]),
                        RCTSEAL = Convert.ToDecimal(Result["RCTSEAL"]),
                        OTHERS = Convert.ToDecimal(Result["OTHS"]),
                        TotalTexable = Convert.ToDecimal(Result["TotalTexable"]),


                        //PayeeCode = Result["PayeeCode"].ToString(),
                        CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                        SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                        IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                        Total = Convert.ToDecimal(Result["Total"]),
                        RoundUp = Convert.ToDecimal(Result["RoundUp"]),
                        Closing = Convert.ToDecimal(Result["Closing"]),


                    });
                    DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    decimal InvoiceAmount = 0, CRAmount = 0;

                    _DBResponse.Data = RegisterPartyWiseSdReportSummary(lstSdexcellist, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"));

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

        private string RegisterPartyWiseSdReportSummary(List<DSR_SD_EXCELREPORT> DSRSDSummaryExcel, string date1, string date2)
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

                exl.MargeCell("A3:H3", "ICD-DASHRATH, VADODARA  ", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A4:H4", "SUMMARY OF SD-ACCOUNT   "+ typeOfValue, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A5:H5", typeOfValue, DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("A4:F4", "DETAILS OF ORIGINAL INOICE", DynamicExcel.CellAlignment.Middle);
                // exl.MargeCell("G4:O4", "DETAILS OF CREDIT NOTE ISSUED", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A5:A6", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B5:B6", "NAME OF THE PARTY", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C5:C6", "OPENING BALANCE", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D5:D6", "SECURITY DEPOSIT", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E5:E6", "CREDIT.", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("F5:I5", "IMPORT.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F6:F6", "STG.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G6:G6", "INS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H6:H6", "H&T", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I6:I6", "GRL.", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("J5:M5", "EXPORT", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J6:J6", "STG.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K6:K6", "INS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("L6:L6", "H&T", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M6:M6", "GRE.", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("N5:P5", "BOND", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("N6:N6", "STG.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("P6:P6", "H&T", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O6:O6", "INS", DynamicExcel.CellAlignment.Middle);            

                exl.MargeCell("Q5:Q6", "WB.CH", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("R5:R6", "DOC. CH.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("S5:S6", "MISC", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("T5:T6", "PCS", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("U5:U6", "RCT SEALING", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("V5:V6", "OTHERS", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("W5:W6", "TAXABLE AMT", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("X5:X6", "SGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Y5:Y6", "CGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Z5:Z6", "IGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AA5:AA6", "Round Up", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AB5:AB6", "TOTAL", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("AC5:AC6", "CLOSING BALANCE", DynamicExcel.CellAlignment.Middle);


               


                //for (var i = 65; i < 86; i++)
                //{
                //    char character = (char)i;
                //    string text = character.ToString();
                //    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                //}

                exl.AddTable("A", 7, DSRSDSummaryExcel, new[] { 6, 20, 20, 30, 12, 20, 20, 15, 15, 15, 14, 12, 12, 8, 14, 10, 10, 10, 12, 18, 18,18,18,18,18,25,26,27,22 });
                //var TaxAmt = PPGAssesmentSheetlcl.Sum(o => o.Total);

                //var igstamt = PPGAssesmentSheetlcl.Sum(o => o.IGST);
                //var sgstamt = PPGAssesmentSheetlcl.Sum(o => o.SGST);
                //var cgstamt = PPGAssesmentSheetlcl.Sum(o => o.CGST);
                //var Area = PPGAssesmentSheetlcl.Sum(o => o.Area);
                //var Weight = PPGAssesmentSheetlcl.Sum(o => o.GrossWt);
                //var ENTRYCHG = PPGAssesmentSheetlcl.Sum(o => o.ENT);
                //var HANDLING = PPGAssesmentSheetlcl.Sum(o => o.HND);


                //var Sto = PPGAssesmentSheetlcl.Sum(o => o.STO);
                //var INS = PPGAssesmentSheetlcl.Sum(o => o.INS);
                //var Oti = PPGAssesmentSheetlcl.Sum(o => o.OTI);
                //var Haz = PPGAssesmentSheetlcl.Sum(o => o.HAZ);
                //var BOEValueDuty = PPGAssesmentSheetlcl.Sum(o => o.BOEValueDuty);
                //exl.AddCell("F" + (PPGAssesmentSheetlcl.Count + 7).ToString(), "Total", DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("G" + (PPGAssesmentSheetlcl.Count + 7).ToString(), BOEValueDuty.ToString(), DynamicExcel.CellAlignment.TopLeft);

                //exl.AddCell("I" + (DSRSDSummaryExcel.Count + 7).ToString(), Area.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("J" + (DSRSDSummaryExcel.Count + 7).ToString(), Weight.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("L" + (DSRSDSummaryExcel.Count + 7).ToString(), ENTRYCHG.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("M" + (DSRSDSummaryExcel.Count + 7).ToString(), HANDLING.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("N" + (DSRSDSummaryExcel.Count + 7).ToString(), Sto.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("O" + (DSRSDSummaryExcel.Count + 7).ToString(), INS.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("P" + (DSRSDSummaryExcel.Count + 7).ToString(), Oti.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("Q" + (DSRSDSummaryExcel.Count + 7).ToString(), Haz.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("R" + (DSRSDSummaryExcel.Count + 7).ToString(), cgstamt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("S" + (DSRSDSummaryExcel.Count + 7).ToString(), sgstamt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("T" + (DSRSDSummaryExcel.Count + 7).ToString(), igstamt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("U" + (DSRSDSummaryExcel.Count + 7).ToString(), TaxAmt.ToString(), DynamicExcel.CellAlignment.TopLeft);

                //exl.AddCell("V" + (DSRSDSummaryExcel.Count + 7).ToString(), Haz.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("W" + (DSRSDSummaryExcel.Count + 7).ToString(), cgstamt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("X" + (DSRSDSummaryExcel.Count + 7).ToString(), sgstamt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("Y" + (DSRSDSummaryExcel.Count + 7).ToString(), igstamt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("Z" + (DSRSDSummaryExcel.Count + 7).ToString(), TaxAmt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("AA" + (DSRSDSummaryExcel.Count + 7).ToString(), TaxAmt.ToString(), DynamicExcel.CellAlignment.TopLeft);
                //exl.AddCell("AB" + (PPGAssesmentSheetlcl.Count + 7).ToString(), TaxAmt.ToString(), DynamicExcel.CellAlignment.TopLeft);


                exl.Save();
            }
            return excelFile;
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

            List<DSR_CashBookWithSDExcel> model = new List<DSR_CashBookWithSDExcel>();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    model = (from DataRow dr in dt.Rows
                             select new DSR_CashBookWithSDExcel()
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
        private string CashBookWithSDExcelExcel(List<DSR_CashBookWithSDExcel> model, decimal InvoiceAmount, decimal CRAmount)
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
                exl.AddTable<DSRRegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16, 12, 30, 14, 14, 14, 14, 14, 40 });*/
                exl.AddTable<DSR_CashBookWithSDExcel>("A", 3, model, new[] { 6, 20, 20, 20, 12, 20, 10, 15, 20, 12, 12, 8, 14, 8, 14, 8, 14, 16, 10, 30, 14, 40, 14, 40, 14, 14, 12, 20, });
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
                            AdvanceAmountAdjust = Convert.ToDecimal(dr["AdvanceAdjust"]),
                            TotalPaid = Convert.ToDecimal(dr["TotalPaid"]),

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
                            EmdAmount = Convert.ToDecimal(dr["EmdAmount"]),
                            AdvanceAmountAdjust = Convert.ToDecimal(dr["AdvanceAdjust"]),
                            TotalPaid = Convert.ToDecimal(dr["TotalPaid"]),
                            EMDAmountAdjust = Convert.ToDecimal(dr["AdjustEmdAmount"]),
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
                            Size = Convert.ToString(dr["Size"]),
                            InDate = Convert.ToString(dr["InDate"]),
                            Shed = Convert.ToString(dr["GodownName"]),
                            Area = Convert.ToDecimal(dr["Area"]),
                            Pkg = Convert.ToDecimal(dr["Area"]),
                            Weight = Convert.ToDecimal(dr["Weight"]),
                            Bidamount = Convert.ToDecimal(dr["BidAmount"]),
                            valueCharge = Convert.ToDecimal(dr["ValuesCharges"]),
                            AuctionCharge = Convert.ToDecimal(dr["AuctionCharges"]),
                            MiscCharge = Convert.ToDecimal(dr["MiscCharges"]),
                            CustomDuty = Convert.ToDecimal(dr["CustomDuty"]),
                            CwcShare = Convert.ToDecimal(dr["CWCShare"]),
                            Remarks = Convert.ToString(dr["Remarks"])

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

        #region PaymentVoucher Report (Imprest / Temporary Advanced)        
        public void GetPaymentVoucherReport(string Fdt, string Tdt, string Purpose)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDt", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(Fdt).ToString("yyyyMMdd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDt", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(Tdt).ToString("yyyyMMdd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Value = Purpose });

            DParam = LstParam.ToArray();
            Result = DataAccess.ExecuteDataSet("GetPaymentVoucherReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result != null && Result.Tables[0].Rows.Count > 0)
                {
                    Status = 1;
                    
                }
                
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                     _DBResponse.Data = DataTableToJSONWithStringBuilder(Result.Tables[0]); ;                  
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new System.Text.StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }


        #endregion



        #region Import Container Income(Seci)
        public void ImportConIncomeSeciDetail(DSR_ImportSeciIncRpt Obj)
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
            List<DSR_ImportSeciIncRpt> lstContDtl = new List<DSR_ImportSeciIncRpt>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstContDtl.Add(new DSR_ImportSeciIncRpt()
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
        public void ExportConIncomeSeciDetail(DSR_ExportSeciIncRpt Obj)
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
            List<DSR_ExportSeciIncRpt> lstContDtl = new List<DSR_ExportSeciIncRpt>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstContDtl.Add(new DSR_ExportSeciIncRpt()
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

        public void ContainerBalanceInCFS(DSRContainerBalanceInCFS ObjContainerBalanceInCFS)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjContainerBalanceInCFS.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjContainerBalanceInCFS.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = PeriodTo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ContainerBalanceInCFSReport", CommandType.StoredProcedure, DParam);
            IList<DSRContainerBalanceInCFS> LstContainerBalanceInCFS = new List<DSRContainerBalanceInCFS>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstContainerBalanceInCFS.Add(new DSRContainerBalanceInCFS
                    {



                        CFsCode = Result["CFSCode"].ToString(),

                        ContainerNo = Result["ContainerNo"].ToString(),

                        Size = Result["Size"].ToString(),
                        Type = Result["ContainerType"].ToString(),
                        DaysAtCfs = Result["Days"].ToString(),
                        EntryDate = Result["EntryDate"].ToString(),
                        ShippingLineName = Result["ShippingLineName"].ToString(),
                        Rotation = Result["Rotation"].ToString()

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


        public void DeStuffingReport(DeStuffingReport ObjDeStuffingReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDeStuffingReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDeStuffingReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            IDataReader Result = DataAccess.ExecuteDataReader("DeStuffingReport", CommandType.StoredProcedure, DParam);
            IList<DeStuffingReport> LstDeStuffingReport = new List<DeStuffingReport>();
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

                    LstDeStuffingReport.Add(new DeStuffingReport
                    {


                        Date = Result["Date"].ToString(),
                        BOEorBl = Result["BOEorBl"].ToString(),
                        Party = Result["Party"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Commodity = Result["Commodity"].ToString(),
                        NoOfPackage = Result["NoOfPackage"].ToString(),
                        WT = Result["WT"].ToString(),
                        GoDown = Result["GoDown"].ToString(),
                        Location = Result["Location"].ToString()

                        //value = Result["value"].ToString()

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDeStuffingReport;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void CargoInstockReport(CargoInStockReport ObjCargoInStockReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjCargoInStockReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjCargoInStockReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            IDataReader Result = DataAccess.ExecuteDataReader("CargoInStockReport", CommandType.StoredProcedure, DParam);
            IList<CargoInStockReport> LstDeStuffingReport = new List<CargoInStockReport>();
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

                    LstDeStuffingReport.Add(new CargoInStockReport
                    {


                        Date = Result["Date"].ToString(),
                        BOEorBl = Result["BOEorBl"].ToString(),
                        Party = Result["Party"].ToString(),

                        Commodity = Result["Commodity"].ToString(),
                        NoOfPackage = Result["NoOfPackage"].ToString(),
                        WT = Result["WT"].ToString(),
                        GoDown = Result["GoDown"].ToString(),
                        Location = Result["Location"].ToString(),
                        CHAName = Result["CHAName"].ToString()
                        //ContainerNo = Result["ContainerNo"].ToString(),
                        //value = Result["value"].ToString()

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDeStuffingReport;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void ContainerMovementRegister(ContainerMovementRegister ObjContainerMovementRegister)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjContainerMovementRegister.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjContainerMovementRegister.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            IDataReader Result = DataAccess.ExecuteDataReader("ContainerMovementRegister", CommandType.StoredProcedure, DParam);
            IList<ContainerMovementRegister> LstDailyPdaActivityReport = new List<ContainerMovementRegister>();
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

                    LstDailyPdaActivityReport.Add(new ContainerMovementRegister
                    {
                        Date = Result["Date"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        Type = Result["Type"].ToString(),
                        LoadedOrEmpty = Result["LoadedOrEmpty"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        Party = Result["Party"].ToString(),
                        LoadedOrEmpty1 = Result["LoadedOrEmpty"].ToString(),
                        InOrOut = Result["InOrOut"].ToString(),
                        ImportExport = Result["OperationType"].ToString()

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


        #region TDS Statement
        public void TdsStatement(string PeriodFrom, string PeriodTo)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(PeriodFrom).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(PeriodTo).ToString("yyyy/MM/dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("TDSStatement", CommandType.StoredProcedure, DParam);
            IList<DSRTDSStatement> LstTDS = new List<DSRTDSStatement>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstTDS.Add(new DSRTDSStatement
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Result["PartyName"].ToString(),
                        ReceiptDate = Result["ReceiptDate"].ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString(),
                        TDSCol = 0,//Convert.ToDecimal(Result["TDSCol"]),
                        CWCTDS = 0,//Convert.ToDecimal(Result["CWCTDS"]),
                        HTTDS = 0,//Convert.ToDecimal(Result["HTTDS"]),
                        TDS = Convert.ToDecimal(Result["TDS"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstTDS;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        #region Bulk Credit Note
        public void GetBulkCreditNoteReport(string PeriodFrom, string PeriodTo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("BulkCRNote", CommandType.StoredProcedure, DParam);
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

        #region Bulk Debit Note
        public void GetBulkDebitNoteReport(string PeriodFrom, string PeriodTo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("BulkDRNote", CommandType.StoredProcedure, DParam);
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

        #region Debtor Report
        public void DebtorReport(DebtorReport ObjDebtorReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDebtorReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDebtorReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DebtorReport", CommandType.StoredProcedure, DParam);
            List<DebtorReport> LstDebtorReport = new List<DebtorReport>();
            EximTraderWithInvoice obj = new EximTraderWithInvoice();
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

                    LstDebtorReport.Add(new DebtorReport
                    {
                        InvoiceNumber = Result["InvoiceNumber"].ToString(),
                        Date = Convert.ToDateTime(Result["InvoiceDate"] == DBNull.Value ? "N/A" : Result["InvoiceDate"]).ToString("dd/MM/yyyy"),
                        //Result["Date"].ToString(),
                        Values = Result["Value"].ToString(),
                        ClosingBalance = Result["ClosingBalance"].ToString()
                        //SAC = Result["SAC"].ToString(),
                        //InvoiceNumber = Result["InvoiceNo"].ToString(),
                        //Date = Result["Date"].ToString(),
                        //CGST = Result["CGSTAmt"].ToString(),
                        //SGST = Result["SGSTAmt"].ToString(),
                        //IGST = Result["IGSTAmt"].ToString(),
                        //TotalValue = Result["TotalValue"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        obj.EximTraderName = Result["EximTraderName"].ToString();
                        obj.GstNo = Result["GSTNo"].ToString();
                    }
                }
                if (Status == 1)
                {
                    LstDebtorReport.ForEach(m =>
                    {
                        m.GstNo = obj.GstNo;
                        m.PartyName = obj.EximTraderName;

                    });


                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDebtorReport;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void GetEximTraderWithInvoice()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("EximTraderWithInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<EximTraderWithInvoice> LstEximTraderWithInvoice = new List<EximTraderWithInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEximTraderWithInvoice.Add(new EximTraderWithInvoice
                    {
                        EximTraderId = Convert.ToInt32(Result["EximTraderId"].ToString()),
                        EximTraderName = Result["EximTraderName"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEximTraderWithInvoice;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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


        #region service code wise Invoice Details
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
            List<DSRSACList> LstSAC = new List<DSRSACList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSAC.Add(new DSRSACList
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

        public void ServiceCodeWiseInvDtls(DSRServiceCodeWiseInvDtls ObjServiceCodeWiseInvDtls)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjServiceCodeWiseInvDtls.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjServiceCodeWiseInvDtls.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            
            int Status = 0;
            
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
           

            LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ServiceCodeWiseInvDtls", CommandType.StoredProcedure, DParam);
            List<DSRServiceCodeWiseInvDtls> LstInvoiceReportDetails = new List<DSRServiceCodeWiseInvDtls>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                   
                    LstInvoiceReportDetails.Add(new DSRServiceCodeWiseInvDtls
                    {
                        SAC = Result["SAC"].ToString(),
                        InvoiceNumber = Result["InvoiceNo"].ToString(),
                        Date = Result["Date"].ToString(),
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

        #region InventoryReportCargoContainer
        public void InventoryReportCargoContainer(DSRInventoryReportModel ObjInventoryReportModel)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjInventoryReportModel.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjInventoryReportModel.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
           
            int Status = 0;
           
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = PeriodTo });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ContainerCargoInventoryReport", CommandType.StoredProcedure, DParam);
            DSRInventoryReportModel objInventoryReportModel = new DSRInventoryReportModel();
            
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    objInventoryReportModel.LstInventoryReportContainer.Add(new DSRInventoryReportContainer
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        Party = Result["Party"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        Type = Result["CargoType"].ToString(),
                        Commodity = Result["Commodity"].ToString(),
                        DaysAtCFS = Result["DaysAtCFS"].ToString()

                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInventoryReportModel.LstInventoryReportCargo.Add(new DSRInvenontoryReportCargo
                        {                           
                            Party = Convert.ToString(Result["EximTraderName"]),
                            Cargo = Convert.ToString(Result["CommodityName"]),
                            GodownNo = Convert.ToString(Result["GodownName"]),
                            Location = Result["LocationName"].ToString(),
                            DaysAtCFS = Result["DaysAtCFS"].ToString()
                        });

                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objInventoryReportModel;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        #region Container Master Register
        public void ContainerMasterRegister(string PeriodFrom, string PeriodTo)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(PeriodFrom).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(PeriodTo).ToString("yyyy/MM/dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ContainerMstReg", CommandType.StoredProcedure, DParam);
            IList<ContainerMstReg> LstCont = new List<ContainerMstReg>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCont.Add(new ContainerMstReg
                    {
                        EntryDate = Result["EntryDate"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        ContainerType = Result["ContainerType"].ToString(),
                        ShippingLine = Result["ShippingLineName"].ToString(),
                        DestuffingDate = Result["DestuffingDate"].ToString(),
                        DeliveryDate = Result["DeliveryDate"].ToString(),
                        ReceivedDate = Result["ReceivedDate"].ToString(),
                        Stuffingdate = Result["Stuffingdate"].ToString(),
                        EExitDate = Result["EExitDate"].ToString(),
                        OExitDate = Result["OExitDate"].ToString(),
                        Utilization = Result["Utilization"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCont;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetExpLoadedContrOut", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<LoadedContrOutList> LstLoadedContr = new List<LoadedContrOutList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstLoadedContr.Add(new LoadedContrOutList
                    {
                        ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"]).ToString(),
                        Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                        ContainerNo = (Result["ContainerNo"] == null ? "" : Result["ContainerNo"]).ToString(),
                        Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                        Seal = (Result["Seal"] == null ? "" : Result["Seal"]).ToString(),
                        Weight = (Result["Weight"] == null ? "" : Result["Weight"]).ToString()
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

        public void StuffingRegister(StuffingRegister ObjStuffingRegister)
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
            IList<StuffingRegister> LstStuffingRegister = new List<StuffingRegister>();
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

                    LstStuffingRegister.Add(new StuffingRegister
                    {



                        Date = Result["RequestDate"].ToString(),

                        CfsCode = Result["CFSCode"].ToString(),

                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),

                        ExporterName = Result["ExporterName"].ToString(),
                        ShippingLineName = Result["ShippingLine"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        Cargo = Result["Cargo"].ToString(),
                        NoOfUnits = Result["NoOfUnits"].ToString(),
                        shippingBillNo = Result["ShippingBillNo"].ToString(),
                        shippingBillDate = Result["ShippingDate"].ToString(),

                        shippingBillAndDate = Result["ShippingBillNo"].ToString() + " " + Result["ShippingDate"].ToString(),
                        pod = Result["POD"].ToString(),
                        Fob = Result["Fob"].ToString(),
                        Weight = Result["GrossWeight"].ToString(),
                        StfRegisterNo = Result["StuffingReqNo"].ToString() + " " + Result["StuffingRequestDate"].ToString()
                        // StfRegisterDate = Result["POD"].ToString()


                    });
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

        #region  Stuffing Request
        public void GetStuffingReqReport(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingReqReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<StuffingRequestList> LstStuffingRequest = new List<StuffingRequestList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffingRequest.Add(new StuffingRequestList
                    {
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
                        Exporter = Result["Exporter"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        Units = Convert.ToString(Result["Units"]),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffingRequest;
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

        #region BTT
        public void GetBTTDetails(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("BTTReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<BTT> LstBTT = new List<BTT>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstBTT.Add(new BTT
                    {
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
                        BTTCargoEntryDate = Result["BTTCargoEntryDate"].ToString(),
                        CommName = Result["CommName"].ToString(),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                        GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                        ENoOfUnits = Convert.ToInt32(Result["ENoOfUnits"]),
                        EGrossWeight = Convert.ToDecimal(Result["EGrossWeight"]),
                        BTTQuantity = Convert.ToInt32(Result["BTTQuantity"]),
                        BTTWeight = Convert.ToDecimal(Result["BTTWeight"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstBTT;
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
        public void GetCargoDetBTTPrintById(int Id)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CarId", MySqlDbType = MySqlDbType.Int32, Value = Id });
            //    lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetCargoDetBTTByIdPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            DSR_BTTCargoDetPrint objBTTCargo = new DSR_BTTCargoDetPrint();
            try
            {


                while (Result.Read())
                {
                    Status = 1;
                    objBTTCargo.ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]);
                    objBTTCargo.ShippingBillDate = Convert.ToString(Result["ShippingBillDate"]);
                    objBTTCargo.CartingDate = Convert.ToString(Result["CartingDate"]);
                    objBTTCargo.DeliveryDate = Convert.ToString(Result["DeliveryDate"]);
                    objBTTCargo.NoOfDays = Convert.ToInt32(Result["NoOfDays"]);
                    objBTTCargo.NoOfWeeks = Convert.ToInt32(Result["NoOfWeeks"]);
                    objBTTCargo.CommodityName = Convert.ToString(Result["CommodityName"]);
                    objBTTCargo.NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]);
                    objBTTCargo.GrossWeight = Convert.ToDecimal(Result["GrossWeight"]);
                    objBTTCargo.Fob = Convert.ToDecimal(Result["Fob"]);
                    objBTTCargo.Area= Convert.ToDecimal(Result["BTTSqm"]);
                    objBTTCargo.CargoType = Convert.ToString(Result["CargoType"]);
                    objBTTCargo.ODCType = Convert.ToString(Result["ODCType"]);


                }


                if (Status == 1)
                {
                    _DBResponse.Data = objBTTCargo;
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

        #region Shipping line cha importer wise daily activity report

        public void ShippingLChaImpList()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("SlineChaImpList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<SlineImpCHAList> LstSlineImpCHAList = new List<SlineImpCHAList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSlineImpCHAList.Add(new SlineImpCHAList
                    {
                        id = Result["id"].ToString(),
                        Name = Result["Name"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSlineImpCHAList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

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
        public void SlineChaImpDailyActivity(SlineChaImpDailyActivity ObjSlineChaImpDailyActivity)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjSlineChaImpDailyActivity.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            // DateTime dtTo = DateTime.ParseExact(ObjSlineChaImpDailyActivity.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_partyId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjSlineChaImpDailyActivity.EximTraderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImportExport", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ObjSlineChaImpDailyActivity.ImportExport });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("SlineChaImpListDailyReport", CommandType.StoredProcedure, DParam);
            IList<SlineChaImpDailyActivity> LstSlineChaImpDailyActivity = new List<SlineChaImpDailyActivity>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstSlineChaImpDailyActivity.Add(new SlineChaImpDailyActivity
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        Commodity = Result["CommodityName"].ToString(),
                        CargoDescription = Result["CargoDescription"].ToString(),
                        VesselNo = Result["Vessel"].ToString(),
                        VoyageNo = Result["Voyage"].ToString(),
                        RotationNo = Result["Rotation"].ToString(),
                        LineNo = Result["LineNo"].ToString(),
                        GateInNo = Result["gateinNO"].ToString(),
                        GateInDate = Result["GateEntryDateTime"].ToString(),
                        GateOutNo = Result["gateExitNo"].ToString(),
                        GateOutDate = Result["GateExitDateTime"].ToString()




                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSlineChaImpDailyActivity;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        #region Monthly Performance Report


        public List<MonthlyPerformaceReport> GetMonthlyPerformanceReportDataToPrint(int monthNo, int yearNo)
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
            List<DSRPartyWiseLedgerList> LstPartyLedger = new List<DSRPartyWiseLedgerList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPartyLedger.Add(new DSRPartyWiseLedgerList
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
        

        #endregion


        #region Chemical Stock Report
        public void GetChemicalStockReport(DSRChemicalStock objPV)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objPV.AsOnDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChemicalId", MySqlDbType = MySqlDbType.Int32, Value = objPV.ChemicalId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChemicalName", MySqlDbType = MySqlDbType.VarChar, Value =objPV.ChemicalName });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ChemicalStockReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            IList<DSRStock> lstPV = new List<DSRStock>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPV.Add(new DSRStock
                    {
                        ChemicalName = Result["ChemicalName"].ToString(),
                        BatchNo = Result["BatchNo"].ToString(),
                        ExpiryDate = Result["ExpiryDate"].ToString(),

                        Stock = Convert.ToDecimal(Result["Stock"])
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
        public void GetCertificateForPreview(String InvoiceNo)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DA.ExecuteDataReader("GetFumigationCertificate", CommandType.StoredProcedure, DParam);
            DSRPestCertificate ObjIssueSlip = new DSRPestCertificate();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjIssueSlip.CertificateNo = Result["CertificateNo"].ToString();
                    ObjIssueSlip.IssueDate = Result["IssueDate"].ToString();
                    ObjIssueSlip.GoodsDesc = Result["GoodsDesc"].ToString();
                    ObjIssueSlip.Quantity = Result["Quantity"].ToString();
                    ObjIssueSlip.DistingMarks = Result["DistingMarks"].ToString();
                    ObjIssueSlip.ContainerNo = Result["ContainerNo"].ToString();
                    ObjIssueSlip.PortLoading = Result["PortLoading"].ToString();
                    ObjIssueSlip.NameOfVessel = Result["NameOfVessel"].ToString();
                    ObjIssueSlip.Destination = Result["Destination"].ToString();
                    ObjIssueSlip.Exporter = Result["Exporter"].ToString();
                    ObjIssueSlip.Consignee = Result["Consignee"].ToString();
                    ObjIssueSlip.NameOfFumigant = Result["NameOfFumigant"].ToString();
                    ObjIssueSlip.DateOfFumi = Result["DateOfFumi"].ToString();
                    ObjIssueSlip.Place = Result["Place"].ToString();
                    ObjIssueSlip.Dosage = Result["Dosage"].ToString();
                    ObjIssueSlip.Duration = Result["Duration"].ToString();
                    ObjIssueSlip.Temp = Result["Temp"].ToString();
                    ObjIssueSlip.GasTight = Result["GasTight"].ToString();
                    ObjIssueSlip.PackType = Result["PackType"].ToString();
                    ObjIssueSlip.FamugationDate = Convert.ToString(Result["DateOfFumigation"]);
                    ObjIssueSlip.PortOfDischarge = Convert.ToString(Result["PortOfDisc"]);
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
        #endregion

        #region PCS Report
        public void GetPCSReport(DSRPcsReportHeader ObjDailyPdaActivityReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyPdaActivityReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyPdaActivityReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChemicalType", MySqlDbType = MySqlDbType.VarChar, Value = ObjDailyPdaActivityReport.MbrType });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPCSReport", CommandType.StoredProcedure, DParam);
          //  DSRPcsReportHeader lstRptDailyPdaActivity = new DSRPcsReportHeader();
            IList<DSRPcsReport> lstRptDailyPdaActivity = new List<DSRPcsReport>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    lstRptDailyPdaActivity.Add(new DSRPcsReport
                    {
                        CertificateNo = Result["CertificateNo"].ToString(),
                        Place = Result["Place"].ToString(),
                        IssueDate = Result["IssueDate"].ToString(),
                        Chemical = Result["Chemical"].ToString(),
                        Dosages = Result["Dosages"].ToString(),
                        PKG = Result["PKG"].ToString(),
                        NoCane = Result["NoCane"].ToString(),
                        Cargo = Result["Cargo"].ToString(),
                        Exporter = Result["Exporter"].ToString(),
                        Container = Result["Container"]==null?"": Result["Container"].ToString(),
                        Volume = Result["Volume"] == null ? "" : Result["Volume"].ToString(),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        Total = Convert.ToDecimal(Result["Total"]),
                        ReceiptNo = Result["ReceiptNo"].ToString(),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        Country = Result["Country"].ToString(),
                        FumigationDate =Convert.ToString(Result["DateOfFumigation"]),
                        Size = Convert.ToInt32(Result["Size"])


                    });
                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstRptDailyPdaActivity;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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


        #region Dosewise PCS Report
        public void GetDosewisePCSReport(DSRPcsReportHeader ObjDailyPdaActivityReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyPdaActivityReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyPdaActivityReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Dosewise", MySqlDbType = MySqlDbType.VarChar, Value = ObjDailyPdaActivityReport.MbrType });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDosewisePCSReport", CommandType.StoredProcedure, DParam);
            //  DSRPcsReportHeader lstRptDailyPdaActivity = new DSRPcsReportHeader();
            IList<DSRPcsReport> lstRptDailyPdaActivity = new List<DSRPcsReport>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    lstRptDailyPdaActivity.Add(new DSRPcsReport
                    {
                        CertificateNo = Result["CertificateNo"].ToString(),
                        IssueDate = Result["IssueDate"].ToString(),
                        Chemical = Result["Chemical"].ToString(),
                        Dosages = Result["Dosages"].ToString(),
                        PKG = Result["PKG"].ToString(),
                        NoCane = Result["NoCane"].ToString(),
                        Cargo = Result["Cargo"].ToString(),
                        Exporter = Result["Exporter"].ToString(),
                        Container = Result["Container"] == null ? "" : Result["Container"].ToString(),
                        Volume = Result["Volume"] == null ? "" : Result["Volume"].ToString(),
                        Amount = Convert.ToDecimal(Result["Amount"]),
                        CGST = Convert.ToDecimal(Result["CGST"]),
                        SGST = Convert.ToDecimal(Result["SGST"]),
                        IGST = Convert.ToDecimal(Result["IGST"]),
                        Total = Convert.ToDecimal(Result["Total"]),
                        ReceiptNo = Result["ReceiptNo"].ToString(),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        Country = Result["Country"].ToString(),
                        FumigationDate = Convert.ToString(Result["DateOfFumigation"]),
                        Size = Convert.ToInt32(Result["size"]),
                        Place = Result["Place"].ToString()


                    });
                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstRptDailyPdaActivity;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        #region PCS Payment Report
        public void GetPCSPaymentReport(DSRPcsPaymentReportHeader ObjDailyPdaActivityReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyPdaActivityReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjDailyPdaActivityReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });            

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPCSPaymentReport", CommandType.StoredProcedure, DParam);
            //  DSRPcsReportHeader lstRptDailyPdaActivity = new DSRPcsReportHeader();
            IList<DSRPcsPaymentReport> lstRptDailyPdaActivity = new List<DSRPcsPaymentReport>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    lstRptDailyPdaActivity.Add(new DSRPcsPaymentReport
                    {
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        CertificateNo = Result["CertificateNo"].ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString(),
                        ReceiptDate = Result["ReceiptDate"].ToString(),
                        PartyDescription = Result["PartyDescription"].ToString(),
                        Amount = Convert.ToDecimal(Result["Amount"].ToString()) 
                    });
                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstRptDailyPdaActivity;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        #region MBR Report
        public void GetMBRReport(DSRFumigationMBR ObjDailyPdaActivityReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjDailyPdaActivityReport.AsOnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            string PeriodTo = DateTime.ParseExact(ObjDailyPdaActivityReport.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
           
            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_chemical", MySqlDbType = MySqlDbType.VarChar, Value = ObjDailyPdaActivityReport.MbrType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Dosages", MySqlDbType = MySqlDbType.VarChar, Value = ObjDailyPdaActivityReport.Dosages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Value = ObjDailyPdaActivityReport.OperationType });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetFumigationUnderChemicalReport", CommandType.StoredProcedure, DParam);           
            IList<MBRReport> lstRptDailyPdaActivity = new List<MBRReport>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    lstRptDailyPdaActivity.Add(new MBRReport
                    {
                        CertificateNo = Result["CertificateNo"].ToString(),
                        Commodity = Result["Commodity"].ToString(),
                        Quantity = Result["Quantity"].ToString(),
                        CountryImport = Result["CountryImport"].ToString(),
                        Dosagesformbr = Result["Dosagesformbr"].ToString(),
                        QuantityUsedMBR = Convert.ToInt32(Result["QuantityUsedMBR"].ToString()),
                        Remarks = Convert.ToInt32(Result["Remarks"].ToString())

                    });
                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstRptDailyPdaActivity;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        #region IWB report
      

        public void PrintInlandWayBill(int GatePassId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = GatePassId });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintIWB", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSRInlandWayBill ObjIWB = new DSRInlandWayBill();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //GatepassId
                    ObjIWB.IWBNo = Result["IWBNo"].ToString();
                    ObjIWB.IWBDate = Result["IWBDate"].ToString();
                    ObjIWB.PortOfLoading = Result["POL"].ToString();
                    ObjIWB.Destination = Result["POD"].ToString();
                    ObjIWB.Country = Result["Country"].ToString();
                    ObjIWB.CargoDesc = Result["CargoDescription"].ToString();
                    ObjIWB.CHAName = Result["CHAName"].ToString();
                    ObjIWB.ExporterName = Result["ExpName"].ToString();
                    ObjIWB.ShippingLineName = Result["ShippingLineName"].ToString();
                    ObjIWB.Container = Result["ContainerNo"].ToString();
                    ObjIWB.ShippingSeal = Result["ShippingSeal"].ToString();
                    ObjIWB.CFSCode = Result["CFSCode"].ToString();
                    ObjIWB.ShippBillNo = Result["ShippBillNo"].ToString();
                    ObjIWB.ShippBillDate = Result["ShippBillDate"].ToString();
                    ObjIWB.Forwarder = Result["ForwarderName"].ToString();
                    ObjIWB.TrailerNo = Result["VehicleNo"].ToString();
                    ObjIWB.CIFValue = Convert.ToDecimal(Result["CIFValue"]);
                    ObjIWB.NoOfPkg = Convert.ToDecimal(Result["NoOfPkg"]);
                    ObjIWB.Weight = Convert.ToDecimal(Result["Weight"]);
                    ObjIWB.Liner = Result["Liner"].ToString();
                    ObjIWB.Transporter = Result["Transporter"].ToString();
                    ObjIWB.OTLNo = Result["OTLNo"].ToString();
                    ObjIWB.SealNo = Result["ShippingSeal"].ToString();
                    ObjIWB.PlaceOfStuffing = Result["PlaceOfStuffing"].ToString();
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjIWB;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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
            List<WFLD_ShippingLineList> LstShippingLine = new List<WFLD_ShippingLineList>();
            // ShippingLine LstShippingLine = new ShippingLine();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstShippingLine.Add(new WFLD_ShippingLineList
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
        public void GetContainerNoForContStatus()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = "0" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ICDCode", MySqlDbType = MySqlDbType.VarChar, Value = "0" });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerForContrStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<WFLD_TrackContainer> LstContainer = new List<WFLD_TrackContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstContainer.Add(new WFLD_TrackContainer
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
        //public void GetContainerDetForContStatus(int ShippingLineId, string ContainerNo)
        //{
        //    int Status = 0;
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ShippingLineId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetContainerForContrStatus", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    List<WFLD_TrackContainerStatusList> LstContainer = new List<WFLD_TrackContainerStatusList>();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            LstContainer.Add(new WFLD_TrackContainerStatusList
        //            {
        //                DestuffingDate = (Result["DestuffingDate"] == null ? "" : Result["DestuffingDate"]).ToString(),
        //                StuffingDate = (Result["StuffingDate"] == null ? "" : Result["StuffingDate"]).ToString(),
        //                AppraisementDate = (Result["AppraisementAppDate"] == null ? "" : Result["AppraisementAppDate"]).ToString(),
        //                GatePassNo = (Result["GatePassNo"] == null ? "" : Result["GatePassNo"]).ToString(),
        //                GateEntryDate = (Result["GateEntryDateTime"] == null ? "" : Result["GateEntryDateTime"]).ToString(),
        //                Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString(),
        //                GateExitDate = (Result["GateExitDateTime"] == null ? "" : Result["GateExitDateTime"]).ToString(),
        //                GatePassDate = (Result["GatePassDate"] == null ? "" : Result["GatePassDate"]).ToString(),
        //                LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
        //                JobOrderDate = (Result["JobOrderDate"] == null ? "" : Result["JobOrderDate"]).ToString(),
        //                GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString(),
        //                Location = (Result["Location"] == null ? "" : Result["Location"]).ToString(),
        //            });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = LstContainer;
        //        }
        //        else
        //        {
        //            _DBResponse.Status = 0;
        //            _DBResponse.Message = "No Data Found";
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
            List<ContainerStatusList> LstContainer = new List<ContainerStatusList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstContainer.Add(new ContainerStatusList
                    {
                        DestuffingDate = (Result["DestuffingDate"] == null ? "" : Result["DestuffingDate"]).ToString(),
                        DestuffingStartDate = (Result["StartDate"] == null ? "" : Result["StartDate"]).ToString(),
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
                        ICDCode = (Result["ICDCode"] == null ? "" : Result["ICDCode"]).ToString(),
                    });
                }
                List<dynamic> lstEmpty = new List<dynamic>();
                List<dynamic> lstLoaded = new List<dynamic>();
                List<dynamic> lstEmptyAsses = new List<dynamic>();
                List<dynamic> lstMisc = new List<dynamic>();
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstEmpty.Add(new
                        {
                            CFSCode=Result["CFSCode"].ToString(),
                            OperationType = Result["OperationType"].ToString(),
                            InDate = Result["InDate"].ToString(),
                            Origin = Result["Origin"].ToString(),
                            DespatchCode = Result["DespatchCode"].ToString(),
                            Status = Result["Status"].ToString(),
                            OutDate = Result["OutDate"].ToString()
                        });

                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstLoaded.Add(new
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            OperationType = Result["OperationType"].ToString(),
                            InDate = Result["InDate"].ToString(),
                            Origin = Result["Origin"].ToString(),
                            DespatchCode = Result["DespatchCode"].ToString(),
                            IWBNo = Result["IWBNo"].ToString(),
                            OutDate = Result["OutDate"].ToString()
                        });

                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstEmptyAsses.Add(new
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            InvoiceNo = Result["InvoiceNo"].ToString(),
                            ReceiptNo = Result["ReceiptNo"].ToString(),
                            StartDate = Result["StartDate"].ToString(),
                            EndDate = Result["EndDate"].ToString()
                        });

                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstMisc.Add(new
                        {
                            InvoiceNo = Result["InvoiceNo"].ToString(),
                            ReceiptNo = Result["ReceiptNo"].ToString(),
                            InvoiceDate = Result["InvoiceDate"].ToString()
                        });

                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data =new { LstContainer, lstEmpty ,lstLoaded,lstEmptyAsses,lstMisc } ;
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


            List<DSR_RegisterOfEInvoiceModel> model = new List<DSR_RegisterOfEInvoiceModel>();
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


        private string RegisterofEInvoiceExcel(List<DSR_RegisterOfEInvoiceModel> model, DataTable dt)
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


                    //if (i % 2 != 0)
                    //{
                    //    Grid.Rows[i].Attributes.Add("class", "textmode");
                    //}
                    //else
                    //{
                    //    Grid.Rows[i].Attributes.Add("class", "textmode2");
                    //}

                }
                //var title = "CENTRAL WAREHOUSING CORPORATION </br>"
                //          + "Principal Place of Business</br>"
                //          + "CENTRAL WAREHOUSE</br>"
                //          + "REGISTER OF OUTWARD SUPPLY (TAX INVOICE/BILL OF SUPPLY)</br>";

                //System.Web.UI.WebControls.TableCell cell1 = new System.Web.UI.WebControls.TableCell();
                //cell1.Text = "<b> CENTRAL WAREHOUSING CORPORATION </b> ";
                //System.Web.UI.WebControls.TableRow tr1 = new System.Web.UI.WebControls.TableRow();
                //tr1.Cells.Add(cell1);
                //tr1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;

                //System.Web.UI.WebControls.TableCell cell2 = new System.Web.UI.WebControls.TableCell();
                //cell2.Text = "Principal Place of Business";
                //System.Web.UI.WebControls.TableRow tr2 = new System.Web.UI.WebControls.TableRow();
                //tr2.Cells.Add(cell2);
                //tr2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;

                //System.Web.UI.WebControls.TableCell cell3 = new System.Web.UI.WebControls.TableCell();
                //cell3.Text = "CENTRAL WAREHOUSE";
                //System.Web.UI.WebControls.TableRow tr3 = new System.Web.UI.WebControls.TableRow();
                //tr3.Cells.Add(cell3);
                //tr3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;


                //System.Web.UI.WebControls.TableCell cell4 = new System.Web.UI.WebControls.TableCell();
                //cell4.Text = "REGISTER OF OUTWARD SUPPLY (TAX INVOICE/BILL OF SUPPLY)";
                //System.Web.UI.WebControls.TableRow tr4 = new System.Web.UI.WebControls.TableRow();
                //tr4.Cells.Add(cell4);
                //tr4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;



                System.Web.UI.WebControls.TableCell cell5 = new System.Web.UI.WebControls.TableCell();
                cell5.Controls.Add(Grid);
                System.Web.UI.WebControls.TableRow tr5 = new System.Web.UI.WebControls.TableRow();
                tr5.Cells.Add(cell5);

                //tb.Rows.Add(tr1);
                //tb.Rows.Add(tr2);
                //tb.Rows.Add(tr3);
                //tb.Rows.Add(tr4);
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
            DSR_BulkIRN objInvoice = new DSR_BulkIRN();
            IDataReader Result = DataAccess.ExecuteDataReader("IRNNotgeneratedInvoiceList", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    objInvoice.lstPostPaymentChrg.Add(new DSR_BulkIRNDetails
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
                    _DBResponse.Message = (Result == 1) ? "IRN Generated Successfully" : "IRN Generated Successfully";
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


        #region Fumigation Report
        public void GetCertificateNo()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();           
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCertificateNoForReport", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            List<dynamic> LstCertificateNo = new List<dynamic>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCertificateNo.Add(new
                    {
                        CerticateNo = Convert.ToString(Result["CertificateNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCertificateNo;
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

        public void GetFumigationReportDetails(string CertificateNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            LstParam.Add(new MySqlParameter { ParameterName = "in_CertificateNo", MySqlDbType = MySqlDbType.VarChar, Value = CertificateNo });
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet  Result = DataAccess.ExecuteDataSet("GetFumigationDetailsByCertificateNo", CommandType.StoredProcedure,DParam);
            _DBResponse = new DatabaseResponse();
          
            try
            {
                _DBResponse.Status = 1;
               
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
                Result.Dispose();
               // Result.Close();
            }
        }

        #endregion


        #region Daily SD Transaction 

        public void GetDailySDTransactionExcel(string FromDate,string ToDate, int PartyId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetPartywiseDailySdTransactionReportExcel", CommandType.StoredProcedure, DParam);
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

        //#region PCS Payment Report
        //public void GetPCSPaymentReport(PCSPaymentReport ObjDailyPdaActivityReport)
        //{

        //    DateTime dtfrom = DateTime.ParseExact(ObjDailyPdaActivityReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //    String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
        //    DateTime dtTo = DateTime.ParseExact(ObjDailyPdaActivityReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //    String PeriodTo = dtTo.ToString("yyyy/MM/dd");

        //    int Status = 0;

        //    IDataParameter[] DParam = { };
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });            

        //    DParam = LstParam.ToArray();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetPCSPaymentReport", CommandType.StoredProcedure, DParam);
        //    //  DSRPcsReportHeader lstRptDailyPdaActivity = new DSRPcsReportHeader();
        //    IList<PCSPaymentReportDetail> lstRptDailyPdaActivity = new List<PCSPaymentReportDetail>();
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;

        //            lstRptDailyPdaActivity.Add(new PCSPaymentReportDetail
        //            {
        //                SrNo = Convert.ToInt32(Result["SrNo"].ToString()),
        //                InvoiceNo = Result["InvoiceNo"].ToString(),
        //                CertificateNo = Result["CertificateNo"].ToString(),
        //                ReceiptNo = Result["ReceiptNo"].ToString(),
        //                ReceiptDate = Result["ReceiptDate"].ToString(),
        //                PartyName = Result["PartyName"].ToString(),
        //                InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"])
        //            });
        //        }


        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = lstRptDailyPdaActivity;
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


        //#endregion

        #region Bond Report Excel

        public void GetBondReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });            
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("BondExcelReport", CommandType.StoredProcedure, DParam);
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


        #region Gate in Operation Excel 
        public void GetOperationExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetGateoperationReportInExcel", CommandType.StoredProcedure, DParam);
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
        public void ReportofImportLoadedGateIN(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
          
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("ReportofImportLoadedGateIN", CommandType.StoredProcedure, DParam);
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
        public void ReportofImportJobOrders(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("ReportofImportJobOrders", CommandType.StoredProcedure, DParam);
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

        public void ReportofImportLoadedOUTFactoryDestuffing(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("ReportofImportLoadedOUTFactoryDestuffing", CommandType.StoredProcedure, DParam);
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

        public void ReportofICDDestuffedContainers(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("ReportofICDDestuffedContainers", CommandType.StoredProcedure, DParam);
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

        public void ReportofImportEmptyFDIN(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("ReportofImportEmptyFDIN", CommandType.StoredProcedure, DParam);
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

        public void ReportofImportEmptyIN(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("ReportofImportEmptyIN", CommandType.StoredProcedure, DParam);
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

        public void ReportofExportEmptyIN(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("ReportofExportEmptyIN", CommandType.StoredProcedure, DParam);
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

        public void ReportofICDStuffingContainerOut(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("ReportofICDStuffingContainerOut", CommandType.StoredProcedure, DParam);
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

        public void ReportofFactoryStuffingJobOrder(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("ReportofFactoryStuffingJobOrder", CommandType.StoredProcedure, DParam);
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

        public void ReportofEmptyContainerGateOut(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("ReportofEmptyContainerGateOut", CommandType.StoredProcedure, DParam);
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

        public void ReportofExportLoadedContainerGateIN(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("ReportofExportLoadedContainerGateIN", CommandType.StoredProcedure, DParam);
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

        public void ReportofExportLoadedContainerGateOUT(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("ReportofExportLoadedContainerGateOUT", CommandType.StoredProcedure, DParam);
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

        #region Import Excel Transaction
        public void ImportReporExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("ReportExlImport", CommandType.StoredProcedure, DParam);
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

        #region ForExternalUser
        public void GetInvoiceListExternalUser(string FromDate, string ToDate, string invoiceType,int EximtraderId)
        {

            DateTime dtfrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy-MM-dd");
            DateTime dtTo = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy-MM-dd");
            if (invoiceType == "All")
            {
                invoiceType = "";
            }

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.String, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.String, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = invoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = EximtraderId });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("InvoiceListWithDateExternalUser", CommandType.StoredProcedure, DParam);
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

        public void ModuleListWithInvoiceExternalUser(DSRBulkInvoiceReport ObjBulkInvoiceReport)
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


        public void GenericBulkInvoiceDetailsForPrintForExternalUser(DSRBulkInvoiceReport ObjBulkInvoiceReport)
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
                if (ObjBulkInvoiceReport.InvoiceModule.ToLower() == "bndadv" || ObjBulkInvoiceReport.InvoiceModule.ToLower() == "bnd"
                    || ObjBulkInvoiceReport.InvoiceModule.ToLower() == "bndadvance")
                {
                    Result = DataAccess.ExecuteDataSet("GetBulkInvoiceDetailsForBondPrintExternalUser", CommandType.StoredProcedure, DParam);
                }
                else
                {
                    Result = DataAccess.ExecuteDataSet("getbulkinvoicedetailsforprintForExternalUser", CommandType.StoredProcedure, DParam);
                }
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

        public void GetReceiptListForExternalUser(string FromDate, string ToDate,int PartyId)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });




            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ReceiptListWithDateForExternalUser", CommandType.StoredProcedure, DParam);
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


        public void GetBulkDebitNoteReportForExternalUser(string PeriodFrom, string PeriodTo,int PartyId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
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
        }

        public void GetBulkCreditNoteReportForExternalUser(string PeriodFrom, string PeriodTo, int PartyId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
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



        #region Export Shipping Bill Details (CCIN Entry) Report in Excel

        public void GetExportShippingBillReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetExportShippingBillReportExcel", CommandType.StoredProcedure, DParam);
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


        #region Export Entry Through Gate CBT Report in Excel

        public void GetExportCBTGateInReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetExportCBTGateInReportExcel", CommandType.StoredProcedure, DParam);
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

        #region Export Carting Register Short Cargo Entry Report in Excel

        public void GetExportCartingShortCargoReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetExportCartingShortCargoReportExcel", CommandType.StoredProcedure, DParam);
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

        #region Export Stuffing Request Details Report in Excel

        public void GetExportStuffingRequestReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetExportStuffingRequestReportExcel", CommandType.StoredProcedure, DParam);
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

        #region Export Container Stuffing Report in Excel

        public void GetExportContainerStuffingReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetExportContainerStuffingReportExcel", CommandType.StoredProcedure, DParam);
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

        #region Export Stuffing Payment Sheet Report in Excel

        public void GetExportStuffingPaymentReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetExportStuffingPaymentReportExcel", CommandType.StoredProcedure, DParam);
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

        #region Export GATE Pass (Export Payment Sheet) Report in Excel

        public void GetExportGatePassReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetExportGatePassReportExcel", CommandType.StoredProcedure, DParam);
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

        #region Export Loaded Container Request/Update Report in Excel

        public void GetExportLoadedContRequestReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetExportLoadedContRequestReportExcel", CommandType.StoredProcedure, DParam);
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

        #region Export Loaded Container Payment Sheet Report in Excel

        public void GetExportLoadedContPaymentReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetExportLoadedContPaymentReportExcel", CommandType.StoredProcedure, DParam);
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

        #region Export Loaded Container Payment Sheet Report in Excel

        public void GetExportLoadedGatePassReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetExportLoadedGatePassReportExcel", CommandType.StoredProcedure, DParam);
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

        #region Export GRE Invoice For Shipping Line Report in Excel

        public void GetExportGREInvoiceReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetExportGREInvoiceReportExcel", CommandType.StoredProcedure, DParam);
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

        #region Export Back to Town Cargo Entry Report in Excel

        public void GetExportBTTCargoReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetExportBTTCargoReportExcel", CommandType.StoredProcedure, DParam);
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

        #region Export BTT Payment Sheet Report in Excel

        public void GetExportBTTPaymentSheetReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetExportBTTPaymentSheetReportExcel", CommandType.StoredProcedure, DParam);
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

        #region Export GATE Pass BTT Report in Excel

        public void GetExportBTTGatePassReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetExportBTTGatePassReportExcel", CommandType.StoredProcedure, DParam);
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

        #region Export Export RCT Sealing Report in Excel

        public void GetExportRCTSealingReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetExportRCTSealingReportExcel", CommandType.StoredProcedure, DParam);
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


        #region Import job order excel
        public void ImportjobReporExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("Importjoborderexlreport", CommandType.StoredProcedure, DParam);
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
        #region Import obl detail excel
        public void ImportoblReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("Importoblexlreport", CommandType.StoredProcedure, DParam);
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
        #region Import gate detail excel
        public void Importgateoperationexcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("Importgateexlreport", CommandType.StoredProcedure, DParam);
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
        #region Import custom appraisement excel
        public void ImportcustomExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("Importcustomexlreport", CommandType.StoredProcedure, DParam);
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
        #region Import destuffing excel
        public void ImportDestuffingExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("Importdestuffexl", CommandType.StoredProcedure, DParam);
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
        #region Import yard paymentsheet excel
        public void ImportyardReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("importyardpayexl", CommandType.StoredProcedure, DParam);
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
        #region Import yard paymentsheet reasses
        public void ImportyardreassessReporExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("importyardreassesexl", CommandType.StoredProcedure, DParam);
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
        #region Import yard fd
        public void ImportyardfdExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("Importyardfdexl", CommandType.StoredProcedure, DParam);
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
        #region Import delivery to invoice excel
        public void ImportdelReporExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("Importdelinvchkexl", CommandType.StoredProcedure, DParam);
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
        #region Import deli in reassess
        public void ImportdelreassessExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("ImportDelinvreexl", CommandType.StoredProcedure, DParam);
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
        #region Import gatepass cargo
        public void ImportgpcReporExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("ImportgpcardelExl", CommandType.StoredProcedure, DParam);
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
        #region Import convert to bond excel
        public void ImportcbReporExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("Importconbndexl", CommandType.StoredProcedure, DParam);
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
        #region Import rct sealing
        public void ImportrctReporExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("Importrctsealexl", CommandType.StoredProcedure, DParam);
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
        #region Import  fd
        public void ImportfdReporExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("Importfdentryexl", CommandType.StoredProcedure, DParam);
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
        #region Import empty container invoice
        public void ImportempcExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("Importemptyconexl", CommandType.StoredProcedure, DParam);
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
        #region Import empty container out excel
        public void ImportempcoutExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodfrom", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_periodto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(PartyId) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("Importemptycontoutexl", CommandType.StoredProcedure, DParam);
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
            List<Dsr_ContStufAckSearch> LstStuffing = new List<Dsr_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Dsr_ContStufAckSearch
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
            List<Dsr_ContStufAckSearch> LstStuff = new List<Dsr_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuff.Add(new Dsr_ContStufAckSearch
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
            List<Dsr_ContStufAckRes> Lststufack = new List<Dsr_ContStufAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Dsr_ContStufAckRes
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
            List<DSR_SBQuery> LstSB = new List<DSR_SBQuery>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new DSR_SBQuery
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
            DSR_SBQuery LstSBQueryReport = new DSR_SBQuery();
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
            List<Dsr_ContStufAckSearch> LstStuff = new List<Dsr_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuff.Add(new Dsr_ContStufAckSearch
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
            List<Dsr_ContStufAckSearch> LstStuff = new List<Dsr_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuff.Add(new Dsr_ContStufAckSearch
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
            List<Dsr_ContStufAckRes> Lststufack = new List<Dsr_ContStufAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Dsr_ContStufAckRes
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
            List<Dsr_GatePassDPAckSearch> lstDPGPAck = new List<Dsr_GatePassDPAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDPGPAck.Add(new Dsr_GatePassDPAckSearch
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

            List<Dsr_ContDPAckSearch> lstDPContACK = new List<Dsr_ContDPAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDPContACK.Add(new Dsr_ContDPAckSearch
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
            List<Dsr_DPAckRes> Lststufack = new List<Dsr_DPAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Dsr_DPAckRes
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
            List<Dsr_GatePassDTAckSearch> lstDTGPAck = new List<Dsr_GatePassDTAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDTGPAck.Add(new Dsr_GatePassDTAckSearch
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

            List<Dsr_ContDTAckSearch> lstDTContACK = new List<Dsr_ContDTAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDTContACK.Add(new Dsr_ContDTAckSearch
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
            List<Dsr_DTAckRes> Lststufack = new List<Dsr_DTAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Dsr_DTAckRes
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
            List<DSR_loadstuf> Lststufack = new List<DSR_loadstuf>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new DSR_loadstuf
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
            List<DSR_loadstufasr> Lststufack = new List<DSR_loadstufasr>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new DSR_loadstufasr
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
            List<DSR_loadstufdp> Lststufack = new List<DSR_loadstufdp>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new DSR_loadstufdp
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
            List<DSR_loadstufdt> Lststufack = new List<DSR_loadstufdt>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new DSR_loadstufdt
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

        #region Indent of Disinfestation Material
        public void GetPcsIndDisinfestationMaterial(string CertificateNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            LstParam.Add(new MySqlParameter { ParameterName = "in_CertificateNo", MySqlDbType = MySqlDbType.VarChar, Value = CertificateNo });
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetPcsIndDisinfestationMaterial", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {
                _DBResponse.Status = 1;

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
                Result.Dispose();
                // Result.Close();
            }
        }
        #endregion
        #region Receipt Register

        public void GetReceiptReportExcel(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("ReceiptRegister", CommandType.StoredProcedure, DParam);
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
        #region CIM SF Report

        public void GetCIMSFReport(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetCIMSFReport", CommandType.StoredProcedure, DParam);
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
        #region CIM DP Report

        public void GetCIMDPReport(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetCIMDPReport", CommandType.StoredProcedure, DParam);
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
        #region CIM ASR Report

        public void GetCIMASRReport(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("GetCIMASRReport_Test", CommandType.StoredProcedure, DParam);
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


        #region Daily Inventory for NLDSL
        public void DailyInventoryForNldsl(string AsOnDate)
        {

            AsOnDate = DateTime.ParseExact(AsOnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
         

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsOnDate", MySqlDbType = MySqlDbType.DateTime, Value = AsOnDate });
           
            DParam = LstParam.ToArray();
            DataSet ds = DataAccess.ExecuteDataSet("GetDailyInventoryforNLDSL", CommandType.StoredProcedure, DParam);
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


        #region Add edit QR Code
        public void AddEditBQRCode(int InvoiceId, string FileName, int CreatedBy)
        {
            string Status = "0";
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_BLNo", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.BLNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FileName", MySqlDbType = MySqlDbType.VarChar, Value = FileName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.VarChar, Value = CreatedBy });


            LstParam.Add(new MySqlParameter { ParameterName = "@RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
            //  LstParam.Add(new MySqlParameter { ParameterName = "@GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });

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
