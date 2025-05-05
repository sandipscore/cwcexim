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
using CwcExim.Areas.CashManagement.Models;
namespace CwcExim.Repositories
{
    public class ReportRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        #region CwcExim Section
        public void GetInvoiceReportDetails(InvoiceReportDetails ObjInvoiceReportDetails)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjInvoiceReportDetails.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjInvoiceReportDetails.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            string Flag = "";
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
            }
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Registered", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInvoiceReportDetails.Registered });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Flag", MySqlDbType = MySqlDbType.VarChar, Value = Flag });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceReportDetails", CommandType.StoredProcedure, DParam);
            List<InvoiceReportDetails> LstInvoiceReportDetails = new List<InvoiceReportDetails>();

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

                    LstInvoiceReportDetails.Add(new InvoiceReportDetails
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

        public void GetInvoiceReportDetailsExcel(InvoiceReportDetails ObjInvoiceReportDetails)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjInvoiceReportDetails.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjInvoiceReportDetails.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            
            int Status = 0;
            string Flag = "";
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
            }
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Registered", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInvoiceReportDetails.Registered });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Flag", MySqlDbType = MySqlDbType.VarChar, Value = Flag });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceReportDetails", CommandType.StoredProcedure, DParam);
            List<InvoiceReportDetailsExcel> LstInvoiceReportDetails = new List<InvoiceReportDetailsExcel>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;                   

                    LstInvoiceReportDetails.Add(new InvoiceReportDetailsExcel
                    {
                        InvoiceNumber = Result["InvoiceNo"].ToString(),
                        Date = Result["Date"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        GSTNo = Result["GstNo"].ToString(),
                        SAC = Result["SAC"].ToString(),
                        Values = Convert.ToDecimal(Result["Value"]),
                        CGST = Convert.ToDecimal(Result["CGSTAmt"]),
                        SGST = Convert.ToDecimal(Result["SGSTAmt"]),
                        IGST = Convert.ToDecimal(Result["IGSTAmt"]),
                        TotalValue = Convert.ToDecimal(Result["TotalValue"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = InvoiceReportDetailsExcel(LstInvoiceReportDetails, ObjInvoiceReportDetails.PeriodFrom, ObjInvoiceReportDetails.PeriodTo);
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();

            }
        }

        private string InvoiceReportDetailsExcel(List<InvoiceReportDetailsExcel> model,string PeriodFrom ,string PeriodTo)
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
                        + "Invoice Report Details";

                var title2 = @"Invoice Report (Details) From "  + PeriodFrom + " To " + PeriodTo;

                exl.MargeCell("A1:J1", title, DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A2:J2", title2, DynamicExcel.CellAlignment.Middle);

                exl.AddCell("A3", "Invoice No", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("B3", "Invoice Date", DynamicExcel.CellAlignment.Middle);               
                exl.AddCell("C3", "Party Name", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("D3", "GST NO.", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("E3", "SAC", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("F3", "Values", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("G3", "CGST", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("H3", "SGST", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("I3", "IGST", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("J3", "Total Values", DynamicExcel.CellAlignment.Middle);
                                              
                exl.AddTable<InvoiceReportDetailsExcel>("A", 4, model, new[] { 20,20,20,20,20,20,20,20,20,20, });
                
                var CGSTAmt = model.Sum(o => o.CGST);
                var SGSTAmt = model.Sum(o => o.SGST);
                var IGSTAmt = model.Sum(o => o.IGST);
                
                var Values = model.Sum(o => o.Values);
                var TotalValues = model.Sum(o => o.TotalValue);                

                exl.AddCell("F" + (model.Count + 4).ToString(), Values.ToString(), DynamicExcel.CellAlignment.CenterLeft);
                exl.AddCell("G" + (model.Count + 4).ToString(), CGSTAmt.ToString(), DynamicExcel.CellAlignment.CenterLeft);
                exl.AddCell("H" + (model.Count + 4).ToString(), SGSTAmt.ToString(), DynamicExcel.CellAlignment.CenterLeft);
                exl.AddCell("I" + (model.Count + 4).ToString(), IGSTAmt.ToString(), DynamicExcel.CellAlignment.CenterLeft);
                exl.AddCell("J" + (model.Count + 4).ToString(), TotalValues.ToString(), DynamicExcel.CellAlignment.CenterLeft);

                exl.Save();
            }
            return excelFile;
        }

        public void GetExemptedService(ExemptedService ObjExemptedService)
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
            List<ExemptedService> LstInvoiceReportDetails = new List<ExemptedService>();

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

                    LstInvoiceReportDetails.Add(new ExemptedService
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
        public void ServiceCodeWiseInvDtls(Kol_ServiceCodeWiseInvDtls ObjServiceCodeWiseInvDtls)
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

            LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ServiceCodeWiseInvDtls", CommandType.StoredProcedure, DParam);
            List<Kol_ServiceCodeWiseInvDtls> LstInvoiceReportDetails = new List<Kol_ServiceCodeWiseInvDtls>();

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

                    LstInvoiceReportDetails.Add(new Kol_ServiceCodeWiseInvDtls
                    {
                        SAC = Result["SAC"].ToString(),
                        InvoiceNumber = Result["InvoiceNo"].ToString(),
                        Date = Result["Date"].ToString(),
                        CGST = Convert.ToDecimal(Result["CGSTAmt"]),
                        SGST = Convert.ToDecimal(Result["SGSTAmt"]),
                        IGST = Convert.ToDecimal(Result["IGSTAmt"]),
                        TotalValue = Convert.ToDecimal(Result["TotalValue"])
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
                if (Status == 1)
                {
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
            List<SACList> LstSAC = new List<SACList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSAC.Add(new SACList
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
        public void CollectionReport(CollectionReport ObjCollectionReport)
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
            FinalCollectionReportTotal LstCollectionReport = new FinalCollectionReportTotal();
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

                    LstCollectionReport.listCollectionReport.Add(new CollectionReport
                    {
                        Date = Result["DateFormatted"].ToString(),
                        Cash = Result["Cash"].ToString(),
                        Bank = Result["Cheque"].ToString(),//bank has data for cheque see view
                        PO = Result["PO"].ToString(),
                        DD = Result["DRAFT"].ToString(),
                        Pd = Result["NEFT"].ToString(),//neft has data for pd see view
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
                        LstCollectionReport.objCollectionReportTotal.TotalDraft = Convert.ToString(Result["TotalDRAFT"]);



                    }
                }
                if (Status == 1)
                {
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
        public void GetInvoiceReportSummary(InvoiceReportSummary ObjInvoiceReportSummary)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjInvoiceReportSummary.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjInvoiceReportSummary.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            //ConsumerList ObjStatusDtl = null;in_Status

            int Status = 0;
            string Flag = "";
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
            }
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Registered", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInvoiceReportSummary.Registered });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Flag", MySqlDbType = MySqlDbType.VarChar, Value = Flag });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceReportSummary", CommandType.StoredProcedure, DParam);
            List<InvoiceReportSummary> LstInvoiceReportSummary = new List<InvoiceReportSummary>();

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

                    LstInvoiceReportSummary.Add(new InvoiceReportSummary
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
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Result["InvoiceDate"].ToString(),
                        CRNo = Result["ReceiptNo"].ToString(),
                        CRDate = Result["ReceiptDate"].ToString(),
                        PartyTAN = Result["TanNumber"].ToString(),
                        InvoiceValue = Result["InvoiceValue"].ToString(),
                        TDS = Result["TDS"].ToString(),
                        TDSPlus = Result["TDSPlus"].ToString(),
                        Amount = Result["Amount"].ToString(),

                    });
                }
                if (Status == 1)
                {
                    objTDSMain.TDSReportLst.Add(new TDSReport
                    {
                        InvoiceNo = "<strong>Total</strong>",
                        InvoiceDate = string.Empty,
                        CRNo = string.Empty,
                        CRDate = string.Empty,
                        PartyTAN = string.Empty,
                        InvoiceValue = "<strong>" + objTDSMain.TDSReportLst.ToList().Sum(m => Convert.ToDecimal(m.InvoiceValue)).ToString() + "</strong>",
                        TDS = "<strong>" + objTDSMain.TDSReportLst.ToList().Sum(m => Convert.ToDecimal(m.TDS)).ToString() + "</strong>",
                        TDSPlus = "<strong>" + objTDSMain.TDSReportLst.ToList().Sum(m => Convert.ToDecimal(m.TDSPlus)).ToString() + "</strong>",
                        Amount = "<strong>" + objTDSMain.TDSReportLst.ToList().Sum(m => Convert.ToDecimal(m.Amount)).ToString() + "</strong>"

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
        public void DailyCashBook(DailyCashBook ObjDailyCashBook)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_comgst", MySqlDbType = MySqlDbType.VarChar, Value = ObjDailyCashBook.CompanyGST });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyCashBookReportgst", CommandType.StoredProcedure, DParam);
            IList<DailyCashBook> LstDailyCashBook = new List<DailyCashBook>();
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

                    LstDailyCashBook.Add(new DailyCashBook
                    {
                        CRNo = Result["CRNo"].ToString(),
                        ReceiptDate = Convert.ToDateTime(Result["ReceiptDate"] == DBNull.Value ? "N/A" : Result["ReceiptDate"]).ToString("dd/MM/yyyy"),
                        //Result["ReceiptDate"].ToString(),
                        Depositor = Result["Depositor"].ToString(),
                        CwcChargeTAX = Result["CwcChargeTAX"].ToString(),
                        CwcChargeNonTAX = Result["CwcChargeNonTAX"].ToString(),
                        CustomRevenueTAX = Result["CustomRevenueTAX"].ToString(),
                        CustomRevenueNonTAX = Result["CustomRevenueNonTAX"].ToString(),
                        InsuranceTAX = Result["InsuranceTAX"].ToString(),
                        InsuranceNonTAX = Result["InsuranceNonTAX"].ToString(),
                        PortTAX = Result["PortTAX"].ToString(),
                        PortNonTAX = Result["PortNonTAX"].ToString(),
                        //LWB = Result["LWB"].ToString(),
                        LWBTAX = Result["LWBTAX"].ToString(),
                        LWBNonTAX = Result["LWBNonTAX"].ToString(),
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
                        NonMISC = Result["NonMISC"].ToString(),
                        TDSPlus = Result["TDSPlus"].ToString(),
                        Exempted = Result["Exempted"].ToString(),
                        PdaPLus = Result["PdaPLus"].ToString(),
                        TDSMinus = Result["TDSMinus"].ToString(),
                        PdaMinus = Result["PdaMinus"].ToString(),
                        HtAdjust = Result["HtAdjust"].ToString(),
                        RoundOff = Result["RoundUp"].ToString(),
                        RowTotal = Result["Total"].ToString(),
                      //  RowCreditNote= Result["CreditNoteAmount"].ToString() 

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
        public void MOnthlyCashBook(MonthlyCashBook ObjDailyCashBook)
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
            IList<MonthlyCashBook> LstMonthlyCashBook = new List<MonthlyCashBook>();
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

                    LstMonthlyCashBook.Add(new MonthlyCashBook
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
                        //LWB = Result["LWB"].ToString(),
                        LWBTAX = Result["LWBTAX"].ToString(),
                        LWBNonTAX = Result["LWBNonTAX"].ToString(),
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
                        NonTaxMISC = Result["NonTaxMISC"].ToString(),
                        TDSPlus = Result["TDSPlus"].ToString(),
                        Exempted = Result["Exempted"].ToString(),
                        PdaPLus = Result["PdaPLus"].ToString(),
                        TDSMinus = Result["TDSMinus"].ToString(),
                        PdaMinus = Result["PdaMinus"].ToString(),
                        HtAdjust = Result["HtAdjust"].ToString(),
                        BankDeposit = Result["BankDeposit"].ToString(),
                        RoundOff = Result["RoundUp"].ToString(),
                        RowTotal = Result["Total"].ToString(),
                  //      RowCreditNote="0",
            //            RowCreditNote = Result["CreditNoteAmount"].ToString(),
                        Balance = Result["Balance"].ToString()




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

        public void MOnthlyCashBookNewFormat(MonthlyCashBook ObjDailyCashBook)
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
            IDataReader Result = DataAccess.ExecuteDataReader("MonthlyCashBookReportNewFormat", CommandType.StoredProcedure, DParam);
            IList<MonthlyCashBook> LstMonthlyCashBook = new List<MonthlyCashBook>();
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

                    LstMonthlyCashBook.Add(new MonthlyCashBook
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
                        //LWB = Result["LWB"].ToString(),
                        LWBTAX = Result["LWBTAX"].ToString(),
                        LWBNonTAX = Result["LWBNonTAX"].ToString(),
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
                        //      RowCreditNote="0",
                        //            RowCreditNote = Result["CreditNoteAmount"].ToString(),
                        Balance = Result["Balance"].ToString(),
                         CurrentMonth =Convert.ToInt32(Result["CurrentMonth"].ToString()),




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
        public void MOnthlyCashBookGst(MonthlyCashBook ObjDailyCashBook)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_comgst", MySqlDbType = MySqlDbType.VarChar, Value = ObjDailyCashBook.CompanyGST });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("MonthlyCashBookReportgst", CommandType.StoredProcedure, DParam);
            IList<MonthlyCashBook> LstMonthlyCashBook = new List<MonthlyCashBook>();
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

                    LstMonthlyCashBook.Add(new MonthlyCashBook
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
                        //LWB = Result["LWB"].ToString(),
                        LWBTAX = Result["LWBTAX"].ToString(),
                        LWBNonTAX = Result["LWBNonTAX"].ToString(),
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
                        //      RowCreditNote="0",
                        //            RowCreditNote = Result["CreditNoteAmount"].ToString(),
                        Balance = Result["Balance"].ToString()




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
        public void StatementOfEmptyContainer(StatementOfEmptyContainer ObjStatementOfEmptyContainer)
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
            IList<StatementOfEmptyContainer> LstStatementOfEmptyContainer = new List<StatementOfEmptyContainer>();
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

                    LstStatementOfEmptyContainer.Add(new StatementOfEmptyContainer
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        DateOfArrival = Result["DateOFarrival"].ToString(),
                        ImportExport = Result["OperationType"].ToString()
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
                        Size = Convert.ToString(Result["Size"] == null ? "" : Result["Size"])
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
        public void SealClosingReport(SealClosingReport ObjSealClosingReport)
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
            IList<SealClosingReport> LstSealClosingReport = new List<SealClosingReport>();
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

                    LstSealClosingReport.Add(new SealClosingReport
                    {


                        Date = Result["Date"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        Commodity = Result["Commodity"].ToString(),

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
        public void BulkInvoiceReport(BulkInvoiceReport ObjBulkInvoiceReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceModule", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = ObjBulkInvoiceReport.InvoiceModule });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNumber", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = ObjBulkInvoiceReport.InvoiceNumber });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjBulkInvoiceReport.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjBulkInvoiceReport.CHAId });


            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("BulkInvoicePrint", CommandType.StoredProcedure, DParam);
            IList<string> htmls = new List<string>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    htmls.Add(Result["html"].ToString());
                    //htmls.Remove("");
                }

                if (Status == 1 && htmls.Count() > 0)
                {
                    htmls.ToList().ForEach(item =>
                    {
                        if (item == "")
                        {
                            htmls.Remove(item);
                        }
                    });


                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = htmls;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();

            }
        }
        public void GetInvoiceList(string FromDate, string ToDate, string Module = "All", int ShippingLineId = 0, int CHAId = 0)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceModule", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = CHAId });




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

        public void GetInvoiceListExternalUser(string FromDate, string ToDate, string Module = "All", int ShippingLineId = 0, int CHAId = 0)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceModule", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = CHAId });




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
                        Location = Result["Location"].ToString()
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
        public void PdSummaryReport(PdSummary ObjPdSummaryReport)
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
        public void GatePassReport(GatePassReport ObjGatePassReport)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ObjGatePassReport.Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ObjGatePassReport.Type });

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
                        ShippingSealLineNo = "",
                        CustomSealLineNo = "",

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
        public void IssueSlipReport(IssueSlipReport ObjIssueSlipReport)
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
            IList<IssueSlipReport> LstIssueSlipReport = new List<IssueSlipReport>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstIssueSlipReport.Add(new IssueSlipReport
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
        public void DeStuffingReportBig(DeStuffingReportBig ObjDeStuffingReportBig)
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
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DeStuffingReportBig", CommandType.StoredProcedure, DParam);
            IList<DeStuffingReportBig> LstDeStuffingReportBig = new List<DeStuffingReportBig>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstDeStuffingReportBig.Add(new DeStuffingReportBig
                    {



                        DeStuffingNo = Result["DestuffingEntryNo"].ToString(),
                        DeStuffingDate = Result["DestuffingEntryDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ContainerSize = Result["Size"].ToString(),


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
        public void CargoStuffingRegister(CargoStuffingRegister ObjCargoStuffingRegister)
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
            IList<CargoStuffingRegister> LstCargoStuffingRegister = new List<CargoStuffingRegister>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstCargoStuffingRegister.Add(new CargoStuffingRegister
                    {
                        Date = Result["StuffingDate"].ToString(),
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
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
            DateTime dtTo = DateTime.ParseExact(ObjSlineChaImpDailyActivity.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_partyId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjSlineChaImpDailyActivity.EximTraderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImportExport", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ObjSlineChaImpDailyActivity.EximTraderName });
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
                        GateOutDate = Result["GateExitDateTime"].ToString(),
                        ImportExport = Result["OperationType"].ToString(),
                        ChaName = Result["CHANAME"].ToString(),
                        GatePassNo = Result["GatePassNo"].ToString(),
                        GatePassDate = Result["GatePassDateTime"].ToString()



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
        public void BulkReceiptReport(BulkReceiptReport ObjBulkReceiptReport)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjBulkReceiptReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjBulkReceiptReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNumber", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjBulkReceiptReport.ReceiptNumber });



            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("BulkReceiptPrint", CommandType.StoredProcedure, DParam);
            IList<string> htmls = new List<string>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    htmls.Add(Result["html"].ToString());

                    // htmls.Remove("");
                    //htmls.Count();
                }

                if (Status == 1 && htmls.Count() > 0)
                {
                    htmls.ToList().ForEach(item =>
                    {
                        if (item == "")
                        {
                            htmls.Remove(item);
                        }
                    });



                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = htmls;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
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


        public void GetReceiptListForExternalUser(string FromDate, string ToDate,int UID)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_UID", MySqlDbType = MySqlDbType.Int32, Value = UID });



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
        public void ChequeSummary(ChequeSummary ObjChequeSummary)
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
            IList<ChequeSummary> LstChequeSummary = new List<ChequeSummary>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstChequeSummary.Add(new ChequeSummary
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
                    LstChequeSummary.Add(new ChequeSummary
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
        public void CartingOrderRegister(CartingOrderRegister ObjCartingOrderRegister)
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
            IList<CartingOrderRegister> LstCartingOrderRegister = new List<CartingOrderRegister>();
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

                    LstCartingOrderRegister.Add(new CartingOrderRegister
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
                        ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"]).ToString()












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
        public void BayWiseRegister(string PeriodFrom, string PeriodTo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("BayWiseRegister", CommandType.StoredProcedure, DParam);
            IList<BayWiseRegister> lstRegister = new List<BayWiseRegister>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstRegister.Add(new BayWiseRegister
                    {
                        DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]),
                        DestuffingEntryDate = Result["DestuffingEntryDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        SLine = Result["SLine"].ToString(),
                        Vessel = Result["Vessel"].ToString(),
                        Voyage = Result["Voyage"].ToString(),
                        LineNo = Result["LineNo"].ToString(),
                        Rotation = Result["Rotation"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                        GodownName = Result["GodownName"].ToString(),
                        GodownWiseLctnNames = Result["GodownWiseLctnNames"].ToString(),
                        CargoDescription = Result["CargoDescription"].ToString(),
                        BOLNo = Result["BOLNo"].ToString(),
                        ImporterName = Result["ImporterName"].ToString(),
                        GatePassNo = Result["GatePassNo"].ToString(),
                        DeliveryDate = Result["DeliveryDate"].ToString(),
                        NoOfPackagesDeli = Convert.ToInt32(Result["NoOfPackagesDeli"]),
                        Remarks = Result["Remarks"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstRegister;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();

            }
        }
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
        public void GetWorkSlipReportList(WorkSlipReport ObjWorkSlipReport)
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

            WorkSlipReport newObjWorkSlipReport = new WorkSlipReport();

            newObjWorkSlipReport.WorkSlipSummaryList = new List<WorkSlipSummary>();
            newObjWorkSlipReport.WorkSlipDetailList = new List<WorkSlipDetail>();

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
                        WorkSlipSummary objWorkSlipSummary = new WorkSlipSummary();
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
                        newObjWorkSlipReport.WorkSlipDetailList.Add(new WorkSlipDetail
                        {
                            Clause = item["Clause"].ToString(),
                            ContainerSize = item["ContainerSize"].ToString(),
                            Containers = item["Containers"].ToString(),
                        });
                    }
                    if (newObjWorkSlipReport.WorkSlipSummaryList.Where(x => x.Clause == "2").Count() > 0)
                    {
                        //Added on 20-04-2019 for 2A clause(REMOVAL CHARGES FROM PORT)
                        //The data will be same as for 2

                        //If clause 2 exists then only it will be added in the first list based on its index
                        int idx = newObjWorkSlipReport.WorkSlipSummaryList.IndexOf(newObjWorkSlipReport.WorkSlipSummaryList.Where(x => x.Clause == "2").FirstOrDefault());
                        int count20 = Convert.ToInt32(newObjWorkSlipReport.WorkSlipSummaryList.Where(x => x.Clause == "2").FirstOrDefault().Count_20);
                        int count40 = Convert.ToInt32(newObjWorkSlipReport.WorkSlipSummaryList.Where(x => x.Clause == "2").FirstOrDefault().Count_40);
                        var SAC = newObjWorkSlipReport.WorkSlipSummaryList.Where(x => x.Clause == "2").FirstOrDefault().SAC.ToString();
                        newObjWorkSlipReport.WorkSlipSummaryList.Insert(idx+1, new WorkSlipSummary()
                        {
                            SrNo = ++count,
                            Particulars = Convert.ToString("REMOVAL CHARGES FROM PORT"),
                            Clause = Convert.ToString("2A"),
                            SAC = SAC,
                            NoOfPackages = "",
                            GrossWeight = "",
                            Count_20 = count20,
                            Count_40 = count40
                        });
                        //end
                        //adjusting the index for inserting container no in the second list
                        int indx = 0;
                        if (newObjWorkSlipReport.WorkSlipDetailList.Where(x => x.Clause == "2" && x.ContainerSize == "20").Count() > 0 &&
                            newObjWorkSlipReport.WorkSlipDetailList.Where(x => x.Clause == "2" && x.ContainerSize == "40").Count() > 0)
                        {
                            idx = newObjWorkSlipReport.WorkSlipDetailList.IndexOf(newObjWorkSlipReport.WorkSlipDetailList.Where(x => x.Clause == "2" && x.ContainerSize == "20").FirstOrDefault());
                            indx = newObjWorkSlipReport.WorkSlipDetailList.IndexOf(newObjWorkSlipReport.WorkSlipDetailList.Where(x => x.Clause == "2" && x.ContainerSize == "40").FirstOrDefault());
                            idx += 2;
                            indx += 2;
                        }
                        else if (newObjWorkSlipReport.WorkSlipDetailList.Where(x => x.Clause == "2" && x.ContainerSize == "20").Count() > 0)
                        {
                            idx = newObjWorkSlipReport.WorkSlipDetailList.IndexOf(newObjWorkSlipReport.WorkSlipDetailList.Where(x => x.Clause == "2" && x.ContainerSize == "20").FirstOrDefault());
                            idx += 1;
                        }
                        else
                        {
                            indx = newObjWorkSlipReport.WorkSlipDetailList.IndexOf(newObjWorkSlipReport.WorkSlipDetailList.Where(x => x.Clause == "2" && x.ContainerSize == "40").FirstOrDefault());
                            indx += 1;
                        }
                        //end
                        if (newObjWorkSlipReport.WorkSlipDetailList.Where(x=>x.Clause=="2" && x.ContainerSize=="20").Count() > 0)
                        {
                            var cont20 = newObjWorkSlipReport.WorkSlipDetailList.Where(x => x.Clause == "2" && x.ContainerSize == "20").FirstOrDefault().Containers;
                            newObjWorkSlipReport.WorkSlipDetailList.Insert(idx, new WorkSlipDetail
                            {
                                Clause = "2A",
                                Containers = cont20,
                                ContainerSize = "20"
                            });
                        }
                        if (newObjWorkSlipReport.WorkSlipDetailList.Where(x => x.Clause == "2" && x.ContainerSize == "40").Count() > 0)
                        {
                            var cont40 = newObjWorkSlipReport.WorkSlipDetailList.Where(x => x.Clause == "2" && x.ContainerSize == "40").FirstOrDefault().Containers;
                            newObjWorkSlipReport.WorkSlipDetailList.Insert(indx, new WorkSlipDetail
                            {
                                Clause = "2A",
                                Containers = cont40,
                                ContainerSize = "40"
                            });
                        }
                        //Updating the serial no in the list 
                        count = 0;
                        newObjWorkSlipReport.WorkSlipSummaryList.ForEach(i =>
                        {
                            i.SrNo = ++count;
                        });
                        //end
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
        private ContainerInfo GetContainerInfo(string containerNo, string size)
        {
            ContainerInfo objContainerInfo = new ContainerInfo();
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
        public void GetDailyTransactionReportList(DailyTransactionReport ObjDailyTransactionReport)
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

            DailyTransactionReport newObjDailyTransactionReport = new DailyTransactionReport();

            newObjDailyTransactionReport.AppeasementSummaryList = new List<AppeasementSummary>();
            newObjDailyTransactionReport.DeStuffingSummaryList = new List<DeStuffingSummary>();
            newObjDailyTransactionReport.CartingSummaryList = new List<CartingSummary>();
            newObjDailyTransactionReport.StuffingSummaryList = new List<StuffingInportExportBONDSummary>();
            newObjDailyTransactionReport.InportInSummaryList = new List<StuffingInportExportBONDSummary>();
            newObjDailyTransactionReport.InportOutSummaryList = new List<StuffingInportExportBONDSummary>();
            newObjDailyTransactionReport.ExportInSummaryList = new List<StuffingInportExportBONDSummary>();
            newObjDailyTransactionReport.ExportOutSummaryList = new List<StuffingInportExportBONDSummary>();
            newObjDailyTransactionReport.BONDUnloadingSummaryList = new List<StuffingInportExportBONDSummary>();
            newObjDailyTransactionReport.BONDDeliverySummaryList = new List<StuffingInportExportBONDSummary>();
            newObjDailyTransactionReport.EmptyInTransporterSummaryList = new List<EmptyTransporterSummary>();
            newObjDailyTransactionReport.EmptyOutTransporterSummaryList = new List<EmptyTransporterSummary>();

            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    #region AppeasementSummary
                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        newObjDailyTransactionReport.AppeasementSummaryList.Add(new AppeasementSummary
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
                        newObjDailyTransactionReport.DeStuffingSummaryList.Add(new DeStuffingSummary
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
                        newObjDailyTransactionReport.CartingSummaryList.Add(new CartingSummary
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
                            newObjDailyTransactionReport.StuffingSummaryList.Add(new StuffingInportExportBONDSummary
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
                                ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.StuffingSummaryList.Where(x => x.ShippingBillNo == item["ShippingBillNo"].ToString()).ToList())
                            {
                                ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
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
                            newObjDailyTransactionReport.ExportInSummaryList.Add(new StuffingInportExportBONDSummary
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
                                ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.ExportInSummaryList.Where(x => x.ShippingBillNo == item["ShippingBillNo"].ToString()).ToList())
                            {
                                ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
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
                            newObjDailyTransactionReport.ExportOutSummaryList.Add(new StuffingInportExportBONDSummary
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
                                ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.ExportOutSummaryList.Where(x => x.ShippingBillNo == item["ShippingBillNo"].ToString()).ToList())
                            {
                                ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
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
                            newObjDailyTransactionReport.InportInSummaryList.Add(new StuffingInportExportBONDSummary
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
                                ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.InportInSummaryList.Where(x => x.ExporterName == item["ImporterName"].ToString()).ToList())
                            {
                                ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
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
                            newObjDailyTransactionReport.InportOutSummaryList.Add(new StuffingInportExportBONDSummary
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
                                ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.InportOutSummaryList.Where(x => x.ExporterName == item["ImporterName"].ToString()).ToList())
                            {
                                ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
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
                            newObjDailyTransactionReport.BONDUnloadingSummaryList.Add(new StuffingInportExportBONDSummary
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
                                ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.BONDUnloadingSummaryList.Where(x => x.ShippingBillNo == item["BOENo"].ToString()).ToList())
                            {
                                ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
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
                            newObjDailyTransactionReport.BONDDeliverySummaryList.Add(new StuffingInportExportBONDSummary
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
                                ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString()));
                            }

                        }
                        else
                        {
                            foreach (var itemDailyTransactionReport in newObjDailyTransactionReport.BONDDeliverySummaryList.Where(x => x.ShippingBillNo == item["BOENo"].ToString()).ToList())
                            {
                                ContainerInfo objContainerInfo = GetContainerInfo(item["ContainerNo"].ToString(), item["Size"].ToString());
                                itemDailyTransactionReport.ContainerInfoList.Add(objContainerInfo);
                            }
                        }

                    }
                    #endregion

                    #region EmptyInTransporterSummary
                    foreach (DataRow item in Result.Tables[10].Rows)
                    {
                        newObjDailyTransactionReport.EmptyInTransporterSummaryList.Add(new EmptyTransporterSummary
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
                        newObjDailyTransactionReport.EmptyOutTransporterSummaryList.Add(new EmptyTransporterSummary
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
        public List<MonthlyEconomyReport> GetMonthlyEconomyReportDataToPrint(int monthNo, int yearNo, out int dataExistStatus)
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

            List<MonthlyEconomyReport> LstMonthlyEconomyReport = new List<MonthlyEconomyReport>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        MonthlyEconomyReport objMonthlyEconomyReport = new MonthlyEconomyReport();
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
        public List<MonthlyEconomyReport> GetMonthlyEconomyReportData(int monthNo, int yearNo, out int dataExistStatus)
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

            List<MonthlyEconomyReport> LstMonthlyEconomyReport = new List<MonthlyEconomyReport>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        MonthlyEconomyReport objMonthlyEconomyReport = new MonthlyEconomyReport();
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
                        objMonthlyPerformaceReport.Particulars = Convert.ToString(item["Particulars"]);
                        objMonthlyPerformaceReport.IsHeader = Convert.ToBoolean(item["IsHeader"]);
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
        public void ContainerBalanceInCFS(ContainerBalanceInCFS ObjContainerBalanceInCFS)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjContainerBalanceInCFS.ImportExport });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_partyNameId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDebtorReport.partyNameId });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ContainerBalanceInCFSReport", CommandType.StoredProcedure, DParam);
            IList<ContainerBalanceInCFS> LstContainerBalanceInCFS = new List<ContainerBalanceInCFS>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstContainerBalanceInCFS.Add(new ContainerBalanceInCFS
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


        public void OblSbinformation(OblSbinformation ObjOblSbinformation)
        {


            DateTime dtfrom = DateTime.ParseExact(ObjOblSbinformation.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjOblSbinformation.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
           
            LstParam.Add(new MySqlParameter { ParameterName = "in_Flag", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = ObjOblSbinformation.Ref });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("OblSbinfromation", CommandType.StoredProcedure, DParam);
            IList<OblSbinformation> LstOblSbinfromation = new List<OblSbinformation>();

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    LstOblSbinfromation.Add(new OblSbinformation
                    {

                        OBLNo = Result["OBLNo"].ToString(),
                        BolDate = Result["BolDate"].ToString(),
                        FobValue = Result["FobValue"].ToString(),
                        NoOfPackages = Result["NoOfPackages"].ToString(),
                        GrossWeight = Result["GrossWeight"].ToString(),
                        ImporterName = Result["ImporterName"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        Duty = Result["CIFValue"].ToString(),
                         BEO = Result["BOENo"].ToString(),
                        Date = Result["EntryDateTime"].ToString(),

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstOblSbinfromation;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
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
                                 RoundUp = Convert.ToDecimal(dr["Roundup"]),
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

                exl.MargeCell("R2:R2", "Round Up", DynamicExcel.CellAlignment.Middle);

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
                exl.AddTable<RegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6,10, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16, 12, 30, 14, 14, 14, 14, 14, 40 });
                var igstamt = model.Sum(o => o.ITaxAmount);
                var sgstamt = model.Sum(o => o.STaxAmount);
                var cgstamt = model.Sum(o => o.CTaxAmount);
                var totalamt = model.Sum(o => o.Total);
                var RoundUp = model.Sum(o => o.RoundUp);
                exl.AddCell("L" + (model.Count + 6).ToString(), igstamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("N" + (model.Count + 6).ToString(), cgstamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 6).ToString(), sgstamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("Q" + (model.Count + 6).ToString(), totalamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("R" + (model.Count + 6).ToString(), RoundUp, DynamicExcel.CellAlignment.CenterRight);
                /*exl.AddCell("O" + (model.Count + 7).ToString(), "Invoice Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 7).ToString(), InvoiceAmount, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 8).ToString(), "Cash Receipt Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 8).ToString(), CRAmount, DynamicExcel.CellAlignment.CenterRight);*/
                exl.Save();
            }
            return excelFile;
        }

        #endregion

        #region Unpaid Invoice List
        public void GetUnpaidInvoiceList(DateTime date1, DateTime date2)
        {

            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.DateTime, Value = date2 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //var objRegOutwardSupply = (List<RegisterOfOutwardSupplyModel>)DataAccess.ExecuteDynamicSet<List<RegisterOfOutwardSupplyModel>>("GetRegisterofOutwardSupply", DParam);
            DataSet ds = DataAccess.ExecuteDataSet("GetUnpaidInvoiceList", CommandType.StoredProcedure, DParam);
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
                                 RoundUp = Convert.ToDecimal(dr["Roundup"]),
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
                _DBResponse.Data = UnpaidInvoiceListExcel(model, InvoiceAmount, CRAmount);
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
        private string UnpaidInvoiceListExcel(List<RegisterOfOutwardSupplyModel> model, decimal InvoiceAmount, decimal CRAmount)
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
                        + Environment.NewLine + " " + Environment.NewLine
                        + "UNPAID INVOICE LIST";
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

                exl.MargeCell("R2:R2", "Round Up", DynamicExcel.CellAlignment.Middle);

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
                exl.AddTable<RegisterOfOutwardSupplyModel>("A", 6, model, new[] { 6, 10, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16, 12, 30, 14, 14, 14, 14, 14, 40 });
                var igstamt = model.Sum(o => o.ITaxAmount);
                var sgstamt = model.Sum(o => o.STaxAmount);
                var cgstamt = model.Sum(o => o.CTaxAmount);
                var totalamt = model.Sum(o => o.Total);
                var RoundUp = model.Sum(o => o.RoundUp);
                exl.AddCell("L" + (model.Count + 6).ToString(), igstamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("N" + (model.Count + 6).ToString(), cgstamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 6).ToString(), sgstamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("Q" + (model.Count + 6).ToString(), totalamt, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("R" + (model.Count + 6).ToString(), RoundUp, DynamicExcel.CellAlignment.CenterRight);
                /*exl.AddCell("O" + (model.Count + 7).ToString(), "Invoice Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 7).ToString(), InvoiceAmount, DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("O" + (model.Count + 8).ToString(), "Cash Receipt Amount", DynamicExcel.CellAlignment.CenterRight);
                exl.AddCell("P" + (model.Count + 8).ToString(), CRAmount, DynamicExcel.CellAlignment.CenterRight);*/
                exl.Save();
            }
            return excelFile;
        }

        #endregion

        #region Stock Position Report
        public void StockPositionReport(StockPositionReportModel ObjChStockPosition)
        {
            DateTime dtfrom = DateTime.ParseExact(ObjChStockPosition.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjChStockPosition.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = PeriodTo });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("StockPositionReport", CommandType.StoredProcedure, DParam);
            IList<StockPositionReportModel> LstStockPosition = new List<StockPositionReportModel>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStockPosition.Add(new StockPositionReportModel
                    {
                        GodownNo = Result["GodownName"].ToString(),
                        Commodity = Result["CommodityName"].ToString(),
                        Units = Convert.ToInt32(Result["Units"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        Party = Result["EximTraderName"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStockPosition;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
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

        #region ContainerStatus
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
            List<ContainerStatusList> LstContainer = new List<ContainerStatusList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstContainer.Add(new ContainerStatusList
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
            List<ContainerList> LstContainer = new List<ContainerList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstContainer.Add(new ContainerList
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
            List<ContainerList> LstContainer = new List<ContainerList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstContainer.Add(new ContainerList
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
            List < ICDList > LstICD= new List<ICDList>();
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
                if(Status==1)
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
            catch(Exception ex)
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
            ICDList ObjIcd =new ICDList();
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
        public void VCCapacityDetailsforKolkata(string dtArray,string date1,string date2)
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
        #endregion

        #region Daily valuation Report of Import Cargo
        public void DailyvaluationReport(string ContainerNo, string BolNo, string BoeNo)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BolNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = BolNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = BoeNo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyValuationRpt", CommandType.StoredProcedure, DParam);
            IList<DailyValuationRpt> LstRpt = new List<DailyValuationRpt>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstRpt.Add(new DailyValuationRpt
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Convert.ToInt32(Result["Size"]),
                        Party = Result["Party"].ToString(),
                        CommodityName = Result["CommodityName"].ToString(),
                        GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                        Duty = Convert.ToDecimal(Result["Duty"]),
                        CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                    });
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
                Result.Close();
                Result.Dispose();
            }
        }
        public void GetAllEnteredContainer()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEnteredContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<string> lstCont = new List<string>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCont.Add(Result["ContainerNo"].ToString());
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCont;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        #region InventoryReportCargoContainer
        public void InventoryReportCargoContainer(InventoryReportModel ObjInventoryReportModel)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjInventoryReportModel.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjInventoryReportModel.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ContainerCargoInventoryReport", CommandType.StoredProcedure, DParam);
            InventoryReportModel objInventoryReportModel = new InventoryReportModel();
            //DataTable dt = new DataTable();
            //dt.Load(Result);
            //DataSet ds = new DataSet();
            //ds.



            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;


                    objInventoryReportModel.LstInventoryReportContainer.Add(new InventoryReportContainer
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
                        objInventoryReportModel.LstInventoryReportCargo.Add(new InvenontoryReportCargo
                        {
                            //CFSCode = Convert.ToString(Result["EximTraderName"]),
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

        #region Job Order Sheet
        public void JobOrderSheet(JobOrderSheet ObjJobOrderSheet)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjJobOrderSheet.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjJobOrderSheet.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("JobOrderSheetReport", CommandType.StoredProcedure, DParam);
            IList<JobOrderSheet> LstJobOrderSheet = new List<JobOrderSheet>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstJobOrderSheet.Add(new JobOrderSheet
                    {
                        SlNo = Convert.ToInt32(Result["SlNo"]),
                        JobOrderNo = Convert.ToString(Result["JobOrderNo"]),
                        JobOrderDate = Convert.ToString(Result["JobOrderDate"]),
                        EximTraderName = Convert.ToString(Result["EximTraderName"]),
                        ReferenceNo = Convert.ToString(Result["ReferenceNo"]),
                        ReferenceDate = Convert.ToString(Result["ReferenceDate"]),
                        FromLocation = Convert.ToString(Result["FromLocation"]),
                        ToLocation = Convert.ToString(Result["ToLocation"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        ContainerSize = Convert.ToString(Result["ContainerSize"]),
                        NoOfContainer = Convert.ToInt32(Result["NoOfContainer"])
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstJobOrderSheet;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
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


         

        #region Register of Outward Supply NEW
        public void GetRegisterofOutwardSupplybyINVTYPE(DateTime date1, DateTime date2, string Type)
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
            DataSet ds = DataAccess.ExecuteDataSet("GetRegisterofOutwardSupplyBYTYPE", CommandType.StoredProcedure, DParam);
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
                    _DBResponse.Data = RegisterofOutwardSupplyExcelbYTYPE(model, InvoiceAmount, CRAmount);
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
        private string RegisterofOutwardSupplyExcelbYTYPE(List<PpgRegisterOfOutwardSupplyModel> model, decimal InvoiceAmount, decimal CRAmount)
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
               // exl.MargeCell("G2:G4", "HSN Code", DynamicExcel.CellAlignment.Middle);
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
              //  exl.MargeCell("G2:G4", "HSN Code", DynamicExcel.CellAlignment.Middle);

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

        #region Carting Work Order
        public void CartingWorkOrder(CartingWorkOrder ObjCartingWorkOrder)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjCartingWorkOrder.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjCartingWorkOrder.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");

            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("CartingWorkOrderReport", CommandType.StoredProcedure, DParam);
            IList<CartingWorkOrder> LstCartingWorkOrder = new List<CartingWorkOrder>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCartingWorkOrder.Add(new CartingWorkOrder
                    {
                        SlNo = Convert.ToInt32(Result["SlNo"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        CommodityName = Convert.ToString(Result["CommodityName"]),
                        CargoDescription = Convert.ToString(Result["CargoDescription"]),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                        WorkOrderNo = Convert.ToString(Result["WorkOrderNo"]),
                        WorkOrderDate = Convert.ToString(Result["WorkOrderDate"]),
                        CartingNo = Convert.ToString(Result["CartingNo"]),
                        ApplicationDate = Convert.ToString(Result["ApplicationDate"])
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCartingWorkOrder;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
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

        #region Delivery Order Register
        public void DeliveryOrderRegisterReport(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("DeliveryOrderRegisterReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DeliveryOrderRegList> LstDeliveryOrder = new List<DeliveryOrderRegList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDeliveryOrder.Add(new DeliveryOrderRegList
                    {
                        BondNo = Result["BondNo"].ToString(),
                        BondDate = (Result["BondDate"] == null ? "" : Result["BondDate"]).ToString(),
                        WRNo = (Result["WRNo"] == null ? "" : Result["WRNo"]).ToString(),
                        WRDate = (Result["WRDate"] == null ? "" : Result["WRDate"]).ToString(),
                        Importer = (Result["ImporterName"] == null ? "" : Result["ImporterName"]).ToString(),
                        Units = (Result["Units"] == null ? "" : Result["Units"]).ToString(),
                        Weight = (Result["Weight"] == null ? "" : Result["Weight"]).ToString(),
                        CIFValue = (Result["CIFValue"] == null ? "" : Result["CIFValue"]).ToString(),
                        DeliveryOrderNo = (Result["DeliveryOrderNo"] == null ? "" : Result["DeliveryOrderNo"]).ToString(),
                        DeliveryOrderDate = (Result["DeliveryOrderDate"] == null ? "" : Result["DeliveryOrderDate"]).ToString(),
                        SacNo = (Result["SacNo"] == null ? "" : Result["SacNo"]).ToString(),
                        SacDate = (Result["SacDate"] == null ? "" : Result["SacDate"]).ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDeliveryOrder;
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
        #endregion

        #region Cargo Daily Report
        public void GetCargoDailyReport(string FromDate, string ToDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCargoDailyReport", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CargoDailyReport ObjCargoDaily = new CargoDailyReport();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjCargoDaily.LstCargoDaily.Add(new CargoDailyList
                    {
                        RegisterDate = Result["RegisterDate"].ToString(),
                        VehicleNo = (Result["VehicleNo"] == null ? "" : Result["VehicleNo"]).ToString(),
                        NoOfPackages = (Result["NoOfPackages"] == null ? "" : Result["NoOfPackages"]).ToString(),
                        Weight = (Result["Weight"] == null ? "" : Result["Weight"]).ToString(),
                        Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                        Party = (Result["Party"] == null ? "" : Result["Party"]).ToString()
                    });
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjCargoDaily.LstCargoSummary.Add(new CargoSummaryList
                        {
                            Weight = (Result["Weight"] == null ? "" : Result["Weight"]).ToString(),
                            Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            Party = (Result["Party"] == null ? "" : Result["Party"]).ToString()
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCargoDaily;
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

        #region Daily valuation report of export cargo
        public void DailyValuationofExp(string PeriodFrom, string PeriodTo)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(PeriodFrom).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(PeriodTo).ToString("yyyy/MM/dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("DailyValuationofExp", CommandType.StoredProcedure, DParam);
            IList<DailyValuationOfExpCrgo> LstDaily = new List<DailyValuationOfExpCrgo>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDaily.Add(new DailyValuationOfExpCrgo
                    {
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
                        CommodityName = Convert.ToString(Result["CommodityName"]),
                        Units = Convert.ToInt32(Result["Units"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        FobValue = Convert.ToDecimal(Result["FobValue"]),
                        CHAName = Convert.ToString(Result["CHAName"]),
                        InsuredBy = Convert.ToString(Result["InsuredBy"]),
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDaily;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
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

        #region Statement of Reefer Container 
        public void StatementOfReeferContainer(string PeriodFrom, string PeriodTo)
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(PeriodFrom).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(PeriodTo).ToString("yyyy/MM/dd") });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("StmtOfReeferCont", CommandType.StoredProcedure, DParam);
            IList<StmtRefCont> LstCont = new List<StmtRefCont>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCont.Add(new StmtRefCont
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        ShippingLineName = Result["ShippingLineName"].ToString(),
                        ContainerType = Result["ContainerType"].ToString(),
                        GateEntryDate = Result["GateEntryDate"].ToString(),
                        GateExitDate = Result["GateExitDate"].ToString(),
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


        #region Cargo Stock Register
        public void CargoStockRegister(CargoStockRegister ObjCargoStockRegister)
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
            CargoStockRegister _ObjCargoStockRegister = new CargoStockRegister();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    _ObjCargoStockRegister.exportCargoStocklst.Add(new exportCargoStock
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
                        _ObjCargoStockRegister.importCargoStocklst.Add(new importCargoStock
                        {
                            BOE = Result["BOENo"].ToString(),
                            Date = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            NoOfPackage = (Result["NoOfUnits"] == null ? "" : Result["NoOfUnits"]).ToString(),
                            Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString(),

                        });
                    }



                }

                if (Result.NextResult())
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



                }
                if (Status == 1)
                {
                    _ObjCargoStockRegister.exportCargoStocklst.ToList().ForEach(m =>
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

        #region  PD A/c Statement
        public void GetPDAStatement(int Month, int Year)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Month", MySqlDbType = MySqlDbType.Int32, Value = Month });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Year", MySqlDbType = MySqlDbType.Int32, Value = Year });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("RptPDAStatement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PDAStatement ObjPDA = new PDAStatement();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjPDA.LstPDA.Add(new PDAList
                    {
                        DepositorName = (Result["DepositorName"] == null ? "" : Result["DepositorName"]).ToString(),
                        Amount = Convert.ToDecimal((Result["Amount"] == null ? 0 : Result["Amount"]).ToString())
                    });
                }
                //total collection and closing is same
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjPDA.TotalAmount = Convert.ToDecimal((Result["closingAmount"] == null ? 0 : Result["closingAmount"]).ToString());
                    }
                }
                //opening
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjPDA.OpeningAmount = Convert.ToDecimal((Result["OpeningAmount"] == null ? 0 : Result["OpeningAmount"]).ToString());
                    }
                }
                // total collection 
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjPDA.Collections = Convert.ToDecimal((Result["TotalAmount"] == null ? 0 : Result["TotalAmount"]).ToString());
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjPDA.Adjustment = Convert.ToDecimal((Result["Adjustment"] == null ? 0 : Result["Adjustment"]).ToString());
                    }
                }
                //closing 
                if (Result.NextResult())

                {
                    while (Result.Read())
                    {
                        ObjPDA.closingAmount = Convert.ToDecimal((Result["closingAmount1"] == null ? 0 : Result["closingAmount1"]).ToString());
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjPDA;
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
            IList<TDSStatement> LstTDS = new List<TDSStatement>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstTDS.Add(new TDSStatement
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Result["PartyName"].ToString(),
                        ReceiptDate = Result["ReceiptDate"].ToString(),
                        ReceiptNo = Result["ReceiptNo"].ToString(),
                        TDSCol = Convert.ToDecimal(Result["TDSCol"]),
                        CWCTDS = Convert.ToDecimal(Result["CWCTDS"]),
                        HTTDS = Convert.ToDecimal(Result["HTTDS"]),
                        TDS = Convert.ToDecimal(Result["TDS"]),
                        ReceivedTDS = Convert.ToDecimal(Result["ReceivedTDS"])
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

        #region PaymentVoucher Statement
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
            IList<PaymentVoucherReport> LstRpt = new List<PaymentVoucherReport>();
            LstRpt = (List<PaymentVoucherReport>)DataAccess.ExecuteDynamicSet<PaymentVoucherReport>("GetPaymentVoucherReport", DParam);
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




        #region SEIS Data
        public void GetSeisData(DateTime FromDate, DateTime ToDate,string Flag)
        {


            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "pFromDate", MySqlDbType = MySqlDbType.DateTime, Value = FromDate });
            LstParam.Add(new MySqlParameter { ParameterName = "pToDate", MySqlDbType = MySqlDbType.DateTime, Value = ToDate });
            LstParam.Add(new MySqlParameter { ParameterName = "Flag", MySqlDbType = MySqlDbType.VarChar, Value = Flag });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //var objRegOutwardSupply = (List<RegisterOfOutwardSupplyModel>)DataAccess.ExecuteDynamicSet<List<RegisterOfOutwardSupplyModel>>("GetRegisterofOutwardSupply", DParam);
           

            DataSet ds = DataAccess.ExecuteDataSet("GetServicesRenderDetails", CommandType.StoredProcedure, DParam);
            DataTable dt = ds.Tables[0];
            List<SEISDataViewModel> lstSEISDataViewModel = new List<SEISDataViewModel>();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    int no = 0;
                    foreach(DataRow dr in dt.Rows)
                    {
                        no +=  1;
                        lstSEISDataViewModel.Add(new SEISDataViewModel
                        {
                               CARGO_HANDLING_AMT=Convert.ToDecimal(dr["CARGO_HANDLING_AMT"]),
                              CONTAINER_NO= Convert.ToString(dr["CONTAINER_NO"]),
                              FOREIGNLINER= Convert.ToString(dr["FOREIGN_LINER"]),
                              ID=Convert.ToInt32(no),
                              INDIAN_AGENT_FOREIGN_LINER = Convert.ToString(dr["INDIAN_AGENT_FOREIGN_LINER"]),
                              INVOICE_BILL_DT= Convert.ToString(dr["INVOICE_BILL_DT"]),
                              INVOICE_BILL_NO= Convert.ToString(dr["INVOICE_BILL_NO"]),
                              OTHER_CHARGES= Convert.ToDecimal(dr["OTHER_CHARGES"]),
                              RECEIPT_DATE= Convert.ToString(dr["RECEIPT_DATE"]),
                              RECEIPT_NO= Convert.ToString(dr["RECEIPT_NO"]),
                              Remarks= Convert.ToString(dr["Remarks"]),
                              STORAGE_WAREHOUSING_AMT= Convert.ToDecimal(dr["STORAGE_WAREHOUSING_AMT"]),
                              TAX_CHR= Convert.ToDecimal(dr["TAX_CHR"]),
                              VESSELNO= Convert.ToString(dr["VESSEL_NO"]),
                              VESSEL_NM= Convert.ToString(dr["VESSEL_NM"]),
                              


                        });
                    }


                   
                }
                decimal InvoiceAmount = 0, CRAmount = 0;
                
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = SeisDataExcel(lstSEISDataViewModel, Flag);
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
        private string SeisDataExcel(List<SEISDataViewModel> model, string Flag)
        {
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + Flag + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {

                exl.AddCell("A1", "Sr no", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("B1", "FOREIGN_LINER", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("C1", "VESSEL_NO", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("D1", "VESSEL_NM", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("E1", "INDIAN_AGENT_FOREIGN_LINER", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("F1", "CONTAINER_NO", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("G1", "INVOICE_BILL_NO", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("H1", "INVOICE_BILL_DT", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("I1", "RECEIPT_NO", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("J1", "RECEIPT_DATE", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("K1", "CARGO_HANDLING_AMT", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("L1", "STORAGE_WAREHOUSING_AMT", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("M1", "OTHER_CHARGES", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("N1", "TAX_CHR", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("O1", "Remarks", DynamicExcel.CellAlignment.Middle);

                //for (var i = 65; i < 90; i++)
                //{
                //    char character = (char)i;
                //    string text = character.ToString();
                //    exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                //}
               
                exl.AddTable<SEISDataViewModel>("A", 2, model, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14 ,20,10,10,10});
               
               
                exl.Save();
            }
            return excelFile;
        }

        #endregion



        #region Misc Report
        public void GenericMISCForPrint(string InvoiceNo)
        {
           
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = InvoiceNo });
                    try
            {
                DParam = LstParam.ToArray();
               
                  Result = DataAccess.ExecuteDataSet("GetMiscForPrint", CommandType.StoredProcedure, DParam);
               

               
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



        #region Register of Outward Supply for Company GST
        public void GetRegisterofOutwardSupplyGst(DateTime date1, DateTime date2,string ComGst)
        {

            var LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_From", MySqlDbType = MySqlDbType.DateTime, Value = date1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_To", MySqlDbType = MySqlDbType.DateTime, Value = date2 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_comgst", MySqlDbType = MySqlDbType.VarChar, Value = ComGst });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //var objRegOutwardSupply = (List<RegisterOfOutwardSupplyModel>)DataAccess.ExecuteDynamicSet<List<RegisterOfOutwardSupplyModel>>("GetRegisterofOutwardSupply", DParam);
            DataSet ds = DataAccess.ExecuteDataSet("GetRegisterofOutwardSupplygst", CommandType.StoredProcedure, DParam);
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
                _DBResponse.Data = RegisterofOutwardSupplyExcelGst(model, InvoiceAmount, CRAmount, ComGst);
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
        private string RegisterofOutwardSupplyExcelGst(List<RegisterOfOutwardSupplyModel> model, decimal InvoiceAmount, decimal CRAmount,string ComGst)
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
                        + "REGISTER OF OUTWARD SUPPLY (TAX INVOICE/BILL OF SUPPLY)"
                        + Environment.NewLine + Environment.NewLine
                        + "Company GST Number:"+ ComGst;
                
                exl.MargeCell("A1:M1", title, DynamicExcel.CellAlignment.Middle);
                exl.AddCell("N1", "BILL REGISTER", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A3:A5", "Sl.No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B3:B5", "GSTIN", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C3:C5", "Place of Supply" + Environment.NewLine + "(Name of State)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D3:D5", "Name of Customer to whom Service rendered", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E3:E5", "Period of Invoice", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("F3:F5", "Nature of Invoice" + Environment.NewLine + "(Resv./Initial Fumigatiom/General Basic/Over & Above)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("G3:G5", "Rate per" + Environment.NewLine + "Bag/MT/Sqm", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H3:J5", "Invoice Details", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("H4:H5", "Invoice No.", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("I4:I5", "Date of Invoice", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("J4:J5", "Value of Service" + Environment.NewLine + "(Before Tax)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K3:P3", "Rate of Tax", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("K4:L4", "IGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("M4:N4", "CGST", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("O4:P4", "SGST", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("K5", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("L5", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("M5", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("N5", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("O5", "%", DynamicExcel.CellAlignment.Middle);
                exl.AddCell("P5", "Amount", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Q3:Q5", "Total Invoice Value" + Environment.NewLine + "(14=(10+12 or 10+14+16))", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("R3:U5", "Perticulars of Payment Received", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("R4:R5", "Received" + Environment.NewLine + "At WH/RO", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("S4:S5", "C.R No. & Date", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("T4:T5", "Cheque/DD No. & Date", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("U4:U5", "Amount of DD/Cheque (Rs.)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("V3:V5", "Amount Received Against Bill (Rs.)", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("W3:W5", "Adjustment/Deduction", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("X3:X5", "Balance", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("Y3:Y5", "Remarks", DynamicExcel.CellAlignment.Middle);
                for (var i = 65; i < 90; i++)
                {
                    char character = (char)i;
                    string text = character.ToString();
                    exl.AddCell(text + "6", (i - 64), DynamicExcel.CellAlignment.Middle);
                }
                /*exl.AddTable<InvoiceData>("A", 6, model.InvoiceData, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16 });
                exl.AddTable<CashReceiptData>("R", 6, model.CashReceiptData, new[] { 12, 30, 14, 14, 14, 14, 14, 40 });*/
                exl.AddTable<RegisterOfOutwardSupplyModel>("A", 7, model, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16, 12, 30, 14, 14, 14, 14, 14, 40 });
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


        #region Bulk Invoice 
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceModule", MySqlDbType = MySqlDbType.String, Value = "All" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjBulkInvoiceReport.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjBulkInvoiceReport.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.String, Value = ObjBulkInvoiceReport.InvoiceNumber });
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


            List<KOLRegisterOfEInvoiceModel> model = new List<KOLRegisterOfEInvoiceModel>();
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


        private string RegisterofEInvoiceExcel(List<KOLRegisterOfEInvoiceModel> model, DataTable dt)
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
            KOL_BulkIRN objInvoice = new KOL_BulkIRN();
            IDataReader Result = DataAccess.ExecuteDataReader("IRNNotgeneratedInvoiceList", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    objInvoice.lstPostPaymentChrg.Add(new KOL_BulkIRNDetails
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
            List<Kol_E04Report> LstE04 = new List<Kol_E04Report>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstE04.Add(new Kol_E04Report
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
            Kol_E04Report objE04Report = new Kol_E04Report();
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
            List<Kol_E04Report> LstE04Report = new List<Kol_E04Report>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstE04Report.Add(new Kol_E04Report
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
            List<Kol_ContStufAckSearch> LstStuffing = new List<Kol_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new Kol_ContStufAckSearch
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
            List<Kol_ContStufAckSearch> LstStuff = new List<Kol_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuff.Add(new Kol_ContStufAckSearch
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
            List<Kol_ContStufAckRes> Lststufack = new List<Kol_ContStufAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Kol_ContStufAckRes
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
            List<Kol_ContStufAckSearch> LstStuff = new List<Kol_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuff.Add(new Kol_ContStufAckSearch
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
            List<Kol_ContStufAckSearch> LstStuff = new List<Kol_ContStufAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstStuff.Add(new Kol_ContStufAckSearch
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
            List<Kol_ContStufAckRes> Lststufack = new List<Kol_ContStufAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Kol_ContStufAckRes
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
            List<Kol_GatePassDPAckSearch> lstDPGPAck = new List<Kol_GatePassDPAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDPGPAck.Add(new Kol_GatePassDPAckSearch
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

            List<Kol_ContDPAckSearch> lstDPContACK = new List<Kol_ContDPAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDPContACK.Add(new Kol_ContDPAckSearch
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
            List<Kol_DPAckRes> Lststufack = new List<Kol_DPAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Kol_DPAckRes
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
            List<Kol_GatePassDTAckSearch> lstDTGPAck = new List<Kol_GatePassDTAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDTGPAck.Add(new Kol_GatePassDTAckSearch
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

            List<Kol_ContDTAckSearch> lstDTContACK = new List<Kol_ContDTAckSearch>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstDTContACK.Add(new Kol_ContDTAckSearch
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
            List<Kol_DTAckRes> Lststufack = new List<Kol_DTAckRes>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lststufack.Add(new Kol_DTAckRes
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

        #region Party Wise Ledger For Container
        public void GetRptPartyWiseLedgerForContainer(string FromDate, string ToDate, int PartyId)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            DParam = LstParam.ToArray();
            DataSet ds = DataAccess.ExecuteDataSet("GetRptPartyWiseLedgerForContainer", CommandType.StoredProcedure, DParam);
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
        #region Receipt Register

        public void GetpartywiseinvbillsmyExcel(string FromDate, string ToDate,int PartyId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("PartyWiseTaxBillSummary", CommandType.StoredProcedure, DParam);
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

        #region For External User
        public void GenericBulkInvoiceDetailsForPrintForExternalUser(BulkInvoiceReport ObjBulkInvoiceReport)
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
                Result = DataAccess.ExecuteDataSet("GetBulkInvoiceDetailsForPrintForExternalUser", CommandType.StoredProcedure, DParam);
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


        public void GetInvoiceListForExternalUser(string FromDate, string ToDate, string invoiceType,int PartyId)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
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

        public void ModuleListWithInvoiceForExternalUser(BulkInvoiceReport ObjBulkInvoiceReport)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceModule", MySqlDbType = MySqlDbType.String, Value = "All" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjBulkInvoiceReport.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjBulkInvoiceReport.CHAId });
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




        #endregion

        #region Debit Note
        public void GetBulkDebitNoteReportForExternalUser(string PeriodFrom, string PeriodTo,int Partyid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = Partyid });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("BulkDRNoteForExterUser", CommandType.StoredProcedure, DParam);
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

        public void GetBulkCreditNoteReportForExternalUser(string PeriodFrom, string PeriodTo, int Partyid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.Date, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.Date, Value = PeriodTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Date, Value = Partyid });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("BulkCRNoteForExterUser", CommandType.StoredProcedure, DParam);
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

        #region Bulk Invoice 

        public void BulkReceiptReportForExternalUser(BulkReceiptReport ObjBulkReceiptReport,int PartyId)
        {

            DateTime dtfrom = DateTime.ParseExact(ObjBulkReceiptReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjBulkReceiptReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReceiptNumber", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjBulkReceiptReport.ReceiptNumber });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Size = 50, Value = PartyId });



            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("BulkReceiptPrintForExternalUser", CommandType.StoredProcedure, DParam);
            IList<string> htmls = new List<string>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    htmls.Add(Result["html"].ToString());

                    // htmls.Remove("");
                    //htmls.Count();
                }

                if (Status == 1 && htmls.Count() > 0)
                {
                    htmls.ToList().ForEach(item =>
                    {
                        if (item == "")
                        {
                            htmls.Remove(item);
                        }
                    });



                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = htmls;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();

            }
        }
        public void GetReceiptListForExternaluser(string FromDate, string ToDate,int UID)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_UID", MySqlDbType = MySqlDbType.Int32, Value = UID });




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
        #endregion
        public void ServiceCodeWiseCrDrDtls(Kol_ServiceCodeWiseInvDtls ObjServiceCodeWiseInvDtls)
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

            LstParam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = ObjServiceCodeWiseInvDtls.SAC });

            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ServiceCodeWiseDebitCreditDtls", CommandType.StoredProcedure, DParam);
            List<Kol_ServiceCodeWiseInvDtls> LstInvoiceReportDetails = new List<Kol_ServiceCodeWiseInvDtls>();

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

                    LstInvoiceReportDetails.Add(new Kol_ServiceCodeWiseInvDtls
                    {
                        SAC = Result["SAC"].ToString(),
                        InvoiceNumber = Result["InvoiceNo"].ToString(),
                        Date = Result["Date"].ToString(),
                        CGST = Convert.ToDecimal(Result["CGSTAmt"]),
                        SGST = Convert.ToDecimal(Result["SGSTAmt"]),
                        IGST = Convert.ToDecimal(Result["IGSTAmt"]),
                        TotalValue = Convert.ToDecimal(Result["TotalValue"])
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
        #region Bulk Credit Note External User

        public void PrintDetailsForBulkCRNoteForExternalUser(string PeriodFrom, string PeriodTo, string CRDR, int UserId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Todate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CRDR", MySqlDbType = MySqlDbType.VarChar, Value = CRDR });
            lstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(UserId) });

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

        #endregion
        #region BOEWise Destuffingentry print
        public void GetAllboenoindestuffing(string boeno, int Page = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_boeno", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value =boeno });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = '0' });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfboedestuffpage", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            // List<StuffingRequestDtl> LstStuffing = new List<StuffingRequestDtl>();
            List<ListOfboe> lstInvcNo = new List<ListOfboe>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstInvcNo.Add(new ListOfboe
                    {
                        DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                         boeno= Result["boeno"].ToString()
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
                    _DBResponse.Data = new { lstInvcNo, State };
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
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
        public void GetBoeDestuffEntryForPrint(string boeno)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_boeno", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = boeno });
           // LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("BOEWiseDestuffingPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            boedestuffing ObjDestuffing = new boedestuffing();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    // ObjDestuffing.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]);
                    ObjDestuffing.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    // ObjDestuffing.DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]);
                    // ObjDestuffing.DODate = (Result["DODate"] == null ? "" : Result["DODate"]).ToString();
                    ObjDestuffing.DestuffingEntryDate = (Result["DestuffingEntryDate"] == null ? "" : Result["DestuffingEntryDate"]).ToString();
                   
                    // ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjDestuffing.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    ObjDestuffing.Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString();
                    ObjDestuffing.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjDestuffing.SealNo = (Result["SealNo"] == null ? "" : Result["SealNo"]).ToString();
                    ObjDestuffing.CustomSealNo = (Result["CustomSealNo"] == null ? "" : Result["CustomSealNo"]).ToString();
                    // ObjDestuffing.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    // ObjDestuffing.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                    ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                    ObjDestuffing.LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString();
                    ObjDestuffing.BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString();
                    ObjDestuffing.BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString();
                    ObjDestuffing.BOLNo = (Result["BOLNo"] == null ? "" : Result["BOLNo"]).ToString();
                    ObjDestuffing.BOLDate = (Result["BOLDate"] == null ? "" : Result["BOLDate"]).ToString();
                    ObjDestuffing.MarksNo = (Result["MarksNo"] == null ? "" : Result["MarksNo"]).ToString();
                    ObjDestuffing.CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString();
                    ObjDestuffing.Cargo = Convert.ToString(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]);
                    ObjDestuffing.Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString();
                    ObjDestuffing.Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString();
                    ObjDestuffing.CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffing.Lstbode.Add(new boedestuffing
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                               Size = Result["Size"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            GodownWiseLctnNames = (Result["GodownWiseLctnNames"] == null ? "" : Result["GodownWiseLctnNames"]).ToString(),
                           
                            GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString(),
                           
                            CUM = Convert.ToDecimal(Result["CUM"] == DBNull.Value ? 0 : Result["CUM"]),
                            SQM = Convert.ToDecimal(Result["SQM"] == DBNull.Value ? 0 : Result["SQM"])
                            //DestuffingWeight=Convert.ToDecimal(Result["DestuffingWeight"])
                        });
                    }
                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDestuffing;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
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